using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.Android;

public class Display : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManager;
    public Canvas flag;
    [SerializeField] public TextMeshProUGUI countdownText;

    void Awake()
    {
    }

    void Start()
    {
        gameManager = GameManager.instance;
        countdownText.text = "";
    }

    public void display(string path)
    {
        countdownText.text += path;
        gameObject.SetActive(true);
    }
}
