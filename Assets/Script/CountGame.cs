using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TMPro;

public class CountGame : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setResult(string str)
    {
        text.text = str + "\n";
        text.text += GameManager.instance.TotalTime[GameManager.instance.num, GameManager.instance.gameCount % 6];
        text.text += "\n" + GameManager.instance.firstSuccess + "\n";
        text.text += GameManager.instance.secondSuccess;
    }
}
