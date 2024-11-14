using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private MinimapIconManager minimapIconManager;  // MinimapIconManager ����
    public Camera mainCamera;
    public Transform nameTagPosition;
    //public GameObject portalPopup;
    public ChoicePopup choicePopup;
    private void Start()
    {
        mainCamera = Camera.main;
        minimapIconManager = FindObjectOfType<MinimapIconManager>();
        if (minimapIconManager != null)
        {
            minimapIconManager.RegisterIcon(transform, EntityType.Gate);
        }
        else
        {
            Debug.LogError("minimapIconManager �� ���Դϴ�.");
        }
    }
    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            nameTagPosition.rotation = Quaternion.LookRotation(nameTagPosition.position - mainCamera.transform.position);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //portalPopup.SetActive(true);
            // �ؽ�ƮȮ��
            choicePopup.ShowPopup($"��������\n���� �Ͻðڽ��ϱ�?", () =>
            {
                SceneceDunguonClick();
            });
        }
    }

    public void SceneceDunguonClick()
    { 
        Changescenemaneger.Instance.DungionScecn();
    }
}
