using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip;
    public TextMeshProUGUI tooltipText;    // ������ ǥ���� �ؽ�Ʈ

    [TextArea(3, 10)]           // �ν����Ϳ��� ���� �� �ؽ�Ʈ �Է� ����
    public string message;      // �� ĳ���͸��� �ٸ� �޽����� ������ �� �ֵ��� ���� �߰�
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.SetActive(true);                  // ���� Ȱ��ȭ
        tooltipText.gameObject.SetActive(true);
        tooltipText.text = message;               // ������ ǥ���� �޽��� ����

        //Vector3 offset = new Vector3(30f, 30f, 0f); // ������ ���� �ణ �̵�
        //tooltip.transform.position = Input.mousePosition;  // ���� ��ġ�� ���콺 ��ġ�� ����
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);  // ���� ��Ȱ��ȭ
        tooltipText.gameObject.SetActive(false);
    }

    
}
