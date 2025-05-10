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

        // �@ ���[���h��Ԃł̃X�v���C�g�̎��ۂ̃T�C�Y���擾
        //    bounds.size �̓����_���[���`�悷�� "���ۂ�" �傫�� (���[���h�P��)
        Vector2 worldSize = sr.bounds.size;

        // �A BoxCollider2D �̃T�C�Y�����킹��
        bc.size = worldSize;

        // �B Collider ���X�v���C�g�̒��S�ɂ���悤�I�t�Z�b�g�����Z�b�g
        bc.offset = Vector2.zero;
    }
}
