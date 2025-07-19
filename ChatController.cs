using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000277 RID: 631
public class ChatController : MonoBehaviour
{
	// Token: 0x06000EF6 RID: 3830 RVA: 0x0009D8FD File Offset: 0x0009BAFD
	private void OnEnable()
	{
		this.TMP_ChatInput.onSubmit.AddListener(new UnityAction<string>(this.AddToChatOutput));
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x0009D91B File Offset: 0x0009BB1B
	private void OnDisable()
	{
		this.TMP_ChatInput.onSubmit.RemoveListener(new UnityAction<string>(this.AddToChatOutput));
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x0009D93C File Offset: 0x0009BB3C
	private void AddToChatOutput(string newText)
	{
		this.TMP_ChatInput.text = string.Empty;
		DateTime now = DateTime.Now;
		TMP_Text tmp_ChatOutput = this.TMP_ChatOutput;
		tmp_ChatOutput.text = string.Concat(new string[]
		{
			tmp_ChatOutput.text,
			"[<#FFFF80>",
			now.Hour.ToString("d2"),
			":",
			now.Minute.ToString("d2"),
			":",
			now.Second.ToString("d2"),
			"</color>] ",
			newText,
			"\n"
		});
		this.TMP_ChatInput.ActivateInputField();
		this.ChatScrollbar.value = 0f;
	}

	// Token: 0x0400182B RID: 6187
	public TMP_InputField TMP_ChatInput;

	// Token: 0x0400182C RID: 6188
	public TMP_Text TMP_ChatOutput;

	// Token: 0x0400182D RID: 6189
	public Scrollbar ChatScrollbar;
}
