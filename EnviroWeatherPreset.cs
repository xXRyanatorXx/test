using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AD RID: 173
[Serializable]
public class EnviroWeatherPreset : ScriptableObject
{
	// Token: 0x040004CB RID: 1227
	public string version;

	// Token: 0x040004CC RID: 1228
	public string Name;

	// Token: 0x040004CD RID: 1229
	[Header("Season Settings")]
	public bool Spring = true;

	// Token: 0x040004CE RID: 1230
	[Range(1f, 100f)]
	public float possibiltyInSpring = 50f;

	// Token: 0x040004CF RID: 1231
	public bool Summer = true;

	// Token: 0x040004D0 RID: 1232
	[Range(1f, 100f)]
	public float possibiltyInSummer = 50f;

	// Token: 0x040004D1 RID: 1233
	public bool Autumn = true;

	// Token: 0x040004D2 RID: 1234
	[Range(1f, 100f)]
	public float possibiltyInAutumn = 50f;

	// Token: 0x040004D3 RID: 1235
	public bool winter = true;

	// Token: 0x040004D4 RID: 1236
	[Range(1f, 100f)]
	public float possibiltyInWinter = 50f;

	// Token: 0x040004D5 RID: 1237
	[Header("Cloud Settings")]
	public EnviroWeatherCloudsConfig cloudsConfig;

	// Token: 0x040004D6 RID: 1238
	[Header("Linear Fog")]
	public float fogStartDistance;

	// Token: 0x040004D7 RID: 1239
	public float fogDistance = 1000f;

	// Token: 0x040004D8 RID: 1240
	[Header("Exp Fog")]
	public float fogDensity = 0.0001f;

	// Token: 0x040004D9 RID: 1241
	[Tooltip("Used to modify sky, direct, ambient light and fog color. The color alpha value defines the intensity")]
	public Gradient weatherSkyMod;

	// Token: 0x040004DA RID: 1242
	public Gradient weatherLightMod;

	// Token: 0x040004DB RID: 1243
	public Gradient weatherFogMod;

	// Token: 0x040004DC RID: 1244
	[Range(0f, 2f)]
	public float volumeLightIntensity = 1f;

	// Token: 0x040004DD RID: 1245
	[Range(-1f, 1f)]
	public float shadowIntensityMod;

	// Token: 0x040004DE RID: 1246
	[Range(0f, 100f)]
	[Tooltip("The density of height based fog for this weather.")]
	public float heightFogDensity = 1f;

	// Token: 0x040004DF RID: 1247
	[Range(0f, 2f)]
	[Tooltip("Define the height of fog rendered in sky.")]
	public float SkyFogHeight = 0.5f;

	// Token: 0x040004E0 RID: 1248
	[Range(0f, 1f)]
	[Tooltip("Define the start height of fog rendered in sky.")]
	public float skyFogStart;

	// Token: 0x040004E1 RID: 1249
	[Tooltip("Define the intensity of fog rendered in sky.")]
	[Range(0f, 2f)]
	public float SkyFogIntensity = 1f;

	// Token: 0x040004E2 RID: 1250
	[Range(1f, 10f)]
	[Tooltip("Define the scattering intensity of fog.")]
	public float FogScatteringIntensity = 1f;

	// Token: 0x040004E3 RID: 1251
	[Range(0f, 1f)]
	[Tooltip("Block the sundisk with fog.")]
	public float fogSunBlocking = 0.25f;

	// Token: 0x040004E4 RID: 1252
	[Range(0f, 1f)]
	[Tooltip("Block the moon with fog.")]
	public float moonIntensity = 1f;

	// Token: 0x040004E5 RID: 1253
	[Header("Weather Settings")]
	public List<EnviroWeatherEffects> effectSystems = new List<EnviroWeatherEffects>();

	// Token: 0x040004E6 RID: 1254
	[Range(0f, 1f)]
	[Tooltip("Wind intensity that will applied to wind zone.")]
	public float WindStrenght = 0.5f;

	// Token: 0x040004E7 RID: 1255
	[Range(0f, 1f)]
	[Tooltip("The maximum wetness level that can be reached.")]
	public float wetnessLevel;

	// Token: 0x040004E8 RID: 1256
	[Range(0f, 1f)]
	[Tooltip("The maximum snow level that can be reached.")]
	public float snowLevel;

	// Token: 0x040004E9 RID: 1257
	[Range(-50f, 50f)]
	[Tooltip("The temperature modifcation for this weather type. (Will be added or substracted)")]
	public float temperatureLevel;

	// Token: 0x040004EA RID: 1258
	[Tooltip("Activate this to enable thunder and lightning.")]
	public bool isLightningStorm;

	// Token: 0x040004EB RID: 1259
	[Range(0f, 2f)]
	[Tooltip("The Intervall of lightning in seconds. Random(lightningInterval,lightningInterval * 2). ")]
	public float lightningInterval = 10f;

	// Token: 0x040004EC RID: 1260
	[Header("Aurora Settings")]
	[Range(0f, 1f)]
	public float auroraIntensity;

	// Token: 0x040004ED RID: 1261
	[Header("Audio Settings - SFX")]
	[Tooltip("Define an sound effect for this weather preset.")]
	public AudioClip weatherSFX;

	// Token: 0x040004EE RID: 1262
	[Header("Audio Settings - Ambient")]
	[Tooltip("This sound wil be played in spring at day.(looped)")]
	public AudioClip SpringDayAmbient;

	// Token: 0x040004EF RID: 1263
	[Tooltip("This sound wil be played in spring at night.(looped)")]
	public AudioClip SpringNightAmbient;

	// Token: 0x040004F0 RID: 1264
	[Tooltip("This sound wil be played in summer at day.(looped)")]
	public AudioClip SummerDayAmbient;

	// Token: 0x040004F1 RID: 1265
	[Tooltip("This sound wil be played in summer at night.(looped)")]
	public AudioClip SummerNightAmbient;

	// Token: 0x040004F2 RID: 1266
	[Tooltip("This sound wil be played in autumn at day.(looped)")]
	public AudioClip AutumnDayAmbient;

	// Token: 0x040004F3 RID: 1267
	[Tooltip("This sound wil be played in autumn at night.(looped)")]
	public AudioClip AutumnNightAmbient;

	// Token: 0x040004F4 RID: 1268
	[Tooltip("This sound wil be played in winter at day.(looped)")]
	public AudioClip WinterDayAmbient;

	// Token: 0x040004F5 RID: 1269
	[Tooltip("This sound wil be played in winter at night.(looped)")]
	public AudioClip WinterNightAmbient;

	// Token: 0x040004F6 RID: 1270
	public float blurDistance = 100f;

	// Token: 0x040004F7 RID: 1271
	public float blurIntensity = 1f;

	// Token: 0x040004F8 RID: 1272
	public float blurSkyIntensity = 1f;

	// Token: 0x040004F9 RID: 1273
	public float sceneExposureMod = 1f;

	// Token: 0x040004FA RID: 1274
	public float skyExposureMod = 1f;

	// Token: 0x040004FB RID: 1275
	public float lightIntensityMod = 1f;
}
