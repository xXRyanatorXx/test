using System;

// Token: 0x020000CA RID: 202
public class EnviroHaltonSequence
{
	// Token: 0x0600043C RID: 1084 RVA: 0x00020F40 File Offset: 0x0001F140
	public float Get()
	{
		float num = 0f;
		float num2 = 1f / (float)this.radix;
		int i = this.storedIndex;
		while (i > 0)
		{
			num += (float)(i % this.radix) * num2;
			i /= this.radix;
			num2 /= (float)this.radix;
		}
		this.storedIndex++;
		return num;
	}

	// Token: 0x040005D0 RID: 1488
	public int radix = 3;

	// Token: 0x040005D1 RID: 1489
	private int storedIndex;
}
