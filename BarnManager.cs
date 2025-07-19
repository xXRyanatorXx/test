using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class BarnManager : MonoBehaviour
{
	// Token: 0x0600001F RID: 31 RVA: 0x000027F0 File Offset: 0x000009F0
	public void Accept()
	{
		if (tools.money >= 2000f)
		{
			tools.money -= 2000f;
			GameObject gameObject = this.BarnPosition[UnityEngine.Random.Range(0, this.BarnPosition.Length)];
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.BarnFindPrefab, gameObject.transform.position, gameObject.transform.rotation);
			gameObject2.GetComponent<SetStart>().barncarspawner.Seed = UnityEngine.Random.Range(0, 999999) + DateTime.Now.Millisecond;
			GameObject.Find("SaveManager").GetComponent<Saver>().BarnFind = gameObject2;
		}
	}

	// Token: 0x0400000D RID: 13
	public GameObject[] BarnPosition;

	// Token: 0x0400000E RID: 14
	public GameObject BarnFindPrefab;

	// Token: 0x0400000F RID: 15
	private GameObject instantiatePrefab;
}
