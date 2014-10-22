using System;

namespace SplunkMint
{
	public enum SPLJSONModelErrorTypes {
		JSONModelErrorInvalidData = 1,
		JSONModelErrorBadResponse = 2,
		JSONModelErrorBadJSON = 3,
		JSONModelErrorModelIsInvalid = 4,
		JSONModelErrorNilInput = 5
	}

	public enum CrashManagerType {
		PLCrashManager
	}

	public enum MintResultState {
		OKResultState = 0,
		ErrorResultState,
		UndefinedResultState
	}

	public enum MintRequestType {
		ErrorRequestType = 0,
		EventRequestType,
		BothRequestType
	}

	public enum MintLogType {
		LoggedException = 0,
		EventLogType
	}

	public enum DeviceConnectionState {
		OffDeviceConnectionState = 0,
		OnDeviceConnectionState = 1,
		NADeviceConnectionState = 2
	}

	public enum FileNameType {
		UnhandledExceptionFileNameType = 0,
		LoggedExceptionFileNameType,
		PingFileNameType,
		GnipFileNameType,
		EventFileNameType,
		TransactionStartFileNameType,
		TransactionStopFileNameType,
		NetworkFileNameType,
		PerformanceFileNameType,
		ScreenFileNameType
	}

	public enum DataType {
		Error = 0,
		Event,
		Ping,
		Gnip,
		Trstart,
		Trstop,
		Network,
		Performance,
		Screen
	}

	public enum TransactionStatus {
		SuccessfullyStartedTransaction = 0,
		UserCancelledTransaction,
		UserSuccessfullyStoppedTransaction,
		FailedTransaction,
		ExistsTransaction,
		NotFoundTransaction
	}

	public enum ConnectionType {
		Wifi = 0,
		Connection3G,
		Connection2G,
		NONE,
		NA
	}

	public enum MintLogLevel {
		DebugLogLevel = 20,
		InfoLogLevel = 30,
		NoticeLogLevel = 40,
		WarningLogLevel = 50,
		ErrorLogLevel = 60,
		CriticalLogLevel = 70,
		AlertLogLevel = 80,
		EmergencyLogLevel = 90
	}
}