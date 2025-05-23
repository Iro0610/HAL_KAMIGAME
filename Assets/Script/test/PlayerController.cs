using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ßÆT§F¶EÚ? / µ? / f?ðÝ  
/// ?Â & ?Â F svßÆ keyId ^ Handle.requiredKeyId 
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Ú?Q")] public float moveSpeed = 5f;
    [Header("µ?¬x")] public float jumpForce = 12f;

    [Header("F??u")]
    public KeyCode interactKey = KeyCode.F;
    public float holdThreshold = 0.25f;

    [Header("Ún??")]
    public Transform groundCheck;
    public float groundCheckRadius = .08f;
    public LayerMask groundMask;

    [Header("O?ú keyId")]
    public string currentItemKey = "";          // "" \¦®¢L

    /* UIiÂ?j */
    public Image hookIcon;
    public Sprite iconGot, iconNot;

    /* à?Ê */
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
        /* ---- Ún?? ---- */
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        /* ---- Ú? / µ? ---- */
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

            /* ?ÂFv?úCz */
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
            /* ZÂF¯?v?úCz */
            if (!slideTrig &&
                fTime < holdThreshold &&
                currentHandle && !currentHandle.isMoving &&
                currentItemKey == currentHandle.requiredKeyId)
            {
                currentHandle.StartMoveAlone();
            }

            isPressingF = false; fTime = 0;
        }

        /* õ?©?? */
        if (isSliding && currentHandle && !currentHandle.isMoving) isSliding = false;
    }

    /* ---- Trigger ? ---- */
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
                currentItemKey = it.keyId;                     // ???ú
                if (hookIcon && iconGot) hookIcon.sprite = iconGot;
                Destroy(other.gameObject);
                Debug.Log($"?¾?ú: {currentItemKey}");
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
