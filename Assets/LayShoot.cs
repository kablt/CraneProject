using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayShoot : MonoBehaviour
{
    public float rayDis = 10f;
    public void ShootAndCheckForCoil()
    {
        // Shoot a ray from the current GameObject in the forward direction
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.right, out hit, rayDis))
        {
            // Check if the hit object has the specified tag "coil"
            if (hit.collider.CompareTag("Coil"))
            {
                Debug.Log("����Ȯ�� �Ϸ�");
            }
            else
            {
                Debug.Log("������ �ƴ� ��ü�� �ֽ��ϴ�");
            }
        }
        else
        {
            Debug.Log("��� ��ü�� ��");
        }
    }
}