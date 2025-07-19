using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200022F RID: 559
public class NotesCanvas : MonoBehaviour
{
	// Token: 0x06000D6E RID: 3438 RVA: 0x00091290 File Offset: 0x0008F490
	public void Delete()
	{
		this.Line1.text = "";
		this.Line2.text = "";
		this.Line3.text = "";
		this.Line4.text = "";
		this.Line5.text = "";
		this.Line6.text = "";
		this.Line7.text = "";
		this.Line8.text = "";
		this.Line9.text = "";
		this.Line10.text = "";
		this.Line11.text = "";
		this.Line12.text = "";
		this.Line13.text = "";
		this.Line14.text = "";
		this.Line15.text = "";
		this.Line16.text = "";
		this.Line17.text = "";
		this.Line18.text = "";
		this.Line19.text = "";
		this.Line20.text = "";
		this.SetValue();
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x000913E4 File Offset: 0x0008F5E4
	private void Start()
	{
		this.Line1.text = this.L1;
		this.Line2.text = this.L2;
		this.Line3.text = this.L3;
		this.Line4.text = this.L4;
		this.Line5.text = this.L5;
		this.Line6.text = this.L6;
		this.Line7.text = this.L7;
		this.Line8.text = this.L8;
		this.Line9.text = this.L9;
		this.Line10.text = this.L10;
		this.Line11.text = this.L11;
		this.Line12.text = this.L12;
		this.Line13.text = this.L13;
		this.Line14.text = this.L14;
		this.Line15.text = this.L15;
		this.Line16.text = this.L16;
		this.Line17.text = this.L17;
		this.Line18.text = this.L18;
		this.Line19.text = this.L19;
		this.Line20.text = this.L20;
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x00091548 File Offset: 0x0008F748
	private void OnEnable()
	{
		this.Line1.text = this.L1;
		this.Line2.text = this.L2;
		this.Line3.text = this.L3;
		this.Line4.text = this.L4;
		this.Line5.text = this.L5;
		this.Line6.text = this.L6;
		this.Line7.text = this.L7;
		this.Line8.text = this.L8;
		this.Line9.text = this.L9;
		this.Line10.text = this.L10;
		this.Line11.text = this.L11;
		this.Line12.text = this.L12;
		this.Line13.text = this.L13;
		this.Line14.text = this.L14;
		this.Line15.text = this.L15;
		this.Line16.text = this.L16;
		this.Line17.text = this.L17;
		this.Line18.text = this.L18;
		this.Line19.text = this.L19;
		this.Line20.text = this.L20;
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x000916AC File Offset: 0x0008F8AC
	public void SetValue()
	{
		this.L1 = this.Line1.text.ToString();
		this.L2 = this.Line2.text.ToString();
		this.L3 = this.Line3.text.ToString();
		this.L4 = this.Line4.text.ToString();
		this.L5 = this.Line5.text.ToString();
		this.L6 = this.Line6.text.ToString();
		this.L7 = this.Line7.text.ToString();
		this.L8 = this.Line8.text.ToString();
		this.L9 = this.Line9.text.ToString();
		this.L10 = this.Line10.text.ToString();
		this.L11 = this.Line11.text.ToString();
		this.L12 = this.Line12.text.ToString();
		this.L13 = this.Line13.text.ToString();
		this.L14 = this.Line14.text.ToString();
		this.L15 = this.Line15.text.ToString();
		this.L16 = this.Line16.text.ToString();
		this.L17 = this.Line17.text.ToString();
		this.L18 = this.Line18.text.ToString();
		this.L19 = this.Line19.text.ToString();
		this.L20 = this.Line20.text.ToString();
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x00091871 File Offset: 0x0008FA71
	private void OnDisable()
	{
		this.SetValue();
	}

	// Token: 0x040015D9 RID: 5593
	public InputField Line1;

	// Token: 0x040015DA RID: 5594
	public InputField Line2;

	// Token: 0x040015DB RID: 5595
	public InputField Line3;

	// Token: 0x040015DC RID: 5596
	public InputField Line4;

	// Token: 0x040015DD RID: 5597
	public InputField Line5;

	// Token: 0x040015DE RID: 5598
	public InputField Line6;

	// Token: 0x040015DF RID: 5599
	public InputField Line7;

	// Token: 0x040015E0 RID: 5600
	public InputField Line8;

	// Token: 0x040015E1 RID: 5601
	public InputField Line9;

	// Token: 0x040015E2 RID: 5602
	public InputField Line10;

	// Token: 0x040015E3 RID: 5603
	public InputField Line11;

	// Token: 0x040015E4 RID: 5604
	public InputField Line12;

	// Token: 0x040015E5 RID: 5605
	public InputField Line13;

	// Token: 0x040015E6 RID: 5606
	public InputField Line14;

	// Token: 0x040015E7 RID: 5607
	public InputField Line15;

	// Token: 0x040015E8 RID: 5608
	public InputField Line16;

	// Token: 0x040015E9 RID: 5609
	public InputField Line17;

	// Token: 0x040015EA RID: 5610
	public InputField Line18;

	// Token: 0x040015EB RID: 5611
	public InputField Line19;

	// Token: 0x040015EC RID: 5612
	public InputField Line20;

	// Token: 0x040015ED RID: 5613
	public string L1;

	// Token: 0x040015EE RID: 5614
	public string L2;

	// Token: 0x040015EF RID: 5615
	public string L3;

	// Token: 0x040015F0 RID: 5616
	public string L4;

	// Token: 0x040015F1 RID: 5617
	public string L5;

	// Token: 0x040015F2 RID: 5618
	public string L6;

	// Token: 0x040015F3 RID: 5619
	public string L7;

	// Token: 0x040015F4 RID: 5620
	public string L8;

	// Token: 0x040015F5 RID: 5621
	public string L9;

	// Token: 0x040015F6 RID: 5622
	public string L10;

	// Token: 0x040015F7 RID: 5623
	public string L11;

	// Token: 0x040015F8 RID: 5624
	public string L12;

	// Token: 0x040015F9 RID: 5625
	public string L13;

	// Token: 0x040015FA RID: 5626
	public string L14;

	// Token: 0x040015FB RID: 5627
	public string L15;

	// Token: 0x040015FC RID: 5628
	public string L16;

	// Token: 0x040015FD RID: 5629
	public string L17;

	// Token: 0x040015FE RID: 5630
	public string L18;

	// Token: 0x040015FF RID: 5631
	public string L19;

	// Token: 0x04001600 RID: 5632
	public string L20;
}
