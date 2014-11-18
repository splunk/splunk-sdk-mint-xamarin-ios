using System;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Runtime.InteropServices;
using System.Linq;

namespace SplunkMint
{
	#region Mint Partial Class

	public partial class Mint
	{
		#region [ Public Operations ]

		#region [ Properties ]

		public Func<Exception, bool> HandleUnobservedException { get; set; }

		#endregion

		#region Events

		public event EventHandler<SplunkUnhandledEventArgs> UnhandledExceptionHandled = delegate { }; 

		#endregion
		 
		[DllImport ("libc")]
		private static extern int sigaction (Signal sig, IntPtr act, IntPtr oact);

		enum Signal
		{
			SIGBUS = 10,
			SIGSEGV = 11
		}

		/// <summary>
		/// Initializes the Splunk>MINT plugin with Xamarin additions.
		/// </summary>
		/// <param name="apiKey">API key.</param>
		public void InitAndStartXamarinSession(string apiKey)
		{
			AddExtraData(new ExtraData("XamarinSDKVersion", "4.0.1"));
			XamarinHelper.XamarinArchitecture ("armv7s");

			TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionsHandler;
			AsyncSynchronizationContext.ExceptionCaught += SyncContextExceptionHandler;
			AsyncSynchronizationContext.Register();

			if (Debugger.IsAttached) {
				DisableCrashReporter ();
			}

			IntPtr sigbus = Marshal.AllocHGlobal (512);
			IntPtr sigsegv = Marshal.AllocHGlobal (512);

			// Store Mono SIGSEGV and SIGBUS handlers
			sigaction (Signal.SIGBUS, IntPtr.Zero, sigbus);
			sigaction (Signal.SIGSEGV, IntPtr.Zero, sigsegv);

			InitAndStartSession (apiKey);

			// Restore Mono SIGSEGV and SIGBUS handlers            
			sigaction (Signal.SIGBUS, sigbus, IntPtr.Zero);
			sigaction (Signal.SIGSEGV, sigsegv, IntPtr.Zero);

			Marshal.FreeHGlobal (sigbus);
			Marshal.FreeHGlobal (sigsegv);
		}

		/// <summary>
		/// It will register the async handler for unawaited void and Task unhandled exceptions thrown in the application.
		/// </summary>
		/// <remarks>
		/// This may be needed in special cases where the synchronization context could be null in early initialization.
		/// The async handlers are registered in the initialization process of the component.
		/// </remarks> 
		public void RegisterAsyncHandlerContext()
		{
			AsyncSynchronizationContext.ExceptionCaught += SyncContextExceptionHandler;
			AsyncSynchronizationContext.Register();
		}

		/// <summary>
		/// Register monitoring of unobserved Task unhandled exceptions.
		/// </summary>
		public void RegisterUnobservedTaskExceptions()
		{
			TaskScheduler.UnobservedTaskException += UnobservedTaskExceptionsHandler;
		}

		/// <summary>
		/// Unregister monitoring of unobserved Task unhandled exceptions.
		/// </summary>
		public void UnregisterUnobservedTaskExceptions()
		{
			TaskScheduler.UnobservedTaskException -= UnobservedTaskExceptionsHandler;
		}

		#endregion

		#region [ Private Methods ]

		private void SyncContextExceptionHandler(object sender, Exception exception)
		{
			Debug.WriteLine ("SyncContextExceptionHandler invoked");
			MintLogResult logResult = LogUnobservedUnawaitedException(exception);
//			OnUnhandledSyncExceptionHandled(exception, logResult);
		}

		private void UnobservedTaskExceptionsHandler(object sender, UnobservedTaskExceptionEventArgs e)
		{
			Debug.WriteLine ("UnobservedTaskExceptionsHandler invoked");
			MintLogResult logResult = LogUnobservedUnawaitedException(e.Exception);
//			OnUnhandledSyncExceptionHandled(e.Exception, logResult);
		}

		private MintLogResult LogUnobservedUnawaitedException(Exception exception)
		{
			if ((HandleUnobservedException != null &&
				HandleUnobservedException (exception)) ||					
				HandleUnobservedException == null) 
				{
					Debug.WriteLine ("LogUnobservedUnawaitedExceptionAsync invoked");
				}

			return new MintLogResult ();

//XamarinLogException (exception.ToSplunkNSException ());
//			return await await XamarinLogExceptionAsync (exception.ToSplunkNSException (), null);
			//			LogException(exception.ToSplunkNSException(), null, 
			//				(MintLogResult logResult) => {
			//					Debug.WriteLine ("LogUnobservedUnawaitedExceptionAsync Completed invoked");
			//					OnUnhandledSyncExceptionHandled(exception, logResult);
			//				});
		}

		private void OnUnhandledSyncExceptionHandled(Exception exception, MintLogResult logResult)
		{
			SplunkUnhandledEventArgs eventArgs = new SplunkUnhandledEventArgs
			{
				ClientJsonRequest = logResult.ClientRequest,
				ExceptionObject = exception,
				HandledSuccessfully = logResult.ResultState == MintResultState.OKResultState
			};

//			UnhandledExceptionHandled (this, eventArgs);
		}

		#endregion
	}

	#endregion

	#region Models

	/// <summary>
	/// Event argument class for the unhandled exceptions occurred and handled by the plugin.
	/// </summary>
	public class SplunkUnhandledEventArgs : EventArgs
	{
		/// <summary>
		/// The JSON fixture for the request to the server.
		/// </summary>
		public string ClientJsonRequest { get; set; }

		/// <summary>
		/// The exception object captured from the system.
		/// </summary>
		public Exception ExceptionObject { get; internal set; }

		/// <summary>
		/// If BugSense handled the exception successfully.
		/// </summary>
		public bool HandledSuccessfully { get; internal set; }        
	}

	#endregion

	#region NSException extension class

	/// <summary>
	/// An extensions static class.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Helper System.Exception extension method.
		/// </summary>
		/// <returns>The Splunk>MINT NSException.</returns>
		/// <param name="exception">The System.Exception.</param>
		public static NSException ToSplunkNSException(this Exception exception)
		{
			NSMutableDictionary dictionary = new NSMutableDictionary ();
			dictionary.SetValueForKey (
				NSObject.FromObject(NSString.FromData(NSData.FromString(exception.StackTrace.Trim()), NSStringEncoding.UTF8)), 
				NSString.FromData(NSData.FromString("SplunkMint-Xamarin-Exception-Stacktrace"), NSStringEncoding.UTF8));

			string[] messages = exception.Message.Split (new [] { '\n' }, 1, StringSplitOptions.RemoveEmptyEntries);
			string message = null;
			if (messages != null &&
			    messages [0] != null) 
			{
				message = messages [0];
			}

			return new NSException (exception.GetType ().FullName, message != null ? message : exception.Message, dictionary);
		}
	}

	#endregion

	#region Splunk Network Xamarin Android Interception HTTP Delegating Handler

	/// <summary>
	/// An interception handler to use with your HttpClient REST client implementation.
	/// </summary>
	public class MintHttpHandler : DelegatingHandler
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="messageHandler">The HttpClientDelegatingHandler.</param>
		public MintHttpHandler(HttpMessageHandler messageHandler)
			: base(messageHandler)
		{
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			string[] urls = Mint.SharedInstance.BlacklistUrls ();
			string url = request.RequestUri.ToString ();
			bool urlIsBlacklisted = urls.FirstOrDefault (p => url.Contains (p)) != null;
			HttpResponseMessage responseMessage = null;

			if (!urlIsBlacklisted) {
				NetworkDataFixture networkData = new NetworkDataFixture ();
				networkData.Url = url;
				networkData.Protocol = request.RequestUri.Scheme;
				byte[] contentBytes = await request.Content.ReadAsByteArrayAsync ();
				networkData.RequestLength = NSNumber.FromInt32 (contentBytes.Length);
				networkData.AppendStartTime ();

				string exceptionCaughtMessage = null;


				try {
					responseMessage = await base.SendAsync (request, cancellationToken);
				} catch (Exception ex) {
					exceptionCaughtMessage = ex.Message;
				}

				networkData.AppendEndTime (); 
				int statusCode = (int)responseMessage.StatusCode;
				NSNumber statusCodeNumber = NSNumber.FromInt32 (statusCode);
				networkData.StatusCode = statusCodeNumber;
				networkData.AppendWithStatusCode (statusCodeNumber);
				byte[] receivedBytes = await responseMessage.Content.ReadAsByteArrayAsync ();
				networkData.ResponseLength = NSNumber.FromInt32 (receivedBytes.Length);
				if (exceptionCaughtMessage != null)
					networkData.Exception = exceptionCaughtMessage;
				networkData.SaveToDisk ();
			} else {
				responseMessage = await base.SendAsync (request, cancellationToken);
			}
			return responseMessage;
		}
	}

	#endregion

	#region [ Internal Class SynchronizationContext For Unawaited Void Methods ]

	internal class AsyncSynchronizationContext : SynchronizationContext
	{
		public static event EventHandler<Exception> ExceptionCaught = delegate { };

		public static AsyncSynchronizationContext Register()
		{
			var syncContext = Current;

			AsyncSynchronizationContext customSynchronizationContext = null;
			if (syncContext != null)
			{
				customSynchronizationContext = syncContext as AsyncSynchronizationContext;

				if (customSynchronizationContext == null)
				{
					customSynchronizationContext = new AsyncSynchronizationContext(syncContext);
					try
					{
						SetSynchronizationContext(customSynchronizationContext);
					}
					catch (Exception ex)
					{
						Console.WriteLine("SetSynchronizationContext Exception: {0}", ex);
					}
				}
			}
			return customSynchronizationContext;
		}

		private readonly SynchronizationContext _syncContext;

		public AsyncSynchronizationContext(SynchronizationContext syncContext)
		{
			_syncContext = syncContext;
		}

		public override SynchronizationContext CreateCopy()
		{
			return new AsyncSynchronizationContext(_syncContext.CreateCopy());
		}

		public override void OperationCompleted()
		{
			_syncContext.OperationCompleted();
		}

		public override void OperationStarted()
		{
			_syncContext.OperationStarted();
		}

		public override void Post(SendOrPostCallback d, object state)
		{
			_syncContext.Post(WrapCallback(d), state);
		}

		public override void Send(SendOrPostCallback d, object state)
		{
			_syncContext.Send(d, state);
		}

		private static SendOrPostCallback WrapCallback(SendOrPostCallback sendOrPostCallback)
		{
			return state =>
			{
				Exception exception = null;

				try
				{
					sendOrPostCallback(state);
				}
				catch (Exception ex)
				{
					exception = ex;
				}

				if (exception != null)
				{
					ExceptionCaught(null, exception);
					// Invoke here the exception
				}
			};
		}
	}

	#endregion
}