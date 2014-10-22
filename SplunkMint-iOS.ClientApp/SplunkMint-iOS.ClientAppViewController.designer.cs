// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace SplunkMintiOS.ClientApp
{
	[Register ("SplunkMint_iOS_ClientAppViewController")]
	partial class SplunkMint_iOS_ClientAppViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton HTTPClientButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton LogEventWithTagButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton LogHandledExceptionButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ModernHttpClientButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton WebRequestButton { get; set; }

		[Action ("ApplicationExceptionButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ApplicationExceptionButton_TouchUpInside (UIButton sender);

		[Action ("ArgumentExceptionButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ArgumentExceptionButton_TouchUpInside (UIButton sender);

		[Action ("CloseSessionButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void CloseSessionButton_TouchUpInside (UIButton sender);

		[Action ("FlushButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void FlushButton_TouchUpInside (UIButton sender);

		[Action ("MintLogButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void MintLogButton_TouchUpInside (UIButton sender);

		[Action ("StartSessionButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void StartSessionButton_TouchUpInside (UIButton sender);

		[Action ("StartTransactionButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void StartTransactionButton_TouchUpInside (UIButton sender);

		[Action ("StopTransactionButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void StopTransactionButton_TouchUpInside (UIButton sender);

		[Action ("UIButton5_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton5_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (HTTPClientButton != null) {
				HTTPClientButton.Dispose ();
				HTTPClientButton = null;
			}
			if (LogEventWithTagButton != null) {
				LogEventWithTagButton.Dispose ();
				LogEventWithTagButton = null;
			}
			if (LogHandledExceptionButton != null) {
				LogHandledExceptionButton.Dispose ();
				LogHandledExceptionButton = null;
			}
			if (ModernHttpClientButton != null) {
				ModernHttpClientButton.Dispose ();
				ModernHttpClientButton = null;
			}
			if (WebRequestButton != null) {
				WebRequestButton.Dispose ();
				WebRequestButton = null;
			}
		}
	}
}
