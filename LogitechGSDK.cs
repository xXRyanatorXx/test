using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class LogitechGSDK
{
	// Token: 0x06000536 RID: 1334
	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxInit(string identifier, string friendlyName, ref LogitechGSDK.logiArxCbContext callback);

	// Token: 0x06000537 RID: 1335
	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxAddFileAs(string filePath, string fileName, string mimeType);

	// Token: 0x06000538 RID: 1336
	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxAddContentAs(byte[] content, int size, string fileName, string mimeType = "");

	// Token: 0x06000539 RID: 1337
	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxAddUTF8StringAs(string stringContent, string fileName, string mimeType = "");

	// Token: 0x0600053A RID: 1338
	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxAddImageFromBitmap(byte[] bitmap, int width, int height, string fileName);

	// Token: 0x0600053B RID: 1339
	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxSetIndex(string fileName);

	// Token: 0x0600053C RID: 1340
	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxSetTagPropertyById(string tagId, string prop, string newValue);

	// Token: 0x0600053D RID: 1341
	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxSetTagsPropertyByClass(string tagsClass, string prop, string newValue);

	// Token: 0x0600053E RID: 1342
	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxSetTagContentById(string tagId, string newContent);

	// Token: 0x0600053F RID: 1343
	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiArxSetTagsContentByClass(string tagsClass, string newContent);

	// Token: 0x06000540 RID: 1344
	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiArxGetLastError();

	// Token: 0x06000541 RID: 1345
	[DllImport("LogitechGArxControlEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern void LogiArxShutdown();

	// Token: 0x06000542 RID: 1346
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedInit();

	// Token: 0x06000543 RID: 1347
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetTargetDevice(int targetDevice);

	// Token: 0x06000544 RID: 1348
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedGetSdkVersion(ref int majorNum, ref int minorNum, ref int buildNum);

	// Token: 0x06000545 RID: 1349
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSaveCurrentLighting();

	// Token: 0x06000546 RID: 1350
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLighting(int redPercentage, int greenPercentage, int bluePercentage);

	// Token: 0x06000547 RID: 1351
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedRestoreLighting();

	// Token: 0x06000548 RID: 1352
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedFlashLighting(int redPercentage, int greenPercentage, int bluePercentage, int milliSecondsDuration, int milliSecondsInterval);

	// Token: 0x06000549 RID: 1353
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedPulseLighting(int redPercentage, int greenPercentage, int bluePercentage, int milliSecondsDuration, int milliSecondsInterval);

	// Token: 0x0600054A RID: 1354
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedStopEffects();

	// Token: 0x0600054B RID: 1355
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLightingFromBitmap(byte[] bitmap);

	// Token: 0x0600054C RID: 1356
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLightingForKeyWithScanCode(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

	// Token: 0x0600054D RID: 1357
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLightingForKeyWithHidCode(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

	// Token: 0x0600054E RID: 1358
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLightingForKeyWithQuartzCode(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

	// Token: 0x0600054F RID: 1359
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLightingForKeyWithKeyName(LogitechGSDK.keyboardNames keyCode, int redPercentage, int greenPercentage, int bluePercentage);

	// Token: 0x06000550 RID: 1360
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSetLightingForTargetZone(LogitechGSDK.DeviceType deviceType, int zone, int redPercentage, int greenPercentage, int bluePercentage);

	// Token: 0x06000551 RID: 1361
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedSaveLightingForKey(LogitechGSDK.keyboardNames keyName);

	// Token: 0x06000552 RID: 1362
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedRestoreLightingForKey(LogitechGSDK.keyboardNames keyName);

	// Token: 0x06000553 RID: 1363
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedFlashSingleKey(LogitechGSDK.keyboardNames keyName, int redPercentage, int greenPercentage, int bluePercentage, int msDuration, int msInterval);

	// Token: 0x06000554 RID: 1364
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedPulseSingleKey(LogitechGSDK.keyboardNames keyName, int startRedPercentage, int startGreenPercentage, int startBluePercentage, int finishRedPercentage, int finishGreenPercentage, int finishBluePercentage, int msDuration, bool isInfinite);

	// Token: 0x06000555 RID: 1365
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool LogiLedStopEffectsOnKey(LogitechGSDK.keyboardNames keyName);

	// Token: 0x06000556 RID: 1366
	[DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern void LogiLedShutdown();

	// Token: 0x06000557 RID: 1367
	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdInit(string friendlyName, int lcdType);

	// Token: 0x06000558 RID: 1368
	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdIsConnected(int lcdType);

	// Token: 0x06000559 RID: 1369
	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdIsButtonPressed(int button);

	// Token: 0x0600055A RID: 1370
	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern void LogiLcdUpdate();

	// Token: 0x0600055B RID: 1371
	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern void LogiLcdShutdown();

	// Token: 0x0600055C RID: 1372
	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdMonoSetBackground(byte[] monoBitmap);

	// Token: 0x0600055D RID: 1373
	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdMonoSetText(int lineNumber, string text);

	// Token: 0x0600055E RID: 1374
	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdColorSetBackground(byte[] colorBitmap);

	// Token: 0x0600055F RID: 1375
	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdColorSetTitle(string text, int red, int green, int blue);

	// Token: 0x06000560 RID: 1376
	[DllImport("LogitechLcdEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiLcdColorSetText(int lineNumber, string text, int red, int green, int blue);

	// Token: 0x06000561 RID: 1377
	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGkeyInitWithoutCallback();

	// Token: 0x06000562 RID: 1378
	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGkeyInitWithoutContext(LogitechGSDK.logiGkeyCB gkeyCB);

	// Token: 0x06000563 RID: 1379
	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGkeyInit(ref LogitechGSDK.logiGKeyCbContext cbStruct);

	// Token: 0x06000564 RID: 1380
	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGkeyIsMouseButtonPressed(int buttonNumber);

	// Token: 0x06000565 RID: 1381
	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern IntPtr LogiGkeyGetMouseButtonString(int buttonNumber);

	// Token: 0x06000566 RID: 1382 RVA: 0x0002A58A File Offset: 0x0002878A
	public static string LogiGkeyGetMouseButtonStr(int buttonNumber)
	{
		return Marshal.PtrToStringUni(LogitechGSDK.LogiGkeyGetMouseButtonString(buttonNumber));
	}

	// Token: 0x06000567 RID: 1383
	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGkeyIsKeyboardGkeyPressed(int gkeyNumber, int modeNumber);

	// Token: 0x06000568 RID: 1384
	[DllImport("LogitechGKeyEnginesWrapper")]
	private static extern IntPtr LogiGkeyGetKeyboardGkeyString(int gkeyNumber, int modeNumber);

	// Token: 0x06000569 RID: 1385 RVA: 0x0002A597 File Offset: 0x00028797
	public static string LogiGkeyGetKeyboardGkeyStr(int gkeyNumber, int modeNumber)
	{
		return Marshal.PtrToStringUni(LogitechGSDK.LogiGkeyGetKeyboardGkeyString(gkeyNumber, modeNumber));
	}

	// Token: 0x0600056A RID: 1386
	[DllImport("LogitechGKeyEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern void LogiGkeyShutdown();

	// Token: 0x0600056B RID: 1387
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiSteeringInitialize(bool ignoreXInputControllers);

	// Token: 0x0600056C RID: 1388
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiSteeringShutdown();

	// Token: 0x0600056D RID: 1389
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiUpdate();

	// Token: 0x0600056E RID: 1390
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr LogiGetStateENGINES(int index);

	// Token: 0x0600056F RID: 1391 RVA: 0x0002A5A8 File Offset: 0x000287A8
	public static LogitechGSDK.DIJOYSTATE2ENGINES LogiGetStateUnity(int index)
	{
		LogitechGSDK.DIJOYSTATE2ENGINES result = default(LogitechGSDK.DIJOYSTATE2ENGINES);
		result.rglSlider = new int[2];
		result.rgdwPOV = new uint[4];
		result.rgbButtons = new byte[128];
		result.rglVSlider = new int[2];
		result.rglASlider = new int[2];
		result.rglFSlider = new int[2];
		try
		{
			result = (LogitechGSDK.DIJOYSTATE2ENGINES)Marshal.PtrToStructure(LogitechGSDK.LogiGetStateENGINES(index), typeof(LogitechGSDK.DIJOYSTATE2ENGINES));
		}
		catch (ArgumentException)
		{
			Debug.Log("Exception catched");
		}
		return result;
	}

	// Token: 0x06000570 RID: 1392
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiGetFriendlyProductName(int index, StringBuilder buffer, int bufferSize);

	// Token: 0x06000571 RID: 1393
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsConnected(int index);

	// Token: 0x06000572 RID: 1394
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsDeviceConnected(int index, int deviceType);

	// Token: 0x06000573 RID: 1395
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsManufacturerConnected(int index, int manufacturerName);

	// Token: 0x06000574 RID: 1396
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsModelConnected(int index, int modelName);

	// Token: 0x06000575 RID: 1397
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiButtonTriggered(int index, int buttonNbr);

	// Token: 0x06000576 RID: 1398
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiButtonReleased(int index, int buttonNbr);

	// Token: 0x06000577 RID: 1399
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiButtonIsPressed(int index, int buttonNbr);

	// Token: 0x06000578 RID: 1400
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiGenerateNonLinearValues(int index, int nonLinCoeff);

	// Token: 0x06000579 RID: 1401
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGetNonLinearValue(int index, int inputValue);

	// Token: 0x0600057A RID: 1402
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiHasForceFeedback(int index);

	// Token: 0x0600057B RID: 1403
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiIsPlaying(int index, int forceType);

	// Token: 0x0600057C RID: 1404
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySpringForce(int index, int offsetPercentage, int saturationPercentage, int coefficientPercentage);

	// Token: 0x0600057D RID: 1405
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopSpringForce(int index);

	// Token: 0x0600057E RID: 1406
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayConstantForce(int index, int magnitudePercentage);

	// Token: 0x0600057F RID: 1407
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopConstantForce(int index);

	// Token: 0x06000580 RID: 1408
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayDamperForce(int index, int coefficientPercentage);

	// Token: 0x06000581 RID: 1409
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopDamperForce(int index);

	// Token: 0x06000582 RID: 1410
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySideCollisionForce(int index, int magnitudePercentage);

	// Token: 0x06000583 RID: 1411
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayFrontalCollisionForce(int index, int magnitudePercentage);

	// Token: 0x06000584 RID: 1412
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayDirtRoadEffect(int index, int magnitudePercentage);

	// Token: 0x06000585 RID: 1413
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopDirtRoadEffect(int index);

	// Token: 0x06000586 RID: 1414
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayBumpyRoadEffect(int index, int magnitudePercentage);

	// Token: 0x06000587 RID: 1415
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopBumpyRoadEffect(int index);

	// Token: 0x06000588 RID: 1416
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySlipperyRoadEffect(int index, int magnitudePercentage);

	// Token: 0x06000589 RID: 1417
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopSlipperyRoadEffect(int index);

	// Token: 0x0600058A RID: 1418
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySurfaceEffect(int index, int type, int magnitudePercentage, int period);

	// Token: 0x0600058B RID: 1419
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopSurfaceEffect(int index);

	// Token: 0x0600058C RID: 1420
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayCarAirborne(int index);

	// Token: 0x0600058D RID: 1421
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopCarAirborne(int index);

	// Token: 0x0600058E RID: 1422
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlaySoftstopForce(int index, int usableRangePercentage);

	// Token: 0x0600058F RID: 1423
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiStopSoftstopForce(int index);

	// Token: 0x06000590 RID: 1424
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiSetPreferredControllerProperties(LogitechGSDK.LogiControllerPropertiesData properties);

	// Token: 0x06000591 RID: 1425
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiGetCurrentControllerProperties(int index, ref LogitechGSDK.LogiControllerPropertiesData properties);

	// Token: 0x06000592 RID: 1426
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern int LogiGetShifterMode(int index);

	// Token: 0x06000593 RID: 1427
	[DllImport("LogitechSteeringWheelEnginesWrapper", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	public static extern bool LogiPlayLeds(int index, float currentRPM, float rpmFirstLedTurnsOn, float rpmRedLine);

	// Token: 0x040006E4 RID: 1764
	public const int LOGI_ARX_ORIENTATION_PORTRAIT = 1;

	// Token: 0x040006E5 RID: 1765
	public const int LOGI_ARX_ORIENTATION_LANDSCAPE = 16;

	// Token: 0x040006E6 RID: 1766
	public const int LOGI_ARX_EVENT_FOCUS_ACTIVE = 1;

	// Token: 0x040006E7 RID: 1767
	public const int LOGI_ARX_EVENT_FOCUS_INACTIVE = 2;

	// Token: 0x040006E8 RID: 1768
	public const int LOGI_ARX_EVENT_TAP_ON_TAG = 4;

	// Token: 0x040006E9 RID: 1769
	public const int LOGI_ARX_EVENT_MOBILEDEVICE_ARRIVAL = 8;

	// Token: 0x040006EA RID: 1770
	public const int LOGI_ARX_EVENT_MOBILEDEVICE_REMOVAL = 16;

	// Token: 0x040006EB RID: 1771
	public const int LOGI_ARX_DEVICETYPE_IPHONE = 1;

	// Token: 0x040006EC RID: 1772
	public const int LOGI_ARX_DEVICETYPE_IPAD = 2;

	// Token: 0x040006ED RID: 1773
	public const int LOGI_ARX_DEVICETYPE_ANDROID_SMALL = 3;

	// Token: 0x040006EE RID: 1774
	public const int LOGI_ARX_DEVICETYPE_ANDROID_NORMAL = 4;

	// Token: 0x040006EF RID: 1775
	public const int LOGI_ARX_DEVICETYPE_ANDROID_LARGE = 5;

	// Token: 0x040006F0 RID: 1776
	public const int LOGI_ARX_DEVICETYPE_ANDROID_XLARGE = 6;

	// Token: 0x040006F1 RID: 1777
	public const int LOGI_ARX_DEVICETYPE_ANDROID_OTHER = 7;

	// Token: 0x040006F2 RID: 1778
	public const int LOGI_LED_BITMAP_WIDTH = 21;

	// Token: 0x040006F3 RID: 1779
	public const int LOGI_LED_BITMAP_HEIGHT = 6;

	// Token: 0x040006F4 RID: 1780
	public const int LOGI_LED_BITMAP_BYTES_PER_KEY = 4;

	// Token: 0x040006F5 RID: 1781
	public const int LOGI_LED_BITMAP_SIZE = 504;

	// Token: 0x040006F6 RID: 1782
	public const int LOGI_LED_DURATION_INFINITE = 0;

	// Token: 0x040006F7 RID: 1783
	private const int LOGI_DEVICETYPE_MONOCHROME_ORD = 0;

	// Token: 0x040006F8 RID: 1784
	private const int LOGI_DEVICETYPE_RGB_ORD = 1;

	// Token: 0x040006F9 RID: 1785
	private const int LOGI_DEVICETYPE_PERKEY_RGB_ORD = 2;

	// Token: 0x040006FA RID: 1786
	public const int LOGI_DEVICETYPE_MONOCHROME = 1;

	// Token: 0x040006FB RID: 1787
	public const int LOGI_DEVICETYPE_RGB = 2;

	// Token: 0x040006FC RID: 1788
	public const int LOGI_DEVICETYPE_PERKEY_RGB = 4;

	// Token: 0x040006FD RID: 1789
	public const int LOGI_LCD_COLOR_BUTTON_LEFT = 256;

	// Token: 0x040006FE RID: 1790
	public const int LOGI_LCD_COLOR_BUTTON_RIGHT = 512;

	// Token: 0x040006FF RID: 1791
	public const int LOGI_LCD_COLOR_BUTTON_OK = 1024;

	// Token: 0x04000700 RID: 1792
	public const int LOGI_LCD_COLOR_BUTTON_CANCEL = 2048;

	// Token: 0x04000701 RID: 1793
	public const int LOGI_LCD_COLOR_BUTTON_UP = 4096;

	// Token: 0x04000702 RID: 1794
	public const int LOGI_LCD_COLOR_BUTTON_DOWN = 8192;

	// Token: 0x04000703 RID: 1795
	public const int LOGI_LCD_COLOR_BUTTON_MENU = 16384;

	// Token: 0x04000704 RID: 1796
	public const int LOGI_LCD_MONO_BUTTON_0 = 1;

	// Token: 0x04000705 RID: 1797
	public const int LOGI_LCD_MONO_BUTTON_1 = 2;

	// Token: 0x04000706 RID: 1798
	public const int LOGI_LCD_MONO_BUTTON_2 = 4;

	// Token: 0x04000707 RID: 1799
	public const int LOGI_LCD_MONO_BUTTON_3 = 8;

	// Token: 0x04000708 RID: 1800
	public const int LOGI_LCD_MONO_WIDTH = 160;

	// Token: 0x04000709 RID: 1801
	public const int LOGI_LCD_MONO_HEIGHT = 43;

	// Token: 0x0400070A RID: 1802
	public const int LOGI_LCD_COLOR_WIDTH = 320;

	// Token: 0x0400070B RID: 1803
	public const int LOGI_LCD_COLOR_HEIGHT = 240;

	// Token: 0x0400070C RID: 1804
	public const int LOGI_LCD_TYPE_MONO = 1;

	// Token: 0x0400070D RID: 1805
	public const int LOGI_LCD_TYPE_COLOR = 2;

	// Token: 0x0400070E RID: 1806
	public const int LOGITECH_MAX_MOUSE_BUTTONS = 20;

	// Token: 0x0400070F RID: 1807
	public const int LOGITECH_MAX_GKEYS = 29;

	// Token: 0x04000710 RID: 1808
	public const int LOGITECH_MAX_M_STATES = 3;

	// Token: 0x04000711 RID: 1809
	public const int LOGI_MAX_CONTROLLERS = 2;

	// Token: 0x04000712 RID: 1810
	public const int LOGI_FORCE_NONE = -1;

	// Token: 0x04000713 RID: 1811
	public const int LOGI_FORCE_SPRING = 0;

	// Token: 0x04000714 RID: 1812
	public const int LOGI_FORCE_CONSTANT = 1;

	// Token: 0x04000715 RID: 1813
	public const int LOGI_FORCE_DAMPER = 2;

	// Token: 0x04000716 RID: 1814
	public const int LOGI_FORCE_SIDE_COLLISION = 3;

	// Token: 0x04000717 RID: 1815
	public const int LOGI_FORCE_FRONTAL_COLLISION = 4;

	// Token: 0x04000718 RID: 1816
	public const int LOGI_FORCE_DIRT_ROAD = 5;

	// Token: 0x04000719 RID: 1817
	public const int LOGI_FORCE_BUMPY_ROAD = 6;

	// Token: 0x0400071A RID: 1818
	public const int LOGI_FORCE_SLIPPERY_ROAD = 7;

	// Token: 0x0400071B RID: 1819
	public const int LOGI_FORCE_SURFACE_EFFECT = 8;

	// Token: 0x0400071C RID: 1820
	public const int LOGI_NUMBER_FORCE_EFFECTS = 9;

	// Token: 0x0400071D RID: 1821
	public const int LOGI_FORCE_SOFTSTOP = 10;

	// Token: 0x0400071E RID: 1822
	public const int LOGI_FORCE_CAR_AIRBORNE = 11;

	// Token: 0x0400071F RID: 1823
	public const int LOGI_PERIODICTYPE_NONE = -1;

	// Token: 0x04000720 RID: 1824
	public const int LOGI_PERIODICTYPE_SINE = 0;

	// Token: 0x04000721 RID: 1825
	public const int LOGI_PERIODICTYPE_SQUARE = 1;

	// Token: 0x04000722 RID: 1826
	public const int LOGI_PERIODICTYPE_TRIANGLE = 2;

	// Token: 0x04000723 RID: 1827
	public const int LOGI_DEVICE_TYPE_NONE = -1;

	// Token: 0x04000724 RID: 1828
	public const int LOGI_DEVICE_TYPE_WHEEL = 0;

	// Token: 0x04000725 RID: 1829
	public const int LOGI_DEVICE_TYPE_JOYSTICK = 1;

	// Token: 0x04000726 RID: 1830
	public const int LOGI_DEVICE_TYPE_GAMEPAD = 2;

	// Token: 0x04000727 RID: 1831
	public const int LOGI_DEVICE_TYPE_OTHER = 3;

	// Token: 0x04000728 RID: 1832
	public const int LOGI_NUMBER_DEVICE_TYPES = 4;

	// Token: 0x04000729 RID: 1833
	public const int LOGI_MANUFACTURER_NONE = -1;

	// Token: 0x0400072A RID: 1834
	public const int LOGI_MANUFACTURER_LOGITECH = 0;

	// Token: 0x0400072B RID: 1835
	public const int LOGI_MANUFACTURER_MICROSOFT = 1;

	// Token: 0x0400072C RID: 1836
	public const int LOGI_MANUFACTURER_OTHER = 2;

	// Token: 0x0400072D RID: 1837
	public const int LOGI_MODEL_G27 = 0;

	// Token: 0x0400072E RID: 1838
	public const int LOGI_MODEL_DRIVING_FORCE_GT = 1;

	// Token: 0x0400072F RID: 1839
	public const int LOGI_MODEL_G25 = 2;

	// Token: 0x04000730 RID: 1840
	public const int LOGI_MODEL_MOMO_RACING = 3;

	// Token: 0x04000731 RID: 1841
	public const int LOGI_MODEL_MOMO_FORCE = 4;

	// Token: 0x04000732 RID: 1842
	public const int LOGI_MODEL_DRIVING_FORCE_PRO = 5;

	// Token: 0x04000733 RID: 1843
	public const int LOGI_MODEL_DRIVING_FORCE = 6;

	// Token: 0x04000734 RID: 1844
	public const int LOGI_MODEL_NASCAR_RACING_WHEEL = 7;

	// Token: 0x04000735 RID: 1845
	public const int LOGI_MODEL_FORMULA_FORCE = 8;

	// Token: 0x04000736 RID: 1846
	public const int LOGI_MODEL_FORMULA_FORCE_GP = 9;

	// Token: 0x04000737 RID: 1847
	public const int LOGI_MODEL_FORCE_3D_PRO = 10;

	// Token: 0x04000738 RID: 1848
	public const int LOGI_MODEL_EXTREME_3D_PRO = 11;

	// Token: 0x04000739 RID: 1849
	public const int LOGI_MODEL_FREEDOM_24 = 12;

	// Token: 0x0400073A RID: 1850
	public const int LOGI_MODEL_ATTACK_3 = 13;

	// Token: 0x0400073B RID: 1851
	public const int LOGI_MODEL_FORCE_3D = 14;

	// Token: 0x0400073C RID: 1852
	public const int LOGI_MODEL_STRIKE_FORCE_3D = 15;

	// Token: 0x0400073D RID: 1853
	public const int LOGI_MODEL_G940_JOYSTICK = 16;

	// Token: 0x0400073E RID: 1854
	public const int LOGI_MODEL_G940_THROTTLE = 17;

	// Token: 0x0400073F RID: 1855
	public const int LOGI_MODEL_G940_PEDALS = 18;

	// Token: 0x04000740 RID: 1856
	public const int LOGI_MODEL_RUMBLEPAD = 19;

	// Token: 0x04000741 RID: 1857
	public const int LOGI_MODEL_RUMBLEPAD_2 = 20;

	// Token: 0x04000742 RID: 1858
	public const int LOGI_MODEL_CORDLESS_RUMBLEPAD_2 = 21;

	// Token: 0x04000743 RID: 1859
	public const int LOGI_MODEL_CORDLESS_GAMEPAD = 22;

	// Token: 0x04000744 RID: 1860
	public const int LOGI_MODEL_DUAL_ACTION_GAMEPAD = 23;

	// Token: 0x04000745 RID: 1861
	public const int LOGI_MODEL_PRECISION_GAMEPAD_2 = 24;

	// Token: 0x04000746 RID: 1862
	public const int LOGI_MODEL_CHILLSTREAM = 25;

	// Token: 0x04000747 RID: 1863
	public const int LOGI_NUMBER_MODELS = 26;

	// Token: 0x020000F8 RID: 248
	// (Invoke) Token: 0x06000596 RID: 1430
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void logiArxCB(int eventType, int eventValue, [MarshalAs(UnmanagedType.LPWStr)] string eventArg, IntPtr context);

	// Token: 0x020000F9 RID: 249
	public struct logiArxCbContext
	{
		// Token: 0x04000748 RID: 1864
		public LogitechGSDK.logiArxCB arxCallBack;

		// Token: 0x04000749 RID: 1865
		public IntPtr arxContext;
	}

	// Token: 0x020000FA RID: 250
	public enum keyboardNames
	{
		// Token: 0x0400074B RID: 1867
		ESC = 1,
		// Token: 0x0400074C RID: 1868
		F1 = 59,
		// Token: 0x0400074D RID: 1869
		F2,
		// Token: 0x0400074E RID: 1870
		F3,
		// Token: 0x0400074F RID: 1871
		F4,
		// Token: 0x04000750 RID: 1872
		F5,
		// Token: 0x04000751 RID: 1873
		F6,
		// Token: 0x04000752 RID: 1874
		F7,
		// Token: 0x04000753 RID: 1875
		F8,
		// Token: 0x04000754 RID: 1876
		F9,
		// Token: 0x04000755 RID: 1877
		F10,
		// Token: 0x04000756 RID: 1878
		F11 = 87,
		// Token: 0x04000757 RID: 1879
		F12,
		// Token: 0x04000758 RID: 1880
		PRINT_SCREEN = 311,
		// Token: 0x04000759 RID: 1881
		SCROLL_LOCK = 70,
		// Token: 0x0400075A RID: 1882
		PAUSE_BREAK = 69,
		// Token: 0x0400075B RID: 1883
		TILDE = 41,
		// Token: 0x0400075C RID: 1884
		ONE = 2,
		// Token: 0x0400075D RID: 1885
		TWO,
		// Token: 0x0400075E RID: 1886
		THREE,
		// Token: 0x0400075F RID: 1887
		FOUR,
		// Token: 0x04000760 RID: 1888
		FIVE,
		// Token: 0x04000761 RID: 1889
		SIX,
		// Token: 0x04000762 RID: 1890
		SEVEN,
		// Token: 0x04000763 RID: 1891
		EIGHT,
		// Token: 0x04000764 RID: 1892
		NINE,
		// Token: 0x04000765 RID: 1893
		ZERO,
		// Token: 0x04000766 RID: 1894
		MINUS,
		// Token: 0x04000767 RID: 1895
		EQUALS,
		// Token: 0x04000768 RID: 1896
		BACKSPACE,
		// Token: 0x04000769 RID: 1897
		INSERT = 338,
		// Token: 0x0400076A RID: 1898
		HOME = 327,
		// Token: 0x0400076B RID: 1899
		PAGE_UP = 329,
		// Token: 0x0400076C RID: 1900
		NUM_LOCK = 325,
		// Token: 0x0400076D RID: 1901
		NUM_SLASH = 309,
		// Token: 0x0400076E RID: 1902
		NUM_ASTERISK = 55,
		// Token: 0x0400076F RID: 1903
		NUM_MINUS = 74,
		// Token: 0x04000770 RID: 1904
		TAB = 15,
		// Token: 0x04000771 RID: 1905
		Q,
		// Token: 0x04000772 RID: 1906
		W,
		// Token: 0x04000773 RID: 1907
		E,
		// Token: 0x04000774 RID: 1908
		R,
		// Token: 0x04000775 RID: 1909
		T,
		// Token: 0x04000776 RID: 1910
		Y,
		// Token: 0x04000777 RID: 1911
		U,
		// Token: 0x04000778 RID: 1912
		I,
		// Token: 0x04000779 RID: 1913
		O,
		// Token: 0x0400077A RID: 1914
		P,
		// Token: 0x0400077B RID: 1915
		OPEN_BRACKET,
		// Token: 0x0400077C RID: 1916
		CLOSE_BRACKET,
		// Token: 0x0400077D RID: 1917
		BACKSLASH = 43,
		// Token: 0x0400077E RID: 1918
		KEYBOARD_DELETE = 339,
		// Token: 0x0400077F RID: 1919
		END = 335,
		// Token: 0x04000780 RID: 1920
		PAGE_DOWN = 337,
		// Token: 0x04000781 RID: 1921
		NUM_SEVEN = 71,
		// Token: 0x04000782 RID: 1922
		NUM_EIGHT,
		// Token: 0x04000783 RID: 1923
		NUM_NINE,
		// Token: 0x04000784 RID: 1924
		NUM_PLUS = 78,
		// Token: 0x04000785 RID: 1925
		CAPS_LOCK = 58,
		// Token: 0x04000786 RID: 1926
		A = 30,
		// Token: 0x04000787 RID: 1927
		S,
		// Token: 0x04000788 RID: 1928
		D,
		// Token: 0x04000789 RID: 1929
		F,
		// Token: 0x0400078A RID: 1930
		G,
		// Token: 0x0400078B RID: 1931
		H,
		// Token: 0x0400078C RID: 1932
		J,
		// Token: 0x0400078D RID: 1933
		K,
		// Token: 0x0400078E RID: 1934
		L,
		// Token: 0x0400078F RID: 1935
		SEMICOLON,
		// Token: 0x04000790 RID: 1936
		APOSTROPHE,
		// Token: 0x04000791 RID: 1937
		ENTER = 28,
		// Token: 0x04000792 RID: 1938
		NUM_FOUR = 75,
		// Token: 0x04000793 RID: 1939
		NUM_FIVE,
		// Token: 0x04000794 RID: 1940
		NUM_SIX,
		// Token: 0x04000795 RID: 1941
		LEFT_SHIFT = 42,
		// Token: 0x04000796 RID: 1942
		Z = 44,
		// Token: 0x04000797 RID: 1943
		X,
		// Token: 0x04000798 RID: 1944
		C,
		// Token: 0x04000799 RID: 1945
		V,
		// Token: 0x0400079A RID: 1946
		B,
		// Token: 0x0400079B RID: 1947
		N,
		// Token: 0x0400079C RID: 1948
		M,
		// Token: 0x0400079D RID: 1949
		COMMA,
		// Token: 0x0400079E RID: 1950
		PERIOD,
		// Token: 0x0400079F RID: 1951
		FORWARD_SLASH,
		// Token: 0x040007A0 RID: 1952
		RIGHT_SHIFT,
		// Token: 0x040007A1 RID: 1953
		ARROW_UP = 328,
		// Token: 0x040007A2 RID: 1954
		NUM_ONE = 79,
		// Token: 0x040007A3 RID: 1955
		NUM_TWO,
		// Token: 0x040007A4 RID: 1956
		NUM_THREE,
		// Token: 0x040007A5 RID: 1957
		NUM_ENTER = 284,
		// Token: 0x040007A6 RID: 1958
		LEFT_CONTROL = 29,
		// Token: 0x040007A7 RID: 1959
		LEFT_WINDOWS = 347,
		// Token: 0x040007A8 RID: 1960
		LEFT_ALT = 56,
		// Token: 0x040007A9 RID: 1961
		SPACE,
		// Token: 0x040007AA RID: 1962
		RIGHT_ALT = 312,
		// Token: 0x040007AB RID: 1963
		RIGHT_WINDOWS = 348,
		// Token: 0x040007AC RID: 1964
		APPLICATION_SELECT,
		// Token: 0x040007AD RID: 1965
		RIGHT_CONTROL = 285,
		// Token: 0x040007AE RID: 1966
		ARROW_LEFT = 331,
		// Token: 0x040007AF RID: 1967
		ARROW_DOWN = 336,
		// Token: 0x040007B0 RID: 1968
		ARROW_RIGHT = 333,
		// Token: 0x040007B1 RID: 1969
		NUM_ZERO = 82,
		// Token: 0x040007B2 RID: 1970
		NUM_PERIOD
	}

	// Token: 0x020000FB RID: 251
	public enum DeviceType
	{
		// Token: 0x040007B4 RID: 1972
		Keyboard,
		// Token: 0x040007B5 RID: 1973
		Mouse = 3,
		// Token: 0x040007B6 RID: 1974
		Mousemat,
		// Token: 0x040007B7 RID: 1975
		Headset = 8,
		// Token: 0x040007B8 RID: 1976
		Speaker = 14
	}

	// Token: 0x020000FC RID: 252
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct GkeyCode
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x0002A64C File Offset: 0x0002884C
		public int keyIdx
		{
			get
			{
				return (int)(this.complete & 255);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x0002A65A File Offset: 0x0002885A
		public int keyDown
		{
			get
			{
				return this.complete >> 8 & 1;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x0002A666 File Offset: 0x00028866
		public int mState
		{
			get
			{
				return this.complete >> 9 & 3;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x0002A673 File Offset: 0x00028873
		public int mouse
		{
			get
			{
				return this.complete >> 11 & 15;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x0002A681 File Offset: 0x00028881
		public int reserved1
		{
			get
			{
				return this.complete >> 15 & 1;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x0002A68E File Offset: 0x0002888E
		public int reserved2
		{
			get
			{
				return this.complete >> 16 & 131071;
			}
		}

		// Token: 0x040007B9 RID: 1977
		public ushort complete;
	}

	// Token: 0x020000FD RID: 253
	// (Invoke) Token: 0x060005A0 RID: 1440
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void logiGkeyCB(LogitechGSDK.GkeyCode gkeyCode, [MarshalAs(UnmanagedType.LPWStr)] string gkeyOrButtonString, IntPtr context);

	// Token: 0x020000FE RID: 254
	public struct logiGKeyCbContext
	{
		// Token: 0x040007BA RID: 1978
		public LogitechGSDK.logiGkeyCB gkeyCallBack;

		// Token: 0x040007BB RID: 1979
		public IntPtr gkeyContext;
	}

	// Token: 0x020000FF RID: 255
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct LogiControllerPropertiesData
	{
		// Token: 0x040007BC RID: 1980
		public bool forceEnable;

		// Token: 0x040007BD RID: 1981
		public int overallGain;

		// Token: 0x040007BE RID: 1982
		public int springGain;

		// Token: 0x040007BF RID: 1983
		public int damperGain;

		// Token: 0x040007C0 RID: 1984
		public bool defaultSpringEnabled;

		// Token: 0x040007C1 RID: 1985
		public int defaultSpringGain;

		// Token: 0x040007C2 RID: 1986
		public bool combinePedals;

		// Token: 0x040007C3 RID: 1987
		public int wheelRange;

		// Token: 0x040007C4 RID: 1988
		public bool gameSettingsEnabled;

		// Token: 0x040007C5 RID: 1989
		public bool allowGameSettings;
	}

	// Token: 0x02000100 RID: 256
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct DIJOYSTATE2ENGINES
	{
		// Token: 0x040007C6 RID: 1990
		public int lX;

		// Token: 0x040007C7 RID: 1991
		public int lY;

		// Token: 0x040007C8 RID: 1992
		public int lZ;

		// Token: 0x040007C9 RID: 1993
		public int lRx;

		// Token: 0x040007CA RID: 1994
		public int lRy;

		// Token: 0x040007CB RID: 1995
		public int lRz;

		// Token: 0x040007CC RID: 1996
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] rglSlider;

		// Token: 0x040007CD RID: 1997
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public uint[] rgdwPOV;

		// Token: 0x040007CE RID: 1998
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public byte[] rgbButtons;

		// Token: 0x040007CF RID: 1999
		public int lVX;

		// Token: 0x040007D0 RID: 2000
		public int lVY;

		// Token: 0x040007D1 RID: 2001
		public int lVZ;

		// Token: 0x040007D2 RID: 2002
		public int lVRx;

		// Token: 0x040007D3 RID: 2003
		public int lVRy;

		// Token: 0x040007D4 RID: 2004
		public int lVRz;

		// Token: 0x040007D5 RID: 2005
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] rglVSlider;

		// Token: 0x040007D6 RID: 2006
		public int lAX;

		// Token: 0x040007D7 RID: 2007
		public int lAY;

		// Token: 0x040007D8 RID: 2008
		public int lAZ;

		// Token: 0x040007D9 RID: 2009
		public int lARx;

		// Token: 0x040007DA RID: 2010
		public int lARy;

		// Token: 0x040007DB RID: 2011
		public int lARz;

		// Token: 0x040007DC RID: 2012
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] rglASlider;

		// Token: 0x040007DD RID: 2013
		public int lFX;

		// Token: 0x040007DE RID: 2014
		public int lFY;

		// Token: 0x040007DF RID: 2015
		public int lFZ;

		// Token: 0x040007E0 RID: 2016
		public int lFRx;

		// Token: 0x040007E1 RID: 2017
		public int lFRy;

		// Token: 0x040007E2 RID: 2018
		public int lFRz;

		// Token: 0x040007E3 RID: 2019
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public int[] rglFSlider;
	}
}
