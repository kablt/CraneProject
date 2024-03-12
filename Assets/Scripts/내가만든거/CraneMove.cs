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
    void Update()
    {
        UseObjectBRay();
    }

    //여러행동이 주가 되었을떄 업데이트문에 switch문을 통해 상황에 맞게 함수가 발동하게 추가 작성해야할것같다.


    //레이를 쏘는 오브젝트를 지정하고 해당 ray에 코일태그를 가진 오브젝트가 있는지 검사 한다.
    void UseObjectBRay()
    {
        // LayShooter의 컴포너틑에 들어가 있는 스크립트 LayShoot에 접근한다.
        LayShoot objectBShooter = LayShooter.GetComponent<LayShoot>();

        //해당 객체가 null이 아니라면.(해당 객제가 있다면)
        if (objectBShooter != null)
        {
            // 객체안의 ShootAndCheckForCoil함수를 실행한다.(해당 객체에서 레이를 쏘고  Tag가 Coil인것을 감지하는 디버그를 찍는함수)
            objectBShooter.ShootAndCheckForCoil();
            //크레인의 위치를 코일의 집기위한 위치로 옮기는 함수
            MovementRoutine();
        }
        else
        {
            Debug.LogError("무언가 잘못되었다는 오류");
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

}
