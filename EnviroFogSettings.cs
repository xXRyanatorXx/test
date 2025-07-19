using System;
using UnityEngine;

// Token: 0x02000086 RID: 134
[Serializable]
public class EnviroFogSettings
{
	// Token: 0x04000386 RID: 902
	[Header("Mode")]
	[Tooltip("Unity's fog mode.")]
	public FogMode Fogmode = FogMode.Exponential;

	// Token: 0x04000387 RID: 903
	[Tooltip("Simple fog = just plain color without scattering.")]
	public bool useSimpleFog;

	// Token: 0x04000388 RID: 904
	[Tooltip("Use Unity Forward Rendering Fog.")]
	public bool useUnityFog;

	// Token: 0x04000389 RID: 905
	[Header("Distance Fog")]
	[Tooltip("Use distance fog?")]
	public bool distanceFog = true;

	// Token: 0x0400038A RID: 906
	[Tooltip("Use radial distance fog?")]
	public bool useRadialDistance = true;

	// Token: 0x0400038B RID: 907
	[Tooltip("The distance where fog starts.")]
	public float startDistance;

	// Token: 0x0400038C RID: 908
	[Range(0f, 10f)]
	[Tooltip("The intensity of distance fog.")]
	public float distanceFogIntensity = 4f;

	// Token: 0x0400038D RID: 909
	[Range(0f, 1f)]
	[Tooltip("The maximum density of fog.")]
	public float maximumFogDensity = 0.9f;

	// Token: 0x0400038E RID: 910
	[Header("Height Fog")]
	[Tooltip("Use heightbased fog?")]
	public bool heightFog = true;

	// Token: 0x0400038F RID: 911
	[Tooltip("The height of heightbased fog.")]
	public float height = 90f;

	// Token: 0x04000390 RID: 912
	[Range(0f, 1f)]
	[Tooltip("The intensity of heightbased fog.")]
	public float heightFogIntensity = 1f;

	// Token: 0x04000391 RID: 913
	[HideInInspector]
	public float heightDensity = 0.15f;

	// Token: 0x04000392 RID: 914
	[Header("Height Fog Noise")]
	[Range(0f, 1f)]
	[Tooltip("The noise intensity of height based fog.")]
	public float noiseIntensity = 1f;

	// Token: 0x04000393 RID: 915
	[Tooltip("The noise intensity offset of height based fog.")]
	[Range(0f, 1f)]
	public float noiseIntensityOffset = 0.3f;

	// Token: 0x04000394 RID: 916
	[Range(0f, 0.1f)]
	[Tooltip("The noise scaling of height based fog.")]
	public float noiseScale = 0.001f;

	// Token: 0x04000395 RID: 917
	[Tooltip("The speed and direction of height based fog.")]
	public Vector2 noiseVelocity = new Vector2(3f, 1.5f);

	// Token: 0x04000396 RID: 918
	[Tooltip("Influence scattering near sun.")]
	public float mie = 5f;

	// Token: 0x04000397 RID: 919
	[Tooltip("Influence scattering near sun.")]
	public float g = 5f;

	// Token: 0x04000398 RID: 920
	[Header("Fog Dithering")]
	[Range(0f, 1f)]
	public float fogDithering = 0.5f;

	// Token: 0x04000399 RID: 921
	[Tooltip("Color gradient for Top Fog")]
	[GradientUsage(true)]
	public Gradient simpleFogColor;

	// Token: 0x0400039A RID: 922
	[HideInInspector]
	public float skyFogIntensity = 1f;

	// Token: 0x0400039B RID: 923
	[Tooltip("Fog tonemapping exposure when using the enviro tonemapper.")]
	public float fogExposure = 1f;

	// Token: 0x0400039C RID: 924
	public bool useEnviroGroundFog;

	// Token: 0x0400039D RID: 925
	public bool useHDRPFog = true;

	// Token: 0x0400039E RID: 926
	[Tooltip("Fog color tint based on sun altitude.")]
	[GradientUsage(true)]
	public Gradient fogColorTint;
}
