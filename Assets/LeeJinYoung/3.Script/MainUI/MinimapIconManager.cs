using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    Player,
    Monster,
    NPCWithQuest,
    NPCNoQuest,
    Gate
}
public class MinimapIconManager : MonoBehaviour
{
    public RectTransform minimapPanel;  // �̴ϸ� UI�� �θ� �г�
    public GameObject playerIconPrefab; // �÷��̾� ������ ������
    private GameObject playerIconInstance; // �÷��̾� ������ �ν��Ͻ�
    public Camera mainCamera;
    public Camera minimapCamera; // �̴ϸ� ���� ī�޶�
    public GameObject npcQuestIconPrefab; // ����Ʈ�� �ִ� NPC ������ ������
    public GameObject npcNormalIconPrefab; // �Ϲ� NPC ������ ������
    public GameObject monsterIconPrefab; // ���� ������ ������
    public GameObject gateIconPrefeb;
    public float minimapScale = 4f;
    private Transform playerTransform;
    private Dictionary<Transform, GameObject> minimapIcons = new Dictionary<Transform, GameObject>();
    // �÷��̾�, ����, NPC ���� �������� �����Ͽ� ���
    void Update()
    {
        if (playerTransform != null && playerIconInstance != null)
        {
            // �÷��̾��� ��ġ�� �̴ϸʿ� �ݿ�
            Vector3 minimapPosition = GetMinimapPosition(playerTransform.position , true);
            // �÷��̾��� ��ġ�� �̴ϸ� �߾ӿ� ����
            playerIconInstance.GetComponent<RectTransform>().localPosition = GetMinimapPosition(playerTransform.position, true);
            //playerIconInstance.GetComponent<RectTransform>().localPosition = minimapPosition;

            // �÷��̾��� Z�� ȸ�� ������ ������ ȸ��
            playerIconInstance.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -mainCamera.transform.eulerAngles.y);

            //// ���� ī�޶��� Z�� ȸ���� �������� �̴ϸ� ȸ��
            float cameraRotationZ = mainCamera.transform.eulerAngles.z;
            minimapPanel.localRotation = Quaternion.Euler(0, 0, cameraRotationZ);
        }
        // ��ϵ� ��� �������� ��ġ�� ȸ�� ������Ʈ
        UpdateAllIconPositions();
    }

    public void RegisterPlayerIcon(Transform player)
    {
        playerTransform = player;
        playerIconInstance = Instantiate(playerIconPrefab, minimapPanel);
    }
    // ���� ������ ���
    // ������ ��� (����, NPC ��)
    public void RegisterIcon(Transform entity, EntityType type)
    {
        GameObject iconPrefab = null;
        switch (type)
        {
            case EntityType.Monster:
                iconPrefab = monsterIconPrefab;
                break;
            case EntityType.NPCWithQuest:
                iconPrefab = npcQuestIconPrefab;
                break;
            case EntityType.NPCNoQuest:
                iconPrefab = npcNormalIconPrefab;
                break;
            case EntityType.Gate:
                iconPrefab = gateIconPrefeb;
                break;
        }



        if (iconPrefab != null && !minimapIcons.ContainsKey(entity))
        {
            GameObject iconInstance = Instantiate(iconPrefab, minimapPanel);
            minimapIcons[entity] = iconInstance;
        }
    }

    // ������ ��ġ�� ������Ʈ (���������� ����)
    public void UpdateAllIconPositions()
    {
        foreach (var kvp in minimapIcons)
        {
            Transform entity = kvp.Key;
            GameObject icon = kvp.Value;
            Vector3 minimapPos = GetMinimapPosition(entity.position);

            // ���� ��� ���̸� �������� �����
            if (minimapPos == Vector3.zero)
            {
                icon.SetActive(false);
            }
            else
            {
                icon.SetActive(true);
                icon.GetComponent<RectTransform>().localPosition = minimapPos;
                // ���� �������� ȸ������ ���� (����)
                if (entity != playerTransform)
                {
                    icon.GetComponent<RectTransform>().localRotation = Quaternion.identity;
                }
            }
        }

        // �÷��̾� �����ܸ� ī�޶��� ȸ���� �ݿ��Ͽ� ȸ��
        if (playerTransform != null && playerIconInstance != null)
        {
            playerIconInstance.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -mainCamera.transform.eulerAngles.y);
        }

    }

    private Vector3 GetMinimapPosition(Vector3 worldPosition, bool isPlayer = false)
    {

        Vector3 offset = worldPosition - playerTransform.position;
        Vector3 minimapPosition = new Vector3(offset.x * minimapScale, offset.z * minimapScale, 0);

        // �̴ϸ� ī�޶��� ���� ���� ���� �ִ��� Ȯ��
        Vector3 viewportPoint = minimapCamera.WorldToViewportPoint(worldPosition);

        // ���� �������� ī�޶��� ���� ���� ���� ���� ������ Vector3.zero�� ��ȯ
        if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            return Vector3.zero; // ���� ���� ���̸� �������� ���� �� �ֵ���
        }
        return minimapPosition;
    }
    // ��ü ���� �� �����ܵ� ����
    public void UnregisterIcon(Transform entity)
    {
        if (minimapIcons.ContainsKey(entity))
        {
            Destroy(minimapIcons[entity]);
            minimapIcons.Remove(entity);
        }
    }
}
