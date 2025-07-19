using System;
using UnityEngine;

// Token: 0x0200012D RID: 301
public class CarLight : MonoBehaviour
{
	// Token: 0x0600065B RID: 1627 RVA: 0x000337E1 File Offset: 0x000319E1
	public void TurnOn()
	{
		Material material = this.LightRend.materials[this.MaterialIndex];
		material.EnableKeyword("_EMISSION");
		material.SetColor("_EmissionColor", this.emissionColor);
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x00033810 File Offset: 0x00031A10
	public void TurnOnBrake()
	{
		Material material = this.LightRend.materials[this.MaterialIndex];
		material.EnableKeyword("_EMISSION");
		material.SetColor("_EmissionColor", this.BrakeColor);
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x0003383F File Offset: 0x00031A3F
	public void TurnOff()
	{
		if (this.LightRend)
		{
			this.LightRend.materials[this.MaterialIndex].DisableKeyword("_EMISSION");
		}
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x0003386C File Offset: 0x00031A6C
	public void Remove()
	{
		if (this.Low)
		{
			this.Low.enabled = false;
		}
		if (this.High)
		{
			this.High.enabled = false;
		}
		this.LightRend.materials[this.MaterialIndex].DisableKeyword("_EMISSION");
	}

	// Token: 0x040009B5 RID: 2485
	public CarProperties LightBulb;

	// Token: 0x040009B6 RID: 2486
	public bool BrakeLight;

	// Token: 0x040009B7 RID: 2487
	public bool ReverseLight;

	// Token: 0x040009B8 RID: 2488
	public bool HeadLightLow;

	// Token: 0x040009B9 RID: 2489
	public bool HeadLightHigh;

	// Token: 0x040009BA RID: 2490
	public bool RunningLight;

	// Token: 0x040009BB RID: 2491
	public bool RightLight;

	// Token: 0x040009BC RID: 2492
	public bool LeftLight;

	// Token: 0x040009BD RID: 2493
	public Renderer LightRend;

	// Token: 0x040009BE RID: 2494
	public Light Low;

	// Token: 0x040009BF RID: 2495
	public Light High;

	// Token: 0x040009C0 RID: 2496
	public int MaterialIndex;

	// Token: 0x040009C1 RID: 2497
	[ColorUsage(true, true)]
	[Tooltip("    Color of the emitted light.")]
	public Color emissionColor;

	// Token: 0x040009C2 RID: 2498
	[ColorUsage(true, true)]
	[Tooltip("    Color of the emitted light.")]
	public Color BrakeColor;

	// Token: 0x040009C3 RID: 2499
	public bool DontNeedBulb;
}
