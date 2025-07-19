using System;
using System.Collections;
using Crosstales.Radio;
using UnityEngine;

// Token: 0x020001EB RID: 491
public class Switch : MonoBehaviour
{
	// Token: 0x06000B81 RID: 2945 RVA: 0x0007F6EC File Offset: 0x0007D8EC
	private void Start()
	{
		this.AudioParent = GameObject.Find("hand");
		if (this.VolumeSwitch)
		{
			this.RadioPlayer.Volume = 0.5f;
		}
		if (this.KickStand && this.KickStandDown.activeSelf && base.transform.root.GetComponent<MainCarProperties>())
		{
			base.transform.root.GetComponent<MainCarProperties>().HandBraking = true;
		}
		this.Player = GameObject.Find("Player");
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x0007F774 File Offset: 0x0007D974
	private void Update()
	{
		if (this.Throttle && base.transform.root.GetComponent<MainCarProperties>())
		{
			if (Input.GetMouseButton(0))
			{
				base.transform.root.GetComponent<MainCarProperties>().revving = true;
				base.gameObject.GetComponent<MeshFilter>().mesh = this.MeshON;
			}
			else
			{
				base.transform.root.GetComponent<MainCarProperties>().revving = false;
				base.gameObject.GetComponent<MeshFilter>().mesh = this.MeshOFF;
			}
		}
		base.enabled = false;
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x0007F809 File Offset: 0x0007DA09
	public void ScrollUP()
	{
		if (this.VolumeSwitch)
		{
			this.RadioPlayer.Volume += 0.1f;
		}
		if (this.ChannelSwitch)
		{
			this.RadioPlayer.Next();
		}
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x0007F83D File Offset: 0x0007DA3D
	public void ScrollDown()
	{
		if (this.VolumeSwitch)
		{
			this.RadioPlayer.Volume -= 0.1f;
		}
		if (this.ChannelSwitch)
		{
			this.RadioPlayer.Previous();
		}
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x0007F874 File Offset: 0x0007DA74
	public void Clicked()
	{
		if (tools.Clicked)
		{
			return;
		}
		if (tools.DontAllowClick)
		{
			return;
		}
		if (this.WindowLiftFL && base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().WindowLiftFL)
		{
			base.transform.root.GetComponent<MainCarProperties>().WindowLiftFL.ElWindowDown();
		}
		else if (this.WindowLiftFR && base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().WindowLiftFR)
		{
			base.transform.root.GetComponent<MainCarProperties>().WindowLiftFR.ElWindowDown();
		}
		else if (this.WindowLiftRL && base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().WindowLiftRL)
		{
			base.transform.root.GetComponent<MainCarProperties>().WindowLiftRL.ElWindowDown();
		}
		else if (this.WindowLiftRR && base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().WindowLiftRR)
		{
			base.transform.root.GetComponent<MainCarProperties>().WindowLiftRR.ElWindowDown();
		}
		if (this.Latch)
		{
			this.Box.Interact();
			tools.Clicked = true;
		}
		if (this.TrailerBrake && base.transform.root.GetComponent<MainTrailerProperties>())
		{
			if (!base.transform.root.GetComponent<MainTrailerProperties>().HbrakeON)
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = this.MeshON;
				base.transform.root.GetComponent<MainTrailerProperties>().SetHbrake();
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().HandbrakeSet);
			}
			else
			{
				base.gameObject.GetComponent<MeshFilter>().mesh = this.MeshOFF;
				base.transform.root.GetComponent<MainTrailerProperties>().ReleaseHbrake();
				this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().HandbrakeRelease);
			}
		}
		if (this.TrailerLift && this.TrailerBed.localEulerAngles.x < 18f)
		{
			this.TrailerBed.localRotation = Quaternion.Euler(this.TrailerBed.localEulerAngles.x + 0.5f, 180f, 0f);
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().HydroPump);
			Debug.Log(this.TrailerBed.localEulerAngles.x);
		}
		if (base.transform.parent.GetComponent<CarProperties>() && base.transform.parent.GetComponent<CarProperties>().Condition < 0.4f && !this.IgnitinSwitch && !this.KickStarter && !this.KickStand && !this.LightSwitch)
		{
			return;
		}
		if ((base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().Electricity && this.EnableThisObject == null) || this.IgnitinSwitch || this.KickStarter || this.KickStand || base.transform.parent.name == "Radio")
		{
			if (tools.tool != 2 && tools.tool != 3 && tools.tool != 4 && tools.tool != 5 && tools.tool != 6 && tools.tool != 7 && tools.tool != 8 && tools.tool != 9 && tools.tool != 12 && tools.tool != 14 && tools.tool != 16 && tools.tool != 19 && ((base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().BatteryWires && base.transform.root.GetComponent<MainCarProperties>().Battery) || base.transform.parent.name == "Radio" || this.IgnitinSwitch || this.LightSwitch || this.KickStarter || this.KickStand))
			{
				tools.cooldown = true;
				base.StartCoroutine(this.Player.GetComponent<tools>().Cooldown());
				if (this.KickStand && base.transform.root.GetComponent<MainCarProperties>())
				{
					if (this.KickStandUp.activeSelf)
					{
						this.KickStandUp.SetActive(false);
						this.KickStandDown.SetActive(true);
						base.transform.root.GetComponent<MainCarProperties>().HandBraking = true;
					}
					else
					{
						this.KickStandUp.SetActive(true);
						this.KickStandDown.SetActive(false);
						base.transform.root.GetComponent<MainCarProperties>().HandBraking = false;
					}
				}
				if (this.KickStarter && base.transform.root.GetComponent<MainCarProperties>())
				{
					base.StartCoroutine(this.KickStart());
				}
				if (this.WiperSwitch)
				{
					base.transform.root.GetComponent<MainCarProperties>().WiperSwitch();
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().ButtonClick);
				}
				if (this.LightSwitch)
				{
					if (!base.transform.root.GetComponent<MainCarProperties>().RunningLightOn)
					{
						base.transform.root.GetComponent<MainCarProperties>().RunningLightTurnOn();
						base.transform.root.GetComponent<MainCarProperties>().HeadLightLowTurnOn();
					}
					else
					{
						base.transform.root.GetComponent<MainCarProperties>().RunningLightTurnOff();
						base.transform.root.GetComponent<MainCarProperties>().HeadLightLowTurnOff();
						base.transform.root.GetComponent<MainCarProperties>().HeadLightHighTurnOff();
					}
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().ButtonClick);
				}
				if (this.IgnitinSwitch)
				{
					if (base.transform.root != null && base.transform.root.tag == "Vehicle")
					{
						if (base.transform.root.GetComponent<MainCarProperties>().IgnitionON)
						{
							base.transform.root.GetComponent<MainCarProperties>().IgnitionON = false;
							base.transform.root.GetComponent<MainCarProperties>().EngineStop();
						}
						else if (!base.transform.root.GetComponent<MainCarProperties>().IgnitionON)
						{
							base.transform.root.GetComponent<MainCarProperties>().IgnitionON = true;
						}
					}
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().ButtonClick);
				}
				if (this.HazardLightSwitch)
				{
					if (!base.transform.root.GetComponent<MainCarProperties>().HazardLightOn)
					{
						base.transform.root.GetComponent<MainCarProperties>().HazardLightOn = true;
					}
					else
					{
						base.transform.root.GetComponent<MainCarProperties>().HazardLightOn = false;
						base.transform.root.GetComponent<MainCarProperties>().LeftLightTurnOff();
						base.transform.root.GetComponent<MainCarProperties>().RightLightTurnOff();
						base.transform.root.GetComponent<MainCarProperties>().LeftLightOn = false;
						base.transform.root.GetComponent<MainCarProperties>().RightLightOn = false;
						if (base.transform.root.GetComponent<MainCarProperties>().RunningLightOn)
						{
							base.transform.root.GetComponent<MainCarProperties>().RunningLightTurnOn();
						}
					}
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().ButtonClick);
				}
				if (this.VolumeSwitch)
				{
					if (!this.ON)
					{
						this.RadioPlayer.Play();
						this.ON = true;
						this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().ButtonClick);
					}
					else
					{
						this.RadioPlayer.Stop();
						this.ON = false;
						this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().ButtonClick);
					}
				}
				if (this.ChannelSwitch)
				{
					this.RadioPlayer.Next();
				}
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f && this.VolumeSwitch)
			{
				this.RadioPlayer.Volume += 0.1f;
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0f && this.VolumeSwitch)
			{
				this.RadioPlayer.Volume -= 0.1f;
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f && this.ChannelSwitch)
			{
				this.RadioPlayer.Next();
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0f && this.ChannelSwitch)
			{
				this.RadioPlayer.Previous();
			}
		}
		if (this.EnableThisObject != null)
		{
			this.EnableThisObject.SetActive(true);
		}
		if (this.DisableThisObject != null)
		{
			this.DisableThisObject.SetActive(false);
		}
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x00080230 File Offset: 0x0007E430
	public void ClickedR()
	{
		if (this.WindowLiftFL && base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().WindowLiftFL)
		{
			base.transform.root.GetComponent<MainCarProperties>().WindowLiftFL.ElWindowUp();
		}
		else if (this.WindowLiftFR && base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().WindowLiftFR)
		{
			base.transform.root.GetComponent<MainCarProperties>().WindowLiftFR.ElWindowUp();
		}
		else if (this.WindowLiftRL && base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().WindowLiftRL)
		{
			base.transform.root.GetComponent<MainCarProperties>().WindowLiftRL.ElWindowUp();
		}
		else if (this.WindowLiftRR && base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().WindowLiftRR)
		{
			base.transform.root.GetComponent<MainCarProperties>().WindowLiftRR.ElWindowUp();
		}
		if (this.TrailerLift && this.TrailerBed.localEulerAngles.x > 0.5f)
		{
			this.TrailerBed.localRotation = Quaternion.Euler(this.TrailerBed.localEulerAngles.x - 0.5f, 180f, 0f);
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().ButtonClick);
		}
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0008040C File Offset: 0x0007E60C
	private IEnumerator KickStart()
	{
		this.KickStarterHandle.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().StarterClick);
		yield return null;
		yield return null;
		this.KickStarterBase.transform.localRotation = Quaternion.Euler(new Vector3(-10f, 0f, 0f));
		yield return null;
		yield return null;
		this.KickStarterBase.transform.localRotation = Quaternion.Euler(new Vector3(-20f, 0f, 0f));
		yield return null;
		yield return null;
		this.KickStarterBase.transform.localRotation = Quaternion.Euler(new Vector3(-30f, 0f, 0f));
		yield return null;
		yield return null;
		this.KickStarterBase.transform.localRotation = Quaternion.Euler(new Vector3(-40f, 0f, 0f));
		yield return null;
		yield return null;
		this.KickStarterBase.transform.localRotation = Quaternion.Euler(new Vector3(-50f, 0f, 0f));
		yield return null;
		yield return null;
		this.KickStarterBase.transform.localRotation = Quaternion.Euler(new Vector3(-60f, 0f, 0f));
		yield return null;
		yield return null;
		this.KickStarterBase.transform.localRotation = Quaternion.Euler(new Vector3(-70f, 0f, 0f));
		yield return null;
		yield return null;
		this.KickStarterBase.transform.localRotation = Quaternion.Euler(new Vector3(-80f, 0f, 0f));
		yield return null;
		yield return null;
		this.KickStarterBase.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
		yield return null;
		yield return null;
		this.KickStarterBase.transform.localRotation = Quaternion.Euler(new Vector3(-60f, 0f, 0f));
		yield return null;
		yield return null;
		this.KickStarterBase.transform.localRotation = Quaternion.Euler(new Vector3(-30f, 0f, 0f));
		yield return null;
		yield return null;
		this.KickStarterBase.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		this.KickStarterHandle.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		base.transform.root.GetComponent<MainCarProperties>().KickStarting();
		yield break;
	}

	// Token: 0x04001400 RID: 5120
	public string SwitchName;

	// Token: 0x04001401 RID: 5121
	public bool Latch;

	// Token: 0x04001402 RID: 5122
	public OpenableBox Box;

	// Token: 0x04001403 RID: 5123
	public bool KickStarter;

	// Token: 0x04001404 RID: 5124
	public GameObject KickStarterBase;

	// Token: 0x04001405 RID: 5125
	public GameObject KickStarterHandle;

	// Token: 0x04001406 RID: 5126
	public bool KickStand;

	// Token: 0x04001407 RID: 5127
	public GameObject KickStandUp;

	// Token: 0x04001408 RID: 5128
	public GameObject KickStandDown;

	// Token: 0x04001409 RID: 5129
	public bool IgnitinSwitch;

	// Token: 0x0400140A RID: 5130
	public bool WiperSwitch;

	// Token: 0x0400140B RID: 5131
	public bool LightSwitch;

	// Token: 0x0400140C RID: 5132
	public bool HazardLightSwitch;

	// Token: 0x0400140D RID: 5133
	public bool TrailerBrake;

	// Token: 0x0400140E RID: 5134
	public bool TrailerLift;

	// Token: 0x0400140F RID: 5135
	public Transform TrailerBed;

	// Token: 0x04001410 RID: 5136
	public bool Throttle;

	// Token: 0x04001411 RID: 5137
	public Mesh MeshON;

	// Token: 0x04001412 RID: 5138
	public Mesh MeshOFF;

	// Token: 0x04001413 RID: 5139
	public bool WindowLiftFL;

	// Token: 0x04001414 RID: 5140
	public bool WindowLiftFR;

	// Token: 0x04001415 RID: 5141
	public bool WindowLiftRL;

	// Token: 0x04001416 RID: 5142
	public bool WindowLiftRR;

	// Token: 0x04001417 RID: 5143
	public GameObject EnableThisObject;

	// Token: 0x04001418 RID: 5144
	public GameObject DisableThisObject;

	// Token: 0x04001419 RID: 5145
	public bool VolumeSwitch;

	// Token: 0x0400141A RID: 5146
	public bool ChannelSwitch;

	// Token: 0x0400141B RID: 5147
	public GameObject AudioParent;

	// Token: 0x0400141C RID: 5148
	public SimplePlayer RadioPlayer;

	// Token: 0x0400141D RID: 5149
	public bool ON;

	// Token: 0x0400141E RID: 5150
	private GameObject Player;
}
