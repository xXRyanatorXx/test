using System;
using UnityEngine;

// Token: 0x0200006E RID: 110
[Serializable]
public class EnviroSatellite
{
	// Token: 0x04000269 RID: 617
	[Tooltip("Name of this satellite")]
	public string name;

	// Token: 0x0400026A RID: 618
	[Tooltip("Prefab with model that get instantiated.")]
	public GameObject prefab;

	// Token: 0x0400026B RID: 619
	[Tooltip("Orbit distance.")]
	public float orbit;

	// Token: 0x0400026C RID: 620
	[Tooltip("Orbit modification on x axis.")]
	public float xRot;

	// Token: 0x0400026D RID: 621
	[Tooltip("Orbit modification on y axis.")]
	public float yRot;

	// Token: 0x0400026E RID: 622
	[Tooltip("Orbit modification on z axis.")]
	public float zRot;
}
