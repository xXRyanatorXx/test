using System;
using UnityEngine;

// Token: 0x02000227 RID: 551
[ExecuteAlways]
public class MaterialChanger : MonoBehaviour
{
	// Token: 0x06000CB9 RID: 3257 RVA: 0x0008CFF8 File Offset: 0x0008B1F8
	private void OnEnable()
	{
		this.FindAllMaterialInChild();
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x0008D000 File Offset: 0x0008B200
	private void Update()
	{
		this._propBlock = new MaterialPropertyBlock();
		this.SetNewValueForAllMaterial(this._value);
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x0008D019 File Offset: 0x0008B219
	private void FindAllMaterialInChild()
	{
		this._renderers = base.transform.GetComponentsInChildren<Renderer>();
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x0008D02C File Offset: 0x0008B22C
	private void SetNewValueForAllMaterial(float value)
	{
		this.FindAllMaterialInChild();
		for (int i = 0; i < this._renderers.Length; i++)
		{
			this._renderers[i].GetPropertyBlock(this._propBlock);
			this._propBlock.SetFloat(this._changeMaterialSetting, value);
			this._renderers[i].SetPropertyBlock(this._propBlock);
		}
	}

	// Token: 0x04001595 RID: 5525
	[SerializeField]
	[Range(0f, 5f)]
	private float _value = 1f;

	// Token: 0x04001596 RID: 5526
	[SerializeField]
	private string _changeMaterialSetting = "_Worn_Level";

	// Token: 0x04001597 RID: 5527
	private Renderer[] _renderers;

	// Token: 0x04001598 RID: 5528
	private MaterialPropertyBlock _propBlock;
}
