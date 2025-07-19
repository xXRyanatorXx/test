using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007F RID: 127
[Serializable]
public class EnviroSatellitesSettings
{
	// Token: 0x0400034C RID: 844
	[Tooltip("List of satellites.")]
	public List<EnviroSatellite> additionalSatellites = new List<EnviroSatellite>();
}
