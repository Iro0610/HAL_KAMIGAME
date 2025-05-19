using System.Collections;
using UnityEngine;

/// <summary>
/// ??控制：?按 / ?按 ???一律先切?拉?状?  
/// 字段 requiredKeyId 用于跟玩家所持?匙匹配
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class HandleController : MonoBehaviour
{
    [Header("滑?速度")]
    public float moveSpeed = 6f;

    [Header("需要的 keyId")]
    public string requiredKeyId = "red";

    public bool isMoving { get; private set; } = false;

    FastenerController rail;

    void Start() => rail = GetComponentInParent<FastenerController>();

    /* ---------- ?按：???独移? ---------- */
    public void StartMoveAlone()
    {
        if (isMoving) return;
        rail.Toggle();                       // ?始即切??体/虚体
        StartCoroutine(MoveCoroutine(null));
    }

    /* ---------- ?按：玩家附着移? ---------- */
    public void StartMoveWithPlayer(GameObject player)
    {
        if (isMoving) return;
        rail.Toggle();
        StartCoroutine(MoveCoroutine(player));
    }

    /* ---------- 核心?程 ---------- */
    IEnumerator MoveCoroutine(GameObject player)
    {
        isMoving = true;

        (Vector2 p0, Vector2 p1) = rail.GetEndPoints();
        Vector2 start = Vector2.Distance(transform.position, p0) <
                        Vector2.Distance(transform.position, p1) ? p0 : p1;
        Vector2 end = (start == p0) ? p1 : p0;

        Rigidbody2D rbP = player ? player.GetComponent<Rigidbody2D>() : null;
        if (rbP) { rbP.velocity = Vector2.zero; rbP.isKinematic = true; }

        float t = 0f, dist = Vector2.Distance(start, end);
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed / dist;
            Vector2 pos = Vector2.Lerp(start, end, t);
            transform.position = pos;
            if (rbP) player.transform.position = pos;
            yield return null;
        }

        if (rbP) rbP.isKinematic = false;
        isMoving = false;
    }
}
