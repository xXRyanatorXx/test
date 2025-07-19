using System;
using UnityEngine;

// Token: 0x02000242 RID: 578
[ExecuteInEditMode]
public class PW_Camera_Depth : MonoBehaviour
{
	// Token: 0x06000DB9 RID: 3513 RVA: 0x00092F7C File Offset: 0x0009117C
	private void Start()
	{
		Camera.main.depthTextureMode = DepthTextureMode.Depth;
		Vector3 vector;
		vector.x = -this.mainLight.transform.forward.x;
		vector.y = -this.mainLight.transform.forward.y;
		vector.z = -this.mainLight.transform.forward.z;
		if (this.waterGO != null)
		{
			this._mat = this.waterGO.GetComponent<Renderer>().sharedMaterial;
		}
		if (this._mat != null)
		{
			this._mat.SetVector("_PW_MainLightDir", -this.mainLight.transform.forward);
			this._mat.SetVector("_PW_MainLightColor", this.mainLight.color * this.mainLight.intensity);
		}
		Shader.SetGlobalVector("_PW_MainLightDir", -this.mainLight.transform.forward);
		Shader.SetGlobalVector("_PW_MainLightColor", this.mainLight.color * this.mainLight.intensity);
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x000930C8 File Offset: 0x000912C8
	private void Update()
	{
		Vector3 vector;
		vector.x = -this.mainLight.transform.forward.x;
		vector.y = -this.mainLight.transform.forward.y;
		vector.z = -this.mainLight.transform.forward.z;
		if (this._mat != null)
		{
			this._mat.SetVector("_PW_MainLightDir", -this.mainLight.transform.forward);
			this._mat.SetVector("_PW_MainLightColor", this.mainLight.color * this.mainLight.intensity);
		}
		Shader.SetGlobalVector("_PW_MainLightDir", -this.mainLight.transform.forward);
		Shader.SetGlobalVector("_PW_MainLightColor", this.mainLight.color * this.mainLight.intensity);
	}

	// Token: 0x04001648 RID: 5704
	private Material _mat;

	// Token: 0x04001649 RID: 5705
	private Mesh _mesh;

	// Token: 0x0400164A RID: 5706
	public Light mainLight;

	// Token: 0x0400164B RID: 5707
	public GameObject waterGO;
}
