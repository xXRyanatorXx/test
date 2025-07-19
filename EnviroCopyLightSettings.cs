using System;
using UnityEngine;

// Token: 0x020000B6 RID: 182
[ExecuteInEditMode]
public class EnviroCopyLightSettings : MonoBehaviour
{
	// Token: 0x06000394 RID: 916 RVA: 0x00018798 File Offset: 0x00016998
	private void OnEnable()
	{
		if (EnviroSkyMgr.instance != null)
		{
			this.enviroLight = EnviroSkyMgr.instance.Components.DirectLight.GetComponent<Light>();
		}
		if (this.myLight == null)
		{
			this.myLight = base.GetComponent<Light>();
		}
	}

	// Token: 0x06000395 RID: 917 RVA: 0x000187E8 File Offset: 0x000169E8
	private void Update()
	{
		if (EnviroSkyMgr.instance != null && this.enviroLight != null && this.myLight != null)
		{
			this.myLight.transform.position = EnviroSkyMgr.instance.Components.DirectLight.position;
			this.myLight.transform.rotation = EnviroSkyMgr.instance.Components.DirectLight.rotation;
			this.myLight.color = this.enviroLight.color;
			this.myLight.intensity = this.enviroLight.intensity;
			this.myLight.shadowStrength = this.enviroLight.shadowStrength;
		}
	}

	// Token: 0x0400052D RID: 1325
	public Light myLight;

	// Token: 0x0400052E RID: 1326
	private Light enviroLight;
}
