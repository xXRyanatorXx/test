using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class DEBUGGG : MonoBehaviour
{
	// Token: 0x060000EB RID: 235 RVA: 0x00009474 File Offset: 0x00007674
	private void OnEnable()
	{
		Application.logMessageReceived += this.Log;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00009487 File Offset: 0x00007687
	private void OnDisable()
	{
		Application.logMessageReceived -= this.Log;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x0000949C File Offset: 0x0000769C
	public void Log(string logString, string stackTrace, LogType type)
	{
		this.output = logString;
		this.stack = stackTrace;
		DEBUGGG.myLog = this.output + "\n" + DEBUGGG.myLog;
		if (DEBUGGG.myLog.Length > 5000)
		{
			DEBUGGG.myLog = DEBUGGG.myLog.Substring(0, 4000);
		}
	}

	// Token: 0x060000EE RID: 238 RVA: 0x000094F7 File Offset: 0x000076F7
	private void OnGUI()
	{
		DEBUGGG.myLog = GUI.TextArea(new Rect(10f, 10f, (float)(Screen.width - 10), (float)(Screen.height - 10)), DEBUGGG.myLog);
	}

	// Token: 0x04000161 RID: 353
	private static string myLog = "";

	// Token: 0x04000162 RID: 354
	private string output;

	// Token: 0x04000163 RID: 355
	private string stack;
}
