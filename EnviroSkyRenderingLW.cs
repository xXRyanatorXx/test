using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020000C3 RID: 195
[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
[RequireComponent(typeof(Camera))]
public class EnviroSkyRenderingLW : MonoBehaviour
{
	// Token: 0x060003ED RID: 1005 RVA: 0x0001BFBC File Offset: 0x0001A1BC
	private void OnEnable()
	{
		if (EnviroSkyMgr.instance == null || EnviroSkyMgr.instance.currentEnviroSkyVersion != EnviroSkyMgr.EnviroSkyVersion.LW)
		{
			base.enabled = false;
			return;
		}
		this.myCam = base.GetComponent<Camera>();
		if (this.myCam.actualRenderingPath == RenderingPath.Forward)
		{
			this.myCam.depthTextureMode = DepthTextureMode.Depth;
		}
		this.CreateFogMaterial();
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x0001C017 File Offset: 0x0001A217
	private void OnDisable()
	{
		this.DestroyFogMaterial();
		if (this.myCam != null && this.myCam.actualRenderingPath == RenderingPath.Forward && this.myCam.depthTextureMode == DepthTextureMode.Depth)
		{
			this.myCam.depthTextureMode = DepthTextureMode.None;
		}
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0001C058 File Offset: 0x0001A258
	private void CreateFogMaterial()
	{
		if (this.material != null)
		{
			UnityEngine.Object.DestroyImmediate(this.material);
		}
		if (!EnviroSkyMgr.instance.FogSettings.useSimpleFog)
		{
			Shader shader = Shader.Find("Enviro/Lite/EnviroFogRendering");
			if (shader == null)
			{
				throw new Exception("Critical Error: \"Enviro/EnviroFogRendering\" shader is missing.");
			}
			this.material = new Material(shader);
		}
		else
		{
			Shader shader2 = Shader.Find("Enviro/Lite/EnviroFogRenderingSimple");
			if (shader2 == null)
			{
				throw new Exception("Critical Error: \"Enviro/EnviroFogRendering\" shader is missing.");
			}
			this.material = new Material(shader2);
		}
		if (EnviroSkyMgr.instance.FogSettings.useSimpleFog)
		{
			Shader.EnableKeyword("ENVIRO_SIMPLE_FOG");
			return;
		}
		Shader.DisableKeyword("ENVIRO_SIMPLE_FOG");
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0001C10D File Offset: 0x0001A30D
	private void DestroyFogMaterial()
	{
		if (this.material != null)
		{
			UnityEngine.Object.DestroyImmediate(this.material);
		}
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0001C128 File Offset: 0x0001A328
	private void Update()
	{
		if (EnviroSkyMgr.instance == null)
		{
			return;
		}
		if (!EnviroSkyMgr.instance.IsAvailable())
		{
			return;
		}
		if (this.currentSimpleFog != EnviroSkyMgr.instance.FogSettings.useSimpleFog)
		{
			this.CreateFogMaterial();
			this.currentSimpleFog = EnviroSkyMgr.instance.FogSettings.useSimpleFog;
		}
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0001C184 File Offset: 0x0001A384
	private void OnPreRender()
	{
		if (this.myCam.stereoEnabled)
		{
			Matrix4x4 inverse = this.myCam.GetStereoViewMatrix(Camera.StereoscopicEye.Left).inverse;
			Matrix4x4 inverse2 = this.myCam.GetStereoViewMatrix(Camera.StereoscopicEye.Right).inverse;
			Matrix4x4 stereoProjectionMatrix = this.myCam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
			Matrix4x4 stereoProjectionMatrix2 = this.myCam.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
			Matrix4x4 inverse3 = GL.GetGPUProjectionMatrix(stereoProjectionMatrix, true).inverse;
			Matrix4x4 inverse4 = GL.GetGPUProjectionMatrix(stereoProjectionMatrix2, true).inverse;
			if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLCore && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES3 && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2)
			{
				ref Matrix4x4 ptr = ref inverse3;
				ptr[1, 1] = ptr[1, 1] * -1f;
				ptr = ref inverse4;
				ptr[1, 1] = ptr[1, 1] * -1f;
			}
			Shader.SetGlobalMatrix("_LeftWorldFromView", inverse);
			Shader.SetGlobalMatrix("_RightWorldFromView", inverse2);
			Shader.SetGlobalMatrix("_LeftViewFromScreen", inverse3);
			Shader.SetGlobalMatrix("_RightViewFromScreen", inverse4);
			return;
		}
		Matrix4x4 cameraToWorldMatrix = this.myCam.cameraToWorldMatrix;
		Matrix4x4 inverse5 = GL.GetGPUProjectionMatrix(this.myCam.projectionMatrix, true).inverse;
		if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLCore && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES3 && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2)
		{
			ref Matrix4x4 ptr = ref inverse5;
			ptr[1, 1] = ptr[1, 1] * -1f;
		}
		Shader.SetGlobalMatrix("_LeftWorldFromView", cameraToWorldMatrix);
		Shader.SetGlobalMatrix("_LeftViewFromScreen", inverse5);
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0001C300 File Offset: 0x0001A500
	[ImageEffectOpaque]
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (EnviroSkyLite.instance == null)
		{
			Graphics.Blit(source, destination);
			return;
		}
		if (this.myCam.actualRenderingPath == RenderingPath.Forward)
		{
			this.myCam.depthTextureMode |= DepthTextureMode.Depth;
		}
		if (this.material == null)
		{
			this.CreateFogMaterial();
		}
		if ((!Application.isPlaying && EnviroSkyLite.instance.showFogInEditor) || Application.isPlaying)
		{
			this.RenderFog(source, destination);
			return;
		}
		Graphics.Blit(source, destination);
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0001C384 File Offset: 0x0001A584
	private void RenderFog(RenderTexture source, RenderTexture destination)
	{
		float num = this.myCam.transform.position.y - EnviroSkyLite.instance.fogSettings.height;
		float z = (num <= 0f) ? 1f : 0f;
		FogMode fogMode = RenderSettings.fogMode;
		float fogDensity = RenderSettings.fogDensity;
		float fogStartDistance = RenderSettings.fogStartDistance;
		float fogEndDistance = RenderSettings.fogEndDistance;
		bool flag = fogMode == FogMode.Linear;
		float num2 = flag ? (fogEndDistance - fogStartDistance) : 0f;
		float num3 = (Mathf.Abs(num2) > 0.0001f) ? (1f / num2) : 0f;
		Vector4 value;
		value.x = fogDensity * 1.2011224f;
		value.y = fogDensity * 1.442695f;
		value.z = (flag ? (-num3) : 0f);
		value.w = (flag ? (fogEndDistance * num3) : 0f);
		Shader.SetGlobalVector("_SceneFogParams", value);
		Shader.SetGlobalVector("_SceneFogMode", new Vector4((float)fogMode, (float)(EnviroSkyLite.instance.fogSettings.useRadialDistance ? 1 : 0), 0f, 0f));
		Shader.SetGlobalVector("_HeightParams", new Vector4(EnviroSkyLite.instance.fogSettings.height, num, z, EnviroSkyLite.instance.fogSettings.heightDensity * 0.5f));
		Shader.SetGlobalVector("_DistanceParams", new Vector4(-Mathf.Max(EnviroSkyLite.instance.fogSettings.startDistance, 0f), 0f, 0f, 0f));
		this.material.SetTexture("_MainTex", source);
		Graphics.Blit(source, destination, this.material);
	}

	// Token: 0x04000582 RID: 1410
	[HideInInspector]
	public bool isAddionalCamera;

	// Token: 0x04000583 RID: 1411
	private Camera myCam;

	// Token: 0x04000584 RID: 1412
	[HideInInspector]
	public Material material;

	// Token: 0x04000585 RID: 1413
	[HideInInspector]
	public bool simpleFog;

	// Token: 0x04000586 RID: 1414
	private bool currentSimpleFog;
}
