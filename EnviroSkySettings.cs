using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
[Serializable]
public class EnviroSkySettings
{
	// Token: 0x04000312 RID: 786
	[Header("Sky Mode:")]
	[Tooltip("Select if you want to use enviro skybox your custom material.")]
	public EnviroSkySettings.SkyboxModi skyboxMode;

	// Token: 0x04000313 RID: 787
	[Tooltip("Select if you want to use enviro skybox your custom material.")]
	public EnviroSkySettings.SkyboxModiLW skyboxModeLW;

	// Token: 0x04000314 RID: 788
	[Tooltip("If SkyboxMode == CustomSkybox : Assign your skybox material here!")]
	public Material customSkyboxMaterial;

	// Token: 0x04000315 RID: 789
	[Tooltip("If SkyboxMode == CustomColor : Select your sky color here!")]
	public Color customSkyboxColor;

	// Token: 0x04000316 RID: 790
	[Tooltip("Enable to render black skybox at ground level.")]
	public bool blackGroundMode;

	// Token: 0x04000317 RID: 791
	[Header("Scattering")]
	[Tooltip("Light Wavelength used for atmospheric scattering. Keep it near defaults for earthlike atmospheres, or change for alien or fantasy atmospheres for example.")]
	public Vector3 waveLength = new Vector3(540f, 496f, 437f);

	// Token: 0x04000318 RID: 792
	[Tooltip("Influence atmospheric scattering.")]
	public float rayleigh = 5.15f;

	// Token: 0x04000319 RID: 793
	[Tooltip("Sky turbidity. Particle in air. Influence atmospheric scattering.")]
	public float turbidity = 1f;

	// Token: 0x0400031A RID: 794
	[Tooltip("Influence scattering near sun.")]
	public float mie = 5f;

	// Token: 0x0400031B RID: 795
	[Tooltip("Influence scattering near sun.")]
	public float g = 0.8f;

	// Token: 0x0400031C RID: 796
	[Tooltip("Intensity gradient for atmospheric scattering. Influence atmospheric scattering based on current sun altitude.")]
	public AnimationCurve scatteringCurve = new AnimationCurve();

	// Token: 0x0400031D RID: 797
	[Tooltip("Color gradient for atmospheric scattering. Influence atmospheric scattering based on current sun altitude.")]
	public Gradient scatteringColor;

	// Token: 0x0400031E RID: 798
	[Header("Sun")]
	public EnviroSkySettings.SunAndMoonCalc sunAndMoonPosition = EnviroSkySettings.SunAndMoonCalc.Realistic;

	// Token: 0x0400031F RID: 799
	[Tooltip("Intensity of Sun Influence Scale and Dropoff of sundisk.")]
	public float sunIntensity = 100f;

	// Token: 0x04000320 RID: 800
	[Tooltip("Scale of rendered sundisk.")]
	public float sunDiskScale = 20f;

	// Token: 0x04000321 RID: 801
	[Tooltip("Intenisty of rendered sundisk.")]
	public float sunDiskIntensity = 3f;

	// Token: 0x04000322 RID: 802
	[Tooltip("Color gradient for sundisk. Influence sundisk color based on current sun altitude")]
	[GradientUsage(true)]
	public Gradient sunDiskColor;

	// Token: 0x04000323 RID: 803
	[Tooltip("Top color of simple skybox.")]
	public Gradient simpleSkyColor;

	// Token: 0x04000324 RID: 804
	[Tooltip("Horizon color of simple skybox.")]
	public Gradient simpleHorizonColor;

	// Token: 0x04000325 RID: 805
	[Tooltip("Horizon color of opposite side of sun.")]
	public Gradient simpleHorizonBackColor;

	// Token: 0x04000326 RID: 806
	[Tooltip("Sun color of simple skybox.")]
	public Gradient simpleSunColor;

	// Token: 0x04000327 RID: 807
	[Tooltip("Ground color of simple skybox.")]
	public Color simpleGroundColor;

	// Token: 0x04000328 RID: 808
	[Tooltip("Size of sun in simple skybox mode.")]
	public AnimationCurve simpleSunDiskSize = new AnimationCurve();

	// Token: 0x04000329 RID: 809
	[Header("Moon")]
	[Tooltip("Whether to render the moon.")]
	public bool renderMoon = true;

	// Token: 0x0400032A RID: 810
	[Tooltip("The Moon phase mode. Custom = for customizable phase.")]
	public EnviroSkySettings.MoonPhases moonPhaseMode = EnviroSkySettings.MoonPhases.Realistic;

	// Token: 0x0400032B RID: 811
	[Tooltip("The Moon texture.")]
	public Texture moonTexture;

	// Token: 0x0400032C RID: 812
	[Tooltip("The Moon's Glow texture.")]
	public Texture glowTexture;

	// Token: 0x0400032D RID: 813
	[Tooltip("The color of the moon")]
	public Color moonColor;

	// Token: 0x0400032E RID: 814
	[Range(0f, 5f)]
	[Tooltip("Brightness of the moon.")]
	public float moonBrightness = 1f;

	// Token: 0x0400032F RID: 815
	[Range(0f, 20f)]
	[Tooltip("Size of the moon.")]
	public float moonSize = 10f;

	// Token: 0x04000330 RID: 816
	[Range(0f, 20f)]
	[Tooltip("Size of the moon glowing effect.")]
	public float glowSize = 10f;

	// Token: 0x04000331 RID: 817
	[Tooltip("Glow around moon.")]
	public AnimationCurve moonGlow = new AnimationCurve();

	// Token: 0x04000332 RID: 818
	[Tooltip("Glow color around moon.")]
	public Color moonGlowColor;

	// Token: 0x04000333 RID: 819
	[Header("Sky Color Corrections")]
	[Tooltip("Higher values = brighter sky.")]
	public AnimationCurve skyLuminence = new AnimationCurve();

	// Token: 0x04000334 RID: 820
	[Tooltip("Higher values = stronger colors applied BEFORE clouds rendered!")]
	public AnimationCurve skyColorPower = new AnimationCurve();

	// Token: 0x04000335 RID: 821
	[Header("Tonemapping")]
	[Tooltip("Sky exposure when using Enviro Tonemapping option in Rendering Setup.")]
	public float skyExposure = 1.5f;

	// Token: 0x04000336 RID: 822
	[Header("Stars")]
	[Tooltip("A cubemap for night sky.")]
	public Cubemap starsCubeMap;

	// Token: 0x04000337 RID: 823
	[Tooltip("Intensity of stars based on time of day.")]
	public AnimationCurve starsIntensity = new AnimationCurve();

	// Token: 0x04000338 RID: 824
	[Tooltip("Stars Twinkling Speed")]
	[Range(0f, 10f)]
	public float starsTwinklingRate = 1f;

	// Token: 0x04000339 RID: 825
	[Header("Galaxy")]
	[Tooltip("A cubemap for night galaxy.")]
	public Cubemap galaxyCubeMap;

	// Token: 0x0400033A RID: 826
	[Tooltip("Intensity of galaxy based on time of day.")]
	public AnimationCurve galaxyIntensity = new AnimationCurve();

	// Token: 0x0400033B RID: 827
	[Header("Sky Dithering")]
	[Range(0f, 1f)]
	public float dithering = 0.5f;

	// Token: 0x0400033C RID: 828
	[Tooltip("HDRP only. Set sky type to EnviroSkybox on start.")]
	public bool setEnviroSkybox = true;

	// Token: 0x0200007B RID: 123
	public enum SunAndMoonCalc
	{
		// Token: 0x0400033E RID: 830
		Simple,
		// Token: 0x0400033F RID: 831
		Realistic
	}

	// Token: 0x0200007C RID: 124
	public enum MoonPhases
	{
		// Token: 0x04000341 RID: 833
		Custom,
		// Token: 0x04000342 RID: 834
		Realistic
	}

	// Token: 0x0200007D RID: 125
	public enum SkyboxModi
	{
		// Token: 0x04000344 RID: 836
		Default,
		// Token: 0x04000345 RID: 837
		Simple,
		// Token: 0x04000346 RID: 838
		CustomSkybox,
		// Token: 0x04000347 RID: 839
		CustomColor
	}

	// Token: 0x0200007E RID: 126
	public enum SkyboxModiLW
	{
		// Token: 0x04000349 RID: 841
		Simple,
		// Token: 0x0400034A RID: 842
		CustomSkybox,
		// Token: 0x0400034B RID: 843
		CustomColor
	}
}
