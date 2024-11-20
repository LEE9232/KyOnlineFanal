using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;  // ������ Ŀ�� �̹���
    public Vector2 hotSpot = Vector2.zero; // Ŀ�� �ֽ��� (Ŀ�� Ŭ�� ����)
    public CursorMode cursorMode = CursorMode.Auto;
    private void Start()
    {
        ChangeCursor();
    }

    public void ChangeCursor()
    {
        if (cursorTexture != null)
        {
            // Ŀ���� customCursor�� ����
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
        else
        {
            Debug.LogError("Ŀ�� �̹����� �������� �ʾҽ��ϴ�.");
        }
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);  // �⺻ Ŀ���� ����
    }
}
