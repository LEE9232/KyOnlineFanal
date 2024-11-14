using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private MinimapIconManager minimapIconManager;  // MinimapIconManager 참조
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
            Debug.LogError("minimapIconManager 가 널입니다.");
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
            // 텍스트확인
            choicePopup.ShowPopup($"던전으로\n입장 하시겠습니까?", () =>
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
