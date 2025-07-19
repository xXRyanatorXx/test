using System;
using UnityEngine;

// Token: 0x02000249 RID: 585
public class RandomText : MonoBehaviour
{
	// Token: 0x06000DDA RID: 3546 RVA: 0x00094524 File Offset: 0x00092724
	private void OnEnable()
	{
		GameObject[] texts = this.Texts;
		for (int i = 0; i < texts.Length; i++)
		{
			texts[i].SetActive(false);
		}
		this.Texts[UnityEngine.Random.Range(0, this.Texts.Length)].SetActive(true);
	}

	// Token: 0x04001684 RID: 5764
	public GameObject[] Texts;
}
