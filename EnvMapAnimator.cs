using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000279 RID: 633
public class EnvMapAnimator : MonoBehaviour
{
	// Token: 0x06000EFC RID: 3836 RVA: 0x0009DA72 File Offset: 0x0009BC72
	private void Awake()
	{
		this.m_textMeshPro = base.GetComponent<TMP_Text>();
		this.m_material = this.m_textMeshPro.fontSharedMaterial;
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x0009DA91 File Offset: 0x0009BC91
	private IEnumerator Start()
	{
		Matrix4x4 matrix = default(Matrix4x4);
		for (;;)
		{
			matrix.SetTRS(Vector3.zero, Quaternion.Euler(Time.time * this.RotationSpeeds.x, Time.time * this.RotationSpeeds.y, Time.time * this.RotationSpeeds.z), Vector3.one);
			this.m_material.SetMatrix("_EnvMatrix", matrix);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001831 RID: 6193
	public Vector3 RotationSpeeds;

	// Token: 0x04001832 RID: 6194
	private TMP_Text m_textMeshPro;

	// Token: 0x04001833 RID: 6195
	private Material m_material;
}
