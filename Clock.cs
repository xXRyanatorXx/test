using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class Clock : MonoBehaviour
{
	// Token: 0x060000C8 RID: 200 RVA: 0x00008BF8 File Offset: 0x00006DF8
	private void Start()
	{
		if (this.realTime)
		{
			this.hour = DateTime.Now.Hour;
			this.minutes = DateTime.Now.Minute;
			this.seconds = DateTime.Now.Second;
		}
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00008C48 File Offset: 0x00006E48
	private void Update()
	{
		this.msecs += Time.deltaTime * this.clockSpeed;
		if (this.msecs >= 1f)
		{
			this.msecs -= 1f;
			this.seconds++;
			if (this.seconds >= 60)
			{
				this.seconds = 0;
				this.minutes++;
				if (this.minutes > 60)
				{
					this.minutes = 0;
					this.hour++;
					if (this.hour >= 24)
					{
						this.hour = 0;
					}
				}
			}
		}
		float z = 6f * (float)this.seconds;
		float z2 = 6f * (float)this.minutes;
		float z3 = 30f * (float)this.hour + 0.5f * (float)this.minutes;
		this.pointerSeconds.transform.localEulerAngles = new Vector3(0f, 0f, z);
		this.pointerMinutes.transform.localEulerAngles = new Vector3(0f, 0f, z2);
		this.pointerHours.transform.localEulerAngles = new Vector3(0f, 0f, z3);
	}

	// Token: 0x04000146 RID: 326
	public int minutes;

	// Token: 0x04000147 RID: 327
	public int hour;

	// Token: 0x04000148 RID: 328
	public int seconds;

	// Token: 0x04000149 RID: 329
	public bool realTime = true;

	// Token: 0x0400014A RID: 330
	public GameObject pointerSeconds;

	// Token: 0x0400014B RID: 331
	public GameObject pointerMinutes;

	// Token: 0x0400014C RID: 332
	public GameObject pointerHours;

	// Token: 0x0400014D RID: 333
	public float clockSpeed = 1f;

	// Token: 0x0400014E RID: 334
	private float msecs;
}
