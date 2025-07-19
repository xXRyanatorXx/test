using System;
using UnityEngine;

// Token: 0x0200008D RID: 141
[Serializable]
public class EnviroAuroraSettings
{
	// Token: 0x040003EC RID: 1004
	public AnimationCurve auroraIntensity = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 1f),
		new Keyframe(0.5f, 0.1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x040003ED RID: 1005
	[Header("Aurora Color and Brightness")]
	public Color auroraColor = new Color(0.1f, 0.5f, 0.7f);

	// Token: 0x040003EE RID: 1006
	public float auroraBrightness = 75f;

	// Token: 0x040003EF RID: 1007
	public float auroraContrast = 10f;

	// Token: 0x040003F0 RID: 1008
	[Header("Aurora Height and Scale")]
	public float auroraHeight = 20000f;

	// Token: 0x040003F1 RID: 1009
	[Range(0f, 0.025f)]
	public float auroraScale = 0.01f;

	// Token: 0x040003F2 RID: 1010
	[Header("Aurora Performance")]
	[Range(8f, 32f)]
	public int auroraSteps = 20;

	// Token: 0x040003F3 RID: 1011
	[Header("Aurora Modelling and Animation")]
	public Vector4 auroraLayer1Settings = new Vector4(0.1f, 0.1f, 0f, 0.5f);

	// Token: 0x040003F4 RID: 1012
	public Vector4 auroraLayer2Settings = new Vector4(5f, 5f, 0f, 0.5f);

	// Token: 0x040003F5 RID: 1013
	public Vector4 auroraColorshiftSettings = new Vector4(0.05f, 0.05f, 0f, 5f);

	// Token: 0x040003F6 RID: 1014
	[Range(0f, 0.1f)]
	public float auroraSpeed = 0.005f;
}
