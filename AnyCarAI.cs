using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class AnyCarAI : MonoBehaviour
{
	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000021 RID: 33 RVA: 0x00002892 File Offset: 0x00000A92
	// (set) Token: 0x06000022 RID: 34 RVA: 0x0000289A File Offset: 0x00000A9A
	public float AccelInput { get; private set; }

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000023 RID: 35 RVA: 0x000028A3 File Offset: 0x00000AA3
	// (set) Token: 0x06000024 RID: 36 RVA: 0x000028AB File Offset: 0x00000AAB
	public float BrakeInput { get; private set; }

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000025 RID: 37 RVA: 0x000028B4 File Offset: 0x00000AB4
	// (set) Token: 0x06000026 RID: 38 RVA: 0x000028BC File Offset: 0x00000ABC
	public Rigidbody rb { get; set; }

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000027 RID: 39 RVA: 0x000028C5 File Offset: 0x00000AC5
	// (set) Token: 0x06000028 RID: 40 RVA: 0x000028CD File Offset: 0x00000ACD
	public float RPM { get; private set; }

	// Token: 0x06000029 RID: 41 RVA: 0x000028D8 File Offset: 0x00000AD8
	public void UnpackPrefab()
	{
		Transform parent = base.transform.parent;
		this.objToUnpack = parent.gameObject;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002900 File Offset: 0x00000B00
	public void CreateColliders()
	{
		this.frontLeft.AddComponent(typeof(SphereCollider));
		this.frontRight.AddComponent(typeof(SphereCollider));
		this.backLeft.AddComponent(typeof(SphereCollider));
		this.backRight.AddComponent(typeof(SphereCollider));
		this.frontLeftCol = new GameObject("FLCOL");
		this.frontRightCol = new GameObject("FRCOL");
		this.backLeftCol = new GameObject("BLCOL");
		this.backRightCol = new GameObject("BRCOL");
		this.frontLeftCol.transform.parent = base.transform.GetChild(0);
		this.frontLeftCol.transform.position = this.frontLeft.transform.position;
		this.frontLeftCol.transform.rotation = this.frontLeft.transform.rotation;
		this.frontRightCol.transform.parent = base.transform.GetChild(0);
		this.frontRightCol.transform.position = this.frontRight.transform.position;
		this.frontRightCol.transform.rotation = this.frontRight.transform.rotation;
		this.backLeftCol.transform.parent = base.transform.GetChild(0);
		this.backLeftCol.transform.position = this.backLeft.transform.position;
		this.backLeftCol.transform.rotation = this.backLeft.transform.rotation;
		this.backRightCol.transform.parent = base.transform.GetChild(0);
		this.backRightCol.transform.position = this.backRight.transform.position;
		this.backRightCol.transform.rotation = this.backRight.transform.rotation;
		this.frontLeftCol.AddComponent(typeof(WheelCollider));
		this.frontRightCol.AddComponent(typeof(WheelCollider));
		this.backLeftCol.AddComponent(typeof(WheelCollider));
		this.backRightCol.AddComponent(typeof(WheelCollider));
		this.frontLeftCol.AddComponent(typeof(WheelsFXAI));
		this.frontRightCol.AddComponent(typeof(WheelsFXAI));
		this.backLeftCol.AddComponent(typeof(WheelsFXAI));
		this.backRightCol.AddComponent(typeof(WheelsFXAI));
		this.FLRadius = this.frontLeft.GetComponent<SphereCollider>().radius * this.frontLeft.transform.lossyScale.x;
		this.FRRadius = this.frontRight.GetComponent<SphereCollider>().radius * this.frontLeft.transform.lossyScale.x;
		this.BLRadius = this.backLeft.GetComponent<SphereCollider>().radius * this.frontLeft.transform.lossyScale.x;
		this.BRRadius = this.backRight.GetComponent<SphereCollider>().radius * this.frontLeft.transform.lossyScale.x;
		this.frontLeftCol.GetComponent<WheelCollider>().radius = Mathf.Abs(this.FLRadius);
		this.frontRightCol.GetComponent<WheelCollider>().radius = Mathf.Abs(this.FRRadius);
		this.backLeftCol.GetComponent<WheelCollider>().radius = Mathf.Abs(this.BLRadius);
		this.backRightCol.GetComponent<WheelCollider>().radius = Mathf.Abs(this.BRRadius);
		UnityEngine.Object.DestroyImmediate(this.frontLeft.GetComponent<SphereCollider>());
		UnityEngine.Object.DestroyImmediate(this.frontRight.GetComponent<SphereCollider>());
		UnityEngine.Object.DestroyImmediate(this.backLeft.GetComponent<SphereCollider>());
		UnityEngine.Object.DestroyImmediate(this.backRight.GetComponent<SphereCollider>());
		if (this.bodyMesh != null)
		{
			this.bodyMesh.AddComponent(typeof(MeshCollider));
			this.bodyMesh.AddComponent(typeof(CompetitiveDrivingCheck));
			this.bodyMesh.GetComponent<MeshCollider>().convex = true;
		}
		this.extraWheelsColList.Clear();
		int num = 1;
		foreach (CarWheelsAI carWheelsAI in this.extraWheels)
		{
			carWheelsAI.model.AddComponent(typeof(SphereCollider));
			this.extraWheelCol.collider = new GameObject("extraWheel " + num.ToString());
			this.extraWheelCol.axel = carWheelsAI.axel;
			this.extraWheelCol.type = carWheelsAI.type;
			this.extraWheelCol.collider.transform.parent = base.transform.GetChild(0);
			this.extraWheelCol.collider.transform.position = carWheelsAI.model.transform.position;
			this.extraWheelCol.collider.transform.rotation = carWheelsAI.model.transform.rotation;
			this.extraWheelCol.collider.AddComponent(typeof(WheelCollider));
			this.extraWheelCol.collider.AddComponent(typeof(WheelsFXAI));
			this.extraWheelRadius = carWheelsAI.model.GetComponent<SphereCollider>().radius * carWheelsAI.model.transform.lossyScale.x;
			this.extraWheelCol.collider.GetComponent<WheelCollider>().radius = Mathf.Abs(this.extraWheelRadius);
			UnityEngine.Object.DestroyImmediate(carWheelsAI.model.GetComponent<SphereCollider>());
			this.extraWheelsColList.Add(this.extraWheelCol);
			num++;
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002F38 File Offset: 0x00001138
	public void CreateDebugBodyCol()
	{
		this.extraBodyCol = (UnityEngine.Object.Instantiate(Resources.Load("BodyCollider"), base.transform.position, Quaternion.identity) as GameObject);
		this.extraBodyCol.transform.parent = base.transform;
		this.extraBodyCol.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z);
		this.extraBodyCol.transform.rotation = base.transform.rotation;
		this.bodyMesh = this.extraBodyCol;
		this.bodyMesh.AddComponent(typeof(CompetitiveDrivingCheck));
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00003008 File Offset: 0x00001208
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.rb.mass = this.vehicleMass;
		this.centerOfMass = this.rb.centerOfMass;
		this.currentTorque = this.motorTorque - this.tractionControl * this.motorTorque;
		this.rearLights = base.transform.GetChild(1).GetChild(2).gameObject;
		this.rearLights.SetActive(false);
		this.smokeParticles = base.transform.root.GetComponentInChildren<ParticleSystem>();
		if (!this.smokeOn)
		{
			this.smokeParticles.Stop();
		}
		else
		{
			this.smokeParticles.Play();
		}
		this.carAIInputs = base.gameObject.AddComponent<CarAIInputs>();
		this.carAIWaypointTracker = base.gameObject.AddComponent<CarAIWaipointTracker>();
		base.gameObject.AddComponent<EngineAudioAI>();
		base.gameObject.AddComponent<DamageSystemAI>();
		this.carAItargetObj = new GameObject("WaypointsTarget");
		this.carAItargetObj.transform.parent = base.transform.GetChild(1);
		this.carAItarget = this.carAItargetObj.transform;
		if (this.exhaustFlame && this.exhaustObj == null)
		{
			this.exhaustObj = base.transform.GetChild(1).Find("ExhaustPipe(Clone)").gameObject;
			this.exhaustVisual = this.exhaustObj.GetComponent<ParticleSystem>();
			this.exhaustSoundSource = this.exhaustObj.GetComponent<AudioSource>();
			this.exhaustSoundSource.clip = this.exhaustSound;
			this.exhaustSoundSource.volume = this.exhaustVolume;
		}
		this.SetWheelsValues();
	}

	// Token: 0x0600002D RID: 45 RVA: 0x000031B4 File Offset: 0x000013B4
	private void Update()
	{
		SpeedTypeAI speedTypeAI = this.speedType;
		if (speedTypeAI != SpeedTypeAI.MPH)
		{
			if (speedTypeAI == SpeedTypeAI.KPH)
			{
				this.currentSpeed = this.rb.velocity.magnitude * 3.6f;
			}
		}
		else
		{
			this.currentSpeed = this.rb.velocity.magnitude * 2.2369363f;
		}
		this.isDrivingDebug();
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00003218 File Offset: 0x00001418
	public void Move(float steering, float Accel, float footbrake, float handbrake)
	{
		this.AnimateWheels();
		steering = Mathf.Clamp(steering, -1f, 1f);
		Accel = (this.AccelInput = Mathf.Clamp(Accel, 0f, 1f));
		footbrake = (this.BrakeInput = -1f * Mathf.Clamp(footbrake, -1f, 0f));
		handbrake = Mathf.Clamp(handbrake, 0f, 1f);
		this.CalculateRPM();
		this.AutoGearSystem();
		this.Steering(steering);
		this.SteerHelper();
		this.ApplyDrive(Accel, footbrake);
		this.MaxSpeedReached();
		this.HandBreaking(handbrake);
		this.AddDownForce();
		if (this.skidMarks)
		{
			this.CheckForWheelSpin();
		}
		this.TractionControl();
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000032D4 File Offset: 0x000014D4
	private void AnimateWheels()
	{
		Vector3 position;
		Quaternion quaternion;
		this.frontLeftCol.GetComponent<WheelCollider>().GetWorldPose(out position, out quaternion);
		quaternion *= Quaternion.Euler(this.wheelsRotation);
		this.frontLeft.transform.position = position;
		this.frontLeft.transform.rotation = quaternion;
		Vector3 position2;
		Quaternion quaternion2;
		this.frontRightCol.GetComponent<WheelCollider>().GetWorldPose(out position2, out quaternion2);
		quaternion2 *= Quaternion.Euler(this.wheelsRotation);
		this.frontRight.transform.position = position2;
		this.frontRight.transform.rotation = quaternion2;
		Vector3 position3;
		Quaternion quaternion3;
		this.backLeftCol.GetComponent<WheelCollider>().GetWorldPose(out position3, out quaternion3);
		quaternion3 *= Quaternion.Euler(this.wheelsRotation);
		this.backLeft.transform.position = position3;
		this.backLeft.transform.rotation = quaternion3;
		Vector3 position4;
		Quaternion quaternion4;
		this.backRightCol.GetComponent<WheelCollider>().GetWorldPose(out position4, out quaternion4);
		quaternion4 *= Quaternion.Euler(this.wheelsRotation);
		this.backRight.transform.position = position4;
		this.backRight.transform.rotation = quaternion4;
		int num = 0;
		foreach (CarWheelsColsAI carWheelsColsAI in this.extraWheelsColList)
		{
			Vector3 position5;
			Quaternion quaternion5;
			carWheelsColsAI.collider.GetComponent<WheelCollider>().GetWorldPose(out position5, out quaternion5);
			quaternion5 *= Quaternion.Euler(this.wheelsRotation);
			this.extraWheels[num].model.transform.position = position5;
			this.extraWheels[num].model.transform.rotation = quaternion5;
			num++;
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000034B4 File Offset: 0x000016B4
	private void Steering(float steering)
	{
		float b = steering * this.maximumSteerAngle;
		this.frontLeftCol.GetComponent<WheelCollider>().steerAngle = Mathf.Lerp(this.frontLeftCol.GetComponent<WheelCollider>().steerAngle, b, 0.5f);
		this.frontRightCol.GetComponent<WheelCollider>().steerAngle = Mathf.Lerp(this.frontRightCol.GetComponent<WheelCollider>().steerAngle, b, 0.5f);
		foreach (CarWheelsColsAI carWheelsColsAI in this.extraWheelsColList)
		{
			if (carWheelsColsAI.axel == AxlAI.Front)
			{
				carWheelsColsAI.collider.GetComponent<WheelCollider>().steerAngle = Mathf.Lerp(carWheelsColsAI.collider.GetComponent<WheelCollider>().steerAngle, b, 0.5f);
			}
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003594 File Offset: 0x00001794
	private void SteerHelper()
	{
		WheelHit wheelHit;
		this.frontLeftCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit);
		WheelHit wheelHit2;
		this.frontRightCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit2);
		WheelHit wheelHit3;
		this.backLeftCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit3);
		WheelHit wheelHit4;
		this.backRightCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit4);
		if (wheelHit.normal == Vector3.zero)
		{
			return;
		}
		if (wheelHit2.normal == Vector3.zero)
		{
			return;
		}
		if (wheelHit3.normal == Vector3.zero)
		{
			return;
		}
		if (wheelHit4.normal == Vector3.zero)
		{
			return;
		}
		int num = 0;
		foreach (CarWheelsColsAI carWheelsColsAI in this.extraWheelsColList)
		{
			WheelHit wheelHit5;
			carWheelsColsAI.collider.GetComponent<WheelCollider>().GetGroundHit(out wheelHit5);
			if (wheelHit5.normal == Vector3.zero)
			{
				return;
			}
			num++;
		}
		if (Mathf.Abs(this.oldRotation - base.transform.eulerAngles.y) < 10f)
		{
			Quaternion rotation = Quaternion.AngleAxis((base.transform.eulerAngles.y - this.oldRotation) * this.steerHelper, Vector3.up);
			this.rb.velocity = rotation * this.rb.velocity;
		}
		this.oldRotation = base.transform.eulerAngles.y;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003730 File Offset: 0x00001930
	private void ApplyDrive(float Accel, float footbrake)
	{
		float num;
		switch (this.carDriveType)
		{
		case CarDriveTypeAI.FrontWheelDrive:
			break;
		case CarDriveTypeAI.RearWheelDrive:
			goto IL_15D;
		case CarDriveTypeAI.FourWheelDrive:
			num = Accel * (this.currentTorque / 4f) * this.enginePower.Evaluate(1f);
			this.frontLeftCol.GetComponent<WheelCollider>().motorTorque = num;
			this.frontRightCol.GetComponent<WheelCollider>().motorTorque = num;
			this.backLeftCol.GetComponent<WheelCollider>().motorTorque = num;
			this.backRightCol.GetComponent<WheelCollider>().motorTorque = num;
			using (List<CarWheelsColsAI>.Enumerator enumerator = this.extraWheelsColList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CarWheelsColsAI carWheelsColsAI = enumerator.Current;
					if (carWheelsColsAI.type == TypeAI.Drive)
					{
						carWheelsColsAI.collider.GetComponent<WheelCollider>().motorTorque = num;
					}
				}
				goto IL_1EA;
			}
			break;
		default:
			goto IL_1EA;
		}
		num = Accel * (this.currentTorque / 2f) * this.enginePower.Evaluate(1f);
		this.frontLeftCol.GetComponent<WheelCollider>().motorTorque = num;
		this.frontRightCol.GetComponent<WheelCollider>().motorTorque = num;
		using (List<CarWheelsColsAI>.Enumerator enumerator = this.extraWheelsColList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CarWheelsColsAI carWheelsColsAI2 = enumerator.Current;
				if (carWheelsColsAI2.type == TypeAI.Drive)
				{
					carWheelsColsAI2.collider.GetComponent<WheelCollider>().motorTorque = num;
				}
			}
			goto IL_1EA;
		}
		IL_15D:
		num = Accel * (this.currentTorque / 2f) * this.enginePower.Evaluate(1f);
		this.backLeftCol.GetComponent<WheelCollider>().motorTorque = num;
		this.backRightCol.GetComponent<WheelCollider>().motorTorque = num;
		foreach (CarWheelsColsAI carWheelsColsAI3 in this.extraWheelsColList)
		{
			if (carWheelsColsAI3.type == TypeAI.Drive)
			{
				carWheelsColsAI3.collider.GetComponent<WheelCollider>().motorTorque = num;
			}
		}
		IL_1EA:
		if (footbrake > 0f)
		{
			if (this.currentSpeed > 5f && Vector3.Angle(base.transform.forward, this.rb.velocity) < 50f)
			{
				this.reverseGearOn = false;
			}
			else
			{
				this.reverseGearOn = true;
			}
		}
		else
		{
			this.reverseGearOn = false;
		}
		if (!this.ABS)
		{
			if (this.currentSpeed > 5f && Vector3.Angle(base.transform.forward, this.rb.velocity) < 50f)
			{
				this.frontLeftCol.GetComponent<WheelCollider>().brakeTorque = this.brakeTorque * footbrake;
				this.frontRightCol.GetComponent<WheelCollider>().brakeTorque = this.brakeTorque * footbrake;
				this.backLeftCol.GetComponent<WheelCollider>().brakeTorque = this.brakeTorque * footbrake;
				this.backRightCol.GetComponent<WheelCollider>().brakeTorque = this.brakeTorque * footbrake;
				foreach (CarWheelsColsAI carWheelsColsAI4 in this.extraWheelsColList)
				{
					carWheelsColsAI4.collider.GetComponent<WheelCollider>().brakeTorque = this.brakeTorque * footbrake;
				}
				if (footbrake > 0f)
				{
					this.rearLights.SetActive(true);
					return;
				}
				this.rearLights.SetActive(false);
				return;
			}
			else
			{
				if (footbrake <= 0f)
				{
					return;
				}
				this.rearLights.SetActive(false);
				this.frontLeftCol.GetComponent<WheelCollider>().brakeTorque = 0f;
				this.frontRightCol.GetComponent<WheelCollider>().brakeTorque = 0f;
				this.backLeftCol.GetComponent<WheelCollider>().brakeTorque = 0f;
				this.backRightCol.GetComponent<WheelCollider>().brakeTorque = 0f;
				this.frontLeftCol.GetComponent<WheelCollider>().motorTorque = -this.reverseTorque * footbrake;
				this.frontRightCol.GetComponent<WheelCollider>().motorTorque = -this.reverseTorque * footbrake;
				this.backLeftCol.GetComponent<WheelCollider>().motorTorque = -this.reverseTorque * footbrake;
				this.backRightCol.GetComponent<WheelCollider>().motorTorque = -this.reverseTorque * footbrake;
				using (List<CarWheelsColsAI>.Enumerator enumerator = this.extraWheelsColList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CarWheelsColsAI carWheelsColsAI5 = enumerator.Current;
						carWheelsColsAI5.collider.GetComponent<WheelCollider>().brakeTorque = 0f;
						carWheelsColsAI5.collider.GetComponent<WheelCollider>().motorTorque = -this.reverseTorque * footbrake;
					}
					return;
				}
			}
		}
		if (this.currentSpeed > 5f && Vector3.Angle(base.transform.forward, this.rb.velocity) < 50f)
		{
			base.StartCoroutine(this.ABSCoroutine(footbrake));
			if (footbrake > 0f)
			{
				this.rearLights.SetActive(true);
				return;
			}
			this.rearLights.SetActive(false);
			return;
		}
		else if (footbrake > 0f)
		{
			this.rearLights.SetActive(false);
			this.frontLeftCol.GetComponent<WheelCollider>().brakeTorque = 0f;
			this.frontRightCol.GetComponent<WheelCollider>().brakeTorque = 0f;
			this.backLeftCol.GetComponent<WheelCollider>().brakeTorque = 0f;
			this.backRightCol.GetComponent<WheelCollider>().brakeTorque = 0f;
			this.frontLeftCol.GetComponent<WheelCollider>().motorTorque = -this.reverseTorque * footbrake;
			this.frontRightCol.GetComponent<WheelCollider>().motorTorque = -this.reverseTorque * footbrake;
			this.backLeftCol.GetComponent<WheelCollider>().motorTorque = -this.reverseTorque * footbrake;
			this.backRightCol.GetComponent<WheelCollider>().motorTorque = -this.reverseTorque * footbrake;
			foreach (CarWheelsColsAI carWheelsColsAI6 in this.extraWheelsColList)
			{
				carWheelsColsAI6.collider.GetComponent<WheelCollider>().brakeTorque = 0f;
				carWheelsColsAI6.collider.GetComponent<WheelCollider>().motorTorque = -this.reverseTorque * footbrake;
			}
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00003D80 File Offset: 0x00001F80
	private void MaxSpeedReached()
	{
		SpeedTypeAI speedTypeAI = this.speedType;
		if (speedTypeAI != SpeedTypeAI.MPH)
		{
			if (speedTypeAI != SpeedTypeAI.KPH)
			{
				return;
			}
			if (this.currentSpeed > this.maxSpeed)
			{
				this.rb.velocity = this.maxSpeed / 3.6f * this.rb.velocity.normalized;
			}
		}
		else if (this.currentSpeed > this.maxSpeed)
		{
			this.rb.velocity = this.maxSpeed / 2.2369363f * this.rb.velocity.normalized;
			return;
		}
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00003E18 File Offset: 0x00002018
	private void HandBreaking(float handbrake)
	{
		if (handbrake > 0f)
		{
			float num = handbrake * this.handbrakeTorque;
			this.backLeftCol.GetComponent<WheelCollider>().brakeTorque = num;
			this.backRightCol.GetComponent<WheelCollider>().brakeTorque = num;
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003E58 File Offset: 0x00002058
	private IEnumerator ABSCoroutine(float footbrake)
	{
		this.frontLeftCol.GetComponent<WheelCollider>().brakeTorque = this.brakeTorque * footbrake;
		this.frontRightCol.GetComponent<WheelCollider>().brakeTorque = this.brakeTorque * footbrake;
		this.backLeftCol.GetComponent<WheelCollider>().brakeTorque = this.brakeTorque * footbrake;
		this.backRightCol.GetComponent<WheelCollider>().brakeTorque = this.brakeTorque * footbrake;
		foreach (CarWheelsColsAI carWheelsColsAI in this.extraWheelsColList)
		{
			carWheelsColsAI.collider.GetComponent<WheelCollider>().brakeTorque = this.brakeTorque * footbrake;
		}
		yield return new WaitForSeconds(0.1f);
		this.frontLeftCol.GetComponent<WheelCollider>().brakeTorque = 0f;
		this.frontRightCol.GetComponent<WheelCollider>().brakeTorque = 0f;
		this.backLeftCol.GetComponent<WheelCollider>().brakeTorque = 0f;
		this.backRightCol.GetComponent<WheelCollider>().brakeTorque = 0f;
		foreach (CarWheelsColsAI carWheelsColsAI2 in this.extraWheelsColList)
		{
			carWheelsColsAI2.collider.GetComponent<WheelCollider>().brakeTorque = 0f;
		}
		yield return new WaitForSeconds(0.1f);
		yield break;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00003E70 File Offset: 0x00002070
	private void isDrivingDebug()
	{
		if (this.rb.velocity.magnitude < 1f && this.isDriving)
		{
			if (this.carAIInputs.reverseGearOn)
			{
				this.rb.AddRelativeForce(0f, 0f, -this.vehicleMass * 1000f * Time.deltaTime);
				return;
			}
			this.rb.AddRelativeForce(0f, 0f, this.vehicleMass * 1000f * Time.deltaTime);
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00003EFC File Offset: 0x000020FC
	private void SetWheelsValues()
	{
		this.frontLeftCol.GetComponent<WheelCollider>().mass = this.wheelsMass;
		this.frontRightCol.GetComponent<WheelCollider>().mass = this.wheelsMass;
		this.backLeftCol.GetComponent<WheelCollider>().mass = this.wheelsMass;
		this.backRightCol.GetComponent<WheelCollider>().mass = this.wheelsMass;
		this.frontLeftCol.GetComponent<WheelCollider>().radius *= this.wheelsRadius;
		this.frontRightCol.GetComponent<WheelCollider>().radius *= this.wheelsRadius;
		this.backLeftCol.GetComponent<WheelCollider>().radius *= this.wheelsRadius;
		this.backRightCol.GetComponent<WheelCollider>().radius *= this.wheelsRadius;
		this.frontLeftCol.GetComponent<WheelCollider>().forceAppPointDistance = this.forcePoint;
		this.frontRightCol.GetComponent<WheelCollider>().forceAppPointDistance = this.forcePoint;
		this.backLeftCol.GetComponent<WheelCollider>().forceAppPointDistance = this.forcePoint;
		this.backRightCol.GetComponent<WheelCollider>().forceAppPointDistance = this.forcePoint;
		this.frontLeftCol.GetComponent<WheelCollider>().wheelDampingRate = this.dumpingRate;
		this.frontRightCol.GetComponent<WheelCollider>().wheelDampingRate = this.dumpingRate;
		this.backLeftCol.GetComponent<WheelCollider>().wheelDampingRate = this.dumpingRate;
		this.backRightCol.GetComponent<WheelCollider>().wheelDampingRate = this.dumpingRate;
		this.frontLeftCol.GetComponent<WheelCollider>().suspensionDistance = this.suspensionDistance;
		this.frontRightCol.GetComponent<WheelCollider>().suspensionDistance = this.suspensionDistance;
		this.backLeftCol.GetComponent<WheelCollider>().suspensionDistance = this.suspensionDistance;
		this.backRightCol.GetComponent<WheelCollider>().suspensionDistance = this.suspensionDistance;
		this.frontLeftCol.GetComponent<WheelCollider>().center = this.wheelsPosition;
		this.frontRightCol.GetComponent<WheelCollider>().center = this.wheelsPosition;
		this.backLeftCol.GetComponent<WheelCollider>().center = this.wheelsPosition;
		this.backRightCol.GetComponent<WheelCollider>().center = this.wheelsPosition;
		JointSpring jointSpring = this.frontLeftCol.GetComponent<WheelCollider>().suspensionSpring;
		JointSpring jointSpring2 = this.frontRightCol.GetComponent<WheelCollider>().suspensionSpring;
		JointSpring jointSpring3 = this.backLeftCol.GetComponent<WheelCollider>().suspensionSpring;
		JointSpring jointSpring4 = this.backRightCol.GetComponent<WheelCollider>().suspensionSpring;
		jointSpring.spring = this.suspensionSpring;
		jointSpring2.spring = this.suspensionSpring;
		jointSpring3.spring = this.suspensionSpring;
		jointSpring4.spring = this.suspensionSpring;
		jointSpring.damper = this.suspensionDamper;
		jointSpring2.damper = this.suspensionDamper;
		jointSpring3.damper = this.suspensionDamper;
		jointSpring4.damper = this.suspensionDamper;
		jointSpring.targetPosition = this.targetPosition;
		jointSpring2.targetPosition = this.targetPosition;
		jointSpring3.targetPosition = this.targetPosition;
		jointSpring4.targetPosition = this.targetPosition;
		this.frontLeftCol.GetComponent<WheelCollider>().suspensionSpring = jointSpring;
		this.frontRightCol.GetComponent<WheelCollider>().suspensionSpring = jointSpring2;
		this.backLeftCol.GetComponent<WheelCollider>().suspensionSpring = jointSpring3;
		this.backRightCol.GetComponent<WheelCollider>().suspensionSpring = jointSpring4;
		WheelFrictionCurve sidewaysFriction = this.frontLeftCol.GetComponent<WheelCollider>().sidewaysFriction;
		WheelFrictionCurve sidewaysFriction2 = this.frontRightCol.GetComponent<WheelCollider>().sidewaysFriction;
		WheelFrictionCurve sidewaysFriction3 = this.backLeftCol.GetComponent<WheelCollider>().sidewaysFriction;
		WheelFrictionCurve sidewaysFriction4 = this.backRightCol.GetComponent<WheelCollider>().sidewaysFriction;
		sidewaysFriction.stiffness = this.wheelStiffness;
		sidewaysFriction2.stiffness = this.wheelStiffness;
		sidewaysFriction3.stiffness = this.wheelStiffness;
		sidewaysFriction4.stiffness = this.wheelStiffness;
		this.frontLeftCol.GetComponent<WheelCollider>().sidewaysFriction = sidewaysFriction;
		this.frontRightCol.GetComponent<WheelCollider>().sidewaysFriction = sidewaysFriction2;
		this.backLeftCol.GetComponent<WheelCollider>().sidewaysFriction = sidewaysFriction3;
		this.backRightCol.GetComponent<WheelCollider>().sidewaysFriction = sidewaysFriction4;
		WheelFrictionCurve forwardFriction = this.frontLeftCol.GetComponent<WheelCollider>().forwardFriction;
		WheelFrictionCurve forwardFriction2 = this.frontRightCol.GetComponent<WheelCollider>().forwardFriction;
		WheelFrictionCurve forwardFriction3 = this.backLeftCol.GetComponent<WheelCollider>().forwardFriction;
		WheelFrictionCurve forwardFriction4 = this.backRightCol.GetComponent<WheelCollider>().forwardFriction;
		forwardFriction.stiffness = this.wheelStiffness;
		forwardFriction2.stiffness = this.wheelStiffness;
		forwardFriction3.stiffness = this.wheelStiffness;
		forwardFriction4.stiffness = this.wheelStiffness;
		this.frontLeftCol.GetComponent<WheelCollider>().forwardFriction = forwardFriction;
		this.frontRightCol.GetComponent<WheelCollider>().forwardFriction = forwardFriction2;
		this.backLeftCol.GetComponent<WheelCollider>().forwardFriction = forwardFriction3;
		this.backRightCol.GetComponent<WheelCollider>().forwardFriction = forwardFriction4;
		foreach (CarWheelsColsAI carWheelsColsAI in this.extraWheelsColList)
		{
			carWheelsColsAI.collider.GetComponent<WheelCollider>().mass = this.wheelsMass;
			carWheelsColsAI.collider.GetComponent<WheelCollider>().radius = this.extraWheelRadius * this.wheelsRadius;
			carWheelsColsAI.collider.GetComponent<WheelCollider>().forceAppPointDistance = this.forcePoint;
			carWheelsColsAI.collider.GetComponent<WheelCollider>().wheelDampingRate = this.dumpingRate;
			carWheelsColsAI.collider.GetComponent<WheelCollider>().suspensionDistance = this.suspensionDistance;
			carWheelsColsAI.collider.GetComponent<WheelCollider>().center = this.wheelsPosition;
			JointSpring jointSpring5 = carWheelsColsAI.collider.GetComponent<WheelCollider>().suspensionSpring;
			jointSpring5.spring = this.suspensionSpring;
			jointSpring5.damper = this.suspensionDamper;
			jointSpring5.targetPosition = this.targetPosition;
			carWheelsColsAI.collider.GetComponent<WheelCollider>().suspensionSpring = jointSpring5;
			WheelFrictionCurve sidewaysFriction5 = carWheelsColsAI.collider.GetComponent<WheelCollider>().sidewaysFriction;
			sidewaysFriction5.stiffness = this.wheelStiffness;
			carWheelsColsAI.collider.GetComponent<WheelCollider>().sidewaysFriction = sidewaysFriction5;
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00004538 File Offset: 0x00002738
	private void AutoGearSystem()
	{
		float num = Mathf.Abs(this.currentSpeed / this.maxSpeed);
		float num2 = 1f / (float)this.numberOfGears * (float)(this.currentGear + 1);
		float num3 = 1f / (float)this.numberOfGears * (float)this.currentGear;
		if (this.currentGear > 0 && num < num3)
		{
			this.currentGear--;
		}
		if (num > num2 && this.currentGear < this.numberOfGears - 1 && !this.reverseGearOn)
		{
			if (this.exhaustFlame)
			{
				this.ExhaustFX();
			}
			this.currentGear++;
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x000045D9 File Offset: 0x000027D9
	private static float BiasCurve(float factor)
	{
		return 1f - (1f - factor) * (1f - factor);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x000045F0 File Offset: 0x000027F0
	private static float SmoothLerp(float from, float to, float value)
	{
		return (1f - value) * from + value * to;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00004600 File Offset: 0x00002800
	private void CalculateGearFactor()
	{
		float num = 1f / (float)this.numberOfGears;
		float b = Mathf.InverseLerp(num * (float)this.currentGear, num * (float)(this.currentGear + 1), Mathf.Abs(this.currentSpeed / this.maxSpeed));
		this.gearFactor = Mathf.Lerp(this.gearFactor, b, Time.deltaTime * 5f);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00004664 File Offset: 0x00002864
	private void CalculateRPM()
	{
		this.CalculateGearFactor();
		float num = (float)this.currentGear / (float)this.numberOfGears;
		float from = AnyCarAI.SmoothLerp(0f, this.rpmRange, AnyCarAI.BiasCurve(num));
		float to = AnyCarAI.SmoothLerp(this.rpmRange, 1f, num);
		this.RPM = AnyCarAI.SmoothLerp(from, to, this.gearFactor);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000046C4 File Offset: 0x000028C4
	private void AddDownForce()
	{
		this.rb.AddForce(-base.transform.up * this.downForce * this.rb.velocity.magnitude);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00004710 File Offset: 0x00002910
	private void TractionControl()
	{
		WheelHit wheelHit;
		WheelHit wheelHit2;
		WheelHit wheelHit3;
		WheelHit wheelHit4;
		switch (this.carDriveType)
		{
		case CarDriveTypeAI.FrontWheelDrive:
			goto IL_18C;
		case CarDriveTypeAI.RearWheelDrive:
			break;
		case CarDriveTypeAI.FourWheelDrive:
			this.frontLeftCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit);
			this.AdjustTorque(wheelHit.forwardSlip);
			this.frontRightCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit2);
			this.AdjustTorque(wheelHit2.forwardSlip);
			this.backLeftCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit3);
			this.AdjustTorque(wheelHit3.forwardSlip);
			this.backRightCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit4);
			this.AdjustTorque(wheelHit4.forwardSlip);
			using (List<CarWheelsColsAI>.Enumerator enumerator = this.extraWheelsColList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CarWheelsColsAI carWheelsColsAI = enumerator.Current;
					WheelHit wheelHit5;
					carWheelsColsAI.collider.GetComponent<WheelCollider>().GetGroundHit(out wheelHit5);
					this.AdjustTorque(wheelHit5.forwardSlip);
				}
				return;
			}
			break;
		default:
			return;
		}
		this.backLeftCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit3);
		this.AdjustTorque(wheelHit3.forwardSlip);
		this.backRightCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit4);
		this.AdjustTorque(wheelHit4.forwardSlip);
		using (List<CarWheelsColsAI>.Enumerator enumerator = this.extraWheelsColList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CarWheelsColsAI carWheelsColsAI2 = enumerator.Current;
				if (carWheelsColsAI2.axel == AxlAI.Rear)
				{
					WheelHit wheelHit5;
					carWheelsColsAI2.collider.GetComponent<WheelCollider>().GetGroundHit(out wheelHit5);
					this.AdjustTorque(wheelHit5.forwardSlip);
				}
			}
			return;
		}
		IL_18C:
		this.frontLeftCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit);
		this.AdjustTorque(wheelHit.forwardSlip);
		this.frontRightCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit2);
		this.AdjustTorque(wheelHit2.forwardSlip);
		foreach (CarWheelsColsAI carWheelsColsAI3 in this.extraWheelsColList)
		{
			if (carWheelsColsAI3.axel == AxlAI.Front)
			{
				WheelHit wheelHit5;
				carWheelsColsAI3.collider.GetComponent<WheelCollider>().GetGroundHit(out wheelHit5);
				this.AdjustTorque(wheelHit5.forwardSlip);
			}
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0000496C File Offset: 0x00002B6C
	private void AdjustTorque(float forwardSlip)
	{
		if (forwardSlip >= this.slipLimit && this.currentTorque >= 0f)
		{
			this.currentTorque -= 10f * this.tractionControl;
			return;
		}
		this.currentTorque += 10f * this.tractionControl;
		if (this.currentTorque > this.motorTorque)
		{
			this.currentTorque = this.motorTorque;
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000049DC File Offset: 0x00002BDC
	private void CheckForWheelSpin()
	{
		WheelHit wheelHit;
		this.frontLeftCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit);
		WheelHit wheelHit2;
		this.frontRightCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit2);
		WheelHit wheelHit3;
		this.backLeftCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit3);
		WheelHit wheelHit4;
		this.backRightCol.GetComponent<WheelCollider>().GetGroundHit(out wheelHit4);
		if (Mathf.Abs(wheelHit.forwardSlip) >= this.slipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= this.slipLimit)
		{
			this.frontLeftCol.GetComponent<WheelsFXAI>().EmitTyreSmoke();
			if (!this.AnySkidSoundPlaying())
			{
				this.frontLeftCol.GetComponent<WheelsFXAI>().PlayAudio();
			}
		}
		else
		{
			if (this.frontLeftCol.GetComponent<WheelsFXAI>().playingAudio)
			{
				this.frontLeftCol.GetComponent<WheelsFXAI>().StopAudio();
			}
			this.frontLeftCol.GetComponent<WheelsFXAI>().EndSkidTrail();
		}
		if (Mathf.Abs(wheelHit2.forwardSlip) >= this.slipLimit || Mathf.Abs(wheelHit2.sidewaysSlip) >= this.slipLimit)
		{
			this.frontRightCol.GetComponent<WheelsFXAI>().EmitTyreSmoke();
			if (!this.AnySkidSoundPlaying())
			{
				this.frontRightCol.GetComponent<WheelsFXAI>().PlayAudio();
			}
		}
		else
		{
			if (this.frontRightCol.GetComponent<WheelsFXAI>().playingAudio)
			{
				this.frontRightCol.GetComponent<WheelsFXAI>().StopAudio();
			}
			this.frontRightCol.GetComponent<WheelsFXAI>().EndSkidTrail();
		}
		if (Mathf.Abs(wheelHit3.forwardSlip) >= this.slipLimit || Mathf.Abs(wheelHit3.sidewaysSlip) >= this.slipLimit)
		{
			this.backLeftCol.GetComponent<WheelsFXAI>().EmitTyreSmoke();
			if (!this.AnySkidSoundPlaying())
			{
				this.backLeftCol.GetComponent<WheelsFXAI>().PlayAudio();
			}
		}
		else
		{
			if (this.backLeftCol.GetComponent<WheelsFXAI>().playingAudio)
			{
				this.backLeftCol.GetComponent<WheelsFXAI>().StopAudio();
			}
			this.backLeftCol.GetComponent<WheelsFXAI>().EndSkidTrail();
		}
		if (Mathf.Abs(wheelHit4.forwardSlip) >= this.slipLimit || Mathf.Abs(wheelHit4.sidewaysSlip) >= this.slipLimit)
		{
			this.backRightCol.GetComponent<WheelsFXAI>().EmitTyreSmoke();
			if (!this.AnySkidSoundPlaying())
			{
				this.backRightCol.GetComponent<WheelsFXAI>().PlayAudio();
			}
		}
		else
		{
			if (this.backRightCol.GetComponent<WheelsFXAI>().playingAudio)
			{
				this.backRightCol.GetComponent<WheelsFXAI>().StopAudio();
			}
			this.backRightCol.GetComponent<WheelsFXAI>().EndSkidTrail();
		}
		foreach (CarWheelsColsAI carWheelsColsAI in this.extraWheelsColList)
		{
			WheelHit wheelHit5;
			carWheelsColsAI.collider.GetComponent<WheelCollider>().GetGroundHit(out wheelHit5);
			if (Mathf.Abs(wheelHit5.forwardSlip) >= this.slipLimit || Mathf.Abs(wheelHit5.sidewaysSlip) >= this.slipLimit)
			{
				carWheelsColsAI.collider.GetComponent<WheelsFXAI>().EmitTyreSmoke();
				if (!this.AnySkidSoundPlaying())
				{
					carWheelsColsAI.collider.GetComponent<WheelsFXAI>().PlayAudio();
				}
			}
			else
			{
				if (carWheelsColsAI.collider.GetComponent<WheelsFXAI>().playingAudio)
				{
					carWheelsColsAI.collider.GetComponent<WheelsFXAI>().StopAudio();
				}
				carWheelsColsAI.collider.GetComponent<WheelsFXAI>().EndSkidTrail();
			}
		}
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00004D2C File Offset: 0x00002F2C
	private bool AnySkidSoundPlaying()
	{
		if (this.frontLeftCol.GetComponent<WheelsFXAI>().playingAudio)
		{
			return true;
		}
		if (this.frontRightCol.GetComponent<WheelsFXAI>().playingAudio)
		{
			return true;
		}
		if (this.backLeftCol.GetComponent<WheelsFXAI>().playingAudio)
		{
			return true;
		}
		if (this.backRightCol.GetComponent<WheelsFXAI>().playingAudio)
		{
			return true;
		}
		using (List<CarWheelsColsAI>.Enumerator enumerator = this.extraWheelsColList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.collider.GetComponent<WheelsFXAI>().playingAudio)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00004DE0 File Offset: 0x00002FE0
	public void ExhaustFX()
	{
		if (this.exhaustObj != null)
		{
			this.exhaustSoundSource.Play();
			this.exhaustVisual.Play();
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00004E06 File Offset: 0x00003006
	public void CreateExhaustGameObj()
	{
		this.exhaustObjectPrefab = Resources.Load<Transform>("ExhaustPipe");
		this.exhaustObject = UnityEngine.Object.Instantiate<Transform>(this.exhaustObjectPrefab);
		this.exhaustObject.transform.parent = base.transform.GetChild(1);
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00004E45 File Offset: 0x00003045
	public void DestroyExhaustGameObj()
	{
		this.exhaustObj = base.transform.GetChild(1).Find("ExhaustPipe(Clone)").gameObject;
		UnityEngine.Object.DestroyImmediate(this.exhaustObj, true);
	}

	// Token: 0x0400002C RID: 44
	public int toolbarTab;

	// Token: 0x0400002D RID: 45
	public string currentTab;

	// Token: 0x0400002E RID: 46
	public List<CarWheelsAI> extraWheels;

	// Token: 0x0400002F RID: 47
	public List<CarWheelsColsAI> extraWheelsColList = new List<CarWheelsColsAI>();

	// Token: 0x04000030 RID: 48
	public CarWheelsColsAI extraWheelCol;

	// Token: 0x04000031 RID: 49
	public GameObject frontLeft;

	// Token: 0x04000032 RID: 50
	public GameObject frontRight;

	// Token: 0x04000033 RID: 51
	public GameObject backLeft;

	// Token: 0x04000034 RID: 52
	public GameObject backRight;

	// Token: 0x04000035 RID: 53
	public GameObject frontLeftCol;

	// Token: 0x04000036 RID: 54
	public GameObject frontRightCol;

	// Token: 0x04000037 RID: 55
	public GameObject backLeftCol;

	// Token: 0x04000038 RID: 56
	public GameObject backRightCol;

	// Token: 0x04000039 RID: 57
	public float extraWheelRadius;

	// Token: 0x0400003A RID: 58
	private float FLRadius;

	// Token: 0x0400003B RID: 59
	private float FRRadius;

	// Token: 0x0400003C RID: 60
	private float BLRadius;

	// Token: 0x0400003D RID: 61
	private float BRRadius;

	// Token: 0x0400003E RID: 62
	public float wheelsMass = 20f;

	// Token: 0x0400003F RID: 63
	public float forcePoint;

	// Token: 0x04000040 RID: 64
	public float dumpingRate = 0.025f;

	// Token: 0x04000041 RID: 65
	public float suspensionDistance = 0.2f;

	// Token: 0x04000042 RID: 66
	public Vector3 wheelsPosition;

	// Token: 0x04000043 RID: 67
	public Vector3 wheelsRotation;

	// Token: 0x04000044 RID: 68
	[Range(0.1f, 1f)]
	public float wheelStiffness = 1f;

	// Token: 0x04000045 RID: 69
	public float suspensionSpring = 70000f;

	// Token: 0x04000046 RID: 70
	public float suspensionDamper = 3500f;

	// Token: 0x04000047 RID: 71
	[Range(0.1f, 1f)]
	public float targetPosition = 0.5f;

	// Token: 0x04000048 RID: 72
	[Range(0.5f, 2f)]
	public float wheelsRadius = 1f;

	// Token: 0x04000049 RID: 73
	public GameObject bodyMesh;

	// Token: 0x0400004A RID: 74
	public GameObject extraBodyCol;

	// Token: 0x0400004D RID: 77
	public AnimationCurve enginePower;

	// Token: 0x0400004E RID: 78
	public float maximumSteerAngle;

	// Token: 0x0400004F RID: 79
	[Range(0f, 1f)]
	public float steerHelper;

	// Token: 0x04000050 RID: 80
	[Range(0f, 0.5f)]
	public float tractionControl;

	// Token: 0x04000051 RID: 81
	[Range(0f, 0.5f)]
	public float slipLimit = 0.3f;

	// Token: 0x04000052 RID: 82
	[SerializeField]
	public CarDriveTypeAI carDriveType = CarDriveTypeAI.FourWheelDrive;

	// Token: 0x04000053 RID: 83
	[SerializeField]
	public SpeedTypeAI speedType = SpeedTypeAI.KPH;

	// Token: 0x04000054 RID: 84
	public Vector3 centerOfMass;

	// Token: 0x04000055 RID: 85
	public float vehicleMass = 1000f;

	// Token: 0x04000056 RID: 86
	public float motorTorque = 2500f;

	// Token: 0x04000057 RID: 87
	public float brakeTorque = 20000f;

	// Token: 0x04000058 RID: 88
	public float reverseTorque = 500f;

	// Token: 0x04000059 RID: 89
	public float handbrakeTorque = 10000000f;

	// Token: 0x0400005A RID: 90
	public float maxSpeed = 200f;

	// Token: 0x0400005B RID: 91
	public int numberOfGears = 5;

	// Token: 0x0400005C RID: 92
	public float downForce = 300f;

	// Token: 0x0400005D RID: 93
	public bool ABS = true;

	// Token: 0x0400005E RID: 94
	public bool skidMarks = true;

	// Token: 0x0400005F RID: 95
	public bool smokeOn;

	// Token: 0x04000060 RID: 96
	private ParticleSystem smokeParticles;

	// Token: 0x04000061 RID: 97
	public bool turboON;

	// Token: 0x04000062 RID: 98
	public AudioClip turboAudioClip;

	// Token: 0x04000063 RID: 99
	[Range(0f, 1f)]
	public float turboVolume = 0.5f;

	// Token: 0x04000064 RID: 100
	public Transform exhaustObjectPrefab;

	// Token: 0x04000065 RID: 101
	public Transform exhaustObject;

	// Token: 0x04000066 RID: 102
	public GameObject exhaustObj;

	// Token: 0x04000067 RID: 103
	public bool exhaustFlame;

	// Token: 0x04000068 RID: 104
	public ParticleSystem exhaustVisual;

	// Token: 0x04000069 RID: 105
	public AudioSource exhaustSoundSource;

	// Token: 0x0400006A RID: 106
	public AudioClip exhaustSound;

	// Token: 0x0400006B RID: 107
	[Range(0.01f, 1f)]
	public float exhaustVolume;

	// Token: 0x0400006C RID: 108
	private float oldRotation;

	// Token: 0x0400006E RID: 110
	public float currentSpeed;

	// Token: 0x0400006F RID: 111
	private float currentTorque;

	// Token: 0x04000070 RID: 112
	private float rpmRange = 1f;

	// Token: 0x04000071 RID: 113
	public int currentGear;

	// Token: 0x04000072 RID: 114
	private float gearFactor;

	// Token: 0x04000073 RID: 115
	public bool reverseGearOn;

	// Token: 0x04000075 RID: 117
	public GameObject objToUnpack;

	// Token: 0x04000076 RID: 118
	public GameObject frontLights;

	// Token: 0x04000077 RID: 119
	public GameObject rearLights;

	// Token: 0x04000078 RID: 120
	public bool collisionSystem;

	// Token: 0x04000079 RID: 121
	public AudioClip collisionSound;

	// Token: 0x0400007A RID: 122
	[Range(0.01f, 1f)]
	public float collisionVolume;

	// Token: 0x0400007B RID: 123
	public OptionalMeshesAI[] optionalMeshList;

	// Token: 0x0400007C RID: 124
	[Range(0.01f, 50f)]
	public float demolutionStrenght;

	// Token: 0x0400007D RID: 125
	[Range(0.1f, 500f)]
	public float demolutionRange;

	// Token: 0x0400007E RID: 126
	public bool customMesh;

	// Token: 0x0400007F RID: 127
	public bool collisionParticles;

	// Token: 0x04000080 RID: 128
	public AudioClip skidSound;

	// Token: 0x04000081 RID: 129
	[Range(0.01f, 1f)]
	public float skidVolume = 0.3f;

	// Token: 0x04000082 RID: 130
	public AudioClip lowAcceleration;

	// Token: 0x04000083 RID: 131
	public AudioClip lowDeceleration;

	// Token: 0x04000084 RID: 132
	public AudioClip highAcceleration;

	// Token: 0x04000085 RID: 133
	public AudioClip highDeceleration;

	// Token: 0x04000086 RID: 134
	[Range(0.01f, 1f)]
	public float engineVolume;

	// Token: 0x04000087 RID: 135
	public AudioSource suspensionsSource;

	// Token: 0x04000088 RID: 136
	public AudioSource skidSource;

	// Token: 0x04000089 RID: 137
	public AudioClip suspensionsSound;

	// Token: 0x0400008A RID: 138
	[Range(0f, 1f)]
	public float suspensionsVolume;

	// Token: 0x0400008B RID: 139
	public CarAIInputs carAIInputs;

	// Token: 0x0400008C RID: 140
	public CarAIWaipointTracker carAIWaypointTracker;

	// Token: 0x0400008D RID: 141
	[SerializeField]
	public BrakeCondition brakeCondition = BrakeCondition.TargetDistance;

	// Token: 0x0400008E RID: 142
	[SerializeField]
	[Range(0f, 1f)]
	public float cautiousSpeedFactor = 0.05f;

	// Token: 0x0400008F RID: 143
	[SerializeField]
	[Range(0f, 180f)]
	public float cautiousAngle = 50f;

	// Token: 0x04000090 RID: 144
	[SerializeField]
	[Range(0f, 200f)]
	public float cautiousDistance = 100f;

	// Token: 0x04000091 RID: 145
	[SerializeField]
	public float cautiousAngularVelocityFactor = 30f;

	// Token: 0x04000092 RID: 146
	[SerializeField]
	[Range(0f, 0.1f)]
	public float steerSensitivity = 0.05f;

	// Token: 0x04000093 RID: 147
	[SerializeField]
	[Range(0f, 0.1f)]
	public float accelSensitivity = 0.04f;

	// Token: 0x04000094 RID: 148
	[SerializeField]
	[Range(0f, 1f)]
	public float brakeSensitivity = 1f;

	// Token: 0x04000095 RID: 149
	[SerializeField]
	[Range(0f, 10f)]
	public float lateralWander = 3f;

	// Token: 0x04000096 RID: 150
	[SerializeField]
	public float lateralWanderSpeed = 0.5f;

	// Token: 0x04000097 RID: 151
	[SerializeField]
	[Range(0f, 1f)]
	public float wanderAmount = 0.1f;

	// Token: 0x04000098 RID: 152
	[SerializeField]
	public float accelWanderSpeed = 0.1f;

	// Token: 0x04000099 RID: 153
	[SerializeField]
	public bool isDriving;

	// Token: 0x0400009A RID: 154
	[SerializeField]
	public Transform carAItarget;

	// Token: 0x0400009B RID: 155
	private GameObject carAItargetObj;

	// Token: 0x0400009C RID: 156
	[SerializeField]
	public bool stopWhenTargetReached;

	// Token: 0x0400009D RID: 157
	[SerializeField]
	public float reachTargetThreshold = 2f;

	// Token: 0x0400009E RID: 158
	[SerializeField]
	[Range(15f, 50f)]
	public float sensorsAngle;

	// Token: 0x0400009F RID: 159
	[SerializeField]
	public float avoidDistance = 10f;

	// Token: 0x040000A0 RID: 160
	[SerializeField]
	public float brakeDistance = 6f;

	// Token: 0x040000A1 RID: 161
	[SerializeField]
	public float reverseDistance = 3f;

	// Token: 0x040000A2 RID: 162
	public bool persuitAiOn;

	// Token: 0x040000A3 RID: 163
	public GameObject persuitTarget;

	// Token: 0x040000A4 RID: 164
	public float persuitDistance;

	// Token: 0x040000A5 RID: 165
	[SerializeField]
	public ProgressStyle progressStyle;

	// Token: 0x040000A6 RID: 166
	[SerializeField]
	public WaypointsPath AIcircuit;

	// Token: 0x040000A7 RID: 167
	[SerializeField]
	[Range(5f, 50f)]
	public float lookAheadForTarget = 5f;

	// Token: 0x040000A8 RID: 168
	[SerializeField]
	public float lookAheadForTargetFactor = 0.1f;

	// Token: 0x040000A9 RID: 169
	[SerializeField]
	public float lookAheadForSpeedOffset = 10f;

	// Token: 0x040000AA RID: 170
	[SerializeField]
	public float lookAheadForSpeedFactor = 0.2f;

	// Token: 0x040000AB RID: 171
	[SerializeField]
	[Range(1f, 10f)]
	public float pointThreshold = 4f;

	// Token: 0x040000AC RID: 172
	public Transform AItarget;
}
