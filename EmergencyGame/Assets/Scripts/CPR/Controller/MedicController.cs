using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicController : MonoBehaviour
{
    private Transform tr;
    private Rigidbody2D rb;

    public float speed = 2f;
    private Transform gateTr;         // 게이트 Transform
    private Transform patientTr;      // 목표 환자 Transform
    private bool isCarryingPatient = false; // 환자 태운 상태
    private bool returningToGate = false;   // 게이트로 복귀 중
    private bool movingToPatient = true;    // 환자에게 이동 중인지

    public void Init(Transform gate, Transform Patient)
    {
        gateTr = gate;
        patientTr = Patient;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = transform;
    }

    void Update()
    {

        if (movingToPatient && patientTr != null && !isCarryingPatient)
        {
            // 환자 방향으로 이동
            
            
            Vector2 dir = (patientTr.position - tr.position).normalized;
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
            

            // 환자 근처 도착 시 태우기
            float distance = Vector2.Distance(tr.position, patientTr.position);
            if (distance < 0.3f)
            {
                PickUpPatient();
            }
        }
        else if (isCarryingPatient && returningToGate)
        {
            // 게이트로 이동
            Vector2 dir = (gateTr.position - tr.position).normalized;
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);

            float distance = Vector2.Distance(tr.position, gateTr.position);
            if (distance < 0.3f)
            {
                // 게이트 도착 → 환자 제거 + Medic 제거
                if (patientTr != null) Destroy(patientTr.gameObject);
                Destroy(gameObject);
            }
        }
    }

    void PickUpPatient()
    {
        if (patientTr == null) return;

        // 환자 태우기
        patientTr.position = tr.position;
        patientTr.SetParent(tr);

        // patientController 스크립트 멈추기
        var patientCtrl = patientTr.GetComponent<patientController>();
        if (patientCtrl != null)
        {
            patientCtrl.isCarried = true;
            var patientRb = patientTr.GetComponent<Rigidbody2D>();
            if (patientRb != null) patientRb.simulated = false; // 물리 정지도 가능
        }

        isCarryingPatient = true;
        movingToPatient = false;

        // 일정 시간 후 게이트로 복귀
        StartCoroutine(ReturnToGate());
    }


    IEnumerator ReturnToGate()
    {
        yield return new WaitForSeconds(1f); // 1초 대기 (태우는 연출)
        returningToGate = true;
    }
}
