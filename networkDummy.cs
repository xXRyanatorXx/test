using System;
using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using NWH.VehiclePhysics2;
using RVP;
using UnityEngine;

// Token: 0x02000291 RID: 657
public class networkDummy : NetworkBehaviour
{
	// Token: 0x06000F5A RID: 3930 RVA: 0x0009F5E0 File Offset: 0x0009D7E0
	private void Start()
	{
		if (this.MPNumber > tools.MPNumber)
		{
			tools.MPNumber = this.MPNumber;
		}
		if (base.hasAuthority)
		{
			base.transform.position = this.Spawnposition;
			base.transform.rotation = this.Spawnrotation;
		}
		if (this.Target == null)
		{
			if (this.SceneNumber > 0 && !this.Target)
			{
				foreach (SavePosition savePosition in UnityEngine.Object.FindObjectsOfType<SavePosition>())
				{
					if (savePosition.SceneNumber == this.SceneNumber)
					{
						this.Target = savePosition.gameObject;
						this.Target.AddComponent<MPobject>();
						this.Target.GetComponent<MPobject>().MPNumber = this.MPNumber;
					}
				}
			}
			if (!this.Target)
			{
				foreach (MPobject mpobject in UnityEngine.Object.FindObjectsOfType<MPobject>())
				{
					if (mpobject.MPNumber == this.MPNumber && mpobject.networkDummy == null)
					{
						this.Target = mpobject.gameObject;
						mpobject.networkDummy = this;
					}
				}
			}
		}
		if (this.Target && !this.initialized)
		{
			this.Start2();
		}
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x0009F720 File Offset: 0x0009D920
	public void Start2()
	{
		this.Target.GetComponent<MPobject>().networkDummy = this;
		this.Target.GetComponent<MPobject>().MPNumber = this.MPNumber;
		if (this.Target.GetComponent<VehicleDamage>())
		{
			this.DamageScript = this.Target.GetComponent<VehicleDamage>();
			this.Target.GetComponent<VehicleDamage>().networkDummy = this;
		}
		if (!this.Target.GetComponent<MainCarProperties>() && !this.Target.GetComponent<MainTrailerProperties>() && !(this.Target.transform.name == "MapMagic"))
		{
			base.GetComponent<NetworkTransform>().enabled = false;
			base.enabled = false;
		}
		if (this.Target.GetComponent<VehicleController>())
		{
			this._vehicleController = this.Target.GetComponent<VehicleController>();
			this.maincarproperties = this.Target.GetComponent<MainCarProperties>();
		}
		if (this.Target && !this.Target.transform.root.GetComponent<MainCarProperties>())
		{
			this.initialized = true;
			return;
		}
		base.StartCoroutine(this.Initialize());
	}

	// Token: 0x06000F5C RID: 3932 RVA: 0x0009F84D File Offset: 0x0009DA4D
	public IEnumerator Initialize()
	{
		yield return new WaitForSeconds(5f);
		if (this.Target)
		{
			this.initialized = true;
		}
		if (tools._MapMagic != null)
		{
			this.mapmagic = tools._MapMagic.transform;
		}
		yield break;
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x0009F85C File Offset: 0x0009DA5C
	private void FixedUpdate()
	{
		if (!this.initialized)
		{
			return;
		}
		if (this.Target)
		{
			if (base.hasAuthority)
			{
				if (this.mapmagic)
				{
					base.transform.position = this.Target.transform.position - this.mapmagic.position;
					base.transform.rotation = this.Target.transform.rotation;
				}
				else
				{
					base.transform.position = this.Target.transform.position;
					base.transform.rotation = this.Target.transform.rotation;
				}
				if (this._vehicleController && this.maincarproperties.SittingInCar)
				{
					this.CmdInputs(this._vehicleController.input.Steering, this._vehicleController.input.Brakes, this._vehicleController.powertrain.engine.angularVelocity, this._vehicleController.Speed);
					return;
				}
			}
			else
			{
				if (this.mapmagic)
				{
					this.Target.transform.position = base.transform.position + this.mapmagic.position;
					this.Target.transform.rotation = base.transform.rotation;
					return;
				}
				this.Target.transform.position = base.transform.position;
				this.Target.transform.rotation = base.transform.rotation;
			}
		}
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x0009FA0C File Offset: 0x0009DC0C
	[Command]
	private void CmdInputs(float value, float value2, float value3, float value4)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteFloat(value);
		writer.WriteFloat(value2);
		writer.WriteFloat(value3);
		writer.WriteFloat(value4);
		base.SendCommandInternal("System.Void networkDummy::CmdInputs(System.Single,System.Single,System.Single,System.Single)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x0009FA60 File Offset: 0x0009DC60
	[ClientRpc]
	private void RpcInputs(float value, float value2, float value3, float value4)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteFloat(value);
		writer.WriteFloat(value2);
		writer.WriteFloat(value3);
		writer.WriteFloat(value4);
		this.SendRPCInternal("System.Void networkDummy::RpcInputs(System.Single,System.Single,System.Single,System.Single)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x0009FAB4 File Offset: 0x0009DCB4
	public void DamageApplication(Vector3 damagePoint1, Vector3 damagePoint2, Vector3 damagePoint3, Vector3 damagePoint4, Vector3 damagePoint5, Vector3 damageForce, float damageForceLimit, Vector3 surfaceNormal, float hitAngle)
	{
		this.CmdCrash(damagePoint1, damagePoint2, damagePoint3, damagePoint4, damagePoint5, damageForce, damageForceLimit, surfaceNormal, hitAngle);
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x0009FAD8 File Offset: 0x0009DCD8
	[Command]
	private void CmdCrash(Vector3 damagePoint1, Vector3 damagePoint2, Vector3 damagePoint3, Vector3 damagePoint4, Vector3 damagePoint5, Vector3 damageForce, float damageForceLimit, Vector3 surfaceNormal, float hitAngle)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteVector3(damagePoint1);
		writer.WriteVector3(damagePoint2);
		writer.WriteVector3(damagePoint3);
		writer.WriteVector3(damagePoint4);
		writer.WriteVector3(damagePoint5);
		writer.WriteVector3(damageForce);
		writer.WriteFloat(damageForceLimit);
		writer.WriteVector3(surfaceNormal);
		writer.WriteFloat(hitAngle);
		base.SendCommandInternal("System.Void networkDummy::CmdCrash(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,System.Single,UnityEngine.Vector3,System.Single)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x0009FB60 File Offset: 0x0009DD60
	[ClientRpc]
	private void RpcCrash(Vector3 damagePoint1, Vector3 damagePoint2, Vector3 damagePoint3, Vector3 damagePoint4, Vector3 damagePoint5, Vector3 damageForce, float damageForceLimit, Vector3 surfaceNormal, float hitAngle)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteVector3(damagePoint1);
		writer.WriteVector3(damagePoint2);
		writer.WriteVector3(damagePoint3);
		writer.WriteVector3(damagePoint4);
		writer.WriteVector3(damagePoint5);
		writer.WriteVector3(damageForce);
		writer.WriteFloat(damageForceLimit);
		writer.WriteVector3(surfaceNormal);
		writer.WriteFloat(hitAngle);
		this.SendRPCInternal("System.Void networkDummy::RpcCrash(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,System.Single,UnityEngine.Vector3,System.Single)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x0009FBE8 File Offset: 0x0009DDE8
	[Command]
	private void CmdDestroyItem()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdDestroyItem()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x0009FC13 File Offset: 0x0009DE13
	public void RemoveWindow()
	{
		tools.NetworkPLayer.pickup(this);
		this.CmdRemoveWindow();
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x0009FC28 File Offset: 0x0009DE28
	[Command(requiresAuthority = false)]
	private void CmdRemoveWindow()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdRemoveWindow()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x0009FC54 File Offset: 0x0009DE54
	[ClientRpc]
	private void RpcRemoveWindow()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcRemoveWindow()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x0009FC7F File Offset: 0x0009DE7F
	public void ReducePickupToolInBox()
	{
		this.CmdReducePickupToolInBox();
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x0009FC88 File Offset: 0x0009DE88
	[Command(requiresAuthority = false)]
	private void CmdReducePickupToolInBox()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdReducePickupToolInBox()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x0009FCB4 File Offset: 0x0009DEB4
	[ClientRpc]
	private void RpcReducePickupToolInBox()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcReducePickupToolInBox()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x0009FCDF File Offset: 0x0009DEDF
	public void RemoveHand()
	{
		tools.NetworkPLayer.pickup(this);
		this.CmdRemoveHand();
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x0009FCF4 File Offset: 0x0009DEF4
	[Command(requiresAuthority = false)]
	private void CmdRemoveHand()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdRemoveHand()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x0009FD20 File Offset: 0x0009DF20
	[ClientRpc]
	private void RpcRemoveHand()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcRemoveHand()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x0009FD4B File Offset: 0x0009DF4B
	public void DropOnGround()
	{
		this.CmdDropOnGround();
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x0009FD54 File Offset: 0x0009DF54
	[Command(requiresAuthority = false)]
	private void CmdDropOnGround()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdDropOnGround()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x0009FD80 File Offset: 0x0009DF80
	[ClientRpc]
	private void RpcDropOnGround()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcDropOnGround()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x0009FDAB File Offset: 0x0009DFAB
	public void StartMounting()
	{
		this.CmdStartMounting();
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x0009FDB4 File Offset: 0x0009DFB4
	[Command(requiresAuthority = false)]
	private void CmdStartMounting()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdStartMounting()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x0009FDE0 File Offset: 0x0009DFE0
	[ClientRpc]
	private void RpcStartMounting()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcStartMounting()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x0009FE0B File Offset: 0x0009E00B
	public void opendoor(bool open)
	{
		this.Cmdopendoor(open);
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x0009FE14 File Offset: 0x0009E014
	[Command(requiresAuthority = false)]
	private void Cmdopendoor(bool open)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteBool(open);
		base.SendCommandInternal("System.Void networkDummy::Cmdopendoor(System.Boolean)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F75 RID: 3957 RVA: 0x0009FE4C File Offset: 0x0009E04C
	[ClientRpc]
	private void Rpcopendoor(bool open)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteBool(open);
		this.SendRPCInternal("System.Void networkDummy::Rpcopendoor(System.Boolean)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F76 RID: 3958 RVA: 0x0009FE81 File Offset: 0x0009E081
	public void PlayParticles()
	{
		this.CmdPlayParticles();
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x0009FE8C File Offset: 0x0009E08C
	[Command(requiresAuthority = false)]
	private void CmdPlayParticles()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdPlayParticles()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x0009FEB8 File Offset: 0x0009E0B8
	[ClientRpc]
	private void RpcPlayParticles()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcPlayParticles()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x0009FEE3 File Offset: 0x0009E0E3
	public void StopParticles()
	{
		this.CmdStopParticles();
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x0009FEEC File Offset: 0x0009E0EC
	[Command(requiresAuthority = false)]
	private void CmdStopParticles()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdStopParticles()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x0009FF18 File Offset: 0x0009E118
	[ClientRpc]
	private void RpcStopParticles()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcStopParticles()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x0009FF43 File Offset: 0x0009E143
	public void PumpIn()
	{
		this.CmdPumpIn();
	}

	// Token: 0x06000F7D RID: 3965 RVA: 0x0009FF4C File Offset: 0x0009E14C
	[Command(requiresAuthority = false)]
	private void CmdPumpIn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdPumpIn()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x0009FF78 File Offset: 0x0009E178
	[ClientRpc]
	private void RpcPumpIn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcPumpIn()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x0009FFA3 File Offset: 0x0009E1A3
	public void PumpOut()
	{
		this.CmdPumpOut();
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x0009FFAC File Offset: 0x0009E1AC
	[Command(requiresAuthority = false)]
	private void CmdPumpOut()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdPumpOut()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F81 RID: 3969 RVA: 0x0009FFD8 File Offset: 0x0009E1D8
	[ClientRpc]
	private void RpcPumpOut()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcPumpOut()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F82 RID: 3970 RVA: 0x000A0003 File Offset: 0x0009E203
	public void DestroyJoint()
	{
		this.CmdDestroyJoint();
	}

	// Token: 0x06000F83 RID: 3971 RVA: 0x000A000C File Offset: 0x0009E20C
	[Command(requiresAuthority = false)]
	private void CmdDestroyJoint()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdDestroyJoint()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x000A0038 File Offset: 0x0009E238
	[ClientRpc]
	private void RpcDestroyJoint()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcDestroyJoint()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x000A0063 File Offset: 0x0009E263
	public void OpenGasCup(bool open)
	{
		this.CmdOpenGasCup(open);
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x000A006C File Offset: 0x0009E26C
	[Command(requiresAuthority = false)]
	private void CmdOpenGasCup(bool open)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteBool(open);
		base.SendCommandInternal("System.Void networkDummy::CmdOpenGasCup(System.Boolean)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x000A00A4 File Offset: 0x0009E2A4
	[ClientRpc]
	private void RpcOpenGasCup(bool open)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteBool(open);
		this.SendRPCInternal("System.Void networkDummy::RpcOpenGasCup(System.Boolean)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x000A00D9 File Offset: 0x0009E2D9
	public void UpdatePickupitems()
	{
		this.CmdUpdatePickupitems();
	}

	// Token: 0x06000F89 RID: 3977 RVA: 0x000A00E4 File Offset: 0x0009E2E4
	[Command(requiresAuthority = false)]
	private void CmdUpdatePickupitems()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdUpdatePickupitems()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x000A0110 File Offset: 0x0009E310
	[ClientRpc]
	private void RpcUpdatePickupitems()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcUpdatePickupitems()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x000A013B File Offset: 0x0009E33B
	public void PickupToolUPDATE(int paintlife, bool updatevisual)
	{
		this.CmdPickupToolUPDATE(paintlife, updatevisual);
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x000A0148 File Offset: 0x0009E348
	[Command(requiresAuthority = false)]
	private void CmdPickupToolUPDATE(int paintlife, bool updatevisual)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(paintlife);
		writer.WriteBool(updatevisual);
		base.SendCommandInternal("System.Void networkDummy::CmdPickupToolUPDATE(System.Int32,System.Boolean)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x000A0188 File Offset: 0x0009E388
	[ClientRpc]
	private void RpcPickupToolUPDATE(int paintlife, bool updatevisual)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(paintlife);
		writer.WriteBool(updatevisual);
		this.SendRPCInternal("System.Void networkDummy::RpcPickupToolUPDATE(System.Int32,System.Boolean)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x000A01C7 File Offset: 0x0009E3C7
	public void PickupToolSync(float paintlife)
	{
		this.CmdPickupToolSync(paintlife);
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x000A01D0 File Offset: 0x0009E3D0
	[Command(requiresAuthority = false)]
	private void CmdPickupToolSync(float paintlife)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteFloat(paintlife);
		base.SendCommandInternal("System.Void networkDummy::CmdPickupToolSync(System.Single)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x000A0208 File Offset: 0x0009E408
	[ClientRpc]
	private void RpcPickupToolSync(float paintlife)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteFloat(paintlife);
		this.SendRPCInternal("System.Void networkDummy::RpcPickupToolSync(System.Single)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x000A023D File Offset: 0x0009E43D
	public void FlashLight(bool on)
	{
		this.CmdFlashLight(on);
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x000A0248 File Offset: 0x0009E448
	[Command(requiresAuthority = false)]
	private void CmdFlashLight(bool on)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteBool(on);
		base.SendCommandInternal("System.Void networkDummy::CmdFlashLight(System.Boolean)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x000A0280 File Offset: 0x0009E480
	[ClientRpc]
	private void RpcFlashLight(bool on)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteBool(on);
		this.SendRPCInternal("System.Void networkDummy::RpcFlashLight(System.Boolean)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F94 RID: 3988 RVA: 0x000A02B5 File Offset: 0x0009E4B5
	public void Tint2(int level)
	{
		this.CmdTint2(level);
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x000A02C0 File Offset: 0x0009E4C0
	[Command(requiresAuthority = false)]
	private void CmdTint2(int level)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(level);
		base.SendCommandInternal("System.Void networkDummy::CmdTint2(System.Int32)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x000A02F8 File Offset: 0x0009E4F8
	[ClientRpc]
	private void RpcTint2(int level)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(level);
		this.SendRPCInternal("System.Void networkDummy::RpcTint2(System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x000A032D File Offset: 0x0009E52D
	public void Tint3(int level)
	{
		this.CmdTint3(level);
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x000A0338 File Offset: 0x0009E538
	[Command(requiresAuthority = false)]
	private void CmdTint3(int level)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(level);
		base.SendCommandInternal("System.Void networkDummy::CmdTint3(System.Int32)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x000A0370 File Offset: 0x0009E570
	[ClientRpc]
	private void RpcTint3(int level)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(level);
		this.SendRPCInternal("System.Void networkDummy::RpcTint3(System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F9A RID: 3994 RVA: 0x000A03A5 File Offset: 0x0009E5A5
	public void Fair2(Vector3 point, bool preview, int priority, float pressure, int seed, Quaternion rotation, Vector3 hit)
	{
		this.CmdFair2(point, preview, priority, pressure, seed, rotation, hit);
	}

	// Token: 0x06000F9B RID: 3995 RVA: 0x000A03B8 File Offset: 0x0009E5B8
	[Command(requiresAuthority = false)]
	private void CmdFair2(Vector3 point, bool preview, int priority, float pressure, int seed, Quaternion rotation, Vector3 hit)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteVector3(point);
		writer.WriteBool(preview);
		writer.WriteInt(priority);
		writer.WriteFloat(pressure);
		writer.WriteInt(seed);
		writer.WriteQuaternion(rotation);
		writer.WriteVector3(hit);
		base.SendCommandInternal("System.Void networkDummy::CmdFair2(UnityEngine.Vector3,System.Boolean,System.Int32,System.Single,System.Int32,UnityEngine.Quaternion,UnityEngine.Vector3)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x000A042C File Offset: 0x0009E62C
	[ClientRpc]
	private void RpcFair2(Vector3 point, bool preview, int priority, float pressure, int seed, Quaternion rotation, Vector3 hit)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteVector3(point);
		writer.WriteBool(preview);
		writer.WriteInt(priority);
		writer.WriteFloat(pressure);
		writer.WriteInt(seed);
		writer.WriteQuaternion(rotation);
		writer.WriteVector3(hit);
		this.SendRPCInternal("System.Void networkDummy::RpcFair2(UnityEngine.Vector3,System.Boolean,System.Int32,System.Single,System.Int32,UnityEngine.Quaternion,UnityEngine.Vector3)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x000A049D File Offset: 0x0009E69D
	public void Repair1(Vector3 point)
	{
		this.CmdRepair1(point);
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x000A04A8 File Offset: 0x0009E6A8
	[Command(requiresAuthority = false)]
	private void CmdRepair1(Vector3 point)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteVector3(point);
		base.SendCommandInternal("System.Void networkDummy::CmdRepair1(UnityEngine.Vector3)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000F9F RID: 3999 RVA: 0x000A04E0 File Offset: 0x0009E6E0
	[ClientRpc]
	private void RpcRepair1(Vector3 point)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteVector3(point);
		this.SendRPCInternal("System.Void networkDummy::RpcRepair1(UnityEngine.Vector3)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x000A0515 File Offset: 0x0009E715
	public void Repair22(Vector3 point)
	{
		this.CmdRepair22(point);
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x000A0520 File Offset: 0x0009E720
	[Command(requiresAuthority = false)]
	private void CmdRepair22(Vector3 point)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteVector3(point);
		base.SendCommandInternal("System.Void networkDummy::CmdRepair22(UnityEngine.Vector3)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x000A0558 File Offset: 0x0009E758
	[ClientRpc]
	private void RpcRepair22(Vector3 point)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteVector3(point);
		this.SendRPCInternal("System.Void networkDummy::RpcRepair22(UnityEngine.Vector3)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x000A058D File Offset: 0x0009E78D
	public void RustRemoveContinue(bool preview, int priority, float pressure, int seed, Quaternion rotation, Vector3 hit)
	{
		this.CmdRustRemoveContinue(preview, priority, pressure, seed, rotation, hit);
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x000A05A0 File Offset: 0x0009E7A0
	[Command(requiresAuthority = false)]
	private void CmdRustRemoveContinue(bool preview, int priority, float pressure, int seed, Quaternion rotation, Vector3 hit)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteBool(preview);
		writer.WriteInt(priority);
		writer.WriteFloat(pressure);
		writer.WriteInt(seed);
		writer.WriteQuaternion(rotation);
		writer.WriteVector3(hit);
		base.SendCommandInternal("System.Void networkDummy::CmdRustRemoveContinue(System.Boolean,System.Int32,System.Single,System.Int32,UnityEngine.Quaternion,UnityEngine.Vector3)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FA5 RID: 4005 RVA: 0x000A0608 File Offset: 0x0009E808
	[ClientRpc]
	private void RpcRustRemoveContinue(bool preview, int priority, float pressure, int seed, Quaternion rotation, Vector3 hit)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteBool(preview);
		writer.WriteInt(priority);
		writer.WriteFloat(pressure);
		writer.WriteInt(seed);
		writer.WriteQuaternion(rotation);
		writer.WriteVector3(hit);
		this.SendRPCInternal("System.Void networkDummy::RpcRustRemoveContinue(System.Boolean,System.Int32,System.Single,System.Int32,UnityEngine.Quaternion,UnityEngine.Vector3)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x000A066F File Offset: 0x0009E86F
	public void LiftObject(int steps, Vector3 position)
	{
		this.CmdLiftObject(steps, position);
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x000A067C File Offset: 0x0009E87C
	[Command(requiresAuthority = false)]
	private void CmdLiftObject(int steps, Vector3 position)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(steps);
		writer.WriteVector3(position);
		base.SendCommandInternal("System.Void networkDummy::CmdLiftObject(System.Int32,UnityEngine.Vector3)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x000A06BC File Offset: 0x0009E8BC
	[ClientRpc]
	private void RpcLiftObject(int steps, Vector3 position)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(steps);
		writer.WriteVector3(position);
		this.SendRPCInternal("System.Void networkDummy::RpcLiftObject(System.Int32,UnityEngine.Vector3)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x000A06FB File Offset: 0x0009E8FB
	public void tighten(int childnumber1)
	{
		this.Cmdtighten(childnumber1);
	}

	// Token: 0x06000FAA RID: 4010 RVA: 0x000A0704 File Offset: 0x0009E904
	[Command(requiresAuthority = false)]
	private void Cmdtighten(int childnumber1)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(childnumber1);
		base.SendCommandInternal("System.Void networkDummy::Cmdtighten(System.Int32)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FAB RID: 4011 RVA: 0x000A073C File Offset: 0x0009E93C
	[ClientRpc]
	private void Rpctighten(int childnumber1)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(childnumber1);
		this.SendRPCInternal("System.Void networkDummy::Rpctighten(System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x000A0771 File Offset: 0x0009E971
	public void Loosen(int childnumber1)
	{
		this.CmdLoosen(childnumber1);
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x000A077C File Offset: 0x0009E97C
	[Command(requiresAuthority = false)]
	private void CmdLoosen(int childnumber1)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(childnumber1);
		base.SendCommandInternal("System.Void networkDummy::CmdLoosen(System.Int32)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x000A07B4 File Offset: 0x0009E9B4
	[ClientRpc]
	private void RpcLoosen(int childnumber1)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(childnumber1);
		this.SendRPCInternal("System.Void networkDummy::RpcLoosen(System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x000A07E9 File Offset: 0x0009E9E9
	public void Cut(int childnumber1, Vector3 point)
	{
		this.CmdCut(childnumber1, point);
	}

	// Token: 0x06000FB0 RID: 4016 RVA: 0x000A07F4 File Offset: 0x0009E9F4
	[Command(requiresAuthority = false)]
	private void CmdCut(int childnumber1, Vector3 point)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(childnumber1);
		writer.WriteVector3(point);
		base.SendCommandInternal("System.Void networkDummy::CmdCut(System.Int32,UnityEngine.Vector3)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FB1 RID: 4017 RVA: 0x000A0834 File Offset: 0x0009EA34
	[ClientRpc]
	private void RpcCut(int childnumber1, Vector3 point)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(childnumber1);
		writer.WriteVector3(point);
		this.SendRPCInternal("System.Void networkDummy::RpcCut(System.Int32,UnityEngine.Vector3)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FB2 RID: 4018 RVA: 0x000A0873 File Offset: 0x0009EA73
	public void Weld(int childnumber1, Vector3 point)
	{
		this.CmdWeld(childnumber1, point);
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x000A0880 File Offset: 0x0009EA80
	[Command(requiresAuthority = false)]
	private void CmdWeld(int childnumber1, Vector3 point)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(childnumber1);
		writer.WriteVector3(point);
		base.SendCommandInternal("System.Void networkDummy::CmdWeld(System.Int32,UnityEngine.Vector3)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FB4 RID: 4020 RVA: 0x000A08C0 File Offset: 0x0009EAC0
	[ClientRpc]
	private void RpcWeld(int childnumber1, Vector3 point)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(childnumber1);
		writer.WriteVector3(point);
		this.SendRPCInternal("System.Void networkDummy::RpcWeld(System.Int32,UnityEngine.Vector3)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FB5 RID: 4021 RVA: 0x000A08FF File Offset: 0x0009EAFF
	public void EnableMovment()
	{
		this.CmdEnableMovment();
	}

	// Token: 0x06000FB6 RID: 4022 RVA: 0x000A0908 File Offset: 0x0009EB08
	[Command(requiresAuthority = false)]
	private void CmdEnableMovment()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdEnableMovment()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FB7 RID: 4023 RVA: 0x000A0934 File Offset: 0x0009EB34
	[ClientRpc]
	private void RpcEnableMovment()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcEnableMovment()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x000A095F File Offset: 0x0009EB5F
	public void BRAKE()
	{
		this.CmdBRAKE();
	}

	// Token: 0x06000FB9 RID: 4025 RVA: 0x000A0968 File Offset: 0x0009EB68
	[Command(requiresAuthority = false)]
	private void CmdBRAKE()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdBRAKE()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FBA RID: 4026 RVA: 0x000A0994 File Offset: 0x0009EB94
	[ClientRpc]
	private void RpcBRAKE()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcBRAKE()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FBB RID: 4027 RVA: 0x000A09BF File Offset: 0x0009EBBF
	public void AttachPickup(MPobject parentObj, int childnumber1, int childnumber2)
	{
		this.CmdAttachPickup(parentObj.MPNumber, childnumber1, childnumber2);
	}

	// Token: 0x06000FBC RID: 4028 RVA: 0x000A09D0 File Offset: 0x0009EBD0
	[Command(requiresAuthority = false)]
	private void CmdAttachPickup(int parentNR, int childnumber1, int childnumber2)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(parentNR);
		writer.WriteInt(childnumber1);
		writer.WriteInt(childnumber2);
		base.SendCommandInternal("System.Void networkDummy::CmdAttachPickup(System.Int32,System.Int32,System.Int32)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FBD RID: 4029 RVA: 0x000A0A1C File Offset: 0x0009EC1C
	[ClientRpc]
	private void RpcAttachPickup(int parentNR, int childnumber1, int childnumber2)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteInt(parentNR);
		writer.WriteInt(childnumber1);
		writer.WriteInt(childnumber2);
		this.SendRPCInternal("System.Void networkDummy::RpcAttachPickup(System.Int32,System.Int32,System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FBE RID: 4030 RVA: 0x000A0A65 File Offset: 0x0009EC65
	public void DestroyMe()
	{
		this.CmdDestroyMe();
	}

	// Token: 0x06000FBF RID: 4031 RVA: 0x000A0A70 File Offset: 0x0009EC70
	[Command(requiresAuthority = false)]
	private void CmdDestroyMe()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdDestroyMe()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FC0 RID: 4032 RVA: 0x000A0A9C File Offset: 0x0009EC9C
	[ClientRpc]
	private void RpcDestroyMe()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcDestroyMe()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FC1 RID: 4033 RVA: 0x000A0AC7 File Offset: 0x0009ECC7
	public void WinnchReeling()
	{
		this.CmdWinnchReeling();
	}

	// Token: 0x06000FC2 RID: 4034 RVA: 0x000A0AD0 File Offset: 0x0009ECD0
	[Command(requiresAuthority = false)]
	private void CmdWinnchReeling()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdWinnchReeling()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FC3 RID: 4035 RVA: 0x000A0AFC File Offset: 0x0009ECFC
	[ClientRpc]
	private void RpcWinnchReeling()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcWinnchReeling()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FC4 RID: 4036 RVA: 0x000A0B27 File Offset: 0x0009ED27
	public void WindowUp()
	{
		this.CmdWindowUp();
	}

	// Token: 0x06000FC5 RID: 4037 RVA: 0x000A0B30 File Offset: 0x0009ED30
	[Command(requiresAuthority = false)]
	private void CmdWindowUp()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdWindowUp()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FC6 RID: 4038 RVA: 0x000A0B5C File Offset: 0x0009ED5C
	[ClientRpc]
	private void RpcWindowUp()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcWindowUp()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FC7 RID: 4039 RVA: 0x000A0B87 File Offset: 0x0009ED87
	public void WindowDown()
	{
		this.CmdWindowDown();
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x000A0B90 File Offset: 0x0009ED90
	[Command(requiresAuthority = false)]
	private void CmdWindowDown()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdWindowDown()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x000A0BBC File Offset: 0x0009EDBC
	[ClientRpc]
	private void RpcWindowDown()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcWindowDown()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x000A0BE7 File Offset: 0x0009EDE7
	public void ApplyChrome2()
	{
		this.CmdApplyChrome2();
	}

	// Token: 0x06000FCB RID: 4043 RVA: 0x000A0BF0 File Offset: 0x0009EDF0
	[Command(requiresAuthority = false)]
	private void CmdApplyChrome2()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdApplyChrome2()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FCC RID: 4044 RVA: 0x000A0C1C File Offset: 0x0009EE1C
	[ClientRpc]
	private void RpcApplyChrome2()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcApplyChrome2()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FCD RID: 4045 RVA: 0x000A0C47 File Offset: 0x0009EE47
	public void SyncFluid(float FluidSize)
	{
		this.CmdSyncFluid(FluidSize);
	}

	// Token: 0x06000FCE RID: 4046 RVA: 0x000A0C50 File Offset: 0x0009EE50
	[Command(requiresAuthority = false)]
	private void CmdSyncFluid(float FluidSize)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteFloat(FluidSize);
		base.SendCommandInternal("System.Void networkDummy::CmdSyncFluid(System.Single)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FCF RID: 4047 RVA: 0x000A0C88 File Offset: 0x0009EE88
	[ClientRpc]
	private void RpcSyncFluid(float FluidSize)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteFloat(FluidSize);
		this.SendRPCInternal("System.Void networkDummy::RpcSyncFluid(System.Single)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FD0 RID: 4048 RVA: 0x000A0CBD File Offset: 0x0009EEBD
	public void RunningLightTurnOn()
	{
		this.CmdRunningLightTurnOn();
	}

	// Token: 0x06000FD1 RID: 4049 RVA: 0x000A0CC8 File Offset: 0x0009EEC8
	[Command(requiresAuthority = false)]
	private void CmdRunningLightTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdRunningLightTurnOn()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FD2 RID: 4050 RVA: 0x000A0CF4 File Offset: 0x0009EEF4
	[ClientRpc]
	private void RpcRunningLightTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcRunningLightTurnOn()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FD3 RID: 4051 RVA: 0x000A0D1F File Offset: 0x0009EF1F
	public void RunningLightTurnOff()
	{
		this.CmdRunningLightTurnOff();
	}

	// Token: 0x06000FD4 RID: 4052 RVA: 0x000A0D28 File Offset: 0x0009EF28
	[Command(requiresAuthority = false)]
	private void CmdRunningLightTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdRunningLightTurnOff()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x000A0D54 File Offset: 0x0009EF54
	[ClientRpc]
	private void RpcRunningLightTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcRunningLightTurnOff()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FD6 RID: 4054 RVA: 0x000A0D7F File Offset: 0x0009EF7F
	public void HeadLightLowTurnOn()
	{
		this.CmdHeadLightLowTurnOn();
	}

	// Token: 0x06000FD7 RID: 4055 RVA: 0x000A0D88 File Offset: 0x0009EF88
	[Command(requiresAuthority = false)]
	private void CmdHeadLightLowTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdHeadLightLowTurnOn()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x000A0DB4 File Offset: 0x0009EFB4
	[ClientRpc]
	private void RpcHeadLightLowTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcHeadLightLowTurnOn()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x000A0DDF File Offset: 0x0009EFDF
	public void HeadLightLowTurnOff()
	{
		this.CmdHeadLightLowTurnOff();
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x000A0DE8 File Offset: 0x0009EFE8
	[Command(requiresAuthority = false)]
	private void CmdHeadLightLowTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdHeadLightLowTurnOff()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x000A0E14 File Offset: 0x0009F014
	[ClientRpc]
	private void RpcHeadLightLowTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcHeadLightLowTurnOff()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x000A0E3F File Offset: 0x0009F03F
	public void HeadLightHighTurnOn()
	{
		this.CmdHeadLightHighTurnOn();
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x000A0E48 File Offset: 0x0009F048
	[Command(requiresAuthority = false)]
	private void CmdHeadLightHighTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdHeadLightHighTurnOn()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x000A0E74 File Offset: 0x0009F074
	[ClientRpc]
	private void RpcHeadLightHighTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcHeadLightHighTurnOn()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x000A0E9F File Offset: 0x0009F09F
	public void HeadLightHighTurnOff()
	{
		this.CmdHeadLightHighTurnOff();
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x000A0EA8 File Offset: 0x0009F0A8
	[Command(requiresAuthority = false)]
	private void CmdHeadLightHighTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdHeadLightHighTurnOff()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x000A0ED4 File Offset: 0x0009F0D4
	[ClientRpc]
	private void RpcHeadLightHighTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcHeadLightHighTurnOff()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FE2 RID: 4066 RVA: 0x000A0EFF File Offset: 0x0009F0FF
	public void BrakeLightTurnOn()
	{
		this.CmdBrakeLightTurnOn();
	}

	// Token: 0x06000FE3 RID: 4067 RVA: 0x000A0F08 File Offset: 0x0009F108
	[Command(requiresAuthority = false)]
	private void CmdBrakeLightTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdBrakeLightTurnOn()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FE4 RID: 4068 RVA: 0x000A0F34 File Offset: 0x0009F134
	[ClientRpc]
	private void RpcBrakeLightTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcBrakeLightTurnOn()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FE5 RID: 4069 RVA: 0x000A0F5F File Offset: 0x0009F15F
	public void BrakeLightTurnOff()
	{
		this.CmdBrakeLightTurnOff();
	}

	// Token: 0x06000FE6 RID: 4070 RVA: 0x000A0F68 File Offset: 0x0009F168
	[Command(requiresAuthority = false)]
	private void CmdBrakeLightTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdBrakeLightTurnOff()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x000A0F94 File Offset: 0x0009F194
	[ClientRpc]
	private void RpcBrakeLightTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcBrakeLightTurnOff()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FE8 RID: 4072 RVA: 0x000A0FBF File Offset: 0x0009F1BF
	public void ReverseLightTurnOn()
	{
		this.CmdReverseLightTurnOn();
	}

	// Token: 0x06000FE9 RID: 4073 RVA: 0x000A0FC8 File Offset: 0x0009F1C8
	[Command(requiresAuthority = false)]
	private void CmdReverseLightTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdReverseLightTurnOn()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FEA RID: 4074 RVA: 0x000A0FF4 File Offset: 0x0009F1F4
	[ClientRpc]
	private void RpcReverseLightTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcReverseLightTurnOn()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FEB RID: 4075 RVA: 0x000A101F File Offset: 0x0009F21F
	public void ReverseLightTurnOff()
	{
		this.CmdReverseLightTurnOff();
	}

	// Token: 0x06000FEC RID: 4076 RVA: 0x000A1028 File Offset: 0x0009F228
	[Command(requiresAuthority = false)]
	private void CmdReverseLightTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdReverseLightTurnOff()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x000A1054 File Offset: 0x0009F254
	[ClientRpc]
	private void RpcReverseLightTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcReverseLightTurnOff()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FEE RID: 4078 RVA: 0x000A107F File Offset: 0x0009F27F
	public void LeftLightTurnOn()
	{
		this.CmdLeftLightTurnOn();
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x000A1088 File Offset: 0x0009F288
	[Command(requiresAuthority = false)]
	private void CmdLeftLightTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdLeftLightTurnOn()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FF0 RID: 4080 RVA: 0x000A10B4 File Offset: 0x0009F2B4
	[ClientRpc]
	private void RpcLeftLightTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcLeftLightTurnOn()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FF1 RID: 4081 RVA: 0x000A10DF File Offset: 0x0009F2DF
	public void LeftLightTurnOff()
	{
		this.CmdLeftLightTurnOff();
	}

	// Token: 0x06000FF2 RID: 4082 RVA: 0x000A10E8 File Offset: 0x0009F2E8
	[Command(requiresAuthority = false)]
	private void CmdLeftLightTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdLeftLightTurnOff()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x000A1114 File Offset: 0x0009F314
	[ClientRpc]
	private void RpcLeftLightTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcLeftLightTurnOff()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x000A113F File Offset: 0x0009F33F
	public void RightLightTurnOn()
	{
		this.CmdRightLightTurnOn();
	}

	// Token: 0x06000FF5 RID: 4085 RVA: 0x000A1148 File Offset: 0x0009F348
	[Command(requiresAuthority = false)]
	private void CmdRightLightTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdRightLightTurnOn()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FF6 RID: 4086 RVA: 0x000A1174 File Offset: 0x0009F374
	[ClientRpc]
	private void RpcRightLightTurnOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcRightLightTurnOn()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x000A119F File Offset: 0x0009F39F
	public void RightLightTurnOff()
	{
		this.CmdRightLightTurnOff();
	}

	// Token: 0x06000FF8 RID: 4088 RVA: 0x000A11A8 File Offset: 0x0009F3A8
	[Command(requiresAuthority = false)]
	private void CmdRightLightTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdRightLightTurnOff()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FF9 RID: 4089 RVA: 0x000A11D4 File Offset: 0x0009F3D4
	[ClientRpc]
	private void RpcRightLightTurnOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcRightLightTurnOff()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x000A11FF File Offset: 0x0009F3FF
	public void WiperSOn()
	{
		this.CmdWiperSOn();
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x000A1208 File Offset: 0x0009F408
	[Command(requiresAuthority = false)]
	private void CmdWiperSOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdWiperSOn()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x000A1234 File Offset: 0x0009F434
	[ClientRpc]
	private void RpcWiperSOn()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcWiperSOn()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x000A125F File Offset: 0x0009F45F
	public void WiperOff()
	{
		this.CmdWiperOff();
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x000A1268 File Offset: 0x0009F468
	[Command(requiresAuthority = false)]
	private void CmdWiperOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdWiperOff()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06000FFF RID: 4095 RVA: 0x000A1294 File Offset: 0x0009F494
	[ClientRpc]
	private void RpcWiperOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcWiperOff()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x000A12BF File Offset: 0x0009F4BF
	public void IgnitionTurnedOff()
	{
		this.CmdIgnitionTurnedOff();
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x000A12C8 File Offset: 0x0009F4C8
	[Command(requiresAuthority = false)]
	private void CmdIgnitionTurnedOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdIgnitionTurnedOff()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001002 RID: 4098 RVA: 0x000A12F4 File Offset: 0x0009F4F4
	[ClientRpc]
	private void RpcIgnitionTurnedOff()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcIgnitionTurnedOff()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001003 RID: 4099 RVA: 0x000A131F File Offset: 0x0009F51F
	public void EngineStop()
	{
		this.CmdEngineStop();
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x000A1328 File Offset: 0x0009F528
	[Command(requiresAuthority = false)]
	private void CmdEngineStop()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdEngineStop()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x000A1354 File Offset: 0x0009F554
	[ClientRpc]
	private void RpcEngineStop()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcEngineStop()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x000A137F File Offset: 0x0009F57F
	public void ChangingGear(string gear, int gearint)
	{
		this.CmdChangingGear(gear, gearint);
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x000A138C File Offset: 0x0009F58C
	[Command(requiresAuthority = false)]
	private void CmdChangingGear(string gear, int gearint)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteString(gear);
		writer.WriteInt(gearint);
		base.SendCommandInternal("System.Void networkDummy::CmdChangingGear(System.String,System.Int32)", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x000A13CC File Offset: 0x0009F5CC
	[ClientRpc]
	private void RpcChangingGear(string gear, int gearint)
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		writer.WriteString(gear);
		writer.WriteInt(gearint);
		this.SendRPCInternal("System.Void networkDummy::RpcChangingGear(System.String,System.Int32)", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x000A140B File Offset: 0x0009F60B
	public void setHandbrake()
	{
		this.CmdsetHandbrake();
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x000A1414 File Offset: 0x0009F614
	[Command(requiresAuthority = false)]
	private void CmdsetHandbrake()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdsetHandbrake()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x000A1440 File Offset: 0x0009F640
	[ClientRpc]
	private void RpcsetHandbrake()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcsetHandbrake()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x000A146B File Offset: 0x0009F66B
	public void releaseHandbrake()
	{
		this.CmdreleaseHandbrake();
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x000A1474 File Offset: 0x0009F674
	[Command(requiresAuthority = false)]
	private void CmdreleaseHandbrake()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdreleaseHandbrake()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x000A14A0 File Offset: 0x0009F6A0
	[ClientRpc]
	private void RpcreleaseHandbrake()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcreleaseHandbrake()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x000A14CB File Offset: 0x0009F6CB
	public void on0()
	{
		this.Cmdon0();
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x000A14D4 File Offset: 0x0009F6D4
	[Command(requiresAuthority = false)]
	private void Cmdon0()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::Cmdon0()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x000A1500 File Offset: 0x0009F700
	[ClientRpc]
	private void Rpcon0()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::Rpcon0()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x000A152B File Offset: 0x0009F72B
	public void on1()
	{
		this.Cmdon1();
	}

	// Token: 0x06001013 RID: 4115 RVA: 0x000A1534 File Offset: 0x0009F734
	[Command(requiresAuthority = false)]
	private void Cmdon1()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::Cmdon1()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x000A1560 File Offset: 0x0009F760
	[ClientRpc]
	private void Rpcon1()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::Rpcon1()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x000A158B File Offset: 0x0009F78B
	public void on2()
	{
		this.Cmdon2();
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x000A1594 File Offset: 0x0009F794
	[Command(requiresAuthority = false)]
	private void Cmdon2()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::Cmdon2()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x000A15C0 File Offset: 0x0009F7C0
	[ClientRpc]
	private void Rpcon2()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::Rpcon2()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001018 RID: 4120 RVA: 0x000A15EB File Offset: 0x0009F7EB
	public void on3()
	{
		this.Cmdon3();
	}

	// Token: 0x06001019 RID: 4121 RVA: 0x000A15F4 File Offset: 0x0009F7F4
	[Command(requiresAuthority = false)]
	private void Cmdon3()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::Cmdon3()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x0600101A RID: 4122 RVA: 0x000A1620 File Offset: 0x0009F820
	[ClientRpc]
	private void Rpcon3()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::Rpcon3()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x0600101B RID: 4123 RVA: 0x000A164B File Offset: 0x0009F84B
	public void StartCar()
	{
		this.CmdStartCar();
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x000A1654 File Offset: 0x0009F854
	[Command(requiresAuthority = false)]
	private void CmdStartCar()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdStartCar()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x000A1680 File Offset: 0x0009F880
	[ClientRpc]
	private void RpcStartCar()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcStartCar()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x000A16AB File Offset: 0x0009F8AB
	public void SetOwnerPlayer()
	{
		base.StartCoroutine(this.SetOwnerPlayer2());
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x000A16BA File Offset: 0x0009F8BA
	private IEnumerator SetOwnerPlayer2()
	{
		yield return new WaitForSeconds(5f);
		this.CmdSetOwnerPlayer();
		yield break;
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x000A16CC File Offset: 0x0009F8CC
	[Command(requiresAuthority = false)]
	private void CmdSetOwnerPlayer()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		base.SendCommandInternal("System.Void networkDummy::CmdSetOwnerPlayer()", writer, 0, false);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001021 RID: 4129 RVA: 0x000A16F8 File Offset: 0x0009F8F8
	[ClientRpc]
	private void RpcSetOwnerPlayer()
	{
		PooledNetworkWriter writer = NetworkWriterPool.GetWriter();
		this.SendRPCInternal("System.Void networkDummy::RpcSetOwnerPlayer()", writer, 0, true);
		NetworkWriterPool.Recycle(writer);
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x0000245B File Offset: 0x0000065B
	private void MirrorProcessed()
	{
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06001024 RID: 4132 RVA: 0x000A1724 File Offset: 0x0009F924
	// (set) Token: 0x06001025 RID: 4133 RVA: 0x000A1737 File Offset: 0x0009F937
	public string NetworkItemname
	{
		get
		{
			return this.Itemname;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.Itemname, 1UL, null);
		}
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06001026 RID: 4134 RVA: 0x000A1754 File Offset: 0x0009F954
	// (set) Token: 0x06001027 RID: 4135 RVA: 0x000A1767 File Offset: 0x0009F967
	public Vector3 NetworkSpawnposition
	{
		get
		{
			return this.Spawnposition;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<Vector3>(value, ref this.Spawnposition, 2UL, null);
		}
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06001028 RID: 4136 RVA: 0x000A1784 File Offset: 0x0009F984
	// (set) Token: 0x06001029 RID: 4137 RVA: 0x000A1797 File Offset: 0x0009F997
	public Quaternion NetworkSpawnrotation
	{
		get
		{
			return this.Spawnrotation;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<Quaternion>(value, ref this.Spawnrotation, 4UL, null);
		}
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x0600102A RID: 4138 RVA: 0x000A17B4 File Offset: 0x0009F9B4
	// (set) Token: 0x0600102B RID: 4139 RVA: 0x000A17C7 File Offset: 0x0009F9C7
	public int NetworkMPNumber
	{
		get
		{
			return this.MPNumber;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.MPNumber, 8UL, null);
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x0600102C RID: 4140 RVA: 0x000A17E4 File Offset: 0x0009F9E4
	// (set) Token: 0x0600102D RID: 4141 RVA: 0x000A17F7 File Offset: 0x0009F9F7
	public int NetworkCupMPNumber
	{
		get
		{
			return this.CupMPNumber;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.CupMPNumber, 16UL, null);
		}
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x0600102E RID: 4142 RVA: 0x000A1814 File Offset: 0x0009FA14
	// (set) Token: 0x0600102F RID: 4143 RVA: 0x000A1827 File Offset: 0x0009FA27
	public int NetworkSceneNumber
	{
		get
		{
			return this.SceneNumber;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.SceneNumber, 32UL, null);
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06001030 RID: 4144 RVA: 0x000A1844 File Offset: 0x0009FA44
	// (set) Token: 0x06001031 RID: 4145 RVA: 0x000A1857 File Offset: 0x0009FA57
	public int NetworkSavePosition
	{
		get
		{
			return this.SavePosition;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.SavePosition, 64UL, null);
		}
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06001032 RID: 4146 RVA: 0x000A1874 File Offset: 0x0009FA74
	// (set) Token: 0x06001033 RID: 4147 RVA: 0x000A1887 File Offset: 0x0009FA87
	public int NetworkObjectNumber
	{
		get
		{
			return this.ObjectNumber;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.ObjectNumber, 128UL, null);
		}
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06001034 RID: 4148 RVA: 0x000A18A4 File Offset: 0x0009FAA4
	// (set) Token: 0x06001035 RID: 4149 RVA: 0x000A18B7 File Offset: 0x0009FAB7
	public int NetworkSceneLoaded
	{
		get
		{
			return this.SceneLoaded;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.SceneLoaded, 256UL, null);
		}
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x000A18D1 File Offset: 0x0009FAD1
	protected void UserCode_CmdInputs__Single__Single__Single__Single(float value, float value2, float value3, float value4)
	{
		this.RpcInputs(value, value2, value3, value4);
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x000A18DE File Offset: 0x0009FADE
	protected static void InvokeUserCode_CmdInputs__Single__Single__Single__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdInputs called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdInputs__Single__Single__Single__Single(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x000A1920 File Offset: 0x0009FB20
	protected void UserCode_RpcInputs__Single__Single__Single__Single(float value, float value2, float value3, float value4)
	{
		if (this._vehicleController && !base.hasAuthority)
		{
			this._vehicleController.input.Steering = value;
			this._vehicleController.input.Brakes = value2;
			this._vehicleController.powertrain.engine.angularVelocity = value3;
			this._vehicleController.speed = value4;
		}
	}

	// Token: 0x06001039 RID: 4153 RVA: 0x000A1987 File Offset: 0x0009FB87
	protected static void InvokeUserCode_RpcInputs__Single__Single__Single__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcInputs called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcInputs__Single__Single__Single__Single(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x000A19C8 File Offset: 0x0009FBC8
	protected void UserCode_CmdCrash__Vector3__Vector3__Vector3__Vector3__Vector3__Vector3__Single__Vector3__Single(Vector3 damagePoint1, Vector3 damagePoint2, Vector3 damagePoint3, Vector3 damagePoint4, Vector3 damagePoint5, Vector3 damageForce, float damageForceLimit, Vector3 surfaceNormal, float hitAngle)
	{
		this.RpcCrash(damagePoint1, damagePoint2, damagePoint3, damagePoint4, damagePoint5, damageForce, damageForceLimit, surfaceNormal, hitAngle);
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x000A19EC File Offset: 0x0009FBEC
	protected static void InvokeUserCode_CmdCrash__Vector3__Vector3__Vector3__Vector3__Vector3__Vector3__Single__Vector3__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCrash called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdCrash__Vector3__Vector3__Vector3__Vector3__Vector3__Vector3__Single__Vector3__Single(reader.ReadVector3(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadFloat(), reader.ReadVector3(), reader.ReadFloat());
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x000A1A54 File Offset: 0x0009FC54
	protected void UserCode_RpcCrash__Vector3__Vector3__Vector3__Vector3__Vector3__Vector3__Single__Vector3__Single(Vector3 damagePoint1, Vector3 damagePoint2, Vector3 damagePoint3, Vector3 damagePoint4, Vector3 damagePoint5, Vector3 damageForce, float damageForceLimit, Vector3 surfaceNormal, float hitAngle)
	{
		if (this.DamageScript)
		{
			this.DamageScript.DamageApplicationMP(damagePoint1, damagePoint2, damagePoint3, damagePoint4, damagePoint5, damageForce, damageForceLimit, surfaceNormal, hitAngle);
		}
	}

	// Token: 0x0600103D RID: 4157 RVA: 0x000A1A88 File Offset: 0x0009FC88
	protected static void InvokeUserCode_RpcCrash__Vector3__Vector3__Vector3__Vector3__Vector3__Vector3__Single__Vector3__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcCrash called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcCrash__Vector3__Vector3__Vector3__Vector3__Vector3__Vector3__Single__Vector3__Single(reader.ReadVector3(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadFloat(), reader.ReadVector3(), reader.ReadFloat());
	}

	// Token: 0x0600103E RID: 4158 RVA: 0x000A1AEE File Offset: 0x0009FCEE
	protected void UserCode_CmdDestroyItem()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600103F RID: 4159 RVA: 0x000A1AFB File Offset: 0x0009FCFB
	protected static void InvokeUserCode_CmdDestroyItem(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDestroyItem called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdDestroyItem();
	}

	// Token: 0x06001040 RID: 4160 RVA: 0x000A1B1E File Offset: 0x0009FD1E
	protected void UserCode_CmdRemoveWindow()
	{
		this.RpcRemoveWindow();
	}

	// Token: 0x06001041 RID: 4161 RVA: 0x000A1B26 File Offset: 0x0009FD26
	protected static void InvokeUserCode_CmdRemoveWindow(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRemoveWindow called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdRemoveWindow();
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x000A1B4C File Offset: 0x0009FD4C
	protected void UserCode_RpcRemoveWindow()
	{
		if (this.Target)
		{
			if (this.Target.GetComponent<RemoveWindow>())
			{
				this.Target.GetComponent<RemoveWindow>().RemovingContinue();
			}
			if (this.Target.GetComponent<RemoveSpring>())
			{
				this.Target.GetComponent<RemoveSpring>().RemovingContinue();
			}
			if (this.Target.GetComponent<PickupHand>())
			{
				this.Target.GetComponent<PickupHand>().RemovingContinue();
			}
			if (this.Target.GetComponent<PickupCup>())
			{
				this.Target.GetComponent<PickupCup>().RemovingContinue();
			}
			if (this.Target.GetComponent<Pickup>())
			{
				this.Target.GetComponent<Pickup>().RemovingContinue();
			}
			if (this.Target.GetComponent<PickupDoor>())
			{
				this.Target.GetComponent<PickupDoor>().RemovingContinue();
			}
		}
		base.GetComponent<NetworkTransform>().enabled = true;
		base.enabled = true;
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x000A1C48 File Offset: 0x0009FE48
	protected static void InvokeUserCode_RpcRemoveWindow(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRemoveWindow called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcRemoveWindow();
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x000A1C6B File Offset: 0x0009FE6B
	protected void UserCode_CmdReducePickupToolInBox()
	{
		this.RpcReducePickupToolInBox();
	}

	// Token: 0x06001045 RID: 4165 RVA: 0x000A1C73 File Offset: 0x0009FE73
	protected static void InvokeUserCode_CmdReducePickupToolInBox(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdReducePickupToolInBox called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdReducePickupToolInBox();
	}

	// Token: 0x06001046 RID: 4166 RVA: 0x000A1C96 File Offset: 0x0009FE96
	protected void UserCode_RpcReducePickupToolInBox()
	{
		if (this.Target && this.Target.GetComponent<PickupTool>())
		{
			this.Target.GetComponent<PickupTool>().InBox--;
		}
	}

	// Token: 0x06001047 RID: 4167 RVA: 0x000A1CCF File Offset: 0x0009FECF
	protected static void InvokeUserCode_RpcReducePickupToolInBox(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcReducePickupToolInBox called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcReducePickupToolInBox();
	}

	// Token: 0x06001048 RID: 4168 RVA: 0x000A1CF2 File Offset: 0x0009FEF2
	protected void UserCode_CmdRemoveHand()
	{
		this.RpcRemoveHand();
	}

	// Token: 0x06001049 RID: 4169 RVA: 0x000A1CFA File Offset: 0x0009FEFA
	protected static void InvokeUserCode_CmdRemoveHand(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRemoveHand called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdRemoveHand();
	}

	// Token: 0x0600104A RID: 4170 RVA: 0x000A1D20 File Offset: 0x0009FF20
	protected void UserCode_RpcRemoveHand()
	{
		if (this.Target && this.Target.GetComponent<PickupHand>())
		{
			this.Target.GetComponent<PickupHand>().RemovingContinue();
		}
		base.GetComponent<NetworkTransform>().enabled = true;
		base.enabled = true;
	}

	// Token: 0x0600104B RID: 4171 RVA: 0x000A1D6F File Offset: 0x0009FF6F
	protected static void InvokeUserCode_RpcRemoveHand(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRemoveHand called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcRemoveHand();
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x000A1D92 File Offset: 0x0009FF92
	protected void UserCode_CmdDropOnGround()
	{
		this.RpcDropOnGround();
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x000A1D9A File Offset: 0x0009FF9A
	protected static void InvokeUserCode_CmdDropOnGround(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDropOnGround called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdDropOnGround();
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x000A1DC0 File Offset: 0x0009FFC0
	protected void UserCode_RpcDropOnGround()
	{
		if (this.Target && this.Target.GetComponent<PickupHand>())
		{
			this.Target.GetComponent<PickupHand>().DropOnGround();
		}
		if (this.Target && this.Target.GetComponent<Pickup>())
		{
			this.Target.GetComponent<Pickup>().DropOnGround();
		}
		if (this.Target && this.Target.GetComponent<PickupDoor>())
		{
			this.Target.GetComponent<PickupDoor>().DropOnGround();
		}
		if (this.Target && this.Target.GetComponent<PickupWindow>())
		{
			this.Target.GetComponent<PickupWindow>().DropOnGround();
		}
		base.GetComponent<NetworkTransform>().enabled = false;
		base.enabled = false;
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x000A1E9C File Offset: 0x000A009C
	protected static void InvokeUserCode_RpcDropOnGround(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcDropOnGround called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcDropOnGround();
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x000A1EBF File Offset: 0x000A00BF
	protected void UserCode_CmdStartMounting()
	{
		this.RpcStartMounting();
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x000A1EC7 File Offset: 0x000A00C7
	protected static void InvokeUserCode_CmdStartMounting(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdStartMounting called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdStartMounting();
	}

	// Token: 0x06001052 RID: 4178 RVA: 0x000A1EEA File Offset: 0x000A00EA
	protected void UserCode_RpcStartMounting()
	{
		if (this.Target)
		{
			this.Target.GetComponentInChildren<LiftHandle>().StartMounting();
		}
	}

	// Token: 0x06001053 RID: 4179 RVA: 0x000A1F09 File Offset: 0x000A0109
	protected static void InvokeUserCode_RpcStartMounting(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcStartMounting called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcStartMounting();
	}

	// Token: 0x06001054 RID: 4180 RVA: 0x000A1F2C File Offset: 0x000A012C
	protected void UserCode_Cmdopendoor__Boolean(bool open)
	{
		this.Rpcopendoor(open);
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x000A1F35 File Offset: 0x000A0135
	protected static void InvokeUserCode_Cmdopendoor__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command Cmdopendoor called on client.");
			return;
		}
		((networkDummy)obj).UserCode_Cmdopendoor__Boolean(reader.ReadBool());
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x000A1F5E File Offset: 0x000A015E
	protected void UserCode_Rpcopendoor__Boolean(bool open)
	{
		if (this.Target)
		{
			this.Target.GetComponent<OpenDoor>().MPdoorOperation(open);
		}
	}

	// Token: 0x06001057 RID: 4183 RVA: 0x000A1F7E File Offset: 0x000A017E
	protected static void InvokeUserCode_Rpcopendoor__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC Rpcopendoor called on server.");
			return;
		}
		((networkDummy)obj).UserCode_Rpcopendoor__Boolean(reader.ReadBool());
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x000A1FA7 File Offset: 0x000A01A7
	protected void UserCode_CmdPlayParticles()
	{
		this.RpcPlayParticles();
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x000A1FAF File Offset: 0x000A01AF
	protected static void InvokeUserCode_CmdPlayParticles(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdPlayParticles called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdPlayParticles();
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x000A1FD2 File Offset: 0x000A01D2
	protected void UserCode_RpcPlayParticles()
	{
		if (this.Target)
		{
			this.Target.GetComponent<PickupTool>().PlayParticles2();
		}
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x000A1FF1 File Offset: 0x000A01F1
	protected static void InvokeUserCode_RpcPlayParticles(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPlayParticles called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcPlayParticles();
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x000A2014 File Offset: 0x000A0214
	protected void UserCode_CmdStopParticles()
	{
		this.RpcStopParticles();
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x000A201C File Offset: 0x000A021C
	protected static void InvokeUserCode_CmdStopParticles(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdStopParticles called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdStopParticles();
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x000A203F File Offset: 0x000A023F
	protected void UserCode_RpcStopParticles()
	{
		if (this.Target)
		{
			this.Target.GetComponent<PickupTool>().StopParticles2();
		}
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x000A205E File Offset: 0x000A025E
	protected static void InvokeUserCode_RpcStopParticles(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcStopParticles called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcStopParticles();
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x000A2081 File Offset: 0x000A0281
	protected void UserCode_CmdPumpIn()
	{
		this.RpcPumpIn();
	}

	// Token: 0x06001061 RID: 4193 RVA: 0x000A2089 File Offset: 0x000A0289
	protected static void InvokeUserCode_CmdPumpIn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdPumpIn called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdPumpIn();
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x000A20AC File Offset: 0x000A02AC
	protected void UserCode_RpcPumpIn()
	{
		if (this.Target)
		{
			if (Vector3.Distance(this.Target.transform.position, this.Target.GetComponent<CarProperties>().AudioParent.transform.position) < 10f)
			{
				this.Target.GetComponent<CarProperties>().AudioParent.GetComponent<AudioSource>().PlayOneShot(this.Target.GetComponent<CarProperties>().AudioParent.GetComponent<AudioManager>().TirePump);
			}
			this.Target.GetComponent<CarProperties>().tireObject.TirePressure += 0.1f;
			this.Target.GetComponent<CarProperties>().ReStart();
		}
	}

	// Token: 0x06001063 RID: 4195 RVA: 0x000A2164 File Offset: 0x000A0364
	protected static void InvokeUserCode_RpcPumpIn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPumpIn called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcPumpIn();
	}

	// Token: 0x06001064 RID: 4196 RVA: 0x000A2187 File Offset: 0x000A0387
	protected void UserCode_CmdPumpOut()
	{
		this.RpcPumpOut();
	}

	// Token: 0x06001065 RID: 4197 RVA: 0x000A218F File Offset: 0x000A038F
	protected static void InvokeUserCode_CmdPumpOut(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdPumpOut called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdPumpOut();
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x000A21B4 File Offset: 0x000A03B4
	protected void UserCode_RpcPumpOut()
	{
		if (this.Target)
		{
			if (Vector3.Distance(this.Target.transform.position, this.Target.GetComponent<CarProperties>().AudioParent.transform.position) < 10f)
			{
				this.Target.GetComponent<CarProperties>().AudioParent.GetComponent<AudioSource>().PlayOneShot(this.Target.GetComponent<CarProperties>().AudioParent.GetComponent<AudioManager>().TirePump);
			}
			this.Target.GetComponent<CarProperties>().tireObject.TirePressure -= 0.1f;
			this.Target.GetComponent<CarProperties>().ReStart();
		}
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x000A226C File Offset: 0x000A046C
	protected static void InvokeUserCode_RpcPumpOut(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPumpOut called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcPumpOut();
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x000A228F File Offset: 0x000A048F
	protected void UserCode_CmdDestroyJoint()
	{
		this.RpcDestroyJoint();
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x000A2297 File Offset: 0x000A0497
	protected static void InvokeUserCode_CmdDestroyJoint(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDestroyJoint called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdDestroyJoint();
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x000A22BC File Offset: 0x000A04BC
	protected void UserCode_RpcDestroyJoint()
	{
		if (this.Target && this.Target.GetComponent<FixedJoint>())
		{
			UnityEngine.Object.Destroy(this.Target.GetComponent<FixedJoint>());
		}
		if (this.Target && this.Target.GetComponent<HingeJoint>())
		{
			UnityEngine.Object.Destroy(this.Target.GetComponent<HingeJoint>());
		}
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x000A2327 File Offset: 0x000A0527
	protected static void InvokeUserCode_RpcDestroyJoint(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcDestroyJoint called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcDestroyJoint();
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x000A234A File Offset: 0x000A054A
	protected void UserCode_CmdOpenGasCup__Boolean(bool open)
	{
		this.RpcOpenGasCup(open);
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x000A2353 File Offset: 0x000A0553
	protected static void InvokeUserCode_CmdOpenGasCup__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdOpenGasCup called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdOpenGasCup__Boolean(reader.ReadBool());
	}

	// Token: 0x0600106E RID: 4206 RVA: 0x000A237C File Offset: 0x000A057C
	protected void UserCode_RpcOpenGasCup__Boolean(bool open)
	{
		if (this.Target && this.Target.GetComponent<PickupCup>())
		{
			this.Target.GetComponent<PickupCup>().OpenGasCup(open);
		}
	}

	// Token: 0x0600106F RID: 4207 RVA: 0x000A23AE File Offset: 0x000A05AE
	protected static void InvokeUserCode_RpcOpenGasCup__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcOpenGasCup called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcOpenGasCup__Boolean(reader.ReadBool());
	}

	// Token: 0x06001070 RID: 4208 RVA: 0x000A23D7 File Offset: 0x000A05D7
	protected void UserCode_CmdUpdatePickupitems()
	{
		this.RpcUpdatePickupitems();
	}

	// Token: 0x06001071 RID: 4209 RVA: 0x000A23DF File Offset: 0x000A05DF
	protected static void InvokeUserCode_CmdUpdatePickupitems(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdUpdatePickupitems called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdUpdatePickupitems();
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x000A2402 File Offset: 0x000A0602
	protected void UserCode_RpcUpdatePickupitems()
	{
		if (this.Target && this.Target.GetComponent<PickupItems>())
		{
			this.Target.GetComponent<PickupItems>().UpdateCondition(1);
		}
	}

	// Token: 0x06001073 RID: 4211 RVA: 0x000A2434 File Offset: 0x000A0634
	protected static void InvokeUserCode_RpcUpdatePickupitems(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcUpdatePickupitems called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcUpdatePickupitems();
	}

	// Token: 0x06001074 RID: 4212 RVA: 0x000A2457 File Offset: 0x000A0657
	protected void UserCode_CmdPickupToolUPDATE__Int32__Boolean(int paintlife, bool updatevisual)
	{
		this.RpcPickupToolUPDATE(paintlife, updatevisual);
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x000A2461 File Offset: 0x000A0661
	protected static void InvokeUserCode_CmdPickupToolUPDATE__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdPickupToolUPDATE called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdPickupToolUPDATE__Int32__Boolean(reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x000A2490 File Offset: 0x000A0690
	protected void UserCode_RpcPickupToolUPDATE__Int32__Boolean(int paintlife, bool updatevisual)
	{
		if (this.Target && this.Target.GetComponent<PickupTool>())
		{
			this.Target.GetComponent<PickupTool>().UpdateCondition(paintlife, updatevisual);
		}
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x000A24C3 File Offset: 0x000A06C3
	protected static void InvokeUserCode_RpcPickupToolUPDATE__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPickupToolUPDATE called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcPickupToolUPDATE__Int32__Boolean(reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x000A24F2 File Offset: 0x000A06F2
	protected void UserCode_CmdPickupToolSync__Single(float paintlife)
	{
		this.RpcPickupToolSync(paintlife);
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x000A24FB File Offset: 0x000A06FB
	protected static void InvokeUserCode_CmdPickupToolSync__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdPickupToolSync called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdPickupToolSync__Single(reader.ReadFloat());
	}

	// Token: 0x0600107A RID: 4218 RVA: 0x000A2525 File Offset: 0x000A0725
	protected void UserCode_RpcPickupToolSync__Single(float paintlife)
	{
		if (this.Target && this.Target.GetComponent<PickupTool>())
		{
			this.Target.GetComponent<PickupTool>().paintlife = paintlife;
		}
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x000A2557 File Offset: 0x000A0757
	protected static void InvokeUserCode_RpcPickupToolSync__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPickupToolSync called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcPickupToolSync__Single(reader.ReadFloat());
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x000A2581 File Offset: 0x000A0781
	protected void UserCode_CmdFlashLight__Boolean(bool on)
	{
		this.RpcFlashLight(on);
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x000A258A File Offset: 0x000A078A
	protected static void InvokeUserCode_CmdFlashLight__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdFlashLight called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdFlashLight__Boolean(reader.ReadBool());
	}

	// Token: 0x0600107E RID: 4222 RVA: 0x000A25B3 File Offset: 0x000A07B3
	protected void UserCode_RpcFlashLight__Boolean(bool on)
	{
		if (this.Target && this.Target.GetComponent<PickupTool>())
		{
			this.Target.GetComponent<PickupTool>().FlashLightTurn(on);
		}
	}

	// Token: 0x0600107F RID: 4223 RVA: 0x000A25E5 File Offset: 0x000A07E5
	protected static void InvokeUserCode_RpcFlashLight__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcFlashLight called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcFlashLight__Boolean(reader.ReadBool());
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x000A260E File Offset: 0x000A080E
	protected void UserCode_CmdTint2__Int32(int level)
	{
		this.RpcTint2(level);
	}

	// Token: 0x06001081 RID: 4225 RVA: 0x000A2617 File Offset: 0x000A0817
	protected static void InvokeUserCode_CmdTint2__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTint2 called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdTint2__Int32(reader.ReadInt());
	}

	// Token: 0x06001082 RID: 4226 RVA: 0x000A2640 File Offset: 0x000A0840
	protected void UserCode_RpcTint2__Int32(int level)
	{
		if (this.Target && this.Target.GetComponent<CarProperties>())
		{
			this.Target.GetComponent<CarProperties>().Tint2(level);
		}
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x000A2672 File Offset: 0x000A0872
	protected static void InvokeUserCode_RpcTint2__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcTint2 called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcTint2__Int32(reader.ReadInt());
	}

	// Token: 0x06001084 RID: 4228 RVA: 0x000A269B File Offset: 0x000A089B
	protected void UserCode_CmdTint3__Int32(int level)
	{
		this.RpcTint3(level);
	}

	// Token: 0x06001085 RID: 4229 RVA: 0x000A26A4 File Offset: 0x000A08A4
	protected static void InvokeUserCode_CmdTint3__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTint3 called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdTint3__Int32(reader.ReadInt());
	}

	// Token: 0x06001086 RID: 4230 RVA: 0x000A26CD File Offset: 0x000A08CD
	protected void UserCode_RpcTint3__Int32(int level)
	{
		if (this.Target && this.Target.GetComponent<CarProperties>())
		{
			this.Target.GetComponent<CarProperties>().Tint3(level);
		}
	}

	// Token: 0x06001087 RID: 4231 RVA: 0x000A26FF File Offset: 0x000A08FF
	protected static void InvokeUserCode_RpcTint3__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcTint3 called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcTint3__Int32(reader.ReadInt());
	}

	// Token: 0x06001088 RID: 4232 RVA: 0x000A2728 File Offset: 0x000A0928
	protected void UserCode_CmdFair2__Vector3__Boolean__Int32__Single__Int32__Quaternion__Vector3(Vector3 point, bool preview, int priority, float pressure, int seed, Quaternion rotation, Vector3 hit)
	{
		this.RpcFair2(point, preview, priority, pressure, seed, rotation, hit);
	}

	// Token: 0x06001089 RID: 4233 RVA: 0x000A273C File Offset: 0x000A093C
	protected static void InvokeUserCode_CmdFair2__Vector3__Boolean__Int32__Single__Int32__Quaternion__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdFair2 called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdFair2__Vector3__Boolean__Int32__Single__Int32__Quaternion__Vector3(reader.ReadVector3(), reader.ReadBool(), reader.ReadInt(), reader.ReadFloat(), reader.ReadInt(), reader.ReadQuaternion(), reader.ReadVector3());
	}

	// Token: 0x0600108A RID: 4234 RVA: 0x000A2795 File Offset: 0x000A0995
	protected void UserCode_RpcFair2__Vector3__Boolean__Int32__Single__Int32__Quaternion__Vector3(Vector3 point, bool preview, int priority, float pressure, int seed, Quaternion rotation, Vector3 hit)
	{
		if (this.Target && this.Target.GetComponent<CarProperties>())
		{
			this.Target.GetComponent<CarProperties>().Fair2(point, preview, priority, pressure, seed, rotation, hit);
		}
	}

	// Token: 0x0600108B RID: 4235 RVA: 0x000A27D4 File Offset: 0x000A09D4
	protected static void InvokeUserCode_RpcFair2__Vector3__Boolean__Int32__Single__Int32__Quaternion__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcFair2 called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcFair2__Vector3__Boolean__Int32__Single__Int32__Quaternion__Vector3(reader.ReadVector3(), reader.ReadBool(), reader.ReadInt(), reader.ReadFloat(), reader.ReadInt(), reader.ReadQuaternion(), reader.ReadVector3());
	}

	// Token: 0x0600108C RID: 4236 RVA: 0x000A282D File Offset: 0x000A0A2D
	protected void UserCode_CmdRepair1__Vector3(Vector3 point)
	{
		this.RpcRepair1(point);
	}

	// Token: 0x0600108D RID: 4237 RVA: 0x000A2836 File Offset: 0x000A0A36
	protected static void InvokeUserCode_CmdRepair1__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRepair1 called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdRepair1__Vector3(reader.ReadVector3());
	}

	// Token: 0x0600108E RID: 4238 RVA: 0x000A285F File Offset: 0x000A0A5F
	protected void UserCode_RpcRepair1__Vector3(Vector3 point)
	{
		if (this.Target && this.Target.GetComponent<CarProperties>())
		{
			this.Target.GetComponent<CarProperties>().Repair1(point);
		}
	}

	// Token: 0x0600108F RID: 4239 RVA: 0x000A2891 File Offset: 0x000A0A91
	protected static void InvokeUserCode_RpcRepair1__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRepair1 called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcRepair1__Vector3(reader.ReadVector3());
	}

	// Token: 0x06001090 RID: 4240 RVA: 0x000A28BA File Offset: 0x000A0ABA
	protected void UserCode_CmdRepair22__Vector3(Vector3 point)
	{
		this.RpcRepair22(point);
	}

	// Token: 0x06001091 RID: 4241 RVA: 0x000A28C3 File Offset: 0x000A0AC3
	protected static void InvokeUserCode_CmdRepair22__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRepair22 called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdRepair22__Vector3(reader.ReadVector3());
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x000A28EC File Offset: 0x000A0AEC
	protected void UserCode_RpcRepair22__Vector3(Vector3 point)
	{
		if (this.Target && this.Target.GetComponent<CarProperties>())
		{
			this.Target.GetComponent<CarProperties>().Repair22(point);
		}
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x000A291E File Offset: 0x000A0B1E
	protected static void InvokeUserCode_RpcRepair22__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRepair22 called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcRepair22__Vector3(reader.ReadVector3());
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x000A2947 File Offset: 0x000A0B47
	protected void UserCode_CmdRustRemoveContinue__Boolean__Int32__Single__Int32__Quaternion__Vector3(bool preview, int priority, float pressure, int seed, Quaternion rotation, Vector3 hit)
	{
		this.RpcRustRemoveContinue(preview, priority, pressure, seed, rotation, hit);
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x000A2958 File Offset: 0x000A0B58
	protected static void InvokeUserCode_CmdRustRemoveContinue__Boolean__Int32__Single__Int32__Quaternion__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRustRemoveContinue called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdRustRemoveContinue__Boolean__Int32__Single__Int32__Quaternion__Vector3(reader.ReadBool(), reader.ReadInt(), reader.ReadFloat(), reader.ReadInt(), reader.ReadQuaternion(), reader.ReadVector3());
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x000A29AB File Offset: 0x000A0BAB
	protected void UserCode_RpcRustRemoveContinue__Boolean__Int32__Single__Int32__Quaternion__Vector3(bool preview, int priority, float pressure, int seed, Quaternion rotation, Vector3 hit)
	{
		if (this.Target && this.Target.GetComponent<CarProperties>())
		{
			this.Target.GetComponent<CarProperties>().RustRemoveContinue(preview, priority, pressure, seed, rotation, hit);
		}
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x000A29E8 File Offset: 0x000A0BE8
	protected static void InvokeUserCode_RpcRustRemoveContinue__Boolean__Int32__Single__Int32__Quaternion__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRustRemoveContinue called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcRustRemoveContinue__Boolean__Int32__Single__Int32__Quaternion__Vector3(reader.ReadBool(), reader.ReadInt(), reader.ReadFloat(), reader.ReadInt(), reader.ReadQuaternion(), reader.ReadVector3());
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x000A2A3B File Offset: 0x000A0C3B
	protected void UserCode_CmdLiftObject__Int32__Vector3(int steps, Vector3 position)
	{
		this.RpcLiftObject(steps, position);
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x000A2A45 File Offset: 0x000A0C45
	protected static void InvokeUserCode_CmdLiftObject__Int32__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdLiftObject called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdLiftObject__Int32__Vector3(reader.ReadInt(), reader.ReadVector3());
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x000A2A74 File Offset: 0x000A0C74
	protected void UserCode_RpcLiftObject__Int32__Vector3(int steps, Vector3 position)
	{
		if (this.Target)
		{
			this.Target.GetComponentInChildren<LiftHandle>().steps = steps;
			this.Target.GetComponentInChildren<LiftHandle>().LiftObject.transform.localPosition = position;
			if (this.Target.GetComponentInChildren<LiftHandle>().RotatingObj)
			{
				this.Target.GetComponentInChildren<LiftHandle>().RotatingObj.transform.LookAt(this.Target.GetComponentInChildren<LiftHandle>().RotatingToObj.transform);
			}
		}
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x000A2B00 File Offset: 0x000A0D00
	protected static void InvokeUserCode_RpcLiftObject__Int32__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcLiftObject called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcLiftObject__Int32__Vector3(reader.ReadInt(), reader.ReadVector3());
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x000A2B2F File Offset: 0x000A0D2F
	protected void UserCode_Cmdtighten__Int32(int childnumber1)
	{
		this.Rpctighten(childnumber1);
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x000A2B38 File Offset: 0x000A0D38
	protected static void InvokeUserCode_Cmdtighten__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command Cmdtighten called on client.");
			return;
		}
		((networkDummy)obj).UserCode_Cmdtighten__Int32(reader.ReadInt());
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x000A2B64 File Offset: 0x000A0D64
	protected void UserCode_Rpctighten__Int32(int childnumber1)
	{
		if (this.Target && this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<FlatNut>())
		{
			this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<FlatNut>().tighten2();
		}
		if (this.Target && this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<BoltNut>())
		{
			this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<BoltNut>().tighten2();
		}
		if (this.Target && this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<HexNut>())
		{
			this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<HexNut>().tighten2();
		}
		if (this.Target && this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<Sparkplug>())
		{
			this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<Sparkplug>().tighten2();
		}
		if (this.Target && this.Target.gameObject.GetComponent<CarProperties>() && this.Target.gameObject.GetComponent<CarProperties>().coilnut)
		{
			this.Target.gameObject.GetComponent<CarProperties>().coilnut.tighten2();
		}
		base.GetComponent<NetworkTransform>().enabled = false;
		base.enabled = false;
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x000A2D1A File Offset: 0x000A0F1A
	protected static void InvokeUserCode_Rpctighten__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC Rpctighten called on server.");
			return;
		}
		((networkDummy)obj).UserCode_Rpctighten__Int32(reader.ReadInt());
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x000A2D43 File Offset: 0x000A0F43
	protected void UserCode_CmdLoosen__Int32(int childnumber1)
	{
		this.RpcLoosen(childnumber1);
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x000A2D4C File Offset: 0x000A0F4C
	protected static void InvokeUserCode_CmdLoosen__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdLoosen called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdLoosen__Int32(reader.ReadInt());
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x000A2D78 File Offset: 0x000A0F78
	protected void UserCode_RpcLoosen__Int32(int childnumber1)
	{
		if (this.Target && this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<FlatNut>())
		{
			this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<FlatNut>().Loosen();
		}
		if (this.Target && this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<BoltNut>())
		{
			this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<BoltNut>().Loosen();
		}
		if (this.Target && this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<HexNut>())
		{
			this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<HexNut>().Loosen();
		}
		if (this.Target && this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<Sparkplug>())
		{
			this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<Sparkplug>().Loosen();
		}
		if (this.Target && this.Target.gameObject.GetComponent<CarProperties>() && this.Target.gameObject.GetComponent<CarProperties>().coilnut)
		{
			this.Target.gameObject.GetComponent<CarProperties>().coilnut.Loosen();
		}
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x000A2F1B File Offset: 0x000A111B
	protected static void InvokeUserCode_RpcLoosen__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcLoosen called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcLoosen__Int32(reader.ReadInt());
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x000A2F44 File Offset: 0x000A1144
	protected void UserCode_CmdCut__Int32__Vector3(int childnumber1, Vector3 point)
	{
		this.RpcCut(childnumber1, point);
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x000A2F4E File Offset: 0x000A114E
	protected static void InvokeUserCode_CmdCut__Int32__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCut called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdCut__Int32__Vector3(reader.ReadInt(), reader.ReadVector3());
	}

	// Token: 0x060010A6 RID: 4262 RVA: 0x000A2F80 File Offset: 0x000A1180
	protected void UserCode_RpcCut__Int32__Vector3(int childnumber1, Vector3 point)
	{
		if (this.Target && this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<WeldCut>())
		{
			this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<WeldCut>().Cut(point);
		}
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x000A2FDD File Offset: 0x000A11DD
	protected static void InvokeUserCode_RpcCut__Int32__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcCut called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcCut__Int32__Vector3(reader.ReadInt(), reader.ReadVector3());
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x000A300C File Offset: 0x000A120C
	protected void UserCode_CmdWeld__Int32__Vector3(int childnumber1, Vector3 point)
	{
		this.RpcWeld(childnumber1, point);
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x000A3016 File Offset: 0x000A1216
	protected static void InvokeUserCode_CmdWeld__Int32__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdWeld called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdWeld__Int32__Vector3(reader.ReadInt(), reader.ReadVector3());
	}

	// Token: 0x060010AA RID: 4266 RVA: 0x000A3048 File Offset: 0x000A1248
	protected void UserCode_RpcWeld__Int32__Vector3(int childnumber1, Vector3 point)
	{
		if (this.Target && this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<WeldCut>())
		{
			this.Target.transform.GetChild(childnumber1).gameObject.GetComponent<WeldCut>().Weld(point);
		}
		base.GetComponent<NetworkTransform>().enabled = false;
		base.enabled = false;
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x000A30B8 File Offset: 0x000A12B8
	protected static void InvokeUserCode_RpcWeld__Int32__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcWeld called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcWeld__Int32__Vector3(reader.ReadInt(), reader.ReadVector3());
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x000A30E7 File Offset: 0x000A12E7
	protected void UserCode_CmdEnableMovment()
	{
		this.RpcEnableMovment();
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x000A30EF File Offset: 0x000A12EF
	protected static void InvokeUserCode_CmdEnableMovment(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdEnableMovment called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdEnableMovment();
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x000A3112 File Offset: 0x000A1312
	protected void UserCode_RpcEnableMovment()
	{
		base.GetComponent<NetworkTransform>().enabled = true;
		base.enabled = true;
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x000A3127 File Offset: 0x000A1327
	protected static void InvokeUserCode_RpcEnableMovment(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcEnableMovment called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcEnableMovment();
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x000A314A File Offset: 0x000A134A
	protected void UserCode_CmdBRAKE()
	{
		this.RpcBRAKE();
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x000A3152 File Offset: 0x000A1352
	protected static void InvokeUserCode_CmdBRAKE(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdBRAKE called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdBRAKE();
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x000A3178 File Offset: 0x000A1378
	protected void UserCode_RpcBRAKE()
	{
		if (this.Target)
		{
			if (this.Target.GetComponent<Pickup>())
			{
				this.Target.GetComponent<Pickup>().BRAKE2();
			}
			if (this.Target.GetComponent<PickupWindow>())
			{
				this.Target.GetComponent<PickupWindow>().BRAKE2();
			}
			if (this.Target.GetComponent<PickupDoor>())
			{
				this.Target.GetComponent<PickupDoor>().BRAKE2();
			}
			if (this.Target.GetComponent<PickupSpring>())
			{
				this.Target.GetComponent<PickupSpring>().BRAKE2();
			}
			if (this.Target.GetComponent<PickupHand>())
			{
				this.Target.GetComponent<PickupHand>().BRAKE2();
			}
			base.GetComponent<NetworkTransform>().enabled = true;
			base.enabled = true;
		}
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x000A3252 File Offset: 0x000A1452
	protected static void InvokeUserCode_RpcBRAKE(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcBRAKE called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcBRAKE();
	}

	// Token: 0x060010B4 RID: 4276 RVA: 0x000A3275 File Offset: 0x000A1475
	protected void UserCode_CmdAttachPickup__Int32__Int32__Int32(int parentNR, int childnumber1, int childnumber2)
	{
		this.RpcAttachPickup(parentNR, childnumber1, childnumber2);
	}

	// Token: 0x060010B5 RID: 4277 RVA: 0x000A3280 File Offset: 0x000A1480
	protected static void InvokeUserCode_CmdAttachPickup__Int32__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdAttachPickup called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdAttachPickup__Int32__Int32__Int32(reader.ReadInt(), reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x000A32B8 File Offset: 0x000A14B8
	protected void UserCode_RpcAttachPickup__Int32__Int32__Int32(int parentNR, int childnumber1, int childnumber2)
	{
		foreach (MPobject mpobject in UnityEngine.Object.FindObjectsOfType<MPobject>())
		{
			if (mpobject.MPNumber == parentNR && this.Target)
			{
				if (childnumber2 == -1)
				{
					if (mpobject.transform.childCount < childnumber1)
					{
						return;
					}
				}
				else
				{
					if (mpobject.transform.childCount < childnumber2)
					{
						return;
					}
					if (mpobject.transform.GetChild(childnumber2).childCount < childnumber1)
					{
						return;
					}
				}
				base.GetComponent<NetworkTransform>().enabled = false;
				base.enabled = false;
				if (this.Target.GetComponent<Pickup>())
				{
					if (childnumber2 == -1)
					{
						this.Target.GetComponent<Pickup>().FitInPlace(mpobject.transform.GetChild(childnumber1).gameObject);
					}
					else
					{
						this.Target.GetComponent<Pickup>().FitInPlace(mpobject.transform.GetChild(childnumber2).GetChild(childnumber1).gameObject);
					}
				}
				if (this.Target.GetComponent<PickupDoor>())
				{
					if (childnumber2 == -1)
					{
						this.Target.GetComponent<PickupDoor>().FitInPlace(mpobject.transform.GetChild(childnumber1).gameObject);
					}
					else
					{
						this.Target.GetComponent<PickupDoor>().FitInPlace(mpobject.transform.GetChild(childnumber2).GetChild(childnumber1).gameObject);
					}
				}
				if (this.Target.GetComponent<PickupWindow>())
				{
					if (childnumber2 == -1)
					{
						this.Target.GetComponent<PickupWindow>().Attach(mpobject.transform.GetChild(childnumber1).gameObject);
					}
					else
					{
						this.Target.GetComponent<PickupWindow>().Attach(mpobject.transform.GetChild(childnumber2).GetChild(childnumber1).gameObject);
					}
				}
				if (this.Target.GetComponent<PickupSpring>())
				{
					if (childnumber2 == -1)
					{
						this.Target.GetComponent<PickupSpring>().Attach(mpobject.transform.GetChild(childnumber1).gameObject);
					}
					else
					{
						this.Target.GetComponent<PickupSpring>().Attach(mpobject.transform.GetChild(childnumber2).GetChild(childnumber1).gameObject);
					}
				}
				if (this.Target.GetComponent<PickupHand>())
				{
					if (childnumber2 == -1)
					{
						this.Target.GetComponent<PickupHand>().Attach(mpobject.transform.GetChild(childnumber1).gameObject);
					}
					else
					{
						this.Target.GetComponent<PickupHand>().Attach(mpobject.transform.GetChild(childnumber2).GetChild(childnumber1).gameObject);
					}
				}
				if (this.Target.GetComponent<PickupCup>())
				{
					if (childnumber2 == -1)
					{
						this.Target.GetComponent<PickupCup>().FitInPlace(mpobject.transform.GetChild(childnumber1).gameObject);
					}
					else
					{
						this.Target.GetComponent<PickupCup>().FitInPlace(mpobject.transform.GetChild(childnumber2).GetChild(childnumber1).gameObject);
					}
				}
				if (this.Target.GetComponent<PickupItems>())
				{
					if (childnumber2 == -1)
					{
						this.Target.GetComponent<PickupItems>().FitInPlace(mpobject.transform.GetChild(childnumber1).gameObject);
					}
					else
					{
						this.Target.GetComponent<PickupItems>().FitInPlace(mpobject.transform.GetChild(childnumber2).GetChild(childnumber1).gameObject);
					}
				}
				if (this.Target.GetComponent<PickupTool>())
				{
					if (childnumber2 == -1)
					{
						this.Target.GetComponent<PickupTool>().AttachPickup2(mpobject.transform.GetChild(childnumber1).gameObject);
						return;
					}
					this.Target.GetComponent<PickupTool>().AttachPickup2(mpobject.transform.GetChild(childnumber2).GetChild(childnumber1).gameObject);
				}
				return;
			}
		}
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x000A3649 File Offset: 0x000A1849
	protected static void InvokeUserCode_RpcAttachPickup__Int32__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcAttachPickup called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcAttachPickup__Int32__Int32__Int32(reader.ReadInt(), reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x000A367E File Offset: 0x000A187E
	protected void UserCode_CmdDestroyMe()
	{
		this.RpcDestroyMe();
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x000A3686 File Offset: 0x000A1886
	protected static void InvokeUserCode_CmdDestroyMe(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDestroyMe called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdDestroyMe();
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x000A36A9 File Offset: 0x000A18A9
	protected void UserCode_RpcDestroyMe()
	{
		if (this.Target != null)
		{
			UnityEngine.Object.Destroy(this.Target);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x000A36CF File Offset: 0x000A18CF
	protected static void InvokeUserCode_RpcDestroyMe(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcDestroyMe called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcDestroyMe();
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x000A36F2 File Offset: 0x000A18F2
	protected void UserCode_CmdWinnchReeling()
	{
		this.RpcWinnchReeling();
	}

	// Token: 0x060010BD RID: 4285 RVA: 0x000A36FA File Offset: 0x000A18FA
	protected static void InvokeUserCode_CmdWinnchReeling(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdWinnchReeling called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdWinnchReeling();
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x000A371D File Offset: 0x000A191D
	protected void UserCode_RpcWinnchReeling()
	{
		if (this.Target)
		{
			this.Target.GetComponentInChildren<WindowLift>().WinnchReeling();
		}
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x000A373C File Offset: 0x000A193C
	protected static void InvokeUserCode_RpcWinnchReeling(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcWinnchReeling called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcWinnchReeling();
	}

	// Token: 0x060010C0 RID: 4288 RVA: 0x000A375F File Offset: 0x000A195F
	protected void UserCode_CmdWindowUp()
	{
		this.RpcWindowUp();
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x000A3767 File Offset: 0x000A1967
	protected static void InvokeUserCode_CmdWindowUp(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdWindowUp called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdWindowUp();
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x000A378A File Offset: 0x000A198A
	protected void UserCode_RpcWindowUp()
	{
		if (this.Target)
		{
			this.Target.GetComponentInChildren<WindowLift>().WindowUp();
		}
	}

	// Token: 0x060010C3 RID: 4291 RVA: 0x000A37A9 File Offset: 0x000A19A9
	protected static void InvokeUserCode_RpcWindowUp(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcWindowUp called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcWindowUp();
	}

	// Token: 0x060010C4 RID: 4292 RVA: 0x000A37CC File Offset: 0x000A19CC
	protected void UserCode_CmdWindowDown()
	{
		this.RpcWindowDown();
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x000A37D4 File Offset: 0x000A19D4
	protected static void InvokeUserCode_CmdWindowDown(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdWindowDown called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdWindowDown();
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x000A37F7 File Offset: 0x000A19F7
	protected void UserCode_RpcWindowDown()
	{
		if (this.Target)
		{
			this.Target.GetComponentInChildren<WindowLift>().WindowDown();
		}
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x000A3816 File Offset: 0x000A1A16
	protected static void InvokeUserCode_RpcWindowDown(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcWindowDown called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcWindowDown();
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x000A3839 File Offset: 0x000A1A39
	protected void UserCode_CmdApplyChrome2()
	{
		this.RpcApplyChrome2();
	}

	// Token: 0x060010C9 RID: 4297 RVA: 0x000A3841 File Offset: 0x000A1A41
	protected static void InvokeUserCode_CmdApplyChrome2(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdApplyChrome2 called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdApplyChrome2();
	}

	// Token: 0x060010CA RID: 4298 RVA: 0x000A3864 File Offset: 0x000A1A64
	protected void UserCode_RpcApplyChrome2()
	{
		if (this.Target)
		{
			this.Target.GetComponent<CarProperties>().ApplyChrome2();
		}
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x000A3883 File Offset: 0x000A1A83
	protected static void InvokeUserCode_RpcApplyChrome2(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcApplyChrome2 called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcApplyChrome2();
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x000A38A6 File Offset: 0x000A1AA6
	protected void UserCode_CmdSyncFluid__Single(float FluidSize)
	{
		this.RpcSyncFluid(FluidSize);
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x000A38AF File Offset: 0x000A1AAF
	protected static void InvokeUserCode_CmdSyncFluid__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSyncFluid called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdSyncFluid__Single(reader.ReadFloat());
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x000A38D9 File Offset: 0x000A1AD9
	protected void UserCode_RpcSyncFluid__Single(float FluidSize)
	{
		if (this.Target)
		{
			this.Target.GetComponentInChildren<FLUID>().FluidSize = FluidSize;
			this.Target.GetComponentInChildren<FLUID>().VisualUpdate();
		}
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x000A3909 File Offset: 0x000A1B09
	protected static void InvokeUserCode_RpcSyncFluid__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSyncFluid called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcSyncFluid__Single(reader.ReadFloat());
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x000A3933 File Offset: 0x000A1B33
	protected void UserCode_CmdRunningLightTurnOn()
	{
		this.RpcRunningLightTurnOn();
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x000A393B File Offset: 0x000A1B3B
	protected static void InvokeUserCode_CmdRunningLightTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRunningLightTurnOn called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdRunningLightTurnOn();
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x000A395E File Offset: 0x000A1B5E
	protected void UserCode_RpcRunningLightTurnOn()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().RunningLightTurnOn2();
		}
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x000A398F File Offset: 0x000A1B8F
	protected static void InvokeUserCode_RpcRunningLightTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRunningLightTurnOn called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcRunningLightTurnOn();
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x000A39B2 File Offset: 0x000A1BB2
	protected void UserCode_CmdRunningLightTurnOff()
	{
		this.RpcRunningLightTurnOff();
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x000A39BA File Offset: 0x000A1BBA
	protected static void InvokeUserCode_CmdRunningLightTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRunningLightTurnOff called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdRunningLightTurnOff();
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x000A39DD File Offset: 0x000A1BDD
	protected void UserCode_RpcRunningLightTurnOff()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().RunningLightTurnOff2();
		}
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x000A3A0E File Offset: 0x000A1C0E
	protected static void InvokeUserCode_RpcRunningLightTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRunningLightTurnOff called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcRunningLightTurnOff();
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x000A3A31 File Offset: 0x000A1C31
	protected void UserCode_CmdHeadLightLowTurnOn()
	{
		this.RpcHeadLightLowTurnOn();
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x000A3A39 File Offset: 0x000A1C39
	protected static void InvokeUserCode_CmdHeadLightLowTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdHeadLightLowTurnOn called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdHeadLightLowTurnOn();
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x000A3A5C File Offset: 0x000A1C5C
	protected void UserCode_RpcHeadLightLowTurnOn()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().HeadLightLowTurnOn2();
		}
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x000A3A8D File Offset: 0x000A1C8D
	protected static void InvokeUserCode_RpcHeadLightLowTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcHeadLightLowTurnOn called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcHeadLightLowTurnOn();
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x000A3AB0 File Offset: 0x000A1CB0
	protected void UserCode_CmdHeadLightLowTurnOff()
	{
		this.RpcHeadLightLowTurnOff();
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x000A3AB8 File Offset: 0x000A1CB8
	protected static void InvokeUserCode_CmdHeadLightLowTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdHeadLightLowTurnOff called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdHeadLightLowTurnOff();
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x000A3ADB File Offset: 0x000A1CDB
	protected void UserCode_RpcHeadLightLowTurnOff()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().HeadLightLowTurnOff2();
		}
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x000A3B0C File Offset: 0x000A1D0C
	protected static void InvokeUserCode_RpcHeadLightLowTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcHeadLightLowTurnOff called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcHeadLightLowTurnOff();
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x000A3B2F File Offset: 0x000A1D2F
	protected void UserCode_CmdHeadLightHighTurnOn()
	{
		this.RpcHeadLightHighTurnOn();
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x000A3B37 File Offset: 0x000A1D37
	protected static void InvokeUserCode_CmdHeadLightHighTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdHeadLightHighTurnOn called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdHeadLightHighTurnOn();
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x000A3B5A File Offset: 0x000A1D5A
	protected void UserCode_RpcHeadLightHighTurnOn()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().HeadLightHighTurnOn2();
		}
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x000A3B8B File Offset: 0x000A1D8B
	protected static void InvokeUserCode_RpcHeadLightHighTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcHeadLightHighTurnOn called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcHeadLightHighTurnOn();
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x000A3BAE File Offset: 0x000A1DAE
	protected void UserCode_CmdHeadLightHighTurnOff()
	{
		this.RpcHeadLightHighTurnOff();
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x000A3BB6 File Offset: 0x000A1DB6
	protected static void InvokeUserCode_CmdHeadLightHighTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdHeadLightHighTurnOff called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdHeadLightHighTurnOff();
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x000A3BD9 File Offset: 0x000A1DD9
	protected void UserCode_RpcHeadLightHighTurnOff()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().HeadLightHighTurnOff2();
		}
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x000A3C0A File Offset: 0x000A1E0A
	protected static void InvokeUserCode_RpcHeadLightHighTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcHeadLightHighTurnOff called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcHeadLightHighTurnOff();
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x000A3C2D File Offset: 0x000A1E2D
	protected void UserCode_CmdBrakeLightTurnOn()
	{
		this.RpcBrakeLightTurnOn();
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x000A3C35 File Offset: 0x000A1E35
	protected static void InvokeUserCode_CmdBrakeLightTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdBrakeLightTurnOn called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdBrakeLightTurnOn();
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x000A3C58 File Offset: 0x000A1E58
	protected void UserCode_RpcBrakeLightTurnOn()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().BrakeLightTurnOn2();
		}
	}

	// Token: 0x060010EB RID: 4331 RVA: 0x000A3C89 File Offset: 0x000A1E89
	protected static void InvokeUserCode_RpcBrakeLightTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcBrakeLightTurnOn called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcBrakeLightTurnOn();
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x000A3CAC File Offset: 0x000A1EAC
	protected void UserCode_CmdBrakeLightTurnOff()
	{
		this.RpcBrakeLightTurnOff();
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x000A3CB4 File Offset: 0x000A1EB4
	protected static void InvokeUserCode_CmdBrakeLightTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdBrakeLightTurnOff called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdBrakeLightTurnOff();
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x000A3CD7 File Offset: 0x000A1ED7
	protected void UserCode_RpcBrakeLightTurnOff()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().BrakeLightTurnOff2();
		}
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x000A3D08 File Offset: 0x000A1F08
	protected static void InvokeUserCode_RpcBrakeLightTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcBrakeLightTurnOff called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcBrakeLightTurnOff();
	}

	// Token: 0x060010F0 RID: 4336 RVA: 0x000A3D2B File Offset: 0x000A1F2B
	protected void UserCode_CmdReverseLightTurnOn()
	{
		this.RpcReverseLightTurnOn();
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x000A3D33 File Offset: 0x000A1F33
	protected static void InvokeUserCode_CmdReverseLightTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdReverseLightTurnOn called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdReverseLightTurnOn();
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x000A3D56 File Offset: 0x000A1F56
	protected void UserCode_RpcReverseLightTurnOn()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().ReverseLightTurnOn2();
		}
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x000A3D87 File Offset: 0x000A1F87
	protected static void InvokeUserCode_RpcReverseLightTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcReverseLightTurnOn called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcReverseLightTurnOn();
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x000A3DAA File Offset: 0x000A1FAA
	protected void UserCode_CmdReverseLightTurnOff()
	{
		this.RpcReverseLightTurnOff();
	}

	// Token: 0x060010F5 RID: 4341 RVA: 0x000A3DB2 File Offset: 0x000A1FB2
	protected static void InvokeUserCode_CmdReverseLightTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdReverseLightTurnOff called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdReverseLightTurnOff();
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x000A3DD5 File Offset: 0x000A1FD5
	protected void UserCode_RpcReverseLightTurnOff()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().ReverseLightTurnOff2();
		}
	}

	// Token: 0x060010F7 RID: 4343 RVA: 0x000A3E06 File Offset: 0x000A2006
	protected static void InvokeUserCode_RpcReverseLightTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcReverseLightTurnOff called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcReverseLightTurnOff();
	}

	// Token: 0x060010F8 RID: 4344 RVA: 0x000A3E29 File Offset: 0x000A2029
	protected void UserCode_CmdLeftLightTurnOn()
	{
		this.RpcLeftLightTurnOn();
	}

	// Token: 0x060010F9 RID: 4345 RVA: 0x000A3E31 File Offset: 0x000A2031
	protected static void InvokeUserCode_CmdLeftLightTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdLeftLightTurnOn called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdLeftLightTurnOn();
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x000A3E54 File Offset: 0x000A2054
	protected void UserCode_RpcLeftLightTurnOn()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().LeftLightTurnOn2();
		}
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x000A3E85 File Offset: 0x000A2085
	protected static void InvokeUserCode_RpcLeftLightTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcLeftLightTurnOn called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcLeftLightTurnOn();
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x000A3EA8 File Offset: 0x000A20A8
	protected void UserCode_CmdLeftLightTurnOff()
	{
		this.RpcLeftLightTurnOff();
	}

	// Token: 0x060010FD RID: 4349 RVA: 0x000A3EB0 File Offset: 0x000A20B0
	protected static void InvokeUserCode_CmdLeftLightTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdLeftLightTurnOff called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdLeftLightTurnOff();
	}

	// Token: 0x060010FE RID: 4350 RVA: 0x000A3ED3 File Offset: 0x000A20D3
	protected void UserCode_RpcLeftLightTurnOff()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().LeftLightTurnOff2();
		}
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x000A3F04 File Offset: 0x000A2104
	protected static void InvokeUserCode_RpcLeftLightTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcLeftLightTurnOff called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcLeftLightTurnOff();
	}

	// Token: 0x06001100 RID: 4352 RVA: 0x000A3F27 File Offset: 0x000A2127
	protected void UserCode_CmdRightLightTurnOn()
	{
		this.RpcRightLightTurnOn();
	}

	// Token: 0x06001101 RID: 4353 RVA: 0x000A3F2F File Offset: 0x000A212F
	protected static void InvokeUserCode_CmdRightLightTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRightLightTurnOn called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdRightLightTurnOn();
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x000A3F52 File Offset: 0x000A2152
	protected void UserCode_RpcRightLightTurnOn()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().RightLightTurnOn2();
		}
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x000A3F83 File Offset: 0x000A2183
	protected static void InvokeUserCode_RpcRightLightTurnOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRightLightTurnOn called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcRightLightTurnOn();
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x000A3FA6 File Offset: 0x000A21A6
	protected void UserCode_CmdRightLightTurnOff()
	{
		this.RpcRightLightTurnOff();
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x000A3FAE File Offset: 0x000A21AE
	protected static void InvokeUserCode_CmdRightLightTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRightLightTurnOff called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdRightLightTurnOff();
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x000A3FD1 File Offset: 0x000A21D1
	protected void UserCode_RpcRightLightTurnOff()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().RightLightTurnOff2();
		}
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x000A4002 File Offset: 0x000A2202
	protected static void InvokeUserCode_RpcRightLightTurnOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRightLightTurnOff called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcRightLightTurnOff();
	}

	// Token: 0x06001108 RID: 4360 RVA: 0x000A4025 File Offset: 0x000A2225
	protected void UserCode_CmdWiperSOn()
	{
		this.RpcWiperSOn();
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x000A402D File Offset: 0x000A222D
	protected static void InvokeUserCode_CmdWiperSOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdWiperSOn called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdWiperSOn();
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x000A4050 File Offset: 0x000A2250
	protected void UserCode_RpcWiperSOn()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().WiperSOn2();
		}
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x000A4081 File Offset: 0x000A2281
	protected static void InvokeUserCode_RpcWiperSOn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcWiperSOn called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcWiperSOn();
	}

	// Token: 0x0600110C RID: 4364 RVA: 0x000A40A4 File Offset: 0x000A22A4
	protected void UserCode_CmdWiperOff()
	{
		this.RpcWiperOff();
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x000A40AC File Offset: 0x000A22AC
	protected static void InvokeUserCode_CmdWiperOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdWiperOff called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdWiperOff();
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x000A40CF File Offset: 0x000A22CF
	protected void UserCode_RpcWiperOff()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().WiperOff2();
		}
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x000A4100 File Offset: 0x000A2300
	protected static void InvokeUserCode_RpcWiperOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcWiperOff called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcWiperOff();
	}

	// Token: 0x06001110 RID: 4368 RVA: 0x000A4123 File Offset: 0x000A2323
	protected void UserCode_CmdIgnitionTurnedOff()
	{
		this.RpcIgnitionTurnedOff();
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x000A412B File Offset: 0x000A232B
	protected static void InvokeUserCode_CmdIgnitionTurnedOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdIgnitionTurnedOff called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdIgnitionTurnedOff();
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x0000245B File Offset: 0x0000065B
	protected void UserCode_RpcIgnitionTurnedOff()
	{
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x000A414E File Offset: 0x000A234E
	protected static void InvokeUserCode_RpcIgnitionTurnedOff(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcIgnitionTurnedOff called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcIgnitionTurnedOff();
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x000A4171 File Offset: 0x000A2371
	protected void UserCode_CmdEngineStop()
	{
		this.RpcEngineStop();
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x000A4179 File Offset: 0x000A2379
	protected static void InvokeUserCode_CmdEngineStop(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdEngineStop called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdEngineStop();
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x000A419C File Offset: 0x000A239C
	protected void UserCode_RpcEngineStop()
	{
		if (this.Target && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().EngineStop2();
		}
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x000A41CD File Offset: 0x000A23CD
	protected static void InvokeUserCode_RpcEngineStop(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcEngineStop called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcEngineStop();
	}

	// Token: 0x06001118 RID: 4376 RVA: 0x000A41F0 File Offset: 0x000A23F0
	protected void UserCode_CmdChangingGear__String__Int32(string gear, int gearint)
	{
		this.RpcChangingGear(gear, gearint);
	}

	// Token: 0x06001119 RID: 4377 RVA: 0x000A41FA File Offset: 0x000A23FA
	protected static void InvokeUserCode_CmdChangingGear__String__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdChangingGear called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdChangingGear__String__Int32(reader.ReadString(), reader.ReadInt());
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x000A4229 File Offset: 0x000A2429
	protected void UserCode_RpcChangingGear__String__Int32(string gear, int gearint)
	{
		if (this.Target != null && this.Target.GetComponent<MainCarProperties>())
		{
			this.Target.GetComponent<MainCarProperties>().ChangingGearContinue(gear, gearint);
		}
	}

	// Token: 0x0600111B RID: 4379 RVA: 0x000A425D File Offset: 0x000A245D
	protected static void InvokeUserCode_RpcChangingGear__String__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcChangingGear called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcChangingGear__String__Int32(reader.ReadString(), reader.ReadInt());
	}

	// Token: 0x0600111C RID: 4380 RVA: 0x000A428C File Offset: 0x000A248C
	protected void UserCode_CmdsetHandbrake()
	{
		this.RpcsetHandbrake();
	}

	// Token: 0x0600111D RID: 4381 RVA: 0x000A4294 File Offset: 0x000A2494
	protected static void InvokeUserCode_CmdsetHandbrake(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdsetHandbrake called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdsetHandbrake();
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x000A42B7 File Offset: 0x000A24B7
	protected void UserCode_RpcsetHandbrake()
	{
		if (this.Target)
		{
			this.Target.GetComponentInChildren<HandbrakeScr>().set();
		}
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x000A42D6 File Offset: 0x000A24D6
	protected static void InvokeUserCode_RpcsetHandbrake(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcsetHandbrake called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcsetHandbrake();
	}

	// Token: 0x06001120 RID: 4384 RVA: 0x000A42F9 File Offset: 0x000A24F9
	protected void UserCode_CmdreleaseHandbrake()
	{
		this.RpcreleaseHandbrake();
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x000A4301 File Offset: 0x000A2501
	protected static void InvokeUserCode_CmdreleaseHandbrake(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdreleaseHandbrake called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdreleaseHandbrake();
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x000A4324 File Offset: 0x000A2524
	protected void UserCode_RpcreleaseHandbrake()
	{
		if (this.Target)
		{
			this.Target.GetComponentInChildren<HandbrakeScr>().release();
		}
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x000A4343 File Offset: 0x000A2543
	protected static void InvokeUserCode_RpcreleaseHandbrake(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcreleaseHandbrake called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcreleaseHandbrake();
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x000A4366 File Offset: 0x000A2566
	protected void UserCode_Cmdon0()
	{
		this.Rpcon0();
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x000A436E File Offset: 0x000A256E
	protected static void InvokeUserCode_Cmdon0(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command Cmdon0 called on client.");
			return;
		}
		((networkDummy)obj).UserCode_Cmdon0();
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x000A4391 File Offset: 0x000A2591
	protected void UserCode_Rpcon0()
	{
		if (this.Target)
		{
			this.Target.GetComponentInChildren<IgnitionKey>().on0();
		}
	}

	// Token: 0x06001127 RID: 4391 RVA: 0x000A43B0 File Offset: 0x000A25B0
	protected static void InvokeUserCode_Rpcon0(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC Rpcon0 called on server.");
			return;
		}
		((networkDummy)obj).UserCode_Rpcon0();
	}

	// Token: 0x06001128 RID: 4392 RVA: 0x000A43D3 File Offset: 0x000A25D3
	protected void UserCode_Cmdon1()
	{
		this.Rpcon1();
	}

	// Token: 0x06001129 RID: 4393 RVA: 0x000A43DB File Offset: 0x000A25DB
	protected static void InvokeUserCode_Cmdon1(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command Cmdon1 called on client.");
			return;
		}
		((networkDummy)obj).UserCode_Cmdon1();
	}

	// Token: 0x0600112A RID: 4394 RVA: 0x000A43FE File Offset: 0x000A25FE
	protected void UserCode_Rpcon1()
	{
		if (this.Target)
		{
			this.Target.GetComponentInChildren<IgnitionKey>().on1();
		}
	}

	// Token: 0x0600112B RID: 4395 RVA: 0x000A441D File Offset: 0x000A261D
	protected static void InvokeUserCode_Rpcon1(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC Rpcon1 called on server.");
			return;
		}
		((networkDummy)obj).UserCode_Rpcon1();
	}

	// Token: 0x0600112C RID: 4396 RVA: 0x000A4440 File Offset: 0x000A2640
	protected void UserCode_Cmdon2()
	{
		this.Rpcon2();
	}

	// Token: 0x0600112D RID: 4397 RVA: 0x000A4448 File Offset: 0x000A2648
	protected static void InvokeUserCode_Cmdon2(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command Cmdon2 called on client.");
			return;
		}
		((networkDummy)obj).UserCode_Cmdon2();
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x000A446B File Offset: 0x000A266B
	protected void UserCode_Rpcon2()
	{
		if (this.Target)
		{
			this.Target.GetComponentInChildren<IgnitionKey>().on2();
		}
	}

	// Token: 0x0600112F RID: 4399 RVA: 0x000A448A File Offset: 0x000A268A
	protected static void InvokeUserCode_Rpcon2(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC Rpcon2 called on server.");
			return;
		}
		((networkDummy)obj).UserCode_Rpcon2();
	}

	// Token: 0x06001130 RID: 4400 RVA: 0x000A44AD File Offset: 0x000A26AD
	protected void UserCode_Cmdon3()
	{
		this.Rpcon3();
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x000A44B5 File Offset: 0x000A26B5
	protected static void InvokeUserCode_Cmdon3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command Cmdon3 called on client.");
			return;
		}
		((networkDummy)obj).UserCode_Cmdon3();
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x000A44D8 File Offset: 0x000A26D8
	protected void UserCode_Rpcon3()
	{
		if (this.Target)
		{
			this.Target.GetComponentInChildren<IgnitionKey>().on3();
		}
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x000A44F7 File Offset: 0x000A26F7
	protected static void InvokeUserCode_Rpcon3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC Rpcon3 called on server.");
			return;
		}
		((networkDummy)obj).UserCode_Rpcon3();
	}

	// Token: 0x06001134 RID: 4404 RVA: 0x000A451A File Offset: 0x000A271A
	protected void UserCode_CmdStartCar()
	{
		this.RpcStartCar();
	}

	// Token: 0x06001135 RID: 4405 RVA: 0x000A4522 File Offset: 0x000A2722
	protected static void InvokeUserCode_CmdStartCar(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdStartCar called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdStartCar();
	}

	// Token: 0x06001136 RID: 4406 RVA: 0x000A4545 File Offset: 0x000A2745
	protected void UserCode_RpcStartCar()
	{
		if (this.Target)
		{
			this.Target.GetComponent<MainCarProperties>().StartCar();
		}
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x000A4564 File Offset: 0x000A2764
	protected static void InvokeUserCode_RpcStartCar(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcStartCar called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcStartCar();
	}

	// Token: 0x06001138 RID: 4408 RVA: 0x000A4587 File Offset: 0x000A2787
	protected void UserCode_CmdSetOwnerPlayer()
	{
		this.RpcSetOwnerPlayer();
	}

	// Token: 0x06001139 RID: 4409 RVA: 0x000A458F File Offset: 0x000A278F
	protected static void InvokeUserCode_CmdSetOwnerPlayer(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetOwnerPlayer called on client.");
			return;
		}
		((networkDummy)obj).UserCode_CmdSetOwnerPlayer();
	}

	// Token: 0x0600113A RID: 4410 RVA: 0x000A45B4 File Offset: 0x000A27B4
	protected void UserCode_RpcSetOwnerPlayer()
	{
		if (this.Target)
		{
			if (this.Target.GetComponent<MainCarProperties>())
			{
				this.Target.GetComponent<MainCarProperties>().SetOwnerPlayer();
				return;
			}
			CarProperties[] componentsInChildren = this.Target.GetComponentsInChildren<CarProperties>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Owner = "Player";
			}
		}
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x000A4618 File Offset: 0x000A2818
	protected static void InvokeUserCode_RpcSetOwnerPlayer(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetOwnerPlayer called on server.");
			return;
		}
		((networkDummy)obj).UserCode_RpcSetOwnerPlayer();
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x000A463C File Offset: 0x000A283C
	static networkDummy()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdInputs(System.Single,System.Single,System.Single,System.Single)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdInputs__Single__Single__Single__Single), true);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdCrash(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,System.Single,UnityEngine.Vector3,System.Single)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdCrash__Vector3__Vector3__Vector3__Vector3__Vector3__Vector3__Single__Vector3__Single), true);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdDestroyItem()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdDestroyItem), true);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdRemoveWindow()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdRemoveWindow), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdReducePickupToolInBox()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdReducePickupToolInBox), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdRemoveHand()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdRemoveHand), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdDropOnGround()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdDropOnGround), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdStartMounting()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdStartMounting), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::Cmdopendoor(System.Boolean)", new RemoteCallDelegate(networkDummy.InvokeUserCode_Cmdopendoor__Boolean), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdPlayParticles()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdPlayParticles), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdStopParticles()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdStopParticles), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdPumpIn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdPumpIn), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdPumpOut()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdPumpOut), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdDestroyJoint()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdDestroyJoint), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdOpenGasCup(System.Boolean)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdOpenGasCup__Boolean), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdUpdatePickupitems()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdUpdatePickupitems), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdPickupToolUPDATE(System.Int32,System.Boolean)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdPickupToolUPDATE__Int32__Boolean), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdPickupToolSync(System.Single)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdPickupToolSync__Single), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdFlashLight(System.Boolean)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdFlashLight__Boolean), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdTint2(System.Int32)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdTint2__Int32), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdTint3(System.Int32)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdTint3__Int32), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdFair2(UnityEngine.Vector3,System.Boolean,System.Int32,System.Single,System.Int32,UnityEngine.Quaternion,UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdFair2__Vector3__Boolean__Int32__Single__Int32__Quaternion__Vector3), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdRepair1(UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdRepair1__Vector3), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdRepair22(UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdRepair22__Vector3), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdRustRemoveContinue(System.Boolean,System.Int32,System.Single,System.Int32,UnityEngine.Quaternion,UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdRustRemoveContinue__Boolean__Int32__Single__Int32__Quaternion__Vector3), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdLiftObject(System.Int32,UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdLiftObject__Int32__Vector3), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::Cmdtighten(System.Int32)", new RemoteCallDelegate(networkDummy.InvokeUserCode_Cmdtighten__Int32), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdLoosen(System.Int32)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdLoosen__Int32), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdCut(System.Int32,UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdCut__Int32__Vector3), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdWeld(System.Int32,UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdWeld__Int32__Vector3), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdEnableMovment()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdEnableMovment), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdBRAKE()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdBRAKE), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdAttachPickup(System.Int32,System.Int32,System.Int32)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdAttachPickup__Int32__Int32__Int32), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdDestroyMe()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdDestroyMe), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdWinnchReeling()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdWinnchReeling), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdWindowUp()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdWindowUp), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdWindowDown()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdWindowDown), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdApplyChrome2()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdApplyChrome2), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdSyncFluid(System.Single)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdSyncFluid__Single), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdRunningLightTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdRunningLightTurnOn), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdRunningLightTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdRunningLightTurnOff), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdHeadLightLowTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdHeadLightLowTurnOn), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdHeadLightLowTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdHeadLightLowTurnOff), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdHeadLightHighTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdHeadLightHighTurnOn), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdHeadLightHighTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdHeadLightHighTurnOff), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdBrakeLightTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdBrakeLightTurnOn), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdBrakeLightTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdBrakeLightTurnOff), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdReverseLightTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdReverseLightTurnOn), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdReverseLightTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdReverseLightTurnOff), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdLeftLightTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdLeftLightTurnOn), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdLeftLightTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdLeftLightTurnOff), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdRightLightTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdRightLightTurnOn), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdRightLightTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdRightLightTurnOff), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdWiperSOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdWiperSOn), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdWiperOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdWiperOff), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdIgnitionTurnedOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdIgnitionTurnedOff), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdEngineStop()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdEngineStop), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdChangingGear(System.String,System.Int32)", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdChangingGear__String__Int32), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdsetHandbrake()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdsetHandbrake), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdreleaseHandbrake()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdreleaseHandbrake), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::Cmdon0()", new RemoteCallDelegate(networkDummy.InvokeUserCode_Cmdon0), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::Cmdon1()", new RemoteCallDelegate(networkDummy.InvokeUserCode_Cmdon1), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::Cmdon2()", new RemoteCallDelegate(networkDummy.InvokeUserCode_Cmdon2), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::Cmdon3()", new RemoteCallDelegate(networkDummy.InvokeUserCode_Cmdon3), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdStartCar()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdStartCar), false);
		RemoteProcedureCalls.RegisterCommand(typeof(networkDummy), "System.Void networkDummy::CmdSetOwnerPlayer()", new RemoteCallDelegate(networkDummy.InvokeUserCode_CmdSetOwnerPlayer), false);
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcInputs(System.Single,System.Single,System.Single,System.Single)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcInputs__Single__Single__Single__Single));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcCrash(UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,System.Single,UnityEngine.Vector3,System.Single)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcCrash__Vector3__Vector3__Vector3__Vector3__Vector3__Vector3__Single__Vector3__Single));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcRemoveWindow()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcRemoveWindow));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcReducePickupToolInBox()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcReducePickupToolInBox));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcRemoveHand()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcRemoveHand));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcDropOnGround()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcDropOnGround));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcStartMounting()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcStartMounting));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::Rpcopendoor(System.Boolean)", new RemoteCallDelegate(networkDummy.InvokeUserCode_Rpcopendoor__Boolean));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcPlayParticles()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcPlayParticles));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcStopParticles()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcStopParticles));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcPumpIn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcPumpIn));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcPumpOut()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcPumpOut));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcDestroyJoint()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcDestroyJoint));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcOpenGasCup(System.Boolean)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcOpenGasCup__Boolean));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcUpdatePickupitems()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcUpdatePickupitems));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcPickupToolUPDATE(System.Int32,System.Boolean)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcPickupToolUPDATE__Int32__Boolean));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcPickupToolSync(System.Single)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcPickupToolSync__Single));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcFlashLight(System.Boolean)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcFlashLight__Boolean));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcTint2(System.Int32)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcTint2__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcTint3(System.Int32)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcTint3__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcFair2(UnityEngine.Vector3,System.Boolean,System.Int32,System.Single,System.Int32,UnityEngine.Quaternion,UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcFair2__Vector3__Boolean__Int32__Single__Int32__Quaternion__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcRepair1(UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcRepair1__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcRepair22(UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcRepair22__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcRustRemoveContinue(System.Boolean,System.Int32,System.Single,System.Int32,UnityEngine.Quaternion,UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcRustRemoveContinue__Boolean__Int32__Single__Int32__Quaternion__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcLiftObject(System.Int32,UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcLiftObject__Int32__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::Rpctighten(System.Int32)", new RemoteCallDelegate(networkDummy.InvokeUserCode_Rpctighten__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcLoosen(System.Int32)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcLoosen__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcCut(System.Int32,UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcCut__Int32__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcWeld(System.Int32,UnityEngine.Vector3)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcWeld__Int32__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcEnableMovment()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcEnableMovment));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcBRAKE()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcBRAKE));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcAttachPickup(System.Int32,System.Int32,System.Int32)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcAttachPickup__Int32__Int32__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcDestroyMe()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcDestroyMe));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcWinnchReeling()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcWinnchReeling));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcWindowUp()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcWindowUp));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcWindowDown()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcWindowDown));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcApplyChrome2()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcApplyChrome2));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcSyncFluid(System.Single)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcSyncFluid__Single));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcRunningLightTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcRunningLightTurnOn));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcRunningLightTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcRunningLightTurnOff));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcHeadLightLowTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcHeadLightLowTurnOn));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcHeadLightLowTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcHeadLightLowTurnOff));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcHeadLightHighTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcHeadLightHighTurnOn));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcHeadLightHighTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcHeadLightHighTurnOff));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcBrakeLightTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcBrakeLightTurnOn));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcBrakeLightTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcBrakeLightTurnOff));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcReverseLightTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcReverseLightTurnOn));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcReverseLightTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcReverseLightTurnOff));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcLeftLightTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcLeftLightTurnOn));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcLeftLightTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcLeftLightTurnOff));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcRightLightTurnOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcRightLightTurnOn));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcRightLightTurnOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcRightLightTurnOff));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcWiperSOn()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcWiperSOn));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcWiperOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcWiperOff));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcIgnitionTurnedOff()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcIgnitionTurnedOff));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcEngineStop()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcEngineStop));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcChangingGear(System.String,System.Int32)", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcChangingGear__String__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcsetHandbrake()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcsetHandbrake));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcreleaseHandbrake()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcreleaseHandbrake));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::Rpcon0()", new RemoteCallDelegate(networkDummy.InvokeUserCode_Rpcon0));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::Rpcon1()", new RemoteCallDelegate(networkDummy.InvokeUserCode_Rpcon1));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::Rpcon2()", new RemoteCallDelegate(networkDummy.InvokeUserCode_Rpcon2));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::Rpcon3()", new RemoteCallDelegate(networkDummy.InvokeUserCode_Rpcon3));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcStartCar()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcStartCar));
		RemoteProcedureCalls.RegisterRpc(typeof(networkDummy), "System.Void networkDummy::RpcSetOwnerPlayer()", new RemoteCallDelegate(networkDummy.InvokeUserCode_RpcSetOwnerPlayer));
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x000A56EC File Offset: 0x000A38EC
	public override bool SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteString(this.Itemname);
			writer.WriteVector3(this.Spawnposition);
			writer.WriteQuaternion(this.Spawnrotation);
			writer.WriteInt(this.MPNumber);
			writer.WriteInt(this.CupMPNumber);
			writer.WriteInt(this.SceneNumber);
			writer.WriteInt(this.SavePosition);
			writer.WriteInt(this.ObjectNumber);
			writer.WriteInt(this.SceneLoaded);
			return true;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteString(this.Itemname);
			result = true;
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteVector3(this.Spawnposition);
			result = true;
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteQuaternion(this.Spawnrotation);
			result = true;
		}
		if ((base.syncVarDirtyBits & 8UL) != 0UL)
		{
			writer.WriteInt(this.MPNumber);
			result = true;
		}
		if ((base.syncVarDirtyBits & 16UL) != 0UL)
		{
			writer.WriteInt(this.CupMPNumber);
			result = true;
		}
		if ((base.syncVarDirtyBits & 32UL) != 0UL)
		{
			writer.WriteInt(this.SceneNumber);
			result = true;
		}
		if ((base.syncVarDirtyBits & 64UL) != 0UL)
		{
			writer.WriteInt(this.SavePosition);
			result = true;
		}
		if ((base.syncVarDirtyBits & 128UL) != 0UL)
		{
			writer.WriteInt(this.ObjectNumber);
			result = true;
		}
		if ((base.syncVarDirtyBits & 256UL) != 0UL)
		{
			writer.WriteInt(this.SceneLoaded);
			result = true;
		}
		return result;
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x000A58C8 File Offset: 0x000A3AC8
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.Itemname, null, reader.ReadString());
			base.GeneratedSyncVarDeserialize<Vector3>(ref this.Spawnposition, null, reader.ReadVector3());
			base.GeneratedSyncVarDeserialize<Quaternion>(ref this.Spawnrotation, null, reader.ReadQuaternion());
			base.GeneratedSyncVarDeserialize<int>(ref this.MPNumber, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.CupMPNumber, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.SceneNumber, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.SavePosition, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.ObjectNumber, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.SceneLoaded, null, reader.ReadInt());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.Itemname, null, reader.ReadString());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<Vector3>(ref this.Spawnposition, null, reader.ReadVector3());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<Quaternion>(ref this.Spawnrotation, null, reader.ReadQuaternion());
		}
		if ((num & 8L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.MPNumber, null, reader.ReadInt());
		}
		if ((num & 16L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.CupMPNumber, null, reader.ReadInt());
		}
		if ((num & 32L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.SceneNumber, null, reader.ReadInt());
		}
		if ((num & 64L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.SavePosition, null, reader.ReadInt());
		}
		if ((num & 128L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.ObjectNumber, null, reader.ReadInt());
		}
		if ((num & 256L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.SceneLoaded, null, reader.ReadInt());
		}
	}

	// Token: 0x04001892 RID: 6290
	public bool initialized;

	// Token: 0x04001893 RID: 6291
	[SyncVar]
	public string Itemname;

	// Token: 0x04001894 RID: 6292
	[SyncVar]
	public Vector3 Spawnposition;

	// Token: 0x04001895 RID: 6293
	[SyncVar]
	public Quaternion Spawnrotation;

	// Token: 0x04001896 RID: 6294
	[SyncVar]
	public int MPNumber;

	// Token: 0x04001897 RID: 6295
	[SyncVar]
	public int CupMPNumber;

	// Token: 0x04001898 RID: 6296
	[SyncVar]
	public int SceneNumber;

	// Token: 0x04001899 RID: 6297
	public Transform mapmagic;

	// Token: 0x0400189A RID: 6298
	[SyncVar]
	public int SavePosition;

	// Token: 0x0400189B RID: 6299
	[SyncVar]
	public int ObjectNumber;

	// Token: 0x0400189C RID: 6300
	[SyncVar]
	public int SceneLoaded;

	// Token: 0x0400189D RID: 6301
	private Transform parent;

	// Token: 0x0400189E RID: 6302
	public VehicleDamage DamageScript;

	// Token: 0x0400189F RID: 6303
	public GameObject Target;

	// Token: 0x040018A0 RID: 6304
	private VehicleController _vehicleController;

	// Token: 0x040018A1 RID: 6305
	private MainCarProperties maincarproperties;
}
