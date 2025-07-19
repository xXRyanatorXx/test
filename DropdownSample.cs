using System;
using TMPro;
using UnityEngine;

// Token: 0x02000278 RID: 632
public class DropdownSample : MonoBehaviour
{
	// Token: 0x06000EFA RID: 3834 RVA: 0x0009DA10 File Offset: 0x0009BC10
	public void OnButtonClick()
	{
		this.text.text = ((this.dropdownWithPlaceholder.value > -1) ? ("Selected values:\n" + this.dropdownWithoutPlaceholder.value.ToString() + " - " + this.dropdownWithPlaceholder.value.ToString()) : "Error: Please make a selection");
	}

	// Token: 0x0400182E RID: 6190
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x0400182F RID: 6191
	[SerializeField]
	private TMP_Dropdown dropdownWithoutPlaceholder;

	// Token: 0x04001830 RID: 6192
	[SerializeField]
	private TMP_Dropdown dropdownWithPlaceholder;
}
