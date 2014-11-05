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

	#region Splunk Network Interception Http Handler

	/// <summary>
	/// An interception handler to use with your HttpClient REST client implementation.
	/// </summary>
	public class SplunkInterceptionHttpHandler : DelegatingHandler
	{
		//		readonly IDeviceUtil _deviceUtil = new DeviceUtil();
		//		readonly IBugSenseFileClient _fileClient = new FileRepository();
		//
		//		public event EventHandler<NetworkDataFixture> NetworkDataLogged = delegate { };
		//
		//		protected virtual void OnNetworkDataLogged(NetworkDataFixture e)
		//		{
		//			EventHandler<NetworkDataFixture> handler = NetworkDataLogged;
		//			if (handler != null) handler(this, e);
		//		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="messageHandler">The HttpClientHandler.</param>
		public SplunkInterceptionHttpHandler(HttpMessageHandler messageHandler)
			: base(messageHandler)
		{
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			//			NetworkDataFixture networkDataFixture =
			//				NetworkDataFixture.GetNetworkDataFixture(_deviceUtil.GetAppEnvironment());
			//			networkDataFixture.Url = request.RequestUri.ToString();
			byte[] contentBytes = await request.Content.ReadAsByteArrayAsync();
			//			networkDataFixture.ContentLength = contentBytes.Length;
			//
			//			foreach (KeyValuePair<string, IEnumerable<string>> requestHeader in request.Headers)
			//			{
			//				networkDataFixture.Headers.Add(requestHeader.Key, requestHeader.Value.FirstOrDefault());
			//			}

			Console.WriteLine(string.Format("Intercepting call!"));
			Console.WriteLine(string.Format("URL: {0}", request.RequestUri));
			Console.WriteLine(string.Format("HTTP Method: {0}", request.Method));
			byte[] sendBytes = await request.Content.ReadAsByteArrayAsync();
			Console.WriteLine(string.Format("Bytes to send: {0}", sendBytes.Length));
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			HttpResponseMessage responseMessage = await base.SendAsync(request, cancellationToken);

			stopwatch.Stop();

			//			networkDataFixture.StatusCode = (int)responseMessage.StatusCode;
			//			networkDataFixture.Failed = !responseMessage.IsSuccessStatusCode;
			//			networkDataFixture.Duration = stopwatch.ElapsedMilliseconds;

			Console.WriteLine(string.Format("Response Code: {0}", responseMessage.StatusCode));
			byte[] receivedBytes = await responseMessage.Content.ReadAsByteArrayAsync();
			Console.WriteLine(string.Format("Latency time in millis: {0}", stopwatch.ElapsedMilliseconds));
			Console.WriteLine(string.Format("Bytes received: {0}", receivedBytes.Length));

			//			BugSenseLogResult logResult = await SaveNetworkActionAsync(networkDataFixture.SerializeToJson());

			//			if (logResult.ResultState == BugSenseResultState.OK &&
			//				RemoteSettingsData.Instance.NetMonitoring)
			//			{
			//				OnNetworkDataLogged(networkDataFixture);
			//			}

			return responseMessage;
		}

		private Task SaveNetworkActionAsync(string networkData)
		{
			return Task.Run(async () =>
				{
					// Save the data
				});
		}
	}

	#endregion
}