using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200024B RID: 587
public class CarSimulator : MonoBehaviour
{
	// Token: 0x06000DDD RID: 3549 RVA: 0x000945C3 File Offset: 0x000927C3
	private void Start()
	{
		this.rpm = this.idle;
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x000945D4 File Offset: 0x000927D4
	private void Update()
	{
		if (this.gasPedalPressing)
		{
			if (this.rpm <= this.maxRPM)
			{
				this.rpm = Mathf.Lerp(this.rpm, this.rpm + this.accelerationSpeed * this.accelSlider.value, Time.deltaTime);
				return;
			}
		}
		else if (this.rpm > this.idle)
		{
			this.rpm = Mathf.Lerp(this.rpm, this.rpm - this.decelerationSpeed * this.accelSlider.value, Time.deltaTime);
		}
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x00094664 File Offset: 0x00092864
	public void onPointerDownRaceButton()
	{
		this.gasPedalPressing = true;
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x0009466D File Offset: 0x0009286D
	public void onPointerUpRaceButton()
	{
		this.gasPedalPressing = false;
	}

	// Token: 0x04001685 RID: 5765
	public bool gasPedalPressing;

	// Token: 0x04001686 RID: 5766
	public float maxRPM = 7000f;

	// Token: 0x04001687 RID: 5767
	public float idle = 900f;

	// Token: 0x04001688 RID: 5768
	public float rpm;

	// Token: 0x04001689 RID: 5769
	public float accelerationSpeed = 1000f;

	// Token: 0x0400168A RID: 5770
	public float decelerationSpeed = 1200f;

	// Token: 0x0400168B RID: 5771
	public Slider accelSlider;
}
