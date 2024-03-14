using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CoilCollision : MonoBehaviour

{
    public GameObject coil;
    public GameObject liftRoot;
    public GameObject liftPoint;
    public GameObject CraneManager;
    CraneMove cranemove ;

    void Awake()
    {
        cranemove = CraneManager.GetComponent<CraneMove>();
    }
 
    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Coil" )
        {
            cranemove.StopLift();
            cranemove.LiftStatus = false;
            Debug.Log("충돌");
        }

        if (collision.gameObject.CompareTag("Skid"))
        {
           
            Debug.Log("스키드와 충돌");
            cranemove.StopMovePoint();
            GetComponent<BoxCollider>().enabled = false;
            coil.GetComponent<CapsuleCollider>().enabled = true;
            coil.transform.position = collision.transform.position + Vector3.up*2.5f;
            coil.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // Skid 태그를 가진 오브젝트와의 충돌이 끝났을 때 호출되는 함수
            DisconnectLiftPoint();
            
        }
    }

    IEnumerator MoveCoilAfterDelay()
    {
        coil.GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        
        
        // 움직이고자 하는 로직 추가
        while(true)
         MoveCoilWithLiftRoot();
        {
            yield return null;
        }
        
    }
    
    void MoveCoilWithLiftRoot()
    {
        coil.transform.position = liftPoint.transform.position;
        
    }
    

    void DisconnectLiftPoint()
    {
        // liftPoint와의 연결을 해제하는 로직 추가
        // 예를 들어, liftPoint 변수를 null로 설정하거나 다른 초기화 작업을 수행할 수 있음
        //GameObject skid = GameObject.Find("skid");
        //coil.transform.position = skid.transform.position;
        //liftPoint = null;
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}
