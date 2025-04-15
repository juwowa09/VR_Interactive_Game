using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Middle : MonoBehaviour
{
    // Start is called before the first frame update
    private XRBaseInteractable interactable;
    public Canvas uiSelect;
    private bool flag;
    
    void Awake()
    {
        flag = true;
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

    void Start()
    {
    }
    
    private void OnActivated(ActivateEventArgs args)
    {
        Debug.Log("ss");
    }
    private void OffActivated(DeactivateEventArgs args)
    {
        Debug.Log("ss");
    }
    
    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        flag = false;
    }
    private void OnEnable()
    {
        StartCoroutine(wait());
    }
    private void OnSelected(SelectEnterEventArgs args)
    {
        if (flag)
            return;
        flag = true;
        Debug.Log("what?");
        uiSelect.gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }
    private void OffSelected(SelectExitEventArgs args)
    {
        Debug.Log("off");
    }
}
