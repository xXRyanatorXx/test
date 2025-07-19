using System;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class ChromeParts : MonoBehaviour
{
	// Token: 0x060000C5 RID: 197 RVA: 0x00008A2C File Offset: 0x00006C2C
	private void Start()
	{
		this.AudioParent = GameObject.Find("hand");
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00008A40 File Offset: 0x00006C40
	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.transform.root.tag != "Vehicle" && col.gameObject.transform.name != "Box" && col.gameObject.transform.GetComponent<CarProperties>() && col.gameObject.transform.GetComponent<CarProperties>().Owner != "Client" && col.gameObject.transform.GetComponent<CarProperties>().Owner != "Multiplayer")
		{
			if (col.gameObject.transform.GetComponent<CarProperties>().PREFAB && col.gameObject.transform.GetComponent<CarProperties>().PREFAB.GetComponent<Partinfo>().Engine)
			{
				this.Price = 200f;
			}
			else
			{
				this.Price = 300f;
			}
			if (tools.money >= this.Price && col.gameObject.transform.GetComponent<CarProperties>().PREFAB.GetComponent<CarProperties>().ChromeMat && col.gameObject.transform.GetComponent<CarProperties>().Condition > 0.4f && !col.gameObject.transform.GetComponent<CarProperties>().Chromed)
			{
				tools.money -= this.Price;
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
				col.gameObject.transform.GetComponent<CarProperties>().ApplyChrome();
			}
		}
	}

	// Token: 0x04000144 RID: 324
	public GameObject AudioParent;

	// Token: 0x04000145 RID: 325
	public float Price;
}
