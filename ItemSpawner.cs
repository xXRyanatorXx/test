using System;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public class ItemSpawner : MonoBehaviour
{
	// Token: 0x0600052B RID: 1323 RVA: 0x0002A350 File Offset: 0x00028550
	private void OnEnable()
	{
		if (UnityEngine.Random.Range(0f, 1f) <= 0.7f)
		{
			if (this.ItemList == null)
			{
				this.ItemList = GameObject.Find("ItemParent");
			}
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
				gameObject.GetComponent<PickupTool>().NestedFluid.FluidSize = 20f;
			}
		}
	}

	// Token: 0x040006DC RID: 1756
	public GameObject ItemList;

	// Token: 0x040006DD RID: 1757
	public GameObject Item;
}
