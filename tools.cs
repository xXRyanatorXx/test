using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assets.SimpleLocalization;
using GleyTrafficSystem;
using GlobalSnowEffect;
using NWH.VehiclePhysics2;
using NWH.VehiclePhysics2.Input;
using Rewired;
using Rewired.UI.ControlMapper;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020001BB RID: 443
public class tools : MonoBehaviour
{
	// Token: 0x06000A4E RID: 2638
	[DllImport("user32.dll")]
	private static extern bool SetCursorPos(int X, int Y);

	// Token: 0x06000A4F RID: 2639 RVA: 0x00065F38 File Offset: 0x00064138
	private void Awake()
	{
		if (this.PlayerCam.GetComponent<GlobalSnow>() && PlayerPrefs.HasKey("WinterOn") && PlayerPrefs.GetFloat("WinterOn") == 1f)
		{
			UnityEngine.Object.Destroy(this.PlayerCam.GetComponent<GlobalSnow>());
		}
		if (!this.MapMagic && PlayerPrefs.HasKey("MapExtension") && PlayerPrefs.GetFloat("MapExtension") == 1f)
		{
			this.EnableExtensionMap();
		}
		if (this.MapMagic)
		{
			tools._MapMagic = this.MapMagic;
		}
		tools.StartCooldown = true;
		base.StartCoroutine(this.WaitStart());
		Debug.Log(Application.version);
		tools.MPNumber = 0;
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x00065FF0 File Offset: 0x000641F0
	private void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
		this.ControlsObj = GameObject.Find("ControlMapper");
		tools.GameLoaded = false;
		tools.UIisOpen = false;
		tools.PizzaDeliveriesCount = 0;
		tools.PizzaDeliveriesDay = 0;
		this.timer = 0f;
		if (this.Backpack != null)
		{
			this.BackpackParts = new List<GameObject>();
		}
		Time.timeScale = 1f;
		tools.Snapping = true;
		tools.HorGrid = false;
		tools.VertGrid = false;
		if (this.WheelSlider != null)
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
		}
		if (PlayerPrefs.HasKey("Quality"))
		{
			if (PlayerPrefs.GetFloat("Quality") == 0f)
			{
				this.QualityText.text = "Very Low";
				QualitySettings.SetQualityLevel(0, true);
				this.Quality = 0f;
			}
			if (PlayerPrefs.GetFloat("Quality") == 1f)
			{
				this.QualityText.text = "Low";
				QualitySettings.SetQualityLevel(1, true);
				this.Quality = 1f;
			}
			if (PlayerPrefs.GetFloat("Quality") == 2f)
			{
				this.QualityText.text = "Medium";
				QualitySettings.SetQualityLevel(2, true);
				this.Quality = 2f;
			}
			if (PlayerPrefs.GetFloat("Quality") == 3f)
			{
				this.QualityText.text = "High";
				QualitySettings.SetQualityLevel(3, true);
				this.Quality = 3f;
			}
			if (PlayerPrefs.GetFloat("Quality") == 4f)
			{
				this.QualityText.text = "Very High";
				QualitySettings.SetQualityLevel(4, true);
				this.Quality = 4f;
			}
			if (PlayerPrefs.GetFloat("Quality") == 5f)
			{
				this.QualityText.text = "Ultra";
				QualitySettings.SetQualityLevel(5, true);
				this.Quality = 5f;
			}
		}
		else if (PlayerPrefs.GetFloat("Quality") == 3f)
		{
			this.QualityText.text = "High";
			QualitySettings.SetQualityLevel(3, true);
			this.Quality = 3f;
		}
		if (this.IslandProbes)
		{
			if (PlayerPrefs.HasKey("ReflectionQuality"))
			{
				if (PlayerPrefs.GetFloat("ReflectionQuality") == 0f)
				{
					this.ReflectionQualityText.text = "Basic";
					this.ReflectionQuality = 0f;
					this.IslandProbes.SetActive(false);
					this.PlayerProbe.SetActive(false);
				}
				if (PlayerPrefs.GetFloat("ReflectionQuality") == 1f)
				{
					this.ReflectionQualityText.text = "Good";
					this.IslandProbes.SetActive(true);
					this.ReflectionQuality = 1f;
				}
				if (PlayerPrefs.GetFloat("ReflectionQuality") == 2f)
				{
					this.ReflectionQualityText.text = "Best(experimental)";
					this.PlayerProbe.SetActive(true);
					this.ReflectionQuality = 2f;
				}
			}
			else
			{
				this.ReflectionQualityText.text = "Basic";
				this.ReflectionQuality = 0f;
				this.IslandProbes.SetActive(false);
				this.PlayerProbe.SetActive(false);
			}
		}
		if (this.TextureQualityText != null)
		{
			if (PlayerPrefs.HasKey("TextureQuality"))
			{
				if (PlayerPrefs.GetFloat("TextureQuality") == 0f)
				{
					this.TextureQualityText.GetComponent<Text>().color = Color.black;
					this.TextureQualityText.text = "512 X 512";
					tools.TextureQuality = 0f;
				}
				if (PlayerPrefs.GetFloat("TextureQuality") == 1f)
				{
					this.TextureQualityText.GetComponent<Text>().color = Color.black;
					this.TextureQualityText.text = "1024 X 1024";
					tools.TextureQuality = 1f;
				}
				if (PlayerPrefs.GetFloat("TextureQuality") == 2f)
				{
					this.TextureQualityText.GetComponent<Text>().color = Color.red;
					this.TextureQualityText.text = "2048 X 2048";
					tools.TextureQuality = 2f;
				}
			}
			else
			{
				this.TextureQualityText.text = "512 X 512";
				tools.TextureQuality = 0f;
			}
		}
		if (PlayerPrefs.HasKey("AutosaveTimer"))
		{
			if (PlayerPrefs.GetFloat("AutosaveTimer") == 900f)
			{
				this.AutosaveTimer = 900f;
				this.AutosaveText.text = "Autosave - 15min";
			}
			else if (PlayerPrefs.GetFloat("AutosaveTimer") == 1800f)
			{
				this.AutosaveTimer = 1800f;
				this.AutosaveText.text = "Autosave - 30min";
			}
			else if (PlayerPrefs.GetFloat("AutosaveTimer") == 3600f)
			{
				this.AutosaveTimer = 3600f;
				this.AutosaveText.text = "Autosave - 1h";
			}
			else if (PlayerPrefs.GetFloat("AutosaveTimer") == 0f)
			{
				this.AutosaveTimer = 0f;
				this.AutosaveText.text = "Autosave - Off";
			}
		}
		if (PlayerPrefs.HasKey("Quality"))
		{
			if (PlayerPrefs.GetFloat("Quality") == 0f)
			{
				QualitySettings.SetQualityLevel(0, true);
			}
			if (PlayerPrefs.GetFloat("Quality") == 1f)
			{
				QualitySettings.SetQualityLevel(1, true);
			}
			if (PlayerPrefs.GetFloat("Quality") == 2f)
			{
				QualitySettings.SetQualityLevel(2, true);
			}
			if (PlayerPrefs.GetFloat("Quality") == 3f)
			{
				QualitySettings.SetQualityLevel(3, true);
			}
			if (PlayerPrefs.GetFloat("Quality") == 4f)
			{
				QualitySettings.SetQualityLevel(4, true);
			}
			if (PlayerPrefs.GetFloat("Quality") == 5f)
			{
				QualitySettings.SetQualityLevel(5, true);
			}
		}
		base.StartCoroutine(this.RestartTool());
		if (!float.IsNaN(tools.money))
		{
			this.StartMoney = tools.money;
		}
		else
		{
			tools.money = this.StartMoney;
		}
		tools.AutoClutch = false;
		tools.Racing = false;
		tools.electricity = false;
		tools.JunkZone = false;
		tools.cooldown = false;
		tools.Clicked = false;
		tools.sitting = false;
		tools.cansit = false;
		tools.canput = false;
		tools.canrepair = false;
		tools.canfair = false;
		tools.canremove = false;
		tools.cantake = false;
		tools.filling = false;
		tools.holding = false;
		tools.holdingitem = false;
		tools.tool = 1;
		tools.helditem = "Nothing";
		tools.lookitem = "Nothing";
		tools.ToolHand = GameObject.Find("ToolHand");
		if (this.buildPos)
		{
			tools._buildPos = this.buildPos;
		}
		if (this.buildparent)
		{
			tools._buildparent = this.buildparent;
		}
		this.HandStatic = GameObject.Find("handStatic");
		this.hand1 = GameObject.Find("hand");
		this.handPaint = GameObject.Find("PaintHand");
		this.handPour = GameObject.Find("PourHand");
		this.ReadHand = GameObject.Find("ReadHand");
		this.AudioParent = GameObject.Find("hand");
		tools.AudioParent_ = this.AudioParent;
		if (PlayerPrefs.HasKey("DirectSteer") && PlayerPrefs.GetFloat("DirectSteer") == 1f)
		{
			tools.DirectSteer = true;
			this.DirectSteerToggle.isOn = true;
		}
		else
		{
			tools.DirectSteer = false;
			this.DirectSteerToggle.isOn = false;
		}
		if (PlayerPrefs.GetFloat("UseWheel") == 1f)
		{
			base.GetComponent<SteeringWheelInputProvider>().enabled = true;
			this.UseWHeelToggle.isOn = true;
		}
		else
		{
			base.GetComponent<SteeringWheelInputProvider>().enabled = false;
			this.UseWHeelToggle.isOn = false;
		}
		if (PlayerPrefs.HasKey("AutoClutch") && PlayerPrefs.GetFloat("AutoClutch") == 0f)
		{
			tools.AutoClutch = false;
			this.AutoClutchToggle.isOn = false;
		}
		else
		{
			tools.AutoClutch = true;
			this.AutoClutchToggle.isOn = true;
		}
		if (PlayerPrefs.HasKey("SwappedInput") && PlayerPrefs.GetFloat("SwappedInput") == 1f)
		{
			this.SwappedInputToggle.isOn = true;
		}
		else
		{
			this.SwappedInputToggle.isOn = false;
		}
		if (PlayerPrefs.HasKey("InvSteering") && PlayerPrefs.GetFloat("InvSteering") == 1f)
		{
			this.InvSteeringToggle.isOn = true;
		}
		else
		{
			this.InvSteeringToggle.isOn = false;
		}
		if (PlayerPrefs.HasKey("InvBraking") && PlayerPrefs.GetFloat("InvBraking") == 1f)
		{
			this.InvBrakesToggle.isOn = true;
		}
		else
		{
			this.InvBrakesToggle.isOn = false;
		}
		if (PlayerPrefs.HasKey("InvThrottle") && PlayerPrefs.GetFloat("InvThrottle") == 1f)
		{
			this.InvThrottleToggle.isOn = true;
		}
		else
		{
			this.InvThrottleToggle.isOn = false;
		}
		if (PlayerPrefs.HasKey("InvClutch") && PlayerPrefs.GetFloat("InvClutch") == 1f)
		{
			this.InvClutchToggle.isOn = true;
		}
		else
		{
			this.InvClutchToggle.isOn = false;
		}
		if (PlayerPrefs.HasKey("UseHeadbob") && PlayerPrefs.GetFloat("UseHeadbob") == 1f)
		{
			this.UseHeadbobToggle.isOn = true;
			base.GetComponent<FirstPersonAIO>().UseHeadbob();
		}
		else
		{
			this.UseHeadbobToggle.isOn = false;
			base.GetComponent<FirstPersonAIO>().DontUseHeadbob();
		}
		if (PlayerPrefs.HasKey("ItemDescription") && PlayerPrefs.GetFloat("ItemDescription") == 1f && this.ItemDescriptionToggle != null)
		{
			this.ItemDescriptionToggle.isOn = false;
			this.ItemDescription = false;
		}
		else
		{
			this.ItemDescriptionToggle.isOn = true;
			this.ItemDescription = true;
		}
		if (PlayerPrefs.HasKey("VSYNC") && PlayerPrefs.GetFloat("VSYNC") == 1f && this.vsyncToggle != null)
		{
			this.vsyncToggle.isOn = false;
			this.vsync = true;
			QualitySettings.vSyncCount = 0;
		}
		else
		{
			this.vsyncToggle.isOn = true;
			this.vsync = false;
			QualitySettings.vSyncCount = 1;
		}
		if (PlayerPrefs.HasKey("Feedback"))
		{
			base.GetComponent<SteeringWheelInputProvider>().overallEffectStrength = PlayerPrefs.GetFloat("Feedback");
		}
		this.FeedbackValue.text = (Mathf.Round(base.GetComponent<SteeringWheelInputProvider>().overallEffectStrength * 100f) / 100f).ToString();
		this.ForceSlider.value = base.GetComponent<SteeringWheelInputProvider>().overallEffectStrength;
		if (PlayerPrefs.HasKey("KeyboardSensitivity"))
		{
			tools.KeyboardSensitivity = PlayerPrefs.GetFloat("KeyboardSensitivity");
		}
		else
		{
			tools.KeyboardSensitivity = 250f;
		}
		this.SensitivityValue.text = tools.KeyboardSensitivity.ToString();
		this.SensitivitySlider.value = tools.KeyboardSensitivity;
		if (this.FOVSlider && PlayerPrefs.HasKey("FOV"))
		{
			this.FOVSlider.value = PlayerPrefs.GetFloat("FOV");
			Camera.main.fieldOfView = this.FOVSlider.value;
			base.GetComponent<FirstPersonAIO>().baseCamFOV = this.FOVSlider.value;
			this.FOVValue.text = (Mathf.Round(this.FOVSlider.value * 100f) / 100f).ToString();
		}
		else if (this.FOVSlider)
		{
			this.ForceSlider.value = Camera.main.fieldOfView;
			this.FOVValue.text = (Mathf.Round(this.FOVSlider.value * 100f) / 100f).ToString();
		}
		if (this.MouseSlider && PlayerPrefs.HasKey("MouseSlider"))
		{
			this.MouseSlider.value = PlayerPrefs.GetFloat("MouseSlider");
			base.GetComponent<FirstPersonAIO>().mouseSensitivity = this.MouseSlider.value;
			Camera.main.GetComponent<carcam>().mouseSensitivity = this.MouseSlider.value;
			this.MouseValue.text = (Mathf.Round(this.MouseSlider.value * 100f) / 100f).ToString();
		}
		else if (this.MouseSlider)
		{
			this.MouseSlider.value = base.GetComponent<FirstPersonAIO>().mouseSensitivity;
			this.MouseValue.text = (Mathf.Round(this.MouseSlider.value * 100f) / 100f).ToString();
		}
		if (this.JunkSlider && PlayerPrefs.HasKey("Junk"))
		{
			this.JunkSlider.value = PlayerPrefs.GetFloat("Junk");
			this.JunkValue.text = this.JunkSlider.value.ToString();
		}
		else if (this.JunkSlider)
		{
			this.JunkSlider.value = 3f;
			this.JunkValue.text = "3";
		}
		if (this.TaxiMenu && PlayerPrefs.HasKey("TaxiOff") && PlayerPrefs.GetFloat("TaxiOff") == 1f)
		{
			this.TaxiMenu.SetActive(false);
		}
		this.HourPassed();
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x00066D33 File Offset: 0x00064F33
	private IEnumerator WaitStart()
	{
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		yield return 0;
		if (!this.Started && this.StartItemSpawnSpot)
		{
			if (PlayerPrefs.HasKey("HardStart") && PlayerPrefs.GetFloat("HardStart") == 1f)
			{
				tools.money = 2500f;
			}
			else
			{
				tools.money = 10000f;
			}
			if (this.TutorialCanvas && !Application.isEditor)
			{
				this.ESC();
				this.TutorialCanvas.SetActive(true);
				this.VideoCanvas.SetActive(true);
			}
			for (int i = 0; i < this.StartObjets.Length; i++)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.StartObjets[i], this.StartItemSpawnSpot.transform.position, Quaternion.identity).transform.name = this.StartObjets[i].transform.name;
			}
		}
		this.Started = true;
		if (this.JM)
		{
			this.JM.OnEnable();
		}
		yield break;
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x00066D42 File Offset: 0x00064F42
	public void ClearPrefs()
	{
		PlayerPrefs.DeleteAll();
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x00066D49 File Offset: 0x00064F49
	public void Unstuck()
	{
		base.transform.position += base.transform.forward * 1f;
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x00066D78 File Offset: 0x00064F78
	public void SteeringSlider()
	{
		this.wheelrotation = this.WheelSlider.value;
		this.rotationText.text = this.wheelrotation.ToString("000");
		PlayerPrefs.SetFloat("SteeringRotation", this.wheelrotation);
		PlayerPrefs.Save();
		base.GetComponent<SteeringWheelInputProvider>().wheelRotationRange = Mathf.RoundToInt(this.wheelrotation);
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x00066DDC File Offset: 0x00064FDC
	public void SetClutch()
	{
		if (tools.AutoClutch)
		{
			tools.AutoClutch = false;
			this.AutoClutchToggle.isOn = false;
			PlayerPrefs.SetFloat("AutoClutch", 0f);
			PlayerPrefs.Save();
			return;
		}
		tools.AutoClutch = true;
		this.AutoClutchToggle.isOn = true;
		PlayerPrefs.SetFloat("AutoClutch", 1f);
		PlayerPrefs.Save();
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x00066E40 File Offset: 0x00065040
	public void DirectSteering()
	{
		if (tools.DirectSteer)
		{
			tools.DirectSteer = false;
			this.DirectSteerToggle.isOn = false;
			PlayerPrefs.SetFloat("DirectSteer", 0f);
			PlayerPrefs.Save();
			return;
		}
		tools.DirectSteer = true;
		this.DirectSteerToggle.isOn = true;
		PlayerPrefs.SetFloat("DirectSteer", 1f);
		PlayerPrefs.Save();
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x00066EA4 File Offset: 0x000650A4
	public void SetSwappedInput()
	{
		if (PlayerPrefs.HasKey("SwappedInput") && PlayerPrefs.GetFloat("SwappedInput") == 1f)
		{
			this.SwappedInputToggle.isOn = false;
			PlayerPrefs.SetFloat("SwappedInput", 0f);
			PlayerPrefs.Save();
			return;
		}
		this.SwappedInputToggle.isOn = true;
		PlayerPrefs.SetFloat("SwappedInput", 1f);
		PlayerPrefs.Save();
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x00066F10 File Offset: 0x00065110
	public void InvSteering()
	{
		if (PlayerPrefs.HasKey("InvSteering") && PlayerPrefs.GetFloat("InvSteering") == 1f)
		{
			this.InvSteeringToggle.isOn = false;
			PlayerPrefs.SetFloat("InvSteering", 0f);
			PlayerPrefs.Save();
			return;
		}
		this.InvSteeringToggle.isOn = true;
		PlayerPrefs.SetFloat("InvSteering", 1f);
		PlayerPrefs.Save();
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x00066F7C File Offset: 0x0006517C
	public void InvBraking()
	{
		if (PlayerPrefs.HasKey("InvBraking") && PlayerPrefs.GetFloat("InvBraking") == 1f)
		{
			this.InvBrakesToggle.isOn = false;
			PlayerPrefs.SetFloat("InvBraking", 0f);
			PlayerPrefs.Save();
			return;
		}
		this.InvBrakesToggle.isOn = true;
		PlayerPrefs.SetFloat("InvBraking", 1f);
		PlayerPrefs.Save();
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x00066FE8 File Offset: 0x000651E8
	public void InvThrottle()
	{
		if (PlayerPrefs.HasKey("InvThrottle") && PlayerPrefs.GetFloat("InvThrottle") == 1f)
		{
			this.InvThrottleToggle.isOn = false;
			PlayerPrefs.SetFloat("InvThrottle", 0f);
			PlayerPrefs.Save();
			return;
		}
		this.InvThrottleToggle.isOn = true;
		PlayerPrefs.SetFloat("InvThrottle", 1f);
		PlayerPrefs.Save();
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x00067054 File Offset: 0x00065254
	public void InvClutch()
	{
		if (PlayerPrefs.HasKey("InvClutch") && PlayerPrefs.GetFloat("InvClutch") == 1f)
		{
			this.InvClutchToggle.isOn = false;
			PlayerPrefs.SetFloat("InvClutch", 0f);
			PlayerPrefs.Save();
			return;
		}
		this.InvClutchToggle.isOn = true;
		PlayerPrefs.SetFloat("InvClutch", 1f);
		PlayerPrefs.Save();
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x000670C0 File Offset: 0x000652C0
	public void SetForceFeedback()
	{
		base.GetComponent<SteeringWheelInputProvider>().overallEffectStrength = this.ForceSlider.value;
		this.FeedbackValue.text = (Mathf.Round(base.GetComponent<SteeringWheelInputProvider>().overallEffectStrength * 100f) / 100f).ToString();
		PlayerPrefs.SetFloat("Feedback", base.GetComponent<SteeringWheelInputProvider>().overallEffectStrength);
		PlayerPrefs.Save();
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x0006712C File Offset: 0x0006532C
	public void SetKeyboardSensitivity()
	{
		tools.KeyboardSensitivity = this.SensitivitySlider.value;
		this.SensitivityValue.text = tools.KeyboardSensitivity.ToString();
		PlayerPrefs.SetFloat("KeyboardSensitivity", tools.KeyboardSensitivity);
		PlayerPrefs.Save();
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00067168 File Offset: 0x00065368
	public void SetFOV()
	{
		base.GetComponent<FirstPersonAIO>().baseCamFOV = this.FOVSlider.value;
		Camera.main.fieldOfView = this.FOVSlider.value;
		PlayerPrefs.SetFloat("FOV", this.FOVSlider.value);
		PlayerPrefs.Save();
		this.FOVValue.text = (Mathf.Round(this.FOVSlider.value * 100f) / 100f).ToString();
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x000671EC File Offset: 0x000653EC
	public void SetJunk()
	{
		PlayerPrefs.SetFloat("Junk", this.JunkSlider.value);
		PlayerPrefs.Save();
		this.JunkValue.text = this.JunkSlider.value.ToString();
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x00067234 File Offset: 0x00065434
	public void SetMouse()
	{
		base.GetComponent<FirstPersonAIO>().mouseSensitivity = this.MouseSlider.value;
		Camera.main.GetComponent<carcam>().mouseSensitivity = this.MouseSlider.value;
		PlayerPrefs.SetFloat("MouseSlider", this.MouseSlider.value);
		PlayerPrefs.Save();
		this.MouseValue.text = (Mathf.Round(this.MouseSlider.value * 100f) / 100f).ToString();
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x000672BC File Offset: 0x000654BC
	public void UseHeadbob()
	{
		if (PlayerPrefs.HasKey("UseHeadbob") && PlayerPrefs.GetFloat("UseHeadbob") == 1f)
		{
			this.UseHeadbobToggle.isOn = false;
			PlayerPrefs.SetFloat("UseHeadbob", 0f);
			PlayerPrefs.Save();
			base.GetComponent<FirstPersonAIO>().DontUseHeadbob();
			return;
		}
		this.UseHeadbobToggle.isOn = true;
		PlayerPrefs.SetFloat("UseHeadbob", 1f);
		PlayerPrefs.Save();
		base.GetComponent<FirstPersonAIO>().UseHeadbob();
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x00067340 File Offset: 0x00065540
	public void ShowDescription()
	{
		if (PlayerPrefs.HasKey("ItemDescription") && PlayerPrefs.GetFloat("ItemDescription") == 1f)
		{
			this.ItemDescriptionToggle.isOn = true;
			PlayerPrefs.SetFloat("ItemDescription", 0f);
			this.ItemDescription = true;
			PlayerPrefs.Save();
			return;
		}
		this.ItemDescriptionToggle.isOn = false;
		PlayerPrefs.SetFloat("ItemDescription", 1f);
		PlayerPrefs.Save();
		this.ItemDescription = false;
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x000673BC File Offset: 0x000655BC
	public void VSYNC()
	{
		if (PlayerPrefs.HasKey("VSYNC") && PlayerPrefs.GetFloat("VSYNC") == 1f)
		{
			this.vsyncToggle.isOn = true;
			PlayerPrefs.SetFloat("VSYNC", 0f);
			this.vsync = false;
			PlayerPrefs.Save();
			QualitySettings.vSyncCount = 1;
			return;
		}
		this.vsyncToggle.isOn = false;
		PlayerPrefs.SetFloat("VSYNC", 1f);
		PlayerPrefs.Save();
		this.vsync = true;
		QualitySettings.vSyncCount = 0;
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00067444 File Offset: 0x00065644
	public void HourPassed()
	{
		if (EnviroSkyMgr.instance.Time.Hours == 23)
		{
			this.ResetSpawners();
		}
		if (EnviroSkyMgr.instance.Time.Hours == 6)
		{
			Manager.UpdateVehicleLights(false);
		}
		if (EnviroSkyMgr.instance.Time.Hours == 16)
		{
			Manager.UpdateVehicleLights(true);
		}
		if (this.PlayerCam.GetComponent<GlobalSnow>())
		{
			if (EnviroSkyMgr.instance.Seasons.currentSeasons == EnviroSeasons.Seasons.Winter)
			{
				if (!this.PlayerCam.GetComponent<GlobalSnow>().enabled)
				{
					this.PlayerCam.GetComponent<GlobalSnow>().enabled = true;
				}
				if (this.snowAmount < 1f)
				{
					this.snowAmount += 0.01f;
					base.StartCoroutine(this.SetSnow());
				}
			}
			else
			{
				this.snowAmount -= 0.05f;
				if (this.snowAmount <= 0f)
				{
					this.snowAmount = 0f;
					this.PlayerCam.GetComponent<GlobalSnow>().enabled = false;
				}
				else
				{
					this.PlayerCam.GetComponent<GlobalSnow>().enabled = true;
				}
			}
			this.PlayerCam.GetComponent<GlobalSnow>().snowAmount = this.snowAmount;
		}
		foreach (TimedActions timedActions in this.TimedActions)
		{
			if (timedActions != null)
			{
				timedActions.HourPassed();
			}
		}
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x000675C8 File Offset: 0x000657C8
	public void Advance4h()
	{
		if (EnviroSkyMgr.instance.Time.Hours > 19)
		{
			this.ResetSpawners();
		}
		EnviroSkyMgr.instance.SetTime(EnviroSkyMgr.instance.Time.Years, EnviroSkyMgr.instance.Time.Days, EnviroSkyMgr.instance.Time.Hours + 4, EnviroSkyMgr.instance.Time.Minutes, 0);
		BatteryCharger[] array = UnityEngine.Object.FindObjectsOfType<BatteryCharger>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].PassTime();
		}
		if (EnviroSkyMgr.instance.Time.Hours > 7 && EnviroSkyMgr.instance.Time.Hours < 19)
		{
			Manager.UpdateVehicleLights(false);
		}
		else
		{
			Manager.UpdateVehicleLights(true);
		}
		if (this.PlayerCam.GetComponent<GlobalSnow>())
		{
			if (EnviroSkyMgr.instance.Seasons.currentSeasons == EnviroSeasons.Seasons.Winter)
			{
				if (!this.PlayerCam.GetComponent<GlobalSnow>().enabled)
				{
					this.PlayerCam.GetComponent<GlobalSnow>().enabled = true;
				}
				if (this.snowAmount < 1f)
				{
					this.snowAmount += 0.2f;
					base.StartCoroutine(this.SetSnow());
				}
			}
			else
			{
				this.snowAmount -= 0.2f;
				if (this.snowAmount <= 0f)
				{
					this.snowAmount = 0f;
					this.PlayerCam.GetComponent<GlobalSnow>().enabled = false;
				}
				else
				{
					this.PlayerCam.GetComponent<GlobalSnow>().enabled = true;
				}
			}
			this.PlayerCam.GetComponent<GlobalSnow>().snowAmount = this.snowAmount;
		}
		if (base.GetComponent<FloatingPoinMyfix>())
		{
			base.GetComponent<FloatingPoinMyfix>().ResetPosition();
		}
		if (tools.Needs == 1)
		{
			if (tools.Health < 0.3f)
			{
				tools.Health = 0.3f;
			}
			tools.Sleep += 0.5f;
			tools.Food -= 0.15f;
			tools.Drink -= 0.15f;
			if (tools.Food < 0f)
			{
				tools.Food = 0f;
			}
			if (tools.Drink < 0f)
			{
				tools.Drink = 0f;
			}
			if (tools.Sleep > 1f)
			{
				tools.Sleep = 1f;
			}
			if (tools.Food > 0f && tools.Drink > 0f)
			{
				tools.Health += 0.3f;
			}
			if (tools.Health > 1f)
			{
				tools.Health = 1f;
			}
		}
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x0006784B File Offset: 0x00065A4B
	private IEnumerator SetSnow()
	{
		yield return new WaitForSeconds(1f);
		EnviroSkyMgr.instance.ChangeWeather(8);
		this.PlayerCam.GetComponent<GlobalSnow>().snowAmount = this.snowAmount;
		yield break;
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x0006785A File Offset: 0x00065A5A
	public IEnumerator Cooldown()
	{
		this.cooldownRUNNING = true;
		yield return new WaitForSeconds(2f);
		tools.cooldown = false;
		this.cooldownRUNNING = false;
		yield break;
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x00067869 File Offset: 0x00065A69
	public IEnumerator Cooldown2()
	{
		tools.cooldown2 = true;
		yield return new WaitForSeconds(2f);
		tools.cooldown2 = false;
		yield break;
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x00067871 File Offset: 0x00065A71
	public IEnumerator Cooldown3()
	{
		tools.cooldown3 = true;
		yield return new WaitForSeconds(0.2f);
		tools.cooldown3 = false;
		yield break;
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x0006787C File Offset: 0x00065A7C
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "DropParts" && other.transform.parent.name == "JunkYard" && ((this.hand.transform.childCount > 0 && (this.hand.transform.GetChild(0).gameObject.layer != LayerMask.NameToLayer("Items") || this.hand.transform.GetChild(0).GetComponent<OpenableBox>())) || this.hand.transform.childCount == 0))
		{
			MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
		}
		if (other.tag == "DropParts" && other.transform.parent.name != "JunkYard")
		{
			this.DropAll();
			MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
		}
		if (other.name == "SkipTIme")
		{
			this.CanSkipTime = true;
		}
		if (other.tag == "RACE" && !tools.Racing)
		{
			this.CanRace = true;
			this.RaceControl = other.GetComponent<RaceControl>();
		}
		if (other.tag == "AutomaticDoor")
		{
			other.GetComponent<OpenGarage>().JustOpen();
		}
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x000679C5 File Offset: 0x00065BC5
	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "DropParts" && other.transform.parent.name != "JunkYard")
		{
			MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
		}
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x000679FC File Offset: 0x00065BFC
	private void OnTriggerExit(Collider other)
	{
		if (other.name == "SkipTIme")
		{
			this.CanSkipTime = false;
		}
		if (other.tag == "RACE")
		{
			this.CanRace = false;
		}
		if (other.tag == "AutomaticDoor")
		{
			other.GetComponent<OpenGarage>().JusClose();
		}
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x00067A58 File Offset: 0x00065C58
	public void GoJunkyard()
	{
		if (tools.Racing)
		{
			return;
		}
		base.transform.position = this.JunkyardSpawn.transform.position;
		tools.money -= 10f;
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x00067A8D File Offset: 0x00065C8D
	public void GoGarage()
	{
		base.transform.position = this.GarageSpawn.transform.position;
		tools.money -= 10f;
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x00067ABA File Offset: 0x00065CBA
	public void GoSign()
	{
		base.transform.position = this.SignSpawn.transform.position;
		tools.money -= 10f;
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x00067AE7 File Offset: 0x00065CE7
	public void GoHouse()
	{
		base.transform.position = this.HouseSpawn.transform.position;
		tools.money -= 10f;
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x00067B14 File Offset: 0x00065D14
	public void ResetSpawners()
	{
		foreach (Transform transform in UnityEngine.Object.FindObjectsOfType<Transform>())
		{
			if (transform.gameObject.GetComponent<CarProperties>() && transform.gameObject.GetComponent<CarProperties>().SinglePart && (transform.gameObject.GetComponent<CarProperties>().Owner == "Junkyard" || transform.gameObject.GetComponent<CarProperties>().Owner == "Dealer"))
			{
				if (transform.GetComponent<MPobject>() && transform.GetComponent<MPobject>().networkDummy)
				{
					transform.GetComponent<MPobject>().networkDummy.DestroyMe();
				}
				else
				{
					UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
			if (transform.gameObject.GetComponent<MainCarProperties>() && (transform.gameObject.GetComponent<MainCarProperties>().Owner == "Junkyard" || transform.gameObject.GetComponent<MainCarProperties>().Owner == "Dealer"))
			{
				if (transform.GetComponent<MPobject>() && transform.GetComponent<MPobject>().networkDummy)
				{
					transform.GetComponent<MPobject>().networkDummy.DestroyMe();
				}
				else
				{
					UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
		}
		if (this.JunkYardSpawner)
		{
			this.JunkYardSpawner.SetActive(false);
		}
		if (this.CarDealerSpawner)
		{
			this.CarDealerSpawner.SetActive(false);
		}
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x00067C94 File Offset: 0x00065E94
	public void SetAutosave()
	{
		if (this.AutosaveTimer == 0f)
		{
			this.AutosaveTimer = 900f;
			PlayerPrefs.SetFloat("AutosaveTimer", 900f);
			PlayerPrefs.Save();
			this.AutosaveText.text = "Autosave - 15min";
		}
		else if (this.AutosaveTimer == 900f)
		{
			this.AutosaveTimer = 1800f;
			PlayerPrefs.SetFloat("AutosaveTimer", 1800f);
			PlayerPrefs.Save();
			this.AutosaveText.text = "Autosave - 30min";
		}
		else if (this.AutosaveTimer == 1800f)
		{
			this.AutosaveTimer = 3600f;
			PlayerPrefs.SetFloat("AutosaveTimer", 3600f);
			PlayerPrefs.Save();
			this.AutosaveText.text = "Autosave - 1h";
		}
		else if (this.AutosaveTimer == 3600f)
		{
			this.AutosaveTimer = 0f;
			PlayerPrefs.SetFloat("AutosaveTimer", 0f);
			PlayerPrefs.Save();
			this.AutosaveText.text = "Autosave - Off";
		}
		this.timer = 0f;
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x00067DA8 File Offset: 0x00065FA8
	private void Update()
	{
		if (this.reflectionProbe && this.PlayerProbe.activeSelf)
		{
			this.reflectionProbe.RenderProbe();
		}
		if (this.LastHour != EnviroSkyMgr.instance.Time.Hours)
		{
			this.HourPassed();
			this.LastHour = EnviroSkyMgr.instance.Time.Hours;
		}
		if (!tools.sitting)
		{
			this.timer += Time.deltaTime;
		}
		if (this.SwitchTimer > 0f && this.ActiveSwitch)
		{
			this.SwitchTimer -= Time.deltaTime;
		}
		else
		{
			this.ActiveSwitch = null;
		}
		if (this.timer >= this.AutosaveTimer && this.AutosaveTimer != 0f)
		{
			this.timer = 0f;
			this.Savingtext.SetActive(true);
			this.saver.Quicksave();
		}
		if (tools.cooldown && !this.cooldownRUNNING)
		{
			tools.cooldown = false;
		}
		if (tools.JunkZone)
		{
			tools.tool = 18;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 1.7f, 1 << LayerMask.NameToLayer("TransparentParts")))
		{
			tools.LookingAtTransparent = true;
		}
		else
		{
			tools.LookingAtTransparent = false;
		}
		if (this.player.GetButtonDown("Sit in car"))
		{
			if (this.CanSkipTime)
			{
				this.Advance4h();
			}
			if (this.CanRace)
			{
				tools.Racing = true;
				this.RaceControl.AcceptedRace();
				this.CanRace = false;
			}
			if (this.RaceControl)
			{
				this.RaceControl.PressedEnter();
			}
		}
		RaycastHit raycastHit2;
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit2, 1.7f, 1 << LayerMask.NameToLayer("Items")) && raycastHit2.collider.GetComponent<SaleItem>())
		{
			raycastHit2.collider.GetComponent<SaleItem>().BUY();
		}
		if (Input.GetMouseButtonUp(0))
		{
			tools.Clicked = false;
			this.PlayerCam.GetComponent<PlayerRayCasting>().highlightColor = Color.green;
		}
		tools.lookitem = "";
		this.MoneyText.text = "";
		this.MoneyText2.text = "";
		this.PriceText.text = "";
		this.TipsText.text = "";
		this.TipsTextRed.text = "";
		this.LookingButtonText.text = "";
		this.LookingText.text = "";
		this.LookingFitCarText.text = "";
		this.LookingFitEngineText.text = "";
		if (tools.sitting)
		{
			this.GearGauge.text = tools.exp.powertrain.transmission.GearName.ToString();
		}
		else
		{
			this.GearGauge.text = "";
		}
		RaycastHit raycastHit3;
		if (tools.sitting && base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().exp.speed > 3f && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit3, 1.7f, 1 << LayerMask.NameToLayer("OpenableParts")) && raycastHit3.collider.GetComponent<Switch>())
		{
			this.LookingButtonText.text = raycastHit3.collider.GetComponent<Switch>().SwitchName;
			this.ActiveSwitch = raycastHit3.collider.GetComponent<Switch>();
			this.SwitchTimer = 0.5f;
		}
		RaycastHit raycastHit4;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit4, 1.7f, 1 << LayerMask.NameToLayer("VisibleParts") | 1 << LayerMask.NameToLayer("Windows") | 1 << LayerMask.NameToLayer("LooseParts") | 1 << LayerMask.NameToLayer("OpenableParts") | 1 << LayerMask.NameToLayer("Items") | 1 << LayerMask.NameToLayer("Repair")))
		{
			if (raycastHit4.collider.transform.name == "TrashBin" && this.hand.transform.childCount > 0)
			{
				if (this.hand.transform.GetChild(0).GetComponent<Partinfo>() && this.hand.transform.GetChild(0).GetComponent<CarProperties>().Owner != "Client")
				{
					this.TipsText.text = LocalizationManager.Localize("Drop to sell").ToString();
					this.PriceText.text = Mathf.Round(this.hand.transform.GetChild(0).GetComponent<Partinfo>().price * 0.05f).ToString() + "$";
				}
				if (this.hand.transform.GetChild(0).gameObject.layer == LayerMask.NameToLayer("Items"))
				{
					this.TipsText.text = LocalizationManager.Localize("Drop to Dispose").ToString();
				}
				if (this.hand.transform.GetChild(0).GetComponent<Partinfo>() && this.hand.transform.GetChild(0).GetComponent<CarProperties>().Owner == "Client")
				{
					this.TipsText.text = LocalizationManager.Localize("Finish job to sell these parts").ToString();
					this.PriceText.text = "";
				}
			}
			if (raycastHit4.collider.transform.name == "PawnShop" && this.hand.transform.childCount > 0)
			{
				if (this.hand.transform.GetChild(0).GetComponent<Partinfo>() && this.hand.transform.GetChild(0).GetComponent<CarProperties>().Owner != "Client")
				{
					this.BuyPrice = 0f;
					foreach (CarProperties carProperties in this.hand.transform.GetChild(0).transform.GetComponentsInChildren<CarProperties>())
					{
						if (carProperties.SinglePart && carProperties.Owner != "Client")
						{
							this.BuyPrice += carProperties.gameObject.transform.GetComponent<Partinfo>().price * carProperties.Condition * 0.6f;
						}
					}
					this.TipsText.text = LocalizationManager.Localize("Drop to sell").ToString();
					this.PriceText.text = Mathf.Round(this.BuyPrice).ToString() + "$";
				}
				if (this.hand.transform.GetChild(0).gameObject.layer == LayerMask.NameToLayer("Items"))
				{
					this.TipsText.text = LocalizationManager.Localize("Drop to Dispose").ToString();
				}
				if (this.hand.transform.GetChild(0).gameObject.transform.GetComponent<MooveItem>())
				{
					this.TipsText.text = LocalizationManager.Localize("Drop to sell").ToString();
					this.PriceText.text = Mathf.Round((float)this.hand.transform.GetChild(0).gameObject.transform.GetComponent<MooveItem>().price).ToString() + "$";
				}
			}
			if (raycastHit4.collider.transform.name == "Jack" || raycastHit4.collider.transform.name == "FllorJack")
			{
				this.TipsText.text = LocalizationManager.Localize("Lower to pick").ToString();
			}
			if (raycastHit4.collider.transform.parent == null || raycastHit4.collider.transform.parent.name == "SHOPITEMS" || raycastHit4.collider.transform.parent.name == "Props")
			{
				if (raycastHit4.collider.GetComponent<CarProperties>())
				{
					if (raycastHit4.collider.GetComponent<CarProperties>().PREFAB && raycastHit4.collider.GetComponent<CarProperties>().PREFAB.GetComponent<CarProperties>().PartName != "")
					{
						tools.lookitem = raycastHit4.collider.GetComponent<CarProperties>().PREFAB.GetComponent<CarProperties>().PartName;
					}
					else
					{
						tools.lookitem = raycastHit4.collider.gameObject.name.Remove(raycastHit4.collider.gameObject.name.Length - 2);
					}
				}
				else
				{
					tools.lookitem = raycastHit4.collider.gameObject.name;
				}
			}
			if (tools.tool == 18)
			{
				if (raycastHit4.collider.GetComponent<CarProperties>() && !raycastHit4.collider.transform.root.GetComponent<MainCarProperties>())
				{
					if (raycastHit4.collider.GetComponent<CarProperties>().PREFAB && raycastHit4.collider.GetComponent<CarProperties>().PREFAB.GetComponent<CarProperties>().PartName != "")
					{
						tools.lookitem = raycastHit4.collider.GetComponent<CarProperties>().PREFAB.GetComponent<CarProperties>().PartName;
					}
					else
					{
						tools.lookitem = raycastHit4.collider.gameObject.name.Remove(raycastHit4.collider.gameObject.name.Length - 2);
					}
				}
				else if (!raycastHit4.collider.GetComponent<CarProperties>())
				{
					tools.lookitem = raycastHit4.collider.gameObject.name;
				}
			}
			if (raycastHit4.collider.transform.name == "WaterHose")
			{
				tools.lookitem = "Water Hose";
			}
			if (raycastHit4.collider.transform.name == "FuelHose")
			{
				tools.lookitem = "Fuel Pistol";
			}
			if (raycastHit4.collider.transform.name == "Hook")
			{
				tools.lookitem = "Hook";
			}
			if (raycastHit4.collider.GetComponent<SaleItem>())
			{
				if (raycastHit4.collider.GetComponent<SaleItem>().GasPay)
				{
					this.PriceText.text = (Mathf.Round(raycastHit4.collider.GetComponent<SaleItem>().AllMoney * 100f) / 100f).ToString() + "$";
					this.TipsText.text = LocalizationManager.Localize("Click to pay").ToString();
				}
				else
				{
					this.PriceText.text = raycastHit4.collider.GetComponent<SaleItem>().Price.ToString() + "$";
					this.TipsText.text = (this.TipsText.text = LocalizationManager.Localize("Click to buy").ToString());
				}
			}
			if (tools.tool == 19)
			{
				this.TipsText.text = LocalizationManager.Localize("Middle mouse to change").ToString();
			}
			if (raycastHit4.collider.GetComponent<CarProperties>() && raycastHit4.collider.GetComponent<CarProperties>().Owner == "Junkyard" && !raycastHit4.collider.transform.root.GetComponent<MainCarProperties>())
			{
				this.BuyPrice = 0f;
				CarProperties[] componentsInChildren = raycastHit4.collider.transform.GetComponentsInChildren<CarProperties>();
				foreach (CarProperties carProperties2 in componentsInChildren)
				{
					if (carProperties2.SinglePart && carProperties2.Owner != "Client")
					{
						this.BuyPrice += carProperties2.gameObject.transform.GetComponent<Partinfo>().price * carProperties2.Condition * 0.6f;
					}
				}
				this.PriceText.text = Mathf.Round(this.BuyPrice).ToString() + "$";
				this.TipsText.text = LocalizationManager.Localize("Right click to buy").ToString();
				if (Input.GetMouseButtonDown(1) && tools.money >= this.BuyPrice)
				{
					foreach (CarProperties carProperties3 in componentsInChildren)
					{
						if (carProperties3.SinglePart && carProperties3.Owner != "Client")
						{
							carProperties3.Owner = "Player";
						}
					}
					if (tools.MPrunning)
					{
						tools.NetworkPLayer.pickup(raycastHit4.collider.transform.GetComponent<MPobject>().networkDummy);
						raycastHit4.collider.transform.GetComponent<MPobject>().networkDummy.SetOwnerPlayer();
					}
					tools.money -= this.BuyPrice;
					raycastHit4.collider.transform.position = this.SpawnPoint.transform.position;
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
				}
			}
			if (raycastHit4.collider.GetComponent<IgnitionKey>())
			{
				this.LookingButtonText.text = LocalizationManager.Localize("Ignition").ToString();
			}
			if (raycastHit4.collider.GetComponent<WindowLift>())
			{
				this.LookingButtonText.text = LocalizationManager.Localize("OpenWindow").ToString();
			}
			if (raycastHit4.collider.GetComponent<WindowLift>() && raycastHit4.collider.GetComponent<WindowLift>().Winch)
			{
				this.LookingButtonText.text = "Handle";
			}
			if (raycastHit4.collider.GetComponent<Book>())
			{
				this.LookingButtonText.text = raycastHit4.collider.transform.name;
			}
			if (raycastHit4.collider.GetComponent<HandbrakeScr>())
			{
				this.LookingButtonText.text = LocalizationManager.Localize("Handbrake").ToString();
			}
			if (raycastHit4.collider.GetComponent<PickupCup>() && raycastHit4.collider.GetComponent<PickupCup>().Dipstick)
			{
				this.LookingButtonText.text = LocalizationManager.Localize("Check Oil").ToString();
			}
			if (raycastHit4.collider.GetComponent<MainCarProperties>())
			{
				tools.lookitem = "";
			}
			if (this.ActiveSwitch)
			{
				this.LookingButtonText.text = this.ActiveSwitch.SwitchName;
			}
			if (tools.lookitem != "")
			{
				this.LookingText.text = LocalizationManager.Localize(tools.lookitem).ToString();
				if (raycastHit4.collider.GetComponent<CarProperties>() && !raycastHit4.collider.transform.parent && raycastHit4.collider.GetComponent<CarProperties>().PREFAB)
				{
					this.LookingText.text = this.LookingText.text + raycastHit4.collider.GetComponent<CarProperties>().PREFAB.GetComponent<CarProperties>().PartNameExtension;
					if (this.ItemDescription)
					{
						this.LookingFitCarText.text = string.Join(" ", raycastHit4.collider.GetComponent<CarProperties>().PREFAB.GetComponent<Partinfo>().FitsToCar);
						this.LookingFitEngineText.text = string.Join(" ", raycastHit4.collider.GetComponent<CarProperties>().PREFAB.GetComponent<Partinfo>().FitsToEngine);
					}
				}
				if (raycastHit4.collider.GetComponent<SaleItem>() && raycastHit4.collider.GetComponent<SaleItem>().Item.GetComponent<CarProperties>() && raycastHit4.collider.GetComponent<SaleItem>().Item.GetComponent<CarProperties>().PREFAB)
				{
					this.LookingFitCarText.text = string.Join(" ", raycastHit4.collider.GetComponent<SaleItem>().Item.GetComponent<CarProperties>().PREFAB.GetComponent<Partinfo>().FitsToCar);
					this.LookingFitEngineText.text = string.Join(" ", raycastHit4.collider.GetComponent<SaleItem>().Item.GetComponent<CarProperties>().PREFAB.GetComponent<Partinfo>().FitsToEngine);
				}
			}
			else
			{
				this.LookingText.text = "";
			}
			if (raycastHit4.collider.gameObject.name == "SprayCan" && raycastHit4.collider.gameObject.GetComponent<PickupTool>())
			{
				this.LookingText.text = LocalizationManager.Localize(tools.lookitem) + " " + ColorUtility.ToHtmlStringRGB(raycastHit4.collider.gameObject.GetComponent<PickupTool>().colorpaint);
			}
			if (raycastHit4.collider.gameObject.GetComponent<PickupTool>() && raycastHit4.collider.gameObject.GetComponent<PickupTool>().MONEY)
			{
				this.LookingText.text = "$ " + raycastHit4.collider.gameObject.GetComponent<PickupTool>().paintlife.ToString();
			}
			if (raycastHit4.collider.gameObject.GetComponent<PickupTool>() && raycastHit4.collider.gameObject.GetComponent<PickupTool>().Box)
			{
				this.LookingText.text = LocalizationManager.Localize(tools.lookitem) + " Inside " + raycastHit4.collider.gameObject.GetComponent<PickupTool>().InBox.ToString();
			}
			if (raycastHit4.collider.gameObject.GetComponent<PickupTool>() && raycastHit4.collider.gameObject.GetComponent<PickupTool>().Garbagebag && raycastHit4.collider.gameObject.GetComponent<PickupTool>().paintlife < 1f)
			{
				this.LookingText.text = "Full ";
			}
			if (raycastHit4.collider.gameObject.GetComponent<PickupTool>() && raycastHit4.collider.gameObject.GetComponent<PickupTool>().DescriptionText != "")
			{
				this.TipsText.text = LocalizationManager.Localize(raycastHit4.collider.gameObject.GetComponent<PickupTool>().DescriptionText).ToString();
			}
		}
		if (tools.tool == 14 || tools.tool == 16 || !base.GetComponent<FirstPersonAIO>().enableCameraMovement)
		{
			this.TipsText.text = LocalizationManager.Localize("Press L ALT to toggle screen freeze").ToString();
		}
		if (tools.tool == 2 || tools.tool == 3 || tools.tool == 8)
		{
			if (!tools.Tighten)
			{
				this.TipsText.text = "- LOOSENING -\nPress F to switch between tightening modes".ToString();
			}
			else
			{
				this.TipsText.text = "- TIGHTENING -\nPress F to switch between tightening modes".ToString();
			}
		}
		if (tools.cansit && !tools.sitting)
		{
			this.TipsText.text = LocalizationManager.Localize("Press ENTER to sit\nPress BACKSPACE to sleep\nPress F1 to toggle mouse steering\nif mouse steering enabled hold RMB to look ").ToString();
		}
		if (this.CanSkipTime)
		{
			this.TipsText.text = "Press ENTER to sleep";
		}
		if (this.CanRace)
		{
			this.TipsText.text = "Press ENTER to participate\n" + this.RaceControl.Description;
		}
		if (tools.tool == 41 || tools.tool == 43 || tools.tool == 44)
		{
			if (!this.Buildingcanvas.activeSelf)
			{
				this.Buildingcanvas.SetActive(true);
			}
			if (Input.GetKeyDown("1"))
			{
				if (tools.Snapping)
				{
					tools.Snapping = false;
				}
				else
				{
					tools.Snapping = true;
				}
			}
			if (Input.GetKeyDown("2"))
			{
				if (tools.HorGrid)
				{
					tools.HorGrid = false;
				}
				else
				{
					tools.HorGrid = true;
				}
			}
			if (Input.GetKeyDown("3"))
			{
				if (tools.VertGrid)
				{
					tools.VertGrid = false;
				}
				else
				{
					tools.VertGrid = true;
				}
			}
			if (Input.GetKey("up") && this.buildPos.transform.localPosition.z < 4f)
			{
				this.buildPos.transform.localPosition = new Vector3(0f, 0f, this.buildPos.transform.localPosition.z + 0.05f);
			}
			if (Input.GetKey("down") && this.buildPos.transform.localPosition.z > 1.5f)
			{
				this.buildPos.transform.localPosition = new Vector3(0f, 0f, this.buildPos.transform.localPosition.z - 0.05f);
			}
			if (!tools.Snapping)
			{
				this.Grid1.text = "ON";
			}
			else
			{
				this.Grid1.text = "OFF";
			}
			if (!tools.VertGrid)
			{
				this.Grid2.text = "1m";
			}
			else
			{
				this.Grid2.text = "0,1m";
			}
			if (!tools.HorGrid)
			{
				this.Grid3.text = "1m";
			}
			else
			{
				this.Grid3.text = "0,1m";
			}
		}
		this.MoneyText.text = tools.money.ToString("F2");
		this.MoneyText2.text = tools.money.ToString("F2");
		if (this.ClockText)
		{
			this.ClockText.text = EnviroSkyMgr.instance.GetTimeString();
		}
		if (base.transform.position.y < 50f && tools.sitting)
		{
			this.SetPlayerCamera();
		}
		if (tools.filling)
		{
			this.LookingText.text = "filling";
		}
		tools.filling = false;
		if (tools.Needs == 1)
		{
			if (tools.Sleep > 0f)
			{
				tools.Sleep -= Time.deltaTime * 0.0001f;
			}
			else
			{
				tools.Health -= Time.deltaTime * 0.0015f;
			}
			if (tools.Drink > 0f)
			{
				tools.Drink -= Time.deltaTime * 0.0001f;
			}
			else
			{
				tools.Health -= Time.deltaTime * 0.0015f;
			}
			if (tools.Food > 0f)
			{
				tools.Food -= Time.deltaTime * 0.0001f;
			}
			else
			{
				tools.Health -= Time.deltaTime * 0.0015f;
			}
			tools.Health += Time.deltaTime * 0.0005f;
			this.HealthSlider.value = tools.Health;
			this.HealthImage.color = Color.Lerp(Color.red, Color.white, tools.Health);
			this.SleepSlider.value = tools.Sleep;
			this.FoodSlider.value = tools.Food;
			this.DrinkSlider.value = tools.Drink;
			if (tools.Health <= 0f && this.GameOverCanvas)
			{
				this.GameOverCanvas.SetActive(true);
				this.DistanceFromStartText2.text = "Distance from start position" + Vector3.Distance(base.transform.position, this.MapMagic.transform.position + this.StartPosition).ToString("0") + "m";
			}
			if (tools.Health < 0f)
			{
				tools.Health = 0f;
			}
			if (tools.Health > 1f)
			{
				tools.Health = 1f;
			}
		}
		if (tools.sitting && this.ActiveSwitch)
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.ActiveSwitch.Clicked();
				tools.Clicked = true;
			}
			if (Input.GetMouseButtonDown(1))
			{
				this.ActiveSwitch.ClickedR();
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				this.ActiveSwitch.ScrollUP();
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				this.ActiveSwitch.ScrollDown();
			}
		}
		else if (tools.sitting && tools.UseMouseSteering)
		{
			if (Input.GetMouseButton(1))
			{
				if (!tools.MouseSteeringPause)
				{
					tools.LastCursorPos = (int)Input.mousePosition.x;
					tools.MouseSteeringPause = true;
					this.PlayerCam.GetComponent<carcam>().enabled = true;
					Cursor.lockState = CursorLockMode.Locked;
				}
			}
			else if (tools.MouseSteeringPause)
			{
				tools.MouseSteeringPause = false;
				this.PlayerCam.GetComponent<carcam>().enabled = false;
				Cursor.lockState = CursorLockMode.None;
				tools.SetCursorPos(tools.LastCursorPos, Screen.height / 2);
			}
		}
		if (this.player.GetButtonDown("MouseSteering"))
		{
			if (tools.UseMouseSteering)
			{
				tools.UseMouseSteering = false;
				this.DisableMouseSteering();
			}
			else
			{
				tools.UseMouseSteering = true;
				this.ActivateMouseSteering();
			}
		}
		if (this.player.GetButtonDown("LeftMouse"))
		{
			MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
		}
		if (this.player.GetButtonUp("LeftMouse"))
		{
			MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
		}
		if (this.player.GetButtonDown("RightMouse"))
		{
			MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.RightDown);
		}
		if (this.player.GetButtonUp("RightMouse"))
		{
			MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.RightUp);
		}
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x000697EA File Offset: 0x000679EA
	public void ActivateMouseSteering()
	{
		if (tools.sitting)
		{
			this.PlayerCam.GetComponent<carcam>().enabled = false;
			Cursor.lockState = CursorLockMode.None;
		}
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0006980A File Offset: 0x00067A0A
	public void DisableMouseSteering()
	{
		if (tools.sitting)
		{
			if (tools.sitting)
			{
				this.PlayerCam.GetComponent<carcam>().enabled = true;
			}
			Cursor.lockState = CursorLockMode.Locked;
		}
		tools.SetCursorPos(Screen.width / 2, Screen.height / 2);
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x00069845 File Offset: 0x00067A45
	private IEnumerator RestartTool()
	{
		yield return new WaitForSeconds(2f);
		tools.StartCooldown = false;
		if (tools.ToolHand.transform.childCount > 0 && tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>())
		{
			tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().enabled = true;
			tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().RestartTool();
			this.SphereJump();
		}
		base.StartCoroutine(this.RestartTool());
		yield break;
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x00069854 File Offset: 0x00067A54
	public void DropAll()
	{
		if (tools.ToolHand.transform.childCount > 0 && tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>())
		{
			tools.ToolHand.transform.GetChild(0).GetComponent<PickupTool>().ForceRelease();
		}
		if (this.HandStatic.transform.childCount > 0 && this.HandStatic.transform.GetChild(0).GetComponent<PickupTool>())
		{
			this.HandStatic.transform.GetChild(0).GetComponent<PickupTool>().ForceRelease();
		}
		if (this.hand1.transform.childCount > 0 && this.hand1.transform.GetChild(0).GetComponent<PickupTool>())
		{
			this.hand1.transform.GetChild(0).GetComponent<PickupTool>().ForceRelease();
		}
		if (this.handPaint.transform.childCount > 0 && this.handPaint.transform.GetChild(0).GetComponent<PickupTool>())
		{
			this.handPaint.transform.GetChild(0).GetComponent<PickupTool>().ForceRelease();
		}
		if (this.handPour.transform.childCount > 0 && this.handPour.transform.GetChild(0).GetComponent<PickupTool>())
		{
			this.handPour.transform.GetChild(0).GetComponent<PickupTool>().ForceRelease();
		}
		if (tools.ToolHand.transform.childCount > 0)
		{
			if (!tools.ToolHand.transform.GetChild(0).gameObject.GetComponent<Rigidbody>())
			{
				tools.ToolHand.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
			}
			tools.ToolHand.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
			tools.ToolHand.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			tools.ToolHand.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = false;
			tools.ToolHand.transform.GetChild(0).gameObject.transform.SetParent(null);
		}
		if (this.hand1.transform.childCount > 0)
		{
			if (!this.hand1.transform.GetChild(0).gameObject.GetComponent<Rigidbody>())
			{
				this.hand1.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
			}
			this.hand1.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
			this.hand1.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			this.hand1.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = false;
			this.hand1.transform.GetChild(0).gameObject.transform.SetParent(null);
		}
		if (this.handPaint.transform.childCount > 0)
		{
			if (!this.handPaint.transform.GetChild(0).gameObject.GetComponent<Rigidbody>())
			{
				this.handPaint.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
			}
			this.handPaint.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
			this.handPaint.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			this.handPaint.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = false;
			this.handPaint.transform.GetChild(0).gameObject.transform.SetParent(null);
		}
		if (this.handPour.transform.childCount > 0)
		{
			if (!this.handPour.transform.GetChild(0).gameObject.GetComponent<Rigidbody>())
			{
				this.handPour.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
			}
			this.handPour.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().useGravity = true;
			this.handPour.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().detectCollisions = true;
			this.handPour.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().isKinematic = false;
			this.handPour.transform.GetChild(0).gameObject.transform.SetParent(null);
		}
		tools.tool = 1;
		tools.holdingitem = false;
		base.StartCoroutine(this.RemoveHeld());
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x00069D47 File Offset: 0x00067F47
	private IEnumerator RemoveHeld()
	{
		yield return 0;
		tools.helditem = "Nothing";
		yield break;
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00069D4F File Offset: 0x00067F4F
	public void OpenBarnCanvas()
	{
		this.BarnCanvas.SetActive(true);
		this.Cursorcanvas.SetActive(false);
		base.GetComponent<FirstPersonAIO>().ControllerPause();
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x00069D74 File Offset: 0x00067F74
	public void ESC()
	{
		base.GetComponent<Rigidbody>().isKinematic = false;
		tools.DontAllowClick = false;
		if (base.GetComponent<FloatingPoinMyfix>())
		{
			base.GetComponent<FloatingPoinMyfix>().ResetPosition();
		}
		if (this.DistanceFromStartText && this.MapMagic)
		{
			this.DistanceFromStartText.text = "Distance from start position" + Vector3.Distance(base.transform.position, this.MapMagic.transform.position + this.StartPosition).ToString("0") + "m";
		}
		if (!this.notes.activeSelf && !this.CarInfo.activeSelf && !this.TrailerInfo.activeSelf && !this.Jobinfo.activeSelf && !this.partshop.activeSelf)
		{
			this.DropAll();
		}
		if (!base.GetComponent<FirstPersonAIO>().controllerPauseState && !tools.sitting)
		{
			this.EscMenu.SetActive(true);
			this.Cursorcanvas.SetActive(false);
			base.GetComponent<FirstPersonAIO>().ControllerPause();
			if (base.GetComponent<SteeringWheelInputProvider>().enabled)
			{
				this.UseWHeelToggle.isOn = true;
			}
			else
			{
				this.UseWHeelToggle.isOn = false;
			}
			if (tools.AutoClutch)
			{
				this.AutoClutchToggle.isOn = true;
			}
			else
			{
				this.AutoClutchToggle.isOn = false;
			}
			Time.timeScale = 0f;
		}
		else if (!this.ControlsObj || !this.ControlsObj.transform.GetChild(0).gameObject.activeSelf)
		{
			this.EscMenu.SetActive(false);
			this.Cursorcanvas.SetActive(true);
			base.GetComponent<FirstPersonAIO>().ControllerUnPause();
			Time.timeScale = 1f;
		}
		this.partshop.SetActive(false);
		this.notes.SetActive(false);
		this.Jobinfo.SetActive(false);
		this.CarInfo.SetActive(false);
		this.TrailerInfo.SetActive(false);
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x00069F7C File Offset: 0x0006817C
	public void SetPlayerCamera()
	{
		this.Cursorcanvas.SetActive(true);
		this.PlayerCam.transform.SetParent(this.PlayerCamParent.transform);
		this.PlayerCam.transform.position = this.PlayerCamParent.transform.position;
		this.PlayerCam.transform.rotation = this.PlayerCamParent.transform.rotation;
		this.PlayerCam.transform.rotation = this.PlayerCam.transform.root.rotation;
		this.OutsideDrivingCanvas.SetActive(false);
		if (tools.sitting)
		{
			this.PlayerCam.GetComponent<carcam>().enabled = true;
		}
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x0006A03C File Offset: 0x0006823C
	private void LateUpdate()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, 2f, 1 << LayerMask.NameToLayer("Bolts")))
		{
			if (tools.tool == 2 || raycastHit.collider.GetComponent<EngineStand>())
			{
				tools.cooldown = true;
			}
			base.StartCoroutine(this.Cooldown());
		}
		RaycastHit raycastHit2;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit2, 2f, 1 << LayerMask.NameToLayer("FlatBolts")))
		{
			if (tools.tool == 3 || tools.tool == 1 || tools.tool == 15)
			{
				tools.cooldown = true;
			}
			base.StartCoroutine(this.Cooldown());
		}
		RaycastHit raycastHit3;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit3, 2f, 1 << LayerMask.NameToLayer("Weld")))
		{
			if (tools.tool == 4 || tools.tool == 5 || raycastHit3.collider.GetComponent<LiftHandle>())
			{
				tools.cooldown = true;
			}
			base.StartCoroutine(this.Cooldown());
		}
		RaycastHit raycastHit4;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit4, 2f, 1 << LayerMask.NameToLayer("Windows")))
		{
			if (tools.tool == 6)
			{
				tools.cooldown = true;
			}
			base.StartCoroutine(this.Cooldown());
		}
		if (Input.GetMouseButtonUp(1) && !tools.cooldown2)
		{
			this.DropAll();
		}
		if (this.player.GetButtonDown("Freeze camera"))
		{
			if (base.GetComponent<FirstPersonAIO>().enableCameraMovement)
			{
				base.GetComponent<FirstPersonAIO>().enableCameraMovement = false;
				Cursor.lockState = CursorLockMode.None;
				this.FollowHand.GetComponent<OnScreenFollow>().enabled = true;
			}
			else
			{
				base.GetComponent<FirstPersonAIO>().enableCameraMovement = true;
				Cursor.lockState = CursorLockMode.Locked;
				this.FollowHand.GetComponent<OnScreenFollow>().enabled = false;
				this.FollowHand.transform.localPosition = new Vector3(0f, 0f, 0.6f);
				this.FollowHand.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}
		if (Input.GetKeyDown("f"))
		{
			if (tools.Tighten)
			{
				tools.Tighten = false;
			}
			else
			{
				tools.Tighten = true;
			}
		}
		if (Application.isEditor && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.O) && Input.GetKeyDown(KeyCode.G))
		{
			if (this.LOG)
			{
				this.LOG = false;
			}
			else
			{
				this.LOG = true;
			}
		}
		if (tools.sitting && base.transform.position.y > 50f)
		{
			if (Input.GetKeyDown("2") || Input.GetKeyDown("3") || Input.GetKeyDown("4") || Input.GetKeyDown("5"))
			{
				if (this.PlayerCam.GetComponent<Camera>().isActiveAndEnabled)
				{
					if (tools.MPrunning && !tools.NetworkPLayer.Avatar.activeSelf)
					{
						tools.NetworkPLayer.Avatar.SetActive(true);
						tools.NetworkPLayer.m_Animator.CrossFade("Driving_01", 0f);
					}
					this.Cursorcanvas.SetActive(false);
					this.PlayerCam.transform.SetParent(this.FollowCam.transform);
					this.PlayerCam.transform.position = this.FollowCam.transform.position;
					this.PlayerCam.transform.rotation = this.FollowCam.transform.rotation;
					this.PlayerCam.GetComponent<carcam>().enabled = false;
				}
				if (base.transform.root.GetComponent<MainCarProperties>() && base.transform.root.GetComponent<MainCarProperties>().Cluster && base.transform.root.GetComponent<MainCarProperties>().Cluster.Condition >= 0.1f)
				{
					this.OutsideDrivingCanvas.SetActive(true);
				}
			}
			if (Input.GetKeyDown("1"))
			{
				this.SetPlayerCamera();
				if (tools.MPrunning && tools.NetworkPLayer.isLocalPlayer)
				{
					tools.NetworkPLayer.Avatar.SetActive(false);
				}
			}
		}
		if (this.player.GetButtonDown("Backpack") && this.Backpack != null && !Input.GetKeyDown(KeyCode.LeftAlt))
		{
			if (this.Backpack.activeSelf)
			{
				this.Backpack.SetActive(false);
				this.BPitem0.text = "";
				this.BPitem1.text = "";
				this.BPitem2.text = "";
				this.BPitem3.text = "";
				this.BPitem4.text = "";
				this.BPitem5.text = "";
				this.BPitem6.text = "";
				this.BPitem7.text = "";
				this.BPitem8.text = "";
				this.BPitem9.text = "";
			}
			else if (!tools.sitting)
			{
				this.Backpack.SetActive(true);
			}
		}
		if (this.player.GetButtonDown("HeadLamp"))
		{
			if (this.HeadLamp.activeSelf)
			{
				this.HeadLamp.SetActive(false);
			}
			else
			{
				this.HeadLamp.SetActive(true);
			}
		}
		if (this.Backpack.activeSelf)
		{
			int count = this.BackpackParts.Count;
			if (count - 1 < this.BackpackActivePart)
			{
				this.BackpackActivePart = 0;
			}
			if (count == 0)
			{
				this.BackpackActivePart = 0;
			}
			if (count > 0)
			{
				this.BPitem0.text = this.BackpackParts[0].transform.name;
				if (this.BackpackActivePart == 0)
				{
					this.BPitem0.color = Color.yellow;
				}
				else
				{
					this.BPitem0.color = Color.white;
				}
			}
			else
			{
				this.BPitem0.text = "";
			}
			if (count > 1)
			{
				this.BPitem1.text = this.BackpackParts[1].transform.name;
				if (this.BackpackActivePart == 1)
				{
					this.BPitem1.color = Color.yellow;
				}
				else
				{
					this.BPitem1.color = Color.white;
				}
			}
			else
			{
				this.BPitem1.text = "";
			}
			if (count > 2)
			{
				this.BPitem2.text = this.BackpackParts[2].transform.name;
				if (this.BackpackActivePart == 2)
				{
					this.BPitem2.color = Color.yellow;
				}
				else
				{
					this.BPitem2.color = Color.white;
				}
			}
			else
			{
				this.BPitem2.text = "";
			}
			if (count > 3)
			{
				this.BPitem3.text = this.BackpackParts[3].transform.name;
				if (this.BackpackActivePart == 3)
				{
					this.BPitem3.color = Color.yellow;
				}
				else
				{
					this.BPitem3.color = Color.white;
				}
			}
			else
			{
				this.BPitem3.text = "";
			}
			if (count > 4)
			{
				this.BPitem4.text = this.BackpackParts[4].transform.name;
				if (this.BackpackActivePart == 4)
				{
					this.BPitem4.color = Color.yellow;
				}
				else
				{
					this.BPitem4.color = Color.white;
				}
			}
			else
			{
				this.BPitem4.text = "";
			}
			if (count > 5)
			{
				this.BPitem5.text = this.BackpackParts[5].transform.name;
				if (this.BackpackActivePart == 5)
				{
					this.BPitem5.color = Color.yellow;
				}
				else
				{
					this.BPitem5.color = Color.white;
				}
			}
			else
			{
				this.BPitem5.text = "";
			}
			if (count > 6)
			{
				this.BPitem6.text = this.BackpackParts[6].transform.name;
				if (this.BackpackActivePart == 6)
				{
					this.BPitem6.color = Color.yellow;
				}
				else
				{
					this.BPitem6.color = Color.white;
				}
			}
			else
			{
				this.BPitem6.text = "";
			}
			if (count > 7)
			{
				this.BPitem7.text = this.BackpackParts[7].transform.name;
				if (this.BackpackActivePart == 7)
				{
					this.BPitem7.color = Color.yellow;
				}
				else
				{
					this.BPitem7.color = Color.white;
				}
			}
			else
			{
				this.BPitem7.text = "";
			}
			if (count > 8)
			{
				this.BPitem8.text = this.BackpackParts[8].transform.name;
				if (this.BackpackActivePart == 8)
				{
					this.BPitem8.color = Color.yellow;
				}
				else
				{
					this.BPitem8.color = Color.white;
				}
			}
			else
			{
				this.BPitem8.text = "";
			}
			if (count > 9)
			{
				this.BPitem9.text = this.BackpackParts[9].transform.name;
				if (this.BackpackActivePart == 9)
				{
					this.BPitem9.color = Color.yellow;
				}
				else
				{
					this.BPitem9.color = Color.white;
				}
			}
			else
			{
				this.BPitem9.text = "";
			}
			if (!tools.cooldown3)
			{
				if (Input.GetAxis("Mouse ScrollWheel") < 0f)
				{
					if (count - 1 < this.BackpackActivePart)
					{
						this.BackpackActivePart = 0;
					}
					else
					{
						this.BackpackActivePart++;
					}
					base.StartCoroutine(this.Cooldown3());
				}
				if (Input.GetAxis("Mouse ScrollWheel") > 0f)
				{
					if (this.BackpackActivePart == 0)
					{
						this.BackpackActivePart = count - 1;
					}
					else
					{
						this.BackpackActivePart--;
					}
					base.StartCoroutine(this.Cooldown3());
				}
			}
			if ((Input.GetMouseButtonDown(2) || this.player.GetButtonDown("DiscardBackpack")) && count > 0)
			{
				if (this.BackpackParts[this.BackpackActivePart].GetComponent<Partinfo>())
				{
					this.BackpackParts[this.BackpackActivePart].GetComponent<Partinfo>().InBackpack = false;
				}
				if (this.BackpackParts[this.BackpackActivePart].GetComponent<SaveItem>())
				{
					this.BackpackParts[this.BackpackActivePart].GetComponent<SaveItem>().InBackpack = false;
				}
				this.BackpackParts[this.BackpackActivePart].transform.position = this.HandStatic.transform.position;
				this.BackpackParts[this.BackpackActivePart].AddComponent<Rigidbody>();
				this.BackpackParts[this.BackpackActivePart].GetComponent<Rigidbody>().velocity = Vector3.zero;
				if (tools.MPrunning && this.BackpackParts[this.BackpackActivePart].GetComponent<MPobject>() && this.BackpackParts[this.BackpackActivePart].GetComponent<MPobject>().networkDummy)
				{
					tools.NetworkPLayer.WaitPutdown(this.BackpackParts[this.BackpackActivePart].GetComponent<MPobject>().networkDummy);
				}
				this.BackpackParts[this.BackpackActivePart].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				this.BackpackParts.RemoveAt(this.BackpackActivePart);
			}
			if (count < 10)
			{
				this.TipsText.text = LocalizationManager.Localize("Press middle mouse button to discard").ToString();
				RaycastHit raycastHit5;
				if (Physics.Raycast(this.PlayerCam.transform.position, this.PlayerCam.transform.forward, out raycastHit5, 1.7f, 1 << LayerMask.NameToLayer("LooseParts") | 1 << LayerMask.NameToLayer("OpenableParts") | 1 << LayerMask.NameToLayer("Windows") | 1 << LayerMask.NameToLayer("Items")) && !raycastHit5.collider.transform.parent && raycastHit5.collider.gameObject.GetComponent<Rigidbody>())
				{
					if (raycastHit5.collider.gameObject.GetComponent<CarProperties>() && raycastHit5.collider.gameObject.GetComponent<CarProperties>().Owner == "Junkyard")
					{
						return;
					}
					if ((raycastHit5.collider.gameObject.GetComponent<PickupTool>() && raycastHit5.collider.gameObject.GetComponent<PickupTool>().CanPutInBox) || (raycastHit5.collider.gameObject.GetComponent<PickupItems>() && raycastHit5.collider.gameObject.GetComponent<PickupItems>().CanPutInBox) || (raycastHit5.collider.gameObject.GetComponent<MooveItem>() && raycastHit5.collider.gameObject.GetComponent<MooveItem>().CanPutInBox) || (raycastHit5.collider.gameObject.GetComponent<CarProperties>() && raycastHit5.collider.gameObject.GetComponent<CarProperties>().PREFAB && raycastHit5.collider.gameObject.GetComponent<CarProperties>().PREFAB.GetComponent<Partinfo>() && raycastHit5.collider.gameObject.GetComponent<CarProperties>().PREFAB.GetComponent<Partinfo>().CanPutInBox))
					{
						this.TipsText.text = LocalizationManager.Localize("Press E to add").ToString();
						if (this.player.GetButtonDown("PickToBackpack"))
						{
							if (tools.MPrunning && raycastHit5.collider.gameObject.GetComponent<MPobject>() && raycastHit5.collider.gameObject.GetComponent<MPobject>().networkDummy)
							{
								tools.NetworkPLayer.pickup(raycastHit5.collider.gameObject.GetComponent<MPobject>().networkDummy);
							}
							if (raycastHit5.collider.gameObject.GetComponent<Partinfo>())
							{
								raycastHit5.collider.gameObject.GetComponent<Partinfo>().InBackpack = true;
							}
							if (raycastHit5.collider.gameObject.name == "Collectible" && !raycastHit5.collider.gameObject.gameObject.GetComponent<SaveItem>())
							{
								raycastHit5.collider.gameObject.gameObject.AddComponent<SaveItem>();
								raycastHit5.collider.gameObject.gameObject.GetComponent<SaveItem>().PrefabName = raycastHit5.collider.gameObject.gameObject.GetComponent<MooveItem>().PrefabName;
							}
							if (raycastHit5.collider.gameObject.GetComponent<SaveItem>())
							{
								raycastHit5.collider.gameObject.GetComponent<SaveItem>().InBackpack = true;
							}
							if (raycastHit5.collider.gameObject.GetComponent<FixedJoint>())
							{
								UnityEngine.Object.Destroy(raycastHit5.collider.gameObject.GetComponent<FixedJoint>());
							}
							UnityEngine.Object.Destroy(raycastHit5.collider.gameObject.GetComponent<Rigidbody>());
							this.BackpackParts.Add(raycastHit5.collider.gameObject);
							raycastHit5.collider.gameObject.transform.position = new Vector3(0f, 45f, 0f);
						}
					}
				}
			}
			else
			{
				this.TipsText.text = LocalizationManager.Localize("Backpack full").ToString();
			}
		}
		if (this.player.GetButtonDown("Map info") && this.MapCanvas != null)
		{
			if (this.partshop.activeSelf || this.notes.activeSelf)
			{
				return;
			}
			if (!this.notes.activeSelf && !this.CarInfo.activeSelf && !this.TrailerInfo.activeSelf && !this.Jobinfo.activeSelf && !this.partshop.activeSelf)
			{
				this.DropAll();
			}
			if (!base.GetComponent<FirstPersonAIO>().controllerPauseState)
			{
				this.MapCanvas.SetActive(true);
				this.Cursorcanvas.SetActive(false);
				base.GetComponent<FirstPersonAIO>().ControllerPause();
				tools.DontAllowClick = true;
			}
			else
			{
				this.MapCanvas.SetActive(false);
				this.Cursorcanvas.SetActive(true);
				base.GetComponent<FirstPersonAIO>().ControllerUnPause();
				tools.DontAllowClick = false;
			}
			this.partshop.SetActive(false);
			this.notes.SetActive(false);
			this.Jobinfo.SetActive(false);
			this.CarInfo.SetActive(false);
			this.TrailerInfo.SetActive(false);
		}
		if (this.player.GetButtonDown("Car info") && !this.notes.activeSelf && !this.EscMenu.activeSelf)
		{
			this.Jobinfo.SetActive(false);
			this.notes.SetActive(false);
			if (this.CarInfo.activeSelf || this.TrailerInfo.activeSelf)
			{
				this.Cursorcanvas.SetActive(true);
				this.CarInfo.SetActive(false);
				this.TrailerInfo.SetActive(false);
				base.GetComponent<FirstPersonAIO>().ControllerUnPause();
			}
			else if (!tools.sitting)
			{
				Transform transform = null;
				float num = float.PositiveInfinity;
				Vector3 position = base.transform.position;
				foreach (Collider collider in Physics.OverlapSphere(position, 4f))
				{
					if (collider.GetComponent<MainCarProperties>() || collider.GetComponent<MainTrailerProperties>())
					{
						float num2 = Vector3.Distance(collider.transform.position, position);
						if (num2 < num)
						{
							transform = collider.transform;
							num = num2;
						}
					}
				}
				if (transform)
				{
					base.GetComponent<FirstPersonAIO>().ControllerPause();
					if (transform.GetComponent<MainCarProperties>())
					{
						this.CarInfo.GetComponent<CarInformation>().Car = transform.transform.gameObject;
						this.Cursorcanvas.SetActive(false);
						this.CarInfo.SetActive(true);
						this.CarInfo.GetComponent<CarInformation>().ReStart(false);
					}
					if (transform.GetComponent<MainTrailerProperties>())
					{
						this.TrailerInfo.GetComponent<TrailerInformation>().Trailer = transform.transform.gameObject;
						this.Cursorcanvas.SetActive(false);
						this.TrailerInfo.SetActive(true);
						this.TrailerInfo.GetComponent<TrailerInformation>().ReStart();
					}
				}
			}
		}
		if (this.player.GetButtonDown("Notes") && !this.EscMenu.activeSelf && !this.partshop.activeSelf)
		{
			Time.timeScale = 1f;
			this.EscMenu.SetActive(false);
			this.Jobinfo.SetActive(false);
			this.CarInfo.SetActive(false);
			this.TrailerInfo.SetActive(false);
			if (!this.notes.activeSelf && !tools.sitting)
			{
				base.GetComponent<FirstPersonAIO>().ControllerPause();
				this.notes.SetActive(true);
			}
		}
		if ((Input.GetKeyDown(KeyCode.Escape) || this.player.GetButtonDown("Escape")) && (tools.GameLoaded || this.MapMagic == null))
		{
			this.ESC();
		}
		if (this.LOG)
		{
			if (Input.GetKeyDown("0"))
			{
				tools.money += 100000f;
			}
			if (tools.sitting && Input.GetKeyDown("="))
			{
				base.transform.root.GetComponent<MainCarProperties>().EngineBlock.Power += 100f;
			}
			if (tools.sitting && Input.GetKeyDown("-"))
			{
				base.transform.root.GetComponent<MainCarProperties>().EngineBlock.Power -= 100f;
			}
		}
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0006B4D2 File Offset: 0x000696D2
	public void SphereJump()
	{
		tools.cooldown = true;
		this.SphereCOl.GetComponent<DisablerCollider>().RestartJump();
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0006B4EA File Offset: 0x000696EA
	public void Controls()
	{
		this.ControlsObj.GetComponent<ControlMapper>().Open();
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x0006B4FC File Offset: 0x000696FC
	public void Quit()
	{
		base.GetComponent<tools>().Loadingtext.SetActive(true);
		SceneManager.LoadScene("MainMenu");
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0006B519 File Offset: 0x00069719
	public void EnableExtensionMap()
	{
		SceneManager.LoadScene("IslandExtension", LoadSceneMode.Additive);
		PlayerPrefs.SetFloat("MapExtension", 1f);
		PlayerPrefs.Save();
		this.taxiservicebutton.SetActive(true);
		ModLoader.OnNewMapLoad();
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0006B54B File Offset: 0x0006974B
	public void DisableExtensionMap()
	{
		SceneManager.UnloadScene("IslandExtension");
		PlayerPrefs.SetFloat("MapExtension", 0f);
		PlayerPrefs.Save();
		this.taxiservicebutton.SetActive(false);
		ModLoader.OnNewMapUnload();
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0006B580 File Offset: 0x00069780
	public void NextQuality()
	{
		if (this.Quality == 0f)
		{
			PlayerPrefs.SetFloat("Quality", 1f);
			this.QualityText.text = "Low";
			QualitySettings.SetQualityLevel(1, true);
			this.Quality = 1f;
		}
		else if (this.Quality == 1f)
		{
			PlayerPrefs.SetFloat("Quality", 2f);
			this.QualityText.text = "Medium";
			QualitySettings.SetQualityLevel(2, true);
			this.Quality = 2f;
		}
		else if (this.Quality == 2f)
		{
			PlayerPrefs.SetFloat("Quality", 3f);
			this.QualityText.text = "High";
			QualitySettings.SetQualityLevel(3, true);
			this.Quality = 3f;
		}
		else if (this.Quality == 3f)
		{
			PlayerPrefs.SetFloat("Quality", 4f);
			this.QualityText.text = "Very High";
			QualitySettings.SetQualityLevel(4, true);
			this.Quality = 4f;
		}
		else if (this.Quality == 4f)
		{
			PlayerPrefs.SetFloat("Quality", 5f);
			this.QualityText.text = "Ultra";
			QualitySettings.SetQualityLevel(5, true);
			this.Quality = 5f;
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0006B6D8 File Offset: 0x000698D8
	public void PreviousQuality()
	{
		if (this.Quality == 1f)
		{
			PlayerPrefs.SetFloat("Quality", 0f);
			this.QualityText.text = "Very Low";
			QualitySettings.SetQualityLevel(0, true);
			this.Quality = 0f;
		}
		else if (this.Quality == 2f)
		{
			PlayerPrefs.SetFloat("Quality", 1f);
			this.QualityText.text = "Low";
			QualitySettings.SetQualityLevel(1, true);
			this.Quality = 1f;
		}
		else if (this.Quality == 3f)
		{
			PlayerPrefs.SetFloat("Quality", 2f);
			this.QualityText.text = "Medium";
			QualitySettings.SetQualityLevel(2, true);
			this.Quality = 2f;
		}
		else if (this.Quality == 4f)
		{
			PlayerPrefs.SetFloat("Quality", 3f);
			this.QualityText.text = "High";
			QualitySettings.SetQualityLevel(3, true);
			this.Quality = 3f;
		}
		else if (this.Quality == 5f)
		{
			PlayerPrefs.SetFloat("Quality", 4f);
			this.QualityText.text = "Very High";
			QualitySettings.SetQualityLevel(4, true);
			this.Quality = 4f;
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0006B830 File Offset: 0x00069A30
	public void NextReflections()
	{
		if (this.ReflectionQuality == 0f)
		{
			PlayerPrefs.SetFloat("ReflectionQuality", 1f);
			this.ReflectionQualityText.text = "Good";
			this.IslandProbes.SetActive(true);
			this.PlayerProbe.SetActive(false);
			this.ReflectionQuality = 1f;
		}
		else if (this.ReflectionQuality == 1f)
		{
			PlayerPrefs.SetFloat("ReflectionQuality", 2f);
			this.ReflectionQualityText.text = "Best(experimental)";
			this.IslandProbes.SetActive(false);
			this.PlayerProbe.SetActive(true);
			this.ReflectionQuality = 2f;
		}
		else if (this.ReflectionQuality == 2f)
		{
			PlayerPrefs.SetFloat("ReflectionQuality", 0f);
			this.ReflectionQualityText.text = "Basic";
			this.IslandProbes.SetActive(false);
			this.PlayerProbe.SetActive(false);
			this.ReflectionQuality = 0f;
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0006B938 File Offset: 0x00069B38
	public void PreviousReflections()
	{
		if (this.ReflectionQuality == 0f)
		{
			PlayerPrefs.SetFloat("ReflectionQuality", 2f);
			this.ReflectionQualityText.text = "Best(experimental)";
			this.IslandProbes.SetActive(false);
			this.PlayerProbe.SetActive(true);
			this.ReflectionQuality = 2f;
		}
		else if (this.ReflectionQuality == 1f)
		{
			PlayerPrefs.SetFloat("ReflectionQuality", 0f);
			this.ReflectionQualityText.text = "Basic";
			this.IslandProbes.SetActive(false);
			this.PlayerProbe.SetActive(false);
			this.ReflectionQuality = 0f;
		}
		else if (this.ReflectionQuality == 2f)
		{
			PlayerPrefs.SetFloat("ReflectionQuality", 1f);
			this.ReflectionQualityText.text = "Good";
			this.IslandProbes.SetActive(true);
			this.PlayerProbe.SetActive(false);
			this.ReflectionQuality = 1f;
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x0006BA40 File Offset: 0x00069C40
	public void NextTectureQuality()
	{
		if (tools.TextureQuality == 0f)
		{
			PlayerPrefs.SetFloat("TextureQuality", 1f);
			this.TextureQualityText.GetComponent<Text>().color = Color.black;
			this.TextureQualityText.text = "1024 X 1024";
			tools.TextureQuality = 1f;
		}
		else if (tools.TextureQuality == 1f)
		{
			PlayerPrefs.SetFloat("TextureQuality", 2f);
			this.TextureQualityText.GetComponent<Text>().color = Color.red;
			this.TextureQualityText.text = "2048 X 2048";
			tools.TextureQuality = 2f;
		}
		else if (tools.TextureQuality == 2f)
		{
			PlayerPrefs.SetFloat("TextureQuality", 0f);
			this.TextureQualityText.GetComponent<Text>().color = Color.black;
			this.TextureQualityText.text = "512 X 512";
			tools.TextureQuality = 0f;
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0006BB38 File Offset: 0x00069D38
	public void PreviousTectureQuality()
	{
		if (tools.TextureQuality == 0f)
		{
			PlayerPrefs.SetFloat("TextureQuality", 2f);
			this.TextureQualityText.GetComponent<Text>().color = Color.red;
			this.TextureQualityText.text = "2048 X 2048";
			tools.TextureQuality = 2f;
		}
		else if (tools.TextureQuality == 1f)
		{
			PlayerPrefs.SetFloat("TextureQuality", 0f);
			this.TextureQualityText.GetComponent<Text>().color = Color.black;
			this.TextureQualityText.text = "512 X 512";
			tools.TextureQuality = 0f;
		}
		else if (tools.TextureQuality == 2f)
		{
			PlayerPrefs.SetFloat("TextureQuality", 1f);
			this.TextureQualityText.GetComponent<Text>().color = Color.black;
			this.TextureQualityText.text = "1024 X 1024";
			tools.TextureQuality = 1f;
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x0006BC30 File Offset: 0x00069E30
	public void UpdateMoneySlider()
	{
		if ((float)int.Parse(this.MoneyIntput.text) > tools.money)
		{
			this.MoneyIntput.text = tools.money.ToString();
		}
		if (int.Parse(this.MoneyIntput.text) > 10000)
		{
			this.MoneyIntput.text = 10000.ToString();
		}
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x0006BC9C File Offset: 0x00069E9C
	public void SpawnMoney()
	{
		float num = (float)int.Parse(this.MoneyIntput.text);
		if (num > 0f && tools.money > 0f)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.MoneyPrefab, this.HandStatic.transform.position, this.HandStatic.transform.rotation);
			gameObject.transform.name = "Money";
			if (num <= tools.money)
			{
				gameObject.GetComponent<PickupTool>().paintlife = num;
				tools.money -= num;
			}
			else
			{
				gameObject.GetComponent<PickupTool>().paintlife = tools.money;
				tools.money = 0f;
			}
			this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().Cash);
		}
		this.UpdateMoneySlider();
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x0006BD74 File Offset: 0x00069F74
	private void OnGUI()
	{
		if (this.LOG)
		{
			EnviroSkyMgr.instance.GetTimeString();
			GUI.Box(new Rect(0f, 300f, 200f, 30f), "tool" + tools.tool.ToString());
			GUI.Box(new Rect(0f, 330f, 200f, 25f), "holdingitem" + tools.holdingitem.ToString());
			GUI.Box(new Rect(0f, 360f, 200f, 25f), "cantake     " + tools.cantake.ToString());
			GUI.Box(new Rect(0f, 390f, 200f, 25f), "cooldown     " + tools.cooldown.ToString());
			GUI.Box(new Rect(0f, 420f, 200f, 30f), "time" + EnviroSkyMgr.instance.GetTimeString());
			GUI.Box(new Rect(0f, 450f, 200f, 30f), "hour" + EnviroSkyMgr.instance.Time.Hours.ToString());
			GUI.Box(new Rect(0f, 470f, 200f, 30f), "SavedVersion" + tools.SavedVersion);
			if (tools.sitting)
			{
				GUI.Box(new Rect(0f, 460f, 200f, 25f), "HP " + base.transform.root.GetComponent<MainCarProperties>().exp.powertrain.engine.maxPower.ToString());
			}
			this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, new GUILayoutOption[]
			{
				GUILayout.Width((float)Screen.width),
				GUILayout.Height(this.height)
			});
			GUILayout.TextArea(tools.text, new GUILayoutOption[]
			{
				GUILayout.MinHeight(this.height)
			});
			GUILayout.EndScrollView();
		}
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x0006BFA9 File Offset: 0x0006A1A9
	public static void Add(string line)
	{
		tools.text = tools.text + line + "\n";
	}

	// Token: 0x04001226 RID: 4646
	public List<TimedActions> TimedActions;

	// Token: 0x04001227 RID: 4647
	public static NetworkPLayer NetworkPLayer;

	// Token: 0x04001228 RID: 4648
	public static int MPNumber;

	// Token: 0x04001229 RID: 4649
	private Player player;

	// Token: 0x0400122A RID: 4650
	public bool Started;

	// Token: 0x0400122B RID: 4651
	public static bool GameLoaded;

	// Token: 0x0400122C RID: 4652
	public static bool TerrainsLoaded = false;

	// Token: 0x0400122D RID: 4653
	public static string SavedVersion;

	// Token: 0x0400122E RID: 4654
	public Vector3 StartPosition;

	// Token: 0x0400122F RID: 4655
	public static VehicleController exp;

	// Token: 0x04001230 RID: 4656
	public GameObject NoSpaceCanvas;

	// Token: 0x04001231 RID: 4657
	public Text rotationText;

	// Token: 0x04001232 RID: 4658
	public float wheelrotation;

	// Token: 0x04001233 RID: 4659
	public Slider WheelSlider;

	// Token: 0x04001234 RID: 4660
	public static int PizzaDeliveriesCount;

	// Token: 0x04001235 RID: 4661
	public static int PizzaDeliveriesDay;

	// Token: 0x04001236 RID: 4662
	public static bool MPrunning;

	// Token: 0x04001237 RID: 4663
	public static bool DontAllowClick;

	// Token: 0x04001238 RID: 4664
	public static bool UIisOpen;

	// Token: 0x04001239 RID: 4665
	public static bool UseMouseSteering;

	// Token: 0x0400123A RID: 4666
	public static bool MouseSteeringPause;

	// Token: 0x0400123B RID: 4667
	public static int LastCursorPos;

	// Token: 0x0400123C RID: 4668
	public static int Needs;

	// Token: 0x0400123D RID: 4669
	public static float Sleep;

	// Token: 0x0400123E RID: 4670
	public static float Food;

	// Token: 0x0400123F RID: 4671
	public static float Drink;

	// Token: 0x04001240 RID: 4672
	public static float Health;

	// Token: 0x04001241 RID: 4673
	public GameObject NeedsCanvas;

	// Token: 0x04001242 RID: 4674
	public GameObject GameOverCanvas;

	// Token: 0x04001243 RID: 4675
	public Text DistanceFromStartText2;

	// Token: 0x04001244 RID: 4676
	public Slider SleepSlider;

	// Token: 0x04001245 RID: 4677
	public Slider FoodSlider;

	// Token: 0x04001246 RID: 4678
	public Slider DrinkSlider;

	// Token: 0x04001247 RID: 4679
	public Slider HealthSlider;

	// Token: 0x04001248 RID: 4680
	public Image HealthImage;

	// Token: 0x04001249 RID: 4681
	public Slider LoadingSlider;

	// Token: 0x0400124A RID: 4682
	public GameObject PlayerCam;

	// Token: 0x0400124B RID: 4683
	public GameObject PlayerCamParent;

	// Token: 0x0400124C RID: 4684
	public GameObject hand;

	// Token: 0x0400124D RID: 4685
	public GameObject HandStatic;

	// Token: 0x0400124E RID: 4686
	public GameObject hand1;

	// Token: 0x0400124F RID: 4687
	public GameObject handPour;

	// Token: 0x04001250 RID: 4688
	public GameObject handPaint;

	// Token: 0x04001251 RID: 4689
	public GameObject FollowHand;

	// Token: 0x04001252 RID: 4690
	public GameObject HittingHand;

	// Token: 0x04001253 RID: 4691
	public static GameObject ToolHand;

	// Token: 0x04001254 RID: 4692
	public GameObject buildPos;

	// Token: 0x04001255 RID: 4693
	public static bool Snapping;

	// Token: 0x04001256 RID: 4694
	public static bool HorGrid;

	// Token: 0x04001257 RID: 4695
	public static bool VertGrid;

	// Token: 0x04001258 RID: 4696
	public static float KeyboardSensitivity;

	// Token: 0x04001259 RID: 4697
	public static GameObject _buildPos;

	// Token: 0x0400125A RID: 4698
	public GameObject FollowCam;

	// Token: 0x0400125B RID: 4699
	public BuildingParent buildparent;

	// Token: 0x0400125C RID: 4700
	public static BuildingParent _buildparent;

	// Token: 0x0400125D RID: 4701
	public GameObject HeadLamp;

	// Token: 0x0400125E RID: 4702
	public GameObject ShopCam;

	// Token: 0x0400125F RID: 4703
	public GameObject SphereCOl;

	// Token: 0x04001260 RID: 4704
	public GameObject ReadHand;

	// Token: 0x04001261 RID: 4705
	public static GameObject PartInHand;

	// Token: 0x04001262 RID: 4706
	public static GameObject ToolInHand;

	// Token: 0x04001263 RID: 4707
	public GameObject SpawnPoint;

	// Token: 0x04001264 RID: 4708
	public GameObject AudioParent;

	// Token: 0x04001265 RID: 4709
	public static GameObject AudioParent_;

	// Token: 0x04001266 RID: 4710
	public GameObject MapMagic;

	// Token: 0x04001267 RID: 4711
	public static GameObject _MapMagic;

	// Token: 0x04001268 RID: 4712
	public GameObject JunkYardSpawner;

	// Token: 0x04001269 RID: 4713
	public GameObject CarDealerSpawner;

	// Token: 0x0400126A RID: 4714
	public GameObject EscMenu;

	// Token: 0x0400126B RID: 4715
	public GameObject SignSpawn;

	// Token: 0x0400126C RID: 4716
	public GameObject GarageSpawn;

	// Token: 0x0400126D RID: 4717
	public GameObject JunkyardSpawn;

	// Token: 0x0400126E RID: 4718
	public GameObject HouseSpawn;

	// Token: 0x0400126F RID: 4719
	public GameObject TaxiMenu;

	// Token: 0x04001270 RID: 4720
	public GameObject MoneyPrefab;

	// Token: 0x04001271 RID: 4721
	public GameObject Backpack;

	// Token: 0x04001272 RID: 4722
	public List<GameObject> BackpackParts;

	// Token: 0x04001273 RID: 4723
	public int BackpackActivePart;

	// Token: 0x04001274 RID: 4724
	public TMP_Text BPitem0;

	// Token: 0x04001275 RID: 4725
	public TMP_Text BPitem1;

	// Token: 0x04001276 RID: 4726
	public TMP_Text BPitem2;

	// Token: 0x04001277 RID: 4727
	public TMP_Text BPitem3;

	// Token: 0x04001278 RID: 4728
	public TMP_Text BPitem4;

	// Token: 0x04001279 RID: 4729
	public TMP_Text BPitem5;

	// Token: 0x0400127A RID: 4730
	public TMP_Text BPitem6;

	// Token: 0x0400127B RID: 4731
	public TMP_Text BPitem7;

	// Token: 0x0400127C RID: 4732
	public TMP_Text BPitem8;

	// Token: 0x0400127D RID: 4733
	public TMP_Text BPitem9;

	// Token: 0x0400127E RID: 4734
	public TMP_Text LookingButtonText;

	// Token: 0x0400127F RID: 4735
	public TMP_Text LookingText;

	// Token: 0x04001280 RID: 4736
	public TMP_Text LookingFitCarText;

	// Token: 0x04001281 RID: 4737
	public TMP_Text LookingFitEngineText;

	// Token: 0x04001282 RID: 4738
	public Text PriceText;

	// Token: 0x04001283 RID: 4739
	public Text DistanceFromStartText;

	// Token: 0x04001284 RID: 4740
	public GameObject Loadingtext;

	// Token: 0x04001285 RID: 4741
	public GameObject MPloadingText;

	// Token: 0x04001286 RID: 4742
	public GameObject Savingtext;

	// Token: 0x04001287 RID: 4743
	public GameObject RemoveParttext;

	// Token: 0x04001288 RID: 4744
	public Text MoneyText;

	// Token: 0x04001289 RID: 4745
	public Text MoneyText2;

	// Token: 0x0400128A RID: 4746
	public Text ClockText;

	// Token: 0x0400128B RID: 4747
	public TMP_Text TipsText;

	// Token: 0x0400128C RID: 4748
	public Text TipsTextRed;

	// Token: 0x0400128D RID: 4749
	public Text GearGauge;

	// Token: 0x0400128E RID: 4750
	public bool cooldownRUNNING;

	// Token: 0x0400128F RID: 4751
	public static bool DirectSteer;

	// Token: 0x04001290 RID: 4752
	public Toggle DirectSteerToggle;

	// Token: 0x04001291 RID: 4753
	public static bool AutoClutch;

	// Token: 0x04001292 RID: 4754
	public Toggle AutoClutchToggle;

	// Token: 0x04001293 RID: 4755
	public Toggle UseWHeelToggle;

	// Token: 0x04001294 RID: 4756
	public Toggle SwappedInputToggle;

	// Token: 0x04001295 RID: 4757
	public Toggle InvSteeringToggle;

	// Token: 0x04001296 RID: 4758
	public Toggle InvBrakesToggle;

	// Token: 0x04001297 RID: 4759
	public Toggle UseHeadbobToggle;

	// Token: 0x04001298 RID: 4760
	public Toggle ItemDescriptionToggle;

	// Token: 0x04001299 RID: 4761
	public Toggle InvThrottleToggle;

	// Token: 0x0400129A RID: 4762
	public Toggle InvClutchToggle;

	// Token: 0x0400129B RID: 4763
	public Toggle vsyncToggle;

	// Token: 0x0400129C RID: 4764
	public Text SensitivityValue;

	// Token: 0x0400129D RID: 4765
	public Slider SensitivitySlider;

	// Token: 0x0400129E RID: 4766
	public Text FeedbackValue;

	// Token: 0x0400129F RID: 4767
	public Slider ForceSlider;

	// Token: 0x040012A0 RID: 4768
	public Slider FOVSlider;

	// Token: 0x040012A1 RID: 4769
	public Slider JunkSlider;

	// Token: 0x040012A2 RID: 4770
	public Slider MouseSlider;

	// Token: 0x040012A3 RID: 4771
	public Text MouseValue;

	// Token: 0x040012A4 RID: 4772
	public Text FOVValue;

	// Token: 0x040012A5 RID: 4773
	public Text JunkValue;

	// Token: 0x040012A6 RID: 4774
	public TMP_InputField MoneyIntput;

	// Token: 0x040012A7 RID: 4775
	public bool ItemDescription;

	// Token: 0x040012A8 RID: 4776
	public bool vsync;

	// Token: 0x040012A9 RID: 4777
	public static bool StartCooldown;

	// Token: 0x040012AA RID: 4778
	public bool CanSkipTime;

	// Token: 0x040012AB RID: 4779
	public bool CanRace;

	// Token: 0x040012AC RID: 4780
	public static bool Racing = false;

	// Token: 0x040012AD RID: 4781
	public RaceControl RaceControl;

	// Token: 0x040012AE RID: 4782
	public float StartMoney;

	// Token: 0x040012AF RID: 4783
	public static bool Tighten;

	// Token: 0x040012B0 RID: 4784
	public static float money;

	// Token: 0x040012B1 RID: 4785
	public static float PriceModifier = 1f;

	// Token: 0x040012B2 RID: 4786
	public static bool electricity = false;

	// Token: 0x040012B3 RID: 4787
	public static bool JunkZone = false;

	// Token: 0x040012B4 RID: 4788
	public static bool cooldown = false;

	// Token: 0x040012B5 RID: 4789
	public static bool cooldown2 = false;

	// Token: 0x040012B6 RID: 4790
	public static bool cooldown3 = false;

	// Token: 0x040012B7 RID: 4791
	public static bool Clicked = false;

	// Token: 0x040012B8 RID: 4792
	public static bool LookingWindow = false;

	// Token: 0x040012B9 RID: 4793
	public static bool sitting = false;

	// Token: 0x040012BA RID: 4794
	public static bool cansit = false;

	// Token: 0x040012BB RID: 4795
	public static bool canput = false;

	// Token: 0x040012BC RID: 4796
	public static bool canrepair = false;

	// Token: 0x040012BD RID: 4797
	public static bool canfair = false;

	// Token: 0x040012BE RID: 4798
	public static bool canremove = false;

	// Token: 0x040012BF RID: 4799
	public static bool cantake = false;

	// Token: 0x040012C0 RID: 4800
	public static bool filling = false;

	// Token: 0x040012C1 RID: 4801
	public static bool holding = false;

	// Token: 0x040012C2 RID: 4802
	public static bool holdingitem = false;

	// Token: 0x040012C3 RID: 4803
	public static int tool = 1;

	// Token: 0x040012C4 RID: 4804
	public static int DaNumber = 1;

	// Token: 0x040012C5 RID: 4805
	public static string helditem = "Nothing";

	// Token: 0x040012C6 RID: 4806
	public static string lookitem = "Nothing";

	// Token: 0x040012C7 RID: 4807
	public static bool LookingAtTransparent;

	// Token: 0x040012C8 RID: 4808
	public GameObject partshop;

	// Token: 0x040012C9 RID: 4809
	public GameObject notes;

	// Token: 0x040012CA RID: 4810
	public GameObject Jobinfo;

	// Token: 0x040012CB RID: 4811
	public GameObject CarInfo;

	// Token: 0x040012CC RID: 4812
	public GameObject TrailerInfo;

	// Token: 0x040012CD RID: 4813
	public GameObject Cursorcanvas;

	// Token: 0x040012CE RID: 4814
	public GameObject Buildingcanvas;

	// Token: 0x040012CF RID: 4815
	public Text Grid1;

	// Token: 0x040012D0 RID: 4816
	public Text Grid2;

	// Token: 0x040012D1 RID: 4817
	public Text Grid3;

	// Token: 0x040012D2 RID: 4818
	public GameObject OutsideDrivingCanvas;

	// Token: 0x040012D3 RID: 4819
	public GameObject TutorialCanvas;

	// Token: 0x040012D4 RID: 4820
	public GameObject VideoCanvas;

	// Token: 0x040012D5 RID: 4821
	public GameObject MapCanvas;

	// Token: 0x040012D6 RID: 4822
	public GameObject BarnCanvas;

	// Token: 0x040012D7 RID: 4823
	public static GameObject SpawnSpot;

	// Token: 0x040012D8 RID: 4824
	public static int BuildingRotation;

	// Token: 0x040012D9 RID: 4825
	private float BuyPrice;

	// Token: 0x040012DA RID: 4826
	public bool LOG;

	// Token: 0x040012DB RID: 4827
	private static string text = "Unity Console v1.4.567\n";

	// Token: 0x040012DC RID: 4828
	private float height = 150f;

	// Token: 0x040012DD RID: 4829
	private Vector2 scrollPosition = new Vector2(0f, 0f);

	// Token: 0x040012DE RID: 4830
	public GameObject[] StartObjets;

	// Token: 0x040012DF RID: 4831
	public GameObject StartItemSpawnSpot;

	// Token: 0x040012E0 RID: 4832
	public Saver saver;

	// Token: 0x040012E1 RID: 4833
	public Text AutosaveText;

	// Token: 0x040012E2 RID: 4834
	public float AutosaveTimer;

	// Token: 0x040012E3 RID: 4835
	public float timer;

	// Token: 0x040012E4 RID: 4836
	public GameObject ControlsObj;

	// Token: 0x040012E5 RID: 4837
	private int LastHour;

	// Token: 0x040012E6 RID: 4838
	public JobsManager JM;

	// Token: 0x040012E7 RID: 4839
	public float snowAmount;

	// Token: 0x040012E8 RID: 4840
	public Text QualityText;

	// Token: 0x040012E9 RID: 4841
	public float Quality;

	// Token: 0x040012EA RID: 4842
	public Text ReflectionQualityText;

	// Token: 0x040012EB RID: 4843
	public float ReflectionQuality;

	// Token: 0x040012EC RID: 4844
	public Text TextureQualityText;

	// Token: 0x040012ED RID: 4845
	public static float TextureQuality;

	// Token: 0x040012EE RID: 4846
	public GameObject IslandProbes;

	// Token: 0x040012EF RID: 4847
	public GameObject PlayerProbe;

	// Token: 0x040012F0 RID: 4848
	public ReflectionProbe reflectionProbe;

	// Token: 0x040012F1 RID: 4849
	public Switch ActiveSwitch;

	// Token: 0x040012F2 RID: 4850
	private float SwitchTimer;

	// Token: 0x040012F3 RID: 4851
	public GameObject taxiservicebutton;
}
