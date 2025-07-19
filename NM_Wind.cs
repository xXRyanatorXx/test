using System;
using UnityEngine;

// Token: 0x0200022B RID: 555
[ExecuteInEditMode]
public class NM_Wind : MonoBehaviour
{
	// Token: 0x06000CC2 RID: 3266 RVA: 0x0008D145 File Offset: 0x0008B345
	private void Start()
	{
		this.ApplySettings();
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x0008D145 File Offset: 0x0008B345
	private void Update()
	{
		this.ApplySettings();
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x0008D145 File Offset: 0x0008B345
	private void OnValidate()
	{
		this.ApplySettings();
	}

	// Token: 0x06000CC5 RID: 3269 RVA: 0x0008D150 File Offset: 0x0008B350
	private void ApplySettings()
	{
		Shader.SetGlobalTexture("WIND_SETTINGS_TexNoise", this.NoiseTexture);
		Shader.SetGlobalTexture("WIND_SETTINGS_TexGust", this.GustMaskTexture);
		Shader.SetGlobalVector("WIND_SETTINGS_WorldDirectionAndSpeed", this.GetDirectionAndSpeed());
		Shader.SetGlobalFloat("WIND_SETTINGS_FlexNoiseScale", 1f / Mathf.Max(0.01f, this.FlexNoiseWorldSize));
		Shader.SetGlobalFloat("WIND_SETTINGS_ShiverNoiseScale", 1f / Mathf.Max(0.01f, this.ShiverNoiseWorldSize));
		Shader.SetGlobalFloat("WIND_SETTINGS_Turbulence", this.WindSpeed * this.Turbulence);
		Shader.SetGlobalFloat("WIND_SETTINGS_GustSpeed", this.GustSpeed);
		Shader.SetGlobalFloat("WIND_SETTINGS_GustScale", this.GustScale);
		Shader.SetGlobalFloat("WIND_SETTINGS_GustWorldScale", 1f / Mathf.Max(0.01f, this.GustWorldSize));
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x0008D224 File Offset: 0x0008B424
	private Vector4 GetDirectionAndSpeed()
	{
		Vector3 normalized = base.transform.forward.normalized;
		return new Vector4(normalized.x, normalized.y, normalized.z, this.WindSpeed * 0.2777f);
	}

	// Token: 0x0400159C RID: 5532
	[Header("General Parameters")]
	[Tooltip("Wind Speed in Kilometers per hour")]
	public float WindSpeed = 30f;

	// Token: 0x0400159D RID: 5533
	[Range(0f, 2f)]
	[Tooltip("Wind Turbulence in percentage of wind Speed")]
	public float Turbulence = 0.25f;

	// Token: 0x0400159E RID: 5534
	[Header("Noise Parameters")]
	[Tooltip("Texture used for wind turbulence")]
	public Texture2D NoiseTexture;

	// Token: 0x0400159F RID: 5535
	[Tooltip("Size of one world tiling patch of the Noise Texture, for bending trees")]
	public float FlexNoiseWorldSize = 175f;

	// Token: 0x040015A0 RID: 5536
	[Tooltip("Size of one world tiling patch of the Noise Texture, for leaf shivering")]
	public float ShiverNoiseWorldSize = 10f;

	// Token: 0x040015A1 RID: 5537
	[Header("Gust Parameters")]
	[Tooltip("Texture used for wind gusts")]
	public Texture2D GustMaskTexture;

	// Token: 0x040015A2 RID: 5538
	[Tooltip("Size of one world tiling patch of the Gust Texture, for leaf shivering")]
	public float GustWorldSize = 600f;

	// Token: 0x040015A3 RID: 5539
	[Tooltip("Wind Gust Speed in Kilometers per hour")]
	public float GustSpeed = 50f;

	// Token: 0x040015A4 RID: 5540
	[Tooltip("Wind Gust Influence on trees")]
	public float GustScale = 1f;
}
