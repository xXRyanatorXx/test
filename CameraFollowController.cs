using System;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class CameraFollowController : MonoBehaviour
{
	// Token: 0x0600061C RID: 1564 RVA: 0x00030CA4 File Offset: 0x0002EEA4
	public void LookAtTarget()
	{
		Quaternion b = Quaternion.LookRotation(this.objectToFollow.position - base.transform.position, Vector3.up);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, b, this.lookSpeed * Time.deltaTime);
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x00030D00 File Offset: 0x0002EF00
	public void MoveToTarget()
	{
		Vector3 b = this.objectToFollow.position + this.objectToFollow.forward * this.offset.z + this.objectToFollow.right * this.offset.x + this.objectToFollow.up * this.offset.y;
		base.transform.position = Vector3.Lerp(base.transform.position, b, this.followSpeed * Time.deltaTime);
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00030DA1 File Offset: 0x0002EFA1
	private void FixedUpdate()
	{
		this.LookAtTarget();
		this.MoveToTarget();
	}

	// Token: 0x04000930 RID: 2352
	public Transform objectToFollow;

	// Token: 0x04000931 RID: 2353
	public Vector3 offset;

	// Token: 0x04000932 RID: 2354
	public float followSpeed = 10f;

	// Token: 0x04000933 RID: 2355
	public float lookSpeed = 10f;
}
