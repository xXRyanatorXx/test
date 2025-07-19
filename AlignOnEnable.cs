using System;
using UnityEngine;

// Token: 0x02000221 RID: 545
public class AlignOnEnable : MonoBehaviour
{
	// Token: 0x06000C9E RID: 3230 RVA: 0x0008C9BE File Offset: 0x0008ABBE
	private void OnEnable()
	{
		this.TryToFloor();
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x0008C9C8 File Offset: 0x0008ABC8
	private void TryToFloor()
	{
		float[] array = new float[]
		{
			500f,
			500f
		};
		int num = 0;
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			transform.localPosition = new Vector3(transform.position.x, AlignOnEnable.GetTerrainPos(transform.position.x, transform.position.z).y, transform.position.z);
			array[num] = transform.position.y;
			num++;
		}
		if (array[0] != 0f || array[1] != 0f)
		{
			return;
		}
		if (this.attempts < this.MaxAttempts)
		{
			this.attempts++;
			base.Invoke("TryToFloor", 1f);
			return;
		}
		Debug.LogFormat("Failed to floor splines {0} on countout", new object[]
		{
			base.gameObject.name
		});
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x0008CAEC File Offset: 0x0008ACEC
	private static Vector3 GetTerrainPos(float x, float y)
	{
		Vector3 origin = new Vector3(x, 500f, y);
		LayerMask mask = LayerMask.GetMask(new string[]
		{
			"World"
		});
		RaycastHit raycastHit;
		Physics.Raycast(new Ray(origin, Vector3.down), out raycastHit, 501f, mask);
		return raycastHit.point;
	}

	// Token: 0x0400157E RID: 5502
	public int attempts;

	// Token: 0x0400157F RID: 5503
	private int MaxAttempts = 20;
}
