using System;
using RVP;
using UnityEngine;

// Token: 0x02000290 RID: 656
public class hitter : MonoBehaviour
{
	// Token: 0x06000F56 RID: 3926 RVA: 0x0009F0FA File Offset: 0x0009D2FA
	private void Start()
	{
		this.AudioParent = GameObject.Find("hand");
		base.gameObject.layer = LayerMask.NameToLayer("Default");
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x0009F124 File Offset: 0x0009D324
	private void Update()
	{
		if (base.gameObject.GetComponent<Rigidbody>() == null)
		{
			base.gameObject.AddComponent<Rigidbody>();
			base.gameObject.GetComponent<Rigidbody>().mass = 1600f;
			base.gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		}
		if (base.transform.localPosition.z <= 1f)
		{
			base.transform.Translate(Vector3.forward * Time.deltaTime * 5f);
			return;
		}
		UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		base.transform.localPosition = new Vector3(0f, 0f, 0f);
		base.gameObject.layer = LayerMask.NameToLayer("Wheels");
		base.transform.rotation = base.transform.parent.transform.rotation;
		base.enabled = false;
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x0009F218 File Offset: 0x0009D418
	private void OnCollisionEnter(Collision collision)
	{
		if (tools.tool == 11)
		{
			if (collision.gameObject.GetComponent<VehicleDamage>())
			{
				collision.collider.gameObject.GetComponent<VehicleDamage>().Start();
			}
			if (collision.collider.gameObject.GetComponent<DetachablePart>())
			{
				if (collision.collider.gameObject.GetComponent<DetachablePart>().breakForce > 17f)
				{
					collision.collider.gameObject.GetComponent<DetachablePart>().breakForce -= 10f;
					this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().BreakOff);
				}
				else
				{
					Pickup component = collision.collider.GetComponent<Pickup>();
					if (component != null)
					{
						if (!collision.collider.gameObject.GetComponent<Rigidbody>())
						{
							collision.collider.gameObject.AddComponent<Rigidbody>();
							collision.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
							collision.collider.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
						}
						component.BRAKE();
						this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().BreakOff2);
					}
					PickupDoor component2 = collision.collider.GetComponent<PickupDoor>();
					if (component2 != null)
					{
						if (collision.collider.GetComponent<FixedJoint>() != null)
						{
							UnityEngine.Object.Destroy(collision.collider.GetComponent<FixedJoint>());
						}
						if (collision.collider.GetComponent<HingeJoint>() != null)
						{
							UnityEngine.Object.Destroy(collision.collider.GetComponent<HingeJoint>());
						}
						component2.BRAKE();
						this.AudioParent.GetComponent<AudioSource>().PlayOneShot(this.AudioParent.GetComponent<AudioManager>().BreakOff2);
					}
				}
			}
			else
			{
				Pickup component3 = collision.collider.GetComponent<Pickup>();
				if (component3 != null)
				{
					if (!collision.collider.gameObject.GetComponent<Rigidbody>())
					{
						collision.collider.gameObject.AddComponent<Rigidbody>();
						collision.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
						collision.collider.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
					}
					component3.BRAKE();
				}
				PickupDoor component4 = collision.collider.GetComponent<PickupDoor>();
				if (component4 != null)
				{
					if (collision.collider.GetComponent<FixedJoint>() != null)
					{
						UnityEngine.Object.Destroy(collision.collider.GetComponent<FixedJoint>());
					}
					if (collision.collider.GetComponent<HingeJoint>() != null)
					{
						UnityEngine.Object.Destroy(collision.collider.GetComponent<HingeJoint>());
					}
					component4.BRAKE();
				}
			}
			CarProperties component5 = collision.collider.GetComponent<CarProperties>();
			if (component5 != null)
			{
				component5.Ruined = true;
			}
			Mesh mesh = collision.collider.GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			Vector3 b = Vector3.zero;
			ContactPoint contactPoint = collision.contacts[0];
			b = collision.collider.transform.InverseTransformPoint(contactPoint.point);
			for (int i = 0; i < vertices.Length; i++)
			{
				if ((vertices[i] - b).sqrMagnitude <= 0.15f)
				{
					vertices[i] = Vector3.Lerp(vertices[i], b, 0.1f);
				}
				mesh.vertices = vertices;
			}
		}
		UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
		base.transform.localPosition = new Vector3(0f, 0f, 0f);
		base.gameObject.layer = LayerMask.NameToLayer("Wheels");
		base.transform.rotation = base.transform.parent.transform.rotation;
		base.enabled = false;
	}

	// Token: 0x04001891 RID: 6289
	public GameObject AudioParent;
}
