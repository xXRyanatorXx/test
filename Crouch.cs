using System;
using System.Collections;
using Rewired;
using UnityEngine;

// Token: 0x02000131 RID: 305
public class Crouch : MonoBehaviour
{
	// Token: 0x0600067F RID: 1663 RVA: 0x0003496E File Offset: 0x00032B6E
	private void Start()
	{
		this.player = ReInput.players.GetPlayer(0);
		this.capsule = base.GetComponent<CapsuleCollider>();
		this.normalheight = 1.6f;
		this.usedwheel = false;
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x000349A0 File Offset: 0x00032BA0
	private void LateUpdate()
	{
		if (!this.inCar && !tools.sitting)
		{
			this.crouchHeight = this.normalheight;
		}
		else
		{
			this.crouchHeight = this.InCARmaxHeight;
		}
		if (!tools.sitting)
		{
			if (this.player.GetButtonUp("Crouch") && !base.GetComponent<tools>().notes.active && !this.usedwheel)
			{
				this.isCrouchingWHEEL = false;
				if (this.capsule.height > 0.8f)
				{
					this.isCrouching = true;
				}
				else if (this.capsule.height > 0.25f)
				{
					this.isCrouching2 = true;
				}
				else
				{
					this.isCrouching = false;
					this.isCrouching2 = false;
				}
			}
			if (this.player.GetButtonDown("Crouch") && !base.GetComponent<tools>().notes.active)
			{
				this.usedwheel = false;
			}
			if (this.player.GetButton("Crouch") && !base.GetComponent<tools>().notes.active)
			{
				if (Input.GetAxis("Mouse ScrollWheel") > 0f)
				{
					this.ScrolingUP = true;
					this.isCrouchingWHEEL = true;
					this.isCrouching2 = true;
					this.usedwheel = true;
				}
				else
				{
					this.ScrolingUP = false;
				}
				if (Input.GetAxis("Mouse ScrollWheel") < 0f)
				{
					this.ScrolingDOWN = true;
					this.isCrouchingWHEEL = true;
					this.isCrouching2 = true;
					this.usedwheel = true;
				}
				else
				{
					this.ScrolingDOWN = false;
				}
			}
			if (this.player.GetButtonDown("Sprint") && !base.GetComponent<tools>().notes.active)
			{
				this.isCrouching2 = false;
				this.isCrouching = false;
				this.usedwheel = false;
				this.isCrouchingWHEEL = false;
			}
			if (this.isCrouching || this.isCrouching2)
			{
				FirstPersonAIO.isCrouching = true;
			}
			else
			{
				FirstPersonAIO.isCrouching = false;
			}
			if (!this.inCar && this.capsule.height < this.normalheight && ((!this.isCrouching && !this.isCrouching2) || this.ScrolingDOWN))
			{
				Collider[] array = Physics.OverlapSphere(this.head.transform.position, 0.2f);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].transform.root.tag == "Vehicle")
					{
						this.crouchHeight = this.capsule.height;
					}
				}
			}
			if (!this.ScrolingDOWN && !this.ScrolingUP && !this.isCrouchingWHEEL && !this.usedwheel)
			{
				if (this.isCrouching && this.capsule.height > 0.8f)
				{
					this.capsule.height = Mathf.MoveTowards(this.capsule.height, 0.8f, 3f * Time.deltaTime);
				}
				if (this.isCrouching2 && this.capsule.height > 0.25f)
				{
					this.capsule.height = Mathf.MoveTowards(this.capsule.height, 0.25f, 3f * Time.deltaTime);
				}
				if (!this.isCrouching && !this.isCrouching2)
				{
					this.capsule.height = Mathf.MoveTowards(this.capsule.height, this.crouchHeight, 3f * Time.deltaTime);
				}
			}
			if (this.ScrolingUP && this.joint2.transform.localScale.x > 0.02f)
			{
				this.capsule.height = Mathf.MoveTowards(this.capsule.height, 0f, 2f * Time.deltaTime);
			}
			if (this.ScrolingDOWN)
			{
				this.capsule.height = Mathf.MoveTowards(this.capsule.height, this.crouchHeight, 2f * Time.deltaTime);
			}
			if (this.capsule.height > 0.01f)
			{
				this.joint2.transform.localScale = new Vector3(this.capsule.height / this.normalheight, this.capsule.height / this.normalheight, this.capsule.height / this.normalheight);
				return;
			}
			this.joint2.transform.localScale = new Vector3(0.01f / this.normalheight, 0.01f / this.normalheight, 0.01f / this.normalheight);
		}
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x00034E0A File Offset: 0x0003300A
	public void WaitStart()
	{
		base.StartCoroutine(this.Wait());
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x00034E19 File Offset: 0x00033019
	private IEnumerator Wait()
	{
		yield return 50;
		if (!this.inCar)
		{
			bool sitting = tools.sitting;
		}
		yield break;
	}

	// Token: 0x040009E6 RID: 2534
	private Player player;

	// Token: 0x040009E7 RID: 2535
	public KeyCode crouchKey = KeyCode.LeftControl;

	// Token: 0x040009E8 RID: 2536
	public bool isCrouching;

	// Token: 0x040009E9 RID: 2537
	public bool isCrouching2;

	// Token: 0x040009EA RID: 2538
	public bool isCrouchingWHEEL;

	// Token: 0x040009EB RID: 2539
	public bool usedwheel;

	// Token: 0x040009EC RID: 2540
	public float InCARmaxHeight = 0.85f;

	// Token: 0x040009ED RID: 2541
	public bool inCar;

	// Token: 0x040009EE RID: 2542
	public float crouchHeight = 1.3f;

	// Token: 0x040009EF RID: 2543
	public float incarheight = 2f;

	// Token: 0x040009F0 RID: 2544
	public float normalheight = 1.3f;

	// Token: 0x040009F1 RID: 2545
	public GameObject joint2;

	// Token: 0x040009F2 RID: 2546
	public bool ScrolingUP;

	// Token: 0x040009F3 RID: 2547
	public bool ScrolingDOWN;

	// Token: 0x040009F4 RID: 2548
	public Transform head;

	// Token: 0x040009F5 RID: 2549
	[HideInInspector]
	[Tooltip("Capsule collider to lower when we crouch.\nCan be empty.")]
	public CapsuleCollider capsuleCollider;

	// Token: 0x040009F6 RID: 2550
	[HideInInspector]
	private CapsuleCollider capsule;
}
