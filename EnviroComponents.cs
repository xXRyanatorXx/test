using System;
using UnityEngine;

// Token: 0x0200006A RID: 106
[Serializable]
public class EnviroComponents
{
	// Token: 0x04000240 RID: 576
	[Tooltip("The Enviro sun object.")]
	public GameObject Sun;

	// Token: 0x04000241 RID: 577
	[Tooltip("The Enviro moon object.")]
	public GameObject Moon;

	// Token: 0x04000242 RID: 578
	[Tooltip("The directional light for directional sun lighting when using dual mode. Used for sun and moon in single mode.")]
	public Transform DirectLight;

	// Token: 0x04000243 RID: 579
	[Tooltip("The directional light for directional moon lighting when using the dual mode.")]
	public Transform AdditionalDirectLight;

	// Token: 0x04000244 RID: 580
	[Tooltip("The Enviro global reflection probe for dynamic reflections.")]
	public EnviroReflectionProbe GlobalReflectionProbe;

	// Token: 0x04000245 RID: 581
	[Tooltip("Your WindZone that reflect our weather wind settings.")]
	public WindZone windZone;

	// Token: 0x04000246 RID: 582
	[Tooltip("The Enviro Lighting Flash Component.")]
	public EnviroLightning LightningGenerator;

	// Token: 0x04000247 RID: 583
	[Tooltip("Link to the object that hold all additional satellites as childs.")]
	public Transform satellites;

	// Token: 0x04000248 RID: 584
	[Tooltip("Just a transform for stars rotation calculations. ")]
	public Transform starsRotation;

	// Token: 0x04000249 RID: 585
	[Tooltip("Plane to cast cloud shadows.")]
	public GameObject particleClouds;
}
