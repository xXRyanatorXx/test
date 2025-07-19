using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A3 RID: 419
public class PickupHand : MonoBehaviour
{
	// Token: 0x060009A4 RID: 2468 RVA: 0x000601B0 File Offset: 0x0005E3B0
	private void Start()
	{
		this.Controller = GameObject.Find("Player").GetComponent<FirstPersonAIO>();
		this.tempParent = this.Controller.gameObject.GetComponent<tools>().hand1;
		this.LoadingSlider = this.Controller.gameObject.GetComponent<tools>().LoadingSlider;
		if (this.trailer)
		{
			base.gameObject.transform.SetParent(this.trailer.transform);
		}
		if (tools.MPrunning && !base.GetComponent<MPobject>() && base.transform.name == "TrailerHandle")
		{
			base.gameObject.AddComponent<MPobject>();
		}
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x00060268 File Offset: 0x0005E468
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
		if (this.LoadingTime > 0f)
		{
			this.LoadingTime -= Time.deltaTime;
		}
		if (this.removalloading)
		{
			this.LoadingSlider.value = this.LoadingTime * 2f;
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.OnMouseUp();
		}
		if (!this.isHolding && !this.removalloading)
		{
			base.enabled = false;
		}
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x000604C9 File Offset: 0x0005E6C9
	private void LateUpdate()
	{
		if (this.seetoput)
		{
			tools.canput = true;
		}
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x000604D9 File Offset: 0x0005E6D9
	public void TakeInHand()
	{
		base.StartCoroutine(this.LateClick());
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x000604E8 File Offset: 0x0005E6E8
	private IEnumerator LateClick()
	{
		yield return 0;
		if (!tools.Clicked)
		{
			tools.Clicked = true;
			RaycastHit hit;
			if (base.gameObject.GetComponent<Partinfo>().tightnuts == 0f && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1.7f, 1 << LayerMask.NameToLayer("Windows")))
			{
				if (base.transform.name == "TrailerHandle")
				{
					if (this.trailer && this.TrailerJoint)
					{
						UnityEngine.Object.Destroy(this.TrailerJoint);
					}
					this.Removing(hit);
				}
				else if (this.PARENTHaveToBeREmoved)
				{
					if (base.transform.parent.parent.parent == null)
					{
						if (base.transform.parent)
						{
							this.cc = base.StartCoroutine(this.RemovalLoading(hit));
						}
						else
						{
							this.Removing(hit);
						}
					}
				}
				else if (this.PartThatNeedsToBeOffname != "")
				{
					bool flag = true;
					foreach (Transform transform in base.transform.root.GetComponentsInChildren<Transform>())
					{
						if (transform.name == this.PartThatNeedsToBeOffname && !transform.gameObject.transform.GetComponent<transparents>())
						{
							flag = false;
						}
					}
					if (flag)
					{
						if (base.transform.parent)
						{
							this.cc = base.StartCoroutine(this.RemovalLoading(hit));
						}
						else
						{
							this.Removing(hit);
						}
					}
				}
				else if (base.transform.parent)
				{
					this.cc = base.StartCoroutine(this.RemovalLoading(hit));
				}
				else
				{
					this.Removing(hit);
				}
			}
		}
		yield break;
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x000604F7 File Offset: 0x0005E6F7
	private IEnumerator RemovalLoading(RaycastHit hit)
	{
		this.LoadingTime = 0.5f;
		base.enabled = true;
		this.removalloading = true;
		this.Controller.ControllerPause();
		yield return new WaitForSeconds(0.5f);
		if (this.removalloading)
		{
			this.Removing(hit);
		}
		this.Controller.ControllerUnPause();
		yield break;
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x00060510 File Offset: 0x0005E710
	public void Removing(RaycastHit hit)
	{
		this.tempParent.transform.position = hit.point;
		this.isHolding = true;
		base.enabled = true;
		if (base.transform.root.GetComponent<MainCarProperties>())
		{
			base.transform.root.GetComponent<MainCarProperties>().Traielerinsideitems = null;
		}
		tools.helditem = base.gameObject.name;
		base.GetComponent<CarProperties>().picked = true;
		base.gameObject.GetComponent<CarProperties>().Remove();
		if (base.gameObject.GetComponent<Rigidbody>() == null)
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		base.gameObject.GetComponent<Rigidbody>().useGravity = false;
		base.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
		base.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		base.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		base.gameObject.transform.SetParent(this.tempParent.transform);
		if (Vector3.Distance(this.tempParent.transform.position, Camera.main.transform.position) > 1.3f)
		{
			this.tempParent.transform.position = Vector3.MoveTowards(this.tempParent.transform.position, Camera.main.transform.position, Vector3.Distance(this.tempParent.transform.position, Camera.main.transform.position) - 1.3f);
		}
		base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
		base.gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
		if (base.GetComponent<MPobject>())
		{
			if (this.trailer != null)
			{
				tools.NetworkPLayer.pickup(this.trailer.GetComponent<MPobject>().networkDummy);
			}
			base.GetComponent<MPobject>().networkDummy.RemoveHand();
			tools.NetworkPLayer.pickup(base.GetComponent<MPobject>().networkDummy);
			return;
		}
		this.RemovingContinue();
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x00060730 File Offset: 0x0005E930
	public void RemovingContinue()
	{
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
		{
			if (gameObject.name == base.gameObject.name)
			{
				gameObject.GetComponent<transparents>().Recheck();
			}
		}
		if (base.GetComponent<MPobject>())
		{
			base.GetComponent<MPobject>().networkDummy.DestroyJoint();
		}
		else if (base.GetComponent<FixedJoint>())
		{
			UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
		}
		base.gameObject.GetComponent<CarProperties>().Remove();
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy && !base.GetComponent<MPobject>().networkDummy.hasAuthority)
		{
			base.gameObject.transform.SetParent(null);
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
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00060894 File Offset: 0x0005EA94
	private void OnMouseUp()
	{
		if (this.removalloading)
		{
			this.removalloading = false;
			this.LoadingSlider.value = 0f;
			if (this.cc != null)
			{
				base.StopCoroutine(this.cc);
			}
			this.Controller.ControllerUnPause();
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
			if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.DropOnGround();
			}
			base.gameObject.transform.SetParent(null);
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("TransparentParts")) && raycastHit.collider.gameObject.name == base.gameObject.name)
			{
				this.placetoput = raycastHit.collider.gameObject;
				this.seetoput = true;
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
					base.gameObject.transform.position = this.placetoput.transform.position;
					base.gameObject.transform.rotation = this.placetoput.transform.rotation;
				}
				else
				{
					this.Attach(this.placetoput);
				}
			}
			base.gameObject.GetComponent<Partinfo>().CheckGround();
		}
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x00060BF0 File Offset: 0x0005EDF0
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

	// Token: 0x060009AE RID: 2478 RVA: 0x00060C90 File Offset: 0x0005EE90
	public void Attach(GameObject place)
	{
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
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			carProperties.Start();
			carProperties.Attach();
		}
		if (base.transform.name == "TrailerHandle" && place.transform.parent.GetComponent<Partinfo>().tightnuts > 0f)
		{
			base.gameObject.transform.SetParent(place.transform.root);
			this.TrailerJoint = this.trailer.AddComponent<ConfigurableJoint>();
			this.TrailerJoint.connectedBody = place.transform.root.GetComponent<Rigidbody>();
			if (base.transform.name == "TrailerHandle" && place.transform.root.GetComponent<MainCarProperties>())
			{
				place.transform.root.GetComponent<MainCarProperties>().Traielerinsideitems = this.trailer.GetComponent<MainTrailerProperties>().insideitems;
			}
			this.TrailerJoint.autoConfigureConnectedAnchor = false;
			this.TrailerJoint.anchor = this.trailer.GetComponent<MainTrailerProperties>().HookPoint;
			this.TrailerJoint.connectedAnchor = base.transform.localPosition;
			base.gameObject.transform.SetParent(place.transform);
			this.TrailerJoint.xMotion = ConfigurableJointMotion.Locked;
			this.TrailerJoint.yMotion = ConfigurableJointMotion.Locked;
			this.TrailerJoint.zMotion = ConfigurableJointMotion.Locked;
			this.TrailerJoint.breakForce = 200000f;
			this.TrailerJoint.enableCollision = true;
		}
		if (base.GetComponent<FixedJoint>())
		{
			UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
		}
		if (base.GetComponent<HingeJoint>())
		{
			UnityEngine.Object.Destroy(base.GetComponent<HingeJoint>());
		}
		if (base.transform.name != "TrailerHandle")
		{
			UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		}
		if (base.GetComponent<MPobject>())
		{
			base.StartCoroutine(this.LaterAttach());
		}
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x00060F2C File Offset: 0x0005F12C
	public IEnumerator LaterAttach()
	{
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

	// Token: 0x060009B0 RID: 2480 RVA: 0x00060F3B File Offset: 0x0005F13B
	private void OnJointBreak(float breakForce)
	{
		this.BRAKE();
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x00060F43 File Offset: 0x0005F143
	public void BRAKE()
	{
		if (base.GetComponent<MPobject>())
		{
			base.GetComponent<MPobject>().networkDummy.BRAKE();
			return;
		}
		this.BRAKE2();
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x00060F6C File Offset: 0x0005F16C
	public void BRAKE2()
	{
		if (base.gameObject.GetComponent<Rigidbody>() == null)
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		if (base.transform.root.GetComponent<MainCarProperties>())
		{
			base.transform.root.GetComponent<MainCarProperties>().Traielerinsideitems.Stand();
		}
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
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x00061074 File Offset: 0x0005F274
	public void CarSold()
	{
		if (this.TrailerJoint)
		{
			UnityEngine.Object.Destroy(this.TrailerJoint);
		}
		if (base.gameObject.GetComponent<Rigidbody>() == null)
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		base.gameObject.GetComponent<Rigidbody>().useGravity = true;
		base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
		base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		base.gameObject.GetComponent<Rigidbody>().mass = base.gameObject.GetComponent<Partinfo>().weight;
		base.gameObject.transform.SetParent(null);
		base.gameObject.GetComponent<Partinfo>().fixedwelds = 0f;
		base.gameObject.GetComponent<Partinfo>().tightnuts = 0f;
	}

	// Token: 0x040011A2 RID: 4514
	private float throwForce;

	// Token: 0x040011A3 RID: 4515
	private Vector3 objectPos;

	// Token: 0x040011A4 RID: 4516
	public bool PARENTHaveToBeREmoved;

	// Token: 0x040011A5 RID: 4517
	public string PartThatNeedsToBeOffname;

	// Token: 0x040011A6 RID: 4518
	public bool canHold = true;

	// Token: 0x040011A7 RID: 4519
	public GameObject placetoput;

	// Token: 0x040011A8 RID: 4520
	public GameObject tempParent;

	// Token: 0x040011A9 RID: 4521
	public GameObject fixbody;

	// Token: 0x040011AA RID: 4522
	public bool isHolding;

	// Token: 0x040011AB RID: 4523
	public bool seetoput;

	// Token: 0x040011AC RID: 4524
	public FirstPersonAIO Controller;

	// Token: 0x040011AD RID: 4525
	public GameObject trailer;

	// Token: 0x040011AE RID: 4526
	public ConfigurableJoint TrailerJoint;

	// Token: 0x040011AF RID: 4527
	public bool removalloading;

	// Token: 0x040011B0 RID: 4528
	public Slider LoadingSlider;

	// Token: 0x040011B1 RID: 4529
	public float LoadingTime;

	// Token: 0x040011B2 RID: 4530
	public Coroutine cc;
}
