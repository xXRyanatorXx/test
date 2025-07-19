using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class EnviroWindSynchronize : MonoBehaviour
{
	// Token: 0x060003C0 RID: 960 RVA: 0x00019987 File Offset: 0x00017B87
	private void Start()
	{
		if (this.syncTerrainGrassWind && this.terrains.Count > 0)
		{
			Debug.Log("Please assign Terrain, or deactivate 'syncTerrainGrassWind'!");
			base.enabled = false;
		}
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x000199B0 File Offset: 0x00017BB0
	private void Update()
	{
		if (EnviroSkyMgr.instance == null)
		{
			return;
		}
		if (this.syncTerrainGrassWind)
		{
			for (int i = 0; i < this.terrains.Count; i++)
			{
				this.terrains[i].terrainData.wavingGrassStrength = Mathf.Lerp(this.terrains[i].terrainData.wavingGrassStrength, EnviroSkyMgr.instance.Components.windZone.windMain, Time.deltaTime * this.windChangingSpeed);
			}
		}
	}

	// Token: 0x04000572 RID: 1394
	[Header("Terrain Grass")]
	public bool syncTerrainGrassWind = true;

	// Token: 0x04000573 RID: 1395
	public List<Terrain> terrains = new List<Terrain>();

	// Token: 0x04000574 RID: 1396
	[Header("Speed")]
	[Range(0f, 10f)]
	public float windChangingSpeed = 1f;
}
