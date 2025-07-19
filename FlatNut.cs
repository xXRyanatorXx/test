using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class FlatNut : MonoBehaviour
{
	// Token: 0x06000923 RID: 2339 RVA: 0x000592EE File Offset: 0x000574EE
	private void Start()
	{
		this.AudioParent = GameObject.Find("hand");
		this.CheckPos = this.AudioParent.GetComponent<AudioManager>().CheckPos;
		this.CheckPos2 = this.AudioParent.GetComponent<AudioManager>().CheckPos2;
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x0005932C File Offset: 0x0005752C
	public void disableREND()
	{
		if (base.transform.parent.parent != null)
		{
			if (!(base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name))
			{
				base.gameObject.GetComponent<MeshRenderer>().enabled = false;
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
			base.gameObject.GetComponent<MeshRenderer>().enabled = false;
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

	// Token: 0x06000925 RID: 2341 RVA: 0x000594F0 File Offset: 0x000576F0
	public void enableREND()
	{
		if (base.gameObject.transform.parent.parent.name == base.gameObject.transform.parent.name)
		{
			base.gameObject.GetComponent<MeshRenderer>().enabled = true;
			base.enabled = true;
		}
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x0005954C File Offset: 0x0005774C
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

	// Token: 0x06000927 RID: 2343 RVA: 0x00059600 File Offset: 0x00057800
	public void Loosen()
	{
		if (!this.tight)
		{
			return;
		}
		if (tools.tool == 3)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Ratcheting2);
		}
		this.tight = false;
		if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts > 0f)
		{
			base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts -= 1f;
		}
		base.gameObject.transform.position += base.transform.TransformDirection(0f, 0.007f, 0f);
		if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts == 0f && base.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts == 0f)
		{
			base.gameObject.transform.parent.GetComponent<Partinfo>().remove(true);
		}
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00059720 File Offset: 0x00057920
	public void tighten2()
	{
		if (this.tight)
		{
			return;
		}
		if (!base.transform.parent.GetComponent<CarProperties>().Ruined && base.transform.parent.parent)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Ratcheting);
			this.tight = true;
			base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts += 1f;
			base.gameObject.transform.position -= base.transform.TransformDirection(0f, 0.007f, 0f);
			if (base.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts > 0f)
			{
				base.gameObject.transform.parent.GetComponent<Partinfo>().attach();
			}
		}
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x00059828 File Offset: 0x00057A28
	private void Update()
	{
		if (tools.tool == 3)
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
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1f, 1 << LayerMask.NameToLayer("FlatBolts")) && raycastHit.collider.gameObject == base.gameObject)
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
						if (transform3.name == this.PartThatNeedsToBeOffname && !transform3.gameObject.transform.GetComponent<transparents>())
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

	// Token: 0x04001104 RID: 4356
	public bool tight;

	// Token: 0x04001105 RID: 4357
	public bool canfix;

	// Token: 0x04001106 RID: 4358
	public bool ChildrenHaveToBeRemoved;

	// Token: 0x04001107 RID: 4359
	public bool NothingFound;

	// Token: 0x04001108 RID: 4360
	public bool ONEChildrenHaveToBeRemoved;

	// Token: 0x04001109 RID: 4361
	public bool PartHaveToBeREmoved;

	// Token: 0x0400110A RID: 4362
	public bool PartHaveToBeREmovedDependsOnSide;

	// Token: 0x0400110B RID: 4363
	public GameObject ONEChildren;

	// Token: 0x0400110C RID: 4364
	public bool NotImportant;

	// Token: 0x0400110D RID: 4365
	public GameObject PartThatNeedsToBeOff;

	// Token: 0x0400110E RID: 4366
	public GameObject PartThatNeedsToBeOffL;

	// Token: 0x0400110F RID: 4367
	public GameObject PartThatNeedsToBeOffR;

	// Token: 0x04001110 RID: 4368
	public string PartThatNeedsToBeOffname;

	// Token: 0x04001111 RID: 4369
	public string PartThatNeedsToBeOffnameL;

	// Token: 0x04001112 RID: 4370
	public string PartThatNeedsToBeOffnameR;

	// Token: 0x04001113 RID: 4371
	public bool Started;

	// Token: 0x04001114 RID: 4372
	public GameObject AudioParent;

	// Token: 0x04001115 RID: 4373
	private Transform CheckPos;

	// Token: 0x04001116 RID: 4374
	private Transform CheckPos2;
}
