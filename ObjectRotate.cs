using System;
using UnityEngine;

// Token: 0x020001F8 RID: 504
public class ObjectRotate : MonoBehaviour
{
	// Token: 0x06000BCC RID: 3020 RVA: 0x00082DCD File Offset: 0x00080FCD
	private void Awake()
	{
		this.tempParent = GameObject.Find("hand");
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x00082DE0 File Offset: 0x00080FE0
	private void OnMouseDown()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 2f, 1 << LayerMask.NameToLayer("Weld")))
		{
			this.tempParent.transform.position = raycastHit.point;
			if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
			{
				tools.NetworkPLayer.pickup(base.GetComponent<MPobject>().networkDummy);
			}
		}
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x00082E74 File Offset: 0x00081074
	private void OnMouseDrag()
	{
		if (base.transform.position.y < base.transform.root.position.y - 0.9f)
		{
			if (this.rotate && Vector3.Distance(this.tempParent.transform.position, base.transform.position) < 1.7f)
			{
				this.target = new Vector3(this.tempParent.transform.position.x, base.transform.position.y, this.tempParent.transform.position.z);
				Vector3 vector = this.target - base.transform.position;
				float maxRadiansDelta = this.speed * Time.deltaTime;
				Vector3 forward = Vector3.RotateTowards(base.transform.forward, vector, maxRadiansDelta, 0f);
				this.oldrotation = base.transform.eulerAngles.y;
				base.transform.rotation = Quaternion.LookRotation(forward);
				if (base.transform.localEulerAngles.y > this.MaxRot || base.transform.localEulerAngles.y < this.MinRot)
				{
					base.transform.rotation = Quaternion.Euler(base.transform.rotation.x, this.oldrotation, base.transform.rotation.z);
				}
			}
			if (this.drag && Vector3.Distance(this.tempParent.transform.position, base.transform.position) < 1.7f)
			{
				Vector3 a = base.transform.TransformDirection(Vector3.forward);
				Vector2 rhs = Camera.main.WorldToScreenPoint(a + base.transform.position) - Camera.main.WorldToScreenPoint(base.transform.position);
				rhs.Normalize();
				float d = Vector2.Dot(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), rhs);
				base.transform.Translate(Vector3.forward * 0.02f * d);
				if (base.transform.localPosition.z < this.MinRot)
				{
					base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, this.MinRot);
				}
				if (base.transform.localPosition.z > this.MaxRot)
				{
					base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, this.MaxRot);
				}
			}
		}
	}

	// Token: 0x04001473 RID: 5235
	public bool rotate;

	// Token: 0x04001474 RID: 5236
	public bool drag;

	// Token: 0x04001475 RID: 5237
	public float MinRot;

	// Token: 0x04001476 RID: 5238
	public float MaxRot;

	// Token: 0x04001477 RID: 5239
	public GameObject tempParent;

	// Token: 0x04001478 RID: 5240
	public float speed = 1f;

	// Token: 0x04001479 RID: 5241
	public Vector3 target;

	// Token: 0x0400147A RID: 5242
	public float oldrotation;
}
