using System;
using System.Collections;
using NWH.WheelController3D;
using UnityEngine;

// Token: 0x02000152 RID: 338
public class MainTrailerProperties : MonoBehaviour
{
	// Token: 0x06000728 RID: 1832 RVA: 0x0003B080 File Offset: 0x00039280
	public void Start()
	{
		base.GetComponent<Rigidbody>().centerOfMass = this.centerOfMass;
		this.StartPosition = base.transform.position;
		this.StartRotation = base.transform.rotation;
		base.StartCoroutine(this.FinishedInitializing());
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x0003B0CD File Offset: 0x000392CD
	private IEnumerator FinishedInitializing()
	{
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return new WaitForSeconds(3f);
		yield return new WaitForSeconds(3f);
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		base.GetComponent<Rigidbody>().drag = 0.1f;
		base.GetComponent<Rigidbody>().angularDrag = 0.1f;
		yield break;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x0003B0DC File Offset: 0x000392DC
	public void SetSleep()
	{
		DISABLER[] componentsInChildren = base.GetComponentsInChildren<DISABLER>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].DisableFromMainCar();
		}
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x0003B108 File Offset: 0x00039308
	public void SetHbrake()
	{
		this.HbrakeON = true;
		if (this.WCFL)
		{
			this.WCFL.dragTorque = 2000f;
		}
		if (this.WCFR)
		{
			this.WCFR.dragTorque = 2000f;
		}
		if (this.WCRL)
		{
			this.WCRL.dragTorque = 2000f;
		}
		if (this.WCRR)
		{
			this.WCRR.dragTorque = 2000f;
		}
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x0003B190 File Offset: 0x00039390
	public void ReleaseHbrake()
	{
		this.HbrakeON = false;
		if (this.WCFL)
		{
			this.WCFL.dragTorque = 10f;
		}
		if (this.WCFR)
		{
			this.WCFR.dragTorque = 10f;
		}
		if (this.WCRL)
		{
			this.WCRL.dragTorque = 10f;
		}
		if (this.WCRR)
		{
			this.WCRR.dragTorque = 10f;
		}
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x0003B218 File Offset: 0x00039418
	public void SetAwake()
	{
		DISABLER[] componentsInChildren = base.GetComponentsInChildren<DISABLER>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].EnableFromMainCar();
		}
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x0003B244 File Offset: 0x00039444
	public void SetOwnerPlayer()
	{
		CarProperties[] componentsInChildren = base.GetComponentsInChildren<CarProperties>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Owner = "Player";
		}
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0003B274 File Offset: 0x00039474
	public void SetMP(bool Loc)
	{
		if (!Loc)
		{
			if (this.WCFL)
			{
				this.WCFL.visualOnlyUpdate = true;
			}
			if (this.WCFR)
			{
				this.WCFR.visualOnlyUpdate = true;
			}
			if (this.WCRL)
			{
				this.WCRL.visualOnlyUpdate = true;
			}
			if (this.WCRR)
			{
				this.WCRR.visualOnlyUpdate = true;
				return;
			}
		}
		else
		{
			if (this.WCFL)
			{
				this.WCFL.visualOnlyUpdate = false;
			}
			if (this.WCFR)
			{
				this.WCFR.visualOnlyUpdate = false;
			}
			if (this.WCRL)
			{
				this.WCRL.visualOnlyUpdate = false;
			}
			if (this.WCRR)
			{
				this.WCRR.visualOnlyUpdate = false;
			}
		}
	}

	// Token: 0x04000B39 RID: 2873
	public Vector3 HookPoint;

	// Token: 0x04000B3A RID: 2874
	public float TrailerPrice;

	// Token: 0x04000B3B RID: 2875
	public bool JustSpawned;

	// Token: 0x04000B3C RID: 2876
	public int ObjectNumber;

	// Token: 0x04000B3D RID: 2877
	public string PrefabName;

	// Token: 0x04000B3E RID: 2878
	public bool InBarn;

	// Token: 0x04000B3F RID: 2879
	public Vector3 StartPosition;

	// Token: 0x04000B40 RID: 2880
	public Quaternion StartRotation;

	// Token: 0x04000B41 RID: 2881
	public WheelController WCFL;

	// Token: 0x04000B42 RID: 2882
	public WheelController WCFR;

	// Token: 0x04000B43 RID: 2883
	public WheelController WCRL;

	// Token: 0x04000B44 RID: 2884
	public WheelController WCRR;

	// Token: 0x04000B45 RID: 2885
	public InsideItems insideitems;

	// Token: 0x04000B46 RID: 2886
	public Vector3 centerOfMass = Vector3.zero;

	// Token: 0x04000B47 RID: 2887
	public bool HbrakeON;
}
