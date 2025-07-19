using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E6 RID: 486
public class Setcursorimage : MonoBehaviour
{
	// Token: 0x06000B5D RID: 2909 RVA: 0x0007C7E0 File Offset: 0x0007A9E0
	private void Start()
	{
		this.img1.enabled = true;
		this.img2.enabled = false;
		this.img3.enabled = false;
		this.img4.enabled = false;
		this.img5.enabled = false;
		this.img6.enabled = false;
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x0007C838 File Offset: 0x0007AA38
	private void Update()
	{
		if (tools.sitting || tools.tool == 14)
		{
			if (tools.LookingWindow || tools.tool == 14)
			{
				this.img1.enabled = false;
				this.img2.enabled = false;
				this.img3.enabled = false;
				this.img4.enabled = false;
				this.img5.enabled = false;
				this.img6.enabled = false;
			}
			else
			{
				this.img1.enabled = true;
			}
		}
		else if (tools.canput || tools.canrepair || tools.canremove || tools.canfair || tools.cantake)
		{
			if (!Input.GetMouseButton(0))
			{
				if (tools.cantake)
				{
					this.img1.enabled = false;
					this.img2.enabled = false;
					this.img3.enabled = false;
					this.img4.enabled = false;
					this.img5.enabled = false;
					this.img6.enabled = true;
				}
				if (tools.canrepair)
				{
					this.img1.enabled = false;
					this.img2.enabled = false;
					this.img3.enabled = true;
					this.img4.enabled = false;
					this.img5.enabled = false;
					this.img6.enabled = false;
				}
				if (tools.canremove)
				{
					this.img1.enabled = false;
					this.img2.enabled = false;
					this.img3.enabled = false;
					this.img4.enabled = true;
					this.img5.enabled = false;
					this.img6.enabled = false;
				}
				if (tools.canfair)
				{
					this.img1.enabled = false;
					this.img2.enabled = false;
					this.img3.enabled = false;
					this.img4.enabled = false;
					this.img5.enabled = true;
					this.img6.enabled = false;
				}
			}
			if (tools.canput)
			{
				this.img1.enabled = false;
				this.img2.enabled = true;
				this.img3.enabled = false;
				this.img4.enabled = false;
				this.img5.enabled = false;
				this.img6.enabled = false;
			}
		}
		else
		{
			this.img1.enabled = true;
			this.img2.enabled = false;
			this.img3.enabled = false;
			this.img4.enabled = false;
			this.img5.enabled = false;
			this.img6.enabled = false;
		}
		tools.canrepair = false;
		tools.canput = false;
		tools.canremove = false;
		tools.canfair = false;
		tools.cantake = false;
		tools.LookingWindow = false;
	}

	// Token: 0x040013CB RID: 5067
	public Image img1;

	// Token: 0x040013CC RID: 5068
	public Image img2;

	// Token: 0x040013CD RID: 5069
	public Image img3;

	// Token: 0x040013CE RID: 5070
	public Image img4;

	// Token: 0x040013CF RID: 5071
	public Image img5;

	// Token: 0x040013D0 RID: 5072
	public Image img6;
}
