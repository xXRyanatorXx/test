using System;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class rocene : MonoBehaviour
{
	// Token: 0x0600063E RID: 1598 RVA: 0x000317F7 File Offset: 0x0002F9F7
	private void Start()
	{
		this.wheel = base.GetComponent<WheelCollider>();
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x00031808 File Offset: 0x0002FA08
	private void FixedUpdate()
	{
		WheelHit wheelHit = default(WheelHit);
		WheelCollider component = base.GetComponent<WheelCollider>();
		if (component.GetGroundHit(out wheelHit))
		{
			WheelFrictionCurve sidewaysFriction = component.sidewaysFriction;
			sidewaysFriction.extremumSlip = 0.2f;
			component.sidewaysFriction = sidewaysFriction;
		}
		if (component.GetGroundHit(out wheelHit) && component.GetGroundHit(out wheelHit))
		{
			WheelFrictionCurve sidewaysFriction2 = component.sidewaysFriction;
			sidewaysFriction2.stiffness = 1f;
			component.sidewaysFriction = sidewaysFriction2;
		}
		if (Input.GetButton("Jump"))
		{
			WheelFrictionCurve sidewaysFriction3 = component.sidewaysFriction;
			sidewaysFriction3.stiffness = 0.1f;
			component.sidewaysFriction = sidewaysFriction3;
		}
	}

	// Token: 0x04000968 RID: 2408
	public WheelCollider wheel;
}
