using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class transparents : MonoBehaviour
{
	// Token: 0x06000AB8 RID: 2744 RVA: 0x0006C604 File Offset: 0x0006A804
	public void Awake()
	{
		if (this.ChildrenMesh)
		{
			if (this.ChildrenMesh1 == null)
			{
				this.ChildrenMesh1 = this.ChildrenMesh;
			}
			if (this.ChildrenMesh2 == null)
			{
				this.ChildrenMesh2 = this.ChildrenMesh;
			}
			if (this.ChildrenMesh3 == null)
			{
				this.ChildrenMesh3 = this.ChildrenMesh;
			}
			if (this.ChildrenMesh4 == null)
			{
				this.ChildrenMesh4 = this.ChildrenMesh;
			}
		}
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x0006C688 File Offset: 0x0006A888
	public void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("TransparentParts");
		base.gameObject.tag = "transparentpart";
		this.ATTACHED = false;
		this.HaveAttached = false;
		this.DependsOn = 0;
		if (this.ChildrenMesh && !this.ChildrenMesh1)
		{
			this.ChildrenMesh1 = this.ChildrenMesh;
		}
		if (this.PartThatNeedsToBeOff)
		{
			this.PartThatNeedsToBeOffname = this.PartThatNeedsToBeOff.name.ToString();
		}
		if (this.PartThatNeedsToBeOffL)
		{
			this.PartThatNeedsToBeOffnameL = this.PartThatNeedsToBeOffL.name.ToString();
		}
		if (this.PartThatNeedsToBeOffR)
		{
			this.PartThatNeedsToBeOffnameR = this.PartThatNeedsToBeOffR.name.ToString();
		}
		transparents.dependantObjects[] dependants = this.DEPENDANTS;
		for (int i = 0; i < dependants.Length; i++)
		{
			dependants[i].dependant.GetComponent<transparents>().IsCanAttach();
			this.DependsOn++;
		}
		this.DISABLE();
		base.StartCoroutine(this.Start2());
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x0006C7A6 File Offset: 0x0006A9A6
	private IEnumerator Start2()
	{
		yield return 0;
		yield return 0;
		yield return 0;
		using (IEnumerator enumerator = base.transform.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (((Transform)enumerator.Current).name == base.name)
				{
					this.HaveAttached = true;
				}
			}
		}
		foreach (transparents.AttachingObjects attachingObjects in this.ATTACHABLES)
		{
			if (base.transform.childCount > 0)
			{
				attachingObjects.Attachable.GetComponent<transparents>().ATTACHED = true;
			}
			else
			{
				attachingObjects.Attachable.GetComponent<transparents>().UninstallATTACHABLES2();
				attachingObjects.Attachable.GetComponent<transparents>().ATTACHED = false;
			}
		}
		this.DISABLE();
		yield break;
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x0006C7B5 File Offset: 0x0006A9B5
	public void Disablerend()
	{
		this.DISABLE();
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x0006C7C0 File Offset: 0x0006A9C0
	public void IsCanAttach()
	{
		this.CanAttach = false;
		using (IEnumerator enumerator = base.transform.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (((Transform)enumerator.Current).name == base.gameObject.name)
				{
					this.CanAttach = true;
				}
			}
		}
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x0006C838 File Offset: 0x0006AA38
	public void DISABLE()
	{
		if (base.gameObject.GetComponent<MeshCollider>())
		{
			UnityEngine.Object.Destroy(base.gameObject.GetComponent<MeshCollider>());
		}
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x0006C85C File Offset: 0x0006AA5C
	public void ENABLE()
	{
		if (!base.gameObject.GetComponent<MeshCollider>())
		{
			base.gameObject.AddComponent<MeshCollider>().convex = true;
			base.gameObject.GetComponent<MeshCollider>().isTrigger = false;
		}
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x0006C894 File Offset: 0x0006AA94
	public void UninstallATTACHABLES()
	{
		foreach (transparents.AttachingObjects attachingObjects in this.ATTACHABLES)
		{
			if (attachingObjects.Attachable.GetComponent<transparents>().ATTACHED)
			{
				attachingObjects.Attachable.GetComponent<transparents>().UninstallATTACHABLES2();
				attachingObjects.Attachable.GetComponent<transparents>().ATTACHED = false;
			}
		}
		if (this.RemovesSpringFL || this.RemovesSpringFR || this.RemovesSpringRL || this.RemovesSpringRR)
		{
			foreach (transparents transparents in base.transform.root.GetComponentsInChildren<transparents>())
			{
				if (transparents.IsSpring && transparents.FL && this.RemovesSpringFL)
				{
					transparents.UninstallATTACHABLES3();
				}
				if (transparents.IsSpring && transparents.FR && this.RemovesSpringFR)
				{
					transparents.UninstallATTACHABLES3();
				}
				if (transparents.IsSpring && transparents.RL && this.RemovesSpringRL)
				{
					transparents.UninstallATTACHABLES3();
				}
				if (transparents.IsSpring && transparents.RR && this.RemovesSpringRR)
				{
					transparents.UninstallATTACHABLES3();
				}
			}
		}
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x0006C9C0 File Offset: 0x0006ABC0
	public void UninstallATTACHABLES2()
	{
		Partinfo[] componentsInChildren = base.gameObject.GetComponentsInChildren<Partinfo>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].remove(true);
			this.ATTACHED = false;
		}
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x0006C9F8 File Offset: 0x0006ABF8
	public void UninstallATTACHABLES3()
	{
		Partinfo[] componentsInChildren = base.gameObject.GetComponentsInChildren<Partinfo>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].remove(true);
		}
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x0006CA28 File Offset: 0x0006AC28
	public void InstallATTACHABLES()
	{
		foreach (transparents.AttachingObjects attachingObjects in this.ATTACHABLES)
		{
			if (!attachingObjects.Attachable.GetComponent<transparents>().ATTACHED)
			{
				attachingObjects.Attachable.GetComponent<transparents>().InstallATTACHABLES2();
				attachingObjects.Attachable.GetComponent<transparents>().ATTACHED = true;
			}
		}
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x0006CA84 File Offset: 0x0006AC84
	public void InstallATTACHABLES2()
	{
		Partinfo[] componentsInChildren = base.gameObject.GetComponentsInChildren<Partinfo>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].attach();
		}
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x0006CAB4 File Offset: 0x0006ACB4
	public void Recheck()
	{
		if (!base.transform.root.GetComponent<MainCarProperties>() || (base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().Owner != "Junkyard"))
		{
			transparents.dependantObjects[] dependants = this.DEPENDANTS;
			for (int i = 0; i < dependants.Length; i++)
			{
				dependants[i].dependant.GetComponent<transparents>().IsCanAttach();
			}
			base.StartCoroutine(this.Check());
		}
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x0006CB44 File Offset: 0x0006AD44
	private IEnumerator DoubleCheck()
	{
		yield return new WaitForSeconds(4f);
		if (tools.helditem == base.gameObject.name && !this.ATTACHED && !this.IsSpring && tools.tool != 18)
		{
			this.ENABLE();
		}
		yield break;
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x0006CB53 File Offset: 0x0006AD53
	private IEnumerator Check()
	{
		yield return 0;
		this.DISABLE();
		this.CanAttachBecauseOfPart = true;
		this.HaveAttached = false;
		using (IEnumerator enumerator = base.transform.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (((Transform)enumerator.Current).name == base.name)
				{
					this.HaveAttached = true;
				}
			}
		}
		if (!this.HaveAttached && tools.tool != 18)
		{
			if (this.PARENTHaveToBeREmoved)
			{
				if (base.transform.parent.parent == null)
				{
					this.ENABLE();
				}
			}
			else
			{
				if (this.PartHaveToBeREmoved)
				{
					if (this.PartHaveToBeREmovedDependsOnSide)
					{
						if (this.FL || this.RL)
						{
							this.PartThatNeedsToBeOffname = this.PartThatNeedsToBeOffnameL;
						}
						if (this.FR || this.RR)
						{
							this.PartThatNeedsToBeOffname = this.PartThatNeedsToBeOffnameR;
						}
					}
					foreach (Transform transform in base.transform.root.GetComponentsInChildren<Transform>())
					{
						if (transform.name == this.PartThatNeedsToBeOffname && transform.gameObject.transform.GetComponent<transparents>() == null)
						{
							this.CanAttachBecauseOfPart = false;
						}
					}
				}
				if (base.transform.childCount > 1)
				{
					using (IEnumerator enumerator = base.transform.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!(((Transform)enumerator.Current).name == base.gameObject.name))
							{
								if (this.DependsOn > 0)
								{
									transparents.dependantObjects[] dependants = this.DEPENDANTS;
									for (int i = 0; i < dependants.Length; i++)
									{
										if (dependants[i].dependant.GetComponent<transparents>().CanAttach && !this.ATTACHED && !this.IsSpring && this.CanAttachBecauseOfPart)
										{
											this.ENABLE();
										}
									}
								}
								else if (tools.helditem == base.gameObject.name && !this.ATTACHED && !this.IsSpring && this.CanAttachBecauseOfPart)
								{
									this.ENABLE();
								}
							}
						}
						yield break;
					}
				}
				if (this.IsSpring && tools.tool == 9)
				{
					this.ENABLE();
				}
				if (this.DependsOn > 0)
				{
					transparents.dependantObjects[] dependants = this.DEPENDANTS;
					for (int i = 0; i < dependants.Length; i++)
					{
						if (dependants[i].dependant.GetComponent<transparents>().CanAttach && !this.ATTACHED && !this.IsSpring && this.CanAttachBecauseOfPart)
						{
							this.ENABLE();
						}
					}
				}
				else if (tools.helditem == base.gameObject.name && !this.ATTACHED && !this.IsSpring && this.CanAttachBecauseOfPart)
				{
					this.ENABLE();
				}
			}
		}
		yield break;
	}

	// Token: 0x04001306 RID: 4870
	public GameObject RealParent;

	// Token: 0x04001307 RID: 4871
	public Mesh ChildrenMesh;

	// Token: 0x04001308 RID: 4872
	public Mesh ChildrenMesh1;

	// Token: 0x04001309 RID: 4873
	public Mesh ChildrenMesh2;

	// Token: 0x0400130A RID: 4874
	public Mesh ChildrenMesh3;

	// Token: 0x0400130B RID: 4875
	public Mesh ChildrenMesh4;

	// Token: 0x0400130C RID: 4876
	public int SavePosition;

	// Token: 0x0400130D RID: 4877
	public int Type;

	// Token: 0x0400130E RID: 4878
	public int Variation;

	// Token: 0x0400130F RID: 4879
	public bool NotImportantPart;

	// Token: 0x04001310 RID: 4880
	public bool invert;

	// Token: 0x04001311 RID: 4881
	public int DependsOn;

	// Token: 0x04001312 RID: 4882
	public bool CanAttach;

	// Token: 0x04001313 RID: 4883
	public transparents.dependantObjects[] DEPENDANTS;

	// Token: 0x04001314 RID: 4884
	public bool ATTACHED;

	// Token: 0x04001315 RID: 4885
	public bool IsSpring;

	// Token: 0x04001316 RID: 4886
	public bool HaveAttached;

	// Token: 0x04001317 RID: 4887
	public bool R;

	// Token: 0x04001318 RID: 4888
	public bool L;

	// Token: 0x04001319 RID: 4889
	public bool FL;

	// Token: 0x0400131A RID: 4890
	public bool FR;

	// Token: 0x0400131B RID: 4891
	public bool RL;

	// Token: 0x0400131C RID: 4892
	public bool RR;

	// Token: 0x0400131D RID: 4893
	public bool OuterTire;

	// Token: 0x0400131E RID: 4894
	public bool FRONT;

	// Token: 0x0400131F RID: 4895
	public bool REAR;

	// Token: 0x04001320 RID: 4896
	public bool BrakePadA;

	// Token: 0x04001321 RID: 4897
	public bool BrakePadB;

	// Token: 0x04001322 RID: 4898
	public bool RemovesSpringFL;

	// Token: 0x04001323 RID: 4899
	public bool RemovesSpringFR;

	// Token: 0x04001324 RID: 4900
	public bool RemovesSpringRL;

	// Token: 0x04001325 RID: 4901
	public bool RemovesSpringRR;

	// Token: 0x04001326 RID: 4902
	public bool PARENTHaveToBeREmoved;

	// Token: 0x04001327 RID: 4903
	public bool PartHaveToBeREmoved;

	// Token: 0x04001328 RID: 4904
	public bool PartHaveToBeREmovedDependsOnSide;

	// Token: 0x04001329 RID: 4905
	public GameObject PartThatNeedsToBeOff;

	// Token: 0x0400132A RID: 4906
	public GameObject PartThatNeedsToBeOffL;

	// Token: 0x0400132B RID: 4907
	public GameObject PartThatNeedsToBeOffR;

	// Token: 0x0400132C RID: 4908
	public string PartThatNeedsToBeOffname;

	// Token: 0x0400132D RID: 4909
	public string PartThatNeedsToBeOffnameL;

	// Token: 0x0400132E RID: 4910
	public string PartThatNeedsToBeOffnameR;

	// Token: 0x0400132F RID: 4911
	public bool CanAttachBecauseOfPart;

	// Token: 0x04001330 RID: 4912
	public transparents.AttachingObjects[] ATTACHABLES;

	// Token: 0x020001C4 RID: 452
	[Serializable]
	public class dependantObjects
	{
		// Token: 0x04001331 RID: 4913
		public GameObject dependant;
	}

	// Token: 0x020001C5 RID: 453
	[Serializable]
	public class AttachingObjects
	{
		// Token: 0x04001332 RID: 4914
		public GameObject Attachable;
	}
}
