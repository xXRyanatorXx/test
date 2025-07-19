using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class HexNut : MonoBehaviour
{
	// Token: 0x0600092B RID: 2347 RVA: 0x00059C08 File Offset: 0x00057E08
	private void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("Bolts");
		this.AudioParent = GameObject.Find("hand");
		this.CheckPos = this.AudioParent.GetComponent<AudioManager>().CheckPos;
		this.CheckPos2 = this.AudioParent.GetComponent<AudioManager>().CheckPos2;
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00059C68 File Offset: 0x00057E68
	public void disableREND()
	{
		if (base.transform.parent.parent != null)
		{
			if (!(base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name))
			{
				if (!this.DontDisableRenderer)
				{
					base.gameObject.GetComponent<MeshRenderer>().enabled = false;
				}
				base.gameObject.GetComponent<BoxCollider>().enabled = false;
				if (this.tight)
				{
					this.tight = false;
					if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts > 0f)
					{
						base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts -= 1f;
					}
					base.gameObject.transform.position += base.transform.TransformDirection(0f, 0.007f, 0f);
					return;
				}
			}
		}
		else
		{
			if (!this.DontDisableRenderer)
			{
				base.gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			base.gameObject.GetComponent<BoxCollider>().enabled = false;
			if (this.tight)
			{
				this.tight = false;
				if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts > 0f)
				{
					base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts -= 1f;
				}
				base.gameObject.transform.position += base.transform.TransformDirection(0f, 0.007f, 0f);
			}
		}
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00059E3C File Offset: 0x0005803C
	public void enableREND()
	{
		if (base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name && base.gameObject.transform.root.name != "TireMounter" && this.Type == base.transform.parent.parent.GetComponent<transparents>().Type)
		{
			base.gameObject.GetComponent<MeshRenderer>().enabled = true;
			base.enabled = true;
			base.GetComponent<BoxCollider>().enabled = true;
		}
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x00059EE8 File Offset: 0x000580E8
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

	// Token: 0x0600092F RID: 2351 RVA: 0x00059F9C File Offset: 0x0005819C
	public void Loosen()
	{
		if (!this.tight)
		{
			return;
		}
		if (tools.tool == 2)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Ratcheting2);
		}
		this.tight = false;
		if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts > 0f)
		{
			base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts -= 1f;
		}
		base.gameObject.transform.position += base.transform.TransformDirection(0f, 0.007f, 0f);
		if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts < 1f && base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts < 1f)
		{
			base.gameObject.transform.parent.GetComponent<Partinfo>().remove(true);
		}
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x0005A0BC File Offset: 0x000582BC
	public void tighten2()
	{
		if (this.tight)
		{
			return;
		}
		if (!base.transform.parent.GetComponent<CarProperties>().Ruined && base.transform.parent && base.gameObject.transform.root.name != "TireMounter" && base.transform.parent.parent && this.Type == base.transform.parent.parent.GetComponent<transparents>().Type)
		{
			this.tight = true;
			if (tools.tool == 2)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Ratcheting);
			}
			base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts += 1f;
			base.gameObject.transform.position -= base.transform.TransformDirection(0f, 0.007f, 0f);
			if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts > 0f)
			{
				base.gameObject.transform.parent.GetComponent<Partinfo>().attach();
			}
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0005A22C File Offset: 0x0005842C
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
			RaycastHit raycastHit;
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.4f, 1 << LayerMask.NameToLayer("Bolts")) && raycastHit.collider.gameObject == base.gameObject)
			{
				if (this.ChildrenHaveToBeRemoved)
				{
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
				else if (this.ONEChildrenHaveToBeRemoved)
				{
					this.NothingFound = true;
					foreach (Transform transform2 in base.transform.parent.GetComponentsInChildren<Transform>())
					{
						if (transform2.name == this.PartThatNeedsToBeOffname && !transform2.gameObject.transform.GetComponent<transparents>())
						{
							this.NothingFound = false;
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
				else
				{
					if (!this.PartHaveToBeREmoved)
					{
						this.tighten();
						return;
					}
					this.NothingFound = true;
					if (this.PartHaveToBeREmovedDependsOnSide)
					{
						if (base.transform.parent.parent.gameObject.GetComponent<transparents>().FL || base.transform.parent.parent.gameObject.GetComponent<transparents>().RL)
						{
							this.PartThatNeedsToBeOffname = this.PartThatNeedsToBeOffnameL;
						}
						if (base.transform.parent.parent.gameObject.GetComponent<transparents>().FR || base.transform.parent.parent.gameObject.GetComponent<transparents>().RR)
						{
							this.PartThatNeedsToBeOffname = this.PartThatNeedsToBeOffnameR;
						}
					}
					foreach (Transform transform3 in base.transform.root.GetComponentsInChildren<Transform>())
					{
						if ((transform3.name == this.PartThatNeedsToBeOffname || (this.PartThatNeedsToBeOffname2 != "" && transform3.name == this.PartThatNeedsToBeOffname2)) && !transform3.gameObject.transform.GetComponent<transparents>())
						{
							this.NothingFound = false;
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
		}
		else if (!tools.cooldown)
		{
			base.gameObject.GetComponent<BoxCollider>().enabled = false;
			base.enabled = false;
		}
	}

	// Token: 0x04001117 RID: 4375
	public bool tight = true;

	// Token: 0x04001118 RID: 4376
	public bool DontDisableRenderer;

	// Token: 0x04001119 RID: 4377
	public bool Started;

	// Token: 0x0400111A RID: 4378
	public bool canfix;

	// Token: 0x0400111B RID: 4379
	public bool NothingFound;

	// Token: 0x0400111C RID: 4380
	public bool ChildrenHaveToBeRemoved;

	// Token: 0x0400111D RID: 4381
	public bool ONEChildrenHaveToBeRemoved;

	// Token: 0x0400111E RID: 4382
	public bool PartHaveToBeREmoved;

	// Token: 0x0400111F RID: 4383
	public bool PartHaveToBeREmovedDependsOnSide;

	// Token: 0x04001120 RID: 4384
	public bool NotImportant;

	// Token: 0x04001121 RID: 4385
	public GameObject ONEChildren;

	// Token: 0x04001122 RID: 4386
	public GameObject PartThatNeedsToBeOff;

	// Token: 0x04001123 RID: 4387
	public GameObject PartThatNeedsToBeOffL;

	// Token: 0x04001124 RID: 4388
	public GameObject PartThatNeedsToBeOffR;

	// Token: 0x04001125 RID: 4389
	public string PartThatNeedsToBeOffname;

	// Token: 0x04001126 RID: 4390
	public string PartThatNeedsToBeOffname2;

	// Token: 0x04001127 RID: 4391
	public string PartThatNeedsToBeOffnameL;

	// Token: 0x04001128 RID: 4392
	public string PartThatNeedsToBeOffnameR;

	// Token: 0x04001129 RID: 4393
	public GameObject AudioParent;

	// Token: 0x0400112A RID: 4394
	public int Type;

	// Token: 0x0400112B RID: 4395
	private Transform CheckPos;

	// Token: 0x0400112C RID: 4396
	private Transform CheckPos2;
}
