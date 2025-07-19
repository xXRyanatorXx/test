using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class CarAIWaipointTracker : MonoBehaviour
{
	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000055 RID: 85 RVA: 0x000060A7 File Offset: 0x000042A7
	// (set) Token: 0x06000056 RID: 86 RVA: 0x000060AF File Offset: 0x000042AF
	[HideInInspector]
	public WaypointsPath.RoutePoint targetPoint { get; private set; }

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000057 RID: 87 RVA: 0x000060B8 File Offset: 0x000042B8
	// (set) Token: 0x06000058 RID: 88 RVA: 0x000060C0 File Offset: 0x000042C0
	[HideInInspector]
	public WaypointsPath.RoutePoint speedPoint { get; private set; }

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000059 RID: 89 RVA: 0x000060C9 File Offset: 0x000042C9
	// (set) Token: 0x0600005A RID: 90 RVA: 0x000060D1 File Offset: 0x000042D1
	[HideInInspector]
	public WaypointsPath.RoutePoint progressPoint { get; private set; }

	// Token: 0x0600005B RID: 91 RVA: 0x000060DC File Offset: 0x000042DC
	private void Start()
	{
		this.ACAI = base.GetComponent<AnyCarAI>();
		this.circuit = this.ACAI.AIcircuit;
		this.lookAheadForTargetOffset = this.ACAI.lookAheadForTarget;
		this.lookAheadForTargetFactor = this.ACAI.lookAheadForTargetFactor;
		this.lookAheadForSpeedOffset = this.ACAI.lookAheadForSpeedOffset;
		this.lookAheadForSpeedFactor = this.ACAI.lookAheadForSpeedFactor;
		this.pointToPointThreshold = this.ACAI.pointThreshold;
		this.progressStyle = (int)this.ACAI.progressStyle;
		if (this.target == null)
		{
			this.target = this.ACAI.carAItarget;
		}
		this.Reset();
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00006194 File Offset: 0x00004394
	public void Reset()
	{
		this.progressDistance = 0f;
		this.progressNum = 0;
		if (this.progressStyle == 1)
		{
			this.target.position = this.circuit.nodes[this.progressNum].position;
			this.target.rotation = this.circuit.nodes[this.progressNum].rotation;
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00006208 File Offset: 0x00004408
	private void Update()
	{
		if (!base.transform.GetComponent<AnyCarAI>().persuitAiOn)
		{
			base.transform.GetComponent<CarAIInputs>().persuitAiOn = false;
			this.FollowPath();
			return;
		}
		if (this.ACAI.persuitTarget != null)
		{
			Transform transform = this.ACAI.persuitTarget.GetComponentInChildren<MeshCollider>().transform;
			this.target = transform;
			return;
		}
		this.FollowPath();
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00006278 File Offset: 0x00004478
	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(base.transform.position, this.target.position);
			Gizmos.DrawWireSphere(this.circuit.GetRoutePosition(this.progressDistance), 1f);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(this.target.position, this.target.position + this.target.forward);
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00006304 File Offset: 0x00004504
	public void FollowPath()
	{
		if (this.ACAI.progressStyle == ProgressStyle.SmoothAlongRoute)
		{
			if (Time.deltaTime > 0f)
			{
				this.speed = Mathf.Lerp(this.speed, (this.lastPosition - base.transform.position).magnitude / Time.deltaTime, Time.deltaTime);
			}
			this.target.position = this.circuit.GetRoutePoint(this.progressDistance + this.lookAheadForTargetOffset + this.lookAheadForTargetFactor * this.speed).position;
			this.target.rotation = Quaternion.LookRotation(this.circuit.GetRoutePoint(this.progressDistance + this.lookAheadForSpeedOffset + this.lookAheadForSpeedFactor * this.speed).direction);
			this.progressPoint = this.circuit.GetRoutePoint(this.progressDistance);
			Vector3 lhs = this.progressPoint.position - base.transform.position;
			if (Vector3.Dot(lhs, this.progressPoint.direction) < 0f)
			{
				this.progressDistance += lhs.magnitude * 0.5f;
			}
			this.lastPosition = base.transform.position;
			return;
		}
		if ((this.target.position - base.transform.position).magnitude < this.pointToPointThreshold)
		{
			this.progressNum = (this.progressNum + 1) % this.circuit.nodes.Count;
		}
		this.target.position = this.circuit.nodes[this.progressNum].position;
		this.target.rotation = this.circuit.nodes[this.progressNum].rotation;
		this.progressPoint = this.circuit.GetRoutePoint(this.progressDistance);
		Vector3 lhs2 = this.progressPoint.position - base.transform.position;
		if (Vector3.Dot(lhs2, this.progressPoint.direction) < 0f)
		{
			this.progressDistance += lhs2.magnitude;
		}
		this.lastPosition = base.transform.position;
	}

	// Token: 0x040000BF RID: 191
	[HideInInspector]
	public Transform target;

	// Token: 0x040000C0 RID: 192
	private WaypointsPath circuit;

	// Token: 0x040000C1 RID: 193
	private AnyCarAI ACAI;

	// Token: 0x040000C2 RID: 194
	private float lookAheadForTargetOffset = 5f;

	// Token: 0x040000C3 RID: 195
	private float lookAheadForTargetFactor = 0.1f;

	// Token: 0x040000C4 RID: 196
	private float lookAheadForSpeedOffset = 10f;

	// Token: 0x040000C5 RID: 197
	private float lookAheadForSpeedFactor = 0.2f;

	// Token: 0x040000C6 RID: 198
	private int progressStyle;

	// Token: 0x040000C7 RID: 199
	private float pointToPointThreshold = 4f;

	// Token: 0x040000C8 RID: 200
	private float progressDistance;

	// Token: 0x040000C9 RID: 201
	private int progressNum;

	// Token: 0x040000CA RID: 202
	private Vector3 lastPosition;

	// Token: 0x040000CB RID: 203
	private float speed;

	// Token: 0x040000CC RID: 204
	[HideInInspector]
	public Transform[] pathTransform;
}
