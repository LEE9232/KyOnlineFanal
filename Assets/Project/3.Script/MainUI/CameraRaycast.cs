using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    public Transform player;
    public LayerMask objMask;
    private List<Renderer> transparentObjects = new List<Renderer>(); // 투명 처리된 오브젝트 리스트

    private void Update()
    {
        ClearPreviousTransparentObjects(); // 이전 투명 오브젝트를 원상태로 복구

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // 레이캐스트로 플레이어와 카메라 사이의 모든 장애물 탐지
        RaycastHit[] hits = Physics.RaycastAll(transform.position, directionToPlayer, distanceToPlayer, objMask);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                Renderer obstruction = hit.collider.GetComponent<Renderer>();

                if (obstruction != null && !transparentObjects.Contains(obstruction))
                {
                    SetObjectTransparent(obstruction, true);
                    transparentObjects.Add(obstruction); // 투명 처리된 오브젝트 리스트에 추가
                }
            }
        }
    }



    // 이전에 투명 처리된 오브젝트를 원래 상태로 복구
    private void ClearPreviousTransparentObjects()
    {
        foreach (Renderer obj in transparentObjects)
        {
            if (obj != null)
            {
                SetObjectTransparent(obj, false);
            }
        }
        transparentObjects.Clear(); // 리스트 초기화
    }



    public void SetObjectTransparent(Renderer obj, bool transparent)
    {
        foreach (var material in obj.materials)  // Renderer에 여러 Material이 있을 경우 모두 처리
        {
            // Material의 Shader를 투명 모드로 변경
            if (transparent)
            {
                material.SetFloat("_Mode", 3);  // 0: Opaque, 1: Cutout, 2: Fade, 3: Transparent
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;

                //// 투명도 조정, 최소값을 0.5로 제한하여 완전 투명해지지 않도록 설정
                //Color color = material.color;
                //color.a = Mathf.Max(0.5f, 0.7f); // 완전 투명으로 만들지 않고 약간 투명하게 처리
                //material.color = color;

            }
            else
            {
                // 원래 상태로 복구
                material.SetFloat("_Mode", 0);  // Opaque 모드로 복구
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;


                //// 투명도 복구
                //Color color = material.color;
                //color.a = 1f; // 원래 불투명 상태로 복구
                //material.color = color;
            }



            Color color = obj.material.color;
            color.a = transparent ? 0.3f : 1f; // 투명도 설정
            obj.material.color = color;
        }
    }
}
