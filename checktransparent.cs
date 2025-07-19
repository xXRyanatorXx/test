using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020001F4 RID: 500
public class checktransparent : MonoBehaviour
{
	// Token: 0x06000BB8 RID: 3000 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x00082536 File Offset: 0x00080736
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			Debug.Log("a");
		}
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x0008254C File Offset: 0x0008074C
	[CompilerGenerated]
	internal static bool <Update>g__IsTransparent|2_0(Texture2D tex)
	{
		for (int i = 0; i < tex.width; i++)
		{
			for (int j = 0; j < tex.height; j++)
			{
				if (tex.GetPixel(i, j).a != 0f)
				{
					return false;
				}
			}
		}
		Debug.Log("3");
		return true;
	}

	// Token: 0x04001467 RID: 5223
	public bool IsTransparent;
}
