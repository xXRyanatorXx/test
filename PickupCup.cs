using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200019E RID: 414
public class PickupCup : MonoBehaviour
{
	// Token: 0x0600097A RID: 2426 RVA: 0x0005D8B8 File Offset: 0x0005BAB8
	private void Start()
	{
		if (!this.Started && this.Fluid)
		{
			this.FluidName = this.Fluid.name.ToString();
		}
		this.Started = true;
		if (this.Dipstick)
		{
			this.DipstickOil.transform.localScale = new Vector3(1f, 1f, 0f);
		}
		if (tools.MPrunning && !base.GetComponent<MPobject>())
		{
			base.gameObject.AddComponent<MPobject>();
		}
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x0005D944 File Offset: 0x0005BB44
	private void Awake()
	{
		this.Controller = GameObject.Find("Player").GetComponent<FirstPersonAIO>();
		this.tempParent = this.Controller.gameObject.GetComponent<tools>().hand1;
		this.LoadingSlider = this.Controller.gameObject.GetComponent<tools>().LoadingSlider;
		this.ReadHand = this.Controller.gameObject.GetComponent<tools>().ReadHand;
		this.SphereCOl = this.Controller.gameObject.GetComponent<tools>().SphereCOl;
		if (this.Fluid)
		{
			this.Fluid.GetComponent<FLUID>().IsOpen = false;
			UnityEngine.Object.Destroy(this.Fluid.GetComponent<Rigidbody>());
		}
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x0005DA00 File Offset: 0x0005BC00
	private IEnumerator RestartWrench()
	{
		tools.tool = 1;
		yield return 1;
		tools.tool = 2;
		if (this.SphereCOl.GetComponent<DisablerCollider>())
		{
			this.SphereCOl.GetComponent<DisablerCollider>().RestartJump();
		}
		yield break;
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0005DA10 File Offset: 0x0005BC10
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
		if (Input.GetMouseButtonUp(2) || Input.GetMouseButtonUp(1))
		{
			if (this.cc != null)
			{
				base.StopCoroutine(this.cc);
			}
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
		}
		if (this.LoadingTime > 0f)
		{
			this.LoadingTime -= Time.deltaTime;
		}
		if (this.removalloading)
		{
			this.LoadingSlider.value = this.LoadingTime * 2f;
		}
		if (!this.isHolding && !this.removalloading)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0005DC60 File Offset: 0x0005BE60
	public void TakeInHand()
	{
		if (this.GasCanCup)
		{
			if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.OpenGasCup(this.Fluid.GetComponent<FLUID>().IsOpen);
			}
			else if (!this.Fluid.GetComponent<FLUID>().IsOpen)
			{
				this.Fluid.GetComponent<FLUID>().IsOpen = true;
				this.Fluid.GetComponent<FLUID>().enabled = true;
				this.Fluid.GetComponent<FLUID>().Start();
				if (this.Fluid.transform.root.GetComponent<CarProperties>())
				{
					base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
				}
				if (this.Fluid.transform.root.GetComponent<MainCarProperties>())
				{
					base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
				}
				if (this.Fluid.GetComponent<Rigidbody>() == null)
				{
					this.Fluid.AddComponent<Rigidbody>();
				}
				this.Fluid.GetComponent<Rigidbody>().isKinematic = true;
				base.transform.Rotate(-120f, 0f, 0f, Space.Self);
			}
			else
			{
				this.Fluid.GetComponent<FLUID>().IsOpen = false;
				UnityEngine.Object.Destroy(this.Fluid.GetComponent<Rigidbody>());
				UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
				base.transform.Rotate(120f, 0f, 0f, Space.Self);
			}
		}
		RaycastHit hit;
		if (!this.GasCanCup && !tools.cooldown && tools.tool != 8 && tools.tool != 12 && tools.tool != 11 && tools.tool != 14 && !tools.sitting && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1.7f, 1 << LayerMask.NameToLayer("Windows")))
		{
			if (base.transform.parent)
			{
				this.cc = base.StartCoroutine(this.RemovalLoading(hit));
			}
			else
			{
				this.Removing(hit);
			}
			tools.helditem = base.gameObject.name;
			tools.PartInHand = base.gameObject;
		}
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x0005DEAF File Offset: 0x0005C0AF
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

	// Token: 0x06000980 RID: 2432 RVA: 0x0005DEC8 File Offset: 0x0005C0C8
	public void OpenGasCup(bool open)
	{
		if (!open)
		{
			this.Fluid.GetComponent<FLUID>().IsOpen = true;
			this.Fluid.GetComponent<FLUID>().enabled = true;
			this.Fluid.GetComponent<FLUID>().Start();
			if (this.Fluid.transform.root.GetComponent<CarProperties>())
			{
				base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
			}
			if (this.Fluid.transform.root.GetComponent<MainCarProperties>())
			{
				base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
			}
			if (this.Fluid.GetComponent<Rigidbody>() == null)
			{
				this.Fluid.AddComponent<Rigidbody>();
			}
			this.Fluid.GetComponent<Rigidbody>().isKinematic = true;
			base.transform.Rotate(-120f, 0f, 0f, Space.Self);
			return;
		}
		this.Fluid.GetComponent<FLUID>().IsOpen = false;
		UnityEngine.Object.Destroy(this.Fluid.GetComponent<Rigidbody>());
		UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		base.transform.Rotate(120f, 0f, 0f, Space.Self);
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x0005DFF8 File Offset: 0x0005C1F8
	public void Removing(RaycastHit hit)
	{
		this.tempParent.transform.position = hit.point;
		this.isHolding = true;
		tools.helditem = base.gameObject.name;
		tools.PartInHand = base.gameObject;
		base.enabled = true;
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
		{
			if (gameObject.name == base.gameObject.name)
			{
				gameObject.GetComponent<transparents>().Recheck();
			}
		}
		if (base.gameObject.GetComponent<Rigidbody>() == null)
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		base.gameObject.GetComponent<Rigidbody>().useGravity = false;
		base.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
		if (base.GetComponent<FixedJoint>())
		{
			UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
		}
		base.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		base.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		if (!this.Dipstick)
		{
			base.gameObject.transform.SetParent(this.tempParent.transform);
			if (Vector3.Distance(this.tempParent.transform.position, Camera.main.transform.position) > 1f)
			{
				this.tempParent.transform.position = Vector3.MoveTowards(this.tempParent.transform.position, Camera.main.transform.position, Vector3.Distance(this.tempParent.transform.position, Camera.main.transform.position) - 1f);
			}
		}
		if (this.Dipstick)
		{
			if (base.transform.parent != null && base.transform.parent.parent != null && (base.transform.parent.parent.GetComponent<CarProperties>().EngineBlock || this.Fluid != null))
			{
				if (base.transform.parent.parent.GetComponent<CarProperties>().EngineBlock)
				{
					foreach (Transform transform in base.transform.root.GetComponentsInChildren<Transform>())
					{
						if (transform.name == "OilFluidContainer")
						{
							this.OIL = transform.gameObject;
						}
					}
				}
				if (this.Fluid)
				{
					this.OIL = this.Fluid.gameObject;
				}
				if (this.OIL)
				{
					this.lerpedColor = Color.Lerp(Color.black, Color.yellow, this.OIL.transform.GetComponent<FLUID>().Condition);
					this.DipstickOil.transform.GetComponent<Renderer>().material.color = this.lerpedColor;
					this.DipstickOil.transform.localScale = new Vector3(1f, 1f, this.OIL.transform.GetComponent<FLUID>().FluidSize / this.OIL.transform.GetComponent<FLUID>().ContainerSize);
				}
				else
				{
					this.DipstickOil.transform.localScale = new Vector3(1f, 1f, 0f);
				}
			}
			this.OIL = null;
			base.gameObject.transform.SetParent(this.ReadHand.transform);
			base.gameObject.transform.position = this.ReadHand.transform.position;
			base.gameObject.transform.rotation = this.ReadHand.transform.rotation;
		}
		base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
		base.gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
		base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (base.GetComponent<MPobject>())
		{
			base.GetComponent<MPobject>().networkDummy.RemoveWindow();
			tools.NetworkPLayer.pickup(base.GetComponent<MPobject>().networkDummy);
			return;
		}
		this.RemovingContinue();
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x0005E44C File Offset: 0x0005C64C
	public void RemovingContinue()
	{
		if (this.Dipstick && base.transform.parent != this.ReadHand.transform)
		{
			if (base.transform.parent != null && base.transform.parent.parent != null && (base.transform.parent.parent.GetComponent<CarProperties>().EngineBlock || this.Fluid != null))
			{
				if (base.transform.parent.parent.GetComponent<CarProperties>().EngineBlock)
				{
					foreach (Transform transform in base.transform.root.GetComponentsInChildren<Transform>())
					{
						if (transform.name == "OilFluidContainer")
						{
							this.OIL = transform.gameObject;
						}
					}
				}
				if (this.Fluid)
				{
					this.OIL = this.Fluid.gameObject;
				}
				if (this.OIL)
				{
					this.lerpedColor = Color.Lerp(Color.black, Color.yellow, this.OIL.transform.GetComponent<FLUID>().Condition);
					this.DipstickOil.transform.GetComponent<Renderer>().material.color = this.lerpedColor;
					this.DipstickOil.transform.localScale = new Vector3(1f, 1f, this.OIL.transform.GetComponent<FLUID>().FluidSize / this.OIL.transform.GetComponent<FLUID>().ContainerSize);
				}
				else
				{
					this.DipstickOil.transform.localScale = new Vector3(1f, 1f, 0f);
				}
			}
			this.OIL = null;
		}
		if (!this.Dipstick && this.Fluid)
		{
			this.Fluid.GetComponent<FLUID>().IsOpen = true;
			this.Fluid.GetComponent<FLUID>().enabled = true;
			this.Fluid.GetComponent<FLUID>().Start();
			if (this.Fluid.transform.root.GetComponent<CarProperties>())
			{
				base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
			}
			if (this.Fluid.transform.root.GetComponent<MainCarProperties>())
			{
				base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
			}
			if (this.Fluid.GetComponent<Rigidbody>() == null)
			{
				this.Fluid.AddComponent<Rigidbody>();
			}
			this.Fluid.GetComponent<Rigidbody>().isKinematic = true;
			this.Fluid = null;
		}
		if (base.transform.parent != null && base.transform.parent.gameObject.GetComponent<transparents>() && base.transform.parent.gameObject.GetComponent<transparents>())
		{
			base.transform.parent.gameObject.GetComponent<transparents>().UninstallATTACHABLES();
			base.transform.parent.gameObject.GetComponent<transparents>().HaveAttached = false;
		}
		if ((base.gameObject.transform.parent && base.gameObject.transform.parent != this.tempParent.transform && base.transform.parent != this.ReadHand.transform) || base.gameObject.transform.parent == null)
		{
			if (base.gameObject.GetComponent<Rigidbody>() == null)
			{
				base.gameObject.AddComponent<Rigidbody>();
			}
			base.gameObject.transform.SetParent(null);
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
			base.gameObject.AddComponent<MeshCollider>().convex = true;
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			base.gameObject.GetComponent<Rigidbody>().mass = base.gameObject.GetComponent<Partinfo>().weight;
			base.gameObject.transform.SetParent(null);
			base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x0005E8D4 File Offset: 0x0005CAD4
	public void RemoveFromHand()
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
		if (!this.GasCanCup)
		{
			tools.helditem = "Nothing";
			if (this.isHolding)
			{
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
				base.gameObject.AddComponent<MeshCollider>().convex = true;
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
				tools.PartInHand = null;
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
							childnumber2 = this.placetoput.transform.GetSiblingIndex();
							childnumber = this.placetoput.transform.parent.GetSiblingIndex();
						}
						base.GetComponent<MPobject>().networkDummy.GetComponent<NetworkTransform>().enabled = false;
						base.GetComponent<MPobject>().networkDummy.enabled = false;
						base.GetComponent<MPobject>().networkDummy.AttachPickup(component, childnumber, childnumber2);
						base.gameObject.transform.position = this.placetoput.transform.position;
						base.gameObject.transform.rotation = this.placetoput.transform.rotation;
						if (!this.Dipstick)
						{
							foreach (Transform transform in this.placetoput.transform.root.GetComponentsInChildren<Transform>())
							{
								if (transform.name == this.FluidName)
								{
									transform.transform.parent.GetComponent<MPobject>().networkDummy.SyncFluid(transform.GetComponent<FLUID>().FluidSize);
								}
							}
						}
					}
					else
					{
						this.FitInPlace(this.placetoput);
					}
				}
				base.gameObject.GetComponent<Partinfo>().CheckGround();
			}
		}
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x0005EC18 File Offset: 0x0005CE18
	public void FitInPlace(GameObject place)
	{
		base.gameObject.transform.position = place.transform.position;
		base.gameObject.transform.rotation = place.transform.rotation;
		if (place.GetComponent<transparents>().invert)
		{
			base.gameObject.transform.Rotate(0f, 0f, 180f);
		}
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
		if (!this.Dipstick)
		{
			foreach (Transform transform in base.transform.root.GetComponentsInChildren<Transform>())
			{
				if (transform.name == this.FluidName)
				{
					this.Fluid = transform.gameObject;
				}
			}
			this.Fluid.GetComponent<FLUID>().IsOpen = false;
			UnityEngine.Object.Destroy(this.Fluid.GetComponent<Rigidbody>());
		}
		UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		if (base.GetComponent<MPobject>())
		{
			base.StartCoroutine(this.LaterAttach());
		}
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x0005ED96 File Offset: 0x0005CF96
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

	// Token: 0x04001177 RID: 4471
	private float throwForce;

	// Token: 0x04001178 RID: 4472
	private Vector3 objectPos;

	// Token: 0x04001179 RID: 4473
	public bool GasCanCup;

	// Token: 0x0400117A RID: 4474
	public bool Dipstick;

	// Token: 0x0400117B RID: 4475
	public bool Started;

	// Token: 0x0400117C RID: 4476
	public bool canHold = true;

	// Token: 0x0400117D RID: 4477
	public GameObject placetoput;

	// Token: 0x0400117E RID: 4478
	public GameObject tempParent;

	// Token: 0x0400117F RID: 4479
	public GameObject ReadHand;

	// Token: 0x04001180 RID: 4480
	public GameObject fixbody;

	// Token: 0x04001181 RID: 4481
	public bool isHolding;

	// Token: 0x04001182 RID: 4482
	public bool seetoput;

	// Token: 0x04001183 RID: 4483
	public GameObject SphereCOl;

	// Token: 0x04001184 RID: 4484
	public GameObject DipstickOil;

	// Token: 0x04001185 RID: 4485
	public Material GoodOil;

	// Token: 0x04001186 RID: 4486
	public Material BadOil;

	// Token: 0x04001187 RID: 4487
	private Color lerpedColor = Color.white;

	// Token: 0x04001188 RID: 4488
	public GameObject OIL;

	// Token: 0x04001189 RID: 4489
	public GameObject Fluid;

	// Token: 0x0400118A RID: 4490
	public string FluidName;

	// Token: 0x0400118B RID: 4491
	public FirstPersonAIO Controller;

	// Token: 0x0400118C RID: 4492
	public bool removalloading;

	// Token: 0x0400118D RID: 4493
	public Slider LoadingSlider;

	// Token: 0x0400118E RID: 4494
	public float LoadingTime;

	// Token: 0x0400118F RID: 4495
	public Coroutine cc;
}
