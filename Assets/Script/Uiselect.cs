using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class Uiselect : MonoBehaviour
{
    // Start is called before the first frame update
    private XRBaseInteractable interactable;
    private GameManager gameManager;
    private bool flag;
    [SerializeField] private TextMeshProUGUI countdownText;
    
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
    
    private IEnumerator HandleSelection()
    {
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            countdownText.fontSize = 0.5f;
            countdownText.color = Color.white;
            // 부드러운 커지는 애니메이션
            float elapsed = 0f;
            float duration = 0.3f;
            

            yield return new WaitForSeconds(1f); // 총 3초 동안 머무르게
        }
        countdownText.gameObject.SetActive(false);
        // 다음 단계
        gameManager.resetGame();
        transform.parent.gameObject.SetActive(false);
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
        // 3초 기다림 + 숫자 연출 추가
        StartCoroutine(HandleSelection());
    }
    
    private void OffSelected(SelectExitEventArgs args)
    {
    }
}
