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
                Debug.Log("코일확인 완료");
            }
            else
            {
                Debug.Log("코일이 아닌 물체가 있습니다");
            }
        }
        else
        {
            Debug.Log("어떠한 물체도 감지되지 않음");
        }
    }
}
