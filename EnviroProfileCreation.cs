using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000091 RID: 145
public static class EnviroProfileCreation
{
	// Token: 0x0600022F RID: 559 RVA: 0x0001238C File Offset: 0x0001058C
	public static void SetupDefaults(EnviroProfile profile)
	{
		EnviroProfile defaultProfile = EnviroProfileCreation.GetDefaultProfile("enviro_internal_default_profile");
		profile.audioSettings = defaultProfile.audioSettings;
		profile.reflectionSettings = defaultProfile.reflectionSettings;
		profile.cloudsSettings = defaultProfile.cloudsSettings;
		profile.distanceBlurSettings = defaultProfile.distanceBlurSettings;
		profile.fogSettings = defaultProfile.fogSettings;
		profile.lightSettings = defaultProfile.lightSettings;
		profile.lightshaftsSettings = defaultProfile.lightshaftsSettings;
		profile.qualitySettings = defaultProfile.qualitySettings;
		profile.satelliteSettings = defaultProfile.satelliteSettings;
		profile.seasonsSettings = defaultProfile.seasonsSettings;
		profile.skySettings = defaultProfile.skySettings;
		profile.volumeLightSettings = defaultProfile.volumeLightSettings;
		profile.reflectionSettings = defaultProfile.reflectionSettings;
		profile.auroraSettings = defaultProfile.auroraSettings;
		profile.weatherSettings = defaultProfile.weatherSettings;
		profile.version = defaultProfile.version;
	}

	// Token: 0x06000230 RID: 560 RVA: 0x00012464 File Offset: 0x00010664
	public static bool UpdateProfile(EnviroProfile profile, string fromV, string toV)
	{
		if (profile == null)
		{
			return false;
		}
		EnviroProfile defaultProfile = EnviroProfileCreation.GetDefaultProfile("enviro_internal_default_profile");
		new List<Color>();
		new List<float>();
		if (fromV == "2.1.0" || fromV == "2.1.1" || (fromV == "2.1.2" && toV == "2.3.2"))
		{
			if (defaultProfile != null)
			{
				profile.cloudsSettings.hgPhase = defaultProfile.cloudsSettings.hgPhase;
				profile.cloudsSettings.silverLiningSpread = defaultProfile.cloudsSettings.silverLiningSpread;
				profile.cloudsSettings.silverLiningIntensity = defaultProfile.cloudsSettings.silverLiningIntensity;
				profile.cloudsSettings.lightIntensity = defaultProfile.cloudsSettings.lightIntensity;
				profile.cloudsSettings.attenuationClamp = defaultProfile.cloudsSettings.attenuationClamp;
				profile.cloudsSettings.volumeCloudsAmbientColor = defaultProfile.cloudsSettings.volumeCloudsAmbientColor;
				profile.cloudsSettings.ambientLightIntensity = defaultProfile.cloudsSettings.ambientLightIntensity;
				profile.auroraSettings.auroraIntensity = defaultProfile.auroraSettings.auroraIntensity;
				profile.cloudsSettings.ParticleCloudsLayer1.height = 0.01f;
				profile.cloudsSettings.ParticleCloudsLayer2.height = 0.01f;
				profile.cloudsSettings.flatCloudsBaseTexture = defaultProfile.cloudsSettings.flatCloudsBaseTexture;
				profile.cloudsSettings.flatCloudsDetailTexture = defaultProfile.cloudsSettings.flatCloudsDetailTexture;
				profile.cloudsSettings.flatCloudsDirectLightColor = defaultProfile.cloudsSettings.flatCloudsDirectLightColor;
				profile.cloudsSettings.flatCloudsAmbientLightColor = defaultProfile.cloudsSettings.flatCloudsAmbientLightColor;
				profile.skySettings.moonTexture = defaultProfile.skySettings.moonTexture;
				profile.skySettings.dithering = 0.015f;
				profile.fogSettings.fogDithering = 0.015f;
				profile.lightSettings.sunIntensityLux = defaultProfile.lightSettings.sunIntensityLux;
				profile.lightSettings.moonIntensityLux = defaultProfile.lightSettings.sunIntensityLux;
				profile.lightSettings.exposurePhysical = defaultProfile.lightSettings.exposurePhysical;
				profile.lightSettings.skyExposurePhysical = defaultProfile.lightSettings.skyExposurePhysical;
				profile.lightSettings.lightColorTemperature = defaultProfile.lightSettings.lightColorTemperature;
				profile.lightSettings.lightColorTint = defaultProfile.lightSettings.lightColorTint;
				profile.fogSettings.fogColorTint = defaultProfile.fogSettings.fogColorTint;
				profile.version = toV;
				return true;
			}
			return false;
		}
		else if (fromV == "2.1.3" || fromV == "2.1.4" || (fromV == "2.1.5" && toV == "2.3.2"))
		{
			if (defaultProfile != null)
			{
				profile.skySettings.moonTexture = defaultProfile.skySettings.moonTexture;
				profile.cloudsSettings.hgPhase = defaultProfile.cloudsSettings.hgPhase;
				profile.cloudsSettings.silverLiningSpread = defaultProfile.cloudsSettings.silverLiningSpread;
				profile.cloudsSettings.silverLiningIntensity = defaultProfile.cloudsSettings.silverLiningIntensity;
				profile.cloudsSettings.lightIntensity = defaultProfile.cloudsSettings.lightIntensity;
				profile.cloudsSettings.attenuationClamp = defaultProfile.cloudsSettings.attenuationClamp;
				profile.cloudsSettings.volumeCloudsAmbientColor = defaultProfile.cloudsSettings.volumeCloudsAmbientColor;
				profile.cloudsSettings.ParticleCloudsLayer1.height = 0.01f;
				profile.cloudsSettings.ParticleCloudsLayer2.height = 0.01f;
				profile.cloudsSettings.flatCloudsBaseTexture = defaultProfile.cloudsSettings.flatCloudsBaseTexture;
				profile.cloudsSettings.flatCloudsDetailTexture = defaultProfile.cloudsSettings.flatCloudsDetailTexture;
				profile.cloudsSettings.flatCloudsDirectLightColor = defaultProfile.cloudsSettings.flatCloudsDirectLightColor;
				profile.cloudsSettings.flatCloudsAmbientLightColor = defaultProfile.cloudsSettings.flatCloudsAmbientLightColor;
				profile.skySettings.moonTexture = defaultProfile.skySettings.moonTexture;
				profile.skySettings.dithering = 0.015f;
				profile.fogSettings.fogDithering = 0.015f;
				profile.lightSettings.sunIntensityLux = defaultProfile.lightSettings.sunIntensityLux;
				profile.lightSettings.moonIntensityLux = defaultProfile.lightSettings.sunIntensityLux;
				profile.lightSettings.exposurePhysical = defaultProfile.lightSettings.exposurePhysical;
				profile.lightSettings.skyExposurePhysical = defaultProfile.lightSettings.skyExposurePhysical;
				profile.lightSettings.lightColorTemperature = defaultProfile.lightSettings.lightColorTemperature;
				profile.lightSettings.lightColorTint = defaultProfile.lightSettings.lightColorTint;
				profile.fogSettings.fogColorTint = defaultProfile.fogSettings.fogColorTint;
				profile.version = toV;
				return true;
			}
			return false;
		}
		else if (fromV == "2.2.0" || fromV == "2.2.1" || (fromV == "2.2.2" && toV == "2.3.2"))
		{
			if (defaultProfile != null)
			{
				profile.cloudsSettings.flatCloudsBaseTexture = defaultProfile.cloudsSettings.flatCloudsBaseTexture;
				profile.cloudsSettings.flatCloudsDetailTexture = defaultProfile.cloudsSettings.flatCloudsDetailTexture;
				profile.cloudsSettings.flatCloudsDirectLightColor = defaultProfile.cloudsSettings.flatCloudsDirectLightColor;
				profile.cloudsSettings.flatCloudsAmbientLightColor = defaultProfile.cloudsSettings.flatCloudsAmbientLightColor;
				profile.skySettings.moonTexture = defaultProfile.skySettings.moonTexture;
				profile.skySettings.dithering = 0.015f;
				profile.fogSettings.fogDithering = 0.015f;
				profile.lightSettings.sunIntensityLux = defaultProfile.lightSettings.sunIntensityLux;
				profile.lightSettings.moonIntensityLux = defaultProfile.lightSettings.sunIntensityLux;
				profile.lightSettings.exposurePhysical = defaultProfile.lightSettings.exposurePhysical;
				profile.lightSettings.skyExposurePhysical = defaultProfile.lightSettings.skyExposurePhysical;
				profile.lightSettings.lightColorTemperature = defaultProfile.lightSettings.lightColorTemperature;
				profile.lightSettings.lightColorTint = defaultProfile.lightSettings.lightColorTint;
				profile.fogSettings.fogColorTint = defaultProfile.fogSettings.fogColorTint;
				profile.version = toV;
				return true;
			}
			return false;
		}
		else if (fromV == "2.3.0" && toV == "2.3.2")
		{
			if (defaultProfile != null)
			{
				profile.cloudsSettings.flatCloudsBaseTexture = defaultProfile.cloudsSettings.flatCloudsBaseTexture;
				profile.cloudsSettings.flatCloudsDetailTexture = defaultProfile.cloudsSettings.flatCloudsDetailTexture;
				profile.cloudsSettings.flatCloudsDirectLightColor = defaultProfile.cloudsSettings.flatCloudsDirectLightColor;
				profile.cloudsSettings.flatCloudsAmbientLightColor = defaultProfile.cloudsSettings.flatCloudsAmbientLightColor;
				profile.skySettings.moonTexture = defaultProfile.skySettings.moonTexture;
				profile.skySettings.dithering = 0.015f;
				profile.fogSettings.fogDithering = 0.015f;
				profile.lightSettings.sunIntensityLux = defaultProfile.lightSettings.sunIntensityLux;
				profile.lightSettings.moonIntensityLux = defaultProfile.lightSettings.sunIntensityLux;
				profile.lightSettings.exposurePhysical = defaultProfile.lightSettings.exposurePhysical;
				profile.lightSettings.skyExposurePhysical = defaultProfile.lightSettings.skyExposurePhysical;
				profile.lightSettings.lightColorTemperature = defaultProfile.lightSettings.lightColorTemperature;
				profile.lightSettings.lightColorTint = defaultProfile.lightSettings.lightColorTint;
				profile.fogSettings.fogColorTint = defaultProfile.fogSettings.fogColorTint;
				profile.version = toV;
				return true;
			}
			return false;
		}
		else
		{
			if (!(fromV == "2.3.1") || !(toV == "2.3.2"))
			{
				return false;
			}
			if (defaultProfile != null)
			{
				profile.lightSettings.sunIntensityLux = defaultProfile.lightSettings.sunIntensityLux;
				profile.lightSettings.moonIntensityLux = defaultProfile.lightSettings.sunIntensityLux;
				profile.lightSettings.exposurePhysical = defaultProfile.lightSettings.exposurePhysical;
				profile.lightSettings.skyExposurePhysical = defaultProfile.lightSettings.skyExposurePhysical;
				profile.lightSettings.lightColorTemperature = defaultProfile.lightSettings.lightColorTemperature;
				profile.lightSettings.lightColorTint = defaultProfile.lightSettings.lightColorTint;
				profile.fogSettings.fogColorTint = defaultProfile.fogSettings.fogColorTint;
				profile.version = toV;
				return true;
			}
			return false;
		}
	}

	// Token: 0x06000231 RID: 561 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static GameObject GetAssetPrefab(string name)
	{
		return null;
	}

	// Token: 0x06000232 RID: 562 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static AudioClip GetAudioClip(string name)
	{
		return null;
	}

	// Token: 0x06000233 RID: 563 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static Cubemap GetAssetCubemap(string name)
	{
		return null;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static EnviroProfile GetDefaultProfile(string name)
	{
		return null;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static Texture GetAssetTexture(string name)
	{
		return null;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x00012CD0 File Offset: 0x00010ED0
	public static Gradient CreateGradient(Color clr1, float time1, Color clr2, float time2)
	{
		Gradient gradient = new Gradient();
		GradientColorKey[] array = new GradientColorKey[2];
		GradientAlphaKey[] array2 = new GradientAlphaKey[2];
		array[0].color = clr1;
		array[0].time = time1;
		array[1].color = clr2;
		array[1].time = time2;
		array2[0].alpha = 1f;
		array2[0].time = 0f;
		array2[1].alpha = 1f;
		array2[1].time = 1f;
		gradient.SetKeys(array, array2);
		return gradient;
	}

	// Token: 0x06000237 RID: 567 RVA: 0x00012D70 File Offset: 0x00010F70
	public static Gradient CreateGradient(List<Color> clrs, List<float> times)
	{
		Gradient gradient = new Gradient();
		GradientColorKey[] array = new GradientColorKey[clrs.Count];
		GradientAlphaKey[] array2 = new GradientAlphaKey[2];
		for (int i = 0; i < clrs.Count; i++)
		{
			array[i].color = clrs[i];
			array[i].time = times[i];
		}
		array2[0].alpha = 1f;
		array2[0].time = 0f;
		array2[1].alpha = 1f;
		array2[1].time = 1f;
		gradient.SetKeys(array, array2);
		return gradient;
	}

	// Token: 0x06000238 RID: 568 RVA: 0x00012E1C File Offset: 0x0001101C
	public static Gradient CreateGradient(List<Color> clrs, List<float> times, List<float> alpha, List<float> timesAlpha)
	{
		Gradient gradient = new Gradient();
		GradientColorKey[] array = new GradientColorKey[clrs.Count];
		GradientAlphaKey[] array2 = new GradientAlphaKey[alpha.Count];
		for (int i = 0; i < clrs.Count; i++)
		{
			array[i].color = clrs[i];
			array[i].time = times[i];
		}
		for (int j = 0; j < alpha.Count; j++)
		{
			array2[j].alpha = alpha[j];
			array2[j].time = timesAlpha[j];
		}
		gradient.SetKeys(array, array2);
		return gradient;
	}

	// Token: 0x06000239 RID: 569 RVA: 0x00012EC8 File Offset: 0x000110C8
	public static Color GetColor(string hex)
	{
		Color result = default(Color);
		ColorUtility.TryParseHtmlString(hex, out result);
		return result;
	}

	// Token: 0x0600023A RID: 570 RVA: 0x00012EE8 File Offset: 0x000110E8
	public static Keyframe CreateKey(float value, float time)
	{
		return new Keyframe
		{
			value = value,
			time = time
		};
	}

	// Token: 0x0600023B RID: 571 RVA: 0x00012F10 File Offset: 0x00011110
	public static Keyframe CreateKey(float value, float time, float inTangent, float outTangent)
	{
		return new Keyframe
		{
			value = value,
			time = time,
			inTangent = inTangent,
			outTangent = outTangent
		};
	}
}
