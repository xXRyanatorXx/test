using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using Assets.SimpleZip;
using FileUtilities;
using Mirror;
using Mirror.RemoteCalls;
using NWH.VehiclePhysics2;
using PaintIn3D;
using UnityEngine;
using UnityEngine.Animations.Rigging;

// Token: 0x0200022C RID: 556
public class NetworkPLayer : NetworkBehaviour
{
	// Token: 0x06000CC8 RID: 3272 RVA: 0x0008D2C8 File Offset: 0x0008B4C8
	private void Start()
	{
		if (base.GetComponent<NetworkIdentity>().netId == 1U)
		{
			this.Avatar = this.Avatar1;
		}
		else
		{
			this.Avatar = this.Avatar2;
		}
		this.Avatar.SetActive(true);
		this.m_Animator = this.Avatar.GetComponent<Animator>();
		this.LHRig = this.Avatar.gameObject.transform.Find("RigLeftArm").GetComponent<Rig>();
		this.RHRig = this.Avatar.gameObject.transform.Find("RigRightArm").GetComponent<Rig>();
		this.previous = base.transform.position;
		this.CarList = GameObject.Find("CarsParent");
		this.Cars = this.CarList.GetComponent<CarList>().Cars;
		this.Saver = GameObject.Find("SaveManager").GetComponent<Saver>();
		if (base.GetComponent<NetworkIdentity>().isLocalPlayer)
		{
			this.Avatar.SetActive(false);
		}
		this.RealPlayer = GameObject.Find("Player").transform.GetChild(1).gameObject;
		if (base.isLocalPlayer)
		{
			tools.NetworkPLayer = this;
		}
		if (Directory.Exists(Application.persistentDataPath + "/Multiplayer"))
		{
			Directory.Delete(Application.persistentDataPath + "/Multiplayer", true);
		}
		Directory.CreateDirectory(Application.persistentDataPath + "/Multiplayer");
		if (base.isServer && base.isLocalPlayer)
		{
			this.StartSceneObjects();
		}
		if (!base.isServer && base.isLocalPlayer)
		{
			this.ClientJoined();
			this.RemoveStartParts();
			this.CmdLoadScene();
		}
		if (base.isServer && base.isLocalPlayer)
		{
			tools.MPrunning = true;
		}
		this.JM = GameObject.Find("Player").GetComponent<tools>().JM;
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x0008D4A0 File Offset: 0x0008B6A0
	private void FixedUpdate()
	{
		float a = this.walkSpeed;
		this.walkSpeed = Vector3.Distance(base.transform.position, this.previous) / Time.deltaTime;
		this.walkSpeed = Mathf.Lerp(a, this.walkSpeed, Time.deltaTime * 4f);
		this.previous = base.transform.position;
		this.m_Animator.SetFloat("Speed", this.walkSpeed);
		if (!base.isLocalPlayer)
		{
			return;
		}
		base.transform.position = this.RealPlayer.transform.position;
		base.transform.rotation = this.RealPlayer.transform.rotation;
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x0008D55C File Offset: 0x0008B75C
	private void Update()
	{
		if (!base.isLocalPlayer)
		{
			return;
		}
		base.transform.position = this.RealPlayer.transform.position;
		base.transform.rotation = this.RealPlayer.transform.rotation;
		if (!tools.sitting && Input.GetKeyDown(KeyCode.F9))
		{
			SaveSystem saveSystem = new SaveSystem(Application.persistentDataPath + "/save1/Car.dat");
			saveSystem.read();
			this.CarName = (string)saveSystem.get("MPCar", "LAD");
			this.CarSave = File.ReadAllBytes(Application.persistentDataPath + "/save1/Car.dat");
			this.CarSave = Zip.Compress(this.CarSave);
			this.CmdSpawn(this.CarName);
			this.SaveName = "Car" + base.GetComponent<NetworkIdentity>().netId.ToString() + ".dat";
			this.CmdCarSave(this.CarSave, this.SaveName);
		}
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x0008D66C File Offset: 0x0008B86C
	private void OnDestroy()
	{
		this.ClientLoaded();
		tools.MPrunning = false;
		MPobject[] array = UnityEngine.Object.FindObjectsOfType<MPobject>();
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i]);
		}
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x0008D6A1 File Offset: 0x0008B8A1
	public void ClientJoined()
	{
		this.CmdClientJoined();
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x0008D6AC File Offset: 0x0008B8AC
	[Command]
	private void CmdClientJoined()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void NetworkPLayer::CmdClientJoined()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CCE RID: 3278 RVA: 0x0008D6D8 File Offset: 0x0008B8D8
	[ClientRpc]
	private void RpcClientJoined()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void NetworkPLayer::RpcClientJoined()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x0008D703 File Offset: 0x0008B903
	public void ClientLoaded()
	{
		this.CmdClientLoaded();
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x0008D70C File Offset: 0x0008B90C
	[Command]
	private void CmdClientLoaded()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void NetworkPLayer::CmdClientLoaded()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x0008D738 File Offset: 0x0008B938
	[ClientRpc]
	private void RpcClientLoaded()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void NetworkPLayer::RpcClientLoaded()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x0008D764 File Offset: 0x0008B964
	[Command]
	private void CmdLoadScene()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void NetworkPLayer::CmdLoadScene()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x0008D78F File Offset: 0x0008B98F
	public IEnumerator LoadScene(bool additional)
	{
		if (Directory.Exists(Application.persistentDataPath + "/Multiplayer"))
		{
			Directory.Delete(Application.persistentDataPath + "/Multiplayer", true);
		}
		Directory.CreateDirectory(Application.persistentDataPath + "/Multiplayer");
		foreach (networkDummy networkDummy in UnityEngine.Object.FindObjectsOfType<networkDummy>())
		{
			if (networkDummy.Target != null)
			{
				networkDummy.transform.position = networkDummy.Target.transform.position;
			}
		}
		this.CreateSave();
		yield return new WaitForSeconds(2f);
		string[] files = Directory.GetFiles(Application.persistentDataPath + "/Multiplayer");
		if (files.Length != 0)
		{
			foreach (string file in files)
			{
				if (!this.PackageCame)
				{
					yield return new WaitForSeconds(1f);
				}
				byte[] data = File.ReadAllBytes(file);
				if (additional)
				{
					this.RpcReceiveAdditionalData(data, Path.GetFileName(file));
				}
				else
				{
					this.RpcReceiveData(data, Path.GetFileName(file));
				}
				if (!this.PackageCame)
				{
					yield return new WaitForSeconds(3f);
				}
				this.PackageCame = false;
				file = null;
			}
			string[] array2 = null;
			if (additional)
			{
				this.RpcLoadAdditionalScene();
			}
			else
			{
				this.RpcLoadScene();
			}
			this.PackageCame = false;
		}
		yield break;
	}

	// Token: 0x06000CD4 RID: 3284 RVA: 0x0008D7A8 File Offset: 0x0008B9A8
	[TargetRpc]
	private void RpcCleanFolder()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendTargetRPCInternal(null, "System.Void NetworkPLayer::RpcCleanFolder()", writer, 0);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CD5 RID: 3285 RVA: 0x0008D7D4 File Offset: 0x0008B9D4
	[TargetRpc]
	private void RpcReceiveData(byte[] Data, string SaveName)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteBytesAndSize(Data);
		writer.WriteString(SaveName);
		this.SendTargetRPCInternal(null, "System.Void NetworkPLayer::RpcReceiveData(System.Byte[],System.String)", writer, 0);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x0008D814 File Offset: 0x0008BA14
	[ClientRpc]
	private void RpcReceiveAdditionalData(byte[] Data, string SaveName)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteBytesAndSize(Data);
		writer.WriteString(SaveName);
		this.SendRPCInternal("System.Void NetworkPLayer::RpcReceiveAdditionalData(System.Byte[],System.String)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CD7 RID: 3287 RVA: 0x0008D854 File Offset: 0x0008BA54
	[Command]
	private void CmdPackegecame()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void NetworkPLayer::CmdPackegecame()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x0008D880 File Offset: 0x0008BA80
	[ClientRpc]
	private void RpcPackegecame()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void NetworkPLayer::RpcPackegecame()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x0008D8AC File Offset: 0x0008BAAC
	[TargetRpc]
	private void RpcLoadScene()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendTargetRPCInternal(null, "System.Void NetworkPLayer::RpcLoadScene()", writer, 0);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x0008D8D8 File Offset: 0x0008BAD8
	[ClientRpc]
	private void RpcLoadAdditionalScene()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void NetworkPLayer::RpcLoadAdditionalScene()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x0008D904 File Offset: 0x0008BB04
	public void AddMPobjects()
	{
		foreach (PickupTool pickupTool in UnityEngine.Object.FindObjectsOfType<PickupTool>())
		{
			if ((pickupTool.transform.name == "WelderHandle" || pickupTool.transform.name == "WaterHose" || pickupTool.transform.name == "FuelHose" || pickupTool.transform.name == "Nozzle") && !pickupTool.GetComponent<MPobject>())
			{
				pickupTool.gameObject.AddComponent<MPobject>();
			}
		}
		foreach (networkDummy networkDummy in UnityEngine.Object.FindObjectsOfType<networkDummy>())
		{
			if (networkDummy.MPNumber > tools.MPNumber)
			{
				tools.MPNumber = networkDummy.MPNumber;
			}
		}
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x0008D9D4 File Offset: 0x0008BBD4
	public void pickup(networkDummy networkDummy)
	{
		this.CmdPickupItem(networkDummy.GetComponent<NetworkIdentity>());
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x0008D9E4 File Offset: 0x0008BBE4
	[Command(requiresAuthority = false)]
	private void CmdPickupItem(NetworkIdentity NetworkIdentity)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteNetworkIdentity(NetworkIdentity);
		base.SendCommandInternal("System.Void NetworkPLayer::CmdPickupItem(Mirror.NetworkIdentity)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x0008DA1C File Offset: 0x0008BC1C
	[ClientRpc]
	private void RpcPickupItem(NetworkIdentity NetworkIdentity)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteNetworkIdentity(NetworkIdentity);
		this.SendRPCInternal("System.Void NetworkPLayer::RpcPickupItem(Mirror.NetworkIdentity)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x0008DA51 File Offset: 0x0008BC51
	public void putdown(networkDummy networkDummy)
	{
		this.Cmdputdown(networkDummy.GetComponent<NetworkIdentity>());
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x0008DA5F File Offset: 0x0008BC5F
	public void WaitPutdown(networkDummy networkDummy)
	{
		base.StartCoroutine(this.WaitPut(networkDummy));
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x0008DA6F File Offset: 0x0008BC6F
	private IEnumerator WaitPut(networkDummy networkDummy)
	{
		yield return new WaitForSeconds(2f);
		this.Cmdputdown(networkDummy.GetComponent<NetworkIdentity>());
		yield break;
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x0008DA88 File Offset: 0x0008BC88
	[Command]
	private void Cmdputdown(NetworkIdentity NetworkIdentity)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteNetworkIdentity(NetworkIdentity);
		base.SendCommandInternal("System.Void NetworkPLayer::Cmdputdown(Mirror.NetworkIdentity)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x0008DAC0 File Offset: 0x0008BCC0
	[ClientRpc]
	private void Rpcputdown(NetworkIdentity NetworkIdentity)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteNetworkIdentity(NetworkIdentity);
		this.SendRPCInternal("System.Void NetworkPLayer::Rpcputdown(Mirror.NetworkIdentity)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x0008DAF8 File Offset: 0x0008BCF8
	public void SpawnObject(int seed, bool newp)
	{
		this.Itemname = this.ITEM.name;
		if (this.ITEM.GetComponent<CarProperties>() && this.ITEM.GetComponent<CarProperties>().PrefabName != "")
		{
			tools.NetworkPLayer.Itemname = this.ITEM.GetComponent<CarProperties>().PrefabName;
		}
		else
		{
			tools.NetworkPLayer.Itemname = this.ITEM.name;
		}
		this.CmdSpawnItem(this.Itemname, this.Spawnposition, this.Spawnrotation, this.colorpaint, newp, seed);
		if (this.ITEM.GetComponent<CarProperties>())
		{
			foreach (PickupCup pickupCup in this.ITEM.GetComponentsInChildren<PickupCup>())
			{
				this.CmdSpawnItem("CUP", pickupCup.transform.position, pickupCup.transform.rotation, this.colorpaint, false, 0);
			}
		}
		if (this.ITEM.name == "Fuelcan")
		{
			foreach (PickupCup pickupCup2 in this.ITEM.GetComponentsInChildren<PickupCup>())
			{
				this.CmdSpawnItem("CUP", pickupCup2.transform.position, pickupCup2.transform.rotation, this.colorpaint, false, 0);
			}
		}
		if (this.ITEM.name == "Welder" || this.ITEM.name == "SandBlaster")
		{
			foreach (PickupTool pickupTool in this.ITEM.GetComponentsInChildren<PickupTool>())
			{
				this.CmdSpawnItem(pickupTool.transform.name, pickupTool.transform.position, pickupTool.transform.rotation, this.colorpaint, false, 0);
			}
		}
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x0000245B File Offset: 0x0000065B
	public void SpawnParts()
	{
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x0008DCE4 File Offset: 0x0008BEE4
	[Command]
	private void CmdSpawnItem(string Itemname, Vector3 Spawnposition, Quaternion Spawnrotation, Color Colorpaint, bool newp, int seed)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteString(Itemname);
		writer.WriteVector3(Spawnposition);
		writer.WriteQuaternion(Spawnrotation);
		writer.WriteColor(Colorpaint);
		writer.WriteBool(newp);
		writer.WriteInt(seed);
		base.SendCommandInternal("System.Void NetworkPLayer::CmdSpawnItem(System.String,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Color,System.Boolean,System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x0008DD4C File Offset: 0x0008BF4C
	[ClientRpc]
	private void RpcSpawnItem(string Itemname, Vector3 Spawnposition, Quaternion Spawnrotation, Color Colorpaint, bool newp, int seed, int mpnr)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteString(Itemname);
		writer.WriteVector3(Spawnposition);
		writer.WriteQuaternion(Spawnrotation);
		writer.WriteColor(Colorpaint);
		writer.WriteBool(newp);
		writer.WriteInt(seed);
		writer.WriteInt(mpnr);
		this.SendRPCInternal("System.Void NetworkPLayer::RpcSpawnItem(System.String,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Color,System.Boolean,System.Int32,System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0008DDC0 File Offset: 0x0008BFC0
	private void SpawnStartObjects()
	{
		this.Spawnposition = this.ITEM.transform.position;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.DUMMY, this.ITEM.transform.position, this.ITEM.transform.rotation);
		if (this.ITEM.GetComponent<PickupCup>())
		{
			gameObject.GetComponent<networkDummy>().NetworkItemname = "CUP";
			if (this.ITEM.transform.parent && this.ITEM.transform.parent.parent && this.ITEM.transform.parent.parent.GetComponent<MPobject>())
			{
				gameObject.GetComponent<networkDummy>().NetworkCupMPNumber = this.ITEM.transform.parent.parent.GetComponent<MPobject>().MPNumber;
			}
			else if (this.ITEM.transform.parent && this.ITEM.transform.parent.GetComponent<MPobject>())
			{
				gameObject.GetComponent<networkDummy>().NetworkCupMPNumber = this.ITEM.transform.parent.GetComponent<MPobject>().MPNumber;
			}
		}
		else
		{
			gameObject.GetComponent<networkDummy>().NetworkItemname = this.ITEM.transform.name;
		}
		gameObject.GetComponent<networkDummy>().NetworkSpawnposition = this.ITEM.transform.position;
		gameObject.GetComponent<networkDummy>().NetworkSpawnrotation = this.ITEM.transform.rotation;
		gameObject.GetComponent<networkDummy>().NetworkMPNumber = this.ITEM.GetComponent<MPobject>().MPNumber;
		if (this.ITEM.gameObject.GetComponent<SavePosition>())
		{
			gameObject.GetComponent<networkDummy>().NetworkSceneNumber = this.ITEM.gameObject.GetComponent<SavePosition>().SceneNumber;
		}
		NetworkServer.Spawn(gameObject, base.connectionToClient);
		gameObject.GetComponent<networkDummy>().Target = this.ITEM.gameObject;
		this.ITEM.gameObject.GetComponent<MPobject>().networkDummy = gameObject.GetComponent<networkDummy>();
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x0008DFF0 File Offset: 0x0008C1F0
	[Command]
	private void CmdSpawn(string CarName)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteString(CarName);
		base.SendCommandInternal("System.Void NetworkPLayer::CmdSpawn(System.String)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x0008E028 File Offset: 0x0008C228
	public void CreateSave()
	{
		this.Saver.Save(false, false, true);
		byte[] bytes = Zip.Compress(File.ReadAllBytes(Application.persistentDataPath + "/Multiplayer/save.dat"));
		File.WriteAllBytes(Application.persistentDataPath + "/Multiplayer/save.dat", bytes);
		new Discompose(3).SplitFileBySizeOfFiles(Application.persistentDataPath + "/Multiplayer/save.dat", 400000, SizeType.Bytes);
		File.Delete(Application.persistentDataPath + "/Multiplayer/save.dat");
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x0008E0A8 File Offset: 0x0008C2A8
	[Command]
	private void CmdCarSave(byte[] CarSave, string SaveName)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteBytesAndSize(CarSave);
		writer.WriteString(SaveName);
		base.SendCommandInternal("System.Void NetworkPLayer::CmdCarSave(System.Byte[],System.String)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x0008E0E8 File Offset: 0x0008C2E8
	[ClientRpc]
	private void RpcCarSave(byte[] CarSave, string SaveName)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteBytesAndSize(CarSave);
		writer.WriteString(SaveName);
		this.SendRPCInternal("System.Void NetworkPLayer::RpcCarSave(System.Byte[],System.String)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x0008E128 File Offset: 0x0008C328
	public void RemoveStartParts()
	{
		foreach (Transform transform in UnityEngine.Object.FindObjectsOfType<Transform>())
		{
			if (transform.gameObject.GetComponent<MainCarProperties>() || transform.gameObject.GetComponent<MainTrailerProperties>() || (transform.gameObject.GetComponent<CarProperties>() && transform.gameObject.GetComponent<CarProperties>().PREFAB) || transform.gameObject.GetComponent<SaveItem>())
			{
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x0008E1B8 File Offset: 0x0008C3B8
	public void StartSceneObjects()
	{
		foreach (Transform transform in UnityEngine.Object.FindObjectsOfType<Transform>())
		{
			if (!transform.gameObject.GetComponent<MPobject>() && (transform.gameObject.GetComponent<MainCarProperties>() || transform.gameObject.GetComponent<MainTrailerProperties>() || (transform.gameObject.GetComponent<CarProperties>() && transform.gameObject.GetComponent<CarProperties>().PREFAB) || (transform.gameObject.GetComponent<SaveItem>() || transform.gameObject.GetComponent<PickupCup>() || transform.gameObject.GetComponent<SavePosition>() || (transform.gameObject.GetComponent<PickupHand>() && transform.name == "TrailerHandle")) || (transform.gameObject.GetComponent<PickupTool>() && (transform.name == "WelderHandle" || transform.name == "WaterHose" || transform.name == "FuelHose" || transform.name == "Nozzle"))))
			{
				tools.MPNumber++;
				transform.gameObject.AddComponent<MPobject>();
				transform.gameObject.GetComponent<MPobject>().MPNumber = tools.MPNumber;
			}
		}
		foreach (MPobject mpobject in UnityEngine.Object.FindObjectsOfType<MPobject>())
		{
			this.ITEM = mpobject.gameObject;
			this.SpawnStartObjects();
		}
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x0008E364 File Offset: 0x0008C564
	public void AddSceneObjects()
	{
		foreach (Transform transform in UnityEngine.Object.FindObjectsOfType<Transform>())
		{
			if (!transform.gameObject.GetComponent<MPobject>() && (transform.gameObject.GetComponent<MainCarProperties>() || transform.gameObject.GetComponent<MainTrailerProperties>() || transform.gameObject.GetComponent<SaveItem>() || (transform.gameObject.GetComponent<CarProperties>() && transform.gameObject.GetComponent<CarProperties>().PREFAB)))
			{
				tools.MPNumber++;
				transform.gameObject.AddComponent<MPobject>();
				transform.gameObject.GetComponent<MPobject>().MPNumber = tools.MPNumber;
			}
		}
		foreach (MPobject mpobject in UnityEngine.Object.FindObjectsOfType<MPobject>())
		{
			this.ITEM = mpobject.gameObject;
			this.SpawnStartObjects();
		}
		base.StartCoroutine(this.LoadScene(true));
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x0008E46C File Offset: 0x0008C66C
	public void SpawnCar(GameObject Car, int mpnr)
	{
		Transform[] componentsInChildren = Car.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			if (!transform.gameObject.GetComponent<MPobject>() && (transform.gameObject.GetComponent<MainCarProperties>() || transform.gameObject.GetComponent<MainTrailerProperties>() || (transform.gameObject.GetComponent<CarProperties>() && transform.gameObject.GetComponent<CarProperties>().PREFAB) || transform.gameObject.GetComponent<PickupCup>()))
			{
				mpnr++;
				transform.gameObject.AddComponent<MPobject>();
				transform.gameObject.GetComponent<MPobject>().MPNumber = mpnr;
			}
		}
		if (base.isServer)
		{
			foreach (Transform transform2 in componentsInChildren)
			{
				if (transform2.gameObject.GetComponent<MPobject>())
				{
					this.ITEM = transform2.gameObject;
					this.SpawnStartObjects();
				}
			}
		}
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x0008E573 File Offset: 0x0008C773
	public void SpawnTrailer()
	{
		this.CmdSpawnTrailer(this.Itemname, this.Spawnposition);
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x0008E588 File Offset: 0x0008C788
	[Command(requiresAuthority = false)]
	private void CmdSpawnTrailer(string trailername, Vector3 spawnpos)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteString(trailername);
		writer.WriteVector3(spawnpos);
		base.SendCommandInternal("System.Void NetworkPLayer::CmdSpawnTrailer(System.String,UnityEngine.Vector3)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x0008E5C8 File Offset: 0x0008C7C8
	[ClientRpc]
	private void RpcSpawnTrailer(string trailername, Vector3 spawnpos, int mpnr)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteString(trailername);
		writer.WriteVector3(spawnpos);
		writer.WriteInt(mpnr);
		this.SendRPCInternal("System.Void NetworkPLayer::RpcSpawnTrailer(System.String,UnityEngine.Vector3,System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x0008E611 File Offset: 0x0008C811
	public void SpawnMoreCars(int seed, Vector3 SpawnPoint, Vector3 SpawnPointEnd, int state)
	{
		tools.MPNumber += 500;
		this.CmdSpawnMoreCars(seed, SpawnPoint, SpawnPointEnd, state, tools.MPNumber);
		tools.MPNumber += 500;
	}

	// Token: 0x06000CF5 RID: 3317 RVA: 0x0008E644 File Offset: 0x0008C844
	[Command(requiresAuthority = false)]
	private void CmdSpawnMoreCars(int seed, Vector3 SpawnPoint, Vector3 SpawnPointEnd, int state, int mpnr)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(seed);
		writer.WriteVector3(SpawnPoint);
		writer.WriteVector3(SpawnPointEnd);
		writer.WriteInt(state);
		writer.WriteInt(mpnr);
		base.SendCommandInternal("System.Void NetworkPLayer::CmdSpawnMoreCars(System.Int32,UnityEngine.Vector3,UnityEngine.Vector3,System.Int32,System.Int32)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CF6 RID: 3318 RVA: 0x0008E6A4 File Offset: 0x0008C8A4
	[ClientRpc]
	private void RpcSpawnMoreCars(int seed, Vector3 SpawnPoint, Vector3 SpawnPointEnd, int state, int mpnr)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(seed);
		writer.WriteVector3(SpawnPoint);
		writer.WriteVector3(SpawnPointEnd);
		writer.WriteInt(state);
		writer.WriteInt(mpnr);
		this.SendRPCInternal("System.Void NetworkPLayer::RpcSpawnMoreCars(System.Int32,UnityEngine.Vector3,UnityEngine.Vector3,System.Int32,System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x0008E701 File Offset: 0x0008C901
	public void Sit(networkDummy networkDummy)
	{
		this.CmdSit(networkDummy);
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x0008E70C File Offset: 0x0008C90C
	[Command(requiresAuthority = false)]
	private void CmdSit(networkDummy networkDummy)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteNetworkBehaviour(networkDummy);
		base.SendCommandInternal("System.Void NetworkPLayer::CmdSit(networkDummy)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x0008E744 File Offset: 0x0008C944
	[ClientRpc]
	private void RpcSit(networkDummy networkDummy)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteNetworkBehaviour(networkDummy);
		this.SendRPCInternal("System.Void NetworkPLayer::RpcSit(networkDummy)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x0008E779 File Offset: 0x0008C979
	public void Stand(networkDummy networkDummy)
	{
		this.CmdStand(networkDummy);
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x0008E784 File Offset: 0x0008C984
	[Command(requiresAuthority = false)]
	private void CmdStand(networkDummy networkDummy)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteNetworkBehaviour(networkDummy);
		base.SendCommandInternal("System.Void NetworkPLayer::CmdStand(networkDummy)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CFC RID: 3324 RVA: 0x0008E7BC File Offset: 0x0008C9BC
	[ClientRpc]
	private void RpcStand(networkDummy networkDummy)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteNetworkBehaviour(networkDummy);
		this.SendRPCInternal("System.Void NetworkPLayer::RpcStand(networkDummy)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000CFD RID: 3325 RVA: 0x0008E7F4 File Offset: 0x0008C9F4
	public void MatchTarget()
	{
		MatchTargetWeightMask weightMask = new MatchTargetWeightMask(Vector3.one, 0f);
		if (!this.m_Animator.isMatchingTarget)
		{
			this.m_Animator.MatchTarget(this.AvatarRightHandTarget.transform.position, this.AvatarRightHandTarget.transform.rotation, AvatarTarget.RightHand, weightMask, 0.141f, 0.78f);
		}
	}

	// Token: 0x06000CFE RID: 3326 RVA: 0x0008E856 File Offset: 0x0008CA56
	public void DidntCome()
	{
		this.CmdDidntCome();
	}

	// Token: 0x06000CFF RID: 3327 RVA: 0x0008E860 File Offset: 0x0008CA60
	[Command(requiresAuthority = false)]
	private void CmdDidntCome()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void NetworkPLayer::CmdDidntCome()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000D00 RID: 3328 RVA: 0x0008E88C File Offset: 0x0008CA8C
	[ClientRpc]
	private void RpcDidntCome()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void NetworkPLayer::RpcDidntCome()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x0008E8B7 File Offset: 0x0008CAB7
	public void SpawnJob(int seed, string ActiveJ)
	{
		tools.MPNumber += 500;
		this.CmdSpawnJob(seed, ActiveJ, tools.MPNumber);
		tools.MPNumber += 500;
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x0008E8E8 File Offset: 0x0008CAE8
	[Command(requiresAuthority = false)]
	private void CmdSpawnJob(int seed, string ActiveJ, int mpnr)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(seed);
		writer.WriteString(ActiveJ);
		writer.WriteInt(mpnr);
		base.SendCommandInternal("System.Void NetworkPLayer::CmdSpawnJob(System.Int32,System.String,System.Int32)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x0008E934 File Offset: 0x0008CB34
	[ClientRpc]
	private void RpcSpawnJob(int seed, string ActiveJ, int mpnr)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(seed);
		writer.WriteString(ActiveJ);
		writer.WriteInt(mpnr);
		this.SendRPCInternal("System.Void NetworkPLayer::RpcSpawnJob(System.Int32,System.String,System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000D04 RID: 3332 RVA: 0x0008E97D File Offset: 0x0008CB7D
	public void SetJobStats(float StartValue, float Partcount, float Realreward, string ActiveJobname)
	{
		this.CmdSetJobStats(StartValue, Partcount, Realreward, ActiveJobname);
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x0008E98C File Offset: 0x0008CB8C
	[Command(requiresAuthority = false)]
	private void CmdSetJobStats(float StartValue, float Partcount, float Realreward, string ActiveJobname)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteFloat(StartValue);
		writer.WriteFloat(Partcount);
		writer.WriteFloat(Realreward);
		writer.WriteString(ActiveJobname);
		base.SendCommandInternal("System.Void NetworkPLayer::CmdSetJobStats(System.Single,System.Single,System.Single,System.String)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x0008E9E0 File Offset: 0x0008CBE0
	[ClientRpc]
	private void RpcSetJobStats(float StartValue, float Partcount, float Realreward, string ActiveJobname)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteFloat(StartValue);
		writer.WriteFloat(Partcount);
		writer.WriteFloat(Realreward);
		writer.WriteString(ActiveJobname);
		this.SendRPCInternal("System.Void NetworkPLayer::RpcSetJobStats(System.Single,System.Single,System.Single,System.String)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000D07 RID: 3335 RVA: 0x0008EA33 File Offset: 0x0008CC33
	public void SpawnPlates2(string OneNR, string TwoNR, string ThreeNR, string FourNR, string FiveNR, string SixNR, Vector3 Spot)
	{
		this.CmdSpawnPlates2(OneNR, TwoNR, ThreeNR, FourNR, FiveNR, SixNR, Spot);
	}

	// Token: 0x06000D08 RID: 3336 RVA: 0x0008EA48 File Offset: 0x0008CC48
	[Command(requiresAuthority = false)]
	private void CmdSpawnPlates2(string OneNR, string TwoNR, string ThreeNR, string FourNR, string FiveNR, string SixNR, Vector3 Spot)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteString(OneNR);
		writer.WriteString(TwoNR);
		writer.WriteString(ThreeNR);
		writer.WriteString(FourNR);
		writer.WriteString(FiveNR);
		writer.WriteString(SixNR);
		writer.WriteVector3(Spot);
		base.SendCommandInternal("System.Void NetworkPLayer::CmdSpawnPlates2(System.String,System.String,System.String,System.String,System.String,System.String,UnityEngine.Vector3)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000D09 RID: 3337 RVA: 0x0008EABC File Offset: 0x0008CCBC
	[ClientRpc]
	private void RpcSpawnPlates2(string OneNR, string TwoNR, string ThreeNR, string FourNR, string FiveNR, string SixNR, Vector3 Spot, int mpNr)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteString(OneNR);
		writer.WriteString(TwoNR);
		writer.WriteString(ThreeNR);
		writer.WriteString(FourNR);
		writer.WriteString(FiveNR);
		writer.WriteString(SixNR);
		writer.WriteVector3(Spot);
		writer.WriteInt(mpNr);
		this.SendRPCInternal("System.Void NetworkPLayer::RpcSpawnPlates2(System.String,System.String,System.String,System.String,System.String,System.String,UnityEngine.Vector3,System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000D0A RID: 3338 RVA: 0x0008EB37 File Offset: 0x0008CD37
	public void ShiftWorld(Vector3 cameraPosition)
	{
		this.CmdShiftWorld(cameraPosition);
	}

	// Token: 0x06000D0B RID: 3339 RVA: 0x0008EB40 File Offset: 0x0008CD40
	[Command(requiresAuthority = false)]
	private void CmdShiftWorld(Vector3 cameraPosition)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteVector3(cameraPosition);
		base.SendCommandInternal("System.Void NetworkPLayer::CmdShiftWorld(UnityEngine.Vector3)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000D0C RID: 3340 RVA: 0x0008EB78 File Offset: 0x0008CD78
	[ClientRpc]
	private void RpcShiftWorld(Vector3 cameraPosition)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteVector3(cameraPosition);
		this.SendRPCInternal("System.Void NetworkPLayer::RpcShiftWorld(UnityEngine.Vector3)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x0000245B File Offset: 0x0000065B
	private void MirrorProcessed()
	{
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0008EBB0 File Offset: 0x0008CDB0
	// (set) Token: 0x06000D10 RID: 3344 RVA: 0x0008EBC3 File Offset: 0x0008CDC3
	public Color32 Networkcolorpaint
	{
		get
		{
			return this.colorpaint;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<Color32>(value, ref this.colorpaint, 1UL, null);
		}
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x0008EBDD File Offset: 0x0008CDDD
	protected void UserCode_CmdClientJoined()
	{
		this.RpcClientJoined();
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x0008EBE5 File Offset: 0x0008CDE5
	protected static void InvokeUserCode_CmdClientJoined(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdClientJoined called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdClientJoined();
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x0008EC08 File Offset: 0x0008CE08
	protected void UserCode_RpcClientJoined()
	{
		this.RealPlayer.transform.parent.GetComponent<tools>().MPloadingText.SetActive(true);
	}

	// Token: 0x06000D14 RID: 3348 RVA: 0x0008EC2A File Offset: 0x0008CE2A
	protected static void InvokeUserCode_RpcClientJoined(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcClientJoined called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcClientJoined();
	}

	// Token: 0x06000D15 RID: 3349 RVA: 0x0008EC4D File Offset: 0x0008CE4D
	protected void UserCode_CmdClientLoaded()
	{
		this.RpcClientLoaded();
	}

	// Token: 0x06000D16 RID: 3350 RVA: 0x0008EC55 File Offset: 0x0008CE55
	protected static void InvokeUserCode_CmdClientLoaded(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdClientLoaded called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdClientLoaded();
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x0008EC78 File Offset: 0x0008CE78
	protected void UserCode_RpcClientLoaded()
	{
		this.RealPlayer.transform.parent.GetComponent<tools>().MPloadingText.SetActive(false);
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x0008EC9A File Offset: 0x0008CE9A
	protected static void InvokeUserCode_RpcClientLoaded(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcClientLoaded called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcClientLoaded();
	}

	// Token: 0x06000D19 RID: 3353 RVA: 0x0008ECBD File Offset: 0x0008CEBD
	protected void UserCode_CmdLoadScene()
	{
		base.StartCoroutine(this.LoadScene(false));
	}

	// Token: 0x06000D1A RID: 3354 RVA: 0x0008ECCD File Offset: 0x0008CECD
	protected static void InvokeUserCode_CmdLoadScene(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdLoadScene called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdLoadScene();
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x0008ECF0 File Offset: 0x0008CEF0
	protected void UserCode_RpcCleanFolder()
	{
		if (Directory.Exists(Application.persistentDataPath + "/Multiplayer"))
		{
			Directory.Delete(Application.persistentDataPath + "/Multiplayer", true);
		}
		Directory.CreateDirectory(Application.persistentDataPath + "/Multiplayer");
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x0008ED3D File Offset: 0x0008CF3D
	protected static void InvokeUserCode_RpcCleanFolder(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC RpcCleanFolder called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcCleanFolder();
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x0008ED60 File Offset: 0x0008CF60
	protected void UserCode_RpcReceiveData__Byte[]__String(byte[] Data, string SaveName)
	{
		File.WriteAllBytes(Application.persistentDataPath + "/Multiplayer/" + SaveName, Data);
		this.CmdPackegecame();
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x0008ED7E File Offset: 0x0008CF7E
	protected static void InvokeUserCode_RpcReceiveData__Byte[]__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC RpcReceiveData called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcReceiveData__Byte[]__String(reader.ReadBytesAndSize(), reader.ReadString());
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x0008EDAD File Offset: 0x0008CFAD
	protected void UserCode_RpcReceiveAdditionalData__Byte[]__String(byte[] Data, string SaveName)
	{
		if (!base.isServer && base.isLocalPlayer)
		{
			File.WriteAllBytes(Application.persistentDataPath + "/Multiplayer/" + SaveName, Data);
			this.CmdPackegecame();
		}
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x0008EDDB File Offset: 0x0008CFDB
	protected static void InvokeUserCode_RpcReceiveAdditionalData__Byte[]__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcReceiveAdditionalData called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcReceiveAdditionalData__Byte[]__String(reader.ReadBytesAndSize(), reader.ReadString());
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x0008EE0A File Offset: 0x0008D00A
	protected void UserCode_CmdPackegecame()
	{
		this.RpcPackegecame();
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x0008EE12 File Offset: 0x0008D012
	protected static void InvokeUserCode_CmdPackegecame(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdPackegecame called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdPackegecame();
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x0008EE35 File Offset: 0x0008D035
	protected void UserCode_RpcPackegecame()
	{
		if (base.isServer && base.isLocalPlayer)
		{
			this.PackageCame = true;
			Debug.Log("PackageCame");
		}
	}

	// Token: 0x06000D24 RID: 3364 RVA: 0x0008EE58 File Offset: 0x0008D058
	protected static void InvokeUserCode_RpcPackegecame(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPackegecame called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcPackegecame();
	}

	// Token: 0x06000D25 RID: 3365 RVA: 0x0008EE7C File Offset: 0x0008D07C
	protected void UserCode_RpcLoadScene()
	{
		new Compose().MergeFiles(Application.persistentDataPath + "/Multiplayer", true);
		byte[] bytes = Zip.Decompress(File.ReadAllBytes(Application.persistentDataPath + "/Multiplayer/save.dat"));
		File.WriteAllBytes(Application.persistentDataPath + "/Multiplayer/save.dat", bytes);
		Debug.Log("Multiplayer filesCame");
		if (this.Saver.mapMagic)
		{
			this.Saver.AwakeContinue(true);
			return;
		}
		this.Saver.Load(true);
	}

	// Token: 0x06000D26 RID: 3366 RVA: 0x0008EF07 File Offset: 0x0008D107
	protected static void InvokeUserCode_RpcLoadScene(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC RpcLoadScene called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcLoadScene();
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x0008EF2A File Offset: 0x0008D12A
	protected void UserCode_RpcLoadAdditionalScene()
	{
		if (!base.isServer && base.isLocalPlayer)
		{
			new Compose().MergeFiles(Application.persistentDataPath + "/Multiplayer", true);
			this.Saver.Load(true);
		}
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x0008EF62 File Offset: 0x0008D162
	protected static void InvokeUserCode_RpcLoadAdditionalScene(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcLoadAdditionalScene called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcLoadAdditionalScene();
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x0008EF88 File Offset: 0x0008D188
	protected void UserCode_CmdPickupItem__NetworkIdentity(NetworkIdentity NetworkIdentity)
	{
		NetworkIdentity.RemoveClientAuthority();
		NetworkIdentity.AssignClientAuthority(base.connectionToClient);
		if (NetworkIdentity.GetComponent<networkDummy>().Target && NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<SavePosition>())
		{
			foreach (MPobject mpobject in NetworkIdentity.GetComponent<networkDummy>().Target.transform.root.GetComponentsInChildren<MPobject>())
			{
				if (mpobject.networkDummy)
				{
					mpobject.networkDummy.GetComponent<NetworkIdentity>().RemoveClientAuthority();
					mpobject.networkDummy.GetComponent<NetworkIdentity>().AssignClientAuthority(base.connectionToClient);
				}
			}
		}
		if (NetworkIdentity.GetComponent<networkDummy>().Target && NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<MainCarProperties>())
		{
			if (NetworkIdentity.hasAuthority)
			{
				NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<Rigidbody>().isKinematic = false;
			}
			else
			{
				NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<Rigidbody>().isKinematic = true;
			}
		}
		this.RpcPickupItem(NetworkIdentity);
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x0008F098 File Offset: 0x0008D298
	protected static void InvokeUserCode_CmdPickupItem__NetworkIdentity(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdPickupItem called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdPickupItem__NetworkIdentity(reader.ReadNetworkIdentity());
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x0008F0C4 File Offset: 0x0008D2C4
	protected void UserCode_RpcPickupItem__NetworkIdentity(NetworkIdentity NetworkIdentity)
	{
		if (NetworkIdentity.GetComponent<networkDummy>() && NetworkIdentity.GetComponent<networkDummy>().Target && (NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<MainCarProperties>() || NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<MainTrailerProperties>()))
		{
			if (NetworkIdentity.hasAuthority)
			{
				NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<Rigidbody>().isKinematic = false;
			}
			else
			{
				NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<Rigidbody>().isKinematic = true;
			}
			if (NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<MainCarProperties>())
			{
				NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<VehicleController>().SetMP(NetworkIdentity.hasAuthority);
			}
			if (NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<MainTrailerProperties>())
			{
				NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<MainTrailerProperties>().SetMP(NetworkIdentity.hasAuthority);
			}
		}
		if (NetworkIdentity.GetComponent<networkDummy>().Target && NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<PickupTool>() && NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<Rigidbody>())
		{
			NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<Rigidbody>().detectCollisions = false;
			NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<Rigidbody>().isKinematic = true;
		}
		NetworkIdentity.GetComponent<NetworkTransform>().enabled = true;
		NetworkIdentity.GetComponent<networkDummy>().enabled = true;
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x0008F245 File Offset: 0x0008D445
	protected static void InvokeUserCode_RpcPickupItem__NetworkIdentity(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPickupItem called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcPickupItem__NetworkIdentity(reader.ReadNetworkIdentity());
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x0008F26E File Offset: 0x0008D46E
	protected void UserCode_Cmdputdown__NetworkIdentity(NetworkIdentity NetworkIdentity)
	{
		this.Rpcputdown(NetworkIdentity);
	}

	// Token: 0x06000D2E RID: 3374 RVA: 0x0008F277 File Offset: 0x0008D477
	protected static void InvokeUserCode_Cmdputdown__NetworkIdentity(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command Cmdputdown called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_Cmdputdown__NetworkIdentity(reader.ReadNetworkIdentity());
	}

	// Token: 0x06000D2F RID: 3375 RVA: 0x0008F2A0 File Offset: 0x0008D4A0
	protected void UserCode_Rpcputdown__NetworkIdentity(NetworkIdentity NetworkIdentity)
	{
		if (NetworkIdentity.GetComponent<networkDummy>().Target && (NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<PickupTool>() || NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<MooveItem>()) && NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<Rigidbody>())
		{
			NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<Rigidbody>().detectCollisions = true;
			NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<Rigidbody>().isKinematic = false;
			NetworkIdentity.GetComponent<networkDummy>().Target.GetComponent<Rigidbody>().detectCollisions = true;
			NetworkIdentity.GetComponent<NetworkTransform>().enabled = false;
			NetworkIdentity.GetComponent<networkDummy>().enabled = false;
		}
	}

	// Token: 0x06000D30 RID: 3376 RVA: 0x0008F361 File Offset: 0x0008D561
	protected static void InvokeUserCode_Rpcputdown__NetworkIdentity(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC Rpcputdown called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_Rpcputdown__NetworkIdentity(reader.ReadNetworkIdentity());
	}

	// Token: 0x06000D31 RID: 3377 RVA: 0x0008F38C File Offset: 0x0008D58C
	protected void UserCode_CmdSpawnItem__String__Vector3__Quaternion__Color__Boolean__Int32(string Itemname, Vector3 Spawnposition, Quaternion Spawnrotation, Color Colorpaint, bool newp, int seed)
	{
		tools.MPNumber++;
		if (Itemname != "CUP" && Itemname != "Nozzle" && Itemname != "WelderHandle")
		{
			this.RpcSpawnItem(Itemname, Spawnposition, Spawnrotation, Colorpaint, newp, seed, tools.MPNumber);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.DUMMY, Spawnposition, Spawnrotation);
		gameObject.GetComponent<networkDummy>().NetworkItemname = Itemname;
		gameObject.GetComponent<networkDummy>().NetworkSpawnposition = Spawnposition;
		gameObject.GetComponent<networkDummy>().NetworkSpawnrotation = Spawnrotation;
		gameObject.GetComponent<networkDummy>().NetworkMPNumber = tools.MPNumber;
		if (Itemname == "CUP" || Itemname == "Nozzle" || Itemname == "WelderHandle")
		{
			gameObject.GetComponent<networkDummy>().NetworkCupMPNumber = tools.MPNumber - 1;
		}
		NetworkServer.Spawn(gameObject, base.connectionToClient);
	}

	// Token: 0x06000D32 RID: 3378 RVA: 0x0008F468 File Offset: 0x0008D668
	protected static void InvokeUserCode_CmdSpawnItem__String__Vector3__Quaternion__Color__Boolean__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSpawnItem called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdSpawnItem__String__Vector3__Quaternion__Color__Boolean__Int32(reader.ReadString(), reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadColor(), reader.ReadBool(), reader.ReadInt());
	}

	// Token: 0x06000D33 RID: 3379 RVA: 0x0008F4BC File Offset: 0x0008D6BC
	protected void UserCode_RpcSpawnItem__String__Vector3__Quaternion__Color__Boolean__Int32__Int32(string Itemname, Vector3 Spawnposition, Quaternion Spawnrotation, Color Colorpaint, bool newp, int seed, int mpnr)
	{
		GameObject gameObject = null;
		try
		{
			gameObject = (UnityEngine.Object.Instantiate(cachedResources.Load(Itemname), Spawnposition, Spawnrotation) as GameObject);
		}
		catch (Exception)
		{
			if (Saver.modParts.ContainsKey(Itemname))
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(Saver.modParts[Itemname] as GameObject, Spawnposition, Spawnrotation);
			}
		}
		if (!gameObject)
		{
			Debug.LogError("Object missing in RpcSpawnItem, possible host-client mismatch! Object " + Itemname);
		}
		if (gameObject.GetComponent<Partinfo>() && gameObject.GetComponent<Partinfo>().RenamedPrefab != "")
		{
			gameObject.transform.name = gameObject.GetComponent<Partinfo>().RenamedPrefab;
		}
		else
		{
			gameObject.transform.name = Itemname;
		}
		if (seed > 0)
		{
			CarProperties component = gameObject.GetComponent<CarProperties>();
			Color color = UnityEngine.Random.ColorHSV();
			component.Condition = UnityEngine.Random.Range(0.003f, 1f);
			if (component.Paintable)
			{
				component.gameObject.GetComponent<P3dPaintableTexture>().Color = color;
			}
			if (component.Tire)
			{
				if (component.Condition < 0.1f)
				{
					component.PartIsOld = true;
					component.gameObject.GetComponent<Renderer>().sharedMaterial = component.OldMaterial;
				}
				component.TirePressure = 0f;
			}
			if (component.Interior)
			{
				component.OriginalInterior = UnityEngine.Random.Range(1, 7);
			}
			if (tools.NetworkPLayer.JM != null)
			{
				component.Owner = "Junkyard";
			}
			else
			{
				component.Owner = "Player";
			}
			component.ReStart();
			if (gameObject.GetComponent<Partinfo>())
			{
				gameObject.GetComponent<Partinfo>().CreatingJunk();
			}
		}
		if (newp && gameObject.GetComponent<Partinfo>() && !gameObject.GetComponent<MainCarProperties>())
		{
			gameObject.GetComponent<Partinfo>().Creating();
		}
		if (gameObject.GetComponent<PickupTool>())
		{
			gameObject.GetComponent<PickupTool>().colorpaint = Colorpaint;
		}
		if (gameObject.GetComponent<MainCarProperties>())
		{
			gameObject.GetComponent<MainCarProperties>().CreatingReady();
		}
		gameObject.AddComponent<MPobject>();
		gameObject.GetComponent<MPobject>().MPNumber = mpnr;
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x0008F6C4 File Offset: 0x0008D8C4
	protected static void InvokeUserCode_RpcSpawnItem__String__Vector3__Quaternion__Color__Boolean__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSpawnItem called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcSpawnItem__String__Vector3__Quaternion__Color__Boolean__Int32__Int32(reader.ReadString(), reader.ReadVector3(), reader.ReadQuaternion(), reader.ReadColor(), reader.ReadBool(), reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x0008F71C File Offset: 0x0008D91C
	protected void UserCode_CmdSpawn__String(string CarName)
	{
		if (this.Car)
		{
			UnityEngine.Object.Destroy(this.Car);
		}
		if (CarName == "LADcoupe")
		{
			this.Car = UnityEngine.Object.Instantiate<GameObject>(this.LADcoupePR, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), base.transform.rotation);
		}
		else if (CarName == "LAD")
		{
			this.Car = UnityEngine.Object.Instantiate<GameObject>(this.LADPR, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), base.transform.rotation);
		}
		else if (CarName == "LADcabrio")
		{
			this.Car = UnityEngine.Object.Instantiate<GameObject>(this.LADcabrioPR, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), base.transform.rotation);
		}
		else if (CarName == "NIV")
		{
			this.Car = UnityEngine.Object.Instantiate<GameObject>(this.NIVPR, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), base.transform.rotation);
		}
		else if (CarName == "Chad")
		{
			this.Car = UnityEngine.Object.Instantiate<GameObject>(this.ChadPR, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), base.transform.rotation);
		}
		else if (CarName == "Bart")
		{
			this.Car = UnityEngine.Object.Instantiate<GameObject>(this.BartPR, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), base.transform.rotation);
		}
		else if (CarName == "L500")
		{
			this.Car = UnityEngine.Object.Instantiate<GameObject>(this.L500PR, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), base.transform.rotation);
		}
		else if (CarName == "Jesse")
		{
			this.Car = UnityEngine.Object.Instantiate<GameObject>(this.JessePR, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), base.transform.rotation);
		}
		else if (CarName == "Hardy")
		{
			this.Car = UnityEngine.Object.Instantiate<GameObject>(this.HardyPR, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), base.transform.rotation);
		}
		else if (CarName == "Wolf")
		{
			this.Car = UnityEngine.Object.Instantiate<GameObject>(this.WolfPR, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 3f), base.transform.rotation);
		}
		NetworkServer.Spawn(this.Car, base.connectionToClient);
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x0008FBA0 File Offset: 0x0008DDA0
	protected static void InvokeUserCode_CmdSpawn__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSpawn called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdSpawn__String(reader.ReadString());
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x0008FBC9 File Offset: 0x0008DDC9
	protected void UserCode_CmdCarSave__Byte[]__String(byte[] CarSave, string SaveName)
	{
		this.RpcCarSave(CarSave, SaveName);
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x0008FBD3 File Offset: 0x0008DDD3
	protected static void InvokeUserCode_CmdCarSave__Byte[]__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCarSave called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdCarSave__Byte[]__String(reader.ReadBytesAndSize(), reader.ReadString());
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x0008FC04 File Offset: 0x0008DE04
	protected void UserCode_RpcCarSave__Byte[]__String(byte[] CarSave, string SaveName)
	{
		byte[] bytes = Zip.Decompress(CarSave);
		File.WriteAllBytes(Application.persistentDataPath + "/Multiplayer/" + SaveName, bytes);
		File.WriteAllBytes(Application.persistentDataPath + "/Multiplayer/CarRARedSize", CarSave);
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x0008FC43 File Offset: 0x0008DE43
	protected static void InvokeUserCode_RpcCarSave__Byte[]__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcCarSave called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcCarSave__Byte[]__String(reader.ReadBytesAndSize(), reader.ReadString());
	}

	// Token: 0x06000D3B RID: 3387 RVA: 0x0008FC72 File Offset: 0x0008DE72
	protected void UserCode_CmdSpawnTrailer__String__Vector3(string trailername, Vector3 spawnpos)
	{
		tools.MPNumber += 500;
		this.RpcSpawnTrailer(trailername, spawnpos, tools.MPNumber);
		tools.MPNumber += 500;
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x0008FCA1 File Offset: 0x0008DEA1
	protected static void InvokeUserCode_CmdSpawnTrailer__String__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSpawnTrailer called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdSpawnTrailer__String__Vector3(reader.ReadString(), reader.ReadVector3());
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x0008FCD0 File Offset: 0x0008DED0
	protected void UserCode_RpcSpawnTrailer__String__Vector3__Int32(string trailername, Vector3 spawnpos, int mpnr)
	{
		GameObject gameObject = base.gameObject;
		if (trailername == this.TrailerCar.transform.name)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TrailerCar, spawnpos, Quaternion.identity);
		}
		if (trailername == this.TrailerLong.transform.name)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TrailerLong, spawnpos, Quaternion.identity);
		}
		if (trailername == this.TrailerShort.transform.name)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TrailerShort, spawnpos, Quaternion.identity);
		}
		if (gameObject.GetComponent<MainTrailerProperties>())
		{
			gameObject.GetComponent<MainTrailerProperties>().JustSpawned = true;
			gameObject.GetComponent<MainTrailerProperties>().SetOwnerPlayer();
		}
		Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			if (!transform.gameObject.GetComponent<MPobject>() && (transform.gameObject.GetComponent<MainTrailerProperties>() || transform.gameObject.GetComponent<PickupHand>() || (transform.gameObject.GetComponent<CarProperties>() && transform.gameObject.GetComponent<CarProperties>().PREFAB) || transform.gameObject.GetComponent<PickupCup>()))
			{
				mpnr++;
				transform.gameObject.AddComponent<MPobject>();
				transform.gameObject.GetComponent<MPobject>().MPNumber = mpnr;
			}
		}
		if (base.isServer)
		{
			foreach (Transform transform2 in componentsInChildren)
			{
				if (transform2.gameObject.GetComponent<MPobject>())
				{
					this.ITEM = transform2.gameObject;
					this.SpawnStartObjects();
				}
			}
		}
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x0008FE89 File Offset: 0x0008E089
	protected static void InvokeUserCode_RpcSpawnTrailer__String__Vector3__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSpawnTrailer called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcSpawnTrailer__String__Vector3__Int32(reader.ReadString(), reader.ReadVector3(), reader.ReadInt());
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x0008FEBE File Offset: 0x0008E0BE
	protected void UserCode_CmdSpawnMoreCars__Int32__Vector3__Vector3__Int32__Int32(int seed, Vector3 SpawnPoint, Vector3 SpawnPointEnd, int state, int mpnr)
	{
		this.RpcSpawnMoreCars(seed, SpawnPoint, SpawnPointEnd, state, mpnr);
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x0008FED0 File Offset: 0x0008E0D0
	protected static void InvokeUserCode_CmdSpawnMoreCars__Int32__Vector3__Vector3__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSpawnMoreCars called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdSpawnMoreCars__Int32__Vector3__Vector3__Int32__Int32(reader.ReadInt(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x0008FF1C File Offset: 0x0008E11C
	protected void UserCode_RpcSpawnMoreCars__Int32__Vector3__Vector3__Int32__Int32(int seed, Vector3 SpawnPoint, Vector3 SpawnPointEnd, int state, int mpnr)
	{
		UnityEngine.Random.InitState(seed);
		if (state == 1)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)], SpawnPointEnd, Quaternion.identity);
			if (gameObject.GetComponent<MainCarProperties>())
			{
				if (base.transform.root.name == "MapMagic")
				{
					gameObject.GetComponent<MainCarProperties>().Seed = seed;
				}
				gameObject.GetComponent<MainCarProperties>().SpawnPoint = SpawnPointEnd;
				gameObject.GetComponent<MainCarProperties>().CreatingJunkyard();
			}
			else
			{
				gameObject.transform.position = SpawnPoint;
			}
			this.SpawnCar(gameObject, mpnr);
		}
		if (state == 2)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)], SpawnPointEnd, Quaternion.identity);
			if (gameObject.GetComponent<MainCarProperties>())
			{
				if (base.transform.root.name == "MapMagic")
				{
					gameObject.GetComponent<MainCarProperties>().Seed = seed;
				}
				gameObject.GetComponent<MainCarProperties>().SpawnPoint = SpawnPointEnd;
				if (gameObject.GetComponent<MainCarProperties>().Bike)
				{
					gameObject.transform.position = SpawnPointEnd;
				}
				gameObject.GetComponent<MainCarProperties>().CreatingUsed();
			}
			else
			{
				gameObject.transform.position = SpawnPoint;
			}
			this.SpawnCar(gameObject, mpnr);
		}
		if (state == 3)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)], SpawnPointEnd, Quaternion.identity);
			if (gameObject.GetComponent<MainCarProperties>())
			{
				if (base.transform.root.name == "MapMagic")
				{
					gameObject.GetComponent<MainCarProperties>().Seed = seed;
				}
				gameObject.GetComponent<MainCarProperties>().SpawnPoint = SpawnPointEnd;
				gameObject.GetComponent<MainCarProperties>().CreatingUsed();
			}
			else
			{
				gameObject.transform.position = SpawnPoint;
			}
			this.SpawnCar(gameObject, mpnr);
		}
		if (state == 4)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)], SpawnPointEnd, Quaternion.identity);
			if (gameObject.GetComponent<MainCarProperties>())
			{
				if (base.transform.root.name == "MapMagic")
				{
					gameObject.GetComponent<MainCarProperties>().Seed = seed;
				}
				gameObject.GetComponent<MainCarProperties>().SpawnPoint = SpawnPointEnd;
				gameObject.GetComponent<MainCarProperties>().CreatingBarnFind();
			}
			else
			{
				gameObject.transform.position = SpawnPoint;
			}
			this.SpawnCar(gameObject, mpnr);
		}
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x00090178 File Offset: 0x0008E378
	protected static void InvokeUserCode_RpcSpawnMoreCars__Int32__Vector3__Vector3__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSpawnMoreCars called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcSpawnMoreCars__Int32__Vector3__Vector3__Int32__Int32(reader.ReadInt(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x000901C4 File Offset: 0x0008E3C4
	protected void UserCode_CmdSit__networkDummy(networkDummy networkDummy)
	{
		this.RpcSit(networkDummy);
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x000901CD File Offset: 0x0008E3CD
	protected static void InvokeUserCode_CmdSit__networkDummy(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSit called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdSit__networkDummy(reader.ReadNetworkBehaviour<networkDummy>());
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x000901F8 File Offset: 0x0008E3F8
	protected void UserCode_RpcSit__networkDummy(networkDummy networkDummy)
	{
		this.Avatar.transform.SetParent(networkDummy.Target.transform);
		this.Avatar.transform.position = this.Avatar.transform.parent.GetComponentInChildren<Sit>().transform.position;
		if (networkDummy.Target.transform.root.GetComponent<MainCarProperties>().Bike)
		{
			this.Avatar.transform.localRotation = Quaternion.Euler(45f, 0f, 0f);
			this.m_Animator.SetTrigger("SitOnBike");
		}
		else
		{
			this.Avatar.transform.localRotation = Quaternion.Euler(-15f, 0.3f, 0f);
			this.m_Animator.SetTrigger("SitInCar");
		}
		if (networkDummy.Target.transform.root.GetComponent<MainCarProperties>() && !this.Avatar.transform.parent.GetComponentInChildren<Sit>().passanger)
		{
			networkDummy.Target.transform.root.GetComponent<MainCarProperties>().SittingInCar = true;
		}
		networkDummy.Target.GetComponentInChildren<Sit>().sittingHere = true;
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x00090339 File Offset: 0x0008E539
	protected static void InvokeUserCode_RpcSit__networkDummy(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSit called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcSit__networkDummy(reader.ReadNetworkBehaviour<networkDummy>());
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x00090362 File Offset: 0x0008E562
	protected void UserCode_CmdStand__networkDummy(networkDummy networkDummy)
	{
		this.RpcStand(networkDummy);
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x0009036B File Offset: 0x0008E56B
	protected static void InvokeUserCode_CmdStand__networkDummy(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdStand called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdStand__networkDummy(reader.ReadNetworkBehaviour<networkDummy>());
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x00090394 File Offset: 0x0008E594
	protected void UserCode_RpcStand__networkDummy(networkDummy networkDummy)
	{
		if (networkDummy.Target.transform.root.GetComponent<MainCarProperties>())
		{
			networkDummy.Target.transform.root.GetComponent<MainCarProperties>().SittingInCar = false;
		}
		if (base.GetComponent<NetworkIdentity>().isLocalPlayer)
		{
			this.Avatar.SetActive(false);
		}
		this.Avatar.transform.SetParent(base.transform);
		this.Avatar.transform.position = base.transform.position;
		this.Avatar.transform.rotation = base.transform.rotation;
		this.m_Animator.SetTrigger("GetOffVehicle");
		networkDummy.Target.GetComponentInChildren<Sit>().sittingHere = false;
		this.Avatar.transform.position = base.transform.position;
		this.Avatar.transform.rotation = base.transform.rotation;
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x00090494 File Offset: 0x0008E694
	protected static void InvokeUserCode_RpcStand__networkDummy(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcStand called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcStand__networkDummy(reader.ReadNetworkBehaviour<networkDummy>());
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x000904BD File Offset: 0x0008E6BD
	protected void UserCode_CmdDidntCome()
	{
		this.RpcDidntCome();
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x000904C5 File Offset: 0x0008E6C5
	protected static void InvokeUserCode_CmdDidntCome(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDidntCome called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdDidntCome();
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x000904E8 File Offset: 0x0008E6E8
	protected void UserCode_RpcDidntCome()
	{
		this.JM.DidntCome2();
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x000904F5 File Offset: 0x0008E6F5
	protected static void InvokeUserCode_RpcDidntCome(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcDidntCome called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcDidntCome();
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x00090518 File Offset: 0x0008E718
	protected void UserCode_CmdSpawnJob__Int32__String__Int32(int seed, string ActiveJ, int mpnr)
	{
		this.RpcSpawnJob(seed, ActiveJ, mpnr);
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x00090523 File Offset: 0x0008E723
	protected static void InvokeUserCode_CmdSpawnJob__Int32__String__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSpawnJob called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdSpawnJob__Int32__String__Int32(reader.ReadInt(), reader.ReadString(), reader.ReadInt());
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x00090558 File Offset: 0x0008E758
	protected void UserCode_RpcSpawnJob__Int32__String__Int32(int seed, string ActiveJ, int mpnr)
	{
		this.JM.GeneratingCar(seed, ActiveJ, mpnr);
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x00090568 File Offset: 0x0008E768
	protected static void InvokeUserCode_RpcSpawnJob__Int32__String__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSpawnJob called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcSpawnJob__Int32__String__Int32(reader.ReadInt(), reader.ReadString(), reader.ReadInt());
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x0009059D File Offset: 0x0008E79D
	protected void UserCode_CmdSetJobStats__Single__Single__Single__String(float StartValue, float Partcount, float Realreward, string ActiveJobname)
	{
		this.RpcSetJobStats(StartValue, Partcount, Realreward, ActiveJobname);
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x000905AA File Offset: 0x0008E7AA
	protected static void InvokeUserCode_CmdSetJobStats__Single__Single__Single__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetJobStats called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdSetJobStats__Single__Single__Single__String(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadString());
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x000905E8 File Offset: 0x0008E7E8
	protected void UserCode_RpcSetJobStats__Single__Single__Single__String(float StartValue, float Partcount, float Realreward, string ActiveJobname)
	{
		this.JM.StartValue = StartValue;
		this.JM.Partcount = Partcount;
		this.JM.Realreward = Realreward;
		if (this.JM.StartValue > 0f)
		{
			this.JM.ActiveJob = GameObject.Find(ActiveJobname);
		}
		if (!base.isServer)
		{
			this.JM.CheckJobProgress(false);
		}
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x00090651 File Offset: 0x0008E851
	protected static void InvokeUserCode_RpcSetJobStats__Single__Single__Single__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetJobStats called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcSetJobStats__Single__Single__Single__String(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadString());
	}

	// Token: 0x06000D57 RID: 3415 RVA: 0x00090690 File Offset: 0x0008E890
	protected void UserCode_CmdSpawnPlates2__String__String__String__String__String__String__Vector3(string OneNR, string TwoNR, string ThreeNR, string FourNR, string FiveNR, string SixNR, Vector3 Spot)
	{
		tools.MPNumber++;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.DUMMY, this.Spawnposition, this.Spawnrotation);
		gameObject.GetComponent<networkDummy>().NetworkItemname = this.NumberPlatePrefab.transform.name;
		gameObject.GetComponent<networkDummy>().NetworkSpawnposition = Spot;
		gameObject.GetComponent<networkDummy>().NetworkSpawnrotation = this.Spawnrotation;
		gameObject.GetComponent<networkDummy>().NetworkMPNumber = tools.MPNumber;
		NetworkServer.Spawn(gameObject, base.connectionToClient);
		tools.MPNumber++;
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.DUMMY, this.Spawnposition, this.Spawnrotation);
		gameObject2.GetComponent<networkDummy>().NetworkItemname = this.NumberPlatePrefab.transform.name;
		gameObject2.GetComponent<networkDummy>().NetworkSpawnposition = Spot;
		gameObject2.GetComponent<networkDummy>().NetworkSpawnrotation = this.Spawnrotation;
		gameObject2.GetComponent<networkDummy>().NetworkMPNumber = tools.MPNumber;
		NetworkServer.Spawn(gameObject2, base.connectionToClient);
		this.RpcSpawnPlates2(OneNR, TwoNR, ThreeNR, FourNR, FiveNR, SixNR, Spot, tools.MPNumber);
	}

	// Token: 0x06000D58 RID: 3416 RVA: 0x000907A4 File Offset: 0x0008E9A4
	protected static void InvokeUserCode_CmdSpawnPlates2__String__String__String__String__String__String__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSpawnPlates2 called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdSpawnPlates2__String__String__String__String__String__String__Vector3(reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadVector3());
	}

	// Token: 0x06000D59 RID: 3417 RVA: 0x000907FC File Offset: 0x0008E9FC
	protected void UserCode_RpcSpawnPlates2__String__String__String__String__String__String__Vector3__Int32(string OneNR, string TwoNR, string ThreeNR, string FourNR, string FiveNR, string SixNR, Vector3 Spot, int mpNr)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.NumberPlatePrefab, Spot, Quaternion.identity);
		gameObject.transform.name = this.NumberPlatePrefab.transform.name;
		gameObject.GetComponent<Partinfo>().Creating();
		gameObject.GetComponent<CarProperties>().One = (Resources.Load(OneNR, typeof(Material)) as Material);
		gameObject.GetComponent<CarProperties>().Two = (Resources.Load(TwoNR, typeof(Material)) as Material);
		gameObject.GetComponent<CarProperties>().Three = (Resources.Load(ThreeNR, typeof(Material)) as Material);
		gameObject.GetComponent<CarProperties>().Four = (Resources.Load(FourNR, typeof(Material)) as Material);
		gameObject.GetComponent<CarProperties>().Five = (Resources.Load(FiveNR, typeof(Material)) as Material);
		gameObject.GetComponent<CarProperties>().Six = (Resources.Load(SixNR, typeof(Material)) as Material);
		gameObject.GetComponent<CarProperties>().LoadNumber();
		gameObject.gameObject.AddComponent<MPobject>();
		gameObject.gameObject.GetComponent<MPobject>().MPNumber = mpNr - 1;
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, Spot, Quaternion.identity);
		gameObject2.transform.name = gameObject.transform.name;
		gameObject2.gameObject.GetComponent<MPobject>().MPNumber = mpNr;
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x00090964 File Offset: 0x0008EB64
	protected static void InvokeUserCode_RpcSpawnPlates2__String__String__String__String__String__String__Vector3__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSpawnPlates2 called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcSpawnPlates2__String__String__String__String__String__String__Vector3__Int32(reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadVector3(), reader.ReadInt());
	}

	// Token: 0x06000D5B RID: 3419 RVA: 0x000909C2 File Offset: 0x0008EBC2
	protected void UserCode_CmdShiftWorld__Vector3(Vector3 cameraPosition)
	{
		this.RpcShiftWorld(cameraPosition);
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x000909CB File Offset: 0x0008EBCB
	protected static void InvokeUserCode_CmdShiftWorld__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdShiftWorld called on client.");
			return;
		}
		((NetworkPLayer)obj).UserCode_CmdShiftWorld__Vector3(reader.ReadVector3());
	}

	// Token: 0x06000D5D RID: 3421 RVA: 0x000909F4 File Offset: 0x0008EBF4
	protected void UserCode_RpcShiftWorld__Vector3(Vector3 cameraPosition)
	{
		this.RealPlayer.transform.parent.GetComponent<FloatingPoinMyfix>().ResetPosition2(cameraPosition);
	}

	// Token: 0x06000D5E RID: 3422 RVA: 0x00090A11 File Offset: 0x0008EC11
	protected static void InvokeUserCode_RpcShiftWorld__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcShiftWorld called on server.");
			return;
		}
		((NetworkPLayer)obj).UserCode_RpcShiftWorld__Vector3(reader.ReadVector3());
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x00090A3C File Offset: 0x0008EC3C
	static NetworkPLayer()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdClientJoined()", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdClientJoined), true);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdClientLoaded()", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdClientLoaded), true);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdLoadScene()", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdLoadScene), true);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdPackegecame()", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdPackegecame), true);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdPickupItem(Mirror.NetworkIdentity)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdPickupItem__NetworkIdentity), false);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::Cmdputdown(Mirror.NetworkIdentity)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_Cmdputdown__NetworkIdentity), true);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdSpawnItem(System.String,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Color,System.Boolean,System.Int32)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdSpawnItem__String__Vector3__Quaternion__Color__Boolean__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdSpawn(System.String)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdSpawn__String), true);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdCarSave(System.Byte[],System.String)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdCarSave__Byte[]__String), true);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdSpawnTrailer(System.String,UnityEngine.Vector3)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdSpawnTrailer__String__Vector3), false);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdSpawnMoreCars(System.Int32,UnityEngine.Vector3,UnityEngine.Vector3,System.Int32,System.Int32)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdSpawnMoreCars__Int32__Vector3__Vector3__Int32__Int32), false);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdSit(networkDummy)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdSit__networkDummy), false);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdStand(networkDummy)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdStand__networkDummy), false);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdDidntCome()", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdDidntCome), false);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdSpawnJob(System.Int32,System.String,System.Int32)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdSpawnJob__Int32__String__Int32), false);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdSetJobStats(System.Single,System.Single,System.Single,System.String)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdSetJobStats__Single__Single__Single__String), false);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdSpawnPlates2(System.String,System.String,System.String,System.String,System.String,System.String,UnityEngine.Vector3)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdSpawnPlates2__String__String__String__String__String__String__Vector3), false);
		RemoteProcedureCalls.RegisterCommand(typeof(NetworkPLayer), "System.Void NetworkPLayer::CmdShiftWorld(UnityEngine.Vector3)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_CmdShiftWorld__Vector3), false);
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcClientJoined()", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcClientJoined));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcClientLoaded()", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcClientLoaded));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcReceiveAdditionalData(System.Byte[],System.String)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcReceiveAdditionalData__Byte[]__String));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcPackegecame()", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcPackegecame));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcLoadAdditionalScene()", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcLoadAdditionalScene));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcPickupItem(Mirror.NetworkIdentity)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcPickupItem__NetworkIdentity));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::Rpcputdown(Mirror.NetworkIdentity)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_Rpcputdown__NetworkIdentity));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcSpawnItem(System.String,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Color,System.Boolean,System.Int32,System.Int32)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcSpawnItem__String__Vector3__Quaternion__Color__Boolean__Int32__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcCarSave(System.Byte[],System.String)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcCarSave__Byte[]__String));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcSpawnTrailer(System.String,UnityEngine.Vector3,System.Int32)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcSpawnTrailer__String__Vector3__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcSpawnMoreCars(System.Int32,UnityEngine.Vector3,UnityEngine.Vector3,System.Int32,System.Int32)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcSpawnMoreCars__Int32__Vector3__Vector3__Int32__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcSit(networkDummy)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcSit__networkDummy));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcStand(networkDummy)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcStand__networkDummy));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcDidntCome()", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcDidntCome));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcSpawnJob(System.Int32,System.String,System.Int32)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcSpawnJob__Int32__String__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcSetJobStats(System.Single,System.Single,System.Single,System.String)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcSetJobStats__Single__Single__Single__String));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcSpawnPlates2(System.String,System.String,System.String,System.String,System.String,System.String,UnityEngine.Vector3,System.Int32)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcSpawnPlates2__String__String__String__String__String__String__Vector3__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcShiftWorld(UnityEngine.Vector3)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcShiftWorld__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcCleanFolder()", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcCleanFolder));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcReceiveData(System.Byte[],System.String)", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcReceiveData__Byte[]__String));
		RemoteProcedureCalls.RegisterRpc(typeof(NetworkPLayer), "System.Void NetworkPLayer::RpcLoadScene()", new RemoteCallDelegate(NetworkPLayer.InvokeUserCode_RpcLoadScene));
	}

	// Token: 0x06000D60 RID: 3424 RVA: 0x00090F3C File Offset: 0x0008F13C
	public override bool SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteColor32(this.colorpaint);
			return true;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteColor32(this.colorpaint);
			result = true;
		}
		return result;
	}

	// Token: 0x06000D61 RID: 3425 RVA: 0x00090F98 File Offset: 0x0008F198
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<Color32>(ref this.colorpaint, null, reader.ReadColor32());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<Color32>(ref this.colorpaint, null, reader.ReadColor32());
		}
	}

	// Token: 0x040015A5 RID: 5541
	public Saver Saver;

	// Token: 0x040015A6 RID: 5542
	public GameObject RealPlayer;

	// Token: 0x040015A7 RID: 5543
	public GameObject LADcoupePR;

	// Token: 0x040015A8 RID: 5544
	public GameObject LADPR;

	// Token: 0x040015A9 RID: 5545
	public GameObject LADcabrioPR;

	// Token: 0x040015AA RID: 5546
	public GameObject NIVPR;

	// Token: 0x040015AB RID: 5547
	public GameObject ChadPR;

	// Token: 0x040015AC RID: 5548
	public GameObject BartPR;

	// Token: 0x040015AD RID: 5549
	public GameObject L500PR;

	// Token: 0x040015AE RID: 5550
	public GameObject HardyPR;

	// Token: 0x040015AF RID: 5551
	public GameObject JessePR;

	// Token: 0x040015B0 RID: 5552
	public GameObject WolfPR;

	// Token: 0x040015B1 RID: 5553
	public GameObject TrailerCar;

	// Token: 0x040015B2 RID: 5554
	public GameObject TrailerLong;

	// Token: 0x040015B3 RID: 5555
	public GameObject TrailerShort;

	// Token: 0x040015B4 RID: 5556
	public GameObject dummy;

	// Token: 0x040015B5 RID: 5557
	public GameObject DUMMY;

	// Token: 0x040015B6 RID: 5558
	public GameObject ITEM;

	// Token: 0x040015B7 RID: 5559
	public GameObject NumberPlatePrefab;

	// Token: 0x040015B8 RID: 5560
	public string Itemname;

	// Token: 0x040015B9 RID: 5561
	public Vector3 Spawnposition;

	// Token: 0x040015BA RID: 5562
	public Quaternion Spawnrotation;

	// Token: 0x040015BB RID: 5563
	public GameObject Avatar1;

	// Token: 0x040015BC RID: 5564
	public GameObject Avatar2;

	// Token: 0x040015BD RID: 5565
	public GameObject Avatar;

	// Token: 0x040015BE RID: 5566
	public GameObject AvatarRightHandTarget;

	// Token: 0x040015BF RID: 5567
	public GameObject AvatarLefttHandTarget;

	// Token: 0x040015C0 RID: 5568
	public Rig RHRig;

	// Token: 0x040015C1 RID: 5569
	public Rig LHRig;

	// Token: 0x040015C2 RID: 5570
	public Animator m_Animator;

	// Token: 0x040015C3 RID: 5571
	private Vector3 previous;

	// Token: 0x040015C4 RID: 5572
	public float walkSpeed;

	// Token: 0x040015C5 RID: 5573
	public GameObject Car;

	// Token: 0x040015C6 RID: 5574
	public byte[] CarSave;

	// Token: 0x040015C7 RID: 5575
	public string SaveName;

	// Token: 0x040015C8 RID: 5576
	public string CarName;

	// Token: 0x040015C9 RID: 5577
	public GameObject CarList;

	// Token: 0x040015CA RID: 5578
	public GameObject[] Cars;

	// Token: 0x040015CB RID: 5579
	public JobsManager JM;

	// Token: 0x040015CC RID: 5580
	[SyncVar]
	public Color32 colorpaint;

	// Token: 0x040015CD RID: 5581
	public bool PackageCame;
}
