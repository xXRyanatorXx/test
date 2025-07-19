using System;
using NWH.VehiclePhysics2;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000033 RID: 51
public class DYNO : MonoBehaviour
{
	// Token: 0x060000F1 RID: 241 RVA: 0x00009538 File Offset: 0x00007738
	public void AttachCar()
	{
		Collider[] array = Physics.OverlapSphere(this.DynoPosition.position, 0.6f);
		int i = 0;
		while (i < array.Length)
		{
			Collider collider = array[i];
			if (collider.gameObject.transform.root.GetComponent<MainCarProperties>())
			{
				this.car = collider.gameObject.transform.root.gameObject;
				this.car.AddComponent<HingeJoint>();
				this.car.GetComponent<HingeJoint>().connectedBody = this.DynoPosition.GetComponent<Rigidbody>();
				this.car.GetComponent<MainCarProperties>().OnDyno = true;
				this.AttachStrap.SetActive(false);
				this.DeAttachStrap.SetActive(true);
				this.mcp = this.car.GetComponent<MainCarProperties>();
				this.vc = this.car.GetComponent<VehicleController>();
				this.maxpower = 0f;
				this.MaxHPtext.text = "";
				this.HPtext.text = "";
				if (!this.car.GetComponent<MainCarProperties>().Bike)
				{
					this.l1.SetPosition(0, this.l1.transform.position);
					this.l1.SetPosition(1, this.car.transform.Find("FrontSusp").position);
					this.l2.SetPosition(0, this.l2.transform.position);
					this.l2.SetPosition(1, this.car.transform.Find("RearSusp").position);
					return;
				}
				break;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x000096E4 File Offset: 0x000078E4
	public void DeAttachCar()
	{
		UnityEngine.Object.Destroy(this.car.GetComponent<HingeJoint>());
		this.car.GetComponent<MainCarProperties>().OnDyno = false;
		this.car = null;
		this.AttachStrap.SetActive(true);
		this.DeAttachStrap.SetActive(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00009740 File Offset: 0x00007940
	private void Update()
	{
		this.Fan.Rotate(Vector3.up, 1200f * Time.deltaTime);
		if (this.car && this.mcp.CurrentGear != "N")
		{
			float num = this.mcp.EnginePower / this.mcp.WCFL.MaxRPM * this.vc.powertrain.engine.RPM;
			if (num > this.maxpower)
			{
				this.maxpower = num;
			}
			this.MaxHPtext.text = "PEAK " + Mathf.Round(this.maxpower).ToString();
			this.HPtext.text = "HP " + Mathf.Round(num).ToString();
			this.car.GetComponent<MainCarProperties>().HP = this.maxpower;
		}
	}

	// Token: 0x04000164 RID: 356
	public GameObject car;

	// Token: 0x04000165 RID: 357
	public Transform DynoPosition;

	// Token: 0x04000166 RID: 358
	public GameObject AttachStrap;

	// Token: 0x04000167 RID: 359
	public GameObject DeAttachStrap;

	// Token: 0x04000168 RID: 360
	public Transform Fan;

	// Token: 0x04000169 RID: 361
	public LineRenderer l1;

	// Token: 0x0400016A RID: 362
	public LineRenderer l2;

	// Token: 0x0400016B RID: 363
	public Text HPtext;

	// Token: 0x0400016C RID: 364
	public Text MaxHPtext;

	// Token: 0x0400016D RID: 365
	public float maxpower;

	// Token: 0x0400016E RID: 366
	public MainCarProperties mcp;

	// Token: 0x0400016F RID: 367
	public VehicleController vc;
}
