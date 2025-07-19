using System;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class FCP_ExampleScript : MonoBehaviour
{
	// Token: 0x06000497 RID: 1175 RVA: 0x00027323 File Offset: 0x00025523
	private void Start()
	{
		this.internalColor = this.externalColor;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00027334 File Offset: 0x00025534
	private void Update()
	{
		if (this.internalColor != this.externalColor)
		{
			this.fcp.color = this.externalColor;
			this.internalColor = this.externalColor;
		}
		this.material.color = this.fcp.color;
	}

	// Token: 0x04000655 RID: 1621
	public FlexibleColorPicker fcp;

	// Token: 0x04000656 RID: 1622
	public Material material;

	// Token: 0x04000657 RID: 1623
	public Color externalColor;

	// Token: 0x04000658 RID: 1624
	private Color internalColor;
}
