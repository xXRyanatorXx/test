using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200028F RID: 655
public class diagnosticsInfo : MonoBehaviour
{
	// Token: 0x06000F54 RID: 3924 RVA: 0x0009EF9C File Offset: 0x0009D19C
	private void OnEnable()
	{
		this.car = base.transform.parent.GetComponent<CarInformation>().Car.GetComponent<MainCarProperties>();
		if (this.car.HP > 0f)
		{
			this.HP.text = Mathf.Round(this.car.HP).ToString() + " HP";
		}
		this.startpr.text = this.car.CantCrank;
		this.runpr.text = this.car.CantRun;
		this.brakepr.text = this.car.BrakeProblems;
		this.engine.text = this.car.RuinedEngineParts;
		this.engineW.text = this.car.WornEngineParts;
		this.suspW.text = this.car.WornSuspensionParts;
		this.susp.text = this.car.RuinedSuspensionParts;
		this.brakes.text = this.car.RuinedBrakeParts;
		this.brakesW.text = this.car.WornBrakeParts;
		this.other.text = this.car.RuinedOtherParts;
		this.otherW.text = this.car.WornOtherparts;
	}

	// Token: 0x04001884 RID: 6276
	public Text HP;

	// Token: 0x04001885 RID: 6277
	public Text startpr;

	// Token: 0x04001886 RID: 6278
	public Text runpr;

	// Token: 0x04001887 RID: 6279
	public Text brakepr;

	// Token: 0x04001888 RID: 6280
	public Text engine;

	// Token: 0x04001889 RID: 6281
	public Text susp;

	// Token: 0x0400188A RID: 6282
	public Text brakes;

	// Token: 0x0400188B RID: 6283
	public Text other;

	// Token: 0x0400188C RID: 6284
	public Text engineW;

	// Token: 0x0400188D RID: 6285
	public Text suspW;

	// Token: 0x0400188E RID: 6286
	public Text brakesW;

	// Token: 0x0400188F RID: 6287
	public Text otherW;

	// Token: 0x04001890 RID: 6288
	public MainCarProperties car;
}
