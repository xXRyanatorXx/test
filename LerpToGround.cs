using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000222 RID: 546
public class LerpToGround : MonoBehaviour
{
	// Token: 0x06000CA2 RID: 3234 RVA: 0x0008CB58 File Offset: 0x0008AD58
	private void OnEnable()
	{
		Vector3 vector = LerpToGround.GetTerrainPos(base.transform.position.x, base.transform.position.z) + new Vector3(0f, -1f, 0f);
		if (vector.y > 0f)
		{
			base.StartCoroutine(this.LerpPosition(vector, this.time));
		}
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x0008CBC8 File Offset: 0x0008ADC8
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

	// Token: 0x06000CA4 RID: 3236 RVA: 0x0008CC21 File Offset: 0x0008AE21
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

	// Token: 0x04001580 RID: 5504
	private Vector3 positionToMoveTo;

	// Token: 0x04001581 RID: 5505
	public float time = 2f;
}
