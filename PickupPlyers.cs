using System;
using UnityEngine;

// Token: 0x020001AB RID: 427
public class PickupPlyers : MonoBehaviour
{
	// Token: 0x060009E4 RID: 2532 RVA: 0x0006251E File Offset: 0x0006071E
	private void Awake()
	{
		this.tempParent = GameObject.Find("hand");
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x00062530 File Offset: 0x00060730
	private void Update()
	{
		this.placetoput = null;
		this.seetoput = false;
		RaycastHit raycastHit;
		if (tools.LookingAtTransparent && tools.helditem == base.gameObject.name && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.4f, 1 << LayerMask.NameToLayer("TransparentParts")) && raycastHit.collider.gameObject.name == base.gameObject.name)
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
		}
		if (!this.isHolding)
		{
			base.enabled = false;
		}
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x00062720 File Offset: 0x00060920
	private void LateUpdate()
	{
		if (this.seetoput)
		{
			tools.canput = true;
		}
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x00062730 File Offset: 0x00060930
	private void OnMouseDown()
	{
		RaycastHit raycastHit;
		if (base.gameObject.GetComponent<Partinfo>().tightnuts == 0f && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.4f, 1 << LayerMask.NameToLayer("Windows")))
		{
			this.tempParent.transform.position = raycastHit.point;
			this.isHolding = true;
			base.enabled = true;
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
			UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			base.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			base.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			base.gameObject.transform.SetParent(this.tempParent.transform);
			base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			base.gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
		}
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x000628AC File Offset: 0x00060AAC
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
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
			base.gameObject.AddComponent<MeshCollider>().convex = true;
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			base.gameObject.GetComponent<Rigidbody>().mass = base.gameObject.GetComponent<Partinfo>().weight;
			base.gameObject.transform.SetParent(null);
			if (this.seetoput && !base.GetComponent<CarProperties>().Ruined)
			{
				base.gameObject.transform.position = this.placetoput.transform.position;
				base.gameObject.transform.rotation = this.placetoput.transform.rotation;
				base.gameObject.transform.SetParent(this.placetoput.transform);
				if (base.transform.root.GetComponent<CarProperties>())
				{
					base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
				}
				if (base.transform.root.GetComponent<MainCarProperties>())
				{
					base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
				}
				base.gameObject.GetComponent<Partinfo>().attach();
				base.gameObject.GetComponent<Partinfo>().tightnuts = 1f;
			}
		}
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x00062A84 File Offset: 0x00060C84
	private void OnJointBreak(float breakForce)
	{
		base.gameObject.GetComponent<Rigidbody>().useGravity = true;
		base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
		base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		base.gameObject.GetComponent<Rigidbody>().mass = base.gameObject.GetComponent<Partinfo>().weight;
		base.gameObject.transform.SetParent(null);
		base.gameObject.GetComponent<Partinfo>().fixedwelds = 0f;
		base.gameObject.GetComponent<Partinfo>().tightnuts = 0f;
	}

	// Token: 0x040011DA RID: 4570
	private float throwForce;

	// Token: 0x040011DB RID: 4571
	private Vector3 objectPos;

	// Token: 0x040011DC RID: 4572
	public bool canHold = true;

	// Token: 0x040011DD RID: 4573
	public GameObject placetoput;

	// Token: 0x040011DE RID: 4574
	public GameObject tempParent;

	// Token: 0x040011DF RID: 4575
	public GameObject fixbody;

	// Token: 0x040011E0 RID: 4576
	public bool isHolding;

	// Token: 0x040011E1 RID: 4577
	public bool seetoput;
}
