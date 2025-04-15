using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CubeSelect : MonoBehaviour
{
    // Start is called before the first frame update
    private Renderer rend;
    private XRBaseInteractable interactable;
    public CubeCreater cc;
    
    void Awake()
    {
        cc = FindObjectOfType<CubeCreater>();
        rend = GetComponent<Renderer>();
        interactable = GetComponent<XRBaseInteractable>();
        if(interactable == null)
            Debug.Log("no");
    
        // interactable.activated.AddListener(OnActivated);
        // interactable.deactivated.AddListener(OffActivated);
        interactable.selectEntered.RemoveAllListeners();
        interactable.selectExited.RemoveAllListeners();
        
        interactable.selectEntered.AddListener(OnSelected);
        interactable.selectExited.AddListener(OffSelected);
    }
    
    private void OnActivated(ActivateEventArgs args)
    {
        Debug.Log("ss");
        rend.material.color = Color.green;  // 선택 시 색상 변경
    }
    private void OffActivated(DeactivateEventArgs args)
    {
        Debug.Log("ss");
        rend.material.color = Color.red;  // 선택 시 색상 변경
    }

    private void OnEnable()
    {
        
    }
    private void OnSelected(SelectEnterEventArgs args)
    {
        cc.disableQuad();
        
        Debug.Log($"OnSelected 호출됨 by {args.interactorObject.transform.name} on {gameObject.name}");
        
        if (rend.material.mainTexture == cc.AnswerMaterial)
        {
            cc.success();
        }
        else
        {
            cc.fail();
        }
    }
    private void OffSelected(SelectExitEventArgs args)
    {
    }
}
