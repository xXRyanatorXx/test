using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NWH.VehiclePhysics2;
using NWH.VehiclePhysics2.VehicleGUI;
using NWH.WheelController3D;
using PaintIn3D;
using RVP;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200015A RID: 346
public class CarProperties : MonoBehaviour
{
	// Token: 0x0600077A RID: 1914 RVA: 0x0003CA94 File Offset: 0x0003AC94
	public void Start()
	{
		if (this.PrefabName == "")
		{
			this.PrefabName = base.transform.name;
		}
		if (base.gameObject.GetComponent<MeshCollider>() && base.gameObject.GetComponent<MeshCollider>().isTrigger && base.gameObject.tag != "Wheel" && !base.transform.parent)
		{
			this.triger = true;
			this.RestartColliders();
		}
		if (this.PartName == "")
		{
			this.PartName = base.transform.name.Remove(base.transform.name.Length - 2).ToString();
		}
		this.MaterialParent = GameObject.Find("MaterialParent");
		if (this.Radiator)
		{
			this.Coolant = base.gameObject.transform.Find("CoolantFluidContainer").gameObject.GetComponent<FLUID>();
		}
		if (this.FuelTank)
		{
			this.Fuel = base.gameObject.transform.Find("FuelContainer").gameObject.GetComponent<FLUID>();
		}
		if (this.OilPan)
		{
			this.EngineOil = base.gameObject.transform.Find("OilFluidContainer").gameObject.GetComponent<FLUID>();
		}
		if (this.BrakeMaster)
		{
			this.BrakeFluid = base.gameObject.transform.Find("BrakeFluidContainer").gameObject.GetComponent<FLUID>();
		}
		if (base.gameObject.GetComponent<DetachablePart>())
		{
			this.DMGRemovablepart = true;
		}
		if (this.Chromed && this.ChromeMat)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.ChromeMat;
		}
		if (!this.started)
		{
			if (base.transform.parent)
			{
				this.Attached = true;
			}
			else if (this.RemovedMesh)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = this.RemovedMesh;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = (base.gameObject.GetComponent<MeshFilter>().mesh = this.RemovedMesh);
			}
			this.x = UnityEngine.Random.Range(1, 4);
			if (this.Condition == 0f)
			{
				this.Condition = 1f;
			}
			this.CleanRatio = 1f;
			this.NoRustRatio = 1f;
			this.PaintRatio = 1f;
			this.StartX = base.transform.localPosition.x;
			this.StartY = base.transform.localPosition.y;
			this.StartZ = base.transform.localPosition.z;
			this.StartXr = base.transform.eulerAngles.x;
			this.StartYr = base.transform.eulerAngles.y;
			this.StartZr = base.transform.eulerAngles.z;
			if (this.Paintable && this.Washable && this.MeshRepairable && !this.CantRust)
			{
				if (tools.TextureQuality == 0f)
				{
					base.GetComponent<P3dPaintableTexture>().Height = 512;
					base.GetComponent<P3dPaintableTexture>().Width = 512;
				}
				else if (tools.TextureQuality == 1f)
				{
					base.GetComponent<P3dPaintableTexture>().Height = 1024;
					base.GetComponent<P3dPaintableTexture>().Width = 1024;
				}
				else if (tools.TextureQuality == 2f)
				{
					base.GetComponent<P3dPaintableTexture>().Height = 2048;
					base.GetComponent<P3dPaintableTexture>().Width = 2048;
				}
				if (this.Condition < 0.4f)
				{
					this.Condition = 0.2f;
					this.BodyMatNumber = UnityEngine.Random.Range(1, 11);
				}
			}
			if (this.Paintable && this.Washable && this.MeshRepairable && this.CantRust)
			{
				if (tools.TextureQuality == 0f)
				{
					base.GetComponent<P3dPaintableTexture>().Height = 512;
					base.GetComponent<P3dPaintableTexture>().Width = 512;
				}
				else if (tools.TextureQuality == 1f)
				{
					base.GetComponent<P3dPaintableTexture>().Height = 1024;
					base.GetComponent<P3dPaintableTexture>().Width = 1024;
				}
				else if (tools.TextureQuality == 2f)
				{
					base.GetComponent<P3dPaintableTexture>().Height = 2048;
					base.GetComponent<P3dPaintableTexture>().Width = 2048;
				}
				if (this.Condition < 0.4f)
				{
					this.Condition = 1f;
				}
			}
			this.started = true;
			if (this.BodyMatNumber == 1)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad1;
			}
			if (this.BodyMatNumber == 2)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad2;
			}
			if (this.BodyMatNumber == 3)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad3;
			}
			if (this.BodyMatNumber == 4)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad4;
			}
			if (this.BodyMatNumber == 5)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad5;
			}
			if (this.BodyMatNumber == 6)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad6;
			}
			if (this.BodyMatNumber == 7)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad7;
			}
			if (this.BodyMatNumber == 8)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad8;
			}
			if (this.BodyMatNumber == 9)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad9;
			}
			if (this.BodyMatNumber == 10)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad10;
			}
		}
		else
		{
			if (this.SinglePart)
			{
				base.StartCoroutine(this.<Start>g__SetPartinfo|344_0());
			}
			if (this.BodyMatNumber == 1)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad1;
			}
			if (this.BodyMatNumber == 2)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad2;
			}
			if (this.BodyMatNumber == 3)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad3;
			}
			if (this.BodyMatNumber == 4)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad4;
			}
			if (this.BodyMatNumber == 5)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad5;
			}
			if (this.BodyMatNumber == 6)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad6;
			}
			if (this.BodyMatNumber == 7)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad7;
			}
			if (this.BodyMatNumber == 8)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad8;
			}
			if (this.BodyMatNumber == 9)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad9;
			}
			if (this.BodyMatNumber == 10)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Bad10;
			}
			if (this.NumberPlate)
			{
				Material[] materials = base.GetComponent<Renderer>().materials;
				materials[2] = this.One;
				materials[3] = this.Two;
				materials[4] = this.Three;
				materials[5] = this.Four;
				materials[6] = this.Five;
				materials[7] = this.Six;
				base.GetComponent<Renderer>().materials = materials;
			}
			foreach (P3dPaintableTexture p3dPaintableTexture in base.GetComponents(typeof(P3dPaintableTexture)))
			{
				if (p3dPaintableTexture.Slot.Name == "_MainTex")
				{
					p3dPaintableTexture.SaveName = this.Texture1;
				}
				if (p3dPaintableTexture.Slot.Name == "_GrungeMap")
				{
					p3dPaintableTexture.SaveName = this.Texture2;
				}
				if (p3dPaintableTexture.Slot.Name == "_L2MetallicRustDustSmoothness")
				{
					p3dPaintableTexture.SaveName = this.Texture3;
				}
				if (p3dPaintableTexture.Slot.Name == "_L2ColorMap")
				{
					p3dPaintableTexture.SaveName = this.Texture4;
				}
				p3dPaintableTexture.Load();
				p3dPaintableTexture.SaveLoad = P3dPaintableTexture.SaveLoadType.Automatic;
			}
		}
		if (this.DMGdeformMesh)
		{
			this.mesh = base.GetComponent<MeshFilter>().mesh;
			this.vertices = this.mesh.vertices;
			this.initialvertices = this.mesh.vertices;
		}
		if (base.GetComponent<WheelController>())
		{
			base.transform.localPosition = new Vector3(this.StartX, this.StartY, this.StartZ);
		}
		if (this.SteeringWheel)
		{
			base.transform.localRotation = Quaternion.identity;
		}
		if (this.OriginalInterior == 1)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior1;
			this.WornMaterial = this.MaterialParent.GetComponent<CarMaterials>().BadInterior1;
		}
		else if (this.OriginalInterior == 2)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior2;
			this.WornMaterial = this.MaterialParent.GetComponent<CarMaterials>().BadInterior2;
		}
		else if (this.OriginalInterior == 3)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior3;
			this.WornMaterial = this.MaterialParent.GetComponent<CarMaterials>().BadInterior3;
		}
		else if (this.OriginalInterior == 4)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior4;
			this.WornMaterial = this.MaterialParent.GetComponent<CarMaterials>().BadInterior4;
		}
		else if (this.OriginalInterior == 5)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior5;
			this.WornMaterial = this.MaterialParent.GetComponent<CarMaterials>().BadInterior5;
		}
		else if (this.OriginalInterior == 6)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior6;
			this.WornMaterial = this.MaterialParent.GetComponent<CarMaterials>().BadInterior6;
		}
		else if (this.OriginalInterior == 7)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior7;
			this.WornMaterial = this.MaterialParent.GetComponent<CarMaterials>().BadInterior7;
		}
		else if (this.OriginalInterior == 8)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Interior8;
			this.WornMaterial = this.MaterialParent.GetComponent<CarMaterials>().BadInterior8;
		}
		if (this.Tintable)
		{
			if (this.TintLevel == 0)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint0;
			}
			if (this.TintLevel == 1)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint1;
			}
			if (this.TintLevel == 2)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint2;
			}
			if (this.TintLevel == 3)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint3;
			}
			if (this.TintLevel == 4)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint4;
			}
		}
		this.ReStart();
		foreach (P3dChangeCounter p3dChangeCounter in base.GetComponents<P3dChangeCounter>())
		{
			if (p3dChangeCounter.Threshold == 0.5f)
			{
				this.RustChangeCounter = p3dChangeCounter;
			}
		}
		this.NoRustRatio = 1f;
		this.MaterialParent = GameObject.Find("CarColors");
		this.AudioParent = GameObject.Find("hand");
		this.RepairDecal = GameObject.Find("RepairDecals").GetComponent<P3dPaintDecal>();
		this.RustRepairDecal = GameObject.Find("RustRepairDecals").GetComponent<P3dPaintDecal>();
		this.RustRepairedDecal = GameObject.Find("RustRepairedDecal").GetComponent<P3dPaintDecal>();
		this.PaintRemoveDecal = GameObject.Find("PaintRemoveDecals").GetComponent<P3dPaintDecal>();
		this.WashDecal = GameObject.Find("WashDecals").GetComponent<P3dPaintDecal>();
		if (this.MeshDamaged || this.MeshLittleDamaged)
		{
			this.mesh = base.GetComponent<MeshFilter>().mesh;
			this.mesh.vertices = this.Damagedvertices;
		}
		this.PreventChildCollisions();
		if (base.transform.name == "SparkPlug")
		{
			base.GetComponent<Partinfo>().attachedbolts = 0f;
			base.GetComponent<Partinfo>().tightnuts = 0f;
		}
		if (!base.transform.parent)
		{
			base.StartCoroutine(this.FinishedInitializing());
		}
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x0003D7EC File Offset: 0x0003B9EC
	public void MPStart()
	{
		foreach (P3dPaintableTexture p3dPaintableTexture in base.GetComponents(typeof(P3dPaintableTexture)))
		{
			if (p3dPaintableTexture.Slot.Name == "_MainTex")
			{
				p3dPaintableTexture.SaveName = this.Texture1;
			}
			if (p3dPaintableTexture.Slot.Name == "_GrungeMap")
			{
				p3dPaintableTexture.SaveName = this.Texture2;
			}
			if (p3dPaintableTexture.Slot.Name == "_L2MetallicRustDustSmoothness")
			{
				p3dPaintableTexture.SaveName = this.Texture3;
			}
			if (p3dPaintableTexture.Slot.Name == "_L2ColorMap")
			{
				p3dPaintableTexture.SaveName = this.Texture4;
			}
			p3dPaintableTexture.Load();
			p3dPaintableTexture.SaveLoad = P3dPaintableTexture.SaveLoadType.Automatic;
		}
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x0003D8C0 File Offset: 0x0003BAC0
	private IEnumerator FinishedInitializing()
	{
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		Partinfo[] componentsInChildren = base.GetComponentsInChildren<Partinfo>();
		foreach (Partinfo partinfo in componentsInChildren)
		{
			partinfo.fixedImportantBolts = 0f;
			partinfo.fixedwelds = 0f;
		}
		BoltNut[] componentsInChildren2 = base.GetComponentsInChildren<BoltNut>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].StartFromMain();
		}
		WeldCut[] componentsInChildren3 = base.GetComponentsInChildren<WeldCut>();
		for (int i = 0; i < componentsInChildren3.Length; i++)
		{
			componentsInChildren3[i].StartFromMain();
		}
		foreach (Partinfo partinfo2 in componentsInChildren)
		{
			if ((partinfo2.attachedbolts > 0f || partinfo2.attachedwelds > 0f || partinfo2.ImportantBolts > 0f) && partinfo2.tightnuts == 0f && partinfo2.fixedwelds == 0f && partinfo2.fixedImportantBolts == 0f && base.transform.root.name != "EngineStand")
			{
				partinfo2.remove(true);
			}
		}
		yield break;
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x0003D8D0 File Offset: 0x0003BAD0
	public void PreventChildCollisions()
	{
		if (!base.transform.parent)
		{
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				for (int j = i + 1; j < componentsInChildren.Length; j++)
				{
					if (!(componentsInChildren[i] == componentsInChildren[j]))
					{
						Physics.IgnoreCollision(componentsInChildren[i], componentsInChildren[j]);
					}
				}
			}
		}
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0003D92C File Offset: 0x0003BB2C
	public void LoadNumber()
	{
		Material[] materials = base.GetComponent<Renderer>().materials;
		materials[2] = this.One;
		materials[3] = this.Two;
		materials[4] = this.Three;
		materials[5] = this.Four;
		materials[6] = this.Five;
		materials[7] = this.Six;
		base.GetComponent<Renderer>().materials = materials;
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x0003D988 File Offset: 0x0003BB88
	public void RestartColliders()
	{
		if (base.gameObject.GetComponent<MeshCollider>())
		{
			if (base.gameObject.tag != "Wheel1")
			{
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
				MeshCollider meshCollider = base.gameObject.AddComponent<MeshCollider>();
				meshCollider.convex = true;
				meshCollider.isTrigger = this.triger;
				if (this.Differentmesh)
				{
					meshCollider.sharedMesh = this.Differentmesh;
				}
			}
			if (base.gameObject.tag == "Wheel1" && base.transform.root.tag == "Vehicle")
			{
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
				base.gameObject.AddComponent<MeshCollider>().convex = true;
				base.gameObject.GetComponent<MeshCollider>().isTrigger = true;
			}
			if (base.gameObject.tag == "Wheel1" && base.transform.root.tag != "Vehicle")
			{
				UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
				base.gameObject.AddComponent<MeshCollider>().convex = true;
				base.gameObject.GetComponent<MeshCollider>().isTrigger = false;
			}
		}
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x0003DAD3 File Offset: 0x0003BCD3
	public void WheelTriggersOn()
	{
		if (base.gameObject.tag == "Wheel1")
		{
			base.gameObject.GetComponent<MeshCollider>().isTrigger = true;
		}
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x0003DAFD File Offset: 0x0003BCFD
	public void WheelTriggersOff()
	{
		if (base.gameObject.tag == "Wheel1")
		{
			base.gameObject.GetComponent<MeshCollider>().isTrigger = false;
		}
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x0003DB28 File Offset: 0x0003BD28
	public void SavingGame()
	{
		if (this.BrakeFluid)
		{
			this.FluidSize = this.BrakeFluid.FluidSize;
			this.FluidCondition = this.BrakeFluid.Condition;
		}
		if (this.Coolant)
		{
			this.FluidSize = this.Coolant.FluidSize;
			this.FluidCondition = this.Coolant.Condition;
		}
		if (this.EngineOil)
		{
			this.FluidSize = this.EngineOil.FluidSize;
			this.FluidCondition = this.EngineOil.Condition;
		}
		if (this.Fuel)
		{
			this.FluidSize = this.Fuel.FluidSize;
			this.FluidCondition = this.Fuel.Condition;
			this.DieselPercent = this.Fuel.DieselPercent;
		}
		if (this.MeshDamaged || this.MeshLittleDamaged)
		{
			this.mesh = base.GetComponent<MeshFilter>().mesh;
			this.Damagedvertices = this.mesh.vertices;
		}
		if (base.gameObject.layer == LayerMask.NameToLayer("LooseParts"))
		{
			base.gameObject.transform.SetParent(null);
		}
		if (base.gameObject.layer == LayerMask.NameToLayer("OpenableParts") && base.gameObject.GetComponent<OpenDoor>() && base.gameObject.GetComponent<Partinfo>().tightnuts == 0f)
		{
			base.gameObject.transform.SetParent(null);
		}
		foreach (P3dPaintableTexture p3dPaintableTexture in base.GetComponents(typeof(P3dPaintableTexture)))
		{
			if (p3dPaintableTexture.Slot.Name == "_MainTex")
			{
				this.Texture1 = Convert.ToBase64String(p3dPaintableTexture.GetPngData(false));
			}
			if (p3dPaintableTexture.Slot.Name == "_L2MetallicRustDustSmoothness")
			{
				this.Texture3 = Convert.ToBase64String(p3dPaintableTexture.GetPngData(false));
			}
			if (p3dPaintableTexture.Slot.Name == "_L2ColorMap")
			{
				this.Texture4 = Convert.ToBase64String(p3dPaintableTexture.GetPngData(false));
			}
		}
		this.Loose.Clear();
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<HexNut>() && !transform.GetComponent<HexNut>().tight)
			{
				this.Loose.Add(transform.name.ToString());
			}
			if (transform.GetComponent<BoltNut>() && !transform.GetComponent<BoltNut>().tight)
			{
				this.Loose.Add(transform.name.ToString());
			}
			if (transform.GetComponent<FlatNut>() && !transform.GetComponent<FlatNut>().tight)
			{
				this.Loose.Add(transform.name.ToString());
			}
			if (transform.GetComponent<WeldCut>() && !transform.GetComponent<WeldCut>().welded)
			{
				this.Loose.Add(transform.name.ToString());
			}
		}
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0003DE7C File Offset: 0x0003C07C
	public void SetMesh()
	{
		if (base.transform.root.GetComponent<MainCarProperties>())
		{
			this.MainProperties = base.transform.root.GetComponent<MainCarProperties>();
			if (this.MainProperties.EngineType == 0)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh;
				return;
			}
			if (this.MainProperties.EngineType == 1)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh1;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh1;
				return;
			}
			if (this.MainProperties.EngineType == 2)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh2;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh2;
				return;
			}
			if (this.MainProperties.EngineType == 3)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh3;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh3;
				return;
			}
			if (this.MainProperties.EngineType == 4)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh4;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh4;
				return;
			}
		}
		else if (base.transform.root.GetComponent<CarProperties>() && base.transform.root.GetComponent<CarProperties>().EngineBlock)
		{
			CarProperties component = base.transform.root.GetComponent<CarProperties>();
			if (component.Type == 0)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh;
				return;
			}
			if (component.Type == 1)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh1;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh1;
				return;
			}
			if (component.Type == 2)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh2;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh2;
				return;
			}
			if (component.Type == 3)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh3;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh3;
				return;
			}
			if (component.Type == 4)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh4;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh4;
				return;
			}
		}
		else if (base.transform.root.GetComponent<AutoAttach>() && base.transform.root.GetComponent<AutoAttach>().CheckSpot.transform.childCount > 0)
		{
			CarProperties component2 = base.transform.root.GetComponent<AutoAttach>().CheckSpot.transform.GetChild(0).GetComponent<CarProperties>();
			if (component2.Type == 0)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh;
				return;
			}
			if (component2.Type == 1)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh1;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh1;
				return;
			}
			if (component2.Type == 2)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh2;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh2;
				return;
			}
			if (component2.Type == 3)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh3;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh3;
				return;
			}
			if (component2.Type == 4)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh4;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh4;
				return;
			}
		}
		else
		{
			base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh;
			base.gameObject.GetComponent<MeshCollider>().sharedMesh = base.transform.parent.GetComponent<transparents>().ChildrenMesh;
		}
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x0003E4B8 File Offset: 0x0003C6B8
	public void ReStart()
	{
		if (this.Gearbox && this.TransmissionGearingProfile == null)
		{
			foreach (GearProfile gearProfile in base.GetComponents(typeof(GearProfile)))
			{
				if (gearProfile.Profilenumber == 0)
				{
					this.TransmissionGearingProfile = gearProfile;
				}
				if (gearProfile.Profilenumber == 1)
				{
					this.TransmissionGearingbroken1 = gearProfile;
				}
				if (gearProfile.Profilenumber == 2)
				{
					this.TransmissionGearingbroken2 = gearProfile;
				}
				if (gearProfile.Profilenumber == 3)
				{
					this.TransmissionGearingbroken3 = gearProfile;
				}
			}
		}
		if (base.transform.parent && base.transform.parent.GetComponent<transparents>() && base.transform.parent.name == base.gameObject.name)
		{
			base.transform.rotation = base.transform.parent.rotation;
		}
		if (base.transform.parent)
		{
			this.Attached = true;
			if (base.transform.parent.GetComponent<transparents>())
			{
				this.SavePosition = base.transform.parent.GetComponent<transparents>().SavePosition;
			}
		}
		else
		{
			this.Attached = false;
		}
		if (base.transform.parent && this.RemovedDifferentMesh && base.transform.parent.GetComponent<transparents>())
		{
			this.SetMesh();
		}
		if (base.transform.root.tag != "Vehicle" && (this.VisualWheel || this.NonRotatingVisualWheel))
		{
			base.transform.localPosition = new Vector3(this.StartX, this.StartY, this.StartZ);
		}
		if (this.RealWheel && base.transform.parent && base.transform.parent.parent)
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
		if (base.transform.root.tag == "Vehicle")
		{
			this.exp = base.transform.root.GetComponent<VehicleController>();
			this.MainProperties = base.transform.root.GetComponent<MainCarProperties>();
			if (base.transform.parent && base.transform.parent.GetComponent<transparents>())
			{
				this.FL = base.transform.parent.GetComponent<transparents>().FL;
				this.FR = base.transform.parent.GetComponent<transparents>().FR;
				this.RL = base.transform.parent.GetComponent<transparents>().RL;
				this.RR = base.transform.parent.GetComponent<transparents>().RR;
				this.FRONT = base.transform.parent.GetComponent<transparents>().FRONT;
				this.REAR = base.transform.parent.GetComponent<transparents>().REAR;
			}
			if ((this.RealWheel || this.Spring) && base.transform.parent.parent.parent && base.transform.parent.parent.parent.GetComponent<transparents>())
			{
				if (base.transform.root.GetComponent<MainCarProperties>())
				{
					Vector3 vector = base.transform.root.InverseTransformPoint(base.transform.position);
					if (vector.x < 0f && vector.z < 0f)
					{
						this.RL = true;
					}
					else
					{
						this.RL = false;
					}
					if (vector.x > 0f && vector.z < 0f)
					{
						this.RR = true;
					}
					else
					{
						this.RR = false;
					}
					if (vector.x < 0f && vector.z > 0f)
					{
						this.FL = true;
					}
					else
					{
						this.FL = false;
					}
					if (vector.x > 0f && vector.z > 0f)
					{
						this.FR = true;
					}
					else
					{
						this.FR = false;
					}
				}
				else
				{
					this.FL = base.transform.parent.parent.parent.GetComponent<transparents>().FL;
					this.FR = base.transform.parent.parent.parent.GetComponent<transparents>().FR;
					this.RL = base.transform.parent.parent.parent.GetComponent<transparents>().RL;
					this.RR = base.transform.parent.parent.parent.GetComponent<transparents>().RR;
				}
			}
			if (this.BrakeDisc && base.transform.parent.parent.parent.GetComponent<transparents>())
			{
				this.FL = base.transform.parent.parent.parent.GetComponent<transparents>().FL;
				this.FR = base.transform.parent.parent.parent.GetComponent<transparents>().FR;
				this.RL = base.transform.parent.parent.parent.GetComponent<transparents>().RL;
				this.RR = base.transform.parent.parent.parent.GetComponent<transparents>().RR;
			}
			if (this.FL)
			{
				foreach (Transform transform2 in base.transform.root.GetComponentsInChildren<Transform>())
				{
					if (transform2.name == "WheelControllerFL")
					{
						this.WheelController = transform2.gameObject;
					}
				}
			}
			if (this.FR)
			{
				foreach (Transform transform3 in base.transform.root.GetComponentsInChildren<Transform>())
				{
					if (transform3.name == "WheelControllerFR")
					{
						this.WheelController = transform3.gameObject;
					}
				}
			}
			if (this.RL)
			{
				foreach (Transform transform4 in base.transform.root.GetComponentsInChildren<Transform>())
				{
					if (transform4.name == "WheelControllerRL")
					{
						this.WheelController = transform4.gameObject;
					}
				}
			}
			if (this.RR)
			{
				foreach (Transform transform5 in base.transform.root.GetComponentsInChildren<Transform>())
				{
					if (transform5.name == "WheelControllerRR")
					{
						this.WheelController = transform5.gameObject;
					}
				}
			}
			if (this.FRONT)
			{
				foreach (Transform transform6 in base.transform.root.GetComponentsInChildren<Transform>())
				{
					if (transform6.name == "WheelControllerFront")
					{
						this.WheelController = transform6.gameObject;
					}
				}
			}
			if (this.REAR)
			{
				foreach (Transform transform7 in base.transform.root.GetComponentsInChildren<Transform>())
				{
					if (transform7.name == "WheelControllerRear")
					{
						this.WheelController = transform7.gameObject;
					}
				}
			}
			if (this.VisualWheel)
			{
				foreach (object obj in this.WheelController.transform)
				{
					Transform transform8 = (Transform)obj;
					if (transform8.name == "VISUAL")
					{
						this.unmountedWheel = transform8.transform.gameObject;
					}
				}
				this.WheelController.GetComponent<WheelController>().Visual = base.gameObject;
				this.WheelController.GetComponent<WheelController>().cachedVisualTransform = base.transform;
			}
			if (this.NonRotatingVisualWheel)
			{
				this.WheelController.GetComponent<WheelController>().NonRotatingVisual = base.gameObject;
			}
			if (this.AffectsFRSuspensionPosition)
			{
				foreach (Transform transform9 in base.transform.root.GetComponentsInChildren<Transform>())
				{
					if (transform9.name == "FRSuspensionPosition")
					{
						this.FRSuspensionPosition = transform9.gameObject;
					}
				}
			}
			if (this.AffectsFLSuspensionPosition)
			{
				foreach (Transform transform10 in base.transform.root.GetComponentsInChildren<Transform>())
				{
					if (transform10.name == "FLSuspensionPosition")
					{
						this.FLSuspensionPosition = transform10.gameObject;
					}
				}
			}
			if (this.AffectsRLSuspensionPositionX || this.AffectsRLSuspensionPositionZ)
			{
				foreach (Transform transform11 in base.transform.root.GetComponentsInChildren<Transform>())
				{
					if (transform11.name == "RLSuspensionPosition")
					{
						this.RLSuspensionPosition = transform11.gameObject;
					}
				}
			}
			if (this.AffectsRRSuspensionPositionX || this.AffectsRRSuspensionPositionZ)
			{
				foreach (Transform transform12 in base.transform.root.GetComponentsInChildren<Transform>())
				{
					if (transform12.name == "RRSuspensionPosition")
					{
						this.RRSuspensionPosition = transform12.gameObject;
					}
				}
			}
			if (this.BrakePad || this.BrakeDisc)
			{
				if (this.Condition > 0.4f)
				{
					this.RealCondition = 1f;
				}
				if (this.Condition < 0.4f)
				{
					this.RealCondition = 0.7f;
					base.gameObject.GetComponent<MeshFilter>().mesh = this.WornMesh;
				}
				if (this.Condition < 0.1f)
				{
					this.RealCondition = 0.1f;
					base.gameObject.GetComponent<MeshFilter>().mesh = this.RuinedMesh;
				}
			}
			if (this.BrakeLine)
			{
				if (this.Condition > 0.1f)
				{
					this.RealCondition = 1f;
				}
				else
				{
					this.RealCondition = 0f;
					base.gameObject.GetComponent<MeshFilter>().mesh = this.RuinedMesh;
				}
				if (this.Condition < 0.4f)
				{
					base.GetComponent<Renderer>().sharedMaterial = this.WornMaterial;
				}
				if (this.FL)
				{
					this.MainProperties.BrakeLIneFL = base.transform.gameObject;
				}
				if (this.FR)
				{
					this.MainProperties.BrakeLIneFR = base.transform.gameObject;
				}
				if (this.RL)
				{
					this.MainProperties.BrakeLIneRL = base.transform.gameObject;
				}
				if (this.RR)
				{
					this.MainProperties.BrakeLIneRR = base.transform.gameObject;
				}
			}
			if (this.MainBrakeLine)
			{
				this.MainProperties.MainBrakeLIne = base.transform.gameObject;
				if (this.Condition < 0.4f)
				{
					base.GetComponent<Renderer>().sharedMaterial = this.WornMaterial;
				}
			}
			if (this.BrakeFluid)
			{
				this.MainProperties.BrakeFluid = this.BrakeFluid;
			}
			if (this.BrakePedal)
			{
				this.MainProperties.BrakePedal = this;
			}
			if (this.HandBrake)
			{
				if (base.GetComponent<HandbrakeScr>())
				{
					base.GetComponent<HandbrakeScr>().attach();
				}
				this.MainProperties.Handbrake = base.gameObject;
				this.MainProperties.HandBrakeInstalled = true;
			}
			if (this.HandBrakeCable)
			{
				this.MainProperties.HandBrakeCableInstalled = true;
				this.MainProperties.HandbrakeCable = base.gameObject;
				if (this.Condition < 0.1f && this.RuinedMesh)
				{
					base.GetComponent<MeshFilter>().mesh = this.RuinedMesh;
				}
			}
			if (this.BrakePad && base.transform.parent.gameObject.GetComponent<transparents>().BrakePadA)
			{
				this.WheelController.GetComponent<WheelController>().BrakePadACondition = this.RealCondition;
			}
			if (this.BrakePad && base.transform.parent.gameObject.GetComponent<transparents>().BrakePadB)
			{
				this.WheelController.GetComponent<WheelController>().BrakePadBCondition = this.RealCondition;
			}
			if (this.BrakeDisc)
			{
				this.WheelController.GetComponent<WheelController>().BrakeDiscCondition = this.RealCondition;
			}
			if (this.AffectsHandling && this.Condition < 0.4f)
			{
				base.GetComponent<Renderer>().sharedMaterial = this.WornMaterial;
			}
			if (this.AffectsHandling && this.Condition < 0.1f && this.RuinedMesh)
			{
				base.GetComponent<MeshFilter>().mesh = this.RuinedMesh;
			}
			if (this.Tire)
			{
				if (base.transform.root.tag == "Vehicle")
				{
					base.gameObject.GetComponent<MeshCollider>().isTrigger = true;
				}
				base.transform.parent.parent.GetComponent<CarProperties>().tireObject = this;
				if (this.Condition < 0.4f)
				{
					base.GetComponent<Renderer>().sharedMaterial = this.WornMaterial;
				}
				if (this.Condition < 0.1f)
				{
					base.GetComponent<Renderer>().sharedMaterial = this.RuinedMaterial;
				}
				base.transform.parent.parent.GetComponent<CarProperties>().ReStart();
				if (base.transform.root.tag != "Vehicle")
				{
					base.GetComponent<MeshCollider>().isTrigger = false;
				}
			}
			if (this.RealWheel)
			{
				if (base.transform.root.tag == "Vehicle")
				{
					base.gameObject.GetComponent<MeshCollider>().isTrigger = true;
				}
				else
				{
					base.gameObject.GetComponent<MeshCollider>().isTrigger = false;
				}
				if (this.tireObject)
				{
					if (base.transform.parent.GetComponent<transparents>().OuterTire)
					{
						this.WheelController.GetComponent<WheelController>().tireObject2 = this.tireObject;
					}
					else
					{
						this.WheelController.GetComponent<WheelController>().tireObject = this.tireObject;
					}
					this.WheelController.GetComponent<WheelController>().SetRadius();
					if (this.tireObject.TireType == 1f)
					{
						this.forwSlipCoef = 0.8f;
						this.forwForcCoef = 1.1f;
						this.sideSlipCoef = 0.8f;
						this.sideForcCoef = 1.1f;
						this.WheelController.GetComponent<WheelController>().dragTorque = 10f;
					}
					if (this.tireObject.TireType == 2f)
					{
						this.forwSlipCoef = 0.8f;
						this.forwForcCoef = 1.1f;
						this.sideSlipCoef = 0.8f;
						this.sideForcCoef = 1.1f;
						this.WheelController.GetComponent<WheelController>().dragTorque = 100f;
					}
					if (this.tireObject.TireType == 3f)
					{
						this.forwSlipCoef = 0.75f;
						this.forwForcCoef = 1.8f;
						if (this.RR || this.RL)
						{
							this.forwForcCoef = 2f;
						}
						this.sideSlipCoef = 0.6f;
						if (this.FL || this.FR)
						{
							this.sideSlipCoef = 0.7f;
						}
						this.sideForcCoef = 1.5f;
						this.WheelController.GetComponent<WheelController>().dragTorque = 10f;
					}
					if (this.tireObject.Condition < 0.1f)
					{
						this.forwSlipCoef = 1f;
						this.forwForcCoef = 1.4f;
						if (this.RR || this.RL)
						{
							this.forwForcCoef = 1.6f;
						}
						this.sideSlipCoef = 1f;
						this.sideForcCoef = 0.8f;
						this.WheelController.GetComponent<WheelController>().dragTorque = 10f;
					}
					if (this.tireObject.Condition < 0f)
					{
						this.forwSlipCoef = 1.3f;
						this.forwForcCoef = 1f;
						if (this.RR || this.RL)
						{
							this.forwForcCoef = 1.2f;
						}
						this.sideSlipCoef = 1.3f;
						this.sideForcCoef = 0.3f;
						this.WheelController.GetComponent<WheelController>().dragTorque = 700f;
					}
					if (this.tireObject.TirePressure < 2f && this.tireObject.Condition > 0f)
					{
						this.WheelController.GetComponent<WheelController>().dragTorque += (2f - this.tireObject.TirePressure) * 40f;
					}
					if (this.tireObject.TirePressure < 0.1f)
					{
						this.forwSlipCoef = 1.3f;
						this.forwForcCoef = 0.3f;
						this.sideSlipCoef = 1.3f;
						this.sideForcCoef = 0.3f;
					}
					if (this.tireObject.TireType == 9f && this.FRONT)
					{
						this.forwSlipCoef = 2f;
						this.forwForcCoef = 2f;
						this.sideSlipCoef = 2f;
						this.sideForcCoef = 1.5f;
						this.WheelController.GetComponent<WheelController>().dragTorque = 10f;
					}
					if (this.tireObject.TireType == 9f && this.REAR)
					{
						this.forwSlipCoef = 1f;
						this.forwForcCoef = 1.3f;
						this.sideSlipCoef = 2f;
						this.sideForcCoef = 2f;
						this.WheelController.GetComponent<WheelController>().dragTorque = 10f;
					}
					if (EnviroSkyMgr.instance && EnviroSkyMgr.instance.Seasons.currentSeasons == EnviroSeasons.Seasons.Winter)
					{
						this.forwForcCoef -= 0.1f;
						this.sideForcCoef -= 0.1f;
					}
				}
				else
				{
					this.WheelController.GetComponent<WheelController>().radius = (this.WheelController.GetComponent<WheelController>().radiusBackup = this.radius);
					this.forwSlipCoef = 1.3f;
					this.forwForcCoef = 0.3f;
					this.sideSlipCoef = 1.3f;
					this.sideForcCoef = 0.3f;
					this.WheelController.GetComponent<WheelController>().dragTorque = 1000f;
				}
				this.WheelController.GetComponent<WheelController>().forwardFriction.slipCoefficient = this.forwSlipCoef;
				this.WheelController.GetComponent<WheelController>().forwSlipCoef = this.forwSlipCoef;
				this.WheelController.GetComponent<WheelController>().forwardFriction.forceCoefficient = this.forwForcCoef;
				this.WheelController.GetComponent<WheelController>().forwForcCoef = this.forwForcCoef;
				this.WheelController.GetComponent<WheelController>().sideFriction.slipCoefficient = this.sideSlipCoef;
				this.WheelController.GetComponent<WheelController>().sideSlipCoef = this.sideSlipCoef;
				this.WheelController.GetComponent<WheelController>().sideFriction.forceCoefficient = this.sideForcCoef;
				this.WheelController.GetComponent<WheelController>().sideForcCoef = this.sideForcCoef;
				this.WheelController.GetComponent<WheelController>().width = this.width;
				if (this.MainProperties && this.MainProperties.Bike && !this.MainProperties.SittingInCar)
				{
					this.WheelController.GetComponent<WheelController>().Freeze();
				}
			}
			if (this.Spring)
			{
				if (this.Condition > 0.4f)
				{
					if (!this.MainProperties.Bike)
					{
						if (this.coilnut)
						{
							this.WheelController.GetComponent<WheelController>().transform.localPosition = new Vector3(this.WheelController.GetComponent<WheelController>().transform.localPosition.x, this.WheelController.GetComponent<WheelController>().springLengthBackup + this.coilnut.SpringLength, this.WheelController.GetComponent<WheelController>().transform.localPosition.z);
						}
						else
						{
							this.WheelController.GetComponent<WheelController>().transform.localPosition = new Vector3(this.WheelController.GetComponent<WheelController>().transform.localPosition.x, this.WheelController.GetComponent<WheelController>().springLengthBackup + this.PREFAB.GetComponent<CarProperties>().SpringLength, this.WheelController.GetComponent<WheelController>().transform.localPosition.z);
						}
					}
					this.WheelController.GetComponent<WheelController>().springMaximumForce = this.PREFAB.GetComponent<CarProperties>().SpringForce;
					if (this.MainProperties.Bike && !this.MainProperties.SittingInCar)
					{
						this.WheelController.GetComponent<WheelController>().Freeze();
					}
				}
				else
				{
					this.WheelController.GetComponent<WheelController>().springMaximumForce = this.PREFAB.GetComponent<CarProperties>().SpringForce / 3f;
					if (this.RuinedMesh)
					{
						base.gameObject.GetComponent<MeshFilter>().mesh = this.RuinedMesh;
					}
				}
			}
			if (this.Damper)
			{
				if (this.Condition > 0.1f)
				{
					this.WheelController.GetComponent<WheelController>().damperBumpForce = this.PREFAB.GetComponent<CarProperties>().DamperBumpForce;
					this.WheelController.GetComponent<WheelController>().damperReboundForce = this.PREFAB.GetComponent<CarProperties>().DamperReboundForce;
				}
				else
				{
					this.WheelController.GetComponent<WheelController>().damperBumpForce = 100f;
					this.WheelController.GetComponent<WheelController>().damperReboundForce = 100f;
					if (this.RuinedMesh)
					{
						base.gameObject.GetComponent<MeshFilter>().mesh = this.RuinedMesh;
					}
				}
			}
			if (this.TieRod && this.FL)
			{
				this.MainProperties.LeftTieRod = this;
			}
			if (this.TieRod && this.FR)
			{
				this.MainProperties.RightTieRod = this;
			}
			if (this.SteeringWheel)
			{
				this.MainProperties.SteeringWheel = this;
			}
			if (this.SteeringWheel)
			{
				this.exp.steering.steeringWheel = base.transform;
			}
			if (this.SteeringBox)
			{
				this.MainProperties.SteeringBox = this;
			}
			if (this.IgnitionKey)
			{
				base.GetComponent<IgnitionKey>().enabled = true;
				base.GetComponent<IgnitionKey>().Start();
				this.MainProperties.IgnitionKey = base.transform.gameObject;
				if (this.MainProperties && (this.MainProperties.Owner == "Junkyard" || this.MainProperties.Owner == "Dealer"))
				{
					base.gameObject.GetComponent<MeshRenderer>().enabled = false;
				}
				else
				{
					base.gameObject.GetComponent<MeshRenderer>().enabled = true;
				}
			}
			if (this.WiperMotor)
			{
				this.MainProperties.WiperMotor = this;
			}
			if (this.WiperL)
			{
				this.MainProperties.WiperL = base.transform;
			}
			if (this.WiperR)
			{
				this.MainProperties.WiperR = base.transform;
			}
			if (this.WiperOnly)
			{
				this.MainProperties.WiperOnly = base.transform;
			}
			if (this.WindowLiftFL)
			{
				this.MainProperties.WindowLiftFL = base.GetComponent<WindowLift>();
			}
			if (this.WindowLiftFR)
			{
				this.MainProperties.WindowLiftFR = base.GetComponent<WindowLift>();
			}
			if (this.WindowLiftRL)
			{
				this.MainProperties.WindowLiftRL = base.GetComponent<WindowLift>();
			}
			if (this.WindowLiftRR)
			{
				this.MainProperties.WindowLiftRR = base.GetComponent<WindowLift>();
			}
			if (this.Bulb)
			{
				base.transform.parent.GetComponent<CarLight>().LightBulb = this;
			}
			if (this.CarTrailer)
			{
				this.MainProperties.CarTrailer = this;
			}
			if (this.Diff)
			{
				this.MainProperties.Differential = this;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.DiffFront)
			{
				this.MainProperties.DifferentialFront = this;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.TransferCase)
			{
				this.MainProperties.TransferCase = this;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.DriveShaft)
			{
				this.MainProperties.DriveShaft = this;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.DriveShaftFront)
			{
				this.MainProperties.DriveShaftFront = this;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.DriveShaftMiddle)
			{
				this.MainProperties.DriveShaftMiddle = this;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.AxleFront)
			{
				if (base.transform.parent.gameObject.GetComponent<transparents>().R)
				{
					this.MainProperties.AxleFR = this;
				}
				if (base.transform.parent.gameObject.GetComponent<transparents>().L)
				{
					this.MainProperties.AxleFL = this;
				}
				this.MainProperties.CheckDrivetrain();
			}
			if (this.AxleRear)
			{
				if (base.transform.parent.gameObject.GetComponent<transparents>().R)
				{
					this.MainProperties.AxleRR = this;
				}
				if (base.transform.parent.gameObject.GetComponent<transparents>().L)
				{
					this.MainProperties.AxleRL = this;
				}
				this.MainProperties.CheckDrivetrain();
			}
			if (this.Chain)
			{
				this.MainProperties.Chain = this;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.ChainSprocket)
			{
				this.MainProperties.ChainSprocket = this;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.WheelSprocket)
			{
				this.MainProperties.WheelSprocket = this;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.Gearbox)
			{
				this.MainProperties.Gearbox = this;
			}
			if (this.Shifter)
			{
				this.MainProperties.Shifter = this;
			}
			if (this.Clutch)
			{
				this.MainProperties.Clutch = this;
			}
			if (this.ClutchCover)
			{
				this.MainProperties.ClutchCover = this;
			}
			if (this.ClutchPedal)
			{
				this.MainProperties.ClutchPedal = this;
			}
			if (this.Flywheel)
			{
				this.MainProperties.Flywheel = this;
			}
			if (this.IgnitionCoil)
			{
				this.MainProperties.IgnitionCoil = this;
			}
			if (this.SparkWires)
			{
				this.MainProperties.SparkWires = this;
			}
			if (this.Distributor)
			{
				this.MainProperties.Distributor = this;
			}
			if (this.Carburetor)
			{
				this.MainProperties.Carburetor = this;
			}
			if (this.Turbo)
			{
				this.MainProperties.Turbo = this;
			}
			if (this.TurboPipe)
			{
				this.MainProperties.TurboPipe = this;
			}
			if (this.Battery)
			{
				this.MainProperties.Battery = this;
			}
			if (this.BatteryWires)
			{
				this.MainProperties.BatteryWires = this;
			}
			if (this.Starter)
			{
				this.MainProperties.Starter = this;
			}
			if (this.Alternator)
			{
				this.MainProperties.Alternator = this;
			}
			if (this.AlternatorPulley)
			{
				this.MainProperties.AlternatorPulley = base.transform;
			}
			if (this.Radiator)
			{
				this.MainProperties.Radiator = this;
			}
			if (this.RadiatorFan)
			{
				this.MainProperties.RadiatorFan = this;
			}
			if (this.RadiatorGT)
			{
				this.MainProperties.RadiatorGT = this;
			}
			if (this.RadiatorFanGT)
			{
				this.MainProperties.RadiatorFanGT = this;
			}
			if (this.WaterPump)
			{
				this.MainProperties.WaterPump = this;
			}
			if (this.WaterPumpBelt)
			{
				this.MainProperties.WaterPumpBelt = this;
			}
			if (this.WaterPumpPulley)
			{
				this.MainProperties.WaterPumpPulley = this;
			}
			if (this.WaterHoseUpper)
			{
				this.MainProperties.WaterHoseUpper = this;
			}
			if (this.WaterHoseLower)
			{
				this.MainProperties.WaterHoseLower = this;
			}
			if (this.ThermostatHousing)
			{
				this.MainProperties.ThermostatHousing = this;
			}
			if (this.Coolant)
			{
				this.MainProperties.Coolant = this.Coolant;
			}
			if (this.EngineOil)
			{
				this.MainProperties.EngineOil = this.EngineOil;
			}
			if (this.Fuel)
			{
				this.MainProperties.Fuel = this.Fuel;
			}
			if (this.GasPedal)
			{
				this.MainProperties.GasPedal = this;
			}
			if (this.FuelLine)
			{
				this.MainProperties.FuelLine = this;
			}
			if (this.FuelTank)
			{
				this.MainProperties.FuelTank = this;
			}
			if (this.FuelPump)
			{
				this.MainProperties.FuelPump = this;
			}
			if (this.CamshaftSprocket)
			{
				this.MainProperties.CamshaftSprocket = this;
			}
			if (this.CrankshaftSprocket)
			{
				this.MainProperties.CrankshaftSprocket = this;
			}
			if (this.HarmonicBalancer)
			{
				this.MainProperties.HarmonicBalancer = this;
			}
			if (this.CrankshaftPulley)
			{
				this.MainProperties.CrankshaftPulley = this;
			}
			if (this.AlternatorBelt)
			{
				this.MainProperties.AlternatorBelt = this;
			}
			if (this.Blower)
			{
				this.MainProperties.Blower = this;
			}
			if (this.BlowerBelt)
			{
				this.MainProperties.BlowerBelt = this;
			}
			if (this.BlowerPulley)
			{
				this.MainProperties.BlowerPulley = this;
			}
			if (this.FuelFilter)
			{
				this.MainProperties.FuelFilter = this;
			}
			if (this.FuelHoses)
			{
				this.MainProperties.FuelHoses = this;
			}
			if (this.GlowPlugRelay)
			{
				this.MainProperties.GlowPlugRelay = this;
			}
			if (this.EngineBlock)
			{
				if (this.injected)
				{
					this.MainProperties.Injected = true;
				}
				else
				{
					this.MainProperties.Injected = false;
				}
				this.MainProperties.EngineBlock = this;
				this.MainProperties.EngineType = this.Type;
				this.MainProperties.DieselEngine = this.DieselEngine;
				if (this.DieselEngine)
				{
					this.MainProperties.FRwhellcontroller.GetComponent<WheelController>().MaxRPM = 5000f;
					this.MainProperties.FLwhellcontroller.GetComponent<WheelController>().MaxRPM = 5000f;
					this.MainProperties.RLwhellcontroller.GetComponent<WheelController>().MaxRPM = 5000f;
					this.MainProperties.RRwhellcontroller.GetComponent<WheelController>().MaxRPM = 5000f;
					this.MainProperties.exp.powertrain.engine.revLimiterRPM = 5000f;
				}
				else if (!this.MainProperties.Bike)
				{
					this.MainProperties.FRwhellcontroller.GetComponent<WheelController>().MaxRPM = 6000f;
					this.MainProperties.FLwhellcontroller.GetComponent<WheelController>().MaxRPM = 6000f;
					this.MainProperties.RLwhellcontroller.GetComponent<WheelController>().MaxRPM = 6000f;
					this.MainProperties.RRwhellcontroller.GetComponent<WheelController>().MaxRPM = 6000f;
					this.MainProperties.exp.powertrain.engine.revLimiterRPM = 6000f;
				}
			}
			if (this.AirCooled)
			{
				this.MainProperties.AirCooled = this;
			}
			if (this.DoubleHeads && this.EngineBlock)
			{
				this.MainProperties.DoubleHeads = true;
			}
			if (this.EngineHead)
			{
				this.MainProperties.EngineHead = this;
			}
			if (this.EngineHead2)
			{
				this.MainProperties.EngineHead2 = this;
			}
			if (this.HeadGasket || this.HeadGasket2)
			{
				if (base.transform.parent.gameObject.GetComponent<transparents>().R)
				{
					this.MainProperties.HeadGasket2 = this;
					this.HeadGasket = false;
					this.HeadGasket2 = true;
				}
				else
				{
					this.MainProperties.HeadGasket = this;
					this.HeadGasket2 = false;
					this.HeadGasket = true;
				}
			}
			if (this.Rockers || this.Rockers2)
			{
				if (base.transform.parent.gameObject.GetComponent<transparents>().R)
				{
					this.MainProperties.Rockers2 = this;
					this.Rockers = false;
					this.Rockers2 = true;
				}
				else
				{
					this.MainProperties.Rockers = this;
					this.Rockers2 = false;
					this.Rockers = true;
				}
			}
			if (this.HeadCover)
			{
				this.MainProperties.HeadCover = this;
			}
			if (this.HeadCover2)
			{
				this.MainProperties.HeadCover2 = this;
			}
			if (this.Crankshaft)
			{
				this.MainProperties.Crankshaft = this;
			}
			if (this.EngineChain)
			{
				this.MainProperties.EngineChain = this;
			}
			if (this.Camshaft)
			{
				this.MainProperties.Camshaft = this;
			}
			if (this.Camshaft2)
			{
				this.MainProperties.Camshaft2 = this;
			}
			if (this.AirFilter)
			{
				this.MainProperties.AirFilter = this;
			}
			if (this.AirFilterCover)
			{
				this.MainProperties.AirFilterCover = this;
			}
			if (this.OilPan)
			{
				this.MainProperties.OilPan = this;
			}
			if (this.OilFilter)
			{
				this.MainProperties.OilFilter = this;
			}
			if (this.Exhaust)
			{
				this.MainProperties.Exhaust = this;
			}
			if (this.ExhaustHeader)
			{
				this.MainProperties.ExhaustHeader = this;
			}
			if (this.ExhaustManifold)
			{
				this.MainProperties.ExhaustManifold = this;
			}
			if (this.ExhaustSmoke)
			{
				this.MainProperties.ExhaustSmoke = base.transform;
			}
			if (this.ExhaustHeaderSmoke)
			{
				this.MainProperties.ExhaustHeaderSmoke = base.transform;
			}
			if (this.ExhaustManifoldSmoke)
			{
				this.MainProperties.ExhaustManifoldSmoke = base.transform;
			}
			if (this.HeadSmoke)
			{
				this.MainProperties.HeadSmoke = base.transform;
			}
			if (this.Exhaust2)
			{
				this.MainProperties.Exhaust2 = this;
			}
			if (this.ExhaustHeader2)
			{
				this.MainProperties.ExhaustHeader2 = this;
			}
			if (this.ExhaustManifold2)
			{
				this.MainProperties.ExhaustManifold2 = this;
			}
			if (this.ExhaustSmoke2)
			{
				this.MainProperties.ExhaustSmoke2 = base.transform;
			}
			if (this.ExhaustHeaderSmoke2)
			{
				this.MainProperties.ExhaustHeaderSmoke2 = base.transform;
			}
			if (this.ExhaustManifoldSmoke2)
			{
				this.MainProperties.ExhaustManifoldSmoke2 = base.transform;
			}
			if (this.HeadSmoke2)
			{
				this.MainProperties.HeadSmoke2 = base.transform;
			}
			if (this.Cluster)
			{
				this.MainProperties.Cluster = this;
			}
			if (this.AnalogRpmGauge)
			{
				this.MainProperties.AnalogRpmGauge = base.transform.gameObject;
			}
			if (this.DigitalRpmGauge)
			{
				this.MainProperties.DigitalRpmGauge = base.transform.gameObject;
			}
			if (this.AnalogSpeedGauge)
			{
				this.MainProperties.AnalogSpeedGauge = base.transform.gameObject;
			}
			if (this.DigitalSpeedGauge)
			{
				this.MainProperties.DigitalSpeedGauge = base.GetComponent<Text>();
			}
			if (this.TempGauge)
			{
				this.MainProperties.TempGauge = base.transform.gameObject;
			}
			if (this.BatteryGauge)
			{
				this.MainProperties.BatteryGauge = base.transform.gameObject;
			}
			if (this.FuelGauge)
			{
				this.MainProperties.FuelGauge = base.transform.gameObject;
			}
			if (this.Clock)
			{
				this.MainProperties.Clock = base.transform.gameObject;
			}
			if (this.Ruined && this.Condition >= 0.1f)
			{
				this.Condition = 0.9f;
			}
		}
		this.CheckVisualCondition();
		if (this.exp && this.MainProperties.exp)
		{
			this.MainProperties.CheckStates();
		}
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x00040898 File Offset: 0x0003EA98
	public void CheckVisualCondition()
	{
		if (this.WornMesh && (this.Condition < 0.4f || this.Ruined))
		{
			base.gameObject.GetComponent<MeshFilter>().mesh = this.WornMesh;
			if (this.VisualObject)
			{
				this.VisualObject.GetComponent<MeshFilter>().mesh = this.WornMesh;
			}
		}
		if (this.RuinedMesh && (this.Condition < 0.1f || this.Ruined))
		{
			base.gameObject.GetComponent<MeshFilter>().mesh = this.RuinedMesh;
			if (this.VisualObject)
			{
				this.VisualObject.GetComponent<MeshFilter>().mesh = this.RuinedMesh;
			}
		}
		if (this.damagedMesh && this.Damaged)
		{
			base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
			if (this.VisualObject)
			{
				this.VisualObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
			}
		}
		if (this.WornMaterial && (this.Condition < 0.4f || this.Ruined))
		{
			base.GetComponent<Renderer>().sharedMaterial = this.WornMaterial;
			if (this.VisualObject)
			{
				this.VisualObject.GetComponent<Renderer>().sharedMaterial = this.WornMaterial;
			}
		}
		if (this.RuinedMaterial && (this.Condition < 0.1f || this.Ruined))
		{
			base.GetComponent<Renderer>().sharedMaterial = this.RuinedMaterial;
			if (this.VisualObject)
			{
				this.VisualObject.GetComponent<Renderer>().sharedMaterial = this.RuinedMaterial;
			}
		}
		if (this.RuinedMaterial && (this.Condition < 0.1f || this.Ruined))
		{
			base.GetComponent<Renderer>().sharedMaterial = this.RuinedMaterial;
			if (this.VisualObject)
			{
				this.VisualObject.GetComponent<Renderer>().sharedMaterial = this.RuinedMaterial;
			}
		}
		if (this.OldMaterial && this.PartIsOld)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.OldMaterial;
			if (this.VisualObject)
			{
				this.VisualObject.GetComponent<Renderer>().sharedMaterial = this.OldMaterial;
			}
		}
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x00040AF4 File Offset: 0x0003ECF4
	public void Remove()
	{
		this.CheckVisualCondition();
		if (base.transform.parent)
		{
			this.Attached = true;
		}
		else
		{
			this.Attached = false;
		}
		this.picked = true;
		if (this.IgnitionKey && this.MainProperties)
		{
			base.GetComponent<IgnitionKey>().enabled = false;
			this.MainProperties.IgnitionKey = null;
			this.MainProperties.IgnitionON = false;
			this.MainProperties.IgnitionTurnedOff();
			this.MainProperties.EngineStop();
		}
		if (this.HandBrake && base.GetComponent<HandbrakeScr>())
		{
			base.GetComponent<HandbrakeScr>().remove();
		}
		if (this.VisualWheel || this.NonRotatingVisualWheel)
		{
			base.transform.localPosition = new Vector3(this.StartX, this.StartY, this.StartZ);
		}
		if (this.RealWheel && this.WheelController)
		{
			if (this.tireObject && this.WheelController)
			{
				if (base.transform.parent && base.transform.parent.GetComponent<transparents>().OuterTire)
				{
					this.WheelController.GetComponent<WheelController>().tireObject2 = null;
				}
				else
				{
					this.WheelController.GetComponent<WheelController>().tireObject = null;
				}
			}
			this.WheelController.GetComponent<WheelController>().SetRadius();
			if (this.MainProperties.Bike && !this.MainProperties.SittingInCar)
			{
				this.WheelController.GetComponent<WheelController>().Freeze();
			}
		}
		if (base.transform.name == "TrailerHandle" && base.GetComponent<PickupHand>() && base.GetComponent<PickupHand>().TrailerJoint)
		{
			UnityEngine.Object.Destroy(base.GetComponent<PickupHand>().TrailerJoint);
		}
		if (this.EngineOil && (!base.transform.parent || (base.transform.parent && !base.transform.parent.parent)))
		{
			this.EngineOil.FluidSize = 0f;
		}
		if (this.exp)
		{
			if (this.PREFAB)
			{
				this.MainProperties.Weight -= base.gameObject.GetComponent<Partinfo>().weight;
			}
			if (this.BrakePad && base.transform.parent.gameObject.GetComponent<transparents>().BrakePadA)
			{
				this.WheelController.GetComponent<WheelController>().BrakePadACondition = 0f;
			}
			if (this.BrakePad && base.transform.parent.gameObject.GetComponent<transparents>().BrakePadB)
			{
				this.WheelController.GetComponent<WheelController>().BrakePadBCondition = 0f;
			}
			if (this.BrakeDisc)
			{
				this.WheelController.GetComponent<WheelController>().BrakeDiscCondition = 0f;
			}
			if (this.BrakeLine)
			{
				if (this.FL)
				{
					this.MainProperties.BrakeLIneFL = null;
				}
				if (this.FR)
				{
					this.MainProperties.BrakeLIneFR = null;
				}
				if (this.RL)
				{
					this.MainProperties.BrakeLIneRL = null;
				}
				if (this.RR)
				{
					this.MainProperties.BrakeLIneRR = null;
				}
			}
			if (this.MainBrakeLine)
			{
				this.MainProperties.MainBrakeLIne = null;
			}
			if (this.BrakeFluid)
			{
				this.MainProperties.BrakeFluid = null;
			}
			if (this.BrakePedal)
			{
				this.MainProperties.BrakePedal = null;
			}
			if (this.WiperMotor)
			{
				this.MainProperties.WiperMotor = null;
			}
			if (this.WiperL)
			{
				this.MainProperties.WiperL = null;
			}
			if (this.WiperR)
			{
				this.MainProperties.WiperR = null;
			}
			if (this.WiperOnly)
			{
				this.MainProperties.WiperOnly = null;
			}
			if (this.WindowLiftFL)
			{
				this.MainProperties.WindowLiftFL = null;
			}
			if (this.WindowLiftFR)
			{
				this.MainProperties.WindowLiftFR = null;
			}
			if (this.WindowLiftRL)
			{
				this.MainProperties.WindowLiftRL = null;
			}
			if (this.WindowLiftRR)
			{
				this.MainProperties.WindowLiftRR = null;
			}
			if (this.Bulb)
			{
				base.transform.parent.GetComponent<CarLight>().LightBulb = null;
				base.transform.parent.GetComponent<CarLight>().Remove();
			}
			if (this.VisualWheel)
			{
				this.WheelController.GetComponent<WheelController>().SetDefaultVisual();
			}
			if (this.NonRotatingVisualWheel)
			{
				this.WheelController.GetComponent<WheelController>().NonRotatingVisual = null;
			}
			if (this.RealWheel)
			{
				this.WheelController.GetComponent<WheelController>().SetRadius();
				if (this.MainProperties.Bike && !this.MainProperties.SittingInCar)
				{
					this.WheelController.GetComponent<WheelController>().Freeze();
				}
			}
			if (this.Spring)
			{
				this.WheelController.GetComponent<WheelController>().springMaximumForce = 100f;
				if (this.MainProperties.Bike && !this.MainProperties.SittingInCar)
				{
					this.WheelController.GetComponent<WheelController>().Freeze();
				}
			}
			if (this.Damper)
			{
				this.WheelController.GetComponent<WheelController>().damperBumpForce = 100f;
				this.WheelController.GetComponent<WheelController>().damperReboundForce = 100f;
			}
			if (this.TieRod && this.FL)
			{
				this.MainProperties.LeftTieRod = null;
			}
			if (this.TieRod && this.FR)
			{
				this.MainProperties.RightTieRod = null;
			}
			if (this.SteeringWheel)
			{
				this.MainProperties.SteeringWheel = null;
			}
			if (this.SteeringWheel)
			{
				this.exp.steering.steeringWheel = null;
			}
			if (this.SteeringBox)
			{
				this.MainProperties.SteeringBox = null;
			}
			if (this.HandBrake)
			{
				this.MainProperties.ReleaseHandbrake();
				this.MainProperties.HandBrakeInstalled = false;
				this.MainProperties.Handbrake = null;
			}
			if (this.HandBrakeCable)
			{
				this.MainProperties.ReleaseHandbrake();
				this.MainProperties.HandBrakeCableInstalled = false;
				this.MainProperties.HandbrakeCable = null;
			}
			if (this.CarTrailer)
			{
				this.MainProperties.CarTrailer = null;
			}
			if (this.Diff)
			{
				this.MainProperties.Differential = null;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.DiffFront)
			{
				this.MainProperties.DifferentialFront = null;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.TransferCase)
			{
				this.MainProperties.TransferCase = null;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.DriveShaft)
			{
				this.MainProperties.DriveShaft = null;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.DriveShaftFront)
			{
				this.MainProperties.DriveShaftFront = null;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.DriveShaftMiddle)
			{
				this.MainProperties.DriveShaftMiddle = null;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.AxleFront)
			{
				this.MainProperties.AxleFR = this;
				if (this)
				{
					this.MainProperties.AxleFR = null;
				}
				this.MainProperties.AxleFL = this;
				if (this)
				{
					this.MainProperties.AxleFL = null;
				}
				this.MainProperties.CheckDrivetrain();
			}
			if (this.Gearbox)
			{
				this.MainProperties.Gearbox = null;
			}
			if (this.Shifter)
			{
				this.MainProperties.Shifter = null;
			}
			if (this.Clutch)
			{
				this.MainProperties.Clutch = null;
			}
			if (this.ClutchCover)
			{
				this.MainProperties.ClutchCover = null;
			}
			if (this.ClutchPedal)
			{
				this.MainProperties.ClutchPedal = null;
			}
			if (this.Flywheel)
			{
				this.MainProperties.Flywheel = null;
			}
			if (this.Chain)
			{
				this.MainProperties.Chain = null;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.ChainSprocket)
			{
				this.MainProperties.ChainSprocket = null;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.WheelSprocket)
			{
				this.MainProperties.WheelSprocket = null;
				this.MainProperties.CheckDrivetrain();
			}
			if (this.IgnitionCoil)
			{
				this.MainProperties.IgnitionCoil = null;
				this.MainProperties.EngineStop();
			}
			if (this.SparkWires)
			{
				this.MainProperties.SparkWires = null;
				this.MainProperties.EngineStop();
			}
			if (this.Distributor)
			{
				this.MainProperties.Distributor = null;
				this.MainProperties.EngineStop();
			}
			if (this.Carburetor)
			{
				this.MainProperties.Carburetor = null;
				this.MainProperties.EngineStop();
			}
			if (this.Turbo)
			{
				this.MainProperties.Turbo = null;
			}
			if (this.TurboPipe)
			{
				this.MainProperties.TurboPipe = null;
			}
			if (this.Battery)
			{
				this.MainProperties.Battery = null;
			}
			if (this.BatteryWires)
			{
				this.MainProperties.BatteryWires = null;
				this.MainProperties.EngineStop();
			}
			if (this.Starter)
			{
				this.MainProperties.Starter = null;
			}
			if (this.Alternator)
			{
				this.MainProperties.Alternator = null;
				this.MainProperties.AlternatorPulley = null;
			}
			if (this.Radiator)
			{
				this.MainProperties.Radiator = null;
			}
			if (this.RadiatorFan)
			{
				this.MainProperties.RadiatorFan = null;
			}
			if (this.RadiatorGT)
			{
				this.MainProperties.RadiatorGT = null;
			}
			if (this.RadiatorFanGT)
			{
				this.MainProperties.RadiatorFanGT = null;
			}
			if (this.WaterPump)
			{
				this.MainProperties.WaterPump = null;
			}
			if (this.WaterPumpBelt)
			{
				this.MainProperties.WaterPumpBelt = null;
			}
			if (this.WaterPumpPulley)
			{
				this.MainProperties.WaterPumpPulley = null;
			}
			if (this.WaterHoseUpper)
			{
				this.MainProperties.WaterHoseUpper = null;
			}
			if (this.WaterHoseLower)
			{
				this.MainProperties.WaterHoseLower = null;
			}
			if (this.ThermostatHousing)
			{
				this.MainProperties.ThermostatHousing = null;
			}
			if (this.Coolant)
			{
				this.MainProperties.Coolant = null;
			}
			if (this.EngineOil)
			{
				this.MainProperties.EngineOil.Condition = 1f;
				this.MainProperties.EngineOil = null;
				if (!base.transform.parent)
				{
					this.EngineOil.FluidSize = 0f;
				}
			}
			if (this.Fuel)
			{
				this.MainProperties.Fuel = null;
				this.MainProperties.EngineStop();
			}
			if (this.GasPedal)
			{
				this.MainProperties.GasPedal = null;
			}
			if (this.FuelLine)
			{
				this.MainProperties.FuelLine = null;
				this.MainProperties.EngineStop();
			}
			if (this.FuelTank)
			{
				this.MainProperties.FuelTank = null;
				this.MainProperties.EngineStop();
			}
			if (this.FuelPump)
			{
				this.MainProperties.FuelPump = null;
				this.MainProperties.EngineStop();
			}
			if (this.CamshaftSprocket)
			{
				this.MainProperties.CamshaftSprocket = this;
			}
			if (this.CrankshaftSprocket)
			{
				this.MainProperties.CrankshaftSprocket = null;
			}
			if (this.HarmonicBalancer)
			{
				this.MainProperties.HarmonicBalancer = null;
			}
			if (this.CrankshaftPulley)
			{
				this.MainProperties.CrankshaftPulley = null;
			}
			if (this.AlternatorBelt)
			{
				this.MainProperties.AlternatorBelt = null;
			}
			if (this.Blower)
			{
				this.MainProperties.Blower = null;
			}
			if (this.BlowerBelt)
			{
				this.MainProperties.BlowerBelt = null;
			}
			if (this.BlowerPulley)
			{
				this.MainProperties.BlowerPulley = null;
			}
			if (this.FuelFilter)
			{
				this.MainProperties.FuelFilter = null;
			}
			if (this.FuelHoses)
			{
				this.MainProperties.FuelHoses = null;
			}
			if (this.GlowPlugRelay)
			{
				this.MainProperties.GlowPlugRelay = null;
			}
			if (this.EngineBlock)
			{
				this.MainProperties.EngineBlock = null;
				this.MainProperties.EngineStop();
				this.MainProperties.DoubleHeads = false;
			}
			if (this.AirCooled)
			{
				this.MainProperties.AirCooled = false;
			}
			if (this.DoubleHeads)
			{
				this.MainProperties.DoubleHeads = false;
			}
			if (this.EngineHead)
			{
				this.MainProperties.EngineHead = null;
				this.MainProperties.EngineStop();
			}
			if (this.EngineHead2)
			{
				this.MainProperties.EngineHead2 = null;
				this.MainProperties.EngineStop();
			}
			if (this.HeadGasket)
			{
				this.MainProperties.HeadGasket = null;
			}
			if (this.HeadGasket2)
			{
				this.MainProperties.HeadGasket2 = null;
			}
			if (this.Rockers)
			{
				this.MainProperties.Rockers = null;
			}
			if (this.Rockers2)
			{
				this.MainProperties.Rockers2 = null;
			}
			if (this.HeadCover)
			{
				this.MainProperties.HeadCover = null;
			}
			if (this.HeadCover2)
			{
				this.MainProperties.HeadCover2 = null;
			}
			if (this.Crankshaft)
			{
				this.MainProperties.Crankshaft = null;
				this.MainProperties.EngineStop();
			}
			if (this.EngineChain)
			{
				this.MainProperties.EngineChain = null;
				this.MainProperties.EngineStop();
			}
			if (this.Camshaft)
			{
				this.MainProperties.Camshaft = null;
			}
			if (this.Camshaft2)
			{
				this.MainProperties.Camshaft2 = null;
			}
			if (this.AirFilter)
			{
				this.MainProperties.AirFilter = null;
			}
			if (this.AirFilterCover)
			{
				this.MainProperties.AirFilterCover = null;
			}
			if (this.OilPan)
			{
				this.MainProperties.OilPan = null;
			}
			if (this.OilFilter)
			{
				this.MainProperties.OilFilter = null;
			}
			if (this.Exhaust)
			{
				this.MainProperties.Exhaust = null;
			}
			if (this.ExhaustHeader)
			{
				this.MainProperties.ExhaustHeader = null;
			}
			if (this.ExhaustManifold)
			{
				this.MainProperties.ExhaustManifold = null;
			}
			if (this.ExhaustSmoke)
			{
				this.MainProperties.ExhaustSmoke = null;
			}
			if (this.ExhaustHeaderSmoke)
			{
				this.MainProperties.ExhaustHeaderSmoke = null;
			}
			if (this.ExhaustManifoldSmoke)
			{
				this.MainProperties.ExhaustManifoldSmoke = null;
			}
			if (this.HeadSmoke)
			{
				this.MainProperties.HeadSmoke = null;
			}
			if (this.Exhaust2)
			{
				this.MainProperties.Exhaust2 = null;
			}
			if (this.ExhaustHeader2)
			{
				this.MainProperties.ExhaustHeader2 = null;
			}
			if (this.ExhaustManifold2)
			{
				this.MainProperties.ExhaustManifold2 = null;
			}
			if (this.ExhaustSmoke2)
			{
				this.MainProperties.ExhaustSmoke2 = null;
			}
			if (this.ExhaustHeaderSmoke2)
			{
				this.MainProperties.ExhaustHeaderSmoke2 = null;
			}
			if (this.ExhaustManifoldSmoke2)
			{
				this.MainProperties.ExhaustManifoldSmoke2 = null;
			}
			if (this.HeadSmoke2)
			{
				this.MainProperties.HeadSmoke2 = null;
			}
			if (this.Cluster)
			{
				this.MainProperties.Cluster = null;
			}
			if (this.AnalogRpmGauge)
			{
				this.MainProperties.AnalogRpmGauge = null;
				base.GetComponent<AnalogGauge>().Value = 0f;
			}
			if (this.DigitalRpmGauge)
			{
				this.MainProperties.DigitalRpmGauge = null;
			}
			if (this.AnalogSpeedGauge)
			{
				this.MainProperties.AnalogSpeedGauge = null;
				base.GetComponent<AnalogGauge>().Value = 0f;
			}
			if (this.DigitalSpeedGauge)
			{
				this.MainProperties.DigitalSpeedGauge = null;
			}
			if (this.TempGauge)
			{
				this.MainProperties.TempGauge = null;
				base.GetComponent<AnalogGauge>().Value = 0f;
			}
			if (this.BatteryGauge)
			{
				this.MainProperties.BatteryGauge = null;
				base.GetComponent<AnalogGauge>().Value = 0f;
			}
			if (this.FuelGauge)
			{
				this.MainProperties.FuelGauge = null;
				base.GetComponent<AnalogGauge>().Value = 0f;
			}
			if (this.Clock)
			{
				this.MainProperties.Clock = null;
			}
			if (this.SparkPlug)
			{
				this.MainProperties.EngineStop();
			}
			this.MainProperties.CheckStates();
		}
		if (this.Tire && base.transform.parent && base.transform.parent.parent)
		{
			base.transform.parent.parent.GetComponent<CarProperties>().tireObject = null;
		}
		this.exp = null;
		this.WheelController = null;
		this.MainProperties = null;
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00041B24 File Offset: 0x0003FD24
	public void Attach()
	{
		if (base.transform.parent)
		{
			this.Attached = true;
			if (base.transform.parent.GetComponent<transparents>())
			{
				this.SavePosition = base.transform.parent.GetComponent<transparents>().SavePosition;
			}
			if (this.RemovedMesh)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<MeshFilter>().mesh;
				base.gameObject.GetComponent<MeshCollider>().sharedMesh = (base.gameObject.GetComponent<MeshFilter>().mesh = base.transform.parent.GetComponent<MeshFilter>().mesh);
			}
		}
		else
		{
			this.Attached = false;
		}
		if (base.transform.root.tag == "Vehicle")
		{
			this.ReStart();
			if (this.PREFAB && this.MainProperties)
			{
				this.MainProperties.Weight += base.gameObject.GetComponent<Partinfo>().weight;
			}
			if (this.VisualWheel)
			{
				this.WheelController.GetComponent<WheelController>().Visual = base.gameObject;
				this.WheelController.GetComponent<WheelController>().cachedVisualTransform = base.transform;
			}
			if (this.NonRotatingVisualWheel)
			{
				this.WheelController.GetComponent<WheelController>().NonRotatingVisual = base.gameObject;
			}
			if (this.HandBrake)
			{
				if (base.GetComponent<HandbrakeScr>())
				{
					base.GetComponent<HandbrakeScr>().attach();
				}
				this.MainProperties.HandBrakeInstalled = true;
				this.MainProperties.Handbrake = base.gameObject;
			}
			if (this.HandBrakeCable)
			{
				this.MainProperties.HandBrakeCableInstalled = true;
				this.MainProperties.HandbrakeCable = base.gameObject;
			}
			if (this.TieRod)
			{
				this.exp.steering.AttachedROd();
			}
			if (this.WheelController != null && this.WheelController.transform.parent.GetComponent<CarProperties>())
			{
				if (this.AffectsWCOLrotationZ)
				{
					this.WheelController.transform.parent.localRotation = Quaternion.Euler(0f, this.WheelController.transform.parent.localEulerAngles.y, 0f);
				}
				if (this.AffectsWCOLrotationY)
				{
					this.WheelController.transform.parent.localRotation = Quaternion.Euler(0f, 0f, this.WheelController.transform.parent.localEulerAngles.z);
				}
				if (this.AffectsWCOLpositionZ)
				{
					this.WheelController.transform.parent.localPosition = new Vector3(this.WheelController.transform.parent.localPosition.x, this.WheelController.transform.parent.GetComponent<CarProperties>().StartY, this.WheelController.transform.parent.GetComponent<CarProperties>().StartZ);
				}
				if (this.AffectsWCOLpositionX)
				{
					this.WheelController.transform.parent.localPosition = new Vector3(this.WheelController.transform.parent.GetComponent<CarProperties>().StartX, this.WheelController.transform.parent.GetComponent<CarProperties>().StartY, this.WheelController.transform.parent.localPosition.z);
				}
			}
			if (this.AffectsFRSuspensionPosition && this.FRSuspensionPosition != null)
			{
				this.FRSuspensionPosition.transform.localPosition = new Vector3(this.FRSuspensionPosition.GetComponent<CarProperties>().StartX, this.FRSuspensionPosition.GetComponent<CarProperties>().StartY, this.FRSuspensionPosition.GetComponent<CarProperties>().StartZ);
			}
			if (this.AffectsFLSuspensionPosition && this.FLSuspensionPosition != null)
			{
				this.FLSuspensionPosition.transform.localPosition = new Vector3(this.FLSuspensionPosition.GetComponent<CarProperties>().StartX, this.FLSuspensionPosition.GetComponent<CarProperties>().StartY, this.FLSuspensionPosition.GetComponent<CarProperties>().StartZ);
			}
			if (this.AffectsRRSuspensionPositionX && this.RRSuspensionPosition != null)
			{
				this.RRSuspensionPosition.transform.localPosition = new Vector3(this.RRSuspensionPosition.GetComponent<CarProperties>().StartX, this.RRSuspensionPosition.GetComponent<CarProperties>().StartY, this.RRSuspensionPosition.transform.localPosition.z);
			}
			if (this.AffectsRRSuspensionPositionZ && this.FRSuspensionPosition != null)
			{
				this.RRSuspensionPosition.transform.localPosition = new Vector3(this.RRSuspensionPosition.transform.localPosition.x, this.RRSuspensionPosition.GetComponent<CarProperties>().StartY, this.RRSuspensionPosition.GetComponent<CarProperties>().StartZ);
			}
			if (this.AffectsRLSuspensionPositionX && this.RRSuspensionPosition != null)
			{
				this.RLSuspensionPosition.transform.localPosition = new Vector3(this.RLSuspensionPosition.GetComponent<CarProperties>().StartX, this.RLSuspensionPosition.GetComponent<CarProperties>().StartY, this.RLSuspensionPosition.transform.localPosition.z);
			}
			if (this.AffectsRLSuspensionPositionZ && this.RLSuspensionPosition != null)
			{
				this.RLSuspensionPosition.transform.localPosition = new Vector3(this.RLSuspensionPosition.transform.localPosition.x, this.RLSuspensionPosition.GetComponent<CarProperties>().StartY, this.RLSuspensionPosition.GetComponent<CarProperties>().StartZ);
			}
			base.StartCoroutine(this.LateAttach());
			if (this.MainProperties)
			{
				this.MainProperties.CheckStates();
			}
		}
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x00042116 File Offset: 0x00040316
	private IEnumerator LateAttach()
	{
		yield return 1;
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.gameObject != base.gameObject)
			{
				carProperties.Attach();
			}
		}
		yield break;
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00042128 File Offset: 0x00040328
	private void oooOnMouseDown()
	{
		if (this.DMGdeformMesh)
		{
			this.mesh = base.GetComponent<MeshFilter>().mesh;
			this.vertices = this.mesh.vertices;
			for (int i = 0; i < this.vertices.Length; i++)
			{
				Vector3 vector = base.transform.TransformPoint(this.vertices[i]);
				int num = 1;
				vector = base.transform.TransformPoint(this.vertices[num]);
				bool preview = !Input.GetKey(KeyCode.Mouse0);
				int priority = 0;
				float pressure = 1f;
				int seed = 0;
				Quaternion rotation = Quaternion.LookRotation(-vector);
				if (this.RepairDecal)
				{
					this.RepairDecal.HandleHitPoint(preview, priority, pressure, seed, vector, rotation);
				}
			}
		}
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x000421F7 File Offset: 0x000403F7
	public void Repair(Vector3 point)
	{
		if (this.DMGdeformMesh && this.MeshDamaged)
		{
			if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.Repair1(point);
				return;
			}
			this.Repair1(point);
		}
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00042230 File Offset: 0x00040430
	public void Repair1(Vector3 point)
	{
		this.RepairSpot = false;
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<CarProperties>())
			{
				transform.transform.localPosition = new Vector3(transform.GetComponent<CarProperties>().StartX, transform.GetComponent<CarProperties>().StartY, transform.GetComponent<CarProperties>().StartZ);
				if (transform.GetComponent<CarProperties>().MeshDamaged)
				{
					transform.GetComponent<MeshFilter>().mesh = transform.GetComponent<CarProperties>().NormalMesh;
					transform.GetComponent<CarProperties>().MeshDamaged = false;
					transform.GetComponent<CarProperties>().Ruined = false;
					transform.GetComponent<CarProperties>().MeshLittleDamaged = false;
					transform.GetComponent<CarProperties>().Condition = 1f;
				}
			}
		}
		this.MeshDamaged = false;
		this.MeshLittleDamaged = false;
		this.vertices = this.mesh.vertices;
		Vector3 b = Vector3.zero;
		b = base.transform.InverseTransformPoint(point);
		for (int i = 0; i < this.vertices.Length; i++)
		{
			if ((this.vertices[i] - b).sqrMagnitude <= 0.25f && (double)Vector3.Distance(this.vertices[i], this.initialvertices[i]) >= 0.01)
			{
				this.vertices[i] = Vector3.Lerp(this.vertices[i], this.initialvertices[i], UnityEngine.Random.Range(0.1f, 0.5f));
				this.RepairSpot = true;
			}
			if (Vector3.Distance(this.vertices[i], this.initialvertices[i]) >= 0.02f)
			{
				this.MeshDamaged = true;
			}
			if (Vector3.Distance(this.vertices[i], this.initialvertices[i]) > 0f)
			{
				this.MeshLittleDamaged = true;
			}
		}
		if (!this.MeshLittleDamaged && !this.MeshDamaged)
		{
			this.vertices = this.initialvertices;
			this.Condition = 1f;
		}
		this.mesh.vertices = this.vertices;
		this.mesh.RecalculateNormals();
		this.mesh.RecalculateBounds();
		if (base.gameObject.transform.root.GetComponent<VehicleDamage>())
		{
			base.gameObject.transform.root.GetComponent<VehicleDamage>().Start();
		}
		if (this.RepairSpot && Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 20f)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Hammer);
		}
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00042534 File Offset: 0x00040734
	public void Repair2(Vector3 point)
	{
		if (this.DMGdeformMesh && this.MeshDamaged)
		{
			if (base.GetComponent<MPobject>())
			{
				base.GetComponent<MPobject>().networkDummy.Repair22(point);
				return;
			}
			this.Repair22(point);
		}
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0004256C File Offset: 0x0004076C
	public void Repair22(Vector3 point)
	{
		this.RepairSpot = false;
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<CarProperties>())
			{
				transform.transform.localPosition = new Vector3(transform.GetComponent<CarProperties>().StartX, transform.GetComponent<CarProperties>().StartY, transform.GetComponent<CarProperties>().StartZ);
				if (transform.GetComponent<CarProperties>().MeshDamaged)
				{
					transform.GetComponent<MeshFilter>().mesh = transform.GetComponent<CarProperties>().NormalMesh;
					transform.GetComponent<CarProperties>().MeshDamaged = false;
					transform.GetComponent<CarProperties>().Ruined = false;
					transform.GetComponent<CarProperties>().MeshLittleDamaged = false;
					transform.GetComponent<CarProperties>().Condition = 1f;
				}
			}
		}
		this.MeshLittleDamaged = false;
		this.MeshDamaged = false;
		this.vertices = this.mesh.vertices;
		Vector3 b = Vector3.zero;
		b = base.transform.InverseTransformPoint(point);
		for (int i = 0; i < this.vertices.Length; i++)
		{
			if ((this.vertices[i] - b).sqrMagnitude <= 0.25f)
			{
				float num = Vector3.Distance(this.vertices[i], this.initialvertices[i]);
				if ((double)num >= 0.02)
				{
					this.RepairSpot = true;
					this.vertices[i] = Vector3.Lerp(this.vertices[i], this.initialvertices[i], 0.2f);
				}
				else if (num > 0f)
				{
					this.RepairSpot = true;
					this.vertices[i] = Vector3.Lerp(this.vertices[i], this.initialvertices[i], 1f);
				}
			}
			if ((double)Vector3.Distance(this.vertices[i], this.initialvertices[i]) >= 0.001)
			{
				this.MeshDamaged = true;
			}
		}
		if (!this.MeshLittleDamaged && !this.MeshDamaged)
		{
			this.vertices = this.initialvertices;
			this.Condition = 1f;
		}
		this.mesh.vertices = this.vertices;
		this.mesh.RecalculateNormals();
		this.mesh.RecalculateBounds();
		if (base.gameObject.transform.root.GetComponent<VehicleDamage>())
		{
			base.gameObject.transform.root.GetComponent<VehicleDamage>().Start();
		}
		if (this.RepairSpot && Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 20f)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Hammer);
		}
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x00042888 File Offset: 0x00040A88
	public void Fair(Vector3 point)
	{
		if (this.DMGdeformMesh)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit = default(RaycastHit);
			if (Physics.Raycast(ray, out raycastHit))
			{
				bool preview = !Input.GetKey(KeyCode.Mouse0);
				int priority = 0;
				float pressure = 1f;
				int seed = 0;
				Quaternion rotation = Quaternion.LookRotation(-raycastHit.normal);
				if (base.GetComponent<MPobject>())
				{
					base.GetComponent<MPobject>().networkDummy.Fair2(point, preview, priority, pressure, seed, rotation, raycastHit.point);
					tools.ToolHand.transform.GetChild(0).GetComponent<MPobject>().networkDummy.PickupToolUPDATE(-1, true);
					return;
				}
				this.Fair2(point, preview, priority, pressure, seed, rotation, raycastHit.point);
			}
		}
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x00042954 File Offset: 0x00040B54
	public void Fair2(Vector3 point, bool preview, int priority, float pressure, int seed, Quaternion rotation, Vector3 hit)
	{
		this.RepairSpot = false;
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<CarProperties>())
			{
				transform.transform.localPosition = new Vector3(transform.GetComponent<CarProperties>().StartX, transform.GetComponent<CarProperties>().StartY, transform.GetComponent<CarProperties>().StartZ);
				if (transform.GetComponent<CarProperties>().MeshDamaged)
				{
					transform.GetComponent<MeshFilter>().mesh = transform.GetComponent<CarProperties>().NormalMesh;
					transform.GetComponent<CarProperties>().MeshDamaged = false;
					transform.GetComponent<CarProperties>().Ruined = false;
					transform.GetComponent<CarProperties>().MeshLittleDamaged = false;
					transform.GetComponent<CarProperties>().Condition = 1f;
				}
			}
		}
		this.MeshLittleDamaged = false;
		this.MeshDamaged = false;
		this.vertices = this.mesh.vertices;
		Vector3 b = Vector3.zero;
		b = base.transform.InverseTransformPoint(point);
		for (int i = 0; i < this.vertices.Length; i++)
		{
			if ((this.vertices[i] - b).sqrMagnitude <= 0.07f)
			{
				float num = Vector3.Distance(this.vertices[i], this.initialvertices[i]);
				if ((double)num >= 0.005)
				{
					this.RepairSpot = true;
					this.vertices[i] = Vector3.Lerp(this.vertices[i], this.initialvertices[i], 0.15f);
				}
				else if (num > 0f)
				{
					this.RepairSpot = true;
					this.vertices[i] = Vector3.Lerp(this.vertices[i], this.initialvertices[i], 1f);
				}
			}
			if ((double)Vector3.Distance(this.vertices[i], this.initialvertices[i]) >= 0.001)
			{
				this.MeshLittleDamaged = true;
				this.MeshDamaged = true;
			}
		}
		if (!this.MeshLittleDamaged && !this.MeshDamaged)
		{
			this.vertices = this.initialvertices;
			this.Condition = 1f;
		}
		this.mesh.vertices = this.vertices;
		this.mesh.RecalculateNormals();
		this.mesh.RecalculateBounds();
		if (base.gameObject.transform.root.GetComponent<VehicleDamage>())
		{
			base.gameObject.transform.root.GetComponent<VehicleDamage>().Start();
		}
		if (this.RepairSpot)
		{
			this.RepairDecal.HandleHitPoint(preview, priority, pressure, seed, hit, rotation);
			if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 10f)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Fair);
			}
			if (!base.GetComponent<MPobject>())
			{
				tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife -= 1f;
				tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().VisualUpdate();
			}
		}
		this.RepairSpot = false;
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x00042CE4 File Offset: 0x00040EE4
	public void Tint()
	{
		if (tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife > 0f && this.Tintable && this.Condition >= 0.1f && !this.Ruined)
		{
			if (tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().TintLevel == 0 && this.TintLevel > 0)
			{
				if (base.GetComponent<MPobject>())
				{
					base.GetComponent<MPobject>().networkDummy.Tint2(tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().TintLevel);
					return;
				}
				this.Tint2(tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().TintLevel);
				return;
			}
			else if (tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().TintLevel > 0 && this.TintLevel == 0)
			{
				if (base.GetComponent<MPobject>())
				{
					base.GetComponent<MPobject>().networkDummy.Tint3(tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().TintLevel);
					tools.ToolHand.transform.GetChild(0).GetComponent<MPobject>().networkDummy.PickupToolUPDATE(-1, true);
					return;
				}
				this.Tint3(tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().TintLevel);
				tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().paintlife -= 1f;
				tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().VisualUpdate();
			}
		}
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x00042EA0 File Offset: 0x000410A0
	public void Tint2(int level)
	{
		if (!this.MaterialParent)
		{
			this.MaterialParent = GameObject.Find("MaterialParent");
		}
		this.TintLevel = level;
		if (level == 0)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint0;
		}
		if (level == 1)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint1;
		}
		if (level == 2)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint2;
		}
		if (level == 3)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint3;
		}
		if (level == 4)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint4;
		}
		if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 10f)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Fair);
		}
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00042FB4 File Offset: 0x000411B4
	public void Tint3(int level)
	{
		if (!this.MaterialParent)
		{
			this.MaterialParent = GameObject.Find("MaterialParent");
		}
		this.TintLevel = level;
		if (level == 0)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint0;
		}
		if (level == 1)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint1;
		}
		if (level == 2)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint2;
		}
		if (level == 3)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint3;
		}
		if (level == 4)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.MaterialParent.GetComponent<CarMaterials>().Tint4;
		}
		if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 10f)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Fair);
		}
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x000430C8 File Offset: 0x000412C8
	public void FixMesh()
	{
		this.vertices = this.mesh.vertices;
		this.vertices = this.initialvertices;
		this.Condition = 1f;
		this.MeshLittleDamaged = false;
		this.MeshDamaged = false;
		this.mesh.RecalculateNormals();
		this.mesh.RecalculateBounds();
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x00043124 File Offset: 0x00041324
	public void RustRemove(Vector3? point = null)
	{
		if (this.DMGdeformMesh || this.Fairable)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit = default(RaycastHit);
			if (Physics.Raycast(ray, out raycastHit))
			{
				bool preview = !Input.GetKey(KeyCode.Mouse0);
				int priority = 0;
				float pressure = 1f;
				int seed = 0;
				Quaternion rotation = Quaternion.LookRotation(-raycastHit.normal);
				if (base.GetComponent<MPobject>())
				{
					base.GetComponent<MPobject>().networkDummy.RustRemoveContinue(preview, priority, pressure, seed, rotation, raycastHit.point);
					return;
				}
				this.RustRemoveContinue(preview, priority, pressure, seed, rotation, raycastHit.point);
			}
		}
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x000431D0 File Offset: 0x000413D0
	public void RustRemoveContinue(bool preview, int priority, float pressure, int seed, Quaternion rotation, Vector3 hit)
	{
		this.RustRepairDecal.HandleHitPoint(preview, priority, pressure, seed, hit, rotation);
		this.PaintRemoveDecal.HandleHitPoint(preview, priority, pressure, seed, hit, rotation);
		this.WashDecal.HandleHitPoint(preview, priority, pressure, seed, hit, rotation);
		if (this.Condition == 1f)
		{
			this.RustRepairedDecal.TargetModel = base.GetComponent<P3dModel>();
			this.RustRepairedDecal.HandleHitPoint(preview, priority, pressure, seed, hit, rotation);
		}
		if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 25f)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().AngleGrinder);
		}
		base.StartCoroutine(this.CheckRust());
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x0004329F File Offset: 0x0004149F
	public void CheckRustStart()
	{
		base.StartCoroutine(this.CheckRust());
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x000432AE File Offset: 0x000414AE
	private IEnumerator CheckRust()
	{
		this.RustChangeCounter.enabled = true;
		yield return 0;
		yield return 0;
		yield return 0;
		if (this.RustChangeCounter.Ratio < 0.045f && !this.Ruined)
		{
			this.Condition = 1f;
		}
		this.RustChangeCounter.enabled = false;
		yield break;
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x000432C0 File Offset: 0x000414C0
	public void DMGcheck()
	{
		if (this.AffectsWCOLrotationZ)
		{
			float num = this.WheelController.transform.parent.localEulerAngles.z - (float)Mathf.CeilToInt(this.WheelController.transform.parent.localEulerAngles.z / 360f) * 360f;
			if (num < 0f)
			{
				num += 360f;
			}
			if (num <= 359f && num >= 1f)
			{
				this.Ruined = true;
				if (this.damagedMesh)
				{
					base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
					this.Damaged = true;
				}
				if (num <= 300f && num >= 180f)
				{
					this.WheelController.transform.parent.localRotation = Quaternion.Euler(0f, this.WheelController.transform.parent.localEulerAngles.y, 300f);
					base.gameObject.GetComponent<DetachablePart>().DefinetDetach();
				}
				if (num >= 60f && num <= 180f)
				{
					this.WheelController.transform.parent.localRotation = Quaternion.Euler(0f, this.WheelController.transform.parent.localEulerAngles.y, 60f);
					base.gameObject.GetComponent<DetachablePart>().DefinetDetach();
				}
			}
		}
		if (this.AffectsWCOLrotationY)
		{
			float num2 = this.WheelController.transform.parent.localEulerAngles.y - (float)Mathf.CeilToInt(this.WheelController.transform.parent.localEulerAngles.y / 360f) * 360f;
			if (num2 < 0f)
			{
				num2 += 360f;
			}
			if (num2 <= 359f && num2 >= 1f)
			{
				this.Ruined = true;
				if (this.damagedMesh)
				{
					base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
					this.Damaged = true;
				}
				if (num2 <= 300f && num2 >= 180f)
				{
					this.WheelController.transform.parent.localRotation = Quaternion.Euler(0f, 300f, this.WheelController.transform.parent.localEulerAngles.z);
					base.gameObject.GetComponent<DetachablePart>().DefinetDetach();
				}
				if (num2 >= 60f && num2 <= 180f)
				{
					this.WheelController.transform.parent.localRotation = Quaternion.Euler(0f, 60f, this.WheelController.transform.parent.localEulerAngles.z);
					base.gameObject.GetComponent<DetachablePart>().DefinetDetach();
				}
			}
		}
		if (this.AffectsWCOLpositionZ && this.WheelController != null && this.WheelController.transform.parent.GetComponent<CarProperties>())
		{
			if (this.WheelController.transform.parent.localPosition.z >= this.WheelController.transform.parent.GetComponent<CarProperties>().StartZ - 0.03f && this.WheelController.transform.parent.localPosition.z <= this.WheelController.transform.parent.GetComponent<CarProperties>().StartZ + 0.03f)
			{
				this.WheelController.transform.parent.localPosition = new Vector3(this.WheelController.transform.parent.localPosition.x, this.WheelController.transform.parent.GetComponent<CarProperties>().StartY, this.WheelController.transform.parent.GetComponent<CarProperties>().StartZ);
			}
			else
			{
				this.Ruined = true;
				if (this.damagedMesh)
				{
					base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
					this.Damaged = true;
				}
			}
		}
		if (this.AffectsWCOLpositionX && this.WheelController != null && this.WheelController.transform.parent.GetComponent<CarProperties>())
		{
			if (this.WheelController.transform.parent.transform.localPosition.x >= this.WheelController.transform.parent.GetComponent<CarProperties>().StartX - 0.03f && this.WheelController.transform.parent.transform.localPosition.x <= this.WheelController.transform.parent.GetComponent<CarProperties>().StartX + 0.03f)
			{
				this.WheelController.transform.parent.localPosition = new Vector3(this.WheelController.transform.parent.GetComponent<CarProperties>().StartX, this.WheelController.transform.parent.GetComponent<CarProperties>().StartY, this.WheelController.transform.parent.localPosition.z);
			}
			else
			{
				this.Ruined = true;
				if (this.damagedMesh)
				{
					base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
					this.Damaged = true;
				}
			}
		}
		if (this.AffectsFRSuspensionPosition)
		{
			if (this.FRSuspensionPosition.transform.localPosition.x >= this.FRSuspensionPosition.GetComponent<CarProperties>().StartX - 0.03f && this.FRSuspensionPosition.transform.localPosition.x <= this.FRSuspensionPosition.GetComponent<CarProperties>().StartX + 0.03f && this.FRSuspensionPosition.transform.localPosition.y >= this.FRSuspensionPosition.GetComponent<CarProperties>().StartY - 0.03f && this.FRSuspensionPosition.transform.localPosition.y <= this.FRSuspensionPosition.GetComponent<CarProperties>().StartY + 0.03f && this.FRSuspensionPosition.transform.localPosition.z >= this.FRSuspensionPosition.GetComponent<CarProperties>().StartZ - 0.03f && this.FRSuspensionPosition.transform.localPosition.z <= this.FRSuspensionPosition.GetComponent<CarProperties>().StartZ + 0.03f)
			{
				this.FRSuspensionPosition.transform.localPosition = new Vector3(this.FRSuspensionPosition.GetComponent<CarProperties>().StartX, this.FRSuspensionPosition.GetComponent<CarProperties>().StartY, this.FRSuspensionPosition.GetComponent<CarProperties>().StartZ);
			}
			else
			{
				this.Ruined = true;
				if (this.damagedMesh)
				{
					base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
					this.Damaged = true;
				}
			}
		}
		if (this.AffectsFLSuspensionPosition)
		{
			if (this.FLSuspensionPosition.transform.localPosition.x >= this.FLSuspensionPosition.GetComponent<CarProperties>().StartX - 0.03f && this.FLSuspensionPosition.transform.localPosition.x <= this.FLSuspensionPosition.GetComponent<CarProperties>().StartX + 0.03f && this.FLSuspensionPosition.transform.localPosition.y >= this.FLSuspensionPosition.GetComponent<CarProperties>().StartY - 0.03f && this.FLSuspensionPosition.transform.localPosition.y <= this.FLSuspensionPosition.GetComponent<CarProperties>().StartY + 0.03f && this.FLSuspensionPosition.transform.localPosition.z >= this.FLSuspensionPosition.GetComponent<CarProperties>().StartZ - 0.03f && this.FLSuspensionPosition.transform.localPosition.z <= this.FLSuspensionPosition.GetComponent<CarProperties>().StartZ + 0.03f)
			{
				this.FLSuspensionPosition.transform.localPosition = new Vector3(this.FLSuspensionPosition.GetComponent<CarProperties>().StartX, this.FLSuspensionPosition.GetComponent<CarProperties>().StartY, this.FLSuspensionPosition.GetComponent<CarProperties>().StartZ);
			}
			else
			{
				this.Ruined = true;
				if (this.damagedMesh)
				{
					base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
					this.Damaged = true;
				}
			}
		}
		if (this.AffectsRRSuspensionPositionX)
		{
			if (this.RRSuspensionPosition.transform.localPosition.x >= this.RRSuspensionPosition.GetComponent<CarProperties>().StartX - 0.03f && this.RRSuspensionPosition.transform.localPosition.x <= this.RRSuspensionPosition.GetComponent<CarProperties>().StartX + 0.03f)
			{
				this.RRSuspensionPosition.transform.localPosition = new Vector3(this.RRSuspensionPosition.GetComponent<CarProperties>().StartX, this.RRSuspensionPosition.GetComponent<CarProperties>().StartY, this.RRSuspensionPosition.transform.localPosition.z);
			}
			else
			{
				this.Ruined = true;
				if (this.damagedMesh)
				{
					base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
					this.Damaged = true;
				}
			}
		}
		if (this.AffectsRRSuspensionPositionZ)
		{
			if (this.RRSuspensionPosition.transform.localPosition.z >= this.RRSuspensionPosition.GetComponent<CarProperties>().StartZ - 0.03f && this.RRSuspensionPosition.transform.localPosition.z <= this.RRSuspensionPosition.GetComponent<CarProperties>().StartZ + 0.03f)
			{
				this.RRSuspensionPosition.transform.localPosition = new Vector3(this.RRSuspensionPosition.GetComponent<CarProperties>().StartX, this.RRSuspensionPosition.GetComponent<CarProperties>().StartY, this.RRSuspensionPosition.transform.localPosition.z);
			}
			else
			{
				this.Ruined = true;
				if (this.damagedMesh)
				{
					base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
					this.Damaged = true;
				}
			}
		}
		if (this.AffectsRLSuspensionPositionX)
		{
			if (this.RLSuspensionPosition.transform.localPosition.x >= this.RLSuspensionPosition.GetComponent<CarProperties>().StartX - 0.03f && this.RLSuspensionPosition.transform.localPosition.x <= this.RLSuspensionPosition.GetComponent<CarProperties>().StartX + 0.03f)
			{
				this.RLSuspensionPosition.transform.localPosition = new Vector3(this.RLSuspensionPosition.GetComponent<CarProperties>().StartX, this.RLSuspensionPosition.GetComponent<CarProperties>().StartY, this.RLSuspensionPosition.transform.localPosition.z);
			}
			else
			{
				this.Ruined = true;
				if (this.damagedMesh)
				{
					base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
					this.Damaged = true;
				}
			}
		}
		if (this.AffectsRLSuspensionPositionZ)
		{
			if (this.RLSuspensionPosition.transform.localPosition.z >= this.RLSuspensionPosition.GetComponent<CarProperties>().StartZ - 0.03f && this.RLSuspensionPosition.transform.localPosition.z <= this.RLSuspensionPosition.GetComponent<CarProperties>().StartZ + 0.03f)
			{
				this.RLSuspensionPosition.transform.localPosition = new Vector3(this.RLSuspensionPosition.GetComponent<CarProperties>().StartX, this.RLSuspensionPosition.GetComponent<CarProperties>().StartY, this.RLSuspensionPosition.transform.localPosition.z);
			}
			else
			{
				this.Ruined = true;
				if (this.damagedMesh)
				{
					base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
					this.Damaged = true;
				}
			}
		}
		if (this.Ruined && this.damagedMesh)
		{
			base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
			this.Damaged = true;
		}
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x00043F3C File Offset: 0x0004213C
	public void TireBlow()
	{
		this.Damaged = true;
		this.Ruined = true;
		this.TirePressure = 0f;
		this.ReStart();
		this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().TireBlow);
		base.gameObject.GetComponent<MeshFilter>().mesh = this.damagedMesh;
		this.Damaged = true;
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x00043FA5 File Offset: 0x000421A5
	public void ApplyChrome()
	{
		if (base.GetComponent<MPobject>())
		{
			base.GetComponent<MPobject>().networkDummy.ApplyChrome2();
			return;
		}
		this.ApplyChrome2();
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00043FCB File Offset: 0x000421CB
	public void ApplyChrome2()
	{
		this.ChromeMat = this.PREFAB.GetComponent<CarProperties>().ChromeMat;
		this.Chromed = true;
		base.GetComponent<Renderer>().sharedMaterial = this.ChromeMat;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x0004400E File Offset: 0x0004220E
	[CompilerGenerated]
	private IEnumerator <Start>g__SetPartinfo|344_0()
	{
		yield return 0;
		base.GetComponent<Partinfo>().tightnuts = this.tightnuts;
		if (base.transform.name == "Rockers07" && base.transform.parent && this.tightnuts == 0f)
		{
			base.GetComponent<Partinfo>().tightnuts = base.GetComponent<Partinfo>().attachedbolts;
		}
		if (base.GetComponent<Partinfo>().tightnuts > base.GetComponent<Partinfo>().attachedbolts)
		{
			base.GetComponent<Partinfo>().tightnuts = base.GetComponent<Partinfo>().attachedbolts;
		}
		base.GetComponent<Partinfo>().fixedwelds = 0f;
		base.GetComponent<Partinfo>().fixedImportantBolts = 0f;
		foreach (string b in this.Loose)
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.name == b && transform.GetComponent<HexNut>())
				{
					transform.GetComponent<HexNut>().tight = false;
					transform.position += transform.TransformDirection(0f, 0.007f, 0f);
				}
				if (transform.gameObject.name == b && transform.GetComponent<BoltNut>())
				{
					transform.GetComponent<BoltNut>().tight = false;
					transform.position += transform.TransformDirection(0f, 0.007f, 0f);
				}
				if (transform.gameObject.name == b && transform.GetComponent<FlatNut>())
				{
					transform.GetComponent<FlatNut>().tight = false;
					transform.position += transform.TransformDirection(0f, 0.007f, 0f);
				}
				if (transform.gameObject.name == b && transform.GetComponent<WeldCut>())
				{
					transform.GetComponent<WeldCut>().welded = false;
				}
			}
		}
		if (base.GetComponent<Partinfo>().tightnuts < 0f)
		{
			base.GetComponent<Partinfo>().tightnuts = 0f;
		}
		yield break;
	}

	// Token: 0x04000B62 RID: 2914
	public string PartName;

	// Token: 0x04000B63 RID: 2915
	public string PartNameExtension;

	// Token: 0x04000B64 RID: 2916
	public string PrefabName;

	// Token: 0x04000B65 RID: 2917
	public int Type;

	// Token: 0x04000B66 RID: 2918
	public int Type2;

	// Token: 0x04000B67 RID: 2919
	public int ObjectNumber;

	// Token: 0x04000B68 RID: 2920
	public int ChildrenNumber;

	// Token: 0x04000B69 RID: 2921
	public bool NumberPlate;

	// Token: 0x04000B6A RID: 2922
	public Material One;

	// Token: 0x04000B6B RID: 2923
	public Material Two;

	// Token: 0x04000B6C RID: 2924
	public Material Three;

	// Token: 0x04000B6D RID: 2925
	public Material Four;

	// Token: 0x04000B6E RID: 2926
	public Material Five;

	// Token: 0x04000B6F RID: 2927
	public Material Six;

	// Token: 0x04000B70 RID: 2928
	public string Nr1;

	// Token: 0x04000B71 RID: 2929
	public string Nr2;

	// Token: 0x04000B72 RID: 2930
	public string Nr3;

	// Token: 0x04000B73 RID: 2931
	public string Nr4;

	// Token: 0x04000B74 RID: 2932
	public string Nr5;

	// Token: 0x04000B75 RID: 2933
	public string Nr6;

	// Token: 0x04000B76 RID: 2934
	public bool WearWorking;

	// Token: 0x04000B77 RID: 2935
	public bool WearDriving;

	// Token: 0x04000B78 RID: 2936
	public bool WearByTime;

	// Token: 0x04000B79 RID: 2937
	public float WearSpeed;

	// Token: 0x04000B7A RID: 2938
	public bool triger;

	// Token: 0x04000B7B RID: 2939
	public Mesh Differentmesh;

	// Token: 0x04000B7C RID: 2940
	public bool tight;

	// Token: 0x04000B7D RID: 2941
	public int SavePosition;

	// Token: 0x04000B7E RID: 2942
	public int JunkSpawnChance;

	// Token: 0x04000B7F RID: 2943
	public string Owner;

	// Token: 0x04000B80 RID: 2944
	public bool SinglePart;

	// Token: 0x04000B81 RID: 2945
	public GameObject PREFAB;

	// Token: 0x04000B82 RID: 2946
	public bool InBarn;

	// Token: 0x04000B83 RID: 2947
	public float ConditionDebug;

	// Token: 0x04000B84 RID: 2948
	public Mesh NormalMesh;

	// Token: 0x04000B85 RID: 2949
	public Mesh RemovedDifferentMesh;

	// Token: 0x04000B86 RID: 2950
	public GameObject VisualObject;

	// Token: 0x04000B87 RID: 2951
	public MainCarProperties MainProperties;

	// Token: 0x04000B88 RID: 2952
	public VehicleController exp;

	// Token: 0x04000B89 RID: 2953
	public bool started;

	// Token: 0x04000B8A RID: 2954
	public bool Attached;

	// Token: 0x04000B8B RID: 2955
	public bool picked;

	// Token: 0x04000B8C RID: 2956
	public bool IsCopy;

	// Token: 0x04000B8D RID: 2957
	public P3dChangeCounter RustChangeCounter;

	// Token: 0x04000B8E RID: 2958
	public P3dPaintDecal RepairDecal;

	// Token: 0x04000B8F RID: 2959
	public P3dPaintDecal RustRepairDecal;

	// Token: 0x04000B90 RID: 2960
	public P3dPaintDecal RustRepairedDecal;

	// Token: 0x04000B91 RID: 2961
	public P3dPaintDecal PaintRemoveDecal;

	// Token: 0x04000B92 RID: 2962
	public P3dPaintDecal WashDecal;

	// Token: 0x04000B93 RID: 2963
	public bool Chromed;

	// Token: 0x04000B94 RID: 2964
	public Material ChromeMat;

	// Token: 0x04000B95 RID: 2965
	public GameObject unmountedWheel;

	// Token: 0x04000B96 RID: 2966
	public GameObject WheelController;

	// Token: 0x04000B97 RID: 2967
	public GameObject FRSuspensionPosition;

	// Token: 0x04000B98 RID: 2968
	public GameObject FLSuspensionPosition;

	// Token: 0x04000B99 RID: 2969
	public GameObject RRSuspensionPosition;

	// Token: 0x04000B9A RID: 2970
	public GameObject RLSuspensionPosition;

	// Token: 0x04000B9B RID: 2971
	public GameObject AudioParent;

	// Token: 0x04000B9C RID: 2972
	public GameObject MaterialParent;

	// Token: 0x04000B9D RID: 2973
	public bool HandBrake;

	// Token: 0x04000B9E RID: 2974
	public bool HandBrakeCable;

	// Token: 0x04000B9F RID: 2975
	public bool MeshRepairable;

	// Token: 0x04000BA0 RID: 2976
	public bool Tintable;

	// Token: 0x04000BA1 RID: 2977
	public int TintLevel;

	// Token: 0x04000BA2 RID: 2978
	public bool CantRust;

	// Token: 0x04000BA3 RID: 2979
	public bool Paintable;

	// Token: 0x04000BA4 RID: 2980
	public bool Washable;

	// Token: 0x04000BA5 RID: 2981
	public bool Fairable;

	// Token: 0x04000BA6 RID: 2982
	public bool Interior;

	// Token: 0x04000BA7 RID: 2983
	public int OriginalInterior;

	// Token: 0x04000BA8 RID: 2984
	public GameObject StartOption1;

	// Token: 0x04000BA9 RID: 2985
	public GameObject StartOption2;

	// Token: 0x04000BAA RID: 2986
	public GameObject StartOption3;

	// Token: 0x04000BAB RID: 2987
	public GameObject StartOption4;

	// Token: 0x04000BAC RID: 2988
	public GameObject StartOption5;

	// Token: 0x04000BAD RID: 2989
	public GameObject StartOption6;

	// Token: 0x04000BAE RID: 2990
	public GameObject StartOption7;

	// Token: 0x04000BAF RID: 2991
	public GameObject StartOption8;

	// Token: 0x04000BB0 RID: 2992
	public GameObject StartOption9;

	// Token: 0x04000BB1 RID: 2993
	public bool VisualWheel;

	// Token: 0x04000BB2 RID: 2994
	public bool NonRotatingVisualWheel;

	// Token: 0x04000BB3 RID: 2995
	public bool RealWheel;

	// Token: 0x04000BB4 RID: 2996
	public bool Hub;

	// Token: 0x04000BB5 RID: 2997
	public CarProperties tireObject;

	// Token: 0x04000BB6 RID: 2998
	public bool Tire;

	// Token: 0x04000BB7 RID: 2999
	public float TirePressure;

	// Token: 0x04000BB8 RID: 3000
	public float TireType;

	// Token: 0x04000BB9 RID: 3001
	public float forwSlipCoef;

	// Token: 0x04000BBA RID: 3002
	public float forwForcCoef;

	// Token: 0x04000BBB RID: 3003
	public float sideSlipCoef;

	// Token: 0x04000BBC RID: 3004
	public float sideForcCoef;

	// Token: 0x04000BBD RID: 3005
	public bool TireValve;

	// Token: 0x04000BBE RID: 3006
	public bool Spring;

	// Token: 0x04000BBF RID: 3007
	public bool Damper;

	// Token: 0x04000BC0 RID: 3008
	public bool SuspensionPart;

	// Token: 0x04000BC1 RID: 3009
	public float SpacerSize;

	// Token: 0x04000BC2 RID: 3010
	public bool BrakePad;

	// Token: 0x04000BC3 RID: 3011
	public bool BrakeDisc;

	// Token: 0x04000BC4 RID: 3012
	public bool BrakeLine;

	// Token: 0x04000BC5 RID: 3013
	public bool MainBrakeLine;

	// Token: 0x04000BC6 RID: 3014
	public bool BrakeMaster;

	// Token: 0x04000BC7 RID: 3015
	public FLUID BrakeFluid;

	// Token: 0x04000BC8 RID: 3016
	public bool TieRod;

	// Token: 0x04000BC9 RID: 3017
	public bool BrakePedal;

	// Token: 0x04000BCA RID: 3018
	public bool SteeringWheel;

	// Token: 0x04000BCB RID: 3019
	public bool SteeringBox;

	// Token: 0x04000BCC RID: 3020
	public bool IgnitionKey;

	// Token: 0x04000BCD RID: 3021
	public bool WindowLift;

	// Token: 0x04000BCE RID: 3022
	public bool WindowLiftFL;

	// Token: 0x04000BCF RID: 3023
	public bool WindowLiftFR;

	// Token: 0x04000BD0 RID: 3024
	public bool WindowLiftRL;

	// Token: 0x04000BD1 RID: 3025
	public bool WindowLiftRR;

	// Token: 0x04000BD2 RID: 3026
	public bool WiperMotor;

	// Token: 0x04000BD3 RID: 3027
	public bool WiperL;

	// Token: 0x04000BD4 RID: 3028
	public bool WiperR;

	// Token: 0x04000BD5 RID: 3029
	public bool WiperOnly;

	// Token: 0x04000BD6 RID: 3030
	public bool WiperBlade;

	// Token: 0x04000BD7 RID: 3031
	public bool TrailerHook;

	// Token: 0x04000BD8 RID: 3032
	public bool BrakeLight;

	// Token: 0x04000BD9 RID: 3033
	public bool ReverseLight;

	// Token: 0x04000BDA RID: 3034
	public bool HeadLightLow;

	// Token: 0x04000BDB RID: 3035
	public bool HeadLightHigh;

	// Token: 0x04000BDC RID: 3036
	public bool RunningLight;

	// Token: 0x04000BDD RID: 3037
	public bool RightLight;

	// Token: 0x04000BDE RID: 3038
	public bool LeftLight;

	// Token: 0x04000BDF RID: 3039
	public bool Bulb;

	// Token: 0x04000BE0 RID: 3040
	public bool CarTrailer;

	// Token: 0x04000BE1 RID: 3041
	public bool FL;

	// Token: 0x04000BE2 RID: 3042
	public bool FR;

	// Token: 0x04000BE3 RID: 3043
	public bool RL;

	// Token: 0x04000BE4 RID: 3044
	public bool RR;

	// Token: 0x04000BE5 RID: 3045
	public bool FRONT;

	// Token: 0x04000BE6 RID: 3046
	public bool REAR;

	// Token: 0x04000BE7 RID: 3047
	public bool Openable;

	// Token: 0x04000BE8 RID: 3048
	public bool Cup;

	// Token: 0x04000BE9 RID: 3049
	public bool CupOpen;

	// Token: 0x04000BEA RID: 3050
	public bool SolidAxle;

	// Token: 0x04000BEB RID: 3051
	public bool ThisIsAfPOSITION;

	// Token: 0x04000BEC RID: 3052
	public bool AffectsHandling;

	// Token: 0x04000BED RID: 3053
	public bool AffectsWCOLrotationZ;

	// Token: 0x04000BEE RID: 3054
	public bool AffectsWCOLrotationY;

	// Token: 0x04000BEF RID: 3055
	public bool AffectsWCOLpositionX;

	// Token: 0x04000BF0 RID: 3056
	public bool AffectsWCOLpositionZ;

	// Token: 0x04000BF1 RID: 3057
	public bool AffectsFRSuspensionPosition;

	// Token: 0x04000BF2 RID: 3058
	public bool AffectsFLSuspensionPosition;

	// Token: 0x04000BF3 RID: 3059
	public bool AffectsRLSuspensionPositionX;

	// Token: 0x04000BF4 RID: 3060
	public bool AffectsRLSuspensionPositionZ;

	// Token: 0x04000BF5 RID: 3061
	public bool AffectsRRSuspensionPositionX;

	// Token: 0x04000BF6 RID: 3062
	public bool AffectsRRSuspensionPositionZ;

	// Token: 0x04000BF7 RID: 3063
	public float RimR;

	// Token: 0x04000BF8 RID: 3064
	public float radius;

	// Token: 0x04000BF9 RID: 3065
	public float width;

	// Token: 0x04000BFA RID: 3066
	public float SpringLength;

	// Token: 0x04000BFB RID: 3067
	public float SpringForce;

	// Token: 0x04000BFC RID: 3068
	public float DamperBumpForce;

	// Token: 0x04000BFD RID: 3069
	public float DamperReboundForce;

	// Token: 0x04000BFE RID: 3070
	public CoilNut coilnut;

	// Token: 0x04000BFF RID: 3071
	public bool Gearbox;

	// Token: 0x04000C00 RID: 3072
	public bool Manual;

	// Token: 0x04000C01 RID: 3073
	public GearProfile TransmissionGearingProfile;

	// Token: 0x04000C02 RID: 3074
	public GearProfile TransmissionGearingbroken1;

	// Token: 0x04000C03 RID: 3075
	public GearProfile TransmissionGearingbroken2;

	// Token: 0x04000C04 RID: 3076
	public GearProfile TransmissionGearingbroken3;

	// Token: 0x04000C05 RID: 3077
	public bool Shifter;

	// Token: 0x04000C06 RID: 3078
	public Mesh Reverse;

	// Token: 0x04000C07 RID: 3079
	public Mesh N;

	// Token: 0x04000C08 RID: 3080
	public Mesh R;

	// Token: 0x04000C09 RID: 3081
	public Mesh P;

	// Token: 0x04000C0A RID: 3082
	public Mesh First;

	// Token: 0x04000C0B RID: 3083
	public Mesh Second;

	// Token: 0x04000C0C RID: 3084
	public Mesh Third;

	// Token: 0x04000C0D RID: 3085
	public Mesh Fourth;

	// Token: 0x04000C0E RID: 3086
	public Mesh Fifth;

	// Token: 0x04000C0F RID: 3087
	public bool Chain;

	// Token: 0x04000C10 RID: 3088
	public bool ChainSprocket;

	// Token: 0x04000C11 RID: 3089
	public bool WheelSprocket;

	// Token: 0x04000C12 RID: 3090
	public bool DriveShaft;

	// Token: 0x04000C13 RID: 3091
	public bool DriveShaftFront;

	// Token: 0x04000C14 RID: 3092
	public bool DriveShaftMiddle;

	// Token: 0x04000C15 RID: 3093
	public bool AxleFront;

	// Token: 0x04000C16 RID: 3094
	public bool AxleRear;

	// Token: 0x04000C17 RID: 3095
	public bool Diff;

	// Token: 0x04000C18 RID: 3096
	public float DiffRatio;

	// Token: 0x04000C19 RID: 3097
	public bool DiffLocked;

	// Token: 0x04000C1A RID: 3098
	public bool DiffFront;

	// Token: 0x04000C1B RID: 3099
	public bool TransferCase;

	// Token: 0x04000C1C RID: 3100
	public bool Flywheel;

	// Token: 0x04000C1D RID: 3101
	public bool Clutch;

	// Token: 0x04000C1E RID: 3102
	public float ClutchSlipTorque;

	// Token: 0x04000C1F RID: 3103
	public bool ClutchCover;

	// Token: 0x04000C20 RID: 3104
	public bool ClutchPedal;

	// Token: 0x04000C21 RID: 3105
	public bool GasPedal;

	// Token: 0x04000C22 RID: 3106
	public bool FuelLine;

	// Token: 0x04000C23 RID: 3107
	public bool FuelTank;

	// Token: 0x04000C24 RID: 3108
	public FLUID Fuel;

	// Token: 0x04000C25 RID: 3109
	public bool FuelPump;

	// Token: 0x04000C26 RID: 3110
	public bool IgnitionCoil;

	// Token: 0x04000C27 RID: 3111
	public bool SparkPlug;

	// Token: 0x04000C28 RID: 3112
	public bool SparkWires;

	// Token: 0x04000C29 RID: 3113
	public bool Distributor;

	// Token: 0x04000C2A RID: 3114
	public bool Carburetor;

	// Token: 0x04000C2B RID: 3115
	public bool Turbo;

	// Token: 0x04000C2C RID: 3116
	public bool TurboPipe;

	// Token: 0x04000C2D RID: 3117
	public bool Battery;

	// Token: 0x04000C2E RID: 3118
	public float BatteryCharge;

	// Token: 0x04000C2F RID: 3119
	public bool BatteryWires;

	// Token: 0x04000C30 RID: 3120
	public bool Alternator;

	// Token: 0x04000C31 RID: 3121
	public bool AlternatorPulley;

	// Token: 0x04000C32 RID: 3122
	public bool Starter;

	// Token: 0x04000C33 RID: 3123
	public bool CamshaftSprocket;

	// Token: 0x04000C34 RID: 3124
	public bool CrankshaftSprocket;

	// Token: 0x04000C35 RID: 3125
	public bool HarmonicBalancer;

	// Token: 0x04000C36 RID: 3126
	public bool CrankshaftPulley;

	// Token: 0x04000C37 RID: 3127
	public bool AlternatorBelt;

	// Token: 0x04000C38 RID: 3128
	public bool BlowerPulley;

	// Token: 0x04000C39 RID: 3129
	public bool Blower;

	// Token: 0x04000C3A RID: 3130
	public bool BlowerBelt;

	// Token: 0x04000C3B RID: 3131
	public bool GlowPlug;

	// Token: 0x04000C3C RID: 3132
	public bool FuelFilter;

	// Token: 0x04000C3D RID: 3133
	public bool Injector;

	// Token: 0x04000C3E RID: 3134
	public bool FuelHoses;

	// Token: 0x04000C3F RID: 3135
	public bool GlowPlugRelay;

	// Token: 0x04000C40 RID: 3136
	public bool DieselEngine;

	// Token: 0x04000C41 RID: 3137
	public bool injected;

	// Token: 0x04000C42 RID: 3138
	public bool EngineBlock;

	// Token: 0x04000C43 RID: 3139
	public bool AirCooled;

	// Token: 0x04000C44 RID: 3140
	public bool DoubleHeads;

	// Token: 0x04000C45 RID: 3141
	public float EngineDisplacement;

	// Token: 0x04000C46 RID: 3142
	public float Power;

	// Token: 0x04000C47 RID: 3143
	public float EngineCylinderCount;

	// Token: 0x04000C48 RID: 3144
	public bool EngineHead;

	// Token: 0x04000C49 RID: 3145
	public bool EngineHead2;

	// Token: 0x04000C4A RID: 3146
	public bool HeadGasket;

	// Token: 0x04000C4B RID: 3147
	public bool HeadGasket2;

	// Token: 0x04000C4C RID: 3148
	public bool HeadCover;

	// Token: 0x04000C4D RID: 3149
	public bool HeadCover2;

	// Token: 0x04000C4E RID: 3150
	public bool Rockers;

	// Token: 0x04000C4F RID: 3151
	public bool Rockers2;

	// Token: 0x04000C50 RID: 3152
	public AudioClip EngineIdling;

	// Token: 0x04000C51 RID: 3153
	public AudioClip EngineRunning;

	// Token: 0x04000C52 RID: 3154
	public AudioClip EngineRunningNoExhaust;

	// Token: 0x04000C53 RID: 3155
	public bool AirFilter;

	// Token: 0x04000C54 RID: 3156
	public bool AirFilterCover;

	// Token: 0x04000C55 RID: 3157
	public bool Crankshaft;

	// Token: 0x04000C56 RID: 3158
	public bool EngineChain;

	// Token: 0x04000C57 RID: 3159
	public bool Camshaft;

	// Token: 0x04000C58 RID: 3160
	public bool Camshaft2;

	// Token: 0x04000C59 RID: 3161
	public bool Piston;

	// Token: 0x04000C5A RID: 3162
	public bool Radiator;

	// Token: 0x04000C5B RID: 3163
	public bool RadiatorFan;

	// Token: 0x04000C5C RID: 3164
	public bool RadiatorGT;

	// Token: 0x04000C5D RID: 3165
	public bool RadiatorFanGT;

	// Token: 0x04000C5E RID: 3166
	public bool WaterPumpPulley;

	// Token: 0x04000C5F RID: 3167
	public bool WaterPump;

	// Token: 0x04000C60 RID: 3168
	public bool WaterPumpBelt;

	// Token: 0x04000C61 RID: 3169
	public bool WaterHoseUpper;

	// Token: 0x04000C62 RID: 3170
	public bool WaterHoseLower;

	// Token: 0x04000C63 RID: 3171
	public bool ThermostatHousing;

	// Token: 0x04000C64 RID: 3172
	public FLUID Coolant;

	// Token: 0x04000C65 RID: 3173
	public bool OilPan;

	// Token: 0x04000C66 RID: 3174
	public bool OilFilter;

	// Token: 0x04000C67 RID: 3175
	public FLUID EngineOil;

	// Token: 0x04000C68 RID: 3176
	public bool Exhaust;

	// Token: 0x04000C69 RID: 3177
	public bool ExhaustHeader;

	// Token: 0x04000C6A RID: 3178
	public bool ExhaustManifold;

	// Token: 0x04000C6B RID: 3179
	public bool ExhaustSmoke;

	// Token: 0x04000C6C RID: 3180
	public bool ExhaustHeaderSmoke;

	// Token: 0x04000C6D RID: 3181
	public bool ExhaustManifoldSmoke;

	// Token: 0x04000C6E RID: 3182
	public bool HeadSmoke;

	// Token: 0x04000C6F RID: 3183
	public bool Exhaust2;

	// Token: 0x04000C70 RID: 3184
	public bool ExhaustHeader2;

	// Token: 0x04000C71 RID: 3185
	public bool ExhaustManifold2;

	// Token: 0x04000C72 RID: 3186
	public bool ExhaustSmoke2;

	// Token: 0x04000C73 RID: 3187
	public bool ExhaustHeaderSmoke2;

	// Token: 0x04000C74 RID: 3188
	public bool ExhaustManifoldSmoke2;

	// Token: 0x04000C75 RID: 3189
	public bool HeadSmoke2;

	// Token: 0x04000C76 RID: 3190
	public bool Cluster;

	// Token: 0x04000C77 RID: 3191
	public bool AnalogRpmGauge;

	// Token: 0x04000C78 RID: 3192
	public bool DigitalRpmGauge;

	// Token: 0x04000C79 RID: 3193
	public bool AnalogSpeedGauge;

	// Token: 0x04000C7A RID: 3194
	public bool DigitalSpeedGauge;

	// Token: 0x04000C7B RID: 3195
	public bool TempGauge;

	// Token: 0x04000C7C RID: 3196
	public bool FuelGauge;

	// Token: 0x04000C7D RID: 3197
	public bool BatteryGauge;

	// Token: 0x04000C7E RID: 3198
	public bool Clock;

	// Token: 0x04000C7F RID: 3199
	public Text MileageText;

	// Token: 0x04000C80 RID: 3200
	public float ClusterMileage;

	// Token: 0x04000C81 RID: 3201
	public GameObject ClusterL;

	// Token: 0x04000C82 RID: 3202
	public GameObject ClusterR;

	// Token: 0x04000C83 RID: 3203
	public GameObject ClusterBat;

	// Token: 0x04000C84 RID: 3204
	public GameObject ClusterHigh;

	// Token: 0x04000C85 RID: 3205
	public GameObject ClusterABS;

	// Token: 0x04000C86 RID: 3206
	public GameObject ClusterCheck;

	// Token: 0x04000C87 RID: 3207
	public GameObject ClusterGlowPlugs;

	// Token: 0x04000C88 RID: 3208
	public float FluidSize;

	// Token: 0x04000C89 RID: 3209
	public float DieselPercent;

	// Token: 0x04000C8A RID: 3210
	public float FluidCondition;

	// Token: 0x04000C8B RID: 3211
	public bool DMGdisplacepart;

	// Token: 0x04000C8C RID: 3212
	public bool DMGdeformMesh;

	// Token: 0x04000C8D RID: 3213
	public bool DMGRemovablepart;

	// Token: 0x04000C8E RID: 3214
	public bool DMGAnyDamag;

	// Token: 0x04000C8F RID: 3215
	public bool Damaged;

	// Token: 0x04000C90 RID: 3216
	public bool Ruined;

	// Token: 0x04000C91 RID: 3217
	public bool MeshDamaged;

	// Token: 0x04000C92 RID: 3218
	public bool MeshLittleDamaged;

	// Token: 0x04000C93 RID: 3219
	public float DamageReceived;

	// Token: 0x04000C94 RID: 3220
	public Mesh RemovedMesh;

	// Token: 0x04000C95 RID: 3221
	public Mesh WornMesh;

	// Token: 0x04000C96 RID: 3222
	public Mesh RuinedMesh;

	// Token: 0x04000C97 RID: 3223
	public Mesh damagedMesh;

	// Token: 0x04000C98 RID: 3224
	public Material WornMaterial;

	// Token: 0x04000C99 RID: 3225
	public Material RuinedMaterial;

	// Token: 0x04000C9A RID: 3226
	public Material OldMaterial;

	// Token: 0x04000C9B RID: 3227
	public bool PartIsOld;

	// Token: 0x04000C9C RID: 3228
	public int BodyMatNumber;

	// Token: 0x04000C9D RID: 3229
	public bool ConditionMatters;

	// Token: 0x04000C9E RID: 3230
	public float Condition;

	// Token: 0x04000C9F RID: 3231
	public float RealCondition;

	// Token: 0x04000CA0 RID: 3232
	public float CleanRatio;

	// Token: 0x04000CA1 RID: 3233
	public float NoRustRatio;

	// Token: 0x04000CA2 RID: 3234
	public float PaintRatio;

	// Token: 0x04000CA3 RID: 3235
	public bool PaintIsSet;

	// Token: 0x04000CA4 RID: 3236
	private float StartX;

	// Token: 0x04000CA5 RID: 3237
	private float StartY;

	// Token: 0x04000CA6 RID: 3238
	private float StartZ;

	// Token: 0x04000CA7 RID: 3239
	private float StartXr;

	// Token: 0x04000CA8 RID: 3240
	private float StartYr;

	// Token: 0x04000CA9 RID: 3241
	private float StartZr;

	// Token: 0x04000CAA RID: 3242
	public bool RepairSpot;

	// Token: 0x04000CAB RID: 3243
	public LiftHandle BikeStand;

	// Token: 0x04000CAC RID: 3244
	public int x;

	// Token: 0x04000CAD RID: 3245
	private Mesh mesh;

	// Token: 0x04000CAE RID: 3246
	private Vector3[] vertices;

	// Token: 0x04000CAF RID: 3247
	public Vector3[] initialvertices;

	// Token: 0x04000CB0 RID: 3248
	public Vector3[] Damagedvertices;

	// Token: 0x04000CB1 RID: 3249
	public CarProperties ChildDamag;

	// Token: 0x04000CB2 RID: 3250
	public string Texture1;

	// Token: 0x04000CB3 RID: 3251
	public string Texture2;

	// Token: 0x04000CB4 RID: 3252
	public string Texture3;

	// Token: 0x04000CB5 RID: 3253
	public string Texture4;

	// Token: 0x04000CB6 RID: 3254
	public List<string> Loose = new List<string>();

	// Token: 0x04000CB7 RID: 3255
	public float tightnuts;

	// Token: 0x04000CB8 RID: 3256
	public float fixedwelds;

	// Token: 0x04000CB9 RID: 3257
	public float fixedImportantBolts;
}
