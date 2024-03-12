using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraneMove : MonoBehaviour
{
    public Transform PointA; //������ �ִ� ��ġ
    public Transform PointB; //������ ���� ���� ��ġ
    public Transform LiftRollBack; // ����Ʈ�� ����ҋ��� y�� ��ġ
    public float moveSpeed = 1f; // ũ������ �����̴� �ӵ�
    public GameObject CraneBody; // ������ ũ���� body
    public GameObject CraneHoist; // ������ ũ���� hosit
    public GameObject CraneLift; // ������ ũ���� lift
    public GameObject LayShooter; // ray�� ��� ��ü
    public LayerMask CoilLayer; // �浹�� ���̾� ����;
    private bool Body = false; // ��ü �����ϋ� �ٸ��� ������ x
    private bool Hoist = false;// ȣ���� �����϶� �ٸ��� ������ x


    enum CraneStatus
    {
        Idle, // ����Ʈ�� ���� �ö�����ִ� ����
        APoint, // ������ �ݱ����� Ʈ���� ������ ��ġ�� �̵�
        Detected, // ����Ʈ�� ������ ������ ���� �Լ�
        CoilMove, // Coil�� ������ ��ġ�� �ű�� �Լ�
    }
    void Update()
    {
        UseObjectBRay();
    }

    //�����ൿ�� �ְ� �Ǿ����� ������Ʈ���� switch���� ���� ��Ȳ�� �°� �Լ��� �ߵ��ϰ� �߰� �ۼ��ؾ��ҰͰ���.


    //���̸� ��� ������Ʈ�� �����ϰ� �ش� ray�� �����±׸� ���� ������Ʈ�� �ִ��� �˻� �Ѵ�.
    void UseObjectBRay()
    {
        // LayShooter�� �����ʺz�� �� �ִ� ��ũ��Ʈ LayShoot�� �����Ѵ�.
        LayShoot objectBShooter = LayShooter.GetComponent<LayShoot>();

        //�ش� ��ü�� null�� �ƴ϶��.(�ش� ������ �ִٸ�)
        if (objectBShooter != null)
        {
            // ��ü���� ShootAndCheckForCoil�Լ��� �����Ѵ�.(�ش� ��ü���� ���̸� ���  Tag�� Coil�ΰ��� �����ϴ� ����׸� ����Լ�)
            objectBShooter.ShootAndCheckForCoil();
            //ũ������ ��ġ�� ������ �������� ��ġ�� �ű�� �Լ�
            MovementRoutine();
        }
        else
        {
            Debug.LogError("���� �߸��Ǿ��ٴ� ����");
        }
    }


    //������ �������� APoint�� �ű�� �Լ�
    void MovementRoutine()
    {
    
            if (!Mathf.Approximately(CraneLift.transform.position.y, LiftRollBack.position.y))
            {
                Vector3 targetPositionY = new Vector3(CraneLift.transform.position.x, LiftRollBack.position.y, CraneLift.transform.position.z);
                CraneLift.transform.position = Vector3.Lerp(CraneLift.transform.position, targetPositionY, moveSpeed * Time.deltaTime);          
            }

            if (Mathf.Abs(CraneLift.transform.position.y - LiftRollBack.position.y) < 0.1f)
            {
                Vector3 targetpositionZ = new Vector3(CraneHoist.transform.position.x, CraneHoist.transform.position.y, PointA.position.z);
                CraneHoist.transform.position = Vector3.Lerp(CraneHoist.transform.position, targetpositionZ, moveSpeed * Time.deltaTime);            
            }

            if (Mathf.Abs(CraneHoist.transform.position.z - PointA.position.z) < 0.1f)
            {
                Vector3 targetpositionX = new Vector3(PointA.position.x, CraneBody.transform.position.y, CraneBody.transform.position.z);
                CraneBody.transform.position = Vector3.Lerp(CraneBody.transform.position, targetpositionX, moveSpeed * Time.deltaTime);   
            }
    }

    //ray�� Coil�±װ� ������ �ش� ������ ���� �ڵ�

}
