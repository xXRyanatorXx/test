using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020000B2 RID: 178
[AddComponentMenu("Enviro/Utility/Audio Mixer Support")]
public class EnviroAudioMixerSupport : MonoBehaviour
{
	// Token: 0x06000385 RID: 901 RVA: 0x000180F9 File Offset: 0x000162F9
	private void Start()
	{
		if (this.audioMixer != null && EnviroSkyMgr.instance != null)
		{
			base.StartCoroutine(this.Setup());
		}
	}

	// Token: 0x06000386 RID: 902 RVA: 0x00018123 File Offset: 0x00016323
	private IEnumerator Setup()
	{
		yield return 0;
		if (EnviroSkyMgr.instance.IsStarted())
		{
			if (this.ambientMixerGroup != "")
			{
				EnviroSkyMgr.instance.AudioSettings.AudioSourceAmbient.audiosrc.outputAudioMixerGroup = this.audioMixer.FindMatchingGroups(this.ambientMixerGroup)[0];
				EnviroSkyMgr.instance.AudioSettings.AudioSourceAmbient2.audiosrc.outputAudioMixerGroup = this.audioMixer.FindMatchingGroups(this.ambientMixerGroup)[0];
			}
			if (this.weatherMixerGroup != "")
			{
				EnviroSkyMgr.instance.AudioSettings.AudioSourceWeather.audiosrc.outputAudioMixerGroup = this.audioMixer.FindMatchingGroups(this.weatherMixerGroup)[0];
				EnviroSkyMgr.instance.AudioSettings.AudioSourceWeather2.audiosrc.outputAudioMixerGroup = this.audioMixer.FindMatchingGroups(this.weatherMixerGroup)[0];
			}
			if (this.thunderMixerGroup != "")
			{
				EnviroSkyMgr.instance.AudioSettings.AudioSourceThunder.audiosrc.outputAudioMixerGroup = this.audioMixer.FindMatchingGroups(this.thunderMixerGroup)[0];
			}
		}
		else
		{
			base.StartCoroutine(this.Setup());
		}
		yield break;
	}

	// Token: 0x04000518 RID: 1304
	[Header("Mixer")]
	public AudioMixer audioMixer;

	// Token: 0x04000519 RID: 1305
	[Header("Group Names")]
	public string ambientMixerGroup;

	// Token: 0x0400051A RID: 1306
	public string weatherMixerGroup;

	// Token: 0x0400051B RID: 1307
	public string thunderMixerGroup;
}
