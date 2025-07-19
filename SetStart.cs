using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200026C RID: 620
public class SetStart : MonoBehaviour
{
	// Token: 0x06000EB4 RID: 3764 RVA: 0x0009BD9C File Offset: 0x00099F9C
	private void Start()
	{
		if (this.barncarspawner)
		{
			if (this.spawned)
			{
				return;
			}
			this.DeliveryBook = GameObject.Find("DelieveriesPizzas").GetComponent<Book>();
			this.Target = UnityEngine.Object.Instantiate<GameObject>(this.MaprkerPrefab, Vector3.zero, Quaternion.identity);
			this.Target.transform.SetParent(this.DeliveryBook.AcceptedPizzas.transform);
			this.Target.transform.localScale = Vector3.one;
			this.Target.GetComponent<RectTransform>().anchoredPosition = new Vector2((base.transform.position.x - this.Target.transform.root.transform.position.x) / 3f, (base.transform.position.z - this.Target.transform.root.transform.position.z) / 3f);
			this.Target.GetComponent<UpdateMapPosition>().SCALE();
		}
		this.tries = 200;
		this.found = false;
		int seed = (int)(base.transform.position.x - base.transform.root.position.x);
		this.Seed = seed;
		UnityEngine.Random.seed = this.Seed;
		if (this.Buildings.Length != 0)
		{
			int num = UnityEngine.Random.Range(0, this.Buildings.Length);
			this.Buildings[num].SetActive(true);
		}
		this.Restart();
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x0009BF30 File Offset: 0x0009A130
	public void Restart()
	{
		if (this.POI || this.POI2)
		{
			this.t = base.transform.parent.parent.Find("Main Terrain").GetComponent<Terrain>();
			this.WaitAttach();
		}
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x0009BF70 File Offset: 0x0009A170
	public void RestartPlayer()
	{
		foreach (Transform transform in UnityEngine.Object.FindObjectsOfType<Transform>())
		{
			if (transform.name == "PlayerSpawn" && !this.found)
			{
				base.transform.position = transform.position;
				this.found = true;
				base.GetComponent<tools>().StartPosition = base.transform.position;
				this.CarList = GameObject.Find("CarsParent");
				this.Cars = this.CarList.GetComponent<CarList>().Cars;
				this.StartCar = this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)];
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.StartCar, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), Quaternion.identity);
				gameObject.GetComponent<MainCarProperties>().SpawnPoint = gameObject.transform.position;
				if (PlayerPrefs.HasKey("DeathWall") && PlayerPrefs.GetFloat("DeathWall") == 1f)
				{
					gameObject.GetComponent<MainCarProperties>().CreatingStock(0);
				}
				else
				{
					gameObject.GetComponent<MainCarProperties>().CreatingStartOldCar();
				}
				for (int j = 0; j < this.StartObjets.Length; j++)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.StartObjets[j], base.transform.position, Quaternion.identity).transform.name = this.StartObjets[j].transform.name;
				}
			}
		}
		if (!this.found)
		{
			Vector3 position = base.transform.position;
			Terrain[] activeTerrains = Terrain.activeTerrains;
			Terrain terrain = Terrain.activeTerrain;
			float num = (new Vector3(activeTerrains[0].transform.position.x + activeTerrains[0].terrainData.size.x / 2f, base.transform.position.y, activeTerrains[0].transform.position.z + activeTerrains[0].terrainData.size.z / 2f) - base.transform.position).sqrMagnitude;
			for (int k = 0; k < activeTerrains.Length; k++)
			{
				float sqrMagnitude = (new Vector3(activeTerrains[k].transform.position.x + activeTerrains[k].terrainData.size.x / 2f, base.transform.position.y, activeTerrains[k].transform.position.z + activeTerrains[k].terrainData.size.z / 2f) - base.transform.position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					terrain = activeTerrains[k];
				}
			}
			if (terrain)
			{
				base.transform.position = new Vector3(base.transform.position.x, terrain.SampleHeight(base.transform.position) + 1f, base.transform.position.z);
				this.CarList = GameObject.Find("CarsParent");
				this.Cars = this.CarList.GetComponent<CarList>().Cars;
				this.StartCar = this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)];
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.StartCar, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), Quaternion.identity);
				gameObject2.GetComponent<MainCarProperties>().SpawnPoint = gameObject2.transform.position;
				if (PlayerPrefs.HasKey("DeathWall") && PlayerPrefs.GetFloat("DeathWall") == 1f)
				{
					gameObject2.GetComponent<MainCarProperties>().CreatingStock(0);
				}
				else
				{
					gameObject2.GetComponent<MainCarProperties>().CreatingStartOldCar();
				}
				for (int l = 0; l < this.StartObjets.Length; l++)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.StartObjets[l], base.transform.position, Quaternion.identity).transform.name = this.StartObjets[l].transform.name;
				}
			}
		}
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x0009C40C File Offset: 0x0009A60C
	private void WaitAttach()
	{
		if (this.POI)
		{
			this.tries--;
			this.GetTerrainTexture();
			if ((this.RoadValue < 0.1f || (this.DirectionTransform2 && this.RoadValue2 < 0.1f)) && this.tries > 0)
			{
				base.transform.rotation = Quaternion.Euler(0f, base.transform.localEulerAngles.y - 3f, 0f);
				this.WaitAttach();
			}
		}
		if (this.POI2)
		{
			this.tries--;
			this.GetTerrainTexture();
			if ((this.RoadSmallValue != 1f || (this.DirectionTransform2 && this.RoadSmallValue <= 0f)) && this.tries > 0)
			{
				base.transform.rotation = Quaternion.Euler(0f, base.transform.localEulerAngles.y - 3f, 0f);
				this.WaitAttach();
			}
		}
		if (this.CAM != null)
		{
			this.CAM.transform.rotation = base.transform.root.rotation;
		}
		if (this.Name != null)
		{
			this.Name.transform.rotation = base.transform.root.rotation;
		}
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x0009C57C File Offset: 0x0009A77C
	public void AddMark()
	{
		if (this.spawned)
		{
			return;
		}
		this.found = false;
		foreach (Collider collider in Physics.OverlapSphere(base.transform.position, 2f))
		{
			if (collider.gameObject.name == "RoadDirObj")
			{
				this.found = true;
				collider.transform.SetParent(base.transform);
			}
		}
		if (!this.found)
		{
			this.spawned = true;
			UnityEngine.Object.Destroy(this.Target);
			if (this.PartSpawner)
			{
				this.PartSpawner.SetActive(true);
				base.StartCoroutine(this.DisableSpawner());
			}
			if (this.RandomCarSpawner && UnityEngine.Random.Range(1, 10) > 5)
			{
				this.RandomCarSpawner.SetActive(true);
			}
			if (!this.barncarspawner)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.MarkObject, base.transform.position, Quaternion.identity);
				gameObject.transform.name = "RoadDirObj";
				gameObject.transform.SetParent(base.transform);
			}
		}
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x0009C69F File Offset: 0x0009A89F
	private IEnumerator DisableSpawner()
	{
		yield return new WaitForSeconds(1f);
		this.PartSpawner.SetActive(false);
		yield break;
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x0009C6AE File Offset: 0x0009A8AE
	private IEnumerator WaitLoadFinish()
	{
		yield return 0;
		yield return 0;
		yield return 0;
		if (tools.GameLoaded)
		{
			this.AddMark();
		}
		else
		{
			base.StartCoroutine(this.WaitLoadFinish());
		}
		yield break;
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x0009C6BD File Offset: 0x0009A8BD
	private IEnumerator StartPlayer()
	{
		yield return 0;
		foreach (Transform transform in UnityEngine.Object.FindObjectsOfType<Transform>())
		{
			if (transform.name == "PlayerSpawn" && !this.found)
			{
				base.transform.position = transform.position;
				this.found = true;
				if (PlayerPrefs.HasKey("DeathWall") && PlayerPrefs.GetFloat("DeathWall") == 1f)
				{
					this.CarList = GameObject.Find("CarsParent");
					this.Cars = this.CarList.GetComponent<CarList>().Cars;
					this.StartCar = this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)];
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.StartCar, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), Quaternion.identity);
				gameObject.GetComponent<MainCarProperties>().SpawnPoint = gameObject.transform.position;
				if (PlayerPrefs.HasKey("DeathWall") && PlayerPrefs.GetFloat("DeathWall") == 1f)
				{
					gameObject.GetComponent<MainCarProperties>().CreatingStock(0);
				}
				else
				{
					gameObject.GetComponent<MainCarProperties>().CreatingStartOldCar();
				}
				for (int j = 0; j < this.StartObjets.Length; j++)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.StartObjets[j], base.transform.position, Quaternion.identity).transform.name = this.StartObjets[j].transform.name;
				}
			}
		}
		if (!this.found)
		{
			base.StartCoroutine(this.StartPlayer());
		}
		yield break;
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x0009C6CC File Offset: 0x0009A8CC
	public void GO()
	{
		if (tools.GameLoaded)
		{
			this.AddMark();
			return;
		}
		base.StartCoroutine(this.WaitLoadFinish());
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x0009C6E9 File Offset: 0x0009A8E9
	public void GetTerrainTexture()
	{
		this.ConvertPosition();
		this.CheckTexture();
	}

	// Token: 0x06000EBE RID: 3774 RVA: 0x0009C6F8 File Offset: 0x0009A8F8
	private void ConvertPosition()
	{
		Vector3 vector = this.DirectionTransform.position - this.t.transform.position;
		Vector3 vector2 = new Vector3(vector.x / this.t.terrainData.size.x, 0f, vector.z / this.t.terrainData.size.z);
		float num = vector2.x * (float)this.t.terrainData.alphamapWidth;
		float num2 = vector2.z * (float)this.t.terrainData.alphamapHeight;
		this.posX = (int)num;
		this.posZ = (int)num2;
		if (this.DirectionTransform2)
		{
			Vector3 vector3 = this.DirectionTransform2.position - this.t.transform.position;
			Vector3 vector4 = new Vector3(vector3.x / this.t.terrainData.size.x, 0f, vector3.z / this.t.terrainData.size.z);
			float num3 = vector4.x * (float)this.t.terrainData.alphamapWidth;
			float num4 = vector4.z * (float)this.t.terrainData.alphamapHeight;
			this.posX2 = (int)num3;
			this.posZ2 = (int)num4;
		}
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x0009C864 File Offset: 0x0009AA64
	private void CheckTexture()
	{
		float[,,] alphamaps = this.t.terrainData.GetAlphamaps(this.posX, this.posZ, 1, 1);
		this.RoadValue = alphamaps[0, 0, 8];
		this.RoadSmallValue = alphamaps[0, 0, 9];
		if (this.DirectionTransform2)
		{
			float[,,] alphamaps2 = this.t.terrainData.GetAlphamaps(this.posX2, this.posZ2, 1, 1);
			this.RoadValue2 = alphamaps2[0, 0, 8];
		}
	}

	// Token: 0x040017EE RID: 6126
	public GameObject CAM;

	// Token: 0x040017EF RID: 6127
	public GameObject Name;

	// Token: 0x040017F0 RID: 6128
	public bool Player;

	// Token: 0x040017F1 RID: 6129
	public bool POI;

	// Token: 0x040017F2 RID: 6130
	public bool POI2;

	// Token: 0x040017F3 RID: 6131
	public GameObject StartCar;

	// Token: 0x040017F4 RID: 6132
	public GameObject SpawnPoint;

	// Token: 0x040017F5 RID: 6133
	public bool found;

	// Token: 0x040017F6 RID: 6134
	public GameObject PartSpawner;

	// Token: 0x040017F7 RID: 6135
	public GameObject RandomCarSpawner;

	// Token: 0x040017F8 RID: 6136
	public GameObject CarList;

	// Token: 0x040017F9 RID: 6137
	public GameObject[] Cars;

	// Token: 0x040017FA RID: 6138
	public GameObject MarkObject;

	// Token: 0x040017FB RID: 6139
	public bool spawned;

	// Token: 0x040017FC RID: 6140
	public Transform DirectionTransform;

	// Token: 0x040017FD RID: 6141
	public Transform DirectionTransform2;

	// Token: 0x040017FE RID: 6142
	public Terrain t;

	// Token: 0x040017FF RID: 6143
	public int posX;

	// Token: 0x04001800 RID: 6144
	public int posZ;

	// Token: 0x04001801 RID: 6145
	public int posX2;

	// Token: 0x04001802 RID: 6146
	public int posZ2;

	// Token: 0x04001803 RID: 6147
	public float RoadValue;

	// Token: 0x04001804 RID: 6148
	public float RoadValue2;

	// Token: 0x04001805 RID: 6149
	public float RoadSmallValue;

	// Token: 0x04001806 RID: 6150
	public int tries;

	// Token: 0x04001807 RID: 6151
	public int Seed;

	// Token: 0x04001808 RID: 6152
	public GameObject[] StartObjets;

	// Token: 0x04001809 RID: 6153
	public GameObject[] Buildings;

	// Token: 0x0400180A RID: 6154
	public JunkSpawner barncarspawner;

	// Token: 0x0400180B RID: 6155
	public Book DeliveryBook;

	// Token: 0x0400180C RID: 6156
	public GameObject Target;

	// Token: 0x0400180D RID: 6157
	public GameObject MaprkerPrefab;
}
