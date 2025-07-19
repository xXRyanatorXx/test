using System;
using Mirror;
using Smooth;
using UnityEngine;

// Token: 0x02000271 RID: 625
public class SmoothSyncMirrorExamplePlayerController : NetworkBehaviour
{
	// Token: 0x06000EDF RID: 3807 RVA: 0x0009CF30 File Offset: 0x0009B130
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.rb2D = base.GetComponent<Rigidbody2D>();
		this.smoothSync = base.GetComponent<SmoothSyncMirror>();
		if (this.smoothSync)
		{
			this.smoothSync.validateStateMethod = new SmoothSyncMirror.validateStateDelegate(SmoothSyncMirrorExamplePlayerController.validateStateOfPlayer);
		}
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x0009CF88 File Offset: 0x0009B188
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (base.hasAuthority)
			{
				base.transform.position = base.transform.position + Vector3.right * 18f;
				this.smoothSync.teleportOwnedObjectFromOwner();
			}
			else if (NetworkServer.active)
			{
				this.smoothSync.teleportAnyObjectFromServer(base.transform.position + Vector3.right * 18f, base.transform.rotation, base.transform.localScale);
			}
		}
		if (!base.hasAuthority && (!NetworkServer.active || base.netIdentity.connectionToClient != null))
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			this.smoothSync.forceStateSendNextFixedUpdate();
		}
		Input.GetKeyDown(KeyCode.C);
		float d = this.transformMovementSpeed * Time.deltaTime;
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Equals))
		{
			base.transform.localScale = base.transform.localScale + new Vector3(1f, 1f, 1f) * d * 0.2f;
		}
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Minus))
		{
			base.transform.localScale = base.transform.localScale - new Vector3(1f, 1f, 1f) * d * 0.2f;
		}
		if (this.childObjectToControl)
		{
			if (Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.Equals))
			{
				this.childObjectToControl.transform.localScale = this.childObjectToControl.transform.localScale + new Vector3(1f, 1f, 1f) * d * 0.2f;
			}
			if (Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.Minus))
			{
				this.childObjectToControl.transform.localScale = this.childObjectToControl.transform.localScale - new Vector3(1f, 1f, 1f) * d * 0.2f;
			}
		}
		if (this.childObjectToControl)
		{
			if (Input.GetKey(KeyCode.S))
			{
				this.childObjectToControl.transform.position = this.childObjectToControl.transform.position + new Vector3(0f, -1.5f, -1f) * d;
			}
			if (Input.GetKey(KeyCode.W))
			{
				this.childObjectToControl.transform.position = this.childObjectToControl.transform.position + new Vector3(0f, 1.5f, 1f) * d;
			}
			if (Input.GetKey(KeyCode.A))
			{
				this.childObjectToControl.transform.position = this.childObjectToControl.transform.position + new Vector3(-1f, 0f, 0f) * d;
			}
			if (Input.GetKey(KeyCode.D))
			{
				this.childObjectToControl.transform.position = this.childObjectToControl.transform.position + new Vector3(1f, 0f, 0f) * d;
			}
		}
		if (this.rb)
		{
			if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				this.rb.velocity = Vector3.zero;
				this.rb.angularVelocity = Vector3.zero;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.rb.AddForce(new Vector3(0f, -1.5f, -1f) * this.rigidbodyMovementForce);
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.rb.AddForce(new Vector3(0f, 1.5f, 1f) * this.rigidbodyMovementForce);
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.rb.AddForce(new Vector3(-1f, 0f, 0f) * this.rigidbodyMovementForce);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.rb.AddForce(new Vector3(1f, 0f, 0f) * this.rigidbodyMovementForce);
				return;
			}
		}
		else if (this.rb2D)
		{
			if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				this.rb2D.velocity = Vector3.zero;
				this.rb2D.angularVelocity = 0f;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.rb2D.AddForce(new Vector3(0f, -1.5f, -1f) * this.rigidbodyMovementForce);
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				this.rb2D.AddForce(new Vector3(0f, 1.5f, 1f) * this.rigidbodyMovementForce);
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.rb2D.AddForce(new Vector3(-1f, 0f, 0f) * this.rigidbodyMovementForce);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.rb2D.AddForce(new Vector3(1f, 0f, 0f) * this.rigidbodyMovementForce);
				return;
			}
		}
		else
		{
			if (Input.GetKey(KeyCode.DownArrow))
			{
				base.transform.position = base.transform.position + new Vector3(0f, 0f, -1f) * d;
			}
			if (Input.GetKey(KeyCode.UpArrow))
			{
				base.transform.position = base.transform.position + new Vector3(0f, 0f, 1f) * d;
			}
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				base.transform.position = base.transform.position + new Vector3(-1f, 0f, 0f) * d;
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				base.transform.position = base.transform.position + new Vector3(1f, 0f, 0f) * d;
			}
		}
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x0009D658 File Offset: 0x0009B858
	public static bool validateStateOfPlayer(StateMirror latestReceivedState, StateMirror latestValidatedState)
	{
		return Vector3.Distance(latestReceivedState.position, latestValidatedState.position) <= 9000f || latestReceivedState.ownerTimestamp - latestValidatedState.receivedOnServerTimestamp >= 0.5f;
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x0000245B File Offset: 0x0000065B
	private void MirrorProcessed()
	{
	}

	// Token: 0x0400181B RID: 6171
	private Rigidbody rb;

	// Token: 0x0400181C RID: 6172
	private Rigidbody2D rb2D;

	// Token: 0x0400181D RID: 6173
	private SmoothSyncMirror smoothSync;

	// Token: 0x0400181E RID: 6174
	public float transformMovementSpeed = 30f;

	// Token: 0x0400181F RID: 6175
	public float rigidbodyMovementForce = 500f;

	// Token: 0x04001820 RID: 6176
	public GameObject childObjectToControl;
}
