using System;
using UnityEngine;

// Token: 0x020001EE RID: 494
public class Winch : MonoBehaviour
{
	// Token: 0x06000B92 RID: 2962 RVA: 0x00080B22 File Offset: 0x0007ED22
	private void Start()
	{
		this.lineRenderer = base.gameObject.GetComponent<LineRenderer>();
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x00080B38 File Offset: 0x0007ED38
	private void Update()
	{
		RaycastHit raycastHit;
		if (Input.GetMouseButtonDown(0) && this.anchor == null && Physics.Raycast(this.currentCamera.ScreenPointToRay(Input.mousePosition), out raycastHit) && raycastHit.transform.gameObject.GetComponent<Rigidbody>() != null)
		{
			this.anchor = raycastHit.transform;
			this.rb = this.anchor.gameObject.GetComponent<Rigidbody>();
			this.lineRenderer.enabled = true;
			this.lastDistance = Vector3.Distance(this.anchor.position, base.transform.position);
		}
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x00080BE4 File Offset: 0x0007EDE4
	private void FixedUpdate()
	{
		if (this.anchor != null)
		{
			this.lineRenderer.SetPosition(0, base.transform.position);
			this.lineRenderer.SetPosition(1, this.anchor.position);
			Vector3 normalized = (base.transform.position - this.anchor.position).normalized;
			if (Input.GetButton("Fire1") && this.rb.velocity.magnitude < (float)this.liftSpeed)
			{
				Debug.Log("raise");
				this.rb.AddForce(normalized * (float)this.winchForce);
				this.lastDistance = Vector3.Distance(this.anchor.position, base.transform.position);
			}
			else if (this.lastDistance < Vector3.Distance(this.anchor.position, base.transform.position) && this.rb.velocity.y < 0f)
			{
				this.rb.AddForce(normalized * (float)this.winchForce * 2f);
			}
			if (Input.GetButton("Fire2"))
			{
				Debug.Log(this.lastDistance.ToString());
				this.lastDistance += this.lowerSpeed / 80f;
			}
			if (Input.GetButtonDown("Jump"))
			{
				Debug.Log("break");
				this.anchor = null;
				this.lineRenderer.enabled = false;
			}
		}
	}

	// Token: 0x0400142D RID: 5165
	[SerializeField]
	private Camera currentCamera;

	// Token: 0x0400142E RID: 5166
	[SerializeField]
	private int winchForce = 10000;

	// Token: 0x0400142F RID: 5167
	[SerializeField]
	private int liftSpeed = 4;

	// Token: 0x04001430 RID: 5168
	[SerializeField]
	private float lowerSpeed = 4f;

	// Token: 0x04001431 RID: 5169
	private Transform anchor;

	// Token: 0x04001432 RID: 5170
	private LineRenderer lineRenderer;

	// Token: 0x04001433 RID: 5171
	private float lastDistance;

	// Token: 0x04001434 RID: 5172
	private float lastSpeed;

	// Token: 0x04001435 RID: 5173
	private bool wasActive;

	// Token: 0x04001436 RID: 5174
	private Rigidbody rb;
}
