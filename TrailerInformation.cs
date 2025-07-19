using System;
using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001ED RID: 493
public class TrailerInformation : MonoBehaviour
{
	// Token: 0x06000B8F RID: 2959 RVA: 0x00080970 File Offset: 0x0007EB70
	public void BuySell()
	{
		tools.money += this.PRICE;
		UnityEngine.Object.Destroy(this.Trailer);
		this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
		this.CursorCanvas.SetActive(true);
		GameObject.Find("Player").GetComponent<FirstPersonAIO>().ControllerUnPause();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x000809E8 File Offset: 0x0007EBE8
	public void ReStart()
	{
		this.AudioParent = GameObject.Find("hand");
		this.MainProperties = this.Trailer.GetComponent<MainTrailerProperties>();
		RuntimePreviewGenerator.BackgroundColor = new Color(0f, 0f, 0f, 0f);
		this.ItemPictureParent.transform.GetComponent<Image>().sprite = Sprite.Create(RuntimePreviewGenerator.GenerateModelPreview(this.Trailer.transform, 500, 500, false), new Rect(0f, 0f, 500f, 500f), new Vector2(0.5f, 0.5f), 100f);
		this.PRICE = this.MainProperties.TrailerPrice * 0.7f;
		this.Price.GetComponent<TextMeshProUGUI>().text = this.PRICE.ToString();
		this.BuyButton.SetActive(true);
		this.PriceText.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Price");
		this.HeaderText.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Trailer Information");
		this.BuyButtonText.GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize("Sell");
	}

	// Token: 0x04001422 RID: 5154
	public GameObject Trailer;

	// Token: 0x04001423 RID: 5155
	public MainTrailerProperties MainProperties;

	// Token: 0x04001424 RID: 5156
	public GameObject ItemPictureParent;

	// Token: 0x04001425 RID: 5157
	public GameObject CursorCanvas;

	// Token: 0x04001426 RID: 5158
	public GameObject AudioParent;

	// Token: 0x04001427 RID: 5159
	public GameObject Price;

	// Token: 0x04001428 RID: 5160
	public GameObject BuyButtonText;

	// Token: 0x04001429 RID: 5161
	public GameObject PriceText;

	// Token: 0x0400142A RID: 5162
	public GameObject HeaderText;

	// Token: 0x0400142B RID: 5163
	public GameObject BuyButton;

	// Token: 0x0400142C RID: 5164
	public float PRICE;
}
