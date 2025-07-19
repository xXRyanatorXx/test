using System;
using System.Collections;
using NWH.VehiclePhysics2;
using UnityEngine;

// Token: 0x0200016D RID: 365
public class IgnitionKey : MonoBehaviour
{
	// Token: 0x060007E3 RID: 2019 RVA: 0x00044C8E File Offset: 0x00042E8E
	public void Start()
	{
		this.exp = base.transform.root.GetComponent<VehicleController>();
		base.StartCoroutine(this.LateStart());
		this.MainProperties = base.transform.root.GetComponent<MainCarProperties>();
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00044CC9 File Offset: 0x00042EC9
	private IEnumerator LateStart()
	{
		yield return new WaitForSeconds(2f);
		this.exp = base.transform.root.GetComponent<VehicleController>();
		if (base.transform.root != null && base.transform.root.tag == "Vehicle")
		{
			this.exp = base.transform.root.GetComponent<VehicleController>();
			this.MainProperties = base.transform.root.GetComponent<MainCarProperties>();
		}
		yield break;
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00044CD8 File Offset: 0x00042ED8
	private void Update()
	{
		if (tools.DontAllowClick)
		{
			return;
		}
		if (tools.tool != 18)
		{
			RaycastHit raycastHit;
			if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1f, 1 << LayerMask.NameToLayer("OpenableParts")) && raycastHit.collider.gameObject == base.gameObject)
			{
				this.Clicked = false;
				if (base.transform.root != null && base.transform.root.tag == "Vehicle")
				{
					if (!this.On && !this.Clicked && !this.Cranked)
					{
						if (base.transform.parent.GetComponent<MPobject>())
						{
							base.transform.parent.GetComponent<MPobject>().networkDummy.on1();
						}
						else if (base.transform.parent.parent.GetComponent<MPobject>())
						{
							base.transform.parent.parent.GetComponent<MPobject>().networkDummy.on1();
						}
						else
						{
							this.on1();
						}
					}
					else if (this.On && !this.Clicked && this.Cranked)
					{
						if (base.transform.parent.GetComponent<MPobject>())
						{
							base.transform.parent.GetComponent<MPobject>().networkDummy.on0();
						}
						else if (base.transform.parent.parent.GetComponent<MPobject>())
						{
							base.transform.parent.parent.GetComponent<MPobject>().networkDummy.on0();
						}
						else
						{
							this.on0();
						}
					}
					else if (this.On && !this.Clicked && !this.Cranked)
					{
						if (base.transform.parent.GetComponent<MPobject>())
						{
							base.transform.parent.GetComponent<MPobject>().networkDummy.on3();
						}
						else if (base.transform.parent.parent.GetComponent<MPobject>())
						{
							base.transform.parent.parent.GetComponent<MPobject>().networkDummy.on3();
						}
						else
						{
							this.on3();
						}
					}
				}
			}
			if (Input.GetMouseButtonUp(0) && this.On && this.Cranked && this.Starter)
			{
				if (base.transform.parent.GetComponent<MPobject>())
				{
					base.transform.parent.GetComponent<MPobject>().networkDummy.on2();
					return;
				}
				if (base.transform.parent.parent.GetComponent<MPobject>())
				{
					base.transform.parent.parent.GetComponent<MPobject>().networkDummy.on2();
					return;
				}
				this.on2();
			}
		}
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x0004500C File Offset: 0x0004320C
	public void on1()
	{
		this.MainProperties.IgnitionON = true;
		if (this.MainProperties.Battery && this.MainProperties.BatteryWires && this.MainProperties.Cluster && this.MainProperties.Cluster.ClusterBat)
		{
			this.MainProperties.Cluster.ClusterBat.GetComponent<MeshRenderer>().enabled = true;
		}
		this.On = true;
		base.transform.localEulerAngles = new Vector3(25f, 0f, 0f);
		this.Clicked = true;
		if (this.MainProperties.DieselEngine)
		{
			if (this.MainProperties.Battery && this.MainProperties.BatteryWires && this.MainProperties.Cluster && this.MainProperties.Cluster.ClusterGlowPlugs)
			{
				this.MainProperties.Cluster.ClusterGlowPlugs.GetComponent<MeshRenderer>().enabled = true;
			}
			this.MainProperties.GlowPlugsready = false;
			this.MainProperties.GlowPlugTimer = 17f;
			if (this.MainProperties.EngineBlock && this.MainProperties.GlowPlugRelay && this.MainProperties.GlowPlugRelay.Condition > 0.1f)
			{
				for (int i = 0; i < this.MainProperties.GlowPlugs.Length; i++)
				{
					if (this.MainProperties.GlowPlugs[i].Condition > 0.1f)
					{
						this.MainProperties.GlowPlugTimer -= 15f / this.MainProperties.EngineBlock.EngineCylinderCount;
					}
				}
			}
			if (this.MainProperties.GlowPlugTimer <= 0f)
			{
				this.MainProperties.GlowPlugTimer = 4f;
			}
			base.StartCoroutine(this.MainProperties.GlowPlugHeating());
		}
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0004521C File Offset: 0x0004341C
	public void on0()
	{
		this.MainProperties.IgnitionON = false;
		this.MainProperties.GlowPlugsready = false;
		if (this.MainProperties.Cluster && this.MainProperties.Cluster.ClusterBat)
		{
			this.MainProperties.Cluster.ClusterBat.GetComponent<MeshRenderer>().enabled = false;
		}
		if (this.MainProperties.Cluster && this.MainProperties.Cluster.ClusterGlowPlugs)
		{
			this.MainProperties.Cluster.ClusterGlowPlugs.GetComponent<MeshRenderer>().enabled = false;
		}
		this.Clicked = true;
		this.On = false;
		this.Cranked = false;
		this.MainProperties.EngineStop();
		base.transform.localEulerAngles = new Vector3(25f, 0f, 35f);
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00045308 File Offset: 0x00043508
	public void on3()
	{
		if (this.MainProperties.Cluster && this.MainProperties.Cluster.ClusterBat)
		{
			this.MainProperties.Cluster.ClusterBat.GetComponent<MeshRenderer>().enabled = false;
		}
		this.MainProperties.Cranking();
		this.Clicked = true;
		this.Starter = true;
		this.Cranked = true;
		base.transform.localEulerAngles = new Vector3(25f, 0f, -30f);
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00045398 File Offset: 0x00043598
	public void on2()
	{
		this.MainProperties.IgnitionON = true;
		this.Cranked = true;
		this.On = true;
		this.Starter = false;
		base.transform.localEulerAngles = new Vector3(25f, 0f, 0f);
		if (!this.exp.powertrain.engine.IsRunning && this.MainProperties.Battery && this.MainProperties.BatteryWires && this.MainProperties.Cluster && this.MainProperties.Cluster.ClusterBat)
		{
			this.MainProperties.Cluster.ClusterBat.GetComponent<MeshRenderer>().enabled = true;
		}
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00045464 File Offset: 0x00043664
	private IEnumerator StartingCar()
	{
		yield return new WaitForSeconds(5f);
		if (this.exp.powertrain.engine.starterActive)
		{
			this.exp.powertrain.engine.ignition = true;
			this.exp.powertrain.engine.starterActive = false;
		}
		yield break;
	}

	// Token: 0x04000ECC RID: 3788
	public bool On;

	// Token: 0x04000ECD RID: 3789
	public bool Starter;

	// Token: 0x04000ECE RID: 3790
	public bool Cranked;

	// Token: 0x04000ECF RID: 3791
	public bool Clicked;

	// Token: 0x04000ED0 RID: 3792
	public VehicleController exp;

	// Token: 0x04000ED1 RID: 3793
	public MainCarProperties MainProperties;
}
