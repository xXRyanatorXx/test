using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class WaypointsPath : MonoBehaviour
{
	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000084 RID: 132 RVA: 0x000072B4 File Offset: 0x000054B4
	// (set) Token: 0x06000085 RID: 133 RVA: 0x000072BC File Offset: 0x000054BC
	public float Length { get; private set; }

	// Token: 0x06000086 RID: 134 RVA: 0x000072C5 File Offset: 0x000054C5
	private void Awake()
	{
		if (this.nodes.Count > 1)
		{
			this.CachePositionsAndDistances();
		}
		this.numPoints = this.nodes.Count;
	}

	// Token: 0x06000087 RID: 135 RVA: 0x000072EC File Offset: 0x000054EC
	public WaypointsPath.RoutePoint GetRoutePoint(float dist)
	{
		Vector3 routePosition = this.GetRoutePosition(dist);
		return new WaypointsPath.RoutePoint(routePosition, (this.GetRoutePosition(dist + 0.1f) - routePosition).normalized);
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00007324 File Offset: 0x00005524
	public Vector3 GetRoutePosition(float dist)
	{
		int num = 0;
		if (this.Length == 0f)
		{
			this.Length = this.distances[this.distances.Length - 1];
		}
		dist = Mathf.Repeat(dist, this.Length);
		while (this.distances[num] < dist)
		{
			num++;
		}
		this.p1n = (num - 1 + this.numPoints) % this.numPoints;
		this.p2n = num;
		this.i = Mathf.InverseLerp(this.distances[this.p1n], this.distances[this.p2n], dist);
		if (this.smoothRoute)
		{
			this.p0n = (num - 2 + this.numPoints) % this.numPoints;
			this.p3n = (num + 1) % this.numPoints;
			this.p2n %= this.numPoints;
			this.P0 = this.points[this.p0n];
			this.P1 = this.points[this.p1n];
			this.P2 = this.points[this.p2n];
			this.P3 = this.points[this.p3n];
			return this.CatmullRom(this.P0, this.P1, this.P2, this.P3, this.i);
		}
		this.p1n = (num - 1 + this.numPoints) % this.numPoints;
		this.p2n = num;
		return Vector3.Lerp(this.points[this.p1n], this.points[this.p2n], this.i);
	}

	// Token: 0x06000089 RID: 137 RVA: 0x000074CC File Offset: 0x000056CC
	private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
	{
		return 0.5f * (2f * p1 + (-p0 + p2) * i + (2f * p0 - 5f * p1 + 4f * p2 - p3) * i * i + (-p0 + 3f * p1 - 3f * p2 + p3) * i * i * i);
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00007594 File Offset: 0x00005794
	private void CachePositionsAndDistances()
	{
		this.points = new Vector3[this.nodes.Count + 1];
		this.distances = new float[this.nodes.Count + 1];
		float num = 0f;
		for (int i = 0; i < this.points.Length; i++)
		{
			Transform transform = this.nodes[i % this.nodes.Count];
			Transform transform2 = this.nodes[(i + 1) % this.nodes.Count];
			if (transform != null && transform2 != null)
			{
				Vector3 position = transform.position;
				Vector3 position2 = transform2.position;
				this.points[i] = this.nodes[i % this.nodes.Count].position;
				this.distances[i] = num;
				num += (position - position2).magnitude;
			}
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x0000768D File Offset: 0x0000588D
	private void OnDrawGizmos()
	{
		this.DrawGizmos(false);
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00007696 File Offset: 0x00005896
	private void OnDrawGizmosSelected()
	{
		this.DrawGizmos(true);
	}

	// Token: 0x0600008D RID: 141 RVA: 0x000076A0 File Offset: 0x000058A0
	private void DrawGizmos(bool selected)
	{
		Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>();
		this.nodes = new List<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != base.transform)
			{
				this.nodes.Add(componentsInChildren[i]);
			}
		}
		if (this.nodes.Count > 1)
		{
			this.numPoints = this.nodes.Count;
			this.CachePositionsAndDistances();
			this.Length = this.distances[this.distances.Length - 1];
			Gizmos.color = (selected ? Color.yellow : new Color(1f, 1f, 0f, 0.5f));
			Vector3 from = this.nodes[0].position;
			if (this.smoothRoute)
			{
				for (float num = 0f; num < this.Length; num += this.Length / this.editorVisualisationSubsteps)
				{
					Vector3 routePosition = this.GetRoutePosition(num + 1f);
					Gizmos.DrawLine(from, routePosition);
					from = routePosition;
				}
				Gizmos.DrawLine(from, this.nodes[0].position);
				return;
			}
			for (int j = 0; j < this.nodes.Count; j++)
			{
				Vector3 position = this.nodes[(j + 1) % this.nodes.Count].position;
				Gizmos.DrawLine(from, position);
				from = position;
			}
		}
	}

	// Token: 0x04000105 RID: 261
	[HideInInspector]
	public List<Transform> nodes = new List<Transform>();

	// Token: 0x04000106 RID: 262
	private int numPoints;

	// Token: 0x04000107 RID: 263
	private Vector3[] points;

	// Token: 0x04000108 RID: 264
	private float[] distances;

	// Token: 0x0400010A RID: 266
	[SerializeField]
	private bool smoothRoute = true;

	// Token: 0x0400010B RID: 267
	[Range(40f, 200f)]
	public float editorVisualisationSubsteps = 100f;

	// Token: 0x0400010C RID: 268
	private int p0n;

	// Token: 0x0400010D RID: 269
	private int p1n;

	// Token: 0x0400010E RID: 270
	private int p2n;

	// Token: 0x0400010F RID: 271
	private int p3n;

	// Token: 0x04000110 RID: 272
	private float i;

	// Token: 0x04000111 RID: 273
	private Vector3 P0;

	// Token: 0x04000112 RID: 274
	private Vector3 P1;

	// Token: 0x04000113 RID: 275
	private Vector3 P2;

	// Token: 0x04000114 RID: 276
	private Vector3 P3;

	// Token: 0x02000020 RID: 32
	public struct RoutePoint
	{
		// Token: 0x0600008F RID: 143 RVA: 0x0000782B File Offset: 0x00005A2B
		public RoutePoint(Vector3 position, Vector3 direction)
		{
			this.position = position;
			this.direction = direction;
		}

		// Token: 0x04000115 RID: 277
		public Vector3 position;

		// Token: 0x04000116 RID: 278
		public Vector3 direction;
	}
}
