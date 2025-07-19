using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class ParticleCollision : MonoBehaviour
{
	// Token: 0x06000164 RID: 356 RVA: 0x0000B636 File Offset: 0x00009836
	private void Start()
	{
		this.m_ParticleSystem = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0000B644 File Offset: 0x00009844
	private void OnParticleCollision(GameObject other)
	{
		int collisionEvents = this.m_ParticleSystem.GetCollisionEvents(other, this.m_CollisionEvents);
		for (int i = 0; i < collisionEvents; i++)
		{
			ExtinguishableFire component = this.m_CollisionEvents[i].colliderComponent.GetComponent<ExtinguishableFire>();
			if (component != null)
			{
				component.Extinguish();
			}
		}
	}

	// Token: 0x040001D7 RID: 471
	private List<ParticleCollisionEvent> m_CollisionEvents = new List<ParticleCollisionEvent>();

	// Token: 0x040001D8 RID: 472
	private ParticleSystem m_ParticleSystem;
}
