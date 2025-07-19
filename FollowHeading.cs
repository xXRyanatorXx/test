using System;
using EVP;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class FollowHeading : MonoBehaviour
{
	// Token: 0x0600011E RID: 286 RVA: 0x0000A5E3 File Offset: 0x000087E3
	private void OnEnable()
	{
		this.m_vehicle = base.GetComponent<VehicleController>();
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0000A5F4 File Offset: 0x000087F4
	private void FixedUpdate()
	{
		float num = Mathf.Clamp(Mathf.DeltaAngle(base.transform.eulerAngles.y, this.heading) / this.m_vehicle.maxSteerAngle, -1f, 1f);
		this.m_vehicle.steerInput += num;
	}

	// Token: 0x0400018F RID: 399
	[Range(-180f, 180f)]
	public float heading;

	// Token: 0x04000190 RID: 400
	private VehicleController m_vehicle;
}
