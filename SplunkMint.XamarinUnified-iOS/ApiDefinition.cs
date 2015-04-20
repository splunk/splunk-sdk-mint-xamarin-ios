 /*
    Copyright 2015 Splunk, Inc.
    *
    Licensed under the Apache License, Version 2.0 (the "License"): you may
    not use this file except in compliance with the License. You may obtain
    a copy of the License at
    *
    *     http://www.apache.org/licenses/LICENSE-2.0
    *
    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
    WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
    License for the specific language governing permissions and limitations
    under the License.
*/

using System;
using System.Drawing;

using ObjCRuntime;
using Foundation;
using UIKit;

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
	/// Used to create a custom extra data instance with key and value properties to attach in every request.
	/// </summary>
	[BaseType (typeof (SPLJSONModel))]
	public partial interface ExtraData {

		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		/// <value>The key.</value>
		[Export ("key", ArgumentSemantic.Retain)]
		string Key { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		[Export ("value", ArgumentSemantic.Retain)]
		string Value { get; set; }

		/// <summary>
		/// Gets the maximum length of the value.
		/// </summary>
		/// <value>The length of the max value.</value>
		[Export ("maxValueLength", ArgumentSemantic.Assign)]
		NSNumber MaxValueLength { get; set; }

		/// <summary>
		/// Constructor with the specified key and value.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		[Export ("initWithKey:andValue:")]
		IntPtr Constructor (string key, string value);

		/// <summary>
		/// Determines whether this instance is equal to extra data for specified extraData.
		/// </summary>
		/// <returns><c>true</c> if this instance is equal to extra data the specified extraData; otherwise, <c>false</c>.</returns>
		/// <param name="extraData">Extra data.</param>
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
	/// Holds a limited number of 32 ExtraData instances.
	/// </summary>
	[BaseType (typeof (SPLJSONModel))]
	public partial interface LimitedExtraDataList {

		/// <summary>
		/// Gets the maximum count of ExtraData instances.
		/// </summary>
		/// <value>The maximum count.</value>
		[Export ("maxCount")]
		uint MaxCount { get; set; }

		/// <summary>
		/// Gets the count of ExtraData instances of the list.
		/// </summary>
		/// <value>The count of ExtraData instances.</value>
		[Export ("count")]
		uint Count { get; set; }

		/// <summary>
		/// Gets the internal ExtraData instances array.
		/// </summary>
		/// <value>The ExtraData array.</value>
		[Export ("extraDataArray", ArgumentSemantic.Retain)]
		NSMutableArray ExtraDataArray { get; set; }

		[Static, Export ("addExtraDataToDataFixture:")]
		void AddExtraDataToDataFixture (DataFixture dataFixture);

		/// <summary>
		/// Gets the singleton shared instance.
		/// </summary>
		/// <value>The singleton shared instance.</value>
		[Static, Export ("sharedInstance")]
		LimitedExtraDataList SharedInstance { get; }

		/// <summary>
		/// Add the specified ExtraData.
		/// </summary>
		/// <param name="extraData">The ExtraData.</param>
		[Export ("add:")]
		void Add (ExtraData extraData);

		/// <summary>
		/// Remove the specified ExtraData.
		/// </summary>
		/// <param name="extraData">The ExtraData.</param>
		[Export ("remove:")]
		void Remove (ExtraData extraData);

		/// <summary>
		/// Adds an ExtraData instance to the array with key and value.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		[Export ("addWithKey:andValue:")]
		void AddWithKey (string key, string value);

		/// <summary>
		/// Removes the ExtraData instance with specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		[Export ("removeWithKey:")]
		void RemoveWithKey (string key);

		/// <summary>
		/// Returns the index of the specified ExtraData in the internal array.
		/// </summary>
		/// <returns>The of.</returns>
		/// <param name="extraData">Extra data.</param>
		[Export ("indexOf:")]
		int IndexOf (ExtraData extraData);

		/// <summary>
		/// Inserts an ExtraData instance to the specified index of the internal array.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="extraData">ExtraData.</param>
		[Export ("insertAtIndex:extraData:")]
		void InsertAtIndex (uint index, ExtraData extraData);

		/// <summary>
		/// Removes the ExtraData instance in the specified index of the internal array.
		/// </summary>
		/// <param name="index">Index.</param>
		[Export ("removeAtIndex:")]
		void RemoveAtIndex (uint index);

		/// <summary>
		/// Clear all the ExtraData instance of the array.
		/// </summary>
		[Export ("clear")]
		void Clear ();

		/// <summary>
		/// Contains the specified ExtraData.
		/// </summary>
		/// <param name="extraData">ExtraData.</param>
		[Export ("contains:")]
		bool Contains (ExtraData extraData);
	}

	/// <summary>
	/// A custom NSException derived class.
	/// </summary>
	[BaseType (typeof (NSException))]
	public partial interface MintMessageException {

		/// <summary>
		/// Constructor with specified name, reason and userInfo.
		/// </summary>
		/// <param name="aName">A name.</param>
		/// <param name="aReason">A reason.</param>
		/// <param name="aUserInfo">A user info dictionary.</param>
		[Export ("initWithName:reason:userInfo:")]
		IntPtr Constructor (string aName, string aReason, NSDictionary aUserInfo);
	}

	/// <summary>
	/// Base type used to return information to user actions.
	/// </summary>
	[BaseType (typeof (NSObject))]
	public partial interface MintResult {

		/// <summary>
		/// The type of the request.
		/// </summary>
		/// <value>The type of the request.</value>
		[Export ("requestType")]
		MintRequestType RequestType { get; set; }

		/// <summary>
		/// The result description, if any.
		/// </summary>
		/// <value>The description.</value>
		[Export ("description", ArgumentSemantic.Retain)]
		string Description { get; set; }

		/// <summary>
		/// The state of the result.
		/// </summary>
		/// <value>The state of the result.</value>
		[Export ("resultState")]
		MintResultState ResultState { get; set; }

		/// <summary>
		/// The exception related to the result, if any.
		/// </summary>
		/// <value>The exception error.</value>
		[Export ("exceptionError", ArgumentSemantic.Retain)]
		MintMessageException ExceptionError { get; set; }

		/// <summary>
		/// The JSON model of the request.
		/// </summary>
		/// <value>The JSON request.</value>
		[Export ("clientRequest", ArgumentSemantic.Retain)]
		string ClientRequest { get; set; }

		/// <summary>
		/// Indicating whether the request is handled while debugging.
		/// </summary>
		/// <value><c>true</c> if handled while debugging; otherwise, <c>false</c>.</value>
		[Export ("handledWhileDebugging")]
		bool HandledWhileDebugging { get; set; }
	}

	/// <summary>
	/// Return response information when an action is directly sent to the server.
	/// </summary>
	[BaseType (typeof (MintResult))]
	public partial interface MintResponseResult {

		/// <summary>
		/// Not applicable, deprecated.
		/// </summary>
		/// <value>The error identifier.</value>
		[Export ("errorId", ArgumentSemantic.Retain)]
		NSNumber ErrorId { get; set; }

		/// <summary>
		/// The server response description.
		/// </summary>
		/// <value>The server response.</value>
		[Export ("serverResponse", ArgumentSemantic.Retain)]
		string ServerResponse { get; set; }

		/// <summary>
		/// Not applicable, deprecated.
		/// </summary>
		/// <value>The URL.</value>
		[Export ("url", ArgumentSemantic.Retain)]
		string Url { get; set; }

		/// <summary>
		/// Not applicable, deprecated.
		/// </summary>
		/// <value>The content text.</value>
		[Export ("contentText", ArgumentSemantic.Retain)]
		string ContentText { get; set; }

		/// <summary>
		/// Not applicable, deprecated.
		/// </summary>
		/// <value>The ticker text.</value>
		[Export ("tickerText", ArgumentSemantic.Retain)]
		string TickerText { get; set; }

		/// <summary>
		/// Not applicable, deprecated.
		/// </summary>
		/// <value>The content title.</value>
		[Export ("contentTitle", ArgumentSemantic.Retain)]
		string ContentTitle { get; set; }

		/// <summary>
		/// Not applicable, deprecated.
		/// </summary>
		/// <value><c>true</c> if this instance is resolved; otherwise, <c>false</c>.</value>
		[Export ("isResolved")]
		bool IsResolved { get; set; }
	}

	/// <summary>
	/// Return log information when an action is used to log a request.
	/// </summary>
	[BaseType (typeof (MintResult))]
	public partial interface MintLogResult {

		/// <summary>
		/// The type of the log.
		/// </summary>
		/// <value>The type of the log.</value>
		[Export ("logType")]
		MintLogType LogType { get; set; }
	}

	/// <summary>
	/// Returned base instance for Transaction actions
	/// </summary>
	[BaseType (typeof (SPLJSONModel))]
	public partial interface TransactionResult {

		/// <summary>
		/// The transaction status.
		/// </summary>
		/// <value>The transaction status.</value>
		[Export ("transactionStatus")]
		TransactionStatus TransactionStatus { get; set; }

		/// <summary>
		/// A helper description.
		/// </summary>
		/// <value>The description.</value>
		[Export ("description", ArgumentSemantic.Retain)]
		string Description { get; set; }
	}

	/// <summary>
	/// Base transaction class.
	/// </summary>
	[BaseType (typeof (DataFixture))]
	public partial interface SPLTransaction {

		/// <summary>
		/// The name of the transaction.
		/// </summary>
		/// <value>The name.</value>
		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; set; }

		/// <summary>
		/// The transaction identifier, for internal purposes.
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
		/// Determines whether this JSON model is a transaction start.
		/// </summary>
		/// <returns><c>true</c> if this instance is JSON transaction start; otherwise, <c>false</c>.</returns>
		/// <param name="json">Json.</param>
		[Static, Export ("isJSONTrStart:")]
		bool IsJSONTrStart (string json);
	}

	/// <summary>
	/// Returned class with information when you start a transaction
	/// </summary>
	[BaseType (typeof (TransactionResult))]
	public partial interface TransactionStartResult {

		/// <summary>
		/// The transaction name.
		/// </summary>
		/// <value>The name of the transaction.</value>
		[Export ("transactionName", ArgumentSemantic.Retain)]
		string TransactionName { get; set; }

		/// <summary>
		/// The TrStart instance object.
		/// </summary>
		/// <value>The TrStart instance object.</value>
		[Export ("transactionStart", ArgumentSemantic.Retain)]
		TrStart TransactionStart { get; set; }
	}

	/// <summary>
	/// The tranaction stop class.
	/// </summary>
	[BaseType (typeof (SPLTransaction))]
	public partial interface TrStop {

		/// <summary>
		/// The duration of the transaction since it started in milliseconds.
		/// </summary>
		/// <value>The duration.</value>
		[Export ("duration", ArgumentSemantic.Retain)]
		NSNumber Duration { get; set; }

		/// <summary>
		/// The status of the transaction.
		/// </summary>
		/// <value>The status.</value>
		[Export ("status", ArgumentSemantic.Retain)]
		string Status { get; set; }

		/// <summary>
		/// Reason if cancelled.
		/// </summary>
		/// <value>The reason.</value>
		[Export ("reason", ArgumentSemantic.Retain)]
		string Reason { get; set; }

		[Static, Export ("getInstanceWithTransactionId:transactionName:appEnvironment:duration:reason:andCompletedStatus:")]
		TrStop GetInstanceWithTransactionId (string transactionId, string transactionName, MintAppEnvironment anAppEnvironment, NSNumber aDuration, string aReason, string aCompletedStatus);
	}

	/// <summary>
	/// Return instance for transaction stop/cancel actions.
	/// </summary>
	[BaseType (typeof (TransactionResult))]
	public partial interface TransactionStopResult {

		/// <summary>
		/// The reason of the transaction when stopped or cancelled.
		/// </summary>
		/// <value>The reason.</value>
		[Export ("reason", ArgumentSemantic.Retain)]
		string Reason { get; set; }

		/// <summary>
		/// The tranaction stop instance.
		/// </summary>
		/// <value>The transaction stop.</value>
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
	/// Network data class.
	/// </summary>
	[BaseType (typeof (DataFixture))]
	public partial interface NetworkDataFixture {

		/// <summary>
		/// The URL of the network call.
		/// </summary>
		/// <value>The URL.</value>
		[Export ("url", ArgumentSemantic.Retain)]
		string Url { get; set; }

		/// <summary>
		/// The protocol schema of the network call.
		/// </summary>
		/// <value>The protocol.</value>
		[Export ("protocol", ArgumentSemantic.Retain)]
		string Protocol { get; set; }

		/// <summary>
		/// The end time of the network call.
		/// </summary>
		/// <value>The end time.</value>
		[Export ("endTime", ArgumentSemantic.Retain)]
		NSNumber EndTime { get; set; }

		/// <summary>
		/// The duration of the network call.
		/// </summary>
		/// <value>The duration.</value>
		[Export ("duration", ArgumentSemantic.Retain)]
		NSNumber Duration { get; set; }

		/// <summary>
		/// The response status code.
		/// </summary>
		/// <value>The status code.</value>
		[Export ("statusCode", ArgumentSemantic.Retain)]
		NSNumber StatusCode { get; set; }

		/// <summary>
		/// The request content length.
		/// </summary>
		/// <value>The length.</value>
		[Export ("contentLength", ArgumentSemantic.Retain)]
		NSNumber ContentLength { get; set; }

		/// <summary>
		/// The request length.
		/// </summary>
		/// <value>The length.</value>
		[Export ("requestLength", ArgumentSemantic.Retain)]
		NSNumber RequestLength { get; set; }

		/// <summary>
		/// Indicating whether the network call is failed.
		/// </summary>
		/// <value><c>true</c> if failed; otherwise, <c>false</c>.</value>
		[Export ("failed")]
		bool Failed { get; set; }

		/// <summary>
		/// The request headers.
		/// </summary>
		/// <value>The request headers.</value>
		[Export ("reqHeaders", ArgumentSemantic.Retain)]
		NSMutableDictionary ReqHeaders { get; set; }

		/// <summary>
		/// The response headers.
		/// </summary>
		/// <value>The resp headers.</value>
		[Export ("respHeaders", ArgumentSemantic.Retain)]
		NSMutableDictionary RespHeaders { get; set; }

		/// <summary>
		/// If any exception is thrown.
		/// </summary>
		/// <value>The exception.</value>
		[Export ("exception", ArgumentSemantic.Retain)]
		string Exception { get; set; }

		/// <summary>
		/// The response length.
		/// </summary>
		/// <value>The length of the response.</value>
		[Export ("responseLength", ArgumentSemantic.Retain)]
		NSNumber ResponseLength { get; set; }

		/// <summary>
		/// The latency of the network call.
		/// </summary>
		/// <value>The latency.</value>
		[Export ("latency", ArgumentSemantic.Retain)]
		string Latency { get; set; }

		/// <summary>
		/// Helper method to append the status code to the instance.
		/// </summary>
		/// <param name="statusCode">Status code.</param>
		[Export ("appendWithStatusCode:")]
		void AppendWithStatusCode (NSNumber statusCode);

		/// <summary>
		/// Helper method to append the start time of the network call.
		/// </summary>
		[Export ("appendStartTime")]
		void AppendStartTime ();

		/// <summary>
		/// Helper method to append the end time of the network call.
		/// </summary>
		[Export ("appendEndTime")]
		void AppendEndTime ();

		/// <summary>
		/// Helper method to append the NSUrlRequest information.
		/// </summary>
		/// <param name="request">Request.</param>
		[Export ("appendRequestInfo:")]
		void AppendRequestInfo (NSUrlRequest request);

		/// <summary>
		/// Helper method to append the NSUrlResponse information.
		/// </summary>
		/// <param name="response">Response.</param>
		[Export ("appendResponseInfo:")]
		void AppendResponseInfo (NSUrlResponse response);

		/// <summary>
		/// Helper method to append the data returned from the server.
		/// </summary>
		/// <param name="data">Data.</param>
		[Export ("appendResponseData:")]
		void AppendResponseData (NSData data);

		/// <summary>
		/// Helper method to append the response data size.
		/// </summary>
		/// <param name="dataSize">Data size.</param>
		[Export ("appendResponseDataSize:")]
		void AppendResponseDataSize (uint dataSize);

		/// <summary>
		/// Helper method to append any error that is happened.
		/// </summary>
		/// <param name="error">Error.</param>
		[Export ("appendWithError:")]
		void AppendWithError (NSError error);

		/// <summary>
		/// Helper method to append the global extra data, if any.
		/// </summary>
		[Export ("appendGlobalExtraData")]
		void AppendGlobalExtraData ();

		/// <summary>
		/// For debugging purposes, prints main properties to the console.
		/// </summary>
		[Export ("debugPrint")]
		void DebugPrint ();

		/// <summary>
		/// For persisting to disk the NetworkDataFixture.
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
	/// Logged request event arguments.
	/// </summary>
	[BaseType (typeof (NSObject))]
	public partial interface LoggedRequestEventArgs {

		/// <summary>
		/// The result information.
		/// </summary>
		/// <value>The response result.</value>
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
		void SendUnhandledRequestAsync (MintExceptionRequest exceptionRequest, ResponseResultBlock resultBlock);

		[Export ("getErrorHashFromJson:")]
		string GetErrorHashFromJson (string jsonRequest);

		[Export ("flushAsyncWithBlock:")]
		void FlushAsync (ResponseResultBlock resultBlock);

		[Export ("transactionStartWithName:andResultBlock:")]
		void TransactionStart (string transactionName, TransactionStartResultBlock resultBlock);

		[Export ("transactionStopWithName:andResultBlock:")]
		void TransactionStop (string transactionName, TransactionStopResultBlock resultBlock);

		[Export ("transactionCancelWithName:reason:andResultBlock:")]
		void TransactionCancel (string transactionName, string reason, TransactionStopResultBlock resultBlock);

		[Export ("stopAllTransactions:")]
		void StopAllTransactions (string errorHash);

		[Export ("startWorker")]
		void StartWorker ();

		[Export ("sendEventAsync:completionBlock:")]
		void SendEventAsync (DataType eventType, ResponseResultBlock completed);

		[Export ("logEventAsync:completionBlock:")]
		void LogEventAsync (DataType eventType, LogResultBlock completed);

		[Export ("logEvent:")]
		MintLogResult LogEvent (DataType eventType);

		[Export ("processPreviousLoggedRequestsAsyncWithBlock:")]
		void ProcessPreviousLoggedRequestsAsync (ResponseResultBlock resultBlock);

		[Export ("getLastCrashIdWithBlock:failure:")]
		void GetLastCrashId (Delegate success, FailureBlock failure);

		[Export ("getTotalCrashesNumWithBlock:failure:")]
		void GetTotalCrashesNum (Delegate success, FailureBlock failure);

		[Export ("clearTotalCrashesNumWithBlock:failure:")]
		void ClearTotalCrashesNum (Delegate success, FailureBlock failure);

		[Export ("sendEventAsyncWithTag:completionBlock:")]
		void SendEventAsync (string tag, ResponseResultBlock completed);

		[Export ("logEventAsyncWithTag:completionBlock:")]
		void LogEventAsync (string tag, LogResultBlock completed);

		[Export ("closeSession")]
		MintLogResult CloseSession { get; }

		[Export ("startSessionAsyncWithCompletionBlock:")]
		void StartSessionAsync (ResponseResultBlock completed);

		[Export ("closeSessionAsyncWithCompletionBlock:")]
		void CloseSessionAsync (LogResultBlock completed);

		[Export ("sendExceptionAsync:extraDataKey:extraDataValue:completionBlock:")]
		void SendExceptionAsync (NSException exception, string key, string value, ResponseResultBlock completed);

		[Export ("sendExceptionAsync:limitedExtraDataList:completionBlock:")]
		void SendExceptionAsync (NSException exception, LimitedExtraDataList extraDataList, ResponseResultBlock completed);

		[Export ("logExceptionAsync:extraDataKey:extraDataValue:completionBlock:")]
		void LogExceptionAsync (NSException exception, string key, string value, LogResultBlock completed);

		[Export ("logExceptionAsync:limitedExtraDataList:completionBlock:")]
		void LogExceptionAsync (NSException exception, LimitedExtraDataList extraDataList, LogResultBlock completed);

		[Export ("logException:extraDataKey:extraDataValue:")]
		MintLogResult LogException (NSException exception, string key, string value);

		[Export ("logException:limitedExtraDataList:")]
		MintLogResult LogException (NSException exception, LimitedExtraDataList extraDataList);

		[Export ("logEventAsyncWithName:logLevel:andCompletionBlock:")]
		void LogEventAsync (string name, MintLogLevel logLevel, LogResultBlock completed);

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
	/// Mint notification delegate.
	/// </summary>
	[Model, BaseType (typeof (NSObject))]
	[Protocol]
	public partial interface MintNotificationDelegate {

		/// <summary>
		/// Provides you with the related information when the cached requests are sent to the server.
		/// </summary>
		/// <param name="args">Logged request informationa arguments.</param>
		[Export ("loggedRequestHandled:")]
		void CachedRequestsSent (LoggedRequestEventArgs args);

		/// <summary>
		/// Provide network data infromation of the call that intercepted.
		/// </summary>
		/// <param name="networkData">The network data.</param>
		[Export ("networkDataLogged:")]
		void NetworkDataIntercepted (NetworkDataFixture networkData);
	}

	//	, Delegates=new string [] {"WeakDelegate"},
	//	Events=new Type [] { typeof (MintNotificationDelegate) }
	/// <summary>
	/// The Splunk>MINT base class
	/// </summary>
	[BaseType (typeof (NSObject), Delegates=new string [] {"WeakDelegate"}, Events=new Type [] { typeof (MintNotificationDelegate) })]
	public partial interface BugSenseBase : RequestWorkerDelegate {

		[Export ("splunkRequestWorker", ArgumentSemantic.Retain)]
		RequestWorkerFacadeDelegate SplunkRequestWorker { get; set; }

		/// <summary>
		/// Whether the plugin is initiaized.
		/// </summary>
		/// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
		[Export ("isInitialized")]
		bool IsInitialized { get; set; }

		/// <summary>
		/// If the session is active.
		/// </summary>
		/// <value><c>true</c> if this instance is session active; otherwise, <c>false</c>.</value>
		[Export ("isSessionActive")]
		bool IsSessionActive { get; set; }

		/// <summary>
		/// The user identifier.
		/// </summary>
		/// <value>The user identifier.</value>
		[Export ("userIdentifier", ArgumentSemantic.Retain)]
		string UserIdentifier { get; set; }

		/// <summary>
		/// Indicating whether Splunk>MINT will handle requests while debugging.
		/// </summary>
		/// <value><c>true</c> if handle while debugging; otherwise, <c>false</c>.</value>
		[Export ("handleWhileDebugging")]
		bool HandleWhileDebugging { get; set; }

		/// <summary>
		/// The LimitedExtraDataList global instance.
		/// </summary>
		/// <value>The extra data list.</value>
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
		/// Flushes all the cached requests logged to the server.
		/// </summary>
		[Export ("flushAsyncWithBlock:")]
		[Async]
		void Flush (ResponseResultBlock resultBlock);

		/// <summary>
		/// Initializes the plugin and starts a new session.
		/// </summary>
		/// <param name="apiKey">API key.</param>
		[Export ("initAndStartSession:")]
		void InitAndStartSession (string apiKey);

		/// <summary>
		/// Adds an ExtraData instance to the global LimitedExtraDataList.
		/// </summary>
		/// <param name="extraData">Extra data.</param>
		[Export ("addExtraData:")]
		void AddExtraData (ExtraData extraData);

		/// <summary>
		/// Adds a LimitedExtraDataList instance with appending the ExtraData instance to the global LimitedExtraDataList.
		/// </summary>
		/// <param name="limitedExtraDataList">Limited extra data list.</param>
		[Export ("addExtraDataList:")]
		void AddExtraDataList (LimitedExtraDataList limitedExtraDataList);

		/// <summary>
		/// Removes an ExtraData instance with the specified key.
		/// </summary>
		/// <returns><c>true</c>, if extra data with key was removed, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		[Export ("removeExtraDataWithKey:")]
		bool RemoveExtraDataWithKey (string key);

		/// <summary>
		/// Clears the global LimitedExtraDataList ExtraData instances.
		/// </summary>
		[Export ("clearExtraData")]
		void ClearExtraData ();

		/// <summary>
		/// Appends a breadcrumb to the global breadcrumbs list.
		/// </summary>
		/// <param name="crumb">The Breadcrumb.</param>
		[Export ("leaveBreadcrumb:")]
		void LeaveBreadcrumb (string crumb);

		/// <summary>
		/// Clears the breadcrumbs from the global list.
		/// </summary>
		[Export ("clearBreadcrumbs")]
		void ClearBreadcrumbs ();

		/// <summary>
		/// Logs an event with tag.
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
		/// Close the active session.
		/// </summary>
		/// <param name="completed">The completed callback.</param>
		[Export ("closeSessionAsyncWithCompletionBlock:")]
		[Async]
		void CloseSession ([NullAllowed] LogResultBlock completed);

		/// <summary>
		/// Logs a handled exception with extra data key-value.
		/// </summary>
		/// <param name="exception">The NSException instance.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="completed">The completed callback.</param>
		[Export ("logExceptionAsync:extraDataKey:extraDataValue:completionBlock:")]
		[Async]
		void LogException (NSException exception, string key, string value, [NullAllowed] LogResultBlock completed);

		/// <summary>
		/// Logs a handled exception with a LimitedExtraDataList instance.
		/// </summary>
		/// <param name="exception">The NSException instance.</param>
		/// <param name="extraDataList">The LimitedExtraDataList.</param>
		/// <param name="completed">The completed callback.</param>
		[Export ("logExceptionAsync:limitedExtraDataList:completionBlock:")]
		[Async]
		void LogException (NSException exception, [NullAllowed] LimitedExtraDataList extraDataList, [NullAllowed] LogResultBlock completed);
	}

	/// <summary>
	/// The base Splunk>MINT BugSense handler.
	/// </summary>
	[BaseType (typeof (BugSenseBase))]
	public partial interface BugSense {

		//		[Export ("notificationDelegate", ArgumentSemantic.Assign)]
		//		MintNotificationDelegate NotificationDelegate { get; set; }

		/// <summary>
		/// The singleton shared instance.
		/// </summary>
		/// <value>The shared instance.</value>
		[Static, Export ("sharedInstance")]
		BugSense SharedInstance { get; }

		[Export ("initWithRequestJsonSerializer:contentTypeResolver:requestWorker:andServiceRepository:")]
		IntPtr Constructor (RequestJsonSerializerDelegate jsonSerializer, ContentTypeDelegate contentTypeResolver, RequestWorkerFacadeDelegate requestWorker, ServiceClientDelegate serviceRepository);
	}

	/// <summary>
	/// The base Splunk>MINT handler.
	/// </summary>
	[BaseType (typeof (BugSense))]
	public partial interface MintBase {

		/// <summary>
		/// Disables the network monitoring.
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
		/// Enables the logging of any debug message using the MintLog function.
		/// </summary>
		/// <param name="value">If set to <c>true</c> value.</param>
		[Export ("enableMintLoggingCache:")]
		void EnableLoggingCache (bool value);

		/// <summary>
		/// Enables the logging of the console output.
		/// </summary>
		/// <param name="value">If set to <c>true</c> value.</param>
		[Export ("enableLogging:")]
		void EnableLogging (bool value);

		/// <summary>
		/// Sets how many console lines to log.
		/// </summary>
		/// <param name="linesCount">Console lines count.</param>
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
		/// Starts a transaction.
		/// </summary>
		/// <param name="transactionId">The transaction identifier.</param>
		/// <param name="reason">The reason to cancel.</param>
		/// <param name="resultBlock">The completed callback.</param>
		[Export ("transactionCancel:reason:andResultBlock:")]
		[Async]
		void TransactionCancel (string transactionId, string reason, [NullAllowed] TransactionStopResultBlock resultBlock);

		/// <summary>
		/// Adds a URL to blacklist from network monitoring.
		/// </summary>
		/// <param name="url">The URL.</param>
		[Export ("addURLToBlackList:")]
		void AddURLToBlacklist (string url);

		/// <summary>
		/// Log an event with message and log level.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="logLevel">The log level.</param>
		/// <param name="resultBlock">The completed callback.</param>
		[Export ("logEventAsyncWithName:logLevel:andCompletionBlock:")]
		[Async]
		void LogEventWithName (string message, MintLogLevel logLevel, [NullAllowed] LogResultBlock resultBlock); 
	}

	/// <summary>
	/// The Splunk>MINT handler class.
	/// </summary>
	[BaseType (typeof (MintBase))]
	public partial interface Mint {

		/// <summary>
		/// The singleton shared instance.
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