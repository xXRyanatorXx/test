using System;
using TurnTheGameOn.SimpleTrafficSystem;
using UnityEngine;

// Token: 0x02000285 RID: 645
public class YieldTriggerGizmo : MonoBehaviour
{
	// Token: 0x06000F29 RID: 3881 RVA: 0x0009E127 File Offset: 0x0009C327
	private void OnDrawGizmos()
	{
		this.DrawGizmos(false);
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x0009E130 File Offset: 0x0009C330
	private void OnDrawGizmosSelected()
	{
		this.DrawGizmos(true);
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x0009E13C File Offset: 0x0009C33C
	private void DrawGizmos(bool selected)
	{
		if (this.m_collider == null)
		{
			this.m_collider = base.GetComponent<BoxCollider>();
		}
		if (this.m_collider != null)
		{
			if (selected)
			{
				Gizmos.color = STSPrefs.selectedYieldTriggerColor;
			}
			else
			{
				Gizmos.color = STSPrefs.yieldTriggerColor;
			}
			this.DrawCube(base.transform.position, base.transform.rotation, base.transform.localScale, this.m_collider.center, this.m_collider.size);
		}
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x0009E1C8 File Offset: 0x0009C3C8
	private void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale, Vector3 center, Vector3 size)
	{
		Matrix4x4 rhs = Matrix4x4.TRS(position, rotation, scale);
		Matrix4x4 matrix = Gizmos.matrix;
		Gizmos.matrix *= rhs;
		Gizmos.DrawCube(center, size);
		Gizmos.matrix = matrix;
	}

	// Token: 0x04001853 RID: 6227
	private BoxCollider m_collider;
}
