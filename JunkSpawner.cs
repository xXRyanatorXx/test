using System;
using UnityEngine;

// Token: 0x0200014F RID: 335
public class JunkSpawner : MonoBehaviour
{
	// Token: 0x06000721 RID: 1825 RVA: 0x0003A6D4 File Offset: 0x000388D4
	private void OnEnable()
	{
		if (tools.MPrunning && this.JunkyardNumber > 4)
		{
			return;
		}
		if (this.JunkyardNumber <= 0 || PlayerPrefs.GetFloat("Junk") >= (float)this.JunkyardNumber)
		{
			if (PlayerPrefs.HasKey("DeathWall") && PlayerPrefs.GetFloat("DeathWall") == 1f)
			{
				this.Junk = false;
				this.Used = true;
			}
			this.CarList = GameObject.Find("CarsParent");
			this.Cars = this.CarList.GetComponent<CarList>().Cars;
			int num = (int)(base.transform.position.x - base.transform.root.position.x);
			if (base.transform.root.name == "MapMagic")
			{
				this.Seed = num;
			}
			else if (this.Seed == 0 || !this.Barnfind)
			{
				this.Seed = UnityEngine.Random.Range(0, 999999) + DateTime.Now.Millisecond + num;
			}
			UnityEngine.Random.seed = this.Seed;
			if (this.Junk && this.Crashed)
			{
				if (tools.MPrunning)
				{
					tools.NetworkPLayer.SpawnMoreCars(this.Seed, this.SpawnPoint.transform.position, this.SpawnPoint.transform.parent.position, 1);
					return;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)], new Vector3(UnityEngine.Random.Range(0.1f, 10f), UnityEngine.Random.Range(-99f, -70f), UnityEngine.Random.Range(0.1f, 10f)) + base.transform.root.transform.position, Quaternion.Euler((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360)));
				if (gameObject.GetComponent<MainCarProperties>())
				{
					if (base.transform.root.name == "MapMagic")
					{
						gameObject.GetComponent<MainCarProperties>().Seed = this.Seed;
					}
					gameObject.GetComponent<MainCarProperties>().SpawnPoint = this.SpawnPoint.transform.parent.position;
					gameObject.GetComponent<MainCarProperties>().CreatingJunkyard();
				}
				else
				{
					gameObject.transform.position = this.SpawnPoint.transform.position;
				}
			}
			if (this.Junk && !this.Crashed)
			{
				if (tools.MPrunning)
				{
					tools.NetworkPLayer.SpawnMoreCars(this.Seed, this.SpawnPoint.transform.position, this.SpawnPoint.transform.parent.position, 1);
					return;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)], this.SpawnPoint.transform.parent.position, Quaternion.identity);
				if (gameObject.GetComponent<MainCarProperties>())
				{
					if (base.transform.root.name == "MapMagic")
					{
						gameObject.GetComponent<MainCarProperties>().Seed = this.Seed;
					}
					gameObject.GetComponent<MainCarProperties>().SpawnPoint = this.SpawnPoint.transform.parent.position;
					gameObject.GetComponent<MainCarProperties>().CreatingJunkyard();
				}
				else
				{
					gameObject.transform.position = this.SpawnPoint.transform.position;
				}
			}
			if (this.Used && this.Crashed)
			{
				if (tools.MPrunning)
				{
					tools.NetworkPLayer.SpawnMoreCars(this.Seed, this.SpawnPoint.transform.position, this.SpawnPoint.transform.parent.position, 2);
					return;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)], new Vector3(UnityEngine.Random.Range(0.1f, 10f), UnityEngine.Random.Range(-99f, -70f), UnityEngine.Random.Range(0.1f, 10f)) + base.transform.root.transform.position, Quaternion.Euler((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360)));
				if (gameObject.GetComponent<MainCarProperties>())
				{
					if (base.transform.root.name == "MapMagic")
					{
						gameObject.GetComponent<MainCarProperties>().Seed = this.Seed;
					}
					gameObject.GetComponent<MainCarProperties>().SpawnPoint = this.SpawnPoint.transform.parent.position;
					if (gameObject.GetComponent<MainCarProperties>().Bike)
					{
						gameObject.transform.position = this.SpawnPoint.transform.parent.position;
					}
					gameObject.GetComponent<MainCarProperties>().CreatingUsed();
				}
				else
				{
					gameObject.transform.position = this.SpawnPoint.transform.position;
				}
			}
			if (this.Used && !this.Crashed)
			{
				if (tools.MPrunning)
				{
					tools.NetworkPLayer.SpawnMoreCars(this.Seed, this.SpawnPoint.transform.position, this.SpawnPoint.transform.parent.position, 3);
					return;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)], this.SpawnPoint.transform.parent.position, Quaternion.identity);
				if (gameObject.GetComponent<MainCarProperties>())
				{
					if (base.transform.root.name == "MapMagic")
					{
						gameObject.GetComponent<MainCarProperties>().Seed = this.Seed;
					}
					gameObject.GetComponent<MainCarProperties>().SpawnPoint = this.SpawnPoint.transform.parent.position;
					gameObject.GetComponent<MainCarProperties>().CreatingUsed();
				}
				else
				{
					gameObject.transform.position = this.SpawnPoint.transform.position;
				}
			}
			if (this.Barnfind)
			{
				if (tools.MPrunning)
				{
					tools.NetworkPLayer.SpawnMoreCars(this.Seed, this.SpawnPoint.transform.position, this.SpawnPoint.transform.parent.position, 4);
					return;
				}
				this.Cars = this.CarList.GetComponent<CarList>().BarnCars;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)], this.SpawnPoint.transform.parent.position, this.SpawnPoint.transform.parent.rotation);
				if (gameObject.GetComponent<MainCarProperties>())
				{
					gameObject.GetComponent<MainCarProperties>().Seed = this.Seed;
					gameObject.GetComponent<MainCarProperties>().SpawnPoint = this.SpawnPoint.transform.parent.position;
					gameObject.GetComponent<MainCarProperties>().CreatingBarnFind();
				}
				else
				{
					gameObject.transform.position = this.SpawnPoint.transform.position;
				}
			}
			if (this.RuinedFind)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)], new Vector3(UnityEngine.Random.Range(0.1f, 10f), UnityEngine.Random.Range(-99f, -70f), UnityEngine.Random.Range(0.1f, 10f)) + base.transform.root.transform.position, Quaternion.Euler((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360)));
				if (gameObject.GetComponent<MainCarProperties>())
				{
					if (base.transform.root.name == "MapMagic")
					{
						gameObject.GetComponent<MainCarProperties>().Seed = this.Seed;
					}
					if (this.UseMultipleSpawns)
					{
						gameObject.GetComponent<MainCarProperties>().SpawnPoint = this.MultipleSppawns[UnityEngine.Random.Range(0, this.MultipleSppawns.Length)].transform.position;
					}
					gameObject.GetComponent<MainCarProperties>().CreatingRuinedFind();
					if (gameObject.GetComponent<MainCarProperties>().Bike)
					{
						UnityEngine.Object.Destroy(gameObject);
					}
				}
				else
				{
					gameObject.transform.position = this.SpawnPoint.transform.position;
				}
			}
			this.Seed = 0;
		}
	}

	// Token: 0x04000B23 RID: 2851
	public GameObject CarList;

	// Token: 0x04000B24 RID: 2852
	public GameObject[] Cars;

	// Token: 0x04000B25 RID: 2853
	public GameObject SpawnPoint;

	// Token: 0x04000B26 RID: 2854
	public GameObject[] MultipleSppawns;

	// Token: 0x04000B27 RID: 2855
	public bool UseMultipleSpawns;

	// Token: 0x04000B28 RID: 2856
	public bool Junk;

	// Token: 0x04000B29 RID: 2857
	public bool Used;

	// Token: 0x04000B2A RID: 2858
	public bool Barnfind;

	// Token: 0x04000B2B RID: 2859
	public bool RuinedFind;

	// Token: 0x04000B2C RID: 2860
	public bool New;

	// Token: 0x04000B2D RID: 2861
	public bool Crashed;

	// Token: 0x04000B2E RID: 2862
	public int Seed;

	// Token: 0x04000B2F RID: 2863
	public int JunkyardNumber;
}
