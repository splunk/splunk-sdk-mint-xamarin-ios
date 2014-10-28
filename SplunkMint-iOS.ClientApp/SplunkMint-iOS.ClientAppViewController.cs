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
		const string Identifier = "com.SimpleBackgroundTransfer.BackgroundSession";
		const string DownloadUrlString = "https://atmire.com/dspace-labs3/bitstream/handle/123456789/7618/earth-map-huge.jpg";

		public NSUrlSessionDownloadTask downloadTask;
		public NSUrlSession session;

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

			if (session == null)
				session = InitBackgroundSession ();

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

		#endregion

		#region Helper Methods

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

		#endregion

		#region Throw Unhandled Exceptions

		partial void UIButton5_TouchUpInside (UIButton sender)
		{
			throw new NotImplementedException ("NOT IMPLEMENTED? WOW!!!");
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

		#endregion

		#region Log Handled Exception

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

		#endregion

		#region Transactions

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

		#endregion

		#region Network Monitoring

		private const string URLRequestBin = "http://requestb.in/19qubyi1";

		#region Not Supported Network Monitoring APIs - Present to prove that the application continues to works properly

		async void WebRequestRestPostButton_TouchUpInside (object sender, EventArgs args)
		{
			await CallRequestBinWithWebRequest();
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

		#endregion

		#region NSURLSession and NSURLConnection native APIs

		partial void Start (UIButton sender)
		{
			if (downloadTask != null)
				return;

			using (var url = NSUrl.FromString (DownloadUrlString))
			using (var request = NSUrlRequest.FromUrl (url)) {
				downloadTask = session.CreateDownloadTask (request);
				downloadTask.Resume ();
			}
		}

		public NSUrlSession InitBackgroundSession ()
		{
			Console.WriteLine ("InitBackgroundSession");
			using (var configuration = NSUrlSessionConfiguration.BackgroundSessionConfiguration (Identifier)) {
				return NSUrlSession.FromConfiguration (configuration, new UrlSessionDelegate (this), null);
			}
		}

		void NSUrlSessionButton_TouchUpInside(object sender, EventArgs args)
		{
			NSUrlSessionConfiguration sessionConfig = NSUrlSessionConfiguration.DefaultSessionConfiguration;
			sessionConfig.AllowsCellularAccess = true;
			sessionConfig.TimeoutIntervalForRequest = 30.0;
			sessionConfig.TimeoutIntervalForResource = 60.0;
			sessionConfig.HttpMaximumConnectionsPerHost = 1;

			NSUrl url = NSUrl.FromString (URLRequestBin);
			NSMutableUrlRequest urlRequest = new NSMutableUrlRequest (url, NSUrlRequestCachePolicy.ReloadIgnoringLocalCacheData, 60.0);
			urlRequest.HttpMethod = "POST";
			urlRequest.Body = NSData.FromString ("data=This is some NSURLSession data");

			NSUrlSession session = NSUrlSession.FromConfiguration(sessionConfig);
			NSUrlSessionDataTask dataTask = session.CreateDataTask (urlRequest);
			dataTask.Resume ();
		}

		async void HttpClientRestPostButton_TouchUpInside (object sender, EventArgs args)
		{
			try
			{
				using (HttpClientHandler handler = new HttpClientHandler())
				{
					SplunkInterceptionHttpHandler interceptionHandler = new SplunkInterceptionHttpHandler(handler);
					HttpClient httpClient = new HttpClient(interceptionHandler);
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

		async void ModernHttpClientButton_TouchUpInside (object sender, EventArgs e)
		{
			// Use SplunkInterceptionHttpHandler to intercept your networking REST calls
			SplunkInterceptionHttpHandler interceptionHandler = new SplunkInterceptionHttpHandler(new NativeMessageHandler());
			HttpClient httpClient = new HttpClient (interceptionHandler);
			HttpResponseMessage responseMessage = await httpClient.PostAsync(URLRequestBin, new StringContent("Just A Test"));

			ShowAlert (responseMessage.ToString ());
		}

		void NSUrlConnectionButton_TouchUpInside(object sender, EventArgs args)
		{
			NSUrl url = NSUrl.FromString (URLRequestBin);
			NSMutableUrlRequest urlRequest = new NSMutableUrlRequest (url, NSUrlRequestCachePolicy.ReloadIgnoringLocalCacheData, 60.0);
			urlRequest.HttpMethod = "POST";
			urlRequest.Body = NSData.FromString ("data=This is some data");

			NSUrlConnection connection = new NSUrlConnection(urlRequest, new RxTermNSURLConnectionDelegate(), true);
		}

		partial void NSUrlConnectionButtonXamarinExample_TouchUpInside(UIButton sender)
		{
			string rxcui = "198440";
			NSMutableUrlRequest request = new NSMutableUrlRequest(new NSUrl(string.Format("http://rxnav.nlm.nih.gov/REST/RxTerms/rxcui/{0}/allinfo", rxcui)), 
				NSUrlRequestCachePolicy.ReloadRevalidatingCacheData, 20);
			request["Accept"] = "application/json";

			RxTermNSURLConnectionDelegate connectionDelegate = new RxTermNSURLConnectionDelegate();

			NSUrlConnection aConnection = new NSUrlConnection(request, connectionDelegate);
			aConnection.Start();
		}

		#endregion

		#endregion

		#region Log Event

		async void LogLevelEventButton_TouchUpInside (object sender, EventArgs args)
		{
			// log type DTO only for enterprise plan
//			MintLogResult logResult = await Mint.SharedInstance.LogEventWithNameAsync ("SplunkMint Xamarin Log Level Event", MintLogLevel.NoticeLogLevel);

			MintLogResult logResult = await Mint.SharedInstance.LogEventWithTagAsync("I pressed the log event with tag button!");
			Debug.WriteLine("LogLevelEvent ResultState: {0}", 
				logResult.ResultState == MintResultState.OKResultState
				? "OK" : "Failed");
		}

		#endregion

		#region Flush Cached Requests

		async void FlushButton_TouchUpInside (object sender, EventArgs args)
		{
			MintResponseResult responseResult = await Mint.SharedInstance.FlushAsync ();

			Debug.WriteLine("Flush is {0} with JSON\r\n{1}",
				responseResult.ResultState == MintResultState.OKResultState
				? "Successful" : "Failed",
				responseResult.ClientRequest);
		}

		#endregion

		#region Session Handling

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

	#region NSURLConnectionDelegate

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

	#endregion

	#region NSURLSessionDelegate

	public class UrlSessionDelegate : NSUrlSessionDownloadDelegate
	{
		public SplunkMint_iOS_ClientAppViewController controller;

		public UrlSessionDelegate (SplunkMint_iOS_ClientAppViewController controller)
		{
			this.controller = controller;
		}

		public override void DidWriteData (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, long bytesWritten, long totalBytesWritten, long totalBytesExpectedToWrite)
		{
			Console.WriteLine ("Set Progress");
			if (downloadTask == controller.downloadTask) {
				float progress = totalBytesWritten / (float)totalBytesExpectedToWrite;
				Console.WriteLine (string.Format ("DownloadTask: {0}  progress: {1}", downloadTask, progress));
				InvokeOnMainThread( () => {
					// Update any UI components like a progress bar
				});
			}
		}

		public override void DidFinishDownloading (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, NSUrl location)
		{
			Console.WriteLine ("Finished");
			Console.WriteLine ("File downloaded in : {0}", location);
			NSFileManager fileManager = NSFileManager.DefaultManager;

			var URLs = fileManager.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);
			NSUrl documentsDictionry = URLs [0];

			NSUrl originalURL = downloadTask.OriginalRequest.Url;
			NSUrl destinationURL = documentsDictionry.Append ("image1.png", false);
			NSError removeCopy;
			NSError errorCopy;

			fileManager.Remove (destinationURL, out removeCopy);
			bool success = fileManager.Copy (location, destinationURL, out errorCopy);

			if (success) {
				// we do not need to be on the main/UI thread to load the UIImage
				UIImage image = UIImage.FromFile (destinationURL.Path);
				InvokeOnMainThread (() => {
					// Update any UI components like a progress bar
				});
			} else {
				Console.WriteLine ("Error during the copy: {0}", errorCopy.LocalizedDescription);
			}
		}

		public override void DidCompleteWithError (NSUrlSession session, NSUrlSessionTask task, NSError error)
		{
			Console.WriteLine ("DidComplete");
			if (error == null)
				Console.WriteLine ("Task: {0} completed successfully", task);
			else
				Console.WriteLine ("Task: {0} completed with error: {1}", task, error.LocalizedDescription);

			float progress = task.BytesReceived / (float)task.BytesExpectedToReceive;
			InvokeOnMainThread (() => {
				// Update any UI components like a progress bar
			});

			controller.downloadTask = null;
		}

		public override void DidResume (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, long resumeFileOffset, long expectedTotalBytes)
		{
			Console.WriteLine ("DidResume");
		}

		public override void DidFinishEventsForBackgroundSession (NSUrlSession session)
		{
			AppDelegate appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
			var handler = appDelegate.BackgroundSessionCompletionHandler;
			if (handler != null) {
				appDelegate.BackgroundSessionCompletionHandler = null;
				handler.Invoke ();
			}
			Console.WriteLine ("All tasks are finished");
		}
	}

	#endregion
}

