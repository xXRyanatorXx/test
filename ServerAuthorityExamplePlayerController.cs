using System;
using Mirror;
using Mirror.RemoteCalls;
using Smooth;
using UnityEngine;

// Token: 0x02000270 RID: 624
public class ServerAuthorityExamplePlayerController : NetworkBehaviour
{
	// Token: 0x06000ED3 RID: 3795 RVA: 0x0009CC41 File Offset: 0x0009AE41
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.smoothSync = base.GetComponent<SmoothSyncMirror>();
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x0009CC5B File Offset: 0x0009AE5B
	public override void OnStartServer()
	{
		this.rb.isKinematic = false;
		base.OnStartServer();
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x0009CC70 File Offset: 0x0009AE70
	private void Update()
	{
		if (base.hasAuthority)
		{
			if (Input.GetKeyUp(KeyCode.DownArrow))
			{
				this.CmdMove(KeyCode.DownArrow);
			}
			if (Input.GetKeyUp(KeyCode.UpArrow))
			{
				this.CmdMove(KeyCode.UpArrow);
			}
			if (Input.GetKeyUp(KeyCode.LeftArrow))
			{
				this.CmdMove(KeyCode.LeftArrow);
			}
			if (Input.GetKeyUp(KeyCode.RightArrow))
			{
				this.CmdMove(KeyCode.RightArrow);
			}
			if (Input.GetKeyUp(KeyCode.T))
			{
				this.CmdTeleport();
			}
		}
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0009CCF0 File Offset: 0x0009AEF0
	[Command]
	private void CmdTeleport()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void ServerAuthorityExamplePlayerController::CmdTeleport()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x0009CD1C File Offset: 0x0009AF1C
	[Command]
	private void CmdMove(KeyCode keyCode)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		Mirror.GeneratedNetworkCode._Write_UnityEngine.KeyCode(writer, keyCode);
		base.SendCommandInternal("System.Void ServerAuthorityExamplePlayerController::CmdMove(UnityEngine.KeyCode)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x0000245B File Offset: 0x0000065B
	private void MirrorProcessed()
	{
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x0009CD70 File Offset: 0x0009AF70
	protected void UserCode_CmdTeleport()
	{
		this.smoothSync.teleportAnyObjectFromServer(base.transform.position + Vector3.right * 5f, base.transform.rotation, base.transform.localScale);
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x0009CDBD File Offset: 0x0009AFBD
	protected static void InvokeUserCode_CmdTeleport(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTeleport called on client.");
			return;
		}
		((ServerAuthorityExamplePlayerController)obj).UserCode_CmdTeleport();
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0009CDE0 File Offset: 0x0009AFE0
	protected void UserCode_CmdMove__KeyCode(KeyCode keyCode)
	{
		switch (keyCode)
		{
		case KeyCode.UpArrow:
			this.rb.AddForce(new Vector3(0f, 1.5f, 1f) * this.rigidbodyMovementForce);
			return;
		case KeyCode.DownArrow:
			this.rb.AddForce(new Vector3(0f, -1.5f, -1f) * this.rigidbodyMovementForce);
			return;
		case KeyCode.RightArrow:
			this.rb.AddForce(new Vector3(1f, 0f, 0f) * this.rigidbodyMovementForce);
			return;
		case KeyCode.LeftArrow:
			this.rb.AddForce(new Vector3(-1f, 0f, 0f) * this.rigidbodyMovementForce);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x0009CEB5 File Offset: 0x0009B0B5
	protected static void InvokeUserCode_CmdMove__KeyCode(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdMove called on client.");
			return;
		}
		((ServerAuthorityExamplePlayerController)obj).UserCode_CmdMove__KeyCode(Mirror.GeneratedNetworkCode._Read_UnityEngine.KeyCode(reader));
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0009CEE0 File Offset: 0x0009B0E0
	static ServerAuthorityExamplePlayerController()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(ServerAuthorityExamplePlayerController), "System.Void ServerAuthorityExamplePlayerController::CmdTeleport()", new RemoteCallDelegate(ServerAuthorityExamplePlayerController.InvokeUserCode_CmdTeleport), true);
		RemoteProcedureCalls.RegisterCommand(typeof(ServerAuthorityExamplePlayerController), "System.Void ServerAuthorityExamplePlayerController::CmdMove(UnityEngine.KeyCode)", new RemoteCallDelegate(ServerAuthorityExamplePlayerController.InvokeUserCode_CmdMove__KeyCode), true);
	}

	// Token: 0x04001817 RID: 6167
	private Rigidbody rb;

	// Token: 0x04001818 RID: 6168
	public float transformMovementSpeed = 30f;

	// Token: 0x04001819 RID: 6169
	public float rigidbodyMovementForce = 500f;

	// Token: 0x0400181A RID: 6170
	private SmoothSyncMirror smoothSync;
}
