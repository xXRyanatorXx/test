using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000280 RID: 640
public class TurnOffSoon : MonoBehaviour
{
	// Token: 0x06000F13 RID: 3859 RVA: 0x0009DED2 File Offset: 0x0009C0D2
	private void OnEnable()
	{
		base.StartCoroutine(this.Wait());
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x0009DEE1 File Offset: 0x0009C0E1
	private IEnumerator Wait()
	{
		yield return new WaitForSeconds(1f);
		base.gameObject.SetActive(false);
		yield break;
	}
}
