using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class FollowPlayerAI : MonoBehaviour
{
	// Token: 0x0600006C RID: 108 RVA: 0x00006D07 File Offset: 0x00004F07
	public void Start()
	{
		base.transform.GetComponent<Camera>().nearClipPlane = 0.2f;
		this.cameraTarget = this.camerasPlaces[0];
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00006D2C File Offset: 0x00004F2C
	private void FixedUpdate()
	{
		if (Input.GetKeyDown("c"))
		{
			this.i++;
			if (this.i >= this.camerasPlaces.Length)
			{
				this.i = 0;
			}
		}
		this.cameraTarget = this.camerasPlaces[this.i];
		Vector3 b = this.cameraTarget.position + this.dist;
		Vector3 position = Vector3.Lerp(base.transform.position, b, this.sSpeed * Time.deltaTime);
		base.transform.position = position;
		base.transform.LookAt(this.lookTarget.position);
	}

	// Token: 0x040000EA RID: 234
	private Transform cameraTarget;

	// Token: 0x040000EB RID: 235
	[HideInInspector]
	public float sSpeed = 10f;

	// Token: 0x040000EC RID: 236
	[HideInInspector]
	public Vector3 dist;

	// Token: 0x040000ED RID: 237
	public Transform lookTarget;

	// Token: 0x040000EE RID: 238
	public Transform[] camerasPlaces;

	// Token: 0x040000EF RID: 239
	private int i;
}
