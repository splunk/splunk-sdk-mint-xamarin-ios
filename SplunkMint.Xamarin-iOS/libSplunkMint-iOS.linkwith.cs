using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libSplunkMint-iOS.a", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, Frameworks = "CoreTelephony SystemConfiguration UIKit Foundation CoreGraphics", LinkerFlags = "-ObjC -all_load -lz -lstdc++", ForceLoad = true )]
