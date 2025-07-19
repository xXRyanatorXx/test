using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
[Serializable]
public class EnviroFogging
{
	// Token: 0x04000283 RID: 643
	[HideInInspector]
	public float skyFogStart;

	// Token: 0x04000284 RID: 644
	[HideInInspector]
	public float skyFogHeight = 1f;

	// Token: 0x04000285 RID: 645
	[HideInInspector]
	public float skyFogIntensity = 0.1f;

	// Token: 0x04000286 RID: 646
	[HideInInspector]
	public float skyFogStrength = 0.1f;

	// Token: 0x04000287 RID: 647
	[HideInInspector]
	public float scatteringStrenght = 0.5f;

	// Token: 0x04000288 RID: 648
	[HideInInspector]
	public float sunBlocking = 0.5f;

	// Token: 0x04000289 RID: 649
	[HideInInspector]
	public float moonIntensity = 1f;

	// Token: 0x0400028A RID: 650
	[HideInInspector]
	public float hdrpRelativeFogHeight;
}
