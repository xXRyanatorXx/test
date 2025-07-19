using System;
using NWH.VehiclePhysics2;
using UnityEngine;

// Token: 0x0200016A RID: 362
public class Getcomponent : MonoBehaviour
{
	// Token: 0x060007D2 RID: 2002 RVA: 0x00044704 File Offset: 0x00042904
	private void Start()
	{
		this.exp = base.GetComponent<VehicleController>();
		this.exp.powertrain.engine.maxPower = 180f;
		this.exp.powertrain.engine.maxRPM = 7000f;
		this.exp.powertrain.engine.idleRPM = 900f;
		this.exp.powertrain.engine.revLimiterEnabled = true;
		this.exp.powertrain.engine.revLimiterRPM = 6500f;
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0000245B File Offset: 0x0000065B
	private void Update()
	{
	}

	// Token: 0x04000EC3 RID: 3779
	public VehicleController exp;
}
