using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000181 RID: 385
public class MyBoneSCR : MonoBehaviour
{
	// Token: 0x060008B4 RID: 2228 RVA: 0x000535CC File Offset: 0x000517CC
	public void Start()
	{
		if (this._initZScale == 0f)
		{
			this._initZScale = 1f;
		}
		this.thisTransform.localScale = new Vector3(1f, 1f, this._initZScale);
		base.transform.localRotation = Quaternion.Euler(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, this.StartZrotation);
		base.StartCoroutine(this.FinishedInitializing());
		base.enabled = false;
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0005365B File Offset: 0x0005185B
	private IEnumerator FinishedInitializing()
	{
		yield return new WaitForSeconds(3f);
		if (this.StrechToName != "")
		{
			foreach (Transform transform in base.transform.root.GetComponentsInChildren<Transform>())
			{
				if (transform.name == this.StrechToName)
				{
					this.targetTransform = transform;
				}
				if (!this.targetTransform)
				{
					base.enabled = false;
				}
			}
		}
		if (!this.doubleSided && !this.BikeShock && this.LocalStrtetchTarget != null)
		{
			this._initDistance = Vector3.Distance(this.thisTransform.position, this.LocalStrtetchTarget.position);
		}
		else if (this.doubleSided)
		{
			this._initDistance = Vector3.Distance(this.thisTransform.position, this.targetTransform.position);
		}
		if (this._initZScale == 0f)
		{
			this._initZScale = 1f;
		}
		this.Started = true;
		this.ReStart();
		yield break;
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x0005366C File Offset: 0x0005186C
	public void ReStart()
	{
		if (!this.doubleSided && this.StrechToName != "")
		{
			this.targetTransform = null;
			if (this._initZScale == 0f)
			{
				this._initZScale = 1f;
			}
			this.thisTransform.localScale = new Vector3(1f, 1f, this._initZScale);
			foreach (Transform transform in base.transform.root.GetComponentsInChildren<Transform>())
			{
				if (transform.name == this.StrechToName)
				{
					this.targetTransform = transform;
					base.enabled = true;
				}
				if (!this.targetTransform)
				{
					base.enabled = false;
				}
			}
		}
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00053730 File Offset: 0x00051930
	public void Update()
	{
		if (!this.targetTransform || this.targetTransform.root != base.transform.root)
		{
			base.enabled = false;
			return;
		}
		if (!this.BikeShock)
		{
			if (this.doubleSided)
			{
				Vector3 position = (this.targetTransform.position + this.targetTransformB.position) / 2f;
				this.thisTransform.position = position;
				if (this.RotateWithPivots)
				{
					this.thisTransform.LookAt(this.targetTransform, base.transform.up);
				}
			}
			if (this.stretchToTarget && this._initDistance != 0f)
			{
				float num = Vector3.Distance(this.thisTransform.position, this.targetTransform.position) / this._initDistance * this._initZScale;
				Vector3 localScale = this.thisTransform.localScale;
				this.thisTransform.localScale = new Vector3(localScale.x, localScale.y, num);
				if (!this.DontRotate)
				{
					base.transform.LookAt(this.targetTransform, base.transform.up);
				}
				if (num > 2.4f)
				{
					this.ReStart();
				}
			}
			if (this.RearBikeShock)
			{
				if (this.FaceOpposite)
				{
					this.thisTransform.LookAt(this.targetTransform, -base.transform.parent.up);
				}
				else
				{
					this.thisTransform.LookAt(this.targetTransform, base.transform.parent.up);
				}
			}
			else if (this.FaceOpposite)
			{
				this.thisTransform.LookAt(this.targetTransform, -base.transform.root.up);
			}
			else
			{
				this.thisTransform.LookAt(this.targetTransform, base.transform.root.up);
			}
			if (this.RotateAsTarget)
			{
				base.transform.localRotation = this.targetTransform.localRotation;
				return;
			}
		}
		else if (this.targetTransform.root == this.thisTransform.root)
		{
			base.transform.position = this.targetTransform.position;
		}
	}

	// Token: 0x0400104E RID: 4174
	public bool KeepOnZ;

	// Token: 0x0400104F RID: 4175
	public bool Started;

	// Token: 0x04001050 RID: 4176
	public float StartZrotation;

	// Token: 0x04001051 RID: 4177
	public bool FaceOpposite;

	// Token: 0x04001052 RID: 4178
	public bool RotateAsTarget;

	// Token: 0x04001053 RID: 4179
	public bool DontRotate;

	// Token: 0x04001054 RID: 4180
	public bool BikeShock;

	// Token: 0x04001055 RID: 4181
	public bool RearBikeShock;

	// Token: 0x04001056 RID: 4182
	[Tooltip("    Should the object be positioned between two points. Also affects position besides rotation.")]
	public bool doubleSided;

	// Token: 0x04001057 RID: 4183
	public bool RotateWithPivots;

	// Token: 0x04001058 RID: 4184
	[Tooltip("    Should the object be stretched between pivot and target?")]
	public bool stretchToTarget = true;

	// Token: 0x04001059 RID: 4185
	[Tooltip("    The transform that represents the lookAtTarget and stretch target.")]
	public Transform targetTransform;

	// Token: 0x0400105A RID: 4186
	public string StrechToName;

	// Token: 0x0400105B RID: 4187
	public Transform LocalStrtetchTarget;

	// Token: 0x0400105C RID: 4188
	[Tooltip("    Second target. Object will be stretched positioned between targetTransform and targetTransformB if doubleSided is\r\n    true.")]
	public Transform targetTransformB;

	// Token: 0x0400105D RID: 4189
	[Tooltip("    The transform that represents the bone.")]
	public Transform thisTransform;

	// Token: 0x0400105E RID: 4190
	public float _initDistance;

	// Token: 0x0400105F RID: 4191
	public float _initZScale;
}
