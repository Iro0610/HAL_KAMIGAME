using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Player : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")]
    private float speedFloat;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // rigidbody�̏����擾
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // A/D or ��/��
        float mx = Input.GetAxisRaw("Horizontal");
        // W/S or ��/��
        float my = Input.GetAxisRaw("Vertical");

        Vector2 m_pos = new Vector2(mx, my).normalized;
        rb.velocity = m_pos * speedFloat;
    }
}
