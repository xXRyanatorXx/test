using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000207 RID: 519
public class MooveItem : MonoBehaviour
{
	// Token: 0x06000C0E RID: 3086 RVA: 0x00085BD8 File Offset: 0x00083DD8
	private void Start()
	{
		this.tempParent = GameObject.Find("hand");
		if (this.Collectible)
		{
			base.transform.name = "Collectible";
			UnityEngine.Object.Destroy(base.GetComponent<SaveItem>());
		}
		base.StartCoroutine(this.wait());
		if (base.transform.name == "ClientSpawn")
		{
			base.GetComponent<Rigidbody>().centerOfMass = this.com;
		}
		base.StartCoroutine(this.waitStart());
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x00085C5C File Offset: 0x00083E5C
	private void OnMouseDown()
	{
		if (this.IsFurniture && tools.tool != 22)
		{
			return;
		}
		if (this.LiftHandle && this.LiftHandle.steps > 0)
		{
			return;
		}
		if ((tools.tool == 18 || tools.tool == 19 || tools.tool == 2 || tools.tool == 3 || tools.tool == 4 || tools.tool == 5 || tools.tool == 6 || tools.tool == 8 || tools.tool == 9 || tools.tool == 12 || tools.tool == 14 || tools.tool == 15 || tools.tool == 16 || tools.tool == 17 || tools.tool == 25 || tools.tool == 20) && base.transform.root.GetComponent<MainCarProperties>())
		{
			return;
		}
		base.StartCoroutine(this.LateClick());
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x00085D49 File Offset: 0x00083F49
	private IEnumerator LateClick()
	{
		yield return 0;
		yield return 0;
		RaycastHit raycastHit;
		if (!tools.sitting && !tools.cooldown && tools.helditem == "Nothing" && !tools.Clicked && tools.tool != 14 && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("Items") | 1 << LayerMask.NameToLayer("Default")) && raycastHit.collider.gameObject == base.gameObject)
		{
			if (base.GetComponent<MPobject>())
			{
				tools.NetworkPLayer.pickup(base.GetComponent<MPobject>().networkDummy);
				base.GetComponent<MPobject>().networkDummy.EnableMovment();
			}
			if (base.GetComponent<FixedJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			}
			Physics.IgnoreCollision(this.tempParent.transform.root.GetComponent<Collider>(), base.GetComponent<Collider>());
			this.tempParent.transform.position = raycastHit.point;
			tools.helditem = base.gameObject.name;
			if (this.SlideOnGround)
			{
				base.enabled = true;
				base.gameObject.transform.SetParent(this.tempParent.transform);
			}
			else
			{
				if (!base.gameObject.GetComponent<Rigidbody>())
				{
					base.gameObject.AddComponent<Rigidbody>();
				}
				if (base.GetComponent<MeshCollider>() && !base.GetComponent<OpenableBox>())
				{
					base.GetComponent<MeshCollider>().isTrigger = false;
				}
				if (base.GetComponent<BoxCollider>() && !base.GetComponent<OpenableBox>())
				{
					base.GetComponent<BoxCollider>().isTrigger = false;
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
				if (base.transform.name == "Jack")
				{
					base.gameObject.GetComponent<Rigidbody>().mass = 50f;
				}
				if (base.transform.name == "FloorJack")
				{
					base.gameObject.GetComponent<Rigidbody>().mass = 100f;
				}
				if (base.GetComponent<Box>())
				{
					base.GetComponent<Box>().enabled = true;
				}
			}
			if (this.EngineParent && this.EngineParent.transform.childCount > 0 && this.EngineParent.transform.GetChild(0).GetComponent<Rigidbody>())
			{
				UnityEngine.Object.Destroy(this.EngineParent.transform.GetChild(0).GetComponent<FixedJoint>());
				this.EngineParent.transform.GetChild(0).transform.SetParent(null);
			}
		}
		yield break;
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x00085D58 File Offset: 0x00083F58
	private IEnumerator MoveEngine()
	{
		yield return 1;
		if (base.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(this.EngineParent.transform.GetChild(0).GetComponent<FixedJoint>());
			this.EngineParent.transform.GetChild(0).transform.position = this.EngineParent.transform.position;
			this.EngineParent.transform.GetChild(0).transform.rotation = this.EngineParent.transform.rotation;
			base.StartCoroutine(this.MoveEngine());
		}
		else
		{
			this.EngineParent.transform.GetChild(0).gameObject.AddComponent<FixedJoint>();
			this.EngineParent.transform.GetChild(0).gameObject.GetComponent<FixedJoint>().connectedBody = base.gameObject.transform.root.GetComponent<Rigidbody>();
			this.EngineParent.transform.GetChild(0).gameObject.transform.GetComponent<FixedJoint>().breakForce = 50000f;
		}
		yield break;
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x00085D68 File Offset: 0x00083F68
	private void OnMouseUp()
	{
		if (base.transform.parent)
		{
			tools.helditem = "Nothing";
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			Physics.IgnoreCollision(this.tempParent.transform.root.GetComponent<Collider>(), base.GetComponent<Collider>(), false);
			base.gameObject.transform.SetParent(null);
			base.StartCoroutine(this.wait());
			base.StartCoroutine(this.GroundCHeck());
			if (base.GetComponent<MPobject>())
			{
				tools.NetworkPLayer.putdown(base.GetComponent<MPobject>().networkDummy);
			}
		}
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x00085E38 File Offset: 0x00084038
	private void Updatess()
	{
		float maxDistanceDelta = 3f * Time.deltaTime;
		base.transform.position = Vector3.MoveTowards(base.transform.position, this.tempParent.transform.position, maxDistanceDelta);
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x00085E80 File Offset: 0x00084080
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "InsideCol" && base.transform.name == "CarLift" && !other.gameObject.transform.root.GetComponent<MainCarProperties>())
		{
			this.InTrailer = true;
			return;
		}
		if (other.gameObject.name == "InsideCol" && base.transform.name != "CarLift")
		{
			this.InTrailer = true;
		}
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x00085F14 File Offset: 0x00084114
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name == "InsideCol")
		{
			this.InTrailer = false;
		}
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x00085F34 File Offset: 0x00084134
	private IEnumerator waitStart()
	{
		yield return new WaitForSeconds(3f);
		if (this.InTrailer && !base.gameObject.GetComponent<Rigidbody>() && !this.EngineParent)
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		yield break;
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x00085F43 File Offset: 0x00084143
	private IEnumerator wait()
	{
		yield return new WaitForSeconds(1f);
		if (!base.transform.parent && this.IsFurniture && base.gameObject.GetComponent<Rigidbody>() && !this.InTrailer && !this.EngineParent)
		{
			UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		}
		if (this.EngineParent && !this.InTrailer)
		{
			base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
		}
		yield break;
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x00085F52 File Offset: 0x00084152
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

	// Token: 0x06000C19 RID: 3097 RVA: 0x00085F61 File Offset: 0x00084161
	private IEnumerator SetDrag()
	{
		yield return new WaitForSeconds(2f);
		if (base.GetComponent<Rigidbody>())
		{
			base.gameObject.GetComponent<Rigidbody>().drag = 0f;
		}
		yield break;
	}

	// Token: 0x040014DB RID: 5339
	public bool IsFurniture;

	// Token: 0x040014DC RID: 5340
	public bool InTrailer;

	// Token: 0x040014DD RID: 5341
	public LiftHandle LiftHandle;

	// Token: 0x040014DE RID: 5342
	public int price;

	// Token: 0x040014DF RID: 5343
	public bool CanPutInBox;

	// Token: 0x040014E0 RID: 5344
	public bool ItemBox;

	// Token: 0x040014E1 RID: 5345
	public GameObject tempParent;

	// Token: 0x040014E2 RID: 5346
	public bool SlideOnGround;

	// Token: 0x040014E3 RID: 5347
	public string PrefabName;

	// Token: 0x040014E4 RID: 5348
	public bool Collectible;

	// Token: 0x040014E5 RID: 5349
	public Vector3 com;

	// Token: 0x040014E6 RID: 5350
	public GameObject EngineParent;

	// Token: 0x040014E7 RID: 5351
	public PickupTool pickuptool;
}
