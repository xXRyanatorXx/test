using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000082 RID: 130
[Serializable]
public class EnviroLightSettings
{
	// Token: 0x04000363 RID: 867
	[Tooltip("Whether you want to use two direcitonal lights for sun and moon or only one that will switch. Dual mode can be expensive in complex scenes!")]
	public EnviroLightSettings.LightingMode directionalLightMode;

	// Token: 0x04000364 RID: 868
	[Tooltip("Color gradient for sun and moon light based on sun position in sky.")]
	[GradientUsage(true)]
	public Gradient LightColor;

	// Token: 0x04000365 RID: 869
	[Tooltip("Direct light sun intensity based on sun position in sky")]
	public AnimationCurve directLightSunIntensity = new AnimationCurve();

	// Token: 0x04000366 RID: 870
	[Tooltip("Direct light moon intensity based on moon position in sky")]
	public AnimationCurve directLightMoonIntensity = new AnimationCurve();

	// Token: 0x04000367 RID: 871
	[Tooltip("Set the speed of how fast light intensity will update.")]
	[Range(0.01f, 10f)]
	public float lightIntensityTransitionSpeed = 1f;

	// Token: 0x04000368 RID: 872
	[Tooltip("Realtime shadow strength of the directional light.")]
	public AnimationCurve shadowIntensity = new AnimationCurve();

	// Token: 0x04000369 RID: 873
	[Tooltip("Direct lighting y-offset.")]
	[Range(0f, 5000f)]
	public float directLightAngleOffset;

	// Token: 0x0400036A RID: 874
	[Header("Ambient")]
	[Tooltip("Ambient Rendering Mode.")]
	public AmbientMode ambientMode = AmbientMode.Flat;

	// Token: 0x0400036B RID: 875
	[Tooltip("Ambientlight intensity based on sun position in sky.")]
	public AnimationCurve ambientIntensity = new AnimationCurve();

	// Token: 0x0400036C RID: 876
	[Tooltip("Ambientlight sky color based on sun position in sky.")]
	[GradientUsage(true)]
	public Gradient ambientSkyColor;

	// Token: 0x0400036D RID: 877
	[Tooltip("Ambientlight Equator color based on sun position in sky.")]
	[GradientUsage(true)]
	public Gradient ambientEquatorColor;

	// Token: 0x0400036E RID: 878
	[Tooltip("Ambientlight Ground color based on sun position in sky.")]
	[GradientUsage(true)]
	public Gradient ambientGroundColor;

	// Token: 0x0400036F RID: 879
	[Tooltip("Activate to stop the rotation of sun and moon at 'rotationStopHigh' sun/moon altitude in sky.")]
	public bool stopRotationAtHigh;

	// Token: 0x04000370 RID: 880
	[Range(0f, 1f)]
	[Tooltip("The altitude of sun/moon in sky (Same as 'DayNightSwitch' or the evaluatation of gradients.")]
	public float rotationStopHigh = 0.5f;

	// Token: 0x04000371 RID: 881
	public bool usePhysicalBasedLighting = true;

	// Token: 0x04000372 RID: 882
	[Tooltip("Direct light sun intensity based on sun position in sky in LUX.")]
	public AnimationCurve sunIntensityLux = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 120000f)
	});

	// Token: 0x04000373 RID: 883
	[Tooltip("Direct light moon intensity based on moon position in sky in LUX.")]
	public AnimationCurve moonIntensityLux = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x04000374 RID: 884
	[Tooltip("Color Temperature based on sun position in sky.")]
	public AnimationCurve lightColorTemperature = new AnimationCurve();

	// Token: 0x04000375 RID: 885
	[GradientUsage(true)]
	public Gradient lightColorTint;

	// Token: 0x04000376 RID: 886
	[Tooltip("Enable this to control the scene exposure with Enviro. Otherwise check the Enviro - Post Processing Volume.")]
	public bool controlSceneExposure = true;

	// Token: 0x04000377 RID: 887
	[Tooltip("Set fixed exposure for your scene.")]
	[Range(0f, 20f)]
	public float exposure = 7f;

	// Token: 0x04000378 RID: 888
	[Tooltip("Set sky exposure.")]
	[Range(0f, 20f)]
	public float skyExposure = 7f;

	// Token: 0x04000379 RID: 889
	[Range(0f, 20f)]
	[Tooltip("Increase standard light intensity to LUX values.")]
	public float lightIntensityLuxMult = 7f;

	// Token: 0x0400037A RID: 890
	[Tooltip("Set fixed exposure for your scene based on sun position in sky.")]
	public AnimationCurve exposurePhysical = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 3f),
		new Keyframe(1f, 14.5f)
	});

	// Token: 0x0400037B RID: 891
	[Tooltip("Set sky exposure based on sun position in sky.")]
	public AnimationCurve skyExposurePhysical = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 3f),
		new Keyframe(1f, 14.5f)
	});

	// Token: 0x0400037C RID: 892
	public EnviroLightSettings.AmbientUpdateMode indirectLightingUpdateMode;

	// Token: 0x02000083 RID: 131
	public enum LightingMode
	{
		// Token: 0x0400037E RID: 894
		Single,
		// Token: 0x0400037F RID: 895
		Dual
	}

	// Token: 0x02000084 RID: 132
	public enum AmbientUpdateMode
	{
		// Token: 0x04000381 RID: 897
		Realtime,
		// Token: 0x04000382 RID: 898
		OnChange
	}
}
