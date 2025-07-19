using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class Box : MonoBehaviour
{
	// Token: 0x06000611 RID: 1553 RVA: 0x000309DD File Offset: 0x0002EBDD
	private void Start()
	{
		base.enabled = false;
		this.m_Collider = this.boundsObject.GetComponent<Collider>();
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x000309F8 File Offset: 0x0002EBF8
	private void OnCollisionEnter(Collision collision)
	{
		if (!this.falling && !collision.gameObject.transform.parent && ((collision.gameObject.GetComponent<PickupTool>() && collision.gameObject.GetComponent<PickupTool>().CanPutInBox) || (collision.gameObject.GetComponent<PickupItems>() && collision.gameObject.GetComponent<PickupItems>().CanPutInBox) || (collision.gameObject.GetComponent<MooveItem>() && collision.gameObject.GetComponent<MooveItem>().CanPutInBox) || (collision.gameObject.GetComponent<Partinfo>() && collision.gameObject.GetComponent<Partinfo>().CanPutInBox)))
		{
			base.StartCoroutine(this.Fall());
			this.falling = true;
			this.item = collision.gameObject;
		}
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x00030ADA File Offset: 0x0002ECDA
	private IEnumerator Fall()
	{
		yield return new WaitForSeconds(0.2f);
		this.falling = false;
		if (this.m_Collider.bounds.Contains(this.item.transform.position))
		{
			this.item.transform.SetParent(base.transform);
			if (this.item.GetComponent<FixedJoint>() == null)
			{
				this.item.AddComponent<FixedJoint>();
			}
			this.item.GetComponent<FixedJoint>().connectedBody = base.GetComponent<Rigidbody>();
			this.item = null;
		}
		yield break;
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x00030AEC File Offset: 0x0002ECEC
	private void Update()
	{
		if (base.gameObject.transform.rotation.eulerAngles.x < 140f && base.gameObject.transform.rotation.eulerAngles.x > 40f)
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.GetComponent<FixedJoint>())
				{
					UnityEngine.Object.Destroy(transform.GetComponent<FixedJoint>());
					transform.SetParent(null);
				}
			}
		}
		if (!base.transform.parent)
		{
			base.enabled = false;
		}
	}

	// Token: 0x04000929 RID: 2345
	private Collider m_Collider;

	// Token: 0x0400092A RID: 2346
	public bool falling;

	// Token: 0x0400092B RID: 2347
	public GameObject item;

	// Token: 0x0400092C RID: 2348
	public GameObject boundsObject;
}
