using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A8 RID: 168
[AddComponentMenu("Enviro/Vegetation Growth Object")]
public class EnviroVegetationInstance : MonoBehaviour
{
	// Token: 0x0600034F RID: 847 RVA: 0x00016594 File Offset: 0x00014794
	private void Start()
	{
		EnviroSkyMgr.instance.RegisterVegetationInstance(this);
		this.currentSeason = EnviroSkyMgr.instance.GetCurrentSeason();
		this.maxAgeInHours = EnviroSkyMgr.instance.GetInHours(this.Age.maxAgeHours, this.Age.maxAgeDays, this.Age.maxAgeYears);
		EnviroSkyMgr.instance.OnSeasonChanged += delegate(EnviroSeasons.Seasons season)
		{
			this.SetSeason();
		};
		if (this.Age.randomStartAge)
		{
			this.Age.startAgeinHours = UnityEngine.Random.Range(0f, (float)this.maxAgeInHours);
			this.Age.randomStartAge = false;
		}
		this.Birth(0, this.Age.startAgeinHours);
	}

	// Token: 0x06000350 RID: 848 RVA: 0x0001664C File Offset: 0x0001484C
	private void OnEnable()
	{
		if (this.GrowStages.Count < 1)
		{
			Debug.LogError("Please setup GrowStages!");
			base.enabled = false;
		}
		for (int i = 0; i < this.GrowStages.Count; i++)
		{
			if (this.GrowStages[i].GrowGameobjectAutumn == null || this.GrowStages[i].GrowGameobjectSpring == null || this.GrowStages[i].GrowGameobjectSummer == null || this.GrowStages[i].GrowGameobjectWinter == null)
			{
				Debug.LogError("One ore more GrowStages missing GrowPrefabs!");
				base.enabled = false;
			}
		}
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00016706 File Offset: 0x00014906
	private void SetSeason()
	{
		this.currentSeason = EnviroSkyMgr.instance.GetCurrentSeason();
		this.VegetationChange();
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00016720 File Offset: 0x00014920
	public void KeepVariablesClear()
	{
		this.GrowStages[0].minAgePercent = 0f;
		for (int i = 0; i < this.GrowStages.Count; i++)
		{
			if (this.GrowStages[i].minAgePercent > 100f)
			{
				this.GrowStages[i].minAgePercent = 100f;
			}
		}
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00016787 File Offset: 0x00014987
	public void UpdateInstance()
	{
		if (this.reBirth)
		{
			this.Birth(0, 0f);
		}
		if (this.shrink)
		{
			this.ShrinkAndDeactivate();
		}
		if (this.canGrow)
		{
			this.UpdateGrowth();
		}
	}

	// Token: 0x06000354 RID: 852 RVA: 0x000167BC File Offset: 0x000149BC
	public void UpdateGrowth()
	{
		this.ageInHours = EnviroSkyMgr.instance.GetCurrentTimeInHours() - this.Age.birthdayInHours;
		this.KeepVariablesClear();
		if (!this.stay)
		{
			if (this.currentStage + 1 < this.GrowStages.Count)
			{
				if (this.maxAgeInHours * (double)(this.GrowStages[this.currentStage + 1].minAgePercent / 100f) <= this.ageInHours && this.ageInHours > 0.0)
				{
					this.currentStage++;
					this.VegetationChange();
					return;
				}
				if (this.GrowStages[this.currentStage].growAction == EnviroVegetationStage.GrowState.Grow)
				{
					this.CalculateScale();
					return;
				}
			}
			else if (!this.stay)
			{
				if (this.ageInHours > this.maxAgeInHours)
				{
					if (!this.Age.Loop)
					{
						this.stay = true;
						return;
					}
					this.currentVegetationObject.SetActive(false);
					if (this.DeadPrefab != null)
					{
						this.DeadPrefabLoop();
						return;
					}
					this.Birth(this.Age.LoopFromGrowStage, 0f);
					return;
				}
				else if (this.GrowStages[this.currentStage].growAction == EnviroVegetationStage.GrowState.Grow)
				{
					this.CalculateScale();
				}
			}
		}
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00016904 File Offset: 0x00014B04
	private void DeadPrefabLoop()
	{
		this.stay = true;
		UnityEngine.Object.Instantiate<GameObject>(this.DeadPrefab, base.transform.position, base.transform.rotation).transform.localScale = this.currentVegetationObject.transform.localScale;
		this.Birth(this.Age.LoopFromGrowStage, 0f);
		this.stay = false;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x00016970 File Offset: 0x00014B70
	private IEnumerator BirthColliders()
	{
		Collider[] colliders = this.currentVegetationObject.GetComponentsInChildren<Collider>();
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].enabled = false;
		}
		yield return new WaitForSeconds(10f);
		for (int j = 0; j < colliders.Length; j++)
		{
			colliders[j].enabled = true;
		}
		yield break;
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00016980 File Offset: 0x00014B80
	private void CalculateScale()
	{
		if (this.rescale)
		{
			this.currentVegetationObject.transform.localScale = this.minScale;
			this.rescale = false;
		}
		double num = this.ageInHours / this.maxAgeInHours * (double)this.GrowSpeedMod;
		this.currentVegetationObject.transform.localScale = this.minScale + new Vector3((float)num, (float)num, (float)num);
		if (this.currentVegetationObject.transform.localScale.y > this.maxScale.y)
		{
			this.currentVegetationObject.transform.localScale = this.maxScale;
		}
		if (this.currentVegetationObject.transform.localScale.y < this.minScale.y)
		{
			this.currentVegetationObject.transform.localScale = this.minScale;
		}
	}

	// Token: 0x06000358 RID: 856 RVA: 0x00016A60 File Offset: 0x00014C60
	public void Birth(int stage, float startAge)
	{
		this.Age.birthdayInHours = EnviroSkyMgr.instance.GetCurrentTimeInHours() - (double)startAge;
		startAge = 0f;
		this.ageInHours = 0.0;
		this.currentStage = stage;
		this.rescale = true;
		this.reBirth = false;
		this.VegetationChange();
		base.StartCoroutine(this.BirthColliders());
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00016AC4 File Offset: 0x00014CC4
	private void SeasonAction()
	{
		if (this.Seasons.seasonAction == EnviroVegetationSeasons.SeasonAction.SpawnDeadPrefab)
		{
			if (this.DeadPrefab != null)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.DeadPrefab, base.transform.position, base.transform.rotation).transform.localScale = this.currentVegetationObject.transform.localScale;
			}
			this.currentVegetationObject.SetActive(false);
			return;
		}
		if (this.Seasons.seasonAction == EnviroVegetationSeasons.SeasonAction.Deactivate)
		{
			this.shrink = true;
			return;
		}
		if (this.Seasons.seasonAction == EnviroVegetationSeasons.SeasonAction.Destroy)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00016B64 File Offset: 0x00014D64
	private void CheckSeason(bool update)
	{
		if (!update && this.canGrow)
		{
			this.SeasonAction();
			this.canGrow = false;
			return;
		}
		if (update && !this.canGrow)
		{
			this.canGrow = true;
			this.reBirth = true;
			return;
		}
		if (!update && !this.canGrow)
		{
			this.SeasonAction();
		}
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00016BB8 File Offset: 0x00014DB8
	private void ShrinkAndDeactivate()
	{
		if (this.currentVegetationObject.transform.localScale.y > this.minScale.y)
		{
			this.currentVegetationObject.transform.localScale = new Vector3(this.currentVegetationObject.transform.localScale.x - 0.1f * Time.deltaTime, this.currentVegetationObject.transform.localScale.y - 0.1f * Time.deltaTime, this.currentVegetationObject.transform.localScale.z - 0.1f * Time.deltaTime);
			return;
		}
		this.shrink = false;
		this.currentVegetationObject.SetActive(false);
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00016C74 File Offset: 0x00014E74
	public void VegetationChange()
	{
		this.canGrow = true;
		if (this.currentVegetationObject != null)
		{
			this.currentVegetationObject.SetActive(false);
		}
		switch (this.currentSeason)
		{
		case EnviroSeasons.Seasons.Spring:
			this.currentVegetationObject = this.GrowStages[this.currentStage].GrowGameobjectSpring;
			this.CalculateScale();
			this.currentVegetationObject.SetActive(true);
			if (!this.Seasons.GrowInSpring)
			{
				this.CheckSeason(false);
				return;
			}
			if (this.Seasons.GrowInSpring)
			{
				this.CheckSeason(true);
				return;
			}
			break;
		case EnviroSeasons.Seasons.Summer:
			this.currentVegetationObject = this.GrowStages[this.currentStage].GrowGameobjectSummer;
			this.CalculateScale();
			this.currentVegetationObject.SetActive(true);
			if (!this.Seasons.GrowInSummer)
			{
				this.CheckSeason(false);
				return;
			}
			if (this.Seasons.GrowInSummer)
			{
				this.CheckSeason(true);
				return;
			}
			break;
		case EnviroSeasons.Seasons.Autumn:
			this.currentVegetationObject = this.GrowStages[this.currentStage].GrowGameobjectAutumn;
			this.CalculateScale();
			this.currentVegetationObject.SetActive(true);
			if (!this.Seasons.GrowInAutumn)
			{
				this.CheckSeason(false);
				return;
			}
			if (this.Seasons.GrowInAutumn)
			{
				this.CheckSeason(true);
				return;
			}
			break;
		case EnviroSeasons.Seasons.Winter:
			this.currentVegetationObject = this.GrowStages[this.currentStage].GrowGameobjectWinter;
			this.CalculateScale();
			this.currentVegetationObject.SetActive(true);
			if (!this.Seasons.GrowInWinter)
			{
				this.CheckSeason(false);
				return;
			}
			if (this.Seasons.GrowInWinter)
			{
				this.CheckSeason(true);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00016E25 File Offset: 0x00015025
	private void LateUpdate()
	{
		if (this.GrowStages[this.currentStage].billboard && this.canGrow)
		{
			base.transform.rotation = Camera.main.transform.rotation;
		}
	}

	// Token: 0x0600035E RID: 862 RVA: 0x00016E61 File Offset: 0x00015061
	private void OnDrawGizmos()
	{
		Gizmos.color = this.GizmoColor;
		Gizmos.DrawCube(base.transform.position, new Vector3(this.GizmoSize, this.GizmoSize, this.GizmoSize));
	}

	// Token: 0x04000490 RID: 1168
	[HideInInspector]
	public int id;

	// Token: 0x04000491 RID: 1169
	public EnviroVegetationAge Age;

	// Token: 0x04000492 RID: 1170
	public EnviroVegetationSeasons Seasons;

	// Token: 0x04000493 RID: 1171
	public List<EnviroVegetationStage> GrowStages = new List<EnviroVegetationStage>();

	// Token: 0x04000494 RID: 1172
	public Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f);

	// Token: 0x04000495 RID: 1173
	public Vector3 maxScale = new Vector3(1f, 1f, 1f);

	// Token: 0x04000496 RID: 1174
	public float GrowSpeedMod = 1f;

	// Token: 0x04000497 RID: 1175
	public GameObject DeadPrefab;

	// Token: 0x04000498 RID: 1176
	public Color GizmoColor = new Color(255f, 0f, 0f, 255f);

	// Token: 0x04000499 RID: 1177
	public float GizmoSize = 0.5f;

	// Token: 0x0400049A RID: 1178
	private EnviroSeasons.Seasons currentSeason;

	// Token: 0x0400049B RID: 1179
	private double ageInHours;

	// Token: 0x0400049C RID: 1180
	private double maxAgeInHours;

	// Token: 0x0400049D RID: 1181
	private int currentStage;

	// Token: 0x0400049E RID: 1182
	private GameObject currentVegetationObject;

	// Token: 0x0400049F RID: 1183
	private bool stay;

	// Token: 0x040004A0 RID: 1184
	private bool reBirth;

	// Token: 0x040004A1 RID: 1185
	private bool rescale = true;

	// Token: 0x040004A2 RID: 1186
	private bool canGrow = true;

	// Token: 0x040004A3 RID: 1187
	private bool shrink;
}
