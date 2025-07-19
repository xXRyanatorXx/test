using System;
using System.Text;
using UnityEngine;

// Token: 0x02000105 RID: 261
public class LogitechSteeringWheel : MonoBehaviour
{
	// Token: 0x060005BA RID: 1466 RVA: 0x0002B298 File Offset: 0x00029498
	private void Start()
	{
		this.activeForces = "";
		this.propertiesEdit = "";
		this.actualState = "";
		this.buttonStatus = "";
		this.forcesLabel = "Press the following keys to activate forces and effects on the steering wheel / gaming controller \n";
		this.forcesLabel += "Spring force : S\n";
		this.forcesLabel += "Constant force : C\n";
		this.forcesLabel += "Damper force : D\n";
		this.forcesLabel += "Side collision : Left or Right Arrow\n";
		this.forcesLabel += "Front collision : Up arrow\n";
		this.forcesLabel += "Dirt road effect : I\n";
		this.forcesLabel += "Bumpy road effect : B\n";
		this.forcesLabel += "Slippery road effect : L\n";
		this.forcesLabel += "Surface effect : U\n";
		this.forcesLabel += "Car Airborne effect : A\n";
		this.forcesLabel += "Soft Stop Force : O\n";
		this.forcesLabel += "Set example controller properties : PageUp\n";
		this.forcesLabel += "Play Leds : P\n";
		this.activeForceAndEffect = new string[9];
		Debug.Log("SteeringInit:" + LogitechGSDK.LogiSteeringInitialize(false).ToString());
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0002B424 File Offset: 0x00029624
	private void OnApplicationQuit()
	{
		Debug.Log("SteeringShutdown:" + LogitechGSDK.LogiSteeringShutdown().ToString());
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x0002B450 File Offset: 0x00029650
	private void OnGUI()
	{
		this.activeForces = GUI.TextArea(new Rect(10f, 10f, 180f, 200f), this.activeForces, 400);
		this.propertiesEdit = GUI.TextArea(new Rect(200f, 10f, 200f, 200f), this.propertiesEdit, 400);
		this.actualState = GUI.TextArea(new Rect(410f, 10f, 300f, 200f), this.actualState, 1000);
		this.buttonStatus = GUI.TextArea(new Rect(720f, 10f, 300f, 200f), this.buttonStatus, 1000);
		GUI.Label(new Rect(10f, 400f, 800f, 400f), this.forcesLabel);
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x0002B540 File Offset: 0x00029740
	private void Update()
	{
		if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			LogitechGSDK.LogiGetFriendlyProductName(0, stringBuilder, 256);
			string str = "Current Controller : ";
			StringBuilder stringBuilder2 = stringBuilder;
			this.propertiesEdit = str + ((stringBuilder2 != null) ? stringBuilder2.ToString() : null) + "\n";
			this.propertiesEdit += "Current controller properties : \n\n";
			LogitechGSDK.LogiControllerPropertiesData logiControllerPropertiesData = default(LogitechGSDK.LogiControllerPropertiesData);
			LogitechGSDK.LogiGetCurrentControllerProperties(0, ref logiControllerPropertiesData);
			this.propertiesEdit = this.propertiesEdit + "forceEnable = " + logiControllerPropertiesData.forceEnable.ToString() + "\n";
			this.propertiesEdit = this.propertiesEdit + "overallGain = " + logiControllerPropertiesData.overallGain.ToString() + "\n";
			this.propertiesEdit = this.propertiesEdit + "springGain = " + logiControllerPropertiesData.springGain.ToString() + "\n";
			this.propertiesEdit = this.propertiesEdit + "damperGain = " + logiControllerPropertiesData.damperGain.ToString() + "\n";
			this.propertiesEdit = this.propertiesEdit + "defaultSpringEnabled = " + logiControllerPropertiesData.defaultSpringEnabled.ToString() + "\n";
			this.propertiesEdit = this.propertiesEdit + "combinePedals = " + logiControllerPropertiesData.combinePedals.ToString() + "\n";
			this.propertiesEdit = this.propertiesEdit + "wheelRange = " + logiControllerPropertiesData.wheelRange.ToString() + "\n";
			this.propertiesEdit = this.propertiesEdit + "gameSettingsEnabled = " + logiControllerPropertiesData.gameSettingsEnabled.ToString() + "\n";
			this.propertiesEdit = this.propertiesEdit + "allowGameSettings = " + logiControllerPropertiesData.allowGameSettings.ToString() + "\n";
			this.actualState = "Steering wheel current state : \n\n";
			LogitechGSDK.DIJOYSTATE2ENGINES dijoystate2ENGINES = LogitechGSDK.LogiGetStateUnity(0);
			this.actualState = this.actualState + "x-axis position :" + dijoystate2ENGINES.lX.ToString() + "\n";
			this.actualState = this.actualState + "y-axis position :" + dijoystate2ENGINES.lY.ToString() + "\n";
			this.actualState = this.actualState + "z-axis position :" + dijoystate2ENGINES.lZ.ToString() + "\n";
			this.actualState = this.actualState + "x-axis rotation :" + dijoystate2ENGINES.lRx.ToString() + "\n";
			this.actualState = this.actualState + "y-axis rotation :" + dijoystate2ENGINES.lRy.ToString() + "\n";
			this.actualState = this.actualState + "z-axis rotation :" + dijoystate2ENGINES.lRz.ToString() + "\n";
			this.actualState = this.actualState + "extra axes positions 1 :" + dijoystate2ENGINES.rglSlider[0].ToString() + "\n";
			this.actualState = this.actualState + "extra axes positions 2 :" + dijoystate2ENGINES.rglSlider[1].ToString() + "\n";
			uint num = dijoystate2ENGINES.rgdwPOV[0];
			if (num <= 13500U)
			{
				if (num <= 4500U)
				{
					if (num == 0U)
					{
						this.actualState += "POV : UP\n";
						goto IL_499;
					}
					if (num == 4500U)
					{
						this.actualState += "POV : UP-RIGHT\n";
						goto IL_499;
					}
				}
				else
				{
					if (num == 9000U)
					{
						this.actualState += "POV : RIGHT\n";
						goto IL_499;
					}
					if (num == 13500U)
					{
						this.actualState += "POV : DOWN-RIGHT\n";
						goto IL_499;
					}
				}
			}
			else if (num <= 22500U)
			{
				if (num == 18000U)
				{
					this.actualState += "POV : DOWN\n";
					goto IL_499;
				}
				if (num == 22500U)
				{
					this.actualState += "POV : DOWN-LEFT\n";
					goto IL_499;
				}
			}
			else
			{
				if (num == 27000U)
				{
					this.actualState += "POV : LEFT\n";
					goto IL_499;
				}
				if (num == 31500U)
				{
					this.actualState += "POV : UP-LEFT\n";
					goto IL_499;
				}
			}
			this.actualState += "POV : CENTER\n";
			IL_499:
			this.buttonStatus = "Button pressed : \n\n";
			for (int i = 0; i < 128; i++)
			{
				if (dijoystate2ENGINES.rgbButtons[i] == 128)
				{
					this.buttonStatus = this.buttonStatus + "Button " + i.ToString() + " pressed\n";
				}
			}
			int num2 = LogitechGSDK.LogiGetShifterMode(0);
			string str2;
			if (num2 == 1)
			{
				str2 = "Gated";
			}
			else if (num2 == 0)
			{
				str2 = "Sequential";
			}
			else
			{
				str2 = "Unknown";
			}
			this.actualState = this.actualState + "\nSHIFTER MODE:" + str2;
			this.activeForces = "Active forces and effects :\n";
			if (Input.GetKeyUp(KeyCode.S))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 0))
				{
					LogitechGSDK.LogiStopSpringForce(0);
					this.activeForceAndEffect[0] = "";
				}
				else
				{
					LogitechGSDK.LogiPlaySpringForce(0, 50, 50, 50);
					this.activeForceAndEffect[0] = "Spring Force\n ";
				}
			}
			if (Input.GetKeyUp(KeyCode.C))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 1))
				{
					LogitechGSDK.LogiStopConstantForce(0);
					this.activeForceAndEffect[1] = "";
				}
				else
				{
					LogitechGSDK.LogiPlayConstantForce(0, 50);
					this.activeForceAndEffect[1] = "Constant Force\n ";
				}
			}
			if (Input.GetKeyUp(KeyCode.D))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 2))
				{
					LogitechGSDK.LogiStopDamperForce(0);
					this.activeForceAndEffect[2] = "";
				}
				else
				{
					LogitechGSDK.LogiPlayDamperForce(0, 50);
					this.activeForceAndEffect[2] = "Damper Force\n ";
				}
			}
			if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
			{
				LogitechGSDK.LogiPlaySideCollisionForce(0, 60);
			}
			if (Input.GetKeyUp(KeyCode.UpArrow))
			{
				LogitechGSDK.LogiPlayFrontalCollisionForce(0, 60);
			}
			if (Input.GetKeyUp(KeyCode.I))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 5))
				{
					LogitechGSDK.LogiStopDirtRoadEffect(0);
					this.activeForceAndEffect[3] = "";
				}
				else
				{
					LogitechGSDK.LogiPlayDirtRoadEffect(0, 50);
					this.activeForceAndEffect[3] = "Dirt Road Effect\n ";
				}
			}
			if (Input.GetKeyUp(KeyCode.B))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 6))
				{
					LogitechGSDK.LogiStopBumpyRoadEffect(0);
					this.activeForceAndEffect[4] = "";
				}
				else
				{
					LogitechGSDK.LogiPlayBumpyRoadEffect(0, 50);
					this.activeForceAndEffect[4] = "Bumpy Road Effect\n";
				}
			}
			if (Input.GetKeyUp(KeyCode.L))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 7))
				{
					LogitechGSDK.LogiStopSlipperyRoadEffect(0);
					this.activeForceAndEffect[5] = "";
				}
				else
				{
					LogitechGSDK.LogiPlaySlipperyRoadEffect(0, 50);
					this.activeForceAndEffect[5] = "Slippery Road Effect\n ";
				}
			}
			if (Input.GetKeyUp(KeyCode.U))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 8))
				{
					LogitechGSDK.LogiStopSurfaceEffect(0);
					this.activeForceAndEffect[6] = "";
				}
				else
				{
					LogitechGSDK.LogiPlaySurfaceEffect(0, 1, 50, 1000);
					this.activeForceAndEffect[6] = "Surface Effect\n";
				}
			}
			if (Input.GetKeyUp(KeyCode.A))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 11))
				{
					LogitechGSDK.LogiStopCarAirborne(0);
					this.activeForceAndEffect[7] = "";
				}
				else
				{
					LogitechGSDK.LogiPlayCarAirborne(0);
					this.activeForceAndEffect[7] = "Car Airborne\n ";
				}
			}
			if (Input.GetKeyUp(KeyCode.O))
			{
				if (LogitechGSDK.LogiIsPlaying(0, 10))
				{
					LogitechGSDK.LogiStopSoftstopForce(0);
					this.activeForceAndEffect[8] = "";
				}
				else
				{
					LogitechGSDK.LogiPlaySoftstopForce(0, 20);
					this.activeForceAndEffect[8] = "Soft Stop Force\n";
				}
			}
			if (Input.GetKeyUp(KeyCode.PageUp))
			{
				this.properties.wheelRange = 90;
				this.properties.forceEnable = true;
				this.properties.overallGain = 80;
				this.properties.springGain = 80;
				this.properties.damperGain = 80;
				this.properties.allowGameSettings = true;
				this.properties.combinePedals = false;
				this.properties.defaultSpringEnabled = true;
				this.properties.defaultSpringGain = 80;
				LogitechGSDK.LogiSetPreferredControllerProperties(this.properties);
			}
			if (Input.GetKeyUp(KeyCode.P))
			{
				LogitechGSDK.LogiPlayLeds(0, 20f, 20f, 20f);
			}
			for (int j = 0; j < 9; j++)
			{
				this.activeForces += this.activeForceAndEffect[j];
			}
			return;
		}
		if (!LogitechGSDK.LogiIsConnected(0))
		{
			this.actualState = "PLEASE PLUG IN A STEERING WHEEL OR A FORCE FEEDBACK CONTROLLER";
			return;
		}
		this.actualState = "THIS WINDOW NEEDS TO BE IN FOREGROUND IN ORDER FOR THE SDK TO WORK PROPERLY";
	}

	// Token: 0x040007ED RID: 2029
	private LogitechGSDK.LogiControllerPropertiesData properties;

	// Token: 0x040007EE RID: 2030
	private string actualState;

	// Token: 0x040007EF RID: 2031
	private string activeForces;

	// Token: 0x040007F0 RID: 2032
	private string propertiesEdit;

	// Token: 0x040007F1 RID: 2033
	private string buttonStatus;

	// Token: 0x040007F2 RID: 2034
	private string forcesLabel;

	// Token: 0x040007F3 RID: 2035
	private string[] activeForceAndEffect;
}
