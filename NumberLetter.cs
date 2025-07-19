using System;
using UnityEngine;

// Token: 0x02000230 RID: 560
public class NumberLetter : MonoBehaviour
{
	// Token: 0x06000D74 RID: 3444 RVA: 0x0009187C File Offset: 0x0008FA7C
	private void Start()
	{
		this.NumberParent = GameObject.Find("NumberPlates").GetComponent<NumberPlateManager>();
		this.Plate = base.transform.parent.GetComponent<CustomPLates>();
		Material[] materials = this.Plate.gameObject.GetComponent<Renderer>().materials;
		if (this.numb == 1)
		{
			this.Mat = materials[2];
		}
		if (this.numb == 2)
		{
			this.Mat = materials[3];
		}
		if (this.numb == 3)
		{
			this.Mat = materials[4];
		}
		if (this.numb == 4)
		{
			this.Mat = materials[5];
		}
		if (this.numb == 5)
		{
			this.Mat = materials[6];
		}
		if (this.numb == 6)
		{
			this.Mat = materials[7];
		}
		this.CurrentNumb = "Empty";
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x00091944 File Offset: 0x0008FB44
	private void OnMouseDown()
	{
		if (this.BuyCustom)
		{
			this.Plate.BuyCustom();
		}
		if (this.BuyRandom)
		{
			this.Plate.BuyRandom();
		}
		Material[] materials = this.Plate.gameObject.GetComponent<Renderer>().materials;
		if (this.CurrentNumb == "Empty")
		{
			this.CurrentNumb = "Line";
			this.Mat = this.NumberParent.Line;
		}
		else if (this.CurrentNumb == "Line")
		{
			this.CurrentNumb = "Zero";
			this.Mat = this.NumberParent.Zero;
		}
		else if (this.CurrentNumb == "Zero")
		{
			this.CurrentNumb = "One";
			this.Mat = this.NumberParent.One;
		}
		else if (this.CurrentNumb == "One")
		{
			this.CurrentNumb = "Two";
			this.Mat = this.NumberParent.Two;
		}
		else if (this.CurrentNumb == "Two")
		{
			this.CurrentNumb = "Three";
			this.Mat = this.NumberParent.Three;
		}
		else if (this.CurrentNumb == "Three")
		{
			this.CurrentNumb = "Four";
			this.Mat = this.NumberParent.Four;
		}
		else if (this.CurrentNumb == "Four")
		{
			this.CurrentNumb = "Five";
			this.Mat = this.NumberParent.Five;
		}
		else if (this.CurrentNumb == "Five")
		{
			this.CurrentNumb = "Six";
			this.Mat = this.NumberParent.Six;
		}
		else if (this.CurrentNumb == "Six")
		{
			this.CurrentNumb = "Seven";
			this.Mat = this.NumberParent.Seven;
		}
		else if (this.CurrentNumb == "Seven")
		{
			this.CurrentNumb = "Eight";
			this.Mat = this.NumberParent.Eight;
		}
		else if (this.CurrentNumb == "Eight")
		{
			this.CurrentNumb = "Nine";
			this.Mat = this.NumberParent.Nine;
		}
		else if (this.CurrentNumb == "Nine")
		{
			this.CurrentNumb = "A";
			this.Mat = this.NumberParent.A;
		}
		else if (this.CurrentNumb == "A")
		{
			this.CurrentNumb = "B";
			this.Mat = this.NumberParent.B;
		}
		else if (this.CurrentNumb == "B")
		{
			this.CurrentNumb = "C";
			this.Mat = this.NumberParent.C;
		}
		else if (this.CurrentNumb == "C")
		{
			this.CurrentNumb = "D";
			this.Mat = this.NumberParent.D;
		}
		else if (this.CurrentNumb == "D")
		{
			this.CurrentNumb = "E";
			this.Mat = this.NumberParent.E;
		}
		else if (this.CurrentNumb == "E")
		{
			this.CurrentNumb = "F";
			this.Mat = this.NumberParent.F;
		}
		else if (this.CurrentNumb == "F")
		{
			this.CurrentNumb = "G";
			this.Mat = this.NumberParent.G;
		}
		else if (this.CurrentNumb == "G")
		{
			this.CurrentNumb = "H";
			this.Mat = this.NumberParent.H;
		}
		else if (this.CurrentNumb == "H")
		{
			this.CurrentNumb = "I";
			this.Mat = this.NumberParent.I;
		}
		else if (this.CurrentNumb == "I")
		{
			this.CurrentNumb = "J";
			this.Mat = this.NumberParent.J;
		}
		else if (this.CurrentNumb == "J")
		{
			this.CurrentNumb = "K";
			this.Mat = this.NumberParent.K;
		}
		else if (this.CurrentNumb == "K")
		{
			this.CurrentNumb = "L";
			this.Mat = this.NumberParent.L;
		}
		else if (this.CurrentNumb == "L")
		{
			this.CurrentNumb = "M";
			this.Mat = this.NumberParent.M;
		}
		else if (this.CurrentNumb == "M")
		{
			this.CurrentNumb = "N";
			this.Mat = this.NumberParent.N;
		}
		else if (this.CurrentNumb == "N")
		{
			this.CurrentNumb = "O";
			this.Mat = this.NumberParent.O;
		}
		else if (this.CurrentNumb == "O")
		{
			this.CurrentNumb = "P";
			this.Mat = this.NumberParent.P;
		}
		else if (this.CurrentNumb == "P")
		{
			this.CurrentNumb = "Q";
			this.Mat = this.NumberParent.Q;
		}
		else if (this.CurrentNumb == "Q")
		{
			this.CurrentNumb = "R";
			this.Mat = this.NumberParent.R;
		}
		else if (this.CurrentNumb == "R")
		{
			this.CurrentNumb = "S";
			this.Mat = this.NumberParent.S;
		}
		else if (this.CurrentNumb == "S")
		{
			this.CurrentNumb = "T";
			this.Mat = this.NumberParent.T;
		}
		else if (this.CurrentNumb == "T")
		{
			this.CurrentNumb = "U";
			this.Mat = this.NumberParent.U;
		}
		else if (this.CurrentNumb == "U")
		{
			this.CurrentNumb = "V";
			this.Mat = this.NumberParent.V;
		}
		else if (this.CurrentNumb == "V")
		{
			this.CurrentNumb = "W";
			this.Mat = this.NumberParent.W;
		}
		else if (this.CurrentNumb == "W")
		{
			this.CurrentNumb = "X";
			this.Mat = this.NumberParent.X;
		}
		else if (this.CurrentNumb == "X")
		{
			this.CurrentNumb = "Y";
			this.Mat = this.NumberParent.Y;
		}
		else if (this.CurrentNumb == "Y")
		{
			this.CurrentNumb = "Z";
			this.Mat = this.NumberParent.Z;
		}
		else if (this.CurrentNumb == "Z")
		{
			this.CurrentNumb = "Empty";
			this.Mat = this.NumberParent.Empty;
		}
		if (this.numb == 1)
		{
			materials[2] = this.Mat;
			this.Plate.One = this.Mat;
		}
		if (this.numb == 2)
		{
			materials[3] = this.Mat;
			this.Plate.Two = this.Mat;
		}
		if (this.numb == 3)
		{
			materials[4] = this.Mat;
			this.Plate.Three = this.Mat;
		}
		if (this.numb == 4)
		{
			materials[5] = this.Mat;
			this.Plate.Four = this.Mat;
		}
		if (this.numb == 5)
		{
			materials[6] = this.Mat;
			this.Plate.Five = this.Mat;
		}
		if (this.numb == 6)
		{
			materials[7] = this.Mat;
			this.Plate.Six = this.Mat;
		}
		this.Plate.gameObject.GetComponent<Renderer>().materials = materials;
	}

	// Token: 0x04001601 RID: 5633
	public CustomPLates Plate;

	// Token: 0x04001602 RID: 5634
	public int numb;

	// Token: 0x04001603 RID: 5635
	public Material Mat;

	// Token: 0x04001604 RID: 5636
	public string CurrentNumb;

	// Token: 0x04001605 RID: 5637
	public NumberPlateManager NumberParent;

	// Token: 0x04001606 RID: 5638
	public bool BuyRandom;

	// Token: 0x04001607 RID: 5639
	public bool BuyCustom;
}
