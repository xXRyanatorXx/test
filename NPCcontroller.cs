using System;
using CrazyMinnow.SALSA;
using UnityEngine;

// Token: 0x02000202 RID: 514
public class NPCcontroller : MonoBehaviour
{
	// Token: 0x06000BF4 RID: 3060 RVA: 0x00084398 File Offset: 0x00082598
	private void Start()
	{
		this.Day = EnviroSkyMgr.instance.Time.Days;
		if (this.Playerlook == null)
		{
			this.Playerlook = GameObject.Find("Player Camera").transform;
		}
		if (this.LocalLook != null)
		{
			this.Eyes.lookTarget = this.LocalLook;
		}
		if (this.Randomaccecories.Length != 0)
		{
			this.RandomaccecoriesActive = this.Randomaccecories[UnityEngine.Random.Range(0, this.Randomaccecories.Length)];
			this.RandomaccecoriesActive.SetActive(true);
		}
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x0008442C File Offset: 0x0008262C
	private void OnEnable()
	{
		this.salsa.audioSrc.clip = null;
		if (this.LocalLook != null)
		{
			this.Eyes.lookTarget = this.LocalLook;
		}
		else
		{
			this.Eyes.lookTarget = null;
		}
		if (EnviroSkyMgr.instance.Seasons.currentSeasons == EnviroSeasons.Seasons.Winter)
		{
			this.WinterCothes.SetActive(true);
		}
		if (this.Randomaccecories[0] != null && this.Day != EnviroSkyMgr.instance.Time.Days)
		{
			if (this.RandomaccecoriesActive != null)
			{
				this.RandomaccecoriesActive.SetActive(false);
			}
			this.RandomaccecoriesActive = this.Randomaccecories[UnityEngine.Random.Range(0, this.Randomaccecories.Length)];
			this.RandomaccecoriesActive.SetActive(true);
		}
		this.Day = EnviroSkyMgr.instance.Time.Days;
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x00084514 File Offset: 0x00082714
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Player" && other.GetComponent<tools>() && Vector3.Distance(other.GetComponent<tools>().hand.transform.position, base.transform.position) < 10f)
		{
			if (this.LookAtPlayerAlways && this.Playerlook != null)
			{
				this.Eyes.lookTarget = this.Playerlook;
			}
			if (this.LastConversationtime != EnviroSkyMgr.instance.Time.Hours || this.TalkAllTime)
			{
				if (this.Clips.Length != 0)
				{
					this.salsa.audioSrc.clip = this.Clips[UnityEngine.Random.Range(0, this.Clips.Length)];
					this.salsa.audioSrc.Play();
				}
				if (this.Playerlook != null)
				{
					this.Eyes.lookTarget = this.Playerlook;
				}
				this.LastConversationtime = EnviroSkyMgr.instance.Time.Hours;
			}
		}
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x00084634 File Offset: 0x00082834
	private void OnTriggerExit(Collider other)
	{
		if (other.transform.tag == "Player")
		{
			if (this.LocalLook != null)
			{
				this.Eyes.lookTarget = this.LocalLook;
				return;
			}
			this.Eyes.lookTarget = null;
		}
	}

	// Token: 0x040014A0 RID: 5280
	public bool TalkAllTime;

	// Token: 0x040014A1 RID: 5281
	public Salsa salsa;

	// Token: 0x040014A2 RID: 5282
	public Eyes Eyes;

	// Token: 0x040014A3 RID: 5283
	public bool LookAtPlayerAlways;

	// Token: 0x040014A4 RID: 5284
	public string Name;

	// Token: 0x040014A5 RID: 5285
	public AudioClip[] FirstClips;

	// Token: 0x040014A6 RID: 5286
	public AudioClip[] Clips;

	// Token: 0x040014A7 RID: 5287
	public AudioClip[] RainClips;

	// Token: 0x040014A8 RID: 5288
	public Transform LocalLook;

	// Token: 0x040014A9 RID: 5289
	public Transform Playerlook;

	// Token: 0x040014AA RID: 5290
	public GameObject WinterCothes;

	// Token: 0x040014AB RID: 5291
	public GameObject SummerClothe;

	// Token: 0x040014AC RID: 5292
	public GameObject[] Randomaccecories;

	// Token: 0x040014AD RID: 5293
	private GameObject RandomaccecoriesActive;

	// Token: 0x040014AE RID: 5294
	private int LastConversationtime;

	// Token: 0x040014AF RID: 5295
	public int Day;
}
