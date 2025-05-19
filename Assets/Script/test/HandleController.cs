using System.Collections;
using UnityEngine;

/// <summary>
/// ??�T���F?�� / ?�� ???�ꗥ���?�f?��?  
/// ���i requiredKeyId �p����߉Ə���?���C�z
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class HandleController : MonoBehaviour
{
    [Header("��?���x")]
    public float moveSpeed = 6f;

    [Header("���v�I keyId")]
    public string requiredKeyId = "red";

    public bool isMoving { get; private set; } = false;

    FastenerController rail;

    void Start() => rail = GetComponentInParent<FastenerController>();

    /* ---------- ?�F???�ƈ�? ---------- */
    public void StartMoveAlone()
    {
        if (isMoving) return;
        rail.Toggle();                       // ?�n����??��/����
        StartCoroutine(MoveCoroutine(null));
    }

    /* ---------- ?�F�߉ƕ�����? ---------- */
    public void StartMoveWithPlayer(GameObject player)
    {
        if (isMoving) return;
        rail.Toggle();
        StartCoroutine(MoveCoroutine(player));
    }

    /* ---------- �j�S?�� ---------- */
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
