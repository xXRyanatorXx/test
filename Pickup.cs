using System;
using System.Collections;
using Mirror;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class Pickup : MonoBehaviour
{
	// Token: 0x06000968 RID: 2408 RVA: 0x0005C123 File Offset: 0x0005A323
	private void Awake()
	{
		this.tempParent = GameObject.Find("hand");
		this.SphereCOl = GameObject.Find("SphereCollider");
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0005C145 File Offset: 0x0005A345
	private IEnumerator RestartWrench()
	{
		if (!tools.sitting)
		{
			this.CurrentTool = tools.tool;
			tools.tool = 1;
			yield return 1;
			tools.tool = this.CurrentTool;
			if (this.SphereCOl.GetComponent<DisablerCollider>())
			{
				this.SphereCOl.GetComponent<DisablerCollider>().RestartJump();
			}
		}
		yield return 0;
		yield break;
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x0005C154 File Offset: 0x0005A354
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
			this.throwForce += 15f;
		}
		if ((Input.GetMouseButtonUp(2) || Input.GetMouseButtonUp(1)) && this.isHolding)
		{
			this.isHolding = false;
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
			base.gameObject.AddComponent<MeshCollider>().convex = true;
			if (base.transform.root.GetComponent<CarProperties>())
			{
				base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
			}
			if (base.transform.root.GetComponent<MainCarProperties>())
			{
				base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
			}
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
		if (Input.GetMouseButtonUp(0))
		{
			this.OnMouseUp();
		}
		if (!this.isHolding)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0005C3C4 File Offset: 0x0005A5C4
	public void OnMouseDowns()
	{
		if (tools.tool == 24 || tools.tool == 25 || tools.UIisOpen)
		{
			return;
		}
		if (!base.transform.GetComponent<CarProperties>().SparkPlug && !base.transform.GetComponent<CarProperties>().GlowPlug && !base.transform.GetComponent<CarProperties>().Injector && tools.cooldown3)
		{
			return;
		}
		RaycastHit raycastHit;
		if (!tools.sitting && !tools.Clicked && !tools.cooldown && tools.tool != 9 && (tools.tool != 8 || !tools.Tighten) && tools.tool != 12 && tools.tool != 11 && tools.tool != 14 && tools.tool != 23 && tools.tool != 4 && tools.tool != 7 && tools.tool != 19 && base.gameObject.GetComponent<Partinfo>().tightnuts == 0f && base.gameObject.GetComponent<Partinfo>().fixedwelds == 0f && !this.ChildrenHaveFixedBolt && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("LooseParts")))
		{
			base.StartCoroutine(this.RestartWrench());
			this.tempParent.transform.position = raycastHit.point;
			this.isHolding = true;
			base.enabled = true;
			tools.Clicked = true;
			tools.helditem = base.gameObject.name;
			base.GetComponent<CarProperties>().picked = true;
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
			{
				if (gameObject.name == base.gameObject.name && gameObject.GetComponent<transparents>().Variation == this.Variation)
				{
					gameObject.GetComponent<transparents>().Recheck();
				}
			}
			if (base.transform.parent != null && base.transform.parent.gameObject.GetComponent<transparents>())
			{
				base.transform.parent.gameObject.GetComponent<transparents>().UninstallATTACHABLES();
				base.transform.parent.gameObject.GetComponent<transparents>().HaveAttached = false;
			}
			if (base.gameObject.GetComponent<Rigidbody>() == null)
			{
				base.gameObject.AddComponent<Rigidbody>();
			}
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
			if (base.GetComponent<FixedJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			}
			base.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			base.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			if (base.gameObject.GetComponent<CarProperties>().RemovedDifferentMesh)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.gameObject.GetComponent<CarProperties>().RemovedDifferentMesh;
				base.gameObject.transform.position = this.tempParent.transform.position;
			}
			base.gameObject.transform.SetParent(this.tempParent.transform);
			if (Vector3.Distance(this.tempParent.transform.position, Camera.main.transform.position) > 1.3f)
			{
				this.tempParent.transform.position = Vector3.MoveTowards(this.tempParent.transform.position, Camera.main.transform.position, Vector3.Distance(this.tempParent.transform.position, Camera.main.transform.position) - 1.3f);
			}
			base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			base.gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
			base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			if (base.GetComponent<CarProperties>().RemovedMesh)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = (base.gameObject.GetComponent<MeshFilter>().mesh = base.GetComponent<CarProperties>().RemovedMesh);
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = (base.gameObject.GetComponent<MeshFilter>().mesh = base.GetComponent<CarProperties>().RemovedMesh);
			}
			FixedJoint[] componentsInChildren = base.gameObject.GetComponentsInChildren<FixedJoint>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (componentsInChildren[j].gameObject != base.gameObject)
				{
					componentsInChildren[j].connectedBody = base.gameObject.GetComponent<Rigidbody>();
				}
			}
			HingeJoint[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<HingeJoint>();
			for (int k = 0; k < componentsInChildren2.Length; k++)
			{
				componentsInChildren2[k].connectedBody = base.gameObject.GetComponent<Rigidbody>();
			}
			HexNut[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<HexNut>();
			for (int l = 0; l < componentsInChildren3.Length; l++)
			{
				componentsInChildren3[l].disableREND();
			}
			FlatNut[] componentsInChildren4 = base.gameObject.GetComponentsInChildren<FlatNut>();
			for (int m = 0; m < componentsInChildren4.Length; m++)
			{
				componentsInChildren4[m].disableREND();
			}
			Sparkplug[] componentsInChildren5 = base.gameObject.GetComponentsInChildren<Sparkplug>();
			for (int n = 0; n < componentsInChildren5.Length; n++)
			{
				componentsInChildren5[n].disableREND();
			}
			DisableRend[] componentsInChildren6 = base.gameObject.GetComponentsInChildren<DisableRend>();
			for (int num = 0; num < componentsInChildren6.Length; num++)
			{
				componentsInChildren6[num].disableREND();
			}
			MyBoneSCR[] componentsInChildren7 = base.GetComponentsInChildren<MyBoneSCR>();
			for (int i = 0; i < componentsInChildren7.Length; i++)
			{
				componentsInChildren7[i].enabled = false;
			}
			base.GetComponent<Partinfo>().AllowFall = false;
			if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.RemoveWindow();
			}
		}
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0005C9E0 File Offset: 0x0005ABE0
	public void RemovingContinue()
	{
		if (base.transform.parent != null && base.transform.parent.gameObject.GetComponent<transparents>())
		{
			base.transform.parent.gameObject.GetComponent<transparents>().UninstallATTACHABLES();
			base.transform.parent.gameObject.GetComponent<transparents>().HaveAttached = false;
		}
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

	// Token: 0x0600096D RID: 2413 RVA: 0x0005CB88 File Offset: 0x0005AD88
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
			if (base.transform.root.GetComponent<CarProperties>())
			{
				base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
			}
			if (base.transform.root.GetComponent<MainCarProperties>())
			{
				base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
			}
			RaycastHit raycastHit;
			if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("TransparentParts")) && raycastHit.collider.gameObject.name == base.gameObject.name)
			{
				this.placetoput = raycastHit.collider.gameObject;
				this.seetoput = true;
				tools.canput = true;
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

	// Token: 0x0600096E RID: 2414 RVA: 0x0005CEBC File Offset: 0x0005B0BC
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

	// Token: 0x0600096F RID: 2415 RVA: 0x0005CF5C File Offset: 0x0005B15C
	public void FitInPlace(GameObject place)
	{
		if (base.gameObject.GetComponent<Rigidbody>() == null)
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		base.gameObject.GetComponent<Rigidbody>().useGravity = true;
		base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
		base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		base.StartCoroutine(this.RestartWrench());
		base.gameObject.transform.position = place.transform.position;
		base.gameObject.transform.rotation = place.transform.rotation;
		if (base.transform.GetComponent<CarProperties>().Tire)
		{
			base.gameObject.transform.position += place.transform.TransformDirection(-0.05f, 0f, 0f);
		}
		if (base.transform.GetComponent<CarProperties>().SparkPlug || base.transform.GetComponent<CarProperties>().GlowPlug || base.transform.GetComponent<CarProperties>().Injector)
		{
			base.gameObject.transform.position += place.transform.TransformDirection(0f, 0.007f, 0f);
		}
		if (place.GetComponent<transparents>().invert)
		{
			base.gameObject.transform.Rotate(0f, 0f, 180f);
		}
		base.gameObject.transform.localScale = place.transform.localScale;
		base.gameObject.transform.SetParent(place.transform);
		if (base.GetComponent<CarProperties>().RealWheel)
		{
			base.transform.localPosition = new Vector3(0f, 0f, 0f);
			foreach (Transform transform in base.transform.parent.parent.GetComponentsInChildren<Transform>())
			{
				if (transform.name == "Spacer" && transform.GetComponent<CarProperties>())
				{
					base.transform.localPosition = new Vector3(transform.GetComponent<CarProperties>().SpacerSize, 0f, 0f);
				}
			}
		}
		if (base.gameObject.GetComponent<CarProperties>().RemovedDifferentMesh)
		{
			base.gameObject.GetComponent<CarProperties>().SetMesh();
		}
		if (base.transform.parent != null)
		{
			base.transform.parent.gameObject.GetComponent<transparents>().InstallATTACHABLES();
			base.transform.parent.gameObject.GetComponent<transparents>().HaveAttached = true;
		}
		if (base.transform.root.GetComponent<CarProperties>())
		{
			base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
		}
		if (base.transform.root.GetComponent<MainCarProperties>())
		{
			base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
		}
		base.gameObject.GetComponent<Partinfo>().remove(false);
		HexNut[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<HexNut>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].enableREND();
		}
		FlatNut[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<FlatNut>();
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			componentsInChildren3[k].enableREND();
		}
		BoltNut[] componentsInChildren4 = base.transform.root.GetComponentsInChildren<BoltNut>();
		for (int l = 0; l < componentsInChildren4.Length; l++)
		{
			componentsInChildren4[l].enabled = true;
			componentsInChildren4[l].enableREND();
		}
		Sparkplug[] componentsInChildren5 = base.gameObject.GetComponentsInChildren<Sparkplug>();
		for (int m = 0; m < componentsInChildren5.Length; m++)
		{
			componentsInChildren5[m].enableREND();
		}
		DisableRend[] componentsInChildren6 = base.gameObject.GetComponentsInChildren<DisableRend>();
		for (int n = 0; n < componentsInChildren6.Length; n++)
		{
			componentsInChildren6[n].enableREND();
		}
		MyBoneSCR[] componentsInChildren7 = base.transform.root.GetComponentsInChildren<MyBoneSCR>();
		for (int i = 0; i < componentsInChildren7.Length; i++)
		{
			componentsInChildren7[i].ReStart();
		}
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x0005D386 File Offset: 0x0005B586
	private void OnJointBreak()
	{
		this.BRAKE();
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0005D38E File Offset: 0x0005B58E
	public void BRAKE()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.BRAKE();
			return;
		}
		this.BRAKE2();
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0005D3C8 File Offset: 0x0005B5C8
	public void BRAKE2()
	{
		foreach (WeldCut weldCut in base.transform.root.GetComponentsInChildren<WeldCut>())
		{
			if (weldCut.otherobject == base.gameObject)
			{
				weldCut.BrokeOff();
			}
			if (weldCut.transform.parent == base.gameObject)
			{
				weldCut.BrokeOff();
			}
		}
		base.StartCoroutine(this.RestartWrench());
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
		if (base.transform.parent != null)
		{
			if (!base.transform.parent.gameObject.GetComponent<transparents>())
			{
				Debug.Log(base.gameObject);
			}
			else
			{
				base.transform.parent.gameObject.GetComponent<transparents>().UninstallATTACHABLES();
				base.transform.parent.gameObject.GetComponent<transparents>().HaveAttached = false;
			}
		}
		base.gameObject.transform.SetParent(null);
		foreach (BoltNut boltNut in base.transform.root.GetComponentsInChildren<BoltNut>())
		{
			if (boltNut.otherobject && boltNut.otherobject.transform.root != base.transform.root && boltNut.tight)
			{
				boltNut.BrokeOff();
			}
		}
		if (base.transform.root.GetComponent<CarProperties>())
		{
			base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
		}
		if (base.transform.root.GetComponent<MainCarProperties>())
		{
			base.gameObject.GetComponent<CarProperties>().PreventChildCollisions();
		}
		base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		FixedJoint[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<FixedJoint>();
		for (int j = 0; j < componentsInChildren3.Length; j++)
		{
			if (componentsInChildren3[j].gameObject != base.gameObject)
			{
				componentsInChildren3[j].connectedBody = base.gameObject.GetComponent<Rigidbody>();
			}
		}
		HingeJoint[] componentsInChildren4 = base.gameObject.GetComponentsInChildren<HingeJoint>();
		for (int k = 0; k < componentsInChildren4.Length; k++)
		{
			componentsInChildren4[k].connectedBody = base.gameObject.GetComponent<Rigidbody>();
		}
		HexNut[] componentsInChildren5 = base.gameObject.GetComponentsInChildren<HexNut>();
		for (int l = 0; l < componentsInChildren5.Length; l++)
		{
			componentsInChildren5[l].disableREND();
		}
		FlatNut[] componentsInChildren6 = base.gameObject.GetComponentsInChildren<FlatNut>();
		for (int m = 0; m < componentsInChildren6.Length; m++)
		{
			componentsInChildren6[m].disableREND();
		}
		foreach (Transform transform in base.GetComponentsInChildren<Transform>())
		{
			if (transform.GetComponent<WeldCut>() != null)
			{
				transform.GetComponent<WeldCut>().BrokeOff();
			}
		}
		CarProperties[] componentsInChildren8 = base.GetComponentsInChildren<CarProperties>();
		for (int i = 0; i < componentsInChildren8.Length; i++)
		{
			componentsInChildren8[i].RestartColliders();
		}
		foreach (MyBoneSCR myBoneSCR in base.GetComponentsInChildren<MyBoneSCR>())
		{
			myBoneSCR.ReStart();
			myBoneSCR.enabled = false;
		}
		base.gameObject.GetComponent<Partinfo>().fixedwelds = 0f;
		base.gameObject.GetComponent<Partinfo>().tightnuts = 0f;
		base.gameObject.GetComponent<Partinfo>().remove(true);
	}

	// Token: 0x04001168 RID: 4456
	private float throwForce;

	// Token: 0x04001169 RID: 4457
	private Vector3 objectPos;

	// Token: 0x0400116A RID: 4458
	public int Variation;

	// Token: 0x0400116B RID: 4459
	public bool ChildrenHaveFixedBolt;

	// Token: 0x0400116C RID: 4460
	public bool canHold = true;

	// Token: 0x0400116D RID: 4461
	public GameObject placetoput;

	// Token: 0x0400116E RID: 4462
	public GameObject tempParent;

	// Token: 0x0400116F RID: 4463
	public GameObject fixbody;

	// Token: 0x04001170 RID: 4464
	public bool isHolding;

	// Token: 0x04001171 RID: 4465
	public bool seetoput;

	// Token: 0x04001172 RID: 4466
	public GameObject SphereCOl;

	// Token: 0x04001173 RID: 4467
	public int CurrentTool;
}
