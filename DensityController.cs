using System;
using System.Collections;
using TurnTheGameOn.SimpleTrafficSystem;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000282 RID: 642
public class DensityController : MonoBehaviour
{
	// Token: 0x06000F1C RID: 3868 RVA: 0x0009DF60 File Offset: 0x0009C160
	private void Start()
	{
		this.poolSizeText.text = "Cars In Pool: " + AITrafficController.Instance.carsInPool.ToString();
		this.densitySlider.maxValue = (float)AITrafficController.Instance.carsInPool;
		this.densitySlider.value = (float)AITrafficController.Instance.density;
		this.SetDensity();
		base.StartCoroutine("UpdateText");
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x0009DFD0 File Offset: 0x0009C1D0
	public void SetDensity()
	{
		AITrafficController.Instance.density = (int)this.densitySlider.value;
		this.densityText.text = "Target Density: " + this.densitySlider.value.ToString();
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x0009E01B File Offset: 0x0009C21B
	public IEnumerator UpdateText()
	{
		for (;;)
		{
			yield return new WaitForSeconds(1f);
			this.activeText.text = "Cars Active: " + AITrafficController.Instance.currentDensity.ToString();
		}
		yield break;
	}

	// Token: 0x04001849 RID: 6217
	public Text poolSizeText;

	// Token: 0x0400184A RID: 6218
	public Text densityText;

	// Token: 0x0400184B RID: 6219
	public Text activeText;

	// Token: 0x0400184C RID: 6220
	public Slider densitySlider;
}
