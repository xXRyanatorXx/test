using System;
using UnityEngine;

// Token: 0x020000A3 RID: 163
[Serializable]
public class EnviroVegetationStage
{
	// Token: 0x04000475 RID: 1141
	[Range(0f, 100f)]
	public float minAgePercent;

	// Token: 0x04000476 RID: 1142
	public EnviroVegetationStage.GrowState growAction;

	// Token: 0x04000477 RID: 1143
	public GameObject GrowGameobjectSpring;

	// Token: 0x04000478 RID: 1144
	public GameObject GrowGameobjectSummer;

	// Token: 0x04000479 RID: 1145
	public GameObject GrowGameobjectAutumn;

	// Token: 0x0400047A RID: 1146
	public GameObject GrowGameobjectWinter;

	// Token: 0x0400047B RID: 1147
	public bool billboard;

	// Token: 0x020000A4 RID: 164
	public enum GrowState
	{
		// Token: 0x0400047D RID: 1149
		Grow,
		// Token: 0x0400047E RID: 1150
		Stay
	}
}
