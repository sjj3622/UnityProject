using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientGate : MonoBehaviour
{
    public Transform[] PatientGates;  // 소환 가능한 위치 배열
    public GameObject Patientfab;     // 소환할 환자 프리팹

    void Start()
    {
        SpawnPatient();
    }

    void SpawnPatient()
    {
        if (PatientGates.Length == 0 || Patientfab == null)
        {
            Debug.LogWarning("PatientGates가 비었거나 Patientfab이 할당되지 않았습니다!");
            return;
        }

        // 0~PatientGates.Length-1 사이의 랜덤 인덱스 선택
        int randomIndex = Random.Range(0, PatientGates.Length);

        // 선택한 위치에 환자 프리팹 소환
        Instantiate(Patientfab, PatientGates[randomIndex].position, Quaternion.identity);

        Debug.Log("Patient spawned at gate: " + randomIndex);
    }
}
