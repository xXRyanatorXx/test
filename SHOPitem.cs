using System;
using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000119 RID: 281
public class SHOPitem : MonoBehaviour
{
	// Token: 0x060005F9 RID: 1529 RVA: 0x0002F158 File Offset: 0x0002D358
	private void Start()
	{
		this.BuyName.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Buy").ToString();
		if (this.ITEM.GetComponent<Partinfo>().Thumbnail)
		{
			this.ItemPictureParent.transform.GetComponent<Image>().sprite = Sprite.Create(this.ITEM.GetComponent<Partinfo>().Thumbnail, new Rect(0f, 0f, 500f, 500f), new Vector2(0.5f, 0.5f), 100f);
		}
		else
		{
			RuntimePreviewGenerator.BackgroundColor = new Color(0f, 0f, 0f, 0f);
			this.ItemPictureParent.transform.GetComponent<Image>().sprite = Sprite.Create(RuntimePreviewGenerator.GenerateModelPreview(this.ITEM.transform, 500, 500, false), new Rect(0f, 0f, 500f, 500f), new Vector2(0.5f, 0.5f), 100f);
		}
		if (this.ITEM.GetComponent<CarProperties>() && this.ITEM.GetComponent<CarProperties>().PartName != "")
		{
			this.Name.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize(this.ITEM.GetComponent<CarProperties>().PartName) + this.ITEM.GetComponent<CarProperties>().PartNameExtension;
		}
		else if (this.ITEM.GetComponent<CarProperties>())
		{
			this.Name.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize(this.ITEM.transform.name.Remove(this.ITEM.transform.name.Length - 2));
		}
		else
		{
			this.Name.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize(this.ITEM.transform.name);
		}
		this.Price.GetComponent<TextMeshProUGUI>().text = (this.ITEM.GetComponent<Partinfo>().price * tools.PriceModifier).ToString() + "$";
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0002F394 File Offset: 0x0002D594
	private void OnEnable()
	{
		if (this.ITEM)
		{
			this.Price.GetComponent<TextMeshProUGUI>().text = (this.ITEM.GetComponent<Partinfo>().price * tools.PriceModifier).ToString() + "$";
		}
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0002F3E8 File Offset: 0x0002D5E8
	public void bought()
	{
		if (tools.money > this.ITEM.GetComponent<Partinfo>().price * tools.PriceModifier)
		{
			tools.money -= this.ITEM.GetComponent<Partinfo>().price * tools.PriceModifier;
			tools.AudioParent_.GetComponent<AudioSource>().PlayOneShot(tools.AudioParent_.GetComponent<AudioManager>().Cash);
			if (!this.car)
			{
				this.ITEM.GetComponent<Partinfo>().SpawnThis();
			}
			if (this.car)
			{
				if (!this.RandomCondition)
				{
					this.player = GameObject.Find("Player");
					UnityEngine.Object.Instantiate<GameObject>(this.ITEM, new Vector3(this.player.transform.position.x, this.player.transform.position.y + 0.5f, this.player.transform.position.z + 3f), Quaternion.identity).GetComponent<MainCarProperties>().CreatingStock(0);
				}
				if (this.RandomCondition)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.ITEM, new Vector3(UnityEngine.Random.Range(0.1f, 10f), UnityEngine.Random.Range(-99f, -70f), UnityEngine.Random.Range(0.1f, 10f)), Quaternion.Euler((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360))).GetComponent<MainCarProperties>().Creating();
				}
			}
		}
	}

	// Token: 0x040008E5 RID: 2277
	public GameObject player;

	// Token: 0x040008E6 RID: 2278
	public bool RandomCondition;

	// Token: 0x040008E7 RID: 2279
	public string[] FitsToCar;

	// Token: 0x040008E8 RID: 2280
	public GameObject ItemPictureParent;

	// Token: 0x040008E9 RID: 2281
	public GameObject ITEM;

	// Token: 0x040008EA RID: 2282
	public GameObject Price;

	// Token: 0x040008EB RID: 2283
	public GameObject Name;

	// Token: 0x040008EC RID: 2284
	public GameObject BuyName;

	// Token: 0x040008ED RID: 2285
	public bool car;
}
