using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class InfiniteTerrain : MonoBehaviour
{
	// Token: 0x060006E4 RID: 1764 RVA: 0x00036B7C File Offset: 0x00034D7C
	private void Start()
	{
		bool flag = true;
		this._original = base.gameObject.GetComponent<Terrain>();
		if (this._original == null)
		{
			flag = false;
			Debug.LogError("InfiniteTerrain: This script can only be linked to a Game Object of type Terrain.");
		}
		else if (this._original.name.EndsWith("(Clone)"))
		{
			this._original = null;
		}
		this._playerObject = this.PlayerObject;
		if (this._playerObject == null)
		{
			Debug.LogError("InfiniteTerrain: PlayerObject cannot be null.");
		}
		if (flag)
		{
			this.EnforceGridSizeRules();
			this.InitialiseGrid();
		}
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x00036C0C File Offset: 0x00034E0C
	private void Update()
	{
		if (this._initialised)
		{
			this.EnforceGridSizeRules();
			if (!this._gridWidth.Equals(this.GridWidth) || !this._gridHeight.Equals(this.GridHeight) || !this._cloneChildren.Equals(this.CloneChildren))
			{
				this._initialised = false;
				this.InitialiseGrid();
			}
			if (this.PlayerObject != this._playerObject)
			{
				this.PlayerObject = this._playerObject;
				Debug.LogWarning("InfiniteTerrain: PlayerObject cannot be changed at runtime.");
			}
			Vector3 vector = new Vector3(this.PlayerObject.transform.position.x, this.PlayerObject.transform.position.y, this.PlayerObject.transform.position.z);
			Terrain x = null;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this._gridWidth; i++)
			{
				for (int j = 0; j < this._gridHeight; j++)
				{
					if (vector.x >= this._grid[i, j].transform.position.x && vector.x <= this._grid[i, j].transform.position.x + this._grid[i, j].terrainData.size.x && vector.z >= this._grid[i, j].transform.position.z && vector.z <= this._grid[i, j].transform.position.z + this._grid[i, j].terrainData.size.z)
					{
						x = this._grid[i, j];
						num = (this._gridWidth - 1) / 2 - i;
						num2 = (this._gridHeight - 1) / 2 - j;
						break;
					}
				}
				if (x != null)
				{
					break;
				}
			}
			if (x != this._grid[(this._gridWidth - 1) / 2, (this._gridHeight - 1) / 2])
			{
				Terrain[,] array = new Terrain[this._gridWidth, this._gridHeight];
				for (int k = 0; k < this._gridWidth; k++)
				{
					for (int l = 0; l < this._gridHeight; l++)
					{
						int num3 = k + num;
						if (num3 < 0)
						{
							num3 = this._gridWidth - 1;
						}
						else if (num3 > this._gridWidth - 1)
						{
							num3 = 0;
						}
						int num4 = l + num2;
						if (num4 < 0)
						{
							num4 = this._gridHeight - 1;
						}
						else if (num4 > this._gridHeight - 1)
						{
							num4 = 0;
						}
						array[num3, num4] = this._grid[k, l];
					}
				}
				this._grid = array;
				this.UpdatePositions();
			}
		}
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00036F10 File Offset: 0x00035110
	private void EnforceGridSizeRules()
	{
		if (this.GridWidth % 2 == 0)
		{
			this.GridWidth--;
		}
		if (this.GridHeight % 2 == 0)
		{
			this.GridHeight--;
		}
		if (this.GridWidth < 3)
		{
			this.GridWidth = 3;
		}
		if (this.GridHeight < 3)
		{
			this.GridHeight = 3;
		}
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x00036F70 File Offset: 0x00035170
	private void InitialiseGrid()
	{
		if (this._original != null)
		{
			if (this._grid != null)
			{
				Terrain terrain = this._grid[(this._gridWidth - 1) / 2, (this._gridHeight - 1) / 2];
				this._original.transform.position = new Vector3(terrain.transform.position.x, terrain.transform.position.y, terrain.transform.position.z);
				for (int i = 0; i < this._gridWidth; i++)
				{
					for (int j = 0; j < this._gridHeight; j++)
					{
						if (this._grid[i, j] != this._original)
						{
							UnityEngine.Object.Destroy(this._grid[i, j].gameObject);
						}
					}
				}
			}
			this._gridWidth = this.GridWidth;
			this._gridHeight = this.GridHeight;
			this._cloneChildren = this.CloneChildren;
			this._grid = new Terrain[this._gridWidth, this._gridHeight];
			for (int k = 0; k < this._gridWidth; k++)
			{
				for (int l = 0; l < this._gridHeight; l++)
				{
					if (k.Equals((this._gridWidth - 1) / 2) && l.Equals((this._gridHeight - 1) / 2))
					{
						this._grid[k, l] = this._original;
					}
					else
					{
						this.index = UnityEngine.Random.Range(0, this.Terrains.Length);
						this.SelectedTerrain = this.Terrains[this.index];
						this._grid[k, l] = this.Clone(this.SelectedTerrain);
					}
				}
			}
			this.UpdatePositions();
			this._initialised = true;
		}
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x00037148 File Offset: 0x00035348
	private Terrain Clone(Terrain original)
	{
		if (this.CloneChildren)
		{
			return UnityEngine.Object.Instantiate<Terrain>(original);
		}
		return Terrain.CreateTerrainGameObject(original.terrainData).GetComponent<Terrain>();
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x0003716C File Offset: 0x0003536C
	private void UpdatePositions()
	{
		Terrain terrain = this._grid[(this._gridWidth - 1) / 2, (this._gridHeight - 1) / 2];
		for (int i = 0; i < this._gridWidth; i++)
		{
			for (int j = 0; j < this._gridHeight; j++)
			{
				if (!i.Equals((this._gridWidth - 1) / 2) || !j.Equals((this._gridHeight - 1) / 2))
				{
					int num = (this._gridWidth - 1) / 2 - i;
					int num2 = (this._gridHeight - 1) / 2 - j;
					this._grid[i, j].transform.position = new Vector3(terrain.transform.position.x - terrain.terrainData.size.x * (float)num, terrain.transform.position.y, terrain.transform.position.z + terrain.terrainData.size.z * (float)num2);
				}
			}
		}
		for (int k = 0; k < this._gridWidth; k++)
		{
			for (int l = 0; l < this._gridHeight; l++)
			{
				Terrain left = (k == 0) ? null : this._grid[k - 1, l];
				Terrain top = (l == 0) ? null : this._grid[k, l - 1];
				Terrain right = (k == this._gridWidth - 1) ? null : this._grid[k + 1, l];
				Terrain bottom = (l == this._gridHeight - 1) ? null : this._grid[k, l + 1];
				this._grid[k, l].SetNeighbors(left, top, right, bottom);
			}
		}
	}

	// Token: 0x04000A5A RID: 2650
	public Terrain[] Terrains;

	// Token: 0x04000A5B RID: 2651
	public Terrain Terrain1;

	// Token: 0x04000A5C RID: 2652
	public Terrain Terrain2;

	// Token: 0x04000A5D RID: 2653
	public Terrain Terrain3;

	// Token: 0x04000A5E RID: 2654
	public Terrain Terrain4;

	// Token: 0x04000A5F RID: 2655
	public Terrain Terrain5;

	// Token: 0x04000A60 RID: 2656
	public Terrain Terrain6;

	// Token: 0x04000A61 RID: 2657
	public Terrain Terrain7;

	// Token: 0x04000A62 RID: 2658
	public Terrain Terrain8;

	// Token: 0x04000A63 RID: 2659
	public Terrain Terrain9;

	// Token: 0x04000A64 RID: 2660
	public Terrain Terrain0;

	// Token: 0x04000A65 RID: 2661
	private int index;

	// Token: 0x04000A66 RID: 2662
	private int seed;

	// Token: 0x04000A67 RID: 2663
	public int GridWidth = 3;

	// Token: 0x04000A68 RID: 2664
	public int GridHeight = 3;

	// Token: 0x04000A69 RID: 2665
	public bool CloneChildren = true;

	// Token: 0x04000A6A RID: 2666
	public GameObject PlayerObject;

	// Token: 0x04000A6B RID: 2667
	private Terrain SelectedTerrain;

	// Token: 0x04000A6C RID: 2668
	private Terrain _original;

	// Token: 0x04000A6D RID: 2669
	private int _gridWidth = 3;

	// Token: 0x04000A6E RID: 2670
	private int _gridHeight = 3;

	// Token: 0x04000A6F RID: 2671
	private bool _cloneChildren = true;

	// Token: 0x04000A70 RID: 2672
	private Terrain[,] _grid;

	// Token: 0x04000A71 RID: 2673
	private bool _initialised;

	// Token: 0x04000A72 RID: 2674
	private GameObject _playerObject;
}
