using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossSkillParticle : MonoBehaviour
{
    public GameObject[] particlePrefabs;
    public int skillIndex; // ��ų �ε����� ����
    public void CreateParticleEffect(int index, Vector3 basePosition,
        float destroyTime , Vector3 direction, int skillIndex)
    {
        if (index >= 0 && index < particlePrefabs.Length && particlePrefabs[index] != null)
        {
            Vector3 effectPosition = basePosition;
            GameObject instantiatedParticle = Instantiate(particlePrefabs[index], basePosition, Quaternion.LookRotation(direction));
            // ���� ��ü�� ��ũ��Ʈ ���� ����
            ParticleCollision particleCollisionScript = instantiatedParticle.GetComponentInChildren<ParticleCollision>();
            if (particleCollisionScript != null)
            {
                particleCollisionScript.bossSkillParticle = this;
                particleCollisionScript.skillIndex = skillIndex; // ��ų �ε����� ����
            }
            Destroy(instantiatedParticle, destroyTime);
        }
    }
}
