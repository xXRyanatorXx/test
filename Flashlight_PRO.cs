using System;
using UnityEngine;

// Token: 0x0200028B RID: 651
public class Flashlight_PRO : MonoBehaviour
{
	// Token: 0x06000F43 RID: 3907 RVA: 0x0009EB38 File Offset: 0x0009CD38
	private void Start()
	{
		this.spotlight = this.Lights.transform.Find("Spotlight").GetComponent<Light>();
		this.ambient_light_material = this.Lights.transform.Find("ambient").GetComponent<Renderer>().material;
		this.ambient_mat_color = this.ambient_light_material.GetColor("_TintColor");
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x0009EBA0 File Offset: 0x0009CDA0
	public void Change_Intensivity(float percentage)
	{
		percentage = Mathf.Clamp(percentage, 0f, 100f);
		this.spotlight.intensity = 8f * percentage / 100f;
		this.ambient_light_material.SetColor("_TintColor", new Color(this.ambient_mat_color.r, this.ambient_mat_color.g, this.ambient_mat_color.b, percentage / 2000f));
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x0009EC14 File Offset: 0x0009CE14
	public void Switch()
	{
		this.is_enabled = !this.is_enabled;
		this.Lights.SetActive(this.is_enabled);
		if (this.switch_sound != null)
		{
			this.switch_sound.Play();
		}
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x0009EC50 File Offset: 0x0009CE50
	public void Enable_Particles(bool value)
	{
		if (this.dust_particles != null)
		{
			if (value)
			{
				this.dust_particles.gameObject.SetActive(true);
				this.dust_particles.Play();
				return;
			}
			this.dust_particles.Stop();
			this.dust_particles.gameObject.SetActive(false);
		}
	}

	// Token: 0x0400186F RID: 6255
	[Space(10f)]
	[SerializeField]
	private GameObject Lights;

	// Token: 0x04001870 RID: 6256
	[SerializeField]
	private AudioSource switch_sound;

	// Token: 0x04001871 RID: 6257
	[SerializeField]
	private ParticleSystem dust_particles;

	// Token: 0x04001872 RID: 6258
	private Light spotlight;

	// Token: 0x04001873 RID: 6259
	private Material ambient_light_material;

	// Token: 0x04001874 RID: 6260
	private Color ambient_mat_color;

	// Token: 0x04001875 RID: 6261
	private bool is_enabled;
}
