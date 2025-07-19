using System;
using UnityEngine;

// Token: 0x02000267 RID: 615
public class Demo_BoxController : MonoBehaviour
{
	// Token: 0x06000EA9 RID: 3753 RVA: 0x0000245B File Offset: 0x0000065B
	private void Awake()
	{
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x0000245B File Offset: 0x0000065B
	private void Start()
	{
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x0009B884 File Offset: 0x00099A84
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			for (int i = 0; i < this.boxesArray.transform.childCount; i++)
			{
				if (Vector3.Distance(base.transform.position, this.boxesArray.transform.GetChild(i).position) < 1.5f)
				{
					Animation component = this.boxesArray.transform.GetChild(i).GetChild(0).GetComponent<Animation>();
					if (!this.boxesStatus[i])
					{
						component.Play("Open");
						this.boxesStatus[i] = true;
					}
					else
					{
						component.Play("Close");
						this.boxesStatus[i] = false;
					}
				}
			}
		}
	}

	// Token: 0x040017CC RID: 6092
	public bool status;

	// Token: 0x040017CD RID: 6093
	public Animation anim;

	// Token: 0x040017CE RID: 6094
	public GameObject boxesArray;

	// Token: 0x040017CF RID: 6095
	private bool[] boxesStatus = new bool[7];
}
