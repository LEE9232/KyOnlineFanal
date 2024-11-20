using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDelete : MonoBehaviour
{
// 이 스크립트에서 버튼 클릭시 아이템정보를 DB에 보내는걸 연결함.


    public Button itemBtn;

    public void ClickBtn()
    {
        Destroy(gameObject);
    }
}
