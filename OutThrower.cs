using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000235 RID: 565
public class OutThrower : MonoBehaviour
{
	// Token: 0x06000D91 RID: 3473 RVA: 0x000928EC File Offset: 0x00090AEC
	private void OnEnable()
	{
		base.StartCoroutine(this.WaitStart());
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x000928FB File Offset: 0x00090AFB
	private IEnumerator WaitStart()
	{
		yield return new WaitForSeconds(2f);
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x0009290A File Offset: 0x00090B0A
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<tools>())
		{
			other.gameObject.transform.position = this.KickOutPosition.position;
		}
	}

	// Token: 0x0400161A RID: 5658
	public Transform KickOutPosition;
}
