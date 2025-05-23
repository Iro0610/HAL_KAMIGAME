using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class SyncColliderToSprite : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D bc;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        // �@ ワールド空間でのスプライトの実際のサイズを取得
        //    bounds.size はレンダラーが描画する "実際の" 大きさ (ワールド単位)
        Vector2 worldSize = sr.bounds.size;

        // �A BoxCollider2D のサイズを合わせる
        bc.size = worldSize;

        // �B Collider がスプライトの中心にくるようオフセットをリセット
        bc.offset = Vector2.zero;
    }
}
