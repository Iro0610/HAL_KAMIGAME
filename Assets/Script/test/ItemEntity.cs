using UnityEngine;

/// <summary>
/// ÂEæ¨iiáF?ú / Hookj
/// G? Player @Cc©ÈI keyId ÊüßÆó??©g
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ItemEntity : MonoBehaviour
{
    [Header("¨i??I keyId")]
    public string keyId = "red";          // á: red / blue 

    void Reset()
    {
        // ??§Ì?©??? Trigger  Tag=Item
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
        gameObject.tag = "Item";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        pc.currentItemKey = keyId;                 // ??ßÆO?ú
        Debug.Log($"{pc.name} ?¾?ú: {keyId}");

        Destroy(gameObject);                       // ¨iÁ¸
        // TODO: dúEæ¹Á½?¦ UI
    }
}
