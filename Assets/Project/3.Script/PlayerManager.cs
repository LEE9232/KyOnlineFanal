using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    private List<GameObject> players = new List<GameObject>(); // �÷��̾� ���
    private void Awake()
    {
        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else
        {
           Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� �ı�
        }
    }
    // �÷��̾ ���ӿ� ��ȿ� ����������
    public void RegisterPlayer(GameObject player)
    {
        if (player != null && !players.Contains(player))
        {
            players.Add(player);
        }
    }
    // �÷��̾ ��������
    public void UnregisterPlayer(GameObject player)
    {
        if (player != null && players.Contains(player))
        {
            players.Remove(player);
        }
    }
    // �÷��̾��� ������ �����Ұ�
    public List<GameObject> GetPlayers()
    {
        return new List<GameObject>(players);
    }
}
