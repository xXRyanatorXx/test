using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class CarAIInputs : MonoBehaviour
{
	// Token: 0x0600004C RID: 76 RVA: 0x00005268 File Offset: 0x00003468
	private void Awake()
	{
		this.carAIReference = base.GetComponent<AnyCarAI>();
		this.frontSensor = base.transform.GetChild(1).GetChild(3).GetChild(0);
		this.rightSensor = base.transform.GetChild(1).GetChild(3).GetChild(1);
		this.leftSensor = base.transform.GetChild(1).GetChild(3).GetChild(2);
		this.persuitAiOn = this.carAIReference.persuitAiOn;
		this.randomValue = UnityEngine.Random.value * 100f;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00005300 File Offset: 0x00003500
	private void FixedUpdate()
	{
		if (!(this.carAIReference.carAItarget == null) && this.carAIReference.isDriving)
		{
			if (this.carAIReference.persuitAiOn && this.carAIReference.persuitTarget != null)
			{
				this.PersuitSensors();
			}
			else if (!this.carAIReference.persuitAiOn)
			{
				this.persuitAiOn = false;
			}
			this.ReverseGearSensors();
			if (!this.reverseGearOn)
			{
				this.AvoidSensors();
				this.BrakeSensors();
			}
			Vector3 to = base.transform.forward;
			if (this.carAIReference.rb.velocity.magnitude > this.carAIReference.maxSpeed * 0.1f)
			{
				to = this.carAIReference.rb.velocity;
			}
			float num = this.carAIReference.maxSpeed;
			switch (this.carAIReference.brakeCondition)
			{
			case BrakeCondition.TargetDirectionDifference:
			{
				float b = Vector3.Angle(this.carAIReference.carAItarget.forward, to);
				float a = this.carAIReference.rb.angularVelocity.magnitude * this.carAIReference.cautiousAngularVelocityFactor;
				float t = Mathf.InverseLerp(0f, this.carAIReference.cautiousAngle, Mathf.Max(a, b));
				num = Mathf.Lerp(this.carAIReference.maxSpeed, this.carAIReference.maxSpeed * this.carAIReference.cautiousSpeedFactor, t);
				break;
			}
			case BrakeCondition.TargetDistance:
			{
				Vector3 vector = this.carAIReference.carAItarget.position - base.transform.position;
				float b2 = Mathf.InverseLerp(this.carAIReference.cautiousDistance, 0f, vector.magnitude);
				float value = this.carAIReference.rb.angularVelocity.magnitude * this.carAIReference.cautiousAngularVelocityFactor;
				float t2 = Mathf.Max(Mathf.InverseLerp(0f, this.carAIReference.cautiousAngle, value), b2);
				num = Mathf.Lerp(this.carAIReference.maxSpeed, this.carAIReference.maxSpeed * this.carAIReference.cautiousSpeedFactor, t2);
				break;
			}
			}
			Vector3 vector2 = this.carAIReference.carAItarget.position;
			if (Time.time < this.avoidOtherCarTime)
			{
				num *= this.avoidOtherCarSlowdown;
				vector2 += this.carAIReference.carAItarget.right * this.avoidPathOffset;
			}
			else
			{
				vector2 += this.carAIReference.carAItarget.right * (Mathf.PerlinNoise(Time.time * this.carAIReference.lateralWanderSpeed, this.randomValue) * 2f - 1f) * this.carAIReference.lateralWander;
			}
			float num2 = (num < this.carAIReference.currentSpeed) ? this.carAIReference.brakeSensitivity : this.carAIReference.accelSensitivity;
			float num3 = Mathf.Clamp((num - this.carAIReference.currentSpeed) * num2, -1f, 1f);
			num3 *= this.carAIReference.wanderAmount + Mathf.PerlinNoise(Time.time * this.carAIReference.accelWanderSpeed, this.randomValue) * this.carAIReference.wanderAmount;
			Vector3 vector3 = base.transform.InverseTransformPoint(vector2);
			if (this.avoidingObstacle)
			{
				this.targetAngle = this.carAIReference.maximumSteerAngle * this.avoidObstacleMultiplier;
			}
			else if (this.carAIReference.persuitAiOn)
			{
				if (this.carAIReference.persuitTarget != null)
				{
					Transform transform = this.carAIReference.persuitTarget.GetComponentInChildren<MeshCollider>().transform;
					Vector3 vector4 = base.transform.InverseTransformPoint(transform.position);
					this.targetAngle = vector4.x / vector4.magnitude * this.carAIReference.maximumSteerAngle;
				}
			}
			else
			{
				this.targetAngle = Mathf.Atan2(vector3.x, vector3.z) * 57.29578f;
			}
			float num4 = Mathf.Clamp(this.targetAngle * this.carAIReference.steerSensitivity, -1f, 1f) * Mathf.Sign(this.carAIReference.currentSpeed);
			if (this.isBraking)
			{
				this.carAIReference.Move(num4, 0f, -1f, 0f);
			}
			else if (this.reverseGearOn)
			{
				this.carAIReference.Move(-num4, -1f, -1f, 0f);
			}
			else
			{
				this.carAIReference.Move(num4, num3, num3, 0f);
			}
			if (this.carAIReference.stopWhenTargetReached && vector3.magnitude < this.carAIReference.reachTargetThreshold)
			{
				this.carAIReference.isDriving = false;
			}
			return;
		}
		if (this.carAIReference.persuitTarget != null && this.carAIReference.persuitAiOn)
		{
			this.PersuitSensors();
			return;
		}
		this.carAIReference.Move(0f, 0f, -1f, 1f);
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00005828 File Offset: 0x00003A28
	private void OnCollisionStay(Collision col)
	{
		if (col.rigidbody != null)
		{
			CarAIInputs component = col.rigidbody.GetComponent<CarAIInputs>();
			if (component != null)
			{
				this.avoidOtherCarTime = Time.time + 1f;
				if (Vector3.Angle(base.transform.forward, component.transform.position - base.transform.position) < 90f)
				{
					this.avoidOtherCarSlowdown = 0.5f;
				}
				else
				{
					this.avoidOtherCarSlowdown = 1f;
				}
				Vector3 vector = base.transform.InverseTransformPoint(component.transform.position);
				float f = Mathf.Atan2(vector.x, vector.z);
				this.avoidPathOffset = this.carAIReference.lateralWander * -Mathf.Sign(f);
			}
		}
	}

	// Token: 0x0600004F RID: 79 RVA: 0x000058FC File Offset: 0x00003AFC
	private void AvoidSensors()
	{
		this.avoidingObstacle = false;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.rightSensor.position, this.frontSensor.transform.forward, out raycastHit, this.carAIReference.avoidDistance))
		{
			if (!raycastHit.collider.GetComponent<CompetitiveDrivingCheck>() && !raycastHit.collider.CompareTag("Terrain"))
			{
				Debug.DrawLine(this.rightSensor.position, raycastHit.point, Color.yellow);
				this.avoidingObstacle = true;
				this.avoidObstacleMultiplier -= 1f;
			}
		}
		else if (Physics.Raycast(this.rightSensor.position, Quaternion.AngleAxis(this.carAIReference.sensorsAngle, this.rightSensor.up) * this.rightSensor.forward, out raycastHit, this.carAIReference.avoidDistance) && !raycastHit.collider.GetComponent<CompetitiveDrivingCheck>() && !raycastHit.collider.CompareTag("Terrain"))
		{
			Debug.DrawLine(this.rightSensor.position, raycastHit.point, Color.yellow);
			this.avoidingObstacle = true;
			this.avoidObstacleMultiplier -= 0.5f;
		}
		if (Physics.Raycast(this.leftSensor.position, this.frontSensor.forward, out raycastHit, this.carAIReference.avoidDistance))
		{
			if (!raycastHit.collider.GetComponent<CompetitiveDrivingCheck>() && !raycastHit.collider.CompareTag("Terrain"))
			{
				Debug.DrawLine(this.leftSensor.position, raycastHit.point, Color.yellow);
				this.avoidingObstacle = true;
				this.avoidObstacleMultiplier += 1f;
			}
		}
		else if (Physics.Raycast(this.leftSensor.position, Quaternion.AngleAxis(-this.carAIReference.sensorsAngle, this.leftSensor.up) * this.leftSensor.forward, out raycastHit, this.carAIReference.avoidDistance) && !raycastHit.collider.GetComponent<CompetitiveDrivingCheck>() && !raycastHit.collider.CompareTag("Terrain"))
		{
			Debug.DrawLine(this.leftSensor.position, raycastHit.point, Color.yellow);
			this.avoidingObstacle = true;
			this.avoidObstacleMultiplier += 0.5f;
		}
		if (this.avoidObstacleMultiplier == 0f && Physics.Raycast(this.frontSensor.position, this.frontSensor.forward, out raycastHit, this.carAIReference.avoidDistance) && !raycastHit.collider.GetComponent<CompetitiveDrivingCheck>() && !raycastHit.collider.CompareTag("Terrain"))
		{
			Debug.DrawLine(this.frontSensor.position, raycastHit.point, Color.yellow);
			this.avoidingObstacle = true;
			if (raycastHit.normal.x < 0f)
			{
				this.avoidObstacleMultiplier = -1f;
				return;
			}
			this.avoidObstacleMultiplier = 1f;
		}
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00005C30 File Offset: 0x00003E30
	private void BrakeSensors()
	{
		this.isBraking = false;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.rightSensor.position, this.frontSensor.forward, out raycastHit, this.carAIReference.brakeDistance) && !raycastHit.collider.GetComponent<CompetitiveDrivingCheck>() && !raycastHit.collider.CompareTag("Terrain"))
		{
			Debug.DrawLine(this.rightSensor.position, raycastHit.point, Color.magenta);
			this.isBraking = true;
		}
		if (Physics.Raycast(this.leftSensor.position, this.frontSensor.forward, out raycastHit, this.carAIReference.brakeDistance) && !raycastHit.collider.GetComponent<CompetitiveDrivingCheck>() && !raycastHit.collider.CompareTag("Terrain"))
		{
			Debug.DrawLine(this.leftSensor.position, raycastHit.point, Color.magenta);
			this.isBraking = true;
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00005D2C File Offset: 0x00003F2C
	private void ReverseGearSensors()
	{
		this.reverseGearOn = false;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.rightSensor.position, this.frontSensor.forward, out raycastHit, this.carAIReference.reverseDistance) && !raycastHit.collider.GetComponent<CompetitiveDrivingCheck>() && !raycastHit.collider.CompareTag("Terrain"))
		{
			Debug.DrawLine(this.rightSensor.position, raycastHit.point, Color.blue);
			this.reverseGearOn = true;
		}
		if (Physics.Raycast(this.leftSensor.position, this.frontSensor.forward, out raycastHit, this.carAIReference.reverseDistance) && !raycastHit.collider.GetComponent<CompetitiveDrivingCheck>() && !raycastHit.collider.CompareTag("Terrain"))
		{
			Debug.DrawLine(this.leftSensor.position, raycastHit.point, Color.blue);
			this.reverseGearOn = true;
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00005E28 File Offset: 0x00004028
	private void PersuitSensors()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(this.rightSensor.position, this.frontSensor.forward, out raycastHit, this.carAIReference.persuitDistance) && raycastHit.collider == this.carAIReference.persuitTarget.GetComponentInChildren<MeshCollider>())
		{
			Debug.DrawLine(this.rightSensor.position, raycastHit.point, Color.white);
			this.persuitAiOn = true;
		}
		if (Physics.Raycast(this.rightSensor.position, Quaternion.AngleAxis(this.carAIReference.sensorsAngle, this.rightSensor.up) * this.rightSensor.forward, out raycastHit, this.carAIReference.persuitDistance) && raycastHit.collider == this.carAIReference.persuitTarget.GetComponentInChildren<MeshCollider>())
		{
			Debug.DrawLine(this.rightSensor.position, raycastHit.point, Color.white);
			this.persuitAiOn = true;
		}
		if (Physics.Raycast(this.leftSensor.position, this.frontSensor.forward, out raycastHit, this.carAIReference.persuitDistance) && raycastHit.collider == this.carAIReference.persuitTarget.GetComponentInChildren<MeshCollider>())
		{
			Debug.DrawLine(this.leftSensor.position, raycastHit.point, Color.white);
			this.persuitAiOn = true;
		}
		if (Physics.Raycast(this.leftSensor.position, Quaternion.AngleAxis(-this.carAIReference.sensorsAngle, this.leftSensor.up) * this.leftSensor.forward, out raycastHit, this.carAIReference.persuitDistance) && raycastHit.collider == this.carAIReference.persuitTarget.GetComponentInChildren<MeshCollider>())
		{
			Debug.DrawLine(this.leftSensor.position, raycastHit.point, Color.white);
			this.persuitAiOn = true;
		}
		if (Physics.Raycast(this.frontSensor.position, this.frontSensor.forward, out raycastHit, this.carAIReference.persuitDistance) && raycastHit.collider == this.carAIReference.persuitTarget.GetComponentInChildren<MeshCollider>())
		{
			Debug.DrawLine(this.leftSensor.position, raycastHit.point, Color.white);
			this.persuitAiOn = true;
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x0000608D File Offset: 0x0000428D
	public void SetTarget(Transform target)
	{
		this.carAIReference.carAItarget = target;
		this.carAIReference.isDriving = true;
	}

	// Token: 0x040000B1 RID: 177
	private AnyCarAI carAIReference;

	// Token: 0x040000B2 RID: 178
	private Transform frontSensor;

	// Token: 0x040000B3 RID: 179
	private Transform rightSensor;

	// Token: 0x040000B4 RID: 180
	private Transform leftSensor;

	// Token: 0x040000B5 RID: 181
	private float randomValue;

	// Token: 0x040000B6 RID: 182
	private float avoidOtherCarTime;

	// Token: 0x040000B7 RID: 183
	private float avoidOtherCarSlowdown;

	// Token: 0x040000B8 RID: 184
	private float avoidPathOffset;

	// Token: 0x040000B9 RID: 185
	[HideInInspector]
	public bool reverseGearOn;

	// Token: 0x040000BA RID: 186
	[HideInInspector]
	public bool persuitAiOn;

	// Token: 0x040000BB RID: 187
	private bool avoidingObstacle;

	// Token: 0x040000BC RID: 188
	private bool isBraking;

	// Token: 0x040000BD RID: 189
	private float avoidObstacleMultiplier;

	// Token: 0x040000BE RID: 190
	private float targetAngle;
}
