using System;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000059 RID: 89
[AddComponentMenu("Enviro/Integration/Mirror Player")]
[RequireComponent(typeof(NetworkIdentity))]
public class EnviroMirrorPlayer : NetworkBehaviour
{
	// Token: 0x0600017B RID: 379 RVA: 0x0000BCBC File Offset: 0x00009EBC
	public void Start()
	{
		this.Player = GameObject.Find("Player");
		this.PlayerCamera = GameObject.Find("Player Camera").GetComponent<Camera>();
		if (!base.isLocalPlayer && !base.isServer)
		{
			base.enabled = false;
			return;
		}
		if (this.PlayerCamera == null && this.findSceneCamera)
		{
			this.PlayerCamera = Camera.main;
		}
		if (base.isLocalPlayer)
		{
			if (this.assignOnStart && this.Player != null && this.PlayerCamera != null)
			{
				EnviroSkyMgr.instance.AssignAndStart(this.Player, this.PlayerCamera);
			}
			this.Cmd_RequestSeason();
			this.Cmd_RequestCurrentWeather();
		}
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000BD78 File Offset: 0x00009F78
	[Command]
	private void Cmd_RequestSeason()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void EnviroMirrorPlayer::Cmd_RequestSeason()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000BDA4 File Offset: 0x00009FA4
	[ClientRpc]
	private void RpcRequestSeason(int season)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(season);
		this.SendRPCInternal("System.Void EnviroMirrorPlayer::RpcRequestSeason(System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000BDDC File Offset: 0x00009FDC
	[Command]
	private void Cmd_RequestCurrentWeather()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void EnviroMirrorPlayer::Cmd_RequestCurrentWeather()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000BE08 File Offset: 0x0000A008
	[ClientRpc]
	private void RpcRequestCurrentWeather(int weather, int zone)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(weather);
		writer.WriteInt(zone);
		this.SendRPCInternal("System.Void EnviroMirrorPlayer::RpcRequestCurrentWeather(System.Int32,System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000245B File Offset: 0x0000065B
	private void MirrorProcessed()
	{
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000BE5D File Offset: 0x0000A05D
	protected void UserCode_Cmd_RequestSeason()
	{
		this.RpcRequestSeason((int)EnviroSkyMgr.instance.GetCurrentSeason());
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000BE6F File Offset: 0x0000A06F
	protected static void InvokeUserCode_Cmd_RequestSeason(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command Cmd_RequestSeason called on client.");
			return;
		}
		((EnviroMirrorPlayer)obj).UserCode_Cmd_RequestSeason();
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0000BE92 File Offset: 0x0000A092
	protected void UserCode_RpcRequestSeason__Int32(int season)
	{
		EnviroSkyMgr.instance.ChangeSeason((EnviroSeasons.Seasons)season);
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0000BE9F File Offset: 0x0000A09F
	protected static void InvokeUserCode_RpcRequestSeason__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRequestSeason called on server.");
			return;
		}
		((EnviroMirrorPlayer)obj).UserCode_RpcRequestSeason__Int32(reader.ReadInt());
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000BEC8 File Offset: 0x0000A0C8
	protected void UserCode_Cmd_RequestCurrentWeather()
	{
		for (int i = 0; i < EnviroSkyMgr.instance.Weather.zones.Count; i++)
		{
			for (int j = 0; j < EnviroSkyMgr.instance.Weather.WeatherPrefabs.Count; j++)
			{
				if (EnviroSkyMgr.instance.Weather.WeatherPrefabs[j] == EnviroSkyMgr.instance.Weather.zones[i].currentActiveZoneWeatherPrefab)
				{
					this.RpcRequestCurrentWeather(j, i);
				}
			}
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000BF51 File Offset: 0x0000A151
	protected static void InvokeUserCode_Cmd_RequestCurrentWeather(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command Cmd_RequestCurrentWeather called on client.");
			return;
		}
		((EnviroMirrorPlayer)obj).UserCode_Cmd_RequestCurrentWeather();
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000BF74 File Offset: 0x0000A174
	protected void UserCode_RpcRequestCurrentWeather__Int32__Int32(int weather, int zone)
	{
		EnviroSkyMgr.instance.ChangeZoneWeather(zone, weather);
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000BF82 File Offset: 0x0000A182
	protected static void InvokeUserCode_RpcRequestCurrentWeather__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRequestCurrentWeather called on server.");
			return;
		}
		((EnviroMirrorPlayer)obj).UserCode_RpcRequestCurrentWeather__Int32__Int32(reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000BFB4 File Offset: 0x0000A1B4
	static EnviroMirrorPlayer()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(EnviroMirrorPlayer), "System.Void EnviroMirrorPlayer::Cmd_RequestSeason()", new RemoteCallDelegate(EnviroMirrorPlayer.InvokeUserCode_Cmd_RequestSeason), true);
		RemoteProcedureCalls.RegisterCommand(typeof(EnviroMirrorPlayer), "System.Void EnviroMirrorPlayer::Cmd_RequestCurrentWeather()", new RemoteCallDelegate(EnviroMirrorPlayer.InvokeUserCode_Cmd_RequestCurrentWeather), true);
		RemoteProcedureCalls.RegisterRpc(typeof(EnviroMirrorPlayer), "System.Void EnviroMirrorPlayer::RpcRequestSeason(System.Int32)", new RemoteCallDelegate(EnviroMirrorPlayer.InvokeUserCode_RpcRequestSeason__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(EnviroMirrorPlayer), "System.Void EnviroMirrorPlayer::RpcRequestCurrentWeather(System.Int32,System.Int32)", new RemoteCallDelegate(EnviroMirrorPlayer.InvokeUserCode_RpcRequestCurrentWeather__Int32__Int32));
	}

	// Token: 0x0400020B RID: 523
	public bool assignOnStart = true;

	// Token: 0x0400020C RID: 524
	public bool findSceneCamera = true;

	// Token: 0x0400020D RID: 525
	public GameObject Player;

	// Token: 0x0400020E RID: 526
	public Camera PlayerCamera;
}
