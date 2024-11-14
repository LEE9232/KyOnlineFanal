using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip;
    public TextMeshProUGUI tooltipText;    // 툴팁에 표시할 텍스트

    [TextArea(3, 10)]           // 인스펙터에서 여러 줄 텍스트 입력 가능
    public string message;      // 각 캐릭터마다 다른 메시지를 설정할 수 있도록 변수 추가
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.SetActive(true);                  // 툴팁 활성화
        tooltipText.gameObject.SetActive(true);
        tooltipText.text = message;               // 툴팁에 표시할 메시지 설정

        //Vector3 offset = new Vector3(30f, 30f, 0f); // 오른쪽 위로 약간 이동
        //tooltip.transform.position = Input.mousePosition;  // 툴팁 위치를 마우스 위치로 설정
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);  // 툴팁 비활성화
        tooltipText.gameObject.SetActive(false);
    }

    
}
