#Splunk MINT SDK for Xamarin iOS#


Splunk MINT allows you to gain mobile intelligence about your mobile apps by using the Splunk MINT SDKs with your existing mobile app projects. Then, you can use Splunk MINT Management Console to monitor and gain insights into all of your mobile apps. 

*Splunk®, Splunk>®, Splunk MINT are trademarks of Splunk Inc., in the United States and other countries.  Xamarin is a trademark of Xamarin Inc.*

The Splunk MINT Software Development Kit for Xamarin iOS is licensed under the Apache License 2.0. Details can be found in the LICENSE file.

##Introduction##

In addition to sending crash reports, you can send additional data to Splunk MINT to monitor specific actions and items in your mobile apps.

* **Monitor transactions**. Track any process in your app from start to finish and identify slow transactions that negatively affect the user experience.
* **Add and report events**.  Add events to your code and report them to track virtually any user activity on your app.
* **Report handled exceptions**. Log handled exceptions that occur, along with any custom information you want to add. 
* **Add custom data and breadcrumbs to crash reports**. Add custom data to your crash reports as key-value pairs. You can also add breadcrumbs to your code to tag events or actions, which are also included in crash reports.
* **Report user-specific data**. Track the experience of any given user by adding user identifiers to your code, then you can search for errors that affected a particular user and examine the corresponding crash data.
* **Send log output**. Collect and view system debug messages depending on the platform. For example, send LogCat output from Android devices or NSLog messages from iOS devices.
* **Report debug messages**. Display debug messages during testing before you deploy to production.

Mobile apps that use the Splunk MINT SDKs send data to the MINT Data Collector, which then sends the data to Splunk MINT Management Console.

**How to monitor your mobile apps with Splunk MINT**

1. Download a Splunk MINT SDK or plugin for a platform that your app runs on, then import the SDK or plugin into your mobile app project.

2. [Log in to Splunk MINT Management Console](https://mint.splunk.com/dashboard) and create a project for your app. You'll get an API key for the project and a line of code to add for that particular platform&mdash;copy it to your clipboard.

3. Paste this line of code containing your project API key into your app to integrate MINT (for details, see the platform-specific sections in this guide).

    When you start using your app, it will begin to send data to the Splunk MINT Data Collector.

4. Go back to MINT Management Console and open your project. You'll start to see data appear in your dashboards in minutes.

    Repeat this procedure for each of the platforms your app runs on. After you've set up your projects in MINT Management Console, you can use the Splunk MINT App in Splunk&reg; Enterprise to see aggregated data for all of your mobile app projects over all time.

**Documentation**

* For more information about Splunk MINT, see the [Splunk MINT Overview](http://docs.splunk.com/Documentation/Mint/latest/ProductOverview/AboutSplunkMINT). 

**How to contribute**

If you would like to contribute to the SDK, go here for more information:

* [Splunk and open source](http://dev.splunk.com/view/opensource/SP-CAAAEDM)

* [Individual contributions](http://dev.splunk.com/goto/individualcontributions)

* [Company contributions](http://dev.splunk.com/view/companycontributions/SP-CAAAEDR)

## Requirements and installation ##

The requirements for the Splunk MINT SDK for Xamarin iOS are:

* Visual Studio 2012 or later, or Xamarin Studio
* NuGet (the latest version)
* Xamarin iOS
* A project in Splunk MINT Management Console for the iOS platform type.

###Install the SDK plugin in Visual Studio ###

To install the Splunk MINT NuGet package by using the Package Manager Console, do the following:

1. Open the project you want to use with Splunk MINT.
2. On the **Tools** menu, point to **Library Package Manager**, and then click **Package Manager Console**.
3. In the **Package Manager Console** at the **PM>** prompt, type the following:

    Install-Package SplunkMint.Xamarin-iOS

To install the Splunk MINT SDK by using the Package Manager window in Visual Studio:

1. Open the project you want to use with Splunk MINT.
2. On the **Tools** menu, point to **Library Package Manager**, and then click **Manage NuGet Packages for Solution**.
3. In the Manage NuGet Packages window, click **Online** from the list on the left, and then enter "SplunkMint.Xamarin-iOS" into the **Search Online** field in the upper-right corner.

    Several Splunk MINT may packages appear in the list.

4. Click **Install** for the Splunk MINT Xamarin iOS package to install.
5.  In the Select Projects window, select the checkboxes next to the projects in which you want to install the package, and then click **OK**.

###Install the SDK plugin in Xamarin Studio ###

To install the Splunk MINT Nuget package in Xamarin Studio:

1. Select the project that you want to target.
2. From the Xamarin Studio menu, select **Project> Add packages**.
3. Search for "SplunkMint.Xamarin-iOS".
4. Select the package, then click **Add Package** to add Splunk MINT to your project

## Add Splunk MINT to your Xamarin iOS project ##

To use the SDK:

1. In the class that will use Splunk MINT, add the following `using` statement:

    `using SplunkMint;`

2. In your **AppDelegate** class in the **WillFinishLaunching** method, add the following code:

    ```
    public override bool WillFinishLaunching (UIApplication application, NSDictionary launchOptions)
    {
        // Code....
        // We disable crash reporting when the debugger is attached because it conflicts with
        // the Xamarin exception manager. Due to the nature of the platform, you cannot
        // capture and report unhandled native crashes when the debugger is attached.
        if (Debugger.IsAttached) 
        {
            Mint.SharedInstance.DisableCrashReporter ();
        }
        Mint.SharedInstance.InitAndStartXamarinSession ("API*KEY");
        // Code....
        return true;
    }
    ```

The **InitAndStartXamarinSession** method installs the Splunk exception handler and the performance monitor, sends all the saved data and performance metrics to Splunk MINT, and starts a new session for your activity.

**Note**  To have a better experience with the Splunk MINT dashboards, use numeric versioning schemes, preferably in MAJOR.MINOR.RELEASE format.

If you crash the app while debugging, the crash will not be reported. To report crashes, you must deploy the app to your device or simulator and then start it outside the debugging environment.

The native crash reporter reports all native unsymbolicated unhandled crashes that occur in your Xamarin application. However, this report is often inadequate because the managed C# code contains important information. To successfully handle every unhandled crash that is reported to the managed environment, wrap the main application entry point code (the **Main(string[]** *args* **)** method of your **Main.cs** file) in a `try-catch` block, as follows:

```
try
{
    UIApplication.Main (args, null, "AppDelegate");
}
catch (Exception ex) {
    Mint.SharedInstance.XamarinLogException (ex.ToSplunkNSException ());
}
```

## Customize session handling ##

By default, Splunk MINT uses the **InitAndStartXamarinSession** method and the time zone of our servers to calculate the time a user's session begins. However, you can customize session handling if you need to.

**Note**  When you close a session, it does not impact any other feature of the Splunk MINT plugin. The plugin will continue working properly and record network information, handled exceptions, events, and any crash that might occur.

Use the following methods to start, close, and flush sessions:

* To explicitly start the session, use the **StartSessionAsync** method. (If a previous session was initialized less than one minute earlier, this call is ignored.)
* To close the active session, use the **CloseSessionAsync** method.
* To manually flush all saved data, use the **Flush** method.

**Example code**

If there are logged handled exceptions, custom events, or any network interception captured by Splunk MINT and you don't want to wait for another **InitAndStartXamarinSession** use the **Flush** method to immediately send any logged requests to the Splunk server.

```
MintResponseResult responseResult = await Mint.SharedInstance.FlushAsync ();
Debug.WriteLine("Flush is {0} with JSON\r\n{1}",
    responseResult.ResultState == MintResultState.OKResultState
    ? "Successful" : "Failed",
    responseResult.ClientRequest);
```

## Monitor transactions ##

Transactions let you keep track of any process inside your application with a beginning and an end. For example, a transaction could be a process such as registration, login, or a purchase.

A transaction is basically an event that starts and then finishes in one of three ways:

* The transaction is completed normally, resulting in a status of "SUCCESS".
* The transaction is cancelled by the user, possibly because the process took too much time to complete, resulting in a status of "CANCEL".
* The transaction failed because the app crashed, resulting in a status of "FAIL".

Use the following methods to work with transactions:

* To indicate the start of a transaction, use the **TransactionStartAsync** method.
* To indicate the end of a transaction, use the **TransactionStopAsync** method.
* To indicate the cancellation of a transaction with a reason, use the **TransactionCancel** method.

To identify slow transactions that negatively affect the user experience, monitor how long transactions take to complete by going to the Transactions dashboard in Splunk MINT Management Console.

**Example code**

```
private const string TransactionId = "SplunkMintXamarinTransaction";

TransactionStartResult transactionStartResult = await Mint.SharedInstance.TransactionStartAsync (TransactionId);

Debug.WriteLine("Transtaction Start {0} with ID {1}", 
    transactionStartResult.TransactionStatus == TransactionStatus.SuccessfullyStartedTransaction
    ? "Successful" : "Failed", TransactionId);

// Continue your code...
// Do something in a block method. End the transaction so you can track that
// in your dashboard to resolve an issue or for monitoring.

TransactionStopResult transactionStopResult = await Mint.SharedInstance.TransactionStopAsync (TransactionId); 

Debug.WriteLine("Transaction Stopped {0} eith ID {1}",
    transactionStopResult.TransactionStatus == TransactionStatus.UserSuccessfullyStoppedTransaction
    ? "Successfully" : "Failed", TransactionId);
```

## Add and report events ##

In addition to reporting the sequence of events leading up to an app crash, Splunk MINT can report events that are not associated with a crash. For example, if your application asks users to make a selection, you can report the user's selection. You can also include the log level with the event.

* To report an event, use the **LogEventWithTagAsync** method.
* To report an event with log level, use the **LogEventWithNameAsync** method.

Add as many events as you like to track virtually any user activity on your app. To view the event data, see the **Insights** page on your Splunk MINT Management Console dashboard.

**Example code**

```
MintLogResult logResult = await Mint.SharedInstance.LogEventWithTagAsync("I pressed the log event with tag button!");

Debug.WriteLine("LogLevelEvent ResultState: {0}", 
    logResult.ResultState == MintResultState.OKResultState
    ? "OK" : "Failed");

// Enterprise only log event with name and log level
MintLogResult logResult = await Mint.SharedInstance.LogEventWithNameAsync ("SplunkMint Xamarin Log Level Event", MintLogLevel.NoticeLogLevel);

Debug.WriteLine("LogLevelEvent ResultState: {0}", 
    logResult.ResultState == MintResultState.OKResultState
    ? "OK" : "Failed");
```

## Report handled exceptions ##

At times, you might expect your app to throws exceptions. When you handle those exceptions with a `try-catch` block, you can use the Splunk MINT exception handling feature to keep track of any exceptions your app throws and catches. Splunk MINT can also collect customized data associated with an exception. The Splunk MINT exception handling feature applies only to exceptions your application throws rather than to any app crashes that might occur. Use this to your advantage to create self-explanatory exceptions.

* To log an exception with an extra key-value pair, use the **LogExceptionAsync(***NSException*, *string*, *string***)** method.
* To log an exception with an extra data list, use the **LogExceptionAsync(***NSException*, *LimitedExtraDataList***)** method.
* To log an exception with no extra data, use the **LogExceptionAsync(***NSException*, *LimitedExtraDataList***)** method but pass `null` in the **LimitedExtraDataList** parameter.

Note that the **LogExceptionAsync** method accepts an **NSException** type, rather than **Exception**, which you normally catch in a Xamarin environment. For this reason an **Exception** extension method is available (**Exception.ToSplunkNSException**) so you can log the C# handled exception stacktrace.

Using this method helps debug any problem. To get information about the request in general, add a block implementation and examine the **MintLogResult** object.

**Example code**

```
// Log an exception with extra key/value data
MintLogResult logResult = await Mint.SharedInstance.LogExceptionAsync (ex.ToSplunkNSException (), "Key1", "Value1");
Debug.WriteLine("Logged Exception Request: {0}", logResult.ClientRequest);

// Log an exception with extra data list
LimitedExtraDataList extraDataList = new LimitedExtraDataList ();
extraDataList.AddWithKey ("HandledExceptionKey1", "HandledExceptionValue1");
extraDataList.AddWithKey ("HandledExceptionKey2", "HandledExceptionValue2");
MintLogResult logResult = await Mint.SharedInstance.LogExceptionAsync (ex.ToSplunkNSException(), extraDataList);
Debug.WriteLine("Logged Exception Request: {0}", logResult.ClientRequest);
```

## Add custom data to crash reports ##

Although Splunk MINT collects plenty of data associated with each crash of your app, you can collect additional custom crash data. To add custom data to your crash reports, use the extra data map. The data values have a length limit of 128 characters.

* To add custom data as a key-value pair, use the **AddExtraData** and **AddExtraDataList** methods as follows:

    ```
    // Add a key-value pair.
    Mint.SharedInstance.AddExtraData(new ExtraData("GlobalKey1", "GlobalValue1"));

    // Add a LimitedExtraDataList.
    LimitedExtraDataList extraDataList = new LimitedExtraDataList ();
    extraDataList.AddWithKey ("ListGlobalKey1", "ListGlobalValue1");
    extraDataList.AddWithKey ("ListGlobalKey2", "ListGlobalValue2");
    Mint.SharedInstance.AddExtraDataList (extraDataList);
    ```
    
    The **AddExtraData** method accepts an **ExtraData** class instance, which you can instantiate and pass your custom key-value.

    The **AddExtraDataList** method adds all instances of the **LimitedExtraDataList** to the **LimitedExtraDataList** global singleton instance object and sends appropriate values to the Splunk MINT server as key-value pairs, which you can then examine your request.

* To access the global **LimitedExtraDataList** use the **Mint.SharedInstance.ExtraDataList** property:

    ```
    // With key-value
    Mint.SharedInstance.ExtraDataList.AddWithKey ("GlobalKey1", "GlobalValue1");

    // With adding an ExtraData instance
    Mint.SharedInstance.ExtraDataList.Add(new ExtraData("GlobalExtraKey1", "GlobalExtraValue1"));
    ```

    This usage gives the same result as the **AddExtraData** method of the **Mint.SharedInstance** because both methods access the same global **LimitedExtraDataList** singleton instance. The **LimitedExtraDataList** is an object that is limited to a maximum count of 32 objects. If you try to add an object that exceeds this count, the first object in the list is removed, FIFO. You can't add multiple values with the same key. If you try, you will not get an error but the value of an existing key will change.

* To remove a specific value from the extra data, use the **RemoveExtraDataWithKey** method.
* To clear the extra data completely, use the **ClearExtraData** method.

To view the custom crash data in Splunk MINT Management Console:

1. Go to the **Errors** dashboard and select an error. 
2. In the error details section, click the **Error Instances** tab. 
3. In the **Show All** column, click the arrow. 

## Add breadcrumbs to crash reports ##

To help investigate the cause of a crash, you can have Splunk MINT to report the flow of events a user experienced leading up to the crash. When you know this sequence of things the user did with your app before it crashed, you are better equipped to reproduce the crash and diagnose the problem. To tag the events or actions in your app, add breadcrumbs to your code. Splunk MINT retains data associated with a maximum of 16 breadcrumbs prior to a crash.

* Use the **LeaveBreadcrumb** method at the points of interest in your code as follows:

    `Mint.SharedInstance.LeaveBreadcrumb ("ViewController:ViewDidLoad");`

* Use the **ClearBreadcrumbs** method to clear the breadcrumb list as follows:

    `Mint.SharedInstance.ClearBreadcrumbs ();`

## Report user-specific data ##

With Splunk MINT, you can closely track the experience of any given user, for example to investigate a complaint. First, provide a user identifier such as an ID number from a database, an email address, a push ID, or a username. (However, please do not transmit any personally-identifiable or sensitive data into Splunk MINT.) Then in the Splunk MINT Management Console dashboard, you can search for errors that affect a particular user ID and examine crash data associated with her or her usage of your app. This feature is useful for apps with a high average revenue per user (ARPU), for apps that are deployed in a mobile device management (MDM) environment, and during quality-assurance testing.

* Set the **UserIdentifier** property to a user identifier as follows:

    `Mint.SharedInstance.UserIdentifier = "Splunk MINT";`

To search errors for a specific user name or ID, go to the Errors dashboard in Splunk MINT Management Console, then enter the user name or ID under **Search by username** in the list of filters.

## Disable network monitoring for URLs ##

Normally, Splunk MINT monitors all native **NSURLSession** and **NSURLConnection** APIs network calls. 

* To disable network monitoring, use the **DisableNetworkMonitoring** method before the **InitAndStartXamarinSession** as follows:

    `Mint.SharedInstance.DisableNetworkMonitoring ();`

You can add URLs to a monitoring blacklist to ignore any requests to these URLs. For example, you can add "www.facebook.com" to your blacklist to ignore monitoring requests to this site.

* To add a URL to the network monitoring blacklist, use the **AddURLToBlackList** method as follows:

    `Mint.SharedInstance.AddURLToBlacklist ("www.splunk.com");`

## Get notifications about internal SDK actions ##

The Splunk MINT SDK for Xamarin iOS exposes two events that you can register and get notified about any cached requests sent to the server, or when a network call is intercepted and the data is captured.

* Use **CachedRequestsSent** to get notified about any requests that are sent to the server, which is usually invoked when calling the **InitAndStartXamarinSession** and **Flush** methods:

    ```
    Mint.SharedInstance.CachedRequestsSent += (sender, args) =>
   {
        LoggedRequestEventArgs loggedRequestEventArgs = (LoggedRequestEventArgs)sender;
        Debug.WriteLine ("Logged Request Handled {0} with JSON:\r\n{1}",
            loggedRequestEventArgs.ResponseResult.ResultState == MintResultState.OKResultState
            ? "Successfully" : "with Failure and",
            loggedRequestEventArgs.ResponseResult.ClientRequest);
    };
    ```
    
* Use **NetworkDataIntercepted** to get notified when a network call is monitored, then examine the captured data as follows:

    ```
    Mint.SharedInstance.NetworkDataIntercepted += (sender, args) => 
    {
        NetworkDataFixture networkDataFixture = (NetworkDataFixture)sender;
        Debug.WriteLine("Network Data Logged: {0}", networkDataFixture.ToJSONString);
    };
    ```

## Report debugging messages ##

When the debugger (LLDB and GDB) is on, the crash controller does not log any reports.

In standard mode, if your application is constantly crashing at or immediately after it starts, Splunk MINT might not get the chance to report the crash. Splunk MINT depends on the application to launch properly to perform reporting. In this mode, a demo app would need to run twice for the crash to be reported (not including any runs with the Debugger on).
