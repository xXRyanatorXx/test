using System;
using UnityEngine;

// Token: 0x0200027C RID: 636
public class Textureformrender : MonoBehaviour
{
	// Token: 0x06000F09 RID: 3849 RVA: 0x0009DC51 File Offset: 0x0009BE51
	private void start()
	{
		this.toTexture2D(this.tex);
		this.cam.SetActive(false);
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x0009DC6C File Offset: 0x0009BE6C
	private Texture2D toTexture2D(RenderTexture rTex)
	{
		Texture2D texture2D = new Texture2D(512, 512, TextureFormat.RGB24, false);
		RenderTexture.active = rTex;
		texture2D.ReadPixels(new Rect(0f, 0f, (float)rTex.width, (float)rTex.height), 0, 0);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x0400183B RID: 6203
	public GameObject cam;

	// Token: 0x0400183C RID: 6204
	public RenderTexture tex;
}
