using System;
using UnityEngine;

// Token: 0x020001E5 RID: 485
public class SellTrash : MonoBehaviour
{
	// Token: 0x06000B5A RID: 2906 RVA: 0x0007C41E File Offset: 0x0007A61E
	private void Start()
	{
		this.AudioParent = GameObject.Find("hand");
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x0007C430 File Offset: 0x0007A630
	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.transform.root.tag != "Vehicle" && col.gameObject.transform.name != "Box")
		{
			if (col.gameObject.transform.GetComponent<Partinfo>() && col.gameObject.transform.GetComponent<CarProperties>() && col.gameObject.transform.GetComponent<CarProperties>().Owner != "Client" && col.gameObject.transform.GetComponent<CarProperties>().Owner != "Multiplayer")
			{
				if (!tools.MPrunning || !col.gameObject.GetComponent<MPobject>() || (!(col.gameObject.GetComponent<MPobject>().networkDummy == null) && col.gameObject.GetComponent<MPobject>().networkDummy.hasAuthority))
				{
					if (this.BuyingParts)
					{
						this.BuyPrice = 0f;
						foreach (CarProperties carProperties in col.gameObject.transform.GetComponentsInChildren<CarProperties>())
						{
							if (carProperties.SinglePart && carProperties.Owner != "Client" && carProperties.SinglePart && carProperties.Owner != "Multiplayer")
							{
								this.BuyPrice += carProperties.gameObject.transform.GetComponent<Partinfo>().price * carProperties.Condition * 0.6f;
							}
						}
						tools.money += this.BuyPrice;
					}
					else
					{
						tools.money += col.gameObject.transform.GetComponent<Partinfo>().price * 0.1f;
					}
					if (col.gameObject.transform == col.gameObject.transform.root)
					{
						this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
					}
					if (tools.MPrunning)
					{
						foreach (MPobject mpobject in col.gameObject.transform.root.GetComponentsInChildren<MPobject>())
						{
							if (mpobject.networkDummy)
							{
								mpobject.networkDummy.DestroyMe();
							}
						}
					}
				}
				UnityEngine.Object.Destroy(col.gameObject);
			}
			if (this.BuyingParts && col.gameObject.transform.GetComponent<MooveItem>() && !col.gameObject.transform.parent)
			{
				tools.money += (float)col.gameObject.transform.GetComponent<MooveItem>().price;
				if (col.gameObject.GetComponent<networkDummy>())
				{
					col.gameObject.GetComponent<networkDummy>().DestroyMe();
				}
				UnityEngine.Object.Destroy(col.gameObject);
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
				return;
			}
			if (col.gameObject.layer == LayerMask.NameToLayer("Items") && col.gameObject != base.transform.parent.gameObject && col.gameObject.transform.name != "Collectible")
			{
				if (col.gameObject.GetComponent<networkDummy>())
				{
					col.gameObject.GetComponent<networkDummy>().DestroyMe();
				}
				UnityEngine.Object.Destroy(col.gameObject);
			}
		}
	}

	// Token: 0x040013C8 RID: 5064
	public GameObject AudioParent;

	// Token: 0x040013C9 RID: 5065
	public bool BuyingParts;

	// Token: 0x040013CA RID: 5066
	public float BuyPrice;
}
