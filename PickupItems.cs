using System;
using System.Collections;
using Mirror;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class PickupItems : MonoBehaviour
{
	// Token: 0x060009C7 RID: 2503 RVA: 0x00061544 File Offset: 0x0005F744
	private void Start()
	{
		if (base.transform.name == "Disc 1")
		{
			base.transform.name = "Disc";
		}
		if (base.transform.parent != null && base.transform.parent.parent != null && base.transform.parent.parent.GetComponent<PickupTool>() != null)
		{
			base.transform.parent.parent.GetComponent<PickupTool>().Attached = base.gameObject;
			if ((this.Grinder || this.Cutter || this.Electrode) && base.GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
			}
			base.GetComponent<MPobject>().networkDummy.GetComponent<NetworkTransform>().enabled = false;
			base.GetComponent<MPobject>().networkDummy.enabled = false;
		}
		if (base.transform.parent == null && !base.GetComponent<Rigidbody>())
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy && !base.GetComponent<MPobject>().networkDummy.hasAuthority)
		{
			base.gameObject.GetComponent<Rigidbody>().useGravity = false;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
		}
		this.UpdateCondition(0);
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x000616C3 File Offset: 0x0005F8C3
	private void Awake()
	{
		this.tempParent = GameObject.Find("hand");
		this.SphereCOl = GameObject.Find("SphereCollider");
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x000616E5 File Offset: 0x0005F8E5
	private IEnumerator RestartWrench()
	{
		tools.tool = 1;
		yield return 1;
		tools.tool = 2;
		this.SphereCOl.GetComponent<DisablerCollider>().RestartJump();
		yield break;
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x000616F4 File Offset: 0x0005F8F4
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

	// Token: 0x060009CB RID: 2507 RVA: 0x000618CC File Offset: 0x0005FACC
	public void UpdateCondition(int condition)
	{
		this.Condition -= (float)condition;
		if (this.Cutter || this.Electrode)
		{
			if (this.Condition == 7f)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = this.B4;
			}
			if (this.Condition == 6f)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = this.B4;
			}
			if (this.Condition == 5f)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = this.B3;
			}
			if (this.Condition == 4f)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = this.B3;
			}
			if (this.Condition == 3f)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = this.B2;
			}
			if (this.Condition == 2f)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = this.B2;
			}
			if (this.Condition == 1f)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = this.B1;
			}
			if (this.Condition == 0f)
			{
				if (base.transform.parent && base.transform.parent.parent && base.transform.parent.parent.GetComponent<PickupTool>())
				{
					base.transform.parent.parent.GetComponent<PickupTool>().Attached = null;
				}
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x00061A68 File Offset: 0x0005FC68
	public void TakeInHand()
	{
		RaycastHit raycastHit;
		if (!tools.sitting && !tools.Clicked && !tools.cooldown && tools.tool != 8 && tools.tool != 12 && tools.tool != 11 && tools.tool != 14 && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("Items")) && raycastHit.collider.gameObject == base.gameObject)
		{
			this.tempParent.transform.position = raycastHit.point;
			this.isHolding = true;
			base.enabled = true;
			tools.Clicked = true;
			tools.helditem = base.gameObject.name;
			tools.PartInHand = base.gameObject;
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("transparentpart"))
			{
				if (gameObject.name == base.gameObject.name)
				{
					gameObject.GetComponent<transparents>().Recheck();
				}
			}
			if (base.transform.parent != null && base.transform.parent.parent != null && base.transform.parent.parent.GetComponent<PickupTool>() != null)
			{
				base.transform.parent.gameObject.GetComponent<transparents>().UninstallATTACHABLES();
				base.transform.parent.gameObject.GetComponent<transparents>().HaveAttached = false;
				base.transform.parent.parent.GetComponent<PickupTool>().Attached = null;
			}
			if (base.gameObject.GetComponent<Rigidbody>() == null)
			{
				base.gameObject.AddComponent<Rigidbody>();
			}
			base.gameObject.GetComponent<Rigidbody>().useGravity = false;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
			if (base.GetComponent<MeshCollider>())
			{
				base.GetComponent<MeshCollider>().isTrigger = false;
			}
			if (base.GetComponent<BoxCollider>())
			{
				base.GetComponent<BoxCollider>().isTrigger = false;
			}
			if (base.GetComponent<FixedJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			}
			if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.DestroyJoint();
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
			base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.EnableMovment();
				tools.NetworkPLayer.pickup(base.GetComponent<MPobject>().networkDummy);
			}
		}
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x00061E2C File Offset: 0x0006002C
	public void RemoveFromHand()
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
			tools.PartInHand = null;
			if (this.seetoput)
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
					this.FitInPlace(this.placetoput);
				}
			}
			base.StartCoroutine(this.GroundCHeck());
		}
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x00062064 File Offset: 0x00060264
	public void FitInPlace(GameObject place)
	{
		if (this.Sand)
		{
			place.transform.parent.GetComponent<MooveItem>().pickuptool.paintlife = 50f;
			place.transform.parent.GetComponent<MooveItem>().pickuptool.VisualUpdate();
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		place.transform.parent.GetComponent<PickupTool>().Attached = base.gameObject;
		base.gameObject.transform.position = place.transform.position;
		base.gameObject.transform.rotation = place.transform.rotation;
		if (place.GetComponent<transparents>().invert)
		{
			base.gameObject.transform.Rotate(0f, 0f, 180f);
		}
		base.gameObject.transform.localScale = place.transform.localScale;
		base.gameObject.transform.SetParent(place.transform);
		if (base.gameObject.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		}
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x00062189 File Offset: 0x00060389
	private IEnumerator GroundCHeck()
	{
		yield return new WaitForSeconds(2f);
		Vector3 position = base.transform.position;
		Terrain[] activeTerrains = Terrain.activeTerrains;
		Terrain terrain = Terrain.activeTerrain;
		float num = (new Vector3(activeTerrains[0].transform.position.x + activeTerrains[0].terrainData.size.x / 2f, base.transform.position.y, activeTerrains[0].transform.position.z + activeTerrains[0].terrainData.size.z / 2f) - base.transform.position).sqrMagnitude;
		for (int i = 0; i < activeTerrains.Length; i++)
		{
			float sqrMagnitude = (new Vector3(activeTerrains[i].transform.position.x + activeTerrains[i].terrainData.size.x / 2f, base.transform.position.y, activeTerrains[i].transform.position.z + activeTerrains[i].terrainData.size.z / 2f) - base.transform.position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				terrain = activeTerrains[i];
			}
		}
		if (terrain && position.y < terrain.SampleHeight(base.transform.position))
		{
			position.y = terrain.SampleHeight(base.transform.position) + 0.3f;
			base.transform.position = position;
			if (base.GetComponent<Rigidbody>())
			{
				base.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
			base.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			base.GetComponent<Rigidbody>().Sleep();
			base.gameObject.GetComponent<Rigidbody>().drag = 10f;
			base.StartCoroutine(this.SetDrag());
		}
		yield break;
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00062198 File Offset: 0x00060398
	private IEnumerator SetDrag()
	{
		yield return new WaitForSeconds(2f);
		if (base.GetComponent<Rigidbody>())
		{
			base.gameObject.GetComponent<Rigidbody>().drag = 0f;
		}
		yield break;
	}

	// Token: 0x040011BD RID: 4541
	public bool CanPutInBox;

	// Token: 0x040011BE RID: 4542
	private float throwForce;

	// Token: 0x040011BF RID: 4543
	private Vector3 objectPos;

	// Token: 0x040011C0 RID: 4544
	public bool Cutter;

	// Token: 0x040011C1 RID: 4545
	public bool Grinder;

	// Token: 0x040011C2 RID: 4546
	public bool Electrode;

	// Token: 0x040011C3 RID: 4547
	public bool Sand;

	// Token: 0x040011C4 RID: 4548
	public float Condition;

	// Token: 0x040011C5 RID: 4549
	public Mesh B4;

	// Token: 0x040011C6 RID: 4550
	public Mesh B3;

	// Token: 0x040011C7 RID: 4551
	public Mesh B2;

	// Token: 0x040011C8 RID: 4552
	public Mesh B1;

	// Token: 0x040011C9 RID: 4553
	public Mesh B0;

	// Token: 0x040011CA RID: 4554
	public bool canHold = true;

	// Token: 0x040011CB RID: 4555
	public GameObject placetoput;

	// Token: 0x040011CC RID: 4556
	public GameObject tempParent;

	// Token: 0x040011CD RID: 4557
	public GameObject fixbody;

	// Token: 0x040011CE RID: 4558
	public bool isHolding;

	// Token: 0x040011CF RID: 4559
	public bool seetoput;

	// Token: 0x040011D0 RID: 4560
	public GameObject SphereCOl;
}
