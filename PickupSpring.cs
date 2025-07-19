using System;
using Mirror;
using UnityEngine;

// Token: 0x020001AC RID: 428
public class PickupSpring : MonoBehaviour
{
	// Token: 0x060009EB RID: 2539 RVA: 0x00062B2E File Offset: 0x00060D2E
	private void Awake()
	{
		this.tempParent = GameObject.Find("hand");
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x00062B40 File Offset: 0x00060D40
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
		if (Input.GetMouseButtonUp(2) || Input.GetMouseButtonUp(1))
		{
			if (this.isHolding)
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
			}
			else if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.DropOnGround();
			}
		}
		if (!this.isHolding)
		{
			base.enabled = false;
		}
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x00062D4F File Offset: 0x00060F4F
	private void LateUpdate()
	{
		if (this.seetoput)
		{
			tools.canput = true;
		}
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x00062D60 File Offset: 0x00060F60
	private void OnMouseDown()
	{
		if (tools.tool == 24 || tools.UIisOpen)
		{
			return;
		}
		RaycastHit raycastHit;
		if (!tools.Clicked && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("LooseParts")))
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
			base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.DestroyJoint();
			}
			else
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

	// Token: 0x060009EF RID: 2543 RVA: 0x00062FB0 File Offset: 0x000611B0
	private void OnMouseUp()
	{
		if (base.GetComponent<MPobject>())
		{
			tools.NetworkPLayer.pickup(base.GetComponent<MPobject>().networkDummy);
		}
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
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
			base.gameObject.AddComponent<MeshCollider>().convex = true;
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
					if (this.placetoput.transform.parent.parent.GetComponent<MPobject>())
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
					this.Attach(this.placetoput);
				}
			}
			base.gameObject.GetComponent<Partinfo>().CheckGround();
		}
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x000631FC File Offset: 0x000613FC
	public void Attach(GameObject place)
	{
		base.gameObject.transform.position = place.transform.position;
		base.gameObject.transform.rotation = place.transform.rotation;
		base.gameObject.transform.localScale = place.transform.localScale;
		base.gameObject.transform.SetParent(place.transform);
		if (base.transform.root.GetComponent<CarProperties>())
		{
			base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
		}
		if (base.transform.root.GetComponent<MainCarProperties>())
		{
			base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
		}
		base.gameObject.GetComponent<Partinfo>().attach();
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x000632CE File Offset: 0x000614CE
	private void OnJointBreak(float breakForce)
	{
		this.BRAKE();
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x000632D6 File Offset: 0x000614D6
	public void BRAKE()
	{
		if (base.transform.parent.GetComponent<MPobject>())
		{
			base.transform.parent.GetComponent<MPobject>().networkDummy.BRAKE();
			return;
		}
		this.BRAKE2();
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x00063310 File Offset: 0x00061510
	public void BRAKE2()
	{
		base.gameObject.GetComponent<Rigidbody>().useGravity = true;
		base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
		base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		base.gameObject.GetComponent<Rigidbody>().mass = base.gameObject.GetComponent<Partinfo>().weight;
		base.gameObject.transform.SetParent(null);
		base.gameObject.GetComponent<Partinfo>().fixedwelds = 0f;
		base.gameObject.GetComponent<Partinfo>().tightnuts = 0f;
		base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	// Token: 0x040011E2 RID: 4578
	private float throwForce;

	// Token: 0x040011E3 RID: 4579
	private Vector3 objectPos;

	// Token: 0x040011E4 RID: 4580
	public bool canHold = true;

	// Token: 0x040011E5 RID: 4581
	public GameObject placetoput;

	// Token: 0x040011E6 RID: 4582
	public GameObject tempParent;

	// Token: 0x040011E7 RID: 4583
	public GameObject fixbody;

	// Token: 0x040011E8 RID: 4584
	public bool isHolding;

	// Token: 0x040011E9 RID: 4585
	public bool seetoput;
}
