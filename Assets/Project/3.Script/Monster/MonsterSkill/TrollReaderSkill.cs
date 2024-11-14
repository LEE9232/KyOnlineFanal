using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TrollReaderSkill : MonoBehaviour
{
    public float Dash = 30f;
    public bool IsJumpDash { get; set; } =  false;
    private ReaderMonsAI reader;

    private void Start()
    {
        reader = GetComponent<ReaderMonsAI>();
    }
    public void ReaderJumpDash()
    {
        reader.agent.SetDestination(reader.GetTargetPlayerPosition().position);
        IsJumpDash = true;
        reader.agent.speed = Dash;
        reader.anim.SetTrigger("IsDash");
        StartCoroutine(ResetSpeedAfterDash());
    }
    public IEnumerator ResetSpeedAfterDash()
    {
        IsJumpDash = true;
        yield return new WaitForSeconds(2f);
        reader.agent.updatePosition = false;
        reader.agent.updateRotation = false;
        reader.agent.isStopped = true;
        reader.agent.velocity = Vector3.zero;  // 즉시 정지
        yield return new WaitForSeconds(1.7f);
        reader.agent.updatePosition = true;
        reader.agent.updateRotation = true;
        reader.agent.isStopped = false;
        reader.agent.speed = reader.monsterStatus.monsSpeed;
        yield return new WaitForSeconds(8f);
        IsJumpDash = false;
    }
}
