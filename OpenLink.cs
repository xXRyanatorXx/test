using System;
using UnityEngine;

// Token: 0x0200018B RID: 395
public class OpenLink : MonoBehaviour
{
	// Token: 0x060008F2 RID: 2290 RVA: 0x000554EE File Offset: 0x000536EE
	public void Open()
	{
		Application.OpenURL(this.path);
	}

	// Token: 0x040010CB RID: 4299
	public string path;
}
