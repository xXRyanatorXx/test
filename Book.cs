using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class Book : MonoBehaviour
{
	// Token: 0x06000609 RID: 1545 RVA: 0x000302DC File Offset: 0x0002E4DC
	private void Start()
	{
		base.enabled = false;
		if (!this.Catalog)
		{
			this.Catalog = GameObject.Find("Player").GetComponent<tools>().partshop;
		}
		if (!this.Cursorcanvas)
		{
			this.Cursorcanvas = GameObject.Find("CursorCanvas");
		}
		this.AudioParent = GameObject.Find("hand");
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x00030344 File Offset: 0x0002E544
	private void OnMouseDown()
	{
		if (this.jobs)
		{
			if (Vector3.Distance(base.transform.position, this.AudioParent.transform.position) < 2f && (!tools.MPrunning || (tools.MPrunning && tools.NetworkPLayer.isServer)))
			{
				this.Open();
				return;
			}
		}
		else
		{
			this.Open();
		}
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x000303A7 File Offset: 0x0002E5A7
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.Close();
		}
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x000303B8 File Offset: 0x0002E5B8
	public void Open()
	{
		if (!this.Cursorcanvas)
		{
			this.Cursorcanvas = GameObject.Find("CursorCanvas");
		}
		if (this.catalog || this.jobs)
		{
			if (!this.Catalog)
			{
				this.Catalog = GameObject.Find("Player").GetComponent<tools>().partshop;
			}
			if (!this.Catalog.active && this.Cursorcanvas.active)
			{
				tools.SpawnSpot = this.SpawnSpot;
				tools.PriceModifier = this.PriceModifier;
				this.Cursorcanvas.SetActive(false);
				this.Catalog.SetActive(true);
				GameObject.Find("Player").GetComponent<FirstPersonAIO>().ControllerPause();
				base.enabled = true;
			}
		}
		if (this.PayDyno && tools.money > 100f && !this.dyno.gameObject.activeSelf)
		{
			tools.money -= 100f;
			this.dyno.gameObject.SetActive(true);
			tools.AudioParent_.GetComponent<AudioSource>().PlayOneShot(tools.AudioParent_.GetComponent<AudioManager>().Cash);
		}
		if (this.PayFullDiag)
		{
			if (tools.money > 600f)
			{
				this.diagnostics.FindCar();
			}
			if (this.diagnostics.car != null && tools.money > 600f)
			{
				tools.money -= 600f;
				tools.AudioParent_.GetComponent<AudioSource>().PlayOneShot(tools.AudioParent_.GetComponent<AudioManager>().Cash);
				this.diagnostics.FullDiagnostics();
			}
		}
		if (this.PayEngDiag)
		{
			if (tools.money > 600f)
			{
				this.diagnostics.FindCar();
			}
			if (this.diagnostics.car != null && tools.money > 600f)
			{
				tools.money -= 600f;
				tools.AudioParent_.GetComponent<AudioSource>().PlayOneShot(tools.AudioParent_.GetComponent<AudioManager>().Cash);
				this.diagnostics.EngineDiagnostics();
			}
		}
		if (this.PayBrakeDiag)
		{
			if (tools.money > 600f)
			{
				this.diagnostics.FindCar();
			}
			if (this.diagnostics.car != null && tools.money > 600f)
			{
				tools.money -= 600f;
				tools.AudioParent_.GetComponent<AudioSource>().PlayOneShot(tools.AudioParent_.GetComponent<AudioManager>().Cash);
				this.diagnostics.BrakeDiagnostics();
			}
		}
		if (this.PayPaint)
		{
			if (tools.money > 2500f)
			{
				this.diagnostics.FindPaintCar();
			}
			if (this.diagnostics.car != null && tools.money > 2500f)
			{
				tools.money -= 2500f;
				tools.AudioParent_.GetComponent<AudioSource>().PlayOneShot(tools.AudioParent_.GetComponent<AudioManager>().Cash);
				this.diagnostics.PaintCar();
			}
		}
		if (this.testforselling)
		{
			this.diagnostics.FindCar();
			if (this.diagnostics.car != null)
			{
				this.diagnostics.TestForSelling();
			}
		}
		if (this.AddStrap)
		{
			this.dyno.AttachCar();
		}
		if (this.ReleaseStrap)
		{
			this.dyno.DeAttachCar();
		}
		if (this.PizzaDeliveries)
		{
			this.UnacceptedPizzas.SetActive(true);
			if (!this.MapCanvas)
			{
				this.MapCanvas = GameObject.Find("Player").GetComponent<tools>().MapCanvas;
			}
			if (!this.MapCanvas.active && this.Cursorcanvas.active)
			{
				tools.SpawnSpot = this.SpawnSpot;
				this.Cursorcanvas.SetActive(false);
				this.MapCanvas.SetActive(true);
				GameObject.Find("Player").GetComponent<FirstPersonAIO>().ControllerPause();
				base.enabled = true;
				if (tools.PizzaDeliveriesCount <= 0 && EnviroSkyMgr.instance.Time.Days != tools.PizzaDeliveriesDay)
				{
					foreach (UpdateMapPosition updateMapPosition in this.UnacceptedPizzas.GetComponentsInChildren<UpdateMapPosition>())
					{
						UnityEngine.Object.Destroy(updateMapPosition.DeliveryTarget.gameObject);
						UnityEngine.Object.Destroy(updateMapPosition.gameObject);
					}
					int num = 0;
					UnityEngine.Random.Range(0, 10);
					tools.PizzaDeliveriesDay = EnviroSkyMgr.instance.Time.Days;
					foreach (GameObject gameObject in Book.GetRandomItemsFromList<GameObject>(this.ClientsPositions, 10))
					{
						GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.ClientPrefab, gameObject.transform.position, Quaternion.identity);
						gameObject2.transform.name = this.ClientPrefab.transform.name;
						if (num == 5)
						{
							gameObject2.GetComponent<DeliveryTarget>().BarnGiver = true;
						}
						num++;
					}
				}
			}
		}
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x000308C4 File Offset: 0x0002EAC4
	public static List<T> GetRandomItemsFromList<T>(List<T> list, int number)
	{
		List<T> list2 = new List<T>(list);
		List<T> list3 = new List<T>();
		while (list3.Count < number && list2.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, list2.Count);
			list3.Add(list2[index]);
			list2.RemoveAt(index);
		}
		return list3;
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x00030914 File Offset: 0x0002EB14
	public void Close()
	{
		if (this.PizzaDeliveries)
		{
			this.UnacceptedPizzas.SetActive(false);
		}
		base.enabled = false;
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x00030934 File Offset: 0x0002EB34
	public void SpawnDeliveryObject()
	{
		if (tools.MPrunning)
		{
			tools.NetworkPLayer.ITEM = this.DeliveryObject;
			tools.NetworkPLayer.Itemname = this.DeliveryObject.name;
			tools.NetworkPLayer.Spawnposition = this.SpawnSpot.transform.position;
			tools.NetworkPLayer.Spawnrotation = Quaternion.identity;
			tools.NetworkPLayer.SpawnObject(0, true);
			return;
		}
		UnityEngine.Object.Instantiate<GameObject>(this.DeliveryObject, this.SpawnSpot.transform.position, Quaternion.identity).transform.name = this.DeliveryObject.transform.name;
	}

	// Token: 0x04000911 RID: 2321
	public GameObject Catalog;

	// Token: 0x04000912 RID: 2322
	public GameObject MapCanvas;

	// Token: 0x04000913 RID: 2323
	public GameObject Cursorcanvas;

	// Token: 0x04000914 RID: 2324
	public GameObject SpawnSpot;

	// Token: 0x04000915 RID: 2325
	public float PriceModifier;

	// Token: 0x04000916 RID: 2326
	public GameObject AudioParent;

	// Token: 0x04000917 RID: 2327
	public bool jobs;

	// Token: 0x04000918 RID: 2328
	public bool catalog;

	// Token: 0x04000919 RID: 2329
	public bool PizzaDeliveries;

	// Token: 0x0400091A RID: 2330
	public bool PayDyno;

	// Token: 0x0400091B RID: 2331
	public DYNO dyno;

	// Token: 0x0400091C RID: 2332
	public bool AddStrap;

	// Token: 0x0400091D RID: 2333
	public bool ReleaseStrap;

	// Token: 0x0400091E RID: 2334
	public bool PayFullDiag;

	// Token: 0x0400091F RID: 2335
	public bool PayEngDiag;

	// Token: 0x04000920 RID: 2336
	public bool PayBrakeDiag;

	// Token: 0x04000921 RID: 2337
	public bool PayPaint;

	// Token: 0x04000922 RID: 2338
	public CarDiagnostics diagnostics;

	// Token: 0x04000923 RID: 2339
	public bool testforselling;

	// Token: 0x04000924 RID: 2340
	public List<GameObject> ClientsPositions;

	// Token: 0x04000925 RID: 2341
	public GameObject ClientPrefab;

	// Token: 0x04000926 RID: 2342
	public GameObject UnacceptedPizzas;

	// Token: 0x04000927 RID: 2343
	public GameObject AcceptedPizzas;

	// Token: 0x04000928 RID: 2344
	public GameObject DeliveryObject;
}
