using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000273 RID: 627
public class SpawnItems : MonoBehaviour
{
	// Token: 0x06000EE6 RID: 3814 RVA: 0x0009D702 File Offset: 0x0009B902
	private void Start()
	{
		base.StartCoroutine(this.WaitStart());
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x0009D711 File Offset: 0x0009B911
	private IEnumerator WaitStart()
	{
		yield return new WaitForSeconds(0.1f);
		this.ItemList = GameObject.Find("RandomItemParent");
		this.Items = this.ItemList.GetComponent<RandomItemList>().RandomItems;
		if (UnityEngine.Random.Range(0, 20) < 8)
		{
			this.TheItem = this.Items[UnityEngine.Random.Range(0, this.Items.Length)];
			UnityEngine.Random.seed = DateTime.Now.Millisecond;
			UnityEngine.Object.Instantiate<GameObject>(this.TheItem, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z), Quaternion.identity);
		}
		yield break;
	}

	// Token: 0x04001821 RID: 6177
	public GameObject ItemList;

	// Token: 0x04001822 RID: 6178
	public GameObject[] Items;

	// Token: 0x04001823 RID: 6179
	public GameObject TheItem;
}
