using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �߉ƍT���F���E��? / ��? / �f?����  
/// ?�� & ?�� F �s�v���߉Ə��� keyId �^ Handle.requiredKeyId ����
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("��?�Q��")] public float moveSpeed = 5f;
    [Header("��?�����x")] public float jumpForce = 12f;

    [Header("F??�u")]
    public KeyCode interactKey = KeyCode.F;
    public float holdThreshold = 0.25f;

    [Header("�ڒn??")]
    public Transform groundCheck;
    public float groundCheckRadius = .08f;
    public LayerMask groundMask;

    [Header("���O?�� keyId")]
    public string currentItemKey = "";          // "" �\���������L

    /* UI�i��?�j */
    public Image hookIcon;
    public Sprite iconGot, iconNot;

    /* ����?�� */
    Rigidbody2D rb;
    bool isGrounded, isSliding;
    HandleController currentHandle;
    bool isPressingF; float fTime; bool slideTrig;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (groundCheck == null) groundCheck = transform.Find("groundCheck");
        if (hookIcon && iconNot) hookIcon.sprite = iconNot;
    }

    void Update()
    {
        /* ---- �ڒn?? ---- */
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        /* ---- ��? / ��? ---- */
        if (!isSliding)
        {
            float h = Input.GetKey(KeyCode.D) ? 1 :
                      Input.GetKey(KeyCode.A) ? -1 : 0;
            rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else rb.velocity = Vector2.zero;

        /* ---- F ??? ---- */
        if (Input.GetKeyDown(interactKey))
        {
            isPressingF = true; fTime = 0; slideTrig = false;
        }

        if (isPressingF)
        {
            fTime += Time.deltaTime;

            /* ?�F�v��?���C�z */
            if (!slideTrig &&
                fTime >= holdThreshold &&
                currentHandle && !currentHandle.isMoving &&
                currentItemKey == currentHandle.requiredKeyId)
            {
                slideTrig = true; isSliding = true;
                currentHandle.StartMoveWithPlayer(gameObject);
            }
        }

        if (Input.GetKeyUp(interactKey))
        {
            /* �Z�F��?�v��?���C�z */
            if (!slideTrig &&
                fTime < holdThreshold &&
                currentHandle && !currentHandle.isMoving &&
                currentItemKey == currentHandle.requiredKeyId)
            {
                currentHandle.StartMoveAlone();
            }

            isPressingF = false; fTime = 0;
        }

        /* ����?��?? */
        if (isSliding && currentHandle && !currentHandle.isMoving) isSliding = false;
    }

    /* ---- Trigger ?�� ---- */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Handle"))
        {
            currentHandle = other.GetComponent<HandleController>();
            return;
        }

        if (other.CompareTag("Item"))
        {
            ItemEntity it = other.GetComponent<ItemEntity>();
            if (it)
            {
                currentItemKey = it.keyId;                     // ???��
                if (hookIcon && iconGot) hookIcon.sprite = iconGot;
                Destroy(other.gameObject);
                Debug.Log($"?��?��: {currentItemKey}");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Handle") &&
            other.GetComponent<HandleController>() == currentHandle)
            currentHandle = null;
    }
}
