using System;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class slip : MonoBehaviour
{
	// Token: 0x06000641 RID: 1601 RVA: 0x0003189E File Offset: 0x0002FA9E
	private void Start()
	{
		this.wheel = base.GetComponent<WheelCollider>();
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x000318AC File Offset: 0x0002FAAC
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
		if (component.GetGroundHit(out wheelHit) && (double)wheelHit.forwardSlip > 0.3 && component.GetGroundHit(out wheelHit))
		{
			WheelFrictionCurve sidewaysFriction3 = component.sidewaysFriction;
			sidewaysFriction3.extremumSlip = 0.3f;
			component.sidewaysFriction = sidewaysFriction3;
		}
		if (component.GetGroundHit(out wheelHit))
		{
			if ((double)wheelHit.forwardSlip < -0.2 && component.GetGroundHit(out wheelHit))
			{
				WheelFrictionCurve sidewaysFriction4 = component.sidewaysFriction;
				sidewaysFriction4.stiffness = 0.5f;
				component.sidewaysFriction = sidewaysFriction4;
			}
			if (component.GetGroundHit(out wheelHit) && (double)wheelHit.sidewaysSlip > 0.1 && component.GetGroundHit(out wheelHit))
			{
				WheelFrictionCurve sidewaysFriction5 = component.sidewaysFriction;
				sidewaysFriction5.stiffness = 0.5f;
				component.sidewaysFriction = sidewaysFriction5;
			}
		}
	}

	// Token: 0x04000969 RID: 2409
	public WheelCollider wheel;
}
