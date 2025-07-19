using System;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class Carcontroll2 : MonoBehaviour
{
	// Token: 0x0600062E RID: 1582 RVA: 0x00031143 File Offset: 0x0002F343
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.rb.centerOfMass = this.centreofmass.transform.localPosition;
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0003116C File Offset: 0x0002F36C
	private void FixedUpdate()
	{
		if (!this.braked)
		{
			this.WheelFL.brakeTorque = 0f;
			this.WheelFR.brakeTorque = 0f;
			this.WheelRL.brakeTorque = 0f;
			this.WheelRR.brakeTorque = 0f;
		}
		this.WheelRR.motorTorque = this.maxTorque * Input.GetAxis("Vertical");
		this.WheelRL.motorTorque = this.maxTorque * Input.GetAxis("Vertical");
		this.WheelFL.steerAngle = 50f * Input.GetAxis("Horizontal");
		this.WheelFR.steerAngle = 50f * Input.GetAxis("Horizontal");
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00031230 File Offset: 0x0002F430
	private void Update()
	{
		this.HandBrake();
		this.WheelFLtrans.Rotate(this.WheelFL.rpm / 60f * 360f * Time.deltaTime, 0f, 0f);
		this.WheelFRtrans.Rotate(this.WheelFR.rpm / 60f * 360f * Time.deltaTime, 0f, 0f);
		this.WheelRLtrans.Rotate(this.WheelRL.rpm / 60f * 360f * Time.deltaTime, 0f, 0f);
		this.WheelRRtrans.Rotate(this.WheelRL.rpm / 60f * 360f * Time.deltaTime, 0f, 0f);
		Vector3 localEulerAngles = this.WheelFLtrans.localEulerAngles;
		Vector3 localEulerAngles2 = this.WheelFRtrans.localEulerAngles;
		localEulerAngles.y = this.WheelFL.steerAngle - this.WheelFLtrans.localEulerAngles.z;
		this.WheelFLtrans.localEulerAngles = localEulerAngles;
		localEulerAngles2.y = this.WheelFR.steerAngle - this.WheelFRtrans.localEulerAngles.z;
		this.WheelFRtrans.localEulerAngles = localEulerAngles2;
		this.eulertest = this.WheelFLtrans.localEulerAngles;
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x00031394 File Offset: 0x0002F594
	private void HandBrake()
	{
		if (Input.GetButton("Jump"))
		{
			this.braked = true;
		}
		else
		{
			this.braked = false;
		}
		if (this.braked)
		{
			this.WheelRL.brakeTorque = this.maxBrakeTorque * 200f;
			this.WheelRR.brakeTorque = this.maxBrakeTorque * 200f;
			this.WheelRL.motorTorque = 0f;
			this.WheelRR.motorTorque = 0f;
		}
	}

	// Token: 0x04000940 RID: 2368
	public WheelCollider WheelFL;

	// Token: 0x04000941 RID: 2369
	public WheelCollider WheelFR;

	// Token: 0x04000942 RID: 2370
	public WheelCollider WheelRL;

	// Token: 0x04000943 RID: 2371
	public WheelCollider WheelRR;

	// Token: 0x04000944 RID: 2372
	public Transform WheelFLtrans;

	// Token: 0x04000945 RID: 2373
	public Transform WheelFRtrans;

	// Token: 0x04000946 RID: 2374
	public Transform WheelRLtrans;

	// Token: 0x04000947 RID: 2375
	public Transform WheelRRtrans;

	// Token: 0x04000948 RID: 2376
	public Vector3 eulertest;

	// Token: 0x04000949 RID: 2377
	private float maxBwdSpeed = 1000f;

	// Token: 0x0400094A RID: 2378
	private float gravity = 9.8f;

	// Token: 0x0400094B RID: 2379
	private bool braked;

	// Token: 0x0400094C RID: 2380
	public float maxBrakeTorque = 500f;

	// Token: 0x0400094D RID: 2381
	private Rigidbody rb;

	// Token: 0x0400094E RID: 2382
	public Transform centreofmass;

	// Token: 0x0400094F RID: 2383
	public float maxTorque = 600f;
}
