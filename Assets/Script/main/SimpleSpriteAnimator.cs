using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class SimpleSpriteAnimator : MonoBehaviour
{
    [Tooltip("�������[�V�����p�X�v���C�g�����Ԃɓ���Ă�������")]
    public Sprite[] walkSprites;
    [Tooltip("1�b�Ԃɐ؂�ւ���R�}��")]
    public float framesPerSecond = 10f;
    [Tooltip("�ړ����x")]
    public float moveSpeed = 5f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private int frameIndex;
    private float timer;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (walkSprites == null || walkSprites.Length == 0)
            Debug.LogWarning("walkSprites �ɃX�v���C�g���Z�b�g���Ă��������B");
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        // �����ɉ����ăX�v���C�g�𔽓]
        if (h > 0f) sr.flipX = false;
        else if (h < 0f) sr.flipX = true;

        // �ړ�
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);

        // �A�j���[�V����
        if (Mathf.Approximately(h, 0f))
        {
            frameIndex = 0;
            timer = 0f;
            sr.sprite = walkSprites[0];
        }
        else
        {
            timer += Time.deltaTime;
            float interval = 1f / framesPerSecond;
            if (timer >= interval)
            {
                timer -= interval;
                frameIndex = (frameIndex + 1) % walkSprites.Length;
                sr.sprite = walkSprites[frameIndex];
            }
        }
    }
}
