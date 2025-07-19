using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AF RID: 175
[AddComponentMenu("Enviro/Weather Zone")]
public class EnviroZone : MonoBehaviour
{
	// Token: 0x06000373 RID: 883 RVA: 0x00017368 File Offset: 0x00015568
	private void Start()
	{
		if (this.zoneWeatherPresets.Count > 0)
		{
			if (!this.useMeshZone)
			{
				this.zoneCollider = base.gameObject.AddComponent<BoxCollider>();
				this.zoneCollider.isTrigger = true;
			}
			else
			{
				this.zoneMeshCollider = base.gameObject.AddComponent<MeshCollider>();
				this.zoneMeshCollider.sharedMesh = this.zoneMesh;
				this.zoneMeshCollider.convex = true;
				this.zoneMeshCollider.isTrigger = true;
			}
			if (!EnviroSkyMgr.instance.IsDefaultZone(base.gameObject))
			{
				EnviroSkyMgr.instance.RegisterZone(this);
			}
			else
			{
				this.isDefault = true;
			}
			this.UpdateZoneScale();
			this.nextUpdate = EnviroSkyMgr.instance.GetCurrentTimeInHours() + (double)this.WeatherUpdateIntervall;
			this.nextUpdateRealtime = Time.time + this.WeatherUpdateIntervall * 60f;
			return;
		}
		Debug.Log("Please add Weather Prefabs to Zone:" + base.gameObject.name);
	}

	// Token: 0x06000374 RID: 884 RVA: 0x00017460 File Offset: 0x00015660
	public void UpdateZoneScale()
	{
		if (!this.isDefault && !this.useMeshZone)
		{
			this.zoneCollider.size = this.zoneScale;
			return;
		}
		if (!this.isDefault && this.useMeshZone)
		{
			base.transform.localScale = this.zoneScale;
			return;
		}
		if (this.isDefault && !this.useMeshZone)
		{
			this.zoneCollider.size = Vector3.one * (1f / base.transform.localScale.y) * 0.25f;
		}
	}

	// Token: 0x06000375 RID: 885 RVA: 0x000174F8 File Offset: 0x000156F8
	public void CreateZoneWeatherTypeList()
	{
		for (int i = 0; i < this.zoneWeatherPresets.Count; i++)
		{
			if (this.zoneWeatherPresets[i] == null)
			{
				Debug.Log("Warning! Missing Weather Preset in Zone: " + this.zoneName);
				return;
			}
			bool flag = true;
			for (int j = 0; j < EnviroSkyMgr.instance.GetCurrentWeatherPresetList().Count; j++)
			{
				if (this.zoneWeatherPresets[i] == EnviroSkyMgr.instance.GetCurrentWeatherPresetList()[j])
				{
					flag = false;
					this.zoneWeather.Add(EnviroSkyMgr.instance.GetCurrentWeatherPrefabList()[j]);
				}
			}
			if (flag)
			{
				GameObject gameObject = new GameObject();
				EnviroWeatherPrefab enviroWeatherPrefab = gameObject.AddComponent<EnviroWeatherPrefab>();
				enviroWeatherPrefab.weatherPreset = this.zoneWeatherPresets[i];
				gameObject.name = enviroWeatherPrefab.weatherPreset.Name;
				for (int k = 0; k < enviroWeatherPrefab.weatherPreset.effectSystems.Count; k++)
				{
					if (enviroWeatherPrefab.weatherPreset.effectSystems[k] == null || enviroWeatherPrefab.weatherPreset.effectSystems[k].prefab == null)
					{
						Debug.Log("Warning! Missing Particle System Entry: " + enviroWeatherPrefab.weatherPreset.Name);
						UnityEngine.Object.Destroy(gameObject);
						return;
					}
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(enviroWeatherPrefab.weatherPreset.effectSystems[k].prefab, gameObject.transform);
					gameObject2.transform.localPosition = enviroWeatherPrefab.weatherPreset.effectSystems[k].localPositionOffset;
					gameObject2.transform.localEulerAngles = enviroWeatherPrefab.weatherPreset.effectSystems[k].localRotationOffset;
					ParticleSystem particleSystem = gameObject2.GetComponent<ParticleSystem>();
					if (particleSystem != null)
					{
						enviroWeatherPrefab.effectSystems.Add(particleSystem);
					}
					else
					{
						particleSystem = gameObject2.GetComponentInChildren<ParticleSystem>();
						if (!(particleSystem != null))
						{
							Debug.Log("No Particle System found in prefab in weather preset: " + enviroWeatherPrefab.weatherPreset.Name);
							UnityEngine.Object.Destroy(gameObject);
							return;
						}
						enviroWeatherPrefab.effectSystems.Add(particleSystem);
					}
				}
				enviroWeatherPrefab.effectEmmisionRates.Clear();
				gameObject.transform.parent = EnviroSkyMgr.instance.GetVFXHolder().transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				this.zoneWeather.Add(enviroWeatherPrefab);
				EnviroSkyMgr.instance.GetCurrentWeatherPrefabList().Add(enviroWeatherPrefab);
				EnviroSkyMgr.instance.GetCurrentWeatherPresetList().Add(this.zoneWeatherPresets[i]);
			}
		}
		for (int l = 0; l < this.zoneWeather.Count; l++)
		{
			for (int m = 0; m < this.zoneWeather[l].effectSystems.Count; m++)
			{
				this.zoneWeather[l].effectEmmisionRates.Add(EnviroSkyMgr.instance.GetEmissionRate(this.zoneWeather[l].effectSystems[m]));
				EnviroSkyMgr.instance.SetEmissionRate(this.zoneWeather[l].effectSystems[m], 0f);
			}
		}
		if (this.isDefault && EnviroSkyMgr.instance.GetStartWeatherPreset() != null)
		{
			EnviroSkyMgr.instance.ChangeWeatherInstant(EnviroSkyMgr.instance.GetStartWeatherPreset());
			for (int n = 0; n < this.zoneWeather.Count; n++)
			{
				if (this.zoneWeather[n].weatherPreset == EnviroSkyMgr.instance.GetStartWeatherPreset())
				{
					this.currentActiveZoneWeatherPrefab = this.zoneWeather[n];
					this.lastActiveZoneWeatherPrefab = this.zoneWeather[n];
				}
			}
			this.currentActiveZoneWeatherPreset = EnviroSkyMgr.instance.GetStartWeatherPreset();
			this.lastActiveZoneWeatherPreset = EnviroSkyMgr.instance.GetStartWeatherPreset();
		}
		else
		{
			this.currentActiveZoneWeatherPrefab = this.zoneWeather[0];
			this.lastActiveZoneWeatherPrefab = this.zoneWeather[0];
			this.currentActiveZoneWeatherPreset = this.zoneWeatherPresets[0];
			this.lastActiveZoneWeatherPreset = this.zoneWeatherPresets[0];
		}
		this.nextUpdate = EnviroSkyMgr.instance.GetCurrentTimeInHours() + (double)this.WeatherUpdateIntervall;
	}

	// Token: 0x06000376 RID: 886 RVA: 0x00017978 File Offset: 0x00015B78
	private void BuildNewWeatherList()
	{
		this.curPossibleZoneWeather = new List<EnviroWeatherPrefab>();
		for (int i = 0; i < this.zoneWeather.Count; i++)
		{
			switch (EnviroSkyMgr.instance.GetCurrentSeason())
			{
			case EnviroSeasons.Seasons.Spring:
				if (this.zoneWeather[i].weatherPreset.Spring)
				{
					this.curPossibleZoneWeather.Add(this.zoneWeather[i]);
				}
				break;
			case EnviroSeasons.Seasons.Summer:
				if (this.zoneWeather[i].weatherPreset.Summer)
				{
					this.curPossibleZoneWeather.Add(this.zoneWeather[i]);
				}
				break;
			case EnviroSeasons.Seasons.Autumn:
				if (this.zoneWeather[i].weatherPreset.Autumn)
				{
					this.curPossibleZoneWeather.Add(this.zoneWeather[i]);
				}
				break;
			case EnviroSeasons.Seasons.Winter:
				if (this.zoneWeather[i].weatherPreset.winter)
				{
					this.curPossibleZoneWeather.Add(this.zoneWeather[i]);
				}
				break;
			}
		}
	}

	// Token: 0x06000377 RID: 887 RVA: 0x00017A9C File Offset: 0x00015C9C
	private EnviroWeatherPrefab PossibiltyCheck()
	{
		List<EnviroWeatherPrefab> list = new List<EnviroWeatherPrefab>();
		for (int i = 0; i < this.curPossibleZoneWeather.Count; i++)
		{
			int num = UnityEngine.Random.Range(0, 100);
			if (EnviroSkyMgr.instance.GetCurrentSeason() == EnviroSeasons.Seasons.Spring)
			{
				if ((float)num <= this.curPossibleZoneWeather[i].weatherPreset.possibiltyInSpring)
				{
					list.Add(this.curPossibleZoneWeather[i]);
				}
			}
			else if (EnviroSkyMgr.instance.GetCurrentSeason() == EnviroSeasons.Seasons.Summer)
			{
				if ((float)num <= this.curPossibleZoneWeather[i].weatherPreset.possibiltyInSummer)
				{
					list.Add(this.curPossibleZoneWeather[i]);
				}
			}
			else if (EnviroSkyMgr.instance.GetCurrentSeason() == EnviroSeasons.Seasons.Autumn)
			{
				if ((float)num <= this.curPossibleZoneWeather[i].weatherPreset.possibiltyInAutumn)
				{
					list.Add(this.curPossibleZoneWeather[i]);
				}
			}
			else if (EnviroSkyMgr.instance.GetCurrentSeason() == EnviroSeasons.Seasons.Winter && (float)num <= this.curPossibleZoneWeather[i].weatherPreset.possibiltyInWinter)
			{
				list.Add(this.curPossibleZoneWeather[i]);
			}
		}
		if (list.Count == 0)
		{
			return this.currentActiveZoneWeatherPrefab;
		}
		if (!this.forceWeatherChange)
		{
			EnviroSkyMgr.instance.NotifyZoneWeatherChanged(list[0].weatherPreset, this);
			return list[0];
		}
		if (!(list[0].weatherPreset == this.currentActiveZoneWeatherPreset))
		{
			EnviroSkyMgr.instance.NotifyZoneWeatherChanged(list[0].weatherPreset, this);
			return list[0];
		}
		if (list.Count > 1)
		{
			EnviroSkyMgr.instance.NotifyZoneWeatherChanged(list[1].weatherPreset, this);
			return list[1];
		}
		EnviroSkyMgr.instance.NotifyZoneWeatherChanged(list[0].weatherPreset, this);
		return list[0];
	}

	// Token: 0x06000378 RID: 888 RVA: 0x00017C7C File Offset: 0x00015E7C
	private void WeatherUpdate()
	{
		this.nextUpdate = EnviroSkyMgr.instance.GetCurrentTimeInHours() + (double)this.WeatherUpdateIntervall;
		this.nextUpdateRealtime = Time.time + this.WeatherUpdateIntervall * 60f;
		this.BuildNewWeatherList();
		this.lastActiveZoneWeatherPrefab = this.currentActiveZoneWeatherPrefab;
		this.lastActiveZoneWeatherPreset = this.currentActiveZoneWeatherPreset;
		this.currentActiveZoneWeatherPrefab = this.PossibiltyCheck();
		this.currentActiveZoneWeatherPreset = this.currentActiveZoneWeatherPrefab.weatherPreset;
		EnviroSkyMgr.instance.NotifyZoneWeatherChanged(this.currentActiveZoneWeatherPreset, this);
	}

	// Token: 0x06000379 RID: 889 RVA: 0x00017D05 File Offset: 0x00015F05
	private IEnumerator CreateWeatherListLate()
	{
		yield return 0;
		this.CreateZoneWeatherTypeList();
		this.init = true;
		yield break;
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00017D14 File Offset: 0x00015F14
	private void LateUpdate()
	{
		if (EnviroSkyMgr.instance == null)
		{
			Debug.Log("No EnviroSky instance found!");
			return;
		}
		if (EnviroSkyMgr.instance.IsStarted() && !this.init)
		{
			if (this.zoneWeatherPresets.Count < 1)
			{
				Debug.Log("Zone with no Presets! Please assign at least one preset. Deactivated for now!");
				base.enabled = false;
				return;
			}
			if (this.isDefault)
			{
				this.CreateZoneWeatherTypeList();
				this.init = true;
			}
			else
			{
				base.StartCoroutine(this.CreateWeatherListLate());
			}
		}
		if (this.updateMode == EnviroZone.WeatherUpdateMode.GameTimeHours)
		{
			if (EnviroSkyMgr.instance.GetCurrentTimeInHours() > this.nextUpdate && EnviroSkyMgr.instance.IsAutoWeatherUpdateActive() && EnviroSkyMgr.instance.IsStarted())
			{
				this.WeatherUpdate();
			}
		}
		else if (Time.time > this.nextUpdateRealtime && EnviroSkyMgr.instance.IsAutoWeatherUpdateActive() && EnviroSkyMgr.instance.IsStarted())
		{
			this.WeatherUpdate();
		}
		if (EnviroSkyMgr.instance.Player == null)
		{
			return;
		}
		if (this.isDefault && this.init && !this.useMeshZone)
		{
			this.zoneCollider.center = new Vector3(0f, (EnviroSkyMgr.instance.Player.transform.position.y - base.transform.position.y) / base.transform.lossyScale.y, 0f);
		}
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00017E78 File Offset: 0x00016078
	private void OnTriggerEnter(Collider col)
	{
		if (EnviroSkyMgr.instance == null)
		{
			return;
		}
		if (EnviroSkyMgr.instance.GetUseWeatherTag())
		{
			if (col.gameObject.tag == EnviroSkyMgr.instance.GetEnviroSkyTag())
			{
				EnviroSkyMgr.instance.SetCurrentActiveZone(this);
				EnviroSkyMgr.instance.NotifyZoneChanged(this);
				return;
			}
		}
		else if (EnviroSkyMgr.instance.IsEnviroSkyAttached(col.gameObject))
		{
			EnviroSkyMgr.instance.SetCurrentActiveZone(this);
			EnviroSkyMgr.instance.NotifyZoneChanged(this);
		}
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00017EFC File Offset: 0x000160FC
	private void OnTriggerExit(Collider col)
	{
		if (!this.ExitToDefault || EnviroSkyMgr.instance == null)
		{
			return;
		}
		if (EnviroSkyMgr.instance.GetUseWeatherTag())
		{
			if (col.gameObject.tag == EnviroSkyMgr.instance.GetEnviroSkyTag())
			{
				EnviroSkyMgr.instance.SetToZone(0);
				EnviroSkyMgr.instance.NotifyZoneChanged(EnviroSkyMgr.instance.GetZoneByID(0));
				return;
			}
		}
		else if (EnviroSkyMgr.instance.IsEnviroSkyAttached(col.gameObject))
		{
			EnviroSkyMgr.instance.SetToZone(0);
			EnviroSkyMgr.instance.NotifyZoneChanged(EnviroSkyMgr.instance.GetZoneByID(0));
		}
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00017F9C File Offset: 0x0001619C
	private void OnDrawGizmos()
	{
		Gizmos.color = this.zoneGizmoColor;
		Gizmos.matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one);
		if (this.useMeshZone && this.zoneMesh != null)
		{
			Gizmos.DrawMesh(this.zoneMesh);
			return;
		}
		Gizmos.DrawCube(Vector3.zero, new Vector3(this.zoneScale.x, this.zoneScale.y, this.zoneScale.z));
	}

	// Token: 0x040004FC RID: 1276
	[Tooltip("Defines the zone name.")]
	public string zoneName;

	// Token: 0x040004FD RID: 1277
	[Tooltip("Uncheck to remove OnTriggerExit call when using overlapping zone layout.")]
	public bool ExitToDefault = true;

	// Token: 0x040004FE RID: 1278
	[Tooltip("Enable this option to force to change weather preset. Disable that option and same weather can occure in row.")]
	public bool forceWeatherChange;

	// Token: 0x040004FF RID: 1279
	public List<EnviroWeatherPrefab> zoneWeather = new List<EnviroWeatherPrefab>();

	// Token: 0x04000500 RID: 1280
	public List<EnviroWeatherPrefab> curPossibleZoneWeather;

	// Token: 0x04000501 RID: 1281
	[Header("Zone weather settings:")]
	[Tooltip("Add all weather prefabs for this zone here.")]
	public List<EnviroWeatherPreset> zoneWeatherPresets = new List<EnviroWeatherPreset>();

	// Token: 0x04000502 RID: 1282
	[Tooltip("Shall weather changes occure based on gametime or realtime?")]
	public EnviroZone.WeatherUpdateMode updateMode;

	// Token: 0x04000503 RID: 1283
	[Tooltip("Defines how often (gametime hours or realtime minutes) the system will heck to change the current weather conditions.")]
	public float WeatherUpdateIntervall = 6f;

	// Token: 0x04000504 RID: 1284
	[Header("Zone scaling and gizmo:")]
	[Tooltip("Enable this to use a mesh for zone trigger.")]
	public bool useMeshZone;

	// Token: 0x04000505 RID: 1285
	[Tooltip("Custom Zone Mesh")]
	public Mesh zoneMesh;

	// Token: 0x04000506 RID: 1286
	[Tooltip("Defines the zone scale.")]
	public Vector3 zoneScale = new Vector3(100f, 100f, 100f);

	// Token: 0x04000507 RID: 1287
	[Tooltip("Defines the color of the zone's gizmo in editor mode.")]
	public Color zoneGizmoColor = Color.gray;

	// Token: 0x04000508 RID: 1288
	[Header("Current active weather:")]
	[Tooltip("The current active weather conditions.")]
	public EnviroWeatherPrefab currentActiveZoneWeatherPrefab;

	// Token: 0x04000509 RID: 1289
	public EnviroWeatherPreset currentActiveZoneWeatherPreset;

	// Token: 0x0400050A RID: 1290
	[HideInInspector]
	public EnviroWeatherPrefab lastActiveZoneWeatherPrefab;

	// Token: 0x0400050B RID: 1291
	[HideInInspector]
	public EnviroWeatherPreset lastActiveZoneWeatherPreset;

	// Token: 0x0400050C RID: 1292
	private BoxCollider zoneCollider;

	// Token: 0x0400050D RID: 1293
	private MeshCollider zoneMeshCollider;

	// Token: 0x0400050E RID: 1294
	private double nextUpdate;

	// Token: 0x0400050F RID: 1295
	private float nextUpdateRealtime;

	// Token: 0x04000510 RID: 1296
	public bool init;

	// Token: 0x04000511 RID: 1297
	private bool isDefault;

	// Token: 0x020000B0 RID: 176
	public enum WeatherUpdateMode
	{
		// Token: 0x04000513 RID: 1299
		GameTimeHours,
		// Token: 0x04000514 RID: 1300
		RealTimeMinutes
	}
}
