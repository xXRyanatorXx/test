using System;
using UnityEngine;

// Token: 0x02000239 RID: 569
public class PlayerChecker : MonoBehaviour
{
	// Token: 0x06000DA0 RID: 3488 RVA: 0x00092C5D File Offset: 0x00090E5D
	private void Start()
	{
		base.GetComponent<Collider>().enabled = true;
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x00092C6B File Offset: 0x00090E6B
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Player")
		{
			this.NPC.SetActive(true);
		}
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x00092C90 File Offset: 0x00090E90
	private void OnTriggerExit(Collider other)
	{
		if (other.transform.tag == "Player")
		{
			this.NPC.SetActive(false);
		}
	}

	// Token: 0x04001627 RID: 5671
	public GameObject NPC;
}
