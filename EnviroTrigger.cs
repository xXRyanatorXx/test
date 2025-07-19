using System;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class EnviroTrigger : MonoBehaviour
{
	// Token: 0x060003B9 RID: 953 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x060003BA RID: 954 RVA: 0x0000245B File Offset: 0x0000065B
	private void Update()
	{
	}

	// Token: 0x060003BB RID: 955 RVA: 0x0001981C File Offset: 0x00017A1C
	private void OnTriggerEnter(Collider col)
	{
		if (EnviroSkyMgr.instance.GetUseWeatherTag())
		{
			if (col.gameObject.tag == EnviroSkyMgr.instance.GetEnviroSkyTag())
			{
				this.EnterExit();
				return;
			}
		}
		else if (EnviroSkyMgr.instance.IsEnviroSkyAttached(col.gameObject))
		{
			this.EnterExit();
		}
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00019870 File Offset: 0x00017A70
	private void OnTriggerExit(Collider col)
	{
		if (this.myZone.zoneTriggerType == EnviroInterior.ZoneTriggerType.Zone)
		{
			if (EnviroSkyMgr.instance.GetUseWeatherTag())
			{
				if (col.gameObject.tag == EnviroSkyMgr.instance.GetEnviroSkyTag())
				{
					this.EnterExit();
					return;
				}
			}
			else if (EnviroSkyMgr.instance.IsEnviroSkyAttached(col.gameObject))
			{
				this.EnterExit();
			}
		}
	}

	// Token: 0x060003BD RID: 957 RVA: 0x000198D4 File Offset: 0x00017AD4
	private void EnterExit()
	{
		if (EnviroSkyMgr.instance.lastInteriorZone != this.myZone)
		{
			if (EnviroSkyMgr.instance.lastInteriorZone != null)
			{
				EnviroSkyMgr.instance.lastInteriorZone.StopAllFading();
			}
			this.myZone.Enter();
			return;
		}
		if (!EnviroSkyMgr.instance.IsInterior())
		{
			this.myZone.Enter();
			return;
		}
		this.myZone.Exit();
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00019948 File Offset: 0x00017B48
	private void OnDrawGizmos()
	{
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.color = new Color(0.2f, 0.2f, 1f, 0.5f);
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
	}

	// Token: 0x04000570 RID: 1392
	public EnviroInterior myZone;

	// Token: 0x04000571 RID: 1393
	public string Name;
}
