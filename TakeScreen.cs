using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x02000218 RID: 536
public class TakeScreen : MonoBehaviour
{
	// Token: 0x06000C86 RID: 3206 RVA: 0x0008C4D8 File Offset: 0x0008A6D8
	private void Update()
	{
		if (Input.GetKeyDown("e"))
		{
			Debug.Log("Saving Screen Shot");
			base.StartCoroutine(this.CreateLayerThumbnail());
		}
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x0008C4FD File Offset: 0x0008A6FD
	private IEnumerator CreateLayerThumbnail()
	{
		yield return new WaitForEndOfFrame();
		Texture2D texture = new Texture2D(this.cam.targetTexture.width, this.cam.targetTexture.height, TextureFormat.RGB24, false);
		this.cam.Render();
		RenderTexture.active = this.cam.targetTexture;
		texture.ReadPixels(new Rect(0f, 0f, (float)this.cam.targetTexture.width, (float)this.cam.targetTexture.height), 0, 0);
		yield return 0;
		texture.Apply();
		yield return 0;
		byte[] bytes = texture.EncodeToPNG();
		string str = "amazingPath.png";
		File.WriteAllBytes("C:/Unity/" + str, bytes);
		UnityEngine.Object.DestroyObject(texture);
		yield break;
	}

	// Token: 0x0400156D RID: 5485
	public Camera cam;
}
