using System;
using UnityEngine;

// Token: 0x020000BD RID: 189
[AddComponentMenu("Enviro/Utility/Seasons for GameObjects")]
public class EnviroSeasonObjectSwitcher : MonoBehaviour
{
	// Token: 0x060003B4 RID: 948 RVA: 0x00019676 File Offset: 0x00017876
	private void Start()
	{
		this.SwitchSeasonObject();
		EnviroSkyMgr.instance.OnSeasonChanged += delegate(EnviroSeasons.Seasons season)
		{
			this.SwitchSeasonObject();
		};
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x00019694 File Offset: 0x00017894
	private void OnEnable()
	{
		if (this.SpringObject == null)
		{
			Debug.LogError("Please assign a spring Object in Inspector!");
			base.enabled = false;
		}
		if (this.SummerObject == null)
		{
			Debug.LogError("Please assign a summer Object in Inspector!");
			base.enabled = false;
		}
		if (this.AutumnObject == null)
		{
			Debug.LogError("Please assign a autumn Object in Inspector!");
			base.enabled = false;
		}
		if (this.WinterObject == null)
		{
			Debug.LogError("Please assign a winter Object in Inspector!");
			base.enabled = false;
		}
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00019720 File Offset: 0x00017920
	private void SwitchSeasonObject()
	{
		switch (EnviroSkyMgr.instance.GetCurrentSeason())
		{
		case EnviroSeasons.Seasons.Spring:
			this.SummerObject.SetActive(false);
			this.AutumnObject.SetActive(false);
			this.WinterObject.SetActive(false);
			this.SpringObject.SetActive(true);
			return;
		case EnviroSeasons.Seasons.Summer:
			this.SpringObject.SetActive(false);
			this.AutumnObject.SetActive(false);
			this.WinterObject.SetActive(false);
			this.SummerObject.SetActive(true);
			return;
		case EnviroSeasons.Seasons.Autumn:
			this.SpringObject.SetActive(false);
			this.SummerObject.SetActive(false);
			this.WinterObject.SetActive(false);
			this.AutumnObject.SetActive(true);
			return;
		case EnviroSeasons.Seasons.Winter:
			this.SpringObject.SetActive(false);
			this.SummerObject.SetActive(false);
			this.AutumnObject.SetActive(false);
			this.WinterObject.SetActive(true);
			return;
		default:
			return;
		}
	}

	// Token: 0x0400056C RID: 1388
	public GameObject SpringObject;

	// Token: 0x0400056D RID: 1389
	public GameObject SummerObject;

	// Token: 0x0400056E RID: 1390
	public GameObject AutumnObject;

	// Token: 0x0400056F RID: 1391
	public GameObject WinterObject;
}
