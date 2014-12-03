using System;
using System.Drawing;

using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

// Frameworks = "CoreTelephony SystemConfiguration UIKit Foundation CoreGraphics", LinkerFlags = "-ObjC -all_load -lz -lstdc++", ForceLoad = true )]
using System.Runtime.InteropServices;

namespace SplunkMint
{
	public delegate NSString SPLJSONModelKeyMapBlock (string keyName);
	public delegate void ResponseResultBlock (MintResponseResult responseResult);
	public delegate void LogResultBlock (MintLogResult logResult);
	public delegate void FailureBlock (NSError error);
	public delegate void TransactionStartResultBlock (TransactionStartResult transactionStartResult);
	public delegate void TransactionStopResultBlock (TransactionStopResult transactionStopResult);
	public delegate void RemoteSettingsBlock (bool succeed, NSError error, RemoteSettingsData remoteSettingsData);

	[BaseType (typeof (NSObject))]
	public partial interface XamarinHelper {

		[Static, Export ("xamarinArchitecture:")]
		void XamarinArchitecture(string architecture);

		[Static, Export ("xamarinVersion:")]
		void XamarinVersion(string version);
	}

	[BaseType (typeof (NSError))]
	public partial interface SPLJSONModelError {

		[Export ("httpResponse", ArgumentSemantic.Retain)]
		NSHttpUrlResponse HttpResponse { get; set; }

		[Static, Export ("errorInvalidDataWithMessage:")]
		NSObject ErrorInvalidDataWithMessage (string message);

		[Static, Export ("errorInvalidDataWithMissingKeys:")]
		NSObject ErrorInvalidDataWithMissingKeys (NSSet keys);

		[Static, Export ("errorInvalidDataWithTypeMismatch:")]
		NSObject ErrorInvalidDataWithTypeMismatch (string mismatchDescription);

		[Static, Export ("errorBadResponse")]
		NSObject ErrorBadResponse { get; }

		[Static, Export ("errorBadJSON")]
		NSObject ErrorBadJSON { get; }

		[Static, Export ("errorModelIsInvalid")]
		NSObject ErrorModelIsInvalid { get; }

		[Static, Export ("errorInputIsNil")]
		NSObject ErrorInputIsNil { get; }

		[Export ("errorByPrependingKeyPathComponent:")]
		SPLJSONModelError ErrorByPrependingKeyPathComponent (string component);
	}

	[BaseType (typeof (NSObject))]
	public partial interface SPLJSONModelArray {

		[Export ("initWithArray:modelClass:")]
		IntPtr Constructor (NSObject [] array, Class cls);

		[Export ("objectAtIndex:")]
		NSObject ObjectAtIndex (uint index);

		[Export ("objectAtIndexedSubscript:")]
		NSObject ObjectAtIndexedSubscript (uint index);

		[Export ("forwardInvocation:")]
		void ForwardInvocation (NSInvocation anInvocation);

		[Export ("count")]
		uint Count { get; }

		[Export ("firstObject")]
		NSObject FirstObject { get; }

		[Export ("lastObject")]
		NSObject LastObject { get; }

		[Export ("modelWithIndexValue:")]
		NSObject ModelWithIndexValue (NSObject indexValue);
	}

	[BaseType (typeof (NSObject))]
	public partial interface SPLJSONValueTransformer {

//		[DllImport ("__Internal")]
//		extern bool isNull (NSObject value);

		[Export ("primitivesNames", ArgumentSemantic.Retain)]
		NSDictionary PrimitivesNames { get; }

		[Static, Export ("classByResolvingClusterClasses:")]
		Class ClassByResolvingClusterClasses (Class sourceClass);

		[Export ("NSMutableStringFromNSString:")]
		NSMutableString NSMutableStringFromNSString (string value);

		[Export ("NSMutableArrayFromNSArray:")]
		NSMutableArray NSMutableArrayFromNSArray (NSObject [] array);

		[Export ("NSArrayFromJSONModelArray:")]
		NSObject [] NSArrayFromJSONModelArray (SPLJSONModelArray array);

		[Export ("NSMutableArrayFromJSONModelArray:")]
		NSMutableArray NSMutableArrayFromJSONModelArray (SPLJSONModelArray array);

		[Export ("NSMutableDictionaryFromNSDictionary:")]
		NSMutableDictionary NSMutableDictionaryFromNSDictionary (NSDictionary dict);

		[Export ("NSSetFromNSArray:")]
		NSSet NSSetFromNSArray (NSArray array);

		[Export ("NSMutableSetFromNSArray:")]
		NSMutableSet NSMutableSetFromNSArray (NSArray array);

		[Export ("JSONObjectFromNSSet:")]
		NSObject [] JSONObjectFromNSSet (NSSet set);

		[Export ("JSONObjectFromNSMutableSet:")]
		NSObject [] JSONObjectFromNSMutableSet (NSMutableSet set);

		[Export ("BOOLFromNSNumber:")]
		NSNumber BOOLFromNSNumber (NSNumber number);

		[Export ("BOOLFromNSString:")]
		NSNumber BOOLFromNSString (string value);

		[Export ("JSONObjectFromBOOL:")]
		NSNumber JSONObjectFromBOOL (NSNumber number);

		[Export ("NSNumberFromNSString:")]
		NSNumber NSNumberFromNSString (string value);

		[Export ("NSStringFromNSNumber:")]
		string NSStringFromNSNumber (NSNumber number);

		[Export ("NSDecimalNumberFromNSString:")]
		NSDecimalNumber NSDecimalNumberFromNSString (string value);

		[Export ("NSStringFromNSDecimalNumber:")]
		string NSStringFromNSDecimalNumber (NSDecimalNumber number);

		[Export ("NSURLFromNSString:")]
		NSUrl NSURLFromNSString (string value);

		[Export ("JSONObjectFromNSURL:")]
		string JSONObjectFromNSURL (NSUrl url);
	}

	[BaseType (typeof (NSObject))]
	public partial interface SPLJSONKeyMapper {

		[Export ("JSONToModelKeyBlock")]
		SPLJSONModelKeyMapBlock JSONToModelKeyBlock { get; }

		[Export ("modelToJSONKeyBlock")]
		SPLJSONModelKeyMapBlock ModelToJSONKeyBlock { get; }

		[Export ("initWithJSONToModelBlock:modelToJSONBlock:")]
		IntPtr Constructor (SPLJSONModelKeyMapBlock toModel, SPLJSONModelKeyMapBlock toJSON);

		[Export ("initWithDictionary:")]
		IntPtr Constructor (NSDictionary map);

		[Static, Export ("mapperFromUnderscoreCaseToCamelCase")]
		SPLJSONKeyMapper MapperFromUnderscoreCaseToCamelCase { get; }

		[Static, Export ("mapperFromUpperCaseToLowerCase")]
		SPLJSONKeyMapper MapperFromUpperCaseToLowerCase { get; }
	}

	[Model, BaseType (typeof (NSObject))]
	public partial interface SPLAbstractJSONModelProtocol {

		[Export ("initWithDictionary:error:")]
		IntPtr Constructor (NSDictionary dict, out NSError err);

		[Export ("initWithData:error:")]
		IntPtr Constructor (NSData data, out NSError error);

		[Export ("toDictionary")]
		NSDictionary ToDictionary { get; }

		[Export ("toDictionaryWithKeys:")]
		NSDictionary ToDictionaryWithKeys (NSArray propertyNames);
	}

	[BaseType (typeof (NSObject))]
	public partial interface SPLJSONModel : SPLAbstractJSONModelProtocol {

		[Export ("initWithString:error:")]
		IntPtr Constructor (string value, out SPLJSONModelError err);

		[Export ("initWithString:usingEncoding:error:")]
		IntPtr Constructor (string value, NSStringEncoding encoding, out SPLJSONModelError err);

		[Export ("initWithDictionary:error:")]
		IntPtr Constructor (NSDictionary dict, out NSError err);

		[Export ("initWithData:error:")]
		IntPtr Constructor (NSData data, out NSError error);

		[Export ("toDictionary")]
		NSDictionary ToDictionary { get; }

		[Export ("toJSONString")]
		string ToJSONString { get; }

		[Export ("toDictionaryWithKeys:")]
		NSDictionary ToDictionaryWithKeys (NSArray propertyNames);

		[Export ("toJSONStringWithKeys:")]
		string ToJSONStringWithKeys (NSArray propertyNames);

		[Static, Export ("arrayOfModelsFromDictionaries:")]
		NSMutableArray ArrayOfModelsFromDictionaries (NSArray array);

		[Static, Export ("arrayOfModelsFromDictionaries:error:")]
		NSMutableArray ArrayOfModelsFromDictionaries (NSArray array, out NSError err);

		[Static, Export ("arrayOfModelsFromData:error:")]
		NSMutableArray ArrayOfModelsFromData (NSData data, out NSError err);

		[Static, Export ("arrayOfDictionariesFromModels:")]
		NSMutableArray ArrayOfDictionariesFromModels (NSArray array);

		[Export ("indexPropertyName")]
		string IndexPropertyName { get; }

		[Export ("isEqual:")]
		bool IsEqual (NSObject obj);

		[Export ("compare:")]
		NSComparisonResult Compare (NSObject obj);

		[Export ("validate:")]
		bool Validate (out NSError error);

		[Static, Export ("keyMapper")]
		SPLJSONKeyMapper KeyMapper { get; }

		[Static, Export ("globalKeyMapper")]
		SPLJSONKeyMapper GlobalKeyMapper { set; }

		[Static, Export ("propertyIsOptional:")]
		bool PropertyIsOptional (string propertyName);

		[Static, Export ("propertyIsIgnored:")]
		bool PropertyIsIgnored (string propertyName);

		[Export ("mergeFromDictionary:useKeyMapping:")]
		void MergeFromDictionary (NSDictionary dict, bool useKeyMapping);
	}

	/// <summary>
	/// A key-value pair containing extra data to attach to crash reports.
	/// </summary>
	[BaseType (typeof (SPLJSONModel))]
	public partial interface ExtraData {

		/// <summary>
		/// Gets or sets the key of the extra data.
		/// </summary>
		/// <value>The key.</value>
		[Export ("key", ArgumentSemantic.Retain)]
		string Key { get; set; }

		/// <summary>
		/// Gets or sets the value of the extra data.
		/// </summary>
		/// <value>The value.</value>
		[Export ("value", ArgumentSemantic.Retain)]
		string Value { get; set; }

		/// <summary>
		/// Gets the length of the maximum value.
		/// </summary>
		/// <value>The length.</value>
		[Export ("maxValueLength", ArgumentSemantic.Assign)]
		NSNumber MaxValueLength { get; set; }

		/// <summary>
		/// Constructor with the specified key and value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		[Export ("initWithKey:andValue:")]
		IntPtr Constructor (string key, string value);

		/// <summary>
		/// Determines whether this extra data is equal to the data in the specified <b>ExtraData</b> instance.
		/// </summary>
		/// <returns><c>true</c> if this data is equal to the specified <b>ExtraData</b> instance; otherwise, <c>false</c>.</returns>
		/// <param name="extraData">The <b>ExtraData</b> instance to compare against.</param>
		[Export ("isEqualToExtraData:")]
		bool IsEqualToExtraData (ExtraData extraData);
	}

	[BaseType (typeof (SPLJSONModel))]
	public partial interface DataFixture {

		[Export ("apiKey", ArgumentSemantic.Retain)]
		string ApiKey { get; set; }

		[Export ("sdkVersion", ArgumentSemantic.Retain)]
		string SdkVersion { get; set; }

		[Export ("sdkPlatform", ArgumentSemantic.Retain)]
		string SdkPlatform { get; set; }

		[Export ("type", ArgumentSemantic.Retain)]
		string Type { get; set; }

		[Export ("device", ArgumentSemantic.Retain)]
		string Device { get; set; }

		[Export ("osVersion", ArgumentSemantic.Retain)]
		string OsVersion { get; set; }

		[Export ("locale", ArgumentSemantic.Retain)]
		string Locale { get; set; }

		[Export ("uuid", ArgumentSemantic.Retain)]
		string Uuid { get; set; }

		[Export ("userIdentifier", ArgumentSemantic.Retain)]
		string UserIdentifier { get; set; }

		[Export ("timestamp", ArgumentSemantic.Retain)]
		string Timestamp { get; set; }

		[Export ("carrier", ArgumentSemantic.Retain)]
		string Carrier { get; set; }

		[Export ("remoteIp", ArgumentSemantic.Retain)]
		string RemoteIp { get; set; }

		[Export ("connection", ArgumentSemantic.Retain)]
		string Connection { get; set; }

		[Export ("state", ArgumentSemantic.Retain)]
		string State { get; set; }

		[Export ("appVersionCode", ArgumentSemantic.Retain)]
		string AppVersionCode { get; set; }

		[Export ("appVersionName", ArgumentSemantic.Retain)]
		string AppVersionName { get; set; }

		[Export ("packageName", ArgumentSemantic.Retain)]
		string PackageName { get; set; }

		[Export ("appVersion", ArgumentSemantic.Retain)]
		string AppVersion { get; set; }

		[Export ("appVersionShort", ArgumentSemantic.Retain)]
		string AppVersionShort { get; set; }

		[Export ("binaryName", ArgumentSemantic.Retain)]
		string BinaryName { get; set; }

		[Export ("gpsStatus", ArgumentSemantic.Retain)]
		string GpsStatus { get; set; }

		[Export ("extraData", ArgumentSemantic.Retain)]
		NSMutableDictionary ExtraData { get; set; }
	}

	/// <summary>
	/// A global list of extra data as an array of up to 32 <b>ExtraData</b> instances.
	/// </summary>
	[BaseType (typeof (SPLJSONModel))]
	public partial interface LimitedExtraDataList {

		/// <summary>
		/// Gets or sets the maximum count of <b>ExtraData</b> instances in the list.
		/// </summary>
		/// <value>The maximum count.</value>
		[Export ("maxCount")]
		uint MaxCount { get; set; }

		/// <summary>
		/// Gets or sets the count of <b>ExtraData</b> instances in the list.
		/// </summary>
		/// <value>The count of <b>ExtraData</b> instances.</value>
		[Export ("count")]
		uint Count { get; set; }

		/// <summary>
		/// Gets or sets the internal <b>ExtraData</b> instances array.
		/// </summary>
		/// <value>The <b>ExtraData</b> array.</value>
		[Export ("extraDataArray", ArgumentSemantic.Retain)]
		NSMutableArray ExtraDataArray { get; set; }

		[Static, Export ("addExtraDataToDataFixture:")]
		void AddExtraDataToDataFixture (DataFixture dataFixture);

		/// <summary>
		/// Gets the singleton shared <b>LimitedExtraDataList</b> instance.
		/// </summary>
		/// <value>The singleton shared instance.</value>
		[Static, Export ("sharedInstance")]
		LimitedExtraDataList SharedInstance { get; }

		/// <summary>
		/// Adds the specified extra data to the list.
		/// </summary>
		/// <param name="extraData">The <b>ExtraData</b> instance to add.</param>
		[Export ("add:")]
		void Add (ExtraData extraData);

		/// <summary>
		/// Removes the specified extra data from the list.
		/// </summary>
		/// <param name="extraData">The <b>ExtraData</b> instance to remove.</param>
		[Export ("remove:")]
		void Remove (ExtraData extraData);

		/// <summary>
		/// Adds extra data to the array as a key-value pair.
		/// </summary>
		/// <param name="key">The extra data key.</param>
		/// <param name="value">The extra data value.</param>
		[Export ("addWithKey:andValue:")]
		void AddWithKey (string key, string value);

		/// <summary>
		/// Removes an <b>ExtraData</b> instance from the list for the specified key.
		/// </summary>
		/// <param name="key">The extra data key.</param>
		[Export ("removeWithKey:")]
		void RemoveWithKey (string key);

		/// <summary>
		/// Returns the index of the specified <b>ExtraData</b> instance in the internal array.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="extraData">The <b>ExtraData</b> instance.</param>
		[Export ("indexOf:")]
		int IndexOf (ExtraData extraData);

		/// <summary>
		/// Inserts an <b>ExtraData</b> instance at the specified index in the internal array.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="extraData">The <b>ExtraData</b> instance.</param>
		[Export ("insertAtIndex:extraData:")]
		void InsertAtIndex (uint index, ExtraData extraData);

		/// <summary>
		/// Removes the <b>ExtraData</b> instance at the specified index in the internal array.
		/// </summary>
		/// <param name="index">The index.</param>
		[Export ("removeAtIndex:")]
		void RemoveAtIndex (uint index);

		/// <summary>
		/// Clears all the <b>ExtraData</b> instances from the array.
		/// </summary>
		[Export ("clear")]
		void Clear ();

		/// <summary>
		/// Indicates whether the list contains the specified <b>ExtraData</b> instance.
		/// </summary>
		/// <returns><c>true</c> if this instance contains the extra data; otherwise, <c>false</c>.</returns>
		/// <param name="extraData">The <b>ExtraData</b> instance.</param>
		[Export ("contains:")]
		bool Contains (ExtraData extraData);
	}

	/// <summary>
	/// A custom <b>NSException</b>-derived class.
	/// </summary>
	[BaseType (typeof (NSException))]
	public partial interface MintMessageException {

		/// <summary>
		/// Constructor with the specified name, reason, and user information.
		/// </summary>
		/// <param name="aName">A name.</param>
		/// <param name="aReason">A reason.</param>
		/// <param name="aUserInfo">A dictionary containing user information.</param>
		[Export ("initWithName:reason:userInfo:")]
		IntPtr Constructor (string aName, string aReason, NSDictionary aUserInfo);
	}

	/// <summary>
	/// The base type used to return information about user actions.
	/// </summary>
	[BaseType (typeof (NSObject))]
	public partial interface MintResult {

		/// <summary>
		/// Gets or sets the type of request.
		/// </summary>
		/// <value>The request type.</value>
		[Export ("requestType")]
		MintRequestType RequestType { get; set; }

		/// <summary>
		/// Gets or sets the result description, if any.
		/// </summary>
		/// <value>The description.</value>
		[Export ("description", ArgumentSemantic.Retain)]
		string Description { get; set; }

		/// <summary>
		/// Gets or sets the state of the result.
		/// </summary>
		/// <value>The result state.</value>
		[Export ("resultState")]
		MintResultState ResultState { get; set; }

		/// <summary>
		/// Gets or sets the exception error that is related to the result, if any.
		/// </summary>
		/// <value>The exception error.</value>
		[Export ("exceptionError", ArgumentSemantic.Retain)]
		MintMessageException ExceptionError { get; set; }

		/// <summary>
		/// Gets or sets the JSON client request.
		/// </summary>
		/// <value>The JSON request.</value>
		[Export ("clientRequest", ArgumentSemantic.Retain)]
		string ClientRequest { get; set; }

		/// <summary>
		/// Indicates whether the request is handled while debugging.
		/// </summary>
		/// <value><c>true</c> if handled while debugging; otherwise, <c>false</c>.</value>
		[Export ("handledWhileDebugging")]
		bool HandledWhileDebugging { get; set; }
	}

	/// <summary>
	/// The MINT response information that is returned when an action is sent directly to the server.
	/// </summary>
	[BaseType (typeof (MintResult))]
	public partial interface MintResponseResult {

		/// <summary>
		/// Deprecated. This value is not used.
		/// </summary>
		/// <value>The error identifier.</value>
		[Export ("errorId", ArgumentSemantic.Retain)]
		NSNumber ErrorId { get; set; }

		/// <summary>
		/// Gets or sets the server response description.
		/// </summary>
		/// <value>The server response description.</value>
		[Export ("serverResponse", ArgumentSemantic.Retain)]
		string ServerResponse { get; set; }

		/// <summary>
		/// Deprecated. This value is not used.
		/// </summary>
		/// <value>The URL.</value>
		[Export ("url", ArgumentSemantic.Retain)]
		string Url { get; set; }

		/// <summary>
		/// Deprecated. This value is not used.
		/// </summary>
		/// <value>The content text.</value>
		[Export ("contentText", ArgumentSemantic.Retain)]
		string ContentText { get; set; }

		/// <summary>
		/// Deprecated. This value is not used.
		/// </summary>
		/// <value>The ticker text.</value>
		[Export ("tickerText", ArgumentSemantic.Retain)]
		string TickerText { get; set; }

		/// <summary>
		/// Deprecated. This value is not used.
		/// </summary>
		/// <value>The content title.</value>
		[Export ("contentTitle", ArgumentSemantic.Retain)]
		string ContentTitle { get; set; }

		/// <summary>
		/// Deprecated. This value is not used.
		/// </summary>
		/// <value><c>true</c> if this instance is resolved; otherwise, <c>false</c>.</value>
		[Export ("isResolved")]
		bool IsResolved { get; set; }
	}

	/// <summary>
	/// The MINT log information that is returned when an action is used to log a request.
	/// </summary>
	[BaseType (typeof (MintResult))]
	public partial interface MintLogResult {

		/// <summary>
		/// Gets or sets the type of log.
		/// </summary>
		/// <value>The log type.</value>
		[Export ("logType")]
		MintLogType LogType { get; set; }
	}

	/// <summary>
	/// A base instance for transaction actions.
	/// </summary>
	[BaseType (typeof (SPLJSONModel))]
	public partial interface TransactionResult {

		/// <summary>
		/// Gets or sets the transaction status.
		/// </summary>
		/// <value>The transaction status.</value>
		[Export ("transactionStatus")]
		TransactionStatus TransactionStatus { get; set; }

		/// <summary>
		/// Gets or sets the helper description.
		/// </summary>
		/// <value>The description.</value>
		[Export ("description", ArgumentSemantic.Retain)]
		string Description { get; set; }
	}

	/// <summary>
	/// The base transaction class.
	/// </summary>
	[BaseType (typeof (DataFixture))]
	public partial interface SPLTransaction {

		/// <summary>
		/// Gets or sets the name of the transaction.
		/// </summary>
		/// <value>The transaction name.</value>
		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the transaction identifier, for internal purposes.
		/// </summary>
		/// <value>The transaction identifier.</value>
		[Export ("transactionId", ArgumentSemantic.Retain)]
		string TransactionId { get; set; }
	}

	[BaseType (typeof (SPLJSONModel))]
	public partial interface MintAppEnvironment {

		[Export ("isEqualToSplunkAppEnvironment:")]
		bool IsEqualToSplunkAppEnvironment (MintAppEnvironment aSplunkAppEnvironment);

		[Export ("uid", ArgumentSemantic.Retain)]
		string Uid { get; set; }

		[Export ("phoneModel", ArgumentSemantic.Retain)]
		string PhoneModel { get; set; }

		[Export ("manufacturer", ArgumentSemantic.Retain)]
		string Manufacturer { get; set; }

		[Export ("internalVersion", ArgumentSemantic.Retain)]
		string InternalVersion { get; set; }

		[Export ("appVersion", ArgumentSemantic.Retain)]
		string AppVersion { get; set; }

		[Export ("brand", ArgumentSemantic.Retain)]
		string Brand { get; set; }

		[Export ("appName", ArgumentSemantic.Retain)]
		string AppName { get; set; }

		[Export ("osVersion", ArgumentSemantic.Retain)]
		string OsVersion { get; set; }

		[Export ("wifiOn")]
		int WifiOn { get; set; }

		[Export ("gpsOn", ArgumentSemantic.Assign)]
		NSNumber GpsOn { get; set; }

		[Export ("cellularData", ArgumentSemantic.Retain)]
		string CellularData { get; set; }

		[Export ("carrier", ArgumentSemantic.Retain)]
		string Carrier { get; set; }

		[Export ("screenWidth", ArgumentSemantic.Retain)]
		NSNumber ScreenWidth { get; set; }

		[Export ("screenHeight", ArgumentSemantic.Retain)]
		NSNumber ScreenHeight { get; set; }

		[Export ("screenOrientation", ArgumentSemantic.Retain)]
		string ScreenOrientation { get; set; }

		[Export ("screenDpi", ArgumentSemantic.Retain)]
		string ScreenDpi { get; set; }

		[Export ("rooted")]
		bool Rooted { get; set; }

		[Export ("locale", ArgumentSemantic.Retain)]
		string Locale { get; set; }

		[Export ("geoRegion", ArgumentSemantic.Retain)]
		string GeoRegion { get; set; }

		[Export ("cpuModel", ArgumentSemantic.Retain)]
		string CpuModel { get; set; }

		[Export ("cpuBitness", ArgumentSemantic.Retain)]
		string CpuBitness { get; set; }

		[Export ("build_uuid", ArgumentSemantic.Retain)]
		string Build_uuid { get; set; }

		[Export ("image_base_address", ArgumentSemantic.Retain)]
		string Image_base_address { get; set; }

		[Export ("architecture", ArgumentSemantic.Retain)]
		string Architecture { get; set; }

		[Export ("registers", ArgumentSemantic.Retain)]
		NSMutableDictionary Registers { get; set; }

		[Export ("execName", ArgumentSemantic.Retain)]
		string ExecName { get; set; }

		[Export ("imageSize", ArgumentSemantic.Retain)]
		string ImageSize { get; set; }

		[Export ("log_data", ArgumentSemantic.Retain)]
		NSMutableDictionary Log_data { get; set; }
	}

	[BaseType (typeof (SPLJSONModel))]
	public partial interface MintPerformance {

		[Export ("appMemAvail", ArgumentSemantic.Retain)]
		string AppMemAvail { get; set; }

		[Export ("appMemMax", ArgumentSemantic.Retain)]
		string AppMemMax { get; set; }

		[Export ("appMemTotal", ArgumentSemantic.Retain)]
		string AppMemTotal { get; set; }

		[Export ("sysMemAvail", ArgumentSemantic.Retain)]
		string SysMemAvail { get; set; }

		[Export ("sysMemLow", ArgumentSemantic.Retain)]
		string SysMemLow { get; set; }

		[Export ("sysMemThreshold", ArgumentSemantic.Retain)]
		string SysMemThreshold { get; set; }
	}

	/// <summary>
	/// The transaction start class.
	/// </summary>
	[BaseType (typeof (SPLTransaction))]
	public partial interface TrStart {
	
		[Static, Export ("getInstanceWithTransactionName:appEnvironment:andPerformance:")]
		TrStart GetInstanceWithTransactionName (string transactionName, MintAppEnvironment anAppEnvironment, MintPerformance aPerformance);

		/// <summary>
		/// Determines whether the JSON model is the start of a transaction.
		/// </summary>
		/// <returns><c>true</c> if this instance is a transaction start; otherwise, <c>false</c>.</returns>
		/// <param name="json">The JSON model.</param>
		[Static, Export ("isJSONTrStart:")]
		bool IsJSONTrStart (string json);
	}

	/// <summary>
	/// The information that is returned when you start a transaction.
	/// </summary>
	[BaseType (typeof (TransactionResult))]
	public partial interface TransactionStartResult {

		/// <summary>
		/// Gets or sets the name of the transaction.
		/// </summary>
		/// <value>The transaction name.</value>
		[Export ("transactionName", ArgumentSemantic.Retain)]
		string TransactionName { get; set; }

		/// <summary>
		/// Gets or sets the transaction start object instance.
		/// </summary>
		/// <value>The <b>TrStart</b> instance.</value>
		[Export ("transactionStart", ArgumentSemantic.Retain)]
		TrStart TransactionStart { get; set; }
	}

	/// <summary>
	/// The transaction stop class.
	/// </summary>
	[BaseType (typeof (SPLTransaction))]
	public partial interface TrStop {

		/// <summary>
		/// Gets or sets the duration of the transaction since it started.
		/// </summary>
		/// <value>The duration, in milliseconds.</value>
		[Export ("duration", ArgumentSemantic.Retain)]
		NSNumber Duration { get; set; }

		/// <summary>
		/// Gets or sets the status of the transaction.
		/// </summary>
		/// <value>The transaction status.</value>
		[Export ("status", ArgumentSemantic.Retain)]
		string Status { get; set; }

		/// <summary>
		/// Gets or sets the the reason for cancelling the transaction.
		/// </summary>
		/// <value>The cancellation reason.</value>
		[Export ("reason", ArgumentSemantic.Retain)]
		string Reason { get; set; }

		[Static, Export ("getInstanceWithTransactionId:transactionName:appEnvironment:duration:reason:andCompletedStatus:")]
		TrStop GetInstanceWithTransactionId (string transactionId, string transactionName, MintAppEnvironment anAppEnvironment, NSNumber aDuration, string aReason, string aCompletedStatus);
	}

	/// <summary>
	/// The class that is returned when transactions are stopped or cancelled.
	/// </summary>
	[BaseType (typeof (TransactionResult))]
	public partial interface TransactionStopResult {

		/// <summary>
		/// Gets or sets the reason the transaction was stopped or cancelled.
		/// </summary>
		/// <value>The reason.</value>
		[Export ("reason", ArgumentSemantic.Retain)]
		string Reason { get; set; }

		/// <summary>
		/// Gets or sets the tranaction stop object instance.
		/// </summary>
		/// <value>The <b>TrStop</b> instance.</value>
		[Export ("transactionStop", ArgumentSemantic.Retain)]
		TrStop TransactionStop { get; set; }
	}

	[BaseType (typeof (SPLJSONModel))]
	public partial interface RemoteSettingsData {

		[Export ("logLevel", ArgumentSemantic.Retain)]
		NSNumber LogLevel { get; set; }

		[Export ("eventLevel", ArgumentSemantic.Retain)]
		NSNumber EventLevel { get; set; }

		[Export ("version", ArgumentSemantic.Retain)]
		string Version { get; set; }

		[Export ("refreshEveryXseconds", ArgumentSemantic.Retain)]
		NSNumber RefreshEveryXseconds { get; set; }

		[Export ("transactionSla", ArgumentSemantic.Retain)]
		NSNumber TransactionSla { get; set; }

		[Export ("netMonitoring")]
		bool NetMonitoring { get; set; }

		[Export ("hashCode", ArgumentSemantic.Retain)]
		string HashCode { get; set; }

		[Export ("devSettings", ArgumentSemantic.Retain)]
		NSMutableDictionary DevSettings { get; set; }

		[Export ("sessionTime", ArgumentSemantic.Assign)]
		NSNumber SessionTime { get; set; }

		[Static, Export ("sharedInstance")]
		RemoteSettingsData SharedInstance { get; }
	}

	[Model, BaseType (typeof (NSObject))]
	public partial interface DeviceInfoDelegate {

		[Export ("appendBugSenseInfo")]
		void AppendBugSenseInfo ();

		[Export ("getDeviceConnectionInfo")]
		void GetDeviceConnectionInfo ();

		[Export ("getScreenInfo")]
		void GetScreenInfo ();

		[Export ("getAppEnvironment")]
		MintAppEnvironment GetAppEnvironment { get; }

		[Export ("getSplunkPerformance")]
		MintPerformance GetSplunkPerformance { get; }

		[Export ("isLowMemDevice")]
		bool IsLowMemDevice { get; }
	}

	[Model, BaseType (typeof (NSObject))]
	public partial interface FileClientDelegate {

		[Export ("createDefaultDirectoriesAsyncWithCompletionBlock:")]
		void CreateDefaultDirectoriesAsyncWithCompletionBlock (Delegate completed);

		[Export ("createDefaultFilesAsync")]
		void CreateDefaultFilesAsync ();

		[Export ("save:andContents:andFailureBlock:")]
		bool AndContents (string fileName, string contents, Delegate failureBlock);

		[Export ("read:")]
		string Read (string fileName);

		[Export ("saveAsync:contents:andCompletionBlock:")]
		void Contents (string fileName, string contents, Delegate completed);

		[Export ("readAsync:andCompletionBlock:")]
		void AndCompletionBlock (string fileName, Delegate completed);

		[Export ("readLoggedExceptionsWithCompletionBlock:")]
		void ReadLoggedExceptionsWithCompletionBlock (Delegate completed);

		[Export ("deleteFile:andFailureBlock:")]
		bool AndFailureBlock (string fileName, Delegate failureBlock);

		[Export ("deleteFileAsync:completionBlock:")]
		void CompletionBlock (string fileName, Delegate completed);

		[Export ("updateCrashOnLastRunErrorId:")]
		bool UpdateCrashOnLastRunErrorId (NSNumber errorId);

		[Export ("loadRemoteSettings")]
		RemoteSettingsData LoadRemoteSettings { get; }

		[Export ("saveRemoteSettings:")]
		bool SaveRemoteSettings (RemoteSettingsData remoteSettingsData);

		[Export ("readLoggedStartedTransactions")]
		NSObject [] ReadLoggedStartedTransactions { get; }

		[Export ("readLoggedStoppedTransactions")]
		NSObject [] ReadLoggedStoppedTransactions { get; }
	}

	[Model, BaseType (typeof (NSObject))]
	public partial interface ServiceClientDelegate {

		[Export ("executeRequestAsyncWithUrl:requestData:requestType:contentType:andCompletedBlock:")]
		void RequestData (string url, string data, MintRequestType requestType, string aContentType, ResponseResultBlock resultBlock);
	}

	[BaseType (typeof (NSObject))]
	public partial interface SerializeResult {

		[Export ("encodedJson", ArgumentSemantic.Retain)]
		string EncodedJson { get; set; }

		[Export ("decodedJson", ArgumentSemantic.Retain)]
		string DecodedJson { get; set; }
	}

	[BaseType (typeof (SPLJSONModel))]
	public partial interface MintException {

		[Static, Export ("getSplunkException:handled:")]
		MintException GetSplunkException (NSException exception, bool isHandled);

		[Static, Export ("getSplunkExceptionWithHandled:")]
		MintException GetSplunkExceptionWithHandled (bool isHandled);

		[Export ("isEqualToSplunkException:")]
		bool IsEqualToSplunkException (MintException aSplunkException);

		[Export ("message", ArgumentSemantic.Retain)]
		string Message { get; set; }

		[Export ("backtrace", ArgumentSemantic.Retain)]
		NSObject [] Backtrace { get; set; }

		[Export ("occuredAt", ArgumentSemantic.Retain)]
		string OccuredAt { get; set; }

		[Export ("klass", ArgumentSemantic.Retain)]
		string Klass { get; set; }

		[Export ("where", ArgumentSemantic.Retain)]
		string Where { get; set; }

		[Export ("tags", ArgumentSemantic.Retain)]
		string Tags { get; set; }

		[Export ("breadcrumbs", ArgumentSemantic.Retain)]
		string Breadcrumbs { get; set; }

		[Export ("handled")]
		int Handled { get; set; }

		[Export ("errorHash", ArgumentSemantic.Retain)]
		string ErrorHash { get; set; }

		[Export ("threadCrashed", ArgumentSemantic.Retain)]
		NSNumber ThreadCrashed { get; set; }

		[Export ("signalCode", ArgumentSemantic.Retain)]
		string SignalCode { get; set; }

		[Export ("signalName", ArgumentSemantic.Retain)]
		string SignalName { get; set; }

		[Export ("timestamp", ArgumentSemantic.Retain)]
		string Timestamp { get; set; }

		[Export ("msFromStart", ArgumentSemantic.Retain)]
		string MsFromStart { get; set; }
	}

	[BaseType (typeof (SPLJSONModel))]
	public partial interface MintClient {

		[Export ("isEqualToSplunkClient:")]
		bool IsEqualToSplunkClient (MintClient aSplunkClient);

		[Export ("version", ArgumentSemantic.Retain)]
		string Version { get; set; }

		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; set; }

		[Export ("flavor", ArgumentSemantic.Retain)]
		string Flavor { get; set; }
	}

	[BaseType (typeof (SPLJSONModel))]
	public partial interface MintInternalRequest {

		[Export ("comment", ArgumentSemantic.Retain)]
		string Comment { get; set; }

		[Export ("userId", ArgumentSemantic.Retain)]
		string UserId { get; set; }
	}

	[BaseType (typeof (SPLJSONModel))]
	public partial interface MintExceptionRequest {

		[Export ("initWithException:environment:performance:andLimitedCrashExtraData:")]
		IntPtr Constructor (MintException anException, MintAppEnvironment anEnvironment, MintPerformance aPerformance, LimitedExtraDataList aLimitedCrashExtraData);

		[Export ("isEqualToSplunkExceptionRequest:")]
		bool IsEqualToSplunkExceptionRequest (MintExceptionRequest aSplunkExceptionRequest);

		[Export ("log_data", ArgumentSemantic.Retain)]
		NSMutableDictionary Log_data { get; set; }

		[Export ("exception", ArgumentSemantic.Retain)]
		MintException Exception { get; set; }

		[Export ("application_environment", ArgumentSemantic.Retain)]
		MintAppEnvironment Application_environment { get; set; }

		[Export ("client", ArgumentSemantic.Retain)]
		MintClient Client { get; set; }

		[Export ("performance", ArgumentSemantic.Retain)]
		MintPerformance Performance { get; set; }

		[Export ("request", ArgumentSemantic.Retain)]
		MintInternalRequest Request { get; set; }
	}

	/// <summary>
	/// The network data class.
	/// </summary>
	[BaseType (typeof (DataFixture))]
	public partial interface NetworkDataFixture {

		/// <summary>
		/// Gets or sets the URL of the network call.
		/// </summary>
		/// <value>The URL.</value>
		[Export ("url", ArgumentSemantic.Retain)]
		string Url { get; set; }

		/// <summary>
		/// Gets or sets the protocol schema of the network call.
		/// </summary>
		/// <value>The protocol schema.</value>
		[Export ("protocol", ArgumentSemantic.Retain)]
		string Protocol { get; set; }

		/// <summary>
		/// Gets or sets the end time of the network call.
		/// </summary>
		/// <value>The end time.</value>
		[Export ("endTime", ArgumentSemantic.Retain)]
		NSNumber EndTime { get; set; }

		/// <summary>
		/// Gets or sets the duration of the network call.
		/// </summary>
		/// <value>The duration, in milliseconds.</value>
		[Export ("duration", ArgumentSemantic.Retain)]
		NSNumber Duration { get; set; }

		/// <summary>
		/// Gets or sets the response status code.
		/// </summary>
		/// <value>The status code.</value>
		[Export ("statusCode", ArgumentSemantic.Retain)]
		NSNumber StatusCode { get; set; }

		/// <summary>
		/// Gets or sets the length of the content.
		/// </summary>
		/// <value>The length.</value>
		[Export ("contentLength", ArgumentSemantic.Retain)]
		NSNumber ContentLength { get; set; }

		/// <summary>
		/// Gets or sets the length of the request.
		/// </summary>
		/// <value>The length.</value>
		[Export ("requestLength", ArgumentSemantic.Retain)]
		NSNumber RequestLength { get; set; }

		/// <summary>
		/// Indicates whether the network call has failed.
		/// </summary>
		/// <value><c>true</c> if failed; otherwise, <c>false</c>.</value>
		[Export ("failed")]
		bool Failed { get; set; }

		/// <summary>
		/// Gets or sets the request headers.
		/// </summary>
		/// <value>The request headers.</value>
		[Export ("reqHeaders", ArgumentSemantic.Retain)]
		NSMutableDictionary ReqHeaders { get; set; }

		/// <summary>
		/// Gets or sets the response headers.
		/// </summary>
		/// <value>The response headers.</value>
		[Export ("respHeaders", ArgumentSemantic.Retain)]
		NSMutableDictionary RespHeaders { get; set; }

		/// <summary>
		/// Gets or sets the the exception, if any was thrown.
		/// </summary>
		/// <value>The exception.</value>
		[Export ("exception", ArgumentSemantic.Retain)]
		string Exception { get; set; }

		/// <summary>
		/// Gets or sets the length of the response.
		/// </summary>
		/// <value>The length.</value>
		[Export ("responseLength", ArgumentSemantic.Retain)]
		NSNumber ResponseLength { get; set; }

		/// <summary>
		/// Gets or sets the latency of the network call.
		/// </summary>
		/// <value>The latency, in milliseconds.</value>
		[Export ("latency", ArgumentSemantic.Retain)]
		string Latency { get; set; }

		/// <summary>
		/// Appends the status code to the network call instance.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		[Export ("appendWithStatusCode:")]
		void AppendWithStatusCode (NSNumber statusCode);

		/// <summary>
		/// Appends the start time of the network call to the network call instance.
		/// </summary>
		[Export ("appendStartTime")]
		void AppendStartTime ();

		/// <summary>
		/// Appends the end time of the network call to the network call instance.
		/// </summary>
		[Export ("appendEndTime")]
		void AppendEndTime ();

		/// <summary>
		/// Appends request information to the network call instance.
		/// </summary>
		/// <param name="request">The <b>NSUrlRequest</b> instance.</param>
		[Export ("appendRequestInfo:")]
		void AppendRequestInfo (NSUrlRequest request);

		/// <summary>
		/// Appends response information to the network call instance.
		/// </summary>
		/// <param name="response">The <b>NSUrlResponse</b> instance.</param>
		[Export ("appendResponseInfo:")]
		void AppendResponseInfo (NSUrlResponse response);

		/// <summary>
		/// Appends the data returned from the server to the network call instance.
		/// </summary>
		/// <param name="data">The <b>NSData</b> instance.</param>
		[Export ("appendResponseData:")]
		void AppendResponseData (NSData data);

		/// <summary>
		/// Appends the size of the response data to the network call instance.
		/// </summary>
		/// <param name="dataSize">The data size.</param>
		[Export ("appendResponseDataSize:")]
		void AppendResponseDataSize (uint dataSize);

		/// <summary>
		/// Appends any error that has occurred to the network call instance.
		/// </summary>
		/// <param name="error">The <b>NSError</b> error instance.</param>
		[Export ("appendWithError:")]
		void AppendWithError (NSError error);

		/// <summary>
		/// Appends any global extra data to the network call instance.
		/// </summary>
		[Export ("appendGlobalExtraData")]
		void AppendGlobalExtraData ();

		/// <summary>
		/// Prints the main properties to the console for debugging purposes.
		/// </summary>
		[Export ("debugPrint")]
		void DebugPrint ();

		/// <summary>
		/// Saves the <b>NetworkDataFixture</b> information to disk.
		/// </summary>
		[Export ("saveToDisk")]
		void SaveToDisk ();
	}

	[Model, BaseType (typeof (NSObject))]
	public partial interface RequestJsonSerializerDelegate {

		[Export ("serializeEventToJsonForType:appEnvironment:")]
		SerializeResult AppEnvironment (DataType eventType, MintAppEnvironment anAppEnvironment);

		[Export ("serializeEventToJsonForEventTag:appEnvironment:")]
		SerializeResult AppEnvironment (string eventTag, MintAppEnvironment anAppEnvironment);

		[Export ("serializeEventToJsonForName:withLogLevel:andAppEnvironment:")]
		SerializeResult WithLogLevel (string name, MintLogLevel logLevel, MintAppEnvironment anAppEnvironment);

		[Export ("serializeLogToJsonWithName:logLevel:andAppEnvironment:")]
		SerializeResult LogLevel (string name, MintLogLevel logLevel, MintAppEnvironment anAppEnvironment);

		[Export ("serializeCrashToJson:appEnvironment:performance:handled:crashExtraDataList:")]
		SerializeResult AppEnvironment (NSException exception, MintAppEnvironment anAppEnvironment, MintPerformance aPerformance, bool isHandled, LimitedExtraDataList extraData);

		[Export ("decodeEncodedCrashJson:")]
		string DecodeEncodedCrashJson (string encodedJson);

		[Export ("getErrorHash:")]
		string GetErrorHash (string jsonRequest);

		[Export ("serializeCrashToJsonWithExceptionRequest:andAppEnvironment:")]
		SerializeResult AndAppEnvironment (MintExceptionRequest exceptionRequest, MintAppEnvironment appEnvironment);

		[Export ("serializeTransactionStart:")]
		SerializeResult SerializeTransactionStart (TrStart trStart);

		[Export ("serializeTransactionStop:")]
		SerializeResult SerializeTransactionStop (TrStop trStop);

		[Export ("serializeNetworkMonitor:")]
		SerializeResult SerializeNetworkMonitor (NetworkDataFixture networkDataFixture);
	}

	[Model, BaseType (typeof (NSObject))]
	public partial interface ContentTypeDelegate {

		[Export ("eventContentType")]
		string EventContentType { get; }

		[Export ("errorContentType")]
		string ErrorContentType { get; }
	}

	/// <summary>
	/// The event arguments of the logged request.
	/// </summary>
	[BaseType (typeof (NSObject))]
	public partial interface LoggedRequestEventArgs {

		/// <summary>
		/// Gets or sets the result information.
		/// </summary>
		/// <value>The <b>ResponseResult</b> instance.</value>
		[Export ("responseResult", ArgumentSemantic.Retain)]
		MintResponseResult ResponseResult { get; set; }
	}

	[Model, BaseType (typeof (NSObject))]
	public partial interface RequestWorkerDelegate {

		[Export ("loggedRequestHandledWithEventArgs:")]
		void LoggedRequestHandled (LoggedRequestEventArgs args);

		[Export ("pingEventCompletedWithResponse:")]
		void PingEventCompleted (MintResponseResult splunkResponseResult);

		[Export ("networkDataLogged:")]
		void NetworkDataLogged (NetworkDataFixture networkData);
	}

	[Model, BaseType (typeof (NSObject))]
	public partial interface RequestWorkerFacadeDelegate {

		[Export ("workerDelegate", ArgumentSemantic.Retain)]
		RequestWorkerDelegate WorkerDelegate { get; set; }

		[Export ("deviceInfo", ArgumentSemantic.Retain)]
		DeviceInfoDelegate DeviceInfo { get; set; }

		[Export ("jsonSerializer", ArgumentSemantic.Retain)]
		RequestJsonSerializerDelegate JsonSerializer { get; set; }

		[Export ("initWithDeviceInfo:fileClient:serviceClient:contentTypeWorker:andJsonSerializer:")]
		IntPtr Constructor (DeviceInfoDelegate deviceUtil, FileClientDelegate fileRepo, ServiceClientDelegate serviceRepo, ContentTypeDelegate aContentTypeWorker, RequestJsonSerializerDelegate jsonWorker);

		[Export ("sendUnhandledRequestAsync:andResultBlock:")]
		void SendUnhandledRequestAsync (MintExceptionRequest exceptionRequest, [NullAllowed] ResponseResultBlock resultBlock);

		[Export ("getErrorHashFromJson:")]
		string GetErrorHashFromJson (string jsonRequest);

		[Export ("flushAsyncWithBlock:")]
		void FlushAsync ( [NullAllowed]ResponseResultBlock resultBlock);

		[Export ("transactionStartWithName:andResultBlock:")]
		void TransactionStart (string transactionName, [NullAllowed] TransactionStartResultBlock resultBlock);

		[Export ("transactionStopWithName:andResultBlock:")]
		void TransactionStop (string transactionName, [NullAllowed] TransactionStopResultBlock resultBlock);

		[Export ("transactionCancelWithName:reason:andResultBlock:")]
		void TransactionCancel (string transactionName, string reason, [NullAllowed] TransactionStopResultBlock resultBlock);

		[Export ("stopAllTransactions:")]
		void StopAllTransactions (string errorHash);

		[Export ("startWorker")]
		void StartWorker ();

		[Export ("sendEventAsync:completionBlock:")]
		void SendEventAsync (DataType eventType, [NullAllowed] ResponseResultBlock completed);

		[Export ("logEventAsync:completionBlock:")]
		void LogEventAsync (DataType eventType, [NullAllowed] LogResultBlock completed);

		[Export ("logEvent:")]
		MintLogResult LogEvent (DataType eventType);

		[Export ("processPreviousLoggedRequestsAsyncWithBlock:")]
		void ProcessPreviousLoggedRequestsAsync ( [NullAllowed] ResponseResultBlock resultBlock);

		[Export ("getLastCrashIdWithBlock:failure:")]
		void GetLastCrashId (Delegate success, [NullAllowed] FailureBlock failure);

		[Export ("getTotalCrashesNumWithBlock:failure:")]
		void GetTotalCrashesNum (Delegate success, FailureBlock failure);

		[Export ("clearTotalCrashesNumWithBlock:failure:")]
		void ClearTotalCrashesNum (Delegate success, [NullAllowed] FailureBlock failure);

		[Export ("sendEventAsyncWithTag:completionBlock:")]
		void SendEventAsync (string tag, [NullAllowed] ResponseResultBlock completed);

		[Export ("logEventAsyncWithTag:completionBlock:")]
		void LogEventAsync (string tag, [NullAllowed] LogResultBlock completed);

		[Export ("closeSession")]
		MintLogResult CloseSession { get; }

		[Export ("startSessionAsyncWithCompletionBlock:")]
		void StartSessionAsync ( [NullAllowed] ResponseResultBlock completed);

		[Export ("closeSessionAsyncWithCompletionBlock:")]
		void CloseSessionAsync ( [NullAllowed] LogResultBlock completed);

		[Export ("sendExceptionAsync:extraDataKey:extraDataValue:completionBlock:")]
		void SendExceptionAsync (NSException exception, string key, string value, [NullAllowed] ResponseResultBlock completed);

		[Export ("sendExceptionAsync:limitedExtraDataList:completionBlock:")]
		void SendExceptionAsync (NSException exception, LimitedExtraDataList extraDataList, [NullAllowed] ResponseResultBlock completed);

		[Export ("logExceptionAsync:extraDataKey:extraDataValue:completionBlock:")]
		void LogExceptionAsync (NSException exception, string key, string value, [NullAllowed] LogResultBlock completed);

		[Export ("logExceptionAsync:limitedExtraDataList:completionBlock:")]
		void LogExceptionAsync (NSException exception, LimitedExtraDataList extraDataList, [NullAllowed] LogResultBlock completed);

		[Export ("xamarinLogExceptionAsync:andCompletionBlock:")]
		void XamarinLogException (NSException exception, [NullAllowed] LogResultBlock resultBlock);

		[Export ("xamarinLogException:")]
		MintLogResult XamarinLogException (NSException exception);

		[Export ("logEventAsyncWithName:logLevel:andCompletionBlock:")]
		void LogEventAsync (string name, MintLogLevel logLevel, [NullAllowed] LogResultBlock completed);

		[Export ("logSplunkMintLogWithMessage:andLogLevel:")]
		void LogSplunkMintLogWithMessage (string message, MintLogLevel logLevel);
	}

	[BaseType (typeof (NSObject))]
	public partial interface UnhandledCrashReportArgs {

		[Export ("clientJsonRequest", ArgumentSemantic.Retain)]
		string ClientJsonRequest { get; set; }

		[Export ("crashReport", ArgumentSemantic.Retain)]
		string CrashReport { get; set; }

		[Export ("handledSuccessfully")]
		bool HandledSuccessfully { get; set; }
	}

	/// <summary>
	/// The MINT notification delegate.
	/// </summary>
	[Model, BaseType (typeof (NSObject))]
	[Protocol]
	public partial interface MintNotificationDelegate {

		/// <summary>
		/// Provides related information when cached requests are sent to the server.
		/// </summary>
		/// <param name="args">The arguments containing the logged request information.</param>
		[Export ("loggedRequestHandled:")]
		void CachedRequestsSent (LoggedRequestEventArgs args);

		/// <summary>
		/// Provides network data information about the call that was intercepted.
		/// </summary>
		/// <param name="networkData">The network data.</param>
		[Export ("networkDataLogged:")]
		void NetworkDataIntercepted (NetworkDataFixture networkData);
	}

//	, Delegates=new string [] {"WeakDelegate"},
//	Events=new Type [] { typeof (MintNotificationDelegate) }
	/// <summary>
	/// The Splunk MINT base class.
	/// </summary>
	[BaseType (typeof (NSObject), Delegates=new string [] {"WeakDelegate"}, Events=new Type [] { typeof (MintNotificationDelegate) })]
	public partial interface BugSenseBase : RequestWorkerDelegate {

		[Export ("splunkRequestWorker", ArgumentSemantic.Retain)]
		RequestWorkerFacadeDelegate SplunkRequestWorker { get; set; }

		/// <summary>
		/// Indicates whether the Splunk MINT plugin is initialized.
		/// </summary>
		/// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
		[Export ("isInitialized")]
		bool IsInitialized { get; set; }

		/// <summary>
		/// Indicates whether the session is active.
		/// </summary>
		/// <value><c>true</c> if this session is active; otherwise, <c>false</c>.</value>
		[Export ("isSessionActive")]
		bool IsSessionActive { get; set; }

		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>The user identifier.</value>
		[Export ("userIdentifier", ArgumentSemantic.Retain)]
		string UserIdentifier { get; set; }

		/// <summary>
		/// Indicates whether Splunk MINT will handle requests while debugging.
		/// </summary>
		/// <value><c>true</c> if requests are handled; otherwise, <c>false</c>.</value>
		[Export ("handleWhileDebugging")]
		bool HandleWhileDebugging { get; set; }

		/// <summary>
		/// Gets or sets the global extra data list.
		/// </summary>
		/// <value>The <b>LimitedExtraDataList</b> instance.</value>
		[Export ("extraDataList", ArgumentSemantic.Retain)]
		LimitedExtraDataList ExtraDataList { get; set; }

		[Export ("notificationDelegate", ArgumentSemantic.Assign)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		MintNotificationDelegate NotificationDelegate { get; set; }

		[Export ("initWithRequestWorker:")]
		IntPtr Constructor (RequestWorkerFacadeDelegate requestWorker);

		/// <summary>
		/// Disables the crash reporter.
		/// </summary>
		[Export ("disableCrashReporter")]
		void DisableCrashReporter ();

		/// <summary>
		/// Flushes all cached requests that have been logged to the server.
		/// </summary>
		[Export ("flushAsyncWithBlock:")]
		[Async]
		void Flush (ResponseResultBlock resultBlock);

		/// <summary>
		/// Initializes the Splunk MINT plugin and starts a new session.
		/// </summary>
		/// <param name="apiKey">Your Splunk MINT API key.</param>
		[Export ("initAndStartSession:")]
		void InitAndStartSession (string apiKey);

		/// <summary>
		/// Adds extra data to the global <b>LimitedExtraDataList</b> list.
		/// </summary>
		/// <param name="extraData">The <b>ExtraData</b> instance.</param>
		[Export ("addExtraData:")]
		void AddExtraData (ExtraData extraData);

		/// <summary>
		/// Adds a <b>LimitedExtraDataList</b> instance by appending the <b>ExtraData</b> instance to the global <b>LimitedExtraDataList</b>.
		/// </summary>
		/// <param name="limitedExtraDataList">The <b>LimitedExtraDataList</b> instance.</param>
		[Export ("addExtraDataList:")]
		void AddExtraDataList (LimitedExtraDataList limitedExtraDataList);

		/// <summary>
		/// Removes an <b>ExtraData</b> instance from the global extra data list for the specified key.
		/// </summary>
		/// <returns><c>true</c> if extra data was removed; otherwise, <c>false</c>.</returns>
		/// <param name="key">The extra data key.</param>
		[Export ("removeExtraDataWithKey:")]
		bool RemoveExtraDataWithKey (string key);

		/// <summary>
		/// Clears the global <b>LimitedExtraDataList</b> of <b>ExtraData</b> instances.
		/// </summary>
		[Export ("clearExtraData")]
		void ClearExtraData ();

		/// <summary>
		/// Appends a breadcrumb to the global breadcrumbs list.
		/// </summary>
		/// <param name="crumb">The breadcrumb.</param>
		[Export ("leaveBreadcrumb:")]
		void LeaveBreadcrumb (string crumb);

		/// <summary>
		/// Clears all breadcrumbs from the global breadcrumbs list.
		/// </summary>
		[Export ("clearBreadcrumbs")]
		void ClearBreadcrumbs ();

		/// <summary>
		/// Logs an event with a tag.
		/// </summary>
		/// <param name="tag">The tag event.</param>
		/// <param name="completed">The completed callback.</param>
		[Export ("logEventAsyncWithTag:completionBlock:")]
		[Async]
		void LogEventWithTag (string tag, [NullAllowed] LogResultBlock completed);

		/// <summary>
		/// Starts a new session.
		/// </summary>
		/// <param name="completed">The completed callback.</param>
		[Export ("startSessionAsyncWithCompletionBlock:")]
		[Async]
		void StartSession ([NullAllowed] ResponseResultBlock completed);

		/// <summary>
		/// Closes the active session.
		/// </summary>
		/// <param name="completed">The completed callback.</param>
		[Export ("closeSessionAsyncWithCompletionBlock:")]
		[Async]
		void CloseSession ([NullAllowed] LogResultBlock completed);

		/// <summary>
		/// Logs a handled exception with extra data as a key-value pair.
		/// </summary>
		/// <param name="exception">The <b>NSException</b> instance.</param>
		/// <param name="key">The extra data key.</param>
		/// <param name="value">The extra data value.</param>
		/// <param name="completed">The completed callback.</param>
		[Export ("logExceptionAsync:extraDataKey:extraDataValue:completionBlock:")]
		[Async]
		void LogException (NSException exception, string key, string value, [NullAllowed] LogResultBlock completed);

		/// <summary>
		/// Logs a handled exception with a <b>LimitedExtraDataList</b> instance.
		/// </summary>
		/// <param name="exception">The <b>NSException</b> instance.</param>
		/// <param name="extraDataList">The <b>LimitedExtraDataList</b> instance.</param>
		/// <param name="completed">The completed callback.</param>
		[Export ("logExceptionAsync:limitedExtraDataList:completionBlock:")]
		[Async]
		void LogException (NSException exception, [NullAllowed] LimitedExtraDataList extraDataList, [NullAllowed] LogResultBlock completed);
	}

	/// <summary>
	/// The base Splunk MINT BugSenseBase handler.
	/// </summary>
	[BaseType (typeof (BugSenseBase))]
	public partial interface BugSense {

//		[Export ("notificationDelegate", ArgumentSemantic.Assign)]
//		MintNotificationDelegate NotificationDelegate { get; set; }

		/// <summary>
		/// Gets the singleton shared <b>BugSense</b> instance.
		/// </summary>
		/// <value>The shared instance.</value>
		[Static, Export ("sharedInstance")]
		BugSense SharedInstance { get; }

		[Export ("initWithRequestJsonSerializer:contentTypeResolver:requestWorker:andServiceRepository:")]
		IntPtr Constructor (RequestJsonSerializerDelegate jsonSerializer, ContentTypeDelegate contentTypeResolver, RequestWorkerFacadeDelegate requestWorker, ServiceClientDelegate serviceRepository);
	}

	/// <summary>
	/// The base Splunk MINT handler.
	/// </summary>
	[BaseType (typeof (BugSense))]
	public partial interface MintBase {

		/// <summary>
		/// Disables network monitoring.
		/// </summary>
		[Export ("disableNetworkMonitoring")]
		void DisableNetworkMonitoring ();

		/// <summary>
		/// Gets the remote developer settings.
		/// </summary>
		/// <value>The developer settings.</value>
		[Export ("getDevSettings")]
		NSDictionary DevSettings { get; }

		/// <summary>
		/// Enables logging of any debug message using the <b>MintLog</b> function.
		/// </summary>
		/// <param name="value">A Boolean that indicates whether to enable logging.</param>
		[Export ("enableMintLoggingCache:")]
		void EnableLoggingCache (bool value);

		/// <summary>
		/// Enables logging output to the console.
		/// </summary>
		/// <param name="value">A Boolean that indicates whether to enable logging.</param>
		[Export ("enableLogging:")]
		void EnableLogging (bool value);

		/// <summary>
		/// Sets the number of console lines to log.
		/// </summary>
		/// <param name="linesCount">The count of lines.</param>
		[Export ("setLogging:")]
		void SetLoggingLinesCount (int linesCount);

		/// <summary>
		/// Starts a transaction.
		/// </summary>
		/// <param name="transactionId">The transaction identifier.</param>
		/// <param name="resultBlock">The completed callback.</param>
		[Export ("transactionStart:andResultBlock:")]
		[Async]
		void TransactionStart (string transactionId, [NullAllowed] TransactionStartResultBlock resultBlock);

		/// <summary>
		/// Stops a transaction.
		/// </summary>
		/// <param name="transactionId">The transaction identifier.</param>
		/// <param name="resultBlock">The completed callback.</param>
		[Export ("transactionStop:andResultBlock:")]
		[Async]
		void TransactionStop (string transactionId, [NullAllowed] TransactionStopResultBlock resultBlock);

		/// <summary>
		/// Cancels a transaction.
		/// </summary>
		/// <param name="transactionId">The transaction identifier.</param>
		/// <param name="reason">The reason for cancelling the transaction.</param>
		/// <param name="resultBlock">The completed callback.</param>
		[Export ("transactionCancel:reason:andResultBlock:")]
		[Async]
		void TransactionCancel (string transactionId, string reason, [NullAllowed] TransactionStopResultBlock resultBlock);

		/// <summary>
		/// Adds a URL to the network monitoring blacklist.
		/// </summary>
		/// <param name="url">The URL.</param>
		[Export ("addURLToBlackList:")]
		void AddURLToBlacklist (string url);

		/// <summary>
		/// The list of URLs that have been blacklisted from network interception.
		/// </summary>
		/// <returns>The URLs.</returns>
		[Export ("blacklistUrls")]
		string[] BlacklistUrls();

		/// <summary>
		/// Logs an event with a message and log level.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="logLevel">The log level.</param>
		/// <param name="resultBlock">The completed callback.</param>
		[Export ("logEventAsyncWithName:logLevel:andCompletionBlock:")]
		[Async]
		void LogEventWithName (string message, MintLogLevel logLevel, [NullAllowed] LogResultBlock resultBlock);

		/// <summary>
		/// Logs a Xamarin exception as unhandled.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="resultBlock">The completed callback.</param>
		[Export ("xamarinLogException:andCompletionBlock:")]
		[Async]
		void XamarinLogException (NSException exception, [NullAllowed] LogResultBlock resultBlock);

		/// <summary>
		/// Logs a Xamarin exception as unhandled.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="resultBlock">The completed callback.</param>
		[Export ("xamarinLogException:")]
		MintLogResult XamarinLogException (NSException exception);

		[Export ("exceptionFixtureFrom:")]
		string ExceptionFixtureFrom (NSException exception);
	}

	/// <summary>
	/// The Splunk MINT handler class.
	/// </summary>
	[BaseType (typeof (MintBase))]
	public partial interface Mint {

		/// <summary>
		/// Gets the singleton shared <b>Mint</b> instance.
		/// </summary>
		/// <value>The shared instance.</value>
		[Static, Export ("sharedInstance")]
		Mint SharedInstance { get; }
	}

	// The first step to creating a binding is to add your native library ("libNativeLibrary.a")
	// to the project by right-clicking (or Control-clicking) the folder containing this source
	// file and clicking "Add files..." and then simply select the native library (or libraries)
	// that you want to bind.
	//
	// When you do that, you'll notice that MonoDevelop generates a code-behind file for each
	// native library which will contain a [LinkWith] attribute. MonoDevelop auto-detects the
	// architectures that the native library supports and fills in that information for you,
	// however, it cannot auto-detect any Frameworks or other system libraries that the
	// native library may depend on, so you'll need to fill in that information yourself.
	//
	// Once you've done that, you're ready to move on to binding the API...
	//
	//
	// Here is where you'd define your API definition for the native Objective-C library.
	//
	// For example, to bind the following Objective-C class:
	//
	//     @interface Widget : NSObject {
	//     }
	//
	// The C# binding would look like this:
	//
	//     [BaseType (typeof (NSObject))]
	//     interface Widget {
	//     }
	//
	// To bind Objective-C properties, such as:
	//
	//     @property (nonatomic, readwrite, assign) CGPoint center;
	//
	// You would add a property definition in the C# interface like so:
	//
	//     [Export ("center")]
	//     PointF Center { get; set; }
	//
	// To bind an Objective-C method, such as:
	//
	//     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
	//
	// You would add a method definition to the C# interface like so:
	//
	//     [Export ("doSomething:atIndex:")]
	//     void DoSomething (NSObject object, int index);
	//
	// Objective-C "constructors" such as:
	//
	//     -(id)initWithElmo:(ElmoMuppet *)elmo;
	//
	// Can be bound as:
	//
	//     [Export ("initWithElmo:")]
	//     IntPtr Constructor (ElmoMuppet elmo);
	//
	// For more information, see http://docs.xamarin.com/ios/advanced_topics/binding_objective-c_libraries
	//
}