using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public static class EnviroCloudsQualityCreation
{
	// Token: 0x0600046D RID: 1133 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static GameObject GetAssetPrefab(string name)
	{
		return null;
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static AudioClip GetAudioClip(string name)
	{
		return null;
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static Cubemap GetAssetCubemap(string name)
	{
		return null;
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static EnviroProfile GetDefaultProfile(string name)
	{
		return null;
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static Texture GetAssetTexture(string name)
	{
		return null;
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x000254DC File Offset: 0x000236DC
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

	// Token: 0x06000473 RID: 1139 RVA: 0x0002557C File Offset: 0x0002377C
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

	// Token: 0x06000474 RID: 1140 RVA: 0x00025628 File Offset: 0x00023828
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

	// Token: 0x06000475 RID: 1141 RVA: 0x000256D4 File Offset: 0x000238D4
	public static Color GetColor(string hex)
	{
		Color result = default(Color);
		ColorUtility.TryParseHtmlString(hex, out result);
		return result;
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x000256F4 File Offset: 0x000238F4
	public static Keyframe CreateKey(float value, float time)
	{
		return new Keyframe
		{
			value = value,
			time = time
		};
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0002571C File Offset: 0x0002391C
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
