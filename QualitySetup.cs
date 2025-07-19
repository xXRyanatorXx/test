using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001CA RID: 458
public class QualitySetup : MonoBehaviour
{
	// Token: 0x06000ADF RID: 2783 RVA: 0x0006FAE8 File Offset: 0x0006DCE8
	private void Start()
	{
		if (PlayerPrefs.HasKey("Quality"))
		{
			if (PlayerPrefs.GetFloat("Quality") == 0f)
			{
				this.QualityText.text = "Very Low";
				QualitySettings.SetQualityLevel(0, true);
				this.Quality = 0f;
			}
			if (PlayerPrefs.GetFloat("Quality") == 1f)
			{
				this.QualityText.text = "Low";
				QualitySettings.SetQualityLevel(1, true);
				this.Quality = 1f;
			}
			if (PlayerPrefs.GetFloat("Quality") == 2f)
			{
				this.QualityText.text = "Medium";
				QualitySettings.SetQualityLevel(2, true);
				this.Quality = 2f;
			}
			if (PlayerPrefs.GetFloat("Quality") == 3f)
			{
				this.QualityText.text = "High";
				QualitySettings.SetQualityLevel(3, true);
				this.Quality = 3f;
			}
			if (PlayerPrefs.GetFloat("Quality") == 4f)
			{
				this.QualityText.text = "Very High";
				QualitySettings.SetQualityLevel(4, true);
				this.Quality = 4f;
			}
			if (PlayerPrefs.GetFloat("Quality") == 5f)
			{
				this.QualityText.text = "Ultra";
				QualitySettings.SetQualityLevel(5, true);
				this.Quality = 5f;
				return;
			}
		}
		else if (PlayerPrefs.GetFloat("Quality") == 3f)
		{
			this.QualityText.text = "High";
			QualitySettings.SetQualityLevel(3, true);
			this.Quality = 3f;
		}
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0006FC6C File Offset: 0x0006DE6C
	public void Next()
	{
		if (this.Quality == 0f)
		{
			PlayerPrefs.SetFloat("Quality", 1f);
			this.QualityText.text = "Low";
			QualitySettings.SetQualityLevel(1, true);
			this.Quality = 1f;
		}
		else if (this.Quality == 1f)
		{
			PlayerPrefs.SetFloat("Quality", 2f);
			this.QualityText.text = "Medium";
			QualitySettings.SetQualityLevel(2, true);
			this.Quality = 2f;
		}
		else if (this.Quality == 2f)
		{
			PlayerPrefs.SetFloat("Quality", 3f);
			this.QualityText.text = "High";
			QualitySettings.SetQualityLevel(3, true);
			this.Quality = 3f;
		}
		else if (this.Quality == 3f)
		{
			PlayerPrefs.SetFloat("Quality", 4f);
			this.QualityText.text = "Very High";
			QualitySettings.SetQualityLevel(4, true);
			this.Quality = 4f;
		}
		else if (this.Quality == 4f)
		{
			PlayerPrefs.SetFloat("Quality", 5f);
			this.QualityText.text = "Ultra";
			QualitySettings.SetQualityLevel(5, true);
			this.Quality = 5f;
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0006FDC4 File Offset: 0x0006DFC4
	public void Previous()
	{
		if (this.Quality == 1f)
		{
			PlayerPrefs.SetFloat("Quality", 0f);
			this.QualityText.text = "Very Low";
			QualitySettings.SetQualityLevel(0, true);
			this.Quality = 0f;
		}
		else if (this.Quality == 2f)
		{
			PlayerPrefs.SetFloat("Quality", 1f);
			this.QualityText.text = "Low";
			QualitySettings.SetQualityLevel(1, true);
			this.Quality = 1f;
		}
		else if (this.Quality == 3f)
		{
			PlayerPrefs.SetFloat("Quality", 2f);
			this.QualityText.text = "Medium";
			QualitySettings.SetQualityLevel(2, true);
			this.Quality = 2f;
		}
		else if (this.Quality == 4f)
		{
			PlayerPrefs.SetFloat("Quality", 3f);
			this.QualityText.text = "High";
			QualitySettings.SetQualityLevel(3, true);
			this.Quality = 3f;
		}
		else if (this.Quality == 5f)
		{
			PlayerPrefs.SetFloat("Quality", 4f);
			this.QualityText.text = "Very High";
			QualitySettings.SetQualityLevel(4, true);
			this.Quality = 4f;
		}
		PlayerPrefs.Save();
	}

	// Token: 0x0400134B RID: 4939
	public Text QualityText;

	// Token: 0x0400134C RID: 4940
	public float Quality;

	// Token: 0x0400134D RID: 4941
	public GameObject Vegetation;
}
