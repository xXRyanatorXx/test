using System;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class LogitechLed : MonoBehaviour
{
	// Token: 0x060005B5 RID: 1461 RVA: 0x0002AED4 File Offset: 0x000290D4
	private void Start()
	{
		this.blue = 0;
		this.red = 0;
		this.green = 0;
		LogitechGSDK.LogiLedInit();
		LogitechGSDK.LogiLedSaveCurrentLighting();
		this.effectLabel = "Press F to test flashing effect, P to test pulsing effect \n Press mouse1 to set all lighting to random color, mouse 2 to set G910 to random bitmap \nPress E to start per-key effects (F1-F12) show on supported devices \nPress S to stop the effects \n";
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x0002AF02 File Offset: 0x00029102
	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 250f, 500f, 200f), this.effectLabel);
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x0002AF28 File Offset: 0x00029128
	private void Update()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			System.Random random = new System.Random();
			this.red = random.Next(0, 100);
			this.blue = random.Next(0, 100);
			this.green = random.Next(0, 100);
			LogitechGSDK.LogiLedSetLighting(this.red, this.blue, this.green);
		}
		if (Input.GetKey(KeyCode.Mouse1))
		{
			byte[] array = new byte[504];
			System.Random random2 = new System.Random();
			for (int i = 0; i < 504; i++)
			{
				array[i] = (byte)random2.Next(0, 255);
			}
			LogitechGSDK.LogiLedSetLightingFromBitmap(array);
			this.red = random2.Next(0, 100);
			this.blue = random2.Next(0, 100);
			this.green = random2.Next(0, 100);
			LogitechGSDK.LogiLedSetLightingForTargetZone(LogitechGSDK.DeviceType.Speaker, 0, this.red, this.blue, this.green);
		}
		if (Input.GetKey(KeyCode.F))
		{
			System.Random random3 = new System.Random();
			this.red = random3.Next(0, 100);
			this.blue = random3.Next(0, 100);
			this.green = random3.Next(0, 100);
			LogitechGSDK.LogiLedFlashLighting(this.red, this.blue, this.green, 0, 200);
		}
		if (Input.GetKey(KeyCode.P))
		{
			System.Random random4 = new System.Random();
			this.red = random4.Next(0, 100);
			this.blue = random4.Next(0, 100);
			this.green = random4.Next(0, 100);
			LogitechGSDK.LogiLedPulseLighting(this.red, this.blue, this.green, 0, 100);
		}
		if (Input.GetKey(KeyCode.E))
		{
			System.Random random5 = new System.Random();
			this.red = random5.Next(0, 100);
			this.blue = random5.Next(0, 100);
			this.green = random5.Next(0, 100);
			int startRedPercentage = random5.Next(0, 100);
			int startBluePercentage = random5.Next(0, 100);
			int startGreenPercentage = random5.Next(0, 100);
			LogitechGSDK.LogiLedPulseSingleKey(LogitechGSDK.keyboardNames.F1, startRedPercentage, startGreenPercentage, startBluePercentage, this.red, this.green, this.blue, 100, true);
			LogitechGSDK.LogiLedPulseSingleKey(LogitechGSDK.keyboardNames.F2, startRedPercentage, startGreenPercentage, startBluePercentage, this.red, this.green, this.blue, 100, true);
			LogitechGSDK.LogiLedPulseSingleKey(LogitechGSDK.keyboardNames.F3, startRedPercentage, startGreenPercentage, startBluePercentage, this.red, this.green, this.blue, 100, true);
			LogitechGSDK.LogiLedPulseSingleKey(LogitechGSDK.keyboardNames.F4, startRedPercentage, startGreenPercentage, startBluePercentage, this.red, this.green, this.blue, 100, true);
			this.red = random5.Next(0, 100);
			this.blue = random5.Next(0, 100);
			this.green = random5.Next(0, 100);
			int msInterval = random5.Next(50, 200);
			LogitechGSDK.LogiLedFlashSingleKey(LogitechGSDK.keyboardNames.F5, this.red, this.green, this.blue, 0, msInterval);
			LogitechGSDK.LogiLedFlashSingleKey(LogitechGSDK.keyboardNames.F6, this.red, this.green, this.blue, 0, msInterval);
			LogitechGSDK.LogiLedFlashSingleKey(LogitechGSDK.keyboardNames.F7, this.red, this.green, this.blue, 0, msInterval);
			LogitechGSDK.LogiLedFlashSingleKey(LogitechGSDK.keyboardNames.F8, this.red, this.green, this.blue, 0, msInterval);
		}
		if (Input.GetKey(KeyCode.S))
		{
			LogitechGSDK.LogiLedStopEffects();
		}
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x0002B28A File Offset: 0x0002948A
	private void OnDestroy()
	{
		LogitechGSDK.LogiLedRestoreLighting();
		LogitechGSDK.LogiLedShutdown();
	}

	// Token: 0x040007E9 RID: 2025
	private int red;

	// Token: 0x040007EA RID: 2026
	private int blue;

	// Token: 0x040007EB RID: 2027
	private int green;

	// Token: 0x040007EC RID: 2028
	public string effectLabel;
}
