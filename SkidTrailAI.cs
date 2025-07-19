using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class SkidTrailAI : MonoBehaviour
{
	// Token: 0x0600007C RID: 124 RVA: 0x00007221 File Offset: 0x00005421
	private IEnumerator Start()
	{
		for (;;)
		{
			yield return null;
			if (base.transform.parent.parent == null)
			{
				UnityEngine.Object.Destroy(base.gameObject, this.persistTime);
			}
		}
		yield break;
	}

	// Token: 0x04000101 RID: 257
	[SerializeField]
	public float persistTime;
}
