using System;
using System.Collections.Generic;
using System.Linq;
using SplunkMint;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SplunkMintiOS.ClientApp
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");

//			try
//			{
//				Mint.SharedInstance.DisableCrashReporter();
//				UIApplication.Main (args, null, "AppDelegate");
//			}
//			catch (Exception ex) {
//				Mint.SharedInstance.XamarinLogException (ex.ToSplunkNSException ());
//			}
		}
	}
}
