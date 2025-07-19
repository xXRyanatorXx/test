using System;
using UnityEngine;

// Token: 0x02000192 RID: 402
public class FLUID : MonoBehaviour
{
	// Token: 0x0600091B RID: 2331 RVA: 0x00058B80 File Offset: 0x00056D80
	public void Start()
	{
		base.gameObject.layer = LayerMask.NameToLayer("NoCarCollider");
		if (base.transform.name == "OilFluidContainerHead")
		{
			foreach (Transform transform in base.transform.root.GetComponentsInChildren<Transform>())
			{
				if (transform.name == "OilFluidContainer")
				{
					this.Container = transform.transform.GetComponent<FLUID>();
				}
			}
		}
		if (base.transform.parent.GetComponent<CarProperties>() && base.transform.parent.GetComponent<CarProperties>().MeshDamaged && this.VisualFluid)
		{
			UnityEngine.Object.Destroy(this.VisualFluid.gameObject);
		}
		this.Started = true;
		this.VisualUpdate();
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x00058C58 File Offset: 0x00056E58
	public void StartFromMain()
	{
		this.Condition = base.transform.parent.GetComponent<CarProperties>().FluidCondition;
		this.FluidSize = base.transform.parent.GetComponent<CarProperties>().FluidSize;
		this.DieselPercent = base.transform.parent.GetComponent<CarProperties>().DieselPercent;
		this.VisualUpdate();
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x00058CBC File Offset: 0x00056EBC
	private void Update()
	{
		if (this.IsOpen)
		{
			if (this.CanEmit)
			{
				if ((base.gameObject.transform.rotation.eulerAngles.z <= 120f || base.gameObject.transform.rotation.eulerAngles.z >= 240f) && this.Container.FluidSize < this.Container.ContainerSize && !this.Oil)
				{
					base.GetComponent<BoxCollider>().enabled = true;
					base.GetComponentInChildren<ParticleSystem>().Stop();
					return;
				}
				if (this.Container.FluidSize > 0f)
				{
					if (this.Oil)
					{
						this.lerpedColor = Color.Lerp(Color.black, Color.yellow, this.Condition);
						base.GetComponentInChildren<ParticleSystem>().startColor = this.lerpedColor;
					}
					base.GetComponent<BoxCollider>().enabled = false;
					base.GetComponentInChildren<ParticleSystem>().Play();
					if (this.Fuel)
					{
						if (this.DieselPercent > 0.8f)
						{
							base.GetComponentInChildren<ParticleSystem>().startColor = this.DieselCol;
						}
						else if (this.DieselPercent < 0.2f)
						{
							base.GetComponentInChildren<ParticleSystem>().startColor = this.GasolineCol;
						}
						else
						{
							base.GetComponentInChildren<ParticleSystem>().startColor = this.MixedCol;
						}
						this.Container.FluidSize -= 2.5f * Time.deltaTime;
					}
					if (this.Container.FluidSize >= this.Container.ContainerSize)
					{
						this.Container.FluidSize -= 0.1f * Time.deltaTime;
					}
					else
					{
						this.Container.FluidSize -= 0.1f * Time.deltaTime;
					}
					this.VisualUpdate();
					return;
				}
				base.GetComponent<BoxCollider>().enabled = true;
				base.GetComponentInChildren<ParticleSystem>().Stop();
				return;
			}
		}
		else
		{
			base.GetComponent<BoxCollider>().enabled = true;
			base.GetComponentInChildren<ParticleSystem>().Stop();
			base.enabled = false;
		}
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x00058ECC File Offset: 0x000570CC
	private void OnParticleCollision(GameObject other)
	{
		if (this.IsOpen && other.gameObject.transform.parent != base.gameObject)
		{
			if (this.Oil)
			{
				foreach (Transform transform in base.transform.parent.parent.parent.parent.parent.GetComponentsInChildren<Transform>())
				{
					if (transform.name == "OilFluidContainerr")
					{
						this.Container = transform.GetComponent<FLUID>();
					}
				}
				if (this.Container.FluidSize <= 0f)
				{
					this.Container.Condition = 1f;
				}
			}
			if (this.Fuel && other.gameObject.tag == "Fuel" && this.FluidSize < this.ContainerSize)
			{
				if (other.gameObject.name == "ParticlesG")
				{
					this.Container.DieselPercent = this.Container.FluidSize * this.Container.DieselPercent / (this.FluidSize + 2.5f * Time.deltaTime);
				}
				else if (other.gameObject.name == "ParticlesD")
				{
					this.Container.DieselPercent = (this.Container.FluidSize * this.Container.DieselPercent + 2.5f * Time.deltaTime) / (this.FluidSize + 2.5f * Time.deltaTime);
				}
				else
				{
					this.Container.DieselPercent = (this.Container.FluidSize * this.Container.DieselPercent + other.transform.parent.GetComponent<FLUID>().Container.DieselPercent * (2.5f * Time.deltaTime)) / (this.FluidSize + 2.5f * Time.deltaTime);
				}
				this.Container.FluidSize += 2.5f * Time.deltaTime;
			}
			if (this.Oil && other.gameObject.tag == "Oil")
			{
				this.Container.FluidSize += 0.1f * Time.deltaTime;
			}
			if (this.BrakeFLuid && other.gameObject.tag == "BrakeFLuid")
			{
				this.Container.FluidSize += 0.1f * Time.deltaTime;
			}
			if (this.Coolant && other.gameObject.tag == "Coolant")
			{
				this.Container.FluidSize += 0.1f * Time.deltaTime;
			}
			tools.filling = true;
			if (this.FluidSize < 0f)
			{
				this.FluidSize = 0f;
			}
			this.VisualUpdate();
		}
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x000591B8 File Offset: 0x000573B8
	public void VisualUpdate()
	{
		if (this.VisualFluid)
		{
			this.VisualFluid.transform.localScale = new Vector3(this.VisualFluid.transform.localScale.x, this.FluidSize / this.ContainerSize, this.VisualFluid.transform.localScale.z);
			if (this.Fuel)
			{
				if (this.DieselPercent > 0.8f)
				{
					this.VisualFluid.GetComponent<Renderer>().material.SetColor("_Color", this.DieselCol);
					return;
				}
				if (this.DieselPercent < 0.2f)
				{
					this.VisualFluid.GetComponent<Renderer>().material.SetColor("_Color", this.GasolineCol);
					return;
				}
				this.VisualFluid.GetComponent<Renderer>().material.SetColor("_Color", this.MixedCol);
			}
		}
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x000592A5 File Offset: 0x000574A5
	private void OnEnable()
	{
		if (this.Funnel)
		{
			this.Funnel.SetActive(true);
		}
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x000592C0 File Offset: 0x000574C0
	private void OnDisable()
	{
		if (this.Funnel)
		{
			this.Funnel.SetActive(false);
		}
	}

	// Token: 0x040010EF RID: 4335
	public bool Started;

	// Token: 0x040010F0 RID: 4336
	public bool IsOpen;

	// Token: 0x040010F1 RID: 4337
	public bool CanEmit;

	// Token: 0x040010F2 RID: 4338
	public FLUID Container;

	// Token: 0x040010F3 RID: 4339
	public float ContainerSize;

	// Token: 0x040010F4 RID: 4340
	public float FluidSize;

	// Token: 0x040010F5 RID: 4341
	public float DieselPercent;

	// Token: 0x040010F6 RID: 4342
	public float Condition;

	// Token: 0x040010F7 RID: 4343
	public float MinFluidSize;

	// Token: 0x040010F8 RID: 4344
	public GameObject VisualFluid;

	// Token: 0x040010F9 RID: 4345
	public Color GasolineCol;

	// Token: 0x040010FA RID: 4346
	public Color DieselCol;

	// Token: 0x040010FB RID: 4347
	public Color MixedCol;

	// Token: 0x040010FC RID: 4348
	private Color lerpedColor = Color.white;

	// Token: 0x040010FD RID: 4349
	public GameObject Funnel;

	// Token: 0x040010FE RID: 4350
	public GameObject Cup;

	// Token: 0x040010FF RID: 4351
	public GameObject FunnelCollider;

	// Token: 0x04001100 RID: 4352
	public bool Fuel;

	// Token: 0x04001101 RID: 4353
	public bool Oil;

	// Token: 0x04001102 RID: 4354
	public bool BrakeFLuid;

	// Token: 0x04001103 RID: 4355
	public bool Coolant;
}
