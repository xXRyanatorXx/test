using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000229 RID: 553
public class GearProfile : MonoBehaviour
{
	// Token: 0x04001599 RID: 5529
	public int Profilenumber;

	// Token: 0x0400159A RID: 5530
	public List<float> forwardGears = new List<float>
	{
		8f,
		5.5f,
		4f,
		3f,
		2.2f,
		1.7f,
		1.3f
	};

	// Token: 0x0400159B RID: 5531
	public List<float> reverseGears = new List<float>
	{
		-5f
	};
}
