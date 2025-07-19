using System;
using System.Collections;
using GlobalSnowEffect;
using Mirror;
using PaintIn3D;
using RVP;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class Partinfo : MonoBehaviour
{
	// Token: 0x06000933 RID: 2355 RVA: 0x0005A640 File Offset: 0x00058840
	public void SpawnThis()
	{
		this.player = GameObject.Find("Player");
		if (tools.MPrunning)
		{
			tools.NetworkPLayer.ITEM = base.transform.gameObject;
			if (base.GetComponent<CarProperties>() && base.GetComponent<CarProperties>().PrefabName != "")
			{
				tools.NetworkPLayer.Itemname = base.GetComponent<CarProperties>().PrefabName;
			}
			else
			{
				tools.NetworkPLayer.Itemname = base.transform.name;
			}
			tools.NetworkPLayer.Spawnposition = tools.SpawnSpot.transform.position;
			tools.NetworkPLayer.Spawnrotation = Quaternion.identity;
			tools.NetworkPLayer.SpawnObject(0, true);
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.transform.gameObject, tools.SpawnSpot.transform.position, Quaternion.identity);
		gameObject.GetComponent<Partinfo>().Creating();
		if (this.RenamedPrefab != "")
		{
			gameObject.transform.name = this.RenamedPrefab;
			return;
		}
		gameObject.transform.name = base.transform.name;
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0005A76C File Offset: 0x0005896C
	private void Start()
	{
		if (this.RenamedPrefab != "")
		{
			base.transform.name = this.RenamedPrefab;
		}
		if (base.transform.parent)
		{
			this.tightnuts = this.attachedbolts;
			this.fixedwelds = this.attachedwelds;
			this.fixedImportantBolts = this.ImportantBolts;
		}
		else
		{
			HexNut[] componentsInChildren = base.gameObject.GetComponentsInChildren<HexNut>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			FlatNut[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<FlatNut>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			DisableRend[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<DisableRend>();
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				componentsInChildren3[k].disableREND();
			}
		}
		this.AudioParent = GameObject.Find("hand");
		this.player = GameObject.Find("Player");
		if (!this.InBackpack && !base.transform.parent && !base.gameObject.GetComponent<Rigidbody>())
		{
			base.gameObject.AddComponent<Rigidbody>();
		}
		if (this.SmallObject && base.gameObject.GetComponent<Rigidbody>())
		{
			base.gameObject.GetComponent<Rigidbody>().drag = 6f;
		}
		if (!base.transform.parent && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast") && !base.GetComponent<MainCarProperties>())
		{
			base.gameObject.layer = LayerMask.NameToLayer("LooseParts");
		}
		if (this.tightnuts < 0f)
		{
			this.tightnuts = 0f;
		}
		if (this.fixedImportantBolts < 0f)
		{
			this.fixedImportantBolts = 0f;
		}
		if (this.fixedwelds < 0f)
		{
			this.fixedwelds = 0f;
		}
		if (this.ChildrenFixedBolts < 0f)
		{
			this.ChildrenFixedBolts = 0f;
		}
		if (base.transform.root.GetComponent<tools>())
		{
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			base.gameObject.transform.SetParent(null);
		}
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0005A9EC File Offset: 0x00058BEC
	public void FindHinge()
	{
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.gameObject.name == "HingePivot")
			{
				this.HingePivot = transform.gameObject;
			}
		}
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0005AA64 File Offset: 0x00058C64
	public void Creating()
	{
		if (base.transform.parent == null)
		{
			if (this.RenamedPrefab != "")
			{
				base.gameObject.transform.name = this.RenamedPrefab;
			}
			base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			if (!base.gameObject.GetComponent<Rigidbody>())
			{
				base.gameObject.AddComponent<Rigidbody>();
			}
			if (this.SmallObject)
			{
				base.gameObject.GetComponent<Rigidbody>().drag = 6f;
			}
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			this.fixedwelds = 0f;
			this.fixedImportantBolts = 0f;
			this.tightnuts = 0f;
			if (base.gameObject.tag == "Wheel1")
			{
				base.gameObject.GetComponent<MeshCollider>().isTrigger = false;
			}
			if (base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast") || base.gameObject.layer == LayerMask.NameToLayer("Repair"))
			{
				base.gameObject.layer = LayerMask.NameToLayer("LooseParts");
			}
			foreach (BoltNut boltNut in base.transform.root.GetComponentsInChildren<BoltNut>())
			{
				boltNut.gameObject.GetComponent<CarProperties>().Start();
				boltNut.tight = false;
				boltNut.gameObject.transform.position += boltNut.transform.TransformDirection(0f, 0.007f, 0f);
			}
			WeldCut[] componentsInChildren2 = base.transform.root.GetComponentsInChildren<WeldCut>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].welded = false;
			}
			foreach (HexNut hexNut in base.transform.root.GetComponentsInChildren<HexNut>())
			{
				if (hexNut.gameObject.GetComponent<CarProperties>())
				{
					hexNut.gameObject.GetComponent<CarProperties>().Start();
				}
				hexNut.tight = false;
				hexNut.gameObject.transform.position += hexNut.transform.TransformDirection(0f, 0.007f, 0f);
			}
			foreach (FlatNut flatNut in base.transform.root.GetComponentsInChildren<FlatNut>())
			{
				if (flatNut.gameObject.GetComponent<CarProperties>())
				{
					flatNut.gameObject.GetComponent<CarProperties>().Start();
				}
				flatNut.tight = false;
				flatNut.gameObject.transform.position += flatNut.transform.TransformDirection(0f, 0.007f, 0f);
			}
			if (base.GetComponent<CarProperties>())
			{
				base.GetComponent<CarProperties>().Owner = "Player";
			}
			CarProperties[] componentsInChildren5 = base.GetComponentsInChildren<CarProperties>();
			for (int i = 0; i < componentsInChildren5.Length; i++)
			{
				componentsInChildren5[i].Owner = "Player";
			}
			if (base.GetComponent<CarProperties>() && base.GetComponent<CarProperties>().Paintable)
			{
				base.GetComponent<P3dPaintableTexture>().Color = Color.grey;
				base.GetComponent<P3dMaterialCloner>().activated = false;
				base.GetComponent<P3dPaintableTexture>().activated = false;
				base.GetComponent<P3dPaintable>().activated = false;
			}
			if (base.GetComponent<MainCarProperties>())
			{
				base.gameObject.GetComponent<Rigidbody>().mass = this.weight;
				base.GetComponent<MainCarProperties>().Owner = "Player";
			}
		}
		base.StartCoroutine(this.WaitCreating());
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0005AE48 File Offset: 0x00059048
	public void CreatingJunk()
	{
		if (base.transform.parent == null)
		{
			if (this.RenamedPrefab != "")
			{
				base.gameObject.transform.name = this.RenamedPrefab;
			}
			base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			if (!this.InBackpack)
			{
				if (!base.gameObject.GetComponent<Rigidbody>())
				{
					base.gameObject.AddComponent<Rigidbody>();
				}
				if (this.SmallObject)
				{
					base.gameObject.GetComponent<Rigidbody>().drag = 6f;
				}
				base.gameObject.GetComponent<Rigidbody>().useGravity = true;
				base.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
				base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
			}
			this.fixedwelds = 0f;
			this.fixedImportantBolts = 0f;
			this.tightnuts = 0f;
			if (base.gameObject.tag == "Wheel1")
			{
				base.gameObject.GetComponent<MeshCollider>().isTrigger = false;
			}
			if (base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast") || base.gameObject.layer == LayerMask.NameToLayer("Repair"))
			{
				base.gameObject.layer = LayerMask.NameToLayer("LooseParts");
			}
			BoltNut[] componentsInChildren = base.transform.root.GetComponentsInChildren<BoltNut>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].tight = false;
			}
			WeldCut[] componentsInChildren2 = base.transform.root.GetComponentsInChildren<WeldCut>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].welded = false;
			}
			foreach (HexNut hexNut in base.transform.root.GetComponentsInChildren<HexNut>())
			{
				hexNut.tight = false;
				hexNut.gameObject.transform.position += hexNut.transform.TransformDirection(0f, 0.007f, 0f);
			}
			foreach (FlatNut flatNut in base.transform.root.GetComponentsInChildren<FlatNut>())
			{
				flatNut.tight = false;
				flatNut.gameObject.transform.position += flatNut.transform.TransformDirection(0f, 0.007f, 0f);
			}
			if (base.GetComponent<CarProperties>() && base.GetComponent<CarProperties>().Paintable)
			{
				base.GetComponent<P3dMaterialCloner>().activated = false;
				base.GetComponent<P3dPaintableTexture>().activated = false;
				base.GetComponent<P3dPaintable>().activated = false;
			}
		}
		base.StartCoroutine(this.WaitCreating());
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x0005B11D File Offset: 0x0005931D
	private IEnumerator WaitCreating()
	{
		yield return 0;
		transparents[] componentsInChildren = base.transform.root.GetComponentsInChildren<transparents>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Start();
		}
		if (this.AudioParent.transform.parent.GetComponent<GlobalSnow>())
		{
			this.AudioParent.transform.parent.GetComponent<GlobalSnow>().Resett();
		}
		yield break;
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0005B12C File Offset: 0x0005932C
	public void remove(bool fall)
	{
		if (this.InBackpack)
		{
			return;
		}
		if (base.transform.root.GetComponent<MainCarProperties>())
		{
			base.transform.root.GetComponent<MainCarProperties>().FreezeForMoment();
		}
		if (this.tightnuts < 0f)
		{
			this.tightnuts = 0f;
		}
		if (this.fixedImportantBolts < 0f)
		{
			this.fixedImportantBolts = 0f;
		}
		if (this.fixedwelds < 0f)
		{
			this.fixedwelds = 0f;
		}
		this.ChildrenFixedBolts = 0f;
		BoltNut[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoltNut>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].CheckAffected();
		}
		if (this.ChildrenFixedBolts == 0f)
		{
			foreach (CarProperties carProperties in base.transform.root.GetComponentsInChildren<CarProperties>())
			{
				if (carProperties.VisualWheel)
				{
					transparents[] componentsInChildren3 = carProperties.gameObject.GetComponentsInChildren<transparents>();
					for (int k = 0; k < componentsInChildren3.Length; k++)
					{
						componentsInChildren3[k].FL = carProperties.FL;
						componentsInChildren3[k].FR = carProperties.FR;
						componentsInChildren3[k].RL = carProperties.RL;
						componentsInChildren3[k].RR = carProperties.RR;
					}
				}
			}
			if (this.fixedwelds == 0f && this.fixedImportantBolts == 0f && this.tightnuts == 0f)
			{
				if (base.gameObject.layer == LayerMask.NameToLayer("LooseParts") || base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast") || base.gameObject.layer == LayerMask.NameToLayer("Repair"))
				{
					if (base.gameObject.GetComponent<Rigidbody>() == null)
					{
						base.gameObject.AddComponent<Rigidbody>();
						base.gameObject.GetComponent<Rigidbody>().mass = this.weight;
						if (this.SmallObject)
						{
							base.gameObject.GetComponent<Rigidbody>().drag = 6f;
						}
					}
					if (base.gameObject.GetComponent<PickupSpring>())
					{
						base.gameObject.AddComponent<FixedJoint>();
						base.gameObject.GetComponent<FixedJoint>().breakForce = 1f;
					}
					if (base.gameObject.GetComponent<PickupSpring>() == null && base.gameObject.transform.parent)
					{
						base.gameObject.AddComponent<FixedJoint>();
						if (base.transform.root.name == "TireMounter")
						{
							base.gameObject.GetComponent<FixedJoint>().breakForce = 200000f;
						}
						else
						{
							base.gameObject.GetComponent<FixedJoint>().breakForce = 20000f;
						}
					}
					base.gameObject.layer = LayerMask.NameToLayer("LooseParts");
					if (base.gameObject.transform.parent)
					{
						if (base.gameObject.transform.parent.parent.GetComponent<Rigidbody>() != null)
						{
							base.gameObject.GetComponent<FixedJoint>().connectedBody = base.gameObject.transform.parent.parent.GetComponent<Rigidbody>();
						}
						else if (base.gameObject.transform.root.GetComponent<Rigidbody>())
						{
							base.gameObject.GetComponent<FixedJoint>().connectedBody = base.gameObject.transform.root.GetComponent<Rigidbody>();
						}
					}
					base.gameObject.layer = LayerMask.NameToLayer("LooseParts");
				}
				Partinfo[] componentsInChildren4 = base.GetComponentsInChildren<Partinfo>();
				for (int j = 0; j < componentsInChildren4.Length; j++)
				{
					componentsInChildren4[j].fixedImportantBolts = 0f;
				}
				BoltNut[] componentsInChildren5 = base.GetComponentsInChildren<BoltNut>();
				for (int j = 0; j < componentsInChildren5.Length; j++)
				{
					componentsInChildren5[j].StartFromMain();
				}
				CarProperties[] componentsInChildren2 = base.GetComponentsInChildren<CarProperties>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					componentsInChildren2[j].Remove();
				}
				if (base.gameObject.tag == "Wheel1")
				{
					componentsInChildren2 = base.GetComponentsInChildren<CarProperties>();
					for (int j = 0; j < componentsInChildren2.Length; j++)
					{
						componentsInChildren2[j].WheelTriggersOff();
					}
				}
				GameObject.Find("Player").GetComponent<tools>().SphereJump();
			}
			base.StartCoroutine(this.GroundCHeck());
			if (fall)
			{
				base.StartCoroutine(this.Fall());
				this.AllowFall = true;
				return;
			}
			this.AllowFall = false;
		}
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0005B5A9 File Offset: 0x000597A9
	public void CheckGround()
	{
		base.StartCoroutine(this.GroundCHeck());
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0005B5B8 File Offset: 0x000597B8
	private IEnumerator GroundCHeck()
	{
		base.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
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
		if (terrain && !this.InBackpack && position.y < terrain.SampleHeight(base.transform.position))
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

	// Token: 0x0600093C RID: 2364 RVA: 0x0005B5C7 File Offset: 0x000597C7
	private IEnumerator SetDrag()
	{
		yield return new WaitForSeconds(2f);
		if (base.GetComponent<Rigidbody>())
		{
			base.gameObject.GetComponent<Rigidbody>().drag = 0f;
		}
		yield break;
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0005B5D6 File Offset: 0x000597D6
	private IEnumerator Fall()
	{
		yield return new WaitForSeconds(3f);
		if (this.fixedwelds == 0f && this.fixedImportantBolts == 0f && this.tightnuts == 0f && this.AllowFall && (!base.transform.parent || (base.transform.parent && base.transform.parent.GetComponent<transparents>() && !base.transform.parent.GetComponent<transparents>().ATTACHED)))
		{
			if (base.GetComponent<CarProperties>())
			{
				base.GetComponent<CarProperties>().RestartColliders();
			}
			if (base.GetComponent<FixedJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			}
			if (base.GetComponent<HingeJoint>())
			{
				UnityEngine.Object.Destroy(base.GetComponent<HingeJoint>());
			}
			if (base.gameObject.GetComponent<Rigidbody>() == null && !this.InBackpack)
			{
				base.gameObject.AddComponent<Rigidbody>();
				base.gameObject.GetComponent<Rigidbody>().mass = this.weight;
				if (this.SmallObject)
				{
					base.gameObject.GetComponent<Rigidbody>().drag = 6f;
				}
			}
			if (base.transform.parent != null && base.transform.parent.gameObject.GetComponent<transparents>())
			{
				base.transform.parent.gameObject.GetComponent<transparents>().UninstallATTACHABLES();
				base.transform.parent.gameObject.GetComponent<transparents>().HaveAttached = false;
			}
			base.gameObject.transform.SetParent(null);
			yield return new WaitForSeconds(3f);
			base.StartCoroutine(this.GroundCHeck());
		}
		yield break;
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0005B5E5 File Offset: 0x000597E5
	private IEnumerator WaitSec()
	{
		yield return 2;
		if (base.gameObject.transform.parent)
		{
			base.gameObject.transform.localScale = base.transform.parent.localScale;
		}
		base.StartCoroutine(this.WaitSec2());
		yield break;
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x0005B5F4 File Offset: 0x000597F4
	private IEnumerator WaitSec2()
	{
		yield return new WaitForSeconds(6f);
		if (base.gameObject.transform.parent)
		{
			base.gameObject.transform.localScale = base.transform.parent.localScale;
		}
		if (base.gameObject.tag == "Wheel1")
		{
			CarProperties[] componentsInChildren = base.GetComponentsInChildren<CarProperties>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].WheelTriggersOff();
			}
		}
		yield break;
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0005B604 File Offset: 0x00059804
	public void attach()
	{
		base.transform.position = base.transform.parent.position;
		if (!base.GetComponent<OpenDoor>() || !base.GetComponent<OpenDoor>().doorOpened)
		{
			base.transform.rotation = base.transform.parent.rotation;
		}
		if (base.gameObject.transform.root.GetComponent<VehicleDamage>())
		{
			base.gameObject.transform.root.GetComponent<VehicleDamage>().Start();
		}
		if (base.gameObject.layer == LayerMask.NameToLayer("LooseParts"))
		{
			UnityEngine.Object.Destroy(base.GetComponent<FixedJoint>());
			UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
			base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		}
		if (base.gameObject.layer == LayerMask.NameToLayer("Windows"))
		{
			UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		}
		if (base.gameObject.layer == LayerMask.NameToLayer("OpenableParts") && base.gameObject.GetComponent<HingeJoint>())
		{
			base.gameObject.GetComponent<HingeJoint>().breakForce = 20000f;
		}
		base.gameObject.transform.position = base.transform.parent.position;
		if (base.gameObject.layer != LayerMask.NameToLayer("OpenableParts"))
		{
			base.gameObject.transform.rotation = base.transform.parent.rotation;
		}
		FixedJoint[] componentsInChildren = base.gameObject.GetComponentsInChildren<FixedJoint>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].connectedBody = base.gameObject.transform.root.GetComponent<Rigidbody>();
		}
		HingeJoint[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<HingeJoint>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].connectedBody = base.gameObject.transform.root.GetComponent<Rigidbody>();
		}
		if (base.GetComponent<CarProperties>() && base.GetComponent<CarProperties>().triger)
		{
			base.gameObject.GetComponent<MeshCollider>().isTrigger = true;
		}
		if (base.gameObject.layer != LayerMask.NameToLayer("OpenableParts"))
		{
			foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
			{
				carProperties.ReStart();
				carProperties.Attach();
			}
		}
		Partinfo[] componentsInChildren4 = base.GetComponentsInChildren<Partinfo>();
		for (int k = 0; k < componentsInChildren4.Length; k++)
		{
			componentsInChildren4[k].RemoveTouchable();
		}
		if (base.gameObject.tag == "Wheel1")
		{
			base.gameObject.GetComponent<MeshCollider>().isTrigger = false;
			CarProperties[] componentsInChildren3 = base.GetComponentsInChildren<CarProperties>();
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				componentsInChildren3[k].WheelTriggersOn();
			}
		}
		if (base.GetComponent<MPobject>())
		{
			base.GetComponent<MPobject>().networkDummy.GetComponent<NetworkTransform>().enabled = false;
			base.GetComponent<MPobject>().networkDummy.enabled = false;
		}
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0005B911 File Offset: 0x00059B11
	public void AddTouchable()
	{
		if (base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast") && this.EnabledTouchable)
		{
			base.gameObject.layer = LayerMask.NameToLayer("LooseParts");
		}
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0005B947 File Offset: 0x00059B47
	public void RemoveTouchable()
	{
		if (base.gameObject.layer == LayerMask.NameToLayer("LooseParts") && this.EnabledTouchable)
		{
			base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		}
	}

	// Token: 0x0400112D RID: 4397
	public bool SmallObject;

	// Token: 0x0400112E RID: 4398
	public bool DontShowInCatalog;

	// Token: 0x0400112F RID: 4399
	public bool DontSpawnInJunyard;

	// Token: 0x04001130 RID: 4400
	public bool CanPutInBox;

	// Token: 0x04001131 RID: 4401
	public bool InBackpack;

	// Token: 0x04001132 RID: 4402
	public Texture2D Thumbnail;

	// Token: 0x04001133 RID: 4403
	public GameObject player;

	// Token: 0x04001134 RID: 4404
	public float attachedparts;

	// Token: 0x04001135 RID: 4405
	public float attachedbolts;

	// Token: 0x04001136 RID: 4406
	public float tightnuts;

	// Token: 0x04001137 RID: 4407
	public float attachedwelds;

	// Token: 0x04001138 RID: 4408
	public float fixedwelds;

	// Token: 0x04001139 RID: 4409
	public float ImportantBolts;

	// Token: 0x0400113A RID: 4410
	public float fixedImportantBolts;

	// Token: 0x0400113B RID: 4411
	public float ChildrenFixedBolts;

	// Token: 0x0400113C RID: 4412
	public bool DontRotateWhenAttaching;

	// Token: 0x0400113D RID: 4413
	public bool Openable;

	// Token: 0x0400113E RID: 4414
	public GameObject HingePivot;

	// Token: 0x0400113F RID: 4415
	public bool Rdoor;

	// Token: 0x04001140 RID: 4416
	public bool Ldoor;

	// Token: 0x04001141 RID: 4417
	public bool Trunk;

	// Token: 0x04001142 RID: 4418
	public bool Hood;

	// Token: 0x04001143 RID: 4419
	public bool HoodHalf;

	// Token: 0x04001144 RID: 4420
	public bool EnabledTouchable;

	// Token: 0x04001145 RID: 4421
	public GameObject AudioParent;

	// Token: 0x04001146 RID: 4422
	public string RenamedPrefab;

	// Token: 0x04001147 RID: 4423
	public float weight = 1f;

	// Token: 0x04001148 RID: 4424
	public float price;

	// Token: 0x04001149 RID: 4425
	public bool Suspension;

	// Token: 0x0400114A RID: 4426
	public bool Brakes;

	// Token: 0x0400114B RID: 4427
	public bool Engine;

	// Token: 0x0400114C RID: 4428
	public bool BodyPanel;

	// Token: 0x0400114D RID: 4429
	public bool Interior;

	// Token: 0x0400114E RID: 4430
	public bool Accessories;

	// Token: 0x0400114F RID: 4431
	public bool Light;

	// Token: 0x04001150 RID: 4432
	public bool Window;

	// Token: 0x04001151 RID: 4433
	public bool Rim;

	// Token: 0x04001152 RID: 4434
	public bool Tire;

	// Token: 0x04001153 RID: 4435
	public string[] FitsToCar;

	// Token: 0x04001154 RID: 4436
	public string[] FitsToEngine;

	// Token: 0x04001155 RID: 4437
	public bool AllowFall;
}
