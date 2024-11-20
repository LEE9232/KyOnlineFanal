using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    //public ChoicePopup choicePopup;
    public GameObject BossObj;
    public GameObject BossCheckObj;
    public Camera cutsceneCamera; // 컷신용 카메라
    public Camera playerCamera; // 플레이어의 기본 카메라
    public Transform bossTarget;         // 카메라가 회전할 때 바라볼 보스의 Transform
    public float rotationSpeed = 20f;    // 카메라 회전 속도
    public float cutsceneDuration = 5f;  // 컷신 지속 시간
    public TextMeshProUGUI cutsceneText; // TextMeshPro UI 텍스트
    public string bossDialogue = "불멸의 왕이 깨어났습니다\n왕을 처치하세요";  // 컷신 중에 표시할 텍스트
    public float typeSpeed = 0.05f;      // 한 글자씩 나타나는 속도
    public float fadeDuration = 4f;      // 텍스트가 서서히 사라지는 시간
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BossObj.SetActive(true);
            CutScene();
        }
    }

    public void CutScene()
    {
        // 카메라 전환: 플레이어 카메라를 비활성화하고 컷신 카메라를 활성화
        playerCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);
        // 일정 시간이 지나면 컷신 종료
        // 카메라 회전을 시작
        // 텍스트를 화면에 띄움
        cutsceneText.gameObject.SetActive(true);
        //cutsceneText.text = "불멸의 왕이 깨어났습니다\n왕을 처치하세요"; 
        StartCoroutine(ShowTextTypewriterEffect());
        StartCoroutine(RotateCameraAroundBoss());
    }

    // 텍스트를 한 글자씩 출력하는 Typewriter 효과
    private IEnumerator ShowTextTypewriterEffect()
    {
        cutsceneText.text = ""; // 텍스트를 비운 후 시작
        foreach (char letter in bossDialogue.ToCharArray())
        {
            cutsceneText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(typeSpeed); // 글자당 대기 시간
        }
        // Typewriter 효과가 끝난 후 텍스트 사라짐 시작
        StartCoroutine(FadeOutText());
    }

    // 텍스트가 서서히 사라지도록 알파값을 줄이는 함수
    private IEnumerator FadeOutText()
    {
        float elapsedTime = 0f;
        Color originalColor = cutsceneText.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration); // 알파값을 1에서 0으로 점차 줄임
            cutsceneText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // 투명해진 후 텍스트 비활성화
        cutsceneText.gameObject.SetActive(false);
    }


    private IEnumerator RotateCameraAroundBoss()
    {
        float elapsedTime = 0f;

        // 일정 시간 동안 카메라가 보스를 중심으로 회전
        while (elapsedTime < cutsceneDuration)
        {
            // 보스를 중심으로 카메라 회전
            cutsceneCamera.transform.RotateAround(bossTarget.position, Vector3.up, rotationSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 컷신 종료
        StartCoroutine(EndCutscene());
    }
    private IEnumerator EndCutscene()
    {
        // 5초 동안 컷신 재생 후 종료
        yield return new WaitForSeconds(5f);

        // 컷신 카메라 비활성화 및 플레이어 카메라 재활성화
        cutsceneCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
        cutsceneText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(BossCheckObj);
    }

}
