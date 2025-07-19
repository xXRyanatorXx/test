using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200013E RID: 318
public class GlobalAudio : MonoBehaviour
{
	// Token: 0x060006C7 RID: 1735 RVA: 0x0003635C File Offset: 0x0003455C
	private void Awake()
	{
		if (GlobalAudio.instance != null && GlobalAudio.instance != this)
		{
			UnityEngine.Object.Destroy(this);
		}
		GlobalAudio.instance = this;
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x00036384 File Offset: 0x00034584
	public static AudioSource PlayAudio(string _category, string _type, Action _finishAction = null, float _volume = 1f, float _pitch = 1f, float _delay = 0f, Transform _parent = null)
	{
		using (List<GlobalAudio.AudioCategory>.Enumerator enumerator = GlobalAudio.instance.categories.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				GlobalAudio.AudioCategory audioCategory = enumerator.Current;
				if (audioCategory.name == _category)
				{
					using (List<GlobalAudio.AudioInfo>.Enumerator enumerator2 = audioCategory.audios.GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							GlobalAudio.AudioInfo audioInfo = enumerator2.Current;
							if (audioInfo.name == _type)
							{
								GameObject gameObject = new GameObject(_type);
								AudioSource audioSource = gameObject.AddComponent<AudioSource>();
								audioSource.clip = audioInfo.clip;
								audioSource.volume = _volume;
								audioSource.pitch = _pitch;
								audioSource.spatialBlend = 0f;
								if (GlobalAudio.instance.output != null)
								{
									audioSource.outputAudioMixerGroup = GlobalAudio.instance.output;
								}
								if (_parent != null)
								{
									gameObject.transform.SetParent(_parent, false);
									audioSource.spatialBlend = 1f;
								}
								else
								{
									gameObject.transform.SetParent(GlobalAudio.instance.transform, false);
								}
								GlobalAudio.AudioPlayer audioPlayer = gameObject.AddComponent<GlobalAudio.AudioPlayer>();
								audioPlayer.category = _category;
								audioPlayer.type = _type;
								audioPlayer.source = audioSource;
								GlobalAudio.AudioPlayer audioPlayer2 = audioPlayer;
								audioPlayer2.onFinish = (Action)Delegate.Combine(audioPlayer2.onFinish, _finishAction);
								audioInfo.players.Add(audioPlayer);
								audioSource.PlayDelayed(_delay);
								audioPlayer.StartChecking();
								return audioSource;
							}
							return null;
						}
					}
					return null;
				}
				return null;
			}
		}
		return null;
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x00036564 File Offset: 0x00034764
	public static void StopAllFromCategory(string _category)
	{
		foreach (GlobalAudio.AudioCategory audioCategory in GlobalAudio.instance.categories)
		{
			if (audioCategory.name == _category)
			{
				foreach (GlobalAudio.AudioInfo audioInfo in audioCategory.audios)
				{
					foreach (GlobalAudio.AudioPlayer audioPlayer in audioInfo.players)
					{
						audioPlayer.enabled = true;
					}
				}
			}
		}
	}

	// Token: 0x04000A43 RID: 2627
	internal static GlobalAudio instance;

	// Token: 0x04000A44 RID: 2628
	public AudioMixerGroup output;

	// Token: 0x04000A45 RID: 2629
	public List<GlobalAudio.AudioCategory> categories = new List<GlobalAudio.AudioCategory>();

	// Token: 0x0200013F RID: 319
	[Serializable]
	public class AudioCategory
	{
		// Token: 0x04000A46 RID: 2630
		public string name;

		// Token: 0x04000A47 RID: 2631
		public List<GlobalAudio.AudioInfo> audios = new List<GlobalAudio.AudioInfo>();
	}

	// Token: 0x02000140 RID: 320
	[Serializable]
	public class AudioInfo
	{
		// Token: 0x04000A48 RID: 2632
		public string name;

		// Token: 0x04000A49 RID: 2633
		public AudioClip clip;

		// Token: 0x04000A4A RID: 2634
		[HideInInspector]
		internal List<GlobalAudio.AudioPlayer> players = new List<GlobalAudio.AudioPlayer>();
	}

	// Token: 0x02000141 RID: 321
	internal class AudioPlayer : MonoBehaviour
	{
		// Token: 0x060006CD RID: 1741 RVA: 0x00036675 File Offset: 0x00034875
		public void StartChecking()
		{
			this.audioCheck = base.StartCoroutine(this.AudioCheck());
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0003668C File Offset: 0x0003488C
		private void OnDisable()
		{
			if (this.audioCheck != null)
			{
				base.StopCoroutine(this.audioCheck);
			}
			GlobalAudio.instance.categories.First((GlobalAudio.AudioCategory x) => x.name == this.category).audios.First((GlobalAudio.AudioInfo x) => x.name == this.type).players.Remove(this);
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x000366F0 File Offset: 0x000348F0
		private IEnumerator AudioCheck()
		{
			while (this.source.isPlaying)
			{
				yield return null;
			}
			Action action = this.onFinish;
			if (action != null)
			{
				action();
			}
			GlobalAudio.instance.categories.First((GlobalAudio.AudioCategory x) => x.name == this.category).audios.First((GlobalAudio.AudioInfo x) => x.name == this.type).players.Remove(this);
			UnityEngine.Object.Destroy(base.gameObject);
			yield return null;
			yield break;
		}

		// Token: 0x04000A4B RID: 2635
		public string category;

		// Token: 0x04000A4C RID: 2636
		public string type;

		// Token: 0x04000A4D RID: 2637
		public AudioSource source;

		// Token: 0x04000A4E RID: 2638
		public Action onFinish;

		// Token: 0x04000A4F RID: 2639
		private Coroutine audioCheck;
	}
}
