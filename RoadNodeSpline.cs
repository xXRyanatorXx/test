using System;
using FluffyUnderware.Curvy;
using UnityEngine;

// Token: 0x02000213 RID: 531
public class RoadNodeSpline : MonoBehaviour
{
	// Token: 0x06000C6A RID: 3178 RVA: 0x0008B0D4 File Offset: 0x000892D4
	private void Start()
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.1f, base.transform.position.z);
		base.gameObject.AddComponent<SphereCollider>();
		base.GetComponent<SphereCollider>().isTrigger = true;
		base.GetComponent<SphereCollider>().radius = 5f;
		if (base.GetComponent<CurvySplineSegment>().IsFirstControlPoint || base.GetComponent<CurvySplineSegment>().IsLastControlPoint)
		{
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 0.5f, base.transform.position.z);
			this.InPlace = false;
			base.gameObject.AddComponent<Rigidbody>();
			base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
			if (base.GetComponent<CurvySplineSegment>().IsFirstControlPoint)
			{
				int siblingIndex = base.transform.GetSiblingIndex();
				if (base.transform.parent != null && base.transform.parent.childCount >= siblingIndex + 1)
				{
					this.tangent = base.transform.parent.GetChild(siblingIndex + 1).InverseTransformPoint(base.transform.position);
				}
				base.GetComponent<CurvySplineSegment>().AutoHandles = false;
				base.GetComponent<CurvySplineSegment>().HandleOut = new Vector3(this.tangent.x * -1f / 2f, 0f, this.tangent.z * -1f / 2f);
				base.GetComponent<CurvySplineSegment>().HandleIn = new Vector3(this.tangent.x / 2f, 0f, this.tangent.z / 2f);
				return;
			}
			int siblingIndex2 = base.transform.GetSiblingIndex();
			if (base.transform.parent != null && base.transform.parent.childCount >= siblingIndex2 + 1)
			{
				this.tangent = base.transform.parent.GetChild(siblingIndex2 - 1).InverseTransformPoint(base.transform.position);
			}
			base.GetComponent<CurvySplineSegment>().AutoHandles = false;
			base.GetComponent<CurvySplineSegment>().HandleOut = new Vector3(this.tangent.x / 2f, 0f, this.tangent.z / 2f);
			base.GetComponent<CurvySplineSegment>().HandleIn = new Vector3(this.tangent.x * -1f / 2f, 0f, this.tangent.z * -1f / 2f);
		}
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x0008B3B0 File Offset: 0x000895B0
	private void OnTriggerEnter(Collider other)
	{
		if (!base.GetComponent<Rigidbody>())
		{
			return;
		}
		if (other.GetComponent<CurvySplineSegment>())
		{
			if (!this.InPlace)
			{
				base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z);
			}
			this.InPlace = true;
			if (this.Connections == 0 && other.GetComponent<RoadNodeSpline>().Connections == 0)
			{
				if (other.GetComponent<CurvySplineSegment>().AutoHandles)
				{
					other.GetComponent<CurvySplineSegment>().AutoHandles = false;
					other.GetComponent<CurvySplineSegment>().HandleOut = new Vector3(other.GetComponent<CurvySplineSegment>().HandleOut.x, 0f, other.GetComponent<CurvySplineSegment>().HandleOut.z);
					other.GetComponent<CurvySplineSegment>().HandleIn = new Vector3(other.GetComponent<CurvySplineSegment>().HandleIn.x, 0f, other.GetComponent<CurvySplineSegment>().HandleIn.z);
				}
				else if (base.GetComponent<CurvySplineSegment>().IsFirstControlPoint)
				{
					if (other.GetComponent<CurvySplineSegment>().IsFirstControlPoint)
					{
						other.GetComponent<CurvySplineSegment>().HandleOut = new Vector3(base.GetComponent<CurvySplineSegment>().HandleIn.x, 0f, base.GetComponent<CurvySplineSegment>().HandleIn.z);
						other.GetComponent<CurvySplineSegment>().HandleIn = new Vector3(base.GetComponent<CurvySplineSegment>().HandleOut.x, 0f, base.GetComponent<CurvySplineSegment>().HandleOut.z);
					}
					else
					{
						other.GetComponent<CurvySplineSegment>().HandleOut = new Vector3(base.GetComponent<CurvySplineSegment>().HandleOut.x, 0f, base.GetComponent<CurvySplineSegment>().HandleOut.z);
						other.GetComponent<CurvySplineSegment>().HandleIn = new Vector3(base.GetComponent<CurvySplineSegment>().HandleIn.x, 0f, base.GetComponent<CurvySplineSegment>().HandleIn.z);
					}
				}
				else if (other.GetComponent<CurvySplineSegment>().IsFirstControlPoint)
				{
					other.GetComponent<CurvySplineSegment>().HandleOut = new Vector3(base.GetComponent<CurvySplineSegment>().HandleOut.x, 0f, base.GetComponent<CurvySplineSegment>().HandleOut.z);
					other.GetComponent<CurvySplineSegment>().HandleIn = new Vector3(base.GetComponent<CurvySplineSegment>().HandleIn.x, 0f, base.GetComponent<CurvySplineSegment>().HandleIn.z);
				}
				else
				{
					other.GetComponent<CurvySplineSegment>().HandleOut = new Vector3(base.GetComponent<CurvySplineSegment>().HandleIn.x, 0f, base.GetComponent<CurvySplineSegment>().HandleIn.z);
					other.GetComponent<CurvySplineSegment>().HandleIn = new Vector3(base.GetComponent<CurvySplineSegment>().HandleOut.x, 0f, base.GetComponent<CurvySplineSegment>().HandleOut.z);
				}
				other.GetComponent<RoadNodeSpline>().Connections = 1;
				this.Connections = 1;
			}
			else if (other.GetComponent<RoadNodeSpline>().Connections == 0)
			{
				if (other.GetComponent<CurvySplineSegment>().AutoHandles)
				{
					other.GetComponent<CurvySplineSegment>().AutoHandles = false;
					base.GetComponent<CurvySplineSegment>().HandleOut = new Vector3(base.GetComponent<CurvySplineSegment>().HandleOut.x, 0f, base.GetComponent<CurvySplineSegment>().HandleOut.z);
					base.GetComponent<CurvySplineSegment>().HandleIn = new Vector3(base.GetComponent<CurvySplineSegment>().HandleIn.x, 0f, base.GetComponent<CurvySplineSegment>().HandleIn.z);
				}
				other.GetComponent<RoadNodeSpline>().Connections = 1;
				this.Connections = 1;
			}
			if (other.transform.position.y > base.transform.position.y)
			{
				base.GetComponent<CurvySplineSegment>().SetPosition(other.transform.position);
				return;
			}
			other.GetComponent<CurvySplineSegment>().SetPosition(base.transform.position);
		}
	}

	// Token: 0x0400155D RID: 5469
	public GameObject endobject;

	// Token: 0x0400155E RID: 5470
	public Vector3 tangent;

	// Token: 0x0400155F RID: 5471
	public bool InPlace;

	// Token: 0x04001560 RID: 5472
	public int Connections;
}
