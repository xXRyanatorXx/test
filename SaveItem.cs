using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class SaveItem : MonoBehaviour
{
	// Token: 0x06000AEC RID: 2796 RVA: 0x000704E4 File Offset: 0x0006E6E4
	private void Start()
	{
		if (this.ObjectName != "" && base.transform.name != "BuildingSite")
		{
			base.transform.name = this.ObjectName;
		}
		if (this.InBackpack && base.GetComponent<Rigidbody>())
		{
			UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		}
		if (base.transform.name == "BuildingSite")
		{
			base.StartCoroutine(this.wait());
		}
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0007056F File Offset: 0x0006E76F
	private IEnumerator wait()
	{
		yield return new WaitForSeconds(2f);
		foreach (Transform transform in UnityEngine.Object.FindObjectsOfType<Transform>())
		{
			if (transform.name == "BuildingSite" && transform != base.transform && transform.transform.position == base.transform.position)
			{
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		yield break;
	}

	// Token: 0x04001368 RID: 4968
	public int ObjectNumber;

	// Token: 0x04001369 RID: 4969
	public string PrefabName;

	// Token: 0x0400136A RID: 4970
	public string ObjectName;

	// Token: 0x0400136B RID: 4971
	public LiftHandle Handle;

	// Token: 0x0400136C RID: 4972
	public bool InBackpack;

	// Token: 0x0400136D RID: 4973
	public bool InBarn;

	// Token: 0x0400136E RID: 4974
	public PickupTool pickuptool;

	// Token: 0x0400136F RID: 4975
	public Renderer ChildrenRend;
}
