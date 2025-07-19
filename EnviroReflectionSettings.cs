using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
[Serializable]
public class EnviroReflectionSettings
{
	// Token: 0x0400034D RID: 845
	[Header("Global Reflections Settings")]
	[Tooltip("Enable/disable enviro reflection probe..")]
	public bool globalReflections = true;

	// Token: 0x0400034E RID: 846
	[Header("Global Reflections Custom Rendering")]
	[Tooltip("Enable/disable if enviro reflection probe should render in custom mode to support clouds and other enviro effects.")]
	public bool globalReflectionCustomRendering = true;

	// Token: 0x0400034F RID: 847
	[Tooltip("Enable/disable if enviro reflection probe should render with fog.")]
	public bool globalReflectionUseFog;

	// Token: 0x04000350 RID: 848
	[Tooltip("Set if enviro reflection probe should update faces individual on different frames.")]
	public bool globalReflectionTimeSlicing = true;

	// Token: 0x04000351 RID: 849
	[Header("Global Reflections Updates Settings")]
	[Tooltip("Enable/disable enviro reflection probe updates based on gametime changes..")]
	public bool globalReflectionsUpdateOnGameTime = true;

	// Token: 0x04000352 RID: 850
	[Tooltip("Enable/disable enviro reflection probe updates based on transform position changes..")]
	public bool globalReflectionsUpdateOnPosition = true;

	// Token: 0x04000353 RID: 851
	[Tooltip("Reflection probe intensity.")]
	[Range(0f, 2f)]
	public float globalReflectionsIntensity = 0.5f;

	// Token: 0x04000354 RID: 852
	[Tooltip("Reflection probe update rate based on game time.")]
	public float globalReflectionsTimeTreshold = 0.025f;

	// Token: 0x04000355 RID: 853
	[Tooltip("Reflection probe update rate based on camera position.")]
	public float globalReflectionsPositionTreshold = 0.25f;

	// Token: 0x04000356 RID: 854
	[Tooltip("Reflection probe scale. Increase that one to increase the area where reflection probe will influence your scene.")]
	[Range(0.1f, 10f)]
	public float globalReflectionsScale = 1f;

	// Token: 0x04000357 RID: 855
	[Tooltip("Reflection probe resolution.")]
	public EnviroReflectionSettings.GlobalReflectionResolution globalReflectionResolution = EnviroReflectionSettings.GlobalReflectionResolution.R256;

	// Token: 0x04000358 RID: 856
	[Tooltip("Reflection probe rendered Layers.")]
	public LayerMask globalReflectionLayers;

	// Token: 0x04000359 RID: 857
	[Tooltip("Set the quality of clouds in reflection rendering. Leave empty to use global settings.")]
	public EnviroVolumeCloudsQuality reflectionCloudsQuality;

	// Token: 0x02000081 RID: 129
	public enum GlobalReflectionResolution
	{
		// Token: 0x0400035B RID: 859
		R16,
		// Token: 0x0400035C RID: 860
		R32,
		// Token: 0x0400035D RID: 861
		R64,
		// Token: 0x0400035E RID: 862
		R128,
		// Token: 0x0400035F RID: 863
		R256,
		// Token: 0x04000360 RID: 864
		R512,
		// Token: 0x04000361 RID: 865
		R1024,
		// Token: 0x04000362 RID: 866
		R2048
	}
}
