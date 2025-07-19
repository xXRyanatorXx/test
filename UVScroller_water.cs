using System;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class UVScroller_water : MonoBehaviour
{
	// Token: 0x06000125 RID: 293 RVA: 0x0000A738 File Offset: 0x00008938
	private void Start()
	{
		this.mat = base.gameObject.GetComponent<MeshRenderer>().material;
		foreach (Light light in UnityEngine.Object.FindObjectsOfType(typeof(Light)) as Light[])
		{
			if (light.type == LightType.Directional)
			{
				this.lightDir = light.transform;
				return;
			}
		}
	}

	// Token: 0x06000126 RID: 294 RVA: 0x0000A798 File Offset: 0x00008998
	private void Update()
	{
		float num = Time.time * this.scrollSpeed;
		this.mat.SetTextureOffset("_MainTex", new Vector2(num * 0.5f, num * 1f));
		this.mat.SetTextureOffset("_HeightTex", new Vector2(num / 2f, num));
		this.mat.SetTextureOffset("_FoamTex", new Vector2(num / 4f, num * 1f));
		if (this.lightDir)
		{
			this.mat.SetVector("_WorldLightDir", this.lightDir.forward);
			return;
		}
		this.mat.SetVector("_WorldLightDir", new Vector3(0.7f, 0.7f, 0f));
	}

	// Token: 0x04000199 RID: 409
	private Material mat;

	// Token: 0x0400019A RID: 410
	public float scrollSpeed = 0.02f;

	// Token: 0x0400019B RID: 411
	private Transform lightDir;
}
