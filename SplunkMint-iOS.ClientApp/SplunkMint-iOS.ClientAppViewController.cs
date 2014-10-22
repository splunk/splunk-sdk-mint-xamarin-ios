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

			Mint.SharedInstance.LoggedRequestsHandled += (sender, args) =>
			{
				var realArgs = (LoggedRequestEventArgs)sender;
				Debug.WriteLine ("Logged Request Handled {0} with JSON:\r\n{1}",
					realArgs.ResponseResult.ResultState == MintResultState.OKResultState
					? "Successfully" : "with Failure and",
					realArgs.ResponseResult.ClientRequest);
			};

			Mint.SharedInstance.NetworkDataIntercepted += (sender, e) => 
			{
				ShowAlert("Network Data Intercepted!");
			};

			Mint.SharedInstance.AddExtraData(new ExtraData("GlobalKey1", "GlobalValue1"));
			LimitedExtraDataList extraDataList = new LimitedExtraDataList ();
			extraDataList.AddWithKey ("ListGlobalKey1", "ListGlobalValue1");
			Mint.SharedInstance.AddExtraDataList (extraDataList);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
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
				Debug.WriteLine("Logged Exception Request: {0}", logResult.ClientRequest);

				// You can also use the LogException method with the delegate parameter to get response of the call
//				Mint.SharedInstance.LogException(new NSException(ex.GetType().FullName, ex.Message, null), new LimitedExtraDataList(), 
//					(MintLogResult logResult) => 
//					{
//						Debug.WriteLine("Logged Exception Request: {0}", logResult.ClientRequest);
//					});
			}
		}

		private const string TransactionId = "SplunkMintXamarinTransaction";

		partial void StartTransactionButton_TouchUpInside (UIButton sender)
		{
			Mint.SharedInstance.TransactionStart(TransactionId, 
				(TransactionStartResult transactionStartResult) => 
				{
					Debug.WriteLine("Transtaction Start {0} with ID {1}", 
						transactionStartResult.TransactionStatus == TransactionStatus.SuccessfullyStartedTransaction
						? "Successful" : "Failed", TransactionId);
				});
		}

		partial void StopTransactionButton_TouchUpInside (UIButton sender)
		{
			Mint.SharedInstance.TransactionStop(TransactionId, 
				(TransactionStopResult transactionStopResult) => 
				{
					Debug.WriteLine("Transaction Stopped {0} eith ID {1}",
						transactionStopResult.TransactionStatus == TransactionStatus.UserSuccessfullyStoppedTransaction
						? "Successfully" : "Failed", TransactionId);
				});
		}

		void ShowAlert(string message)
		{
			UIAlertView alert = new UIAlertView();
			alert.Title = "Alert";
			alert.AddButton("Ok");
			alert.Message = message;
			alert.Show();
		}

		private const string URLRequestBin = "http://requestb.in/1em241j1";

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

		async void LogLevelEventButton_TouchUpInside (object sender, EventArgs args)
		{
			// log type DTO only for enterprise plan
//			Mint.SharedInstance.LogEventWithName("SplunkMint Xamarin Log Level Event", MintLogLevel.NoticeLogLevel, 
//				(MintLogResult logResult) =>
//				{
//					Debug.WriteLine("LogLevelEvent ResultState: {0}", 
//						logResult.ResultState == MintResultState.OKResultState
//						? "OK" : "Failed");
//				});

			MintLogResult logResult = await Mint.SharedInstance.LogEventWithTagAsync("I pressed the loge event with tag button!");
		}

		partial void FlushButton_TouchUpInside (UIButton sender)
		{
			Mint.SharedInstance.Flush(
				(MintResponseResult responseResult) => 
				{
					Debug.WriteLine("Flush is {0} with JSON\r\n{1}",
						responseResult.ResultState == MintResultState.OKResultState
						? "Successful" : "Failed",
						responseResult.ClientRequest);
				});
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

		partial void StartSessionButton_TouchUpInside (UIButton sender)
		{
			Mint.SharedInstance.StartSession(
				(MintResponseResult responseResult) => 
				{
					Debug.WriteLine("Start a new session {0}", 
						responseResult.ResultState == MintResultState.OKResultState
						? "Succeed" : "Failed");
				});
		}

		partial void CloseSessionButton_TouchUpInside (UIButton sender)
		{
			Mint.SharedInstance.CloseSession(
				(MintLogResult logResult) => 
				{
					Debug.WriteLine("Log close active session request {0}",
						logResult.ResultState == MintResultState.OKResultState
						? "Succeed" : "Failed");
				});
		}

//		partial void LogCustomTagEventButton_TouchUpInside (UIButton sender)
//		{
//			Mint.SharedInstance.LogEventWithTag("Log Custom Event Tag", 
//				(MintLogResult logResult) =>
//				{
//					Debug.WriteLine("Log a custom event with tag {0}",
//						logResult.ResultState == MintResultState.OKResultState
//						? "Succeed" : "Failed");
//				});
//		}

		partial void MintLogButton_TouchUpInside (UIButton sender)
		{
			throw new NotImplementedException ();
		}
			
		#endregion
	}
}

