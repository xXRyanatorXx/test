using System;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class BatteryCharger : MonoBehaviour
{
	// Token: 0x06000603 RID: 1539 RVA: 0x0002FB4C File Offset: 0x0002DD4C
	private void Start()
	{
		this.tempParent = GameObject.Find("hand");
		this.SphereCOl = GameObject.Find("SphereCollider");
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0002FB70 File Offset: 0x0002DD70
	public void PassTime()
	{
		if (this.Battery && this.Battery.GetComponent<CarProperties>().BatteryCharge < 12.8f && this.Battery.GetComponent<CarProperties>().Condition > 0.1f)
		{
			this.Battery.GetComponent<CarProperties>().BatteryCharge += 0.5f;
			if (this.Battery.GetComponent<CarProperties>().BatteryCharge > 12.8f)
			{
				this.Battery.GetComponent<CarProperties>().BatteryCharge = 12.8f;
			}
		}
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0002FC00 File Offset: 0x0002DE00
	private void Update()
	{
		if (this.Battery && this.Battery.GetComponent<CarProperties>().BatteryCharge < 12.8f && this.Battery.GetComponent<CarProperties>().Condition > 0.1f)
		{
			this.Battery.GetComponent<CarProperties>().BatteryCharge += 0.0001f * Time.deltaTime;
		}
		if (!this.Battery)
		{
			this.Needle.transform.localRotation = Quaternion.Euler(-40f, 0f, 0f);
		}
		else
		{
			this.Needle.transform.localRotation = Quaternion.Euler(30f - (this.Battery.GetComponent<CarProperties>().BatteryCharge - 11.8f) * 60f, 0f, 0f);
		}
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

	// Token: 0x06000606 RID: 1542 RVA: 0x0002FEA8 File Offset: 0x0002E0A8
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

	// Token: 0x06000607 RID: 1543 RVA: 0x00030090 File Offset: 0x0002E290
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
			this.Battery = null;
			if (this.seetoput)
			{
				base.gameObject.transform.position = this.placetoput.transform.position;
				base.gameObject.transform.rotation = this.placetoput.transform.rotation;
				base.gameObject.transform.SetParent(this.placetoput.transform);
				this.Battery = base.transform.parent.parent.gameObject;
				if (base.transform.root.GetComponent<CarProperties>())
				{
					this.Battery.GetComponent<CarProperties>().PreventChildCollisions();
				}
				if (base.transform.root.GetComponent<MainCarProperties>())
				{
					this.Battery.GetComponent<CarProperties>().PreventChildCollisions();
				}
				base.gameObject.AddComponent<FixedJoint>();
				base.gameObject.GetComponent<FixedJoint>().breakForce = 5000f;
				base.gameObject.GetComponent<FixedJoint>().connectedBody = base.gameObject.transform.root.GetComponent<Rigidbody>();
				return;
			}
			base.enabled = false;
		}
	}

	// Token: 0x04000906 RID: 2310
	public GameObject Battery;

	// Token: 0x04000907 RID: 2311
	public GameObject Needle;

	// Token: 0x04000908 RID: 2312
	private float throwForce;

	// Token: 0x04000909 RID: 2313
	private Vector3 objectPos;

	// Token: 0x0400090A RID: 2314
	public bool canHold = true;

	// Token: 0x0400090B RID: 2315
	public GameObject placetoput;

	// Token: 0x0400090C RID: 2316
	public GameObject tempParent;

	// Token: 0x0400090D RID: 2317
	public GameObject fixbody;

	// Token: 0x0400090E RID: 2318
	public bool isHolding;

	// Token: 0x0400090F RID: 2319
	public bool seetoput;

	// Token: 0x04000910 RID: 2320
	public GameObject SphereCOl;
}
