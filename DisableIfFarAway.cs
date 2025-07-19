using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000135 RID: 309
public class DisableIfFarAway : MonoBehaviour
{
	// Token: 0x06000695 RID: 1685 RVA: 0x00035588 File Offset: 0x00033788
	private void Start()
	{
		this.itemActivatorObject = GameObject.Find("Player");
		this.activationScript = this.itemActivatorObject.GetComponent<ItemActivator>();
		base.StartCoroutine("AddToList");
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x000355B7 File Offset: 0x000337B7
	private IEnumerator AddToList()
	{
		yield return new WaitForSeconds(7f);
		this.activationScript.addList.Add(new ActivatorItem
		{
			item = base.gameObject
		});
		yield break;
	}

	// Token: 0x04000A13 RID: 2579
	private GameObject itemActivatorObject;

	// Token: 0x04000A14 RID: 2580
	private ItemActivator activationScript;
}
