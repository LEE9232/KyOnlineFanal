using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossSkillParticle : MonoBehaviour
{
    public GameObject[] particlePrefabs;
    public int skillIndex; // 스킬 인덱스를 선언
    public void CreateParticleEffect(int index, Vector3 basePosition,
        float destroyTime , Vector3 direction, int skillIndex)
    {
        if (index >= 0 && index < particlePrefabs.Length && particlePrefabs[index] != null)
        {
            Vector3 effectPosition = basePosition;
            GameObject instantiatedParticle = Instantiate(particlePrefabs[index], basePosition, Quaternion.LookRotation(direction));
            // 하위 객체의 스크립트 참조 설정
            ParticleCollision particleCollisionScript = instantiatedParticle.GetComponentInChildren<ParticleCollision>();
            if (particleCollisionScript != null)
            {
                particleCollisionScript.bossSkillParticle = this;
                particleCollisionScript.skillIndex = skillIndex; // 스킬 인덱스를 설정
            }
            Destroy(instantiatedParticle, destroyTime);
        }
    }
}
