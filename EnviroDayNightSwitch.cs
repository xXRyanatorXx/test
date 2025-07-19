using System;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class EnviroDayNightSwitch : MonoBehaviour
{
	// Token: 0x06000397 RID: 919 RVA: 0x000188B4 File Offset: 0x00016AB4
	private void Start()
	{
		this.lightsArray = base.GetComponentsInChildren<Light>();
		EnviroSkyMgr.instance.OnDayTime += delegate()
		{
			this.Deactivate();
		};
		EnviroSkyMgr.instance.OnNightTime += delegate()
		{
			this.Activate();
		};
		if (EnviroSkyMgr.instance.IsNight())
		{
			this.Activate();
			return;
		}
		this.Deactivate();
	}

	// Token: 0x06000398 RID: 920 RVA: 0x00018914 File Offset: 0x00016B14
	private void Activate()
	{
		for (int i = 0; i < this.lightsArray.Length; i++)
		{
			this.lightsArray[i].enabled = true;
		}
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00018944 File Offset: 0x00016B44
	private void Deactivate()
	{
		for (int i = 0; i < this.lightsArray.Length; i++)
		{
			this.lightsArray[i].enabled = false;
		}
	}

	// Token: 0x0400052F RID: 1327
	private Light[] lightsArray;
}
