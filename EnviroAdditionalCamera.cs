using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D5 RID: 213
[ExecuteInEditMode]
[AddComponentMenu("Enviro/Standard/AddionalCamera")]
public class EnviroAdditionalCamera : MonoBehaviour
{
	// Token: 0x06000486 RID: 1158 RVA: 0x00026A20 File Offset: 0x00024C20
	private void OnEnable()
	{
		this.myCam = base.GetComponent<Camera>();
		if (this.myCam != null)
		{
			this.InitImageEffects();
		}
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x00026A42 File Offset: 0x00024C42
	private void Start()
	{
		if (this.addWeatherEffects)
		{
			this.CreateEffectHolder();
			base.StartCoroutine(this.SetupWeatherEffects());
		}
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x00026A5F File Offset: 0x00024C5F
	private void Update()
	{
		if (this.addWeatherEffects)
		{
			this.UpdateWeatherEffects();
		}
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x00026A70 File Offset: 0x00024C70
	private void CreateEffectHolder()
	{
		for (int i = this.myCam.transform.childCount - 1; i >= 0; i--)
		{
			if (this.myCam.transform.GetChild(i).gameObject.name == "Effect Holder")
			{
				UnityEngine.Object.DestroyImmediate(this.myCam.transform.GetChild(i).gameObject);
			}
		}
		this.EffectHolder = new GameObject();
		this.EffectHolder.name = "Effect Holder";
		this.EffectHolder.transform.SetParent(this.myCam.transform, false);
		this.VFX = new GameObject();
		this.VFX.name = "VFX";
		this.VFX.transform.SetParent(this.EffectHolder.transform, false);
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x00026B4A File Offset: 0x00024D4A
	private IEnumerator SetupWeatherEffects()
	{
		yield return new WaitForSeconds(1f);
		for (int i = 0; i < EnviroSky.instance.Weather.weatherPresets.Count; i++)
		{
			GameObject gameObject = new GameObject();
			EnviroWeatherPrefab enviroWeatherPrefab = gameObject.AddComponent<EnviroWeatherPrefab>();
			enviroWeatherPrefab.weatherPreset = EnviroSky.instance.Weather.weatherPresets[i];
			gameObject.name = enviroWeatherPrefab.weatherPreset.Name;
			for (int j = 0; j < enviroWeatherPrefab.weatherPreset.effectSystems.Count; j++)
			{
				if (enviroWeatherPrefab.weatherPreset.effectSystems[j] == null || enviroWeatherPrefab.weatherPreset.effectSystems[j].prefab == null)
				{
					Debug.Log("Warning! Missing Particle System Entry: " + enviroWeatherPrefab.weatherPreset.Name);
					UnityEngine.Object.Destroy(gameObject);
					break;
				}
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(enviroWeatherPrefab.weatherPreset.effectSystems[j].prefab, gameObject.transform);
				gameObject2.transform.localPosition = enviroWeatherPrefab.weatherPreset.effectSystems[j].localPositionOffset;
				gameObject2.transform.localEulerAngles = enviroWeatherPrefab.weatherPreset.effectSystems[j].localRotationOffset;
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
						break;
					}
					enviroWeatherPrefab.effectSystems.Add(particleSystem);
				}
			}
			enviroWeatherPrefab.effectEmmisionRates.Clear();
			gameObject.transform.parent = this.VFX.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			this.zoneWeather.Add(enviroWeatherPrefab);
		}
		for (int k = 0; k < this.zoneWeather.Count; k++)
		{
			for (int l = 0; l < this.zoneWeather[k].effectSystems.Count; l++)
			{
				this.zoneWeather[k].effectEmmisionRates.Add(EnviroSkyMgr.instance.GetEmissionRate(this.zoneWeather[k].effectSystems[l]));
				EnviroSkyMgr.instance.SetEmissionRate(this.zoneWeather[k].effectSystems[l], 0f);
			}
		}
		if (EnviroSky.instance.Weather.currentActiveWeatherPrefab != null)
		{
			for (int m = 0; m < this.zoneWeather.Count; m++)
			{
				if (this.zoneWeather[m].weatherPreset == EnviroSky.instance.Weather.currentActiveWeatherPrefab.weatherPreset)
				{
					this.currentWeather = this.zoneWeather[m];
				}
			}
		}
		yield break;
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x00026B5C File Offset: 0x00024D5C
	private void UpdateWeatherEffects()
	{
		if (EnviroSky.instance.Weather.currentActiveWeatherPrefab == null || this.currentWeather == null)
		{
			return;
		}
		if (EnviroSky.instance.Weather.currentActiveWeatherPrefab.weatherPreset != this.currentWeather.weatherPreset)
		{
			for (int i = 0; i < this.zoneWeather.Count; i++)
			{
				if (this.zoneWeather[i].weatherPreset == EnviroSky.instance.Weather.currentActiveWeatherPrefab.weatherPreset)
				{
					this.currentWeather = this.zoneWeather[i];
				}
			}
		}
		this.UpdateEffectSystems(this.currentWeather, true);
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x00026C18 File Offset: 0x00024E18
	private void UpdateEffectSystems(EnviroWeatherPrefab id, bool withTransition)
	{
		if (id != null)
		{
			float t = 500f * Time.deltaTime;
			if (withTransition)
			{
				t = EnviroSkyMgr.instance.WeatherSettings.effectTransitionSpeed * Time.deltaTime;
			}
			for (int i = 0; i < id.effectSystems.Count; i++)
			{
				if (id.effectSystems[i].isStopped)
				{
					id.effectSystems[i].Play();
				}
				float emissionRate = Mathf.Lerp(EnviroSkyMgr.instance.GetEmissionRate(id.effectSystems[i]), id.effectEmmisionRates[i] * EnviroSky.instance.qualitySettings.GlobalParticleEmissionRates, t) * EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorWeatherEffectMod;
				EnviroSkyMgr.instance.SetEmissionRate(id.effectSystems[i], emissionRate);
			}
			for (int j = 0; j < this.zoneWeather.Count; j++)
			{
				if (this.zoneWeather[j].gameObject != id.gameObject)
				{
					for (int k = 0; k < this.zoneWeather[j].effectSystems.Count; k++)
					{
						float num = Mathf.Lerp(EnviroSkyMgr.instance.GetEmissionRate(this.zoneWeather[j].effectSystems[k]), 0f, t);
						if (num < 1f)
						{
							num = 0f;
						}
						EnviroSkyMgr.instance.SetEmissionRate(this.zoneWeather[j].effectSystems[k], num);
						if (num == 0f && !this.zoneWeather[j].effectSystems[k].isStopped)
						{
							this.zoneWeather[j].effectSystems[k].Stop();
						}
					}
				}
			}
		}
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x00026E08 File Offset: 0x00025008
	private void InitImageEffects()
	{
		if (this.addEnviroSkyRendering)
		{
			this.skyRender = this.myCam.gameObject.GetComponent<EnviroSkyRendering>();
			if (this.skyRender == null)
			{
				this.skyRender = this.myCam.gameObject.AddComponent<EnviroSkyRendering>();
			}
			this.skyRender.isAddionalCamera = true;
		}
		if (this.addEnviroSkyPostProcessing)
		{
			this.enviroPostProcessing = this.myCam.gameObject.GetComponent<EnviroPostProcessing>();
			if (this.enviroPostProcessing == null)
			{
				this.enviroPostProcessing = this.myCam.gameObject.AddComponent<EnviroPostProcessing>();
			}
		}
	}

	// Token: 0x04000648 RID: 1608
	public bool addEnviroSkyRendering = true;

	// Token: 0x04000649 RID: 1609
	public bool addEnviroSkyPostProcessing = true;

	// Token: 0x0400064A RID: 1610
	public bool addWeatherEffects = true;

	// Token: 0x0400064B RID: 1611
	private Camera myCam;

	// Token: 0x0400064C RID: 1612
	private EnviroSkyRendering skyRender;

	// Token: 0x0400064D RID: 1613
	private EnviroPostProcessing enviroPostProcessing;

	// Token: 0x0400064E RID: 1614
	private GameObject EffectHolder;

	// Token: 0x0400064F RID: 1615
	private GameObject VFX;

	// Token: 0x04000650 RID: 1616
	private List<EnviroWeatherPrefab> zoneWeather = new List<EnviroWeatherPrefab>();

	// Token: 0x04000651 RID: 1617
	private EnviroWeatherPrefab currentWeather;
}
