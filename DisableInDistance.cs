using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000036 RID: 54
public class DisableInDistance : MonoBehaviour
{
	// Token: 0x060000FC RID: 252 RVA: 0x00009A30 File Offset: 0x00007C30
	private void Start()
	{
		this.listOfTransforms = new List<Transform>();
		this.listOfTransforms2 = new List<Transform>();
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00009A48 File Offset: 0x00007C48
	private void Awake()
	{
		this.Player = GameObject.Find("Player");
		if (PlayerPrefs.HasKey("MapDist"))
		{
			this.Dist = PlayerPrefs.GetFloat("MapDist");
		}
		this.SetBasemap500();
		this.Frame = -6;
		if (base.transform.name != "UnloadablesMain" && base.transform.name != "MapMagic")
		{
			GameObject.Find("UnloadablesMain").GetComponent<DisableInDistance>().Disabler2 = this;
		}
		this.SetTerrain1();
		base.StartCoroutine(this.Prefabupdate());
		base.StartCoroutine(this.Terrainupdate());
		base.StartCoroutine(this.Transformupdate());
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00009AFF File Offset: 0x00007CFF
	private IEnumerator Prefabupdate()
	{
		yield return 0;
		int num;
		for (int i = 0; i < this.Objects1.Length; i = num + 1)
		{
			GameObject gameObject = this.Objects1[i];
			if (Vector3.Distance(gameObject.transform.position, this.Player.transform.position) < 600f)
			{
				if (!gameObject.active)
				{
					gameObject.SetActive(true);
				}
			}
			else if (gameObject.active)
			{
				gameObject.SetActive(false);
			}
			yield return 0;
			num = i;
		}
		base.StartCoroutine(this.Prefabupdate());
		yield break;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00009B0E File Offset: 0x00007D0E
	private IEnumerator Transformupdate()
	{
		this.listOfTransforms2 = new List<Transform>();
		foreach (Transform item in this.listOfTransforms)
		{
			this.listOfTransforms2.Add(item);
		}
		yield return 0;
		foreach (Transform transform in this.listOfTransforms2)
		{
			if (Vector3.Distance(transform.position, this.Player.transform.position) < 2500f)
			{
				if (!transform.gameObject.active)
				{
					transform.gameObject.SetActive(true);
				}
			}
			else if (transform.gameObject.active)
			{
				transform.gameObject.SetActive(false);
			}
			yield return 0;
		}
		List<Transform>.Enumerator enumerator2 = default(List<Transform>.Enumerator);
		base.StartCoroutine(this.Transformupdate());
		yield break;
		yield break;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00009B1D File Offset: 0x00007D1D
	private IEnumerator Terrainupdate()
	{
		yield return 0;
		int num;
		for (int i = 0; i < this.Terrains.Length; i = num + 1)
		{
			GameObject gameObject = this.Terrains[i];
			if (Vector3.Distance(new Vector3(gameObject.transform.position.x + 500f, gameObject.transform.position.y, gameObject.transform.position.z + 500f), this.Player.transform.position) < this.Dist)
			{
				if (!gameObject.active)
				{
					gameObject.SetActive(true);
				}
			}
			else if (gameObject.active)
			{
				gameObject.SetActive(false);
			}
			yield return 0;
			num = i;
		}
		base.StartCoroutine(this.Terrainupdate());
		yield break;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00009B2C File Offset: 0x00007D2C
	private void SetTerrain1()
	{
		for (int i = 0; i < this.Terrains.Length; i++)
		{
			GameObject gameObject = this.Terrains[i];
			if (Vector3.Distance(new Vector3(gameObject.transform.position.x + 500f, gameObject.transform.position.y, gameObject.transform.position.z + 500f), this.Player.transform.position) < this.Dist)
			{
				if (!gameObject.active)
				{
					gameObject.SetActive(true);
				}
			}
			else if (gameObject.active)
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00009BDC File Offset: 0x00007DDC
	public void SetDist()
	{
		this.Dist = this.DistSlider.value;
		this.DistText.text = this.Dist.ToString() + "m";
		PlayerPrefs.SetFloat("MapDist", this.Dist);
		PlayerPrefs.Save();
		if (this.Disabler2)
		{
			this.Disabler2.Dist = this.Dist;
		}
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00009C50 File Offset: 0x00007E50
	public void SetBasemap200()
	{
		for (int i = 0; i < this.Terrains.Length; i++)
		{
			this.Terrains[i].GetComponent<Terrain>().basemapDistance = 200f;
		}
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00009C88 File Offset: 0x00007E88
	public void SetBasemap500()
	{
		for (int i = 0; i < this.Terrains.Length; i++)
		{
			this.Terrains[i].GetComponent<Terrain>().basemapDistance = 500f;
		}
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00009CC0 File Offset: 0x00007EC0
	public void SetBasemap1000()
	{
		for (int i = 0; i < this.Terrains.Length; i++)
		{
			this.Terrains[i].GetComponent<Terrain>().basemapDistance = 1000f;
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00009CF8 File Offset: 0x00007EF8
	public void SetPixel1()
	{
		for (int i = 0; i < this.Terrains.Length; i++)
		{
			this.Terrains[i].GetComponent<Terrain>().heightmapPixelError = 1f;
		}
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00009D30 File Offset: 0x00007F30
	public void SetPixel200()
	{
		for (int i = 0; i < this.Terrains.Length; i++)
		{
			this.Terrains[i].GetComponent<Terrain>().heightmapPixelError = 200f;
		}
	}

	// Token: 0x04000176 RID: 374
	public GameObject[] Terrains;

	// Token: 0x04000177 RID: 375
	public GameObject[] Objects1;

	// Token: 0x04000178 RID: 376
	public GameObject[] Objects2;

	// Token: 0x04000179 RID: 377
	public GameObject[] Objects3;

	// Token: 0x0400017A RID: 378
	public GameObject[] SmallObjects1;

	// Token: 0x0400017B RID: 379
	public GameObject Player;

	// Token: 0x0400017C RID: 380
	public List<Transform> listOfTransforms;

	// Token: 0x0400017D RID: 381
	public List<Transform> listOfTransforms2;

	// Token: 0x0400017E RID: 382
	public float Dist;

	// Token: 0x0400017F RID: 383
	public Slider DistSlider;

	// Token: 0x04000180 RID: 384
	public Text DistText;

	// Token: 0x04000181 RID: 385
	public int Frame;

	// Token: 0x04000182 RID: 386
	public DisableInDistance Disabler2;
}
