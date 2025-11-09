using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public GameObject medicPrefab;  // 프리팹 연결 (Inspector에서 지정)
    



    // Medic을 게이트 위치에서 소환해서 환자에게 보냄
    public void SpawnMedic(Transform patient)
    {
        if (medicPrefab == null || patient == null)
        {
            Debug.LogWarning("Medic prefab 또는 patient가 없습니다!");
            return;
        }

        // 게이트 위치에 Medic 생성
        GameObject medicObj = Instantiate(medicPrefab, transform.position, Quaternion.identity);
        

        // MedicController에 게이트와 환자 정보 전달
        MedicController medic = medicObj.GetComponent<MedicController>();
        if (medic != null)
        {
            medic.Init(transform, patient);
            Debug.Log("소환 완료");
        }
    }
}
