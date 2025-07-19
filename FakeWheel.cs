using System;
using UnityEngine;

// Token: 0x02000284 RID: 644
public class FakeWheel : MonoBehaviour
{
	// Token: 0x06000F26 RID: 3878 RVA: 0x0009E0B7 File Offset: 0x0009C2B7
	private void Awake()
	{
		this.fakeWheel = base.transform;
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x0009E0C8 File Offset: 0x0009C2C8
	private void Update()
	{
		this.newPosition = this.fakeWheel.position;
		this.newPosition.y = this.referenceWheel.position.y;
		this.fakeWheel.transform.SetPositionAndRotation(this.newPosition, this.referenceWheel.transform.rotation);
	}

	// Token: 0x04001850 RID: 6224
	public Transform referenceWheel;

	// Token: 0x04001851 RID: 6225
	private Transform fakeWheel;

	// Token: 0x04001852 RID: 6226
	private Vector3 newPosition;
}
