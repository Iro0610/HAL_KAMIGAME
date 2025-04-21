// KIZK Created

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_Fastener : MonoBehaviour
{
    // �����蔻��̉ӏ�(Horizontal�� ���E, Vertical �� �㉺, Both �� ���E�㉺)
    public enum EdgeDir { Horizontal, Vertical, Both }

    [Header("�[�̔���")]
    [Range(0f, 0.5f)]
    public float edgePercent = 0.2f; // �[�̕��i0.2 = 20%�j
    public EdgeDir edgeDirection = EdgeDir.Horizontal;

    // �������Ɣ��肳��鎞��
    private float longPress_time = 0.3f;
    // ���͎���
    private float press_time = 0.0f;
    // ���͔���t���O
    private bool isPressing = false;

    private bool canInput = false;

    private Collider2D triggerCollider;

    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
        if (!triggerCollider || !triggerCollider.isTrigger)
        {
            Debug.LogWarning("���̃I�u�W�F�N�g�ɂ� Trigger �ɐݒ肳�ꂽ Collider2D ���K�v�ł��I");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!canInput) return;

        // ���͏���(���͎�)
        if (Input.GetKeyDown(KeyCode.F))
        {
            press_time = Time.time;
            isPressing = true;
        }

        // ���͏���(���͒�)
        if (Input.GetKeyUp(KeyCode.F))
        {
            float heldDuration=Time.time- press_time;
            isPressing = true;

            if(heldDuration < longPress_time)
            {
                Debug.Log("short press");
                ShortPressAction();
            }
            else
            {
                Debug.Log("long press");
                LongPressAction();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector2 playerPos = other.transform.position;
            Bounds bounds = triggerCollider.bounds;

            bool isOnHorizontalEdge = false;
            bool isOnVerticalEdge = false;

            if (edgeDirection == EdgeDir.Horizontal || edgeDirection == EdgeDir.Both)
            {
                float width = bounds.size.x;
                float leftEdge = bounds.min.x + width * edgePercent;
                float rightEdge = bounds.max.x - width * edgePercent;
                isOnHorizontalEdge = (playerPos.x <= leftEdge || playerPos.x >= rightEdge);
            }

            if (edgeDirection == EdgeDir.Vertical || edgeDirection == EdgeDir.Both)
            {
                float height = bounds.size.y;
                float bottomEdge = bounds.min.y + height * edgePercent;
                float topEdge = bounds.max.y - height * edgePercent;
                isOnVerticalEdge = (playerPos.y <= bottomEdge || playerPos.y >= topEdge);
            }

            canInput = isOnHorizontalEdge || isOnVerticalEdge;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInput = false;
            isPressing = false;
        }
    }

    // �Z��������
    void ShortPressAction()
    {

    }
    
    // ����������
    void LongPressAction()
    {

    }
}
