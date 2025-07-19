using System;
using System.Collections.Generic;
using System.IO;
using PaintIn3D;
using UnityEngine;

// Token: 0x0200027B RID: 635
public class TextureList : MonoBehaviour
{
	// Token: 0x06000F05 RID: 3845 RVA: 0x0009DB70 File Offset: 0x0009BD70
	private void Awake()
	{
		if (!Directory.Exists(Application.persistentDataPath + "/Decals"))
		{
			Directory.CreateDirectory(Application.persistentDataPath + "/Decals");
		}
		this.decls.AddRange(this.Decals);
		string[] files = Directory.GetFiles(Application.persistentDataPath + "/Decals");
		for (int i = 0; i < files.Length; i++)
		{
			byte[] data = File.ReadAllBytes(files[i]);
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(data);
			this.decls.Add(texture2D);
		}
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x0000245B File Offset: 0x0000065B
	private void OnEnable()
	{
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0009DC04 File Offset: 0x0009BE04
	public void Update()
	{
		if (tools.tool == 19)
		{
			if (Input.GetMouseButtonDown(2))
			{
				this.ThisDecal.Texture = this.decls[UnityEngine.Random.Range(0, this.decls.Count)];
				return;
			}
		}
		else
		{
			base.enabled = false;
		}
	}

	// Token: 0x04001838 RID: 6200
	public P3dPaintDecal ThisDecal;

	// Token: 0x04001839 RID: 6201
	public Texture[] Decals;

	// Token: 0x0400183A RID: 6202
	public List<Texture> decls;
}
