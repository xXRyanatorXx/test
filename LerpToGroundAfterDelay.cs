using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000224 RID: 548
public class LerpToGroundAfterDelay : MonoBehaviour
{
	// Token: 0x06000CAC RID: 3244 RVA: 0x0008CD23 File Offset: 0x0008AF23
	private void OnEnable()
	{
		base.Invoke("DoLerping", this.delaytime);
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x0008CD38 File Offset: 0x0008AF38
	private void DoLerping()
	{
		Vector3 vector = LerpToGroundAfterDelay.GetTerrainPos(base.transform.position.x, base.transform.position.z) + new Vector3(0f, -1f, 0f);
		if (vector.y > 0f)
		{
			base.StartCoroutine(this.LerpPosition(vector, this.lerptime));
		}
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x0008CDA8 File Offset: 0x0008AFA8
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

	// Token: 0x06000CAF RID: 3247 RVA: 0x0008CE01 File Offset: 0x0008B001
	private IEnumerator LerpPosition(Vector3 targetPosition, float duration)
	{
		float time = 0f;
		Vector3 startPosition = base.transform.position;
		while (time < duration)
		{
			base.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
			time += Time.deltaTime;
			yield return null;
		}
		base.transform.position = targetPosition;
		yield break;
	}

	// Token: 0x04001589 RID: 5513
	private Vector3 positionToMoveTo;

	// Token: 0x0400158A RID: 5514
	public float lerptime = 2f;

	// Token: 0x0400158B RID: 5515
	public float delaytime = 2f;
}
