using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class DecalDestroyer : MonoBehaviour
{
	// Token: 0x06000142 RID: 322 RVA: 0x0000AF77 File Offset: 0x00009177
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(this.lifeTime);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040001B2 RID: 434
	public float lifeTime = 5f;
}
