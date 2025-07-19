using System;
using TMPro;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class AdjustTimeScale : MonoBehaviour
{
	// Token: 0x0600013E RID: 318 RVA: 0x0000AE57 File Offset: 0x00009057
	private void Start()
	{
		this.textMesh = base.GetComponent<TextMeshProUGUI>();
	}

	// Token: 0x0600013F RID: 319 RVA: 0x0000AE68 File Offset: 0x00009068
	private void Update()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			if (Time.timeScale < 1f)
			{
				Time.timeScale += 0.1f;
			}
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
			if (this.textMesh != null)
			{
				this.textMesh.text = "Time Scale : " + Math.Round((double)Time.timeScale, 2).ToString();
				return;
			}
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			if (Time.timeScale >= 0.2f)
			{
				Time.timeScale -= 0.1f;
			}
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
			if (this.textMesh != null)
			{
				this.textMesh.text = "Time Scale : " + Math.Round((double)Time.timeScale, 2).ToString();
			}
		}
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0000AF61 File Offset: 0x00009161
	private void OnApplicationQuit()
	{
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f;
	}

	// Token: 0x040001B1 RID: 433
	private TextMeshProUGUI textMesh;
}
