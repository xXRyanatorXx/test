using System;
using UnityEngine;

// Token: 0x020000E8 RID: 232
public class FPSDisplay : MonoBehaviour
{
	// Token: 0x06000503 RID: 1283 RVA: 0x00029665 File Offset: 0x00027865
	private void Update()
	{
		this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00029688 File Offset: 0x00027888
	private void OnGUI()
	{
		int width = Screen.width;
		int height = Screen.height;
		GUIStyle guistyle = new GUIStyle();
		Rect position = new Rect(0f, 0f, (float)width, (float)(height * 2 / 100));
		guistyle.alignment = TextAnchor.UpperLeft;
		guistyle.fontSize = height * 2 / 100;
		guistyle.normal.textColor = new Color(1f, 1f, 1f, 1f);
		float num = this.deltaTime * 1000f;
		float num2 = 1f / this.deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", num, num2);
		GUI.Label(position, text, guistyle);
	}

	// Token: 0x040006B9 RID: 1721
	private float deltaTime;
}
