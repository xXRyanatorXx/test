using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001EA RID: 490
public class SteeringSetup : MonoBehaviour
{
	// Token: 0x06000B69 RID: 2921 RVA: 0x0007D734 File Offset: 0x0007B934
	private void Start()
	{
		if (PlayerPrefs.HasKey("SteeringRotation"))
		{
			this.wheelrotation = PlayerPrefs.GetFloat("SteeringRotation");
		}
		else
		{
			this.wheelrotation = 900f;
			this.WheelSlider.value = this.wheelrotation;
			PlayerPrefs.SetFloat("SteeringRotation", this.wheelrotation);
		}
		this.WheelSlider.value = this.wheelrotation;
		this.rotationText.text = this.wheelrotation.ToString("000");
		this.ResetText();
		if (PlayerPrefs.HasKey("AutoClutch") && PlayerPrefs.GetFloat("AutoClutch") == 0f)
		{
			this.AutoClutch = false;
			this.AutoClutchToggle.isOn = false;
			return;
		}
		this.AutoClutch = true;
		this.AutoClutchToggle.isOn = true;
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x0007D804 File Offset: 0x0007BA04
	public void ResetText()
	{
		if (PlayerPrefs.HasKey("waitUp"))
		{
			this.Up.text = PlayerPrefs.GetInt("waitUp").ToString();
		}
		if (PlayerPrefs.HasKey("waitDown"))
		{
			this.Down.text = PlayerPrefs.GetInt("waitDown").ToString();
		}
		if (PlayerPrefs.HasKey("wait1"))
		{
			this.W1.text = PlayerPrefs.GetInt("wait1").ToString();
		}
		if (PlayerPrefs.HasKey("wait2"))
		{
			this.W2.text = PlayerPrefs.GetInt("wait2").ToString();
		}
		if (PlayerPrefs.HasKey("wait3"))
		{
			this.W3.text = PlayerPrefs.GetInt("wait3").ToString();
		}
		if (PlayerPrefs.HasKey("wait4"))
		{
			this.W4.text = PlayerPrefs.GetInt("wait4").ToString();
		}
		if (PlayerPrefs.HasKey("wait5"))
		{
			this.W5.text = PlayerPrefs.GetInt("wait5").ToString();
		}
		if (PlayerPrefs.HasKey("wait6"))
		{
			this.W6.text = PlayerPrefs.GetInt("wait6").ToString();
		}
		if (PlayerPrefs.HasKey("waitR"))
		{
			this.WR.text = PlayerPrefs.GetInt("waitR").ToString();
		}
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x0007D984 File Offset: 0x0007BB84
	public void SteeringSlider()
	{
		this.wheelrotation = this.WheelSlider.value;
		this.rotationText.text = this.wheelrotation.ToString("000");
		PlayerPrefs.SetFloat("SteeringRotation", this.wheelrotation);
		PlayerPrefs.Save();
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x0007D9D4 File Offset: 0x0007BBD4
	public void SetSteeringAxis()
	{
		if (this.SteeringDR.value == 0)
		{
			PlayerPrefs.SetInt("SteeringAxis", 0);
		}
		if (this.SteeringDR.value == 1)
		{
			PlayerPrefs.SetInt("SteeringAxis", 1);
		}
		if (this.SteeringDR.value == 2)
		{
			PlayerPrefs.SetInt("SteeringAxis", 2);
		}
		if (this.SteeringDR.value == 3)
		{
			PlayerPrefs.SetInt("SteeringAxis", 3);
		}
		if (this.SteeringDR.value == 4)
		{
			PlayerPrefs.SetInt("SteeringAxis", 4);
		}
		if (this.SteeringDR.value == 5)
		{
			PlayerPrefs.SetInt("SteeringAxis", 5);
		}
		if (this.SteeringDR.value == 6)
		{
			PlayerPrefs.SetInt("SteeringAxis", 6);
		}
		if (this.SteeringDR.value == 7)
		{
			PlayerPrefs.SetInt("SteeringAxis", 7);
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x0007DAB0 File Offset: 0x0007BCB0
	public void SetThrottleAxis()
	{
		if (this.ThrottleDR.value == 0)
		{
			PlayerPrefs.SetInt("ThrottleAxis", 0);
		}
		if (this.ThrottleDR.value == 1)
		{
			PlayerPrefs.SetInt("ThrottleAxis", 1);
		}
		if (this.ThrottleDR.value == 2)
		{
			PlayerPrefs.SetInt("ThrottleAxis", 2);
		}
		if (this.ThrottleDR.value == 3)
		{
			PlayerPrefs.SetInt("ThrottleAxis", 3);
		}
		if (this.ThrottleDR.value == 4)
		{
			PlayerPrefs.SetInt("ThrottleAxis", 4);
		}
		if (this.ThrottleDR.value == 5)
		{
			PlayerPrefs.SetInt("ThrottleAxis", 5);
		}
		if (this.ThrottleDR.value == 6)
		{
			PlayerPrefs.SetInt("ThrottleAxis", 6);
		}
		if (this.ThrottleDR.value == 7)
		{
			PlayerPrefs.SetInt("ThrottleAxis", 7);
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x0007DB8C File Offset: 0x0007BD8C
	public void SetBrakegAxis()
	{
		if (this.BrakesDR.value == 0)
		{
			PlayerPrefs.SetInt("BrakeAxis", 0);
		}
		if (this.BrakesDR.value == 1)
		{
			PlayerPrefs.SetInt("BrakeAxis", 1);
		}
		if (this.BrakesDR.value == 2)
		{
			PlayerPrefs.SetInt("BrakeAxis", 2);
		}
		if (this.BrakesDR.value == 3)
		{
			PlayerPrefs.SetInt("BrakeAxis", 3);
		}
		if (this.BrakesDR.value == 4)
		{
			PlayerPrefs.SetInt("BrakeAxis", 4);
		}
		if (this.BrakesDR.value == 5)
		{
			PlayerPrefs.SetInt("BrakeAxis", 5);
		}
		if (this.BrakesDR.value == 6)
		{
			PlayerPrefs.SetInt("BrakeAxis", 6);
		}
		if (this.BrakesDR.value == 7)
		{
			PlayerPrefs.SetInt("BrakeAxis", 7);
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x0007DC68 File Offset: 0x0007BE68
	public void SetClutchAxis()
	{
		if (this.ClutchDR.value == 0)
		{
			PlayerPrefs.SetInt("ClutchAxis", 0);
		}
		if (this.ClutchDR.value == 1)
		{
			PlayerPrefs.SetInt("ClutchAxis", 1);
		}
		if (this.ClutchDR.value == 2)
		{
			PlayerPrefs.SetInt("ClutchAxis", 2);
		}
		if (this.ClutchDR.value == 3)
		{
			PlayerPrefs.SetInt("ClutchAxis", 3);
		}
		if (this.ClutchDR.value == 4)
		{
			PlayerPrefs.SetInt("ClutchAxis", 4);
		}
		if (this.ClutchDR.value == 5)
		{
			PlayerPrefs.SetInt("ClutchAxis", 5);
		}
		if (this.ClutchDR.value == 6)
		{
			PlayerPrefs.SetInt("ClutchAxis", 6);
		}
		if (this.ClutchDR.value == 7)
		{
			PlayerPrefs.SetInt("ClutchAxis", 7);
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0007DD44 File Offset: 0x0007BF44
	public void SetClutch()
	{
		if (this.AutoClutch)
		{
			this.AutoClutch = false;
			this.AutoClutchToggle.isOn = false;
			PlayerPrefs.SetFloat("AutoClutch", 0f);
			PlayerPrefs.Save();
			return;
		}
		this.AutoClutch = true;
		this.AutoClutchToggle.isOn = true;
		PlayerPrefs.SetFloat("AutoClutch", 1f);
		PlayerPrefs.Save();
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0007DDA8 File Offset: 0x0007BFA8
	public void SetHh()
	{
		if (this.UseH)
		{
			this.UseH = false;
			this.UseHToggle.isOn = false;
			PlayerPrefs.SetFloat("UseH", 0f);
			PlayerPrefs.Save();
			return;
		}
		this.UseH = true;
		this.UseHToggle.isOn = true;
		PlayerPrefs.SetFloat("UseH", 1f);
		PlayerPrefs.Save();
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x0007DE0C File Offset: 0x0007C00C
	public void wUp()
	{
		this.waitUp = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x0007DE21 File Offset: 0x0007C021
	public void wDown()
	{
		this.waitDown = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x0007DE36 File Offset: 0x0007C036
	public void wR()
	{
		this.waitR = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x0007DE4B File Offset: 0x0007C04B
	public void w1()
	{
		this.wait1 = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x0007DE60 File Offset: 0x0007C060
	public void w2()
	{
		this.wait2 = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x0007DE75 File Offset: 0x0007C075
	public void w3()
	{
		this.wait3 = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x0007DE8A File Offset: 0x0007C08A
	public void w4()
	{
		this.wait4 = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x0007DE9F File Offset: 0x0007C09F
	public void w5()
	{
		this.wait5 = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x0007DEB4 File Offset: 0x0007C0B4
	public void w6()
	{
		this.wait6 = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x0007DEC9 File Offset: 0x0007C0C9
	public void wW()
	{
		this.waitW = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x0007DEDE File Offset: 0x0007C0DE
	public void wT()
	{
		this.waitT = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x0007DEF3 File Offset: 0x0007C0F3
	public void wB()
	{
		this.waitB = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x0007DF08 File Offset: 0x0007C108
	public void wC()
	{
		this.waitC = true;
		this.PressText.SetActive(true);
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x0007DF20 File Offset: 0x0007C120
	private void Update()
	{
		if (Input.GetKey(KeyCode.JoystickButton0))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 0);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 0);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 0);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 0);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 0);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 0);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 0);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 0);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 0);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton1))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 1);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 1);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 1);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 1);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 1);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 1);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 1);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 1);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 1);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton2))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 2);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 2);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 2);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 2);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 2);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 2);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 2);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 2);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 2);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton3))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 3);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 3);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 3);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 3);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 3);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 3);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 3);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 3);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 3);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton4))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 4);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 4);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 4);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 4);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 4);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 4);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 4);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 4);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 4);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton5))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 5);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 5);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 5);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 5);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 5);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 5);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 5);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 5);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 5);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton6))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 6);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 6);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 6);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 6);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 6);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 6);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 6);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 6);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 6);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton7))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 7);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 7);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 7);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 7);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 7);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 7);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 7);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 7);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 7);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton8))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 8);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 8);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 8);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 8);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 8);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 8);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 8);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 8);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 8);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton9))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 9);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 9);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 9);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 9);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 9);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 9);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 9);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 9);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 9);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton10))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 10);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 10);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 10);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 10);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 10);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 10);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 10);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 10);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 10);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton11))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 11);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 11);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 11);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 11);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 11);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 11);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 11);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 11);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 11);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton12))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 12);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 12);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 12);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 12);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 12);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 12);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 12);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 12);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 12);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton13))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 13);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 13);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 13);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 13);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 13);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 13);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 13);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 13);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 13);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton14))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 14);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 14);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 14);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 14);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 14);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 14);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 14);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 14);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 14);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton15))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 15);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 15);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 15);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 15);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 15);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 15);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 15);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 15);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 15);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton16))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 16);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 16);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 16);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 16);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 16);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 16);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 16);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 16);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton1))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 17);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 17);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 17);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 17);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 17);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 17);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 17);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 17);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 17);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton18))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 18);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 18);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 18);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 18);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 18);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 18);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 18);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 18);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 18);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
		if (Input.GetKey(KeyCode.JoystickButton19))
		{
			if (this.waitUp)
			{
				PlayerPrefs.SetInt("waitUp", 19);
			}
			if (this.waitDown)
			{
				PlayerPrefs.SetInt("waitDown", 19);
			}
			if (this.wait1)
			{
				PlayerPrefs.SetInt("wait1", 19);
			}
			if (this.wait2)
			{
				PlayerPrefs.SetInt("wait2", 19);
			}
			if (this.wait3)
			{
				PlayerPrefs.SetInt("wait3", 19);
			}
			if (this.wait4)
			{
				PlayerPrefs.SetInt("wait4", 19);
			}
			if (this.wait5)
			{
				PlayerPrefs.SetInt("wait5", 19);
			}
			if (this.wait6)
			{
				PlayerPrefs.SetInt("wait6", 19);
			}
			if (this.waitR)
			{
				PlayerPrefs.SetInt("waitR", 19);
			}
			PlayerPrefs.Save();
			this.waitUp = false;
			this.waitDown = false;
			this.waitR = false;
			this.wait1 = false;
			this.wait2 = false;
			this.wait3 = false;
			this.wait4 = false;
			this.wait5 = false;
			this.wait6 = false;
			this.waitW = false;
			this.waitT = false;
			this.waitB = false;
			this.waitC = false;
			this.PressText.SetActive(false);
			this.ResetText();
		}
	}

	// Token: 0x040013D7 RID: 5079
	public Text rotationText;

	// Token: 0x040013D8 RID: 5080
	public float wheelrotation;

	// Token: 0x040013D9 RID: 5081
	public Slider WheelSlider;

	// Token: 0x040013DA RID: 5082
	public GameObject PressText;

	// Token: 0x040013DB RID: 5083
	public bool waitSt;

	// Token: 0x040013DC RID: 5084
	public bool waitTh;

	// Token: 0x040013DD RID: 5085
	public bool waitCl;

	// Token: 0x040013DE RID: 5086
	public bool waitUp;

	// Token: 0x040013DF RID: 5087
	public bool waitDown;

	// Token: 0x040013E0 RID: 5088
	public bool waitR;

	// Token: 0x040013E1 RID: 5089
	public bool wait1;

	// Token: 0x040013E2 RID: 5090
	public bool wait2;

	// Token: 0x040013E3 RID: 5091
	public bool wait3;

	// Token: 0x040013E4 RID: 5092
	public bool wait4;

	// Token: 0x040013E5 RID: 5093
	public bool wait5;

	// Token: 0x040013E6 RID: 5094
	public bool wait6;

	// Token: 0x040013E7 RID: 5095
	public bool waitW;

	// Token: 0x040013E8 RID: 5096
	public bool waitT;

	// Token: 0x040013E9 RID: 5097
	public bool waitB;

	// Token: 0x040013EA RID: 5098
	public bool waitC;

	// Token: 0x040013EB RID: 5099
	public Text Up;

	// Token: 0x040013EC RID: 5100
	public Text Down;

	// Token: 0x040013ED RID: 5101
	public Text WR;

	// Token: 0x040013EE RID: 5102
	public Text W1;

	// Token: 0x040013EF RID: 5103
	public Text W2;

	// Token: 0x040013F0 RID: 5104
	public Text W3;

	// Token: 0x040013F1 RID: 5105
	public Text W4;

	// Token: 0x040013F2 RID: 5106
	public Text W5;

	// Token: 0x040013F3 RID: 5107
	public Text W6;

	// Token: 0x040013F4 RID: 5108
	public Text WW;

	// Token: 0x040013F5 RID: 5109
	public Text TT;

	// Token: 0x040013F6 RID: 5110
	public Text BB;

	// Token: 0x040013F7 RID: 5111
	public Text CC;

	// Token: 0x040013F8 RID: 5112
	public Dropdown SteeringDR;

	// Token: 0x040013F9 RID: 5113
	public Dropdown ThrottleDR;

	// Token: 0x040013FA RID: 5114
	public Dropdown BrakesDR;

	// Token: 0x040013FB RID: 5115
	public Dropdown ClutchDR;

	// Token: 0x040013FC RID: 5116
	public bool AutoClutch;

	// Token: 0x040013FD RID: 5117
	public Toggle AutoClutchToggle;

	// Token: 0x040013FE RID: 5118
	public bool UseH;

	// Token: 0x040013FF RID: 5119
	public Toggle UseHToggle;
}
