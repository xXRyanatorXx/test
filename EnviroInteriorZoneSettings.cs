using System;
using UnityEngine;

// Token: 0x0200006D RID: 109
[Serializable]
public class EnviroInteriorZoneSettings
{
	// Token: 0x0400025F RID: 607
	[HideInInspector]
	public Color currentInteriorDirectLightMod;

	// Token: 0x04000260 RID: 608
	[HideInInspector]
	public Color currentInteriorAmbientLightMod;

	// Token: 0x04000261 RID: 609
	[HideInInspector]
	public Color currentInteriorAmbientEQLightMod;

	// Token: 0x04000262 RID: 610
	[HideInInspector]
	public Color currentInteriorAmbientGRLightMod;

	// Token: 0x04000263 RID: 611
	[HideInInspector]
	public Color currentInteriorSkyboxMod;

	// Token: 0x04000264 RID: 612
	[HideInInspector]
	public Color currentInteriorFogColorMod = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04000265 RID: 613
	[HideInInspector]
	public float currentInteriorFogMod = 1f;

	// Token: 0x04000266 RID: 614
	[HideInInspector]
	public float currentInteriorWeatherEffectMod = 1f;

	// Token: 0x04000267 RID: 615
	[HideInInspector]
	public float currentInteriorZoneAudioVolume = 1f;

	// Token: 0x04000268 RID: 616
	[HideInInspector]
	public float currentInteriorZoneAudioFadingSpeed = 1f;
}
