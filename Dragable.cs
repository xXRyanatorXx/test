using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000137 RID: 311
[RequireComponent(typeof(Rigidbody))]
public class Dragable : MonoBehaviour
{
	// Token: 0x0600069E RID: 1694 RVA: 0x00035650 File Offset: 0x00033850
	private void Start()
	{
		if (base.transform.name == "Trolley" || base.transform.name == "EngineStand" || base.transform.name == "TrashBin")
		{
			base.GetComponent<Rigidbody>().centerOfMass = this.com;
		}
		this.tempParent = GameObject.Find("hand");
		base.enabled = false;
		this.myRigidbody = base.GetComponent<Rigidbody>();
		this.myTransform = base.transform;
		if (!this.cam)
		{
			this.cam = Camera.main;
		}
		if (!this.cam)
		{
			Debug.LogError("Can't find camera tagged MainCamera");
			return;
		}
		this.camTransform = this.cam.transform;
		this.sqrMoveLimit = this.moveLimit * this.moveLimit;
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x00035738 File Offset: 0x00033938
	private void OnMouseDown()
	{
		if (Vector3.Distance(base.transform.position, this.tempParent.transform.position) < 2f && !tools.cooldown)
		{
			if (base.GetComponent<MPobject>())
			{
				tools.NetworkPLayer.pickup(base.GetComponent<MPobject>().networkDummy);
			}
			this.tempParent.transform.position = base.transform.position;
			base.enabled = true;
			this.canMove = true;
			this.myTransform.Translate(Vector3.up * this.addHeightWhenClicked);
			this.gravitySetting = this.myRigidbody.useGravity;
			this.myRigidbody.useGravity = false;
			this.myRigidbody.freezeRotation = this.freezeRotationOnDrag;
			this.yPos = this.myTransform.position.y;
		}
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00035824 File Offset: 0x00033A24
	private void OnMouseUp()
	{
		if (base.enabled)
		{
			this.canMove = false;
			this.myRigidbody.useGravity = this.gravitySetting;
			if (!this.myRigidbody.useGravity)
			{
				Vector3 position = this.myTransform.position;
				position.y = this.yPos - this.addHeightWhenClicked;
				this.myTransform.position = position;
			}
		}
		base.StartCoroutine(this.AllowRotate());
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x00035897 File Offset: 0x00033A97
	private IEnumerator AllowRotate()
	{
		yield return new WaitForSeconds(1f);
		if (!this.canMove)
		{
			base.enabled = false;
			this.myRigidbody.freezeRotation = false;
		}
		yield break;
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x0000245B File Offset: 0x0000065B
	private void OnCollisionEnter()
	{
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0000245B File Offset: 0x0000065B
	private void OnCollisionExit()
	{
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x000358A8 File Offset: 0x00033AA8
	private void FixedUpdate()
	{
		if (!this.canMove)
		{
			return;
		}
		this.myRigidbody.velocity = Vector3.zero;
		this.myRigidbody.angularVelocity = Vector3.zero;
		Vector3 position = this.myTransform.position;
		position.y = this.yPos;
		this.myTransform.position = position;
		Vector3 mousePosition = Input.mousePosition;
		Vector3 b = new Vector3(this.tempParent.transform.position.x, this.tempParent.transform.position.y - this.myTransform.position.y, this.tempParent.transform.position.z) - this.myTransform.position;
		b.y = 0f;
		if (this.collisionCount > this.normalCollisionCount)
		{
			b = b.normalized * this.collisionMoveFactor;
		}
		else if (b.sqrMagnitude > this.sqrMoveLimit)
		{
			b = b.normalized * this.moveLimit;
		}
		this.myRigidbody.MovePosition(this.myRigidbody.position + b);
	}

	// Token: 0x04000A18 RID: 2584
	public int normalCollisionCount = 1;

	// Token: 0x04000A19 RID: 2585
	public float moveLimit = 0.05f;

	// Token: 0x04000A1A RID: 2586
	public float collisionMoveFactor = 0.01f;

	// Token: 0x04000A1B RID: 2587
	public float addHeightWhenClicked;

	// Token: 0x04000A1C RID: 2588
	public bool freezeRotationOnDrag = true;

	// Token: 0x04000A1D RID: 2589
	public Camera cam;

	// Token: 0x04000A1E RID: 2590
	private Rigidbody myRigidbody;

	// Token: 0x04000A1F RID: 2591
	private Transform myTransform;

	// Token: 0x04000A20 RID: 2592
	private bool canMove;

	// Token: 0x04000A21 RID: 2593
	private float yPos;

	// Token: 0x04000A22 RID: 2594
	private bool gravitySetting;

	// Token: 0x04000A23 RID: 2595
	private bool freezeRotationSetting;

	// Token: 0x04000A24 RID: 2596
	private float sqrMoveLimit;

	// Token: 0x04000A25 RID: 2597
	private int collisionCount;

	// Token: 0x04000A26 RID: 2598
	private Transform camTransform;

	// Token: 0x04000A27 RID: 2599
	public GameObject tempParent;

	// Token: 0x04000A28 RID: 2600
	public Vector3 com;
}
