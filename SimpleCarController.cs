using System;
using UnityEngine;

// Token: 0x02000127 RID: 295
public class SimpleCarController : MonoBehaviour
{
	// Token: 0x06000637 RID: 1591 RVA: 0x000316A9 File Offset: 0x0002F8A9
	public void GetInput()
	{
		this.m_horizontalInput = Input.GetAxis("Horizontal");
		this.m_verticalInput = Input.GetAxis("Vertical");
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x000316CB File Offset: 0x0002F8CB
	private void Steer()
	{
		this.m_steeringAngle = this.maxSteerAngle * this.m_horizontalInput;
		this.frontDriverW.steerAngle = this.m_steeringAngle;
		this.frontPassengerW.steerAngle = this.m_steeringAngle;
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x00031702 File Offset: 0x0002F902
	private void Accelerate()
	{
		this.frontDriverW.motorTorque = this.m_verticalInput * this.motorForce;
		this.frontPassengerW.motorTorque = this.m_verticalInput * this.motorForce;
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x00031734 File Offset: 0x0002F934
	private void UpdateWheelPoses()
	{
		this.UpdateWheelPose(this.frontDriverW, this.frontDriverT);
		this.UpdateWheelPose(this.frontPassengerW, this.frontPassengerT);
		this.UpdateWheelPose(this.rearDriverW, this.rearDriverT);
		this.UpdateWheelPose(this.rearPassengerW, this.rearPassengerT);
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0003178C File Offset: 0x0002F98C
	private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
	{
		Vector3 position = _transform.position;
		Quaternion rotation = _transform.rotation;
		_collider.GetWorldPose(out position, out rotation);
		_transform.position = position;
		_transform.rotation = rotation;
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x000317BF File Offset: 0x0002F9BF
	private void FixedUpdate()
	{
		this.GetInput();
		this.Steer();
		this.Accelerate();
		this.UpdateWheelPoses();
	}

	// Token: 0x0400095B RID: 2395
	private float m_horizontalInput;

	// Token: 0x0400095C RID: 2396
	private float m_verticalInput;

	// Token: 0x0400095D RID: 2397
	private float m_steeringAngle;

	// Token: 0x0400095E RID: 2398
	public WheelCollider frontDriverW;

	// Token: 0x0400095F RID: 2399
	public WheelCollider frontPassengerW;

	// Token: 0x04000960 RID: 2400
	public WheelCollider rearDriverW;

	// Token: 0x04000961 RID: 2401
	public WheelCollider rearPassengerW;

	// Token: 0x04000962 RID: 2402
	public Transform frontDriverT;

	// Token: 0x04000963 RID: 2403
	public Transform frontPassengerT;

	// Token: 0x04000964 RID: 2404
	public Transform rearDriverT;

	// Token: 0x04000965 RID: 2405
	public Transform rearPassengerT;

	// Token: 0x04000966 RID: 2406
	public float maxSteerAngle = 30f;

	// Token: 0x04000967 RID: 2407
	public float motorForce = 50f;
}
