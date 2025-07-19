using System;
using UnityEngine;

// Token: 0x020000BC RID: 188
[AddComponentMenu("Enviro/Utility/Seasons for Meshes")]
public class EnviroMeshSeasons : MonoBehaviour
{
	// Token: 0x060003AF RID: 943 RVA: 0x00019518 File Offset: 0x00017718
	private void Start()
	{
		this.myRenderer = base.GetComponent<MeshRenderer>();
		if (this.myRenderer == null)
		{
			Debug.LogError("Please correct script placement! We need a MeshRenderer to work with!");
			base.enabled = false;
		}
		this.UpdateSeasonMaterial();
		EnviroSkyMgr.instance.OnSeasonChanged += delegate(EnviroSeasons.Seasons season)
		{
			this.UpdateSeasonMaterial();
		};
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0001956C File Offset: 0x0001776C
	private void OnEnable()
	{
		if (this.SpringMaterial == null)
		{
			Debug.LogError("Please assign a spring material in Inspector!");
			base.enabled = false;
		}
		if (this.SummerMaterial == null)
		{
			Debug.LogError("Please assign a summer material in Inspector!");
			base.enabled = false;
		}
		if (this.AutumnMaterial == null)
		{
			Debug.LogError("Please assign a autumn material in Inspector!");
			base.enabled = false;
		}
		if (this.WinterMaterial == null)
		{
			Debug.LogError("Please assign a winter material in Inspector!");
			base.enabled = false;
		}
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x000195F8 File Offset: 0x000177F8
	private void UpdateSeasonMaterial()
	{
		switch (EnviroSkyMgr.instance.GetCurrentSeason())
		{
		case EnviroSeasons.Seasons.Spring:
			this.myRenderer.sharedMaterial = this.SpringMaterial;
			return;
		case EnviroSeasons.Seasons.Summer:
			this.myRenderer.sharedMaterial = this.SummerMaterial;
			return;
		case EnviroSeasons.Seasons.Autumn:
			this.myRenderer.sharedMaterial = this.AutumnMaterial;
			return;
		case EnviroSeasons.Seasons.Winter:
			this.myRenderer.sharedMaterial = this.WinterMaterial;
			return;
		default:
			return;
		}
	}

	// Token: 0x04000567 RID: 1383
	public Material SpringMaterial;

	// Token: 0x04000568 RID: 1384
	public Material SummerMaterial;

	// Token: 0x04000569 RID: 1385
	public Material AutumnMaterial;

	// Token: 0x0400056A RID: 1386
	public Material WinterMaterial;

	// Token: 0x0400056B RID: 1387
	private MeshRenderer myRenderer;
}
