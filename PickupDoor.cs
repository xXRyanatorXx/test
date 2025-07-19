using System;
using Mirror;
using UnityEngine;

// Token: 0x020001A2 RID: 418
public class PickupDoor : MonoBehaviour
{
	// Token: 0x06000999 RID: 2457 RVA: 0x0005F02B File Offset: 0x0005D22B
	private void Awake()
	{
		this.tempParent = GameObject.Find("hand");
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x0005F040 File Offset: 0x0005D240
	private void Update()
	{
		this.placetoput = null;
		this.seetoput = false;
		RaycastHit raycastHit;
		if (tools.LookingAtTransparent && tools.helditem == base.gameObject.name && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("TransparentParts")) && raycastHit.collider.gameObject.name == base.gameObject.name)
		{
			this.placetoput = raycastHit.collider.gameObject;
			this.seetoput = true;
			tools.canput = true;
		}
		if (Input.GetMouseButton(2) && this.isHolding && this.throwForce < 1000f)
		{
			this.throwForce += 5f;
		}
		if ((Input.GetMouseButtonUp(2) || Input.GetMouseButtonUp(1)) && this.isHolding)
		{
			this.isHolding = false;
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
			base.gameObject.AddComponent<MeshCollider>().convex = true;
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			base.gameObject.transform.SetParent(null);
			base.gameObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * this.throwForce);
			this.throwForce = 0f;
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
			{
				if (gameObject.name == base.gameObject.name)
				{
					gameObject.GetComponent<transparents>().Disablerend();
				}
			}
			if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.DropOnGround();
			}
		}
		if (!this.isHolding)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0005F254 File Offset: 0x0005D454
	public void OnMouseDown()
	{
		if (tools.tool == 24 || tools.tool == 25 || tools.UIisOpen)
		{
			return;
		}
		RaycastHit raycastHit;
		if (!tools.sitting && !tools.Clicked && !tools.cooldown && tools.tool != 12 && tools.tool != 11 && tools.tool != 23 && tools.tool != 14 && tools.tool != 4 && tools.tool != 7 && tools.tool != 19 && base.gameObject.GetComponent<Partinfo>().tightnuts == 0f && base.gameObject.GetComponent<Partinfo>().fixedwelds == 0f && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("OpenableParts")))
		{
			this.tempParent.transform.position = raycastHit.point;
			this.isHolding = true;
			base.enabled = true;
			tools.Clicked = true;
			tools.helditem = base.gameObject.name;
			base.GetComponent<CarProperties>().picked = true;
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
			{
				if (gameObject.name == base.gameObject.name)
				{
					gameObject.GetComponent<transparents>().Recheck();
				}
			}
			base.gameObject.GetComponent<Rigidbody>().useGravity = false;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
			if (base.GetComponent<FixedJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			}
			if (base.GetComponent<HingeJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<HingeJoint>());
			}
			base.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			base.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			base.gameObject.transform.SetParent(this.tempParent.transform);
			if (Vector3.Distance(this.tempParent.transform.position, Camera.main.transform.position) > 1.3f)
			{
				this.tempParent.transform.position = Vector3.MoveTowards(this.tempParent.transform.position, Camera.main.transform.position, Vector3.Distance(this.tempParent.transform.position, Camera.main.transform.position) - 1.3f);
			}
			base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			base.gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
			HexNut[] componentsInChildren = base.gameObject.GetComponentsInChildren<HexNut>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].disableREND();
			}
			FlatNut[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<FlatNut>();
			for (int k = 0; k < componentsInChildren2.Length; k++)
			{
				componentsInChildren2[k].disableREND();
			}
			BoltNut[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<BoltNut>();
			for (int l = 0; l < componentsInChildren3.Length; l++)
			{
				componentsInChildren3[l].disableREND();
			}
			DisableRend[] componentsInChildren4 = base.gameObject.GetComponentsInChildren<DisableRend>();
			for (int m = 0; m < componentsInChildren4.Length; m++)
			{
				componentsInChildren4[m].disableREND();
			}
			base.GetComponent<Partinfo>().AllowFall = false;
			if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.RemoveWindow();
			}
		}
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x0005F5FC File Offset: 0x0005D7FC
	public void RemovingContinue()
	{
		base.GetComponent<Partinfo>().AllowFall = false;
		if (base.gameObject.GetComponent<Rigidbody>())
		{
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
		}
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy && !base.GetComponent<MPobject>().networkDummy.hasAuthority)
		{
			if (base.GetComponent<FixedJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			}
			if (base.GetComponent<HingeJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<HingeJoint>());
			}
			if (base.gameObject.GetComponent<Rigidbody>() == null)
			{
				base.gameObject.AddComponent<Rigidbody>();
			}
			base.gameObject.GetComponent<Rigidbody>().useGravity = false;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
			base.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			base.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			base.gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
			base.gameObject.transform.SetParent(null);
		}
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x0005F740 File Offset: 0x0005D940
	private void OnMouseUp()
	{
		tools.helditem = "Nothing";
		if (this.isHolding)
		{
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
			{
				if (gameObject.name == base.gameObject.name)
				{
					gameObject.GetComponent<transparents>().Disablerend();
				}
			}
			this.isHolding = false;
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			base.gameObject.GetComponent<Rigidbody>().mass = base.gameObject.GetComponent<Partinfo>().weight;
			base.gameObject.transform.SetParent(null);
			CarProperties[] componentsInChildren = base.GetComponentsInChildren<CarProperties>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].RestartColliders();
			}
			if (this.seetoput && !base.GetComponent<CarProperties>().Ruined)
			{
				if (base.GetComponent<MPobject>())
				{
					MPobject component = base.GetComponent<MPobject>();
					int childnumber = -1;
					int childnumber2 = -1;
					if (this.placetoput.transform.parent.GetComponent<MPobject>())
					{
						component = this.placetoput.transform.parent.GetComponent<MPobject>();
						childnumber = this.placetoput.transform.GetSiblingIndex();
					}
					if (this.placetoput.transform.parent.parent && this.placetoput.transform.parent.parent.GetComponent<MPobject>())
					{
						component = this.placetoput.transform.parent.parent.GetComponent<MPobject>();
						childnumber = this.placetoput.transform.GetSiblingIndex();
						childnumber2 = this.placetoput.transform.parent.GetSiblingIndex();
					}
					base.GetComponent<MPobject>().networkDummy.GetComponent<NetworkTransform>().enabled = false;
					base.GetComponent<MPobject>().networkDummy.enabled = false;
					base.GetComponent<MPobject>().networkDummy.AttachPickup(component, childnumber, childnumber2);
				}
				else
				{
					this.FitInPlace(this.placetoput);
				}
			}
			else if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.DropOnGround();
			}
			base.gameObject.GetComponent<Partinfo>().CheckGround();
		}
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x0005F9A4 File Offset: 0x0005DBA4
	public void DropOnGround()
	{
		if (base.gameObject.GetComponent<Rigidbody>() == null)
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
		base.gameObject.AddComponent<MeshCollider>().convex = true;
		base.gameObject.GetComponent<Rigidbody>().useGravity = true;
		base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
		base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		base.gameObject.GetComponent<Rigidbody>().mass = base.gameObject.GetComponent<Partinfo>().weight;
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x0005FA44 File Offset: 0x0005DC44
	public void FitInPlace(GameObject place)
	{
		if (base.gameObject.GetComponent<Rigidbody>() == null)
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		base.gameObject.GetComponent<Rigidbody>().useGravity = true;
		base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
		base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		base.gameObject.transform.position = place.transform.position;
		base.gameObject.transform.rotation = place.transform.rotation;
		base.gameObject.transform.SetParent(place.transform);
		base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (base.transform.root.GetComponent<CarProperties>())
		{
			base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
		}
		if (base.transform.root.GetComponent<MainCarProperties>())
		{
			base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
		}
		base.gameObject.AddComponent<HingeJoint>();
		base.gameObject.GetComponent<HingeJoint>().connectedBody = base.gameObject.transform.root.GetComponent<Rigidbody>();
		base.gameObject.GetComponent<HingeJoint>().anchor = new Vector3(0f, 0f, 0f);
		base.gameObject.GetComponent<HingeJoint>().breakForce = 2000f;
		base.gameObject.GetComponent<Rigidbody>().mass = 1f;
		if (base.gameObject.GetComponent<Partinfo>().Rdoor)
		{
			base.gameObject.GetComponent<HingeJoint>().axis = Vector3.down;
			HingeJoint component = base.GetComponent<HingeJoint>();
			JointLimits limits = component.limits;
			limits.min = 0f;
			limits.bounciness = 0f;
			limits.bounceMinVelocity = 0f;
			limits.max = 90f;
			component.limits = limits;
			component.useLimits = true;
		}
		if (base.gameObject.GetComponent<Partinfo>().Ldoor)
		{
			base.gameObject.GetComponent<HingeJoint>().axis = Vector3.up;
			HingeJoint component2 = base.GetComponent<HingeJoint>();
			JointLimits limits2 = component2.limits;
			limits2.min = 0f;
			limits2.bounciness = 0f;
			limits2.bounceMinVelocity = 0f;
			limits2.max = 90f;
			component2.limits = limits2;
			component2.useLimits = true;
		}
		if (base.gameObject.GetComponent<Partinfo>().Trunk)
		{
			base.gameObject.GetComponent<HingeJoint>().axis = Vector3.right;
			HingeJoint component3 = base.GetComponent<HingeJoint>();
			JointLimits limits3 = component3.limits;
			limits3.min = 0f;
			limits3.bounciness = 0f;
			limits3.bounceMinVelocity = 0f;
			limits3.max = 90f;
			component3.limits = limits3;
			component3.useLimits = true;
		}
		if (base.gameObject.GetComponent<Partinfo>().Hood)
		{
			base.gameObject.GetComponent<HingeJoint>().axis = Vector3.left;
			HingeJoint component4 = base.GetComponent<HingeJoint>();
			JointLimits limits4 = component4.limits;
			limits4.min = 0f;
			limits4.bounciness = 0f;
			limits4.bounceMinVelocity = 0f;
			limits4.max = 90f;
			component4.limits = limits4;
			component4.useLimits = true;
		}
		if (base.gameObject.GetComponent<Partinfo>().HoodHalf)
		{
			base.gameObject.GetComponent<HingeJoint>().axis = Vector3.left;
			HingeJoint component5 = base.GetComponent<HingeJoint>();
			JointLimits limits5 = component5.limits;
			limits5.min = 0f;
			limits5.bounciness = 0f;
			limits5.bounceMinVelocity = 0f;
			limits5.max = 50f;
			component5.limits = limits5;
			component5.useLimits = true;
		}
		base.gameObject.GetComponent<OpenDoor>().installed();
		HexNut[] componentsInChildren = base.gameObject.GetComponentsInChildren<HexNut>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enableREND();
		}
		FlatNut[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<FlatNut>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].enableREND();
		}
		BoltNut[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<BoltNut>();
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			componentsInChildren3[k].enableREND();
		}
		DisableRend[] componentsInChildren4 = base.gameObject.GetComponentsInChildren<DisableRend>();
		for (int l = 0; l < componentsInChildren4.Length; l++)
		{
			componentsInChildren4[l].enableREND();
		}
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x0005FECB File Offset: 0x0005E0CB
	private void OnJointBreak(float breakForce)
	{
		this.BRAKE();
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x0005FED3 File Offset: 0x0005E0D3
	public void BRAKE()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.BRAKE();
			return;
		}
		this.BRAKE2();
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x0005FF0C File Offset: 0x0005E10C
	public void BRAKE2()
	{
		if (base.gameObject.GetComponent<FixedJoint>() != null && tools.tool != 11)
		{
			base.gameObject.GetComponent<OpenDoor>().doorOpened = true;
			if (base.gameObject.GetComponent<HingeJoint>() != null)
			{
				base.gameObject.GetComponent<HingeJoint>().breakForce = 15000f;
				base.transform.GetComponent<HingeJoint>().anchor = base.transform.GetComponent<Partinfo>().HingePivot.transform.localPosition;
				return;
			}
		}
		else
		{
			if (!base.gameObject.GetComponent<Rigidbody>())
			{
				base.gameObject.AddComponent<Rigidbody>();
			}
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
			{
				base.GetComponent<MPobject>().networkDummy.DropOnGround();
			}
			base.gameObject.GetComponent<Rigidbody>().mass = base.gameObject.GetComponent<Partinfo>().weight;
			base.gameObject.transform.SetParent(null);
			HexNut[] componentsInChildren = base.gameObject.GetComponentsInChildren<HexNut>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].disableREND();
			}
			FlatNut[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<FlatNut>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].disableREND();
			}
			CarProperties[] componentsInChildren3 = base.GetComponentsInChildren<CarProperties>();
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				componentsInChildren3[k].RestartColliders();
			}
			if (tools.tool == 11)
			{
				base.gameObject.GetComponent<Rigidbody>().useGravity = true;
				base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
				base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
				base.gameObject.GetComponent<Rigidbody>().mass = base.gameObject.GetComponent<Partinfo>().weight;
				base.gameObject.transform.SetParent(null);
				HexNut[] componentsInChildren4 = base.gameObject.GetComponentsInChildren<HexNut>();
				for (int l = 0; l < componentsInChildren4.Length; l++)
				{
					componentsInChildren4[l].disableREND();
				}
				FlatNut[] componentsInChildren5 = base.gameObject.GetComponentsInChildren<FlatNut>();
				for (int m = 0; m < componentsInChildren5.Length; m++)
				{
					componentsInChildren5[m].disableREND();
				}
				componentsInChildren3 = base.GetComponentsInChildren<CarProperties>();
				for (int k = 0; k < componentsInChildren3.Length; k++)
				{
					componentsInChildren3[k].RestartColliders();
				}
			}
		}
	}

	// Token: 0x0400119A RID: 4506
	private float throwForce;

	// Token: 0x0400119B RID: 4507
	private Vector3 objectPos;

	// Token: 0x0400119C RID: 4508
	public bool canHold = true;

	// Token: 0x0400119D RID: 4509
	public GameObject placetoput;

	// Token: 0x0400119E RID: 4510
	public GameObject tempParent;

	// Token: 0x0400119F RID: 4511
	public GameObject fixbody;

	// Token: 0x040011A0 RID: 4512
	public bool isHolding;

	// Token: 0x040011A1 RID: 4513
	public bool seetoput;
}
