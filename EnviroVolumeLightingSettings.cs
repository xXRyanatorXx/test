using System;
using UnityEngine;

// Token: 0x02000087 RID: 135
[Serializable]
public class EnviroVolumeLightingSettings
{
	// Token: 0x0400039F RID: 927
	[Tooltip("Downsampling of volume light rendering.")]
	public EnviroSkyRendering.VolumtericResolution Resolution = EnviroSkyRendering.VolumtericResolution.Quarter;

	// Token: 0x040003A0 RID: 928
	[Tooltip("Activate or deactivate directional volume light rendering.")]
	public bool dirVolumeLighting = true;

	// Token: 0x040003A1 RID: 929
	[Header("Quality")]
	[Range(1f, 64f)]
	public int SampleCount = 8;

	// Token: 0x040003A2 RID: 930
	[Header("Light Settings")]
	public AnimationCurve ScatteringCoef = new AnimationCurve();

	// Token: 0x040003A3 RID: 931
	[Range(0f, 0.1f)]
	public float ExtinctionCoef = 0.05f;

	// Token: 0x040003A4 RID: 932
	[Range(0f, 0.999f)]
	public float Anistropy = 0.1f;

	// Token: 0x040003A5 RID: 933
	public float MaxRayLength = 10f;

	// Token: 0x040003A6 RID: 934
	[Header("3D Noise")]
	[Tooltip("Use 3D noise for directional lighting. Attention: Expensive operation for directional lights with high sample count!")]
	public bool directLightNoise;

	// Token: 0x040003A7 RID: 935
	[Range(0f, 1f)]
	[Tooltip("The noise intensity volume lighting.")]
	public float noiseIntensity = 1f;

	// Token: 0x040003A8 RID: 936
	[Tooltip("The noise intensity offset of volume lighting.")]
	[Range(0f, 1f)]
	public float noiseIntensityOffset = 0.3f;

	// Token: 0x040003A9 RID: 937
	[Range(0f, 0.1f)]
	[Tooltip("The noise scaling of volume lighting.")]
	public float noiseScale = 0.001f;

	// Token: 0x040003AA RID: 938
	[Tooltip("The speed and direction of volume lighting.")]
	public Vector2 noiseVelocity = new Vector2(3f, 1.5f);
}
