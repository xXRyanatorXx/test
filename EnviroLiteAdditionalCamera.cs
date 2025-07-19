using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C4 RID: 196
[ExecuteInEditMode]
[AddComponentMenu("Enviro/Lite/AddionalCamera")]
public class EnviroLiteAdditionalCamera : MonoBehaviour
{
	// Token: 0x060003F6 RID: 1014 RVA: 0x0001C52D File Offset: 0x0001A72D
	private void OnEnable()
	{
		this.myCam = base.GetComponent<Camera>();
		if (this.myCam != null)
		{
			this.InitImageEffects();
		}
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0001C54F File Offset: 0x0001A74F
	private void Start()
	{
		if (this.addWeatherEffects)
		{
			this.CreateEffectHolder();
			base.StartCoroutine(this.SetupWeatherEffects());
		}
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x0001C56C File Offset: 0x0001A76C
	private void Update()
	{
		if (this.addWeatherEffects)
		{
			this.UpdateWeatherEffects();
		}
		if (EnviroSkyLite.instance != null)
		{
			this.UpdateSkyRenderer();
		}
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0001C590 File Offset: 0x0001A790
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

	// Token: 0x060003FA RID: 1018 RVA: 0x0001C66A File Offset: 0x0001A86A
	private IEnumerator SetupWeatherEffects()
	{
		yield return new WaitForSeconds(1f);
		for (int i = 0; i < EnviroSkyMgr.instance.Weather.weatherPresets.Count; i++)
		{
			GameObject gameObject = new GameObject();
			EnviroWeatherPrefab enviroWeatherPrefab = gameObject.AddComponent<EnviroWeatherPrefab>();
			enviroWeatherPrefab.weatherPreset = EnviroSkyMgr.instance.Weather.weatherPresets[i];
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
		if (EnviroSkyMgr.instance.Weather.currentActiveWeatherPrefab != null)
		{
			for (int m = 0; m < this.zoneWeather.Count; m++)
			{
				if (this.zoneWeather[m].weatherPreset == EnviroSkyMgr.instance.Weather.currentActiveWeatherPrefab.weatherPreset)
				{
					this.currentWeather = this.zoneWeather[m];
				}
			}
		}
		yield break;
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0001C67C File Offset: 0x0001A87C
	private void UpdateWeatherEffects()
	{
		if (EnviroSkyMgr.instance.Weather.currentActiveWeatherPrefab == null || this.currentWeather == null)
		{
			return;
		}
		if (EnviroSkyMgr.instance.Weather.currentActiveWeatherPrefab.weatherPreset != this.currentWeather.weatherPreset)
		{
			for (int i = 0; i < this.zoneWeather.Count; i++)
			{
				if (this.zoneWeather[i].weatherPreset == EnviroSkyMgr.instance.Weather.currentActiveWeatherPrefab.weatherPreset)
				{
					this.currentWeather = this.zoneWeather[i];
				}
			}
		}
		this.UpdateEffectSystems(this.currentWeather, true);
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0001C738 File Offset: 0x0001A938
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
				float emissionRate = Mathf.Lerp(EnviroSkyMgr.instance.GetEmissionRate(id.effectSystems[i]), id.effectEmmisionRates[i] * EnviroSkyLite.instance.qualitySettings.GlobalParticleEmissionRates, t) * EnviroSkyMgr.instance.InteriorZoneSettings.currentInteriorWeatherEffectMod;
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

	// Token: 0x060003FD RID: 1021 RVA: 0x0001C928 File Offset: 0x0001AB28
	private void InitImageEffects()
	{
		if (this.addEnviroSkyRendering)
		{
			this.skyRender = this.myCam.gameObject.GetComponent<EnviroSkyRenderingLW>();
			if (this.skyRender == null)
			{
				this.skyRender = this.myCam.gameObject.AddComponent<EnviroSkyRenderingLW>();
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

	// Token: 0x060003FE RID: 1022 RVA: 0x0001C9C8 File Offset: 0x0001ABC8
	private void UpdateSkyRenderer()
	{
		if (EnviroSkyMgr.instance.FogSettings.useUnityFog && EnviroSkyMgr.instance.Camera != null && EnviroSkyMgr.instance.Camera.renderingPath == RenderingPath.Forward)
		{
			RenderSettings.fog = true;
			if (this.skyRender != null && this.skyRender.isActiveAndEnabled)
			{
				this.skyRender.enabled = false;
				return;
			}
		}
		else
		{
			if (EnviroSkyLite.instance.usePostEffectFog && this.skyRender != null && !this.skyRender.isActiveAndEnabled)
			{
				this.skyRender.enabled = true;
				return;
			}
			if (!EnviroSkyLite.instance.usePostEffectFog && this.skyRender != null && this.skyRender.isActiveAndEnabled)
			{
				this.skyRender.enabled = false;
			}
		}
	}

	// Token: 0x04000587 RID: 1415
	public bool addEnviroSkyRendering = true;

	// Token: 0x04000588 RID: 1416
	public bool addEnviroSkyPostProcessing = true;

	// Token: 0x04000589 RID: 1417
	public bool addWeatherEffects = true;

	// Token: 0x0400058A RID: 1418
	private Camera myCam;

	// Token: 0x0400058B RID: 1419
	private EnviroSkyRenderingLW skyRender;

	// Token: 0x0400058C RID: 1420
	private EnviroPostProcessing enviroPostProcessing;

	// Token: 0x0400058D RID: 1421
	private GameObject EffectHolder;

	// Token: 0x0400058E RID: 1422
	private GameObject VFX;

	// Token: 0x0400058F RID: 1423
	private List<EnviroWeatherPrefab> zoneWeather = new List<EnviroWeatherPrefab>();

	// Token: 0x04000590 RID: 1424
	private EnviroWeatherPrefab currentWeather;
}
