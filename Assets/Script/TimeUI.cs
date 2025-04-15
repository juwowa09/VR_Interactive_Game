using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class TimeUI : MonoBehaviour
{
    // Start is called before the first frame update
    private XRBaseInteractable interactable;
    private GameManager gameManager;
    private bool flag;
    [SerializeField] public TextMeshProUGUI countdownText;
    
    void Awake()
    {
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
        flag = true;
        gameManager = GameManager.instance;
        if (countdownText == null)
        {
            countdownText = GameObject.Find("CountdownText").GetComponent<TextMeshProUGUI>();
        }
    }
    
    private void OnActivated(ActivateEventArgs args)
    {
        Debug.Log("ss");
    }
    private void OffActivated(DeactivateEventArgs args)
    {
        Debug.Log("ss");
    }

    public void gameResult(bool result, float time)
    {
        if (result)
        {
            countdownText.text = "성공!\nTime: " + time;
        }
        else
        {
            countdownText.text = "실패..\nTime: " + time;
        }

        countdownText.color = Color.white;
        transform.parent.gameObject.SetActive(true);
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
        
        transform.parent.gameObject.SetActive(false);
        GameManager.instance.StartGame();
    }
    
    private void OffSelected(SelectExitEventArgs args)
    {
    }
}
