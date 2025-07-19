using System;
using UnityEngine;

// Token: 0x02000088 RID: 136
[Serializable]
public class EnviroLightShaftsSettings
{
	// Token: 0x040003AB RID: 939
	[Header("Quality Settings")]
	[Tooltip("Lightshafts resolution quality setting.")]
	public EnviroPostProcessing.SunShaftsResolution resolution = EnviroPostProcessing.SunShaftsResolution.Normal;

	// Token: 0x040003AC RID: 940
	[Tooltip("Lightshafts blur mode.")]
	public EnviroPostProcessing.ShaftsScreenBlendMode screenBlendMode;

	// Token: 0x040003AD RID: 941
	[Tooltip("Use cameras depth to hide lightshafts?")]
	public bool useDepthTexture = true;

	// Token: 0x040003AE RID: 942
	[Header("Intensity Settings")]
	[Tooltip("Color gradient for lightshafts based on sun position.")]
	public Gradient lightShaftsColorSun;

	// Token: 0x040003AF RID: 943
	[Tooltip("Color gradient for lightshafts based on moon position.")]
	public Gradient lightShaftsColorMoon;

	// Token: 0x040003B0 RID: 944
	[Tooltip("Treshhold gradient for lightshafts based on sun position. This will influence lightshafts intensity!")]
	public Gradient thresholdColorSun;

	// Token: 0x040003B1 RID: 945
	[Tooltip("Treshhold gradient for lightshafts based on moon position. This will influence lightshafts intensity!")]
	public Gradient thresholdColorMoon;

	// Token: 0x040003B2 RID: 946
	[Tooltip("Radius of blurring applied.")]
	public float blurRadius = 6f;

	// Token: 0x040003B3 RID: 947
	[Tooltip("Global Lightshafts intensity.")]
	public float intensity = 0.6f;

	// Token: 0x040003B4 RID: 948
	[Tooltip("Lightshafts maximum radius.")]
	public float maxRadius = 10f;
}
