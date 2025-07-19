using System;
using PaintIn3D;
using UnityEngine;

// Token: 0x0200028E RID: 654
public class colortester : MonoBehaviour
{
	// Token: 0x06000F51 RID: 3921 RVA: 0x0009EDE8 File Offset: 0x0009CFE8
	private void Update()
	{
		RaycastHit raycastHit;
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(base.transform.position, base.transform.forward, out raycastHit, 1f, 1 << LayerMask.NameToLayer("OpenableParts") | 1 << LayerMask.NameToLayer("Repair") | 1 << LayerMask.NameToLayer("LooseParts")))
		{
			if (raycastHit.collider.GetComponent<Rigidbody>())
			{
				this.IsRB = true;
				this.RB = raycastHit.collider.GetComponent<Rigidbody>();
				this.RB.isKinematic = true;
			}
			if (raycastHit.collider.GetComponent<MeshCollider>())
			{
				this.MC = raycastHit.collider.GetComponent<MeshCollider>();
				if (this.MC.isTrigger)
				{
					this.trigger = true;
				}
				this.MC.convex = false;
			}
			base.gameObject.GetComponent<P3dHitScreen>().enabled = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			base.gameObject.GetComponent<P3dHitScreen>().enabled = false;
			if (this.RB != null && this.IsRB)
			{
				this.RB.isKinematic = false;
			}
			if (this.MC != null)
			{
				this.MC.convex = true;
				if (this.trigger)
				{
					this.MC.isTrigger = true;
				}
			}
			this.MC = null;
			this.RB = null;
			this.trigger = false;
		}
		if (!base.transform.root.GetComponent<tools>())
		{
			base.gameObject.GetComponent<P3dHitScreen>().enabled = false;
			base.enabled = false;
		}
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x0009EF91 File Offset: 0x0009D191
	public void COLOR(Color col)
	{
		this.color = col;
	}

	// Token: 0x0400187E RID: 6270
	public Color color;

	// Token: 0x0400187F RID: 6271
	public GameObject part;

	// Token: 0x04001880 RID: 6272
	public Rigidbody RB;

	// Token: 0x04001881 RID: 6273
	public bool IsRB;

	// Token: 0x04001882 RID: 6274
	public MeshCollider MC;

	// Token: 0x04001883 RID: 6275
	public bool trigger;
}
