using System;
using UnityEngine;

// Token: 0x0200027D RID: 637
public class TimedActions : MonoBehaviour
{
	// Token: 0x06000F0C RID: 3852 RVA: 0x0009DCBC File Offset: 0x0009BEBC
	private void Start()
	{
		this.tools = GameObject.Find("Player").GetComponent<tools>();
		this.tools.TimedActions.Add(this);
		this.OpenObject.SetActive(true);
		if (this.CloseObject)
		{
			this.CloseObject.SetActive(false);
		}
		this.Open = true;
		this.HourPassed();
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x0009DD24 File Offset: 0x0009BF24
	public void HourPassed()
	{
		if (EnviroSkyMgr.instance.Time.Hours >= this.OpenTime && EnviroSkyMgr.instance.Time.Hours < this.CloseTime)
		{
			if (!this.OpenObject.activeSelf)
			{
				this.OpenObject.SetActive(true);
				if (this.CloseObject)
				{
					this.CloseObject.SetActive(false);
				}
				this.Open = true;
				if (this.Mapmarker)
				{
					Transform transform = this.Positions[UnityEngine.Random.Range(0, this.Positions.Length)];
					base.transform.position = transform.position;
					base.transform.rotation = transform.rotation;
					this.Mapmarker.SetActive(true);
					return;
				}
			}
		}
		else
		{
			if (this.CloseObject && !this.CloseObject.activeSelf)
			{
				this.OpenObject.SetActive(false);
				this.CloseObject.SetActive(true);
				if (this.OutThroower)
				{
					this.OutThroower.SetActive(true);
				}
				this.Open = false;
				if (this.Mapmarker)
				{
					this.Mapmarker.SetActive(false);
				}
			}
			if (!this.CloseObject)
			{
				this.OpenObject.SetActive(false);
				if (this.OutThroower)
				{
					this.OutThroower.SetActive(true);
				}
				this.Open = false;
				if (this.Mapmarker)
				{
					this.Mapmarker.SetActive(false);
				}
			}
		}
	}

	// Token: 0x0400183D RID: 6205
	public tools tools;

	// Token: 0x0400183E RID: 6206
	public int OpenTime;

	// Token: 0x0400183F RID: 6207
	public GameObject OpenObject;

	// Token: 0x04001840 RID: 6208
	public int CloseTime;

	// Token: 0x04001841 RID: 6209
	public GameObject CloseObject;

	// Token: 0x04001842 RID: 6210
	public GameObject OutThroower;

	// Token: 0x04001843 RID: 6211
	public bool Open;

	// Token: 0x04001844 RID: 6212
	public GameObject Mapmarker;

	// Token: 0x04001845 RID: 6213
	public Transform[] Positions;
}
