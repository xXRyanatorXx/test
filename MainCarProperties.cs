using System;
using System.Collections;
using System.Collections.Generic;
using CarDisablerTest;
using GlobalSnowEffect;
using NWH.Common.SceneManagement;
using NWH.VehiclePhysics2;
using NWH.VehiclePhysics2.Powertrain;
using NWH.VehiclePhysics2.Powertrain.Wheel;
using NWH.VehiclePhysics2.VehicleGUI;
using NWH.WheelController3D;
using PaintIn3D;
using Rewired;
using RVP;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000170 RID: 368
public class MainCarProperties : MonoBehaviour
{
	// Token: 0x060007F8 RID: 2040 RVA: 0x000455F0 File Offset: 0x000437F0
	public void Start()
	{
		GameObject gameObject = new GameObject();
		this.DriftDirection = gameObject.transform;
		this.DriftDirection.parent = base.transform;
		this.DriftDirection.localPosition = new Vector3(0f, 0.4f, 0.15f);
		this.StartPosition = base.transform.position;
		this.StartRotation = base.transform.rotation;
		base.GetComponent<Rigidbody>().isKinematic = true;
		if (base.transform.position.y > 40f)
		{
			base.GetComponent<Rigidbody>().drag = 10f;
			base.GetComponent<Rigidbody>().angularDrag = 10f;
		}
		this.exp = base.transform.root.GetComponent<VehicleController>();
		if (!tools.MPrunning)
		{
			this.exp.Starts();
		}
		base.StartCoroutine(this.FinishedInitializing());
		this.AudioParent = GameObject.Find("hand");
		this.NumberParent = GameObject.Find("NumberPlates").GetComponent<NumberPlateManager>();
		this.ExhaustSmokes = base.transform.Find("ExhaustSmoke").GetComponent<ParticleSystem>();
		this.ExhaustSmokes2 = base.transform.Find("ExhaustSmoke2").GetComponent<ParticleSystem>();
		this.CoolantSmokes = base.transform.Find("CoolantSmoke").GetComponent<ParticleSystem>();
		this.PreventChildCollisions();
		this.player = ReInput.players.GetPlayer(0);
		this.WipersStopped = true;
		if (!this.Bike)
		{
			this.WCFL = this.FLwhellcontroller.GetComponent<WheelController>();
			this.WCFR = this.FRwhellcontroller.GetComponent<WheelController>();
			this.WCRL = this.RLwhellcontroller.GetComponent<WheelController>();
			this.WCRR = this.RRwhellcontroller.GetComponent<WheelController>();
		}
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x000457B9 File Offset: 0x000439B9
	private IEnumerator FinishedInitializing()
	{
		yield return 0;
		yield return 0;
		if (base.transform.position.y > 40f)
		{
			base.GetComponent<Rigidbody>().drag = 10f;
			base.GetComponent<Rigidbody>().angularDrag = 10f;
		}
		if (this.Started)
		{
			FLUID[] componentsInChildren = base.GetComponentsInChildren<FLUID>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].StartFromMain();
			}
		}
		this.Player = GameObject.Find("Player");
		this.exp = base.transform.root.GetComponent<VehicleController>();
		yield return 0;
		yield return 0;
		yield return 0;
		base.transform.position = this.StartPosition;
		base.transform.rotation = this.StartRotation;
		if (base.transform.position.y > 40f)
		{
			base.GetComponent<Rigidbody>().drag = 10f;
			base.GetComponent<Rigidbody>().angularDrag = 10f;
		}
		Partinfo[] componentsInChildren2 = base.GetComponentsInChildren<Partinfo>();
		foreach (Partinfo partinfo in componentsInChildren2)
		{
			partinfo.fixedImportantBolts = 0f;
			partinfo.fixedwelds = 0f;
		}
		BoltNut[] componentsInChildren3 = base.GetComponentsInChildren<BoltNut>();
		for (int i = 0; i < componentsInChildren3.Length; i++)
		{
			componentsInChildren3[i].StartFromMain();
		}
		WeldCut[] componentsInChildren4 = base.GetComponentsInChildren<WeldCut>();
		for (int i = 0; i < componentsInChildren4.Length; i++)
		{
			componentsInChildren4[i].StartFromMain();
		}
		foreach (Partinfo partinfo2 in componentsInChildren2)
		{
			if ((partinfo2.attachedbolts > 0f || partinfo2.attachedwelds > 0f || partinfo2.ImportantBolts > 0f) && partinfo2.tightnuts == 0f && partinfo2.fixedwelds == 0f && partinfo2.fixedImportantBolts == 0f)
			{
				partinfo2.remove(true);
			}
		}
		this.Player.GetComponent<VehicleChanger>().FindVehicles();
		yield return new WaitForSeconds(3f);
		if (base.GetComponent<DisablerSlave>() && !base.GetComponent<DisablerSlave>().CurrentlySleeping)
		{
			base.GetComponent<Rigidbody>().isKinematic = false;
		}
		this.CheckStates();
		this.CheckDrivetrain();
		this.exp.Starts();
		this.ExhaustSmokes.Stop();
		this.ExhaustSmokes2.Stop();
		this.CoolantSmokes.Stop();
		this.ResetWheelControllers();
		base.GetComponent<VehicleDamage>().Start();
		if (this.Crankshaft && this.Crankshaft.VisualObject)
		{
			this.Crankshaft.VisualObject.transform.localRotation = Quaternion.identity;
		}
		this.Started = true;
		yield return new WaitForSeconds(5f);
		base.GetComponent<Rigidbody>().drag = 0f;
		base.GetComponent<Rigidbody>().angularDrag = 0f;
		yield break;
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x000457C8 File Offset: 0x000439C8
	public void GetPartStatsEng()
	{
		this.RuinedEngineParts = "";
		this.WornEngineParts = "";
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.PREFAB && carProperties.PREFAB.GetComponent<Partinfo>().Engine)
			{
				if (carProperties.Condition < 0.1f || carProperties.Ruined)
				{
					this.RuinedEngineParts = this.RuinedEngineParts + carProperties.PREFAB.name + "\n";
				}
				else if (carProperties.Condition < 0.4f)
				{
					this.WornEngineParts = this.WornEngineParts + carProperties.PREFAB.name + "\n";
				}
			}
		}
		if (this.RuinedEngineParts == "")
		{
			this.RuinedEngineParts = "No ruined parts";
		}
		if (this.WornEngineParts == "")
		{
			this.WornEngineParts = "No worn parts";
		}
		this.CantCrank = "";
		if (!this.Starter)
		{
			this.CantCrank += " missing starter,";
		}
		else if (this.Starter.Condition < 0.1f)
		{
			this.CantCrank += " bad starter,";
		}
		if (!this.Battery)
		{
			this.CantCrank += " missing battery,";
		}
		else if (this.Battery.BatteryCharge < 12f)
		{
			this.CantCrank += " low voltage,";
		}
		if (!this.BatteryWires)
		{
			this.CantCrank += " missing main wires,";
		}
		else if (this.BatteryWires.GetComponent<Partinfo>().fixedImportantBolts < 4f)
		{
			this.CantCrank += " main wires loose,";
		}
		if (!this.Flywheel)
		{
			this.CantCrank += " missing flywheel,";
		}
		if (this.CantCrank != "")
		{
			this.CantCrank = "Cant crank because of " + this.CantCrank;
		}
		this.CantRun = "";
		if (!this.EngineBlock)
		{
			this.CantRun += " missing engine block,";
		}
		else if (this.EngineBlock.Condition < 0f)
		{
			this.CantRun += " bad engine block,";
		}
		if (!this.EngineHead)
		{
			this.CantRun += " missing engine head,";
		}
		else if (this.EngineHead.Condition < 0f)
		{
			this.CantRun += " bad engine head,";
		}
		if (!this.Rockers)
		{
			this.CantRun += " missing rockers,";
		}
		if (!this.Crankshaft)
		{
			this.CantRun += " missing engine crankshaft,";
		}
		if (!this.Camshaft)
		{
			this.CantRun += " missing engine camshaft,";
		}
		if (!this.Camshaft2 && this.TwinCam)
		{
			this.CantRun += " missing engine camshaft2,";
		}
		if (!this.EngineChain)
		{
			this.CantRun += " missing engine engine chain,";
		}
		else if (this.EngineChain.Condition < 0.1f)
		{
			this.CantRun += " bad engine chain,";
		}
		if (!this.CamshaftSprocket)
		{
			this.CantRun += " camshaft sprocket,";
		}
		if (!this.CrankshaftSprocket)
		{
			this.CantRun += " missing crankshaft sprocket,";
		}
		if (!this.Fuel)
		{
			this.CantRun += " missing gas tank,";
		}
		else if (this.Fuel.FluidSize <= 0.1f)
		{
			this.CantRun += " missing fuel";
		}
		if (!this.FuelLine)
		{
			this.CantRun += " missing fuel line,";
		}
		if (!this.FuelPump)
		{
			this.CantRun += " missing fuel pump,";
		}
		else if (this.FuelPump.Condition < 0.1f)
		{
			this.CantRun += " bad fuel pump,";
		}
		if (this.pistons != this.EngineBlock.EngineCylinderCount)
		{
			this.CantRun += " missing pistons,";
		}
		if (this.DieselEngine)
		{
			if (this.Fuel && this.Fuel.DieselPercent < 0.6f)
			{
				this.CantRun += " wrong fuel,";
			}
			if (!this.FuelHoses)
			{
				this.CantRun += " missing fuel hoses,";
			}
			if (this.injectors != this.EngineBlock.EngineCylinderCount)
			{
				this.CantRun += " missing injectors,";
			}
		}
		else
		{
			if (this.Fuel && this.Fuel.DieselPercent > 0.4f)
			{
				this.CantRun += " wrong fuel,";
			}
			if (!this.Carburetor)
			{
				this.CantRun += " missing carburetor,";
			}
			else if (this.Carburetor.Condition < 0f)
			{
				this.CantRun += " bad carburetor,";
			}
			if (!this.IgnitionCoil)
			{
				this.CantRun += " missing ignition coil,";
			}
			else if (this.IgnitionCoil.Condition < 0f)
			{
				this.CantRun += " bad ignition coil,";
			}
			if (!this.SparkWires)
			{
				this.CantRun += " missing sparkwires,";
			}
			if (!this.Distributor)
			{
				this.CantRun += " missing distributor,";
			}
			if (this.sparkplugs != this.EngineBlock.EngineCylinderCount)
			{
				this.CantRun += " missing sparkplugs,";
			}
			if (this.Injected)
			{
				if (!this.FuelHoses)
				{
					this.CantRun += " missing fuel rail,";
				}
				if (this.injectors != this.EngineBlock.EngineCylinderCount)
				{
					this.CantRun += " missing injectors,";
				}
			}
		}
		if (this.CantRun != "")
		{
			this.CantRun = "Cant run because of " + this.CantRun;
		}
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00045F0C File Offset: 0x0004410C
	public void GetPartStatsBr()
	{
		this.RuinedBrakeParts = "";
		this.WornBrakeParts = "";
		float num = 0f;
		float num2 = 0f;
		bool flag = false;
		bool flag2 = false;
		this.BrakeProblems = "";
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.PREFAB)
			{
				if (carProperties.PREFAB.GetComponent<Partinfo>().Brakes)
				{
					if (carProperties.Condition < 0.1f || carProperties.Ruined)
					{
						this.RuinedBrakeParts = this.RuinedBrakeParts + carProperties.PREFAB.name + "\n";
					}
					else if (carProperties.Condition < 0.4f)
					{
						this.WornBrakeParts = this.WornBrakeParts + carProperties.PREFAB.name + "\n";
					}
				}
				if (carProperties.BrakeDisc)
				{
					num += 1f;
					if (carProperties.Condition < 0.4f)
					{
						flag = true;
					}
				}
				if (carProperties.BrakePad)
				{
					num2 += 1f;
					if (carProperties.Condition < 0.4f)
					{
						flag2 = true;
					}
				}
			}
		}
		if (this.RuinedBrakeParts == "")
		{
			this.RuinedBrakeParts = "No ruined parts";
		}
		if (this.WornBrakeParts == "")
		{
			this.WornBrakeParts = "No worn parts";
		}
		if (!this.Bike)
		{
			if (!this.MainBrakeLIne)
			{
				this.BrakeProblems += " missing main brake line,";
			}
			else if (this.MainBrakeLIne.GetComponent<CarProperties>().Condition < 0.1f)
			{
				this.BrakeProblems += " bad main brake line,";
			}
			if (!this.BrakeLIneFL || !this.BrakeLIneFR || !this.BrakeLIneRL || !this.BrakeLIneRR)
			{
				this.BrakeProblems += " missing fuel lines,";
			}
			else if (this.BrakeLIneFL.GetComponent<CarProperties>().Condition < 0.1f || this.BrakeLIneFR.GetComponent<CarProperties>().Condition < 0.1f || this.BrakeLIneRL.GetComponent<CarProperties>().Condition < 0.1f || this.BrakeLIneRR.GetComponent<CarProperties>().Condition < 0.1f)
			{
				this.BrakeProblems += "bad fuel lines,";
			}
			if (this.MainBrakeLIne && this.BrakeLIneFL && this.BrakeLIneFR && this.BrakeLIneRL && this.BrakeLIneRR && (this.MainBrakeLIne.GetComponent<Partinfo>().fixedImportantBolts < this.MainBrakeLIne.GetComponent<Partinfo>().ImportantBolts || this.MainBrakeLIne.GetComponent<Partinfo>().tightnuts < this.MainBrakeLIne.GetComponent<Partinfo>().attachedbolts || this.BrakeLIneFL.GetComponent<Partinfo>().fixedImportantBolts < this.BrakeLIneFL.GetComponent<Partinfo>().ImportantBolts || this.BrakeLIneFL.GetComponent<Partinfo>().tightnuts < this.BrakeLIneFL.GetComponent<Partinfo>().attachedbolts || this.BrakeLIneFR.GetComponent<Partinfo>().fixedImportantBolts < this.BrakeLIneFR.GetComponent<Partinfo>().ImportantBolts || this.BrakeLIneFR.GetComponent<Partinfo>().tightnuts < this.BrakeLIneFR.GetComponent<Partinfo>().attachedbolts || this.BrakeLIneRL.GetComponent<Partinfo>().fixedImportantBolts < this.BrakeLIneRL.GetComponent<Partinfo>().ImportantBolts || this.BrakeLIneRL.GetComponent<Partinfo>().tightnuts < this.BrakeLIneRL.GetComponent<Partinfo>().attachedbolts || this.BrakeLIneRR.GetComponent<Partinfo>().fixedImportantBolts < this.BrakeLIneRR.GetComponent<Partinfo>().ImportantBolts || this.BrakeLIneRR.GetComponent<Partinfo>().tightnuts < this.BrakeLIneRR.GetComponent<Partinfo>().attachedbolts))
			{
				this.BrakeProblems += " brake lines have loose bolts,";
			}
			if (!this.BrakePedal)
			{
				this.BrakeProblems += " missing brake pedal,";
			}
			if (!this.BrakeFluid)
			{
				this.BrakeProblems += " missing master cylinder,";
			}
			else if (this.BrakeFluid.FluidSize < this.BrakeFluid.MinFluidSize)
			{
				this.BrakeProblems += " low brake fluid level,";
			}
			if (num < 4f)
			{
				this.BrakeProblems += " missing brake discs,";
			}
			if (num2 < 8f)
			{
				this.BrakeProblems += " missing brake pads,";
			}
			if (flag)
			{
				this.BrakeProblems += " bad brake discs,";
			}
			if (flag2)
			{
				this.BrakeProblems += " bad brake pads,";
			}
		}
		else
		{
			if (!this.BrakePedal)
			{
				this.BrakeProblems += " missing brake pedal,";
			}
			if (num < 2f)
			{
				this.BrakeProblems += " missing brake discs,";
			}
			if (num2 < 4f)
			{
				this.BrakeProblems += " missing brake pads,";
			}
			if (flag)
			{
				this.BrakeProblems += " bad brake discs,";
			}
			if (flag2)
			{
				this.BrakeProblems += " bad brake pads,";
			}
		}
		if (this.BrakeProblems != "")
		{
			this.BrakeProblems = "Brake problems : " + this.BrakeProblems;
		}
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x0004650C File Offset: 0x0004470C
	public void GetPartStatsSusp()
	{
		this.RuinedSuspensionParts = "";
		this.WornSuspensionParts = "";
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.PREFAB && carProperties.PREFAB.GetComponent<Partinfo>().Suspension)
			{
				if (carProperties.Condition < 0.1f || carProperties.Ruined)
				{
					this.RuinedSuspensionParts = this.RuinedSuspensionParts + carProperties.PREFAB.name + "\n";
				}
				else if (carProperties.Condition < 0.4f)
				{
					this.WornSuspensionParts = this.WornSuspensionParts + carProperties.PREFAB.name + "\n";
				}
			}
		}
		if (this.RuinedSuspensionParts == "")
		{
			this.RuinedSuspensionParts = "No ruined parts";
		}
		if (this.WornSuspensionParts == "")
		{
			this.WornSuspensionParts = "No worn parts";
		}
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00046610 File Offset: 0x00044810
	public void GetPartStatsFull()
	{
		float num = 0f;
		float num2 = 0f;
		bool flag = false;
		bool flag2 = false;
		this.RuinedEngineParts = "";
		this.WornEngineParts = "";
		this.RuinedSuspensionParts = "";
		this.WornSuspensionParts = "";
		this.RuinedBrakeParts = "";
		this.WornBrakeParts = "";
		this.RuinedOtherParts = "";
		this.WornOtherparts = "";
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.PREFAB)
			{
				if (carProperties.PREFAB.GetComponent<Partinfo>().Engine)
				{
					if (carProperties.Condition < 0.1f || carProperties.Ruined)
					{
						this.RuinedEngineParts = this.RuinedEngineParts + carProperties.PREFAB.name + "\n";
					}
					else if (carProperties.Condition < 0.4f)
					{
						this.WornEngineParts = this.WornEngineParts + carProperties.PREFAB.name + "\n";
					}
				}
				else if (carProperties.PREFAB.GetComponent<Partinfo>().Suspension)
				{
					if (carProperties.Condition < 0.1f || carProperties.Ruined)
					{
						this.RuinedSuspensionParts = this.RuinedSuspensionParts + carProperties.PREFAB.name + "\n";
					}
					else if (carProperties.Condition < 0.4f)
					{
						this.WornSuspensionParts = this.WornSuspensionParts + carProperties.PREFAB.name + "\n";
					}
				}
				else if (carProperties.PREFAB.GetComponent<Partinfo>().Brakes)
				{
					if (carProperties.Condition < 0.1f || carProperties.Ruined)
					{
						this.RuinedBrakeParts = this.RuinedBrakeParts + carProperties.PREFAB.name + "\n";
					}
					else if (carProperties.Condition < 0.4f)
					{
						this.WornBrakeParts = this.WornBrakeParts + carProperties.PREFAB.name + "\n";
					}
				}
				else if (carProperties.Condition < 0.1f)
				{
					this.RuinedOtherParts = this.RuinedOtherParts + carProperties.PREFAB.name + "\n";
				}
				else if (carProperties.Condition < 0.4f)
				{
					this.WornOtherparts = this.WornOtherparts + carProperties.PREFAB.name + "\n";
				}
				if (carProperties.BrakeDisc)
				{
					num += 1f;
					if (carProperties.Condition < 0.4f)
					{
						flag = true;
					}
				}
				if (carProperties.BrakePad)
				{
					num2 += 1f;
					if (carProperties.Condition < 0.4f)
					{
						flag2 = true;
					}
				}
			}
		}
		if (this.RuinedEngineParts == "")
		{
			this.RuinedEngineParts = "No ruined parts";
		}
		if (this.WornEngineParts == "")
		{
			this.WornEngineParts = "No worn parts";
		}
		if (this.RuinedSuspensionParts == "")
		{
			this.RuinedSuspensionParts = "No ruined parts";
		}
		if (this.WornSuspensionParts == "")
		{
			this.WornSuspensionParts = "No worn parts";
		}
		if (this.RuinedBrakeParts == "")
		{
			this.RuinedBrakeParts = "No ruined parts";
		}
		if (this.WornBrakeParts == "")
		{
			this.WornBrakeParts = "No worn parts";
		}
		if (this.RuinedOtherParts == "")
		{
			this.RuinedOtherParts = "No ruined parts";
		}
		if (this.WornOtherparts == "")
		{
			this.WornOtherparts = "No worn parts";
		}
		this.CantCrank = "";
		if (!this.Starter)
		{
			this.CantCrank += " missing starter,";
		}
		else if (this.Starter.Condition < 0.1f)
		{
			this.CantCrank += " bad starter,";
		}
		if (!this.Battery)
		{
			this.CantCrank += " missing battery,";
		}
		else if (this.Battery.BatteryCharge < 12f)
		{
			this.CantCrank += " low voltage,";
		}
		if (!this.BatteryWires)
		{
			this.CantCrank += " missing main wires,";
		}
		else if (this.BatteryWires.GetComponent<Partinfo>().fixedImportantBolts < 4f)
		{
			this.CantCrank += " main wires loose,";
		}
		if (!this.Flywheel)
		{
			this.CantCrank += " missing flywheel,";
		}
		if (this.CantCrank != "")
		{
			this.CantCrank = "Cant crank because of " + this.CantCrank;
		}
		this.CantRun = "";
		if (!this.EngineBlock)
		{
			this.CantRun += " missing engine block,";
		}
		else if (this.EngineBlock.Condition < 0f)
		{
			this.CantRun += " bad engine block,";
		}
		if (!this.EngineHead)
		{
			this.CantRun += " missing engine head,";
		}
		else if (this.EngineHead.Condition < 0f)
		{
			this.CantRun += " bad engine head,";
		}
		if (!this.Rockers)
		{
			this.CantRun += " missing rockers,";
		}
		if (!this.Crankshaft)
		{
			this.CantRun += " missing engine crankshaft,";
		}
		if (!this.Camshaft)
		{
			this.CantRun += " missing engine camshaft,";
		}
		if (!this.EngineChain)
		{
			this.CantRun += " missing engine engine chain,";
		}
		else if (this.EngineChain.Condition < 0.1f)
		{
			this.CantRun += " bad engine chain,";
		}
		if (!this.CamshaftSprocket)
		{
			this.CantRun += " camshaft sprocket,";
		}
		if (!this.CrankshaftSprocket)
		{
			this.CantRun += " missing crankshaft sprocket,";
		}
		if (!this.Camshaft2 && this.TwinCam)
		{
			this.CantRun += " missing engine camshaft2,";
		}
		if (!this.Fuel)
		{
			this.CantRun += " missing gas tank,";
		}
		else if (this.Fuel.FluidSize <= 0.1f)
		{
			this.CantRun += " missing fuel";
		}
		if (!this.FuelLine)
		{
			this.CantRun += " missing fuel line,";
		}
		if (!this.FuelPump)
		{
			this.CantRun += " missing fuel pump,";
		}
		else if (this.FuelPump.Condition < 0.1f)
		{
			this.CantRun += " bad fuel pump,";
		}
		if (this.pistons != this.EngineBlock.EngineCylinderCount)
		{
			this.CantRun += " missing pistons,";
		}
		if (this.DieselEngine)
		{
			if (this.Fuel && this.Fuel.DieselPercent < 0.6f)
			{
				this.CantRun += " wrong fuel,";
			}
			if (!this.FuelHoses)
			{
				this.CantRun += " missing fuel hoses,";
			}
			if (this.injectors != this.EngineBlock.EngineCylinderCount)
			{
				this.CantRun += " missing injectors,";
			}
		}
		else
		{
			if (this.Fuel && this.Fuel.DieselPercent > 0.4f)
			{
				this.CantRun += " wrong fuel,";
			}
			if (!this.Carburetor)
			{
				this.CantRun += " missing carburetor,";
			}
			else if (this.Carburetor.Condition < 0f)
			{
				this.CantRun += " bad carburetor,";
			}
			if (!this.IgnitionCoil)
			{
				this.CantRun += " missing ignition coil,";
			}
			else if (this.IgnitionCoil.Condition < 0f)
			{
				this.CantRun += " bad ignition coil,";
			}
			if (!this.SparkWires)
			{
				this.CantRun += " missing sparkwires,";
			}
			if (!this.Distributor)
			{
				this.CantRun += " missing distributor,";
			}
			if (this.sparkplugs != this.EngineBlock.EngineCylinderCount)
			{
				this.CantRun += " missing sparkplugs,";
			}
			if (this.Injected)
			{
				if (!this.FuelHoses)
				{
					this.CantRun += " missing fuel rail,";
				}
				if (this.injectors != this.EngineBlock.EngineCylinderCount)
				{
					this.CantRun += " missing injectors,";
				}
			}
		}
		if (this.CantRun != "")
		{
			this.CantRun = "Cant run because of " + this.CantRun;
		}
		this.BrakeProblems = "";
		if (!this.Bike)
		{
			if (!this.MainBrakeLIne)
			{
				this.BrakeProblems += " missing main brake line,";
			}
			else if (this.MainBrakeLIne.GetComponent<CarProperties>().Condition < 0.1f)
			{
				this.BrakeProblems += " bad main brake line,";
			}
			if (!this.BrakeLIneFL || !this.BrakeLIneFR || !this.BrakeLIneRL || !this.BrakeLIneRR)
			{
				this.BrakeProblems += " missing fuel lines,";
			}
			else if (this.BrakeLIneFL.GetComponent<CarProperties>().Condition < 0.1f || this.BrakeLIneFR.GetComponent<CarProperties>().Condition < 0.1f || this.BrakeLIneRL.GetComponent<CarProperties>().Condition < 0.1f || this.BrakeLIneRR.GetComponent<CarProperties>().Condition < 0.1f)
			{
				this.BrakeProblems += "bad fuel lines,";
			}
			if (this.MainBrakeLIne && this.BrakeLIneFL && this.BrakeLIneFR && this.BrakeLIneRL && this.BrakeLIneRR && (this.MainBrakeLIne.GetComponent<Partinfo>().fixedImportantBolts < this.MainBrakeLIne.GetComponent<Partinfo>().ImportantBolts || this.MainBrakeLIne.GetComponent<Partinfo>().tightnuts < this.MainBrakeLIne.GetComponent<Partinfo>().attachedbolts || this.BrakeLIneFL.GetComponent<Partinfo>().fixedImportantBolts < this.BrakeLIneFL.GetComponent<Partinfo>().ImportantBolts || this.BrakeLIneFL.GetComponent<Partinfo>().tightnuts < this.BrakeLIneFL.GetComponent<Partinfo>().attachedbolts || this.BrakeLIneFR.GetComponent<Partinfo>().fixedImportantBolts < this.BrakeLIneFR.GetComponent<Partinfo>().ImportantBolts || this.BrakeLIneFR.GetComponent<Partinfo>().tightnuts < this.BrakeLIneFR.GetComponent<Partinfo>().attachedbolts || this.BrakeLIneRL.GetComponent<Partinfo>().fixedImportantBolts < this.BrakeLIneRL.GetComponent<Partinfo>().ImportantBolts || this.BrakeLIneRL.GetComponent<Partinfo>().tightnuts < this.BrakeLIneRL.GetComponent<Partinfo>().attachedbolts || this.BrakeLIneRR.GetComponent<Partinfo>().fixedImportantBolts < this.BrakeLIneRR.GetComponent<Partinfo>().ImportantBolts || this.BrakeLIneRR.GetComponent<Partinfo>().tightnuts < this.BrakeLIneRR.GetComponent<Partinfo>().attachedbolts))
			{
				this.BrakeProblems += " brake lines have loose bolts,";
			}
			if (!this.BrakePedal)
			{
				this.BrakeProblems += " missing brake pedal,";
			}
			if (!this.BrakeFluid)
			{
				this.BrakeProblems += " missing master cylinder,";
			}
			else if (this.BrakeFluid.FluidSize < this.BrakeFluid.MinFluidSize)
			{
				this.BrakeProblems += " low brake fluid level,";
			}
			if (num < 4f)
			{
				this.BrakeProblems += " missing brake discs,";
			}
			if (num2 < 8f)
			{
				this.BrakeProblems += " missing brake pads,";
			}
			if (flag)
			{
				this.BrakeProblems += " bad brake discs,";
			}
			if (flag2)
			{
				this.BrakeProblems += " bad brake pads,";
			}
		}
		else
		{
			if (!this.BrakePedal)
			{
				this.BrakeProblems += " missing brake pedal,";
			}
			if (num < 2f)
			{
				this.BrakeProblems += " missing brake discs,";
			}
			if (num2 < 4f)
			{
				this.BrakeProblems += " missing brake pads,";
			}
			if (flag)
			{
				this.BrakeProblems += " bad brake discs,";
			}
			if (flag2)
			{
				this.BrakeProblems += " bad brake pads,";
			}
		}
		if (this.BrakeProblems != "")
		{
			this.BrakeProblems = "Brake problems : " + this.BrakeProblems;
		}
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x000474BC File Offset: 0x000456BC
	public void PreventChildCollisions()
	{
		if (!base.transform.parent)
		{
			Collider[] componentsInChildren = base.GetComponentsInChildren<MeshCollider>();
			Collider[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = i + 1; j < array.Length; j++)
				{
					if (!(array[i] == array[j]))
					{
						Physics.IgnoreCollision(array[i], array[j]);
					}
				}
			}
		}
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x0004751C File Offset: 0x0004571C
	public void ResetWheelControllers()
	{
		WheelController[] componentsInChildren = base.GetComponentsInChildren<WheelController>();
		foreach (WheelController wheelController in componentsInChildren)
		{
			wheelController.radius = (wheelController.radiusBackup = 0.14f);
			if (!this.Bike)
			{
				wheelController.springLength = 0.35f;
			}
			else
			{
				wheelController.spring.maxLength = 0.25f;
			}
			wheelController.springMaximumForce = 100f;
			wheelController.damperBumpForce = 100f;
			wheelController.damperReboundForce = 100f;
			if (!this.Bike)
			{
				wheelController.GetComponent<WheelController>().NonRotatingVisual = null;
			}
			if (this.NoRearSprings && wheelController.transform.name == "WheelControllerRear")
			{
				wheelController.springLength = 0.02f;
				wheelController.springMaximumForce = 4000f;
				wheelController.damperBumpForce = 4000f;
				wheelController.damperReboundForce = 4000f;
			}
		}
		CarProperties[] componentsInChildren2 = base.GetComponentsInChildren<CarProperties>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].ReStart();
		}
		foreach (WheelController wheelController2 in componentsInChildren)
		{
			foreach (object obj in wheelController2.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.name == "SurfaceParticles")
				{
					transform.transform.position = wheelController2.wheel.worldPosition - wheelController2.cachedTransform.up * (wheelController2.radius * 0.5f);
					if (EnviroSkyMgr.instance && EnviroSkyMgr.instance.Seasons.currentSeasons == EnviroSeasons.Seasons.Winter)
					{
						transform.gameObject.SetActive(false);
					}
					else
					{
						transform.gameObject.SetActive(true);
					}
				}
				if (transform.gameObject.name == "SurfaceChunks")
				{
					transform.transform.position = wheelController2.wheel.worldPosition - wheelController2.cachedTransform.up * wheelController2.radius - wheelController2.cachedTransform.forward * (wheelController2.radius * 0.7f);
					if (EnviroSkyMgr.instance && EnviroSkyMgr.instance.Seasons.currentSeasons == EnviroSeasons.Seasons.Winter)
					{
						transform.gameObject.SetActive(false);
					}
					else
					{
						transform.gameObject.SetActive(true);
					}
				}
			}
			if (this.Bike && !this.SittingInCar)
			{
				wheelController2.Freeze();
			}
		}
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x000477F4 File Offset: 0x000459F4
	public void SetOwnerPlayer()
	{
		this.Owner = "Player";
		CarProperties[] componentsInChildren = base.GetComponentsInChildren<CarProperties>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Owner = "Player";
		}
		if (this.IgnitionKey)
		{
			this.IgnitionKey.GetComponent<CarProperties>().ReStart();
		}
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x0004784C File Offset: 0x00045A4C
	public void SetSleep()
	{
		DISABLER[] componentsInChildren = base.GetComponentsInChildren<DISABLER>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].DisableFromMainCar();
		}
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00047878 File Offset: 0x00045A78
	public void SetAwake()
	{
		DISABLER[] componentsInChildren = base.GetComponentsInChildren<DISABLER>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].EnableFromMainCar();
		}
		this.CheckStates();
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x000478A8 File Offset: 0x00045AA8
	public void CheckChildVisConditions()
	{
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.Condition <= 0.4f && carProperties.SinglePart)
			{
				carProperties.CheckVisualCondition();
			}
		}
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x000478EC File Offset: 0x00045AEC
	public void CheckStates()
	{
		if (this.SittingInCar || (this.insideitems && this.insideitems.transform.childCount > 1))
		{
			return;
		}
		if (this.HandbrakeCable && this.Handbrake)
		{
			if (this.HandbrakeCable.GetComponent<CarProperties>().Condition > 0.1f)
			{
				this.exp.brakes.HandbrakeWorkingOrder = true;
			}
			else
			{
				this.exp.brakes.HandbrakeWorkingOrder = false;
			}
		}
		else
		{
			this.exp.brakes.HandbrakeWorkingOrder = false;
		}
		if (!this.Bike && this.MainBrakeLIne && this.BrakeLIneFL && this.BrakeLIneFR && this.BrakeLIneRL && this.BrakeLIneRR)
		{
			if (this.MainBrakeLIne.GetComponent<CarProperties>().Condition < 0.1f || this.MainBrakeLIne.GetComponent<Partinfo>().fixedImportantBolts < this.MainBrakeLIne.GetComponent<Partinfo>().ImportantBolts || this.MainBrakeLIne.GetComponent<Partinfo>().tightnuts < this.MainBrakeLIne.GetComponent<Partinfo>().attachedbolts || this.BrakeLIneFL.GetComponent<CarProperties>().Condition < 0.1f || this.BrakeLIneFL.GetComponent<Partinfo>().fixedImportantBolts < this.BrakeLIneFL.GetComponent<Partinfo>().ImportantBolts || this.BrakeLIneFL.GetComponent<Partinfo>().tightnuts < this.BrakeLIneFL.GetComponent<Partinfo>().attachedbolts || this.BrakeLIneFR.GetComponent<CarProperties>().Condition < 0.1f || this.BrakeLIneFR.GetComponent<Partinfo>().fixedImportantBolts < this.BrakeLIneFR.GetComponent<Partinfo>().ImportantBolts || this.BrakeLIneFR.GetComponent<Partinfo>().tightnuts < this.BrakeLIneFR.GetComponent<Partinfo>().attachedbolts || this.BrakeLIneRL.GetComponent<CarProperties>().Condition < 0.1f || this.BrakeLIneRL.GetComponent<Partinfo>().fixedImportantBolts < this.BrakeLIneRL.GetComponent<Partinfo>().ImportantBolts || this.BrakeLIneRL.GetComponent<Partinfo>().tightnuts < this.BrakeLIneRL.GetComponent<Partinfo>().attachedbolts || this.BrakeLIneRR.GetComponent<CarProperties>().Condition < 0.1f || this.BrakeLIneRR.GetComponent<Partinfo>().fixedImportantBolts < this.BrakeLIneRR.GetComponent<Partinfo>().ImportantBolts || this.BrakeLIneRR.GetComponent<Partinfo>().tightnuts < this.BrakeLIneRR.GetComponent<Partinfo>().attachedbolts)
			{
				this.BrakesLeaking = true;
			}
			else
			{
				this.BrakesLeaking = false;
			}
		}
		else if (!this.Bike)
		{
			this.BrakesLeaking = true;
		}
		else
		{
			this.BrakesLeaking = false;
		}
		this.CanBrake = false;
		if (this.BrakePedal && this.MainBrakeLIne && this.BrakeFluid && this.BrakeFluid.FluidSize > this.BrakeFluid.MinFluidSize && this.BrakeLIneFL && this.BrakeLIneFR && this.BrakeLIneRL && this.BrakeLIneRR)
		{
			this.CanBrake = true;
		}
		if (this.Bike && this.BrakePedal)
		{
			this.CanBrake = true;
		}
		this.FLOkParts = 0f;
		this.FROkParts = 0f;
		this.RLOkParts = 0f;
		this.RROkParts = 0f;
		this.Weight = 10f;
		if (this.Bike)
		{
			this.Weight = 100f;
		}
		this.CarPartPrice = 0f;
		CarProperties[] componentsInChildren = base.GetComponentsInChildren<CarProperties>();
		foreach (CarProperties carProperties in componentsInChildren)
		{
			if (carProperties.AffectsHandling && carProperties.Condition > 0.1f)
			{
				if (carProperties.FL)
				{
					this.FLOkParts += 1f;
				}
				if (carProperties.FR)
				{
					this.FROkParts += 1f;
				}
				if (carProperties.RL)
				{
					this.RLOkParts += 1f;
				}
				if (carProperties.RR)
				{
					this.RROkParts += 1f;
				}
			}
			if (carProperties.PREFAB)
			{
				this.Weight += carProperties.gameObject.GetComponent<Partinfo>().weight;
				this.CarPartPrice += carProperties.gameObject.GetComponent<Partinfo>().price;
			}
		}
		base.GetComponent<Rigidbody>().mass = this.Weight;
		if (!this.Bike)
		{
			base.GetComponent<Rigidbody>().mass = 1100f;
		}
		if (this.Gearbox && this.Shifter && this.EngineBlock && (this.EngineBlock.Type == this.Gearbox.Type || this.EngineBlock.Type2 == this.Gearbox.Type))
		{
			if (this.Gearbox.Condition > 0.1f)
			{
				this.gearingProfil = this.Gearbox.TransmissionGearingProfile;
			}
			if (this.Gearbox.Condition <= 0.1f && this.Gearbox.x == 1)
			{
				this.gearingProfil = this.Gearbox.TransmissionGearingbroken1;
			}
			if (this.Gearbox.Condition <= 0.1f && this.Gearbox.x == 2)
			{
				this.gearingProfil = this.Gearbox.TransmissionGearingbroken2;
			}
			if (this.Gearbox.Condition <= 0.1f && this.Gearbox.x == 3)
			{
				this.gearingProfil = this.Gearbox.TransmissionGearingbroken3;
			}
			this.exp.powertrain.transmission.gearingProfil = this.gearingProfil;
			this.exp.powertrain.transmission.ReconstructGearList();
			if (this.Gearbox.Manual)
			{
				this.exp.powertrain.transmission.TransmissionType = TransmissionComponent.Type.Manual;
			}
			else
			{
				this.exp.powertrain.transmission.TransmissionType = TransmissionComponent.Type.Automatic;
				this.exp.powertrain.transmission.UpshiftRPM = 2000f;
			}
			this.ChangingGear(this.CurrentGear, this.exp.powertrain.transmission.Gear);
		}
		else
		{
			this.gearingProfil = base.GetComponent<GearProfile>();
			this.exp.powertrain.transmission.gearingProfil = this.gearingProfil;
			this.exp.powertrain.transmission.ReconstructGearList();
		}
		if (this.Clutch != null && this.ClutchCover != null && (this.ClutchPedal != null || (this.Gearbox && !this.Gearbox.Manual)))
		{
			if (this.Clutch.Condition > 0.1f)
			{
				this.exp.powertrain.clutch.slipTorque = this.Clutch.ClutchSlipTorque;
			}
			if (this.Clutch.Condition <= 0.1f)
			{
				this.exp.powertrain.clutch.slipTorque = 30f;
			}
		}
		else
		{
			this.exp.powertrain.clutch.slipTorque = 0f;
		}
		List<CarProperties> list = new List<CarProperties>();
		List<CarProperties> list2 = new List<CarProperties>();
		List<CarProperties> list3 = new List<CarProperties>();
		List<CarProperties> list4 = new List<CarProperties>();
		List<CarProperties> list5 = new List<CarProperties>();
		List<CarProperties> list6 = new List<CarProperties>();
		List<CarProperties> list7 = new List<CarProperties>();
		List<CarLight> list8 = new List<CarLight>();
		List<CarLight> list9 = new List<CarLight>();
		List<CarLight> list10 = new List<CarLight>();
		List<CarLight> list11 = new List<CarLight>();
		List<CarLight> list12 = new List<CarLight>();
		List<CarLight> list13 = new List<CarLight>();
		List<CarLight> list14 = new List<CarLight>();
		if (this.EngineBlock)
		{
			this.EngineType = this.EngineBlock.Type;
		}
		this.pistons = 0f;
		this.sparkplugs = 0f;
		this.injectors = 0f;
		foreach (CarProperties carProperties2 in componentsInChildren)
		{
			if (carProperties2.GlowPlug)
			{
				list.Add(carProperties2);
			}
			if (carProperties2.Injector)
			{
				list2.Add(carProperties2);
			}
			if (carProperties2.SparkPlug)
			{
				list3.Add(carProperties2);
			}
			if (carProperties2.Piston)
			{
				list4.Add(carProperties2);
			}
			if (carProperties2.WearDriving)
			{
				list5.Add(carProperties2);
			}
			if (carProperties2.WearWorking)
			{
				list6.Add(carProperties2);
			}
			if (carProperties2.WearByTime)
			{
				list7.Add(carProperties2);
			}
			if (carProperties2.RemovedDifferentMesh)
			{
				carProperties2.SetMesh();
			}
			if (carProperties2.Piston && carProperties2.Condition > 0.1f)
			{
				this.pistons += 1f;
			}
			if (carProperties2.SparkPlug && carProperties2.Condition > 0.1f)
			{
				this.sparkplugs += 1f;
			}
			if (carProperties2.Injector && carProperties2.Condition > 0.1f)
			{
				this.injectors += 1f;
			}
		}
		foreach (CarLight carLight in base.GetComponentsInChildren<CarLight>())
		{
			if (carLight.HeadLightLow)
			{
				list8.Add(carLight);
			}
			if (carLight.HeadLightHigh)
			{
				list9.Add(carLight);
			}
			if (carLight.RunningLight)
			{
				list10.Add(carLight);
			}
			if (carLight.BrakeLight)
			{
				list11.Add(carLight);
			}
			if (carLight.ReverseLight)
			{
				list12.Add(carLight);
			}
			if (carLight.RightLight)
			{
				list13.Add(carLight);
			}
			if (carLight.LeftLight)
			{
				list14.Add(carLight);
			}
		}
		this.GlowPlugs = list.ToArray();
		this.Injectors = list2.ToArray();
		this.SparkPlugs = list3.ToArray();
		this.Pistons = list4.ToArray();
		this.WearDriving = list5.ToArray();
		this.WearWorking = list6.ToArray();
		this.WearByTime = list7.ToArray();
		this.HeadLightLow = list8.ToArray();
		this.HeadLightHigh = list9.ToArray();
		this.RunningLight = list10.ToArray();
		this.BrakeLight = list11.ToArray();
		this.ReverseLight = list12.ToArray();
		this.RightLight = list13.ToArray();
		this.LeftLight = list14.ToArray();
		if (this.RunningLightOn)
		{
			this.RunningLightTurnOn();
		}
		if (this.ReverseLightOn)
		{
			this.ReverseLightTurnOn();
		}
		if (this.HeadLightHighOn)
		{
			this.HeadLightHighTurnOn();
		}
		if (this.BrakeLightOn)
		{
			this.BrakeLightTurnOn();
		}
		this.OilLeak = 0f;
		this.CoolantLeak = 0f;
		this.FuelLeak = 0f;
		if (this.Turbo && this.Turbo.Condition > 0.1f)
		{
			this.exp.powertrain.engine.forcedInduction.useForcedInduction = true;
			this.exp.soundManager.turboFlutterComponent.IsOn = true;
			this.exp.soundManager.turboWhistleComponent.IsOn = true;
		}
		else
		{
			this.exp.powertrain.engine.forcedInduction.useForcedInduction = false;
		}
		if (this.TurboPipe && this.TurboPipe.Condition > 0.1f)
		{
			this.exp.powertrain.engine.forcedInduction.powerGainMultiplier = 1.8f;
		}
		else
		{
			this.exp.powertrain.engine.forcedInduction.powerGainMultiplier = 1f;
		}
		if (!this.HeadGasket)
		{
			this.OilLeak += 0.02f;
			this.CoolantLeak += 0.02f;
		}
		else if (this.HeadGasket.Condition < 0.1f)
		{
			this.OilLeak += 0.005f;
			this.CoolantLeak += 0.02f;
		}
		if (this.DoubleHeads)
		{
			if (!this.HeadGasket2)
			{
				this.OilLeak += 0.02f;
				this.CoolantLeak += 0.02f;
			}
			else if (this.HeadGasket2.Condition < 0.1f)
			{
				this.OilLeak += 0.005f;
				this.CoolantLeak += 0.02f;
			}
		}
		if (!this.OilFilter)
		{
			this.OilLeak += 0.03f;
		}
		if (this.EngineHead)
		{
			if (this.EngineHead.Condition < 0.1f)
			{
				this.OilLeak += 0.0001f;
			}
			this.CoolantLeak += 0.0001f;
		}
		if (this.EngineHead2)
		{
			if (this.EngineHead2.Condition < 0.1f)
			{
				this.OilLeak += 0.0001f;
			}
			this.CoolantLeak += 0.0001f;
		}
		if (!this.FuelLine)
		{
			this.FuelLeak += 0.01f;
		}
		else
		{
			if (this.FuelLine.Condition < 0.1f)
			{
				this.FuelLeak += 0.005f;
			}
			if (this.FuelLine.GetComponent<Partinfo>().fixedImportantBolts < this.FuelLine.GetComponent<Partinfo>().ImportantBolts)
			{
				this.FuelLeak += 0.005f;
			}
		}
		if (!this.WaterHoseLower)
		{
			this.CoolantLeak += 0.1f;
		}
		else
		{
			if (this.WaterHoseLower.Condition < 0.1f)
			{
				this.CoolantLeak += 0.005f;
			}
			if (this.WaterHoseLower.GetComponent<Partinfo>().tightnuts < this.WaterHoseLower.GetComponent<Partinfo>().attachedbolts)
			{
				this.CoolantLeak += 0.005f;
			}
		}
		if (!this.WaterHoseUpper)
		{
			this.CoolantLeak += 0.1f;
		}
		else
		{
			if (this.WaterHoseUpper.Condition < 0.1f)
			{
				this.CoolantLeak += 0.005f;
			}
			if (this.WaterHoseUpper.GetComponent<Partinfo>().tightnuts < this.WaterHoseUpper.GetComponent<Partinfo>().attachedbolts)
			{
				this.CoolantLeak += 0.005f;
			}
		}
		if (!this.ThermostatHousing)
		{
			this.CoolantLeak += 0.1f;
		}
		else if (this.ThermostatHousing.Condition < 0.1f)
		{
			this.CoolantLeak += 0.005f;
		}
		if (this.Radiator)
		{
			if (this.Radiator.Condition < 0.1f)
			{
				this.CoolantLeak += 0.005f;
			}
			if (this.Radiator.MeshDamaged || this.Radiator.MeshLittleDamaged)
			{
				if (this.Radiator.Coolant.VisualFluid)
				{
					UnityEngine.Object.Destroy(this.Radiator.Coolant.VisualFluid.gameObject);
				}
				this.CoolantLeak += 0.1f;
				this.Radiator.GetComponent<Renderer>().sharedMaterial = this.Radiator.RuinedMaterial;
			}
		}
		if (!this.WaterPump)
		{
			this.CoolantLeak += 0.1f;
		}
		if (this.EngineBlock)
		{
			this.EngineMaxPower = this.EngineBlock.Power;
			if (this.EngineBlock.Condition > 0.2f)
			{
				this.EnginePower = this.EngineBlock.Power;
			}
			else
			{
				this.EnginePower = this.EngineBlock.Power / 2f;
			}
		}
		else
		{
			this.EnginePower = 0f;
		}
		if (this.EngineHead)
		{
			this.EngineMaxPower *= this.EngineHead.Power;
			if (this.EngineHead.Condition > 0.2f)
			{
				this.EnginePower *= this.EngineHead.Power;
			}
			else
			{
				this.EnginePower = this.EngineHead.Power / 2f;
			}
		}
		else
		{
			this.EnginePower = 0f;
		}
		if (this.DoubleHeads)
		{
			if (this.EngineHead2)
			{
				this.EngineMaxPower *= this.EngineHead2.Power;
				if (this.EngineHead2.Condition > 0.2f)
				{
					this.EnginePower *= this.EngineHead2.Power;
				}
				else
				{
					this.EnginePower = this.EngineHead2.Power / 2f;
				}
			}
			else
			{
				this.EnginePower = 0f;
			}
		}
		if (this.Rockers)
		{
			this.EngineMaxPower *= this.Rockers.Power;
			if (this.Rockers.Condition > 0.2f)
			{
				this.EnginePower *= this.Rockers.Power;
			}
			else
			{
				this.EnginePower = this.Rockers.Power / 2f;
			}
		}
		else
		{
			this.EnginePower = 0f;
		}
		if (this.DoubleHeads)
		{
			if (this.Rockers2)
			{
				this.EngineMaxPower *= this.Rockers2.Power;
				if (this.Rockers2.Condition > 0.2f)
				{
					this.EnginePower *= this.Rockers2.Power;
				}
				else
				{
					this.EnginePower = this.Rockers2.Power / 2f;
				}
			}
			else
			{
				this.EnginePower = 0f;
			}
		}
		if (this.Crankshaft)
		{
			this.EngineMaxPower *= this.Crankshaft.Power;
			if (this.Crankshaft.Condition > 0.13f)
			{
				this.EnginePower *= this.Crankshaft.Power;
			}
			else
			{
				this.EnginePower *= this.Crankshaft.Power / 2f;
			}
		}
		else
		{
			this.EnginePower = 0f;
		}
		if (this.Camshaft)
		{
			this.EngineMaxPower *= this.Camshaft.Power;
			if (this.Camshaft.Condition > 0.3f)
			{
				this.EnginePower *= this.Camshaft.Power;
			}
			else
			{
				this.EnginePower *= this.Camshaft.Power / 2f;
			}
		}
		else
		{
			this.EnginePower = 0f;
		}
		if (this.Camshaft2 && this.TwinCam)
		{
			this.EngineMaxPower *= this.Camshaft2.Power;
			if (this.Camshaft2.Condition > 0.3f)
			{
				this.EnginePower *= this.Camshaft2.Power;
			}
			else
			{
				this.EnginePower *= this.Camshaft2.Power / 2f;
			}
		}
		else if (this.TwinCam)
		{
			this.EnginePower = 0f;
		}
		if (!this.HeadGasket)
		{
			this.EnginePower *= 0.8f;
		}
		if (this.DoubleHeads && !this.HeadGasket2)
		{
			this.EnginePower *= 0.8f;
		}
		if (this.FuelPump)
		{
			this.EngineMaxPower *= this.FuelPump.Power;
			if (this.FuelPump.Condition > 0.1f)
			{
				this.EnginePower *= this.FuelPump.Power;
			}
			else
			{
				this.EnginePower = 0f;
			}
		}
		else
		{
			this.EnginePower = 0f;
		}
		if (!this.DieselEngine)
		{
			if (this.Carburetor)
			{
				this.EngineMaxPower *= this.Carburetor.Power;
				if (this.Carburetor.Condition > 0.3f)
				{
					this.EnginePower *= this.Carburetor.Power;
				}
				else
				{
					this.EnginePower *= this.Carburetor.Power * 0.8f;
				}
			}
			else
			{
				this.EnginePower = 0f;
			}
		}
		if (this.Blower && this.BlowerBelt && this.BlowerPulley)
		{
			this.EngineMaxPower *= this.Blower.Power;
			if (this.Blower.Condition > 0.3f)
			{
				this.EnginePower *= this.Blower.Power;
			}
			else
			{
				this.EnginePower *= this.Blower.Power * 0.8f;
			}
		}
		else
		{
			this.EnginePower = this.EnginePower;
		}
		if (this.AirFilter && this.AirFilterCover)
		{
			if (this.AirFilter.Condition > 0.3f)
			{
				this.EnginePower *= 1f;
			}
			else
			{
				this.EnginePower *= 0.8f;
			}
		}
		else
		{
			this.EnginePower *= 1.05f;
		}
		if (this.EngineTemp > 120f)
		{
			if (this.EngineTemp > 130f)
			{
				this.EnginePower *= 0.4f;
			}
			else
			{
				this.EnginePower *= 0.7f;
			}
		}
		if (this.Turbo && this.Turbo.Condition > 0.3f && this.TurboPipe && this.TurboPipe.Condition > 0.3f)
		{
			this.EnginePower *= this.Turbo.Power;
		}
		this.InjectorPower = 0f;
		this.SparkPower = 0f;
		this.PistonPower = 0f;
		for (int j = 0; j < this.Pistons.Length; j++)
		{
			CarProperties carProperties3 = this.Pistons[j];
			if (carProperties3)
			{
				this.EngineMaxPower *= carProperties3.Power;
				if (carProperties3.Condition > 0.3f)
				{
					this.PistonPower += carProperties3.Power;
				}
				else
				{
					this.PistonPower += carProperties3.Power * 0.7f;
				}
			}
		}
		if (this.DieselEngine)
		{
			for (int k = 0; k < this.Injectors.Length; k++)
			{
				CarProperties carProperties3 = this.Injectors[k];
				if (carProperties3)
				{
					this.EngineMaxPower *= carProperties3.Power;
					if (carProperties3.Condition > 0.3f)
					{
						this.InjectorPower += carProperties3.Power;
					}
					else
					{
						this.InjectorPower += carProperties3.Power * 0.7f;
					}
				}
			}
		}
		else
		{
			for (int l = 0; l < this.SparkPlugs.Length; l++)
			{
				CarProperties carProperties3 = this.SparkPlugs[l];
				if (carProperties3)
				{
					this.EngineMaxPower *= carProperties3.Power;
					if (carProperties3.Condition > 0.3f)
					{
						this.SparkPower += carProperties3.Power;
					}
					else
					{
						this.SparkPower += carProperties3.Power * 0.7f;
					}
				}
			}
			if (this.Injected)
			{
				for (int m = 0; m < this.Injectors.Length; m++)
				{
					CarProperties carProperties3 = this.Injectors[m];
					if (carProperties3)
					{
						this.EngineMaxPower *= carProperties3.Power;
						if (carProperties3.Condition > 0.3f)
						{
							this.InjectorPower += carProperties3.Power;
						}
						else
						{
							this.InjectorPower += carProperties3.Power * 0.7f;
						}
					}
				}
			}
		}
		if (this.EngineBlock)
		{
			this.EnginePower *= (this.SparkPower + this.PistonPower + this.InjectorPower) / (this.EngineBlock.EngineCylinderCount * 2f);
		}
		this.exp.powertrain.engine.maxPower = this.EnginePower;
		this.ExhaustSmokes.transform.GetComponent<SMOKE>().UsingColor = this.ExhaustSmokes.transform.GetComponent<SMOKE>().normalColor;
		this.ExhaustSmokes.transform.GetComponent<SMOKE>().maxSizeMultiplier = 1.3f;
		if (this.EngineBlock)
		{
			if (this.EngineBlock.Condition < 0.2f)
			{
				this.ExhaustSmokes.transform.GetComponent<SMOKE>().UsingColor = this.ExhaustSmokes.transform.GetComponent<SMOKE>().BlueColor;
			}
			this.ExhaustSmokes.transform.GetComponent<SMOKE>().maxSizeMultiplier = 7f;
		}
		if (this.AirFilter && this.AirFilterCover)
		{
			if (this.AirFilter.Condition < 0.3f)
			{
				this.ExhaustSmokes.transform.GetComponent<SMOKE>().UsingColor = this.ExhaustSmokes.transform.GetComponent<SMOKE>().BlackColor;
			}
			this.ExhaustSmokes.transform.GetComponent<SMOKE>().maxSizeMultiplier = 5f;
		}
		if (!this.HeadGasket)
		{
			this.ExhaustSmokes.transform.GetComponent<SMOKE>().UsingColor = this.ExhaustSmokes.transform.GetComponent<SMOKE>().BlueColor;
			this.ExhaustSmokes.transform.GetComponent<SMOKE>().maxSizeMultiplier = 20f;
		}
		if (this.HeadGasket && this.HeadGasket.Condition < 0.1f)
		{
			this.ExhaustSmokes.transform.GetComponent<SMOKE>().UsingColor = this.ExhaustSmokes.transform.GetComponent<SMOKE>().BlueColor;
			this.ExhaustSmokes.transform.GetComponent<SMOKE>().maxSizeMultiplier = 20f;
		}
		if (this.DoubleHeads)
		{
			this.ExhaustSmokes2.transform.GetComponent<SMOKE>().UsingColor = this.ExhaustSmokes2.transform.GetComponent<SMOKE>().normalColor;
			this.ExhaustSmokes2.transform.GetComponent<SMOKE>().maxSizeMultiplier = 1.3f;
			if (this.EngineBlock)
			{
				if (this.EngineBlock.Condition < 0.2f)
				{
					this.ExhaustSmokes2.transform.GetComponent<SMOKE>().UsingColor = this.ExhaustSmokes2.transform.GetComponent<SMOKE>().BlueColor;
				}
				this.ExhaustSmokes2.transform.GetComponent<SMOKE>().maxSizeMultiplier = 7f;
			}
			if (this.AirFilter && this.AirFilterCover)
			{
				if (this.AirFilter.Condition < 0.3f)
				{
					this.ExhaustSmokes2.transform.GetComponent<SMOKE>().UsingColor = this.ExhaustSmokes2.transform.GetComponent<SMOKE>().BlackColor;
				}
				this.ExhaustSmokes2.transform.GetComponent<SMOKE>().maxSizeMultiplier = 5f;
			}
			if (!this.HeadGasket2)
			{
				this.ExhaustSmokes2.transform.GetComponent<SMOKE>().UsingColor = this.ExhaustSmokes2.transform.GetComponent<SMOKE>().BlueColor;
				this.ExhaustSmokes2.transform.GetComponent<SMOKE>().maxSizeMultiplier = 20f;
			}
			if (this.HeadGasket2 && this.HeadGasket2.Condition < 0.1f)
			{
				this.ExhaustSmokes2.transform.GetComponent<SMOKE>().UsingColor = this.ExhaustSmokes2.transform.GetComponent<SMOKE>().BlueColor;
				this.ExhaustSmokes2.transform.GetComponent<SMOKE>().maxSizeMultiplier = 20f;
			}
		}
		if (this.EngineHead && !this.DoubleHeads)
		{
			if (this.exp && this.EngineBlock)
			{
				this.exp.soundManager.engineRunningComponent.Clip = this.EngineBlock.EngineRunning;
			}
			if (this.Exhaust && this.Exhaust.Condition > 0.4f && this.ExhaustHeader && this.ExhaustHeader.Condition > 0.4f && this.ExhaustManifold && this.ExhaustSmoke)
			{
				this.ExhaustSmokes.transform.position = this.ExhaustSmoke.position;
				this.ExhaustSmokes.transform.rotation = this.ExhaustSmoke.rotation;
				this.exp.soundManager.engineRunningComponent.maxDistortion = 0.4f;
				this.exp.soundManager.engineRunningComponent.baseVolume = 0.2f;
			}
			else
			{
				if (this.ExhaustManifold && this.ExhaustManifoldSmoke)
				{
					this.ExhaustSmokes.transform.position = this.ExhaustManifoldSmoke.position;
					this.ExhaustSmokes.transform.rotation = this.ExhaustManifoldSmoke.rotation;
				}
				if (this.ExhaustHeader && this.ExhaustHeader.Condition > 0.4f && this.ExhaustManifold && this.ExhaustHeaderSmoke)
				{
					this.ExhaustSmokes.transform.position = this.ExhaustHeaderSmoke.position;
					this.ExhaustSmokes.transform.rotation = this.ExhaustHeaderSmoke.rotation;
				}
				if (!this.Exhaust && !this.ExhaustHeader && !this.ExhaustManifold && this.HeadSmoke)
				{
					this.ExhaustSmokes.transform.position = this.HeadSmoke.position;
					this.ExhaustSmokes.transform.rotation = this.HeadSmoke.rotation;
				}
				this.exp.soundManager.engineRunningComponent.maxDistortion = 0.9f;
				this.exp.soundManager.engineRunningComponent.baseVolume = 1f;
				if (!this.Exhaust)
				{
					this.exp.soundManager.engineRunningComponent.maxDistortion = 0.9f;
					this.exp.soundManager.engineRunningComponent.baseVolume = 1f;
				}
				else
				{
					this.exp.soundManager.engineRunningComponent.maxDistortion = 0.6f;
					this.exp.soundManager.engineRunningComponent.baseVolume = 0.5f;
				}
			}
		}
		if (!this.EngineHead || !this.EngineHead2 || !this.DoubleHeads)
		{
			this.ExhaustSmokes2.transform.position = this.ExhaustSmokes.transform.position;
			this.ExhaustSmokes2.Stop();
			return;
		}
		if (this.exp && this.EngineBlock)
		{
			this.exp.soundManager.engineRunningComponent.Clip = this.EngineBlock.EngineRunning;
		}
		if (this.Exhaust && this.Exhaust.Condition > 0.4f && this.ExhaustHeader && this.ExhaustHeader.Condition > 0.4f && this.ExhaustManifold && this.ExhaustSmoke && this.Exhaust2 && this.Exhaust2.Condition > 0.4f && this.ExhaustHeader2 && this.ExhaustHeader2.Condition > 0.4f && this.ExhaustManifold2 && this.ExhaustSmoke2)
		{
			this.ExhaustSmokes.transform.position = this.ExhaustSmoke.position;
			this.ExhaustSmokes.transform.rotation = this.ExhaustSmoke.rotation;
			this.ExhaustSmokes2.transform.position = this.ExhaustSmoke2.position;
			this.ExhaustSmokes2.transform.rotation = this.ExhaustSmoke2.rotation;
			this.exp.soundManager.engineRunningComponent.maxDistortion = 0.4f;
			this.exp.soundManager.engineRunningComponent.baseVolume = 0.2f;
			return;
		}
		if (this.Exhaust && this.Exhaust.Condition > 0.4f && this.ExhaustHeader && this.ExhaustHeader.Condition > 0.4f && this.ExhaustManifold && this.ExhaustSmoke)
		{
			this.ExhaustSmokes.transform.position = this.ExhaustSmoke.position;
			this.ExhaustSmokes.transform.rotation = this.ExhaustSmoke.rotation;
		}
		if (this.Exhaust2 && this.Exhaust2.Condition > 0.4f && this.ExhaustHeader2 && this.ExhaustHeader2.Condition > 0.4f && this.ExhaustManifold2 && this.ExhaustSmoke2)
		{
			this.ExhaustSmokes2.transform.position = this.ExhaustSmoke2.position;
			this.ExhaustSmokes2.transform.rotation = this.ExhaustSmoke2.rotation;
		}
		if (this.ExhaustManifold && this.ExhaustManifoldSmoke)
		{
			this.ExhaustSmokes.transform.position = this.ExhaustManifoldSmoke.position;
			this.ExhaustSmokes.transform.rotation = this.ExhaustManifoldSmoke.rotation;
		}
		if (this.ExhaustManifold2 && this.ExhaustManifoldSmoke2)
		{
			this.ExhaustSmokes2.transform.position = this.ExhaustManifoldSmoke2.position;
			this.ExhaustSmokes2.transform.rotation = this.ExhaustManifoldSmoke2.rotation;
		}
		if (this.ExhaustHeader && this.ExhaustHeader.Condition > 0.4f && this.ExhaustManifold && this.ExhaustHeaderSmoke)
		{
			this.ExhaustSmokes.transform.position = this.ExhaustHeaderSmoke.position;
			this.ExhaustSmokes.transform.rotation = this.ExhaustHeaderSmoke.rotation;
		}
		if (this.ExhaustHeader2 && this.ExhaustHeader2.Condition > 0.4f && this.ExhaustManifold2 && this.ExhaustHeaderSmoke2)
		{
			this.ExhaustSmokes2.transform.position = this.ExhaustHeaderSmoke2.position;
			this.ExhaustSmokes2.transform.rotation = this.ExhaustHeaderSmoke2.rotation;
		}
		if (!this.Exhaust && !this.ExhaustHeader && !this.ExhaustManifold && this.HeadSmoke)
		{
			this.ExhaustSmokes.transform.position = this.HeadSmoke.position;
			this.ExhaustSmokes.transform.rotation = this.HeadSmoke.rotation;
		}
		if (!this.Exhaust2 && !this.ExhaustHeader2 && !this.ExhaustManifold2 && this.HeadSmoke2)
		{
			this.ExhaustSmokes2.transform.position = this.HeadSmoke2.position;
			this.ExhaustSmokes2.transform.rotation = this.HeadSmoke2.rotation;
		}
		this.exp.soundManager.engineRunningComponent.maxDistortion = 0.9f;
		this.exp.soundManager.engineRunningComponent.baseVolume = 1f;
		if (!this.Exhaust || !this.Exhaust2)
		{
			this.exp.soundManager.engineRunningComponent.maxDistortion = 0.9f;
			this.exp.soundManager.engineRunningComponent.baseVolume = 1f;
			return;
		}
		this.exp.soundManager.engineRunningComponent.maxDistortion = 0.6f;
		this.exp.soundManager.engineRunningComponent.baseVolume = 0.5f;
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x00049F0C File Offset: 0x0004810C
	public void SetHandbrake()
	{
		if (this.HandbrakeCable && this.Handbrake)
		{
			this.HandBraking = true;
			this.exp.brakes.HandbrakeDeployed = true;
			if (this.HandbrakeObject)
			{
				this.HandbrakeObject.GetComponent<MeshCollider>().isTrigger = false;
			}
			if (this.HandbrakeObject)
			{
				this.HandbrakeObject.GetComponent<Rigidbody>().mass = 300f;
			}
			this.exp.brakes.brakeWhileAsleep = true;
			this.exp.brakes.brakeWhileIdle = true;
		}
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x00049FB0 File Offset: 0x000481B0
	public void ReleaseHandbrake()
	{
		this.HandBraking = false;
		this.exp.brakes.HandbrakeDeployed = false;
		if (this.HandbrakeObject)
		{
			this.HandbrakeObject.GetComponent<MeshCollider>().isTrigger = true;
		}
		if (this.HandbrakeObject)
		{
			this.HandbrakeObject.GetComponent<Rigidbody>().mass = 0.1f;
		}
		this.exp.brakes.brakeWhileAsleep = false;
		this.exp.brakes.brakeWhileIdle = false;
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0004A038 File Offset: 0x00048238
	public void MPstart()
	{
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.Paintable && carProperties.gameObject.GetComponent<P3dPaintableTexture>())
			{
				carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = this.Color;
			}
		}
		base.StartCoroutine(this.FinishedInitializing());
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x0004A09C File Offset: 0x0004829C
	public void Creating()
	{
		if (this.MPSeed != 0)
		{
			UnityEngine.Random.InitState(this.MPSeed);
		}
		this.Color = UnityEngine.Random.ColorHSV();
		this.OriginalColor = this.Color;
		this.Mileage = (float)UnityEngine.Random.Range(1, 900000);
		if (this.Mileage < 100000f)
		{
			this.Mileage *= 10f;
		}
		this.OriginalInterior = UnityEngine.Random.Range(1, 9);
		this.SetInteriorName();
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.Cluster && !this.Bike)
			{
				carProperties.ClusterMileage = this.Mileage;
				carProperties.MileageText.text = carProperties.ClusterMileage.ToString("F0");
			}
			carProperties.Condition = UnityEngine.Random.Range(0.095f, 1f);
			if (carProperties.Paintable && carProperties.gameObject.GetComponent<P3dPaintableTexture>())
			{
				carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = this.Color;
			}
			if (carProperties.Washable)
			{
				Material[] materials = carProperties.gameObject.GetComponent<Renderer>().materials;
				materials[1] = this.MossyMaterial;
				carProperties.gameObject.GetComponent<Renderer>().materials = materials;
			}
			if (carProperties.Tire)
			{
				carProperties.PartIsOld = true;
				carProperties.Condition = UnityEngine.Random.Range(0.2f, 0.5f);
				carProperties.TirePressure = UnityEngine.Random.Range(0.1f, 4f);
				if (carProperties.TirePressure > 2f)
				{
					carProperties.TirePressure = 2f;
				}
			}
			if (carProperties.Interior)
			{
				carProperties.OriginalInterior = this.OriginalInterior;
			}
		}
		base.StartCoroutine(this.WaitCreating());
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x0004A258 File Offset: 0x00048458
	public void SetCarOptions()
	{
		char c = this.StartOptions.ToString()[1];
		char c2 = this.StartOptions.ToString()[2];
		char c3 = this.StartOptions.ToString()[3];
		char c4 = this.StartOptions.ToString()[4];
		char c5 = this.StartOptions.ToString()[5];
		char c6 = this.StartOptions.ToString()[6];
		char c7 = this.StartOptions.ToString()[7];
		char c8 = this.StartOptions.ToString()[8];
		char c9 = this.StartOptions.ToString()[9];
		if (this.Opt1 && char.GetNumericValue(c) < 7.0)
		{
			this.Opt1 = false;
		}
		if (this.Opt2 && char.GetNumericValue(c2) < 7.0)
		{
			this.Opt2 = false;
		}
		if (this.Opt3 && char.GetNumericValue(c3) < 7.0)
		{
			this.Opt3 = false;
		}
		if (this.Opt4 && char.GetNumericValue(c4) < 7.0)
		{
			this.Opt4 = false;
		}
		if (this.Opt5 && char.GetNumericValue(c5) < 7.0)
		{
			this.Opt5 = false;
		}
		if (this.Opt6 && char.GetNumericValue(c6) < 7.0)
		{
			this.Opt6 = false;
		}
		if (this.Opt7 && char.GetNumericValue(c7) < 7.0)
		{
			this.Opt7 = false;
		}
		if (this.Opt8 && char.GetNumericValue(c8) < 7.0)
		{
			this.Opt8 = false;
		}
		if (this.Opt9 && char.GetNumericValue(c9) < 7.0)
		{
			this.Opt9 = false;
		}
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (this.Opt1 && carProperties.StartOption1)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(carProperties.StartOption1.transform.gameObject, carProperties.transform.position, carProperties.transform.rotation);
				gameObject.transform.SetParent(carProperties.transform.parent);
				gameObject.transform.name = carProperties.transform.name;
				UnityEngine.Object.Destroy(carProperties.transform.gameObject);
			}
			else if (this.Opt2 && carProperties.StartOption2)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(carProperties.StartOption2.transform.gameObject, carProperties.transform.position, carProperties.transform.rotation);
				gameObject2.transform.SetParent(carProperties.transform.parent);
				gameObject2.transform.name = carProperties.transform.name;
				UnityEngine.Object.Destroy(carProperties.transform.gameObject);
			}
			else if (this.Opt3 && carProperties.StartOption3)
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(carProperties.StartOption3.transform.gameObject, carProperties.transform.position, carProperties.transform.rotation);
				gameObject3.transform.SetParent(carProperties.transform.parent);
				gameObject3.transform.name = carProperties.transform.name;
				UnityEngine.Object.Destroy(carProperties.transform.gameObject);
			}
			else if (this.Opt4 && carProperties.StartOption4)
			{
				GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(carProperties.StartOption4.transform.gameObject, carProperties.transform.position, carProperties.transform.rotation);
				gameObject4.transform.SetParent(carProperties.transform.parent);
				gameObject4.transform.name = carProperties.transform.name;
				UnityEngine.Object.Destroy(carProperties.transform.gameObject);
			}
			else if (this.Opt5 && carProperties.StartOption5)
			{
				GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(carProperties.StartOption5.transform.gameObject, carProperties.transform.position, carProperties.transform.rotation);
				gameObject5.transform.SetParent(carProperties.transform.parent);
				gameObject5.transform.name = carProperties.transform.name;
				UnityEngine.Object.Destroy(carProperties.transform.gameObject);
			}
			else if (this.Opt6 && carProperties.StartOption6)
			{
				GameObject gameObject6 = UnityEngine.Object.Instantiate<GameObject>(carProperties.StartOption6.transform.gameObject, carProperties.transform.position, carProperties.transform.rotation);
				gameObject6.transform.SetParent(carProperties.transform.parent);
				gameObject6.transform.name = carProperties.transform.name;
				UnityEngine.Object.Destroy(carProperties.transform.gameObject);
			}
			else if (this.Opt7 && carProperties.StartOption7)
			{
				GameObject gameObject7 = UnityEngine.Object.Instantiate<GameObject>(carProperties.StartOption7.transform.gameObject, carProperties.transform.position, carProperties.transform.rotation);
				gameObject7.transform.SetParent(carProperties.transform.parent);
				gameObject7.transform.name = carProperties.transform.name;
				UnityEngine.Object.Destroy(carProperties.transform.gameObject);
			}
			else if (this.Opt8 && carProperties.StartOption8)
			{
				GameObject gameObject8 = UnityEngine.Object.Instantiate<GameObject>(carProperties.StartOption8.transform.gameObject, carProperties.transform.position, carProperties.transform.rotation);
				gameObject8.transform.SetParent(carProperties.transform.parent);
				gameObject8.transform.name = carProperties.transform.name;
				UnityEngine.Object.Destroy(carProperties.transform.gameObject);
			}
			else if (this.Opt9 && carProperties.StartOption9)
			{
				GameObject gameObject9 = UnityEngine.Object.Instantiate<GameObject>(carProperties.StartOption9.transform.gameObject, carProperties.transform.position, carProperties.transform.rotation);
				gameObject9.transform.SetParent(carProperties.transform.parent);
				gameObject9.transform.name = carProperties.transform.name;
				UnityEngine.Object.Destroy(carProperties.transform.gameObject);
			}
		}
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x0004A914 File Offset: 0x00048B14
	public void CreatingStock(int seed)
	{
		if (seed != 0)
		{
			UnityEngine.Random.InitState(seed);
		}
		this.Mileage = (float)UnityEngine.Random.Range(1, 900000);
		if (this.Mileage < 100000f)
		{
			this.Mileage *= 10f;
		}
		if (this.Color == Color.black)
		{
			this.Color = UnityEngine.Random.ColorHSV();
		}
		this.Owner = "Player";
		this.OriginalColor = this.Color;
		this.OriginalInterior = UnityEngine.Random.Range(1, 9);
		this.SetInteriorName();
		this.NumberParent = GameObject.Find("NumberPlates").GetComponent<NumberPlateManager>();
		this.NumberParent.CreateRandomNumber();
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			carProperties.Owner = "Player";
			if (carProperties.Cluster && !this.Bike)
			{
				carProperties.ClusterMileage = this.Mileage;
				carProperties.MileageText.text = carProperties.ClusterMileage.ToString("F0");
			}
			if (carProperties.Paintable && !carProperties.PaintIsSet && carProperties.gameObject.GetComponent<P3dPaintableTexture>())
			{
				carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = this.Color;
			}
			if (carProperties.Interior)
			{
				carProperties.OriginalInterior = this.OriginalInterior;
			}
			if (carProperties.NumberPlate)
			{
				Material[] materials = carProperties.gameObject.GetComponent<Renderer>().materials;
				materials[2] = this.NumberParent.M1;
				materials[3] = this.NumberParent.M2;
				materials[4] = this.NumberParent.M3;
				materials[5] = this.NumberParent.M4;
				materials[6] = this.NumberParent.M5;
				materials[7] = this.NumberParent.M6;
				carProperties.gameObject.GetComponent<Renderer>().materials = materials;
				carProperties.One = this.NumberParent.M1;
				carProperties.Two = this.NumberParent.M2;
				carProperties.Three = this.NumberParent.M3;
				carProperties.Four = this.NumberParent.M4;
				carProperties.Five = this.NumberParent.M5;
				carProperties.Six = this.NumberParent.M6;
			}
		}
		this.AudioParent = GameObject.Find("hand");
		if (this.AudioParent.transform.parent.GetComponent<GlobalSnow>())
		{
			this.AudioParent.transform.parent.GetComponent<GlobalSnow>().Resett();
		}
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x0004ABA0 File Offset: 0x00048DA0
	public void CreatingReady()
	{
		this.Owner = "Player";
		this.OriginalColor = this.Color;
		this.SetInteriorName();
		CarProperties[] componentsInChildren = base.GetComponentsInChildren<CarProperties>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Owner = "Player";
		}
		this.AudioParent = GameObject.Find("hand");
		if (this.AudioParent.transform.parent.GetComponent<GlobalSnow>())
		{
			this.AudioParent.transform.parent.GetComponent<GlobalSnow>().Resett();
		}
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x0004AC34 File Offset: 0x00048E34
	public void CreatingStockCrached()
	{
		if (this.MPSeed != 0)
		{
			UnityEngine.Random.InitState(this.MPSeed);
		}
		this.Mileage = (float)UnityEngine.Random.Range(1, 900000);
		if (this.Mileage < 100000f)
		{
			this.Mileage *= 10f;
		}
		if (this.Color == Color.black)
		{
			this.Color = UnityEngine.Random.ColorHSV();
		}
		this.OriginalColor = this.Color;
		this.OriginalInterior = UnityEngine.Random.Range(1, 9);
		this.SetInteriorName();
		this.NumberParent = GameObject.Find("NumberPlates").GetComponent<NumberPlateManager>();
		this.NumberParent.CreateRandomNumber();
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.Cluster && !this.Bike)
			{
				carProperties.ClusterMileage = this.Mileage;
				carProperties.MileageText.text = carProperties.ClusterMileage.ToString("F0");
			}
			if (carProperties.Paintable && carProperties.gameObject.GetComponent<P3dPaintableTexture>())
			{
				carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = this.Color;
			}
			if (carProperties.Interior)
			{
				carProperties.OriginalInterior = this.OriginalInterior;
			}
			if (carProperties.NumberPlate)
			{
				Material[] materials = carProperties.gameObject.GetComponent<Renderer>().materials;
				materials[2] = this.NumberParent.M1;
				materials[3] = this.NumberParent.M2;
				materials[4] = this.NumberParent.M3;
				materials[5] = this.NumberParent.M4;
				materials[6] = this.NumberParent.M5;
				materials[7] = this.NumberParent.M6;
				carProperties.gameObject.GetComponent<Renderer>().materials = materials;
				carProperties.One = this.NumberParent.M1;
				carProperties.Two = this.NumberParent.M2;
				carProperties.Three = this.NumberParent.M3;
				carProperties.Four = this.NumberParent.M4;
				carProperties.Five = this.NumberParent.M5;
				carProperties.Six = this.NumberParent.M6;
			}
		}
		base.StartCoroutine(this.WaitCreating());
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x0004AE70 File Offset: 0x00049070
	public void CreatingUsed()
	{
		if (this.MPSeed != 0)
		{
			UnityEngine.Random.InitState(this.MPSeed);
		}
		this.Mileage = (float)UnityEngine.Random.Range(1, 900000);
		if (this.Mileage < 100000f)
		{
			this.Mileage *= 10f;
		}
		this.Owner = "Dealer";
		this.Color = UnityEngine.Random.ColorHSV();
		this.OriginalInterior = UnityEngine.Random.Range(1, 9);
		this.SetInteriorName();
		this.StartOptions = UnityEngine.Random.Range(1000000000, 1999999999);
		this.NumberParent = GameObject.Find("NumberPlates").GetComponent<NumberPlateManager>();
		this.NumberParent.CreateRandomNumber();
		this.SetCarOptions();
		if (this.OriginalInterior >= 2)
		{
			this.OriginalColor = this.Color;
		}
		else
		{
			this.OriginalColor = UnityEngine.Random.ColorHSV();
		}
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.Cluster && !this.Bike)
			{
				carProperties.ClusterMileage = this.Mileage;
				carProperties.MileageText.text = carProperties.ClusterMileage.ToString("F0");
			}
			carProperties.Condition = UnityEngine.Random.Range(0.25f, 1f);
			if (carProperties.Paintable && carProperties.gameObject.GetComponent<P3dPaintableTexture>())
			{
				if (UnityEngine.Random.Range(0, 25) < 3)
				{
					carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = UnityEngine.Random.ColorHSV();
				}
				else
				{
					carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = this.Color;
				}
			}
			if (carProperties.EngineOil != null)
			{
				carProperties.EngineOil.Condition = UnityEngine.Random.Range(0.01f, 0.8f);
			}
			if (carProperties.Tire)
			{
				if (carProperties.Condition < 0.1f)
				{
					carProperties.PartIsOld = true;
				}
				carProperties.TirePressure = UnityEngine.Random.Range(0.9f, 4f);
				if (carProperties.TirePressure > 2f)
				{
					carProperties.TirePressure = 2f;
				}
			}
			if (carProperties.Interior)
			{
				carProperties.OriginalInterior = this.OriginalInterior;
			}
			if (carProperties.NumberPlate)
			{
				Material[] materials = carProperties.gameObject.GetComponent<Renderer>().materials;
				materials[2] = this.NumberParent.M1;
				materials[3] = this.NumberParent.M2;
				materials[4] = this.NumberParent.M3;
				materials[5] = this.NumberParent.M4;
				materials[6] = this.NumberParent.M5;
				materials[7] = this.NumberParent.M6;
				carProperties.gameObject.GetComponent<Renderer>().materials = materials;
				carProperties.One = this.NumberParent.M1;
				carProperties.Two = this.NumberParent.M2;
				carProperties.Three = this.NumberParent.M3;
				carProperties.Four = this.NumberParent.M4;
				carProperties.Five = this.NumberParent.M5;
				carProperties.Six = this.NumberParent.M6;
			}
			if (carProperties.CantRust && !carProperties.MeshLittleDamaged)
			{
				carProperties.Condition = 1f;
			}
		}
		base.StartCoroutine(this.WaitCreating3());
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x0004B19C File Offset: 0x0004939C
	public void CreatingBarnFind()
	{
		if (this.MPSeed != 0)
		{
			UnityEngine.Random.InitState(this.MPSeed);
		}
		this.Mileage = (float)UnityEngine.Random.Range(1, 900000);
		if (this.Mileage < 100000f)
		{
			this.Mileage *= 10f;
		}
		this.Owner = "Player";
		this.Color = UnityEngine.Random.ColorHSV();
		this.OriginalInterior = UnityEngine.Random.Range(1, 9);
		this.SetInteriorName();
		this.StartOptions = UnityEngine.Random.Range(1000000000, 1999999999);
		this.NumberParent = GameObject.Find("NumberPlates").GetComponent<NumberPlateManager>();
		this.NumberParent.CreateRandomNumber();
		this.SetCarOptions();
		if (this.OriginalInterior >= 2)
		{
			this.OriginalColor = this.Color;
		}
		else
		{
			this.OriginalColor = UnityEngine.Random.ColorHSV();
		}
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.Cluster && !this.Bike)
			{
				carProperties.ClusterMileage = this.Mileage;
				carProperties.MileageText.text = carProperties.ClusterMileage.ToString("F0");
			}
			carProperties.Owner = "Player";
			carProperties.Condition = UnityEngine.Random.Range(0.01f, 1f);
			if (carProperties.EngineOil != null)
			{
				carProperties.EngineOil.Condition = UnityEngine.Random.Range(0.01f, 0.8f);
			}
			if (carProperties.Paintable && carProperties.gameObject.GetComponent<P3dPaintableTexture>())
			{
				if (UnityEngine.Random.Range(0, 25) < 3)
				{
					carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = UnityEngine.Random.ColorHSV();
				}
				else
				{
					carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = this.Color;
				}
			}
			if (carProperties.Washable)
			{
				Material[] materials = carProperties.gameObject.GetComponent<Renderer>().materials;
				materials[1] = this.DirtyMaterial;
				carProperties.gameObject.GetComponent<Renderer>().materials = materials;
			}
			if (carProperties.Battery)
			{
				carProperties.BatteryCharge = 0f;
			}
			if (carProperties.Tire)
			{
				if (carProperties.Condition < 0.1f)
				{
					carProperties.PartIsOld = true;
				}
				carProperties.TirePressure = UnityEngine.Random.Range(0.1f, 2f);
			}
			if (carProperties.Interior)
			{
				carProperties.OriginalInterior = this.OriginalInterior;
			}
			if (carProperties.NumberPlate)
			{
				Material[] materials2 = carProperties.gameObject.GetComponent<Renderer>().materials;
				materials2[2] = this.NumberParent.M1;
				materials2[3] = this.NumberParent.M2;
				materials2[4] = this.NumberParent.M3;
				materials2[5] = this.NumberParent.M4;
				materials2[6] = this.NumberParent.M5;
				materials2[7] = this.NumberParent.M6;
				carProperties.gameObject.GetComponent<Renderer>().materials = materials2;
				carProperties.One = this.NumberParent.M1;
				carProperties.Two = this.NumberParent.M2;
				carProperties.Three = this.NumberParent.M3;
				carProperties.Four = this.NumberParent.M4;
				carProperties.Five = this.NumberParent.M5;
				carProperties.Six = this.NumberParent.M6;
			}
			if (carProperties.CantRust && !carProperties.MeshLittleDamaged)
			{
				carProperties.Condition = 1f;
			}
		}
		base.StartCoroutine(this.WaitCreating5());
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x0004B508 File Offset: 0x00049708
	public void CreatingStartOldCar()
	{
		if (this.MPSeed != 0)
		{
			UnityEngine.Random.InitState(this.MPSeed);
		}
		this.Mileage = (float)UnityEngine.Random.Range(1, 900000);
		if (this.Mileage < 100000f)
		{
			this.Mileage *= 10f;
		}
		this.Owner = "Player";
		this.Color = UnityEngine.Random.ColorHSV();
		this.OriginalInterior = UnityEngine.Random.Range(1, 9);
		this.SetInteriorName();
		this.NumberParent = GameObject.Find("NumberPlates").GetComponent<NumberPlateManager>();
		this.NumberParent.CreateRandomNumber();
		if (this.OriginalInterior >= 4)
		{
			this.OriginalColor = this.Color;
		}
		else
		{
			this.OriginalColor = UnityEngine.Random.ColorHSV();
		}
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.Cluster && !this.Bike)
			{
				carProperties.ClusterMileage = this.Mileage;
				carProperties.MileageText.text = carProperties.ClusterMileage.ToString("F0");
			}
			carProperties.Owner = "Player";
			if (carProperties.EngineOil != null)
			{
				carProperties.EngineOil.Condition = UnityEngine.Random.Range(0.01f, 0.8f);
			}
			if (carProperties.PREFAB != null && carProperties.PREFAB.GetComponent<Partinfo>() && carProperties.PREFAB.GetComponent<Partinfo>().Engine)
			{
				carProperties.Condition = UnityEngine.Random.Range(0.15f, 0.6f);
			}
			else if (carProperties.Spring)
			{
				carProperties.Condition = UnityEngine.Random.Range(0.25f, 0.6f);
			}
			else
			{
				carProperties.Condition = UnityEngine.Random.Range(0.01f, 0.8f);
			}
			if (carProperties.Paintable && carProperties.gameObject.GetComponent<P3dPaintableTexture>())
			{
				if (UnityEngine.Random.Range(0, 25) < 3)
				{
					carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = UnityEngine.Random.ColorHSV();
				}
				else
				{
					carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = this.Color;
				}
			}
			if (carProperties.Condition <= 0.4f && carProperties.JunkSpawnChance == 4 && carProperties.OldMaterial)
			{
				carProperties.Condition = 0.15f;
				carProperties.PartIsOld = true;
				carProperties.gameObject.GetComponent<Renderer>().sharedMaterial = carProperties.OldMaterial;
			}
			if (carProperties.Condition <= 0.1f && carProperties.Tintable)
			{
				UnityEngine.Object.Destroy(carProperties.transform.gameObject);
			}
			if (carProperties.Tire)
			{
				if (carProperties.Condition < 0.1f)
				{
					carProperties.PartIsOld = true;
				}
				carProperties.TirePressure = UnityEngine.Random.Range(0.9f, 4f);
				if (carProperties.TirePressure > 2f)
				{
					carProperties.TirePressure = 2f;
				}
			}
			if (carProperties.Interior)
			{
				carProperties.OriginalInterior = this.OriginalInterior;
			}
			if (carProperties.NumberPlate)
			{
				Material[] materials = carProperties.gameObject.GetComponent<Renderer>().materials;
				materials[2] = this.NumberParent.M1;
				materials[3] = this.NumberParent.M2;
				materials[4] = this.NumberParent.M3;
				materials[5] = this.NumberParent.M4;
				materials[6] = this.NumberParent.M5;
				materials[7] = this.NumberParent.M6;
				carProperties.gameObject.GetComponent<Renderer>().materials = materials;
				carProperties.One = this.NumberParent.M1;
				carProperties.Two = this.NumberParent.M2;
				carProperties.Three = this.NumberParent.M3;
				carProperties.Four = this.NumberParent.M4;
				carProperties.Five = this.NumberParent.M5;
				carProperties.Six = this.NumberParent.M6;
			}
			if (carProperties.CantRust && !carProperties.MeshLittleDamaged)
			{
				carProperties.Condition = 1f;
			}
		}
		this.AudioParent = GameObject.Find("hand");
		if (this.AudioParent.transform.parent.GetComponent<GlobalSnow>())
		{
			this.AudioParent.transform.parent.GetComponent<GlobalSnow>().Resett();
		}
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x0004B934 File Offset: 0x00049B34
	public void CreatingJunkyard()
	{
		if (this.Seed != 0)
		{
			UnityEngine.Random.InitState(this.Seed);
			this.Owner = "Player";
		}
		else
		{
			this.Owner = "Junkyard";
			if (this.MPSeed != 0)
			{
				UnityEngine.Random.InitState(this.MPSeed);
			}
		}
		this.Mileage = (float)UnityEngine.Random.Range(1, 900000);
		if (this.Mileage < 100000f)
		{
			this.Mileage *= 10f;
		}
		this.Color = UnityEngine.Random.ColorHSV();
		this.StartOptions = UnityEngine.Random.Range(1000000000, 1999999999);
		this.SetCarOptions();
		this.OriginalInterior = UnityEngine.Random.Range(1, 9);
		this.SetInteriorName();
		this.NumberParent = GameObject.Find("NumberPlates").GetComponent<NumberPlateManager>();
		this.NumberParent.CreateRandomNumber();
		if (this.OriginalInterior >= 2)
		{
			this.OriginalColor = this.Color;
		}
		else
		{
			this.OriginalColor = UnityEngine.Random.ColorHSV();
		}
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.Cluster && !this.Bike)
			{
				carProperties.ClusterMileage = this.Mileage;
				carProperties.MileageText.text = carProperties.ClusterMileage.ToString("F0");
			}
			carProperties.Condition = UnityEngine.Random.Range(0.01f, 1f);
			if (carProperties.EngineOil != null)
			{
				carProperties.EngineOil.Condition = UnityEngine.Random.Range(0.01f, 0.5f);
			}
			if (this.Seed != 0)
			{
				carProperties.Owner = "Player";
			}
			else
			{
				carProperties.Owner = "Junkyard";
			}
			if (carProperties.Paintable && carProperties.gameObject.GetComponent<P3dPaintableTexture>())
			{
				if (UnityEngine.Random.Range(0, 25) < 3)
				{
					carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = UnityEngine.Random.ColorHSV();
				}
				else
				{
					carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = this.Color;
				}
			}
			if (carProperties.Washable)
			{
				Material[] materials = carProperties.gameObject.GetComponent<Renderer>().materials;
				materials[1] = this.DirtyMaterial;
				carProperties.gameObject.GetComponent<Renderer>().materials = materials;
			}
			if (carProperties.Tire)
			{
				if (carProperties.Condition < 0.1f)
				{
					carProperties.PartIsOld = true;
				}
				carProperties.TirePressure = UnityEngine.Random.Range(0.1f, 4f);
				if (carProperties.TirePressure > 2f)
				{
					carProperties.TirePressure = 2f;
				}
			}
			if (carProperties.Interior)
			{
				carProperties.OriginalInterior = this.OriginalInterior;
			}
			if (carProperties.NumberPlate)
			{
				Material[] materials2 = carProperties.gameObject.GetComponent<Renderer>().materials;
				materials2[2] = this.NumberParent.M1;
				materials2[3] = this.NumberParent.M2;
				materials2[4] = this.NumberParent.M3;
				materials2[5] = this.NumberParent.M4;
				materials2[6] = this.NumberParent.M5;
				materials2[7] = this.NumberParent.M6;
				carProperties.gameObject.GetComponent<Renderer>().materials = materials2;
				carProperties.One = this.NumberParent.M1;
				carProperties.Two = this.NumberParent.M2;
				carProperties.Three = this.NumberParent.M3;
				carProperties.Four = this.NumberParent.M4;
				carProperties.Five = this.NumberParent.M5;
				carProperties.Six = this.NumberParent.M6;
			}
		}
		base.StartCoroutine(this.WaitCreating2());
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x0004BCC0 File Offset: 0x00049EC0
	public void CreatingRuinedFind()
	{
		this.Mileage = (float)UnityEngine.Random.Range(1, 900000);
		if (this.Mileage < 100000f)
		{
			this.Mileage *= 10f;
		}
		this.Owner = "None";
		this.Color = UnityEngine.Random.ColorHSV();
		this.OriginalInterior = UnityEngine.Random.Range(1, 9);
		this.SetInteriorName();
		this.NumberParent = GameObject.Find("NumberPlates").GetComponent<NumberPlateManager>();
		this.NumberParent.CreateRandomNumber();
		if (this.OriginalInterior >= 2)
		{
			this.OriginalColor = this.Color;
		}
		else
		{
			this.OriginalColor = UnityEngine.Random.ColorHSV();
		}
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			carProperties.Condition = UnityEngine.Random.Range(0.01f, 0.2f);
			if (carProperties.Cluster && !this.Bike)
			{
				carProperties.ClusterMileage = this.Mileage;
				carProperties.MileageText.text = carProperties.ClusterMileage.ToString("F0");
			}
			if (carProperties.Paintable && carProperties.gameObject.GetComponent<P3dPaintableTexture>())
			{
				if (UnityEngine.Random.Range(0, 25) < 3)
				{
					carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = UnityEngine.Random.ColorHSV();
				}
				else
				{
					carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = this.Color;
				}
			}
			if (carProperties.Washable)
			{
				Material[] materials = carProperties.gameObject.GetComponent<Renderer>().materials;
				materials[1] = this.DirtyMaterial;
				carProperties.gameObject.GetComponent<Renderer>().materials = materials;
			}
			if (carProperties.Tire)
			{
				if (carProperties.Condition < 0.1f)
				{
					carProperties.PartIsOld = true;
				}
				carProperties.TirePressure = UnityEngine.Random.Range(0.1f, 4f);
				if (carProperties.TirePressure > 2f)
				{
					carProperties.TirePressure = 2f;
				}
			}
			if (carProperties.Interior)
			{
				carProperties.OriginalInterior = this.OriginalInterior;
			}
			if (carProperties.NumberPlate)
			{
				Material[] materials2 = carProperties.gameObject.GetComponent<Renderer>().materials;
				materials2[2] = this.NumberParent.M1;
				materials2[3] = this.NumberParent.M2;
				materials2[4] = this.NumberParent.M3;
				materials2[5] = this.NumberParent.M4;
				materials2[6] = this.NumberParent.M5;
				materials2[7] = this.NumberParent.M6;
				carProperties.gameObject.GetComponent<Renderer>().materials = materials2;
				carProperties.One = this.NumberParent.M1;
				carProperties.Two = this.NumberParent.M2;
				carProperties.Three = this.NumberParent.M3;
				carProperties.Four = this.NumberParent.M4;
				carProperties.Five = this.NumberParent.M5;
				carProperties.Six = this.NumberParent.M6;
			}
		}
		base.StartCoroutine(this.WaitCreating4());
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x0004BFB3 File Offset: 0x0004A1B3
	private IEnumerator WaitCreating()
	{
		yield return new WaitForSeconds(3f);
		yield return new WaitForSeconds(3f);
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		base.transform.position = this.SpawnPoint;
		base.transform.rotation = Quaternion.identity;
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		base.GetComponent<Rigidbody>().drag = 0.1f;
		base.GetComponent<Rigidbody>().angularDrag = 0.1f;
		if (this.AudioParent.transform.parent.GetComponent<GlobalSnow>())
		{
			this.AudioParent.transform.parent.GetComponent<GlobalSnow>().Resett();
		}
		if (this.DieselEngine)
		{
			this.Fuel.DieselPercent = 1f;
		}
		this.ResetWheelControllers();
		yield break;
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x0004BFC2 File Offset: 0x0004A1C2
	private IEnumerator WaitCreating2()
	{
		if (this.Seed == 0)
		{
			base.GetComponent<Rigidbody>().drag = 0f;
			base.GetComponent<Rigidbody>().angularDrag = 0f;
			yield return new WaitForSeconds(7f);
			base.GetComponent<Rigidbody>().drag = 10f;
			base.GetComponent<Rigidbody>().angularDrag = 10f;
			yield return null;
			base.GetComponent<Rigidbody>().drag = 10f;
			base.GetComponent<Rigidbody>().angularDrag = 10f;
			yield return null;
		}
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.SinglePart)
			{
				if (carProperties.JunkSpawnChance == 1)
				{
					if (carProperties.gameObject.GetComponent<MPobject>())
					{
						carProperties.gameObject.GetComponent<MPobject>().networkDummy = null;
						UnityEngine.Object.Destroy(carProperties.gameObject.GetComponent<MPobject>());
					}
					if (carProperties.gameObject.GetComponent<Pickup>() && !carProperties.Tire)
					{
						carProperties.gameObject.GetComponent<Pickup>().BRAKE2();
					}
					if (carProperties.gameObject.GetComponent<PickupDoor>())
					{
						carProperties.gameObject.GetComponent<PickupDoor>().BRAKE2();
					}
					UnityEngine.Object.Destroy(carProperties.transform.gameObject);
				}
				if (carProperties.Condition >= 0.6f && carProperties.JunkSpawnChance == 2)
				{
					if (carProperties.gameObject.GetComponent<MPobject>())
					{
						carProperties.gameObject.GetComponent<MPobject>().networkDummy = null;
						UnityEngine.Object.Destroy(carProperties.gameObject.GetComponent<MPobject>());
					}
					if (carProperties.gameObject.GetComponent<Pickup>() && !carProperties.Tire)
					{
						carProperties.gameObject.GetComponent<Pickup>().BRAKE2();
					}
					if (carProperties.gameObject.GetComponent<PickupDoor>())
					{
						carProperties.gameObject.GetComponent<PickupDoor>().BRAKE2();
					}
					UnityEngine.Object.Destroy(carProperties.transform.gameObject);
				}
				if (carProperties.Condition >= 0.8f && carProperties.JunkSpawnChance == 3)
				{
					if (carProperties.gameObject.GetComponent<MPobject>())
					{
						carProperties.gameObject.GetComponent<MPobject>().networkDummy = null;
						UnityEngine.Object.Destroy(carProperties.gameObject.GetComponent<MPobject>());
					}
					if (carProperties.gameObject.GetComponent<Pickup>() && !carProperties.Tire)
					{
						carProperties.gameObject.GetComponent<Pickup>().BRAKE2();
					}
					if (carProperties.gameObject.GetComponent<PickupDoor>())
					{
						carProperties.gameObject.GetComponent<PickupDoor>().BRAKE2();
					}
					UnityEngine.Object.Destroy(carProperties.transform.gameObject);
				}
				if (carProperties.Condition <= 0.4f && carProperties.JunkSpawnChance == 4 && carProperties.OldMaterial)
				{
					carProperties.Condition = 0.15f;
					carProperties.PartIsOld = true;
					carProperties.gameObject.GetComponent<Renderer>().sharedMaterial = carProperties.OldMaterial;
				}
				if (carProperties.Tintable && !carProperties.RuinedMaterial)
				{
					carProperties.Condition = 1f;
				}
				if (carProperties.CantRust && !carProperties.MeshLittleDamaged)
				{
					carProperties.Condition = 1f;
				}
			}
		}
		if (this.Seed == 0)
		{
			base.transform.position = this.SpawnPoint;
			base.transform.rotation = Quaternion.identity;
			base.transform.localRotation = Quaternion.Euler(0f, -50f, 0f);
			base.GetComponent<Rigidbody>().drag = 10f;
			base.GetComponent<Rigidbody>().angularDrag = 10f;
		}
		yield return null;
		base.GetComponent<Rigidbody>().drag = 0.1f;
		base.GetComponent<Rigidbody>().angularDrag = 0.1f;
		if (this.AudioParent.transform.parent.GetComponent<GlobalSnow>())
		{
			this.AudioParent.transform.parent.GetComponent<GlobalSnow>().Resett();
		}
		if (this.DieselEngine && this.Fuel != null)
		{
			this.Fuel.DieselPercent = 1f;
		}
		base.StartCoroutine(this.SetStartPrice());
		this.ResetWheelControllers();
		yield break;
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x0004BFD1 File Offset: 0x0004A1D1
	private IEnumerator WaitCreating3()
	{
		yield return new WaitForSeconds(7f);
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		this.ResetWheelControllers();
		base.transform.position = this.SpawnPoint;
		base.transform.rotation = Quaternion.identity;
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		base.GetComponent<Rigidbody>().drag = 0.1f;
		base.GetComponent<Rigidbody>().angularDrag = 0.1f;
		if (this.AudioParent.transform.parent.GetComponent<GlobalSnow>())
		{
			this.AudioParent.transform.parent.GetComponent<GlobalSnow>().Resett();
		}
		if (this.DieselEngine)
		{
			this.Fuel.DieselPercent = 1f;
		}
		base.StartCoroutine(this.SetStartPrice());
		yield break;
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x0004BFE0 File Offset: 0x0004A1E0
	private IEnumerator WaitCreating4()
	{
		yield return new WaitForSeconds(3f);
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.SinglePart)
			{
				if (carProperties.JunkSpawnChance == 1)
				{
					if (carProperties.gameObject.GetComponent<Pickup>() && !carProperties.Tire)
					{
						carProperties.gameObject.GetComponent<Pickup>().BRAKE2();
					}
					if (carProperties.gameObject.GetComponent<PickupDoor>())
					{
						carProperties.gameObject.GetComponent<PickupDoor>().BRAKE2();
					}
					UnityEngine.Object.Destroy(carProperties.transform.gameObject);
				}
				if (carProperties.JunkSpawnChance == 2)
				{
					if (carProperties.gameObject.GetComponent<Pickup>() && !carProperties.Tire)
					{
						carProperties.gameObject.GetComponent<Pickup>().BRAKE2();
					}
					if (carProperties.gameObject.GetComponent<PickupDoor>())
					{
						carProperties.gameObject.GetComponent<PickupDoor>().BRAKE2();
					}
					UnityEngine.Object.Destroy(carProperties.transform.gameObject);
				}
				if (carProperties.JunkSpawnChance == 3)
				{
					if (carProperties.gameObject.GetComponent<Pickup>() && !carProperties.Tire)
					{
						carProperties.gameObject.GetComponent<Pickup>().BRAKE2();
					}
					if (carProperties.gameObject.GetComponent<PickupDoor>())
					{
						carProperties.gameObject.GetComponent<PickupDoor>().BRAKE2();
					}
					UnityEngine.Object.Destroy(carProperties.transform.gameObject);
				}
				if (carProperties.Condition <= 0.4f && carProperties.JunkSpawnChance == 4 && carProperties.OldMaterial)
				{
					carProperties.Condition = 0.15f;
					carProperties.PartIsOld = true;
					carProperties.gameObject.GetComponent<Renderer>().sharedMaterial = carProperties.OldMaterial;
				}
				if (carProperties.Tintable && !carProperties.RuinedMaterial)
				{
					carProperties.Condition = 1f;
				}
			}
		}
		base.transform.position = this.SpawnPoint;
		base.transform.rotation = Quaternion.identity;
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		base.GetComponent<Rigidbody>().drag = 0.1f;
		base.GetComponent<Rigidbody>().angularDrag = 0.1f;
		if (this.AudioParent.transform.parent.GetComponent<GlobalSnow>())
		{
			this.AudioParent.transform.parent.GetComponent<GlobalSnow>().Resett();
		}
		if (this.DieselEngine && this.Fuel != null)
		{
			this.Fuel.DieselPercent = 1f;
		}
		this.ResetWheelControllers();
		yield break;
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x0004BFEF File Offset: 0x0004A1EF
	private IEnumerator WaitCreating5()
	{
		yield return new WaitForSeconds(3f);
		yield return new WaitForSeconds(3f);
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		base.GetComponent<Rigidbody>().drag = 10f;
		base.GetComponent<Rigidbody>().angularDrag = 10f;
		yield return null;
		this.ResetWheelControllers();
		base.GetComponent<Rigidbody>().drag = 0.1f;
		base.GetComponent<Rigidbody>().angularDrag = 0.1f;
		if (this.AudioParent.transform.parent.GetComponent<GlobalSnow>())
		{
			this.AudioParent.transform.parent.GetComponent<GlobalSnow>().Resett();
		}
		if (this.DieselEngine)
		{
			this.Fuel.DieselPercent = 1f;
		}
		base.StartCoroutine(this.SetStartPrice());
		yield break;
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x0004C000 File Offset: 0x0004A200
	private void SetInteriorName()
	{
		if (this.OriginalInterior == 1)
		{
			this.OriginalInteriorColor = "Black Leather";
		}
		if (this.OriginalInterior == 2)
		{
			this.OriginalInteriorColor = "Red Leather";
		}
		if (this.OriginalInterior == 3)
		{
			this.OriginalInteriorColor = "Brown Leather";
		}
		if (this.OriginalInterior == 4)
		{
			this.OriginalInteriorColor = "Beige Leather";
		}
		if (this.OriginalInterior == 5)
		{
			this.OriginalInteriorColor = "Blue Leather";
		}
		if (this.OriginalInterior == 6)
		{
			this.OriginalInteriorColor = "White Leather";
		}
		if (this.OriginalInterior == 7)
		{
			this.OriginalInteriorColor = "Grey Leather";
		}
		if (this.OriginalInterior == 8)
		{
			this.OriginalInteriorColor = "Green Leather";
		}
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x0004C0AD File Offset: 0x0004A2AD
	private IEnumerator SetStartPrice()
	{
		yield return 0;
		yield return 0;
		yield return 0;
		this.allparts = 0;
		this.existingparts = 0;
		this.CarPriceStart = 0f;
		foreach (transparents transparents in base.GetComponentsInChildren<transparents>())
		{
			if (!transparents.NotImportantPart)
			{
				this.allparts++;
			}
			if (!transparents.NotImportantPart && transparents.HaveAttached)
			{
				this.existingparts++;
			}
		}
		yield return null;
		P3dChangeCounter[] targetLis = base.GetComponentsInChildren<P3dChangeCounter>();
		foreach (P3dChangeCounter p3dChangeCounter in targetLis)
		{
			p3dChangeCounter.enabled = true;
			if (p3dChangeCounter.gameObject.GetComponent<CarProperties>().Paintable && p3dChangeCounter.Threshold == 0.1f)
			{
				p3dChangeCounter.changeDirty = true;
				p3dChangeCounter.Color = this.Color;
			}
		}
		yield return 0;
		yield return 0;
		yield return 0;
		this.CleanRatio = 0.1f;
		this.NoRustRatio = 0.1f;
		this.PaintRatio = 0.1f;
		this.CleanRatioParts = 0f;
		this.NoRustRatioParts = 0f;
		this.PaintRatioParts = 0f;
		this.PaintGoodParts = 0f;
		this.RustGoodParts = 0f;
		foreach (P3dChangeCounter p3dChangeCounter2 in targetLis)
		{
			float num = 1f - p3dChangeCounter2.Ratio;
			if (p3dChangeCounter2.gameObject.GetComponent<CarProperties>().Washable && p3dChangeCounter2.Threshold == 0.7f)
			{
				if ((double)num > 0.6)
				{
					this.CleanRatio += 1f;
				}
				this.CleanRatioParts += 1f;
				p3dChangeCounter2.gameObject.GetComponent<CarProperties>().CleanRatio = num;
			}
			if (p3dChangeCounter2.gameObject.GetComponent<CarProperties>().Paintable && p3dChangeCounter2.Threshold == 0.5f)
			{
				this.NoRustRatio += num;
				this.NoRustRatioParts += 1f;
				p3dChangeCounter2.gameObject.GetComponent<CarProperties>().NoRustRatio = num;
				if (num > 0.95f)
				{
					this.RustGoodParts += 1f;
				}
			}
			if (p3dChangeCounter2.gameObject.GetComponent<CarProperties>().Paintable && p3dChangeCounter2.Threshold == 0.1f)
			{
				this.PaintRatio += num;
				this.PaintRatioParts += 1f;
				p3dChangeCounter2.gameObject.GetComponent<CarProperties>().PaintRatio = num;
				if (num > 0.9f)
				{
					this.PaintGoodParts += 1f;
				}
			}
			p3dChangeCounter2.enabled = false;
		}
		this.DamagedBodyPanels = 0f;
		this.AllBodyPanels = 0f;
		if (this.CleanRatioParts > 0f)
		{
			this.CleanRatio /= this.CleanRatioParts;
		}
		else
		{
			this.CleanRatio = 0.9f;
		}
		if (this.NoRustRatioParts > 0f)
		{
			this.NoRustRatio = this.RustGoodParts / this.NoRustRatioParts;
		}
		else
		{
			this.NoRustRatio = 0.9f;
		}
		if (this.PaintRatioParts > 0f)
		{
			this.PaintRatio = this.PaintGoodParts / this.PaintRatioParts;
		}
		else
		{
			this.PaintRatio = 0.9f;
		}
		this.AverageCondition = 0f;
		foreach (CarProperties carProperties in base.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.SinglePart)
			{
				this.conditCount = 0f;
				if (carProperties.MeshRepairable && (this.Owner == "Client" || carProperties.Owner != "Client"))
				{
					if (carProperties.MeshDamaged)
					{
						this.DamagedBodyPanels += 1f;
					}
					this.AllBodyPanels += 1f;
					if (carProperties.Ruined || carProperties.NoRustRatio < 0.9f)
					{
						this.conditCount += 0f;
					}
					else if (carProperties.MeshDamaged || carProperties.NoRustRatio < 0.95f)
					{
						this.conditCount += 0.1f;
					}
					else
					{
						this.conditCount += 1f;
					}
				}
				if (!carProperties.MeshRepairable && (this.Owner == "Client" || carProperties.Owner != "Client"))
				{
					if (carProperties.Condition < 0.4f && carProperties.Condition >= 0.1f && (carProperties.WornMesh || carProperties.WornMaterial))
					{
						this.conditCount += 0.1f;
					}
					else if (carProperties.Condition <= 0.1f && (carProperties.RuinedMesh || carProperties.RuinedMaterial || carProperties.WornMesh || carProperties.WornMaterial))
					{
						this.conditCount += 0f;
					}
					else if (carProperties.PartIsOld)
					{
						this.conditCount += 0f;
					}
					else
					{
						this.conditCount += 1f;
					}
				}
				carProperties.ConditionDebug = this.conditCount;
				this.AverageCondition += this.conditCount;
			}
		}
		if (this.existingparts >= this.allparts)
		{
			this.AverageCondition /= (float)this.existingparts;
		}
		else
		{
			this.AverageCondition /= this.PartsCount;
		}
		this.CarPriceStart = this.CarPrice * this.AverageCondition * (this.NoRustRatio * this.NoRustRatio);
		this.CarPriceStart = this.CarPriceStart / 2f + this.CarPriceStart / 2f * this.PaintRatio;
		this.CarPriceStart = this.CarPriceStart / 50f * 49f + this.CarPriceStart / 50f * this.CleanRatio;
		if (this.AllBodyPanels > 1f)
		{
			this.CarPriceStart = this.CarPriceStart / 3f + this.CarPriceStart / 3f * 2f * ((this.AllBodyPanels - this.DamagedBodyPanels) / this.AllBodyPanels);
		}
		if (float.IsNaN(this.CarPriceStart))
		{
			this.CarPriceStart = 20f;
		}
		yield break;
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x0004C0BC File Offset: 0x0004A2BC
	private void Update()
	{
		Vector3 normalized = this.exp.vehicleRigidbody.velocity.normalized;
		Vector3 forward = this.exp.transform.forward;
		if (!this.Bike && this.SittingInCar)
		{
			this.Raw_driftAngle = Mathf.Atan2(Vector3.Dot(Vector3.Cross(normalized, forward), this.exp.transform.up), Vector3.Dot(normalized, forward)) * 57.29578f;
			this._driftAngle = Mathf.Abs(this.Raw_driftAngle);
			this.DriftDirection.rotation = Quaternion.LookRotation(((this.WCFL.wheelHit.forwardDir + this.WCFR.wheelHit.forwardDir) / 2f).normalized, base.transform.up);
		}
		if (this.exp.powertrain.engine.IsRunning && this.Blower && this.BlowerBelt && this.BlowerPulley && this.Blower.GetComponent<AudioSource>())
		{
			if (this.exp.powertrain.engine.RPM > 1000f)
			{
				if (!this.Blower.GetComponent<AudioSource>().isPlaying)
				{
					this.Blower.GetComponent<AudioSource>().Play();
				}
				this.Blower.GetComponent<AudioSource>().pitch = Mathf.Clamp((this.exp.powertrain.engine.RPM - 1000f) / 3000f + 1.1f * Time.deltaTime, 0.7f, 1.3f);
				this.Blower.GetComponent<AudioSource>().volume = this.Blower.GetComponent<AudioSource>().pitch / 5f - 0.15f;
			}
			else
			{
				this.Blower.GetComponent<AudioSource>().Stop();
			}
		}
		this.timer += Time.deltaTime;
		if (this.timer > 0.4f)
		{
			if (base.transform.position.y < -3000f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (this.exp.speed > 1f)
			{
				float num = Vector3.Distance(base.transform.position, this.previousLoc) / 1000f;
				this.Mileage += num;
				if (this.Cluster && !this.Bike)
				{
					this.Cluster.ClusterMileage += num;
					this.Cluster.MileageText.text = this.Cluster.ClusterMileage.ToString("F0");
				}
				this.previousLoc = base.transform.position;
			}
			this.timer = 0f;
			foreach (WheelGroup wheelGroup in this.exp.WheelGroups)
			{
				foreach (WheelComponent wheelComponent in wheelGroup.Wheels)
				{
					wheelComponent.wheelController.CheckTires();
					this.checkingSusp = false;
				}
			}
			if (this.exp.powertrain.engine.IsRunning && this.EngineBlock.GetComponent<AudioSource>())
			{
				if (this.exp.powertrain.engine.RPM < 3000f && this.EngineBlock.EngineIdling)
				{
					if (!this.EngineBlock.GetComponent<AudioSource>().isPlaying && !this.EngineBlock.GetComponent<AudioSource>().isPlaying)
					{
						this.EngineBlock.GetComponent<AudioSource>().Play();
					}
				}
				else
				{
					this.EngineBlock.GetComponent<AudioSource>().Stop();
				}
			}
			else if (this.EngineBlock && this.EngineBlock.EngineIdling)
			{
				this.EngineBlock.GetComponent<AudioSource>().Stop();
			}
			if (this.Interval)
			{
				this.Interval = false;
			}
			else
			{
				this.Interval = true;
			}
			if (this.HazardLightOn)
			{
				if (!this.Interval)
				{
					this.LeftLightTurnOn();
					this.RightLightTurnOn();
					base.GetComponent<AudioSource>().PlayOneShot(this.BlinkerOn);
				}
				else
				{
					this.LeftLightTurnOff();
					this.RightLightTurnOff();
					base.GetComponent<AudioSource>().PlayOneShot(this.BlinkerOff);
				}
			}
			else
			{
				if (this.LeftLightOn)
				{
					if (!this.Interval)
					{
						this.LeftLightTurnOn();
						base.GetComponent<AudioSource>().PlayOneShot(this.BlinkerOn);
					}
					else
					{
						this.LeftLightTurnOff();
						base.GetComponent<AudioSource>().PlayOneShot(this.BlinkerOff);
					}
				}
				if (this.RightLightOn)
				{
					if (!this.Interval)
					{
						this.RightLightTurnOn();
						base.GetComponent<AudioSource>().PlayOneShot(this.BlinkerOn);
					}
					else
					{
						this.RightLightTurnOff();
						base.GetComponent<AudioSource>().PlayOneShot(this.BlinkerOff);
					}
				}
			}
			if (this.exp.powertrain.engine.IsRunning && this.EngineHead.transform.position.y < 50f)
			{
				if (this.EngineHead2)
				{
					this.EngineHead2.Condition = 0.01f;
				}
				this.EngineHead.Condition = 0.01f;
				this.EngineBlock.Condition = 0f;
				this.EngineBlock.GetComponent<MeshFilter>().mesh = this.EngineBlock.damagedMesh;
				this.EngineBlock.Damaged = true;
				for (int i = 0; i < this.Pistons.Length; i++)
				{
					CarProperties carProperties = this.Pistons[i];
					carProperties.Condition = 0f;
					carProperties.GetComponent<MeshFilter>().mesh = carProperties.damagedMesh;
				}
				this.EngineStop();
			}
		}
		this.timer2 += Time.deltaTime;
		if (this.timer2 > 1f)
		{
			this.timer2 = 0f;
			if (this.exp.powertrain.engine.IsRunning)
			{
				this.consumptionPerHour = 2f + this.exp.powertrain.engine.RPM / 300f * (this.exp.speed / 10f + 0.1f) * (50f + this.EngineMaxPower / 3f);
				this.consumptionPerHour = Mathf.Clamp(this.consumptionPerHour, 0f, 6000f);
				this.Fuel.FluidSize -= this.consumptionPerHour / 3600f * Time.deltaTime;
				this.Fuel.FluidSize = Mathf.Clamp(this.Fuel.FluidSize, 0f, this.Fuel.ContainerSize);
				if (this.Fuel.FluidSize <= 0f || (this.DieselEngine && this.Fuel.DieselPercent < 0.6f) || (!this.DieselEngine && this.Fuel.DieselPercent > 0.4f))
				{
					this.EngineStop();
				}
				this.revving = false;
				if (this.EngineOil && this.EngineOil.FluidSize > 0f)
				{
					this.EngineOil.FluidSize -= this.OilLeak;
					this.EngineOil.Condition -= 0.0001f;
				}
				if (this.OilFilter)
				{
					this.OilFilter.Condition -= 1.7E-05f;
					if (this.OilFilter.Condition < 0.1f && this.EngineOil)
					{
						this.EngineOil.Condition -= 0.0002f;
					}
				}
				if (this.Coolant && this.Coolant.FluidSize > 0f)
				{
					this.Coolant.FluidSize -= this.CoolantLeak;
				}
				if (this.Fuel && this.Fuel.FluidSize > 0f)
				{
					this.Fuel.FluidSize -= this.FuelLeak;
				}
				if (!this.AirCooled)
				{
					if (this.EngineTemp < 90f)
					{
						this.EngineTemp += 1f;
					}
					if (!this.WaterPump)
					{
						this.EngineTemp += 0.5f;
					}
					else if (this.WaterPump.Condition < 0.1f)
					{
						this.EngineTemp += 0.3f;
					}
					if (!this.WaterPumpBelt)
					{
						this.EngineTemp += 0.5f;
					}
					if (this.Coolant)
					{
						if (this.Coolant.FluidSize < this.Coolant.MinFluidSize)
						{
							this.EngineTemp += 0.3f;
						}
						if (this.Coolant.FluidSize < 0.2f)
						{
							this.EngineTemp += 0.8f;
						}
					}
					else
					{
						this.EngineTemp += 1f;
					}
					if (this.RadiatorFan)
					{
						this.EngineTemp -= 0.3f;
					}
					if (this.RadiatorFanGT)
					{
						this.EngineTemp -= 0.2f;
					}
					if (this.RadiatorGT)
					{
						this.EngineTemp -= 0.2f;
					}
					if (this.exp.powertrain.engine.RPM > 2500f)
					{
						this.EngineTemp += 0.1f;
					}
					if (this.exp.powertrain.engine.RPM > 4500f)
					{
						this.EngineTemp += 0.2f;
					}
					if (this.exp.powertrain.engine.RPM > 5500f)
					{
						this.EngineTemp += 0.3f;
					}
					if (this.exp.speed > 10f && (this.Coolant || this.AirCooled))
					{
						this.EngineTemp -= 0.2f;
					}
					if (this.EngineTemp > 130f)
					{
						if (this.EngineHead2)
						{
							this.EngineHead2.Condition -= 0.0025f;
						}
						this.EngineHead.Condition -= 0.0025f;
						if (this.HeadGasket)
						{
							this.HeadGasket.Condition -= 0.005f;
						}
						if (this.HeadGasket2)
						{
							this.HeadGasket2.Condition -= 0.005f;
						}
						for (int j = 0; j < this.Pistons.Length; j++)
						{
							CarProperties carProperties = this.Pistons[j];
							carProperties.Condition -= 0.0025f;
							if (carProperties.Condition < 0.15f)
							{
								carProperties.Condition = 0f;
								carProperties.GetComponent<MeshFilter>().mesh = carProperties.damagedMesh;
								carProperties.GetComponent<DetachablePart>().DefinetDetach();
								this.EngineBlock.GetComponent<MeshFilter>().mesh = this.EngineBlock.damagedMesh;
								this.EngineBlock.Condition = 0f;
								this.EngineBlock.Damaged = true;
								this.EngineStop();
								base.GetComponent<AudioSource>().PlayOneShot(this.EngineExploaded);
							}
						}
						if (this.EngineTemp > 200f)
						{
							if (this.EngineHead2)
							{
								this.EngineHead2.Condition = 0f;
							}
							this.EngineHead.Condition = 0f;
							this.HeadGasket.Condition = 0f;
							if (this.HeadGasket2)
							{
								this.HeadGasket2.Condition = 0f;
							}
							this.EngineStop();
						}
					}
				}
				if (this.EngineOil)
				{
					if ((double)this.EngineOil.Condition < 0.1 || this.EngineOil.FluidSize < this.EngineOil.MinFluidSize)
					{
						if (this.EngineOil.FluidSize > 0.15f)
						{
							for (int k = 0; k < this.Pistons.Length; k++)
							{
								CarProperties carProperties = this.Pistons[k];
								carProperties.Condition -= 0.0005f;
							}
							if (this.EngineHead2)
							{
								this.EngineHead2.Condition -= 0.0003f;
							}
							this.EngineHead.Condition -= 0.0003f;
							this.Crankshaft.Condition -= 0.0006f;
							this.Camshaft.Condition -= 0.00055f;
							if (this.Camshaft2)
							{
								this.Camshaft2.Condition -= 0.00055f;
							}
							this.EngineBlock.Condition -= 0.0002f;
							this.EngineChain.Condition -= 0.00022f;
						}
						else
						{
							for (int l = 0; l < this.Pistons.Length; l++)
							{
								CarProperties carProperties = this.Pistons[l];
								carProperties.Condition -= 0.005f;
								if (carProperties.Condition < 0.15f)
								{
									carProperties.Condition = 0f;
									carProperties.GetComponent<MeshFilter>().mesh = carProperties.damagedMesh;
									if (carProperties.GetComponent<DetachablePart>())
									{
										carProperties.GetComponent<DetachablePart>().DefinetDetach();
									}
									this.EngineBlock.GetComponent<MeshFilter>().mesh = this.EngineBlock.damagedMesh;
									this.EngineBlock.Condition = 0f;
									this.EngineBlock.Damaged = true;
									this.EngineStop();
									base.GetComponent<AudioSource>().PlayOneShot(this.EngineExploaded);
								}
							}
							if (this.EngineHead2)
							{
								this.EngineHead2.Condition -= 0.003f;
							}
							this.EngineHead.Condition -= 0.003f;
							this.Crankshaft.Condition -= 0.006f;
							this.Camshaft.Condition -= 0.0055f;
							if (this.Camshaft2)
							{
								this.Camshaft2.Condition -= 0.00055f;
							}
							this.EngineBlock.Condition -= 0.002f;
							this.EngineChain.Condition -= 0.0022f;
						}
					}
				}
				else
				{
					for (int m = 0; m < this.Pistons.Length; m++)
					{
						CarProperties carProperties = this.Pistons[m];
						carProperties.Condition -= 0.005f;
						if (carProperties.Condition < 0.18f)
						{
							carProperties.Condition = 0f;
							carProperties.GetComponent<MeshFilter>().mesh = carProperties.damagedMesh;
							carProperties.GetComponent<DetachablePart>().DefinetDetach();
							this.EngineBlock.GetComponent<MeshFilter>().mesh = this.EngineBlock.damagedMesh;
							this.EngineBlock.Condition = 0f;
							this.EngineBlock.Damaged = true;
							this.EngineStop();
							base.GetComponent<AudioSource>().PlayOneShot(this.EngineExploaded);
						}
					}
					if (this.EngineHead2)
					{
						this.EngineHead2.Condition -= 0.003f;
					}
					this.EngineHead.Condition -= 0.003f;
					this.Crankshaft.Condition -= 0.006f;
					this.Camshaft.Condition -= 0.0055f;
					this.EngineBlock.Condition -= 0.002f;
					this.EngineChain.Condition -= 0.0022f;
				}
				if (!this.AirFilter || !this.AirFilterCover)
				{
					for (int n = 0; n < this.Pistons.Length; n++)
					{
						CarProperties carProperties = this.Pistons[n];
						carProperties.Condition -= 0.001f;
					}
					if (this.EngineHead2)
					{
						this.EngineHead2.Condition -= 0.001f;
					}
					this.EngineHead.Condition -= 0.001f;
					this.EngineBlock.Condition -= 0.001f;
					if (this.Carburetor)
					{
						this.Carburetor.Condition -= 0.001f;
					}
				}
				this.Volts = 11.5f;
				if (this.Alternator && this.AlternatorBelt && this.Battery && this.BatteryWires && this.Alternator.Condition > 0.1f)
				{
					if (this.Battery.BatteryCharge < 12.8f)
					{
						this.Battery.BatteryCharge += 0.002f;
						this.Volts = 13.4f;
					}
				}
				else if (!this.BatteryWires)
				{
					this.EngineStop();
				}
				else
				{
					if (this.Cluster && this.Cluster.ClusterGlowPlugs)
					{
						this.Cluster.ClusterGlowPlugs.GetComponent<MeshRenderer>().enabled = false;
					}
					if (this.Cluster && this.Cluster.ClusterBat && this.Battery)
					{
						this.Cluster.ClusterBat.GetComponent<MeshRenderer>().enabled = true;
					}
					if (!this.AlternatorBelt || !this.Alternator || this.Alternator.Condition < 0.1f)
					{
						if (this.Battery && this.Battery.BatteryCharge < 11.8f)
						{
							this.EngineStop();
						}
						if (!this.Battery)
						{
							this.EngineStop();
						}
					}
					if (!this.Battery && this.Alternator && this.Alternator.Condition < 0.1f)
					{
						this.EngineStop();
					}
					if (this.Battery && this.BatteryWires && this.Battery.BatteryCharge > 11.7f)
					{
						this.Battery.BatteryCharge -= 0.002f;
					}
				}
				if (this.Bike && this.AlternatorBelt && this.Alternator && this.Alternator.Condition > 0.1f)
				{
					this.Electricity = true;
				}
				if (this.EngineBlock)
				{
					if (this.EngineBlock.Condition > 0.2f)
					{
						this.EnginePower = this.EngineBlock.Power;
					}
					else
					{
						this.EnginePower = this.EngineBlock.Power / 2f;
					}
				}
				else
				{
					this.EngineStop();
				}
				if (this.EngineHead)
				{
					if (this.EngineHead.Condition > 0.17f)
					{
						this.EnginePower *= this.EngineHead.Power;
					}
					else
					{
						this.EnginePower *= this.EngineHead.Power / 2f;
					}
				}
				else
				{
					this.EngineStop();
				}
				if (this.DoubleHeads)
				{
					if (this.EngineHead2)
					{
						if (this.EngineHead2.Condition > 0.17f)
						{
							this.EnginePower *= this.EngineHead2.Power;
						}
						else
						{
							this.EnginePower *= this.EngineHead2.Power / 2f;
						}
					}
					else
					{
						this.EngineStop();
					}
				}
				if (this.Rockers)
				{
					if (this.Rockers.Condition > 0.17f)
					{
						this.EnginePower *= this.Rockers.Power;
					}
					else
					{
						this.EnginePower *= this.Rockers.Power / 2f;
					}
				}
				else
				{
					this.EngineStop();
				}
				if (this.DoubleHeads)
				{
					if (this.Rockers2)
					{
						if (this.Rockers2.Condition > 0.17f)
						{
							this.EnginePower *= this.Rockers2.Power;
						}
						else
						{
							this.EnginePower *= this.Rockers2.Power / 2f;
						}
					}
					else
					{
						this.EngineStop();
					}
				}
				if (this.Crankshaft)
				{
					if (this.Crankshaft.Condition > 0.13f)
					{
						this.EnginePower *= this.Crankshaft.Power;
					}
					else
					{
						this.EnginePower *= this.Crankshaft.Power / 2f;
					}
				}
				else
				{
					this.EngineStop();
				}
				if (this.Camshaft)
				{
					if (this.Camshaft.Condition > 0.3f)
					{
						this.EnginePower *= this.Camshaft.Power;
					}
					else
					{
						this.EnginePower *= this.Camshaft.Power / 2f;
					}
				}
				else
				{
					this.EngineStop();
				}
				if (this.Camshaft2)
				{
					if (this.Camshaft2.Condition > 0.3f)
					{
						this.EnginePower *= this.Camshaft2.Power;
					}
					else
					{
						this.EnginePower *= this.Camshaft2.Power / 2f;
					}
				}
				else if (this.TwinCam)
				{
					this.EngineStop();
				}
				if (!this.DieselEngine)
				{
					if (this.SparkWires)
					{
						if (this.SparkWires.Condition > 0.2f)
						{
							this.EnginePower *= this.SparkWires.Power;
						}
						else
						{
							this.EnginePower *= this.SparkWires.Power / 2f;
						}
					}
					else
					{
						this.EngineStop();
					}
				}
				if (this.DieselEngine || this.Injected)
				{
					if (this.FuelHoses)
					{
						if (this.FuelHoses.Condition > 0.2f)
						{
							this.EnginePower *= this.FuelHoses.Power;
						}
						else
						{
							this.EnginePower *= this.FuelHoses.Power / 2f;
						}
					}
					else
					{
						this.EngineStop();
					}
				}
				if (!this.DieselEngine)
				{
					if (this.Distributor)
					{
						if (this.Distributor.Condition > 0.2f)
						{
							this.EnginePower *= this.Distributor.Power;
						}
						else
						{
							this.EnginePower *= this.Distributor.Power / 2f;
						}
					}
					else
					{
						this.EngineStop();
					}
				}
				if (!this.HeadGasket)
				{
					this.EnginePower *= 0.8f;
				}
				if (this.DoubleHeads && !this.HeadGasket2)
				{
					this.EnginePower *= 0.8f;
				}
				if (this.FuelPump)
				{
					if (this.FuelPump.Condition > 0.1f)
					{
						this.EnginePower *= this.FuelPump.Power;
					}
					else
					{
						this.EnginePower = 0f;
					}
				}
				else
				{
					this.EngineStop();
				}
				if (!this.DieselEngine)
				{
					if (this.Carburetor)
					{
						if (this.Carburetor.Condition > 0.3f)
						{
							this.EnginePower *= this.Carburetor.Power;
						}
						else
						{
							this.EnginePower *= this.Carburetor.Power * 0.8f;
						}
					}
					else
					{
						this.EngineStop();
					}
				}
				if (this.Blower && this.BlowerBelt && this.BlowerPulley)
				{
					if (this.Blower.Condition > 0.3f)
					{
						this.EnginePower *= this.Blower.Power;
					}
					else
					{
						this.EnginePower *= this.Blower.Power * 0.8f;
					}
				}
				else
				{
					this.EnginePower = this.EnginePower;
				}
				if (this.AirFilter && this.AirFilterCover)
				{
					if (this.AirFilter.Condition > 0.3f)
					{
						this.EnginePower *= 1f;
					}
					else
					{
						this.EnginePower *= 0.8f;
					}
				}
				else
				{
					this.EnginePower *= 1.05f;
				}
				if (this.Turbo && this.Turbo.Condition > 0.3f && this.TurboPipe && this.TurboPipe.Condition > 0.3f)
				{
					this.EnginePower *= this.Turbo.Power;
				}
				if (this.EngineTemp > 120f)
				{
					this.EnginePower *= 1f - (this.EngineTemp - 120f) * 0.04f;
				}
				this.SparkPower = 0f;
				this.PistonPower = 0f;
				this.InjectorPower = 0f;
				for (int num2 = 0; num2 < this.Pistons.Length; num2++)
				{
					CarProperties carProperties2 = this.Pistons[num2];
					if (carProperties2)
					{
						if (carProperties2.Condition > 0.3f)
						{
							this.PistonPower += carProperties2.Power;
						}
						else
						{
							this.PistonPower += carProperties2.Power * 0.7f;
						}
					}
				}
				if (this.DieselEngine)
				{
					for (int num3 = 0; num3 < this.Injectors.Length; num3++)
					{
						CarProperties carProperties = this.Injectors[num3];
						if (carProperties)
						{
							if (carProperties.Condition > 0.3f)
							{
								this.InjectorPower += carProperties.Power;
							}
							else
							{
								this.InjectorPower += carProperties.Power * 0.7f;
							}
						}
					}
				}
				else
				{
					for (int num4 = 0; num4 < this.SparkPlugs.Length; num4++)
					{
						CarProperties carProperties = this.SparkPlugs[num4];
						if (carProperties)
						{
							if (carProperties.Condition > 0.3f)
							{
								this.SparkPower += carProperties.Power;
							}
							else
							{
								this.SparkPower += carProperties.Power * 0.7f;
							}
						}
					}
					if (this.Injected)
					{
						for (int num5 = 0; num5 < this.Injectors.Length; num5++)
						{
							CarProperties carProperties = this.Injectors[num5];
							if (carProperties)
							{
								if (carProperties.Condition > 0.3f)
								{
									this.InjectorPower += carProperties.Power;
								}
								else
								{
									this.InjectorPower += carProperties.Power * 0.7f;
								}
							}
						}
					}
				}
				if (this.EngineBlock)
				{
					this.EnginePower *= (this.SparkPower + this.PistonPower + this.InjectorPower) / (this.EngineBlock.EngineCylinderCount * 2f);
				}
				this.exp.powertrain.engine.maxPower = this.EnginePower;
			}
			else
			{
				if (!this.Battery || !this.BatteryWires || (this.Battery && this.Battery.BatteryCharge < 11.8f))
				{
					this.Electricity = false;
					this.NoElectricity();
				}
				else
				{
					this.Electricity = true;
				}
				if (this.Coolant && this.Coolant.FluidSize > 0f)
				{
					this.Coolant.FluidSize -= this.CoolantLeak / 10f;
				}
				if (this.Fuel && this.Fuel.FluidSize > 0f)
				{
					this.Fuel.FluidSize -= this.FuelLeak / 10f;
				}
				if (this.Battery)
				{
					this.Volts = this.Battery.BatteryCharge;
				}
				if (this.EngineTemp > 20f)
				{
					this.EngineTemp -= 0.3f;
				}
			}
			if (this.EngineTemp > 80f && this.Radiator)
			{
				if (this.EngineTemp > 130f)
				{
					if (this.Coolant)
					{
						this.CoolantSmokes.Play();
					}
					else
					{
						this.CoolantSmokes.Stop();
					}
				}
				if (this.Radiator && this.Coolant)
				{
					if (this.Radiator.MeshDamaged)
					{
						this.CoolantSmokes.Play();
					}
					else
					{
						this.CoolantSmokes.Stop();
					}
				}
			}
			else
			{
				this.CoolantSmokes.Stop();
			}
		}
		this.timer3 += Time.deltaTime;
		if ((double)this.timer3 > 11.1)
		{
			this.timer3 = 0f;
			if (this.DUST != null && this.exp.speed > 4f && EnviroSkyMgr.instance && EnviroSkyMgr.instance.Seasons.currentSeasons != EnviroSeasons.Seasons.Winter && (this.FRwhellcontroller.GetComponent<WheelController>().activeFrictionPreset.name == "GravelTireFrictionPreset" || this.FRwhellcontroller.GetComponent<WheelController>().activeFrictionPreset.name == "SandTireFrictionPreset"))
			{
				this.DUST.HandleHitPoint(false, 0, 1f, 0, base.transform.position, Quaternion.identity);
			}
			if (this.exp.powertrain.engine.IsRunning)
			{
				for (int num6 = 0; num6 < this.WearWorking.Length; num6++)
				{
					CarProperties carProperties = this.WearWorking[num6];
					carProperties.Condition -= 0.0001f * (0.1f + carProperties.WearSpeed);
				}
				if (this.exp.speed > 5f)
				{
					for (int num7 = 0; num7 < this.WearDriving.Length; num7++)
					{
						CarProperties carProperties = this.WearDriving[num7];
						carProperties.Condition -= 0.0001f * (0.1f + carProperties.WearSpeed);
					}
				}
			}
		}
		this.timer4 += Time.deltaTime;
		if (this.timer4 > 100f)
		{
			this.timer4 = 0f;
			if (this.exp.powertrain.engine.IsRunning)
			{
				for (int num8 = 0; num8 < this.WearByTime.Length; num8++)
				{
					CarProperties carProperties = this.WearByTime[num8];
					carProperties.Condition -= 0.0001f * UnityEngine.Random.Range(0.1f, 3f);
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.K) && this.SittingInCar)
		{
			this.WiperSwitch();
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().ButtonClick);
		}
		if (Input.GetKeyDown(KeyCode.L) && this.SittingInCar && this.RunningLightOn)
		{
			if (!this.HeadLightHighOn)
			{
				this.HeadLightHighTurnOn();
			}
			else
			{
				this.HeadLightHighTurnOff();
			}
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().ButtonClick);
		}
		if (this.player.GetButtonDown("LeftBlinker") && this.SittingInCar && !this.HazardLightOn)
		{
			if (!this.LeftLightOn)
			{
				this.LeftLightOn = true;
				this.RightLightOn = false;
				this.LeftLightTurnOn();
				this.RightLightTurnOff();
			}
			else
			{
				this.LeftLightOn = false;
				this.LeftLightTurnOff();
			}
			if (this.RunningLightOn)
			{
				this.RunningLightTurnOn();
			}
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().ButtonClick);
		}
		if (this.player.GetButtonDown("RightBlinker") && this.SittingInCar && !this.HazardLightOn)
		{
			if (!this.RightLightOn)
			{
				this.RightLightOn = true;
				this.LeftLightOn = false;
				this.RightLightTurnOn();
				this.LeftLightTurnOff();
			}
			else
			{
				this.RightLightOn = false;
				this.RightLightTurnOff();
			}
			if (this.RunningLightOn)
			{
				this.RunningLightTurnOn();
			}
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().ButtonClick);
		}
		if (this.exp.input.InputSwappedBrakes > 0.1f && !this.BrakeLightOn && this.BrakePedal)
		{
			this.BrakeLightTurnOn();
		}
		if (this.exp.input.InputSwappedBrakes <= 0.1f && this.BrakeLightOn)
		{
			this.BrakeLightTurnOff();
		}
		if (this.Gearbox && this.exp.powertrain.transmission.Gear < 0 && !this.ReverseLightOn)
		{
			this.ReverseLightTurnOn();
		}
		if (this.Gearbox && this.exp.powertrain.transmission.Gear >= 0 && this.ReverseLightOn)
		{
			this.ReverseLightTurnOff();
		}
		if (this.Gearbox && this.CurrentGear != this.exp.powertrain.transmission.GearName.ToString())
		{
			this.CurrentGear = this.exp.powertrain.transmission.GearName.ToString();
			this.ChangingGear(this.CurrentGear, this.exp.powertrain.transmission.Gear);
		}
		if (this.exp.powertrain.engine.IsRunning)
		{
			if (this.HarmonicBalancer)
			{
				this.HarmonicBalancer.transform.Rotate(Vector3.forward, this.exp.powertrain.engine.RPM * 6f * Time.deltaTime);
			}
			if (this.CrankshaftPulley)
			{
				this.CrankshaftPulley.transform.Rotate(Vector3.forward, this.exp.powertrain.engine.RPM * 6f * Time.deltaTime);
				if (this.HarmonicBalancer)
				{
					this.HarmonicBalancer.transform.Rotate(Vector3.forward, this.exp.powertrain.engine.RPM * 6f * Time.deltaTime);
				}
				if (this.WaterPumpBelt && this.WaterPumpPulley && this.AlternatorPulley)
				{
					this.WaterPumpPulley.transform.Rotate(Vector3.forward, this.exp.powertrain.engine.RPM * 6f * Time.deltaTime);
				}
				if (this.AlternatorBelt && this.AlternatorPulley && this.WaterPumpPulley)
				{
					this.AlternatorPulley.transform.Rotate(Vector3.forward, this.exp.powertrain.engine.RPM * 10f * Time.deltaTime);
				}
				if (this.Camshaft.VisualObject)
				{
					this.Camshaft.VisualObject.transform.Rotate(Vector3.forward, this.exp.powertrain.engine.RPM * 3f * Time.deltaTime);
				}
				if (this.Camshaft2 && this.Camshaft2.VisualObject)
				{
					this.Camshaft2.VisualObject.transform.Rotate(Vector3.forward, this.exp.powertrain.engine.RPM * 3f * Time.deltaTime);
				}
				if (this.Crankshaft.VisualObject)
				{
					this.Crankshaft.VisualObject.transform.Rotate(Vector3.forward, this.exp.powertrain.engine.RPM * 6f * Time.deltaTime);
				}
				this.Flywheel.transform.Rotate(Vector3.forward, this.exp.powertrain.engine.RPM * 6f * Time.deltaTime);
			}
			if (this.DriveShaft && this.Clutch.Condition > 0.1f)
			{
				this.DriveShaft.transform.Rotate(Vector3.forward, this.exp.powertrain.engine.RPM * 6f * this.exp.powertrain.transmission.ratio * this.exp.powertrain.clutch.clutchEngagement * Time.deltaTime);
			}
			if (this.DriveShaftFront && this.Clutch.Condition > 0.1f)
			{
				this.DriveShaftFront.transform.Rotate(Vector3.forward, this.exp.powertrain.engine.RPM * -6f * this.exp.powertrain.transmission.ratio * this.exp.powertrain.clutch.clutchEngagement * Time.deltaTime);
			}
		}
		if (this.Cluster && this.Cluster.Condition >= 0.1f)
		{
			if (this.AnalogSpeedGauge && this.Cluster.Condition >= 0.4f)
			{
				this.AnalogSpeedGauge.GetComponent<AnalogGauge>().Value = this.exp.speed * 3.6f;
			}
			if (this.DigitalSpeedGauge)
			{
				if (this.IgnitionON && this.Electricity)
				{
					this.DigitalSpeedGauge.text = Mathf.Round(this.exp.speed * 3.6f).ToString();
				}
				else
				{
					this.DigitalSpeedGauge.text = "";
				}
			}
			if (this.AnalogRpmGauge && this.Cluster.Condition >= 0.4f)
			{
				this.AnalogRpmGauge.GetComponent<AnalogGauge>().Value = this.exp.powertrain.engine.RPM;
			}
			if (this.TempGauge)
			{
				if (this.IgnitionON && this.Battery && this.BatteryWires)
				{
					this.TempGauge.GetComponent<AnalogGauge>().Value = this.EngineTemp - 50f;
				}
				else
				{
					this.TempGauge.GetComponent<AnalogGauge>().Value = 0f;
				}
			}
			if (this.FuelGauge)
			{
				if (this.FuelTank && this.IgnitionON && this.Battery && this.BatteryWires)
				{
					this.FuelGauge.GetComponent<AnalogGauge>().Value = this.Fuel.FluidSize;
				}
				else
				{
					this.FuelGauge.GetComponent<AnalogGauge>().Value = 0f;
				}
			}
			if (this.BatteryGauge)
			{
				if (this.IgnitionON && this.Battery && this.BatteryWires)
				{
					this.BatteryGauge.GetComponent<AnalogGauge>().Value = this.Volts - 10f;
				}
				else
				{
					this.BatteryGauge.GetComponent<AnalogGauge>().Value = 0f;
				}
			}
		}
		if (this.GasPedal)
		{
			this.GasPedal.transform.localEulerAngles = new Vector3(-15f * Mathf.Clamp01(this.exp.input.InputSwappedThrottle), this.GasPedal.transform.localEulerAngles.y, this.GasPedal.transform.localEulerAngles.z);
		}
		if (this.BrakePedal)
		{
			this.BrakePedal.transform.localEulerAngles = new Vector3(-15f * Mathf.Clamp01(this.exp.input.InputSwappedBrakes), this.BrakePedal.transform.localEulerAngles.y, this.BrakePedal.transform.localEulerAngles.z);
		}
		if (this.ClutchPedal && this.Gearbox && this.Gearbox.Manual)
		{
			if (this.SittingInCar)
			{
				this.ClutchPedal.transform.localEulerAngles = new Vector3(15f * Mathf.Clamp01(this.exp.powertrain.clutch.clutchEngagement), this.ClutchPedal.transform.localEulerAngles.y, this.ClutchPedal.transform.localEulerAngles.z);
				return;
			}
			this.ClutchPedal.transform.localEulerAngles = new Vector3(20f, this.ClutchPedal.transform.localEulerAngles.y, this.ClutchPedal.transform.localEulerAngles.z);
		}
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0004EB28 File Offset: 0x0004CD28
	public void Accelerating()
	{
		if (!this.checkingSusp && !this.Bike)
		{
			this.FLwhellcontroller.transform.localRotation = Quaternion.Euler(0f, 0.9f + UnityEngine.Random.Range(1f, 2f) * (this.FLmaxBrokenParts - this.FLOkParts), 0f);
			this.FRwhellcontroller.transform.localRotation = Quaternion.Euler(0f, -0.9f - UnityEngine.Random.Range(1f, 2f) * (this.FRmaxBrokenParts - this.FROkParts), 0f);
			this.RLwhellcontroller.transform.localRotation = Quaternion.Euler(0f, -90f + UnityEngine.Random.Range(1f, 2f) * (this.RLmaxBrokenParts - this.RLOkParts), 0f);
			this.RRwhellcontroller.transform.localRotation = Quaternion.Euler(0f, -90f - UnityEngine.Random.Range(1f, 2f) * (this.RRmaxBrokenParts - this.RROkParts), 0f);
			this.checkingSusp = true;
		}
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x0004EC5C File Offset: 0x0004CE5C
	public void Decelerating()
	{
		if (!this.checkingSusp && !this.Bike)
		{
			this.FLwhellcontroller.transform.localRotation = Quaternion.Euler(0f, 0.9f - UnityEngine.Random.Range(1f, 2f) * (this.FLmaxBrokenParts - this.FLOkParts), 0f);
			this.FRwhellcontroller.transform.localRotation = Quaternion.Euler(0f, -0.9f + UnityEngine.Random.Range(1f, 2f) * (this.FRmaxBrokenParts - this.FROkParts), 0f);
			this.RLwhellcontroller.transform.localRotation = Quaternion.Euler(0f, -90f - UnityEngine.Random.Range(1f, 2f) * (this.RLmaxBrokenParts - this.RLOkParts), 0f);
			this.RRwhellcontroller.transform.localRotation = Quaternion.Euler(0f, -90f + UnityEngine.Random.Range(1f, 2f) * (this.RRmaxBrokenParts - this.RROkParts), 0f);
			this.checkingSusp = true;
		}
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x0004ED90 File Offset: 0x0004CF90
	public void Rolling()
	{
		if (!this.checkingSusp && !this.Bike)
		{
			this.FLwhellcontroller.transform.localRotation = Quaternion.Euler(0f, 0.9f, 0f);
			this.FRwhellcontroller.transform.localRotation = Quaternion.Euler(0f, -0.9f, 0f);
			this.RLwhellcontroller.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
			this.RRwhellcontroller.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
			this.checkingSusp = true;
		}
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x0004EE4C File Offset: 0x0004D04C
	public void CheckDrivetrain()
	{
		if (this.Bike)
		{
			if (this.Chain && this.ChainSprocket && this.WheelSprocket)
			{
				this.exp.powertrain.transmission.finalGearRatio = 5f;
				return;
			}
			this.exp.powertrain.transmission.finalGearRatio = 0f;
			return;
		}
		else
		{
			if (!this.WCFL)
			{
				return;
			}
			this.WCFL.drive = false;
			this.WCFR.drive = false;
			this.WCRL.drive = false;
			this.WCRR.drive = false;
			if (this.AWD)
			{
				if (this.Differential != null && this.DriveShaft != null && this.DifferentialFront != null && this.DriveShaftFront != null && this.TransferCase != null && this.AxleFL != null && this.AxleFR != null && this.AxleRL != null && this.AxleRR != null)
				{
					if (this.Differential.DiffLocked)
					{
						this.exp.powertrain.differentials[0].differentialType = DifferentialComponent.Type.Locked;
					}
					else
					{
						this.exp.powertrain.differentials[0].differentialType = DifferentialComponent.Type.Open;
					}
					this.exp.powertrain.differentials[1].differentialType = DifferentialComponent.Type.Locked;
					this.exp.powertrain.transmission.finalGearRatio = this.Differential.DiffRatio;
					this.WCFL.drive = true;
					this.WCFR.drive = true;
					this.WCRL.drive = true;
					this.WCRR.drive = true;
				}
				else if (this.DifferentialFront != null && this.DriveShaftFront != null && this.TransferCase != null && this.AxleFL != null && this.AxleFR != null)
				{
					this.exp.powertrain.differentials[1].differentialType = DifferentialComponent.Type.Open;
					this.exp.powertrain.differentials[1].biasAB = 0f;
					this.exp.powertrain.transmission.finalGearRatio = this.DifferentialFront.DiffRatio;
					this.WCFL.drive = true;
					this.WCFR.drive = true;
				}
				else if (this.Differential != null && this.DriveShaft != null && this.TransferCase != null && this.AxleRL != null && this.AxleRR != null)
				{
					if (this.Differential.DiffLocked)
					{
						this.exp.powertrain.differentials[0].differentialType = DifferentialComponent.Type.Locked;
					}
					else
					{
						this.exp.powertrain.differentials[0].differentialType = DifferentialComponent.Type.Open;
					}
					this.exp.powertrain.differentials[1].differentialType = DifferentialComponent.Type.Open;
					this.exp.powertrain.differentials[1].biasAB = 1f;
					this.exp.powertrain.transmission.finalGearRatio = this.Differential.DiffRatio;
					this.WCRL.drive = true;
					this.WCRR.drive = true;
				}
				else
				{
					this.exp.powertrain.transmission.finalGearRatio = 0f;
				}
				if (this.DriveShaftMiddle == null)
				{
					this.exp.powertrain.transmission.finalGearRatio = 0f;
					return;
				}
			}
			else
			{
				if (this.Differential != null && this.DriveShaft != null && this.AxleRL != null && this.AxleRR != null)
				{
					if (this.Differential.DiffLocked)
					{
						this.exp.powertrain.differentials[0].differentialType = DifferentialComponent.Type.Locked;
					}
					else
					{
						this.exp.powertrain.differentials[0].differentialType = DifferentialComponent.Type.Open;
					}
					this.exp.powertrain.transmission.finalGearRatio = this.Differential.DiffRatio;
				}
				else if (!this.Bike)
				{
					this.exp.powertrain.transmission.finalGearRatio = 0f;
				}
				this.RRwhellcontroller.GetComponent<WheelController>().drive = true;
				this.RLwhellcontroller.GetComponent<WheelController>().drive = true;
			}
			return;
		}
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x0004F368 File Offset: 0x0004D568
	public void KickStarting()
	{
		this.IgnitionON = true;
		this.StartingPossible = false;
		if (this.SparkWires && this.Distributor && this.Carburetor && this.EngineBlock && (this.EngineHead || this.EngineHead2) && (this.Rockers || this.Rockers2) && this.Crankshaft && this.EngineChain && this.Camshaft && ((this.TwinCam && this.Camshaft2) || !this.TwinCam) && this.CamshaftSprocket && this.CrankshaftSprocket && this.Fuel && this.FuelLine && this.FuelPump && this.IgnitionCoil && this.IgnitionCoil.Condition >= 0.1f && this.EngineBlock.Condition > 0f && this.EngineHead.Condition > 0.1f && this.Carburetor.Condition >= 0.1f && this.EngineChain.Condition >= 0.1f && this.Fuel.FluidSize >= 0.1f && this.FuelPump.Condition >= 0.1f)
		{
			if (this.pistons == this.EngineBlock.EngineCylinderCount && this.sparkplugs >= this.EngineBlock.EngineCylinderCount / 2f)
			{
				this.StartingPossible = true;
			}
			if (this.StartingPossible)
			{
				if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
				{
					base.GetComponent<MPobject>().networkDummy.StartCar();
					return;
				}
				this.StartCar();
			}
		}
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x0004F5B0 File Offset: 0x0004D7B0
	public void Cranking()
	{
		if (!this.StarterCoroutineRunning && this.Starter && this.Battery && this.BatteryWires && this.Starter.Condition > 0.1f && this.Battery.BatteryCharge > 11.8f && this.BatteryWires.GetComponent<Partinfo>().fixedImportantBolts >= 4f && this.Flywheel)
		{
			this.Starter.GetComponent<AudioSource>().PlayOneShot(this.StarterClick);
			if (this.Battery.BatteryCharge > 12f)
			{
				if (this.Starter.Condition > 0.1f)
				{
					this.Starter.GetComponent<AudioSource>().clip = this.StarterRun;
				}
				if (this.Starter.Condition < 0.4f)
				{
					this.Starter.GetComponent<AudioSource>().clip = this.StarterRunWorn;
				}
				this.Starter.GetComponent<AudioSource>().loop = true;
				this.Starter.GetComponent<AudioSource>().Play();
				base.StartCoroutine(this.CrankingStarter());
				this.StarterCoroutineRunning = true;
				this.StartingTimer = 0f;
				this.StartingTime = 0f;
				this.StartingPossible = false;
				if (((!this.DieselEngine && this.SparkWires && this.Distributor && this.Carburetor && this.IgnitionCoil) || (this.DieselEngine && this.FuelHoses)) && this.EngineBlock && (this.EngineHead || this.EngineHead2) && (this.Rockers || this.Rockers2) && this.Crankshaft && this.EngineChain && this.Camshaft && ((this.Injected && this.FuelHoses) || !this.Injected) && ((this.TwinCam && this.Camshaft2) || !this.TwinCam) && this.CamshaftSprocket && this.CrankshaftSprocket && this.Fuel && this.FuelLine && this.FuelPump && ((!this.DieselEngine && this.IgnitionCoil.Condition >= 0.1f) || this.DieselEngine) && this.EngineBlock.Condition > 0f && this.EngineHead.Condition > 0.1f && ((!this.DieselEngine && this.Carburetor.Condition >= 0.1f) || this.DieselEngine) && this.EngineChain.Condition >= 0.1f && this.Fuel.FluidSize >= 0.1f && this.FuelPump.Condition >= 0.1f)
				{
					if (this.pistons == this.EngineBlock.EngineCylinderCount && ((this.Injected && this.injectors >= this.EngineBlock.EngineCylinderCount / 2f) || !this.Injected) && ((this.DieselEngine && this.injectors >= this.EngineBlock.EngineCylinderCount / 2f) || (!this.DieselEngine && this.sparkplugs >= this.EngineBlock.EngineCylinderCount / 2f)))
					{
						this.StartingPossible = true;
						if (!this.DieselEngine)
						{
							this.StartingTime += (this.EngineBlock.EngineCylinderCount - this.sparkplugs) * 3f;
						}
					}
					if (!this.DieselEngine && this.IgnitionCoil.Condition <= 0.3f)
					{
						this.StartingTime += 0.2f;
					}
					if (!this.DieselEngine && this.SparkWires.Condition <= 0.3f)
					{
						this.StartingTime += 3f;
					}
					if (!this.DieselEngine && this.Distributor.Condition <= 0.3f)
					{
						this.StartingTime += 3f;
					}
					if (!this.DieselEngine && this.Carburetor.Condition <= 0.3f)
					{
						this.StartingTime += 2f;
					}
					if (this.EngineHead.Condition <= 0.3f)
					{
						this.StartingTime += 0.2f;
					}
					if (this.EngineChain.Condition <= 0.3f)
					{
						this.StartingTime += 0.2f;
					}
					if (this.Camshaft.Condition <= 0.3f)
					{
						this.StartingTime += 0.3f;
					}
					if (this.FuelLine.Condition <= 0.1f)
					{
						this.StartingTime += 4f;
					}
					if (this.FuelPump.Condition <= 0.3f)
					{
						this.StartingTime += 3f;
					}
				}
				this.StartingTime += 0.5f;
			}
		}
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x0004FB58 File Offset: 0x0004DD58
	public IEnumerator GlowPlugHeating()
	{
		yield return new WaitForSeconds(this.GlowPlugTimer);
		this.GlowPlugsready = true;
		if (this.Cluster && this.Cluster.ClusterGlowPlugs && this.GlowPlugRelay && this.GlowPlugRelay.Condition > 0.1f)
		{
			this.Cluster.ClusterGlowPlugs.GetComponent<MeshRenderer>().enabled = false;
		}
		yield break;
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x0004FB67 File Offset: 0x0004DD67
	private IEnumerator CrankingStarter()
	{
		yield return null;
		if (!this.exp.powertrain.engine.IsRunning)
		{
			this.StartingTimer += Time.deltaTime;
			if (this.HarmonicBalancer)
			{
				this.HarmonicBalancer.transform.Rotate(Vector3.forward, this.exp.powertrain.engine.RPM * 6f * Time.deltaTime);
			}
			if (this.CrankshaftPulley && this.Flywheel)
			{
				this.CrankshaftPulley.transform.Rotate(Vector3.forward, 100f * this.Starter.GetComponent<AudioSource>().pitch * 6f * Time.deltaTime);
				if (this.WaterPumpBelt && this.WaterPumpPulley && this.AlternatorPulley)
				{
					this.WaterPumpPulley.transform.Rotate(Vector3.forward, 100f * this.Starter.GetComponent<AudioSource>().pitch * 6f * Time.deltaTime);
				}
				if (this.AlternatorBelt && this.AlternatorPulley && this.WaterPumpPulley)
				{
					this.AlternatorPulley.transform.Rotate(Vector3.forward, 100f * this.Starter.GetComponent<AudioSource>().pitch * 10f * Time.deltaTime);
				}
				if (this.Camshaft && this.EngineChain && this.Camshaft.VisualObject)
				{
					this.Camshaft.VisualObject.transform.Rotate(Vector3.forward, 100f * this.Starter.GetComponent<AudioSource>().pitch * 3f * Time.deltaTime);
				}
				if (this.Camshaft2 && this.EngineChain && this.Camshaft2.VisualObject)
				{
					this.Camshaft2.VisualObject.transform.Rotate(Vector3.forward, 100f * this.Starter.GetComponent<AudioSource>().pitch * 3f * Time.deltaTime);
				}
				if (this.Crankshaft && this.Crankshaft.VisualObject)
				{
					this.Crankshaft.VisualObject.transform.Rotate(Vector3.forward, 100f * this.Starter.GetComponent<AudioSource>().pitch * 6f * Time.deltaTime);
				}
				if (this.Flywheel)
				{
					this.Flywheel.transform.Rotate(Vector3.forward, 100f * this.Starter.GetComponent<AudioSource>().pitch * 6f * Time.deltaTime);
				}
			}
			if (this.StartingPossible && ((this.DieselEngine && this.GlowPlugsready) || !this.DieselEngine) && this.StartingTimer >= this.StartingTime)
			{
				this.StarterCoroutineRunning = false;
				if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
				{
					base.GetComponent<MPobject>().networkDummy.StartCar();
				}
				else
				{
					this.StartCar();
				}
			}
			else if (!this.IgnitionKey.GetComponent<IgnitionKey>().Starter || this.Battery.BatteryCharge <= 12f)
			{
				this.Starter.GetComponent<AudioSource>().loop = false;
				this.Starter.GetComponent<AudioSource>().Stop();
				this.StarterCoroutineRunning = false;
				if (this.Crankshaft && this.Crankshaft.VisualObject)
				{
					this.Crankshaft.VisualObject.transform.localRotation = Quaternion.identity;
				}
			}
			else
			{
				if (this.Battery.BatteryCharge <= 12.2f)
				{
					this.Starter.GetComponent<AudioSource>().pitch = 1f - (12.2f - this.Battery.BatteryCharge) * 2f;
				}
				this.Battery.BatteryCharge -= 0.002f * Time.deltaTime;
				base.StartCoroutine(this.CrankingStarter());
			}
		}
		yield break;
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x0004FB78 File Offset: 0x0004DD78
	public void StartCar()
	{
		if (!this.Bike)
		{
			if (!this.SittingInCar)
			{
				this.ResetWheelControllers();
			}
			this.Starter.GetComponent<AudioSource>().loop = false;
			this.Starter.GetComponent<AudioSource>().Stop();
			this.StarterCoroutineRunning = false;
			this.exp.soundManager.Initialize();
			this.exp.powertrain.engine.Start();
			this.ExhaustSmokes.Play();
			this.ExhaustSmokes.transform.GetComponent<SMOKE>().enabled = true;
			if (this.DoubleHeads)
			{
				this.ExhaustSmokes2.Play();
				this.ExhaustSmokes2.transform.GetComponent<SMOKE>().enabled = true;
				return;
			}
		}
		else
		{
			this.IgnitionON = true;
			if (!this.SittingInCar)
			{
				this.ResetWheelControllers();
			}
			this.exp.soundManager.Initialize();
			this.exp.powertrain.engine.Start();
			this.ExhaustSmokes.Play();
			this.ExhaustSmokes.transform.GetComponent<SMOKE>().enabled = true;
			if (this.DoubleHeads)
			{
				this.ExhaustSmokes2.Play();
				this.ExhaustSmokes2.transform.GetComponent<SMOKE>().enabled = true;
			}
		}
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0004FCC0 File Offset: 0x0004DEC0
	public void ChangingGear(string gear, int gearint)
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy != null)
		{
			base.GetComponent<MPobject>().networkDummy.ChangingGear(gear, this.exp.powertrain.transmission.Gear);
			return;
		}
		this.ChangingGearContinue(gear, this.exp.powertrain.transmission.Gear);
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x0004FD30 File Offset: 0x0004DF30
	public void ChangingGearContinue(string gear, int gearint)
	{
		this.CurrentGear = gear;
		this.exp.powertrain.transmission.ShiftInto(gearint, false);
		this.exp.brakes.InPark = false;
		if (this.Shifter)
		{
			if (this.Gearbox && this.Gearbox.Manual)
			{
				if (this.CurrentGear == "R")
				{
					this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.Reverse;
				}
				if (this.CurrentGear == "N")
				{
					this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.N;
				}
				if (this.CurrentGear == "1")
				{
					this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.First;
				}
				if (this.CurrentGear == "2")
				{
					this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.Second;
				}
				if (this.CurrentGear == "3")
				{
					this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.Third;
				}
				if (this.CurrentGear == "4")
				{
					this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.Fourth;
				}
				if (this.CurrentGear == "5")
				{
					this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.Fifth;
				}
			}
			else
			{
				if (this.CurrentGear == "P")
				{
					if (this.Shifter.P == null)
					{
						this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.Third;
					}
					else
					{
						this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.P;
					}
					this.exp.brakes.InPark = true;
				}
				if (this.CurrentGear == "R")
				{
					if (this.Shifter.R == null)
					{
						this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.N;
					}
					else
					{
						this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.R;
					}
				}
				if (this.CurrentGear == "N")
				{
					this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.N;
				}
				if (this.CurrentGear == "D")
				{
					this.Shifter.gameObject.GetComponent<MeshFilter>().mesh = this.Shifter.Fourth;
				}
			}
			this.exp.powertrain.transmission.GetCurrentGearRatio();
			if (this.WCFL)
			{
				float num = 1.5f;
				if (this.WCFL.drive)
				{
					num -= 0.5f;
				}
				if (this.WCRL.drive)
				{
					num -= 0.5f;
				}
				if (this.WCFL.drive)
				{
					this.WCFL.maxpower = this.exp.powertrain.transmission.GetCurrentGearRatio() * this.EnginePower * 1.8f * num;
				}
				else
				{
					this.WCFL.maxpower = 0f;
				}
				if (this.WCFR.drive)
				{
					this.WCFR.maxpower = this.exp.powertrain.transmission.GetCurrentGearRatio() * this.EnginePower * 1.8f * num;
				}
				else
				{
					this.WCFR.maxpower = 0f;
				}
				if (this.WCRL.drive)
				{
					this.WCRL.maxpower = this.exp.powertrain.transmission.GetCurrentGearRatio() * this.EnginePower * 1.8f * num;
				}
				else
				{
					this.WCRL.maxpower = 0f;
				}
				if (this.WCRR.drive)
				{
					this.WCRR.maxpower = this.exp.powertrain.transmission.GetCurrentGearRatio() * this.EnginePower * 1.8f * num;
					return;
				}
				this.WCRR.maxpower = 0f;
			}
		}
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x000501D2 File Offset: 0x0004E3D2
	public void EngineStop()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.EngineStop();
			return;
		}
		this.EngineStop2();
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x0005020C File Offset: 0x0004E40C
	public void EngineStop2()
	{
		if (this.Cluster && this.Cluster.ClusterBat)
		{
			this.Cluster.ClusterBat.GetComponent<MeshRenderer>().enabled = false;
		}
		if (this.Cluster && this.Cluster.ClusterGlowPlugs)
		{
			this.Cluster.ClusterGlowPlugs.GetComponent<MeshRenderer>().enabled = false;
		}
		this.ExhaustSmokes.Stop();
		this.ExhaustSmokes2.Stop();
		this.exp.powertrain.engine.Stop();
		this.ExhaustSmokes.transform.GetComponent<SMOKE>().enabled = false;
		this.ExhaustSmokes2.transform.GetComponent<SMOKE>().enabled = false;
		if (this.Crankshaft && this.Crankshaft.VisualObject)
		{
			this.Crankshaft.VisualObject.transform.localRotation = Quaternion.identity;
		}
		if (this.EngineBlock && this.EngineBlock.GetComponent<AudioSource>())
		{
			this.EngineBlock.GetComponent<AudioSource>().Stop();
		}
		if (this.Blower && this.Blower.GetComponent<AudioSource>())
		{
			this.Blower.GetComponent<AudioSource>().Stop();
		}
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x00050371 File Offset: 0x0004E571
	private IEnumerator RotateWiperR()
	{
		Quaternion currentRotation = this.WiperR.transform.localRotation;
		Quaternion newRotation = currentRotation * Quaternion.AngleAxis(90f, Vector3.up);
		for (float t = 0f; t < 0.5f; t += Time.deltaTime)
		{
			Quaternion localRotation = Quaternion.Slerp(currentRotation, newRotation, t * 2f);
			this.WiperR.transform.localRotation = localRotation;
			yield return null;
		}
		this.WiperR.transform.localRotation = newRotation;
		base.StartCoroutine(this.RotateWiperRB());
		yield break;
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x00050380 File Offset: 0x0004E580
	private IEnumerator RotateWiperL()
	{
		Quaternion currentRotation = this.WiperL.transform.localRotation;
		Quaternion newRotation = currentRotation * Quaternion.AngleAxis(90f, Vector3.up);
		for (float t = 0f; t < 0.5f; t += Time.deltaTime)
		{
			Quaternion localRotation = Quaternion.Slerp(currentRotation, newRotation, t * 2f);
			this.WiperL.transform.localRotation = localRotation;
			yield return null;
		}
		this.WiperL.transform.localRotation = newRotation;
		base.StartCoroutine(this.RotateWiperLB());
		yield break;
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x0005038F File Offset: 0x0004E58F
	private IEnumerator RotateWiperOnly()
	{
		Quaternion currentRotation = this.WiperOnly.transform.localRotation;
		Quaternion newRotation = currentRotation * Quaternion.AngleAxis(-160f, Vector3.up);
		for (float t = 0f; t < 0.5f; t += Time.deltaTime)
		{
			Quaternion localRotation = Quaternion.Slerp(currentRotation, newRotation, t * 2f);
			this.WiperOnly.transform.localRotation = localRotation;
			yield return null;
		}
		this.WiperOnly.transform.localRotation = newRotation;
		base.StartCoroutine(this.RotateWiperOnlyB());
		yield break;
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x0005039E File Offset: 0x0004E59E
	private IEnumerator RotateWiperRB()
	{
		Quaternion currentRotation = this.WiperR.transform.localRotation;
		Quaternion newRotation = currentRotation * Quaternion.AngleAxis(-90f, Vector3.up);
		for (float t = 0f; t < 0.5f; t += Time.deltaTime)
		{
			Quaternion localRotation = Quaternion.Slerp(currentRotation, newRotation, t * 2f);
			this.WiperR.transform.localRotation = localRotation;
			yield return null;
		}
		this.WiperR.transform.localRotation = newRotation;
		if (this.WipersOn && this.IgnitionON && this.BatteryWires && this.Battery && this.Battery.BatteryCharge > 11.8f)
		{
			base.StartCoroutine(this.RotateWiperR());
		}
		else
		{
			this.WipersOn = false;
			this.WipersStopped = true;
		}
		yield break;
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x000503AD File Offset: 0x0004E5AD
	private IEnumerator RotateWiperLB()
	{
		Quaternion currentRotation = this.WiperL.transform.localRotation;
		Quaternion newRotation = currentRotation * Quaternion.AngleAxis(-90f, Vector3.up);
		for (float t = 0f; t < 0.5f; t += Time.deltaTime)
		{
			Quaternion localRotation = Quaternion.Slerp(currentRotation, newRotation, t * 2f);
			this.WiperL.transform.localRotation = localRotation;
			yield return null;
		}
		this.WiperL.transform.localRotation = newRotation;
		if (this.WipersOn && this.IgnitionON && this.BatteryWires && this.Battery && this.Battery.BatteryCharge > 11.8f)
		{
			base.StartCoroutine(this.RotateWiperL());
		}
		else
		{
			this.WipersOn = false;
			this.WipersStopped = true;
		}
		yield break;
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x000503BC File Offset: 0x0004E5BC
	private IEnumerator RotateWiperOnlyB()
	{
		Quaternion currentRotation = this.WiperOnly.transform.localRotation;
		Quaternion newRotation = currentRotation * Quaternion.AngleAxis(160f, Vector3.up);
		for (float t = 0f; t < 0.5f; t += Time.deltaTime)
		{
			Quaternion localRotation = Quaternion.Slerp(currentRotation, newRotation, t * 2f);
			this.WiperOnly.transform.localRotation = localRotation;
			yield return null;
		}
		this.WiperOnly.transform.localRotation = newRotation;
		if (this.WipersOn && this.IgnitionON && this.BatteryWires && this.Battery && this.Battery.BatteryCharge > 11.8f)
		{
			base.StartCoroutine(this.RotateWiperOnly());
		}
		else
		{
			this.WipersOn = false;
			this.WipersStopped = true;
		}
		yield break;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x000503CB File Offset: 0x0004E5CB
	public void RunningLightTurnOn()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.RunningLightTurnOn();
			return;
		}
		this.RunningLightTurnOn2();
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x00050404 File Offset: 0x0004E604
	public void RunningLightTurnOn2()
	{
		if (this.Electricity)
		{
			this.RunningLightOn = true;
			for (int i = 0; i < this.RunningLight.Length; i++)
			{
				CarLight carLight = this.RunningLight[i];
				if ((carLight.LightBulb && carLight.LightBulb.Condition > 0.4f) || carLight.DontNeedBulb)
				{
					carLight.TurnOn();
				}
			}
		}
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00050469 File Offset: 0x0004E669
	public void RunningLightTurnOff()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.RunningLightTurnOff();
			return;
		}
		this.RunningLightTurnOff2();
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x000504A4 File Offset: 0x0004E6A4
	public void RunningLightTurnOff2()
	{
		this.RunningLightOn = false;
		for (int i = 0; i < this.RunningLight.Length; i++)
		{
			CarLight carLight = this.RunningLight[i];
			if (carLight)
			{
				carLight.TurnOff();
			}
		}
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x000504E2 File Offset: 0x0004E6E2
	public void HeadLightLowTurnOn()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.HeadLightLowTurnOn();
			return;
		}
		this.HeadLightLowTurnOn2();
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0005051C File Offset: 0x0004E71C
	public void HeadLightLowTurnOn2()
	{
		if (this.Electricity)
		{
			this.HeadLightLowOn = true;
			for (int i = 0; i < this.HeadLightLow.Length; i++)
			{
				CarLight carLight = this.HeadLightLow[i];
				if (carLight.LightBulb && carLight.LightBulb.Condition > 0.4f)
				{
					carLight.Low.enabled = true;
				}
			}
		}
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x0005057F File Offset: 0x0004E77F
	public void HeadLightLowTurnOff()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.HeadLightLowTurnOff();
			return;
		}
		this.HeadLightLowTurnOff2();
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x000505B8 File Offset: 0x0004E7B8
	public void HeadLightLowTurnOff2()
	{
		this.HeadLightLowOn = false;
		for (int i = 0; i < this.HeadLightLow.Length; i++)
		{
			CarLight carLight = this.HeadLightLow[i];
			if (carLight)
			{
				carLight.Low.enabled = false;
			}
		}
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x000505FC File Offset: 0x0004E7FC
	public void HeadLightHighTurnOn()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.HeadLightHighTurnOn();
			return;
		}
		this.HeadLightHighTurnOn2();
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00050634 File Offset: 0x0004E834
	public void HeadLightHighTurnOn2()
	{
		if (this.Electricity)
		{
			this.HeadLightHighOn = true;
			for (int i = 0; i < this.HeadLightHigh.Length; i++)
			{
				CarLight carLight = this.HeadLightHigh[i];
				if (carLight.LightBulb && carLight.LightBulb.Condition > 0.4f)
				{
					carLight.High.enabled = true;
				}
			}
			if (this.Cluster && this.Cluster.ClusterHigh)
			{
				this.Cluster.ClusterHigh.GetComponent<MeshRenderer>().enabled = true;
			}
		}
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x000506CF File Offset: 0x0004E8CF
	public void HeadLightHighTurnOff()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.HeadLightHighTurnOff();
			return;
		}
		this.HeadLightHighTurnOff2();
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x00050708 File Offset: 0x0004E908
	public void HeadLightHighTurnOff2()
	{
		this.HeadLightHighOn = false;
		for (int i = 0; i < this.HeadLightHigh.Length; i++)
		{
			CarLight carLight = this.HeadLightHigh[i];
			if (carLight)
			{
				carLight.High.enabled = false;
			}
		}
		if (this.Cluster && this.Cluster.ClusterHigh)
		{
			this.Cluster.ClusterHigh.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00050781 File Offset: 0x0004E981
	public void BrakeLightTurnOn()
	{
		this.BrakeLightTurnOn2();
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x0005078C File Offset: 0x0004E98C
	public void BrakeLightTurnOn2()
	{
		if (this.Electricity)
		{
			this.BrakeLightOn = true;
			for (int i = 0; i < this.BrakeLight.Length; i++)
			{
				CarLight carLight = this.BrakeLight[i];
				if (carLight.LightBulb && carLight.LightBulb.Condition > 0.4f)
				{
					carLight.TurnOnBrake();
				}
			}
		}
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x000507E9 File Offset: 0x0004E9E9
	public void BrakeLightTurnOff()
	{
		this.BrakeLightTurnOff2();
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x000507F4 File Offset: 0x0004E9F4
	public void BrakeLightTurnOff2()
	{
		this.BrakeLightOn = false;
		for (int i = 0; i < this.BrakeLight.Length; i++)
		{
			CarLight carLight = this.BrakeLight[i];
			if (this.RunningLightOn && carLight.LightBulb && carLight.LightBulb.Condition > 0.4f && carLight.RunningLight)
			{
				carLight.TurnOn();
			}
			else
			{
				carLight.TurnOff();
			}
		}
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00050861 File Offset: 0x0004EA61
	public void ReverseLightTurnOn()
	{
		this.ReverseLightTurnOn2();
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x0005086C File Offset: 0x0004EA6C
	public void ReverseLightTurnOn2()
	{
		if (this.Electricity)
		{
			this.ReverseLightOn = true;
			for (int i = 0; i < this.ReverseLight.Length; i++)
			{
				CarLight carLight = this.ReverseLight[i];
				if (carLight.LightBulb && carLight.LightBulb.Condition > 0.4f)
				{
					carLight.TurnOn();
				}
			}
		}
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x000508C9 File Offset: 0x0004EAC9
	public void ReverseLightTurnOff()
	{
		this.ReverseLightTurnOff2();
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x000508D4 File Offset: 0x0004EAD4
	public void ReverseLightTurnOff2()
	{
		this.ReverseLightOn = false;
		for (int i = 0; i < this.ReverseLight.Length; i++)
		{
			CarLight carLight = this.ReverseLight[i];
			if (carLight)
			{
				carLight.TurnOff();
			}
		}
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00050912 File Offset: 0x0004EB12
	public void LeftLightTurnOn()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.LeftLightTurnOn();
			return;
		}
		this.LeftLightTurnOn2();
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x0005094C File Offset: 0x0004EB4C
	public void LeftLightTurnOn2()
	{
		for (int i = 0; i < this.LeftLight.Length; i++)
		{
			CarLight carLight = this.LeftLight[i];
			if (carLight.LightBulb && carLight.LightBulb.Condition > 0.4f)
			{
				carLight.TurnOn();
			}
		}
		if (this.Cluster && this.Cluster.ClusterL)
		{
			this.Cluster.ClusterL.GetComponent<MeshRenderer>().enabled = true;
		}
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x000509CF File Offset: 0x0004EBCF
	public void LeftLightTurnOff()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.LeftLightTurnOff();
			return;
		}
		this.LeftLightTurnOff2();
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x00050A08 File Offset: 0x0004EC08
	public void LeftLightTurnOff2()
	{
		for (int i = 0; i < this.LeftLight.Length; i++)
		{
			CarLight carLight = this.LeftLight[i];
			if (carLight)
			{
				carLight.TurnOff();
			}
		}
		if (this.Cluster && this.Cluster.ClusterL)
		{
			this.Cluster.ClusterL.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x00050A74 File Offset: 0x0004EC74
	public void RightLightTurnOn()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.RightLightTurnOn();
			return;
		}
		this.RightLightTurnOn2();
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x00050AAC File Offset: 0x0004ECAC
	public void RightLightTurnOn2()
	{
		for (int i = 0; i < this.RightLight.Length; i++)
		{
			CarLight carLight = this.RightLight[i];
			if (carLight.LightBulb && carLight.LightBulb.Condition > 0.4f)
			{
				carLight.TurnOn();
			}
		}
		if (this.Cluster && this.Cluster.ClusterR)
		{
			this.Cluster.ClusterR.GetComponent<MeshRenderer>().enabled = true;
		}
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00050B2F File Offset: 0x0004ED2F
	public void RightLightTurnOff()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.RightLightTurnOff();
			return;
		}
		this.RightLightTurnOff2();
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x00050B68 File Offset: 0x0004ED68
	public void RightLightTurnOff2()
	{
		for (int i = 0; i < this.RightLight.Length; i++)
		{
			CarLight carLight = this.RightLight[i];
			if (carLight)
			{
				carLight.TurnOff();
			}
		}
		if (this.Cluster && this.Cluster.ClusterR)
		{
			this.Cluster.ClusterR.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x00050BD4 File Offset: 0x0004EDD4
	public void WiperSwitch()
	{
		if (!this.WipersOn && this.IgnitionON && this.BatteryWires && this.Battery && this.Battery.BatteryCharge > 11.8f && this.WiperMotor && this.WiperMotor.Condition > 0.4f && this.WipersStopped)
		{
			this.WiperSOn();
			return;
		}
		this.WiperOff();
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00050C51 File Offset: 0x0004EE51
	public void WiperSOn()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.WiperSOn();
			return;
		}
		this.WiperSOn2();
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x00050C8C File Offset: 0x0004EE8C
	public void WiperSOn2()
	{
		if (this.WiperL)
		{
			base.StartCoroutine(this.RotateWiperL());
		}
		if (this.WiperR)
		{
			base.StartCoroutine(this.RotateWiperR());
		}
		if (this.WiperOnly)
		{
			base.StartCoroutine(this.RotateWiperOnly());
		}
		this.WipersOn = true;
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x00050CEE File Offset: 0x0004EEEE
	public void WiperOff()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.WiperOff();
			return;
		}
		this.WiperOff2();
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x00050D26 File Offset: 0x0004EF26
	public void WiperOff2()
	{
		this.WipersOn = false;
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x00050D2F File Offset: 0x0004EF2F
	public void IgnitionTurnedOff()
	{
		if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy)
		{
			base.GetComponent<MPobject>().networkDummy.IgnitionTurnedOff();
			return;
		}
		this.IgnitionTurnedOff2();
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x00050D67 File Offset: 0x0004EF67
	public void IgnitionTurnedOff2()
	{
		this.EngineStop();
		if (!this.HazardLightOn)
		{
			this.RightLightTurnOff();
			this.LeftLightTurnOff();
		}
		Debug.Log("enginestopped");
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00050D90 File Offset: 0x0004EF90
	public void NoElectricity()
	{
		this.EngineStop();
		this.RightLightTurnOff();
		this.LeftLightTurnOff();
		this.RunningLightTurnOff();
		this.BrakeLightTurnOff();
		this.ReverseLightTurnOff();
		this.HeadLightLowTurnOff();
		this.HeadLightHighTurnOff();
		this.HazardLightOn = false;
		this.WipersOn = false;
		if (this.Cluster && this.Cluster.ClusterHigh)
		{
			this.Cluster.ClusterHigh.GetComponent<MeshRenderer>().enabled = false;
		}
		if (this.Cluster && this.Cluster.ClusterL)
		{
			this.Cluster.ClusterL.GetComponent<MeshRenderer>().enabled = false;
		}
		if (this.Cluster && this.Cluster.ClusterR)
		{
			this.Cluster.ClusterR.GetComponent<MeshRenderer>().enabled = false;
		}
		if (this.Cluster && this.Cluster.ClusterBat)
		{
			this.Cluster.ClusterBat.GetComponent<MeshRenderer>().enabled = false;
		}
		if (this.Cluster && this.Cluster.ClusterGlowPlugs)
		{
			this.Cluster.ClusterGlowPlugs.GetComponent<MeshRenderer>().enabled = false;
		}
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x00050EE4 File Offset: 0x0004F0E4
	public void FreezeForMoment()
	{
		base.GetComponent<Rigidbody>().isKinematic = true;
		base.StartCoroutine(this.waittt());
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x00050EFF File Offset: 0x0004F0FF
	private IEnumerator waittt()
	{
		yield return new WaitForSeconds(4f);
		if (!tools.MPrunning)
		{
			base.GetComponent<Rigidbody>().isKinematic = false;
		}
		else if (base.GetComponent<MPobject>() && base.GetComponent<MPobject>().networkDummy.hasAuthority)
		{
			base.GetComponent<Rigidbody>().isKinematic = false;
		}
		yield break;
	}

	// Token: 0x04000ED8 RID: 3800
	public bool Opt1;

	// Token: 0x04000ED9 RID: 3801
	public bool Opt2;

	// Token: 0x04000EDA RID: 3802
	public bool Opt3;

	// Token: 0x04000EDB RID: 3803
	public bool Opt4;

	// Token: 0x04000EDC RID: 3804
	public bool Opt5;

	// Token: 0x04000EDD RID: 3805
	public bool Opt6;

	// Token: 0x04000EDE RID: 3806
	public bool Opt7;

	// Token: 0x04000EDF RID: 3807
	public bool Opt8;

	// Token: 0x04000EE0 RID: 3808
	public bool Opt9;

	// Token: 0x04000EE1 RID: 3809
	public float Raw_driftAngle;

	// Token: 0x04000EE2 RID: 3810
	public float _driftAngle;

	// Token: 0x04000EE3 RID: 3811
	public float _steerAngle;

	// Token: 0x04000EE4 RID: 3812
	public float DriftModifier;

	// Token: 0x04000EE5 RID: 3813
	public Transform DriftDirection;

	// Token: 0x04000EE6 RID: 3814
	public bool OnDyno;

	// Token: 0x04000EE7 RID: 3815
	public float HP;

	// Token: 0x04000EE8 RID: 3816
	public string RuinedEngineParts;

	// Token: 0x04000EE9 RID: 3817
	public string WornEngineParts;

	// Token: 0x04000EEA RID: 3818
	public string RuinedSuspensionParts;

	// Token: 0x04000EEB RID: 3819
	public string WornSuspensionParts;

	// Token: 0x04000EEC RID: 3820
	public string RuinedBrakeParts;

	// Token: 0x04000EED RID: 3821
	public string WornBrakeParts;

	// Token: 0x04000EEE RID: 3822
	public string RuinedOtherParts;

	// Token: 0x04000EEF RID: 3823
	public string WornOtherparts;

	// Token: 0x04000EF0 RID: 3824
	public string CantCrank;

	// Token: 0x04000EF1 RID: 3825
	public string CantRun;

	// Token: 0x04000EF2 RID: 3826
	public string BrakeProblems;

	// Token: 0x04000EF3 RID: 3827
	public float consumptionPerHour;

	// Token: 0x04000EF4 RID: 3828
	public float maxConsumptionPerHour;

	// Token: 0x04000EF5 RID: 3829
	public Vector3 StartPosition;

	// Token: 0x04000EF6 RID: 3830
	public Quaternion StartRotation;

	// Token: 0x04000EF7 RID: 3831
	public int ObjectNumber;

	// Token: 0x04000EF8 RID: 3832
	public GameObject PREFAB;

	// Token: 0x04000EF9 RID: 3833
	public bool InBarn;

	// Token: 0x04000EFA RID: 3834
	public VehicleController exp;

	// Token: 0x04000EFB RID: 3835
	public NumberPlateManager NumberParent;

	// Token: 0x04000EFC RID: 3836
	public Vector3 SpawnPoint;

	// Token: 0x04000EFD RID: 3837
	public GameObject HandbrakeObject;

	// Token: 0x04000EFE RID: 3838
	public InsideItems insideitems;

	// Token: 0x04000EFF RID: 3839
	public InsideItems Traielerinsideitems;

	// Token: 0x04000F00 RID: 3840
	public CarProperties LeftTieRod;

	// Token: 0x04000F01 RID: 3841
	public CarProperties RightTieRod;

	// Token: 0x04000F02 RID: 3842
	public CarProperties SteeringWheel;

	// Token: 0x04000F03 RID: 3843
	public CarProperties SteeringBox;

	// Token: 0x04000F04 RID: 3844
	public float InCarHeight;

	// Token: 0x04000F05 RID: 3845
	public GameObject Player;

	// Token: 0x04000F06 RID: 3846
	public GameObject AudioParent;

	// Token: 0x04000F07 RID: 3847
	public bool Started;

	// Token: 0x04000F08 RID: 3848
	public bool SittingInCar;

	// Token: 0x04000F09 RID: 3849
	public string CurrentGear;

	// Token: 0x04000F0A RID: 3850
	public string Owner;

	// Token: 0x04000F0B RID: 3851
	public string CarName;

	// Token: 0x04000F0C RID: 3852
	public float CarPrice;

	// Token: 0x04000F0D RID: 3853
	public float CarPriceStart;

	// Token: 0x04000F0E RID: 3854
	public float Mileage;

	// Token: 0x04000F0F RID: 3855
	public string OriginalInteriorColor;

	// Token: 0x04000F10 RID: 3856
	public float AllWelds;

	// Token: 0x04000F11 RID: 3857
	public float AllBolts;

	// Token: 0x04000F12 RID: 3858
	public float Weight;

	// Token: 0x04000F13 RID: 3859
	public float CarPartPrice;

	// Token: 0x04000F14 RID: 3860
	public bool HandBraking;

	// Token: 0x04000F15 RID: 3861
	public bool HandBrakeInstalled;

	// Token: 0x04000F16 RID: 3862
	public bool HandBrakeCableInstalled;

	// Token: 0x04000F17 RID: 3863
	public GameObject Handbrake;

	// Token: 0x04000F18 RID: 3864
	public GameObject HandbrakeCable;

	// Token: 0x04000F19 RID: 3865
	public GameObject MainBrakeLIne;

	// Token: 0x04000F1A RID: 3866
	public GameObject BrakeLIneFL;

	// Token: 0x04000F1B RID: 3867
	public GameObject BrakeLIneFR;

	// Token: 0x04000F1C RID: 3868
	public GameObject BrakeLIneRL;

	// Token: 0x04000F1D RID: 3869
	public GameObject BrakeLIneRR;

	// Token: 0x04000F1E RID: 3870
	public FLUID BrakeFluid;

	// Token: 0x04000F1F RID: 3871
	public CarProperties BrakePedal;

	// Token: 0x04000F20 RID: 3872
	public bool BrakesLeaking;

	// Token: 0x04000F21 RID: 3873
	public bool CanBrake;

	// Token: 0x04000F22 RID: 3874
	public float FLmaxBrokenParts;

	// Token: 0x04000F23 RID: 3875
	public float FRmaxBrokenParts;

	// Token: 0x04000F24 RID: 3876
	public float RLmaxBrokenParts;

	// Token: 0x04000F25 RID: 3877
	public float RRmaxBrokenParts;

	// Token: 0x04000F26 RID: 3878
	public float FLOkParts;

	// Token: 0x04000F27 RID: 3879
	public float FROkParts;

	// Token: 0x04000F28 RID: 3880
	public float RLOkParts;

	// Token: 0x04000F29 RID: 3881
	public float RROkParts;

	// Token: 0x04000F2A RID: 3882
	public GameObject FLwhellcontroller;

	// Token: 0x04000F2B RID: 3883
	public GameObject FRwhellcontroller;

	// Token: 0x04000F2C RID: 3884
	public GameObject RLwhellcontroller;

	// Token: 0x04000F2D RID: 3885
	public GameObject RRwhellcontroller;

	// Token: 0x04000F2E RID: 3886
	public WheelController WCFL;

	// Token: 0x04000F2F RID: 3887
	public WheelController WCFR;

	// Token: 0x04000F30 RID: 3888
	public WheelController WCRL;

	// Token: 0x04000F31 RID: 3889
	public WheelController WCRR;

	// Token: 0x04000F32 RID: 3890
	public CarProperties CarTrailer;

	// Token: 0x04000F33 RID: 3891
	public bool AWD;

	// Token: 0x04000F34 RID: 3892
	public CarProperties Differential;

	// Token: 0x04000F35 RID: 3893
	public CarProperties DifferentialFront;

	// Token: 0x04000F36 RID: 3894
	public CarProperties TransferCase;

	// Token: 0x04000F37 RID: 3895
	public CarProperties DriveShaft;

	// Token: 0x04000F38 RID: 3896
	public CarProperties DriveShaftFront;

	// Token: 0x04000F39 RID: 3897
	public CarProperties DriveShaftMiddle;

	// Token: 0x04000F3A RID: 3898
	public CarProperties AxleFL;

	// Token: 0x04000F3B RID: 3899
	public CarProperties AxleFR;

	// Token: 0x04000F3C RID: 3900
	public CarProperties AxleRL;

	// Token: 0x04000F3D RID: 3901
	public CarProperties AxleRR;

	// Token: 0x04000F3E RID: 3902
	public CarProperties Shifter;

	// Token: 0x04000F3F RID: 3903
	public CarProperties Gearbox;

	// Token: 0x04000F40 RID: 3904
	public GearProfile gearingProfil;

	// Token: 0x04000F41 RID: 3905
	public CarProperties Chain;

	// Token: 0x04000F42 RID: 3906
	public CarProperties ChainSprocket;

	// Token: 0x04000F43 RID: 3907
	public CarProperties WheelSprocket;

	// Token: 0x04000F44 RID: 3908
	public CarProperties Flywheel;

	// Token: 0x04000F45 RID: 3909
	public CarProperties Clutch;

	// Token: 0x04000F46 RID: 3910
	public CarProperties ClutchCover;

	// Token: 0x04000F47 RID: 3911
	public CarProperties ClutchPedal;

	// Token: 0x04000F48 RID: 3912
	public CarProperties IgnitionCoil;

	// Token: 0x04000F49 RID: 3913
	public CarProperties SparkWires;

	// Token: 0x04000F4A RID: 3914
	public CarProperties Distributor;

	// Token: 0x04000F4B RID: 3915
	public CarProperties Carburetor;

	// Token: 0x04000F4C RID: 3916
	public CarProperties Turbo;

	// Token: 0x04000F4D RID: 3917
	public CarProperties TurboPipe;

	// Token: 0x04000F4E RID: 3918
	public CarProperties WiperMotor;

	// Token: 0x04000F4F RID: 3919
	public Transform WiperL;

	// Token: 0x04000F50 RID: 3920
	public Transform WiperR;

	// Token: 0x04000F51 RID: 3921
	public Transform WiperOnly;

	// Token: 0x04000F52 RID: 3922
	public bool WipersOn;

	// Token: 0x04000F53 RID: 3923
	public bool WipersStopped;

	// Token: 0x04000F54 RID: 3924
	public WindowLift WindowLiftFL;

	// Token: 0x04000F55 RID: 3925
	public WindowLift WindowLiftFR;

	// Token: 0x04000F56 RID: 3926
	public WindowLift WindowLiftRL;

	// Token: 0x04000F57 RID: 3927
	public WindowLift WindowLiftRR;

	// Token: 0x04000F58 RID: 3928
	public CarLight[] BrakeLight;

	// Token: 0x04000F59 RID: 3929
	public CarLight[] ReverseLight;

	// Token: 0x04000F5A RID: 3930
	public CarLight[] HeadLightLow;

	// Token: 0x04000F5B RID: 3931
	public CarLight[] HeadLightHigh;

	// Token: 0x04000F5C RID: 3932
	public CarLight[] RunningLight;

	// Token: 0x04000F5D RID: 3933
	public CarLight[] RightLight;

	// Token: 0x04000F5E RID: 3934
	public CarLight[] LeftLight;

	// Token: 0x04000F5F RID: 3935
	public CarProperties ClusterL;

	// Token: 0x04000F60 RID: 3936
	public CarProperties ClusterR;

	// Token: 0x04000F61 RID: 3937
	public CarProperties ClusterBat;

	// Token: 0x04000F62 RID: 3938
	public CarProperties ClusterHigh;

	// Token: 0x04000F63 RID: 3939
	public CarProperties ClusterABS;

	// Token: 0x04000F64 RID: 3940
	public CarProperties ClusterCheck;

	// Token: 0x04000F65 RID: 3941
	public bool BrakeLightOn;

	// Token: 0x04000F66 RID: 3942
	public bool ReverseLightOn;

	// Token: 0x04000F67 RID: 3943
	public bool HeadLightLowOn;

	// Token: 0x04000F68 RID: 3944
	public bool HeadLightHighOn;

	// Token: 0x04000F69 RID: 3945
	public bool RunningLightOn;

	// Token: 0x04000F6A RID: 3946
	public bool RightLightOn;

	// Token: 0x04000F6B RID: 3947
	public bool LeftLightOn;

	// Token: 0x04000F6C RID: 3948
	public bool HazardLightOn;

	// Token: 0x04000F6D RID: 3949
	public CarProperties Battery;

	// Token: 0x04000F6E RID: 3950
	public CarProperties BatteryWires;

	// Token: 0x04000F6F RID: 3951
	public CarProperties Starter;

	// Token: 0x04000F70 RID: 3952
	public AudioClip StarterClick;

	// Token: 0x04000F71 RID: 3953
	public AudioClip StarterRun;

	// Token: 0x04000F72 RID: 3954
	public AudioClip StarterRunWorn;

	// Token: 0x04000F73 RID: 3955
	public AudioClip EngineStarted;

	// Token: 0x04000F74 RID: 3956
	public AudioClip EngineExploaded;

	// Token: 0x04000F75 RID: 3957
	public AudioClip BlinkerOn;

	// Token: 0x04000F76 RID: 3958
	public AudioClip BlinkerOff;

	// Token: 0x04000F77 RID: 3959
	public CarProperties Alternator;

	// Token: 0x04000F78 RID: 3960
	public Transform AlternatorPulley;

	// Token: 0x04000F79 RID: 3961
	public GameObject IgnitionKey;

	// Token: 0x04000F7A RID: 3962
	public bool IgnitionON;

	// Token: 0x04000F7B RID: 3963
	public CarProperties Radiator;

	// Token: 0x04000F7C RID: 3964
	public CarProperties RadiatorFan;

	// Token: 0x04000F7D RID: 3965
	public CarProperties RadiatorGT;

	// Token: 0x04000F7E RID: 3966
	public CarProperties RadiatorFanGT;

	// Token: 0x04000F7F RID: 3967
	public CarProperties WaterPump;

	// Token: 0x04000F80 RID: 3968
	public CarProperties WaterPumpBelt;

	// Token: 0x04000F81 RID: 3969
	public CarProperties WaterPumpPulley;

	// Token: 0x04000F82 RID: 3970
	public CarProperties WaterHoseUpper;

	// Token: 0x04000F83 RID: 3971
	public CarProperties WaterHoseLower;

	// Token: 0x04000F84 RID: 3972
	public CarProperties ThermostatHousing;

	// Token: 0x04000F85 RID: 3973
	public CarProperties BlowerPulley;

	// Token: 0x04000F86 RID: 3974
	public CarProperties Blower;

	// Token: 0x04000F87 RID: 3975
	public CarProperties BlowerBelt;

	// Token: 0x04000F88 RID: 3976
	public CarProperties FuelFilter;

	// Token: 0x04000F89 RID: 3977
	public CarProperties FuelHoses;

	// Token: 0x04000F8A RID: 3978
	public CarProperties GlowPlugRelay;

	// Token: 0x04000F8B RID: 3979
	public float glowplugs;

	// Token: 0x04000F8C RID: 3980
	public float injectors;

	// Token: 0x04000F8D RID: 3981
	public CarProperties[] GlowPlugs;

	// Token: 0x04000F8E RID: 3982
	public CarProperties[] Injectors;

	// Token: 0x04000F8F RID: 3983
	public bool DoubleHeads;

	// Token: 0x04000F90 RID: 3984
	public int EngineType;

	// Token: 0x04000F91 RID: 3985
	public CarProperties EngineBlock;

	// Token: 0x04000F92 RID: 3986
	public bool AirCooled;

	// Token: 0x04000F93 RID: 3987
	public CarProperties EngineHead;

	// Token: 0x04000F94 RID: 3988
	public CarProperties EngineHead2;

	// Token: 0x04000F95 RID: 3989
	public CarProperties HeadGasket;

	// Token: 0x04000F96 RID: 3990
	public CarProperties HeadGasket2;

	// Token: 0x04000F97 RID: 3991
	public CarProperties HeadCover;

	// Token: 0x04000F98 RID: 3992
	public CarProperties HeadCover2;

	// Token: 0x04000F99 RID: 3993
	public CarProperties Rockers;

	// Token: 0x04000F9A RID: 3994
	public CarProperties Rockers2;

	// Token: 0x04000F9B RID: 3995
	public float pistons;

	// Token: 0x04000F9C RID: 3996
	public float sparkplugs;

	// Token: 0x04000F9D RID: 3997
	public CarProperties[] Pistons;

	// Token: 0x04000F9E RID: 3998
	public CarProperties[] SparkPlugs;

	// Token: 0x04000F9F RID: 3999
	public CarProperties[] WearWorking;

	// Token: 0x04000FA0 RID: 4000
	public CarProperties[] WearDriving;

	// Token: 0x04000FA1 RID: 4001
	public CarProperties[] WearByTime;

	// Token: 0x04000FA2 RID: 4002
	public CarProperties CamshaftSprocket;

	// Token: 0x04000FA3 RID: 4003
	public CarProperties CrankshaftSprocket;

	// Token: 0x04000FA4 RID: 4004
	public CarProperties HarmonicBalancer;

	// Token: 0x04000FA5 RID: 4005
	public CarProperties CrankshaftPulley;

	// Token: 0x04000FA6 RID: 4006
	public CarProperties AlternatorBelt;

	// Token: 0x04000FA7 RID: 4007
	public CarProperties Crankshaft;

	// Token: 0x04000FA8 RID: 4008
	public CarProperties EngineChain;

	// Token: 0x04000FA9 RID: 4009
	public CarProperties Camshaft;

	// Token: 0x04000FAA RID: 4010
	public CarProperties Camshaft2;

	// Token: 0x04000FAB RID: 4011
	public CarProperties AirFilter;

	// Token: 0x04000FAC RID: 4012
	public CarProperties AirFilterCover;

	// Token: 0x04000FAD RID: 4013
	public CarProperties OilPan;

	// Token: 0x04000FAE RID: 4014
	public CarProperties OilFilter;

	// Token: 0x04000FAF RID: 4015
	public CarProperties GasPedal;

	// Token: 0x04000FB0 RID: 4016
	public CarProperties FuelLine;

	// Token: 0x04000FB1 RID: 4017
	public CarProperties FuelTank;

	// Token: 0x04000FB2 RID: 4018
	public CarProperties FuelPump;

	// Token: 0x04000FB3 RID: 4019
	public CarProperties Exhaust;

	// Token: 0x04000FB4 RID: 4020
	public CarProperties ExhaustHeader;

	// Token: 0x04000FB5 RID: 4021
	public CarProperties ExhaustManifold;

	// Token: 0x04000FB6 RID: 4022
	public CarProperties Exhaust2;

	// Token: 0x04000FB7 RID: 4023
	public CarProperties ExhaustHeader2;

	// Token: 0x04000FB8 RID: 4024
	public CarProperties ExhaustManifold2;

	// Token: 0x04000FB9 RID: 4025
	public Transform ExhaustSmoke;

	// Token: 0x04000FBA RID: 4026
	public Transform ExhaustHeaderSmoke;

	// Token: 0x04000FBB RID: 4027
	public Transform ExhaustManifoldSmoke;

	// Token: 0x04000FBC RID: 4028
	public Transform HeadSmoke;

	// Token: 0x04000FBD RID: 4029
	public Transform ExhaustSmoke2;

	// Token: 0x04000FBE RID: 4030
	public Transform ExhaustHeaderSmoke2;

	// Token: 0x04000FBF RID: 4031
	public Transform ExhaustManifoldSmoke2;

	// Token: 0x04000FC0 RID: 4032
	public Transform HeadSmoke2;

	// Token: 0x04000FC1 RID: 4033
	public FLUID Coolant;

	// Token: 0x04000FC2 RID: 4034
	public FLUID EngineOil;

	// Token: 0x04000FC3 RID: 4035
	public FLUID Fuel;

	// Token: 0x04000FC4 RID: 4036
	public bool DieselEngine;

	// Token: 0x04000FC5 RID: 4037
	public bool Injected;

	// Token: 0x04000FC6 RID: 4038
	public bool TwinCam;

	// Token: 0x04000FC7 RID: 4039
	public float EngineTemp;

	// Token: 0x04000FC8 RID: 4040
	public float Volts;

	// Token: 0x04000FC9 RID: 4041
	public bool Electricity;

	// Token: 0x04000FCA RID: 4042
	public float EnginePower;

	// Token: 0x04000FCB RID: 4043
	public float EngineMaxPower;

	// Token: 0x04000FCC RID: 4044
	public CarProperties Cluster;

	// Token: 0x04000FCD RID: 4045
	public GameObject AnalogRpmGauge;

	// Token: 0x04000FCE RID: 4046
	public GameObject DigitalRpmGauge;

	// Token: 0x04000FCF RID: 4047
	public GameObject AnalogSpeedGauge;

	// Token: 0x04000FD0 RID: 4048
	public Text DigitalSpeedGauge;

	// Token: 0x04000FD1 RID: 4049
	public GameObject TempGauge;

	// Token: 0x04000FD2 RID: 4050
	public GameObject FuelGauge;

	// Token: 0x04000FD3 RID: 4051
	public GameObject BatteryGauge;

	// Token: 0x04000FD4 RID: 4052
	public GameObject Clock;

	// Token: 0x04000FD5 RID: 4053
	public ParticleSystem ExhaustSmokes;

	// Token: 0x04000FD6 RID: 4054
	public ParticleSystem ExhaustSmokes2;

	// Token: 0x04000FD7 RID: 4055
	public ParticleSystem CoolantSmokes;

	// Token: 0x04000FD8 RID: 4056
	private float CleanRatio;

	// Token: 0x04000FD9 RID: 4057
	private float NoRustRatio;

	// Token: 0x04000FDA RID: 4058
	private float PaintRatio;

	// Token: 0x04000FDB RID: 4059
	private float DamagedBodyPanels;

	// Token: 0x04000FDC RID: 4060
	private float AllBodyPanels;

	// Token: 0x04000FDD RID: 4061
	private float CleanRatioParts;

	// Token: 0x04000FDE RID: 4062
	private float NoRustRatioParts;

	// Token: 0x04000FDF RID: 4063
	private float PaintRatioParts;

	// Token: 0x04000FE0 RID: 4064
	private float PaintGoodParts;

	// Token: 0x04000FE1 RID: 4065
	private float RustGoodParts;

	// Token: 0x04000FE2 RID: 4066
	private float AverageCondition;

	// Token: 0x04000FE3 RID: 4067
	private float conditCount;

	// Token: 0x04000FE4 RID: 4068
	private bool checkingSusp;

	// Token: 0x04000FE5 RID: 4069
	public bool StartingPossible;

	// Token: 0x04000FE6 RID: 4070
	public bool GlowPlugsready;

	// Token: 0x04000FE7 RID: 4071
	public float StartingTime;

	// Token: 0x04000FE8 RID: 4072
	public float StartingTimer;

	// Token: 0x04000FE9 RID: 4073
	public float GlowPlugTimer;

	// Token: 0x04000FEA RID: 4074
	private bool StarterCoroutineRunning;

	// Token: 0x04000FEB RID: 4075
	public float timer;

	// Token: 0x04000FEC RID: 4076
	public float timer2;

	// Token: 0x04000FED RID: 4077
	public float timer3;

	// Token: 0x04000FEE RID: 4078
	public float timer4;

	// Token: 0x04000FEF RID: 4079
	public bool Interval;

	// Token: 0x04000FF0 RID: 4080
	public float OilLeak;

	// Token: 0x04000FF1 RID: 4081
	public float FuelLeak;

	// Token: 0x04000FF2 RID: 4082
	public float CoolantLeak;

	// Token: 0x04000FF3 RID: 4083
	public float InjectorPower;

	// Token: 0x04000FF4 RID: 4084
	public float SparkPower;

	// Token: 0x04000FF5 RID: 4085
	public float PistonPower;

	// Token: 0x04000FF6 RID: 4086
	public bool revving;

	// Token: 0x04000FF7 RID: 4087
	private int allparts;

	// Token: 0x04000FF8 RID: 4088
	private int existingparts;

	// Token: 0x04000FF9 RID: 4089
	public Material GoodMaterial;

	// Token: 0x04000FFA RID: 4090
	public Material[] BadMaterials;

	// Token: 0x04000FFB RID: 4091
	public Material DirtyMaterial;

	// Token: 0x04000FFC RID: 4092
	public Material MossyMaterial;

	// Token: 0x04000FFD RID: 4093
	public int StartOptions;

	// Token: 0x04000FFE RID: 4094
	public int OriginalInterior;

	// Token: 0x04000FFF RID: 4095
	public Material Interior;

	// Token: 0x04001000 RID: 4096
	public Color[] StockColors;

	// Token: 0x04001001 RID: 4097
	public Color OriginalColor;

	// Token: 0x04001002 RID: 4098
	public Color Color;

	// Token: 0x04001003 RID: 4099
	public float PartsCount;

	// Token: 0x04001004 RID: 4100
	public int Seed;

	// Token: 0x04001005 RID: 4101
	public int MPSeed;

	// Token: 0x04001006 RID: 4102
	public bool Bike;

	// Token: 0x04001007 RID: 4103
	public bool NoRearSprings;

	// Token: 0x04001008 RID: 4104
	public Player player;

	// Token: 0x04001009 RID: 4105
	public P3dPaintSphere DUST;

	// Token: 0x0400100A RID: 4106
	private Vector3 previousLoc;
}
