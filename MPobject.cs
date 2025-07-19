using System;
using Mirror;
using UnityEngine;

// Token: 0x02000215 RID: 533
public class MPobject : MonoBehaviour
{
	// Token: 0x06000C70 RID: 3184 RVA: 0x0008BACC File Offset: 0x00089CCC
	private void Start()
	{
		if (!this.networkDummy && this.MPNumber != 0)
		{
			foreach (networkDummy networkDummy in UnityEngine.Object.FindObjectsOfType<networkDummy>())
			{
				if (networkDummy.MPNumber == this.MPNumber && networkDummy.Target == null)
				{
					this.networkDummy = networkDummy;
					this.networkDummy.Target = base.gameObject;
					this.networkDummy.Start2();
				}
			}
		}
		if (!this.networkDummy && this.MPNumber == 0 && (base.transform.name == "WelderHandle" || base.transform.name == "WaterHose" || base.transform.name == "FuelHose" || base.transform.name == "TrailerHandle" || base.transform.name == "Nozzle"))
		{
			float num = 1E+15f;
			foreach (networkDummy networkDummy2 in UnityEngine.Object.FindObjectsOfType<networkDummy>())
			{
				if (networkDummy2.Itemname == base.transform.name && Vector3.Distance(networkDummy2.transform.position, base.transform.position) < num && networkDummy2.Target == null)
				{
					num = Vector3.Distance(networkDummy2.transform.position, base.transform.position);
					this.networkDummy = networkDummy2;
				}
			}
			if (this.networkDummy)
			{
				this.networkDummy.Target = base.gameObject;
				this.networkDummy.Start2();
			}
		}
		if (!this.networkDummy && this.MPNumber == 0 && base.GetComponent<PickupCup>())
		{
			foreach (networkDummy networkDummy3 in UnityEngine.Object.FindObjectsOfType<networkDummy>())
			{
				if (networkDummy3.Itemname == "CUP" && networkDummy3.Target == null)
				{
					if (base.transform.parent.parent && base.transform.parent.parent.GetComponent<MPobject>() && networkDummy3.CupMPNumber == base.transform.parent.parent.GetComponent<MPobject>().networkDummy.MPNumber)
					{
						this.networkDummy = networkDummy3;
					}
					else if (base.transform.parent && base.transform.parent.GetComponent<MPobject>() && networkDummy3.CupMPNumber == base.transform.parent.GetComponent<MPobject>().networkDummy.MPNumber)
					{
						this.networkDummy = networkDummy3;
					}
				}
			}
			if (this.networkDummy)
			{
				this.networkDummy.Target = base.gameObject;
				this.networkDummy.Start2();
			}
		}
		if (this.networkDummy && !(base.transform.name == "MapMagic") && !base.GetComponent<CarProperties>() && !base.GetComponent<MainCarProperties>() && !base.GetComponent<MainTrailerProperties>())
		{
			this.networkDummy.GetComponent<NetworkTransform>().enabled = false;
			this.networkDummy.enabled = false;
		}
		if (this.networkDummy)
		{
			this.MPNumber = this.networkDummy.MPNumber;
			this.networkDummy.Start2();
		}
		if (this.networkDummy && base.transform.parent && !base.GetComponent<SavePosition>())
		{
			this.networkDummy.GetComponent<NetworkTransform>().enabled = false;
			this.networkDummy.enabled = false;
		}
	}

	// Token: 0x04001561 RID: 5473
	public networkDummy networkDummy;

	// Token: 0x04001562 RID: 5474
	public int MPNumber;
}
