using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class EnviroSkyMgr : MonoBehaviour
{
	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000268 RID: 616 RVA: 0x0001431B File Offset: 0x0001251B
	public static EnviroSkyMgr instance
	{
		get
		{
			if (EnviroSkyMgr._instance == null)
			{
				EnviroSkyMgr._instance = UnityEngine.Object.FindObjectOfType<EnviroSkyMgr>();
			}
			return EnviroSkyMgr._instance;
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x00014339 File Offset: 0x00012539
	private void Start()
	{
		if (Application.isPlaying && this.dontDestroy)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00014355 File Offset: 0x00012555
	private void Awake()
	{
		if (EnviroSkyMgr.instance != this)
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00014370 File Offset: 0x00012570
	public void ActivateHDInstance()
	{
		if (this.enviroHDInstance != null)
		{
			if (this.enviroLWInstance != null)
			{
				this.enviroLWInstance.Deactivate(false);
				this.enviroLWInstance.gameObject.SetActive(false);
			}
			this.enviroHDInstance.gameObject.SetActive(true);
			this.enviroHDInstance.Activate();
			this.currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.HD;
		}
	}

	// Token: 0x0600026C RID: 620 RVA: 0x000143DC File Offset: 0x000125DC
	public void DeactivateHDInstance()
	{
		if (this.enviroHDInstance != null)
		{
			this.enviroHDInstance.Deactivate(false);
			this.enviroHDInstance.gameObject.SetActive(false);
			if (this.enviroLWInstance != null && !this.enviroLWInstance.gameObject.activeSelf)
			{
				this.currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.None;
				return;
			}
			this.currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.LW;
		}
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00014444 File Offset: 0x00012644
	public void ActivateLWInstance()
	{
		if (this.enviroLWInstance != null)
		{
			if (this.enviroHDInstance != null)
			{
				this.enviroHDInstance.Deactivate(false);
				this.enviroHDInstance.gameObject.SetActive(false);
			}
			this.enviroLWInstance.gameObject.SetActive(true);
			this.enviroLWInstance.Activate();
			this.currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.LW;
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x000144B0 File Offset: 0x000126B0
	public void DeactivateLWInstance()
	{
		if (this.enviroLWInstance != null)
		{
			this.enviroLWInstance.Deactivate(false);
			this.enviroLWInstance.gameObject.SetActive(false);
			if (this.enviroHDInstance != null && !this.enviroHDInstance.gameObject.activeSelf)
			{
				this.currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.None;
				return;
			}
			this.currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.HD;
		}
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00014518 File Offset: 0x00012718
	public void DeleteHDInstance()
	{
		if (this.enviroHDInstance != null)
		{
			UnityEngine.Object.DestroyImmediate(this.enviroHDInstance.EffectsHolder);
			UnityEngine.Object.DestroyImmediate(this.enviroHDInstance.gameObject);
			if (this.enviroHDInstance.EnviroSkyRender != null)
			{
				UnityEngine.Object.DestroyImmediate(this.enviroHDInstance.EnviroSkyRender);
			}
			if (this.enviroHDInstance.EnviroPostProcessing != null)
			{
				UnityEngine.Object.DestroyImmediate(this.enviroHDInstance.EnviroPostProcessing);
			}
			this.currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.None;
		}
	}

	// Token: 0x06000270 RID: 624 RVA: 0x000145A0 File Offset: 0x000127A0
	public void DeleteLWInstance()
	{
		if (this.enviroLWInstance != null)
		{
			UnityEngine.Object.DestroyImmediate(this.enviroLWInstance.EffectsHolder);
			UnityEngine.Object.DestroyImmediate(this.enviroLWInstance.gameObject);
			if (this.enviroLWInstance.EnviroSkyRender != null)
			{
				UnityEngine.Object.DestroyImmediate(this.enviroLWInstance.EnviroSkyRender);
			}
			if (this.enviroLWInstance.EnviroPostProcessing != null)
			{
				UnityEngine.Object.DestroyImmediate(this.enviroLWInstance.EnviroPostProcessing);
			}
			this.currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.None;
		}
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00014628 File Offset: 0x00012828
	public void SearchForEnviroInstances()
	{
		this.enviroHDInstance = base.GetComponentInChildren<EnviroSky>();
		this.enviroLWInstance = base.GetComponentInChildren<EnviroSkyLite>();
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00014644 File Offset: 0x00012844
	public void CreateEnviroHDInstance()
	{
		GameObject assetPrefab = this.GetAssetPrefab("Internal_Enviro_HD");
		if (assetPrefab != null && EnviroSky.instance == null)
		{
			this.DeactivateAllInstances();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(assetPrefab, Vector3.zero, Quaternion.identity);
			gameObject.name = "EnviroSky Standard";
			gameObject.transform.SetParent(base.transform);
			this.enviroHDInstance = gameObject.GetComponent<EnviroSky>();
			gameObject.SetActive(false);
			this.currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.None;
		}
	}

	// Token: 0x06000273 RID: 627 RVA: 0x000146C0 File Offset: 0x000128C0
	public void CreateEnviroHDVRInstance()
	{
		GameObject assetPrefab = this.GetAssetPrefab("Internal_Enviro_HD_VR");
		if (assetPrefab != null && EnviroSky.instance == null)
		{
			this.DeactivateAllInstances();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(assetPrefab, Vector3.zero, Quaternion.identity);
			gameObject.name = "EnviroSky Standard for VR";
			gameObject.transform.SetParent(base.transform);
			this.enviroHDInstance = gameObject.GetComponent<EnviroSky>();
			gameObject.SetActive(false);
			this.currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.None;
		}
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0001473C File Offset: 0x0001293C
	public void CreateEnviroLWInstance()
	{
		GameObject assetPrefab = this.GetAssetPrefab("Internal_Enviro_LW");
		if (assetPrefab != null && EnviroSkyLite.instance == null)
		{
			this.DeactivateAllInstances();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(assetPrefab, Vector3.zero, Quaternion.identity);
			gameObject.name = "EnviroSky Lite";
			gameObject.transform.SetParent(base.transform);
			this.enviroLWInstance = gameObject.GetComponent<EnviroSkyLite>();
			gameObject.SetActive(false);
			this.currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.None;
		}
	}

	// Token: 0x06000275 RID: 629 RVA: 0x000147B8 File Offset: 0x000129B8
	public void CreateEnviroLWMobileInstance()
	{
		GameObject assetPrefab = this.GetAssetPrefab("Internal_Enviro_LW_MOBILE");
		if (assetPrefab != null && EnviroSkyLite.instance == null)
		{
			this.DeactivateAllInstances();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(assetPrefab, Vector3.zero, Quaternion.identity);
			gameObject.name = "EnviroSky Lite for Mobiles";
			gameObject.transform.SetParent(base.transform);
			this.enviroLWInstance = gameObject.GetComponent<EnviroSkyLite>();
			gameObject.SetActive(false);
			this.currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.None;
		}
	}

	// Token: 0x06000276 RID: 630 RVA: 0x00014834 File Offset: 0x00012A34
	private void DeactivateAllInstances()
	{
		if (this.enviroHDInstance != null)
		{
			this.DeactivateHDInstance();
		}
		if (this.enviroLWInstance != null)
		{
			this.DeactivateLWInstance();
		}
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00012CCD File Offset: 0x00010ECD
	public GameObject GetAssetPrefab(string name)
	{
		return null;
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000278 RID: 632 RVA: 0x0001485E File Offset: 0x00012A5E
	// (set) Token: 0x06000279 RID: 633 RVA: 0x0001487E File Offset: 0x00012A7E
	public EnviroComponents Components
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.Components;
			}
			return EnviroSkyLite.instance.Components;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.Components = value;
				return;
			}
			EnviroSkyLite.instance.Components = value;
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x0600027A RID: 634 RVA: 0x000148A0 File Offset: 0x00012AA0
	// (set) Token: 0x0600027B RID: 635 RVA: 0x000148C0 File Offset: 0x00012AC0
	public EnviroQualitySettings QualitySettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.qualitySettings;
			}
			return EnviroSkyLite.instance.qualitySettings;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.qualitySettings = value;
				return;
			}
			EnviroSkyLite.instance.qualitySettings = value;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x0600027C RID: 636 RVA: 0x000148E2 File Offset: 0x00012AE2
	// (set) Token: 0x0600027D RID: 637 RVA: 0x00014902 File Offset: 0x00012B02
	public EnviroTime Time
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.GameTime;
			}
			return EnviroSkyLite.instance.GameTime;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.GameTime = value;
				return;
			}
			EnviroSkyLite.instance.GameTime = value;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x0600027E RID: 638 RVA: 0x00014924 File Offset: 0x00012B24
	// (set) Token: 0x0600027F RID: 639 RVA: 0x00014944 File Offset: 0x00012B44
	public EnviroSeasons Seasons
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.Seasons;
			}
			return EnviroSkyLite.instance.Seasons;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.Seasons = value;
				return;
			}
			EnviroSkyLite.instance.Seasons = value;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000280 RID: 640 RVA: 0x00014966 File Offset: 0x00012B66
	// (set) Token: 0x06000281 RID: 641 RVA: 0x00014986 File Offset: 0x00012B86
	public EnviroSeasonSettings SeasonSettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.seasonsSettings;
			}
			return EnviroSkyLite.instance.seasonsSettings;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.seasonsSettings = value;
				return;
			}
			EnviroSkyLite.instance.seasonsSettings = value;
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000282 RID: 642 RVA: 0x000149A8 File Offset: 0x00012BA8
	// (set) Token: 0x06000283 RID: 643 RVA: 0x000149BF File Offset: 0x00012BBF
	public EnviroAuroraSettings AuroraSettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.auroraSettings;
			}
			return null;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.auroraSettings = value;
				return;
			}
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000284 RID: 644 RVA: 0x000149D6 File Offset: 0x00012BD6
	// (set) Token: 0x06000285 RID: 645 RVA: 0x000149F6 File Offset: 0x00012BF6
	public EnviroReflectionSettings ReflectionSettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.reflectionSettings;
			}
			return EnviroSkyLite.instance.reflectionSettings;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.reflectionSettings = value;
				return;
			}
			EnviroSkyLite.instance.reflectionSettings = value;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000286 RID: 646 RVA: 0x00014A18 File Offset: 0x00012C18
	// (set) Token: 0x06000287 RID: 647 RVA: 0x00014A38 File Offset: 0x00012C38
	public EnviroCloudSettings CloudSettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.cloudsSettings;
			}
			return EnviroSkyLite.instance.cloudsSettings;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.cloudsSettings = value;
				return;
			}
			EnviroSkyLite.instance.cloudsSettings = value;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000288 RID: 648 RVA: 0x00014A5A File Offset: 0x00012C5A
	// (set) Token: 0x06000289 RID: 649 RVA: 0x00014A7A File Offset: 0x00012C7A
	public EnviroInteriorZoneSettings InteriorZoneSettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.interiorZoneSettings;
			}
			return EnviroSkyLite.instance.interiorZoneSettings;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.interiorZoneSettings = value;
				return;
			}
			EnviroSkyLite.instance.interiorZoneSettings = value;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x0600028A RID: 650 RVA: 0x00014A9C File Offset: 0x00012C9C
	// (set) Token: 0x0600028B RID: 651 RVA: 0x00014ABC File Offset: 0x00012CBC
	public EnviroAudio AudioSettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.Audio;
			}
			return EnviroSkyLite.instance.Audio;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.Audio = value;
				return;
			}
			EnviroSkyLite.instance.Audio = value;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x0600028C RID: 652 RVA: 0x00014ADE File Offset: 0x00012CDE
	// (set) Token: 0x0600028D RID: 653 RVA: 0x00014AFE File Offset: 0x00012CFE
	public EnviroWeatherCloudsConfig Clouds
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.cloudsConfig;
			}
			return EnviroSkyLite.instance.cloudsConfig;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.cloudsConfig = value;
				return;
			}
			EnviroSkyLite.instance.cloudsConfig = value;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x0600028E RID: 654 RVA: 0x00014B20 File Offset: 0x00012D20
	// (set) Token: 0x0600028F RID: 655 RVA: 0x00014B40 File Offset: 0x00012D40
	public EnviroWeatherSettings WeatherSettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.weatherSettings;
			}
			return EnviroSkyLite.instance.weatherSettings;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.weatherSettings = value;
				return;
			}
			EnviroSkyLite.instance.weatherSettings = value;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000290 RID: 656 RVA: 0x00014B62 File Offset: 0x00012D62
	// (set) Token: 0x06000291 RID: 657 RVA: 0x00014B82 File Offset: 0x00012D82
	public EnviroLightSettings LightSettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.lightSettings;
			}
			return EnviroSkyLite.instance.lightSettings;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.lightSettings = value;
				return;
			}
			EnviroSkyLite.instance.lightSettings = value;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000292 RID: 658 RVA: 0x00014BA4 File Offset: 0x00012DA4
	// (set) Token: 0x06000293 RID: 659 RVA: 0x00014BC4 File Offset: 0x00012DC4
	public EnviroVolumeLightingSettings VolumeLightSettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.volumeLightSettings;
			}
			return EnviroSkyLite.instance.volumeLightSettings;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.volumeLightSettings = value;
				return;
			}
			EnviroSkyLite.instance.volumeLightSettings = value;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000294 RID: 660 RVA: 0x00014BE6 File Offset: 0x00012DE6
	// (set) Token: 0x06000295 RID: 661 RVA: 0x00014C06 File Offset: 0x00012E06
	public EnviroLightShaftsSettings LightShaftsSettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.lightshaftsSettings;
			}
			return EnviroSkyLite.instance.lightshaftsSettings;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.lightshaftsSettings = value;
				return;
			}
			EnviroSkyLite.instance.lightshaftsSettings = value;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000296 RID: 662 RVA: 0x00014C28 File Offset: 0x00012E28
	// (set) Token: 0x06000297 RID: 663 RVA: 0x00014C48 File Offset: 0x00012E48
	public EnviroSkySettings SkySettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.skySettings;
			}
			return EnviroSkyLite.instance.skySettings;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.skySettings = value;
				return;
			}
			EnviroSkyLite.instance.skySettings = value;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000298 RID: 664 RVA: 0x00014C6A File Offset: 0x00012E6A
	// (set) Token: 0x06000299 RID: 665 RVA: 0x00014C8A File Offset: 0x00012E8A
	public EnviroFogSettings FogSettings
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.fogSettings;
			}
			return EnviroSkyLite.instance.fogSettings;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.fogSettings = value;
				return;
			}
			EnviroSkyLite.instance.fogSettings = value;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x0600029A RID: 666 RVA: 0x00014CAC File Offset: 0x00012EAC
	// (set) Token: 0x0600029B RID: 667 RVA: 0x00014CCC File Offset: 0x00012ECC
	public GameObject Player
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.Player;
			}
			return EnviroSkyLite.instance.Player;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.Player = value;
				return;
			}
			EnviroSkyLite.instance.Player = value;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x0600029C RID: 668 RVA: 0x00014CEE File Offset: 0x00012EEE
	// (set) Token: 0x0600029D RID: 669 RVA: 0x00014D0E File Offset: 0x00012F0E
	public Camera Camera
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.PlayerCamera;
			}
			return EnviroSkyLite.instance.PlayerCamera;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.PlayerCamera = value;
				return;
			}
			EnviroSkyLite.instance.PlayerCamera = value;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600029E RID: 670 RVA: 0x00014D30 File Offset: 0x00012F30
	// (set) Token: 0x0600029F RID: 671 RVA: 0x00014D50 File Offset: 0x00012F50
	public bool Tonemapping
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.tonemapping;
			}
			return EnviroSkyLite.instance.tonemapping;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.tonemapping = value;
				return;
			}
			EnviroSkyLite.instance.tonemapping = value;
		}
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x00014D72 File Offset: 0x00012F72
	public void AssignAndStart(GameObject Player, Camera cam)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.AssignAndStart(Player, cam);
			return;
		}
		EnviroSkyLite.instance.AssignAndStart(Player, cam);
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00014D96 File Offset: 0x00012F96
	public void ChangeFocus(GameObject Player, Camera cam)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.ChangeFocus(Player, cam);
			return;
		}
		EnviroSkyLite.instance.ChangeFocus(Player, cam);
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x00014DBA File Offset: 0x00012FBA
	public void StartAsServer()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.StartAsServer();
			return;
		}
		EnviroSkyLite.instance.StartAsServer();
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00014DDA File Offset: 0x00012FDA
	public void ReInit()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.ReInit();
			return;
		}
		EnviroSkyLite.instance.ReInit();
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00014DFA File Offset: 0x00012FFA
	public void SetupSkybox()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.SetupSkybox();
			return;
		}
		EnviroSkyLite.instance.SetupSkybox();
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00014E1A File Offset: 0x0001301A
	public bool IsNight()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.isNight;
		}
		return EnviroSkyLite.instance.isNight;
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00014E3A File Offset: 0x0001303A
	public bool IsStarted()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.started;
		}
		return EnviroSkyLite.instance.started;
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00014E5A File Offset: 0x0001305A
	public bool HasInstance()
	{
		return (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD && EnviroSky.instance != null) || (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.LW && EnviroSkyLite.instance != null);
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00014E8B File Offset: 0x0001308B
	public bool IsInterior()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.interiorMode;
		}
		return EnviroSkyLite.instance.interiorMode;
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x00014EAB File Offset: 0x000130AB
	public bool IsEnviroSkyAttached(GameObject obj)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return obj.GetComponent<EnviroSky>();
		}
		return obj.GetComponent<EnviroSkyLite>();
	}

	// Token: 0x060002AA RID: 682 RVA: 0x00014ECD File Offset: 0x000130CD
	public bool IsDefaultZone(GameObject zone)
	{
		return zone.GetComponent<EnviroSky>() || zone.GetComponent<EnviroSkyLite>();
	}

	// Token: 0x060002AB RID: 683 RVA: 0x00014EEC File Offset: 0x000130EC
	public bool IsAutoWeatherUpdateActive()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.Weather.updateWeather;
		}
		return EnviroSkyLite.instance.Weather.updateWeather;
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00014F16 File Offset: 0x00013116
	public bool IsAvailable()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return !(EnviroSky.instance == null);
		}
		return !(EnviroSkyLite.instance == null);
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060002AD RID: 685 RVA: 0x00014F42 File Offset: 0x00013142
	// (set) Token: 0x060002AE RID: 686 RVA: 0x00014F62 File Offset: 0x00013162
	public EnviroWeather Weather
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.Weather;
			}
			return EnviroSkyLite.instance.Weather;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.Weather = value;
				return;
			}
			EnviroSkyLite.instance.Weather = value;
		}
	}

	// Token: 0x060002AF RID: 687 RVA: 0x00014F84 File Offset: 0x00013184
	public bool GetUseWeatherTag()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.weatherSettings.useTag;
		}
		return EnviroSkyLite.instance.weatherSettings.useTag;
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x00014FAE File Offset: 0x000131AE
	public string GetEnviroSkyTag()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.tag;
		}
		return EnviroSkyLite.instance.tag;
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00014FCE File Offset: 0x000131CE
	public float GetSnowIntensity()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.Weather.curSnowStrength;
		}
		return EnviroSkyLite.instance.Weather.curSnowStrength;
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00014FF8 File Offset: 0x000131F8
	public float GetWetnessIntensity()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.Weather.curWetness;
		}
		return EnviroSkyLite.instance.Weather.curWetness;
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00015024 File Offset: 0x00013224
	public string GetCurrentTemperatureString()
	{
		int num;
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			num = (int)EnviroSky.instance.Weather.currentTemperature;
		}
		else
		{
			num = (int)EnviroSkyLite.instance.Weather.currentTemperature;
		}
		return num.ToString() + "°C";
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060002B4 RID: 692 RVA: 0x00015071 File Offset: 0x00013271
	// (set) Token: 0x060002B5 RID: 693 RVA: 0x00015091 File Offset: 0x00013291
	public float CustomFogIntensity
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.customFogIntensity;
			}
			return EnviroSkyLite.instance.customFogIntensity;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.customFogIntensity = value;
				return;
			}
			EnviroSkyLite.instance.customFogIntensity = value;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060002B6 RID: 694 RVA: 0x000150B3 File Offset: 0x000132B3
	// (set) Token: 0x060002B7 RID: 695 RVA: 0x000150D3 File Offset: 0x000132D3
	public Color CustomFogColor
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.customFogColor;
			}
			return EnviroSkyLite.instance.customFogColor;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.customFogColor = value;
				return;
			}
			EnviroSkyLite.instance.customFogColor = value;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060002B8 RID: 696 RVA: 0x000150F5 File Offset: 0x000132F5
	// (set) Token: 0x060002B9 RID: 697 RVA: 0x00015115 File Offset: 0x00013315
	public bool UpdateFogIntensity
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.updateFogDensity;
			}
			return EnviroSkyLite.instance.updateFogDensity;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.updateFogDensity = value;
				return;
			}
			EnviroSkyLite.instance.updateFogDensity = value;
		}
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00015137 File Offset: 0x00013337
	public EnviroZone GetZoneByID(int id)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.Weather.zones[id];
		}
		return EnviroSkyLite.instance.Weather.zones[id];
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0001516D File Offset: 0x0001336D
	public void RegisterZone(EnviroZone z)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.RegisterZone(z);
			return;
		}
		EnviroSkyLite.instance.RegisterZone(z);
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00015190 File Offset: 0x00013390
	public float GetUniversalTimeOfDay()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.internalHour - (float)EnviroSky.instance.GameTime.utcOffset;
		}
		return EnviroSkyLite.instance.internalHour - (float)EnviroSkyLite.instance.GameTime.utcOffset;
	}

	// Token: 0x060002BD RID: 701 RVA: 0x000151DD File Offset: 0x000133DD
	public float GetTimeOfDay()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.internalHour;
		}
		return EnviroSkyLite.instance.internalHour;
	}

	// Token: 0x060002BE RID: 702 RVA: 0x000151FD File Offset: 0x000133FD
	public double GetCurrentTimeInHours()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.currentTimeInHours;
		}
		return EnviroSkyLite.instance.currentTimeInHours;
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0001521D File Offset: 0x0001341D
	public EnviroSeasons.Seasons GetCurrentSeason()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.Seasons.currentSeasons;
		}
		return EnviroSkyLite.instance.Seasons.currentSeasons;
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00015247 File Offset: 0x00013447
	public void SetYears(int year)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.GameTime.Years = year;
			return;
		}
		EnviroSkyLite.instance.GameTime.Years = year;
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00015273 File Offset: 0x00013473
	public void SetDays(int days)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.GameTime.Days = days;
			return;
		}
		EnviroSkyLite.instance.GameTime.Days = days;
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x0001529F File Offset: 0x0001349F
	public void SetTime(DateTime date)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.SetTime(date);
			return;
		}
		EnviroSkyLite.instance.SetTime(date);
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x000152C1 File Offset: 0x000134C1
	public void SetTime(int year, int dayOfYear, int hour, int minute, int seconds)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.SetTime(year, dayOfYear, hour, minute, seconds);
			return;
		}
		EnviroSkyLite.instance.SetTime(year, dayOfYear, hour, minute, seconds);
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x000152EF File Offset: 0x000134EF
	public void ResetHourEventTimer()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.ResetHourEventTimer();
			return;
		}
		EnviroSkyLite.instance.ResetHourEventTimer();
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0001530F File Offset: 0x0001350F
	public void SetTimeOfDay(float timeOfDay)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.SetInternalTimeOfDay(timeOfDay);
			return;
		}
		EnviroSkyLite.instance.SetInternalTimeOfDay(timeOfDay);
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x00015331 File Offset: 0x00013531
	public void ChangeSeason(EnviroSeasons.Seasons s)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.ChangeSeason(s);
			return;
		}
		EnviroSkyLite.instance.ChangeSeason(s);
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x00015353 File Offset: 0x00013553
	public void SetTimeProgress(EnviroTime.TimeProgressMode tpm)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.GameTime.ProgressTime = tpm;
			return;
		}
		EnviroSkyLite.instance.GameTime.ProgressTime = tpm;
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00015380 File Offset: 0x00013580
	public string GetTimeStringWithSeconds()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return string.Format("{0:00}:{1:00}:{2:00}", EnviroSky.instance.GameTime.Hours, EnviroSky.instance.GameTime.Minutes, EnviroSky.instance.GameTime.Seconds);
		}
		return string.Format("{0:00}:{1:00}:{2:00}", EnviroSkyLite.instance.GameTime.Hours, EnviroSkyLite.instance.GameTime.Minutes, EnviroSkyLite.instance.GameTime.Seconds);
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00015424 File Offset: 0x00013624
	public string GetTimeString()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return string.Format("{0:00}:{1:00}", EnviroSky.instance.GameTime.Hours, EnviroSky.instance.GameTime.Minutes);
		}
		return string.Format("{0:00}:{1:00}", EnviroSkyLite.instance.GameTime.Hours, EnviroSkyLite.instance.GameTime.Minutes);
	}

	// Token: 0x060002CA RID: 714 RVA: 0x0001549F File Offset: 0x0001369F
	public int GetCurrentYear()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.GameTime.Years;
		}
		return EnviroSkyLite.instance.GameTime.Years;
	}

	// Token: 0x060002CB RID: 715 RVA: 0x000154CC File Offset: 0x000136CC
	public int GetCurrentMonth()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			DateTime dateTime = default(DateTime);
			DateTime dateTime2 = dateTime.AddYears(EnviroSky.instance.GameTime.Years);
			return dateTime.AddDays((double)EnviroSky.instance.GameTime.Days).Month;
		}
		DateTime dateTime3 = default(DateTime);
		DateTime dateTime4 = dateTime3.AddYears(EnviroSkyLite.instance.GameTime.Years);
		return dateTime3.AddDays((double)EnviroSkyLite.instance.GameTime.Days).Month;
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00015560 File Offset: 0x00013760
	public DateTime GetDateAsDateTime()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return default(DateTime).AddYears(EnviroSky.instance.GameTime.Years - 1).AddDays((double)(EnviroSky.instance.GameTime.Days - 1)).AddHours((double)EnviroSky.instance.GameTime.Hours).AddMinutes((double)EnviroSky.instance.GameTime.Minutes).AddSeconds((double)EnviroSky.instance.GameTime.Seconds);
		}
		return default(DateTime).AddYears(EnviroSkyLite.instance.GameTime.Years - 1).AddDays((double)(EnviroSkyLite.instance.GameTime.Days - 1)).AddHours((double)EnviroSkyLite.instance.GameTime.Hours).AddMinutes((double)EnviroSkyLite.instance.GameTime.Minutes).AddSeconds((double)EnviroSkyLite.instance.GameTime.Seconds);
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00015682 File Offset: 0x00013882
	public int GetCurrentDay()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.GameTime.Days;
		}
		return EnviroSkyLite.instance.GameTime.Days;
	}

	// Token: 0x060002CE RID: 718 RVA: 0x000156AC File Offset: 0x000138AC
	public int GetCurrentHour()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.GameTime.Hours;
		}
		return EnviroSkyLite.instance.GameTime.Hours;
	}

	// Token: 0x060002CF RID: 719 RVA: 0x000156D6 File Offset: 0x000138D6
	public int GetCurrentMinute()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.GameTime.Minutes;
		}
		return EnviroSkyLite.instance.GameTime.Minutes;
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00015700 File Offset: 0x00013900
	public int GetCurrentSecond()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.GameTime.Seconds;
		}
		return EnviroSkyLite.instance.GameTime.Seconds;
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x0001572A File Offset: 0x0001392A
	public void ChangeWeatherInstant(int weatherId)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.SetWeatherOverwrite(weatherId);
			return;
		}
		EnviroSkyLite.instance.SetWeatherOverwrite(weatherId);
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x0001574C File Offset: 0x0001394C
	public void ChangeWeatherInstant(EnviroWeatherPreset preset)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.SetWeatherOverwrite(preset);
			return;
		}
		EnviroSkyLite.instance.SetWeatherOverwrite(preset);
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x0001576E File Offset: 0x0001396E
	public void ChangeWeather(int weatherId)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.ChangeWeather(weatherId);
			return;
		}
		EnviroSkyLite.instance.ChangeWeather(weatherId);
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00015790 File Offset: 0x00013990
	public void ChangeWeather(EnviroWeatherPreset preset)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.ChangeWeather(preset);
			return;
		}
		EnviroSkyLite.instance.ChangeWeather(preset);
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x000157B2 File Offset: 0x000139B2
	public void ChangeWeather(string Name)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.ChangeWeather(Name);
			return;
		}
		EnviroSkyLite.instance.ChangeWeather(Name);
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x000157D4 File Offset: 0x000139D4
	public EnviroZone GetCurrentActiveZone()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.Weather.currentActiveZone;
		}
		return EnviroSkyLite.instance.Weather.currentActiveZone;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x000157FE File Offset: 0x000139FE
	public void SetCurrentActiveZone(EnviroZone z)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.Weather.currentActiveZone = z;
			return;
		}
		EnviroSkyLite.instance.Weather.currentActiveZone = z;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0001582A File Offset: 0x00013A2A
	public void InstantWeatherChange(EnviroWeatherPreset preset, EnviroWeatherPrefab prefab)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.InstantWeatherChange(preset, prefab);
			return;
		}
		EnviroSkyLite.instance.InstantWeatherChange(preset, prefab);
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00015850 File Offset: 0x00013A50
	public void SetToZone(int z)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.Weather.currentActiveZone = EnviroSky.instance.Weather.zones[z];
			return;
		}
		EnviroSkyLite.instance.Weather.currentActiveZone = EnviroSkyLite.instance.Weather.zones[z];
	}

	// Token: 0x060002DA RID: 730 RVA: 0x000158AF File Offset: 0x00013AAF
	public EnviroWeatherPreset GetCurrentWeatherPreset()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.Weather.currentActiveWeatherPreset;
		}
		return EnviroSkyLite.instance.Weather.currentActiveWeatherPreset;
	}

	// Token: 0x060002DB RID: 731 RVA: 0x000158D9 File Offset: 0x00013AD9
	public EnviroWeatherPreset GetStartWeatherPreset()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.Weather.startWeatherPreset;
		}
		return EnviroSkyLite.instance.Weather.startWeatherPreset;
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00015903 File Offset: 0x00013B03
	public List<EnviroWeatherPreset> GetCurrentWeatherPresetList()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.Weather.weatherPresets;
		}
		return EnviroSkyLite.instance.Weather.weatherPresets;
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0001592D File Offset: 0x00013B2D
	public List<EnviroWeatherPrefab> GetCurrentWeatherPrefabList()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.Weather.WeatherPrefabs;
		}
		return EnviroSkyLite.instance.Weather.WeatherPrefabs;
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00015957 File Offset: 0x00013B57
	public List<EnviroZone> GetZoneList()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.Weather.zones;
		}
		return EnviroSkyLite.instance.Weather.zones;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00015984 File Offset: 0x00013B84
	public void ChangeZoneWeather(int zoneId, int weatherId)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.Weather.zones[zoneId].currentActiveZoneWeatherPrefab = EnviroSky.instance.Weather.WeatherPrefabs[weatherId];
			EnviroSky.instance.Weather.zones[zoneId].currentActiveZoneWeatherPreset = EnviroSky.instance.Weather.WeatherPrefabs[weatherId].weatherPreset;
			return;
		}
		EnviroSkyLite.instance.Weather.zones[zoneId].currentActiveZoneWeatherPrefab = EnviroSkyLite.instance.Weather.WeatherPrefabs[weatherId];
		EnviroSkyLite.instance.Weather.zones[zoneId].currentActiveZoneWeatherPreset = EnviroSkyLite.instance.Weather.WeatherPrefabs[weatherId].weatherPreset;
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x00015A61 File Offset: 0x00013C61
	public void SetAutoWeatherUpdates(bool b)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.Weather.updateWeather = b;
			return;
		}
		EnviroSkyLite.instance.Weather.updateWeather = b;
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060002E1 RID: 737 RVA: 0x00015A8D File Offset: 0x00013C8D
	// (set) Token: 0x060002E2 RID: 738 RVA: 0x00015AB7 File Offset: 0x00013CB7
	public float ambientAudioVolume
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.Audio.ambientSFXVolume;
			}
			return EnviroSkyLite.instance.Audio.ambientSFXVolume;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.Audio.ambientSFXVolume = value;
				return;
			}
			EnviroSkyLite.instance.Audio.ambientSFXVolume = value;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060002E3 RID: 739 RVA: 0x00015AE3 File Offset: 0x00013CE3
	// (set) Token: 0x060002E4 RID: 740 RVA: 0x00015B0D File Offset: 0x00013D0D
	public float weatherAudioVolume
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.Audio.weatherSFXVolume;
			}
			return EnviroSkyLite.instance.Audio.weatherSFXVolume;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.Audio.weatherSFXVolume = value;
				return;
			}
			EnviroSkyLite.instance.Audio.weatherSFXVolume = value;
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060002E5 RID: 741 RVA: 0x00015B39 File Offset: 0x00013D39
	// (set) Token: 0x060002E6 RID: 742 RVA: 0x00015B63 File Offset: 0x00013D63
	public float ambientAudioVolumeModifier
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.Audio.ambientSFXVolumeMod;
			}
			return EnviroSkyLite.instance.Audio.ambientSFXVolumeMod;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.Audio.ambientSFXVolumeMod = value;
				return;
			}
			EnviroSkyLite.instance.Audio.ambientSFXVolumeMod = value;
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060002E7 RID: 743 RVA: 0x00015B8F File Offset: 0x00013D8F
	// (set) Token: 0x060002E8 RID: 744 RVA: 0x00015BB9 File Offset: 0x00013DB9
	public float weatherAudioVolumeModifier
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.Audio.weatherSFXVolumeMod;
			}
			return EnviroSkyLite.instance.Audio.weatherSFXVolumeMod;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.Audio.weatherSFXVolumeMod = value;
				return;
			}
			EnviroSkyLite.instance.Audio.weatherSFXVolumeMod = value;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060002E9 RID: 745 RVA: 0x00015BE5 File Offset: 0x00013DE5
	// (set) Token: 0x060002EA RID: 746 RVA: 0x00015C0F File Offset: 0x00013E0F
	public float audioTransitionSpeed
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.weatherSettings.audioTransitionSpeed;
			}
			return EnviroSkyLite.instance.weatherSettings.audioTransitionSpeed;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.weatherSettings.audioTransitionSpeed = value;
				return;
			}
			EnviroSkyLite.instance.weatherSettings.audioTransitionSpeed = value;
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060002EB RID: 747 RVA: 0x00015C3B File Offset: 0x00013E3B
	// (set) Token: 0x060002EC RID: 748 RVA: 0x00015C65 File Offset: 0x00013E65
	public float interiorZoneAudioVolume
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.interiorZoneSettings.currentInteriorZoneAudioVolume;
			}
			return EnviroSkyLite.instance.interiorZoneSettings.currentInteriorZoneAudioVolume;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.interiorZoneSettings.currentInteriorZoneAudioVolume = value;
				return;
			}
			EnviroSkyLite.instance.interiorZoneSettings.currentInteriorZoneAudioVolume = value;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060002ED RID: 749 RVA: 0x00015C91 File Offset: 0x00013E91
	// (set) Token: 0x060002EE RID: 750 RVA: 0x00015CBB File Offset: 0x00013EBB
	public float interiorZoneAudioFadingSpeed
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.interiorZoneSettings.currentInteriorZoneAudioFadingSpeed;
			}
			return EnviroSkyLite.instance.interiorZoneSettings.currentInteriorZoneAudioFadingSpeed;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.interiorZoneSettings.currentInteriorZoneAudioFadingSpeed = value;
				return;
			}
			EnviroSkyLite.instance.interiorZoneSettings.currentInteriorZoneAudioFadingSpeed = value;
		}
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00015CE7 File Offset: 0x00013EE7
	public GameObject GetVFXHolder()
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.Weather.VFXHolder;
		}
		return EnviroSkyLite.instance.Weather.VFXHolder;
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00015D11 File Offset: 0x00013F11
	public void SetLightningFlashTrigger(float trigger)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.thunder = trigger;
			return;
		}
		EnviroSkyLite.instance.thunder = trigger;
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00015D34 File Offset: 0x00013F34
	public float GetEmissionRate(ParticleSystem system)
	{
		return system.emission.rateOverTime.constantMax;
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x00015D58 File Offset: 0x00013F58
	public void SetEmissionRate(ParticleSystem sys, float emissionRate)
	{
		ParticleSystem.EmissionModule emission = sys.emission;
		ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
		rateOverTime.constantMax = emissionRate;
		emission.rateOverTime = rateOverTime;
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x00015D84 File Offset: 0x00013F84
	public void RegisterVegetationInstance(EnviroVegetationInstance v)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSky.instance.RegisterMe(v);
			return;
		}
		EnviroSkyLite.instance.RegisterMe(v);
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00015DA8 File Offset: 0x00013FA8
	public double GetInHours(float hours, float days, float years)
	{
		if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			return EnviroSky.instance.GetInHours(hours, days, years, EnviroSky.instance.GameTime.DaysInYear);
		}
		return EnviroSkyLite.instance.GetInHours(hours, days, years, EnviroSkyLite.instance.GameTime.DaysInYear);
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060002F5 RID: 757 RVA: 0x00015DF7 File Offset: 0x00013FF7
	// (set) Token: 0x060002F6 RID: 758 RVA: 0x00015E0E File Offset: 0x0001400E
	public bool useVolumeClouds
	{
		get
		{
			return this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD && EnviroSky.instance.useVolumeClouds;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.useVolumeClouds = value;
			}
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x060002F7 RID: 759 RVA: 0x00015E24 File Offset: 0x00014024
	// (set) Token: 0x060002F8 RID: 760 RVA: 0x00015E3B File Offset: 0x0001403B
	public bool useAurora
	{
		get
		{
			return this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD && EnviroSky.instance.useAurora;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.useAurora = value;
			}
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x060002F9 RID: 761 RVA: 0x00015E51 File Offset: 0x00014051
	// (set) Token: 0x060002FA RID: 762 RVA: 0x00015E68 File Offset: 0x00014068
	public bool useVolumeLighting
	{
		get
		{
			return this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD && EnviroSky.instance.useVolumeLighting;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.useVolumeLighting = value;
			}
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x060002FB RID: 763 RVA: 0x00015E7E File Offset: 0x0001407E
	// (set) Token: 0x060002FC RID: 764 RVA: 0x00015E95 File Offset: 0x00014095
	public bool useFlatClouds
	{
		get
		{
			return this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD && EnviroSky.instance.useFlatClouds;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.useFlatClouds = value;
			}
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x060002FD RID: 765 RVA: 0x00015EAB File Offset: 0x000140AB
	// (set) Token: 0x060002FE RID: 766 RVA: 0x00015ECB File Offset: 0x000140CB
	public bool useParticleClouds
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.useParticleClouds;
			}
			return EnviroSkyLite.instance.useParticleClouds;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.useParticleClouds = value;
				return;
			}
			EnviroSkyLite.instance.useParticleClouds = value;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x060002FF RID: 767 RVA: 0x00015EED File Offset: 0x000140ED
	// (set) Token: 0x06000300 RID: 768 RVA: 0x00015F17 File Offset: 0x00014117
	public bool useSunShafts
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.LightShafts.sunLightShafts;
			}
			return EnviroSkyLite.instance.LightShafts.sunLightShafts;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.LightShafts.sunLightShafts = value;
				return;
			}
			EnviroSkyLite.instance.LightShafts.sunLightShafts = value;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000301 RID: 769 RVA: 0x00015F43 File Offset: 0x00014143
	// (set) Token: 0x06000302 RID: 770 RVA: 0x00015F6D File Offset: 0x0001416D
	public bool useMoonShafts
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.LightShafts.moonLightShafts;
			}
			return EnviroSkyLite.instance.LightShafts.moonLightShafts;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.LightShafts.moonLightShafts = value;
				return;
			}
			EnviroSkyLite.instance.LightShafts.moonLightShafts = value;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000303 RID: 771 RVA: 0x00015F99 File Offset: 0x00014199
	// (set) Token: 0x06000304 RID: 772 RVA: 0x00015FB0 File Offset: 0x000141B0
	public bool useDistanceBlur
	{
		get
		{
			return this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD && EnviroSky.instance.useDistanceBlur;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.useDistanceBlur = value;
			}
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000305 RID: 773 RVA: 0x00015FC6 File Offset: 0x000141C6
	// (set) Token: 0x06000306 RID: 774 RVA: 0x00015FE6 File Offset: 0x000141E6
	public bool useFog
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.useFog;
			}
			return EnviroSkyLite.instance.usePostEffectFog;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.useFog = value;
				return;
			}
			EnviroSkyLite.instance.usePostEffectFog = value;
		}
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000307 RID: 775 RVA: 0x00016008 File Offset: 0x00014208
	// (remove) Token: 0x06000308 RID: 776 RVA: 0x00016040 File Offset: 0x00014240
	public event EnviroSkyMgr.HourPassed OnHourPassed;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000309 RID: 777 RVA: 0x00016078 File Offset: 0x00014278
	// (remove) Token: 0x0600030A RID: 778 RVA: 0x000160B0 File Offset: 0x000142B0
	public event EnviroSkyMgr.DayPassed OnDayPassed;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600030B RID: 779 RVA: 0x000160E8 File Offset: 0x000142E8
	// (remove) Token: 0x0600030C RID: 780 RVA: 0x00016120 File Offset: 0x00014320
	public event EnviroSkyMgr.YearPassed OnYearPassed;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x0600030D RID: 781 RVA: 0x00016158 File Offset: 0x00014358
	// (remove) Token: 0x0600030E RID: 782 RVA: 0x00016190 File Offset: 0x00014390
	public event EnviroSkyMgr.WeatherChanged OnWeatherChanged;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600030F RID: 783 RVA: 0x000161C8 File Offset: 0x000143C8
	// (remove) Token: 0x06000310 RID: 784 RVA: 0x00016200 File Offset: 0x00014400
	public event EnviroSkyMgr.ZoneWeatherChanged OnZoneWeatherChanged;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000311 RID: 785 RVA: 0x00016238 File Offset: 0x00014438
	// (remove) Token: 0x06000312 RID: 786 RVA: 0x00016270 File Offset: 0x00014470
	public event EnviroSkyMgr.SeasonChanged OnSeasonChanged;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000313 RID: 787 RVA: 0x000162A8 File Offset: 0x000144A8
	// (remove) Token: 0x06000314 RID: 788 RVA: 0x000162E0 File Offset: 0x000144E0
	public event EnviroSkyMgr.isNightE OnNightTime;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000315 RID: 789 RVA: 0x00016318 File Offset: 0x00014518
	// (remove) Token: 0x06000316 RID: 790 RVA: 0x00016350 File Offset: 0x00014550
	public event EnviroSkyMgr.isDay OnDayTime;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000317 RID: 791 RVA: 0x00016388 File Offset: 0x00014588
	// (remove) Token: 0x06000318 RID: 792 RVA: 0x000163C0 File Offset: 0x000145C0
	public event EnviroSkyMgr.ZoneChanged OnZoneChanged;

	// Token: 0x06000319 RID: 793 RVA: 0x000163F5 File Offset: 0x000145F5
	public virtual void NotifyHourPassed()
	{
		if (this.OnHourPassed != null)
		{
			this.OnHourPassed();
		}
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0001640A File Offset: 0x0001460A
	public virtual void NotifyDayPassed()
	{
		if (this.OnDayPassed != null)
		{
			this.OnDayPassed();
		}
	}

	// Token: 0x0600031B RID: 795 RVA: 0x0001641F File Offset: 0x0001461F
	public virtual void NotifyYearPassed()
	{
		if (this.OnYearPassed != null)
		{
			this.OnYearPassed();
		}
	}

	// Token: 0x0600031C RID: 796 RVA: 0x00016434 File Offset: 0x00014634
	public virtual void NotifyWeatherChanged(EnviroWeatherPreset type)
	{
		if (this.OnWeatherChanged != null)
		{
			this.OnWeatherChanged(type);
		}
	}

	// Token: 0x0600031D RID: 797 RVA: 0x0001644A File Offset: 0x0001464A
	public virtual void NotifyZoneWeatherChanged(EnviroWeatherPreset type, EnviroZone zone)
	{
		if (this.OnZoneWeatherChanged != null)
		{
			this.OnZoneWeatherChanged(type, zone);
		}
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00016461 File Offset: 0x00014661
	public virtual void NotifySeasonChanged(EnviroSeasons.Seasons season)
	{
		if (this.OnSeasonChanged != null)
		{
			this.OnSeasonChanged(season);
		}
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00016477 File Offset: 0x00014677
	public virtual void NotifyIsNight()
	{
		if (this.OnNightTime != null)
		{
			this.OnNightTime();
		}
	}

	// Token: 0x06000320 RID: 800 RVA: 0x0001648C File Offset: 0x0001468C
	public virtual void NotifyIsDay()
	{
		if (this.OnDayTime != null)
		{
			this.OnDayTime();
		}
	}

	// Token: 0x06000321 RID: 801 RVA: 0x000164A1 File Offset: 0x000146A1
	public virtual void NotifyZoneChanged(EnviroZone zone)
	{
		if (this.OnZoneChanged != null)
		{
			this.OnZoneChanged(zone);
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000322 RID: 802 RVA: 0x00014E8B File Offset: 0x0001308B
	// (set) Token: 0x06000323 RID: 803 RVA: 0x000164B7 File Offset: 0x000146B7
	public bool interiorMode
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.interiorMode;
			}
			return EnviroSkyLite.instance.interiorMode;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.interiorMode = value;
				return;
			}
			EnviroSkyLite.instance.interiorMode = value;
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000324 RID: 804 RVA: 0x000164D9 File Offset: 0x000146D9
	// (set) Token: 0x06000325 RID: 805 RVA: 0x000164F9 File Offset: 0x000146F9
	public EnviroInterior lastInteriorZone
	{
		get
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				return EnviroSky.instance.lastInteriorZone;
			}
			return EnviroSkyLite.instance.lastInteriorZone;
		}
		set
		{
			if (this.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				EnviroSky.instance.lastInteriorZone = value;
				return;
			}
			EnviroSkyLite.instance.lastInteriorZone = value;
		}
	}

	// Token: 0x04000458 RID: 1112
	private static EnviroSkyMgr _instance;

	// Token: 0x04000459 RID: 1113
	[Header("General")]
	[Tooltip("Enable to make sure thast enviro objects don't get destroyed on scene load.")]
	public bool dontDestroy;

	// Token: 0x0400045A RID: 1114
	public bool showSetup = true;

	// Token: 0x0400045B RID: 1115
	public bool showInstances = true;

	// Token: 0x0400045C RID: 1116
	public bool showThirdParty;

	// Token: 0x0400045D RID: 1117
	public bool showUtilities;

	// Token: 0x0400045E RID: 1118
	public bool showThirdPartyShaders;

	// Token: 0x0400045F RID: 1119
	public bool showThirdPartyMisc;

	// Token: 0x04000460 RID: 1120
	public bool showThirdPartyNetwork;

	// Token: 0x04000461 RID: 1121
	public bool showUtiliies;

	// Token: 0x04000462 RID: 1122
	public RenderTexture cube;

	// Token: 0x04000463 RID: 1123
	public EnviroSkyMgr.EnviroBaking skyBaking;

	// Token: 0x04000464 RID: 1124
	public EnviroSkyMgr.EnviroSkyVersion currentEnviroSkyVersion = EnviroSkyMgr.EnviroSkyVersion.HD;

	// Token: 0x04000465 RID: 1125
	public EnviroSky enviroHDInstance;

	// Token: 0x04000466 RID: 1126
	public EnviroSkyLite enviroLWInstance;

	// Token: 0x02000098 RID: 152
	[Serializable]
	public class EnviroBaking
	{
		// Token: 0x04000470 RID: 1136
		public int resolution = 2048;
	}

	// Token: 0x02000099 RID: 153
	public enum EnviroSkyVersion
	{
		// Token: 0x04000472 RID: 1138
		None,
		// Token: 0x04000473 RID: 1139
		LW,
		// Token: 0x04000474 RID: 1140
		HD
	}

	// Token: 0x0200009A RID: 154
	// (Invoke) Token: 0x06000329 RID: 809
	public delegate void HourPassed();

	// Token: 0x0200009B RID: 155
	// (Invoke) Token: 0x0600032D RID: 813
	public delegate void DayPassed();

	// Token: 0x0200009C RID: 156
	// (Invoke) Token: 0x06000331 RID: 817
	public delegate void YearPassed();

	// Token: 0x0200009D RID: 157
	// (Invoke) Token: 0x06000335 RID: 821
	public delegate void WeatherChanged(EnviroWeatherPreset weatherType);

	// Token: 0x0200009E RID: 158
	// (Invoke) Token: 0x06000339 RID: 825
	public delegate void ZoneWeatherChanged(EnviroWeatherPreset weatherType, EnviroZone zone);

	// Token: 0x0200009F RID: 159
	// (Invoke) Token: 0x0600033D RID: 829
	public delegate void SeasonChanged(EnviroSeasons.Seasons season);

	// Token: 0x020000A0 RID: 160
	// (Invoke) Token: 0x06000341 RID: 833
	public delegate void isNightE();

	// Token: 0x020000A1 RID: 161
	// (Invoke) Token: 0x06000345 RID: 837
	public delegate void isDay();

	// Token: 0x020000A2 RID: 162
	// (Invoke) Token: 0x06000349 RID: 841
	public delegate void ZoneChanged(EnviroZone zone);
}
