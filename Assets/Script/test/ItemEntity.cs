using UnityEngine;

/// <summary>
/// �E�敨�i�i��F?�� / Hook�j
/// �G? Player �@�C�c���ȓI keyId �ʓ��߉ƛ�??���g
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ItemEntity : MonoBehaviour
{
    [Header("���i??�I keyId")]
    public string keyId = "red";          // ��: red / blue ��

    void Reset()
    {
        // ?��?����?��??? Trigger �� Tag=Item
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
        gameObject.tag = "Item";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        pc.currentItemKey = keyId;                 // ??�߉Ɠ��O?��
        Debug.Log($"{pc.name} ?��?��: {keyId}");

        Destroy(gameObject);                       // ���i����
        // TODO: �d���E�批����?�� UI
    }
}
