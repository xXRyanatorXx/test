using System;
using UnityEngine;

// Token: 0x02000276 RID: 630
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Brightness")]
public class Brightness : MonoBehaviour
{
	// Token: 0x06000EF1 RID: 3825 RVA: 0x0009D845 File Offset: 0x0009BA45
	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (!this.shaderDerp || !this.shaderDerp.isSupported)
		{
			base.enabled = false;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x0009D877 File Offset: 0x0009BA77
	private Material material
	{
		get
		{
			if (this.m_Material == null)
			{
				this.m_Material = new Material(this.shaderDerp);
				this.m_Material.hideFlags = HideFlags.HideAndDontSave;
			}
			return this.m_Material;
		}
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x0009D8AB File Offset: 0x0009BAAB
	private void OnDisable()
	{
		if (this.m_Material)
		{
			UnityEngine.Object.DestroyImmediate(this.m_Material);
		}
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x0009D8C5 File Offset: 0x0009BAC5
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.material.SetFloat("_Brightness", this.brightness);
		Graphics.Blit(source, destination, this.material);
	}

	// Token: 0x04001828 RID: 6184
	public Shader shaderDerp;

	// Token: 0x04001829 RID: 6185
	private Material m_Material;

	// Token: 0x0400182A RID: 6186
	[Range(0.5f, 2f)]
	public float brightness = 1f;
}
