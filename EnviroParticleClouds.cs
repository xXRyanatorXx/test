using System;
using UnityEngine;

// Token: 0x0200008C RID: 140
[Serializable]
public class EnviroParticleClouds
{
	// Token: 0x040003EA RID: 1002
	[Tooltip("Particle clouds height.")]
	[Range(0.01f, 0.2f)]
	public float height = 0.1f;

	// Token: 0x040003EB RID: 1003
	[Tooltip("Global Color for flat clouds based sun positon.")]
	[GradientUsage(true)]
	public Gradient particleCloudsColor;
}
