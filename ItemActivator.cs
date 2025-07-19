using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class ItemActivator : MonoBehaviour
{
	// Token: 0x060006EB RID: 1771 RVA: 0x0003737C File Offset: 0x0003557C
	private void Start()
	{
		this.player = GameObject.Find("Player");
		this.activatorItems = new List<ActivatorItem>();
		this.addList = new List<ActivatorItem>();
		this.AddToList();
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x000373AC File Offset: 0x000355AC
	private void AddToList()
	{
		if (this.addList.Count > 0)
		{
			foreach (ActivatorItem activatorItem in this.addList)
			{
				if (activatorItem.item != null)
				{
					this.activatorItems.Add(activatorItem);
				}
			}
			this.addList.Clear();
		}
		base.StartCoroutine("CheckActivation");
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x00037438 File Offset: 0x00035638
	private IEnumerator CheckActivation()
	{
		List<ActivatorItem> removeList = new List<ActivatorItem>();
		if (this.activatorItems.Count > 0)
		{
			foreach (ActivatorItem activatorItem in this.activatorItems)
			{
				if (Vector3.Distance(this.player.transform.position, activatorItem.item.transform.position) > (float)this.distanceFromPlayer)
				{
					if (activatorItem.item == null)
					{
						removeList.Add(activatorItem);
					}
					else
					{
						activatorItem.item.SetActive(false);
					}
				}
				else if (activatorItem.item == null)
				{
					removeList.Add(activatorItem);
				}
				else if (!activatorItem.item.activeSelf)
				{
					activatorItem.item.SetActive(true);
					ParticleSystem[] componentsInChildren = activatorItem.item.GetComponentsInChildren<ParticleSystem>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].Stop();
					}
				}
				yield return new WaitForSeconds(0.01f);
			}
			List<ActivatorItem>.Enumerator enumerator = default(List<ActivatorItem>.Enumerator);
		}
		yield return new WaitForSeconds(0.01f);
		if (removeList.Count > 0)
		{
			foreach (ActivatorItem item in removeList)
			{
				this.activatorItems.Remove(item);
			}
		}
		yield return new WaitForSeconds(0.01f);
		this.AddToList();
		yield break;
		yield break;
	}

	// Token: 0x04000A73 RID: 2675
	[SerializeField]
	private int distanceFromPlayer;

	// Token: 0x04000A74 RID: 2676
	private GameObject player;

	// Token: 0x04000A75 RID: 2677
	private List<ActivatorItem> activatorItems;

	// Token: 0x04000A76 RID: 2678
	public List<ActivatorItem> addList;
}
