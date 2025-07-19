using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class VehicleNitro : MonoBehaviour
{
	// Token: 0x06000121 RID: 289 RVA: 0x0000A64B File Offset: 0x0000884B
	private void OnEnable()
	{
		this.m_rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000122 RID: 290 RVA: 0x0000A65C File Offset: 0x0000885C
	private void Update()
	{
		if (this.mode == VehicleNitro.Mode.Impulse && Input.GetKeyDown(this.key) && this.m_rigidbody.velocity.magnitude < this.maxVelocity)
		{
			this.m_rigidbody.AddRelativeForce(Vector3.forward * this.value, ForceMode.VelocityChange);
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x0000A6B8 File Offset: 0x000088B8
	private void FixedUpdate()
	{
		if (this.mode == VehicleNitro.Mode.Acceleration && Input.GetKey(this.key) && this.m_rigidbody.velocity.magnitude < this.maxVelocity)
		{
			this.m_rigidbody.AddRelativeForce(Vector3.forward * this.value, ForceMode.Acceleration);
		}
	}

	// Token: 0x04000191 RID: 401
	public VehicleNitro.Mode mode;

	// Token: 0x04000192 RID: 402
	public float value = 10f;

	// Token: 0x04000193 RID: 403
	public float maxVelocity = 50f;

	// Token: 0x04000194 RID: 404
	public KeyCode key = KeyCode.N;

	// Token: 0x04000195 RID: 405
	private Rigidbody m_rigidbody;

	// Token: 0x0200003D RID: 61
	public enum Mode
	{
		// Token: 0x04000197 RID: 407
		Acceleration,
		// Token: 0x04000198 RID: 408
		Impulse
	}
}
