using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using SplunkMint;
using System.Diagnostics;
using System.Threading;

namespace SplunkMintiOS.ClientApp
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations

		public NSAction BackgroundSessionCompletionHandler { get; set; }

		public override UIWindow Window {
			get;
			set;
		}

		public override bool WillFinishLaunching (UIApplication application, NSDictionary launchOptions)
		{
			// NOTE: Don't call the base implementation on a Model class
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events

			if (Debugger.IsAttached) Mint.SharedInstance.DisableCrashReporter ();

//			Mint.SharedInstance.DisableNetworkMonitoring ();

			Mint.SharedInstance.InitAndStartSession ("bc7388ee");

			return true;
		}

		public override void HandleEventsForBackgroundUrl (UIApplication application, string sessionIdentifier, NSAction completionHandler)
		{
			Console.WriteLine ("Surely first {0}", Thread.CurrentThread);
			BackgroundSessionCompletionHandler = completionHandler;
		}

		public override void OnActivated (UIApplication application)
		{
			Console.WriteLine ("OnActivated");
		}

		public override void OnResignActivation (UIApplication application)
		{
			Console.WriteLine ("OnResignActivation");
		}
	}
}

