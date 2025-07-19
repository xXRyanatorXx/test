using System;
using UnityEngine;

// Token: 0x020000CF RID: 207
[Serializable]
public class EnviroVolumeCloudsQualitySettings
{
	// Token: 0x0400061F RID: 1567
	[Header("Clouds Height Settings")]
	[Tooltip("Clouds start height.")]
	public float bottomCloudHeight = 3000f;

	// Token: 0x04000620 RID: 1568
	[Tooltip("Clouds end height.")]
	public float topCloudHeight = 7000f;

	// Token: 0x04000621 RID: 1569
	[Header("Raymarch Step Settings")]
	[Range(32f, 256f)]
	[Tooltip("Number of raymarching samples.")]
	public int raymarchSteps = 150;

	// Token: 0x04000622 RID: 1570
	[Tooltip("Increase performance by using less steps when clouds are hidden by objects.")]
	[Range(0.1f, 1f)]
	public float stepsInDepthModificator = 0.75f;

	// Token: 0x04000623 RID: 1571
	[Tooltip("Increase performance by using early exit expensive raymarching. Higher values = more performant but less accurate lighting.")]
	[Range(0f, 0.5f)]
	public float transmissionToExit = 0.05f;

	// Token: 0x04000624 RID: 1572
	[Range(1f, 8f)]
	[Header("Resolution, Upsample and Reprojection")]
	[Tooltip("Downsampling of clouds rendering. 1 = full res, 2 = half Res, ...")]
	public int cloudsRenderResolution = 1;

	// Token: 0x04000625 RID: 1573
	public EnviroVolumeCloudsQualitySettings.ReprojectionPixelSize reprojectionPixelSize;

	// Token: 0x04000626 RID: 1574
	[Header("Clouds Modelling")]
	[Tooltip("LOD Distance for using lower res 3d texture for far away clouds. ")]
	[Range(0f, 1f)]
	public float lodDistance = 0.5f;

	// Token: 0x04000627 RID: 1575
	[Tooltip("The UV scale of base noise. High Values = Low performance!")]
	[Range(2f, 100f)]
	public float baseNoiseUV = 20f;

	// Token: 0x04000628 RID: 1576
	[Tooltip("The UV scale of detail noise. High Values = Low performance!")]
	[Range(2f, 100f)]
	public float detailNoiseUV = 50f;

	// Token: 0x04000629 RID: 1577
	[Tooltip("Enable to use a curl noise to further enhance the detail erode.")]
	public bool useCurlNoise;

	// Token: 0x0400062A RID: 1578
	[Tooltip("Resolution of Base Noise Texture.")]
	public EnviroVolumeCloudsQualitySettings.CloudDetailQuality baseQuality;

	// Token: 0x0400062B RID: 1579
	[Tooltip("Resolution of Detail Noise Texture.")]
	public EnviroVolumeCloudsQualitySettings.CloudDetailQuality detailQuality;

	// Token: 0x020000D0 RID: 208
	public enum ReprojectionPixelSize
	{
		// Token: 0x0400062D RID: 1581
		Off,
		// Token: 0x0400062E RID: 1582
		Low,
		// Token: 0x0400062F RID: 1583
		Medium,
		// Token: 0x04000630 RID: 1584
		High
	}

	// Token: 0x020000D1 RID: 209
	public enum CloudDetailQuality
	{
		// Token: 0x04000632 RID: 1586
		Low,
		// Token: 0x04000633 RID: 1587
		High
	}
}
