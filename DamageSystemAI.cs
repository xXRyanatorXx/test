using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class DamageSystemAI : MonoBehaviour
{
	// Token: 0x06000062 RID: 98 RVA: 0x00006594 File Offset: 0x00004794
	private void Start()
	{
		this.ACAI = base.transform.GetComponent<AnyCarAI>();
		this.collisionSystemOn = this.ACAI.collisionSystem;
		this.collisionSound = this.ACAI.collisionSound;
		this.SetUpCollisionAudioSource(this.collisionSound);
		this.collisionVolume = this.ACAI.collisionVolume;
		if (this.collisionSystemOn)
		{
			if (this.ACAI.optionalMeshList.Length == 0 || !this.ACAI.customMesh)
			{
				this.ACAI.bodyMesh.AddComponent<MeshCollisionScriptAI>();
				this.ACAI.bodyMesh.GetComponent<MeshCollisionScriptAI>().maxCollisionStrength /= this.ACAI.demolutionStrenght;
				this.ACAI.bodyMesh.GetComponent<MeshCollisionScriptAI>().demolutionRange = this.ACAI.demolutionRange;
				this.ACAI.bodyMesh.GetComponent<MeshCollisionScriptAI>().meshFilter = this.ACAI.bodyMesh.GetComponent<MeshFilter>();
				this.ACAI.bodyMesh.GetComponent<MeshCollisionScriptAI>().collisionParticlesON = this.ACAI.collisionParticles;
				return;
			}
			if (this.ACAI.customMesh)
			{
				foreach (OptionalMeshesAI optionalMeshesAI in this.ACAI.optionalMeshList)
				{
					optionalMeshesAI.modelMesh.gameObject.AddComponent<MeshCollisionScriptAI>();
					optionalMeshesAI.modelMesh.gameObject.GetComponent<MeshCollisionScriptAI>().maxCollisionStrength /= this.ACAI.demolutionStrenght;
					optionalMeshesAI.modelMesh.gameObject.GetComponent<MeshCollisionScriptAI>().demolutionRange = this.ACAI.demolutionRange;
					optionalMeshesAI.modelMesh.gameObject.GetComponent<MeshCollisionScriptAI>().meshFilter = optionalMeshesAI.modelMesh.gameObject.GetComponent<MeshFilter>();
					optionalMeshesAI.modelMesh.gameObject.GetComponent<MeshCollisionScriptAI>().collisionParticlesON = this.ACAI.collisionParticles;
					optionalMeshesAI.modelMesh.gameObject.GetComponent<MeshCollisionScriptAI>().loseAftCollisions = optionalMeshesAI.loseAftCollisions;
					if (optionalMeshesAI.modelMesh.gameObject.GetComponent<MeshCollider>() == null)
					{
						optionalMeshesAI.modelMesh.gameObject.AddComponent(typeof(MeshCollider));
						optionalMeshesAI.modelMesh.gameObject.GetComponent<MeshCollider>().convex = true;
						optionalMeshesAI.modelMesh.gameObject.GetComponent<MeshCollider>().isTrigger = true;
					}
				}
			}
		}
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00006808 File Offset: 0x00004A08
	public void OnCollisionEnter(Collision collision)
	{
		this.hitPoint = collision;
		if (this.collisionSystemOn)
		{
			if (this.ACAI.optionalMeshList.Length == 0 || !this.ACAI.customMesh)
			{
				this.ACAI.bodyMesh.GetComponent<MeshCollisionScriptAI>().hitPoint = this.hitPoint;
				this.ACAI.bodyMesh.GetComponent<MeshCollisionScriptAI>().collisionHappened = true;
			}
			else if (this.ACAI.customMesh)
			{
				OptionalMeshesAI[] optionalMeshList = this.ACAI.optionalMeshList;
				for (int i = 0; i < optionalMeshList.Length; i++)
				{
					optionalMeshList[i].modelMesh.gameObject.GetComponent<MeshCollisionScriptAI>().hitPoint = this.hitPoint;
				}
			}
		}
		this.crashSound.volume = this.hitPoint.relativeVelocity.magnitude / 100f * this.collisionVolume;
		this.crashSound.Play();
	}

	// Token: 0x06000064 RID: 100 RVA: 0x000068F4 File Offset: 0x00004AF4
	private AudioSource SetUpCollisionAudioSource(AudioClip clip)
	{
		this.crashSound = base.transform.gameObject.AddComponent<AudioSource>();
		this.crashSound.clip = clip;
		this.crashSound.loop = false;
		this.crashSound.pitch = 1f;
		this.crashSound.playOnAwake = false;
		this.crashSound.minDistance = 5f;
		this.crashSound.reverbZoneMix = 1.5f;
		this.crashSound.maxDistance = 600f;
		this.crashSound.dopplerLevel = 2f;
		return this.crashSound;
	}

	// Token: 0x040000D0 RID: 208
	private Collision hitPoint;

	// Token: 0x040000D1 RID: 209
	private bool collisionSystemOn;

	// Token: 0x040000D2 RID: 210
	private AudioSource crashSound;

	// Token: 0x040000D3 RID: 211
	private AudioClip collisionSound;

	// Token: 0x040000D4 RID: 212
	private float collisionVolume;

	// Token: 0x040000D5 RID: 213
	private AnyCarAI ACAI;
}
