using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006F RID: 111
[Serializable]
public class EnviroWeather
{
	// Token: 0x0400026F RID: 623
	[Tooltip("If disabled the weather will never change.")]
	public bool updateWeather = true;

	// Token: 0x04000270 RID: 624
	public List<EnviroWeatherPreset> weatherPresets = new List<EnviroWeatherPreset>();

	// Token: 0x04000271 RID: 625
	public List<EnviroWeatherPrefab> WeatherPrefabs = new List<EnviroWeatherPrefab>();

	// Token: 0x04000272 RID: 626
	[Tooltip("List of additional zones. Will be updated on startup!")]
	public List<EnviroZone> zones = new List<EnviroZone>();

	// Token: 0x04000273 RID: 627
	public EnviroWeatherPreset startWeatherPreset;

	// Token: 0x04000274 RID: 628
	[Tooltip("The current active zone.")]
	public EnviroZone currentActiveZone;

	// Token: 0x04000275 RID: 629
	[Tooltip("The current active weather conditions.")]
	public EnviroWeatherPrefab currentActiveWeatherPrefab;

	// Token: 0x04000276 RID: 630
	public EnviroWeatherPreset currentActiveWeatherPreset;

	// Token: 0x04000277 RID: 631
	[HideInInspector]
	public EnviroWeatherPrefab lastActiveWeatherPrefab;

	// Token: 0x04000278 RID: 632
	[HideInInspector]
	public EnviroWeatherPreset lastActiveWeatherPreset;

	// Token: 0x04000279 RID: 633
	[HideInInspector]
	public GameObject VFXHolder;

	// Token: 0x0400027A RID: 634
	[HideInInspector]
	public float wetness;

	// Token: 0x0400027B RID: 635
	[HideInInspector]
	public float curWetness;

	// Token: 0x0400027C RID: 636
	[HideInInspector]
	public float snowStrength;

	// Token: 0x0400027D RID: 637
	[HideInInspector]
	public float curSnowStrength;

	// Token: 0x0400027E RID: 638
	[HideInInspector]
	public int thundersfx;

	// Token: 0x0400027F RID: 639
	[HideInInspector]
	public EnviroAudioSource currentAudioSource;

	// Token: 0x04000280 RID: 640
	[HideInInspector]
	public bool weatherFullyChanged;

	// Token: 0x04000281 RID: 641
	[HideInInspector]
	public float currentTemperature;

	// Token: 0x04000282 RID: 642
	[HideInInspector]
	public float temperatureModifier;
}
