using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalReturn : MonoBehaviour
{
    private MinimapIconManager minimapIconManager;
    public Camera mainCamera;
    public Transform nameTagPosition;
    public ChoicePopup choicePopup;
    private void Start()
    {
        mainCamera = Camera.main;
        minimapIconManager = FindObjectOfType<MinimapIconManager>();
        if (minimapIconManager != null)
        {
            minimapIconManager.RegisterIcon(transform, EntityType.Gate);
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
            choicePopup.ShowPopup($"마을로\n이동 하시겠습니까?", () =>
            {
                SceneceDunguonClick();
            });
        }
    }

    public void SceneceDunguonClick()
    {
        Changescenemaneger.Instance.StageOneScene();
    }
}
