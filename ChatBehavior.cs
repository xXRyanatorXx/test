using System;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000130 RID: 304
public class ChatBehavior : NetworkBehaviour
{
	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06000670 RID: 1648 RVA: 0x000346F0 File Offset: 0x000328F0
	// (remove) Token: 0x06000671 RID: 1649 RVA: 0x00034724 File Offset: 0x00032924
	private static event Action<string> OnMessage;

	// Token: 0x06000672 RID: 1650 RVA: 0x00034757 File Offset: 0x00032957
	public override void OnStartAuthority()
	{
		this.canvas.SetActive(true);
		ChatBehavior.OnMessage += this.HandleNewMessage;
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00034776 File Offset: 0x00032976
	[ClientCallback]
	private void OnDestroy()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		if (!base.hasAuthority)
		{
			return;
		}
		ChatBehavior.OnMessage -= this.HandleNewMessage;
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0003479D File Offset: 0x0003299D
	private void HandleNewMessage(string message)
	{
		Text text = this.chatText;
		text.text += message;
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x000347B8 File Offset: 0x000329B8
	[Client]
	public void Send()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ChatBehavior::Send()' called when client was not active");
			return;
		}
		if (!Input.GetKeyDown(KeyCode.Return))
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(this.inputField.text))
		{
			return;
		}
		this.CmdSendMessage(this.inputField.text);
		this.inputField.text = string.Empty;
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x00034818 File Offset: 0x00032A18
	[Command]
	private void CmdSendMessage(string message)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteString(message);
		base.SendCommandInternal("System.Void ChatBehavior::CmdSendMessage(System.String)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x00034850 File Offset: 0x00032A50
	[ClientRpc]
	private void RpcHandleMessage(string message)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteString(message);
		this.SendRPCInternal("System.Void ChatBehavior::RpcHandleMessage(System.String)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0000245B File Offset: 0x0000065B
	private void MirrorProcessed()
	{
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0003488D File Offset: 0x00032A8D
	protected void UserCode_CmdSendMessage__String(string message)
	{
		this.RpcHandleMessage(string.Format("[{0}]: {1}", base.connectionToClient.connectionId, message));
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x000348B0 File Offset: 0x00032AB0
	protected static void InvokeUserCode_CmdSendMessage__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSendMessage called on client.");
			return;
		}
		((ChatBehavior)obj).UserCode_CmdSendMessage__String(reader.ReadString());
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x000348D9 File Offset: 0x00032AD9
	protected void UserCode_RpcHandleMessage__String(string message)
	{
		Action<string> onMessage = ChatBehavior.OnMessage;
		if (onMessage == null)
		{
			return;
		}
		onMessage("\n" + message);
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x000348F5 File Offset: 0x00032AF5
	protected static void InvokeUserCode_RpcHandleMessage__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcHandleMessage called on server.");
			return;
		}
		((ChatBehavior)obj).UserCode_RpcHandleMessage__String(reader.ReadString());
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x00034920 File Offset: 0x00032B20
	static ChatBehavior()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(ChatBehavior), "System.Void ChatBehavior::CmdSendMessage(System.String)", new RemoteCallDelegate(ChatBehavior.InvokeUserCode_CmdSendMessage__String), true);
		RemoteProcedureCalls.RegisterRpc(typeof(ChatBehavior), "System.Void ChatBehavior::RpcHandleMessage(System.String)", new RemoteCallDelegate(ChatBehavior.InvokeUserCode_RpcHandleMessage__String));
	}

	// Token: 0x040009E2 RID: 2530
	[SerializeField]
	private Text chatText;

	// Token: 0x040009E3 RID: 2531
	[SerializeField]
	private InputField inputField;

	// Token: 0x040009E4 RID: 2532
	[SerializeField]
	private GameObject canvas;
}
