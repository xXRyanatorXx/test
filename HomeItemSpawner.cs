using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class HomeItemSpawner : MonoBehaviour
{
	// Token: 0x06000510 RID: 1296 RVA: 0x00029EFC File Offset: 0x000280FC
	private void OnEnable()
	{
		if (UnityEngine.Random.Range(0f, 1f) <= 0.7f)
		{
			this.ItemList = GameObject.Find("HomeItemParent");
			this.Item = this.ItemList.GetComponent<JunkPartsList>().Parts[UnityEngine.Random.Range(0, this.ItemList.GetComponent<JunkPartsList>().Parts.Length)];
			if (tools.MPrunning)
			{
				int num = (int)(base.transform.position.x - base.transform.root.position.x);
				int seed = DateTime.Now.Millisecond + num;
				tools.NetworkPLayer.ITEM = this.Item;
				tools.NetworkPLayer.Spawnposition = base.transform.position;
				tools.NetworkPLayer.SpawnObject(seed, false);
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Item, base.transform.position, Quaternion.identity);
			gameObject.transform.name = this.Item.transform.name;
			if (gameObject.transform.name == "Fuelcan")
			{
				gameObject.GetComponent<PickupTool>().NestedFluid.FluidSize = 8f;
			}
		}
	}

	// Token: 0x040006CF RID: 1743
	public GameObject ItemList;

	// Token: 0x040006D0 RID: 1744
	public GameObject Item;
}
