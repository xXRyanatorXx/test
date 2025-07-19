using System;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public class ExampleWheelController : MonoBehaviour
{
	// Token: 0x060004F7 RID: 1271 RVA: 0x00029391 File Offset: 0x00027591
	private void Start()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Rigidbody.maxAngularVelocity = 100f;
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x000293B0 File Offset: 0x000275B0
	private void Update()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(-1f * this.acceleration, 0f, 0f), ForceMode.Acceleration);
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(1f * this.acceleration, 0f, 0f), ForceMode.Acceleration);
		}
		float value = -this.m_Rigidbody.angularVelocity.x / 100f;
		if (this.motionVectorRenderer)
		{
			this.motionVectorRenderer.material.SetFloat(ExampleWheelController.Uniforms._MotionAmount, Mathf.Clamp(value, -0.25f, 0.25f));
		}
	}

	// Token: 0x040006AA RID: 1706
	public float acceleration;

	// Token: 0x040006AB RID: 1707
	public Renderer motionVectorRenderer;

	// Token: 0x040006AC RID: 1708
	private Rigidbody m_Rigidbody;

	// Token: 0x020000E5 RID: 229
	private static class Uniforms
	{
		// Token: 0x040006AD RID: 1709
		internal static readonly int _MotionAmount = Shader.PropertyToID("_MotionAmount");
	}
}
