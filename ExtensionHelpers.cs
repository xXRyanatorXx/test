using System;
using UnityEngine;

// Token: 0x0200024A RID: 586
internal static class ExtensionHelpers
{
	// Token: 0x06000DDC RID: 3548 RVA: 0x0009456C File Offset: 0x0009276C
	public static GameObject GetFirstParentWithComponent<T>(this GameObject gameObject)
	{
		GameObject gameObject2 = null;
		GameObject gameObject3 = gameObject.transform.parent.gameObject;
		while (gameObject2 == null && gameObject3 != null)
		{
			if (gameObject3.GetComponent<T>() != null)
			{
				gameObject2 = gameObject3;
			}
			else
			{
				gameObject3 = gameObject3.transform.parent.gameObject;
			}
		}
		return gameObject2;
	}
}
