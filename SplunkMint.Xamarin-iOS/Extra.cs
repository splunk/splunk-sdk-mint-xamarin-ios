using System;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using System.Diagnostics;

namespace SplunkMint
{
	public static class Extensions
	{
		public static NSException ToSplunkNSException(this Exception exception)
		{
			NSMutableDictionary dictionary = new NSMutableDictionary ();
			dictionary.SetValueForKey (
				NSObject.FromObject(NSString.FromData(NSData.FromString(exception.StackTrace), NSStringEncoding.UTF8)), 
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
}