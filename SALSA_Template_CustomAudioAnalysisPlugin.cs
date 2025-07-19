using System;
using CrazyMinnow.SALSA;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class SALSA_Template_CustomAudioAnalysisPlugin : MonoBehaviour
{
	// Token: 0x060000D8 RID: 216 RVA: 0x000091A5 File Offset: 0x000073A5
	private void Start()
	{
		base.GetComponent<Salsa>().audioAnalyzer = new Salsa.AudioAnalyzer(this.CalcSimpleMaxPeakValue);
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x000091C0 File Offset: 0x000073C0
	private float CalcSimpleMaxPeakValue(int channels, float[] audioData)
	{
		float num = 0f;
		for (int i = 0; i < audioData.Length; i += channels)
		{
			if (audioData[i] > num)
			{
				num = audioData[i];
			}
		}
		return num;
	}
}
