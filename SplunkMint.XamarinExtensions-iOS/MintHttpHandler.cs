using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using SplunkMint;
using MonoTouch.Foundation;
using System.Diagnostics;

namespace SplunkMint.XamarinExtensionsiOS
{
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

