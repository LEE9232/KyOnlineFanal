using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDelete : MonoBehaviour
{
// �� ��ũ��Ʈ���� ��ư Ŭ���� ������������ DB�� �����°� ������.


    public Button itemBtn;

    public void ClickBtn()
    {
        Destroy(gameObject);
    }
}
