using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class BoltNut : MonoBehaviour
{
	// Token: 0x060008F5 RID: 2293 RVA: 0x000554FC File Offset: 0x000536FC
	private void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("Bolts");
		if (this.otherobjectNameL == "")
		{
			this.otherobjectNameL = this.otherobjectName;
		}
		if (this.otherobjectNameR == "")
		{
			this.otherobjectNameR = this.otherobjectName;
		}
		this.AudioParent = GameObject.Find("hand");
		this.CheckPos = this.AudioParent.GetComponent<AudioManager>().CheckPos;
		this.CheckPos2 = this.AudioParent.GetComponent<AudioManager>().CheckPos2;
		this.FindOther();
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x0005559C File Offset: 0x0005379C
	public void CheckAffected()
	{
		if (this.tight)
		{
			if (this.AffectsGrandParent1 && base.gameObject.transform.parent.parent && base.gameObject.transform.parent.parent.parent)
			{
				base.gameObject.transform.parent.parent.parent.GetComponent<Partinfo>().ChildrenFixedBolts += 1f;
			}
			if (this.AffectsGrandParent2 && base.gameObject.transform.parent.parent && base.gameObject.transform.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Partinfo>())
			{
				base.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Partinfo>().ChildrenFixedBolts += 1f;
			}
			if (this.AffectsGrandParent3 && base.gameObject.transform.parent.parent && base.gameObject.transform.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<Partinfo>())
			{
				base.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<Partinfo>().ChildrenFixedBolts += 1f;
			}
		}
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x000558C0 File Offset: 0x00053AC0
	public void StartFromMain()
	{
		if (this.tight)
		{
			this.ReStart();
			if (this.otherobject)
			{
				base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts += 1f;
				this.otherobject.GetComponent<Partinfo>().fixedImportantBolts += 1f;
				if (base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts > base.gameObject.transform.parent.GetComponent<Partinfo>().ImportantBolts)
				{
					base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts = base.gameObject.transform.parent.GetComponent<Partinfo>().ImportantBolts;
				}
				if (this.otherobject.GetComponent<Partinfo>().fixedImportantBolts > this.otherobject.GetComponent<Partinfo>().ImportantBolts)
				{
					this.otherobject.GetComponent<Partinfo>().fixedImportantBolts = this.otherobject.GetComponent<Partinfo>().ImportantBolts;
				}
			}
		}
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x000559DC File Offset: 0x00053BDC
	public void tighten()
	{
		if (!this.tight || tools.Tighten)
		{
			if (!this.tight && tools.Tighten)
			{
				if (base.transform.parent.GetComponent<MPobject>())
				{
					base.transform.parent.GetComponent<MPobject>().networkDummy.tighten(base.transform.GetSiblingIndex());
					return;
				}
				this.tighten2();
			}
			return;
		}
		if (base.transform.parent.GetComponent<MPobject>())
		{
			base.transform.parent.GetComponent<MPobject>().networkDummy.Loosen(base.transform.GetSiblingIndex());
			return;
		}
		this.Loosen();
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x00055A90 File Offset: 0x00053C90
	public void Loosen()
	{
		if (!this.tight)
		{
			return;
		}
		this.ReStart();
		if (tools.tool == 2)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Ratcheting2);
		}
		this.tight = false;
		base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts -= 1f;
		this.otherobject.GetComponent<Partinfo>().fixedImportantBolts -= 1f;
		base.gameObject.transform.position += base.transform.TransformDirection(0f, 0.007f, 0f);
		if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts == 0f && base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts == 0f)
		{
			base.gameObject.transform.parent.GetComponent<Partinfo>().remove(true);
		}
		if (this.otherobject.GetComponent<Partinfo>().tightnuts < 1f && this.otherobject.GetComponent<Partinfo>().fixedImportantBolts < 1f)
		{
			this.otherobject.GetComponent<Partinfo>().remove(true);
		}
		if (this.AffectsGrandParent1 && base.gameObject.transform.parent.parent.parent && !base.gameObject.transform.parent.parent.parent.GetComponent<Partinfo>())
		{
			base.gameObject.transform.parent.parent.parent.GetComponent<Partinfo>().ChildrenFixedBolts -= 1f;
		}
		if (this.AffectsGrandParent2 && base.gameObject.transform.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent)
		{
			base.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Partinfo>().ChildrenFixedBolts -= 1f;
		}
		if (this.AffectsGrandParent3 && base.gameObject.transform.parent.parent.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<Partinfo>())
		{
			base.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<Partinfo>().ChildrenFixedBolts -= 1f;
		}
		this.CheckBolts();
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x00055DF8 File Offset: 0x00053FF8
	public void tighten2()
	{
		if (this.tight)
		{
			return;
		}
		this.ReStart();
		if (!base.transform.parent.GetComponent<CarProperties>().Ruined && this.otherobject && !this.otherobject.GetComponent<CarProperties>().Ruined && (!this.MatchTypeToBolt || (this.MatchTypeToBolt && base.transform.parent.GetComponent<CarProperties>().Type == this.otherobject.GetComponent<CarProperties>().Type) || (this.MatchTypeToBolt && base.transform.parent.GetComponent<CarProperties>().Gearbox && base.transform.parent.GetComponent<CarProperties>().Type == this.otherobject.GetComponent<CarProperties>().Type2 && this.otherobject.GetComponent<CarProperties>().Type2 != 0)))
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Ratcheting);
			this.tight = true;
			base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts += 1f;
			this.otherobject.GetComponent<Partinfo>().fixedImportantBolts += 1f;
			base.gameObject.transform.position -= base.transform.TransformDirection(0f, 0.007f, 0f);
			if (base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts == 1f)
			{
				base.gameObject.transform.parent.GetComponent<Partinfo>().attach();
			}
			if (this.otherobject.GetComponent<Partinfo>().fixedImportantBolts == 1f)
			{
				this.otherobject.GetComponent<Partinfo>().attach();
			}
			if (this.AffectsGrandParent1 && base.gameObject.transform.parent.parent.parent)
			{
				base.gameObject.transform.parent.parent.parent.GetComponent<Partinfo>().ChildrenFixedBolts += 1f;
			}
			if (this.AffectsGrandParent2 && base.gameObject.transform.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent)
			{
				base.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Partinfo>().ChildrenFixedBolts += 1f;
			}
			if (this.AffectsGrandParent3 && base.gameObject.transform.parent.parent.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<Partinfo>())
			{
				base.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<Partinfo>().ChildrenFixedBolts += 1f;
			}
		}
		this.CheckBolts();
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x000561D4 File Offset: 0x000543D4
	public void ReStart()
	{
		this.otherobject = null;
		this.otherobjectL = null;
		this.otherobjectR = null;
		foreach (Transform transform in base.transform.root.GetComponentsInChildren<Transform>())
		{
			if (transform.name == this.otherobjectName && transform.gameObject.layer != LayerMask.NameToLayer("TransparentParts") && !base.transform.parent.parent.gameObject.GetComponent<transparents>().L && !base.transform.parent.parent.gameObject.GetComponent<transparents>().R)
			{
				this.otherobject = transform.gameObject;
			}
			if (transform.name == this.otherobjectNameL && transform.gameObject.layer != LayerMask.NameToLayer("TransparentParts"))
			{
				this.otherobjectL = transform.gameObject;
			}
			if (transform.name == this.otherobjectNameR && transform.gameObject.layer != LayerMask.NameToLayer("TransparentParts"))
			{
				this.otherobjectR = transform.gameObject;
			}
		}
		if (this.otherobjectL && base.transform.parent.parent.gameObject.GetComponent<transparents>().L)
		{
			this.otherobject = this.otherobjectL;
		}
		if (this.otherobjectR && base.transform.parent.parent.gameObject.GetComponent<transparents>().R)
		{
			this.otherobject = this.otherobjectR;
		}
		if (this.otherobject)
		{
			this.canfix = true;
			this.CheckedForOther = true;
		}
		if (!this.otherobject)
		{
			base.enabled = false;
			base.gameObject.GetComponent<MeshRenderer>().enabled = false;
			if (this.tight)
			{
				this.tight = false;
				base.gameObject.transform.position += base.transform.TransformDirection(0f, 0.007f, 0f);
				base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts -= 1f;
			}
		}
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x00056424 File Offset: 0x00054624
	public void FindOther()
	{
		this.otherobject = null;
		this.otherobjectL = null;
		this.otherobjectR = null;
		foreach (Transform transform in base.transform.root.GetComponentsInChildren<Transform>())
		{
			if (transform.name == this.otherobjectName && transform.gameObject.layer != LayerMask.NameToLayer("TransparentParts") && !base.transform.parent.parent.gameObject.GetComponent<transparents>().L && !base.transform.parent.parent.gameObject.GetComponent<transparents>().R)
			{
				this.otherobject = transform.gameObject;
			}
			if (transform.name == this.otherobjectNameL && transform.gameObject.layer != LayerMask.NameToLayer("TransparentParts"))
			{
				this.otherobjectL = transform.gameObject;
			}
			if (transform.name == this.otherobjectNameR && transform.gameObject.layer != LayerMask.NameToLayer("TransparentParts"))
			{
				this.otherobjectR = transform.gameObject;
			}
		}
		if (this.otherobjectL && base.transform.parent.parent.gameObject.GetComponent<transparents>().L)
		{
			this.otherobject = this.otherobjectL;
		}
		if (this.otherobjectR && base.transform.parent.parent.gameObject.GetComponent<transparents>().R)
		{
			this.otherobject = this.otherobjectR;
		}
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x000565C8 File Offset: 0x000547C8
	public void CheckBolts()
	{
		foreach (Partinfo partinfo in base.transform.root.GetComponentsInChildren<Partinfo>())
		{
			if ((partinfo.attachedbolts > 0f || partinfo.attachedwelds > 0f || partinfo.ImportantBolts > 0f) && partinfo.transform.root.name != "EngineStand")
			{
				if (partinfo.transform.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast") && partinfo.transform.GetComponent<Partinfo>().tightnuts == 0f && partinfo.fixedImportantBolts == 0f && partinfo.fixedwelds == 0f)
				{
					partinfo.remove(true);
				}
				if (partinfo.transform.gameObject.layer == LayerMask.NameToLayer("LooseParts") && (partinfo.tightnuts > 0f || partinfo.fixedImportantBolts > 0f || partinfo.ChildrenFixedBolts > 0f || partinfo.fixedwelds > 0f) && partinfo.transform.parent)
				{
					partinfo.attach();
				}
			}
		}
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x00056704 File Offset: 0x00054904
	public void BrokeOff()
	{
		if (this.tight)
		{
			this.tight = false;
			base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts -= 1f;
			if (!this.otherobject)
			{
				foreach (Transform transform in base.transform.root.GetComponentsInChildren<Transform>())
				{
					if (transform.name == this.otherobjectName && transform.gameObject.layer != LayerMask.NameToLayer("TransparentParts") && !base.transform.parent.parent.gameObject.GetComponent<transparents>().L && !base.transform.parent.parent.gameObject.GetComponent<transparents>().R)
					{
						this.otherobject = transform.gameObject;
					}
					if (transform.name == this.otherobjectNameL && transform.gameObject.layer != LayerMask.NameToLayer("TransparentParts"))
					{
						this.otherobjectL = transform.gameObject;
					}
					if (transform.name == this.otherobjectNameR && transform.gameObject.layer != LayerMask.NameToLayer("TransparentParts"))
					{
						this.otherobjectR = transform.gameObject;
					}
				}
				if (this.otherobjectL && base.transform.parent.parent.gameObject.GetComponent<transparents>().L)
				{
					this.otherobject = this.otherobjectL;
				}
				if (this.otherobjectR && base.transform.parent.parent.gameObject.GetComponent<transparents>().R)
				{
					this.otherobject = this.otherobjectR;
				}
				if (this.otherobject)
				{
					this.canfix = true;
					this.CheckedForOther = true;
				}
			}
			if (this.otherobject)
			{
				if (base.transform.parent.parent && base.transform.parent.parent.gameObject.GetComponent<transparents>() && base.transform.parent.parent.gameObject.GetComponent<transparents>().L)
				{
					this.otherobject = this.otherobjectL;
				}
				if (base.transform.parent.parent && base.transform.parent.parent.gameObject.GetComponent<transparents>() && base.transform.parent.parent.gameObject.GetComponent<transparents>().R)
				{
					this.otherobject = this.otherobjectR;
				}
				this.otherobject.GetComponent<Partinfo>().fixedImportantBolts -= 1f;
			}
			base.gameObject.transform.position += base.transform.TransformDirection(0f, 0.007f, 0f);
			if (this.AffectsGrandParent1 && base.gameObject.transform.parent && base.gameObject.transform.parent.parent && base.gameObject.transform.parent.parent.parent)
			{
				base.gameObject.transform.parent.parent.parent.GetComponent<Partinfo>().ChildrenFixedBolts -= 1f;
			}
			if (this.AffectsGrandParent2 && base.gameObject.transform.parent && base.gameObject.transform.parent.parent && base.gameObject.transform.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent)
			{
				base.gameObject.transform.parent.parent.parent.parent.parent.GetComponent<Partinfo>().ChildrenFixedBolts -= 1f;
			}
			if (this.AffectsGrandParent3 && base.gameObject.transform.parent && base.gameObject.transform.parent.parent && base.gameObject.transform.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent.parent.parent && base.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<Partinfo>())
			{
				base.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<Partinfo>().ChildrenFixedBolts -= 1f;
			}
			if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts == 0f && base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts == 0f && base.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds == 0f)
			{
				base.gameObject.transform.parent.GetComponent<Partinfo>().remove(true);
			}
			if (this.otherobject && this.otherobject.GetComponent<Partinfo>().tightnuts == 0f && this.otherobject.GetComponent<Partinfo>().fixedImportantBolts == 0f && this.otherobject.GetComponent<Partinfo>().fixedwelds == 0f)
			{
				this.otherobject.GetComponent<Partinfo>().remove(true);
			}
		}
		if (!this.DontDisableRenderer)
		{
			base.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
		base.gameObject.GetComponent<BoxCollider>().enabled = false;
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x00056E24 File Offset: 0x00055024
	public void disableREND()
	{
		if (base.transform.parent.parent != null)
		{
			base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name;
			if (!this.DontDisableRenderer)
			{
				base.gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			base.gameObject.GetComponent<BoxCollider>().enabled = false;
			return;
		}
		if (!this.DontDisableRenderer)
		{
			base.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
		base.gameObject.GetComponent<BoxCollider>().enabled = false;
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x00056ED4 File Offset: 0x000550D4
	public void enableREND()
	{
		if (base.gameObject.transform.parent.parent && base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name)
		{
			base.gameObject.GetComponent<MeshRenderer>().enabled = true;
		}
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x00056F44 File Offset: 0x00055144
	private void Update()
	{
		if (tools.tool == 2)
		{
			if (Vector3.Distance(base.transform.position, this.CheckPos.position) < 0.3f || Vector3.Distance(base.transform.position, this.CheckPos2.position) < 0.3f)
			{
				base.gameObject.GetComponent<BoxCollider>().enabled = true;
			}
			else
			{
				base.gameObject.GetComponent<BoxCollider>().enabled = false;
			}
			if (this.CheckedForOther)
			{
				if (!this.canfix)
				{
					this.disableREND();
					return;
				}
				this.enableREND();
				RaycastHit raycastHit;
				if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.4f, 1 << LayerMask.NameToLayer("Bolts")) && raycastHit.collider.gameObject == base.gameObject)
				{
					if (!this.ChildrenHaveToBeRemoved)
					{
						this.tighten();
						return;
					}
					this.NothingFound = true;
					foreach (object obj in base.transform.parent)
					{
						Transform transform = (Transform)obj;
						if (transform.gameObject.transform.GetComponent<transparents>() != null)
						{
							using (IEnumerator enumerator2 = transform.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									if (((Transform)enumerator2.Current).name == transform.name)
									{
										this.NothingFound = false;
									}
								}
							}
						}
					}
					if (this.NothingFound)
					{
						this.tighten();
						return;
					}
					GameObject.Find("Player Camera").GetComponent<PlayerRayCasting>().highlightColor = Color.red;
					return;
				}
			}
			else
			{
				if (base.transform.parent.parent)
				{
					this.ReStart();
					return;
				}
				base.enabled = false;
				base.gameObject.GetComponent<MeshRenderer>().enabled = false;
				return;
			}
		}
		else if (!tools.cooldown)
		{
			this.CheckedForOther = false;
			if (!this.tight && !this.DontDisableRenderer)
			{
				base.gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			base.gameObject.GetComponent<BoxCollider>().enabled = false;
			base.enabled = false;
		}
	}

	// Token: 0x040010CF RID: 4303
	public GameObject otherobject;

	// Token: 0x040010D0 RID: 4304
	public GameObject otherobjectL;

	// Token: 0x040010D1 RID: 4305
	public GameObject otherobjectR;

	// Token: 0x040010D2 RID: 4306
	public string otherobjectName;

	// Token: 0x040010D3 RID: 4307
	public string otherobjectNameL;

	// Token: 0x040010D4 RID: 4308
	public string otherobjectNameR;

	// Token: 0x040010D5 RID: 4309
	public bool Started;

	// Token: 0x040010D6 RID: 4310
	public bool DontDisableRenderer;

	// Token: 0x040010D7 RID: 4311
	public bool CheckedForOther;

	// Token: 0x040010D8 RID: 4312
	public bool tight;

	// Token: 0x040010D9 RID: 4313
	public bool canfix;

	// Token: 0x040010DA RID: 4314
	public bool ChildrenHaveToBeRemoved;

	// Token: 0x040010DB RID: 4315
	public bool NothingFound;

	// Token: 0x040010DC RID: 4316
	public bool NotImportant;

	// Token: 0x040010DD RID: 4317
	public bool AffectsGrandParent1;

	// Token: 0x040010DE RID: 4318
	public bool AffectsGrandParent2;

	// Token: 0x040010DF RID: 4319
	public bool AffectsGrandParent3;

	// Token: 0x040010E0 RID: 4320
	public bool MatchTypeToBolt;

	// Token: 0x040010E1 RID: 4321
	public bool DisallowDistantBreaking;

	// Token: 0x040010E2 RID: 4322
	public GameObject AudioParent;

	// Token: 0x040010E3 RID: 4323
	public FixedJoint joint;

	// Token: 0x040010E4 RID: 4324
	private Transform CheckPos;

	// Token: 0x040010E5 RID: 4325
	private Transform CheckPos2;
}
