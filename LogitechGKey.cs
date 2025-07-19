using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class LogitechGKey : MonoBehaviour
{
	// Token: 0x060005AA RID: 1450 RVA: 0x0002A910 File Offset: 0x00028B10
	private void Start()
	{
		this.descriptionLabel = "Last g-key event : ";
		LogitechGKey.lastKeyPress = "No g-key event";
		if (this.usingCallback)
		{
			LogitechGSDK.logiGkeyCB gkeyCallBack = new LogitechGSDK.logiGkeyCB(this.GkeySDKCallback);
			LogitechGSDK.logiGKeyCbContext logiGKeyCbContext;
			logiGKeyCbContext.gkeyCallBack = gkeyCallBack;
			logiGKeyCbContext.gkeyContext = IntPtr.Zero;
			LogitechGSDK.LogiGkeyInit(ref logiGKeyCbContext);
			return;
		}
		LogitechGSDK.LogiGkeyInitWithoutCallback();
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0002A96C File Offset: 0x00028B6C
	private void Update()
	{
		if (!this.usingCallback)
		{
			for (int i = 6; i <= 20; i++)
			{
				if (LogitechGSDK.LogiGkeyIsMouseButtonPressed(i) == 1)
				{
					LogitechGKey.lastKeyPress = "MOUSE DOWN Button : " + i.ToString();
				}
			}
			for (int j = 1; j <= 29; j++)
			{
				for (int k = 1; k <= 3; k++)
				{
					if (LogitechGSDK.LogiGkeyIsKeyboardGkeyPressed(j, k) == 1)
					{
						LogitechGKey.lastKeyPress = "KEYBOARD/HEADSET DOWN Button : " + j.ToString();
					}
				}
			}
		}
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x0002A9E8 File Offset: 0x00028BE8
	private void GkeySDKCallback(LogitechGSDK.GkeyCode gKeyCode, string gKeyOrButtonString, IntPtr context)
	{
		if (gKeyCode.keyDown == 0)
		{
			if (gKeyCode.mouse == 1)
			{
				LogitechGKey.lastKeyPress = "MOUSE UP" + gKeyOrButtonString;
				return;
			}
			LogitechGKey.lastKeyPress = "KEYBOARD/HEADSET RELEASED " + gKeyOrButtonString;
			return;
		}
		else
		{
			if (gKeyCode.mouse == 1)
			{
				LogitechGKey.lastKeyPress = "MOUSE DOWN " + gKeyOrButtonString;
				return;
			}
			LogitechGKey.lastKeyPress = "KEYBOARD/HEADSET PRESSED " + gKeyOrButtonString;
			return;
		}
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0002AA55 File Offset: 0x00028C55
	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 450f, 200f, 50f), this.descriptionLabel + LogitechGKey.lastKeyPress);
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x0002AA85 File Offset: 0x00028C85
	private void OnDestroy()
	{
		LogitechGSDK.LogiGkeyShutdown();
	}

	// Token: 0x040007E5 RID: 2021
	public bool usingCallback;

	// Token: 0x040007E6 RID: 2022
	private static string lastKeyPress = "";

	// Token: 0x040007E7 RID: 2023
	private string descriptionLabel = "";
}
