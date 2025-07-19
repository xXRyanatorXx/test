using System;
using UnityEngine;

// Token: 0x02000085 RID: 133
[Serializable]
public class EnviroDistanceBlurSettings
{
	// Token: 0x04000383 RID: 899
	public bool antiFlicker = true;

	// Token: 0x04000384 RID: 900
	public bool highQuality = true;

	// Token: 0x04000385 RID: 901
	[Range(1f, 7f)]
	public float radius = 7f;
}
