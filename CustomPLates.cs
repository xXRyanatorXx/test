using System;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class CustomPLates : MonoBehaviour
{
	// Token: 0x0600068A RID: 1674 RVA: 0x00034ED7 File Offset: 0x000330D7
	private void Start()
	{
		this.AudioParent = GameObject.Find("hand");
		this.NumberParent = GameObject.Find("NumberPlates").GetComponent<NumberPlateManager>();
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x00034F00 File Offset: 0x00033100
	public void BuyRandom()
	{
		if (tools.money >= 38f)
		{
			this.NumberParent.CreateRandomNumber();
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
			this.SpawnPlates(this.NumberParent.M1.name.ToString(), this.NumberParent.M2.name.ToString(), this.NumberParent.M3.name.ToString(), this.NumberParent.M4.name.ToString(), this.NumberParent.M5.name.ToString(), this.NumberParent.M6.name.ToString());
			tools.money -= 38f;
		}
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x00034FDC File Offset: 0x000331DC
	public void BuyCustom()
	{
		if (tools.money >= 2500f)
		{
			this.NumberParent.CreateRandomNumber();
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
			this.SpawnPlates(this.One.name.ToString(), this.Two.name.ToString(), this.Three.name.ToString(), this.Four.name.ToString(), this.Five.name.ToString(), this.Six.name.ToString());
			tools.money -= 2500f;
		}
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x0003509C File Offset: 0x0003329C
	public void SpawnPlates(string OneNR, string TwoNR, string ThreeNR, string FourNR, string FiveNR, string SixNR)
	{
		if (tools.MPrunning)
		{
			tools.NetworkPLayer.SpawnPlates2(OneNR, TwoNR, ThreeNR, FourNR, FiveNR, SixNR, this.SpawnSpot.transform.position);
			return;
		}
		this.SpawnPlates2(OneNR, TwoNR, ThreeNR, FourNR, FiveNR, SixNR, this.SpawnSpot.transform.position);
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x000350F4 File Offset: 0x000332F4
	public void SpawnPlates2(string OneNR, string TwoNR, string ThreeNR, string FourNR, string FiveNR, string SixNR, Vector3 Spot)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Item, Spot, Quaternion.identity);
		gameObject.transform.name = this.Item.transform.name;
		gameObject.GetComponent<Partinfo>().Creating();
		gameObject.GetComponent<CarProperties>().One = (Resources.Load(OneNR, typeof(Material)) as Material);
		gameObject.GetComponent<CarProperties>().Two = (Resources.Load(TwoNR, typeof(Material)) as Material);
		gameObject.GetComponent<CarProperties>().Three = (Resources.Load(ThreeNR, typeof(Material)) as Material);
		gameObject.GetComponent<CarProperties>().Four = (Resources.Load(FourNR, typeof(Material)) as Material);
		gameObject.GetComponent<CarProperties>().Five = (Resources.Load(FiveNR, typeof(Material)) as Material);
		gameObject.GetComponent<CarProperties>().Six = (Resources.Load(SixNR, typeof(Material)) as Material);
		gameObject.GetComponent<CarProperties>().LoadNumber();
		UnityEngine.Object.Instantiate<GameObject>(gameObject, Spot, Quaternion.identity).transform.name = gameObject.transform.name;
	}

	// Token: 0x040009FA RID: 2554
	public GameObject Item;

	// Token: 0x040009FB RID: 2555
	public GameObject SpawnSpot;

	// Token: 0x040009FC RID: 2556
	public GameObject AudioParent;

	// Token: 0x040009FD RID: 2557
	public NumberPlateManager NumberParent;

	// Token: 0x040009FE RID: 2558
	public Material One;

	// Token: 0x040009FF RID: 2559
	public Material Two;

	// Token: 0x04000A00 RID: 2560
	public Material Three;

	// Token: 0x04000A01 RID: 2561
	public Material Four;

	// Token: 0x04000A02 RID: 2562
	public Material Five;

	// Token: 0x04000A03 RID: 2563
	public Material Six;
}
