using System;
using UnityEngine;

// Token: 0x020001FE RID: 510
public class stand : MonoBehaviour
{
	// Token: 0x06000BEE RID: 3054 RVA: 0x000841CC File Offset: 0x000823CC
	private void Update()
	{
		base.transform.position = this.seat.transform.TransformPoint(0f, 0f, 3f);
		if (tools.tool == 1)
		{
			this.distance = Vector3.Distance(this.gtout.transform.position, this.tempParent.transform.position);
		}
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00084238 File Offset: 0x00082438
	private void OnMouseDown()
	{
		if (tools.tool == 1 && tools.sitting)
		{
			this.player.GetComponent<Rigidbody>().useGravity = true;
			this.player.GetComponent<Rigidbody>().detectCollisions = true;
			this.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
			this.player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			this.player.transform.SetParent(null);
			UnityEngine.Object.Destroy(this.player.GetComponent<FixedJoint>());
			this.player.transform.position = this.tempParent.transform.position;
			tools.sitting = false;
			this.Camera.GetComponent<carcam>().enabled = false;
			this.Camera.transform.SetParent(this.CameraParent.transform);
			this.Camera.transform.position = this.CameraParent.transform.position;
			this.Camera.transform.rotation = this.CameraParent.transform.rotation;
			GameObject.Find("FirstPerson-AIO").GetComponent<FirstPersonAIO>().enabled = true;
			this.seat.transform.parent.GetComponent<Carcontroll2>().enabled = false;
		}
	}

	// Token: 0x0400148C RID: 5260
	private Vector3 objectPos;

	// Token: 0x0400148D RID: 5261
	private float distance;

	// Token: 0x0400148E RID: 5262
	public GameObject gtout;

	// Token: 0x0400148F RID: 5263
	public GameObject player;

	// Token: 0x04001490 RID: 5264
	public GameObject tempParent;

	// Token: 0x04001491 RID: 5265
	public GameObject Camera;

	// Token: 0x04001492 RID: 5266
	public GameObject CameraParent;

	// Token: 0x04001493 RID: 5267
	public GameObject seat;
}
