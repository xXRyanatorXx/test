using System;
using UnityEngine;

// Token: 0x0200021A RID: 538
[ExecuteInEditMode]
public class TextureGenerator : MonoBehaviour
{
	// Token: 0x06000C8F RID: 3215 RVA: 0x0008C671 File Offset: 0x0008A871
	private void Start()
	{
		RuntimePreviewGenerator.BackgroundColor = new Color(0f, 0f, 0f, 0f);
		this.result = RuntimePreviewGenerator.GenerateModelPreview(this.target, this.thumbnailSize, this.thumbnailSize, false);
	}

	// Token: 0x04001572 RID: 5490
	public Transform target;

	// Token: 0x04001573 RID: 5491
	public Texture2D result;

	// Token: 0x04001574 RID: 5492
	public int thumbnailSize = 128;
}
