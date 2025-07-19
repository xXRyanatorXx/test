using System;
using UnityEngine;

// Token: 0x02000269 RID: 617
public class CoatingRandomizer : MonoBehaviour
{
	// Token: 0x06000EAD RID: 3757 RVA: 0x0009B954 File Offset: 0x00099B54
	private void Awake()
	{
		this.propertyBlock = new MaterialPropertyBlock();
		this.rendererComp = base.GetComponent<Renderer>();
		this.rendererComp.GetPropertyBlock(this.propertyBlock);
		if (this.shaderType == ShaderType.Coating2Layers)
		{
			this.layer2Intensity = "_MaskIntensityOffset";
			this.layer2Contrast = "_MaskContrastOffset";
			if (this.bRandomizeLayer2Intensity)
			{
				this.propertyBlock.SetFloat(this.layer2Intensity, UnityEngine.Random.Range(this.layer2IntensityMin, this.layer2IntensityMax));
			}
			if (this.bRandomizeLayer2Contrast)
			{
				this.propertyBlock.SetFloat(this.layer2Contrast, UnityEngine.Random.Range(this.layer2ContrastMin, this.layer2ContrastMax));
			}
		}
		else if (this.shaderType == ShaderType.Coating3Layers)
		{
			this.layer2Intensity = "_L2MaskIntensityOffset";
			this.layer2Contrast = "_L2MaskContrastOffset";
			this.layer3Intensity = "_L3MaskIntensityOffset";
			this.layer3Contrast = "_L3MaskContrastOffset";
			if (this.bRandomizeLayer2Intensity)
			{
				this.propertyBlock.SetFloat(this.layer2Intensity, UnityEngine.Random.Range(this.layer2IntensityMin, this.layer2IntensityMax));
			}
			if (this.bRandomizeLayer2Contrast)
			{
				this.propertyBlock.SetFloat(this.layer2Contrast, UnityEngine.Random.Range(this.layer2ContrastMin, this.layer2ContrastMax));
			}
			if (this.bRandomizeLayer3Intensity)
			{
				this.propertyBlock.SetFloat(this.layer3Intensity, UnityEngine.Random.Range(this.layer3IntensityMin, this.layer3IntensityMax));
			}
			if (this.bRandomizeLayer3Contrast)
			{
				this.propertyBlock.SetFloat(this.layer3Contrast, UnityEngine.Random.Range(this.layer3ContrastMin, this.layer3ContrastMax));
			}
		}
		this.rendererComp.SetPropertyBlock(this.propertyBlock);
	}

	// Token: 0x040017D3 RID: 6099
	public ShaderType shaderType;

	// Token: 0x040017D4 RID: 6100
	public bool bRandomizeLayer2Intensity;

	// Token: 0x040017D5 RID: 6101
	public bool bRandomizeLayer2Contrast;

	// Token: 0x040017D6 RID: 6102
	public bool bRandomizeLayer3Intensity;

	// Token: 0x040017D7 RID: 6103
	public bool bRandomizeLayer3Contrast;

	// Token: 0x040017D8 RID: 6104
	[Range(1f, 10f)]
	public float test = 1f;

	// Token: 0x040017D9 RID: 6105
	public float layer2IntensityMin;

	// Token: 0x040017DA RID: 6106
	public float layer2IntensityMax = 10f;

	// Token: 0x040017DB RID: 6107
	public float layer2ContrastMin = 0.1f;

	// Token: 0x040017DC RID: 6108
	public float layer2ContrastMax = 10f;

	// Token: 0x040017DD RID: 6109
	public float layer3IntensityMin;

	// Token: 0x040017DE RID: 6110
	public float layer3IntensityMax = 10f;

	// Token: 0x040017DF RID: 6111
	public float layer3ContrastMin = 0.1f;

	// Token: 0x040017E0 RID: 6112
	public float layer3ContrastMax = 10f;

	// Token: 0x040017E1 RID: 6113
	private string layer2Intensity = "_L2MaskIntensityOffset";

	// Token: 0x040017E2 RID: 6114
	private string layer2Contrast = "_L2MaskContrastOffset";

	// Token: 0x040017E3 RID: 6115
	private string layer3Intensity = "_L3MaskIntensityOffset";

	// Token: 0x040017E4 RID: 6116
	private string layer3Contrast = "_L3MaskContrastOffset";

	// Token: 0x040017E5 RID: 6117
	private Renderer rendererComp;

	// Token: 0x040017E6 RID: 6118
	private MaterialPropertyBlock propertyBlock;
}
