using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class LoadTexture : MonoBehaviour
{
	// Token: 0x0600052D RID: 1325 RVA: 0x0002A497 File Offset: 0x00028697
	private void Start()
	{
		this.m_Path = Application.dataPath;
		base.StartCoroutine("load_image");
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0002A4B0 File Offset: 0x000286B0
	private IEnumerator load_image()
	{
		string filePaths = Application.persistentDataPath + "/shopsign.png";
		WWW www = new WWW("file://" + filePaths);
		yield return www;
		Texture2D texture2D = new Texture2D(512, 512);
		www.LoadImageIntoTexture(texture2D);
		if (File.Exists(filePaths))
		{
			base.GetComponent<Renderer>().material.mainTexture = texture2D;
		}
		yield break;
	}

	// Token: 0x040006DE RID: 1758
	private string m_Path;
}
