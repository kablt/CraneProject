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
    public float downSpeed = 1f; // ũ���� �������� �ӵ�
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
    CraneStatus cranstatus;
    void Start()
    {
        cranstatus = CraneStatus.Idle;
    }
    void Update()
    {
        switch(cranstatus)
        {
            case CraneStatus.Idle:
                 Idle(); 
                 break;
            case CraneStatus.Detected:
                CraneDetectedCoil();
                break;
            case CraneStatus.CoilMove:
                MovePoint();
                break;
            default: 
                Debug.Log("������ �߻��߽��ϴ�"); 
                break;
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
    void CraneDetectedCoil()
    {
        Vector3 dir = new Vector3(0, -6f, 0); //����(�ϰ�)
        dir = dir.normalized;
        CraneLift.transform.position += dir * downSpeed * Time.deltaTime;
        //����Ʈ�� Ư�� ������ ������ �浹�� ������ ��ġ�� ����Ʈ�� Ư����ġ�� ������Ʈ�ϴ� �Լ������.
        cranstatus = CraneStatus.CoilMove;
    }

    //�������ϋ� ����⿡�� ray���� ������ �ִ��� ��� üũ. ������ PointA�� �̵�
    void Idle()
    {
        // LayShooter�� �����ʺz�� �� �ִ� ��ũ��Ʈ LayShoot�� �����Ѵ�.
        LayShoot objectBShooter = LayShooter.GetComponent<LayShoot>();

        //�ش� ��ü�� null�� �ƴ϶��.(�ش� ������ �ִٸ�)
        if (objectBShooter != null)
        {
            // ��ü���� ShootAndCheckForCoil�Լ��� �����Ѵ�.(�ش� ��ü���� ���̸� ���  Tag�� Coil�ΰ��� �����ϴ� ����׸� ����Լ�)
            objectBShooter.ShootAndCheckForCoil();
            MovementRoutine();
            cranstatus = CraneStatus.Detected;
            //ũ������ ��ġ�� ������ �������� ��ġ�� �ű�� �Լ�
        }
        else
        {
            Debug.LogError("���� �߸��Ǿ��ٴ� ����");
        }
    }

    //���� �浹�� ��ġ�� ����Ʈ�� ������Ʈ ��� ������ ��ǥ�������� �̵��ϴ� �Լ�
    void MovePoint()
    {
        if (!Mathf.Approximately(CraneLift.transform.position.y, LiftRollBack.position.y))
            {
                Vector3 targetPositionY = new Vector3(CraneLift.transform.position.x, LiftRollBack.position.y, CraneLift.transform.position.z);
                CraneLift.transform.position = Vector3.Lerp(CraneLift.transform.position, targetPositionY, moveSpeed * Time.deltaTime);          
            }
        if (Mathf.Abs(CraneLift.transform.position.y - PointB.position.y) < 0.1f)
        {
            Vector3 targetpositionZ = new Vector3(CraneHoist.transform.position.x, CraneHoist.transform.position.y, PointB.position.z);
            CraneHoist.transform.position = Vector3.Lerp(CraneHoist.transform.position, targetpositionZ, moveSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(CraneHoist.transform.position.z - PointB.position.z) < 0.1f)
        {
            Vector3 targetpositionX = new Vector3(PointB.position.x, CraneBody.transform.position.y, CraneBody.transform.position.z);
            CraneBody.transform.position = Vector3.Lerp(CraneBody.transform.position, targetpositionX, moveSpeed * Time.deltaTime);
        }
        //�ش���ġ ������ �� , ������ ��ġ�� ����Ʈ�� Ư����ġ�� ������Ʈ�ǰ��ִ� ��Ȳ���� ����Ʈ�� �������� �ż��� ����. �������µ��� ��Ű���� Ư���κа� �浹�� ������ ��ġ�� ����Ʈ�� Ư�� ��ġ�� ������Ʈ �Ǵ� �Լ� ����. 
        //������ ��ġ�� ��Ű���� Ư����ġ�� �ű�� �Լ� ���� ������ �ش� ��ġ�� �������� ��ó�� ���̰� �����
    }
}
