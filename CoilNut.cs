using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
public class CoilNut : MonoBehaviour
{
	// Token: 0x060000D0 RID: 208 RVA: 0x00008DF0 File Offset: 0x00006FF0
	private void Start()
	{
		this.restart();
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00008DF8 File Offset: 0x00006FF8
	public void tighten()
	{
		if (tools.Tighten)
		{
			if (tools.Tighten)
			{
				if (base.transform.parent.parent.GetComponent<MPobject>())
				{
					base.transform.parent.parent.GetComponent<MPobject>().networkDummy.tighten(base.transform.GetSiblingIndex());
					return;
				}
				this.tighten2();
			}
			return;
		}
		if (base.transform.parent.parent.GetComponent<MPobject>())
		{
			base.transform.parent.parent.GetComponent<MPobject>().networkDummy.Loosen(base.transform.GetSiblingIndex());
			return;
		}
		this.Loosen();
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00008EB0 File Offset: 0x000070B0
	public void Loosen()
	{
		if (this.height == 1f)
		{
			return;
		}
		if (tools.tool == 2)
		{
			tools.AudioParent_.GetComponent<AudioSource>().PlayOneShot(tools.AudioParent_.GetComponent<AudioManager>().Ratcheting2);
		}
		this.height -= 1f;
		this.restart();
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00008F0C File Offset: 0x0000710C
	public void tighten2()
	{
		if (this.height == 10f)
		{
			return;
		}
		if (tools.tool == 2)
		{
			tools.AudioParent_.GetComponent<AudioSource>().PlayOneShot(tools.AudioParent_.GetComponent<AudioManager>().Ratcheting2);
		}
		this.height += 1f;
		this.restart();
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00008F68 File Offset: 0x00007168
	public void restart()
	{
		this.SpringLength = this.height / 100f;
		base.transform.parent.parent.GetComponent<CarProperties>().ReStart();
		base.transform.parent.transform.localScale = new Vector3(1f, 1f, 0.65f + 0.35f * (this.height / 10f));
	}

	// Token: 0x04000151 RID: 337
	public float height;

	// Token: 0x04000152 RID: 338
	public float SpringLength;
}
