using System.Collections;
using UnityEngine;

public class HorizontalFastener : MonoBehaviour
{
    [Header("�[�̔���")]
    [Range(0f, 0.5f)]
    public float edgePercent = 0.1f;

    private float longPress_time = 0.2f;
    private float press_time = 0.0f;
    private bool isPressing = false;
    private bool canInput = true; // ������Ԃ͓��͂��󂯕t����

    private Collider2D triggerCollider;
    private GameObject playerObj;

    [Header("�L����")]
    public bool useHorizontal = true;  // ��������L���ɂ���t���O
    public bool useVertical = false;  // �c�����𖳌��ɐݒ�

    void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
        if (!triggerCollider || !triggerCollider.isTrigger)
        {
            Debug.LogWarning("���̃I�u�W�F�N�g�ɂ� Trigger �ɐݒ肳�ꂽ Collider2D ���K�v�ł��I");
        }
    }

    void Update()
    {
        if (!canInput) return; // ���͎�t�������Ȃ珈�����Ȃ�

        if (Input.GetKeyDown(KeyCode.F))
        {
            press_time = Time.time;
            isPressing = true;
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            float heldDuration = Time.time - press_time;
            isPressing = false;

            if (heldDuration < longPress_time)
            {
                Debug.Log("short press");
                ShortPressAction();
            }
            else
            {
                Debug.Log("long press");
                LongPressAction();
            }

            // ��������͍ēx���͂�������
            canInput = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerObj = other.gameObject;
            Vector2 playerPos = other.transform.position;
            Bounds bounds = triggerCollider.bounds;

            bool isOnHorizontalEdge = false;

            if (useHorizontal)
            {
                float width = bounds.size.x;
                float leftEdge = bounds.min.x + width * edgePercent;
                float rightEdge = bounds.max.x - width * edgePercent;
                isOnHorizontalEdge = (playerPos.x <= leftEdge || playerPos.x >= rightEdge);
            }

            canInput = isOnHorizontalEdge; // ���[�ɂ���Ƃ��������͂��󂯕t����
        }
    }

    void ShortPressAction()
    {
        // �Z�����͂̏ꍇ�A�g���K�[��؂�ւ���
        if (triggerCollider)
        {
            triggerCollider.isTrigger = !triggerCollider.isTrigger;
            Debug.Log("ShortPress: Collider trigger = " + triggerCollider.isTrigger);
        }
    }

    void LongPressAction()
    {
        if (!playerObj) return;

        // �������̏����̂�
        if (useHorizontal)
        {
            Transform playerTrans = playerObj.transform;
            Collider2D playerCol = playerObj.GetComponent<Collider2D>();
            if (!playerCol) return;

            Bounds objBounds = triggerCollider.bounds;
            Bounds playerBounds = playerCol.bounds;

            Vector3 axis = transform.right.normalized; // ������
            float halfExtent = objBounds.extents.x;

            // �v���C���[�̓��e�͈͂��v�Z
            float maxProj = GetMaxProjection(playerBounds, axis);

            Vector3 center = objBounds.center;
            Vector3 toPlayer = playerTrans.position - center;
            float dot = Vector3.Dot(toPlayer, axis);

            // �������̃^�[�Q�b�g�ʒu
            Vector3 targetPos = (dot >= 0) ?
                center - axis * (halfExtent - maxProj) :
                center + axis * (halfExtent - maxProj);

            // �������ɓ�����
            if (Mathf.Abs(playerTrans.position.y - targetPos.y) > Mathf.Epsilon)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothMove(playerTrans, targetPos, 0.3f, () =>
                {
                    triggerCollider.isTrigger = !triggerCollider.isTrigger;
                    canInput = true; // ��������ɓ��͂��ēx�L���ɂ���
                }));
            }
        }
    }

    // �v���C���[�̓��e�͈͌v�Z
    float GetMaxProjection(Bounds playerBounds, Vector3 axis)
    {
        Vector3[] corners = new Vector3[8];
        Vector3 min = playerBounds.min;
        Vector3 max = playerBounds.max;

        int i = 0;
        for (int x = 0; x <= 1; x++)
            for (int y = 0; y <= 1; y++)
                for (int z = 0; z <= 1; z++)
                    corners[i++] = new Vector3(
                        x == 0 ? min.x : max.x,
                        y == 0 ? min.y : max.y,
                        z == 0 ? min.z : max.z
                    );

        float maxProjection = 0f;
        foreach (var corner in corners)
        {
            float proj = Mathf.Abs(Vector3.Dot(corner - playerBounds.center, axis));
            if (proj > maxProjection)
                maxProjection = proj;
        }

        return maxProjection;
    }

    // SmoothMove �R���[�`��
    IEnumerator SmoothMove(Transform obj, Vector3 target, float duration, System.Action onComplete = null)
    {
        Vector3 startPos = obj.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            obj.position = Vector3.Lerp(startPos, target, t);
            yield return null;
        }

        obj.position = target;
        onComplete?.Invoke(); // �I����ɃR�[���o�b�N�����s
    }
}
