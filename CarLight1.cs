using System;
using UnityEngine;

// Token: 0x02000160 RID: 352
public class CarLight1 : MonoBehaviour
{
	// Token: 0x060007B9 RID: 1977 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x0000245B File Offset: 0x0000065B
	private void Update()
	{
	}

	// Token: 0x04000CDB RID: 3291
	public CarProperties LightBulb;

	// Token: 0x04000CDC RID: 3292
	public bool BrakeLight;

	// Token: 0x04000CDD RID: 3293
	public bool ReverseLight;

	// Token: 0x04000CDE RID: 3294
	public bool HeadLightLow;

	// Token: 0x04000CDF RID: 3295
	public bool HeadLightHigh;

	// Token: 0x04000CE0 RID: 3296
	public bool RunningLight;

	// Token: 0x04000CE1 RID: 3297
	public bool RightLight;

	// Token: 0x04000CE2 RID: 3298
	public bool LeftLight;

	// Token: 0x04000CE3 RID: 3299
	public Renderer LightRend;

	// Token: 0x04000CE4 RID: 3300
	public Light Low;

	// Token: 0x04000CE5 RID: 3301
	public Light High;

	// Token: 0x04000CE6 RID: 3302
	public int MaterialIndex;

	// Token: 0x04000CE7 RID: 3303
	[ColorUsage(true, true)]
	[Tooltip("    Color of the emitted light.")]
	public Color emissionColor;

	// Token: 0x04000CE8 RID: 3304
	[ColorUsage(true, true)]
	[Tooltip("    Color of the emitted light.")]
	public Color BrakeColor;

	// Token: 0x04000CE9 RID: 3305
	public bool DontNeedBulb;
}
