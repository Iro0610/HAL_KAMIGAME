using UnityEngine;

/// <summary>
/// 可拾取物品（例：?匙 / Hook）
/// 触? Player 后，把自己的 keyId 写入玩家并??自身
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ItemEntity : MonoBehaviour
{
    [Header("物品??的 keyId")]
    public string keyId = "red";          // 例: red / blue 等

    void Reset()
    {
        // ?建?制体?自??? Trigger 且 Tag=Item
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
        gameObject.tag = "Item";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        pc.currentItemKey = keyId;                 // ??玩家当前?匙
        Debug.Log($"{pc.name} ?得?匙: {keyId}");

        Destroy(gameObject);                       // 物品消失
        // TODO: 播放拾取音效或?示 UI
    }
}
