using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000092 RID: 146
[AddComponentMenu("Enviro/Reflection Probe")]
[RequireComponent(typeof(ReflectionProbe))]
[ExecuteInEditMode]
public class EnviroReflectionProbe : MonoBehaviour
{
	// Token: 0x0600023C RID: 572 RVA: 0x00012F48 File Offset: 0x00011148
	private void OnEnable()
	{
		this.myProbe = base.GetComponent<ReflectionProbe>();
		if (!this.standalone && this.myProbe != null)
		{
			this.myProbe.enabled = true;
		}
		if (this.customRendering)
		{
			this.myProbe.mode = ReflectionProbeMode.Custom;
			this.myProbe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
			this.CreateCubemap();
			this.CreateTexturesAndMaterial();
			this.CreateRenderCamera();
			this.currentRes = this.myProbe.resolution;
			this.rendering = false;
			base.StartCoroutine(this.RefreshFirstTime());
			return;
		}
		this.myProbe.mode = ReflectionProbeMode.Realtime;
		this.myProbe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
		this.myProbe.RenderProbe();
	}

	// Token: 0x0600023D RID: 573 RVA: 0x00012FFF File Offset: 0x000111FF
	private void OnDisable()
	{
		this.Cleanup();
		if (!this.standalone && this.myProbe != null)
		{
			this.myProbe.enabled = false;
		}
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0001302C File Offset: 0x0001122C
	private void Cleanup()
	{
		if (this.refreshing != null)
		{
			base.StopCoroutine(this.refreshing);
		}
		if (this.cubemap != null)
		{
			if (this.renderCam != null)
			{
				this.renderCam.targetTexture = null;
			}
			UnityEngine.Object.DestroyImmediate(this.cubemap);
		}
		if (this.renderCamObj != null)
		{
			UnityEngine.Object.DestroyImmediate(this.renderCamObj);
		}
		if (this.mirrorTexture != null)
		{
			UnityEngine.Object.DestroyImmediate(this.mirrorTexture);
		}
		if (this.renderTexture != null)
		{
			UnityEngine.Object.DestroyImmediate(this.renderTexture);
		}
	}

	// Token: 0x0600023F RID: 575 RVA: 0x000130CC File Offset: 0x000112CC
	private void CreateRenderCamera()
	{
		if (this.renderCamObj == null)
		{
			this.renderCamObj = new GameObject();
			this.renderCamObj.name = "Reflection Probe Cam";
			this.renderCamObj.hideFlags = HideFlags.HideAndDontSave;
			this.renderCam = this.renderCamObj.AddComponent<Camera>();
			this.renderCam.gameObject.SetActive(true);
			this.renderCam.cameraType = CameraType.Reflection;
			this.renderCam.fieldOfView = 90f;
			this.renderCam.farClipPlane = this.myProbe.farClipPlane;
			this.renderCam.nearClipPlane = this.myProbe.nearClipPlane;
			this.renderCam.clearFlags = (CameraClearFlags)this.myProbe.clearFlags;
			this.renderCam.backgroundColor = this.myProbe.backgroundColor;
			this.renderCam.allowHDR = this.myProbe.hdr;
			this.renderCam.targetTexture = this.cubemap;
			this.renderCam.enabled = false;
			if (EnviroSkyMgr.instance != null && EnviroSkyMgr.instance.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
			{
				this.eSky = this.renderCamObj.AddComponent<EnviroSkyRendering>();
				this.eSky.isAddionalCamera = true;
				this.eSky.useGlobalRenderingSettings = false;
				this.eSky.customRenderingSettings.useVolumeClouds = EnviroSkyMgr.instance.useVolumeClouds;
				this.eSky.customRenderingSettings.useVolumeLighting = false;
				this.eSky.customRenderingSettings.useDistanceBlur = false;
				this.eSky.customRenderingSettings.useFog = EnviroSkyMgr.instance.ReflectionSettings.globalReflectionUseFog;
				if (this.customCloudsQuality != null)
				{
					this.eSky.customRenderingSettings.customCloudsQuality = this.customCloudsQuality;
				}
			}
		}
	}

	// Token: 0x06000240 RID: 576 RVA: 0x000132A4 File Offset: 0x000114A4
	private void UpdateCameraSettings()
	{
		if (this.renderCam != null)
		{
			this.renderCam.cullingMask = this.myProbe.cullingMask;
			if (EnviroSkyMgr.instance != null && EnviroSkyMgr.instance.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD && this.eSky != null)
			{
				if (this.customCloudsQuality != null)
				{
					this.eSky.customRenderingSettings.customCloudsQuality = this.customCloudsQuality;
				}
				this.eSky.customRenderingSettings.useVolumeClouds = EnviroSkyMgr.instance.useVolumeClouds;
				this.eSky.customRenderingSettings.useFog = this.useFog;
			}
		}
	}

	// Token: 0x06000241 RID: 577 RVA: 0x00013354 File Offset: 0x00011554
	private Camera CreateBakingCamera()
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "Reflection Probe Cam";
		Camera camera = gameObject.AddComponent<Camera>();
		camera.enabled = false;
		camera.gameObject.SetActive(true);
		camera.cameraType = CameraType.Reflection;
		camera.fieldOfView = 90f;
		camera.farClipPlane = this.myProbe.farClipPlane;
		camera.nearClipPlane = this.myProbe.nearClipPlane;
		camera.cullingMask = this.myProbe.cullingMask;
		camera.clearFlags = (CameraClearFlags)this.myProbe.clearFlags;
		camera.backgroundColor = this.myProbe.backgroundColor;
		camera.allowHDR = this.myProbe.hdr;
		camera.targetTexture = this.cubemap;
		if (EnviroSkyMgr.instance != null && EnviroSkyMgr.instance.currentEnviroSkyVersion == EnviroSkyMgr.EnviroSkyVersion.HD)
		{
			EnviroSkyRendering enviroSkyRendering = gameObject.AddComponent<EnviroSkyRendering>();
			enviroSkyRendering.isAddionalCamera = true;
			enviroSkyRendering.useGlobalRenderingSettings = false;
			enviroSkyRendering.customRenderingSettings.useVolumeClouds = true;
			enviroSkyRendering.customRenderingSettings.useVolumeLighting = false;
			enviroSkyRendering.customRenderingSettings.useDistanceBlur = false;
			enviroSkyRendering.customRenderingSettings.useFog = true;
		}
		gameObject.hideFlags = HideFlags.HideAndDontSave;
		return camera;
	}

	// Token: 0x06000242 RID: 578 RVA: 0x00013478 File Offset: 0x00011678
	private void CreateCubemap()
	{
		if (this.cubemap != null && this.myProbe.resolution == this.currentRes)
		{
			return;
		}
		if (this.cubemap != null)
		{
			UnityEngine.Object.DestroyImmediate(this.cubemap);
		}
		if (this.finalCubemap != null)
		{
			UnityEngine.Object.DestroyImmediate(this.finalCubemap);
		}
		int resolution = this.myProbe.resolution;
		this.currentRes = resolution;
		RenderTextureFormat format = this.myProbe.hdr ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
		this.cubemap = new RenderTexture(resolution, resolution, 16, format, RenderTextureReadWrite.Linear);
		this.cubemap.dimension = TextureDimension.Cube;
		this.cubemap.useMipMap = true;
		this.cubemap.autoGenerateMips = false;
		this.cubemap.name = "Enviro Reflection Temp Cubemap";
		this.cubemap.Create();
		this.finalCubemap = new RenderTexture(resolution, resolution, 16, format, RenderTextureReadWrite.Linear);
		this.finalCubemap.dimension = TextureDimension.Cube;
		this.finalCubemap.useMipMap = true;
		this.finalCubemap.autoGenerateMips = false;
		this.finalCubemap.name = "Enviro Reflection Final Cubemap";
		this.finalCubemap.Create();
	}

	// Token: 0x06000243 RID: 579 RVA: 0x000135A4 File Offset: 0x000117A4
	private void CreateTexturesAndMaterial()
	{
		if (this.mirror == null)
		{
			this.mirror = new Material(Shader.Find("Hidden/Enviro/ReflectionProbe"));
		}
		if (this.convolutionMat == null)
		{
			this.convolutionMat = new Material(Shader.Find("Hidden/CubeBlur"));
		}
		int resolution = this.myProbe.resolution;
		RenderTextureFormat format = this.myProbe.hdr ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
		if (this.mirrorTexture == null || this.mirrorTexture.width != resolution || this.mirrorTexture.height != resolution)
		{
			if (this.mirrorTexture != null)
			{
				UnityEngine.Object.DestroyImmediate(this.mirrorTexture);
			}
			this.mirrorTexture = new RenderTexture(resolution, resolution, 16, format, RenderTextureReadWrite.Linear);
			this.mirrorTexture.useMipMap = true;
			this.mirrorTexture.autoGenerateMips = false;
			this.mirrorTexture.name = "Enviro Reflection Mirror Texture";
			this.mirrorTexture.Create();
		}
		if (this.renderTexture == null || this.renderTexture.width != resolution || this.renderTexture.height != resolution)
		{
			if (this.renderTexture != null)
			{
				UnityEngine.Object.DestroyImmediate(this.renderTexture);
			}
			this.renderTexture = new RenderTexture(resolution, resolution, 16, format, RenderTextureReadWrite.Linear);
			this.renderTexture.useMipMap = true;
			this.renderTexture.autoGenerateMips = false;
			this.renderTexture.name = "Enviro Reflection Target Texture";
			this.renderTexture.Create();
		}
	}

	// Token: 0x06000244 RID: 580 RVA: 0x00013728 File Offset: 0x00011928
	public void RefreshReflection(bool timeSlice = false)
	{
		if (!this.customRendering)
		{
			if (!this.paused)
			{
				base.StartCoroutine(this.RefreshUnity());
			}
			return;
		}
		if (this.rendering || this.paused)
		{
			return;
		}
		this.CreateTexturesAndMaterial();
		if (this.renderCam == null)
		{
			this.CreateRenderCamera();
		}
		this.UpdateCameraSettings();
		this.renderCam.transform.position = base.transform.position;
		this.renderCam.targetTexture = this.renderTexture;
		if (!Application.isPlaying)
		{
			this.refreshing = base.StartCoroutine(this.RefreshInstant(this.renderTexture, this.mirrorTexture));
			return;
		}
		if (!timeSlice)
		{
			this.refreshing = base.StartCoroutine(this.RefreshInstant(this.renderTexture, this.mirrorTexture));
			return;
		}
		this.refreshing = base.StartCoroutine(this.RefreshOvertime(this.renderTexture, this.mirrorTexture));
	}

	// Token: 0x06000245 RID: 581 RVA: 0x00013819 File Offset: 0x00011A19
	private IEnumerator RefreshFirstTime()
	{
		yield return null;
		this.RefreshReflection(false);
		this.RefreshReflection(false);
		yield break;
	}

	// Token: 0x06000246 RID: 582 RVA: 0x00013828 File Offset: 0x00011A28
	public IEnumerator RefreshUnity()
	{
		yield return null;
		this.myProbe.RenderProbe();
		yield break;
	}

	// Token: 0x06000247 RID: 583 RVA: 0x00013837 File Offset: 0x00011A37
	public IEnumerator RefreshInstant(RenderTexture renderTex, RenderTexture mirrorTex)
	{
		this.CreateCubemap();
		yield return null;
		for (int i = 0; i < 6; i++)
		{
			this.renderCam.transform.rotation = EnviroReflectionProbe.orientations[i];
			this.renderCam.Render();
			Graphics.Blit(renderTex, mirrorTex, this.mirror);
			Graphics.CopyTexture(mirrorTex, 0, 0, this.cubemap, i, 0);
		}
		this.ConvolutionCubemap();
		this.myProbe.customBakedTexture = this.finalCubemap;
		this.refreshing = null;
		yield break;
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00013854 File Offset: 0x00011A54
	public IEnumerator RefreshOvertime(RenderTexture renderTex, RenderTexture mirrorTex)
	{
		this.CreateCubemap();
		int num;
		for (int face = 0; face < 6; face = num + 1)
		{
			yield return null;
			this.renderCam.transform.rotation = EnviroReflectionProbe.orientations[face];
			this.renderCam.Render();
			Graphics.Blit(renderTex, mirrorTex, this.mirror);
			Graphics.CopyTexture(mirrorTex, 0, 0, this.cubemap, face, 0);
			num = face;
		}
		this.ConvolutionCubemap();
		this.myProbe.customBakedTexture = this.finalCubemap;
		this.refreshing = null;
		yield break;
	}

	// Token: 0x06000249 RID: 585 RVA: 0x00013874 File Offset: 0x00011A74
	public RenderTexture BakeCubemapFace(int face, int res)
	{
		if (this.bakeMat == null)
		{
			this.bakeMat = new Material(Shader.Find("Hidden/Enviro/BakeCubemap"));
		}
		if (this.bakingCam == null)
		{
			this.bakingCam = this.CreateBakingCamera();
		}
		this.bakingCam.transform.rotation = EnviroReflectionProbe.orientations[face];
		RenderTexture temporary = RenderTexture.GetTemporary(res, res, 0, RenderTextureFormat.ARGBFloat);
		this.bakingCam.targetTexture = temporary;
		this.bakingCam.Render();
		RenderTexture renderTexture = new RenderTexture(res, res, 0, RenderTextureFormat.ARGBFloat);
		Graphics.Blit(temporary, renderTexture, this.bakeMat);
		RenderTexture.ReleaseTemporary(temporary);
		return renderTexture;
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0001391A File Offset: 0x00011B1A
	private void ClearTextures()
	{
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = this.renderTexture;
		GL.Clear(true, true, Color.clear);
		RenderTexture.active = this.mirrorTexture;
		GL.Clear(true, true, Color.clear);
		RenderTexture.active = active;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x00013954 File Offset: 0x00011B54
	private void ConvolutionCubemap()
	{
		int num = 7;
		GL.PushMatrix();
		GL.LoadOrtho();
		for (int i = 0; i < num + 1; i++)
		{
			Graphics.CopyTexture(this.cubemap, 0, i, this.finalCubemap, 0, i);
			Graphics.CopyTexture(this.cubemap, 1, i, this.finalCubemap, 1, i);
			Graphics.CopyTexture(this.cubemap, 2, i, this.finalCubemap, 2, i);
			Graphics.CopyTexture(this.cubemap, 3, i, this.finalCubemap, 3, i);
			Graphics.CopyTexture(this.cubemap, 4, i, this.finalCubemap, 4, i);
			Graphics.CopyTexture(this.cubemap, 5, i, this.finalCubemap, 5, i);
			int num2 = i + 1;
			if (num2 == num)
			{
				break;
			}
			int num3 = this.finalCubemap.width / (int)Mathf.Pow(2f, (float)i);
			this.convolutionMat.SetTexture("_MainTex", this.finalCubemap);
			this.convolutionMat.SetFloat("_Texel", 7f / (float)num3);
			this.convolutionMat.SetFloat("_Level", (float)i);
			this.convolutionMat.SetFloat("_Scale", 0.001f * (float)i);
			this.convolutionMat.SetPass(0);
			Graphics.SetRenderTarget(this.cubemap, num2, CubemapFace.PositiveX);
			GL.Begin(7);
			GL.TexCoord3(1f, 1f, 1f);
			GL.Vertex3(0f, 0f, 1f);
			GL.TexCoord3(1f, -1f, 1f);
			GL.Vertex3(0f, 1f, 1f);
			GL.TexCoord3(1f, -1f, -1f);
			GL.Vertex3(1f, 1f, 1f);
			GL.TexCoord3(1f, 1f, -1f);
			GL.Vertex3(1f, 0f, 1f);
			GL.End();
			Graphics.SetRenderTarget(this.cubemap, num2, CubemapFace.NegativeX);
			GL.Begin(7);
			GL.TexCoord3(-1f, 1f, -1f);
			GL.Vertex3(0f, 0f, 1f);
			GL.TexCoord3(-1f, -1f, -1f);
			GL.Vertex3(0f, 1f, 1f);
			GL.TexCoord3(-1f, -1f, 1f);
			GL.Vertex3(1f, 1f, 1f);
			GL.TexCoord3(-1f, 1f, 1f);
			GL.Vertex3(1f, 0f, 1f);
			GL.End();
			Graphics.SetRenderTarget(this.cubemap, num2, CubemapFace.PositiveY);
			GL.Begin(7);
			GL.TexCoord3(-1f, 1f, -1f);
			GL.Vertex3(0f, 0f, 1f);
			GL.TexCoord3(-1f, 1f, 1f);
			GL.Vertex3(0f, 1f, 1f);
			GL.TexCoord3(1f, 1f, 1f);
			GL.Vertex3(1f, 1f, 1f);
			GL.TexCoord3(1f, 1f, -1f);
			GL.Vertex3(1f, 0f, 1f);
			GL.End();
			Graphics.SetRenderTarget(this.cubemap, num2, CubemapFace.NegativeY);
			GL.Begin(7);
			GL.TexCoord3(-1f, -1f, 1f);
			GL.Vertex3(0f, 0f, 1f);
			GL.TexCoord3(-1f, -1f, -1f);
			GL.Vertex3(0f, 1f, 1f);
			GL.TexCoord3(1f, -1f, -1f);
			GL.Vertex3(1f, 1f, 1f);
			GL.TexCoord3(1f, -1f, 1f);
			GL.Vertex3(1f, 0f, 1f);
			GL.End();
			Graphics.SetRenderTarget(this.cubemap, num2, CubemapFace.PositiveZ);
			GL.Begin(7);
			GL.TexCoord3(-1f, 1f, 1f);
			GL.Vertex3(0f, 0f, 1f);
			GL.TexCoord3(-1f, -1f, 1f);
			GL.Vertex3(0f, 1f, 1f);
			GL.TexCoord3(1f, -1f, 1f);
			GL.Vertex3(1f, 1f, 1f);
			GL.TexCoord3(1f, 1f, 1f);
			GL.Vertex3(1f, 0f, 1f);
			GL.End();
			Graphics.SetRenderTarget(this.cubemap, num2, CubemapFace.NegativeZ);
			GL.Begin(7);
			GL.TexCoord3(1f, 1f, -1f);
			GL.Vertex3(0f, 0f, 1f);
			GL.TexCoord3(1f, -1f, -1f);
			GL.Vertex3(0f, 1f, 1f);
			GL.TexCoord3(-1f, -1f, -1f);
			GL.Vertex3(1f, 1f, 1f);
			GL.TexCoord3(-1f, 1f, -1f);
			GL.Vertex3(1f, 0f, 1f);
			GL.End();
		}
		GL.PopMatrix();
	}

	// Token: 0x0600024C RID: 588 RVA: 0x00013EE0 File Offset: 0x000120E0
	private void UpdateStandaloneReflection()
	{
		if ((EnviroSkyMgr.instance.GetCurrentTimeInHours() > this.lastRelfectionUpdate + (double)this.reflectionsUpdateTreshhold || EnviroSkyMgr.instance.GetCurrentTimeInHours() < this.lastRelfectionUpdate - (double)this.reflectionsUpdateTreshhold) && this.updateReflectionOnGameTime)
		{
			this.lastRelfectionUpdate = EnviroSkyMgr.instance.GetCurrentTimeInHours();
			this.RefreshReflection(!this.useTimeSlicing);
		}
	}

	// Token: 0x0600024D RID: 589 RVA: 0x00013F48 File Offset: 0x00012148
	private void Update()
	{
		if (this.currentMode != this.customRendering)
		{
			this.currentMode = this.customRendering;
			if (this.customRendering)
			{
				this.OnEnable();
			}
			else
			{
				this.OnEnable();
				this.Cleanup();
			}
		}
		if (EnviroSkyMgr.instance != null && this.standalone)
		{
			this.UpdateStandaloneReflection();
		}
	}

	// Token: 0x0400042B RID: 1067
	public bool standalone;

	// Token: 0x0400042C RID: 1068
	public bool updateReflectionOnGameTime = true;

	// Token: 0x0400042D RID: 1069
	public float reflectionsUpdateTreshhold = 0.025f;

	// Token: 0x0400042E RID: 1070
	public bool useTimeSlicing = true;

	// Token: 0x0400042F RID: 1071
	public Camera renderCam;

	// Token: 0x04000430 RID: 1072
	public bool useEnviroEffects = true;

	// Token: 0x04000431 RID: 1073
	[HideInInspector]
	public bool rendering;

	// Token: 0x04000432 RID: 1074
	[HideInInspector]
	public ReflectionProbe myProbe;

	// Token: 0x04000433 RID: 1075
	public bool customRendering;

	// Token: 0x04000434 RID: 1076
	public EnviroVolumeCloudsQuality customCloudsQuality;

	// Token: 0x04000435 RID: 1077
	private EnviroSkyRendering eSky;

	// Token: 0x04000436 RID: 1078
	public bool useFog;

	// Token: 0x04000437 RID: 1079
	private Camera bakingCam;

	// Token: 0x04000438 RID: 1080
	private bool currentMode;

	// Token: 0x04000439 RID: 1081
	private int currentRes;

	// Token: 0x0400043A RID: 1082
	private RenderTexture cubemap;

	// Token: 0x0400043B RID: 1083
	private RenderTexture finalCubemap;

	// Token: 0x0400043C RID: 1084
	private RenderTexture mirrorTexture;

	// Token: 0x0400043D RID: 1085
	private RenderTexture renderTexture;

	// Token: 0x0400043E RID: 1086
	private GameObject renderCamObj;

	// Token: 0x0400043F RID: 1087
	private Material mirror;

	// Token: 0x04000440 RID: 1088
	private Material bakeMat;

	// Token: 0x04000441 RID: 1089
	private Material convolutionMat;

	// Token: 0x04000442 RID: 1090
	private Coroutine refreshing;

	// Token: 0x04000443 RID: 1091
	private bool paused;

	// Token: 0x04000444 RID: 1092
	private int renderID;

	// Token: 0x04000445 RID: 1093
	private static Quaternion[] orientations = new Quaternion[]
	{
		Quaternion.LookRotation(Vector3.right, Vector3.down),
		Quaternion.LookRotation(Vector3.left, Vector3.down),
		Quaternion.LookRotation(Vector3.up, Vector3.forward),
		Quaternion.LookRotation(Vector3.down, Vector3.back),
		Quaternion.LookRotation(Vector3.forward, Vector3.down),
		Quaternion.LookRotation(Vector3.back, Vector3.down)
	};

	// Token: 0x04000446 RID: 1094
	private double lastRelfectionUpdate;
}
