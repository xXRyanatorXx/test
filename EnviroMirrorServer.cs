using System;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x0200005A RID: 90
[AddComponentMenu("Enviro/Integration/Mirror Server Component")]
[RequireComponent(typeof(NetworkIdentity))]
public class EnviroMirrorServer : NetworkBehaviour
{
	// Token: 0x0600018B RID: 395 RVA: 0x0000C044 File Offset: 0x0000A244
	public override void OnStartServer()
	{
		if (this.isHeadless)
		{
			EnviroSkyMgr.instance.StartAsServer();
		}
		EnviroSkyMgr.instance.SetAutoWeatherUpdates(true);
		EnviroSkyMgr.instance.OnSeasonChanged += delegate(EnviroSeasons.Seasons season)
		{
			this.SendSeasonToClient(season);
		};
		EnviroSkyMgr.instance.OnZoneWeatherChanged += delegate(EnviroWeatherPreset type, EnviroZone zone)
		{
			this.SendWeatherToClient(type, zone);
		};
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000C09A File Offset: 0x0000A29A
	public void Start()
	{
		if (!base.isServer)
		{
			EnviroSkyMgr.instance.SetTimeProgress(EnviroTime.TimeProgressMode.None);
			EnviroSkyMgr.instance.SetAutoWeatherUpdates(false);
		}
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000C0BC File Offset: 0x0000A2BC
	private void SendWeatherToClient(EnviroWeatherPreset w, EnviroZone z)
	{
		int zone = 0;
		for (int i = 0; i < EnviroSkyMgr.instance.GetZoneList().Count; i++)
		{
			if (EnviroSkyMgr.instance.GetZoneList()[i] == z)
			{
				zone = i;
			}
		}
		for (int j = 0; j < EnviroSkyMgr.instance.GetCurrentWeatherPresetList().Count; j++)
		{
			if (EnviroSkyMgr.instance.GetCurrentWeatherPresetList()[j] == w)
			{
				this.RpcWeatherUpdate(j, zone);
			}
		}
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000C139 File Offset: 0x0000A339
	private void SendSeasonToClient(EnviroSeasons.Seasons s)
	{
		this.RpcSeasonUpdate((int)s);
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000C144 File Offset: 0x0000A344
	[ClientRpc]
	private void RpcSeasonUpdate(int season)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(season);
		this.SendRPCInternal("System.Void EnviroMirrorServer::RpcSeasonUpdate(System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000C17C File Offset: 0x0000A37C
	[ClientRpc]
	private void RpcWeatherUpdate(int weather, int zone)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(weather);
		writer.WriteInt(zone);
		this.SendRPCInternal("System.Void EnviroMirrorServer::RpcWeatherUpdate(System.Int32,System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000C1BC File Offset: 0x0000A3BC
	private void Update()
	{
		if (EnviroSkyMgr.instance == null)
		{
			return;
		}
		if (!base.isServer)
		{
			if (this.networkHours < 1f && EnviroSkyMgr.instance.GetUniversalTimeOfDay() > 23f)
			{
				EnviroSkyMgr.instance.SetTimeOfDay(this.networkHours);
			}
			EnviroSkyMgr.instance.SetTimeOfDay(Mathf.Lerp(EnviroSkyMgr.instance.GetUniversalTimeOfDay(), this.networkHours, Time.deltaTime * this.updateSmoothing));
			EnviroSkyMgr.instance.SetYears(this.networkYears);
			EnviroSkyMgr.instance.SetDays(this.networkDays);
			return;
		}
		this.NetworknetworkHours = EnviroSkyMgr.instance.GetUniversalTimeOfDay();
		this.NetworknetworkDays = EnviroSkyMgr.instance.GetCurrentDay();
		this.NetworknetworkYears = EnviroSkyMgr.instance.GetCurrentYear();
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000245B File Offset: 0x0000065B
	private void MirrorProcessed()
	{
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000196 RID: 406 RVA: 0x0000C2B8 File Offset: 0x0000A4B8
	// (set) Token: 0x06000197 RID: 407 RVA: 0x0000C2CB File Offset: 0x0000A4CB
	public float NetworknetworkHours
	{
		get
		{
			return this.networkHours;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<float>(value, ref this.networkHours, 1UL, null);
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000198 RID: 408 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
	// (set) Token: 0x06000199 RID: 409 RVA: 0x0000C2FB File Offset: 0x0000A4FB
	public int NetworknetworkDays
	{
		get
		{
			return this.networkDays;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.networkDays, 2UL, null);
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x0600019A RID: 410 RVA: 0x0000C318 File Offset: 0x0000A518
	// (set) Token: 0x0600019B RID: 411 RVA: 0x0000C32B File Offset: 0x0000A52B
	public int NetworknetworkYears
	{
		get
		{
			return this.networkYears;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.networkYears, 4UL, null);
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000BE92 File Offset: 0x0000A092
	protected void UserCode_RpcSeasonUpdate__Int32(int season)
	{
		EnviroSkyMgr.instance.ChangeSeason((EnviroSeasons.Seasons)season);
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000C345 File Offset: 0x0000A545
	protected static void InvokeUserCode_RpcSeasonUpdate__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSeasonUpdate called on server.");
			return;
		}
		((EnviroMirrorServer)obj).UserCode_RpcSeasonUpdate__Int32(reader.ReadInt());
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000BF74 File Offset: 0x0000A174
	protected void UserCode_RpcWeatherUpdate__Int32__Int32(int weather, int zone)
	{
		EnviroSkyMgr.instance.ChangeZoneWeather(zone, weather);
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000C36E File Offset: 0x0000A56E
	protected static void InvokeUserCode_RpcWeatherUpdate__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcWeatherUpdate called on server.");
			return;
		}
		((EnviroMirrorServer)obj).UserCode_RpcWeatherUpdate__Int32__Int32(reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000C3A0 File Offset: 0x0000A5A0
	static EnviroMirrorServer()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(EnviroMirrorServer), "System.Void EnviroMirrorServer::RpcSeasonUpdate(System.Int32)", new RemoteCallDelegate(EnviroMirrorServer.InvokeUserCode_RpcSeasonUpdate__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(EnviroMirrorServer), "System.Void EnviroMirrorServer::RpcWeatherUpdate(System.Int32,System.Int32)", new RemoteCallDelegate(EnviroMirrorServer.InvokeUserCode_RpcWeatherUpdate__Int32__Int32));
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000C3F0 File Offset: 0x0000A5F0
	public override bool SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(this.networkHours);
			writer.WriteInt(this.networkDays);
			writer.WriteInt(this.networkYears);
			return true;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteFloat(this.networkHours);
			result = true;
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteInt(this.networkDays);
			result = true;
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteInt(this.networkYears);
			result = true;
		}
		return result;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000C4AC File Offset: 0x0000A6AC
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<float>(ref this.networkHours, null, reader.ReadFloat());
			base.GeneratedSyncVarDeserialize<int>(ref this.networkDays, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.networkYears, null, reader.ReadInt());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<float>(ref this.networkHours, null, reader.ReadFloat());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.networkDays, null, reader.ReadInt());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.networkYears, null, reader.ReadInt());
		}
	}

	// Token: 0x0400020F RID: 527
	public float updateSmoothing = 15f;

	// Token: 0x04000210 RID: 528
	[SyncVar]
	private float networkHours;

	// Token: 0x04000211 RID: 529
	[SyncVar]
	private int networkDays;

	// Token: 0x04000212 RID: 530
	[SyncVar]
	private int networkYears;

	// Token: 0x04000213 RID: 531
	public bool isHeadless = true;
}
