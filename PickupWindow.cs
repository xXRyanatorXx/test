using System;
using System.Collections;
using Mirror;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class PickupWindow : MonoBehaviour
{
	// Token: 0x060009F5 RID: 2549 RVA: 0x000633DE File Offset: 0x000615DE
	private void Awake()
	{
		this.tempParent = GameObject.Find("hand");
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x000633F0 File Offset: 0x000615F0
	private void Update()
	{
		this.placetoput = null;
		this.seetoput = false;
		RaycastHit raycastHit;
		if (tools.LookingAtTransparent && tools.helditem == base.gameObject.name && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("TransparentParts")) && raycastHit.collider.gameObject.name == base.gameObject.name)
		{
			this.placetoput = raycastHit.collider.gameObject;
			this.seetoput = true;
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

	// Token: 0x060009F7 RID: 2551 RVA: 0x000635FD File Offset: 0x000617FD
	private void LateUpdate()
	{
		if (this.seetoput)
		{
			tools.canput = true;
		}
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x00063610 File Offset: 0x00061810
	public void OnMouseDown()
	{
		if (tools.tool == 24 || tools.UIisOpen)
		{
			return;
		}
		RaycastHit raycastHit;
		if (!tools.sitting && !tools.Clicked && base.gameObject.GetComponent<Partinfo>().tightnuts == 0f && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("Windows")))
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
			if (base.GetComponent<MPobject>())
			{
				if (base.GetComponent<MPobject>())
				{
					tools.NetworkPLayer.pickup(base.GetComponent<MPobject>().networkDummy);
				}
				base.GetComponent<MPobject>().networkDummy.DestroyJoint();
			}
			if (base.GetComponent<FixedJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
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
		}
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x0006388C File Offset: 0x00061A8C
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
			CarProperties[] componentsInChildren = base.GetComponentsInChildren<CarProperties>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].RestartColliders();
			}
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			base.gameObject.GetComponent<Rigidbody>().mass = base.gameObject.GetComponent<Partinfo>().weight;
			base.gameObject.transform.SetParent(null);
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
					base.gameObject.transform.position = this.placetoput.transform.position;
					base.gameObject.transform.rotation = this.placetoput.transform.rotation;
				}
				else
				{
					this.Attach(this.placetoput);
				}
			}
			else if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.DropOnGround();
			}
			base.gameObject.GetComponent<Partinfo>().CheckGround();
		}
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x00063B30 File Offset: 0x00061D30
	public void Attach(GameObject place)
	{
		if (base.GetComponent<FixedJoint>())
		{
			UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
		}
		base.gameObject.transform.position = place.transform.position;
		base.gameObject.transform.rotation = place.transform.rotation;
		base.gameObject.transform.SetParent(place.transform);
		if (base.transform.root.GetComponent<CarProperties>())
		{
			base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
		}
		if (base.transform.root.GetComponent<MainCarProperties>())
		{
			base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
		}
		DisableRend[] componentsInChildren = base.gameObject.GetComponentsInChildren<DisableRend>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enableREND();
		}
		if (base.transform.parent != null)
		{
			base.transform.parent.gameObject.GetComponent<transparents>().InstallATTACHABLES();
			base.transform.parent.gameObject.GetComponent<transparents>().HaveAttached = true;
		}
		base.gameObject.GetComponent<Partinfo>().attach();
		base.gameObject.GetComponent<Partinfo>().tightnuts = 1f;
		if (base.GetComponent<MPobject>())
		{
			base.StartCoroutine(this.LaterAttach());
		}
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x00063C98 File Offset: 0x00061E98
	public IEnumerator LaterAttach()
	{
		yield return new WaitForSeconds(1f);
		if (base.transform.parent)
		{
			base.gameObject.transform.position = base.transform.parent.position;
			base.gameObject.transform.rotation = base.transform.parent.rotation;
		}
		yield return new WaitForSeconds(3f);
		if (base.transform.parent)
		{
			base.gameObject.transform.position = base.transform.parent.position;
			base.gameObject.transform.rotation = base.transform.parent.rotation;
		}
		yield break;
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x00063CA8 File Offset: 0x00061EA8
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

	// Token: 0x060009FD RID: 2557 RVA: 0x00063D48 File Offset: 0x00061F48
	private void OnJointBreak(float breakForce)
	{
		this.BRAKE();
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x00063D50 File Offset: 0x00061F50
	public void BRAKE()
	{
		if (base.GetComponent<MPobject>())
		{
			base.GetComponent<MPobject>().networkDummy.BRAKE();
			return;
		}
		this.BRAKE2();
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x00063D78 File Offset: 0x00061F78
	public void BRAKE2()
	{
		base.gameObject.GetComponent<Rigidbody>().useGravity = true;
		base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
		base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		if (base.GetComponent<MPobject>())
		{
			base.GetComponent<MPobject>().networkDummy.DropOnGround();
		}
		base.gameObject.GetComponent<Rigidbody>().mass = base.gameObject.GetComponent<Partinfo>().weight;
		base.gameObject.transform.SetParent(null);
		base.gameObject.GetComponent<Partinfo>().fixedwelds = 0f;
		base.gameObject.GetComponent<Partinfo>().tightnuts = 0f;
		if (base.GetComponent<MPobject>())
		{
			base.GetComponent<MPobject>().networkDummy.EnableMovment();
		}
	}

	// Token: 0x040011EA RID: 4586
	private float throwForce;

	// Token: 0x040011EB RID: 4587
	private Vector3 objectPos;

	// Token: 0x040011EC RID: 4588
	public bool canHold = true;

	// Token: 0x040011ED RID: 4589
	public GameObject placetoput;

	// Token: 0x040011EE RID: 4590
	public GameObject tempParent;

	// Token: 0x040011EF RID: 4591
	public GameObject fixbody;

	// Token: 0x040011F0 RID: 4592
	public bool isHolding;

	// Token: 0x040011F1 RID: 4593
	public bool seetoput;
}
