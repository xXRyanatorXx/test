using System;
using MapMagic.Core;
using MapMagic.Products;
using MapMagic.Terrains;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000143 RID: 323
public class HudMM : MonoBehaviour
{
	// Token: 0x060006DB RID: 1755 RVA: 0x0003680C File Offset: 0x00034A0C
	private void Start()
	{
		this.isEditor = false;
		this.isGameStarted = false;
		Debug.Log("Starting");
		this.startPlane.transform.position = this.player.transform.position - new Vector3(0f, 10f, 0f);
		TerrainTile.OnTileApplied = (Action<TerrainTile, TileData, StopToken>)Delegate.Combine(TerrainTile.OnTileApplied, new Action<TerrainTile, TileData, StopToken>(this.OnTileApplied));
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0003688A File Offset: 0x00034A8A
	public void OnTileApplied(TerrainTile tile, TileData data, StopToken stopToken)
	{
		Debug.Log("On Tile applied");
		if (tile.ActiveTerrain == null)
		{
			Debug.Log("Why terrain is null?");
			return;
		}
		HudMM.GetTerrainAltitude(this.player.position);
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x000368C0 File Offset: 0x00034AC0
	private void StartGame(float alt)
	{
		if (alt < 0.1f)
		{
			return;
		}
		base.GetComponent<Text>().text = "Game started";
		this.startPlane.SetActive(false);
		this.player.transform.position = new Vector3(this.player.transform.position.x, alt + 5f, this.player.transform.position.z);
		this.isGameStarted = true;
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x00036940 File Offset: 0x00034B40
	private void Update()
	{
		if (Time.time >= this.nextTime)
		{
			float terrainAltitude = HudMM.GetTerrainAltitude(this.player.position);
			if (this.mmObject.IsGenerating())
			{
				base.GetComponent<Text>().text = "Generating: " + (this.mmObject.GetProgress() * 100f).ToString("#.") + " %";
			}
			else if (!this.isGameStarted)
			{
				this.StartGame(terrainAltitude);
			}
			this.nextTime += this.interval;
		}
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x000369D4 File Offset: 0x00034BD4
	private static void OnGeneratedTerrain(Vector3 playerPos)
	{
		if (Terrain.activeTerrains.Length == 0)
		{
			return;
		}
		HudMM.GetClosestCurrentTerrainInt(playerPos);
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x000369E8 File Offset: 0x00034BE8
	private static Terrain GetClosestCurrentTerrain(Vector3 playerPos)
	{
		Terrain[] activeTerrains = Terrain.activeTerrains;
		if (activeTerrains == null)
		{
			return null;
		}
		int closestCurrentTerrainInt = HudMM.GetClosestCurrentTerrainInt(playerPos);
		Terrain terrain = activeTerrains[closestCurrentTerrainInt];
		if (terrain == null)
		{
			return null;
		}
		return terrain;
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x00036A18 File Offset: 0x00034C18
	private static float GetTerrainAltitude(Vector3 playerPos)
	{
		Terrain[] activeTerrains = Terrain.activeTerrains;
		if (activeTerrains.Length == 0)
		{
			return 0f;
		}
		int closestCurrentTerrainInt = HudMM.GetClosestCurrentTerrainInt(playerPos);
		return activeTerrains[closestCurrentTerrainInt].SampleHeight(playerPos);
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x00036A48 File Offset: 0x00034C48
	private static int GetClosestCurrentTerrainInt(Vector3 playerPos)
	{
		Terrain[] activeTerrains = Terrain.activeTerrains;
		float num = (new Vector3(activeTerrains[0].transform.position.x + activeTerrains[0].terrainData.size.x / 2f, playerPos.y, activeTerrains[0].transform.position.z + activeTerrains[0].terrainData.size.z / 2f) - playerPos).sqrMagnitude;
		int result = 0;
		for (int i = 0; i < activeTerrains.Length; i++)
		{
			float sqrMagnitude = (new Vector3(activeTerrains[i].transform.position.x + activeTerrains[i].terrainData.size.x / 2f, playerPos.y, activeTerrains[i].transform.position.z + activeTerrains[i].terrainData.size.z / 2f) - playerPos).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				result = i;
			}
		}
		return result;
	}

	// Token: 0x04000A53 RID: 2643
	private float interval = 1f;

	// Token: 0x04000A54 RID: 2644
	private float nextTime;

	// Token: 0x04000A55 RID: 2645
	public bool isGameStarted;

	// Token: 0x04000A56 RID: 2646
	private bool isEditor;

	// Token: 0x04000A57 RID: 2647
	public Transform player;

	// Token: 0x04000A58 RID: 2648
	public GameObject startPlane;

	// Token: 0x04000A59 RID: 2649
	public MapMagicObject mmObject;
}
