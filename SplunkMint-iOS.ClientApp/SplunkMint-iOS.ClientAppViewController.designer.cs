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
		UIButton CloseSessionButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton FlushButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton HTTPClientButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton LogEventWithTagButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton LogHandledException1Button { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton LogHandledException2Button { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ModernHttpClientButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton NSUrlConnectionButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton NSUrlSessionButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton StartSessionButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton StartTransactionButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton StopTransactionButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton UnobservedTaskButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton WebRequestButton { get; set; }

		[Action ("ApplicationExceptionButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ApplicationExceptionButton_TouchUpInside (UIButton sender);

		[Action ("ArgumentExceptionButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ArgumentExceptionButton_TouchUpInside (UIButton sender);

		[Action ("NSUrlConnectionButtonXamarinExample_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void NSUrlConnectionButtonXamarinExample_TouchUpInside (UIButton sender);

		[Action ("Start:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Start (UIButton sender);

		[Action ("UIButton5_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton5_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (CloseSessionButton != null) {
				CloseSessionButton.Dispose ();
				CloseSessionButton = null;
			}
			if (FlushButton != null) {
				FlushButton.Dispose ();
				FlushButton = null;
			}
			if (HTTPClientButton != null) {
				HTTPClientButton.Dispose ();
				HTTPClientButton = null;
			}
			if (LogEventWithTagButton != null) {
				LogEventWithTagButton.Dispose ();
				LogEventWithTagButton = null;
			}
			if (LogHandledException1Button != null) {
				LogHandledException1Button.Dispose ();
				LogHandledException1Button = null;
			}
			if (LogHandledException2Button != null) {
				LogHandledException2Button.Dispose ();
				LogHandledException2Button = null;
			}
			if (ModernHttpClientButton != null) {
				ModernHttpClientButton.Dispose ();
				ModernHttpClientButton = null;
			}
			if (NSUrlConnectionButton != null) {
				NSUrlConnectionButton.Dispose ();
				NSUrlConnectionButton = null;
			}
			if (NSUrlSessionButton != null) {
				NSUrlSessionButton.Dispose ();
				NSUrlSessionButton = null;
			}
			if (StartSessionButton != null) {
				StartSessionButton.Dispose ();
				StartSessionButton = null;
			}
			if (StartTransactionButton != null) {
				StartTransactionButton.Dispose ();
				StartTransactionButton = null;
			}
			if (StopTransactionButton != null) {
				StopTransactionButton.Dispose ();
				StopTransactionButton = null;
			}
			if (UnobservedTaskButton != null) {
				UnobservedTaskButton.Dispose ();
				UnobservedTaskButton = null;
			}
			if (WebRequestButton != null) {
				WebRequestButton.Dispose ();
				WebRequestButton = null;
			}
		}
	}
}
