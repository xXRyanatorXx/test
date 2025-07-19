using System;
using System.Collections.Generic;
using NWH.VehiclePhysics2;
using UnityEngine;

// Token: 0x020001CD RID: 461
public class SMOKE : MonoBehaviour
{
	// Token: 0x06000AE8 RID: 2792 RVA: 0x0006FFBC File Offset: 0x0006E1BC
	public void Start()
	{
		this.vc = base.transform.root.GetComponent<VehicleController>();
		foreach (ParticleSystem particleSystem in this.particleSystems)
		{
			this._emissionModule = this.particleSystems[0].emission;
			this._mainModule = this.particleSystems[0].main;
			this._initLifetime = this._mainModule.startLifetime.constant;
			this._initStartSpeedMin = this._mainModule.startSpeed.constantMin;
			this._initStartSpeedMax = this._mainModule.startSpeed.constantMax;
			this._initStartSizeMin = this._mainModule.startSize.constantMin;
			this._initStartSizeMax = this._mainModule.startSize.constantMax;
		}
		this.maxSizeMultiplier = Mathf.Clamp(this.maxSizeMultiplier, 1f, float.PositiveInfinity);
		this.maxSpeedMultiplier = Mathf.Clamp(this.maxSpeedMultiplier, 1f, float.PositiveInfinity);
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x00070108 File Offset: 0x0006E308
	public void Update()
	{
		if (this.vc.powertrain.Active && this.vc.powertrain.engine.IsRunning)
		{
			this._vehicleSpeed = this.vc.Speed;
			this._absVehicleSpeed = ((this._vehicleSpeed < 0f) ? (-this._vehicleSpeed) : this._vehicleSpeed);
			using (List<ParticleSystem>.Enumerator enumerator = this.particleSystems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ParticleSystem particleSystem = enumerator.Current;
					if (!particleSystem.isPlaying)
					{
						particleSystem.Play();
					}
					this._emissionModule = particleSystem.emission;
					this._mainModule = particleSystem.main;
					this.vc.powertrain.engine.GetLoad();
					float rpmpercent = this.vc.powertrain.engine.RPMPercent;
					if (!this._emissionModule.enabled)
					{
						this._emissionModule.enabled = true;
					}
					float b = (this._vehicleSpeed < 0.2f && this._vehicleSpeed > -0.2f) ? 0f : (this.lifetimeDistance / this._vehicleSpeed);
					float constant = Mathf.Lerp(this._initLifetime, b, this._absVehicleSpeed * 0.5f);
					this._mainModule.startLifetime = constant;
					this._mainModule.startColor = this.UsingColor;
					float num = this.maxSpeedMultiplier - 1f;
					this._minMaxCurve = this._mainModule.startSpeed;
					this._minMaxCurve.constantMin = this._initStartSpeedMin + rpmpercent * num;
					this._minMaxCurve.constantMax = this._initStartSpeedMax + rpmpercent * num;
					this._mainModule.startSpeed = this._minMaxCurve;
					float num2 = this.maxSizeMultiplier - 1f;
					this._minMaxCurve = this._mainModule.startSize;
					this._minMaxCurve.constantMin = this._initStartSizeMin + rpmpercent * num2;
					this._minMaxCurve.constantMax = this._initStartSizeMax + rpmpercent * num2;
					this._mainModule.startSize = this._minMaxCurve;
					if (this.vc.damageHandler.IsEnabled)
					{
						this._sootAmount += this.vc.damageHandler.Damage;
					}
				}
				return;
			}
		}
		foreach (ParticleSystem particleSystem2 in this.particleSystems)
		{
			if (particleSystem2.isPlaying)
			{
				particleSystem2.Stop();
			}
			particleSystem2.emission.enabled = false;
		}
	}

	// Token: 0x04001350 RID: 4944
	public VehicleController vc;

	// Token: 0x04001351 RID: 4945
	[Range(0f, 1f)]
	public float lifetimeDistance = 0.4f;

	// Token: 0x04001352 RID: 4946
	[Range(0f, 1f)]
	public float sootIntensity = 0.4f;

	// Token: 0x04001353 RID: 4947
	[Range(1f, 5f)]
	public float maxSpeedMultiplier = 1.4f;

	// Token: 0x04001354 RID: 4948
	[Range(1f, 20f)]
	public float maxSizeMultiplier = 1.2f;

	// Token: 0x04001355 RID: 4949
	[Tooltip("    Normal particle start color. Used when there is no throttle - engine is under no load.")]
	public Color normalColor = new Color(0.6f, 0.6f, 0.6f, 0.3f);

	// Token: 0x04001356 RID: 4950
	[Tooltip("    Normal particle start color. Used when there is no throttle - engine is under no load.")]
	public Color UsingColor = new Color(0.6f, 0.6f, 0.6f, 0.3f);

	// Token: 0x04001357 RID: 4951
	[Tooltip("    Soot particle start color. Used under heavy throttle - engine is under load.")]
	public Color sootColor = new Color(0.1f, 0.1f, 0.8f);

	// Token: 0x04001358 RID: 4952
	[Tooltip("    Soot particle start color. Used under heavy throttle - engine is under load.")]
	public Color BlackColor = new Color(0.1f, 0.1f, 0.8f);

	// Token: 0x04001359 RID: 4953
	[Tooltip("    Soot particle start color. Used under heavy throttle - engine is under load.")]
	public Color WhiteColor = new Color(0.1f, 0.1f, 0.8f);

	// Token: 0x0400135A RID: 4954
	[Tooltip("    Soot particle start color. Used under heavy throttle - engine is under load.")]
	public Color BlueColor = new Color(0.1f, 0.1f, 0.8f);

	// Token: 0x0400135B RID: 4955
	[Tooltip("    List of exhaust particle systems.")]
	public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	// Token: 0x0400135C RID: 4956
	private float _initLifetime;

	// Token: 0x0400135D RID: 4957
	private float _initStartSpeedMin;

	// Token: 0x0400135E RID: 4958
	private float _initStartSpeedMax;

	// Token: 0x0400135F RID: 4959
	private float _initStartSizeMin;

	// Token: 0x04001360 RID: 4960
	private float _initStartSizeMax;

	// Token: 0x04001361 RID: 4961
	private float _sootAmount;

	// Token: 0x04001362 RID: 4962
	private ParticleSystem.EmissionModule _emissionModule;

	// Token: 0x04001363 RID: 4963
	private ParticleSystem.MainModule _mainModule;

	// Token: 0x04001364 RID: 4964
	private ParticleSystem.MinMaxCurve _minMaxCurve;

	// Token: 0x04001365 RID: 4965
	private float _vehicleSpeed;

	// Token: 0x04001366 RID: 4966
	private float _absVehicleSpeed;
}
