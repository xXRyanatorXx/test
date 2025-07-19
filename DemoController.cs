using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200024C RID: 588
public class DemoController : MonoBehaviour
{
	// Token: 0x06000DE2 RID: 3554 RVA: 0x000946AC File Offset: 0x000928AC
	private void Start()
	{
		if (SceneManager.GetActiveScene().name == "_slider_demo_scene")
		{
			this.isMobileDemoScene = false;
		}
		if (SceneManager.GetActiveScene().name == "mobile_slider_demo_scene")
		{
			this.isMobileDemoScene = true;
		}
		this.carSimulator = this.gasPedalButton.GetComponent<CarSimulator>();
		if (this.isMobileDemoScene)
		{
			this.resmob = base.GetComponentsInChildren<RealisticEngineSound_mobile>();
			for (int i = 0; i < this.resmob.Length; i++)
			{
				this.resmob[i].carMaxSpeed = 7000f;
				if (i % 2 == 1)
				{
					this.resmob[i].gameObject.SetActive(false);
				}
			}
			return;
		}
		this.res = base.GetComponentsInChildren<RealisticEngineSound>();
		for (int j = 0; j < this.res.Length; j++)
		{
			this.res[j].carMaxSpeed = 7000f;
			if (j % 2 == 1)
			{
				this.res[j].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x000947A8 File Offset: 0x000929A8
	private void Update()
	{
		this.rpmText.text = "Engine RPM: " + ((int)this.rpmSlider.value).ToString();
		this.pitchText.text = (this.pitchSlider.value.ToString() ?? "");
		if (this.isMobileDemoScene)
		{
			for (int i = 0; i < this.resmob.Length; i++)
			{
				this.resmob[i].pitchMultiplier = this.pitchSlider.value;
				if (this.simulated)
				{
					this.resmob[i].engineCurrentRPM = this.carSimulator.rpm;
					this.rpmSlider.value = this.carSimulator.rpm;
				}
				else
				{
					this.resmob[i].engineCurrentRPM = this.rpmSlider.value;
					this.carSimulator.rpm = this.rpmSlider.value;
				}
				this.resmob[i].carCurrentSpeed = this.rpmSlider.value / 127f;
			}
		}
		else
		{
			for (int j = 0; j < this.res.Length; j++)
			{
				this.res[j].pitchMultiplier = this.pitchSlider.value;
				if (this.simulated)
				{
					this.res[j].engineCurrentRPM = this.carSimulator.rpm;
					this.rpmSlider.value = this.carSimulator.rpm;
				}
				else
				{
					this.res[j].engineCurrentRPM = this.rpmSlider.value;
					this.carSimulator.rpm = this.rpmSlider.value;
				}
				this.res[j].carCurrentSpeed = this.rpmSlider.value / 127f;
			}
		}
		if (this.simulated && this.gasPedalPressing != null)
		{
			this.gasPedalPressing.isOn = this.carSimulator.gasPedalPressing;
		}
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x000949AC File Offset: 0x00092BAC
	public void UpdateRPM(Toggle togl)
	{
		if (this.isMobileDemoScene)
		{
			for (int i = 0; i < this.resmob.Length; i++)
			{
				this.resmob[i].useRPMLimit = togl.isOn;
			}
			return;
		}
		for (int j = 0; j < this.res.Length; j++)
		{
			this.res[j].useRPMLimit = togl.isOn;
		}
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x00094A10 File Offset: 0x00092C10
	public void UpdateReverseGear(Toggle togl)
	{
		if (this.isMobileDemoScene)
		{
			for (int i = 0; i < this.resmob.Length; i++)
			{
				this.resmob[i].enableReverseGear = togl.isOn;
			}
		}
		else
		{
			for (int j = 0; j < this.res.Length; j++)
			{
				this.res[j].enableReverseGear = togl.isOn;
			}
		}
		if (!togl.isOn)
		{
			this.isReversing.isOn = false;
			this.isReversing.gameObject.SetActive(false);
			return;
		}
		if (!this.isReversing.gameObject.activeSelf)
		{
			this.isReversing.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x00094ABC File Offset: 0x00092CBC
	public void IsReversing(Toggle togl)
	{
		if (this.isMobileDemoScene)
		{
			for (int i = 0; i < this.resmob.Length; i++)
			{
				this.resmob[i].isReversing = togl.isOn;
			}
			return;
		}
		for (int j = 0; j < this.res.Length; j++)
		{
			this.res[j].isReversing = togl.isOn;
		}
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x00094B20 File Offset: 0x00092D20
	public void IsSimulated(Dropdown drpDown)
	{
		if (drpDown.value == 0)
		{
			this.simulated = true;
			this.accelerationSpeed.SetActive(true);
			this.gasPedalButton.SetActive(true);
		}
		if (drpDown.value == 1)
		{
			this.simulated = false;
			this.accelerationSpeed.SetActive(false);
			this.gasPedalButton.SetActive(false);
			if (!this.isMobileDemoScene)
			{
				this.gasPedalPressing.isOn = true;
			}
		}
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x00094B90 File Offset: 0x00092D90
	public void ChangeCarSound(int a)
	{
		if (this.isMobileDemoScene)
		{
			for (int i = 0; i < this.resmob.Length; i++)
			{
				if (i != a && i != a + 1)
				{
					this.resmob[i].enabled = false;
				}
				this.resmob[a].enabled = true;
				this.resmob[a + 1].enabled = true;
			}
			return;
		}
		for (int j = 0; j < this.res.Length; j++)
		{
			if (j != a && j != a + 1)
			{
				this.res[j].enabled = false;
			}
			this.res[a].enabled = true;
			this.res[a + 1].enabled = true;
		}
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x00094C38 File Offset: 0x00092E38
	public void UpdateGasPedal(Toggle togl)
	{
		if (this.isMobileDemoScene)
		{
			for (int i = 0; i < this.resmob.Length; i++)
			{
				this.resmob[i].gasPedalPressing = togl.isOn;
			}
			return;
		}
		for (int j = 0; j < this.res.Length; j++)
		{
			this.res[j].gasPedalPressing = togl.isOn;
		}
	}

	// Token: 0x0400168C RID: 5772
	[HideInInspector]
	public RealisticEngineSound[] res;

	// Token: 0x0400168D RID: 5773
	[HideInInspector]
	public RealisticEngineSound_mobile[] resmob;

	// Token: 0x0400168E RID: 5774
	public GameObject gasPedalButton;

	// Token: 0x0400168F RID: 5775
	public Slider rpmSlider;

	// Token: 0x04001690 RID: 5776
	public Slider pitchSlider;

	// Token: 0x04001691 RID: 5777
	public Text pitchText;

	// Token: 0x04001692 RID: 5778
	public Text rpmText;

	// Token: 0x04001693 RID: 5779
	public Toggle isReversing;

	// Token: 0x04001694 RID: 5780
	public Toggle gasPedalPressing;

	// Token: 0x04001695 RID: 5781
	public GameObject accelerationSpeed;

	// Token: 0x04001696 RID: 5782
	public bool simulated = true;

	// Token: 0x04001697 RID: 5783
	private bool isMobileDemoScene;

	// Token: 0x04001698 RID: 5784
	private CarSimulator carSimulator;
}
