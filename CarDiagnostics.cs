using System;
using PaintIn3D;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class CarDiagnostics : MonoBehaviour
{
	// Token: 0x060000AC RID: 172 RVA: 0x00007ECC File Offset: 0x000060CC
	public void FindCar()
	{
		this.car = null;
		foreach (Collider collider in Physics.OverlapSphere(this.ServicePosition.position, 0.6f))
		{
			if (collider.gameObject.transform.root.GetComponent<MainCarProperties>())
			{
				this.car = collider.gameObject.transform.root.gameObject;
				break;
			}
		}
		if (this.car == null)
		{
			foreach (Collider collider2 in Physics.OverlapSphere(this.ParkPosition.position, 0.6f))
			{
				if (collider2.gameObject.transform.root.GetComponent<MainCarProperties>())
				{
					this.car = collider2.gameObject.transform.root.gameObject;
					break;
				}
			}
		}
		if (this.car)
		{
			this.car.transform.SetPositionAndRotation(this.ServicePosition.position, this.ServicePosition.rotation);
		}
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00007FE4 File Offset: 0x000061E4
	public void FindPaintCar()
	{
		this.car = null;
		foreach (Collider collider in Physics.OverlapSphere(base.transform.position, 0.6f))
		{
			if (collider.gameObject.transform.root.GetComponent<MainCarProperties>())
			{
				this.car = collider.gameObject.transform.root.gameObject;
				return;
			}
		}
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00008058 File Offset: 0x00006258
	public void FullDiagnostics()
	{
		this.car.GetComponent<MainCarProperties>().GetPartStatsFull();
	}

	// Token: 0x060000AF RID: 175 RVA: 0x0000806A File Offset: 0x0000626A
	public void EngineDiagnostics()
	{
		this.car.GetComponent<MainCarProperties>().GetPartStatsEng();
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x0000807C File Offset: 0x0000627C
	public void BrakeDiagnostics()
	{
		this.car.GetComponent<MainCarProperties>().GetPartStatsBr();
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00008090 File Offset: 0x00006290
	public void TestForSelling()
	{
		this.carinfo = tools.AudioParent_.transform.root.GetComponent<tools>().CarInfo.GetComponent<CarInformation>();
		tools.AudioParent_.transform.root.GetComponent<FirstPersonAIO>().ControllerPause();
		this.carinfo.Car = this.car;
		tools.AudioParent_.transform.root.GetComponent<tools>().Cursorcanvas.SetActive(false);
		this.carinfo.gameObject.SetActive(true);
		this.carinfo.ReStart(true);
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00008128 File Offset: 0x00006328
	public void PaintCar()
	{
		Color color = this.car.GetComponent<MainCarProperties>().Color;
		foreach (P3dPaintableTexture p3dPaintableTexture in this.car.GetComponentsInChildren<P3dPaintableTexture>())
		{
			if (p3dPaintableTexture.Slot.Index == 0)
			{
				if (p3dPaintableTexture.Slot.Name == "_GrungeMap")
				{
					p3dPaintableTexture.Color = Color.black;
				}
				else if (p3dPaintableTexture.Slot.Name == "_L2MetallicRustDustSmoothness")
				{
					p3dPaintableTexture.Color = Color.black;
				}
				else
				{
					if (!(p3dPaintableTexture.Slot.Name == "_L2ColorMap"))
					{
						goto IL_A1;
					}
					p3dPaintableTexture.Color = color;
				}
				p3dPaintableTexture.Clear();
			}
			IL_A1:;
		}
		this.paint1.HandleHitPoint(false, 0, 1f, 0, base.transform.position, Quaternion.identity);
	}

	// Token: 0x04000131 RID: 305
	public GameObject car;

	// Token: 0x04000132 RID: 306
	public Transform ServicePosition;

	// Token: 0x04000133 RID: 307
	public Transform ParkPosition;

	// Token: 0x04000134 RID: 308
	public CarInformation carinfo;

	// Token: 0x04000135 RID: 309
	public P3dPaintSphere paint1;

	// Token: 0x04000136 RID: 310
	public P3dPaintSphere paint2;

	// Token: 0x04000137 RID: 311
	public P3dPaintSphere paint3;
}
