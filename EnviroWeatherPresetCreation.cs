using System;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class EnviroWeatherPresetCreation
{
	// Token: 0x0600036B RID: 875 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static GameObject GetAssetPrefab(string name)
	{
		return null;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static Cubemap GetAssetCubemap(string name)
	{
		return null;
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00012CCD File Offset: 0x00010ECD
	public static Texture GetAssetTexture(string name)
	{
		return null;
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00017238 File Offset: 0x00015438
	public static Gradient CreateGradient()
	{
		Gradient gradient = new Gradient();
		GradientColorKey[] array = new GradientColorKey[2];
		GradientAlphaKey[] array2 = new GradientAlphaKey[2];
		array[0].color = Color.white;
		array[0].time = 0f;
		array[1].color = Color.white;
		array[1].time = 0f;
		array2[0].alpha = 0f;
		array2[0].time = 0f;
		array2[1].alpha = 0f;
		array2[1].time = 1f;
		gradient.SetKeys(array, array2);
		return gradient;
	}

	// Token: 0x0600036F RID: 879 RVA: 0x000172E8 File Offset: 0x000154E8
	public static Color GetColor(string hex)
	{
		Color result = default(Color);
		ColorUtility.TryParseHtmlString(hex, out result);
		return result;
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00017308 File Offset: 0x00015508
	public static Keyframe CreateKey(float value, float time)
	{
		return new Keyframe
		{
			value = value,
			time = time
		};
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00017330 File Offset: 0x00015530
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
