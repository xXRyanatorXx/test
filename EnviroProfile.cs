using System;
using UnityEngine;

// Token: 0x0200008E RID: 142
[Serializable]
public class EnviroProfile : ScriptableObject
{
	// Token: 0x040003F7 RID: 1015
	public string version;

	// Token: 0x040003F8 RID: 1016
	public EnviroLightSettings lightSettings = new EnviroLightSettings();

	// Token: 0x040003F9 RID: 1017
	public EnviroReflectionSettings reflectionSettings = new EnviroReflectionSettings();

	// Token: 0x040003FA RID: 1018
	public EnviroVolumeLightingSettings volumeLightSettings = new EnviroVolumeLightingSettings();

	// Token: 0x040003FB RID: 1019
	public EnviroDistanceBlurSettings distanceBlurSettings = new EnviroDistanceBlurSettings();

	// Token: 0x040003FC RID: 1020
	public EnviroSkySettings skySettings = new EnviroSkySettings();

	// Token: 0x040003FD RID: 1021
	public EnviroCloudSettings cloudsSettings = new EnviroCloudSettings();

	// Token: 0x040003FE RID: 1022
	public EnviroWeatherSettings weatherSettings = new EnviroWeatherSettings();

	// Token: 0x040003FF RID: 1023
	public EnviroFogSettings fogSettings = new EnviroFogSettings();

	// Token: 0x04000400 RID: 1024
	public EnviroLightShaftsSettings lightshaftsSettings = new EnviroLightShaftsSettings();

	// Token: 0x04000401 RID: 1025
	public EnviroSeasonSettings seasonsSettings = new EnviroSeasonSettings();

	// Token: 0x04000402 RID: 1026
	public EnviroAudioSettings audioSettings = new EnviroAudioSettings();

	// Token: 0x04000403 RID: 1027
	public EnviroSatellitesSettings satelliteSettings = new EnviroSatellitesSettings();

	// Token: 0x04000404 RID: 1028
	public EnviroQualitySettings qualitySettings = new EnviroQualitySettings();

	// Token: 0x04000405 RID: 1029
	public EnviroAuroraSettings auroraSettings = new EnviroAuroraSettings();

	// Token: 0x04000406 RID: 1030
	[HideInInspector]
	public EnviroProfile.settingsMode viewMode;

	// Token: 0x04000407 RID: 1031
	[HideInInspector]
	public EnviroProfile.settingsModeLW viewModeLW;

	// Token: 0x04000408 RID: 1032
	[HideInInspector]
	public bool showPlayerSetup = true;

	// Token: 0x04000409 RID: 1033
	[HideInInspector]
	public bool showRenderingSetup;

	// Token: 0x0400040A RID: 1034
	[HideInInspector]
	public bool showComponentsSetup;

	// Token: 0x0400040B RID: 1035
	[HideInInspector]
	public bool showTimeUI;

	// Token: 0x0400040C RID: 1036
	[HideInInspector]
	public bool showWeatherUI;

	// Token: 0x0400040D RID: 1037
	[HideInInspector]
	public bool showAudioUI;

	// Token: 0x0400040E RID: 1038
	[HideInInspector]
	public bool showEffectsUI;

	// Token: 0x0400040F RID: 1039
	[HideInInspector]
	public bool modified;

	// Token: 0x0200008F RID: 143
	public enum settingsMode
	{
		// Token: 0x04000411 RID: 1041
		Lighting,
		// Token: 0x04000412 RID: 1042
		Sky,
		// Token: 0x04000413 RID: 1043
		Reflections,
		// Token: 0x04000414 RID: 1044
		Weather,
		// Token: 0x04000415 RID: 1045
		Season,
		// Token: 0x04000416 RID: 1046
		Clouds,
		// Token: 0x04000417 RID: 1047
		Fog,
		// Token: 0x04000418 RID: 1048
		VolumeLighting,
		// Token: 0x04000419 RID: 1049
		Lightshafts,
		// Token: 0x0400041A RID: 1050
		DistanceBlur,
		// Token: 0x0400041B RID: 1051
		Aurora,
		// Token: 0x0400041C RID: 1052
		Satellites,
		// Token: 0x0400041D RID: 1053
		Audio,
		// Token: 0x0400041E RID: 1054
		Quality
	}

	// Token: 0x02000090 RID: 144
	public enum settingsModeLW
	{
		// Token: 0x04000420 RID: 1056
		Lighting,
		// Token: 0x04000421 RID: 1057
		Sky,
		// Token: 0x04000422 RID: 1058
		Reflections,
		// Token: 0x04000423 RID: 1059
		Weather,
		// Token: 0x04000424 RID: 1060
		Season,
		// Token: 0x04000425 RID: 1061
		Clouds,
		// Token: 0x04000426 RID: 1062
		Fog,
		// Token: 0x04000427 RID: 1063
		Lightshafts,
		// Token: 0x04000428 RID: 1064
		Satellites,
		// Token: 0x04000429 RID: 1065
		Audio,
		// Token: 0x0400042A RID: 1066
		Quality
	}
}
