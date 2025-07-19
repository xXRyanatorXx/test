using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

// Token: 0x02000288 RID: 648
public class UIQualityManager : MonoBehaviour
{
	// Token: 0x06000F36 RID: 3894 RVA: 0x0009E790 File Offset: 0x0009C990
	private void Start()
	{
		List<string> list = new List<string>();
		list.AddRange(QualitySettings.names);
		this.dpQuality.AddOptions(list);
		this.dpQuality.value = this.dpQuality.options.Count - 1;
		List<string> list2 = new List<string>();
		foreach (PostProcessProfile postProcessProfile in this.volumes)
		{
			list2.Add(postProcessProfile.name);
		}
		this.dpPostProcessing.AddOptions(list2);
		List<string> list3 = new List<string>();
		foreach (NameValue nameValue in this.detailDensities)
		{
			list3.Add(nameValue.name);
		}
		this.dpDetailDensity.AddOptions(list3);
		List<string> list4 = new List<string>();
		foreach (NameValue nameValue2 in this.shapeQualities)
		{
			list4.Add(nameValue2.name);
		}
		this.dpShapeQuality.AddOptions(list4);
	}

	// Token: 0x06000F37 RID: 3895 RVA: 0x0009E8D4 File Offset: 0x0009CAD4
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			this.canvas.enabled = !this.canvas.enabled;
		}
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x0009E8F8 File Offset: 0x0009CAF8
	public void SetPostProcessing(int id)
	{
		this.postProcessVolume.profile = this.volumes[id];
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x0009E90D File Offset: 0x0009CB0D
	public void SetQuality(int id)
	{
		QualitySettings.SetQualityLevel(id);
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x0009E915 File Offset: 0x0009CB15
	public void SetDetailDensity(int id)
	{
		this.terrain.detailObjectDensity = this.detailDensities[id].value;
		this.terrain.Flush();
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x0009E93E File Offset: 0x0009CB3E
	public void SetShapeQuality(int id)
	{
		this.terrain.heightmapPixelError = this.shapeQualities[id].value;
		this.terrain.Flush();
	}

	// Token: 0x04001860 RID: 6240
	public Canvas canvas;

	// Token: 0x04001861 RID: 6241
	public PostProcessVolume postProcessVolume;

	// Token: 0x04001862 RID: 6242
	public PostProcessProfile[] volumes;

	// Token: 0x04001863 RID: 6243
	public Dropdown dpPostProcessing;

	// Token: 0x04001864 RID: 6244
	public Dropdown dpQuality;

	// Token: 0x04001865 RID: 6245
	public Terrain terrain;

	// Token: 0x04001866 RID: 6246
	public List<NameValue> detailDensities;

	// Token: 0x04001867 RID: 6247
	public Dropdown dpDetailDensity;

	// Token: 0x04001868 RID: 6248
	public List<NameValue> shapeQualities;

	// Token: 0x04001869 RID: 6249
	public Dropdown dpShapeQuality;
}
