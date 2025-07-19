using System;
using UnityEngine;

// Token: 0x020000AB RID: 171
[Serializable]
public class EnviroWeatherCloudsConfig
{
	// Token: 0x040004AB RID: 1195
	[Tooltip("Ambient Light Intensity.")]
	[Range(0f, 1f)]
	public float ambientSkyColorIntensity = 1f;

	// Token: 0x040004AC RID: 1196
	[Tooltip("Light extinction factor.")]
	[Range(0f, 2f)]
	public float scatteringCoef = 1f;

	// Token: 0x040004AD RID: 1197
	[Tooltip("Darkens the edges of clouds from in-out scattering.")]
	[Range(1f, 3f)]
	public float edgeDarkness = 2f;

	// Token: 0x040004AE RID: 1198
	public float baseErosionIntensity;

	// Token: 0x040004AF RID: 1199
	public float detailErosionIntensity = 0.2f;

	// Token: 0x040004B0 RID: 1200
	[Tooltip("Density factor of clouds.")]
	public float density = 1f;

	// Token: 0x040004B1 RID: 1201
	[Tooltip("Light Step modifier.")]
	public float lightStepModifier = 0.5f;

	// Token: 0x040004B2 RID: 1202
	[Tooltip("Min lighting variance intensity.")]
	public float lightVariance = 0.5f;

	// Token: 0x040004B3 RID: 1203
	[Tooltip("Global coverage multiplicator of clouds.")]
	[Range(0f, 1f)]
	public float coverage = 1f;

	// Token: 0x040004B4 RID: 1204
	[Tooltip("Defines how much light will be absorbed from cloud particles.")]
	[Range(0f, 1f)]
	public float lightAbsorbtion = 0.4f;

	// Token: 0x040004B5 RID: 1205
	[Tooltip("Coverage type of clouds. 1 = more round scattered shapes , 0 = connected islands style")]
	[Range(0f, 1f)]
	public float coverageType = 1f;

	// Token: 0x040004B6 RID: 1206
	[Tooltip("Clouds raynarching step modifier.")]
	[Range(0.25f, 1f)]
	public float raymarchingScale = 1f;

	// Token: 0x040004B7 RID: 1207
	[Tooltip("Clouds modelling type.")]
	[Range(0f, 1f)]
	public float cloudType = 1f;

	// Token: 0x040004B8 RID: 1208
	[Tooltip("Cirrus Clouds Alpha")]
	[Range(0f, 1f)]
	public float cirrusAlpha;

	// Token: 0x040004B9 RID: 1209
	[Tooltip("Cirrus Clouds Coverage")]
	[Range(0f, 1f)]
	public float cirrusCoverage;

	// Token: 0x040004BA RID: 1210
	[Tooltip("Cirrus Clouds Color Power")]
	[Range(0f, 1f)]
	public float cirrusColorPow = 2f;

	// Token: 0x040004BB RID: 1211
	[Tooltip("Flat Clouds Density")]
	[Range(0f, 5f)]
	public float flatCloudsDensity = 2.5f;

	// Token: 0x040004BC RID: 1212
	[Tooltip("Flat Clouds Absorbtion")]
	[Range(0f, 5f)]
	public float flatCloudsAbsorbtion = 2.5f;

	// Token: 0x040004BD RID: 1213
	[Tooltip("Flat Clouds Direct Light Intensity")]
	[Range(0f, 20f)]
	public float flatCloudsDirectLightIntensity = 15f;

	// Token: 0x040004BE RID: 1214
	[Tooltip("Flat Clouds Ambient Light Intensity")]
	[Range(0f, 2f)]
	public float flatCloudsAmbientLightIntensity = 1f;

	// Token: 0x040004BF RID: 1215
	[Tooltip("Flat Clouds HG Phase")]
	[Range(0f, 1f)]
	public float flatCloudsHGPhase = 0.9f;

	// Token: 0x040004C0 RID: 1216
	[Tooltip("Flat Clouds Coverage")]
	[Range(0f, 2f)]
	public float flatCoverage;

	// Token: 0x040004C1 RID: 1217
	[Tooltip("Particle Clouds Alpha")]
	[Range(0f, 1f)]
	public float particleLayer1Alpha;

	// Token: 0x040004C2 RID: 1218
	[Tooltip("Particle Clouds Brightness")]
	[Range(0f, 1f)]
	public float particleLayer1Brightness = 0.75f;

	// Token: 0x040004C3 RID: 1219
	[Tooltip("Particle Clouds Color Power")]
	[Range(0f, 1f)]
	public float particleLayer1ColorPow = 2f;

	// Token: 0x040004C4 RID: 1220
	[Tooltip("Particle Clouds Alpha")]
	[Range(0f, 1f)]
	public float particleLayer2Alpha;

	// Token: 0x040004C5 RID: 1221
	[Tooltip("Particle Clouds Brightness")]
	[Range(0f, 1f)]
	public float particleLayer2Brightness = 0.75f;

	// Token: 0x040004C6 RID: 1222
	[Tooltip("Particle Clouds Color Power")]
	[Range(0f, 1f)]
	public float particleLayer2ColorPow = 2f;

	// Token: 0x040004C7 RID: 1223
	[Tooltip("Use particle clouds here even when it is disabled!")]
	public bool particleCloudsOverwrite;
}
