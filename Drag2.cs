using System;
using UnityEngine;

// Token: 0x020001F6 RID: 502
public class Drag2 : MonoBehaviour
{
	// Token: 0x06000BC3 RID: 3011 RVA: 0x00082B78 File Offset: 0x00080D78
	private void Start()
	{
		this.targetCamera = base.GetComponent<Camera>();
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x00082B86 File Offset: 0x00080D86
	private void Update()
	{
		if (!this.targetCamera)
		{
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			this.selectedRigidbody = this.GetRigidbodyFromMouseClick();
		}
		if (Input.GetMouseButtonUp(0) && this.selectedRigidbody)
		{
			this.selectedRigidbody = null;
		}
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x00082BC8 File Offset: 0x00080DC8
	private void FixedUpdate()
	{
		if (this.selectedRigidbody)
		{
			Vector3 b = this.targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.selectionDistance)) - this.originalScreenTargetPosition;
			this.selectedRigidbody.velocity = (this.originalRigidbodyPos + b - this.selectedRigidbody.transform.position) * this.forceAmount * Time.deltaTime;
		}
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x00082C5C File Offset: 0x00080E5C
	private Rigidbody GetRigidbodyFromMouseClick()
	{
		RaycastHit raycastHit = default(RaycastHit);
		Ray ray = this.targetCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out raycastHit) && raycastHit.collider.gameObject.GetComponent<Rigidbody>())
		{
			this.selectionDistance = Vector3.Distance(ray.origin, raycastHit.point);
			this.originalScreenTargetPosition = this.targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.selectionDistance));
			this.originalRigidbodyPos = raycastHit.collider.transform.position;
			return raycastHit.collider.gameObject.GetComponent<Rigidbody>();
		}
		return null;
	}

	// Token: 0x0400146B RID: 5227
	public float forceAmount = 500f;

	// Token: 0x0400146C RID: 5228
	private Rigidbody selectedRigidbody;

	// Token: 0x0400146D RID: 5229
	private Camera targetCamera;

	// Token: 0x0400146E RID: 5230
	private Vector3 originalScreenTargetPosition;

	// Token: 0x0400146F RID: 5231
	private Vector3 originalRigidbodyPos;

	// Token: 0x04001470 RID: 5232
	private float selectionDistance;
}
