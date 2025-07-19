using System;
using System.Collections;
using TurnTheGameOn.SimpleTrafficSystem;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public class FloatingPoinMyfix : MonoBehaviour
{
	// Token: 0x060004ED RID: 1261 RVA: 0x00029110 File Offset: 0x00027310
	public void ResetPosition()
	{
		if (this.DontRunInMultiplayer && PlayerPrefs.HasKey("MapExtension") && PlayerPrefs.GetFloat("MapExtension") == 0f)
		{
			return;
		}
		if (this.SHIFTING)
		{
			return;
		}
		this.SHIFTING = true;
		Vector3 position = base.gameObject.transform.position;
		position.y = 0f;
		if (tools.MPrunning)
		{
			if (tools.NetworkPLayer.isServer && !this.DontRunInMultiplayer)
			{
				tools.NetworkPLayer.ShiftWorld(position);
				return;
			}
		}
		else
		{
			this.ResetPosition2(position);
		}
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x000291A0 File Offset: 0x000273A0
	public void ResetPosition2(Vector3 cameraPosition)
	{
		if (cameraPosition.magnitude > this.threshold)
		{
			if (this.CAM)
			{
				this.CAM.SetActive(false);
			}
			base.StartCoroutine(this.SetCam());
			foreach (Transform transform in UnityEngine.Object.FindObjectsOfType(typeof(Transform)))
			{
				if (transform.parent == null && transform.name != "DamagePlace" && !transform.GetComponent<networkDummy>() && !transform.GetComponent<NetworkPLayer>())
				{
					if (transform.GetComponent<MPobject>())
					{
						if (transform.GetComponent<MPobject>().networkDummy != null && transform.GetComponent<MPobject>().networkDummy.hasAuthority)
						{
							transform.position -= cameraPosition;
						}
					}
					else if (transform.position.y < 0f)
					{
						UnityEngine.Object.Destroy(transform.gameObject);
					}
					else
					{
						transform.position -= cameraPosition;
					}
				}
			}
			if (this._AITrafficController != null)
			{
				this._AITrafficController.ShiftedWorld();
			}
		}
		this.SHIFTING = false;
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x000292EC File Offset: 0x000274EC
	private IEnumerator SetCam()
	{
		yield return 0;
		if (this.CAM)
		{
			this.CAM.SetActive(true);
		}
		yield break;
	}

	// Token: 0x0400069F RID: 1695
	public float threshold = 100f;

	// Token: 0x040006A0 RID: 1696
	public float physicsThreshold;

	// Token: 0x040006A1 RID: 1697
	public float defaultSleepThreshold = 0.14f;

	// Token: 0x040006A2 RID: 1698
	public GameObject CAM;

	// Token: 0x040006A3 RID: 1699
	public bool DontRunInMultiplayer;

	// Token: 0x040006A4 RID: 1700
	public AITrafficController _AITrafficController;

	// Token: 0x040006A5 RID: 1701
	public bool SHIFTING;

	// Token: 0x040006A6 RID: 1702
	private ParticleSystem.Particle[] parts;
}
