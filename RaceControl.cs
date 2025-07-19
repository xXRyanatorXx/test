using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000246 RID: 582
public class RaceControl : MonoBehaviour
{
	// Token: 0x06000DC5 RID: 3525 RVA: 0x000934DC File Offset: 0x000916DC
	private void Start()
	{
		if (!this.Checkpoint)
		{
			if (this.Drift)
			{
				this.currentLapTime = this.MaxTime;
				if (PlayerPrefs.HasKey(this.RaceName))
				{
					this.bestLapTime = PlayerPrefs.GetFloat(this.RaceName);
				}
				else
				{
					this.bestLapTime = 0f;
				}
				if (!this.Player)
				{
					this.Player = GameObject.Find("Player");
				}
				this.HighScoreTable.text = this.bestLapTime.ToString("F0");
				this.DriftPoints = 0f;
			}
			else
			{
				this.currentLapTime = 9999f;
				if (PlayerPrefs.HasKey(this.RaceName))
				{
					this.bestLapTime = PlayerPrefs.GetFloat(this.RaceName);
				}
				else
				{
					this.bestLapTime = 9999f;
				}
				if (!this.Player)
				{
					this.Player = GameObject.Find("Player");
				}
				if (this.bestLapTime < 9998f)
				{
					TimeSpan timeSpan = TimeSpan.FromSeconds((double)this.bestLapTime);
					this.HighScoreTable.text = timeSpan.ToString("mm':'ss':'ff");
				}
				else
				{
					this.HighScoreTable.text = "------";
				}
			}
		}
		else
		{
			this.MainRaceControl = base.transform.parent.GetComponent<RaceControl>();
		}
		this._started = false;
		base.enabled = false;
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x00093638 File Offset: 0x00091838
	private void OnEnable()
	{
		if (!this.Checkpoint)
		{
			if (this.Drift)
			{
				this.currentLapTime = this.MaxTime;
				if (PlayerPrefs.HasKey(this.RaceName))
				{
					this.bestLapTime = PlayerPrefs.GetFloat(this.RaceName);
				}
				else
				{
					this.bestLapTime = 0f;
				}
				if (!this.Player)
				{
					this.Player = GameObject.Find("Player");
				}
				TimeSpan timeSpan = TimeSpan.FromSeconds((double)this.currentLapTime);
				this.currentLapTimeText.text = timeSpan.ToString("mm':'ss':'ff");
				if (this.TimeToBeat < 9998f)
				{
					this.TimeToBeatText.text = this.TimeToBeat.ToString();
				}
				this.DriftPoints = 0f;
				this.bestLapTimeText.text = this.bestLapTime.ToString("F0");
			}
			else
			{
				this.currentLapTime = 9999f;
				if (PlayerPrefs.HasKey(this.RaceName))
				{
					this.bestLapTime = PlayerPrefs.GetFloat(this.RaceName);
				}
				else
				{
					this.bestLapTime = 9999f;
				}
				if (!this.Player)
				{
					this.Player = GameObject.Find("Player");
				}
				this.currentLapTime = 0f;
				TimeSpan timeSpan2 = TimeSpan.FromSeconds((double)this.currentLapTime);
				this.currentLapTimeText.text = timeSpan2.ToString("mm':'ss':'ff");
				if (this.TimeToBeat < 9998f)
				{
					TimeSpan timeSpan3 = TimeSpan.FromSeconds((double)this.TimeToBeat);
					this.TimeToBeatText.text = timeSpan3.ToString("mm':'ss':'ff");
				}
				if (this.bestLapTime < 9998f)
				{
					TimeSpan timeSpan4 = TimeSpan.FromSeconds((double)this.bestLapTime);
					this.bestLapTimeText.text = timeSpan4.ToString("mm':'ss':'ff");
				}
			}
		}
		else
		{
			this.MainRaceControl = base.transform.parent.GetComponent<RaceControl>();
		}
		this._started = false;
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x00093824 File Offset: 0x00091A24
	public void AcceptedRace()
	{
		base.enabled = true;
		this.RaceCanvas.SetActive(true);
		this.RacePrefabs.SetActive(true);
		this.GoToStartText.SetActive(true);
		this.StartTrigger.SetActive(true);
		this.currentLapTime = 0f;
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x00093874 File Offset: 0x00091A74
	public void InStartPosition()
	{
		if (this.Checkpoint1)
		{
			this.Checkpoint1.SetActive(true);
		}
		else
		{
			this.FinishTrigger.SetActive(true);
		}
		this.GoToStartText.SetActive(false);
		base.StartCoroutine(this.StartingRace());
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x000938C1 File Offset: 0x00091AC1
	public void StartRace()
	{
		this.currentLapTime = this.MaxTime;
		this._started = true;
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x000938D8 File Offset: 0x00091AD8
	private void Finished()
	{
		if (this.Drift)
		{
			if (this.DriftPoints > this.TimeToBeat && this.currentLapTime > 0f)
			{
				this.bestLapTime = this.DriftPoints;
				PlayerPrefs.SetFloat(this.RaceName, this.bestLapTime);
				this.HighScoreTable.text = this.bestLapTime.ToString("F0");
			}
			this.PressToClosetText.SetActive(true);
			if (this.DriftPoints > this.TimeToBeat && this.currentLapTime > 0f)
			{
				this.CloseText.text = "Race finished \n Your Points " + this.bestLapTime.ToString("F0") + " \nYou won!\nCollect your reward\nPress ENTER to close";
				this.RECORD();
			}
			else
			{
				this.CloseText.text = "Race finished \nPress ENTER to close";
			}
			if (this.bestLapTime > 0f)
			{
				this.bestLapTimeText.text = this.bestLapTime.ToString("F0");
			}
		}
		else
		{
			if (this.currentLapTime < this.bestLapTime || this.bestLapTime == 9999f)
			{
				this.bestLapTime = this.currentLapTime;
				PlayerPrefs.SetFloat(this.RaceName, this.bestLapTime);
				TimeSpan timeSpan = TimeSpan.FromSeconds((double)this.bestLapTime);
				this.HighScoreTable.text = timeSpan.ToString("mm':'ss':'ff");
			}
			TimeSpan timeSpan2 = TimeSpan.FromSeconds((double)this.currentLapTime);
			this.PressToClosetText.SetActive(true);
			if (this.TimeToBeat > this.currentLapTime)
			{
				this.CloseText.text = "Race finished \n Your time was " + timeSpan2.ToString("mm':'ss':'ff") + " \nYou won!\nCollect your reward\nPress ENTER to close";
				this.RECORD();
			}
			else
			{
				this.CloseText.text = "Race finished \n Your time was " + timeSpan2.ToString("mm':'ss':'ff") + " \nPress ENTER to close";
			}
			TimeSpan timeSpan3 = TimeSpan.FromSeconds((double)this.TimeToBeat);
			this.TimeToBeatText.text = timeSpan3.ToString("mm':'ss':'ff");
			if (this.bestLapTime < 9998f)
			{
				TimeSpan timeSpan4 = TimeSpan.FromSeconds((double)this.bestLapTime);
				this.bestLapTimeText.text = timeSpan4.ToString("mm':'ss':'ff");
			}
		}
		this.currentLapTime = 0f;
		tools.Racing = false;
		base.enabled = false;
		this._started = false;
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x00093B28 File Offset: 0x00091D28
	private void RECORD()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.Reward[UnityEngine.Random.Range(0, this.Reward.Length)], this.RewardPosition.position, this.RewardPosition.rotation).transform.name = "Trophy";
		if (this.rewardMoney == 0f)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.MoneyPrefab, this.MoneyPosition.position, this.MoneyPosition.rotation);
		gameObject.transform.name = "Money";
		gameObject.GetComponent<PickupTool>().paintlife = this.rewardMoney;
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x00093BC3 File Offset: 0x00091DC3
	public void PressedEnter()
	{
		if (this.PressToClosetText.active)
		{
			this.PressToClosetText.SetActive(false);
			this.RaceCanvas.SetActive(false);
			base.enabled = false;
			this._started = false;
		}
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x00093BF8 File Offset: 0x00091DF8
	public void Abandon()
	{
		this.CountdownText.SetActive(false);
		this.GoToStartText.SetActive(false);
		this.PressToClosetText.SetActive(false);
		this.RaceCanvas.SetActive(false);
		base.enabled = false;
		this._started = false;
		this.StartTrigger.SetActive(false);
		this.FinishTrigger.SetActive(false);
		this.currentLapTime = 0f;
		tools.Racing = false;
		if (this.Checkpoint1 && this.Checkpoint1.active)
		{
			this.Checkpoint1.SetActive(false);
		}
		if (this.Checkpoint2 && this.Checkpoint2.active)
		{
			this.Checkpoint2.SetActive(false);
		}
		if (this.Checkpoint3 && this.Checkpoint3.active)
		{
			this.Checkpoint3.SetActive(false);
		}
		if (this.Checkpoint4 && this.Checkpoint4.active)
		{
			this.Checkpoint4.SetActive(false);
		}
		if (this.Checkpoint5 && this.Checkpoint5.active)
		{
			this.Checkpoint5.SetActive(false);
		}
		if (this.Checkpoint6 && this.Checkpoint6.active)
		{
			this.Checkpoint6.SetActive(false);
		}
		if (this.Checkpoint7 && this.Checkpoint7.active)
		{
			this.Checkpoint7.SetActive(false);
		}
		if (this.Checkpoint8 && this.Checkpoint8.active)
		{
			this.Checkpoint8.SetActive(false);
		}
		if (this.Checkpoint9 && this.Checkpoint9.active)
		{
			this.Checkpoint9.SetActive(false);
		}
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x00093DC2 File Offset: 0x00091FC2
	private IEnumerator StartingRace()
	{
		this.CountdownText.SetActive(true);
		this.Countdown.text = "Get Ready!";
		yield return new WaitForSeconds(10f);
		this.Countdown.text = "3";
		yield return new WaitForSeconds(1f);
		this.Countdown.text = "2";
		yield return new WaitForSeconds(1f);
		this.Countdown.text = "1";
		yield return new WaitForSeconds(1f);
		this.Countdown.text = "GO!";
		if (base.enabled)
		{
			this.StartRace();
		}
		yield return new WaitForSeconds(2f);
		this.CountdownText.SetActive(false);
		yield break;
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x00093DD4 File Offset: 0x00091FD4
	public void CheckpointPass()
	{
		if (this.Checkpoint1 && this.Checkpoint1.active)
		{
			this.Checkpoint1.SetActive(false);
			if (this.Checkpoint2)
			{
				this.Checkpoint2.SetActive(true);
				return;
			}
			this.FinishTrigger.SetActive(true);
			return;
		}
		else if (this.Checkpoint2 && this.Checkpoint2.active)
		{
			this.Checkpoint2.SetActive(false);
			if (this.Checkpoint3)
			{
				this.Checkpoint3.SetActive(true);
				return;
			}
			this.FinishTrigger.SetActive(true);
			return;
		}
		else if (this.Checkpoint3 && this.Checkpoint3.active)
		{
			this.Checkpoint3.SetActive(false);
			if (this.Checkpoint4)
			{
				this.Checkpoint4.SetActive(true);
				return;
			}
			this.FinishTrigger.SetActive(true);
			return;
		}
		else if (this.Checkpoint4 && this.Checkpoint4.active)
		{
			this.Checkpoint4.SetActive(false);
			if (this.Checkpoint5)
			{
				this.Checkpoint5.SetActive(true);
				return;
			}
			this.FinishTrigger.SetActive(true);
			return;
		}
		else if (this.Checkpoint5 && this.Checkpoint5.active)
		{
			this.Checkpoint5.SetActive(false);
			if (this.Checkpoint6)
			{
				this.Checkpoint6.SetActive(true);
				return;
			}
			this.FinishTrigger.SetActive(true);
			return;
		}
		else if (this.Checkpoint6 && this.Checkpoint6.active)
		{
			this.Checkpoint6.SetActive(false);
			if (this.Checkpoint7)
			{
				this.Checkpoint7.SetActive(true);
				return;
			}
			this.FinishTrigger.SetActive(true);
			return;
		}
		else if (this.Checkpoint7 && this.Checkpoint7.active)
		{
			this.Checkpoint7.SetActive(false);
			if (this.Checkpoint8)
			{
				this.Checkpoint8.SetActive(true);
				return;
			}
			this.FinishTrigger.SetActive(true);
			return;
		}
		else if (this.Checkpoint8 && this.Checkpoint8.active)
		{
			this.Checkpoint8.SetActive(false);
			if (this.Checkpoint9)
			{
				this.Checkpoint9.SetActive(true);
				return;
			}
			this.FinishTrigger.SetActive(true);
			return;
		}
		else
		{
			if (this.Checkpoint9 && this.Checkpoint9.active)
			{
				this.Checkpoint9.SetActive(false);
				this.FinishTrigger.SetActive(true);
				return;
			}
			if (this.FinishTrigger.active)
			{
				this.FinishTrigger.SetActive(false);
				this.Finished();
			}
			return;
		}
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x0009409C File Offset: 0x0009229C
	private void Update()
	{
		if (!this.Checkpoint && this._started && !this.Drift)
		{
			this.currentLapTime += Time.deltaTime;
			if (this.currentLapTime < 9998f)
			{
				TimeSpan timeSpan = TimeSpan.FromSeconds((double)this.currentLapTime);
				this.currentLapTimeText.text = timeSpan.ToString("mm':'ss':'ff");
			}
			if (this.TimeToBeat < 9998f)
			{
				TimeSpan timeSpan2 = TimeSpan.FromSeconds((double)this.TimeToBeat);
				this.TimeToBeatText.text = timeSpan2.ToString("mm':'ss':'ff");
			}
			if (this.bestLapTime < 9998f)
			{
				TimeSpan timeSpan3 = TimeSpan.FromSeconds((double)this.bestLapTime);
				this.bestLapTimeText.text = timeSpan3.ToString("mm':'ss':'ff");
			}
		}
		if (!this.Checkpoint && this._started && this.Drift)
		{
			this.currentLapTime -= Time.deltaTime;
			if (this.currentLapTime < 9998f)
			{
				TimeSpan timeSpan4 = TimeSpan.FromSeconds((double)this.currentLapTime);
				this.currentLapTimeText.text = timeSpan4.ToString("mm':'ss':'ff");
			}
			if (this.currentLapTime <= 0f)
			{
				this.currentLapTimeText.text = "0";
			}
			if (this.bestLapTime > 0f)
			{
				this.bestLapTimeText.text = this.DriftPoints.ToString("F0");
			}
			if (this.Player.transform.root.GetComponent<MainCarProperties>() && this.Player.transform.root.GetComponent<MainCarProperties>()._driftAngle > 10f && this.Player.transform.root.GetComponent<MainCarProperties>().DriftModifier > 0.2f)
			{
				this.DriftPoints += 10f * this.Player.transform.root.GetComponent<MainCarProperties>()._driftAngle * Time.deltaTime;
			}
			this.DriftScoreText.text = this.DriftPoints.ToString("F0");
		}
		if (Input.GetKey(KeyCode.P))
		{
			this.Abandon();
		}
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x000942D4 File Offset: 0x000924D4
	private void OnTriggerEnter(Collider other)
	{
		if (this.Checkpoint && !this.MainRaceControl._started && this.MainRaceControl.StartTrigger.active && (other.GetComponent<tools>() || (other.GetComponent<MainCarProperties>() && other.GetComponent<MainCarProperties>().SittingInCar)))
		{
			this.MainRaceControl.InStartPosition();
			this.MainRaceControl.StartTrigger.SetActive(false);
		}
		if (this.Checkpoint && this.MainRaceControl._started && (other.GetComponent<tools>() || (other.GetComponent<MainCarProperties>() && other.GetComponent<MainCarProperties>().SittingInCar)))
		{
			this.MainRaceControl.CheckpointPass();
		}
	}

	// Token: 0x04001658 RID: 5720
	public string RaceName;

	// Token: 0x04001659 RID: 5721
	public bool Drift;

	// Token: 0x0400165A RID: 5722
	public float DriftPoints;

	// Token: 0x0400165B RID: 5723
	public GameObject[] Reward;

	// Token: 0x0400165C RID: 5724
	public Transform RewardPosition;

	// Token: 0x0400165D RID: 5725
	public GameObject MoneyPrefab;

	// Token: 0x0400165E RID: 5726
	public Transform MoneyPosition;

	// Token: 0x0400165F RID: 5727
	public float rewardMoney;

	// Token: 0x04001660 RID: 5728
	public GameObject Player;

	// Token: 0x04001661 RID: 5729
	public GameObject RacePrefabs;

	// Token: 0x04001662 RID: 5730
	public GameObject RaceCanvas;

	// Token: 0x04001663 RID: 5731
	public GameObject GoToStartText;

	// Token: 0x04001664 RID: 5732
	public GameObject CountdownText;

	// Token: 0x04001665 RID: 5733
	public TMP_Text Countdown;

	// Token: 0x04001666 RID: 5734
	public GameObject PressToClosetText;

	// Token: 0x04001667 RID: 5735
	public TMP_Text CloseText;

	// Token: 0x04001668 RID: 5736
	public string Description;

	// Token: 0x04001669 RID: 5737
	public float bestLapTime;

	// Token: 0x0400166A RID: 5738
	public Text bestLapTimeText;

	// Token: 0x0400166B RID: 5739
	public float MaxTime;

	// Token: 0x0400166C RID: 5740
	public float currentLapTime = 9999f;

	// Token: 0x0400166D RID: 5741
	public Text currentLapTimeText;

	// Token: 0x0400166E RID: 5742
	private bool _started;

	// Token: 0x0400166F RID: 5743
	public float TimeToBeat = 9999f;

	// Token: 0x04001670 RID: 5744
	public Text TimeToBeatText;

	// Token: 0x04001671 RID: 5745
	public TMP_Text DriftScoreText;

	// Token: 0x04001672 RID: 5746
	public RaceControl MainRaceControl;

	// Token: 0x04001673 RID: 5747
	public bool Checkpoint;

	// Token: 0x04001674 RID: 5748
	public GameObject StartTrigger;

	// Token: 0x04001675 RID: 5749
	public GameObject Checkpoint1;

	// Token: 0x04001676 RID: 5750
	public GameObject Checkpoint2;

	// Token: 0x04001677 RID: 5751
	public GameObject Checkpoint3;

	// Token: 0x04001678 RID: 5752
	public GameObject Checkpoint4;

	// Token: 0x04001679 RID: 5753
	public GameObject Checkpoint5;

	// Token: 0x0400167A RID: 5754
	public GameObject Checkpoint6;

	// Token: 0x0400167B RID: 5755
	public GameObject Checkpoint7;

	// Token: 0x0400167C RID: 5756
	public GameObject Checkpoint8;

	// Token: 0x0400167D RID: 5757
	public GameObject Checkpoint9;

	// Token: 0x0400167E RID: 5758
	public GameObject FinishTrigger;

	// Token: 0x0400167F RID: 5759
	public Text HighScoreTable;
}
