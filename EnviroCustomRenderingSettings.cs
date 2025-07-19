using System;
using UnityEngine;

// Token: 0x020000CB RID: 203
[Serializable]
public class EnviroCustomRenderingSettings
{
	// Token: 0x040005D2 RID: 1490
	[Header("Feature Control")]
	public bool useVolumeClouds = true;

	// Token: 0x040005D3 RID: 1491
	public bool useVolumeLighting = true;

	// Token: 0x040005D4 RID: 1492
	public bool useDistanceBlur = true;

	// Token: 0x040005D5 RID: 1493
	public bool useFog = true;

	// Token: 0x040005D6 RID: 1494
	public EnviroVolumeCloudsQuality customCloudsQuality;
}
