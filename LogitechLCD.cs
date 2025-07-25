﻿using System;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class LogitechLCD : MonoBehaviour
{
	// Token: 0x060005B1 RID: 1457 RVA: 0x0002AAAC File Offset: 0x00028CAC
	private void Start()
	{
		LogitechGSDK.LogiLcdInit("UNITY_TEST", 3);
		LogitechGSDK.LogiLcdColorSetTitle("Testing", 255, 0, 0);
		LogitechGSDK.LogiLcdColorSetText(0, "zero", 255, 255, 0);
		LogitechGSDK.LogiLcdColorSetText(1, "first", 0, 255, 0);
		LogitechGSDK.LogiLcdColorSetText(2, "second", 0, 255, 30);
		LogitechGSDK.LogiLcdColorSetText(3, "third", 0, 255, 50);
		LogitechGSDK.LogiLcdColorSetText(4, "fourth", 0, 255, 90);
		LogitechGSDK.LogiLcdColorSetText(5, "fifth", 0, 255, 140);
		LogitechGSDK.LogiLcdColorSetText(6, "sixth", 0, 255, 200);
		LogitechGSDK.LogiLcdColorSetText(7, "seventh", 0, 255, 255);
		LogitechGSDK.LogiLcdColorSetText(8, "eight", 0, 255, 255);
		LogitechGSDK.LogiLcdMonoSetText(0, "testing");
		LogitechGSDK.LogiLcdMonoSetText(1, "mono");
		LogitechGSDK.LogiLcdMonoSetText(2, "chrome");
		LogitechGSDK.LogiLcdMonoSetText(3, "lcd");
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x0002ABCC File Offset: 0x00028DCC
	private void Update()
	{
		string text = "";
		string text2 = "";
		if (LogitechGSDK.LogiLcdIsButtonPressed(2048))
		{
			text += "Cancel";
		}
		if (LogitechGSDK.LogiLcdIsButtonPressed(8192))
		{
			text += "Down";
		}
		if (LogitechGSDK.LogiLcdIsButtonPressed(256))
		{
			text += "Left";
		}
		if (LogitechGSDK.LogiLcdIsButtonPressed(16384))
		{
			text += "Menu";
		}
		if (LogitechGSDK.LogiLcdIsButtonPressed(1024))
		{
			text += "Ok";
		}
		if (LogitechGSDK.LogiLcdIsButtonPressed(512))
		{
			text += "Right";
		}
		if (LogitechGSDK.LogiLcdIsButtonPressed(4096))
		{
			text += "Up";
		}
		if (LogitechGSDK.LogiLcdIsButtonPressed(1))
		{
			text2 += "Button 0";
		}
		if (LogitechGSDK.LogiLcdIsButtonPressed(2))
		{
			text2 += "Button 1";
		}
		if (LogitechGSDK.LogiLcdIsButtonPressed(4))
		{
			text2 += "Button 2";
		}
		if (LogitechGSDK.LogiLcdIsButtonPressed(8))
		{
			text2 += "Button 3";
		}
		LogitechGSDK.LogiLcdMonoSetText(0, text2);
		LogitechGSDK.LogiLcdColorSetText(5, text, 255, 255, 0);
		string text3 = "LCDs connected :";
		if (LogitechGSDK.LogiLcdIsConnected(1))
		{
			text3 += "MONO ";
		}
		if (LogitechGSDK.LogiLcdIsConnected(2))
		{
			text3 += "COLOR";
		}
		LogitechGSDK.LogiLcdMonoSetText(1, text3);
		LogitechGSDK.LogiLcdColorSetText(2, text3, 255, 255, 0);
		LogitechGSDK.LogiLcdUpdate();
		if (Input.GetKey(KeyCode.Mouse0))
		{
			this.pixelMatrix = new byte[307200];
			System.Random random = new System.Random();
			int num = random.Next(0, 255);
			int num2 = random.Next(0, 255);
			int num3 = random.Next(0, 255);
			int num4 = random.Next(0, 255);
			for (int i = 0; i < 307200; i++)
			{
				if (i % 1 == 0)
				{
					this.pixelMatrix[i] = (byte)num2;
				}
				if (i % 2 == 0)
				{
					this.pixelMatrix[i] = (byte)num3;
				}
				if (i % 3 == 0)
				{
					this.pixelMatrix[i] = (byte)num;
				}
				if (i % 4 == 0)
				{
					this.pixelMatrix[i] = (byte)num4;
				}
			}
			LogitechGSDK.LogiLcdColorSetBackground(this.pixelMatrix);
			LogitechGSDK.LogiLcdColorSetText(6, string.Concat(new string[]
			{
				"color : ",
				num.ToString(),
				" - ",
				num2.ToString(),
				" - ",
				num3.ToString(),
				" - ",
				num4.ToString()
			}), 255, 0, 0);
		}
		if (Input.GetKey(KeyCode.Mouse1))
		{
			this.pixelMatrix = new byte[6880];
			for (int j = 0; j < 6880; j++)
			{
				int num5 = new System.Random().Next(0, 255);
				this.pixelMatrix[j] = (byte)num5;
			}
			LogitechGSDK.LogiLcdMonoSetBackground(this.pixelMatrix);
		}
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x0002AECD File Offset: 0x000290CD
	private void OnDestroy()
	{
		LogitechGSDK.LogiLcdShutdown();
	}

	// Token: 0x040007E8 RID: 2024
	private byte[] pixelMatrix;
}
