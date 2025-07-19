using System;
using UnityEngine;

// Token: 0x0200008A RID: 138
[Serializable]
public class EnviroCloudSettings
{
	// Token: 0x040003B6 RID: 950
	public EnviroVolumeCloudsQualitySettings cloudsQualitySettings;

	// Token: 0x040003B7 RID: 951
	[Range(10000f, 486000f)]
	[Tooltip("Clouds world scale. This settings will influece rendering of clouds at horizon.")]
	public float cloudsWorldScale = 113081f;

	// Token: 0x040003B8 RID: 952
	[Tooltip("Change Clouds Height.")]
	[Range(-2000f, 2000f)]
	public float cloudsHeightMod;

	// Token: 0x040003B9 RID: 953
	[Tooltip("Changes the way sky fog will blend with clouds when inside/above the volumetric clouds.")]
	[Range(1000f, 20000f)]
	public float cloudsSkyFogHeightBlending = 15000f;

	// Token: 0x040003BA RID: 954
	[Tooltip("Enable this option to blend clouds with your scene.")]
	public bool depthBlending;

	// Token: 0x040003BB RID: 955
	[Tooltip("Use this option to minimize blending artifacts of downsampled clouds with full resolution scene.")]
	public bool bilateralUpsampling;

	// Token: 0x040003BC RID: 956
	[Header("Clouds Wind Animation")]
	public bool useWindZoneDirection;

	// Token: 0x040003BD RID: 957
	[Range(-1f, 1f)]
	[Tooltip("Time scale / wind animation speed of clouds.")]
	public float cloudsTimeScale = 1f;

	// Token: 0x040003BE RID: 958
	[Range(0f, 1f)]
	[Tooltip("Global clouds wind speed modificator.")]
	public float cloudsWindIntensity = 0.001f;

	// Token: 0x040003BF RID: 959
	[Range(0f, 1f)]
	[Tooltip("Global clouds detail wind speed modificator.")]
	public float cloudsDetailWindIntensity = 0.001f;

	// Token: 0x040003C0 RID: 960
	[Range(0f, 1f)]
	[Tooltip("Global clouds upwards wind speed modificator.")]
	public float cloudsUpwardsWindIntensity = 0.001f;

	// Token: 0x040003C1 RID: 961
	[Range(0f, 1f)]
	[Tooltip("Cirrus clouds wind speed modificator.")]
	public float cirrusWindIntensity = 0.001f;

	// Token: 0x040003C2 RID: 962
	[Range(-1f, 1f)]
	[Tooltip("Global clouds wind direction X axes.")]
	public float cloudsWindDirectionX = 1f;

	// Token: 0x040003C3 RID: 963
	[Range(-1f, 1f)]
	[Tooltip("Global clouds wind direction Y axes.")]
	public float cloudsWindDirectionY = 1f;

	// Token: 0x040003C4 RID: 964
	[Tooltip("Clamps directional shadows on clouds.")]
	public AnimationCurve attenuationClamp = new AnimationCurve();

	// Token: 0x040003C5 RID: 965
	[Tooltip("Sun highlight in near of sun.")]
	[Range(0.01f, 1f)]
	public float hgPhase = 0.5f;

	// Token: 0x040003C6 RID: 966
	[Range(0.01f, 1f)]
	[Tooltip("SilverLining intensity away from sun. Evaluated based on sun position. Keep between 0-1 range!")]
	public float silverLiningIntensity = 0.5f;

	// Token: 0x040003C7 RID: 967
	[Tooltip("SilverLining spread away from sun. Evaluated based on sun position. Keep between 0-1 range!")]
	public AnimationCurve silverLiningSpread = new AnimationCurve();

	// Token: 0x040003C8 RID: 968
	[Tooltip("Global Color for volume clouds based sun positon.")]
	[GradientUsage(true)]
	public Gradient volumeCloudsColor = new Gradient();

	// Token: 0x040003C9 RID: 969
	[Tooltip("Global Color for clouds based moon positon.")]
	[GradientUsage(true)]
	public Gradient volumeCloudsMoonColor = new Gradient();

	// Token: 0x040003CA RID: 970
	[Tooltip("Global ambient color add for volume clouds based sun positon.")]
	[GradientUsage(true)]
	public Gradient volumeCloudsAmbientColor = new Gradient();

	// Token: 0x040003CB RID: 971
	[Tooltip("Raie or lower the light intensity based on sun altitude.")]
	public AnimationCurve lightIntensity = new AnimationCurve();

	// Token: 0x040003CC RID: 972
	[Tooltip("Tweak the ambient lighting from sky based on sun altitude.")]
	public AnimationCurve ambientLightIntensity = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 1f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x040003CD RID: 973
	[Tooltip("Tonemapping exposure")]
	public float cloudsExposure = 1f;

	// Token: 0x040003CE RID: 974
	[Tooltip("Use Halton Sequence based raymarching offset to help with undersampling raymarching. This option will make clouds more noisy, so use it with TAA only.")]
	public bool useHaltonRaymarchOffset;

	// Token: 0x040003CF RID: 975
	[Tooltip("Enable this to only use half the amount of raymarching steps when using the halton sequence offset together with TAA.")]
	public bool useLessSteps;

	// Token: 0x040003D0 RID: 976
	[Tooltip("Tiling of the generated weather map.")]
	public int weatherMapTiling = 5;

	// Token: 0x040003D1 RID: 977
	[Tooltip("Tiling modification of lighting variance.")]
	[Range(0f, 5f)]
	public float lightingVarianceTiling = 1f;

	// Token: 0x040003D2 RID: 978
	[Tooltip("Option to add own weather map. Red Channel = Coverage, Blue = Clouds Height")]
	public Texture2D customWeatherMap;

	// Token: 0x040003D3 RID: 979
	[Tooltip("Weathermap sampling offset.")]
	public Vector2 locationOffset;

	// Token: 0x040003D4 RID: 980
	[Range(0f, 1f)]
	[Tooltip("Weathermap animation speed.")]
	public float weatherAnimSpeedScale = 0.33f;

	// Token: 0x040003D5 RID: 981
	[Header("Global Clouds Control")]
	[Range(0f, 2f)]
	public float globalCloudCoverage = 1f;

	// Token: 0x040003D6 RID: 982
	[Tooltip("Texture for cirrus clouds.")]
	public Texture cirrusCloudsTexture;

	// Token: 0x040003D7 RID: 983
	[Tooltip("Global Color for flat clouds based sun positon.")]
	public Gradient cirrusCloudsColor;

	// Token: 0x040003D8 RID: 984
	[Range(5f, 15f)]
	[Tooltip("Flat Clouds Altitude")]
	public float cirrusCloudsAltitude = 10f;

	// Token: 0x040003D9 RID: 985
	[Tooltip("Base texture for 2D clouds.")]
	public Texture2D flatCloudsBaseTexture;

	// Token: 0x040003DA RID: 986
	[Range(1f, 10f)]
	public float flatCloudsBaseTextureTiling = 2f;

	// Token: 0x040003DB RID: 987
	[Tooltip("Detail texture for 2D clouds.")]
	public Texture2D flatCloudsDetailTexture;

	// Token: 0x040003DC RID: 988
	[Range(1f, 20f)]
	public float flatCloudsDetailTextureTiling = 8f;

	// Token: 0x040003DD RID: 989
	[Tooltip("Direct Light Color for flat clouds based sun positon.")]
	[GradientUsage(true)]
	public Gradient flatCloudsDirectLightColor;

	// Token: 0x040003DE RID: 990
	[Tooltip("Light Color for flat clouds based sun positon.")]
	[GradientUsage(true)]
	public Gradient flatCloudsAmbientLightColor;

	// Token: 0x040003DF RID: 991
	[Tooltip("Flat Clouds Altitude")]
	[Range(50f, 250f)]
	public float flatCloudsAltitude = 150f;

	// Token: 0x040003E0 RID: 992
	[Tooltip("Clouds Shadowcast Intensity. 0 = disabled")]
	[Range(0f, 1f)]
	public float shadowIntensity;

	// Token: 0x040003E1 RID: 993
	[Tooltip("Size of the shadow cookie.")]
	[Range(100f, 100000f)]
	public int shadowCookieSize = 100000;

	// Token: 0x040003E2 RID: 994
	public EnviroParticleClouds ParticleCloudsLayer1 = new EnviroParticleClouds();

	// Token: 0x040003E3 RID: 995
	public EnviroParticleClouds ParticleCloudsLayer2 = new EnviroParticleClouds();

	// Token: 0x040003E4 RID: 996
	[Tooltip("Enable this to use two layer of particle clouds.")]
	public bool dualLayerParticleClouds = true;

	// Token: 0x0200008B RID: 139
	public enum FlatCloudResolution
	{
		// Token: 0x040003E6 RID: 998
		R512,
		// Token: 0x040003E7 RID: 999
		R1024,
		// Token: 0x040003E8 RID: 1000
		R2048,
		// Token: 0x040003E9 RID: 1001
		R4096
	}
}
