using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010C RID: 268
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[AddComponentMenu("First Person AIO")]
public class FirstPersonAIO : MonoBehaviour
{
	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0002C508 File Offset: 0x0002A708
	// (set) Token: 0x060005D3 RID: 1491 RVA: 0x0002C510 File Offset: 0x0002A710
	public bool IsGrounded { get; private set; }

	// Token: 0x060005D4 RID: 1492 RVA: 0x0002C519 File Offset: 0x0002A719
	public void Sick()
	{
		this.walkSpeedInternal = this.walkSpeed / 2f;
		this.sprintSpeedInternal = this.walkSpeed / 2f;
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x0002C53F File Offset: 0x0002A73F
	public void Healthy()
	{
		this.walkSpeedInternal = this.walkSpeed;
		this.sprintSpeedInternal = this.walkSpeed;
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0002C55C File Offset: 0x0002A75C
	public void Awake()
	{
		this.player = ReInput.players.GetPlayer(0);
		if (PlayerPrefs.HasKey("masterInvertY"))
		{
			if (PlayerPrefs.GetInt("masterInvertY") == 0)
			{
				this.mouseInputInversion = FirstPersonAIO.InvertMouseInput.None;
			}
			if (PlayerPrefs.GetInt("masterInvertY") == 1)
			{
				this.mouseInputInversion = FirstPersonAIO.InvertMouseInput.Y;
			}
		}
		this.originalRotation = base.transform.localRotation.eulerAngles;
		this.walkSpeedInternal = this.walkSpeed;
		this.sprintSpeedInternal = this.sprintSpeed;
		this.jumpPowerInternal = this.jumpPower;
		this.capsule = base.GetComponent<CapsuleCollider>();
		this.IsGrounded = true;
		FirstPersonAIO.isCrouching = false;
		this.fps_Rigidbody = base.GetComponent<Rigidbody>();
		this.fps_Rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
		this.fps_Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
		this._crouchModifiers.colliderHeight = this.capsule.height;
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0002C640 File Offset: 0x0002A840
	public void Start()
	{
		this.followAngles.x = 0f;
		this.followAngles.y = 0f;
		if (this.autoCrosshair || this.drawStaminaMeter)
		{
			Canvas canvas = new GameObject("AutoCrosshair").AddComponent<Canvas>();
			canvas.gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.pixelPerfect = true;
			canvas.transform.SetParent(this.playerCamera.transform);
			canvas.transform.position = Vector3.zero;
			if (this.autoCrosshair)
			{
				Image image = new GameObject("Crosshair").AddComponent<Image>();
				image.sprite = this.Crosshair;
				image.rectTransform.sizeDelta = new Vector2(25f, 25f);
				image.transform.SetParent(canvas.transform);
				image.transform.position = Vector3.zero;
			}
			if (this.drawStaminaMeter)
			{
				this.StaminaMeterBG = new GameObject("StaminaMeter").AddComponent<Image>();
				this.StaminaMeter = new GameObject("Meter").AddComponent<Image>();
				this.StaminaMeter.transform.SetParent(this.StaminaMeterBG.transform);
				this.StaminaMeterBG.transform.SetParent(canvas.transform);
				this.StaminaMeterBG.transform.position = Vector3.zero;
				this.StaminaMeterBG.rectTransform.anchorMax = new Vector2(0.5f, 0f);
				this.StaminaMeterBG.rectTransform.anchorMin = new Vector2(0.5f, 0f);
				this.StaminaMeterBG.rectTransform.anchoredPosition = new Vector2(0f, 15f);
				this.StaminaMeterBG.rectTransform.sizeDelta = new Vector2(250f, 6f);
				this.StaminaMeterBG.color = new Color(0f, 0f, 0f, 0f);
				this.StaminaMeter.rectTransform.sizeDelta = new Vector2(250f, 6f);
				this.StaminaMeter.color = new Color(0f, 0f, 0f, 0f);
			}
		}
		this.cameraStartingPosition = this.playerCamera.transform.localPosition;
		if (this.lockAndHideCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		this.baseCamFOV = this.playerCamera.fieldOfView;
		this.staminaInternal = this.staminaLevel;
		this.advanced.zeroFrictionMaterial = new PhysicMaterial("Zero_Friction");
		this.advanced.zeroFrictionMaterial.dynamicFriction = 0f;
		this.advanced.zeroFrictionMaterial.staticFriction = 0f;
		this.advanced.zeroFrictionMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
		this.advanced.zeroFrictionMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
		this.advanced.highFrictionMaterial = new PhysicMaterial("Max_Friction");
		this.advanced.highFrictionMaterial.dynamicFriction = 1f;
		this.advanced.highFrictionMaterial.staticFriction = 1f;
		this.advanced.highFrictionMaterial.frictionCombine = PhysicMaterialCombine.Maximum;
		this.advanced.highFrictionMaterial.bounceCombine = PhysicMaterialCombine.Average;
		this.originalLocalPosition = (this.snapHeadjointToCapsul ? new Vector3(this.head.localPosition.x, (this.capsule.height - 0.5f) / 2f * this.head.localScale.y, this.head.localPosition.z) : this.head.localPosition);
		if (base.GetComponent<AudioSource>() == null)
		{
			base.gameObject.AddComponent<AudioSource>();
		}
		this.previousPosition = this.fps_Rigidbody.position;
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0002CA38 File Offset: 0x0002AC38
	public void UseHeadbob()
	{
		this.headbobSwayAngle = 2.5f;
		this.headbobHeight = 1f;
		this.headbobSideMovement = 0.5f;
		this.jumpLandIntensity = 3f;
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0002CA66 File Offset: 0x0002AC66
	public void DontUseHeadbob()
	{
		this.headbobSwayAngle = 0f;
		this.headbobHeight = 0f;
		this.headbobSideMovement = 0f;
		this.jumpLandIntensity = 0f;
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0002CA94 File Offset: 0x0002AC94
	private void Update()
	{
		if (this.enableCameraMovement && !this.controllerPauseState)
		{
			float num = 0f;
			float fieldOfView = this.playerCamera.fieldOfView;
			float num2;
			if (this.cameraInputMethod == FirstPersonAIO.CameraInputMethod.Traditional || this.cameraInputMethod == FirstPersonAIO.CameraInputMethod.TraditionalWithConstraints)
			{
				num = ((this.mouseInputInversion == FirstPersonAIO.InvertMouseInput.None || this.mouseInputInversion == FirstPersonAIO.InvertMouseInput.X) ? (Input.GetAxis("Mouse Y") + this.player.GetAxis("LookingVertical")) : (-Input.GetAxis("Mouse Y") - this.player.GetAxis("LookingVertical")));
				num2 = ((this.mouseInputInversion == FirstPersonAIO.InvertMouseInput.None || this.mouseInputInversion == FirstPersonAIO.InvertMouseInput.Y) ? (Input.GetAxis("Mouse X") + this.player.GetAxis("LookingHorizontal")) : (-Input.GetAxis("Mouse X") - this.player.GetAxis("LookingHorizontal")));
			}
			else
			{
				num2 = Input.GetAxis("Horizontal") * (float)((this.mouseInputInversion == FirstPersonAIO.InvertMouseInput.None || this.mouseInputInversion == FirstPersonAIO.InvertMouseInput.Y) ? 1 : -1);
			}
			if (this.targetAngles.y > 180f)
			{
				this.targetAngles.y = this.targetAngles.y - 360f;
				this.followAngles.y = this.followAngles.y - 360f;
			}
			else if (this.targetAngles.y < -180f)
			{
				this.targetAngles.y = this.targetAngles.y + 360f;
				this.followAngles.y = this.followAngles.y + 360f;
			}
			if (this.targetAngles.x > 180f)
			{
				this.targetAngles.x = this.targetAngles.x - 360f;
				this.followAngles.x = this.followAngles.x - 360f;
			}
			else if (this.targetAngles.x < -180f)
			{
				this.targetAngles.x = this.targetAngles.x + 360f;
				this.followAngles.x = this.followAngles.x + 360f;
			}
			this.targetAngles.y = this.targetAngles.y + num2 * (this.mouseSensitivity - (this.baseCamFOV - fieldOfView) * this.fOVToMouseSensitivity / 6f);
			if (this.cameraInputMethod == FirstPersonAIO.CameraInputMethod.Traditional)
			{
				this.targetAngles.x = this.targetAngles.x + num * (this.mouseSensitivity - (this.baseCamFOV - fieldOfView) * this.fOVToMouseSensitivity / 6f);
			}
			else
			{
				this.targetAngles.x = 0f;
			}
			this.targetAngles.x = Mathf.Clamp(this.targetAngles.x, -0.5f * this.verticalRotationRange, 0.5f * this.verticalRotationRange);
			this.followAngles = Vector3.SmoothDamp(this.followAngles, this.targetAngles, ref this.followVelocity, this.cameraSmoothing / 100f);
			if (!this.leaned)
			{
				this.playerCamera.transform.localRotation = Quaternion.Euler(-this.followAngles.x + this.originalRotation.x, 0f, 0f);
				base.transform.localRotation = Quaternion.Euler(0f, this.followAngles.y + this.originalRotation.y, 0f);
			}
		}
		if (this.player.GetButton("Jump") && this.jumpheight < 2f)
		{
			this.jumpheight += 0.1f;
		}
		if (this.canHoldJump ? (this.canJump && this.player.GetButton("Jump")) : (this.player.GetButtonUp("Jump") && this.canJump))
		{
			this.jumpInput = true;
		}
		else if (this.player.GetButtonUp("Jump"))
		{
			this.jumpInput = false;
		}
		if (this.player.GetButtonDown("Crouch"))
		{
			if (!FirstPersonAIO.isCrouching)
			{
				if (!FirstPersonAIO.isCrouching2)
				{
					FirstPersonAIO.isCrouching = true;
					return;
				}
				FirstPersonAIO.isCrouching2 = false;
				return;
			}
			else if (FirstPersonAIO.isCrouching && this.player.GetButtonDown("Crouch"))
			{
				FirstPersonAIO.isCrouching2 = true;
				FirstPersonAIO.isCrouching = this._crouchModifiers.crouchOverride;
			}
		}
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0002CEBC File Offset: 0x0002B0BC
	private void FixedUpdate()
	{
		if (this.useStamina)
		{
			this.isSprinting = (this.player.GetButton("Sprint") && !FirstPersonAIO.isCrouching && this.staminaInternal > 0f && (Mathf.Abs(this.fps_Rigidbody.velocity.x) > 0.01f || Mathf.Abs(this.fps_Rigidbody.velocity.z) > 0.01f));
			if (this.isSprinting)
			{
				this.staminaInternal -= this.staminaDepletionSpeed * 2f * Time.deltaTime;
				if (this.drawStaminaMeter)
				{
					this.StaminaMeterBG.color = Vector4.MoveTowards(this.StaminaMeterBG.color, new Vector4(0f, 0f, 0f, 0.5f), 0.15f);
					this.StaminaMeter.color = Vector4.MoveTowards(this.StaminaMeter.color, new Vector4(1f, 1f, 1f, 1f), 0.15f);
				}
			}
			else if ((!this.player.GetButton("Sprint") || Mathf.Abs(this.fps_Rigidbody.velocity.x) < 0.01f || Mathf.Abs(this.fps_Rigidbody.velocity.z) < 0.01f || FirstPersonAIO.isCrouching) && this.staminaInternal < this.staminaLevel)
			{
				this.staminaInternal += this.staminaDepletionSpeed * Time.deltaTime;
			}
			if (this.drawStaminaMeter)
			{
				if (this.staminaInternal == this.staminaLevel)
				{
					this.StaminaMeterBG.color = Vector4.MoveTowards(this.StaminaMeterBG.color, new Vector4(0f, 0f, 0f, 0f), 0.15f);
					this.StaminaMeter.color = Vector4.MoveTowards(this.StaminaMeter.color, new Vector4(1f, 1f, 1f, 0f), 0.15f);
				}
				float x = Mathf.Clamp(Mathf.SmoothDamp(this.StaminaMeter.transform.localScale.x, this.staminaInternal / this.staminaLevel * this.StaminaMeterBG.transform.localScale.x, ref this.smoothRef, 1f * Time.deltaTime, 1f), 0.001f, this.StaminaMeterBG.transform.localScale.x);
				this.StaminaMeter.transform.localScale = new Vector3(x, 1f, 1f);
			}
			this.staminaInternal = Mathf.Clamp(this.staminaInternal, 0f, this.staminaLevel);
		}
		else
		{
			this.isSprinting = this.player.GetButton("Sprint");
		}
		if (tools.Needs == 1)
		{
			this.walkSpeedInternal = this.walkSpeed * 0.2f + this.walkSpeed * 0.8f * tools.Health;
			this.sprintSpeedInternal = this.walkSpeedInternal * 2f;
		}
		Vector3 vector = Vector3.zero;
		this.speed = (this.walkByDefault ? (FirstPersonAIO.isCrouching ? this.walkSpeedInternal : (this.isSprinting ? this.sprintSpeedInternal : this.walkSpeedInternal)) : (this.isSprinting ? this.walkSpeedInternal : this.sprintSpeedInternal));
		if (this.advanced.maxSlopeAngle > 0f)
		{
			if (this.advanced.isTouchingUpright && this.advanced.isTouchingWalkable)
			{
				vector = base.transform.forward * this.inputXY.y * this.speed + base.transform.right * this.inputXY.x * this.walkSpeedInternal;
				if (!this.didJump)
				{
					this.fps_Rigidbody.constraints = (RigidbodyConstraints)116;
				}
			}
			else if (this.advanced.isTouchingUpright && !this.advanced.isTouchingWalkable)
			{
				this.fps_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			}
			else
			{
				this.fps_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
				vector = (base.transform.forward * this.inputXY.y * this.speed + base.transform.right * this.inputXY.x * this.walkSpeedInternal) * ((this.fps_Rigidbody.velocity.y > 0.01f) ? this.SlopeCheck() : 0.8f);
			}
		}
		else
		{
			vector = base.transform.forward * this.inputXY.y * this.speed + base.transform.right * this.inputXY.x * this.walkSpeedInternal;
		}
		if (this.leaned)
		{
			vector = base.transform.forward * this.inputXY.y * this.speed + this.playerCamera.transform.right * this.inputXY.x * this.walkSpeedInternal;
		}
		int mask = LayerMask.GetMask(new string[]
		{
			"Default",
			"StepLayer"
		});
		RaycastHit raycastHit;
		RaycastHit raycastHit2;
		if (this.advanced.maxStepHeight > 0f && Physics.Raycast(base.transform.position - new Vector3(0f, (this.capsule.height / 2f + 0.25f) * base.transform.localScale.y - 0.01f, 0f), vector, out raycastHit, this.capsule.radius + 0.15f, mask, QueryTriggerInteraction.Ignore) && Vector3.Angle(raycastHit.normal, Vector3.up) > 88f && !Physics.Raycast(base.transform.position - new Vector3(0f, this.capsule.height / 2f * base.transform.localScale.y - this.advanced.maxStepHeight, 0f), vector, out raycastHit2, this.capsule.radius + 0.25f, mask, QueryTriggerInteraction.Ignore))
		{
			this.advanced.stairMiniHop = true;
			base.transform.position += new Vector3(0f, this.advanced.maxStepHeight * 1.2f, 0f);
		}
		Debug.DrawRay(base.transform.position, vector, Color.red, 0f, false);
		float axis = this.player.GetAxis("Horizontal");
		float axis2 = this.player.GetAxis("Vertical");
		this.inputXY = new Vector2(axis, axis2);
		if (this.inputXY.magnitude > 1f)
		{
			this.inputXY.Normalize();
		}
		this.yVelocity = this.fps_Rigidbody.velocity.y;
		if (this.IsGrounded && this.jumpInput && this.jumpPowerInternal > 0f && !this.didJump)
		{
			if (this.advanced.maxSlopeAngle > 0f)
			{
				if (this.advanced.isTouchingFlat || this.advanced.isTouchingWalkable)
				{
					this.didJump = true;
					this.jumpInput = false;
					this.yVelocity += ((this.fps_Rigidbody.velocity.y < 0.01f) ? (this.jumpPowerInternal + this.jumpheight) : ((this.jumpPowerInternal + this.jumpheight) / 3f));
					this.advanced.isTouchingWalkable = false;
					this.advanced.isTouchingFlat = false;
					this.advanced.isTouchingUpright = false;
					this.fps_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
				}
			}
			else
			{
				this.didJump = true;
				this.jumpInput = false;
				this.yVelocity += this.jumpPowerInternal + this.jumpheight;
			}
			this.jumpheight = 0f;
		}
		if (this.advanced.maxSlopeAngle > 0f)
		{
			if (!this.didJump && this.advanced.lastKnownSlopeAngle > 5f && this.advanced.isTouchingWalkable)
			{
				this.yVelocity *= this.SlopeCheck() / 4f;
			}
			if (this.advanced.isTouchingUpright && !this.advanced.isTouchingWalkable && !this.didJump)
			{
				this.yVelocity += Physics.gravity.y;
			}
		}
		if (this.playerCanMove && !this.controllerPauseState)
		{
			this.fps_Rigidbody.velocity = vector + Vector3.up * this.yVelocity;
		}
		else
		{
			this.fps_Rigidbody.velocity = Vector3.zero;
		}
		if (this.inputXY.magnitude > 0f || !this.IsGrounded)
		{
			this.capsule.sharedMaterial = this.advanced.zeroFrictionMaterial;
		}
		else
		{
			this.capsule.sharedMaterial = this.advanced.highFrictionMaterial;
		}
		this.fps_Rigidbody.AddForce(Physics.gravity * (this.advanced.gravityMultiplier - 1f));
		if (this.advanced.FOVKickAmount > 0f)
		{
			if (this.isSprinting && !FirstPersonAIO.isCrouching && this.playerCamera.fieldOfView != this.baseCamFOV + this.advanced.FOVKickAmount * 2f - 0.01f)
			{
				if (Mathf.Abs(this.fps_Rigidbody.velocity.x) > 0.5f || Mathf.Abs(this.fps_Rigidbody.velocity.z) > 0.5f)
				{
					this.playerCamera.fieldOfView = Mathf.SmoothDamp(this.playerCamera.fieldOfView, this.baseCamFOV + this.advanced.FOVKickAmount * 2f, ref this.advanced.fovRef, this.advanced.changeTime);
				}
			}
			else if (this.playerCamera.fieldOfView != this.baseCamFOV)
			{
				this.playerCamera.fieldOfView = Mathf.SmoothDamp(this.playerCamera.fieldOfView, this.baseCamFOV, ref this.advanced.fovRef, this.advanced.changeTime * 0.5f);
			}
		}
		float y = 0f;
		float x2 = 0f;
		float z = 0f;
		float x3 = 0f;
		if (this.useHeadbob || this.enableAudioSFX)
		{
			Vector3 vector2 = (this.fps_Rigidbody.position - this.previousPosition) / Time.deltaTime;
			Vector3 vector3 = vector2 - this.previousVelocity;
			this.previousPosition = this.fps_Rigidbody.position;
			this.previousVelocity = vector2;
			this.springVelocity -= vector3.y;
			this.springVelocity -= this.springPosition * this.springElastic;
			this.springVelocity *= this.springDampen;
			this.springPosition += this.springVelocity * Time.deltaTime;
			this.springPosition = Mathf.Clamp(this.springPosition, -0.3f, 0.3f);
			if (Mathf.Abs(this.springVelocity) < this.springVelocityThreshold && Mathf.Abs(this.springPosition) < this.springPositionThreshold)
			{
				this.springPosition = 0f;
				this.springVelocity = 0f;
			}
			float magnitude = new Vector3(vector2.x, 0f, vector2.z).magnitude;
			float num = 1f + magnitude * (this.headbobFrequency * 2f / 10f);
			this.headbobCycle += magnitude / num * (Time.deltaTime / this.headbobFrequency);
			float num2 = Mathf.Sin(this.headbobCycle * 3.1415927f * 2f);
			float num3 = Mathf.Sin(3.1415927f * (2f * this.headbobCycle + 0.5f));
			num2 = 1f - (num2 * 0.5f + 1f);
			num2 *= num2;
			y = 0f;
			x2 = 0f;
			z = 0f;
			if (this.jumpLandIntensity > 0f && !this.advanced.stairMiniHop)
			{
				x3 = -this.springPosition * (this.jumpLandIntensity * 5.5f);
			}
			else if (!this.advanced.stairMiniHop)
			{
				x3 = -this.springPosition;
			}
			if (this.IsGrounded)
			{
				if (new Vector3(vector2.x, 0f, vector2.z).magnitude < 0.1f)
				{
					this.headbobFade = Mathf.MoveTowards(this.headbobFade, 0f, 0.5f);
				}
				else
				{
					this.headbobFade = Mathf.MoveTowards(this.headbobFade, 1f, Time.deltaTime);
				}
				float num4 = 1f + magnitude * 0.3f;
				x2 = -(this.headbobSideMovement / 10f) * this.headbobFade * num3;
				y = this.springPosition * (this.jumpLandIntensity / 10f) + num2 * (this.headbobHeight / 10f) * this.headbobFade * num4;
				z = num3 * (this.headbobSwayAngle / 10f) * this.headbobFade;
			}
		}
		if (this.useHeadbob)
		{
			if (this.fps_Rigidbody.velocity.magnitude > 0.1f)
			{
				this.head.localPosition = Vector3.MoveTowards(this.head.localPosition, this.snapHeadjointToCapsul ? (new Vector3(this.originalLocalPosition.x, (this.capsule.height - 0.5f) / 2f * this.head.localScale.y, this.originalLocalPosition.z) + new Vector3(x2, y, 0f)) : (this.originalLocalPosition + new Vector3(x2, y, 0f)), 0.5f);
			}
			else
			{
				this.head.localPosition = Vector3.SmoothDamp(this.head.localPosition, this.snapHeadjointToCapsul ? (new Vector3(this.originalLocalPosition.x, (this.capsule.height - 0.5f) / 2f * this.head.localScale.y, this.originalLocalPosition.z) + new Vector3(x2, y, 0f)) : (this.originalLocalPosition + new Vector3(x2, y, 0f)), ref this.miscRefVel, 0.15f);
			}
			this.head.localRotation = Quaternion.Euler(x3, 0f, z);
		}
		if (this.enableAudioSFX)
		{
			if (this.fsmode == FirstPersonAIO.FSMode.Dynamic)
			{
				RaycastHit raycastHit3 = default(RaycastHit);
				if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit3))
				{
					if (this.dynamicFootstep.materialMode == FirstPersonAIO.DynamicFootStep.matMode.physicMaterial)
					{
						this.dynamicFootstep.currentClipSet = ((this.dynamicFootstep.woodPhysMat.Any<PhysicMaterial>() && this.dynamicFootstep.woodPhysMat.Contains(raycastHit3.collider.sharedMaterial) && this.dynamicFootstep.woodClipSet.Any<AudioClip>()) ? this.dynamicFootstep.woodClipSet : ((this.dynamicFootstep.grassPhysMat.Any<PhysicMaterial>() && this.dynamicFootstep.grassPhysMat.Contains(raycastHit3.collider.sharedMaterial) && this.dynamicFootstep.grassClipSet.Any<AudioClip>()) ? this.dynamicFootstep.grassClipSet : ((this.dynamicFootstep.metalAndGlassPhysMat.Any<PhysicMaterial>() && this.dynamicFootstep.metalAndGlassPhysMat.Contains(raycastHit3.collider.sharedMaterial) && this.dynamicFootstep.metalAndGlassClipSet.Any<AudioClip>()) ? this.dynamicFootstep.metalAndGlassClipSet : ((this.dynamicFootstep.rockAndConcretePhysMat.Any<PhysicMaterial>() && this.dynamicFootstep.rockAndConcretePhysMat.Contains(raycastHit3.collider.sharedMaterial) && this.dynamicFootstep.rockAndConcreteClipSet.Any<AudioClip>()) ? this.dynamicFootstep.rockAndConcreteClipSet : ((this.dynamicFootstep.dirtAndGravelPhysMat.Any<PhysicMaterial>() && this.dynamicFootstep.dirtAndGravelPhysMat.Contains(raycastHit3.collider.sharedMaterial) && this.dynamicFootstep.dirtAndGravelClipSet.Any<AudioClip>()) ? this.dynamicFootstep.dirtAndGravelClipSet : ((this.dynamicFootstep.mudPhysMat.Any<PhysicMaterial>() && this.dynamicFootstep.mudPhysMat.Contains(raycastHit3.collider.sharedMaterial) && this.dynamicFootstep.mudClipSet.Any<AudioClip>()) ? this.dynamicFootstep.mudClipSet : ((this.dynamicFootstep.customPhysMat.Any<PhysicMaterial>() && this.dynamicFootstep.customPhysMat.Contains(raycastHit3.collider.sharedMaterial) && this.dynamicFootstep.customClipSet.Any<AudioClip>()) ? this.dynamicFootstep.customClipSet : this.footStepSounds)))))));
					}
					else if (raycastHit3.collider.GetComponent<MeshRenderer>())
					{
						this.dynamicFootstep.currentClipSet = ((this.dynamicFootstep.woodMat.Any<Material>() && this.dynamicFootstep.woodMat.Contains(raycastHit3.collider.GetComponent<MeshRenderer>().sharedMaterial) && this.dynamicFootstep.woodClipSet.Any<AudioClip>()) ? this.dynamicFootstep.woodClipSet : ((this.dynamicFootstep.grassMat.Any<Material>() && this.dynamicFootstep.grassMat.Contains(raycastHit3.collider.GetComponent<MeshRenderer>().sharedMaterial) && this.dynamicFootstep.grassClipSet.Any<AudioClip>()) ? this.dynamicFootstep.grassClipSet : ((this.dynamicFootstep.metalAndGlassMat.Any<Material>() && this.dynamicFootstep.metalAndGlassMat.Contains(raycastHit3.collider.GetComponent<MeshRenderer>().sharedMaterial) && this.dynamicFootstep.metalAndGlassClipSet.Any<AudioClip>()) ? this.dynamicFootstep.metalAndGlassClipSet : ((this.dynamicFootstep.rockAndConcreteMat.Any<Material>() && this.dynamicFootstep.rockAndConcreteMat.Contains(raycastHit3.collider.GetComponent<MeshRenderer>().sharedMaterial) && this.dynamicFootstep.rockAndConcreteClipSet.Any<AudioClip>()) ? this.dynamicFootstep.rockAndConcreteClipSet : ((this.dynamicFootstep.dirtAndGravelMat.Any<Material>() && this.dynamicFootstep.dirtAndGravelMat.Contains(raycastHit3.collider.GetComponent<MeshRenderer>().sharedMaterial) && this.dynamicFootstep.dirtAndGravelClipSet.Any<AudioClip>()) ? this.dynamicFootstep.dirtAndGravelClipSet : ((this.dynamicFootstep.mudMat.Any<Material>() && this.dynamicFootstep.mudMat.Contains(raycastHit3.collider.GetComponent<MeshRenderer>().sharedMaterial) && this.dynamicFootstep.mudClipSet.Any<AudioClip>()) ? this.dynamicFootstep.mudClipSet : ((this.dynamicFootstep.customMat.Any<Material>() && this.dynamicFootstep.customMat.Contains(raycastHit3.collider.GetComponent<MeshRenderer>().sharedMaterial) && this.dynamicFootstep.customClipSet.Any<AudioClip>()) ? this.dynamicFootstep.customClipSet : (this.footStepSounds.Any<AudioClip>() ? this.footStepSounds : null))))))));
					}
					if (this.IsGrounded)
					{
						if (!this.previousGrounded)
						{
							if (this.dynamicFootstep.currentClipSet.Any<AudioClip>())
							{
								this.audioSource.PlayOneShot(this.dynamicFootstep.currentClipSet[UnityEngine.Random.Range(0, this.dynamicFootstep.currentClipSet.Count)], this.Volume / 10f);
							}
							this.nextStepTime = this.headbobCycle + 0.5f;
						}
						else if (this.headbobCycle > this.nextStepTime)
						{
							this.nextStepTime = this.headbobCycle + 0.5f;
							if (this.dynamicFootstep.currentClipSet.Any<AudioClip>())
							{
								this.audioSource.PlayOneShot(this.dynamicFootstep.currentClipSet[UnityEngine.Random.Range(0, this.dynamicFootstep.currentClipSet.Count)], this.Volume / 10f);
							}
						}
						this.previousGrounded = true;
					}
					else
					{
						if (this.previousGrounded && this.dynamicFootstep.currentClipSet.Any<AudioClip>())
						{
							this.audioSource.PlayOneShot(this.dynamicFootstep.currentClipSet[UnityEngine.Random.Range(0, this.dynamicFootstep.currentClipSet.Count)], this.Volume / 10f);
						}
						this.previousGrounded = false;
					}
				}
				else
				{
					this.dynamicFootstep.currentClipSet = this.footStepSounds;
					if (this.IsGrounded)
					{
						if (!this.previousGrounded)
						{
							if (this.landSound)
							{
								this.audioSource.PlayOneShot(this.landSound, this.Volume / 10f);
							}
							this.nextStepTime = this.headbobCycle + 0.5f;
						}
						else if (this.headbobCycle > this.nextStepTime)
						{
							this.nextStepTime = this.headbobCycle + 0.5f;
							int index = UnityEngine.Random.Range(0, this.footStepSounds.Count);
							if (this.footStepSounds.Any<AudioClip>())
							{
								this.audioSource.PlayOneShot(this.footStepSounds[index], this.Volume / 10f);
							}
							this.footStepSounds[index] = this.footStepSounds[0];
						}
						this.previousGrounded = true;
					}
					else
					{
						if (this.previousGrounded && this.jumpSound)
						{
							this.audioSource.PlayOneShot(this.jumpSound, this.Volume / 10f);
						}
						this.previousGrounded = false;
					}
				}
			}
			else if (this.IsGrounded)
			{
				if (!this.previousGrounded)
				{
					if (this.landSound)
					{
						this.audioSource.PlayOneShot(this.landSound, this.Volume / 10f);
					}
					this.nextStepTime = this.headbobCycle + 0.5f;
				}
				else if (this.headbobCycle > this.nextStepTime)
				{
					this.nextStepTime = this.headbobCycle + 0.5f;
					int index2 = UnityEngine.Random.Range(0, this.footStepSounds.Count);
					if (this.footStepSounds.Any<AudioClip>() && this.footStepSounds[index2] != null && !FirstPersonAIO.isCrouching)
					{
						this.audioSource.PlayOneShot(this.footStepSounds[index2], this.Volume / 10f);
					}
				}
				this.previousGrounded = true;
			}
			else
			{
				if (this.previousGrounded && this.jumpSound)
				{
					this.audioSource.PlayOneShot(this.jumpSound, this.Volume / 10f);
				}
				this.previousGrounded = false;
			}
		}
		this.IsGrounded = false;
		if (this.advanced.maxSlopeAngle > 0f)
		{
			if (this.advanced.isTouchingFlat || this.advanced.isTouchingWalkable || this.advanced.isTouchingUpright)
			{
				this.didJump = false;
			}
			this.advanced.isTouchingWalkable = false;
			this.advanced.isTouchingUpright = false;
			this.advanced.isTouchingFlat = false;
		}
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0002E7B9 File Offset: 0x0002C9B9
	public IEnumerator CameraShake(float Duration, float Magnitude)
	{
		float elapsed = 0f;
		while (elapsed < Duration && this.enableCameraShake)
		{
			this.playerCamera.transform.localPosition = Vector3.MoveTowards(this.playerCamera.transform.localPosition, new Vector3(this.cameraStartingPosition.x + (float)UnityEngine.Random.Range(-1, 1) * Magnitude, this.cameraStartingPosition.y + (float)UnityEngine.Random.Range(-1, 1) * Magnitude, this.cameraStartingPosition.z), Magnitude * 2f);
			yield return new WaitForSecondsRealtime(0.001f);
			elapsed += Time.deltaTime;
			yield return null;
		}
		this.playerCamera.transform.localPosition = this.cameraStartingPosition;
		yield break;
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0002E7D8 File Offset: 0x0002C9D8
	public void RotateCamera(Vector2 Rotation, bool Snap)
	{
		this.enableCameraMovement = !this.enableCameraMovement;
		if (Snap)
		{
			this.followAngles = Rotation;
			this.targetAngles = Rotation;
		}
		else
		{
			this.targetAngles = Rotation;
		}
		this.enableCameraMovement = !this.enableCameraMovement;
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0002E82C File Offset: 0x0002CA2C
	public void ControllerPause()
	{
		this.controllerPauseState = true;
		if (this.lockAndHideCursor)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0002E849 File Offset: 0x0002CA49
	public void ControllerUnPause()
	{
		this.controllerPauseState = false;
		if (this.lockAndHideCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0002E868 File Offset: 0x0002CA68
	private float SlopeCheck()
	{
		this.advanced.lastKnownSlopeAngle = Mathf.MoveTowards(this.advanced.lastKnownSlopeAngle, Vector3.Angle(this.advanced.curntGroundNormal, Vector3.up), 5f);
		return new AnimationCurve(new Keyframe[]
		{
			new Keyframe(-90f, 1f),
			new Keyframe(0f, 1f),
			new Keyframe(this.advanced.maxSlopeAngle + 15f, 0f),
			new Keyframe(this.advanced.maxWallShear, 0f),
			new Keyframe(this.advanced.maxWallShear + 0.1f, 1f),
			new Keyframe(90f, 1f)
		})
		{
			preWrapMode = WrapMode.Once,
			postWrapMode = WrapMode.ClampForever
		}.Evaluate(this.advanced.lastKnownSlopeAngle);
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0002E978 File Offset: 0x0002CB78
	private void OnCollisionEnter(Collision CollisionData)
	{
		for (int i = 0; i < CollisionData.contactCount; i++)
		{
			float num = Vector3.Angle(CollisionData.GetContact(i).normal, Vector3.up);
			if (CollisionData.GetContact(i).point.y < base.transform.position.y - (this.capsule.height / 2f - this.capsule.radius * 0.95f))
			{
				if (!this.IsGrounded)
				{
					this.IsGrounded = true;
					this.advanced.stairMiniHop = false;
					if (this.didJump && num <= 70f)
					{
						this.didJump = false;
					}
				}
				if (this.advanced.maxSlopeAngle > 0f)
				{
					if (num < 5.1f)
					{
						this.advanced.isTouchingFlat = true;
						this.advanced.isTouchingWalkable = true;
					}
					else if (num < this.advanced.maxSlopeAngle + 0.1f)
					{
						this.advanced.isTouchingWalkable = true;
					}
					else if (num < 90f)
					{
						this.advanced.isTouchingUpright = true;
					}
					this.advanced.curntGroundNormal = CollisionData.GetContact(i).normal;
				}
			}
		}
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0002EABC File Offset: 0x0002CCBC
	private void OnCollisionStay(Collision CollisionData)
	{
		for (int i = 0; i < CollisionData.contactCount; i++)
		{
			float num = Vector3.Angle(CollisionData.GetContact(i).normal, Vector3.up);
			if (CollisionData.GetContact(i).point.y < base.transform.position.y - (this.capsule.height / 2f - this.capsule.radius * 0.95f))
			{
				if (!this.IsGrounded)
				{
					this.IsGrounded = true;
					this.advanced.stairMiniHop = false;
				}
				if (this.advanced.maxSlopeAngle > 0f)
				{
					if (num < 5.1f)
					{
						this.advanced.isTouchingFlat = true;
						this.advanced.isTouchingWalkable = true;
					}
					else if (num < this.advanced.maxSlopeAngle + 0.1f)
					{
						this.advanced.isTouchingWalkable = true;
					}
					else if (num < 90f)
					{
						this.advanced.isTouchingUpright = true;
					}
					this.advanced.curntGroundNormal = CollisionData.GetContact(i).normal;
				}
			}
		}
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0002EBE8 File Offset: 0x0002CDE8
	private void OnCollisionExit(Collision CollisionData)
	{
		this.IsGrounded = false;
		if (this.advanced.maxSlopeAngle > 0f)
		{
			this.advanced.curntGroundNormal = Vector3.up;
			this.advanced.lastKnownSlopeAngle = 0f;
			this.advanced.isTouchingWalkable = false;
			this.advanced.isTouchingUpright = false;
		}
	}

	// Token: 0x04000844 RID: 2116
	private Player player;

	// Token: 0x04000845 RID: 2117
	public float jumpheight;

	// Token: 0x04000846 RID: 2118
	public bool controllerPauseState;

	// Token: 0x04000847 RID: 2119
	public bool enableCameraMovement = true;

	// Token: 0x04000848 RID: 2120
	public FirstPersonAIO.InvertMouseInput mouseInputInversion;

	// Token: 0x04000849 RID: 2121
	public FirstPersonAIO.CameraInputMethod cameraInputMethod;

	// Token: 0x0400084A RID: 2122
	public bool leaned;

	// Token: 0x0400084B RID: 2123
	public float verticalRotationRange = 170f;

	// Token: 0x0400084C RID: 2124
	public float mouseSensitivity = 10f;

	// Token: 0x0400084D RID: 2125
	public float fOVToMouseSensitivity = 1f;

	// Token: 0x0400084E RID: 2126
	public float cameraSmoothing = 5f;

	// Token: 0x0400084F RID: 2127
	public bool lockAndHideCursor;

	// Token: 0x04000850 RID: 2128
	public Camera playerCamera;

	// Token: 0x04000851 RID: 2129
	public bool enableCameraShake;

	// Token: 0x04000852 RID: 2130
	internal Vector3 cameraStartingPosition;

	// Token: 0x04000853 RID: 2131
	public float baseCamFOV;

	// Token: 0x04000854 RID: 2132
	public bool autoCrosshair;

	// Token: 0x04000855 RID: 2133
	public bool drawStaminaMeter = true;

	// Token: 0x04000856 RID: 2134
	private float smoothRef;

	// Token: 0x04000857 RID: 2135
	private Image StaminaMeter;

	// Token: 0x04000858 RID: 2136
	private Image StaminaMeterBG;

	// Token: 0x04000859 RID: 2137
	public Sprite Crosshair;

	// Token: 0x0400085A RID: 2138
	public Vector3 targetAngles;

	// Token: 0x0400085B RID: 2139
	public Vector3 followAngles;

	// Token: 0x0400085C RID: 2140
	private Vector3 followVelocity;

	// Token: 0x0400085D RID: 2141
	public Vector3 originalRotation;

	// Token: 0x0400085E RID: 2142
	public bool playerCanMove = true;

	// Token: 0x0400085F RID: 2143
	public bool walkByDefault = true;

	// Token: 0x04000860 RID: 2144
	public float walkSpeed = 4f;

	// Token: 0x04000861 RID: 2145
	public KeyCode sprintKey = KeyCode.LeftShift;

	// Token: 0x04000862 RID: 2146
	public float sprintSpeed = 8f;

	// Token: 0x04000863 RID: 2147
	public float jumpPower = 5f;

	// Token: 0x04000864 RID: 2148
	public bool canJump = true;

	// Token: 0x04000865 RID: 2149
	public bool canHoldJump;

	// Token: 0x04000866 RID: 2150
	private bool jumpInput;

	// Token: 0x04000867 RID: 2151
	private bool didJump;

	// Token: 0x04000868 RID: 2152
	public bool useStamina = true;

	// Token: 0x04000869 RID: 2153
	public float staminaDepletionSpeed = 5f;

	// Token: 0x0400086A RID: 2154
	public float staminaLevel = 50f;

	// Token: 0x0400086B RID: 2155
	public float speed;

	// Token: 0x0400086C RID: 2156
	public float staminaInternal;

	// Token: 0x0400086D RID: 2157
	internal float walkSpeedInternal;

	// Token: 0x0400086E RID: 2158
	internal float sprintSpeedInternal;

	// Token: 0x0400086F RID: 2159
	internal float jumpPowerInternal;

	// Token: 0x04000870 RID: 2160
	public FirstPersonAIO.CrouchModifiers _crouchModifiers = new FirstPersonAIO.CrouchModifiers();

	// Token: 0x04000871 RID: 2161
	public FirstPersonAIO.AdvancedSettings advanced = new FirstPersonAIO.AdvancedSettings();

	// Token: 0x04000872 RID: 2162
	private CapsuleCollider capsule;

	// Token: 0x04000874 RID: 2164
	private Vector2 inputXY;

	// Token: 0x04000875 RID: 2165
	public static bool isCrouching;

	// Token: 0x04000876 RID: 2166
	public static bool isCrouching2;

	// Token: 0x04000877 RID: 2167
	private float yVelocity;

	// Token: 0x04000878 RID: 2168
	private float checkedSlope;

	// Token: 0x04000879 RID: 2169
	private bool isSprinting;

	// Token: 0x0400087A RID: 2170
	public Rigidbody fps_Rigidbody;

	// Token: 0x0400087B RID: 2171
	public bool useHeadbob = true;

	// Token: 0x0400087C RID: 2172
	public Transform head;

	// Token: 0x0400087D RID: 2173
	public bool snapHeadjointToCapsul = true;

	// Token: 0x0400087E RID: 2174
	public float headbobFrequency = 1.5f;

	// Token: 0x0400087F RID: 2175
	public float headbobSwayAngle = 5f;

	// Token: 0x04000880 RID: 2176
	public float headbobHeight = 3f;

	// Token: 0x04000881 RID: 2177
	public float headbobSideMovement = 5f;

	// Token: 0x04000882 RID: 2178
	public float jumpLandIntensity = 3f;

	// Token: 0x04000883 RID: 2179
	private Vector3 originalLocalPosition;

	// Token: 0x04000884 RID: 2180
	private float nextStepTime = 0.5f;

	// Token: 0x04000885 RID: 2181
	private float headbobCycle;

	// Token: 0x04000886 RID: 2182
	private float headbobFade;

	// Token: 0x04000887 RID: 2183
	private float springPosition;

	// Token: 0x04000888 RID: 2184
	private float springVelocity;

	// Token: 0x04000889 RID: 2185
	private float springElastic = 1.1f;

	// Token: 0x0400088A RID: 2186
	private float springDampen = 0.8f;

	// Token: 0x0400088B RID: 2187
	private float springVelocityThreshold = 0.05f;

	// Token: 0x0400088C RID: 2188
	private float springPositionThreshold = 0.05f;

	// Token: 0x0400088D RID: 2189
	private Vector3 previousPosition;

	// Token: 0x0400088E RID: 2190
	private Vector3 previousVelocity = Vector3.zero;

	// Token: 0x0400088F RID: 2191
	private Vector3 miscRefVel;

	// Token: 0x04000890 RID: 2192
	private bool previousGrounded;

	// Token: 0x04000891 RID: 2193
	private AudioSource audioSource;

	// Token: 0x04000892 RID: 2194
	public bool enableAudioSFX = true;

	// Token: 0x04000893 RID: 2195
	public float Volume = 5f;

	// Token: 0x04000894 RID: 2196
	public AudioClip jumpSound;

	// Token: 0x04000895 RID: 2197
	public AudioClip landSound;

	// Token: 0x04000896 RID: 2198
	public List<AudioClip> footStepSounds;

	// Token: 0x04000897 RID: 2199
	public FirstPersonAIO.FSMode fsmode;

	// Token: 0x04000898 RID: 2200
	public FirstPersonAIO.DynamicFootStep dynamicFootstep = new FirstPersonAIO.DynamicFootStep();

	// Token: 0x0200010D RID: 269
	public enum InvertMouseInput
	{
		// Token: 0x0400089A RID: 2202
		None,
		// Token: 0x0400089B RID: 2203
		X,
		// Token: 0x0400089C RID: 2204
		Y,
		// Token: 0x0400089D RID: 2205
		Both
	}

	// Token: 0x0200010E RID: 270
	public enum CameraInputMethod
	{
		// Token: 0x0400089F RID: 2207
		Traditional,
		// Token: 0x040008A0 RID: 2208
		TraditionalWithConstraints,
		// Token: 0x040008A1 RID: 2209
		Retro
	}

	// Token: 0x0200010F RID: 271
	[Serializable]
	public class CrouchModifiers
	{
		// Token: 0x040008A2 RID: 2210
		public bool useCrouch = true;

		// Token: 0x040008A3 RID: 2211
		public bool toggleCrouch;

		// Token: 0x040008A4 RID: 2212
		public KeyCode crouchKey = KeyCode.LeftControl;

		// Token: 0x040008A5 RID: 2213
		public float crouchWalkSpeedMultiplier = 0.5f;

		// Token: 0x040008A6 RID: 2214
		public float crouchJumpPowerMultiplier;

		// Token: 0x040008A7 RID: 2215
		public bool crouchOverride;

		// Token: 0x040008A8 RID: 2216
		internal float colliderHeight;
	}

	// Token: 0x02000110 RID: 272
	[Serializable]
	public class AdvancedSettings
	{
		// Token: 0x040008A9 RID: 2217
		public float gravityMultiplier = 1f;

		// Token: 0x040008AA RID: 2218
		public PhysicMaterial zeroFrictionMaterial;

		// Token: 0x040008AB RID: 2219
		public PhysicMaterial highFrictionMaterial;

		// Token: 0x040008AC RID: 2220
		public float maxSlopeAngle = 55f;

		// Token: 0x040008AD RID: 2221
		internal bool isTouchingWalkable;

		// Token: 0x040008AE RID: 2222
		internal bool isTouchingUpright;

		// Token: 0x040008AF RID: 2223
		internal bool isTouchingFlat;

		// Token: 0x040008B0 RID: 2224
		public float maxWallShear = 89f;

		// Token: 0x040008B1 RID: 2225
		public float maxStepHeight = 0.2f;

		// Token: 0x040008B2 RID: 2226
		internal bool stairMiniHop;

		// Token: 0x040008B3 RID: 2227
		public RaycastHit surfaceAngleCheck;

		// Token: 0x040008B4 RID: 2228
		public Vector3 curntGroundNormal;

		// Token: 0x040008B5 RID: 2229
		public Vector2 moveDirRef;

		// Token: 0x040008B6 RID: 2230
		public float lastKnownSlopeAngle;

		// Token: 0x040008B7 RID: 2231
		public float FOVKickAmount = 2.5f;

		// Token: 0x040008B8 RID: 2232
		public float changeTime = 0.75f;

		// Token: 0x040008B9 RID: 2233
		public float fovRef;
	}

	// Token: 0x02000111 RID: 273
	public enum FSMode
	{
		// Token: 0x040008BB RID: 2235
		Static,
		// Token: 0x040008BC RID: 2236
		Dynamic
	}

	// Token: 0x02000112 RID: 274
	[Serializable]
	public class DynamicFootStep
	{
		// Token: 0x040008BD RID: 2237
		public FirstPersonAIO.DynamicFootStep.matMode materialMode;

		// Token: 0x040008BE RID: 2238
		public List<PhysicMaterial> woodPhysMat;

		// Token: 0x040008BF RID: 2239
		public List<PhysicMaterial> metalAndGlassPhysMat;

		// Token: 0x040008C0 RID: 2240
		public List<PhysicMaterial> grassPhysMat;

		// Token: 0x040008C1 RID: 2241
		public List<PhysicMaterial> dirtAndGravelPhysMat;

		// Token: 0x040008C2 RID: 2242
		public List<PhysicMaterial> rockAndConcretePhysMat;

		// Token: 0x040008C3 RID: 2243
		public List<PhysicMaterial> mudPhysMat;

		// Token: 0x040008C4 RID: 2244
		public List<PhysicMaterial> customPhysMat;

		// Token: 0x040008C5 RID: 2245
		public List<Material> woodMat;

		// Token: 0x040008C6 RID: 2246
		public List<Material> metalAndGlassMat;

		// Token: 0x040008C7 RID: 2247
		public List<Material> grassMat;

		// Token: 0x040008C8 RID: 2248
		public List<Material> dirtAndGravelMat;

		// Token: 0x040008C9 RID: 2249
		public List<Material> rockAndConcreteMat;

		// Token: 0x040008CA RID: 2250
		public List<Material> mudMat;

		// Token: 0x040008CB RID: 2251
		public List<Material> customMat;

		// Token: 0x040008CC RID: 2252
		public List<AudioClip> currentClipSet;

		// Token: 0x040008CD RID: 2253
		public List<AudioClip> woodClipSet;

		// Token: 0x040008CE RID: 2254
		public List<AudioClip> metalAndGlassClipSet;

		// Token: 0x040008CF RID: 2255
		public List<AudioClip> grassClipSet;

		// Token: 0x040008D0 RID: 2256
		public List<AudioClip> dirtAndGravelClipSet;

		// Token: 0x040008D1 RID: 2257
		public List<AudioClip> rockAndConcreteClipSet;

		// Token: 0x040008D2 RID: 2258
		public List<AudioClip> mudClipSet;

		// Token: 0x040008D3 RID: 2259
		public List<AudioClip> customClipSet;

		// Token: 0x02000113 RID: 275
		public enum matMode
		{
			// Token: 0x040008D5 RID: 2261
			physicMaterial,
			// Token: 0x040008D6 RID: 2262
			Material
		}
	}
}
