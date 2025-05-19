using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家控制：左右移? / 跳? / 拉?交互  
/// ?按 & ?按 F 都要求玩家所持 keyId 与 Handle.requiredKeyId 相等
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("移?参数")] public float moveSpeed = 5f;
    [Header("跳?初速度")] public float jumpForce = 12f;

    [Header("F??置")]
    public KeyCode interactKey = KeyCode.F;
    public float holdThreshold = 0.25f;

    [Header("接地??")]
    public Transform groundCheck;
    public float groundCheckRadius = .08f;
    public LayerMask groundMask;

    [Header("当前?匙 keyId")]
    public string currentItemKey = "";          // "" 表示尚未持有

    /* UI（可?） */
    public Image hookIcon;
    public Sprite iconGot, iconNot;

    /* 内部?量 */
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
        /* ---- 接地?? ---- */
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        /* ---- 移? / 跳? ---- */
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

            /* ?按：要求?匙匹配 */
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
            /* 短按：同?要求?匙匹配 */
            if (!slideTrig &&
                fTime < holdThreshold &&
                currentHandle && !currentHandle.isMoving &&
                currentItemKey == currentHandle.requiredKeyId)
            {
                currentHandle.StartMoveAlone();
            }

            isPressingF = false; fTime = 0;
        }

        /* 滑索?束?? */
        if (isSliding && currentHandle && !currentHandle.isMoving) isSliding = false;
    }

    /* ---- Trigger ?理 ---- */
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
                currentItemKey = it.keyId;                     // ???匙
                if (hookIcon && iconGot) hookIcon.sprite = iconGot;
                Destroy(other.gameObject);
                Debug.Log($"?得?匙: {currentItemKey}");
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
