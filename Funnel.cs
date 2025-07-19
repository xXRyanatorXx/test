using System;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class Funnel : MonoBehaviour
{
	// Token: 0x06000509 RID: 1289 RVA: 0x000297A2 File Offset: 0x000279A2
	private void Start()
	{
		this.tempParent = GameObject.Find("hand");
		this.SphereCOl = GameObject.Find("SphereCollider");
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x000297C4 File Offset: 0x000279C4
	private void Update()
	{
		this.placetoput = null;
		this.seetoput = false;
		RaycastHit raycastHit;
		if (tools.LookingAtTransparent && tools.helditem == base.gameObject.name && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1f, 1 << LayerMask.NameToLayer("TransparentParts")) && raycastHit.collider.gameObject.name == base.gameObject.name)
		{
			this.placetoput = raycastHit.collider.gameObject;
			this.seetoput = true;
			tools.canput = true;
		}
		if (Input.GetMouseButton(2) && this.isHolding && this.throwForce < 1000f)
		{
			this.throwForce += 15f;
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
			if (base.gameObject.GetComponent<CarProperties>())
			{
				base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.OnMouseUp();
		}
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x0002999C File Offset: 0x00027B9C
	public void OnMouseDown()
	{
		RaycastHit raycastHit;
		if (!tools.sitting && !tools.Clicked && !tools.cooldown && tools.tool != 9 && tools.tool != 8 && tools.tool != 12 && tools.tool != 11 && tools.tool != 14 && tools.tool != 4 && tools.tool != 7 && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1f, 1 << LayerMask.NameToLayer("Items")))
		{
			this.tempParent.transform.position = raycastHit.point;
			this.isHolding = true;
			base.enabled = true;
			tools.Clicked = true;
			tools.helditem = base.gameObject.name;
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
			{
				if (gameObject.name == base.gameObject.name)
				{
					gameObject.GetComponent<transparents>().Recheck();
				}
			}
			base.GetComponent<MeshCollider>().isTrigger = false;
			base.gameObject.GetComponent<Rigidbody>().useGravity = false;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
			UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			base.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			base.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			base.gameObject.transform.SetParent(this.tempParent.transform);
			base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00029B84 File Offset: 0x00027D84
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
			base.gameObject.transform.SetParent(null);
			CarProperties[] componentsInChildren = base.GetComponentsInChildren<CarProperties>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].RestartColliders();
			}
			if (base.transform.root.GetComponent<CarProperties>())
			{
				base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
			}
			if (base.transform.root.GetComponent<MainCarProperties>())
			{
				base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
			}
			if (this.fluid && this.fluid.Cup)
			{
				this.fluid.Cup.SetActive(true);
				this.fluid.FunnelCollider.SetActive(false);
			}
			this.fluid = null;
			if (this.seetoput)
			{
				base.gameObject.transform.position = this.placetoput.transform.position;
				base.gameObject.transform.rotation = this.placetoput.transform.rotation;
				base.gameObject.transform.SetParent(this.placetoput.transform);
				this.fluid = base.transform.parent.parent.GetComponent<FLUID>();
				if (this.fluid.Cup)
				{
					this.fluid.Cup.SetActive(false);
					this.fluid.FunnelCollider.SetActive(true);
				}
				base.gameObject.AddComponent<FixedJoint>();
				base.gameObject.GetComponent<FixedJoint>().breakForce = 5000f;
				base.gameObject.GetComponent<FixedJoint>().connectedBody = base.gameObject.transform.root.GetComponent<Rigidbody>();
				return;
			}
			base.enabled = false;
		}
	}

	// Token: 0x040006BB RID: 1723
	private float throwForce;

	// Token: 0x040006BC RID: 1724
	private Vector3 objectPos;

	// Token: 0x040006BD RID: 1725
	public bool canHold = true;

	// Token: 0x040006BE RID: 1726
	public GameObject placetoput;

	// Token: 0x040006BF RID: 1727
	public GameObject tempParent;

	// Token: 0x040006C0 RID: 1728
	public GameObject fixbody;

	// Token: 0x040006C1 RID: 1729
	public bool isHolding;

	// Token: 0x040006C2 RID: 1730
	public bool seetoput;

	// Token: 0x040006C3 RID: 1731
	public GameObject SphereCOl;

	// Token: 0x040006C4 RID: 1732
	public FLUID fluid;
}
