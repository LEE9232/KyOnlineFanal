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
    public RectTransform minimapPanel;  // 미니맵 UI의 부모 패널
    public GameObject playerIconPrefab; // 플레이어 아이콘 프리팹
    private GameObject playerIconInstance; // 플레이어 아이콘 인스턴스
    public Camera mainCamera;
    public Camera minimapCamera; // 미니맵 전용 카메라
    public GameObject npcQuestIconPrefab; // 퀘스트가 있는 NPC 아이콘 프리팹
    public GameObject npcNormalIconPrefab; // 일반 NPC 아이콘 프리팹
    public GameObject monsterIconPrefab; // 몬스터 아이콘 프리팹
    public GameObject gateIconPrefeb;
    public float minimapScale = 4f;
    private Transform playerTransform;
    private Dictionary<Transform, GameObject> minimapIcons = new Dictionary<Transform, GameObject>();
    // 플레이어, 몬스터, NPC 등의 아이콘을 생성하여 등록
    void Update()
    {
        if (playerTransform != null && playerIconInstance != null)
        {
            // 플레이어의 위치를 미니맵에 반영
            Vector3 minimapPosition = GetMinimapPosition(playerTransform.position , true);
            // 플레이어의 위치를 미니맵 중앙에 고정
            playerIconInstance.GetComponent<RectTransform>().localPosition = GetMinimapPosition(playerTransform.position, true);
            //playerIconInstance.GetComponent<RectTransform>().localPosition = minimapPosition;

            // 플레이어의 Z축 회전 값으로 아이콘 회전
            playerIconInstance.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -mainCamera.transform.eulerAngles.y);

            //// 메인 카메라의 Z축 회전을 기준으로 미니맵 회전
            float cameraRotationZ = mainCamera.transform.eulerAngles.z;
            minimapPanel.localRotation = Quaternion.Euler(0, 0, cameraRotationZ);
        }
        // 등록된 모든 아이콘의 위치와 회전 업데이트
        UpdateAllIconPositions();
    }

    public void RegisterPlayerIcon(Transform player)
    {
        playerTransform = player;
        playerIconInstance = Instantiate(playerIconPrefab, minimapPanel);
    }
    // 몬스터 아이콘 등록
    // 아이콘 등록 (몬스터, NPC 등)
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

    // 아이콘 위치를 업데이트 (지속적으로 실행)
    public void UpdateAllIconPositions()
    {
        foreach (var kvp in minimapIcons)
        {
            Transform entity = kvp.Key;
            GameObject icon = kvp.Value;
            Vector3 minimapPos = GetMinimapPosition(entity.position);

            // 만약 경계 밖이면 아이콘을 숨기기
            if (minimapPos == Vector3.zero)
            {
                icon.SetActive(false);
            }
            else
            {
                icon.SetActive(true);
                icon.GetComponent<RectTransform>().localPosition = minimapPos;
                // 몬스터 아이콘은 회전하지 않음 (고정)
                if (entity != playerTransform)
                {
                    icon.GetComponent<RectTransform>().localRotation = Quaternion.identity;
                }
            }
        }

        // 플레이어 아이콘만 카메라의 회전을 반영하여 회전
        if (playerTransform != null && playerIconInstance != null)
        {
            playerIconInstance.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -mainCamera.transform.eulerAngles.y);
        }

    }

    private Vector3 GetMinimapPosition(Vector3 worldPosition, bool isPlayer = false)
    {

        Vector3 offset = worldPosition - playerTransform.position;
        Vector3 minimapPosition = new Vector3(offset.x * minimapScale, offset.z * minimapScale, 0);

        // 미니맵 카메라의 가시 범위 내에 있는지 확인
        Vector3 viewportPoint = minimapCamera.WorldToViewportPoint(worldPosition);

        // 만약 아이콘이 카메라의 가시 범위 내에 있지 않으면 Vector3.zero를 반환
        if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            return Vector3.zero; // 가시 범위 밖이면 아이콘을 숨길 수 있도록
        }
        return minimapPosition;
    }
    // 객체 제거 시 아이콘도 삭제
    public void UnregisterIcon(Transform entity)
    {
        if (minimapIcons.ContainsKey(entity))
        {
            Destroy(minimapIcons[entity]);
            minimapIcons.Remove(entity);
        }
    }
}
