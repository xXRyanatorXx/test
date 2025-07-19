using System;
using System.Collections;
using Assets.SimpleLocalization;
using PaintIn3D;
using RVP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200012A RID: 298
public class CarInformation : MonoBehaviour
{
	// Token: 0x06000644 RID: 1604 RVA: 0x000319E3 File Offset: 0x0002FBE3
	public void ChangeColor()
	{
		this.MainProperties.Color = this.FCP.color;
		this.ReStart(false);
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x00031A04 File Offset: 0x0002FC04
	public void BuySell()
	{
		if (!float.IsNaN(this.PRICE))
		{
			this.ReStart(this.dealerprice);
			this.Car.GetComponent<VehicleDamage>().Start();
			if (this.MainProperties.Owner == "Player")
			{
				if (!float.IsNaN(this.PRICE))
				{
					tools.money += this.PRICE;
				}
				foreach (Transform transform in this.Car.GetComponentsInChildren<Transform>())
				{
					if (transform.name == "TrailerHandle" && transform.GetComponent<PickupHand>())
					{
						transform.GetComponent<PickupHand>().CarSold();
					}
					if (transform.name == "Hook" && transform.GetComponent<WinchHook>())
					{
						transform.GetComponent<WinchHook>().RemoveFromHand();
					}
					if (transform.name == "BikeStand" && transform.GetComponent<PickupHand>())
					{
						transform.GetComponent<PickupHand>().CarSold();
					}
					if (transform.GetComponent<MPobject>() && transform.GetComponent<MPobject>().networkDummy)
					{
						transform.GetComponent<MPobject>().networkDummy.DestroyMe();
					}
				}
				UnityEngine.Object.Destroy(this.Car);
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
				this.CursorCanvas.SetActive(true);
				GameObject.Find("Player").GetComponent<FirstPersonAIO>().ControllerUnPause();
				base.gameObject.SetActive(false);
			}
			if (this.MainProperties.Owner == "None")
			{
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
				this.MainProperties.SetOwnerPlayer();
				this.CursorCanvas.SetActive(true);
				GameObject.Find("Player").GetComponent<FirstPersonAIO>().ControllerUnPause();
				base.gameObject.SetActive(false);
			}
			if ((this.MainProperties.Owner == "Junkyard" || this.MainProperties.Owner == "Dealer") && tools.money >= this.PRICE && this.PRICE != 0f)
			{
				bool flag = false;
				if (this.MainProperties.Owner == "Junkyard")
				{
					foreach (GameObject gameObject in this.JunkSpawns)
					{
						flag = true;
						this.ThePoint = gameObject;
						Collider[] array2 = Physics.OverlapSphere(gameObject.transform.position, 1f);
						for (int j = 0; j < array2.Length; j++)
						{
							if (array2[j].transform.root.tag == "Vehicle")
							{
								flag = false;
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				else
				{
					foreach (GameObject gameObject2 in this.DealerSpawns)
					{
						flag = true;
						this.ThePoint = gameObject2;
						Collider[] array2 = Physics.OverlapSphere(gameObject2.transform.position, 1f);
						for (int j = 0; j < array2.Length; j++)
						{
							if (array2[j].transform.root.tag == "Vehicle")
							{
								flag = false;
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				if (flag)
				{
					tools.money -= this.PRICE;
					if (tools.MPrunning)
					{
						tools.NetworkPLayer.pickup(this.Car.transform.GetComponent<MPobject>().networkDummy);
						base.StartCoroutine(this.CarBuyContinue());
						return;
					}
					this.BuyCOntin();
					return;
				}
				else
				{
					this.NoSpaceCanvas.SetActive(true);
				}
			}
		}
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x00031DC4 File Offset: 0x0002FFC4
	public void BuyCOntin()
	{
		foreach (Transform transform in this.Car.GetComponentsInChildren<Transform>())
		{
			if (transform.name == "TrailerHandle" && transform.GetComponent<PickupHand>())
			{
				transform.GetComponent<PickupHand>().CarSold();
			}
			if (transform.name == "Hook" && transform.GetComponent<WinchHook>())
			{
				transform.GetComponent<WinchHook>().RemoveFromHand();
			}
		}
		this.Car.transform.position = this.ThePoint.transform.position;
		this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
		this.MainProperties.SetOwnerPlayer();
		this.CursorCanvas.SetActive(true);
		GameObject.Find("Player").GetComponent<FirstPersonAIO>().ControllerUnPause();
		base.gameObject.SetActive(false);
		if (tools.MPrunning)
		{
			this.Car.GetComponent<MPobject>().networkDummy.SetOwnerPlayer();
		}
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x00031ED6 File Offset: 0x000300D6
	private IEnumerator CarBuyContinue()
	{
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		foreach (Transform transform in this.Car.GetComponentsInChildren<Transform>())
		{
			if (transform.name == "TrailerHandle" && transform.GetComponent<PickupHand>())
			{
				transform.GetComponent<PickupHand>().CarSold();
			}
			if (transform.name == "Hook" && transform.GetComponent<WinchHook>())
			{
				transform.GetComponent<WinchHook>().RemoveFromHand();
			}
		}
		this.Car.transform.position = this.ThePoint.transform.position;
		this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
		this.MainProperties.SetOwnerPlayer();
		this.CursorCanvas.SetActive(true);
		GameObject.Find("Player").GetComponent<FirstPersonAIO>().ControllerUnPause();
		base.gameObject.SetActive(false);
		if (tools.MPrunning)
		{
			this.Car.GetComponent<MPobject>().networkDummy.SetOwnerPlayer();
		}
		yield break;
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x00031EE8 File Offset: 0x000300E8
	public void TowToGarage()
	{
		bool flag = false;
		foreach (GameObject gameObject in this.GarageSpawns)
		{
			flag = true;
			this.ThePoint = gameObject;
			Collider[] array = Physics.OverlapSphere(gameObject.transform.position, 1f);
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].transform.root.tag == "Vehicle")
				{
					flag = false;
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (flag)
		{
			if (this.Car.GetComponent<HingeJoint>())
			{
				UnityEngine.Object.Destroy(this.Car.GetComponent<HingeJoint>());
			}
			foreach (Transform transform in this.Car.GetComponentsInChildren<Transform>())
			{
				if (transform.name == "TrailerHandle" && transform.GetComponent<PickupHand>())
				{
					transform.GetComponent<PickupHand>().CarSold();
				}
				if (transform.name == "Hook" && transform.GetComponent<WinchHook>())
				{
					transform.GetComponent<WinchHook>().RemoveFromHand();
				}
			}
			this.Car.transform.position = this.ThePoint.transform.position;
			tools.money -= 100f;
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
			return;
		}
		this.NoSpaceCanvas.SetActive(true);
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0003206C File Offset: 0x0003026C
	public void TowToSign()
	{
		bool flag = true;
		this.ThePoint = this.SignSpawn;
		Collider[] array = Physics.OverlapSphere(this.SignSpawn.transform.position, 1f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].transform.root.tag == "Vehicle")
			{
				flag = false;
			}
		}
		if (flag)
		{
			if (this.Car.GetComponent<HingeJoint>())
			{
				UnityEngine.Object.Destroy(this.Car.GetComponent<HingeJoint>());
			}
			foreach (Transform transform in this.Car.GetComponentsInChildren<Transform>())
			{
				if (transform.name == "TrailerHandle" && transform.GetComponent<PickupHand>())
				{
					transform.GetComponent<PickupHand>().CarSold();
				}
				if (transform.name == "Hook" && transform.GetComponent<WinchHook>())
				{
					transform.GetComponent<WinchHook>().RemoveFromHand();
				}
			}
			this.Car.transform.position = this.SignSpawn.transform.position;
			this.Car.transform.rotation = this.SignSpawn.transform.rotation;
			tools.money -= 100f;
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
			return;
		}
		this.NoSpaceCanvas.SetActive(true);
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x000321F0 File Offset: 0x000303F0
	public void TowToHouse()
	{
		bool flag = false;
		foreach (GameObject gameObject in this.HouseSpawns)
		{
			flag = true;
			this.ThePoint = gameObject;
			Collider[] array = Physics.OverlapSphere(gameObject.transform.position, 1f);
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].transform.root.tag == "Vehicle")
				{
					flag = false;
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (flag)
		{
			if (this.Car.GetComponent<HingeJoint>())
			{
				UnityEngine.Object.Destroy(this.Car.GetComponent<HingeJoint>());
			}
			foreach (Transform transform in this.Car.GetComponentsInChildren<Transform>())
			{
				if (transform.name == "TrailerHandle" && transform.GetComponent<PickupHand>())
				{
					transform.GetComponent<PickupHand>().CarSold();
				}
				if (transform.name == "Hook" && transform.GetComponent<WinchHook>())
				{
					transform.GetComponent<WinchHook>().RemoveFromHand();
				}
			}
			this.Car.transform.position = this.ThePoint.transform.position;
			tools.money -= 100f;
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
			return;
		}
		this.NoSpaceCanvas.SetActive(true);
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x00032374 File Offset: 0x00030574
	public void TowToService()
	{
		if (tools.money < 500f)
		{
			return;
		}
		bool flag = true;
		Collider[] array = Physics.OverlapSphere(this.ServiceSpawn.transform.position, 1f);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].transform.root.tag == "Vehicle")
			{
				flag = false;
			}
		}
		if (flag)
		{
			if (this.Car.GetComponent<HingeJoint>())
			{
				UnityEngine.Object.Destroy(this.Car.GetComponent<HingeJoint>());
			}
			foreach (Transform transform in this.Car.GetComponentsInChildren<Transform>())
			{
				if (transform.name == "TrailerHandle" && transform.GetComponent<PickupHand>())
				{
					transform.GetComponent<PickupHand>().CarSold();
				}
				if (transform.name == "Hook" && transform.GetComponent<WinchHook>())
				{
					transform.GetComponent<WinchHook>().RemoveFromHand();
				}
			}
			this.Car.transform.SetPositionAndRotation(this.ServiceSpawn.transform.position, this.ServiceSpawn.transform.rotation);
			tools.money -= 500f;
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
			return;
		}
		this.NoSpaceCanvas.SetActive(true);
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x000324EC File Offset: 0x000306EC
	public void ReStart(bool dealership)
	{
		this.dealerprice = dealership;
		this.AudioParent = GameObject.Find("hand");
		this.MainProperties = this.Car.GetComponent<MainCarProperties>();
		RuntimePreviewGenerator.BackgroundColor = new Color(0f, 0f, 0f, 0f);
		this.ItemPictureParent.transform.GetComponent<Image>().sprite = Sprite.Create(RuntimePreviewGenerator.GenerateModelPreview(this.Car.transform, 500, 500, false), new Rect(0f, 0f, 500f, 500f), new Vector2(0.5f, 0.5f), 100f);
		this.CarModel.GetComponent<TextMeshProUGUI>().text = this.MainProperties.CarName;
		this.OwnerName.GetComponent<TextMeshProUGUI>().text = this.MainProperties.Owner;
		this.OriginalCarColor.color = this.MainProperties.OriginalColor;
		this.OriginalColorName.GetComponent<TextMeshProUGUI>().text = "#" + ColorUtility.ToHtmlStringRGB(this.MainProperties.OriginalColor);
		this.CarColor.color = this.MainProperties.Color;
		this.ColorName.GetComponent<TextMeshProUGUI>().text = "#" + ColorUtility.ToHtmlStringRGB(this.MainProperties.Color);
		if (this.MainProperties.EngineBlock)
		{
			this.EngineName.GetComponent<TextMeshProUGUI>().text = this.MainProperties.EngineBlock.PartNameExtension.ToString();
		}
		if (this.MainProperties.Gearbox)
		{
			this.GearboxName.GetComponent<TextMeshProUGUI>().text = this.MainProperties.Gearbox.PartNameExtension.ToString();
		}
		if (this.MainProperties.Differential)
		{
			this.DiffName.GetComponent<TextMeshProUGUI>().text = this.MainProperties.Differential.PartNameExtension.ToString();
		}
		if (this.Mileage != null && !this.MainProperties.Bike)
		{
			this.Mileage.text = "Mileage                  " + this.MainProperties.Mileage.ToString("F0") + "Km";
		}
		else if (this.Mileage != null)
		{
			this.Mileage.text = "";
		}
		if (this.Interior != null)
		{
			this.Interior.text = this.MainProperties.OriginalInteriorColor;
		}
		this.PRICEv2 = 0f;
		this.allparts = 0;
		this.existingparts = 0;
		foreach (transparents transparents in this.Car.GetComponentsInChildren<transparents>())
		{
			if (!transparents.NotImportantPart)
			{
				this.allparts++;
			}
			if (!transparents.NotImportantPart && transparents.HaveAttached)
			{
				this.existingparts++;
			}
		}
		if (this.existingparts >= this.allparts)
		{
			this.PartCount.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("No Missing Parts");
		}
		if (this.existingparts < this.allparts)
		{
			this.PartCount.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Have Missing Parts");
		}
		this.BuyButton.SetActive(true);
		this.PriceToDisable.SetActive(true);
		this.JobsButton.SetActive(false);
		if (this.MainProperties.Owner == "Player")
		{
			this.BuyButtonText.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Sell");
			this.PaintButton.SetActive(true);
		}
		if (this.MainProperties.Owner == "None")
		{
			this.BuyButtonText.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Claim");
			this.PaintButton.SetActive(false);
		}
		if (this.MainProperties.Owner == "Junkyard" || this.MainProperties.Owner == "Dealer")
		{
			this.BuyButtonText.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Buy");
			this.PaintButton.SetActive(false);
		}
		if (this.MainProperties.Owner == "Client")
		{
			this.BuyButton.SetActive(false);
			this.PaintButton.SetActive(false);
			this.PriceToDisable.SetActive(false);
			this.JobsButton.SetActive(true);
		}
		if (this.MainProperties.Owner == "Multiplayer")
		{
			this.BuyButton.SetActive(false);
			this.PaintButton.SetActive(false);
			this.PriceToDisable.SetActive(false);
		}
		base.StartCoroutine(this.CheckPaints());
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x000329C6 File Offset: 0x00030BC6
	private IEnumerator CheckPaints()
	{
		P3dChangeCounter[] targetLis = this.Car.GetComponentsInChildren<P3dChangeCounter>();
		foreach (P3dChangeCounter p3dChangeCounter in targetLis)
		{
			p3dChangeCounter.enabled = true;
			if (p3dChangeCounter.gameObject.GetComponent<CarProperties>().Paintable && p3dChangeCounter.Threshold == 0.1f)
			{
				p3dChangeCounter.changeDirty = true;
				p3dChangeCounter.Color = this.MainProperties.Color;
			}
		}
		yield return 0;
		yield return 0;
		yield return 0;
		this.CleanRatio = 0.1f;
		this.NoRustRatio = 0.1f;
		this.PaintRatio = 0.1f;
		this.CleanRatioParts = 0f;
		this.NoRustRatioParts = 0f;
		this.PaintRatioParts = 0f;
		this.PaintGoodParts = 0f;
		this.RustGoodParts = 0f;
		foreach (P3dChangeCounter p3dChangeCounter2 in targetLis)
		{
			float num = 1f - p3dChangeCounter2.Ratio;
			if (p3dChangeCounter2.gameObject.GetComponent<CarProperties>().Washable && p3dChangeCounter2.Threshold == 0.7f)
			{
				if ((double)num > 0.6)
				{
					this.CleanRatio += 1f;
				}
				this.CleanRatioParts += 1f;
				p3dChangeCounter2.gameObject.GetComponent<CarProperties>().CleanRatio = num;
			}
			if (p3dChangeCounter2.gameObject.GetComponent<CarProperties>().Paintable && p3dChangeCounter2.Threshold == 0.5f)
			{
				this.NoRustRatio += num;
				this.NoRustRatioParts += 1f;
				p3dChangeCounter2.gameObject.GetComponent<CarProperties>().NoRustRatio = num;
				if (num > 0.95f)
				{
					this.RustGoodParts += 1f;
				}
			}
			if (p3dChangeCounter2.gameObject.GetComponent<CarProperties>().Paintable && p3dChangeCounter2.Threshold == 0.1f)
			{
				this.PaintRatio += num;
				this.PaintRatioParts += 1f;
				p3dChangeCounter2.gameObject.GetComponent<CarProperties>().PaintRatio = num;
				if (num > 0.9f)
				{
					this.PaintGoodParts += 1f;
				}
			}
			p3dChangeCounter2.enabled = false;
		}
		this.DamagedBodyPanels = 0f;
		this.AllBodyPanels = 0f;
		if (this.CleanRatioParts > 0f)
		{
			this.CleanRatio /= this.CleanRatioParts;
		}
		else
		{
			this.CleanRatio = 0.9f;
		}
		if (this.NoRustRatioParts > 0f)
		{
			this.NoRustRatio = this.RustGoodParts / this.NoRustRatioParts;
		}
		else
		{
			this.NoRustRatio = 0.9f;
		}
		if (this.NoRustRatio < 0.3f)
		{
			this.NoRustRatio = 0.3f;
		}
		if (this.PaintRatioParts > 0f)
		{
			this.PaintRatio = this.PaintGoodParts / this.PaintRatioParts;
		}
		else
		{
			this.PaintRatio = 0.9f;
		}
		this.AverageCondition = 0f;
		foreach (CarProperties carProperties in this.Car.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.SinglePart)
			{
				this.conditCount = 0f;
				if (carProperties.MeshRepairable && (this.MainProperties.Owner == "Client" || carProperties.Owner != "Client"))
				{
					if (carProperties.MeshDamaged)
					{
						this.DamagedBodyPanels += 1f;
					}
					this.AllBodyPanels += 1f;
					if (carProperties.Ruined || carProperties.NoRustRatio < 0.9f)
					{
						this.conditCount += 0f;
					}
					else if (carProperties.MeshDamaged || carProperties.NoRustRatio < 0.95f)
					{
						this.conditCount += 0.1f;
					}
					else
					{
						this.conditCount += 1f;
					}
				}
				if (!carProperties.MeshRepairable && (this.MainProperties.Owner == "Client" || carProperties.Owner != "Client"))
				{
					if (carProperties.Condition < 0.4f && carProperties.Condition >= 0.1f && (carProperties.WornMesh || carProperties.WornMaterial))
					{
						this.conditCount += 0.1f;
					}
					else if (carProperties.Condition <= 0.1f && (carProperties.RuinedMesh || carProperties.RuinedMaterial || carProperties.WornMesh || carProperties.WornMaterial))
					{
						this.conditCount += 0f;
					}
					else if (carProperties.PartIsOld)
					{
						this.conditCount += 0f;
					}
					else
					{
						this.conditCount += 1f;
					}
				}
				carProperties.ConditionDebug = this.conditCount;
				this.AverageCondition += this.conditCount;
				if (carProperties.Condition > 0.4f)
				{
					this.PRICEv2 += carProperties.gameObject.GetComponent<Partinfo>().price;
				}
			}
		}
		if (this.existingparts >= this.allparts)
		{
			this.AverageCondition /= (float)this.existingparts;
		}
		else
		{
			this.AverageCondition /= this.MainProperties.PartsCount;
		}
		this.PRICE = this.MainProperties.CarPrice * this.AverageCondition * (this.NoRustRatio * this.NoRustRatio);
		this.PRICE = this.PRICE / 2f + this.PRICE / 2f * this.PaintRatio;
		this.PRICE = this.PRICE / 50f * 49f + this.PRICE / 50f * this.CleanRatio;
		if (this.AllBodyPanels > 1f)
		{
			this.PRICE = this.PRICE / 3f + this.PRICE / 3f * 2f * ((this.AllBodyPanels - this.DamagedBodyPanels) / this.AllBodyPanels);
		}
		if (float.IsNaN(this.PRICE))
		{
			this.PRICE = 20f;
		}
		if (this.dealerprice && this.AverageCondition > 0.95f && this.CleanRatio > 0.85f && this.NoRustRatio > 0.98f && this.PaintRatio > 0.98f)
		{
			this.PRICE *= 1.3f;
		}
		this.Price.GetComponent<TextMeshProUGUI>().text = this.PRICE.ToString("F2");
		if (this.MainProperties.Owner == "None")
		{
			this.Price.GetComponent<TextMeshProUGUI>().text = "Free";
		}
		if (this.AverageCondition > 0.95f)
		{
			this.OverallStar.sprite = this.star5;
		}
		else if (this.AverageCondition > 0.9f)
		{
			this.OverallStar.sprite = this.star4;
		}
		else if (this.AverageCondition > 0.8f)
		{
			this.OverallStar.sprite = this.star3;
		}
		else if (this.AverageCondition > 0.7f)
		{
			this.OverallStar.sprite = this.star2;
		}
		else if (this.AverageCondition > 0.6f)
		{
			this.OverallStar.sprite = this.star1;
		}
		else
		{
			this.OverallStar.sprite = this.star0;
		}
		if (this.CleanRatio > 0.85f)
		{
			this.CleanStar.sprite = this.star5;
		}
		else if (this.CleanRatio > 0.75f)
		{
			this.CleanStar.sprite = this.star4;
		}
		else if (this.CleanRatio > 0.6f)
		{
			this.CleanStar.sprite = this.star3;
		}
		else if (this.CleanRatio > 0.4f)
		{
			this.CleanStar.sprite = this.star2;
		}
		else if (this.CleanRatio > 0.2f)
		{
			this.CleanStar.sprite = this.star1;
		}
		else
		{
			this.CleanStar.sprite = this.star0;
		}
		if (this.NoRustRatio > 0.98f)
		{
			this.RustStar.sprite = this.star5;
		}
		else if (this.NoRustRatio > 0.8f)
		{
			this.RustStar.sprite = this.star4;
		}
		else if (this.NoRustRatio > 0.6f)
		{
			this.RustStar.sprite = this.star3;
		}
		else if (this.NoRustRatio > 0.4f)
		{
			this.RustStar.sprite = this.star2;
		}
		else if (this.NoRustRatio > 0.2f)
		{
			this.RustStar.sprite = this.star1;
		}
		else
		{
			this.RustStar.sprite = this.star0;
		}
		if (this.PaintRatio > 0.98f)
		{
			this.ColorStar.sprite = this.star5;
		}
		else if (this.PaintRatio > 0.8f)
		{
			this.ColorStar.sprite = this.star4;
		}
		else if (this.PaintRatio > 0.6f)
		{
			this.ColorStar.sprite = this.star3;
		}
		else if (this.PaintRatio > 0.4f)
		{
			this.ColorStar.sprite = this.star2;
		}
		else if (this.PaintRatio > 0.2f)
		{
			this.ColorStar.sprite = this.star1;
		}
		else
		{
			this.ColorStar.sprite = this.star0;
		}
		yield break;
	}

	// Token: 0x0400096A RID: 2410
	public GameObject Car;

	// Token: 0x0400096B RID: 2411
	public MainCarProperties MainProperties;

	// Token: 0x0400096C RID: 2412
	public GameObject ItemPictureParent;

	// Token: 0x0400096D RID: 2413
	public GameObject CursorCanvas;

	// Token: 0x0400096E RID: 2414
	private bool dealerprice;

	// Token: 0x0400096F RID: 2415
	public GameObject JobsButton;

	// Token: 0x04000970 RID: 2416
	private GameObject ThePoint;

	// Token: 0x04000971 RID: 2417
	public GameObject SignSpawn;

	// Token: 0x04000972 RID: 2418
	public GameObject[] GarageSpawns;

	// Token: 0x04000973 RID: 2419
	public GameObject[] JunkSpawns;

	// Token: 0x04000974 RID: 2420
	public GameObject[] DealerSpawns;

	// Token: 0x04000975 RID: 2421
	public GameObject[] HouseSpawns;

	// Token: 0x04000976 RID: 2422
	public GameObject ServiceSpawn;

	// Token: 0x04000977 RID: 2423
	public GameObject NoSpaceCanvas;

	// Token: 0x04000978 RID: 2424
	public GameObject AudioParent;

	// Token: 0x04000979 RID: 2425
	public GameObject CarModel;

	// Token: 0x0400097A RID: 2426
	public GameObject OwnerName;

	// Token: 0x0400097B RID: 2427
	public Image OriginalCarColor;

	// Token: 0x0400097C RID: 2428
	public GameObject OriginalColorName;

	// Token: 0x0400097D RID: 2429
	public Image CarColor;

	// Token: 0x0400097E RID: 2430
	public GameObject ColorName;

	// Token: 0x0400097F RID: 2431
	public Sprite star5;

	// Token: 0x04000980 RID: 2432
	public Sprite star4;

	// Token: 0x04000981 RID: 2433
	public Sprite star3;

	// Token: 0x04000982 RID: 2434
	public Sprite star2;

	// Token: 0x04000983 RID: 2435
	public Sprite star1;

	// Token: 0x04000984 RID: 2436
	public Sprite star0;

	// Token: 0x04000985 RID: 2437
	public Image CleanStar;

	// Token: 0x04000986 RID: 2438
	public Image RustStar;

	// Token: 0x04000987 RID: 2439
	public Image ColorStar;

	// Token: 0x04000988 RID: 2440
	public Image OverallStar;

	// Token: 0x04000989 RID: 2441
	public GameObject EngineName;

	// Token: 0x0400098A RID: 2442
	public GameObject GearboxName;

	// Token: 0x0400098B RID: 2443
	public GameObject DiffName;

	// Token: 0x0400098C RID: 2444
	public GameObject PartCount;

	// Token: 0x0400098D RID: 2445
	public GameObject Conditiontxt;

	// Token: 0x0400098E RID: 2446
	public GameObject CleanCondition;

	// Token: 0x0400098F RID: 2447
	public GameObject RustCondition;

	// Token: 0x04000990 RID: 2448
	public GameObject PaintCondition;

	// Token: 0x04000991 RID: 2449
	public FlexibleColorPicker FCP;

	// Token: 0x04000992 RID: 2450
	public GameObject PaintButton;

	// Token: 0x04000993 RID: 2451
	public GameObject PriceToDisable;

	// Token: 0x04000994 RID: 2452
	public GameObject Price;

	// Token: 0x04000995 RID: 2453
	public GameObject BuyButtonText;

	// Token: 0x04000996 RID: 2454
	public GameObject BuyButton;

	// Token: 0x04000997 RID: 2455
	public int allparts;

	// Token: 0x04000998 RID: 2456
	public int existingparts;

	// Token: 0x04000999 RID: 2457
	public float AverageCondition;

	// Token: 0x0400099A RID: 2458
	public float conditCount;

	// Token: 0x0400099B RID: 2459
	public float CheckedParts;

	// Token: 0x0400099C RID: 2460
	public Text Interior;

	// Token: 0x0400099D RID: 2461
	public Text Mileage;

	// Token: 0x0400099E RID: 2462
	public float SuspensionCondition;

	// Token: 0x0400099F RID: 2463
	public float InteriorCondition;

	// Token: 0x040009A0 RID: 2464
	public float BodyCondition;

	// Token: 0x040009A1 RID: 2465
	public float EngineCondition;

	// Token: 0x040009A2 RID: 2466
	public float DamagedBodyPanels;

	// Token: 0x040009A3 RID: 2467
	public float AllBodyPanels;

	// Token: 0x040009A4 RID: 2468
	public float CleanRatio;

	// Token: 0x040009A5 RID: 2469
	public float NoRustRatio;

	// Token: 0x040009A6 RID: 2470
	public float PaintRatio;

	// Token: 0x040009A7 RID: 2471
	public float CleanRatioParts;

	// Token: 0x040009A8 RID: 2472
	public float NoRustRatioParts;

	// Token: 0x040009A9 RID: 2473
	public float PaintRatioParts;

	// Token: 0x040009AA RID: 2474
	public float PaintGoodParts;

	// Token: 0x040009AB RID: 2475
	public float RustGoodParts;

	// Token: 0x040009AC RID: 2476
	public float PRICE;

	// Token: 0x040009AD RID: 2477
	public float PRICEv2;
}
