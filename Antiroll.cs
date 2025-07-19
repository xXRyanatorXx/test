using System;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class Antiroll : MonoBehaviour
{
	// Token: 0x0600062B RID: 1579 RVA: 0x00030FFE File Offset: 0x0002F1FE
	private void Start()
	{
		this.carRigidBody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0003100C File Offset: 0x0002F20C
	private void FixedUpdate()
	{
		WheelHit wheelHit = default(WheelHit);
		float num = 1f;
		float num2 = 1f;
		bool groundHit = this.WheelL.GetGroundHit(out wheelHit);
		if (groundHit)
		{
			num = (-this.WheelL.transform.InverseTransformPoint(wheelHit.point).y - this.WheelL.radius) / this.WheelL.suspensionDistance;
		}
		bool groundHit2 = this.WheelR.GetGroundHit(out wheelHit);
		if (groundHit2)
		{
			num2 = (-this.WheelR.transform.InverseTransformPoint(wheelHit.point).y - this.WheelR.radius) / this.WheelR.suspensionDistance;
		}
		float num3 = (num - num2) * this.AntiRoll;
		if (groundHit)
		{
			this.carRigidBody.AddForceAtPosition(this.WheelL.transform.up * -num3, this.WheelL.transform.position);
		}
		if (groundHit2)
		{
			this.carRigidBody.AddForceAtPosition(this.WheelR.transform.up * num3, this.WheelR.transform.position);
		}
	}

	// Token: 0x0400093C RID: 2364
	public WheelCollider WheelL;

	// Token: 0x0400093D RID: 2365
	public WheelCollider WheelR;

	// Token: 0x0400093E RID: 2366
	private Rigidbody carRigidBody;

	// Token: 0x0400093F RID: 2367
	public float AntiRoll = 5000f;
}
