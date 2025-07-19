using System;
using System.Collections;
using Assets.SimpleLocalization;
using PaintIn3D;
using TMPro;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class JobsManager : MonoBehaviour
{
	// Token: 0x060006F8 RID: 1784 RVA: 0x00037714 File Offset: 0x00035914
	private void GO()
	{
		this.count = 0;
		foreach (object obj in this.JobParent.transform)
		{
			Transform transform = (Transform)obj;
			this.count++;
		}
		this.Jobs = new GameObject[this.count];
		this.count = 0;
		foreach (object obj2 in this.JobParent.transform)
		{
			Transform transform2 = (Transform)obj2;
			this.Jobs[this.count] = transform2.gameObject;
			this.count++;
		}
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00037800 File Offset: 0x00035A00
	public void OnEnable()
	{
		if (!this.started)
		{
			this.GO();
		}
		this.started = true;
		this.AudioParent = GameObject.Find("hand");
		this.CarList = GameObject.Find("CarsParent");
		this.Cars = this.CarList.GetComponent<CarList>().JobCars;
		this.Reward.text = "";
		this.TotalReward.text = "";
		this.Description.text = "";
		this.Boltsmissing.text = "";
		this.Weldsmissing.text = "";
		this.Fluidsmissing.text = "";
		this.Partsmissing.text = "";
		this.PartsDamaged.text = "";
		this.PartsCrashed.text = "";
		this.NotDone.text = "";
		if (!this.ActiveJob && !this.PreviewJob)
		{
			this.ChangeJob();
		}
		if (this.PreviewJob)
		{
			this.SkipJobButton.SetActive(true);
			this.TakeJobButton.SetActive(true);
			this.AbandonButton.SetActive(false);
			this.CompleteButton.SetActive(false);
			this.JobScript = this.PreviewJob.GetComponent<Job>();
			this.Description.text = LocalizationManager.Localize(this.JobScript.DescriptionKey).ToString();
			if (this.JobScript.Reward > 0f)
			{
				this.Reward.text = LocalizationManager.Localize("Reward").ToString() + this.JobScript.Reward.ToString() + "$";
			}
			else
			{
				this.Reward.text = LocalizationManager.Localize("Reward Not specified").ToString();
			}
		}
		if (this.ActiveJob)
		{
			foreach (MainCarProperties mainCarProperties in UnityEngine.Object.FindObjectsOfType<MainCarProperties>())
			{
				if (mainCarProperties.Owner == "Client")
				{
					this.Car = mainCarProperties.gameObject;
				}
			}
			this.SkipJobButton.SetActive(false);
			this.TakeJobButton.SetActive(false);
			this.AbandonButton.SetActive(true);
			this.CompleteButton.SetActive(false);
			this.JobScript = this.ActiveJob.GetComponent<Job>();
			this.Description.text = LocalizationManager.Localize(this.JobScript.DescriptionKey).ToString();
			base.StartCoroutine(this.CheckCarCondition());
		}
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00037A9F File Offset: 0x00035C9F
	private void OnDisable()
	{
		this.Car = null;
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x00037AA8 File Offset: 0x00035CA8
	public void ChangeJob()
	{
		this.SkipJobButton.SetActive(true);
		this.TakeJobButton.SetActive(true);
		this.AbandonButton.SetActive(false);
		this.CompleteButton.SetActive(false);
		UnityEngine.Random.seed = DateTime.Now.Millisecond;
		this.PreviewJob = this.Jobs[UnityEngine.Random.Range(0, this.Jobs.Length)];
		this.JobScript = this.PreviewJob.GetComponent<Job>();
		this.Description.text = LocalizationManager.Localize(this.JobScript.DescriptionKey).ToString();
		if (this.JobScript.Reward > 0f)
		{
			this.Reward.text = LocalizationManager.Localize("Reward").ToString() + this.JobScript.Reward.ToString() + "$";
			return;
		}
		this.Reward.text = LocalizationManager.Localize("Reward Not specified").ToString();
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x00037BA4 File Offset: 0x00035DA4
	public void AcceptJob()
	{
		this.SkipJobButton.SetActive(false);
		this.TakeJobButton.SetActive(false);
		this.AbandonButton.SetActive(true);
		this.CompleteButton.SetActive(false);
		this.ActiveJob = this.PreviewJob;
		if (this.JobScript.CrashedCar)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)], new Vector3(UnityEngine.Random.Range(0.1f, 10f), UnityEngine.Random.Range(-99f, -70f), UnityEngine.Random.Range(0.1f, 10f)), Quaternion.Euler((float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360), (float)UnityEngine.Random.Range(0, 360)));
			gameObject.GetComponent<MainCarProperties>().SpawnPoint = this.SpawnSpot.transform.position;
			gameObject.GetComponent<MainCarProperties>().CreatingStockCrached();
			this.Car = gameObject;
			this.AllBolts = this.Car.GetComponent<MainCarProperties>().AllBolts;
			this.AllWelds = this.Car.GetComponent<MainCarProperties>().AllWelds;
			this.Car.GetComponent<MainCarProperties>().Owner = "Client";
			this.Car.GetComponent<MainCarProperties>().Start();
			this.TryTimes = 20;
			base.StartCoroutine(this.WaitCreating());
			return;
		}
		int millisecond = DateTime.Now.Millisecond;
		if (tools.MPrunning)
		{
			tools.NetworkPLayer.SpawnJob(millisecond, this.ActiveJob.transform.name);
			return;
		}
		this.GeneratingCar(millisecond, this.ActiveJob.transform.name, 0);
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00037D50 File Offset: 0x00035F50
	public void GeneratingCar(int seed, string ActiveJ, int mpnr)
	{
		this.ActiveJob = GameObject.Find(ActiveJ);
		this.JobScript = this.ActiveJob.GetComponent<Job>();
		UnityEngine.Random.seed = seed;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Cars[UnityEngine.Random.Range(0, this.Cars.Length)], this.SpawnSpot.transform.position, this.SpawnSpot.transform.rotation);
		gameObject.GetComponent<MainCarProperties>().SpawnPoint = this.SpawnSpot.transform.position;
		gameObject.GetComponent<MainCarProperties>().CreatingStock(seed);
		this.Car = gameObject;
		this.AllBolts = this.Car.GetComponent<MainCarProperties>().AllBolts;
		this.AllWelds = this.Car.GetComponent<MainCarProperties>().AllWelds;
		if (tools.MPrunning)
		{
			tools.NetworkPLayer.SpawnCar(this.Car, mpnr);
		}
		this.Car.GetComponent<MainCarProperties>().Owner = "Client";
		if (this.JobScript.Reward > 0f)
		{
			this.Realreward = this.JobScript.Reward;
		}
		this.TryTimes = 20;
		this.AfterPartcount = 0;
		this.Partcount = this.Car.GetComponent<MainCarProperties>().PartsCount;
		CarProperties[] componentsInChildren = this.Car.GetComponentsInChildren<CarProperties>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].SinglePart)
			{
				this.AfterPartcount++;
			}
		}
		this.Partcount = (float)this.AfterPartcount;
		this.SetCarPartCondition(seed);
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x00037ED0 File Offset: 0x000360D0
	private IEnumerator WaitCreating()
	{
		yield return new WaitForSeconds(3f);
		if (this.Car.transform.position.y > 40f && this.TryTimes > 0)
		{
			if (this.JobScript.Reward > 0f)
			{
				this.Realreward = this.JobScript.Reward;
			}
			this.TryTimes = 20;
			base.StartCoroutine(this.FirstCheckCarCondition());
		}
		else
		{
			this.TryTimes--;
			base.StartCoroutine(this.WaitCreating());
		}
		yield break;
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x00037EE0 File Offset: 0x000360E0
	public void SetCarPartCondition(int seed)
	{
		UnityEngine.Random.seed = seed;
		this.TryTimes--;
		this.SetBadPart = false;
		foreach (FLUID fluid in this.Car.GetComponentsInChildren<FLUID>())
		{
			if (this.JobScript.ChangeOil && fluid.Oil)
			{
				fluid.Condition = 0.02f;
			}
		}
		this.Color = UnityEngine.Random.ColorHSV();
		this.Color = new Color(1f - this.Color.r, 1f - this.Color.g, 1f - this.Color.b);
		this.AffectedParts = 0;
		foreach (CarProperties carProperties in this.Car.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.SinglePart)
			{
				carProperties.Condition = 1f;
				carProperties.Owner = "Client";
				if (this.JobScript.WashCar && carProperties.Washable)
				{
					Material[] materials = carProperties.gameObject.GetComponent<Renderer>().materials;
					materials[1] = this.MossyMaterial;
					this.SetBadPart = true;
					carProperties.gameObject.GetComponent<Renderer>().materials = materials;
				}
				if (this.JobScript.ChangeOil && carProperties.OilFilter)
				{
					carProperties.Condition = 0.02f;
					this.SetBadPart = true;
				}
				if (this.JobScript.RepaintPart && carProperties.Paintable && !this.SetBadPart && carProperties.gameObject.GetComponent<P3dChangeCounter>())
				{
					if (UnityEngine.Random.Range(1, 15) < 3)
					{
						carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = Color.grey;
						this.SetBadPart = true;
						carProperties.PaintIsSet = true;
					}
					this.AffectedParts++;
				}
				if (this.JobScript.Repaint && carProperties.Paintable)
				{
					carProperties.gameObject.GetComponent<P3dPaintableTexture>().Color = this.Color;
					this.SetBadPart = true;
					carProperties.PaintIsSet = true;
					this.AffectedParts++;
				}
				if (this.JobScript.FixRust && carProperties.Paintable && carProperties.Washable && carProperties.MeshRepairable && carProperties.gameObject.GetComponent<P3dChangeCounter>())
				{
					carProperties.Condition = UnityEngine.Random.Range(0.01f, 0.7f);
					if (carProperties.Condition < 0.4f)
					{
						this.Realreward += 100f;
						this.SetBadPart = true;
					}
				}
				if (this.JobScript.FixRustPart && carProperties.Paintable && carProperties.Washable && carProperties.MeshRepairable && !this.SetBadPart && carProperties.gameObject.GetComponent<P3dChangeCounter>())
				{
					carProperties.Condition = UnityEngine.Random.Range(0.1f, 0.6f);
					if (carProperties.Condition < 0.4f)
					{
						carProperties.Condition = 0.2f;
						this.SetBadPart = true;
					}
				}
				if (this.TryTimes < 2)
				{
					this.MaxCondition = 0.07f;
				}
				else
				{
					this.MaxCondition = 0.15f;
				}
				if (this.JobScript.RealWheel && carProperties.RealWheel)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Tire && carProperties.Tire)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.HandBrakeCable && carProperties.HandBrakeCable)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.SuspensionPart && carProperties.SuspensionPart)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Hub && carProperties.Hub)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Spring && carProperties.Spring)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Damper && carProperties.Damper)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.BrakePad && carProperties.BrakePad)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.BrakeDisc && carProperties.BrakeDisc)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.BrakeLine && carProperties.BrakeLine)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.MainBrakeLine && carProperties.MainBrakeLine)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.TieRod && carProperties.TieRod)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.SteeringWheel && carProperties.SteeringWheel)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.WindowLift && carProperties.WindowLift)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.WiperMotor && carProperties.WiperMotor)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.WiperBlade && carProperties.WiperBlade)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Bulb && carProperties.Bulb)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.DriveShaft && carProperties.DriveShaft)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Gearbox && carProperties.Gearbox)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Shifter && carProperties.Shifter)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Diff && carProperties.Diff)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Flywheel && carProperties.Flywheel)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Clutch && carProperties.Clutch)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.ClutchCover && carProperties.ClutchCover)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.FuelLine && carProperties.FuelLine)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.FuelTank && carProperties.FuelTank)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.FuelPump && carProperties.FuelPump)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.IgnitionCoil && carProperties.IgnitionCoil)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.SparkPlug && carProperties.SparkPlug)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.SparkWires && carProperties.SparkWires)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Distributor && carProperties.Distributor)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Carburetor && carProperties.Carburetor)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Battery && carProperties.Battery)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
					if (carProperties.Condition < 0.4f)
					{
						carProperties.BatteryCharge = 11f;
					}
				}
				if (this.JobScript.BatteryWires && carProperties.BatteryWires)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Alternator && carProperties.Alternator)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Starter && carProperties.Starter)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.CrankshaftPulley && carProperties.CrankshaftPulley)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.AlternatorBelt && carProperties.AlternatorBelt)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.EngineBlock && carProperties.EngineBlock)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.EngineHead && carProperties.EngineHead)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.HeadGasket && carProperties.HeadGasket)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.HeadCover && carProperties.HeadCover)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.AirFilter && carProperties.AirFilter)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.AirFilterCover && carProperties.AirFilterCover)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Crankshaft && carProperties.Crankshaft)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.EngineChain && carProperties.EngineChain)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Camshaft && carProperties.Camshaft)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Piston && carProperties.Piston)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Radiator && carProperties.Radiator)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.RadiatorFan && carProperties.RadiatorFan)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.RadiatorGT && carProperties.RadiatorGT)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.RadiatorFanGT && carProperties.RadiatorFanGT)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.WaterPump && carProperties.WaterPump)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.WaterPumpBelt && carProperties.WaterPumpBelt)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.WaterHoseUpper && carProperties.WaterHoseUpper)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.WaterHoseLower && carProperties.WaterHoseLower)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.OilPan && carProperties.OilPan)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.OilFilter && carProperties.OilFilter)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Exhaust && carProperties.Exhaust)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.ExhaustHeader && carProperties.ExhaustHeader)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.ExhaustManifold && carProperties.ExhaustManifold)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (this.JobScript.Cluster && carProperties.Cluster)
				{
					carProperties.Condition = UnityEngine.Random.Range(this.MinCondition, this.MaxCondition);
				}
				if (carProperties.Condition < 0.1f)
				{
					this.SetBadPart = true;
				}
				else
				{
					carProperties.Condition = 1f;
				}
			}
		}
		if (!this.SetBadPart && this.TryTimes > 0)
		{
			this.SetCarPartCondition(seed);
			return;
		}
		if (!this.SetBadPart && this.TryTimes == 0)
		{
			this.DidntCome();
			return;
		}
		if (!tools.MPrunning || (tools.MPrunning && tools.NetworkPLayer.isServer))
		{
			base.StartCoroutine(this.FirstCheckCarCondition());
		}
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x00038DDC File Offset: 0x00036FDC
	public void AbandonJob()
	{
		this.ActiveJob = null;
		this.PreviewJob = null;
		if (this.Car)
		{
			foreach (Transform transform in this.Car.GetComponentsInChildren<Transform>())
			{
				if (transform.name == "TrailerHandle" && transform.GetComponent<PickupHand>())
				{
					transform.GetComponent<PickupHand>().CarSold();
				}
				if (transform.name == "Hook" && transform.GetComponent<WinchHook>())
				{
					transform.GetComponent<WinchHook>().RemoveFromHand();
				}
				if (transform.GetComponent<MPobject>() && transform.GetComponent<MPobject>().networkDummy)
				{
					transform.GetComponent<MPobject>().networkDummy.DestroyMe();
				}
			}
			if (this.Car)
			{
				UnityEngine.Object.Destroy(this.Car);
			}
			this.GetRidOfParts();
		}
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x00038ECC File Offset: 0x000370CC
	public void CompleteJob()
	{
		this.ActiveJob = null;
		this.PreviewJob = null;
		foreach (Transform transform in this.Car.GetComponentsInChildren<Transform>())
		{
			if (transform.name == "TrailerHandle" && transform.GetComponent<PickupHand>())
			{
				transform.GetComponent<PickupHand>().CarSold();
			}
			if (transform.name == "Hook" && transform.GetComponent<WinchHook>())
			{
				transform.GetComponent<WinchHook>().RemoveFromHand();
			}
			if (transform.GetComponent<MPobject>() && transform.GetComponent<MPobject>().networkDummy)
			{
				transform.GetComponent<MPobject>().networkDummy.DestroyMe();
			}
		}
		if (this.Car)
		{
			UnityEngine.Object.Destroy(this.Car);
		}
		if (this.JobScript.CrashedCar)
		{
			if (this.CurrentValue >= this.Realreward / this.JobScript.RewardModifier)
			{
				tools.money += this.Realreward - this.Penalty;
			}
			else
			{
				tools.money += (this.CurrentValue - this.StartValue) * 1.3f;
			}
		}
		else
		{
			tools.money += this.Realreward - this.Penalty;
		}
		this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
		this.KeepParts();
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00039048 File Offset: 0x00037248
	public void CheckJobProgress(bool firstcheck)
	{
		if (!this.Car)
		{
			this.DidntCome();
			return;
		}
		if (this.JobScript.Reward > 0f)
		{
			this.Realreward = this.JobScript.Reward;
		}
		this.Penalty = 0f;
		if ((!this.JobScript.PaintMatters || (this.NoRustRatio > 0.98f && this.PaintRatio > 0.98f && this.CleanRatio > 0.85f)) && this.NoMeshDamagedParts && this.NoRuinedParts && this.NoWornParts && (float)this.AfterPartcount >= this.Partcount)
		{
			this.CompleteButton.SetActive(true);
			if (this.JobScript.ChangeOil)
			{
				this.CompleteButton.SetActive(false);
				foreach (FLUID fluid in this.Car.GetComponentsInChildren<FLUID>())
				{
					if (fluid.Oil && fluid.Condition > 0.1f && fluid.FluidSize >= fluid.MinFluidSize)
					{
						this.CompleteButton.SetActive(true);
					}
				}
			}
		}
		foreach (FLUID fluid2 in this.Car.GetComponentsInChildren<FLUID>())
		{
			if (fluid2.FluidSize < fluid2.MinFluidSize)
			{
				this.CompleteButton.SetActive(false);
				this.Fluidsmissing.text = LocalizationManager.Localize("Missing fluids").ToString();
			}
		}
		if (this.JobScript.Reward > 0f)
		{
			this.Realreward = this.JobScript.Reward;
		}
		this.Reward.text = LocalizationManager.Localize("Reward").ToString() + this.Realreward.ToString() + "$";
		this.DebugText.text = string.Concat(new string[]
		{
			Application.version.ToString(),
			" ",
			this.CleanRatio.ToString(),
			" ",
			this.NoRustRatio.ToString(),
			" ",
			this.PaintRatio.ToString(),
			this.NoRuinedParts.ToString(),
			this.NoWornParts.ToString(),
			this.NoMeshDamagedParts.ToString(),
			this.Partcount.ToString(),
			this.AfterPartcount.ToString()
		});
		if (this.CompleteButton.activeSelf && this.TightBolts < this.AllBolts - 4f)
		{
			this.Boltsmissing.text = LocalizationManager.Localize("There are loose bolts").ToString() + (this.Realreward / 20f * (this.AllBolts - this.TightBolts)).ToString() + "$";
			this.Penalty += this.Realreward / 20f * (this.AllBolts - this.TightBolts);
		}
		if (this.CompleteButton.activeSelf && this.TightWelds < this.AllWelds - 4f)
		{
			this.Weldsmissing.text = LocalizationManager.Localize("There are loose welds").ToString() + (this.Realreward / 20f * (this.AllWelds - this.TightWelds)).ToString() + "$";
			this.Penalty += this.Realreward / 20f * (this.AllWelds - this.TightWelds);
		}
		if ((float)this.AfterPartcount < this.Partcount)
		{
			this.Partsmissing.text = LocalizationManager.Localize("Some parts are missing").ToString();
		}
		if (!this.NoWornParts && !this.NoRuinedParts)
		{
			this.PartsDamaged.text = LocalizationManager.Localize("Some parts are damaged").ToString();
		}
		if (!this.NoMeshDamagedParts)
		{
			this.PartsCrashed.text = LocalizationManager.Localize("Car have dented panels").ToString();
		}
		if (this.CompleteButton.activeSelf)
		{
			this.TotalReward.text = LocalizationManager.Localize("Total").ToString() + (this.Realreward - this.Penalty).ToString() + "$";
		}
		if (this.Realreward <= 0f || (firstcheck && this.CompleteButton.activeSelf) || this.StartValue == 0f)
		{
			this.DidntCome();
			return;
		}
		if (tools.MPrunning && tools.NetworkPLayer.isServer)
		{
			tools.NetworkPLayer.SetJobStats(this.StartValue, this.Partcount, this.Realreward, this.ActiveJob.transform.name);
		}
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x00039519 File Offset: 0x00037719
	public void DidntCome()
	{
		this.DidntCome2();
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x00039524 File Offset: 0x00037724
	public void DidntCome2()
	{
		Debug.Log("didntcome");
		if (this.Car != null)
		{
			foreach (Transform transform in this.Car.GetComponentsInChildren<Transform>())
			{
				if (transform.name == "TrailerHandle" && transform.GetComponent<PickupHand>())
				{
					transform.GetComponent<PickupHand>().CarSold();
				}
				if (transform.name == "Hook" && transform.GetComponent<WinchHook>())
				{
					transform.GetComponent<WinchHook>().RemoveFromHand();
				}
				if (transform.GetComponent<MPobject>() && transform.GetComponent<MPobject>().networkDummy)
				{
					transform.GetComponent<MPobject>().networkDummy.DestroyMe();
				}
			}
			if (this.Car)
			{
				UnityEngine.Object.Destroy(this.Car);
			}
		}
		this.Reward.text = "";
		this.TotalReward.text = "";
		this.Description.text = LocalizationManager.Localize("Client didn't come").ToString();
		this.Boltsmissing.text = "";
		this.Weldsmissing.text = "";
		this.Fluidsmissing.text = "";
		this.Partsmissing.text = "";
		this.PartsDamaged.text = "";
		this.PartsCrashed.text = "";
		this.NotDone.text = "";
		this.ActiveJob = null;
		this.PreviewJob = null;
		this.SkipJobButton.SetActive(true);
		this.TakeJobButton.SetActive(false);
		this.AbandonButton.SetActive(false);
		this.CompleteButton.SetActive(false);
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x000396F3 File Offset: 0x000378F3
	private IEnumerator CheckCarCondition()
	{
		P3dChangeCounter[] targetLis = this.Car.GetComponentsInChildren<P3dChangeCounter>();
		foreach (P3dChangeCounter p3dChangeCounter in targetLis)
		{
			p3dChangeCounter.enabled = true;
			if (p3dChangeCounter.gameObject.GetComponent<CarProperties>().Paintable && p3dChangeCounter.Threshold == 0.1f)
			{
				p3dChangeCounter.Color = this.Car.GetComponent<MainCarProperties>().Color;
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
		this.CurrentValue = 0f;
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
				p3dChangeCounter2.gameObject.GetComponent<CarProperties>().CheckRustStart();
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
		this.CleanRatio /= this.CleanRatioParts;
		this.NoRustRatio = this.RustGoodParts / this.NoRustRatioParts;
		this.PaintRatio = this.PaintGoodParts / this.PaintRatioParts;
		this.NoRuinedParts = true;
		this.NoWornParts = true;
		this.NoMeshDamagedParts = true;
		this.MoneySpent = 0f;
		this.AfterPartcount = 0;
		foreach (CarProperties carProperties in this.Car.GetComponentsInChildren<CarProperties>())
		{
			if (carProperties.SinglePart)
			{
				Debug.Log("SINGLEPART " + carProperties.transform.name);
				this.AfterPartcount++;
				if (carProperties.Condition < 0.4f)
				{
					Debug.Log("wornPart is " + carProperties.transform.name);
					this.NoWornParts = false;
				}
				if (carProperties.Condition < 0.1f)
				{
					this.NoWornParts = false;
					this.NoRuinedParts = false;
				}
				if (carProperties.MeshDamaged)
				{
					this.NoMeshDamagedParts = false;
				}
				if (carProperties.Tire && carProperties.TirePressure < 1.7f)
				{
					this.NoWornParts = false;
				}
				if (carProperties.Owner == "Client" && carProperties.Condition > 0.4f)
				{
					this.MoneySpent += carProperties.gameObject.GetComponent<Partinfo>().price;
				}
				if (carProperties.Condition > 0.4f)
				{
					this.CurrentValue += carProperties.gameObject.GetComponent<Partinfo>().price;
				}
				if (!carProperties.PREFAB)
				{
					string str = "missing partPreFAB";
					GameObject gameObject = carProperties.gameObject;
					Debug.Log(str + ((gameObject != null) ? gameObject.ToString() : null));
				}
			}
			else if (carProperties.PREFAB)
			{
				string str2 = "missing part";
				GameObject gameObject2 = carProperties.gameObject;
				Debug.Log(str2 + ((gameObject2 != null) ? gameObject2.ToString() : null));
			}
			else if (base.transform.parent.GetComponent<transparents>())
			{
				string str3 = "missing part";
				GameObject gameObject3 = carProperties.gameObject;
				Debug.Log(str3 + ((gameObject3 != null) ? gameObject3.ToString() : null));
			}
		}
		this.TightBolts = 0f;
		this.TightWelds = 0f;
		foreach (Partinfo partinfo in this.Car.GetComponentsInChildren<Partinfo>())
		{
			this.TightBolts += partinfo.fixedImportantBolts + partinfo.tightnuts;
			this.TightWelds += partinfo.fixedwelds;
		}
		this.CheckJobProgress(false);
		yield break;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00039702 File Offset: 0x00037902
	private IEnumerator FirstCheckCarCondition()
	{
		P3dChangeCounter[] targetLis = this.Car.GetComponentsInChildren<P3dChangeCounter>();
		foreach (P3dChangeCounter p3dChangeCounter in targetLis)
		{
			p3dChangeCounter.enabled = true;
			if (p3dChangeCounter.gameObject.GetComponent<CarProperties>().Paintable && p3dChangeCounter.Threshold == 0.1f)
			{
				p3dChangeCounter.Color = this.Car.GetComponent<MainCarProperties>().Color;
			}
		}
		yield return 0;
		yield return 0;
		yield return 0;
		if (this.Car.transform.position.y < 49f)
		{
			this.FirstCheckCarCondition();
		}
		else
		{
			this.CleanRatio = 0.1f;
			this.NoRustRatio = 0.1f;
			this.PaintRatio = 0.1f;
			this.CleanRatioParts = 0f;
			this.NoRustRatioParts = 0f;
			this.PaintRatioParts = 0f;
			this.PaintGoodParts = 0f;
			this.RustGoodParts = 0f;
			this.StartValue = 0f;
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
			this.CleanRatio /= this.CleanRatioParts;
			this.NoRustRatio = this.RustGoodParts / this.NoRustRatioParts;
			this.PaintRatio = this.PaintGoodParts / this.PaintRatioParts;
			this.NoRuinedParts = true;
			this.NoWornParts = true;
			this.NoMeshDamagedParts = true;
			this.Realreward = 0f;
			foreach (CarProperties carProperties in this.Car.GetComponentsInChildren<CarProperties>())
			{
				if (carProperties.SinglePart)
				{
					if (carProperties.Condition < 0.4f)
					{
						this.NoWornParts = false;
						this.Realreward += carProperties.gameObject.GetComponent<Partinfo>().price;
					}
					if (carProperties.Condition < 0.1f)
					{
						this.NoWornParts = false;
						this.NoRuinedParts = false;
					}
					if (carProperties.MeshDamaged && !this.JobScript.CrashedCar)
					{
						this.NoMeshDamagedParts = false;
						this.Realreward += carProperties.gameObject.GetComponent<Partinfo>().price;
					}
					if (this.JobScript.CrashedCar && (carProperties.MeshLittleDamaged || carProperties.MeshDamaged))
					{
						this.Realreward += carProperties.gameObject.GetComponent<Partinfo>().price;
						carProperties.Condition = 0.05f;
					}
					if (carProperties.Condition > 0.4f)
					{
						this.StartValue += carProperties.gameObject.GetComponent<Partinfo>().price;
					}
				}
			}
			this.CurrentValue = this.StartValue;
			if (this.JobScript.RewardModifier > 0f)
			{
				this.Realreward *= this.JobScript.RewardModifier;
			}
			this.Realreward *= UnityEngine.Random.Range(0.9f, 1.1f);
			this.Realreward = Mathf.Round(this.Realreward);
			this.CheckJobProgress(true);
		}
		yield break;
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x00039714 File Offset: 0x00037914
	public void GetRidOfParts()
	{
		foreach (CarProperties carProperties in UnityEngine.Object.FindObjectsOfType<CarProperties>())
		{
			foreach (Transform transform in this.Car.GetComponentsInChildren<Transform>())
			{
				if (transform.name == "TrailerHandle" && transform.GetComponent<PickupHand>())
				{
					transform.GetComponent<PickupHand>().CarSold();
				}
			}
			if (carProperties.Owner == "Client")
			{
				UnityEngine.Object.Destroy(carProperties.gameObject);
			}
		}
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x000397A8 File Offset: 0x000379A8
	public void KeepParts()
	{
		foreach (CarProperties carProperties in UnityEngine.Object.FindObjectsOfType<CarProperties>())
		{
			if (carProperties.Owner == "Client")
			{
				carProperties.Owner = "Player";
			}
		}
	}

	// Token: 0x04000AD4 RID: 2772
	public float MinCondition;

	// Token: 0x04000AD5 RID: 2773
	public float MaxCondition;

	// Token: 0x04000AD6 RID: 2774
	public float AllWelds;

	// Token: 0x04000AD7 RID: 2775
	public float AllBolts;

	// Token: 0x04000AD8 RID: 2776
	public float TightWelds;

	// Token: 0x04000AD9 RID: 2777
	public float TightBolts;

	// Token: 0x04000ADA RID: 2778
	public float StartValue;

	// Token: 0x04000ADB RID: 2779
	public float CurrentValue;

	// Token: 0x04000ADC RID: 2780
	public int AffectedParts;

	// Token: 0x04000ADD RID: 2781
	public float MoneySpent;

	// Token: 0x04000ADE RID: 2782
	public float Partcount;

	// Token: 0x04000ADF RID: 2783
	public int AfterPartcount;

	// Token: 0x04000AE0 RID: 2784
	public Color Color;

	// Token: 0x04000AE1 RID: 2785
	public GameObject abandonask;

	// Token: 0x04000AE2 RID: 2786
	public GameObject SkipJobButton;

	// Token: 0x04000AE3 RID: 2787
	public GameObject TakeJobButton;

	// Token: 0x04000AE4 RID: 2788
	public GameObject AbandonButton;

	// Token: 0x04000AE5 RID: 2789
	public GameObject CompleteButton;

	// Token: 0x04000AE6 RID: 2790
	public Material MossyMaterial;

	// Token: 0x04000AE7 RID: 2791
	public GameObject AudioParent;

	// Token: 0x04000AE8 RID: 2792
	public GameObject SpawnSpot;

	// Token: 0x04000AE9 RID: 2793
	public GameObject CarList;

	// Token: 0x04000AEA RID: 2794
	public GameObject[] Cars;

	// Token: 0x04000AEB RID: 2795
	public GameObject Car;

	// Token: 0x04000AEC RID: 2796
	public GameObject[] Jobs;

	// Token: 0x04000AED RID: 2797
	public GameObject ActiveJob;

	// Token: 0x04000AEE RID: 2798
	public GameObject PreviewJob;

	// Token: 0x04000AEF RID: 2799
	public bool SetBadPart;

	// Token: 0x04000AF0 RID: 2800
	public int TryTimes;

	// Token: 0x04000AF1 RID: 2801
	public Job JobScript;

	// Token: 0x04000AF2 RID: 2802
	private Collider m_Collider;

	// Token: 0x04000AF3 RID: 2803
	public TMP_Text Description;

	// Token: 0x04000AF4 RID: 2804
	public TMP_Text DebugText;

	// Token: 0x04000AF5 RID: 2805
	public TMP_Text Boltsmissing;

	// Token: 0x04000AF6 RID: 2806
	public TMP_Text Weldsmissing;

	// Token: 0x04000AF7 RID: 2807
	public TMP_Text Fluidsmissing;

	// Token: 0x04000AF8 RID: 2808
	public TMP_Text Partsmissing;

	// Token: 0x04000AF9 RID: 2809
	public TMP_Text PartsDamaged;

	// Token: 0x04000AFA RID: 2810
	public TMP_Text PartsCrashed;

	// Token: 0x04000AFB RID: 2811
	public TMP_Text NotDone;

	// Token: 0x04000AFC RID: 2812
	public TMP_Text Reward;

	// Token: 0x04000AFD RID: 2813
	public TMP_Text TotalReward;

	// Token: 0x04000AFE RID: 2814
	public float Realreward;

	// Token: 0x04000AFF RID: 2815
	public float Penalty;

	// Token: 0x04000B00 RID: 2816
	public float CleanRatio;

	// Token: 0x04000B01 RID: 2817
	public float NoRustRatio;

	// Token: 0x04000B02 RID: 2818
	public float PaintRatio;

	// Token: 0x04000B03 RID: 2819
	public bool NoRuinedParts;

	// Token: 0x04000B04 RID: 2820
	public bool NoWornParts;

	// Token: 0x04000B05 RID: 2821
	public bool NoMeshDamagedParts;

	// Token: 0x04000B06 RID: 2822
	public float CleanRatioParts;

	// Token: 0x04000B07 RID: 2823
	public float NoRustRatioParts;

	// Token: 0x04000B08 RID: 2824
	public float PaintRatioParts;

	// Token: 0x04000B09 RID: 2825
	public int count;

	// Token: 0x04000B0A RID: 2826
	public GameObject JobParent;

	// Token: 0x04000B0B RID: 2827
	public bool started;

	// Token: 0x04000B0C RID: 2828
	public float PaintGoodParts;

	// Token: 0x04000B0D RID: 2829
	public float RustGoodParts;

	// Token: 0x04000B0E RID: 2830
	public CheckCondition CheckCondition;
}
