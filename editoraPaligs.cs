using System;
using UnityEngine;

// Token: 0x020001F5 RID: 501
[ExecuteInEditMode]
public class editoraPaligs : MonoBehaviour
{
	// Token: 0x06000BBC RID: 3004 RVA: 0x0008259C File Offset: 0x0008079C
	private void Updatddde()
	{
		base.GetComponent<MainCarProperties>().PartsCount = 0f;
		base.GetComponent<MainCarProperties>().CarPrice = 0f;
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.SinglePart)
			{
				if (!carProperties.PREFAB)
				{
					string str = "nav prefab";
					GameObject gameObject = carProperties.gameObject;
					Debug.Log(str + ((gameObject != null) ? gameObject.ToString() : null));
				}
				if (carProperties.gameObject.GetComponent<Partinfo>().price == 0f)
				{
					string str2 = "nav price";
					GameObject gameObject2 = carProperties.gameObject;
					Debug.Log(str2 + ((gameObject2 != null) ? gameObject2.ToString() : null));
				}
				carProperties.gameObject.GetComponent<Partinfo>().price = carProperties.PREFAB.GetComponent<Partinfo>().price;
				base.GetComponent<MainCarProperties>().CarPrice += carProperties.gameObject.GetComponent<Partinfo>().price;
				base.GetComponent<MainCarProperties>().PartsCount += 1f;
			}
			if (carProperties.PREFAB)
			{
				if (!carProperties.SinglePart)
				{
					string str3 = "singlepart";
					GameObject gameObject3 = carProperties.gameObject;
					Debug.Log(str3 + ((gameObject3 != null) ? gameObject3.ToString() : null));
				}
				if (carProperties.gameObject.GetComponent<Partinfo>().price == 0f)
				{
					string str4 = "nav price";
					GameObject gameObject4 = carProperties.gameObject;
					Debug.Log(str4 + ((gameObject4 != null) ? gameObject4.ToString() : null));
				}
			}
		}
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x00082720 File Offset: 0x00080920
	private void Updatffe()
	{
		base.GetComponent<MainCarProperties>().PartsCount = 0f;
		base.GetComponent<MainCarProperties>().CarPrice = 0f;
		this.parts = this.Partslist.GetComponent<JunkPartsList>().Parts;
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.SinglePart)
			{
				for (int j = 0; j < this.parts.Length; j++)
				{
					GameObject gameObject = this.parts[j];
					if (gameObject.transform.name == carProperties.transform.name)
					{
						carProperties.PREFAB = gameObject;
					}
				}
				if (!carProperties.PREFAB)
				{
					string str = "nav prefab";
					GameObject gameObject2 = carProperties.gameObject;
					Debug.Log(str + ((gameObject2 != null) ? gameObject2.ToString() : null));
				}
				if (carProperties.gameObject.GetComponent<Partinfo>().price == 0f)
				{
					string str2 = "nav price";
					GameObject gameObject3 = carProperties.gameObject;
					Debug.Log(str2 + ((gameObject3 != null) ? gameObject3.ToString() : null));
				}
				carProperties.gameObject.GetComponent<Partinfo>().price = carProperties.PREFAB.GetComponent<Partinfo>().price;
				base.GetComponent<MainCarProperties>().CarPrice += carProperties.gameObject.GetComponent<Partinfo>().price;
				base.GetComponent<MainCarProperties>().PartsCount += 1f;
			}
			if (carProperties.PREFAB)
			{
				if (!carProperties.SinglePart)
				{
					string str3 = "singlepart";
					GameObject gameObject4 = carProperties.gameObject;
					Debug.Log(str3 + ((gameObject4 != null) ? gameObject4.ToString() : null));
				}
				if (carProperties.gameObject.GetComponent<Partinfo>().price == 0f)
				{
					string str4 = "nav price";
					GameObject gameObject5 = carProperties.gameObject;
					Debug.Log(str4 + ((gameObject5 != null) ? gameObject5.ToString() : null));
				}
			}
		}
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x00082900 File Offset: 0x00080B00
	private void Updatesssss()
	{
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.PREFAB != null)
			{
				if (carProperties.PREFAB.GetComponent<CarProperties>().PrefabName != "")
				{
					carProperties.PREFAB = (GameObject)Resources.Load(carProperties.PREFAB.GetComponent<CarProperties>().PrefabName);
				}
				else
				{
					carProperties.PREFAB = (GameObject)Resources.Load(carProperties.transform.name);
				}
			}
		}
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x00082990 File Offset: 0x00080B90
	private void Updatee()
	{
		foreach (Partinfo partinfo in base.GetComponentsInChildren<Partinfo>())
		{
			partinfo.attachedwelds = 0f;
			partinfo.fixedwelds = 0f;
		}
		foreach (WeldCut weldCut in base.GetComponentsInChildren<WeldCut>())
		{
			weldCut.ReStart();
			weldCut.gameObject.transform.parent.GetComponent<Partinfo>().attachedwelds += 1f;
			if (!weldCut.otherobject)
			{
				Debug.Log(weldCut.gameObject);
			}
			if (!weldCut.otherobject.GetComponent<Partinfo>())
			{
				Debug.Log(weldCut.gameObject);
			}
			weldCut.otherobject.transform.GetComponent<Partinfo>().attachedwelds += 1f;
			weldCut.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds += 1f;
			weldCut.otherobject.GetComponent<Partinfo>().fixedwelds += 1f;
		}
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x00082AB0 File Offset: 0x00080CB0
	private void Updatse()
	{
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.PrefabName != "")
			{
				carProperties.PREFAB = (Resources.Load(carProperties.PrefabName) as GameObject);
			}
		}
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x00082B00 File Offset: 0x00080D00
	private void Updaffte()
	{
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			Debug.Log("saa");
			if (carProperties.SinglePart)
			{
				for (int j = 0; j < this.a.Length; j++)
				{
					GameObject gameObject = this.a[j];
					if (gameObject.name == carProperties.gameObject.name)
					{
						carProperties.PREFAB = gameObject;
					}
				}
			}
		}
	}

	// Token: 0x04001468 RID: 5224
	public GameObject[] a;

	// Token: 0x04001469 RID: 5225
	public GameObject Partslist;

	// Token: 0x0400146A RID: 5226
	public GameObject[] parts;
}
