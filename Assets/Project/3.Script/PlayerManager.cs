using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    private List<GameObject> players = new List<GameObject>(); // 플레이어 목록
    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
           Destroy(gameObject); // 이미 인스턴스가 존재하면 파괴
        }
    }
    // 플레이어가 게임에 룸안에 접속했을때
    public void RegisterPlayer(GameObject player)
    {
        if (player != null && !players.Contains(player))
        {
            players.Add(player);
        }
    }
    // 플레이어가 나갔을때
    public void UnregisterPlayer(GameObject player)
    {
        if (player != null && players.Contains(player))
        {
            players.Remove(player);
        }
    }
    // 플레이어의 정보를 전달할곳
    public List<GameObject> GetPlayers()
    {
        return new List<GameObject>(players);
    }
}
