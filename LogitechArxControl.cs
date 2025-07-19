using System;
using System.IO;
using UnityEngine;

// Token: 0x02000101 RID: 257
public class LogitechArxControl : MonoBehaviour
{
	// Token: 0x060005A3 RID: 1443 RVA: 0x0002A6A0 File Offset: 0x000288A0
	private void Start()
	{
		LogitechGSDK.logiArxCB arxCallBack = new LogitechGSDK.logiArxCB(this.ArxSDKCallback);
		LogitechGSDK.logiArxCbContext logiArxCbContext;
		logiArxCbContext.arxCallBack = arxCallBack;
		logiArxCbContext.arxContext = IntPtr.Zero;
		LogitechGSDK.LogiArxInit("com.logitech.unitysample", "Unity Sample", ref logiArxCbContext);
		this.descriptionLabel = "Click the left mouse button to update the progress bar, Press G to switch to a different index file, press I to go back to the original one.";
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x0002A6EB File Offset: 0x000288EB
	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 350f, 500f, 50f), this.descriptionLabel);
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x0002A714 File Offset: 0x00028914
	private void Update()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			LogitechGSDK.LogiArxSetTagPropertyById("progressbarProgress", "style.width", new System.Random().Next(0, 100).ToString() + "%");
		}
		if (Input.GetKey(KeyCode.I))
		{
			LogitechGSDK.LogiArxSetIndex("applet.html");
		}
		if (Input.GetKey(KeyCode.G))
		{
			LogitechGSDK.LogiArxSetIndex("gameover.html");
		}
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x0002A784 File Offset: 0x00028984
	public static string getHtmlString()
	{
		return "" + "<html><center><body bgcolor='black'><a href='applet.html'><img src='gameover.png'/></a></body></center></html>";
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x0002A798 File Offset: 0x00028998
	private void ArxSDKCallback(int eventType, int eventValue, string eventArg, IntPtr context)
	{
		Debug.Log(string.Concat(new string[]
		{
			"CALLBACK: type:",
			eventType.ToString(),
			", value:",
			eventValue.ToString(),
			", arg:",
			eventArg
		}));
		if (eventType == 8)
		{
			if (!LogitechGSDK.LogiArxAddFileAs("Assets//Logitech SDK//AppletData//applet.html", "applet.html", ""))
			{
				Debug.Log("Could not send applet.html : " + LogitechGSDK.LogiArxGetLastError().ToString());
			}
			if (!LogitechGSDK.LogiArxAddFileAs("Assets//Logitech SDK//AppletData//background.png", "background.png", ""))
			{
				Debug.Log("Could not send background.png : " + LogitechGSDK.LogiArxGetLastError().ToString());
			}
			if (!LogitechGSDK.LogiArxAddUTF8StringAs(LogitechArxControl.getHtmlString(), "gameover.html", ""))
			{
				Debug.Log("Could not send gameover.html  : " + LogitechGSDK.LogiArxGetLastError().ToString());
			}
			byte[] array = File.ReadAllBytes("Assets//Logitech SDK//AppletData//gameover.png");
			if (!LogitechGSDK.LogiArxAddContentAs(array, array.Length, "gameover.png", ""))
			{
				Debug.Log("Could not send gameover.png  : " + LogitechGSDK.LogiArxGetLastError().ToString());
			}
			if (!LogitechGSDK.LogiArxSetIndex("applet.html"))
			{
				Debug.Log("Could not set index : " + LogitechGSDK.LogiArxGetLastError().ToString());
				return;
			}
		}
		else
		{
			if (eventType == 16)
			{
				Debug.Log("NO DEVICES");
				return;
			}
			if (eventType == 4)
			{
				Debug.Log("Tap on tag with id :" + eventArg);
			}
		}
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x0002A909 File Offset: 0x00028B09
	private void OnDestroy()
	{
		LogitechGSDK.LogiArxShutdown();
	}

	// Token: 0x040007E4 RID: 2020
	private string descriptionLabel;
}
