using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class InsideCollider : MonoBehaviour
{
	// Token: 0x06000517 RID: 1303 RVA: 0x0002A0B6 File Offset: 0x000282B6
	private void OnEnable()
	{
		base.StartCoroutine(this.WaitStart());
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x0002A0C5 File Offset: 0x000282C5
	private IEnumerator WaitStart()
	{
		yield return new WaitForSeconds(1f);
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x0002A0D4 File Offset: 0x000282D4
	private void OnTriggerEnter(Collider other)
	{
		if (base.transform.root.GetComponent<MainCarProperties>().SittingInCar && other.gameObject.GetComponent<Rigidbody>() && other.gameObject.tag != "Vehicle" && !other.gameObject.GetComponent<tools>() && !other.gameObject.GetComponent<FLUID>() && other.gameObject.transform.parent == null)
		{
			UnityEngine.Object.Destroy(other.GetComponent<Rigidbody>());
			other.transform.SetParent(this.insideitems.transform);
			this.insideitems.ObjectList.Add(other.gameObject);
		}
	}

	// Token: 0x040006D4 RID: 1748
	public InsideItems insideitems;
}
