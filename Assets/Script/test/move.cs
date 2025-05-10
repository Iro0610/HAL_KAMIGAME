//ozaki
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float moveSpeed = 2f;  // �ړ����x
    private bool isPlayerOn = false;  // �v���C���[������Ă��邩

    void Update()
    {
        if (isPlayerOn)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    // �v���C���[���������t���O�𗧂Ă�
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOn = true;
        }
    }

    // �v���C���[���~�肽��t���O��������i�K�v�Ȃ�j
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOn = false;
        }
    }
}