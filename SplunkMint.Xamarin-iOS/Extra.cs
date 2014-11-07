using System;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace SplunkMint
{
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
			NetworkDataFixture networkData = new NetworkDataFixture ();
			networkData.Url = request.RequestUri.ToString ();
			networkData.Protocol = request.RequestUri.Scheme;
			byte[] contentBytes = await request.Content.ReadAsByteArrayAsync();
			networkData.RequestLength = NSNumber.FromInt32 (contentBytes.Length);
			networkData.AppendStartTime ();

			string exceptionCaughtMessage = null;
			HttpResponseMessage responseMessage = null;

			try
			{
				responseMessage = await base.SendAsync(request, cancellationToken);
			}
			catch (Exception ex) {
				exceptionCaughtMessage = ex.Message;
			}

			networkData.AppendEndTime (); 
			networkData.AppendWithStatusCode (NSNumber.FromInt32 ((int)responseMessage.StatusCode));
			byte[] receivedBytes = await responseMessage.Content.ReadAsByteArrayAsync();
			networkData.ResponseLength = NSNumber.FromInt32 (receivedBytes.Length);
			if (exceptionCaughtMessage != null)
				networkData.Exception = exceptionCaughtMessage;
			networkData.SaveToDisk ();

			return responseMessage;
		}
	}

	#endregion
}