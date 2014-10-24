using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SplunkMint;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using ModernHttpClient;

namespace SplunkMintiOS.ClientApp
{
	public partial class SplunkMint_iOS_ClientAppViewController : UIViewController
	{

		public SplunkMint_iOS_ClientAppViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.

			LogHandledExceptionButton.TouchUpInside += LogHandledExceptionButton_TouchUpInside;
			WebRequestButton.TouchUpInside += WebRequestRestPostButton_TouchUpInside;
			HTTPClientButton.TouchUpInside += HttpClientRestPostButton_TouchUpInside;
			LogEventWithTagButton.TouchUpInside += LogLevelEventButton_TouchUpInside;
			ModernHttpClientButton.TouchUpInside += ModernHttpClientButton_TouchUpInside;
			StartSessionButton.TouchUpInside += StartSessionButton_TouchUpInside;
			CloseSessionButton.TouchUpInside += CloseSessionButton_TouchUpInside;
			FlushButton.TouchUpInside += FlushButton_TouchUpInside;
			StartSessionButton.TouchUpInside += StartTransactionButton_TouchUpInside;
			StopTransactionButton.TouchUpInside += StopTransactionButton_TouchUpInside;
			NSUrlSessionButton.TouchUpInside += NSUrlSessionButton_TouchUpInside;
			NSUrlConnectionButton.TouchUpInside += NSUrlConnectionButton_TouchUpInside;

			Mint.SharedInstance.CachedRequestsSent += (sender, args) =>
			{
				LoggedRequestEventArgs loggedRequestEventArgs = (LoggedRequestEventArgs)sender;
				Debug.WriteLine ("Logged Request Handled {0} with JSON:\r\n{1}",
					loggedRequestEventArgs.ResponseResult.ResultState == MintResultState.OKResultState
					? "Successfully" : "with Failure and",
					loggedRequestEventArgs.ResponseResult.ClientRequest);
			};

			Mint.SharedInstance.NetworkDataIntercepted += (sender, args) => 
			{
				NetworkDataFixture networkDataFixture = (NetworkDataFixture)sender;
				Debug.WriteLine("Network Data Logged: {0}", networkDataFixture.ToJSONString);
			};

			Mint.SharedInstance.AddExtraData(new ExtraData("GlobalKey1", "GlobalValue1"));

			LimitedExtraDataList extraDataList = new LimitedExtraDataList ();
			extraDataList.AddWithKey ("ListGlobalKey1", "ListGlobalValue1");

			Mint.SharedInstance.AddExtraDataList (extraDataList);

			Mint.SharedInstance.ClearExtraData ();

			LimitedExtraDataList globalExtraDataList = Mint.SharedInstance.ExtraDataList;

			Mint.SharedInstance.ExtraDataList.AddWithKey ("GlobalKey1", "GlobalValue1");

			Mint.SharedInstance.ExtraDataList.Add(new ExtraData("GlobalExtraKey1", "GlobalExtraValue1"));

			Mint.SharedInstance.RemoveExtraDataWithKey ("GlobalKey1");

			Mint.SharedInstance.ClearBreadcrumbs ();

			Mint.SharedInstance.LeaveBreadcrumb ("SplunkMint-iOS.ClientAppViewController:ViewDidLoad");

			Mint.SharedInstance.UserIdentifier = "gtaskos@splunk.com";

			Mint.SharedInstance.AddURLToBlacklist ("www.splunk.com");
		}

		void ShowAlert(string message)
		{
			UIApplication.SharedApplication.InvokeOnMainThread (() => {
				UIAlertView alert = new UIAlertView ();
				alert.Title = "Alert";
				alert.AddButton ("Ok");
				alert.Message = message;
				alert.Show ();
			});
		}

		partial void UIButton5_TouchUpInside (UIButton sender)
		{
			throw new NotImplementedException ("NOT IMPLEMENTED? WOW!!!");
		}

		async void LogHandledExceptionButton_TouchUpInside (object sender, EventArgs args)
		{
			try
			{
				throw new ArgumentNullException("aParam", "This is a purposed handled exception");
			}
			catch (Exception ex)
			{
				LimitedExtraDataList extraDataList = new LimitedExtraDataList ();
				extraDataList.AddWithKey ("HandledExceptionKey1", "HandledExceptionValue1");
				MintLogResult logResult = await Mint.SharedInstance.LogExceptionAsync (ex.ToSplunkNSException(), extraDataList);
//				MintLogResult logResult = await Mint.SharedInstance.LogExceptionAsync (ex.ToSplunkNSException (), "Key1", "Value1");

				Debug.WriteLine("Logged Exception Request: {0}", logResult.ClientRequest);
			}
		}

		private const string TransactionId = "SplunkMintXamarinTransaction";

		async void StartTransactionButton_TouchUpInside (object sender, EventArgs args)
		{
			TransactionStartResult transactionStartResult = await Mint.SharedInstance.TransactionStartAsync (TransactionId);

			Debug.WriteLine("Transtaction Start {0} with ID {1}", 
				transactionStartResult.TransactionStatus == TransactionStatus.SuccessfullyStartedTransaction
				? "Successful" : "Failed", TransactionId);
		}

		async void StopTransactionButton_TouchUpInside (object sender, EventArgs args)
		{
			TransactionStopResult transactionStopResult = await Mint.SharedInstance.TransactionStopAsync (TransactionId); 

			Debug.WriteLine("Transaction Stopped {0} eith ID {1}",
				transactionStopResult.TransactionStatus == TransactionStatus.UserSuccessfullyStoppedTransaction
				? "Successfully" : "Failed", TransactionId);
		}

		private const string URLRequestBin = "http://requestb.in/19qubyi1";

		async void ModernHttpClientButton_TouchUpInside (object sender, EventArgs e)
		{
			HttpClient httpClient = new HttpClient(new NativeMessageHandler());
			HttpResponseMessage responseMessage = await httpClient.PostAsync(URLRequestBin, new StringContent("Just A Test"));

			ShowAlert (responseMessage.ToString ());
		}

		async void WebRequestRestPostButton_TouchUpInside (object sender, EventArgs args)
		{
			await CallRequestBinWithWebRequest();
		}

		async void HttpClientRestPostButton_TouchUpInside (object sender, EventArgs args)
		{
			await CallRequestBinWithHttpClient();
		}

		private async Task CallRequestBinWithHttpClient()
		{
			try
			{
				using (HttpClientHandler handler = new HttpClientHandler())
				{
					HttpClient httpClient = new HttpClient(handler);
					StringContent dataStringContent = new StringContent("Sample Text Data for HttpClient!");
					dataStringContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
					HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URLRequestBin)
					{
						Content = dataStringContent
					};
					HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);
					string responseString = await response.Content.ReadAsStringAsync();
					if (response.StatusCode == HttpStatusCode.OK &&
						response.IsSuccessStatusCode)
					{
						ShowAlert("HttpClient Succeed!");
					}
					else
					{
						ShowAlert("HttpClient Failed!");
					}
				}
			}
			catch(Exception ex) {
				ShowAlert(string.Format("Exception from HttpClient request: {0}", ex));
			}
		}

		private async Task CallRequestBinWithWebRequest()
		{
			try
			{
				var rxcui = "198440";
				var request = HttpWebRequest.Create(string.Format(@"http://rxnav.nlm.nih.gov/REST/RxTerms/rxcui/{0}/allinfo", rxcui));
				request.ContentType = "application/json";
				request.Method = "GET";

				using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
				{
					if (response.StatusCode != HttpStatusCode.OK)
						Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
					using (StreamReader reader = new StreamReader(response.GetResponseStream()))
					{
						var content = await reader.ReadToEndAsync();
						if(string.IsNullOrWhiteSpace(content)) {
							Console.Out.WriteLine("Response contained empty body...");
						}
						else {
							Console.Out.WriteLine("Response Body: \r\n {0}", content);
						}

						ShowAlert(string.Format("WebResponse: {0}", content));
					}
				}
			}
			catch (Exception ex)
			{
				ShowAlert (string.Format("Exception from WebRequest request: {0}", ex));
			}
		}

		void NSUrlSessionButton_TouchUpInside(object sender, EventArgs args)
		{
			NSUrlSessionConfiguration sessionConfig = NSUrlSessionConfiguration.DefaultSessionConfiguration;
			sessionConfig.AllowsCellularAccess = true;
//			sessionConfig.HttpAdditionalHeaders.SetValueForKey (NSObject.FromObject("NSURLSessionRequest"), "Splunk-Network-Interception");
			sessionConfig.TimeoutIntervalForRequest = 30.0;
			sessionConfig.TimeoutIntervalForResource = 60.0;
			sessionConfig.HttpMaximumConnectionsPerHost = 1;

			NSUrlSession session = NSUrlSession.FromConfiguration(sessionConfig);
			NSUrlSessionDataTask dataTask = session.CreateDataTask (NSUrl.FromString (URLRequestBin));
			dataTask.Resume ();
		}

		void NSUrlConnectionButton_TouchUpInside(object sender, EventArgs args)
		{
			NSUrl url = NSUrl.FromString (URLRequestBin);
			NSMutableUrlRequest urlRequest = new NSMutableUrlRequest (url, NSUrlRequestCachePolicy.ReloadIgnoringLocalCacheData, 60.0);
			urlRequest.HttpMethod = "POST";
			urlRequest.Body = NSData.FromString ("data=This is some data");

			NSUrlConnection connection = new NSUrlConnection(urlRequest, new RxTermNSURLConnectionDelegate(), true);
			connection.Start ();
		}

		async void LogLevelEventButton_TouchUpInside (object sender, EventArgs args)
		{
			// log type DTO only for enterprise plan
//			MintLogResult logResult = await Mint.SharedInstance.LogEventWithNameAsync ("SplunkMint Xamarin Log Level Event", MintLogLevel.NoticeLogLevel);

			MintLogResult logResult = await Mint.SharedInstance.LogEventWithTagAsync("I pressed the log event with tag button!");
			Debug.WriteLine("LogLevelEvent ResultState: {0}", 
				logResult.ResultState == MintResultState.OKResultState
				? "OK" : "Failed");
		}

		async void FlushButton_TouchUpInside (object sender, EventArgs args)
		{
			MintResponseResult responseResult = await Mint.SharedInstance.FlushAsync ();

			Debug.WriteLine("Flush is {0} with JSON\r\n{1}",
				responseResult.ResultState == MintResultState.OKResultState
				? "Successful" : "Failed",
				responseResult.ClientRequest);
		}

		partial void ApplicationExceptionButton_TouchUpInside (UIButton sender)
		{
			throw new ApplicationException("ApplicationException is thrown on purpose with InnerException",
				new NullReferenceException("Just an InnerException of type NullReferenceException"));
		}

		partial void ArgumentExceptionButton_TouchUpInside (UIButton sender)
		{
			throw new ArgumentException("Your param whatever is not complying to the requirements", "whatever");
		}

		async void StartSessionButton_TouchUpInside (object sender, EventArgs args)
		{
			MintResponseResult responseResult = await Mint.SharedInstance.StartSessionAsync ();

			Debug.WriteLine("Start a new session {0}", 
				responseResult.ResultState == MintResultState.OKResultState
				? "Succeed" : "Failed");
		}

		async void CloseSessionButton_TouchUpInside (object sender, EventArgs args)
		{
			MintLogResult logResult = await Mint.SharedInstance.CloseSessionAsync ();

			Debug.WriteLine("Log close active session request {0}",
				logResult.ResultState == MintResultState.OKResultState
				? "Succeed" : "Failed");
		}
			
		#endregion
	}

	public class RxTermNSURLConnectionDelegate : NSUrlConnectionDelegate
	{
		StringBuilder _ResponseBuilder;
		public bool IsFinishedLoading { get; set; }
		public string ResponseContent { get; set; }

		public RxTermNSURLConnectionDelegate()
			: base()
		{
			_ResponseBuilder = new StringBuilder();
		}

		public override void ReceivedData(NSUrlConnection connection, NSData data)
		{
			if(data != null) {
				_ResponseBuilder.Append(data.ToString());
			}
		}
		public override void FinishedLoading(NSUrlConnection connection)
		{
			IsFinishedLoading = true;
			ResponseContent = _ResponseBuilder.ToString();
		}
	}
}

