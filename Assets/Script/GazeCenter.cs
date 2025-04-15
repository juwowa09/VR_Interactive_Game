using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GazeCenter : MonoBehaviour
{
    public XRRayInteractor rayInteractor; // GazeInteractor를 여기에 드래그
    public LayerMask ignoreLayerMask;
    public float desiredSizeOnScreen = 10f; // 화면에서 원하는 비율 (예: 0.1 = 화면 높이의 10%)
    private Camera cam;


    // void start()
    // {
    //     cam = Camera.main;
    // }
    // private void LateUpdate()
    // {
    //     if (cam == null)
    //     {
    //         return;
    //     }
    //     float distance = Vector3.Distance(transform.position, cam.transform.position);
    //
    //     // 카메라의 수직 FOV 및 화면 높이를 고려하여 오브젝트의 스케일을 결정
    //     // 여기서는 간단히 거리 * 원하는 크기로 계산하지만, 필요시 더 정확한 계산식을 사용할 수 있음.
    //     float scaleFactor = distance * desiredSizeOnScreen;
    //
    //     transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    // }

    private void Update()
    {
        
        if (rayInteractor == null)
        {
            Debug.LogWarning("rayInteractor is null!");
            return;
        }
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if ((ignoreLayerMask.value & (1 << hit.collider.gameObject.layer)) != 0)
            {
                // 이 레이어에 속한 오브젝트라면 reticle(이 오브젝트)을 숨기고, 업데이트 종료
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            transform.position = hit.point;
        }
        else
        {
           // gameObject.SetActive(false); // 닿는 게 없을 때 숨김
        }
    }
}
