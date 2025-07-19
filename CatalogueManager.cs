using System;
using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;

// Token: 0x0200012F RID: 303
public class CatalogueManager : MonoBehaviour
{
	// Token: 0x06000661 RID: 1633 RVA: 0x000338C8 File Offset: 0x00031AC8
	private void Start()
	{
		this.AudioParent = GameObject.Find("hand");
		this.Engbutt.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Engine");
		this.Allcarsbutt.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("AllCars");
		this.Colorbutt.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Color");
		this.Brakesbutt.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Brakes");
		this.Suspbutt.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Suspension");
		this.Bodybutt.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Body");
		this.interbutt.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Interior");
		this.accessbutt.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Accessories");
		this.windbutt.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Windows");
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x000339D0 File Offset: 0x00031BD0
	public void ChangeCar()
	{
		this.ActiveCar = this.CarDropdown.options[this.CarDropdown.value].text;
		this.ActiveEngine = this.EngineDropdown.options[this.EngineDropdown.value].text;
		if (this.ActiveType == "Suspension")
		{
			this.CheckSuspension();
		}
		if (this.ActiveType == "Engine")
		{
			this.CheckEngine();
		}
		if (this.ActiveType == "BodyPanel")
		{
			this.CheckBodyPanel();
		}
		if (this.ActiveType == "Interior")
		{
			this.CheckInterior();
		}
		if (this.ActiveType == "Accessories")
		{
			this.CheckAccessories();
		}
		if (this.ActiveType == "Window")
		{
			this.CheckWindow();
		}
		if (this.ActiveType == "Rim")
		{
			this.CheckRim();
		}
		if (this.ActiveType == "Tire")
		{
			this.CheckTire();
		}
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00033AEC File Offset: 0x00031CEC
	public void Search(string input)
	{
		this.ActiveCar = this.CarDropdown.options[this.CarDropdown.value].text;
		this.ActiveEngine = this.EngineDropdown.options[this.EngineDropdown.value].text;
		foreach (SHOPitem shopitem in base.GetComponentsInChildren<SHOPitem>(true))
		{
			this.found = false;
			shopitem.gameObject.SetActive(true);
			if (this.ActiveCar == "" || this.ActiveCar == "All Cars")
			{
				this.found = true;
			}
			else
			{
				string[] array = shopitem.ITEM.GetComponent<Partinfo>().FitsToCar;
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] == this.ActiveCar)
					{
						this.found = true;
					}
				}
			}
			if (this.found && !(this.ActiveEngine == "") && !(this.ActiveEngine == "All Engines"))
			{
				this.found = false;
				string[] array = shopitem.ITEM.GetComponent<Partinfo>().FitsToEngine;
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] == this.ActiveEngine)
					{
						this.found = true;
					}
				}
				if (shopitem.ITEM.GetComponent<Partinfo>().FitsToEngine.Length == 0)
				{
					this.found = true;
				}
			}
			if (this.found && (!shopitem.ITEM.GetComponent<CarProperties>() || string.IsNullOrWhiteSpace(shopitem.ITEM.GetComponent<CarProperties>().PartName) || shopitem.ITEM.GetComponent<CarProperties>().PartName.IndexOf(input, StringComparison.OrdinalIgnoreCase) < 0) && shopitem.ITEM.name.IndexOf(input, StringComparison.OrdinalIgnoreCase) < 0)
			{
				this.found = false;
			}
			if (!this.found)
			{
				shopitem.gameObject.SetActive(false);
			}
			this.found = false;
		}
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x00033CF0 File Offset: 0x00031EF0
	public void CheckSuspension()
	{
		this.ActiveType = "Suspension";
		foreach (SHOPitem shopitem in base.GetComponentsInChildren<SHOPitem>(true))
		{
			this.found = false;
			shopitem.gameObject.SetActive(true);
			if (!shopitem.ITEM.GetComponent<Partinfo>().Suspension)
			{
				shopitem.gameObject.SetActive(false);
			}
			if (!(this.ActiveCar == "") && !(this.ActiveCar == "All Cars"))
			{
				string[] fitsToCar = shopitem.ITEM.GetComponent<Partinfo>().FitsToCar;
				for (int j = 0; j < fitsToCar.Length; j++)
				{
					if (fitsToCar[j] == this.ActiveCar)
					{
						this.found = true;
					}
				}
				if (!this.found)
				{
					shopitem.gameObject.SetActive(false);
				}
			}
			this.found = false;
		}
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00033DD4 File Offset: 0x00031FD4
	public void CheckBrakes()
	{
		this.ActiveType = "Brakes";
		foreach (SHOPitem shopitem in base.GetComponentsInChildren<SHOPitem>(true))
		{
			this.found = false;
			shopitem.gameObject.SetActive(true);
			if (!shopitem.ITEM.GetComponent<Partinfo>().Brakes)
			{
				shopitem.gameObject.SetActive(false);
			}
			if (!(this.ActiveCar == "") && !(this.ActiveCar == "All Cars"))
			{
				string[] fitsToCar = shopitem.ITEM.GetComponent<Partinfo>().FitsToCar;
				for (int j = 0; j < fitsToCar.Length; j++)
				{
					if (fitsToCar[j] == this.ActiveCar)
					{
						this.found = true;
					}
				}
				if (!this.found)
				{
					shopitem.gameObject.SetActive(false);
				}
			}
			this.found = false;
		}
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00033EB8 File Offset: 0x000320B8
	public void CheckEngine()
	{
		this.ActiveType = "Engine";
		foreach (SHOPitem shopitem in base.GetComponentsInChildren<SHOPitem>(true))
		{
			this.found = false;
			shopitem.gameObject.SetActive(true);
			if (!shopitem.ITEM.GetComponent<Partinfo>().Engine)
			{
				shopitem.gameObject.SetActive(false);
			}
			if (this.ActiveCar == "" || this.ActiveCar == "All Cars")
			{
				this.found = true;
			}
			else
			{
				string[] array = shopitem.ITEM.GetComponent<Partinfo>().FitsToCar;
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] == this.ActiveCar)
					{
						this.found = true;
					}
				}
			}
			if (this.found && !(this.ActiveEngine == "") && !(this.ActiveEngine == "All Engines"))
			{
				this.found = false;
				string[] array = shopitem.ITEM.GetComponent<Partinfo>().FitsToEngine;
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] == this.ActiveEngine)
					{
						this.found = true;
					}
				}
				if (shopitem.ITEM.GetComponent<Partinfo>().FitsToEngine.Length == 0)
				{
					this.found = true;
				}
			}
			if (!this.found)
			{
				shopitem.gameObject.SetActive(false);
			}
			this.found = false;
		}
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00034030 File Offset: 0x00032230
	public void CheckBodyPanel()
	{
		this.ActiveType = "BodyPanel";
		foreach (SHOPitem shopitem in base.GetComponentsInChildren<SHOPitem>(true))
		{
			this.found = false;
			shopitem.gameObject.SetActive(true);
			if (!shopitem.ITEM.GetComponent<Partinfo>().BodyPanel)
			{
				shopitem.gameObject.SetActive(false);
			}
			if (!(this.ActiveCar == "") && !(this.ActiveCar == "All Cars"))
			{
				string[] fitsToCar = shopitem.ITEM.GetComponent<Partinfo>().FitsToCar;
				for (int j = 0; j < fitsToCar.Length; j++)
				{
					if (fitsToCar[j] == this.ActiveCar)
					{
						this.found = true;
					}
				}
				if (!this.found)
				{
					shopitem.gameObject.SetActive(false);
				}
			}
			this.found = false;
		}
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00034114 File Offset: 0x00032314
	public void CheckInterior()
	{
		this.ActiveType = "Interior";
		foreach (SHOPitem shopitem in base.GetComponentsInChildren<SHOPitem>(true))
		{
			this.found = false;
			shopitem.gameObject.SetActive(true);
			if (!shopitem.ITEM.GetComponent<Partinfo>().Interior)
			{
				shopitem.gameObject.SetActive(false);
			}
			if (!(this.ActiveCar == "") && !(this.ActiveCar == "All Cars"))
			{
				string[] fitsToCar = shopitem.ITEM.GetComponent<Partinfo>().FitsToCar;
				for (int j = 0; j < fitsToCar.Length; j++)
				{
					if (fitsToCar[j] == this.ActiveCar)
					{
						this.found = true;
					}
				}
				if (!this.found)
				{
					shopitem.gameObject.SetActive(false);
				}
			}
			this.found = false;
		}
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x000341F8 File Offset: 0x000323F8
	public void CheckAccessories()
	{
		this.ActiveType = "Accessories";
		foreach (SHOPitem shopitem in base.GetComponentsInChildren<SHOPitem>(true))
		{
			this.found = false;
			shopitem.gameObject.SetActive(true);
			if (!shopitem.ITEM.GetComponent<Partinfo>().Accessories)
			{
				shopitem.gameObject.SetActive(false);
			}
			if (!(this.ActiveCar == "") && !(this.ActiveCar == "All Cars"))
			{
				string[] fitsToCar = shopitem.ITEM.GetComponent<Partinfo>().FitsToCar;
				for (int j = 0; j < fitsToCar.Length; j++)
				{
					if (fitsToCar[j] == this.ActiveCar)
					{
						this.found = true;
					}
				}
				if (!this.found)
				{
					shopitem.gameObject.SetActive(false);
				}
			}
			this.found = false;
		}
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x000342DC File Offset: 0x000324DC
	public void CheckWindow()
	{
		this.ActiveType = "Window";
		foreach (SHOPitem shopitem in base.GetComponentsInChildren<SHOPitem>(true))
		{
			this.found = false;
			shopitem.gameObject.SetActive(true);
			if (!shopitem.ITEM.GetComponent<Partinfo>().Window)
			{
				shopitem.gameObject.SetActive(false);
			}
			if (!(this.ActiveCar == "") && !(this.ActiveCar == "All Cars"))
			{
				string[] fitsToCar = shopitem.ITEM.GetComponent<Partinfo>().FitsToCar;
				for (int j = 0; j < fitsToCar.Length; j++)
				{
					if (fitsToCar[j] == this.ActiveCar)
					{
						this.found = true;
					}
				}
				if (!this.found)
				{
					shopitem.gameObject.SetActive(false);
				}
			}
			this.found = false;
		}
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x000343C0 File Offset: 0x000325C0
	public void CheckRim()
	{
		this.ActiveType = "Rim";
		foreach (SHOPitem shopitem in base.GetComponentsInChildren<SHOPitem>(true))
		{
			shopitem.gameObject.SetActive(true);
			if (!shopitem.ITEM.GetComponent<Partinfo>().Rim)
			{
				shopitem.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x0003441C File Offset: 0x0003261C
	public void CheckTire()
	{
		this.ActiveType = "Tire";
		foreach (SHOPitem shopitem in base.GetComponentsInChildren<SHOPitem>(true))
		{
			shopitem.gameObject.SetActive(true);
			if (!shopitem.ITEM.GetComponent<Partinfo>().Tire)
			{
				shopitem.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00034478 File Offset: 0x00032678
	public void BuyColor()
	{
		if (tools.money >= this.Price)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
			if (tools.MPrunning)
			{
				tools.NetworkPLayer.ITEM = this.Item;
				tools.NetworkPLayer.Itemname = this.Item.transform.name;
				tools.NetworkPLayer.Spawnposition = tools.SpawnSpot.transform.position;
				tools.NetworkPLayer.Spawnrotation = Quaternion.identity;
				tools.NetworkPLayer.Networkcolorpaint = this.FCP.color;
				tools.NetworkPLayer.SpawnObject(0, true);
			}
			else
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Item, tools.SpawnSpot.transform.position, Quaternion.identity);
				gameObject.transform.name = this.Item.transform.name;
				if (gameObject.GetComponent<PickupTool>() && gameObject.GetComponent<PickupTool>().paint)
				{
					gameObject.GetComponent<PickupTool>().colorpaint = this.FCP.color;
				}
			}
			tools.money -= this.Price;
		}
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x000345B4 File Offset: 0x000327B4
	public void BuyColorPen()
	{
		if (tools.money >= this.Price)
		{
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
			if (tools.MPrunning)
			{
				tools.NetworkPLayer.ITEM = this.PaintPen;
				tools.NetworkPLayer.Itemname = this.Item.transform.name;
				tools.NetworkPLayer.Spawnposition = tools.SpawnSpot.transform.position;
				tools.NetworkPLayer.Spawnrotation = Quaternion.identity;
				tools.NetworkPLayer.Networkcolorpaint = this.FCP.color;
				tools.NetworkPLayer.SpawnObject(0, true);
			}
			else
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PaintPen, tools.SpawnSpot.transform.position, Quaternion.identity);
				gameObject.transform.name = this.PaintPen.transform.name;
				if (gameObject.GetComponent<PickupTool>() && gameObject.GetComponent<PickupTool>().paint)
				{
					gameObject.GetComponent<PickupTool>().colorpaint = this.FCP.color;
				}
			}
			tools.money -= this.Price;
		}
	}

	// Token: 0x040009C7 RID: 2503
	public GameObject Engbutt;

	// Token: 0x040009C8 RID: 2504
	public GameObject Allcarsbutt;

	// Token: 0x040009C9 RID: 2505
	public GameObject Colorbutt;

	// Token: 0x040009CA RID: 2506
	public GameObject Brakesbutt;

	// Token: 0x040009CB RID: 2507
	public GameObject Suspbutt;

	// Token: 0x040009CC RID: 2508
	public GameObject Bodybutt;

	// Token: 0x040009CD RID: 2509
	public GameObject interbutt;

	// Token: 0x040009CE RID: 2510
	public GameObject accessbutt;

	// Token: 0x040009CF RID: 2511
	public GameObject windbutt;

	// Token: 0x040009D0 RID: 2512
	public GameObject CarMenu;

	// Token: 0x040009D1 RID: 2513
	public GameObject ItemCardPrefab;

	// Token: 0x040009D2 RID: 2514
	public GameObject[] parts;

	// Token: 0x040009D3 RID: 2515
	private bool found;

	// Token: 0x040009D4 RID: 2516
	public GameObject PartList;

	// Token: 0x040009D5 RID: 2517
	public GameObject Part;

	// Token: 0x040009D6 RID: 2518
	public GameObject Item;

	// Token: 0x040009D7 RID: 2519
	public GameObject PaintPen;

	// Token: 0x040009D8 RID: 2520
	public FlexibleColorPicker FCP;

	// Token: 0x040009D9 RID: 2521
	public GameObject SpawnSpot;

	// Token: 0x040009DA RID: 2522
	public float Price;

	// Token: 0x040009DB RID: 2523
	public GameObject AudioParent;

	// Token: 0x040009DC RID: 2524
	public GameObject dropdown;

	// Token: 0x040009DD RID: 2525
	public TMP_Dropdown CarDropdown;

	// Token: 0x040009DE RID: 2526
	public TMP_Dropdown EngineDropdown;

	// Token: 0x040009DF RID: 2527
	public string ActiveType;

	// Token: 0x040009E0 RID: 2528
	public string ActiveCar;

	// Token: 0x040009E1 RID: 2529
	public string ActiveEngine;
}
