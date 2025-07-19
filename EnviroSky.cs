using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020000C7 RID: 199
[ExecuteInEditMode]
public class EnviroSky : EnviroCore
{
	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x0001CE53 File Offset: 0x0001B053
	public static EnviroSky instance
	{
		get
		{
			if (EnviroSky._instance == null)
			{
				EnviroSky._instance = UnityEngine.Object.FindObjectOfType<EnviroSky>();
			}
			return EnviroSky._instance;
		}
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x0001CE74 File Offset: 0x0001B074
	private void Start()
	{
		if (EnviroSkyMgr.instance == null)
		{
			Debug.Log("Please use the EnviroSky Manager!");
			base.gameObject.SetActive(false);
			return;
		}
		this.started = false;
		base.SetTime(this.GameTime.Years, this.GameTime.Days, this.GameTime.Hours, this.GameTime.Minutes, this.GameTime.Seconds);
		this.lastHourUpdate = (float)Mathf.RoundToInt(this.internalHour);
		this.currentTimeInHours = base.GetInHours(this.internalHour, (float)this.GameTime.Days, (float)this.GameTime.Years, this.GameTime.DaysInYear);
		this.Weather.weatherFullyChanged = false;
		this.thunder = 0f;
		if (this.Components.GlobalReflectionProbe == null)
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.name == "GlobalReflections")
				{
					GameObject gameObject = transform.gameObject;
					this.Components.GlobalReflectionProbe = gameObject.GetComponent<EnviroReflectionProbe>();
					if (this.Components.GlobalReflectionProbe == null)
					{
						this.Components.GlobalReflectionProbe = gameObject.AddComponent<EnviroReflectionProbe>();
					}
				}
			}
		}
		if (this.profileLoaded)
		{
			base.InvokeRepeating("UpdateEnviroment", 0f, this.qualitySettings.UpdateInterval);
			if (this.PlayerCamera != null && this.Player != null && !this.AssignInRuntime && this.startMode == EnviroCore.EnviroStartMode.Started)
			{
				this.Init();
			}
		}
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x0001D048 File Offset: 0x0001B248
	private IEnumerator SetSceneSettingsLate()
	{
		yield return 0;
		if (this.skyMat != null && RenderSettings.skybox != this.skyMat)
		{
			this.SetupSkybox();
		}
		if (RenderSettings.fogMode != this.fogSettings.Fogmode)
		{
			RenderSettings.fogMode = this.fogSettings.Fogmode;
		}
		if (RenderSettings.ambientMode != this.lightSettings.ambientMode)
		{
			RenderSettings.ambientMode = this.lightSettings.ambientMode;
		}
		yield break;
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0001D058 File Offset: 0x0001B258
	private void OnEnable()
	{
		if (EnviroSkyMgr.instance == null)
		{
			return;
		}
		this.LoadRessources();
		this.Weather.currentActiveWeatherPreset = this.Weather.zones[0].currentActiveZoneWeatherPreset;
		this.Weather.lastActiveWeatherPreset = this.Weather.currentActiveWeatherPreset;
		if (this.weatherMapMat == null)
		{
			this.weatherMapMat = new Material(Shader.Find("Enviro/Standard/WeatherMap"));
		}
		if (this.profile == null)
		{
			Debug.LogError("No profile assigned!");
			return;
		}
		if (!this.profileLoaded)
		{
			base.ApplyProfile(this.profile);
		}
		this.PreInit();
		if (this.AssignInRuntime)
		{
			this.started = false;
		}
		else if (this.PlayerCamera != null && this.Player != null && this.startMode == EnviroCore.EnviroStartMode.Started)
		{
			this.Init();
		}
		this.PopulateCloudsQualityList();
		if (this.currentActiveCloudsQualityPreset != null)
		{
			this.ApplyVolumeCloudsQualityPreset(this.currentActiveCloudsQualityPreset);
		}
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x0001D161 File Offset: 0x0001B361
	public void ReInit()
	{
		this.OnEnable();
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x0001D16C File Offset: 0x0001B36C
	private void PreInit()
	{
		if (this.GameTime.solarTime < this.GameTime.dayNightSwitch)
		{
			this.isNight = true;
		}
		else
		{
			this.isNight = false;
		}
		base.CreateEffects("Enviro Effects");
		if (this.weatherSettings.lightningEffect != null && this.lightningEffect == null)
		{
			this.lightningEffect = UnityEngine.Object.Instantiate<GameObject>(this.weatherSettings.lightningEffect, this.EffectsHolder.transform).GetComponent<ParticleSystem>();
		}
		if (this.serverMode)
		{
			return;
		}
		base.CheckSatellites();
		if (this.Components.GlobalReflectionProbe == null)
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.name == "GlobalReflections")
				{
					GameObject gameObject = transform.gameObject;
					this.Components.GlobalReflectionProbe = gameObject.GetComponent<EnviroReflectionProbe>();
					if (this.Components.GlobalReflectionProbe == null)
					{
						this.Components.GlobalReflectionProbe = gameObject.AddComponent<EnviroReflectionProbe>();
					}
				}
			}
		}
		if (!this.Components.Sun)
		{
			Debug.LogError("Please set sun object in inspector!");
		}
		if (!this.Components.satellites)
		{
			Debug.LogError("Please set satellite object in inspector!");
		}
		if (this.Components.Moon)
		{
			this.MoonTransform = this.Components.Moon.transform;
		}
		else
		{
			Debug.LogError("Please set moon object in inspector!");
		}
		if (this.weatherMap != null)
		{
			UnityEngine.Object.DestroyImmediate(this.weatherMap);
		}
		if (this.weatherMap == null)
		{
			this.weatherMap = new RenderTexture(512, 512, 0, RenderTextureFormat.Default);
			this.weatherMap.wrapMode = TextureWrapMode.Repeat;
		}
		if (this.lightSettings.directionalLightMode == EnviroLightSettings.LightingMode.Single)
		{
			this.SetupMainLight();
		}
		else
		{
			this.SetupMainLight();
			this.SetupAdditionalLight();
		}
		if (this.cloudShadowMap != null)
		{
			UnityEngine.Object.DestroyImmediate(this.cloudShadowMap);
		}
		if (this.cloudShadowMap == null)
		{
			this.cloudShadowMap = new RenderTexture(2048, 2048, 0, RenderTextureFormat.Default);
			this.cloudShadowMap.wrapMode = TextureWrapMode.Repeat;
		}
		if (this.cloudShadowMat != null)
		{
			UnityEngine.Object.DestroyImmediate(this.cloudShadowMat);
		}
		this.cloudShadowMat = new Material(Shader.Find("Enviro/Standard/ShadowCookie"));
		if (this.cloudsSettings.shadowIntensity > 0f)
		{
			Graphics.Blit(this.weatherMap, this.cloudShadowMap, this.cloudShadowMat);
			this.MainLight.cookie = this.cloudShadowMap;
			this.MainLight.cookieSize = 10000f;
		}
		else
		{
			this.MainLight.cookie = null;
		}
		if (this.Components.particleClouds)
		{
			ParticleSystem[] componentsInChildren = this.Components.particleClouds.GetComponentsInChildren<ParticleSystem>();
			if (componentsInChildren.Length != 0)
			{
				this.particleClouds.layer1System = componentsInChildren[0];
			}
			if (componentsInChildren.Length > 1)
			{
				this.particleClouds.layer2System = componentsInChildren[1];
			}
			if (this.particleClouds.layer1System != null)
			{
				this.particleClouds.layer1Material = this.particleClouds.layer1System.GetComponent<ParticleSystemRenderer>().sharedMaterial;
			}
			if (this.particleClouds.layer2System != null)
			{
				this.particleClouds.layer2Material = this.particleClouds.layer2System.GetComponent<ParticleSystemRenderer>().sharedMaterial;
				return;
			}
		}
		else
		{
			Debug.LogError("Please set particleCLouds object in inspector!");
		}
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x0001D514 File Offset: 0x0001B714
	public void SetupSkybox()
	{
		if (this.skySettings.skyboxMode == EnviroSkySettings.SkyboxModi.Simple)
		{
			if (this.skyMat != null)
			{
				UnityEngine.Object.DestroyImmediate(this.skyMat);
			}
			this.skyMat = new Material(Shader.Find("Enviro/Standard/SkyboxSimple"));
			if (this.skySettings.starsCubeMap != null)
			{
				this.skyMat.SetTexture("_Stars", this.skySettings.starsCubeMap);
			}
			if (this.skySettings.galaxyCubeMap != null)
			{
				this.skyMat.SetTexture("_Galaxy", this.skySettings.galaxyCubeMap);
			}
			if (this.ressources.starsTwinklingNoise != null)
			{
				this.skyMat.SetTexture("_StarsTwinklingNoise", this.ressources.starsTwinklingNoise);
			}
			if (this.ressources.aurora_layer_1 != null)
			{
				this.skyMat.SetTexture("_Aurora_Layer_1", this.ressources.aurora_layer_1);
			}
			if (this.ressources.aurora_layer_2 != null)
			{
				this.skyMat.SetTexture("_Aurora_Layer_2", this.ressources.aurora_layer_2);
			}
			if (this.ressources.aurora_colorshift != null)
			{
				this.skyMat.SetTexture("_Aurora_Colorshift", this.ressources.aurora_colorshift);
			}
			RenderSettings.skybox = this.skyMat;
		}
		else if (this.skySettings.skyboxMode == EnviroSkySettings.SkyboxModi.Default)
		{
			if (this.skyMat != null)
			{
				UnityEngine.Object.DestroyImmediate(this.skyMat);
			}
			if (!this.useFlatClouds)
			{
				this.skyMat = new Material(Shader.Find("Enviro/Standard/Skybox"));
				this.flatCloudsSkybox = false;
			}
			else
			{
				this.skyMat = new Material(Shader.Find("Enviro/Standard/SkyboxFlatClouds"));
				this.flatCloudsSkybox = true;
			}
			if (this.skySettings.starsCubeMap != null)
			{
				this.skyMat.SetTexture("_Stars", this.skySettings.starsCubeMap);
			}
			if (this.skySettings.galaxyCubeMap != null)
			{
				this.skyMat.SetTexture("_Galaxy", this.skySettings.galaxyCubeMap);
			}
			if (this.ressources.starsTwinklingNoise != null)
			{
				this.skyMat.SetTexture("_StarsTwinklingNoise", this.ressources.starsTwinklingNoise);
			}
			if (this.ressources.aurora_layer_1 != null)
			{
				this.skyMat.SetTexture("_Aurora_Layer_1", this.ressources.aurora_layer_1);
			}
			if (this.ressources.aurora_layer_2 != null)
			{
				this.skyMat.SetTexture("_Aurora_Layer_2", this.ressources.aurora_layer_2);
			}
			if (this.ressources.aurora_colorshift != null)
			{
				this.skyMat.SetTexture("_Aurora_Colorshift", this.ressources.aurora_colorshift);
			}
			RenderSettings.skybox = this.skyMat;
		}
		else if (this.skySettings.skyboxMode == EnviroSkySettings.SkyboxModi.CustomSkybox)
		{
			if (this.skySettings.customSkyboxMaterial != null)
			{
				RenderSettings.skybox = this.skySettings.customSkyboxMaterial;
				this.skyMat = this.skySettings.customSkyboxMaterial;
			}
		}
		else if (this.skySettings.skyboxMode == EnviroSkySettings.SkyboxModi.CustomColor && this.skyMat != null)
		{
			UnityEngine.Object.DestroyImmediate(this.skyMat);
		}
		if (this.lightSettings.ambientMode == AmbientMode.Skybox)
		{
			base.StartCoroutine(this.UpdateAmbientLightWithDelay());
		}
		this.currentSkyboxMode = this.skySettings.skyboxMode;
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x0001D8A6 File Offset: 0x0001BAA6
	private IEnumerator UpdateAmbientLightWithDelay()
	{
		yield return 0;
		DynamicGI.UpdateEnvironment();
		yield break;
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x0001D8B0 File Offset: 0x0001BAB0
	public void LoadRessources()
	{
		if (this.ressources.starsTwinklingNoise == null)
		{
			this.ressources.starsTwinklingNoise = (Resources.Load("cube_enviro_starsNoise") as Cubemap);
		}
		if (this.ressources.aurora_layer_1 == null)
		{
			this.ressources.aurora_layer_1 = (Resources.Load("tex_enviro_aurora_layer_1") as Texture2D);
		}
		if (this.ressources.aurora_layer_2 == null)
		{
			this.ressources.aurora_layer_2 = (Resources.Load("tex_enviro_aurora_layer_2") as Texture2D);
		}
		if (this.ressources.aurora_colorshift == null)
		{
			this.ressources.aurora_colorshift = (Resources.Load("tex_enviro_aurora_colorshift") as Texture2D);
		}
		if (this.ressources.noiseTextureHigh == null)
		{
			this.ressources.noiseTextureHigh = (Resources.Load("enviro_clouds_base") as Texture3D);
		}
		if (this.ressources.noiseTexture == null)
		{
			this.ressources.noiseTexture = (Resources.Load("enviro_clouds_base_low") as Texture3D);
		}
		if (this.ressources.detailNoiseTexture == null)
		{
			this.ressources.detailNoiseTexture = (Resources.Load("enviro_clouds_detail_low") as Texture3D);
		}
		if (this.ressources.detailNoiseTextureHigh == null)
		{
			this.ressources.detailNoiseTextureHigh = (Resources.Load("enviro_clouds_detail_high") as Texture3D);
		}
		if (this.ressources.dither == null)
		{
			this.ressources.dither = (Resources.Load("tex_enviro_dither") as Texture2D);
		}
		if (this.ressources.blueNoise == null)
		{
			this.ressources.blueNoise = (Resources.Load("tex_enviro_blueNoise", typeof(Texture2D)) as Texture2D);
		}
		if (this.ressources.distributionTexture == null)
		{
			this.ressources.distributionTexture = (Resources.Load("tex_enviro_linear", typeof(Texture2D)) as Texture2D);
		}
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x0001DAC0 File Offset: 0x0001BCC0
	private void Init()
	{
		if (this.profile == null)
		{
			return;
		}
		if (this.serverMode)
		{
			this.started = true;
			return;
		}
		if (this.skyMat != null && RenderSettings.skybox != this.skyMat)
		{
			this.SetupSkybox();
		}
		else if (this.skyMat == null)
		{
			this.SetupSkybox();
		}
		if (RenderSettings.fogMode != this.fogSettings.Fogmode)
		{
			RenderSettings.fogMode = this.fogSettings.Fogmode;
		}
		if (RenderSettings.ambientMode != this.lightSettings.ambientMode)
		{
			RenderSettings.ambientMode = this.lightSettings.ambientMode;
		}
		this.InitImageEffects();
		if (this.PlayerCamera != null && this.setCameraClearFlags)
		{
			this.PlayerCamera.clearFlags = CameraClearFlags.Skybox;
		}
		if (this.satelliteSettings.additionalSatellites.Count > 0)
		{
			this.CreateSatCamera();
		}
		this.started = true;
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x0001DBB4 File Offset: 0x0001BDB4
	private void InitImageEffects()
	{
		this.EnviroSkyRender = this.PlayerCamera.gameObject.GetComponent<EnviroSkyRendering>();
		if (this.EnviroSkyRender == null)
		{
			this.EnviroSkyRender = this.PlayerCamera.gameObject.AddComponent<EnviroSkyRendering>();
		}
		this.EnviroPostProcessing = this.PlayerCamera.gameObject.GetComponent<EnviroPostProcessing>();
		if (this.EnviroPostProcessing == null)
		{
			this.EnviroPostProcessing = this.PlayerCamera.gameObject.AddComponent<EnviroPostProcessing>();
		}
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0001DC38 File Offset: 0x0001BE38
	public void CreateSatCamera()
	{
		Camera[] array = UnityEngine.Object.FindObjectsOfType<Camera>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].cullingMask &= ~(1 << this.satelliteRenderingLayer);
		}
		UnityEngine.Object.DestroyImmediate(GameObject.Find("Enviro Sat Camera"));
		this.satCamera = new GameObject
		{
			name = "Enviro Sat Camera",
			transform = 
			{
				position = this.PlayerCamera.transform.position,
				rotation = this.PlayerCamera.transform.rotation
			},
			hideFlags = HideFlags.DontSave
		}.AddComponent<Camera>();
		this.satCamera.farClipPlane = this.PlayerCamera.farClipPlane;
		this.satCamera.nearClipPlane = this.PlayerCamera.nearClipPlane;
		this.satCamera.aspect = this.PlayerCamera.aspect;
		this.satCamera.useOcclusionCulling = false;
		this.satCamera.renderingPath = RenderingPath.Forward;
		this.satCamera.fieldOfView = this.PlayerCamera.fieldOfView;
		this.satCamera.clearFlags = CameraClearFlags.Color;
		this.satCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		this.satCamera.cullingMask = 1 << this.satelliteRenderingLayer;
		this.satCamera.depth = this.PlayerCamera.depth + 1f;
		this.satCamera.enabled = true;
		this.PlayerCamera.cullingMask &= ~(1 << this.satelliteRenderingLayer);
		RenderTextureFormat format = base.GetCameraHDR(this.satCamera) ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
		this.satRenderTarget = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 16, format);
		this.satCamera.targetTexture = this.satRenderTarget;
		this.satCamera.enabled = false;
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x0001DE38 File Offset: 0x0001C038
	private void SetupMainLight()
	{
		if (this.Components.DirectLight)
		{
			this.MainLight = this.Components.DirectLight.GetComponent<Light>();
			if (this.directVolumeLight == null)
			{
				this.directVolumeLight = this.Components.DirectLight.GetComponent<EnviroVolumeLight>();
			}
			if (this.directVolumeLight == null)
			{
				this.directVolumeLight = this.Components.DirectLight.gameObject.AddComponent<EnviroVolumeLight>();
			}
			if (EnviroSkyMgr.instance.dontDestroy && Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(this.Components.DirectLight);
			}
		}
		else
		{
			GameObject gameObject = GameObject.Find("Enviro Directional Light");
			if (gameObject != null)
			{
				this.Components.DirectLight = gameObject.transform;
			}
			else
			{
				this.Components.DirectLight = base.CreateDirectionalLight(false);
			}
			this.MainLight = this.Components.DirectLight.GetComponent<Light>();
			if (this.directVolumeLight == null)
			{
				this.directVolumeLight = this.Components.DirectLight.GetComponent<EnviroVolumeLight>();
			}
			if (this.directVolumeLight == null)
			{
				this.directVolumeLight = this.Components.DirectLight.gameObject.AddComponent<EnviroVolumeLight>();
			}
			if (EnviroSkyMgr.instance.dontDestroy && Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(this.Components.DirectLight);
			}
		}
		if (this.lightSettings.directionalLightMode == EnviroLightSettings.LightingMode.Single && this.Components.AdditionalDirectLight != null)
		{
			UnityEngine.Object.DestroyImmediate(this.Components.AdditionalDirectLight.gameObject);
		}
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0001DFE0 File Offset: 0x0001C1E0
	private void SetupAdditionalLight()
	{
		if (this.Components.AdditionalDirectLight)
		{
			this.AdditionalLight = this.Components.AdditionalDirectLight.GetComponent<Light>();
			if (this.additionalDirectVolumeLight == null)
			{
				this.additionalDirectVolumeLight = this.Components.AdditionalDirectLight.GetComponent<EnviroVolumeLight>();
			}
			if (this.additionalDirectVolumeLight == null)
			{
				this.additionalDirectVolumeLight = this.Components.AdditionalDirectLight.gameObject.AddComponent<EnviroVolumeLight>();
			}
			if (EnviroSkyMgr.instance.dontDestroy && Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(this.Components.AdditionalDirectLight);
				return;
			}
		}
		else
		{
			GameObject gameObject = GameObject.Find("Enviro Directional Light - Moon");
			if (gameObject != null)
			{
				this.Components.AdditionalDirectLight = gameObject.transform;
			}
			else
			{
				this.Components.AdditionalDirectLight = base.CreateDirectionalLight(true);
			}
			this.AdditionalLight = this.Components.DirectLight.GetComponent<Light>();
			if (this.additionalDirectVolumeLight == null)
			{
				this.additionalDirectVolumeLight = this.Components.AdditionalDirectLight.GetComponent<EnviroVolumeLight>();
			}
			if (this.additionalDirectVolumeLight == null)
			{
				this.additionalDirectVolumeLight = this.Components.AdditionalDirectLight.gameObject.AddComponent<EnviroVolumeLight>();
			}
			if (EnviroSkyMgr.instance.dontDestroy && Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(this.Components.AdditionalDirectLight);
			}
		}
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x0001E150 File Offset: 0x0001C350
	private void RenderWeatherMap()
	{
		if (this.cloudsSettings.customWeatherMap == null)
		{
			this.weatherMapMat.SetVector("_WindDir", this.cloudAnimNonScaled);
			this.weatherMapMat.SetFloat("_AnimSpeedScale", this.cloudsSettings.weatherAnimSpeedScale);
			this.weatherMapMat.SetInt("_Tiling", this.cloudsSettings.weatherMapTiling);
			this.weatherMapMat.SetVector("_Location", this.cloudsSettings.locationOffset);
			double value = (double)(this.cloudsConfig.coverage * this.cloudsSettings.globalCloudCoverage);
			this.weatherMapMat.SetFloat("_Coverage", (float)Math.Round(value, 4));
			this.weatherMapMat.SetFloat("_CloudsType", this.cloudsConfig.cloudType);
			this.weatherMapMat.SetFloat("_CoverageType", this.cloudsConfig.coverageType);
			this.weatherMapMat.SetVector("_LightingVariance", new Vector4(1f - this.cloudsConfig.lightVariance, this.cloudsSettings.lightingVarianceTiling, 0f, 0f));
			Graphics.Blit(null, this.weatherMap, this.weatherMapMat);
		}
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0001E298 File Offset: 0x0001C498
	public void RenderCloudMaps()
	{
		if (Application.isPlaying)
		{
			if (this.useVolumeClouds)
			{
				this.RenderWeatherMap();
				return;
			}
		}
		else if (this.useVolumeClouds && this.showVolumeCloudsInEditor)
		{
			this.RenderWeatherMap();
		}
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x0001E2C8 File Offset: 0x0001C4C8
	private void Update()
	{
		if (this.profile == null)
		{
			Debug.Log("No profile applied! Please create and assign a profile.");
			return;
		}
		if (!Application.isPlaying && this.startMode != EnviroCore.EnviroStartMode.Started)
		{
			if (this.startMode == EnviroCore.EnviroStartMode.Paused)
			{
				this.Stop(true, true);
			}
			else
			{
				this.GameTime.ProgressTime = EnviroTime.TimeProgressMode.Simulated;
				this.Stop(true, false);
			}
		}
		else if (!Application.isPlaying && this.startMode == EnviroCore.EnviroStartMode.Started && !this.started)
		{
			this.Play(this.GameTime.ProgressTime);
		}
		if (!this.started && !this.serverMode)
		{
			base.UpdateTime(this.GameTime.DaysInYear);
			base.UpdateSunAndMoonPosition();
			base.UpdateSceneView();
			base.CalculateDirectLight();
			base.UpdateReflections();
			if (!this.AssignInRuntime || !(this.PlayerTag != "") || !(this.CameraTag != "") || !Application.isPlaying)
			{
				this.started = false;
				return;
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag(this.PlayerTag);
			if (gameObject != null)
			{
				this.Player = gameObject;
			}
			for (int i = 0; i < Camera.allCameras.Length; i++)
			{
				if (Camera.allCameras[i].tag == this.CameraTag)
				{
					this.PlayerCamera = Camera.allCameras[i];
				}
			}
			if (!(this.Player != null) || !(this.PlayerCamera != null))
			{
				this.started = false;
				return;
			}
			this.Init();
			this.started = true;
		}
		base.UpdateTime(this.GameTime.DaysInYear);
		this.ValidateParameters();
		if (!this.serverMode)
		{
			if (this.useFlatClouds != this.flatCloudsSkybox)
			{
				this.SetupSkybox();
			}
			if (this.currentSkyboxMode != this.skySettings.skyboxMode)
			{
				this.SetupSkybox();
			}
			if (RenderSettings.skybox != this.skyMat)
			{
				this.SetupSkybox();
			}
			base.UpdateSceneView();
			if (!Application.isPlaying && this.Weather.startWeatherPreset != null && this.startMode == EnviroCore.EnviroStartMode.Started)
			{
				this.UpdateClouds(this.Weather.startWeatherPreset, false);
				this.UpdateFog(this.Weather.startWeatherPreset, false);
				this.UpdatePostProcessing(this.Weather.startWeatherPreset, false);
				base.UpdateWeatherVariables(this.Weather.startWeatherPreset);
			}
			base.UpdateAmbientLight();
			base.UpdateReflections();
			this.UpdateWeather();
			if (this.Weather.currentActiveWeatherPreset != null && this.Weather.currentActiveWeatherPreset.cloudsConfig.particleCloudsOverwrite)
			{
				base.UpdateParticleClouds(true);
			}
			else
			{
				base.UpdateParticleClouds(this.useParticleClouds);
			}
			this.UpdateCloudShadows();
			base.UpdateSunAndMoonPosition();
			base.CalculateDirectLight();
			this.SetMaterialsVariables();
			if (this.directVolumeLight != null && !this.directVolumeLight.isActiveAndEnabled && this.volumeLightSettings.dirVolumeLighting)
			{
				this.directVolumeLight.enabled = true;
			}
			if (!this.isNight && this.GameTime.solarTime < this.GameTime.dayNightSwitch)
			{
				this.isNight = true;
				if (this.Audio.AudioSourceAmbient != null)
				{
					base.TryPlayAmbientSFX();
				}
				EnviroSkyMgr.instance.NotifyIsNight();
				return;
			}
			if (this.isNight && this.GameTime.solarTime >= this.GameTime.dayNightSwitch)
			{
				this.isNight = false;
				if (this.Audio.AudioSourceAmbient != null)
				{
					base.TryPlayAmbientSFX();
				}
				EnviroSkyMgr.instance.NotifyIsDay();
				return;
			}
		}
		else
		{
			this.UpdateWeather();
			if (!this.isNight && this.GameTime.solarTime < this.GameTime.dayNightSwitch)
			{
				this.isNight = true;
				EnviroSkyMgr.instance.NotifyIsNight();
				return;
			}
			if (this.isNight && this.GameTime.solarTime >= this.GameTime.dayNightSwitch)
			{
				this.isNight = false;
				EnviroSkyMgr.instance.NotifyIsDay();
			}
		}
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x0001E6D4 File Offset: 0x0001C8D4
	private void LateUpdate()
	{
		if (!this.serverMode && this.PlayerCamera != null && this.Player != null)
		{
			base.transform.position = this.Player.transform.position;
			base.transform.localScale = new Vector3(this.PlayerCamera.farClipPlane, this.PlayerCamera.farClipPlane, this.PlayerCamera.farClipPlane);
			if (this.EffectsHolder != null)
			{
				if (this.cloudsSettings.cloudsQualitySettings != null && this.Player.transform.position.y > this.cloudsSettings.cloudsQualitySettings.bottomCloudHeight + this.cloudsSettings.cloudsHeightMod)
				{
					this.EffectsHolder.transform.position = new Vector3(this.Player.transform.position.x, this.cloudsSettings.cloudsQualitySettings.bottomCloudHeight + this.cloudsSettings.cloudsHeightMod, this.Player.transform.position.z);
					return;
				}
				this.EffectsHolder.transform.position = this.Player.transform.position;
			}
		}
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0001E82C File Offset: 0x0001CA2C
	private void UpdateCloudShadows()
	{
		if (this.cloudsSettings.shadowIntensity == 0f || !this.useVolumeClouds)
		{
			if (this.MainLight.cookie != null)
			{
				this.MainLight.cookie = null;
				return;
			}
		}
		else if (this.cloudsSettings.shadowIntensity > 0f)
		{
			this.cloudShadowMat.SetFloat("_shadowIntensity", this.cloudsSettings.shadowIntensity);
			if (this.useVolumeClouds)
			{
				this.cloudShadowMat.SetTexture("_MainTex", this.weatherMap);
				Graphics.Blit(this.weatherMap, this.cloudShadowMap, this.cloudShadowMat);
			}
			if (Application.isPlaying)
			{
				this.MainLight.cookie = this.cloudShadowMap;
			}
			else
			{
				this.MainLight.cookie = null;
			}
			this.MainLight.cookieSize = (float)this.cloudsSettings.shadowCookieSize;
		}
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0001E918 File Offset: 0x0001CB18
	public void UpdateSkyShaderVariables(Material skyMat)
	{
		if (this.skySettings.skyboxMode == EnviroSkySettings.SkyboxModi.Simple)
		{
			skyMat.SetColor("_SkyColor", this.skySettings.simpleSkyColor.Evaluate(this.GameTime.solarTime));
			skyMat.SetColor("_HorizonColor", this.skySettings.simpleHorizonColor.Evaluate(this.GameTime.solarTime));
			skyMat.SetColor("_HorizonBackColor", this.skySettings.simpleHorizonBackColor.Evaluate(this.GameTime.solarTime));
			skyMat.SetColor("_SunColor", this.skySettings.simpleSunColor.Evaluate(this.GameTime.solarTime));
			skyMat.SetColor("_GroundColor", this.skySettings.simpleGroundColor);
			skyMat.SetFloat("_SunDiskSizeSimple", this.skySettings.simpleSunDiskSize.Evaluate(this.GameTime.solarTime));
			skyMat.SetVector("_MoonDir", this.Components.Moon.transform.forward);
			skyMat.SetVector("_SunDir", -this.Components.Sun.transform.forward);
			skyMat.SetColor("_MoonColor", this.skySettings.moonColor);
			skyMat.SetColor("_weatherSkyMod", Color.Lerp(this.currentWeatherSkyMod, this.interiorZoneSettings.currentInteriorSkyboxMod, this.interiorZoneSettings.currentInteriorSkyboxMod.a));
			skyMat.SetFloat("_StarsIntensity", this.skySettings.starsIntensity.Evaluate(this.GameTime.solarTime));
			skyMat.SetFloat("_GalaxyIntensity", this.skySettings.galaxyIntensity.Evaluate(this.GameTime.solarTime));
			if (this.skySettings.moonPhaseMode == EnviroSkySettings.MoonPhases.Realistic)
			{
				float num = Vector3.SignedAngle(this.Components.Moon.transform.forward, this.Components.Sun.transform.forward, -base.transform.forward);
				if (this.GameTime.Latitude >= 0f)
				{
					if (num < 0f)
					{
						this.customMoonPhase = base.Remap(num, 0f, -180f, -2f, 0f);
					}
					else
					{
						this.customMoonPhase = base.Remap(num, 0f, 180f, 2f, 0f);
					}
				}
				else if (num < 0f)
				{
					this.customMoonPhase = base.Remap(num, 0f, -180f, 2f, 0f);
				}
				else
				{
					this.customMoonPhase = base.Remap(num, 0f, 180f, -2f, 0f);
				}
			}
			skyMat.SetColor("_moonGlowColor", this.skySettings.moonGlowColor);
			skyMat.SetVector("_moonParams", new Vector4(this.skySettings.moonSize, this.skySettings.glowSize, this.skySettings.moonGlow.Evaluate(this.GameTime.solarTime), this.customMoonPhase));
			if (this.skySettings.renderMoon)
			{
				skyMat.SetTexture("_MoonTex", this.skySettings.moonTexture);
				skyMat.SetTexture("_GlowTex", this.skySettings.glowTexture);
			}
			else
			{
				skyMat.SetTexture("_MoonTex", null);
				skyMat.SetTexture("_GlowTex", null);
			}
			skyMat.SetFloat("_StarsTwinkling", this.skySettings.starsTwinklingRate);
			if (this.skySettings.starsTwinklingRate > 0f)
			{
				this.starsTwinklingRot += this.skySettings.starsTwinklingRate * Time.deltaTime;
				Quaternion q = Quaternion.Euler(this.starsTwinklingRot, this.starsTwinklingRot, this.starsTwinklingRot);
				Matrix4x4 value = Matrix4x4.TRS(Vector3.zero, q, new Vector3(1f, 1f, 1f));
				skyMat.SetMatrix("_StarsTwinklingMatrix", value);
			}
			if (this.useAurora)
			{
				skyMat.EnableKeyword("ENVIRO_AURORA");
				skyMat.SetFloat("_AuroraIntensity", Mathf.Clamp01(this.auroraIntensity * this.auroraSettings.auroraIntensity.Evaluate(this.GameTime.solarTime)));
				skyMat.SetFloat("_AuroraBrightness", this.auroraSettings.auroraBrightness);
				skyMat.SetFloat("_AuroraContrast", this.auroraSettings.auroraContrast);
				skyMat.SetColor("_AuroraColor", this.auroraSettings.auroraColor);
				skyMat.SetFloat("_AuroraHeight", this.auroraSettings.auroraHeight);
				skyMat.SetFloat("_AuroraScale", this.auroraSettings.auroraScale);
				skyMat.SetFloat("_AuroraSpeed", this.auroraSettings.auroraSpeed);
				skyMat.SetFloat("_AuroraSteps", (float)this.auroraSettings.auroraSteps);
				skyMat.SetFloat("_AuroraSteps", (float)this.auroraSettings.auroraSteps);
				skyMat.SetVector("_Aurora_Tiling_Layer1", this.auroraSettings.auroraLayer1Settings);
				skyMat.SetVector("_Aurora_Tiling_Layer2", this.auroraSettings.auroraLayer2Settings);
				skyMat.SetVector("_Aurora_Tiling_ColorShift", this.auroraSettings.auroraColorshiftSettings);
			}
			else
			{
				skyMat.DisableKeyword("ENVIRO_AURORA");
			}
		}
		else
		{
			skyMat.SetVector("_SunDir", -this.Components.Sun.transform.forward);
			skyMat.SetVector("_MoonDir", this.Components.Moon.transform.forward);
			skyMat.SetColor("_MoonColor", this.skySettings.moonColor);
			skyMat.SetColor("_scatteringColor", this.skySettings.scatteringColor.Evaluate(this.GameTime.solarTime));
			skyMat.SetColor("_sunDiskColor", this.skySettings.sunDiskColor.Evaluate(this.GameTime.solarTime));
			skyMat.SetColor("_weatherSkyMod", Color.Lerp(this.currentWeatherSkyMod, this.interiorZoneSettings.currentInteriorSkyboxMod, this.interiorZoneSettings.currentInteriorSkyboxMod.a));
			skyMat.SetColor("_weatherFogMod", Color.Lerp(this.currentWeatherFogMod, this.interiorZoneSettings.currentInteriorFogColorMod, this.interiorZoneSettings.currentInteriorFogColorMod.a));
			skyMat.SetVector("_Bm", base.BetaMie(this.skySettings.turbidity, this.skySettings.waveLength) * (this.skySettings.mie * this.Fog.scatteringStrenght));
			skyMat.SetVector("_Br", base.BetaRay(this.skySettings.waveLength) * this.skySettings.rayleigh);
			skyMat.SetVector("_mieG", base.GetMieG(this.skySettings.g));
			skyMat.SetFloat("_SunIntensity", this.skySettings.sunIntensity);
			skyMat.SetFloat("_SunDiskSize", this.skySettings.sunDiskScale);
			skyMat.SetFloat("_SunDiskIntensity", this.skySettings.sunDiskIntensity);
			skyMat.SetFloat("_SunDiskSize", this.skySettings.sunDiskScale);
			skyMat.SetFloat("_Tonemapping", this.tonemapping ? 1f : 0f);
			skyMat.SetFloat("_SkyExposure", this.skySettings.skyExposure);
			skyMat.SetFloat("_SkyLuminance", this.skySettings.skyLuminence.Evaluate(this.GameTime.solarTime));
			skyMat.SetFloat("_scatteringPower", this.skySettings.scatteringCurve.Evaluate(this.GameTime.solarTime));
			skyMat.SetFloat("_SkyColorPower", this.skySettings.skyColorPower.Evaluate(this.GameTime.solarTime));
			skyMat.SetFloat("_StarsIntensity", this.skySettings.starsIntensity.Evaluate(this.GameTime.solarTime));
			skyMat.SetFloat("_GalaxyIntensity", this.skySettings.galaxyIntensity.Evaluate(this.GameTime.solarTime));
			skyMat.SetFloat("_DitheringIntensity", this.skySettings.dithering);
			if (this.skySettings.moonPhaseMode == EnviroSkySettings.MoonPhases.Realistic)
			{
				float num2 = Vector3.SignedAngle(this.Components.Moon.transform.forward, this.Components.Sun.transform.forward, base.transform.forward);
				if (this.GameTime.Latitude >= 0f)
				{
					if (num2 < 0f)
					{
						this.customMoonPhase = base.Remap(num2, 0f, -180f, -2f, 0f);
					}
					else
					{
						this.customMoonPhase = base.Remap(num2, 0f, 180f, 2f, 0f);
					}
				}
				else if (num2 < 0f)
				{
					this.customMoonPhase = base.Remap(num2, 0f, -180f, 2f, 0f);
				}
				else
				{
					this.customMoonPhase = base.Remap(num2, 0f, 180f, -2f, 0f);
				}
			}
			skyMat.SetColor("_moonGlowColor", this.skySettings.moonGlowColor);
			skyMat.SetVector("_moonParams", new Vector4(this.skySettings.moonSize, this.skySettings.glowSize, this.skySettings.moonGlow.Evaluate(this.GameTime.solarTime), this.customMoonPhase));
			if (this.skySettings.renderMoon)
			{
				skyMat.SetTexture("_MoonTex", this.skySettings.moonTexture);
				skyMat.SetTexture("_GlowTex", this.skySettings.glowTexture);
			}
			else
			{
				skyMat.SetTexture("_MoonTex", null);
				skyMat.SetTexture("_GlowTex", null);
			}
			if (this.skySettings.blackGroundMode)
			{
				skyMat.SetInt("_blackGround", 1);
			}
			else
			{
				skyMat.SetInt("_blackGround", 0);
			}
			skyMat.SetFloat("_StarsTwinkling", this.skySettings.starsTwinklingRate);
			if (this.skySettings.starsTwinklingRate > 0f)
			{
				this.starsTwinklingRot += this.skySettings.starsTwinklingRate * Time.deltaTime;
				Quaternion q2 = Quaternion.Euler(this.starsTwinklingRot, this.starsTwinklingRot, this.starsTwinklingRot);
				Matrix4x4 value2 = Matrix4x4.TRS(Vector3.zero, q2, new Vector3(1f, 1f, 1f));
				skyMat.SetMatrix("_StarsTwinklingMatrix", value2);
			}
			if (this.useAurora)
			{
				skyMat.EnableKeyword("ENVIRO_AURORA");
				skyMat.SetFloat("_AuroraIntensity", Mathf.Clamp01(this.auroraIntensity * this.auroraSettings.auroraIntensity.Evaluate(this.GameTime.solarTime)));
				skyMat.SetFloat("_AuroraBrightness", this.auroraSettings.auroraBrightness);
				skyMat.SetFloat("_AuroraContrast", this.auroraSettings.auroraContrast);
				skyMat.SetColor("_AuroraColor", this.auroraSettings.auroraColor);
				skyMat.SetFloat("_AuroraHeight", this.auroraSettings.auroraHeight);
				skyMat.SetFloat("_AuroraScale", this.auroraSettings.auroraScale);
				skyMat.SetFloat("_AuroraSpeed", this.auroraSettings.auroraSpeed);
				skyMat.SetFloat("_AuroraSteps", (float)this.auroraSettings.auroraSteps);
				skyMat.SetFloat("_AuroraSteps", (float)this.auroraSettings.auroraSteps);
				skyMat.SetVector("_Aurora_Tiling_Layer1", this.auroraSettings.auroraLayer1Settings);
				skyMat.SetVector("_Aurora_Tiling_Layer2", this.auroraSettings.auroraLayer2Settings);
				skyMat.SetVector("_Aurora_Tiling_ColorShift", this.auroraSettings.auroraColorshiftSettings);
			}
			else
			{
				skyMat.DisableKeyword("ENVIRO_AURORA");
			}
		}
		skyMat.SetVector("_CloudCirrusAnimation", this.cirrusAnim);
		if (this.useFlatClouds)
		{
			if (this.cloudsSettings.flatCloudsBaseTexture != null)
			{
				skyMat.SetTexture("_FlatCloudsBaseTexture", this.cloudsSettings.flatCloudsBaseTexture);
			}
			if (this.cloudsSettings.flatCloudsDetailTexture != null)
			{
				skyMat.SetTexture("_FlatCloudsDetailTexture", this.cloudsSettings.flatCloudsDetailTexture);
			}
			skyMat.SetVector("_FlatCloudsLightDirection", this.Components.DirectLight.transform.forward);
			skyMat.SetColor("_FlatCloudsLightColor", this.cloudsSettings.flatCloudsDirectLightColor.Evaluate(this.GameTime.solarTime));
			skyMat.SetColor("_FlatCloudsAmbientColor", this.cloudsSettings.flatCloudsAmbientLightColor.Evaluate(this.GameTime.solarTime));
			skyMat.SetVector("_FlatCloudsParams", new Vector4(this.cloudsConfig.flatCoverage, this.cloudsConfig.flatCloudsDensity, this.cloudsSettings.flatCloudsAltitude, this.tonemapping ? 1f : 0f));
			skyMat.SetVector("_FlatCloudsTiling", new Vector4(this.cloudsSettings.flatCloudsBaseTextureTiling, this.cloudsSettings.flatCloudsDetailTextureTiling, 0f, 0f));
			skyMat.SetVector("_FlatCloudsLightingParams", new Vector4(this.cloudsConfig.flatCloudsDirectLightIntensity, this.cloudsConfig.flatCloudsAmbientLightIntensity, this.cloudsConfig.flatCloudsAbsorbtion, this.cloudsConfig.flatCloudsHGPhase));
			skyMat.SetVector("_FlatCloudsAnimation", new Vector4(this.cloudFlatBaseAnim.x, this.cloudFlatBaseAnim.y, this.cloudFlatDetailAnim.x, this.cloudFlatDetailAnim.y));
			skyMat.SetFloat("_CloudsExposure", this.cloudsSettings.cloudsExposure);
		}
		if (this.cloudsSettings.cirrusCloudsTexture != null)
		{
			skyMat.SetTexture("_CloudMap", this.cloudsSettings.cirrusCloudsTexture);
		}
		skyMat.SetColor("_CloudColor", this.cloudsSettings.cirrusCloudsColor.Evaluate(this.GameTime.solarTime));
		skyMat.SetFloat("_CloudAltitude", this.cloudsSettings.cirrusCloudsAltitude);
		skyMat.SetFloat("_CloudAlpha", this.cloudsConfig.cirrusAlpha);
		skyMat.SetFloat("_CloudCoverage", this.cloudsConfig.cirrusCoverage);
		skyMat.SetFloat("_CloudColorPower", this.cloudsConfig.cirrusColorPow);
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0001F7B8 File Offset: 0x0001D9B8
	private void SetMaterialsVariables()
	{
		if (this.skyMat != null)
		{
			this.UpdateSkyShaderVariables(this.skyMat);
		}
		Shader.SetGlobalColor("_EnviroLighting", this.lightSettings.LightColor.Evaluate(this.GameTime.solarTime));
		Shader.SetGlobalVector("_SunDirection", -this.Components.Sun.transform.forward);
		Shader.SetGlobalVector("_SunPosition", this.Components.Sun.transform.localPosition + -this.Components.Sun.transform.forward * 10000f);
		Shader.SetGlobalVector("_MoonPosition", this.Components.Moon.transform.localPosition);
		Shader.SetGlobalVector("_SunDir", -this.Components.Sun.transform.forward);
		Shader.SetGlobalVector("_MoonDir", -this.Components.Moon.transform.forward);
		Shader.SetGlobalColor("_scatteringColor", this.skySettings.scatteringColor.Evaluate(this.GameTime.solarTime));
		Shader.SetGlobalColor("_sunDiskColor", this.skySettings.sunDiskColor.Evaluate(this.GameTime.solarTime));
		Shader.SetGlobalColor("_weatherSkyMod", Color.Lerp(this.currentWeatherSkyMod, this.interiorZoneSettings.currentInteriorSkyboxMod, this.interiorZoneSettings.currentInteriorSkyboxMod.a));
		Shader.SetGlobalColor("_weatherFogMod", Color.Lerp(this.currentWeatherFogMod, this.interiorZoneSettings.currentInteriorFogColorMod, this.interiorZoneSettings.currentInteriorFogColorMod.a));
		Shader.SetGlobalFloat("_gameTime", Mathf.Clamp(1f - this.GameTime.solarTime, 0.5f, 1f));
		Shader.SetGlobalVector("_EnviroSkyFog", new Vector4(this.Fog.skyFogHeight, this.Fog.skyFogIntensity, this.Fog.skyFogStart, this.fogSettings.heightFogIntensity));
		Shader.SetGlobalFloat("_scatteringStrenght", this.Fog.scatteringStrenght);
		Shader.SetGlobalFloat("_SunBlocking", this.Fog.sunBlocking);
		Shader.SetGlobalVector("_EnviroParams", new Vector4(Mathf.Clamp(1f - this.GameTime.solarTime, 0.5f, 1f), this.fogSettings.distanceFog ? 1f : 0f, this.fogSettings.heightFog ? 1f : 0f, this.tonemapping ? 1f : 0f));
		Shader.SetGlobalVector("_Bm", base.BetaMie(this.skySettings.turbidity, this.skySettings.waveLength) * (this.skySettings.mie * (this.Fog.scatteringStrenght * this.GameTime.solarTime)));
		Shader.SetGlobalVector("_BmScene", base.BetaMie(this.skySettings.turbidity, this.skySettings.waveLength) * (this.fogSettings.mie * (this.Fog.scatteringStrenght * this.GameTime.solarTime)));
		Shader.SetGlobalVector("_Br", base.BetaRay(this.skySettings.waveLength) * this.skySettings.rayleigh);
		Shader.SetGlobalVector("_mieG", base.GetMieG(this.skySettings.g));
		Shader.SetGlobalVector("_mieGScene", base.GetMieGScene(this.skySettings.g));
		Shader.SetGlobalVector("_SunParameters", new Vector4(this.skySettings.sunIntensity, this.skySettings.sunDiskScale, this.skySettings.sunDiskIntensity, this.cloudsSettings.cloudsSkyFogHeightBlending));
		Shader.SetGlobalFloat("_FogExposure", this.fogSettings.fogExposure);
		Shader.SetGlobalFloat("_SkyLuminance", this.skySettings.skyLuminence.Evaluate(this.GameTime.solarTime));
		Shader.SetGlobalFloat("_scatteringPower", this.skySettings.scatteringCurve.Evaluate(this.GameTime.solarTime));
		Shader.SetGlobalFloat("_SkyColorPower", this.skySettings.skyColorPower.Evaluate(this.GameTime.solarTime));
		Shader.SetGlobalFloat("_distanceFogIntensity", this.fogSettings.distanceFogIntensity);
		if (this.cloudsSettings.depthBlending)
		{
			Shader.SetGlobalTexture("_EnviroCloudsTex", this.cloudsRenderTarget);
		}
		if (Application.isPlaying || this.showFogInEditor)
		{
			Shader.SetGlobalFloat("_maximumFogDensity", 1f - this.fogSettings.maximumFogDensity);
		}
		else if (!this.showFogInEditor)
		{
			Shader.SetGlobalFloat("_maximumFogDensity", 1f);
		}
		Shader.SetGlobalFloat("_lightning", this.thunder);
		if (this.fogSettings.useSimpleFog)
		{
			Shader.EnableKeyword("ENVIRO_SIMPLE_FOG");
			return;
		}
		Shader.DisableKeyword("ENVIRO_SIMPLE_FOG");
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0001FD1C File Offset: 0x0001DF1C
	private void ValidateParameters()
	{
		this.internalHour = Mathf.Repeat(this.internalHour, 24f);
		this.GameTime.Longitude = Mathf.Clamp(this.GameTime.Longitude, -180f, 180f);
		this.GameTime.Latitude = Mathf.Clamp(this.GameTime.Latitude, -90f, 90f);
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0001FD8C File Offset: 0x0001DF8C
	private void UpdateClouds(EnviroWeatherPreset i, bool withTransition)
	{
		if (i == null)
		{
			return;
		}
		float num = 500f * Time.deltaTime;
		if (withTransition)
		{
			num = this.weatherSettings.cloudTransitionSpeed * Time.deltaTime;
		}
		this.cloudsConfig.cirrusAlpha = Mathf.Lerp(this.cloudsConfig.cirrusAlpha, i.cloudsConfig.cirrusAlpha, num);
		this.cloudsConfig.cirrusCoverage = Mathf.Lerp(this.cloudsConfig.cirrusCoverage, i.cloudsConfig.cirrusCoverage, num);
		this.cloudsConfig.cirrusColorPow = Mathf.Lerp(this.cloudsConfig.cirrusColorPow, i.cloudsConfig.cirrusColorPow, num);
		this.cloudsConfig.coverage = Mathf.Lerp(this.cloudsConfig.coverage, i.cloudsConfig.coverage, num);
		this.cloudsConfig.ambientSkyColorIntensity = Mathf.Lerp(this.cloudsConfig.ambientSkyColorIntensity, i.cloudsConfig.ambientSkyColorIntensity, num);
		if (this.useVolumeClouds)
		{
			this.cloudsConfig.raymarchingScale = Mathf.Lerp(this.cloudsConfig.raymarchingScale, i.cloudsConfig.raymarchingScale, num);
			this.cloudsConfig.ambientSkyColorIntensity = Mathf.Lerp(this.cloudsConfig.ambientSkyColorIntensity, i.cloudsConfig.ambientSkyColorIntensity, num);
			this.cloudsConfig.density = Mathf.Lerp(this.cloudsConfig.density, i.cloudsConfig.density, num);
			this.cloudsConfig.lightStepModifier = Mathf.Lerp(this.cloudsConfig.lightStepModifier, i.cloudsConfig.lightStepModifier, num);
			this.cloudsConfig.lightAbsorbtion = Mathf.Lerp(this.cloudsConfig.lightAbsorbtion, i.cloudsConfig.lightAbsorbtion, num);
			this.cloudsConfig.lightVariance = Mathf.Lerp(this.cloudsConfig.lightVariance, i.cloudsConfig.lightVariance, num);
			this.cloudsConfig.scatteringCoef = Mathf.Lerp(this.cloudsConfig.scatteringCoef, i.cloudsConfig.scatteringCoef, num);
			this.cloudsConfig.cloudType = Mathf.Lerp(this.cloudsConfig.cloudType, i.cloudsConfig.cloudType, num);
			this.cloudsConfig.coverageType = Mathf.Lerp(this.cloudsConfig.coverageType, i.cloudsConfig.coverageType, num);
			this.cloudsConfig.edgeDarkness = Mathf.Lerp(this.cloudsConfig.edgeDarkness, i.cloudsConfig.edgeDarkness, num);
			this.cloudsConfig.baseErosionIntensity = Mathf.Lerp(this.cloudsConfig.baseErosionIntensity, i.cloudsConfig.baseErosionIntensity, num);
			this.cloudsConfig.detailErosionIntensity = Mathf.Lerp(this.cloudsConfig.detailErosionIntensity, i.cloudsConfig.detailErosionIntensity, num);
		}
		if (this.useFlatClouds)
		{
			this.cloudsConfig.flatCloudsAbsorbtion = Mathf.Lerp(this.cloudsConfig.flatCloudsAbsorbtion, i.cloudsConfig.flatCloudsAbsorbtion, num);
			this.cloudsConfig.flatCoverage = Mathf.Lerp(this.cloudsConfig.flatCoverage, i.cloudsConfig.flatCoverage, num);
			this.cloudsConfig.flatCloudsAmbientLightIntensity = Mathf.Lerp(this.cloudsConfig.flatCloudsAmbientLightIntensity, i.cloudsConfig.flatCloudsAmbientLightIntensity, num);
			this.cloudsConfig.flatCloudsDirectLightIntensity = Mathf.Lerp(this.cloudsConfig.flatCloudsDirectLightIntensity, i.cloudsConfig.flatCloudsDirectLightIntensity, num);
			this.cloudsConfig.flatCloudsDensity = Mathf.Lerp(this.cloudsConfig.flatCloudsDensity, i.cloudsConfig.flatCloudsDensity, num);
			this.cloudsConfig.flatCloudsHGPhase = Mathf.Lerp(this.cloudsConfig.flatCloudsHGPhase, i.cloudsConfig.flatCloudsHGPhase, num);
		}
		this.cloudsConfig.particleLayer1Alpha = Mathf.Lerp(this.cloudsConfig.particleLayer1Alpha, i.cloudsConfig.particleLayer1Alpha, num * 0.25f);
		this.cloudsConfig.particleLayer1Brightness = Mathf.Lerp(this.cloudsConfig.particleLayer1Brightness, i.cloudsConfig.particleLayer1Brightness, num * 0.25f);
		this.cloudsConfig.particleLayer2Alpha = Mathf.Lerp(this.cloudsConfig.particleLayer2Alpha, i.cloudsConfig.particleLayer2Alpha, num * 0.25f);
		this.cloudsConfig.particleLayer2Brightness = Mathf.Lerp(this.cloudsConfig.particleLayer2Brightness, i.cloudsConfig.particleLayer2Brightness, num * 0.25f);
		this.globalVolumeLightIntensity = Mathf.Lerp(this.globalVolumeLightIntensity, i.volumeLightIntensity, num);
		this.shadowIntensityMod = Mathf.Lerp(this.shadowIntensityMod, i.shadowIntensityMod, num);
		this.currentWeatherSkyMod = Color.Lerp(this.currentWeatherSkyMod, i.weatherSkyMod.Evaluate(this.GameTime.solarTime), num);
		this.currentWeatherFogMod = Color.Lerp(this.currentWeatherFogMod, i.weatherFogMod.Evaluate(this.GameTime.solarTime), num * 10f);
		this.currentWeatherLightMod = Color.Lerp(this.currentWeatherLightMod, i.weatherLightMod.Evaluate(this.GameTime.solarTime), num);
		this.auroraIntensity = Mathf.Lerp(this.auroraIntensity, i.auroraIntensity, num);
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x000202D8 File Offset: 0x0001E4D8
	private void UpdateFog(EnviroWeatherPreset i, bool withTransition)
	{
		RenderSettings.fogColor = Color.Lerp(Color.Lerp(this.fogSettings.simpleFogColor.Evaluate(this.GameTime.solarTime), this.customFogColor, this.customFogIntensity), this.currentWeatherFogMod, this.currentWeatherFogMod.a);
		if (i != null)
		{
			float t = 500f * Time.deltaTime;
			if (withTransition)
			{
				t = this.weatherSettings.fogTransitionSpeed * Time.deltaTime;
			}
			if (this.fogSettings.Fogmode == FogMode.Linear)
			{
				RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, i.fogDistance, t);
				RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, i.fogStartDistance, t);
			}
			else if (this.updateFogDensity)
			{
				RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, i.fogDensity, t) * this.interiorZoneSettings.currentInteriorFogMod;
			}
			this.fogSettings.heightDensity = Mathf.Lerp(this.fogSettings.heightDensity, i.heightFogDensity, t);
			this.Fog.skyFogStart = Mathf.Lerp(this.Fog.skyFogStart, i.skyFogStart, t);
			this.Fog.skyFogHeight = Mathf.Lerp(this.Fog.skyFogHeight, i.SkyFogHeight, t);
			this.Fog.skyFogIntensity = Mathf.Lerp(this.Fog.skyFogIntensity, i.SkyFogIntensity, t);
			this.fogSettings.skyFogIntensity = Mathf.Lerp(this.fogSettings.skyFogIntensity, i.SkyFogIntensity, t);
			this.Fog.scatteringStrenght = Mathf.Lerp(this.Fog.scatteringStrenght, i.FogScatteringIntensity, t);
			this.Fog.sunBlocking = Mathf.Lerp(this.Fog.sunBlocking, i.fogSunBlocking, t);
		}
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x000204B0 File Offset: 0x0001E6B0
	private void UpdatePostProcessing(EnviroWeatherPreset i, bool withTransition)
	{
		if (i != null)
		{
			float t = 500f * Time.deltaTime;
			if (withTransition)
			{
				t = 10f * Time.deltaTime;
			}
			this.blurDistance = Mathf.Lerp(this.blurDistance, i.blurDistance, t);
			this.blurIntensity = Mathf.Lerp(this.blurIntensity, i.blurIntensity, t);
			this.blurSkyIntensity = Mathf.Lerp(this.blurSkyIntensity, i.blurSkyIntensity, t);
		}
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x0002052C File Offset: 0x0001E72C
	private void UpdateEffectSystems(EnviroWeatherPrefab id, bool withTransition)
	{
		if (id != null)
		{
			float num = 500f * Time.deltaTime;
			if (withTransition)
			{
				num = this.weatherSettings.effectTransitionSpeed * Time.deltaTime;
			}
			for (int i = 0; i < id.effectSystems.Count; i++)
			{
				if (id.effectSystems[i].isStopped)
				{
					id.effectSystems[i].Play();
				}
				float emissionRate = Mathf.Lerp(EnviroSkyMgr.instance.GetEmissionRate(id.effectSystems[i]), id.effectEmmisionRates[i] * this.qualitySettings.GlobalParticleEmissionRates, num) * this.interiorZoneSettings.currentInteriorWeatherEffectMod;
				EnviroSkyMgr.instance.SetEmissionRate(id.effectSystems[i], emissionRate);
			}
			for (int j = 0; j < this.Weather.WeatherPrefabs.Count; j++)
			{
				if (this.Weather.WeatherPrefabs[j].gameObject != id.gameObject)
				{
					for (int k = 0; k < this.Weather.WeatherPrefabs[j].effectSystems.Count; k++)
					{
						float num2 = Mathf.Lerp(EnviroSkyMgr.instance.GetEmissionRate(this.Weather.WeatherPrefabs[j].effectSystems[k]), 0f, num * 10f);
						if (num2 < 1f)
						{
							num2 = 0f;
						}
						EnviroSkyMgr.instance.SetEmissionRate(this.Weather.WeatherPrefabs[j].effectSystems[k], num2);
						if (num2 == 0f && !this.Weather.WeatherPrefabs[j].effectSystems[k].isStopped)
						{
							this.Weather.WeatherPrefabs[j].effectSystems[k].Stop();
						}
					}
				}
			}
			base.UpdateWeatherVariables(id.weatherPreset);
		}
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x00020744 File Offset: 0x0001E944
	private void UpdateWeather()
	{
		if (this.Weather.currentActiveWeatherPreset != this.Weather.currentActiveZone.currentActiveZoneWeatherPreset)
		{
			this.Weather.lastActiveWeatherPreset = this.Weather.currentActiveWeatherPreset;
			this.Weather.lastActiveWeatherPrefab = this.Weather.currentActiveWeatherPrefab;
			this.Weather.currentActiveWeatherPreset = this.Weather.currentActiveZone.currentActiveZoneWeatherPreset;
			this.Weather.currentActiveWeatherPrefab = this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab;
			if (this.Weather.currentActiveWeatherPreset != null)
			{
				EnviroSkyMgr.instance.NotifyWeatherChanged(this.Weather.currentActiveWeatherPreset);
				this.Weather.weatherFullyChanged = false;
				if (!this.serverMode)
				{
					base.TryPlayAmbientSFX();
					base.UpdateAudioSource(this.Weather.currentActiveWeatherPreset);
					if (this.Weather.currentActiveWeatherPreset.isLightningStorm)
					{
						base.StartCoroutine(base.PlayThunderRandom());
					}
					else
					{
						base.StopCoroutine(base.PlayThunderRandom());
						this.Components.LightningGenerator.StopLightning();
					}
				}
			}
		}
		if (this.Weather.currentActiveWeatherPrefab != null && !this.serverMode)
		{
			this.UpdateClouds(this.Weather.currentActiveWeatherPreset, true);
			this.UpdateFog(this.Weather.currentActiveWeatherPreset, true);
			this.UpdatePostProcessing(this.Weather.currentActiveWeatherPreset, true);
			this.UpdateEffectSystems(this.Weather.currentActiveWeatherPrefab, true);
			if (!this.Weather.weatherFullyChanged)
			{
				base.CalcWeatherTransitionState();
				return;
			}
		}
		else if (this.Weather.currentActiveWeatherPrefab != null)
		{
			base.UpdateWeatherVariables(this.Weather.currentActiveWeatherPrefab.weatherPreset);
		}
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x0000245B File Offset: 0x0000065B
	public void PopulateCloudsQualityList()
	{
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x00020908 File Offset: 0x0001EB08
	public void ApplyVolumeCloudsQualityPreset(EnviroVolumeCloudsQuality preset)
	{
		this.cloudsSettings.cloudsQualitySettings = preset.qualitySettings;
		this.currentActiveCloudsQualityPreset = preset;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00020924 File Offset: 0x0001EB24
	public void ApplyVolumeCloudsQualityPreset(string name)
	{
		for (int i = 0; i < this.cloudsQualityList.Count; i++)
		{
			if (this.cloudsQualityList[i].name == name)
			{
				this.cloudsSettings.cloudsQualitySettings = this.cloudsQualityList[i].qualitySettings;
				this.currentActiveCloudsQualityPreset = this.cloudsQualityList[i];
				this.selectedCloudsQuality = i;
			}
		}
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x00020998 File Offset: 0x0001EB98
	public void ApplyVolumeCloudsQualityPreset(int id)
	{
		if (id < this.cloudsQualityList.Count && id >= 0)
		{
			this.cloudsSettings.cloudsQualitySettings = this.cloudsQualityList[id].qualitySettings;
			this.currentActiveCloudsQualityPreset = this.cloudsQualityList[id];
			this.selectedCloudsQuality = id;
		}
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x000209EC File Offset: 0x0001EBEC
	public void InstantWeatherChange(EnviroWeatherPreset preset, EnviroWeatherPrefab prefab)
	{
		this.UpdateClouds(preset, false);
		this.UpdateFog(preset, false);
		this.UpdatePostProcessing(preset, false);
		this.UpdateEffectSystems(prefab, false);
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x00020A10 File Offset: 0x0001EC10
	public void AssignAndStart(GameObject player, Camera Camera)
	{
		this.Player = player;
		this.PlayerCamera = Camera;
		this.Init();
		this.started = true;
		if (this.reflectionSettings.globalReflections && this.Components.GlobalReflectionProbe != null)
		{
			if (this.reflectionSettings.globalReflectionCustomRendering)
			{
				this.Components.GlobalReflectionProbe.RefreshReflection(false);
				return;
			}
			this.Components.GlobalReflectionProbe.myProbe.RenderProbe();
		}
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00020A8D File Offset: 0x0001EC8D
	public void StartAsServer()
	{
		this.Player = base.gameObject;
		this.serverMode = true;
		this.Init();
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x00020AA8 File Offset: 0x0001ECA8
	public void ChangeFocus(GameObject player, Camera Camera)
	{
		this.Player = player;
		if (this.PlayerCamera != null)
		{
			this.RemoveEnviroCameraComponents(this.PlayerCamera);
		}
		this.PlayerCamera = Camera;
		this.InitImageEffects();
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x00020AD8 File Offset: 0x0001ECD8
	private void RemoveEnviroCameraComponents(Camera cam)
	{
		EnviroSkyRendering component = cam.GetComponent<EnviroSkyRendering>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		EnviroPostProcessing component2 = cam.GetComponent<EnviroPostProcessing>();
		if (component2 != null)
		{
			UnityEngine.Object.Destroy(component2);
		}
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00020B14 File Offset: 0x0001ED14
	public void Play(EnviroTime.TimeProgressMode progressMode = EnviroTime.TimeProgressMode.Simulated)
	{
		base.StartCoroutine(this.SetSceneSettingsLate());
		if (!this.Components.DirectLight.gameObject.activeSelf)
		{
			this.Components.DirectLight.gameObject.SetActive(true);
		}
		this.GameTime.ProgressTime = progressMode;
		if (this.EffectsHolder != null)
		{
			this.EffectsHolder.SetActive(true);
		}
		if (this.EnviroSkyRender != null)
		{
			this.EnviroSkyRender.enabled = true;
		}
		if (this.EnviroPostProcessing != null)
		{
			this.EnviroPostProcessing.enabled = true;
		}
		base.TryPlayAmbientSFX();
		if (this.Weather.currentAudioSource != null)
		{
			this.Weather.currentAudioSource.audiosrc.Play();
		}
		this.started = true;
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x00020BEC File Offset: 0x0001EDEC
	public void Stop(bool disableLight = false, bool stopTime = true)
	{
		if (disableLight)
		{
			this.Components.DirectLight.gameObject.SetActive(false);
		}
		if (stopTime)
		{
			this.GameTime.ProgressTime = EnviroTime.TimeProgressMode.None;
		}
		if (this.EffectsHolder != null)
		{
			this.EffectsHolder.SetActive(false);
		}
		if (this.EnviroSkyRender != null)
		{
			this.EnviroSkyRender.enabled = false;
		}
		if (this.EnviroPostProcessing != null)
		{
			this.EnviroPostProcessing.enabled = false;
		}
		this.started = false;
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x00020C78 File Offset: 0x0001EE78
	public void Deactivate(bool disableLight = false)
	{
		if (disableLight)
		{
			this.Components.DirectLight.gameObject.SetActive(false);
		}
		if (this.EffectsHolder != null)
		{
			this.EffectsHolder.SetActive(false);
		}
		if (this.EnviroSkyRender != null)
		{
			this.EnviroSkyRender.enabled = false;
		}
		if (this.EnviroPostProcessing != null)
		{
			this.EnviroPostProcessing.enabled = false;
		}
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x00020CEC File Offset: 0x0001EEEC
	public void Activate()
	{
		this.Components.DirectLight.gameObject.SetActive(true);
		if (this.EffectsHolder != null)
		{
			this.EffectsHolder.SetActive(true);
		}
		if (this.EnviroSkyRender != null)
		{
			this.EnviroSkyRender.enabled = true;
		}
		if (this.EnviroPostProcessing != null)
		{
			this.EnviroPostProcessing.enabled = true;
		}
		base.TryPlayAmbientSFX();
		if (this.Weather.currentAudioSource != null)
		{
			this.Weather.currentAudioSource.audiosrc.Play();
		}
	}

	// Token: 0x040005A0 RID: 1440
	private static EnviroSky _instance;

	// Token: 0x040005A1 RID: 1441
	[Tooltip("Enable this when using singlepass rendering.")]
	public bool singlePassVR;

	// Token: 0x040005A2 RID: 1442
	[Tooltip("Enable this when using singlepass instanced rendering.")]
	public bool singlePassInstancedVR;

	// Token: 0x040005A3 RID: 1443
	[Tooltip("Enable this to activate volume lighing")]
	[HideInInspector]
	public bool useVolumeLighting = true;

	// Token: 0x040005A4 RID: 1444
	[HideInInspector]
	public bool useVolumeClouds = true;

	// Token: 0x040005A5 RID: 1445
	[HideInInspector]
	public bool useFog = true;

	// Token: 0x040005A6 RID: 1446
	[HideInInspector]
	public bool useFlatClouds;

	// Token: 0x040005A7 RID: 1447
	[HideInInspector]
	public bool useParticleClouds;

	// Token: 0x040005A8 RID: 1448
	[HideInInspector]
	public bool useDistanceBlur = true;

	// Token: 0x040005A9 RID: 1449
	[HideInInspector]
	public bool useAurora;

	// Token: 0x040005AA RID: 1450
	private bool flatCloudsSkybox;

	// Token: 0x040005AB RID: 1451
	public bool showVolumeLightingInEditor = true;

	// Token: 0x040005AC RID: 1452
	public bool showVolumeCloudsInEditor = true;

	// Token: 0x040005AD RID: 1453
	public bool showFlatCloudsInEditor = true;

	// Token: 0x040005AE RID: 1454
	public bool showFogInEditor = true;

	// Token: 0x040005AF RID: 1455
	public bool showDistanceBlurInEditor = true;

	// Token: 0x040005B0 RID: 1456
	public bool showSettings;

	// Token: 0x040005B1 RID: 1457
	[HideInInspector]
	public Camera satCamera;

	// Token: 0x040005B2 RID: 1458
	[HideInInspector]
	public EnviroVolumeLight directVolumeLight;

	// Token: 0x040005B3 RID: 1459
	[HideInInspector]
	public EnviroVolumeLight additionalDirectVolumeLight;

	// Token: 0x040005B4 RID: 1460
	[HideInInspector]
	public EnviroSkyRendering EnviroSkyRender;

	// Token: 0x040005B5 RID: 1461
	public float globalVolumeLightIntensity;

	// Token: 0x040005B6 RID: 1462
	public float auroraIntensity;

	// Token: 0x040005B7 RID: 1463
	public EnviroVolumeCloudsQuality currentActiveCloudsQualityPreset;

	// Token: 0x040005B8 RID: 1464
	[HideInInspector]
	public RenderTexture cloudsRenderTarget;

	// Token: 0x040005B9 RID: 1465
	[HideInInspector]
	public RenderTexture weatherMap;

	// Token: 0x040005BA RID: 1466
	[HideInInspector]
	public RenderTexture satRenderTarget;

	// Token: 0x040005BB RID: 1467
	[HideInInspector]
	public RenderTexture cloudShadowMap;

	// Token: 0x040005BC RID: 1468
	[HideInInspector]
	public Material skyMat;

	// Token: 0x040005BD RID: 1469
	[HideInInspector]
	public Material skyReflectionMat;

	// Token: 0x040005BE RID: 1470
	private Material weatherMapMat;

	// Token: 0x040005BF RID: 1471
	private Material cloudShadowMat;

	// Token: 0x040005C0 RID: 1472
	public List<EnviroVolumeCloudsQuality> cloudsQualityList = new List<EnviroVolumeCloudsQuality>();

	// Token: 0x040005C1 RID: 1473
	private string[] cloudsQualityPresetsFound;

	// Token: 0x040005C2 RID: 1474
	public int selectedCloudsQuality;

	// Token: 0x040005C3 RID: 1475
	private float starsTwinklingRot;

	// Token: 0x040005C4 RID: 1476
	private EnviroSkySettings.SkyboxModi currentSkyboxMode;

	// Token: 0x040005C5 RID: 1477
	public float blurDistance = 100f;

	// Token: 0x040005C6 RID: 1478
	public float blurIntensity = 1f;

	// Token: 0x040005C7 RID: 1479
	public float blurSkyIntensity = 1f;

	// Token: 0x040005C8 RID: 1480
	public Transform floatingPointOriginAnchor;

	// Token: 0x040005C9 RID: 1481
	public Vector3 floatingPointOriginMod = Vector3.zero;

	// Token: 0x040005CA RID: 1482
	[HideInInspector]
	public EnviroRessources ressources;
}
