// KIZK Created

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Player : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")]
    public float speedFloat = 5.0f;
    [SerializeField, Header("�W�����v��")]
    public float jumpPower = 10.0f;
    [SerializeField, Header("Raduis")]
    private float radius = 0.5f;
    [SerializeField, Header("�ڐG����")]
    public Transform groundCheck;
    [SerializeField, Header("LayerMask")]
    public LayerMask groundLayer;
    [SerializeField, Header("���O�\��")]
    public bool showLog = true;

    // ���W�b�g�{�f�B���擾�p�ϐ�
    private Rigidbody2D rb;
    // �n�ʂƂ̐ڐG�t���O
    private bool touchFg;

    // Start is called before the first frame update
    void Start()
    {
        // rigidbody�̏����擾
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // touchFg = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);
        // Debug.Log("touchFg: " + touchFg);

        // A/D or ��/�� �ŉ��ړ�
        float mx = Input.GetAxisRaw("Horizontal");
        // ���ړ��̂ݍX�V
        rb.velocity = new Vector2(mx * speedFloat, rb.velocity.y);

        // �W�����v����
        if(Input.GetButtonDown("Jump") && touchFg)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    private void FixedUpdate()
    {
        touchFg = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);
        if(showLog) Debug.Log("touchFg: " + touchFg);
    }

    // ����͈͂�����
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = touchFg ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, radius);
        }
    }
}
