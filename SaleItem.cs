using System;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class SaleItem : MonoBehaviour
{
	// Token: 0x060005FD RID: 1533 RVA: 0x0002F574 File Offset: 0x0002D774
	private void Start()
	{
		this.SphereCOl = GameObject.Find("SphereCollider");
		this.AudioParent = GameObject.Find("hand");
		if (this.PaintObj1)
		{
			this.PaintObj1.GetComponent<Renderer>().material.SetColor("_Color", this.Color);
		}
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0002F5D0 File Offset: 0x0002D7D0
	public void BUY()
	{
		if (this.TimedActions == null || (this.TimedActions != null && this.TimedActions.Open))
		{
			if (this.Item && this.Item.GetComponent<MainCarProperties>())
			{
				Collider[] array = Physics.OverlapSphere(this.SpawnSpot.transform.position, 1f);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].transform.root.tag == "Vehicle")
					{
						GameObject.Find("Player").GetComponent<tools>().NoSpaceCanvas.SetActive(true);
						return;
					}
				}
			}
			if (tools.money >= this.Price && !this.GasPay && !this.BuyHouse && !this.UpgradeHouse && this.Item != null)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
				if (tools.MPrunning)
				{
					tools.NetworkPLayer.ITEM = this.Item;
					tools.NetworkPLayer.Itemname = this.Item.name;
					tools.NetworkPLayer.Spawnposition = this.SpawnSpot.transform.position;
					tools.NetworkPLayer.Spawnrotation = this.SpawnSpot.transform.rotation;
					if (this.Item.name == "SprayCan")
					{
						tools.NetworkPLayer.Networkcolorpaint = this.Color;
					}
					tools.money -= this.Price;
					if (this.Item.GetComponent<MainTrailerProperties>())
					{
						tools.NetworkPLayer.SpawnTrailer();
					}
					else
					{
						tools.NetworkPLayer.SpawnObject(0, true);
					}
				}
				else
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Item, this.SpawnSpot.transform.position, this.SpawnSpot.transform.rotation);
					gameObject.transform.name = this.Item.transform.name;
					if (gameObject.GetComponent<MainCarProperties>())
					{
						gameObject.GetComponent<MainCarProperties>().CreatingReady();
					}
					if (gameObject.GetComponent<MainTrailerProperties>())
					{
						gameObject.GetComponent<MainTrailerProperties>().JustSpawned = true;
						gameObject.GetComponent<MainTrailerProperties>().SetOwnerPlayer();
					}
					if (gameObject.GetComponent<Partinfo>() && !gameObject.GetComponent<MainCarProperties>())
					{
						gameObject.GetComponent<Partinfo>().Creating();
					}
					if (gameObject.GetComponent<PickupTool>() && gameObject.GetComponent<PickupTool>().paint)
					{
						gameObject.GetComponent<PickupTool>().colorpaint = this.Color;
					}
					if (gameObject.GetComponent<CarProperties>())
					{
						gameObject.GetComponent<CarProperties>().OriginalInterior = this.interior;
					}
					if (this.boughtMaterial)
					{
						gameObject.GetComponent<PickupTool>().previewLabel.sharedMaterial = this.boughtMaterial;
						gameObject.GetComponent<PickupTool>().material = this.boughtMaterial;
					}
					tools.money -= this.Price;
					this.SphereCOl.GetComponent<DisablerCollider>().RestartJump();
				}
			}
			if (this.GasPay && this.AllMoney > 0f)
			{
				this.AudioParent.GetComponent<AudioManager>().canplay = true;
				tools.money -= this.AllMoney;
				this.SpawnSpot.GetComponent<PickupTool>().FuelLiters.text = "";
				this.SpawnSpot.GetComponent<PickupTool>().FuelMoney.text = "";
				this.SpawnSpot.GetComponent<PickupTool>().CashReg.text = "0.00";
				this.SpawnSpot.GetComponent<PickupTool>().money = 0f;
				this.SpawnSpot2.GetComponent<PickupTool>().FuelLiters.text = "";
				this.SpawnSpot2.GetComponent<PickupTool>().FuelMoney.text = "";
				this.SpawnSpot2.GetComponent<PickupTool>().CashReg.text = "0.00";
				this.SpawnSpot2.GetComponent<PickupTool>().money = 0f;
				this.AllMoney = 0f;
				this.SpawnSpot.GetComponent<PickupTool>().liters = 0f;
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
			}
			if (tools.money >= this.Price && !this.GasPay && !this.BuyHouse && this.UpgradeHouse)
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
				tools.money -= this.Price;
				this.HouseToUpgrade.SetActive(true);
				this.HouseToDowngrade.SetActive(false);
				this.SphereCOl.GetComponent<DisablerCollider>().RestartJump();
			}
		}
	}

	// Token: 0x040008EE RID: 2286
	public GameObject SphereCOl;

	// Token: 0x040008EF RID: 2287
	public GameObject AudioParent;

	// Token: 0x040008F0 RID: 2288
	public GameObject Item;

	// Token: 0x040008F1 RID: 2289
	public float Price;

	// Token: 0x040008F2 RID: 2290
	public GameObject SpawnSpot;

	// Token: 0x040008F3 RID: 2291
	public GameObject SpawnSpot2;

	// Token: 0x040008F4 RID: 2292
	public bool GasPay;

	// Token: 0x040008F5 RID: 2293
	public bool UpgradeHouse;

	// Token: 0x040008F6 RID: 2294
	public bool BuyHouse;

	// Token: 0x040008F7 RID: 2295
	public GameObject HouseToBuy;

	// Token: 0x040008F8 RID: 2296
	public GameObject HouseToUpgrade;

	// Token: 0x040008F9 RID: 2297
	public GameObject HouseToDowngrade;

	// Token: 0x040008FA RID: 2298
	public int interior;

	// Token: 0x040008FB RID: 2299
	public Color Color;

	// Token: 0x040008FC RID: 2300
	public GameObject PaintObj1;

	// Token: 0x040008FD RID: 2301
	public TimedActions TimedActions;

	// Token: 0x040008FE RID: 2302
	public Material boughtMaterial;

	// Token: 0x040008FF RID: 2303
	public float AllMoney;
}
