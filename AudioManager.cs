using System;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class AudioManager : MonoBehaviour
{
	// Token: 0x060005C1 RID: 1473 RVA: 0x0002BE08 File Offset: 0x0002A008
	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			if (this.canplay && tools.tool == 14 && !base.GetComponent<AudioSource>().isPlaying)
			{
				base.GetComponent<AudioSource>().clip = this.PaintSpray;
				base.GetComponent<AudioSource>().Play();
			}
			if (tools.tool == 16 && !base.GetComponent<AudioSource>().isPlaying)
			{
				base.GetComponent<AudioSource>().clip = this.WaterSpray;
				base.GetComponent<AudioSource>().Play();
			}
			if (this.canplay && tools.tool == 17 && !base.GetComponent<AudioSource>().isPlaying)
			{
				base.GetComponent<AudioSource>().clip = this.GasPump;
				base.GetComponent<AudioSource>().Play();
			}
			if (this.canplay && tools.tool == 25 && !base.GetComponent<AudioSource>().isPlaying)
			{
				base.GetComponent<AudioSource>().clip = this.SandBlast;
				base.GetComponent<AudioSource>().Play();
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (tools.tool == 14 && base.GetComponent<AudioSource>().isPlaying)
			{
				base.GetComponent<AudioSource>().clip = this.PaintSpray;
				base.GetComponent<AudioSource>().Stop();
			}
			if (tools.tool == 16 && base.GetComponent<AudioSource>().isPlaying)
			{
				base.GetComponent<AudioSource>().clip = this.WaterSpray;
				base.GetComponent<AudioSource>().Stop();
			}
			if (tools.tool == 17 && base.GetComponent<AudioSource>().isPlaying)
			{
				base.GetComponent<AudioSource>().clip = this.GasPump;
				base.GetComponent<AudioSource>().Stop();
			}
			if (tools.tool == 25 && base.GetComponent<AudioSource>().isPlaying)
			{
				base.GetComponent<AudioSource>().clip = this.SandBlast;
				base.GetComponent<AudioSource>().Stop();
			}
		}
	}

	// Token: 0x040007F4 RID: 2036
	public AudioClip Hammer;

	// Token: 0x040007F5 RID: 2037
	public AudioClip OpenCarDoor;

	// Token: 0x040007F6 RID: 2038
	public AudioClip CloseCarDoor;

	// Token: 0x040007F7 RID: 2039
	public AudioClip PaintSpray;

	// Token: 0x040007F8 RID: 2040
	public AudioClip AngleGrinder;

	// Token: 0x040007F9 RID: 2041
	public AudioClip Welding;

	// Token: 0x040007FA RID: 2042
	public AudioClip Ratcheting;

	// Token: 0x040007FB RID: 2043
	public AudioClip Ratcheting2;

	// Token: 0x040007FC RID: 2044
	public AudioClip BreakOff;

	// Token: 0x040007FD RID: 2045
	public AudioClip BreakOff2;

	// Token: 0x040007FE RID: 2046
	public AudioClip Fair;

	// Token: 0x040007FF RID: 2047
	public AudioClip TireBlow;

	// Token: 0x04000800 RID: 2048
	public AudioClip ButtonClick;

	// Token: 0x04000801 RID: 2049
	public AudioClip BlinkerOn;

	// Token: 0x04000802 RID: 2050
	public AudioClip BlinkerOff;

	// Token: 0x04000803 RID: 2051
	public AudioClip WaterSpray;

	// Token: 0x04000804 RID: 2052
	public AudioClip WindowManualLift;

	// Token: 0x04000805 RID: 2053
	public AudioClip WindowElectriclift;

	// Token: 0x04000806 RID: 2054
	public AudioClip GasPump;

	// Token: 0x04000807 RID: 2055
	public AudioClip TirePump;

	// Token: 0x04000808 RID: 2056
	public AudioClip Cash;

	// Token: 0x04000809 RID: 2057
	public AudioClip HandbrakeSet;

	// Token: 0x0400080A RID: 2058
	public AudioClip HandbrakeRelease;

	// Token: 0x0400080B RID: 2059
	public AudioClip GarageDoor;

	// Token: 0x0400080C RID: 2060
	public AudioClip Winching;

	// Token: 0x0400080D RID: 2061
	public AudioClip HydroPump;

	// Token: 0x0400080E RID: 2062
	public AudioClip StarterClick;

	// Token: 0x0400080F RID: 2063
	public AudioClip CarLift;

	// Token: 0x04000810 RID: 2064
	public AudioClip OpenDoor;

	// Token: 0x04000811 RID: 2065
	public AudioClip CloseDoor;

	// Token: 0x04000812 RID: 2066
	public AudioClip SlidingDoor;

	// Token: 0x04000813 RID: 2067
	public AudioClip SandBlast;

	// Token: 0x04000814 RID: 2068
	public AudioClip SledgeHammer;

	// Token: 0x04000815 RID: 2069
	public AudioClip PlaceWall;

	// Token: 0x04000816 RID: 2070
	public bool canplay;

	// Token: 0x04000817 RID: 2071
	public Transform CheckPos;

	// Token: 0x04000818 RID: 2072
	public Transform CheckPos2;
}
