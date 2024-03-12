using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraneMove : MonoBehaviour
{
    public Transform PointA; //코일이 있는 위치
    public Transform PointB; //코일을 갖다 놓을 위치
    public Transform LiftRollBack; // 리프트가 대기할떄의 y축 위치
    public float moveSpeed = 1f; // 크레인이 움직이는 속도
    public float downSpeed = 1f; // 크레인 내려가는 속도
    public GameObject CraneBody; // 움직일 크레인 body
    public GameObject CraneHoist; // 움직일 크레인 hosit
    public GameObject CraneLift; // 움직일 크레인 lift
    public GameObject LayShooter; // ray를 쏘는 객체
    public LayerMask CoilLayer; // 충돌할 레이어 변수;
    private bool Body = false; // 몸체 움직일떄 다른거 움직임 x
    private bool Hoist = false;// 호스팃 움직일때 다른거 움직임 x

    enum CraneStatus
    {
        Idle, // 리프트가 위로 올라와져있는 상태
        APoint, // 코일을 줍기위해 트럭이 들어오는 위치로 이동
        Detected, // 리프트를 내려서 코일을 집는 함수
        CoilMove, // Coil을 적절한 위치에 옮기는 함수
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
                Debug.Log("오류가 발생했습니다"); 
                break;
        }
    }


    //코일을 집기위해 APoint로 옮기는 함수
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

    //ray에 Coil태그가 있을때 해당 코일을 집는 코드
    void CraneDetectedCoil()
    {
        Vector3 dir = new Vector3(0, -6f, 0); //방향(하강)
        dir = dir.normalized;
        CraneLift.transform.position += dir * downSpeed * Time.deltaTime;
        //리프트의 특정 지점과 코일이 충돌시 코일의 위치를 리프트의 특정위치로 업데이트하는 함수만들기.
        cranstatus = CraneStatus.CoilMove;
    }

    //대기상태일떄 막대기에서 ray쏴서 코일이 있는지 상시 체크. 있으면 PointA로 이동
    void Idle()
    {
        // LayShooter의 컴포너틑에 들어가 있는 스크립트 LayShoot에 접근한다.
        LayShoot objectBShooter = LayShooter.GetComponent<LayShoot>();

        //해당 객체가 null이 아니라면.(해당 객제가 있다면)
        if (objectBShooter != null)
        {
            // 객체안의 ShootAndCheckForCoil함수를 실행한다.(해당 객체에서 레이를 쏘고  Tag가 Coil인것을 감지하는 디버그를 찍는함수)
            objectBShooter.ShootAndCheckForCoil();
            MovementRoutine();
            cranstatus = CraneStatus.Detected;
            //크레인의 위치를 코일의 집기위한 위치로 옮기는 함수
        }
        else
        {
            Debug.LogError("무언가 잘못되었다는 오류");
        }
    }

    //코일 충돌후 위치가 리프트로 업데이트 디고 있을떄 목표지점으로 이동하는 함수
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
        //해당위치 도착한 후 , 코일의 위치가 리프트의 특정위치로 업데이트되고있는 상황에서 리프트가 내려가는 매서드 실행. 내려가는동안 스키드의 특정부분과 충돌시 코일의 위치가 리프트의 특정 위치로 업데이트 되는 함수 종료. 
        //코일의 위치를 스키드의 특정위치로 옮기는 함수 만들어서 코일이 해당 위치에 놓여지는 것처럼 보이게 만들기
    }
}
