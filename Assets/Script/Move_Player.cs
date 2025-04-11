using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Player : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")]
    public float speedFloat;
    [SerializeField, Header("�W�����v��")]
    public float jumpPower;
    [SerializeField, Header("Transform")]
    public new Transform transform;
    [SerializeField, Header("LayerMask")]
    public LayerMask layer;
    [SerializeField, Header("Raduis")]
    private float radius = 0.3f;

    // ���W�b�g�{�f�B���擾�p�ϐ�
    private Rigidbody2D rb;
    // �n�ʂƂ̐ڐG�t���O
    private bool touchFg;

    // Start is called before the first frame update
    void Start()
    {
        // rigidbody�̏����擾
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        touchFg = Physics2D.OverlapCircle(transform.position, radius, layer);
        Debug.Log("touchFg: " + touchFg);

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

    // ����͈͂�����
    private void OnDrawGizmosSelected()
    {
        if (transform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
