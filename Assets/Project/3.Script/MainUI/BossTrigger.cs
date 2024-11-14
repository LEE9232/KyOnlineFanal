using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    //public ChoicePopup choicePopup;
    public GameObject BossObj;
    public GameObject BossCheckObj;
    public Camera cutsceneCamera; // �ƽſ� ī�޶�
    public Camera playerCamera; // �÷��̾��� �⺻ ī�޶�
    public Transform bossTarget;         // ī�޶� ȸ���� �� �ٶ� ������ Transform
    public float rotationSpeed = 20f;    // ī�޶� ȸ�� �ӵ�
    public float cutsceneDuration = 5f;  // �ƽ� ���� �ð�
    public TextMeshProUGUI cutsceneText; // TextMeshPro UI �ؽ�Ʈ
    public string bossDialogue = "�Ҹ��� ���� ������ϴ�\n���� óġ�ϼ���";  // �ƽ� �߿� ǥ���� �ؽ�Ʈ
    public float typeSpeed = 0.05f;      // �� ���ھ� ��Ÿ���� �ӵ�
    public float fadeDuration = 4f;      // �ؽ�Ʈ�� ������ ������� �ð�
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
        // ī�޶� ��ȯ: �÷��̾� ī�޶� ��Ȱ��ȭ�ϰ� �ƽ� ī�޶� Ȱ��ȭ
        playerCamera.gameObject.SetActive(false);
        cutsceneCamera.gameObject.SetActive(true);
        // ���� �ð��� ������ �ƽ� ����
        // ī�޶� ȸ���� ����
        // �ؽ�Ʈ�� ȭ�鿡 ���
        cutsceneText.gameObject.SetActive(true);
        //cutsceneText.text = "�Ҹ��� ���� ������ϴ�\n���� óġ�ϼ���"; 
        StartCoroutine(ShowTextTypewriterEffect());
        StartCoroutine(RotateCameraAroundBoss());
    }

    // �ؽ�Ʈ�� �� ���ھ� ����ϴ� Typewriter ȿ��
    private IEnumerator ShowTextTypewriterEffect()
    {
        cutsceneText.text = ""; // �ؽ�Ʈ�� ��� �� ����
        foreach (char letter in bossDialogue.ToCharArray())
        {
            cutsceneText.text += letter; // �� ���ھ� �߰�
            yield return new WaitForSeconds(typeSpeed); // ���ڴ� ��� �ð�
        }
        // Typewriter ȿ���� ���� �� �ؽ�Ʈ ����� ����
        StartCoroutine(FadeOutText());
    }

    // �ؽ�Ʈ�� ������ ��������� ���İ��� ���̴� �Լ�
    private IEnumerator FadeOutText()
    {
        float elapsedTime = 0f;
        Color originalColor = cutsceneText.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration); // ���İ��� 1���� 0���� ���� ����
            cutsceneText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // �������� �� �ؽ�Ʈ ��Ȱ��ȭ
        cutsceneText.gameObject.SetActive(false);
    }


    private IEnumerator RotateCameraAroundBoss()
    {
        float elapsedTime = 0f;

        // ���� �ð� ���� ī�޶� ������ �߽����� ȸ��
        while (elapsedTime < cutsceneDuration)
        {
            // ������ �߽����� ī�޶� ȸ��
            cutsceneCamera.transform.RotateAround(bossTarget.position, Vector3.up, rotationSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �ƽ� ����
        StartCoroutine(EndCutscene());
    }
    private IEnumerator EndCutscene()
    {
        // 5�� ���� �ƽ� ��� �� ����
        yield return new WaitForSeconds(5f);

        // �ƽ� ī�޶� ��Ȱ��ȭ �� �÷��̾� ī�޶� ��Ȱ��ȭ
        cutsceneCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
        cutsceneText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(BossCheckObj);
    }

}
