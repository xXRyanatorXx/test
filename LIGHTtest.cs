using System;
using UnityEngine;

// Token: 0x02000150 RID: 336
public class LIGHTtest : MonoBehaviour
{
	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000723 RID: 1827 RVA: 0x0003AF50 File Offset: 0x00039150
	// (set) Token: 0x06000724 RID: 1828 RVA: 0x0003AF58 File Offset: 0x00039158
	public bool IsOn { get; private set; }

	// Token: 0x06000725 RID: 1829 RVA: 0x0003AF64 File Offset: 0x00039164
	public virtual void TurnOff()
	{
		bool isOn = this.IsOn;
		if (this.type == LIGHTtest.LightType.Light && this.light != null)
		{
			this.light.enabled = false;
		}
		else if (Application.isPlaying)
		{
			if (this.meshRenderer == null || this.meshRenderer.material == null)
			{
				return;
			}
			this.meshRenderer.materials[this.rendererMaterialIndex].DisableKeyword("_EMISSION");
		}
		this.IsOn = false;
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x0003AFE8 File Offset: 0x000391E8
	public virtual void TurnOn()
	{
		bool isOn = this.IsOn;
		if (this.type == LIGHTtest.LightType.Light && this.light != null)
		{
			this.light.enabled = true;
		}
		else if (Application.isPlaying)
		{
			if (this.meshRenderer == null || this.meshRenderer.material == null)
			{
				return;
			}
			Material material = this.meshRenderer.materials[this.rendererMaterialIndex];
			material.EnableKeyword("_EMISSION");
			material.SetColor("_EmissionColor", this.emissionColor);
		}
		this.IsOn = true;
	}

	// Token: 0x04000B30 RID: 2864
	[ColorUsage(true, true)]
	[Tooltip("    Color of the emitted light.")]
	public Color emissionColor;

	// Token: 0x04000B31 RID: 2865
	[Tooltip("Light (point/spot/directional/etc.) representing the vehicle light. Will only be used if light type is set to\r\nLight.")]
	public Light light;

	// Token: 0x04000B32 RID: 2866
	[Tooltip("Mesh renderer using standard shader. Emission on the material will be turned on or off depending on light state.")]
	public MeshRenderer meshRenderer;

	// Token: 0x04000B33 RID: 2867
	[Tooltip("    If your mesh has more than one material set this number to the index of required material.")]
	public int rendererMaterialIndex;

	// Token: 0x04000B34 RID: 2868
	[Tooltip("    Type of the light.")]
	public LIGHTtest.LightType type;

	// Token: 0x02000151 RID: 337
	public enum LightType
	{
		// Token: 0x04000B37 RID: 2871
		Light,
		// Token: 0x04000B38 RID: 2872
		Mesh
	}
}
