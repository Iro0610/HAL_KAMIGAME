using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Player : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")]
    public float speedFloat = 5.0f;
    [SerializeField, Header("�W�����v��")]
    public float jumpPower = 10.0f;
    [SerializeField, Header("Radius")]
    public float radius = 0.5f;            // Inspector �Œ����ł���悤 public ��
    [SerializeField, Header("�ڐG����")]
    public Transform groundCheck;
    [SerializeField, Header("LayerMask")]
    public LayerMask groundLayer;
    [SerializeField, Header("���O�\��")]
    public bool showLog = true;

    // Rigidbody2D �擾�p
    private Rigidbody2D rb;
    // �n�ʐڐG�t���O
    private bool touchFg;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // �O�̂��ߓ|��h�~
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // ���E�ړ�
        float mx = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(mx * speedFloat, rb.velocity.y);

        // �W�����v
        if (Input.GetButtonDown("Jump") && touchFg)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    void FixedUpdate()
    {
        // �n�ʐڐG�`�F�b�N
        touchFg = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);
        if (showLog) Debug.Log("touchFg: " + touchFg);
    }

    public bool IsGrounded()
    {
        return touchFg;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = touchFg ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, radius);
        }
    }
}
