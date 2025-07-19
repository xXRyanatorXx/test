using System;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class WeldCut : MonoBehaviour
{
	// Token: 0x06000A46 RID: 2630 RVA: 0x000654C8 File Offset: 0x000636C8
	private void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("Weld");
		this.AudioParent = GameObject.Find("hand");
		this.SparksPefab = GameObject.Find("SparksEffect");
		this.WeldPefab = GameObject.Find("WeldEffect");
		this.ReStart();
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x00065520 File Offset: 0x00063720
	public void StartFromMain()
	{
		if (this.welded)
		{
			this.ReStart();
			if (this.otherobject && !this.WeldsToParent)
			{
				base.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds += 1f;
				this.otherobject.GetComponent<Partinfo>().fixedwelds += 1f;
				if (base.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds > base.gameObject.transform.parent.GetComponent<Partinfo>().attachedwelds)
				{
					base.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds = base.gameObject.transform.parent.GetComponent<Partinfo>().attachedwelds;
				}
				if (this.otherobject.GetComponent<Partinfo>().fixedwelds > this.otherobject.GetComponent<Partinfo>().attachedwelds)
				{
					this.otherobject.GetComponent<Partinfo>().fixedwelds = this.otherobject.GetComponent<Partinfo>().attachedwelds;
				}
			}
			if (this.WeldsToParent)
			{
				base.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds += 1f;
				if (base.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds > base.gameObject.transform.parent.GetComponent<Partinfo>().attachedwelds)
				{
					base.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds = base.gameObject.transform.parent.GetComponent<Partinfo>().attachedwelds;
				}
			}
		}
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x000656E4 File Offset: 0x000638E4
	public void ReStart()
	{
		this.otherobject = null;
		if (!this.WeldsToParent)
		{
			foreach (Transform transform in base.transform.root.GetComponentsInChildren<Transform>())
			{
				if (transform.name == this.otherobjectName && transform.gameObject.layer != LayerMask.NameToLayer("TransparentParts"))
				{
					this.otherobject = transform.gameObject;
					this.canweld = true;
					this.CheckedForOther = true;
				}
			}
			if (!this.otherobject)
			{
				this.welded = false;
				return;
			}
		}
		else
		{
			this.canweld = true;
			this.CheckedForOther = true;
		}
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x0006578C File Offset: 0x0006398C
	private void Update()
	{
		base.gameObject.GetComponent<MeshRenderer>().enabled = false;
		base.gameObject.GetComponent<MeshCollider>().enabled = false;
		if (this.WeldsToParent && base.transform.root.tag != "Vehicle")
		{
			base.transform.root.GetComponent<Rigidbody>().WakeUp();
		}
		if (tools.tool == 5 && tools.ToolHand.transform.childCount > 0 && tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().Attached)
		{
			this.PickupItems = tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().Attached.GetComponent<PickupItems>();
			if (this.PickupItems.Electrode)
			{
				if (this.CheckedForOther)
				{
					if (this.canweld && !base.transform.parent.GetComponent<CarProperties>().Ruined && ((this.otherobject && !this.otherobject.GetComponent<CarProperties>().Ruined) || this.WeldsToParent))
					{
						if (!this.welded)
						{
							base.gameObject.GetComponent<MeshRenderer>().enabled = true;
							base.gameObject.GetComponent<MeshCollider>().enabled = true;
						}
						else
						{
							base.gameObject.GetComponent<MeshRenderer>().enabled = false;
							base.gameObject.GetComponent<MeshCollider>().enabled = false;
						}
						RaycastHit raycastHit;
						if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.4f, 1 << LayerMask.NameToLayer("Weld")) && raycastHit.collider.gameObject == base.gameObject)
						{
							Vector3 point = raycastHit.point;
							if (base.transform.parent.GetComponent<MPobject>())
							{
								base.transform.parent.GetComponent<MPobject>().networkDummy.Weld(base.transform.GetSiblingIndex(), point);
								this.PickupItems.GetComponent<MPobject>().networkDummy.UpdatePickupitems();
							}
							else
							{
								this.Weld(point);
								this.PickupItems.UpdateCondition(1);
							}
						}
					}
				}
				else if (base.transform.parent.parent)
				{
					this.ReStart();
				}
				else
				{
					base.enabled = false;
				}
			}
		}
		else
		{
			this.canweld = false;
			this.CheckedForOther = false;
		}
		if (tools.tool == 4 && tools.ToolHand.transform.childCount > 0 && tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().Attached)
		{
			this.PickupItems = tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().Attached.GetComponent<PickupItems>();
			if (this.PickupItems.Cutter)
			{
				if (this.welded)
				{
					base.gameObject.GetComponent<MeshRenderer>().enabled = true;
					base.gameObject.GetComponent<MeshCollider>().enabled = true;
				}
				else
				{
					base.gameObject.GetComponent<MeshRenderer>().enabled = false;
					base.gameObject.GetComponent<MeshCollider>().enabled = false;
				}
				RaycastHit raycastHit2;
				if (this.welded && Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit2, 1.4f, 1 << LayerMask.NameToLayer("Weld")) && raycastHit2.collider.gameObject == base.gameObject)
				{
					Vector3 point2 = raycastHit2.point;
					if (base.transform.parent.GetComponent<MPobject>())
					{
						base.transform.parent.GetComponent<MPobject>().networkDummy.Cut(base.transform.GetSiblingIndex(), point2);
						this.PickupItems.GetComponent<MPobject>().networkDummy.UpdatePickupitems();
					}
					else
					{
						this.Cut(point2);
						this.PickupItems.UpdateCondition(1);
					}
				}
			}
		}
		if (tools.tool != 5 && tools.tool != 4)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x00065BDC File Offset: 0x00063DDC
	public void Weld(Vector3 point)
	{
		this.ReStart();
		if (!this.welded)
		{
			this.welded = true;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.WeldPefab, point, Quaternion.identity);
			gameObject.GetComponent<ParticleSystem>().Play();
			gameObject.GetComponent<SelfDestroy>().enabled = true;
			if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 20f)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Welding);
			}
			base.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds += 1f;
			if (!this.WeldsToParent)
			{
				this.otherobject.GetComponent<Partinfo>().fixedwelds += 1f;
			}
			base.gameObject.transform.parent.GetComponent<Partinfo>().attach();
			if (!this.WeldsToParent)
			{
				this.otherobject.GetComponent<Partinfo>().attach();
			}
		}
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00065CEC File Offset: 0x00063EEC
	public void Cut(Vector3 point)
	{
		this.ReStart();
		if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 20f)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().AngleGrinder);
		}
		this.welded = false;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.SparksPefab, point, Quaternion.identity);
		gameObject.GetComponent<ParticleSystem>().Play();
		gameObject.GetComponent<SelfDestroy>().enabled = true;
		base.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds -= 1f;
		if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts < 1f && base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts < 1f)
		{
			base.gameObject.transform.parent.GetComponent<Partinfo>().remove(true);
		}
		if (!this.WeldsToParent)
		{
			this.otherobject.GetComponent<Partinfo>().fixedwelds -= 1f;
			if (this.otherobject.GetComponent<Partinfo>().tightnuts < 1f && this.otherobject.GetComponent<Partinfo>().fixedImportantBolts < 1f)
			{
				this.otherobject.GetComponent<Partinfo>().remove(true);
			}
		}
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x00065E5C File Offset: 0x0006405C
	public void BrokeOff()
	{
		if (this.welded)
		{
			this.welded = false;
			base.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds -= 1f;
			if (base.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds == 0f)
			{
				base.gameObject.transform.parent.GetComponent<Partinfo>().remove(true);
			}
			if (this.otherobject && !this.WeldsToParent)
			{
				this.otherobject.GetComponent<Partinfo>().fixedwelds -= 1f;
				if (this.otherobject.GetComponent<Partinfo>().fixedwelds == 0f)
				{
					this.otherobject.GetComponent<Partinfo>().remove(true);
				}
			}
		}
	}

	// Token: 0x04001218 RID: 4632
	public bool WeldsToParent;

	// Token: 0x04001219 RID: 4633
	public GameObject otherobject;

	// Token: 0x0400121A RID: 4634
	public string otherobjectName;

	// Token: 0x0400121B RID: 4635
	public bool CheckedForOther;

	// Token: 0x0400121C RID: 4636
	public bool welded;

	// Token: 0x0400121D RID: 4637
	public bool canweld;

	// Token: 0x0400121E RID: 4638
	public FixedJoint joint;

	// Token: 0x0400121F RID: 4639
	public GameObject AudioParent;

	// Token: 0x04001220 RID: 4640
	public bool NotImportant;

	// Token: 0x04001221 RID: 4641
	public GameObject WeldSparks;

	// Token: 0x04001222 RID: 4642
	public GameObject Sparks;

	// Token: 0x04001223 RID: 4643
	public GameObject SparksPefab;

	// Token: 0x04001224 RID: 4644
	public GameObject WeldPefab;

	// Token: 0x04001225 RID: 4645
	public PickupItems PickupItems;
}
