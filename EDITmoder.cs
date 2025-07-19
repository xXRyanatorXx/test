using System;
using RVP;
using UnityEngine;

// Token: 0x0200003A RID: 58
[ExecuteInEditMode]
public class EDITmoder : MonoBehaviour
{
	// Token: 0x0600011C RID: 284 RVA: 0x0000A1F4 File Offset: 0x000083F4
	private void Start()
	{
		if (Input.GetKeyDown("return"))
		{
			base.GetComponent<MainCarProperties>().Weight = 0f;
			foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
			{
				if (carProperties.PREFAB)
				{
					carProperties.gameObject.GetComponent<Partinfo>().weight = carProperties.PREFAB.gameObject.GetComponent<Partinfo>().weight;
					base.GetComponent<MainCarProperties>().Weight += carProperties.PREFAB.gameObject.GetComponent<Partinfo>().weight;
				}
			}
			foreach (CarProperties carProperties2 in base.GetComponentsInChildren<CarProperties>())
			{
				if (carProperties2.DMGRemovablepart && !carProperties2.gameObject.GetComponent<DetachablePart>())
				{
					carProperties2.gameObject.AddComponent<DetachablePart>();
				}
			}
		}
		foreach (Partinfo partinfo in base.GetComponentsInChildren<Partinfo>())
		{
			partinfo.fixedImportantBolts = 0f;
			partinfo.fixedwelds = 0f;
			partinfo.attachedwelds = 0f;
			partinfo.ImportantBolts = 0f;
			partinfo.tightnuts = 0f;
			partinfo.attachedbolts = 0f;
		}
		foreach (HexNut hexNut in base.GetComponentsInChildren<HexNut>())
		{
			hexNut.gameObject.transform.parent.GetComponent<Partinfo>().attachedbolts += 1f;
			hexNut.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts += 1f;
		}
		foreach (FlatNut flatNut in base.GetComponentsInChildren<FlatNut>())
		{
			if (!flatNut.gameObject.transform.parent.GetComponent<Partinfo>())
			{
				Debug.Log(flatNut);
			}
			flatNut.gameObject.transform.parent.GetComponent<Partinfo>().attachedbolts += 1f;
			flatNut.gameObject.transform.parent.GetComponent<Partinfo>().tightnuts += 1f;
		}
		foreach (BoltNut boltNut in base.GetComponentsInChildren<BoltNut>())
		{
			if (!boltNut.gameObject.transform.parent.GetComponent<Partinfo>())
			{
				Debug.Log(boltNut);
			}
			boltNut.gameObject.transform.parent.GetComponent<Partinfo>().ImportantBolts += 1f;
			boltNut.gameObject.transform.parent.GetComponent<Partinfo>().fixedImportantBolts += 1f;
			if (!boltNut.otherobject)
			{
				Debug.Log(boltNut);
			}
			if (!boltNut.otherobject.GetComponent<Partinfo>())
			{
				Debug.Log(boltNut);
			}
			boltNut.otherobject.GetComponent<Partinfo>().fixedImportantBolts += 1f;
			boltNut.otherobject.GetComponent<Partinfo>().ImportantBolts += 1f;
		}
		foreach (WeldCut weldCut in base.GetComponentsInChildren<WeldCut>())
		{
			weldCut.gameObject.transform.parent.GetComponent<Partinfo>().fixedwelds += 1f;
			weldCut.gameObject.transform.parent.GetComponent<Partinfo>().attachedwelds += 1f;
			if (!weldCut.otherobject)
			{
				Debug.Log(weldCut);
			}
			weldCut.otherobject.GetComponent<Partinfo>().fixedwelds += 1f;
			weldCut.otherobject.GetComponent<Partinfo>().attachedwelds += 1f;
		}
	}
}
