using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    public Transform player;
    public LayerMask objMask;
    private List<Renderer> transparentObjects = new List<Renderer>(); // ���� ó���� ������Ʈ ����Ʈ

    private void Update()
    {
        ClearPreviousTransparentObjects(); // ���� ���� ������Ʈ�� �����·� ����

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // ����ĳ��Ʈ�� �÷��̾�� ī�޶� ������ ��� ��ֹ� Ž��
        RaycastHit[] hits = Physics.RaycastAll(transform.position, directionToPlayer, distanceToPlayer, objMask);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                Renderer obstruction = hit.collider.GetComponent<Renderer>();

                if (obstruction != null && !transparentObjects.Contains(obstruction))
                {
                    SetObjectTransparent(obstruction, true);
                    transparentObjects.Add(obstruction); // ���� ó���� ������Ʈ ����Ʈ�� �߰�
                }
            }
        }
    }



    // ������ ���� ó���� ������Ʈ�� ���� ���·� ����
    private void ClearPreviousTransparentObjects()
    {
        foreach (Renderer obj in transparentObjects)
        {
            if (obj != null)
            {
                SetObjectTransparent(obj, false);
            }
        }
        transparentObjects.Clear(); // ����Ʈ �ʱ�ȭ
    }



    public void SetObjectTransparent(Renderer obj, bool transparent)
    {
        foreach (var material in obj.materials)  // Renderer�� ���� Material�� ���� ��� ��� ó��
        {
            // Material�� Shader�� ���� ���� ����
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

                //// ���� ����, �ּҰ��� 0.5�� �����Ͽ� ���� ���������� �ʵ��� ����
                //Color color = material.color;
                //color.a = Mathf.Max(0.5f, 0.7f); // ���� �������� ������ �ʰ� �ణ �����ϰ� ó��
                //material.color = color;

            }
            else
            {
                // ���� ���·� ����
                material.SetFloat("_Mode", 0);  // Opaque ���� ����
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;


                //// ���� ����
                //Color color = material.color;
                //color.a = 1f; // ���� ������ ���·� ����
                //material.color = color;
            }



            Color color = obj.material.color;
            color.a = transparent ? 0.3f : 1f; // ���� ����
            obj.material.color = color;
        }
    }
}
