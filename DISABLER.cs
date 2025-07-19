using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200018E RID: 398
public class DISABLER : MonoBehaviour
{
	// Token: 0x06000903 RID: 2307 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x000571C8 File Offset: 0x000553C8
	private IEnumerator waitsec()
	{
		yield return new WaitForSeconds(3f);
		if (base.GetComponent<HexNut>() != null)
		{
			base.GetComponent<HexNut>().disableREND();
		}
		if (base.GetComponent<BoltNut>() != null)
		{
			base.GetComponent<BoltNut>().enabled = false;
			base.GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<BoxCollider>().enabled = false;
		}
		if (base.GetComponent<FlatNut>() != null)
		{
			base.GetComponent<FlatNut>().enabled = false;
			base.GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<BoxCollider>().enabled = false;
		}
		if (base.GetComponent<Sparkplug>() != null)
		{
			base.GetComponent<Sparkplug>().enabled = false;
			base.GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<BoxCollider>().enabled = false;
		}
		if (base.GetComponent<WeldCut>() != null)
		{
			base.GetComponent<WeldCut>().enabled = false;
		}
		if (base.GetComponent<MyBoneSCR>() != null)
		{
			base.GetComponent<MyBoneSCR>().enabled = false;
		}
		if (tools.tool == 12 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().MeshDamaged && !base.gameObject.GetComponent<CarProperties>().Ruined && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 11 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 14 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 7 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Fairable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 4 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 19 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool != 7 && tools.tool != 11 && tools.tool != 12 && tools.tool != 14 && tools.tool != 18 && tools.tool != 19 && tools.tool != 21 && base.gameObject.layer == LayerMask.NameToLayer("Repair"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		}
		if (base.GetComponent<PickupItems>() != null && tools.tool == 1)
		{
			base.GetComponent<PickupItems>().enabled = false;
		}
		if (base.GetComponent<PickupTool>() != null && tools.tool == 1)
		{
			base.GetComponent<PickupTool>().enabled = false;
		}
		if (base.GetComponent<LiftHandle>() != null)
		{
			base.GetComponent<LiftHandle>().enabled = false;
		}
		if (base.GetComponent<FLUID>() != null)
		{
			base.GetComponent<FLUID>().enabled = true;
		}
		if (base.name == "TireValve")
		{
			base.GetComponent<MeshRenderer>().enabled = false;
			base.GetComponent<BoxCollider>().enabled = false;
		}
		yield break;
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x000571D8 File Offset: 0x000553D8
	public void DisableOtherScripts()
	{
		this.inside = false;
		if (!tools.sitting && base.GetComponent<MyBoneSCR>() != null)
		{
			base.GetComponent<MyBoneSCR>().enabled = false;
		}
		if (tools.tool == 12 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().MeshDamaged && !base.gameObject.GetComponent<CarProperties>().Ruined && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 11 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 14 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 7 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Fairable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 4 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 19 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool != 7 && tools.tool != 11 && tools.tool != 12 && tools.tool != 14 && tools.tool != 18 && tools.tool != 19 && tools.tool != 21 && base.gameObject.layer == LayerMask.NameToLayer("Repair"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		}
		if (base.GetComponent<PickupItems>() != null && tools.tool == 1)
		{
			base.GetComponent<PickupItems>().enabled = false;
		}
		if (base.GetComponent<PickupTool>() != null && tools.tool == 1)
		{
			base.GetComponent<PickupTool>().enabled = false;
		}
		if (base.GetComponent<LiftHandle>() != null)
		{
			base.GetComponent<LiftHandle>().enabled = false;
		}
		if (base.GetComponent<FLUID>() != null)
		{
			base.GetComponent<FLUID>().enabled = false;
		}
		if (this.DisableRenderer)
		{
			base.GetComponent<MeshRenderer>().enabled = false;
		}
		if (this.DisableCollider && !base.GetComponent<Rigidbody>() && base.transform.parent && !base.transform.parent.GetComponent<OpenableBox>())
		{
			base.GetComponent<MeshCollider>().enabled = false;
		}
		if (base.GetComponent<PickupTool>() != null && base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
		}
		if (base.transform.name == "EngineStand" && ((base.GetComponent<CarProperties>() != null && base.transform.root.tag != "Vehicle") || base.transform.name == "EngineStand"))
		{
			DISABLER[] componentsInChildren = base.GetComponentsInChildren<DISABLER>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].DisableFromMainCar();
			}
		}
		base.StartCoroutine(this.WaitDisable());
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x00057610 File Offset: 0x00055810
	public void EnableOtherScripts()
	{
		this.inside = true;
		if (base.GetComponent<HexNut>() != null && base.transform.parent.parent != null && base.transform.root.gameObject.layer != LayerMask.NameToLayer("Items"))
		{
			base.GetComponent<HexNut>().enableREND();
		}
		if (base.GetComponent<BoltNut>() != null)
		{
			base.GetComponent<BoltNut>().enabled = true;
			base.GetComponent<MeshRenderer>().enabled = true;
		}
		if (base.GetComponent<FlatNut>() != null && base.transform.parent.parent != null)
		{
			base.GetComponent<FlatNut>().enabled = true;
			base.GetComponent<MeshRenderer>().enabled = true;
		}
		if (base.GetComponent<Sparkplug>() != null)
		{
			base.GetComponent<Sparkplug>().enabled = true;
			base.GetComponent<MeshRenderer>().enabled = true;
		}
		if (base.GetComponent<WeldCut>() != null)
		{
			base.GetComponent<WeldCut>().enabled = true;
		}
		if (tools.tool == 12 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().MeshDamaged && !base.gameObject.GetComponent<CarProperties>().Ruined && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 11 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 14 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 7 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Fairable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 4 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 19 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool != 4 && tools.tool != 7 && tools.tool != 11 && tools.tool != 12 && tools.tool != 14 && tools.tool != 18 && tools.tool != 19 && base.gameObject.layer == LayerMask.NameToLayer("Repair"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		}
		if (tools.tool == 21 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Tintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (base.GetComponent<PickupItems>() != null && tools.tool == 1)
		{
			base.GetComponent<PickupItems>().enabled = true;
		}
		if (base.GetComponent<PickupTool>() != null && tools.tool == 1)
		{
			base.GetComponent<PickupTool>().enabled = true;
		}
		if (base.GetComponent<LiftHandle>() != null)
		{
			base.GetComponent<LiftHandle>().enabled = true;
		}
		if (base.GetComponent<FLUID>() != null)
		{
			base.GetComponent<FLUID>().enabled = true;
		}
		if (base.name == "TireValve")
		{
			base.GetComponent<MeshRenderer>().enabled = true;
			base.GetComponent<BoxCollider>().enabled = true;
		}
		if (base.GetComponent<PickupTool>() != null && base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		}
		if (this.DisableRenderer && base.gameObject.transform.parent && base.gameObject.transform.parent.parent && base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name)
		{
			base.GetComponent<MeshRenderer>().enabled = true;
		}
		if (this.DisableCollider)
		{
			base.GetComponent<MeshCollider>().enabled = true;
		}
		if ((base.GetComponent<CarProperties>() != null && base.transform.root.tag != "Vehicle") || base.transform.name == "EngineStand")
		{
			DISABLER[] componentsInChildren = base.GetComponentsInChildren<DISABLER>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].EnableFromMainCar();
			}
		}
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00057BB0 File Offset: 0x00055DB0
	public void DisableFromMainCar()
	{
		this.inside = false;
		if (!tools.sitting && base.GetComponent<MyBoneSCR>() != null)
		{
			base.GetComponent<MyBoneSCR>().enabled = false;
		}
		if (tools.tool == 12 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().MeshDamaged && !base.gameObject.GetComponent<CarProperties>().Ruined && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 11 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 14 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 7 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Fairable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 4 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 19 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool != 4 && tools.tool != 7 && tools.tool != 11 && tools.tool != 12 && tools.tool != 14 && tools.tool != 18 && tools.tool != 19 && tools.tool != 21 && base.gameObject.layer == LayerMask.NameToLayer("Repair"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		}
		if (this.DisableRenderer)
		{
			base.GetComponent<MeshRenderer>().enabled = false;
		}
		if (this.DisableCollider && !base.GetComponent<Rigidbody>() && base.transform.parent && !base.transform.parent.GetComponent<OpenableBox>())
		{
			base.GetComponent<MeshCollider>().enabled = false;
		}
		if (base.GetComponent<FLUID>() != null)
		{
			base.GetComponent<FLUID>().enabled = false;
		}
		base.StartCoroutine(this.WaitDisable());
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x00057EF8 File Offset: 0x000560F8
	public void EnableFromMainCar()
	{
		this.inside = true;
		if (base.GetComponent<HexNut>() != null && base.transform.parent.parent != null)
		{
			base.GetComponent<HexNut>().enableREND();
		}
		if (base.GetComponent<BoltNut>() != null)
		{
			base.GetComponent<BoltNut>().enabled = true;
			base.GetComponent<MeshRenderer>().enabled = true;
		}
		if (base.GetComponent<FlatNut>() != null && base.transform.parent.parent != null)
		{
			base.GetComponent<FlatNut>().enabled = true;
			base.GetComponent<MeshRenderer>().enabled = true;
		}
		if (base.GetComponent<Sparkplug>() != null)
		{
			base.GetComponent<Sparkplug>().enabled = true;
			base.GetComponent<MeshRenderer>().enabled = true;
			if (tools.tool == 8)
			{
				base.GetComponent<BoxCollider>().enabled = true;
			}
		}
		if (base.GetComponent<WeldCut>() != null)
		{
			base.GetComponent<WeldCut>().enabled = true;
		}
		if (base.GetComponent<MyBoneSCR>() != null && base.transform.root.tag == "Vehicle")
		{
			base.GetComponent<MyBoneSCR>().enabled = true;
		}
		if (tools.tool == 12 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().MeshDamaged && !base.gameObject.GetComponent<CarProperties>().Ruined && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 11 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 14 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 7 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Fairable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 4 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool != 4 && tools.tool != 7 && tools.tool != 11 && tools.tool != 12 && tools.tool != 14 && tools.tool != 18 && tools.tool != 19 && base.gameObject.layer == LayerMask.NameToLayer("Repair"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		}
		if (tools.tool == 21 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Tintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (tools.tool == 19 && base.gameObject.GetComponent<CarProperties>() != null && base.gameObject.GetComponent<CarProperties>().Paintable && base.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
		{
			base.gameObject.layer = LayerMask.NameToLayer("Repair");
		}
		if (this.DisableRenderer && base.gameObject.transform.parent && base.gameObject.transform.parent.parent && base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name)
		{
			base.GetComponent<MeshRenderer>().enabled = true;
		}
		if (this.DisableCollider)
		{
			base.GetComponent<MeshCollider>().enabled = true;
		}
		if (base.GetComponent<FLUID>() != null)
		{
			base.GetComponent<FLUID>().enabled = true;
		}
		if (base.name == "TireValve")
		{
			base.GetComponent<MeshRenderer>().enabled = true;
			base.GetComponent<BoxCollider>().enabled = true;
		}
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x000583DD File Offset: 0x000565DD
	private IEnumerator WaitDisable()
	{
		yield return 0;
		yield return 0;
		yield return 0;
		if (!this.inside)
		{
			if (base.GetComponent<HexNut>() != null)
			{
				base.GetComponent<HexNut>().disableREND();
			}
			if (base.GetComponent<BoltNut>() != null)
			{
				base.GetComponent<BoltNut>().enabled = false;
				base.GetComponent<MeshRenderer>().enabled = false;
				base.GetComponent<BoxCollider>().enabled = false;
			}
			if (base.GetComponent<FlatNut>() != null)
			{
				base.GetComponent<FlatNut>().enabled = false;
				base.GetComponent<MeshRenderer>().enabled = false;
				base.GetComponent<BoxCollider>().enabled = false;
			}
			if (base.GetComponent<Sparkplug>() != null)
			{
				base.GetComponent<Sparkplug>().enabled = false;
				base.GetComponent<MeshRenderer>().enabled = false;
				base.GetComponent<BoxCollider>().enabled = false;
			}
			if (base.GetComponent<WeldCut>() != null)
			{
				base.GetComponent<WeldCut>().enabled = false;
			}
			if (base.name == "TireValve")
			{
				base.GetComponent<MeshRenderer>().enabled = false;
				base.GetComponent<BoxCollider>().enabled = false;
			}
		}
		yield break;
	}

	// Token: 0x040010E6 RID: 4326
	public bool DisableRenderer;

	// Token: 0x040010E7 RID: 4327
	public bool DisableCollider;

	// Token: 0x040010E8 RID: 4328
	public bool inside;
}
