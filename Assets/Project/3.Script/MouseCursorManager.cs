using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;  // 변경할 커서 이미지
    public Vector2 hotSpot = Vector2.zero; // 커서 핫스팟 (커서 클릭 지점)
    public CursorMode cursorMode = CursorMode.Auto;
    private void Start()
    {
        ChangeCursor();
    }

    public void ChangeCursor()
    {
        if (cursorTexture != null)
        {
            // 커서를 customCursor로 변경
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
        else
        {
            Debug.LogError("커서 이미지가 설정되지 않았습니다.");
        }
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);  // 기본 커서로 변경
    }
}
