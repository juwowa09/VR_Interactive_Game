using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class CubeCreater : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]public GameObject cubePrefab; // Cube 프리팹 할당
    [SerializeField]public Transform trans;
    [SerializeField]public int randomFolder = -1;  // 기본값 1
    
    private int rows = 7;
    private int columns = 8;
    private float spacing = 0.4f;
    
    public RectTransform fillImage;
    private float initialWidth;

    public float curTime
    {
        get;
        private set;
    }

    public float maxTime
    {
        get;
        private set;
    } = 10.0f;

    public GameObject[] cube
    {
        get;
        private set;
    }

    private GameManager _gameManager;

    public CountGame ui;
    public TimeUI timeUi;

    public Texture AnswerMaterial
    {
        get;
        private set;
    }

    public Texture DefaultMaterial
    {
        get;
        private set;
    }
    void Start()
    {
        initialWidth = fillImage.sizeDelta.x;
    }

    public void delete()
    {
        if (cube != null)
        {
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    int index = y * columns + x;
                    if (cube[index] != null)
                    {
                        Destroy(cube[index]);
                    }
                }
            }
        }
        cube = null;
    }
    
    public void setting()
    {
        if (GameManager.instance.game_level)
        {
            rows = 8;
            columns = 12;
        }
        else
        {
            rows = 7;
            columns = 8;
        }
        
        cube = new GameObject[rows*columns];
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                int index = y * columns + x;
                Vector3 position = new Vector3(
                    x * spacing - (columns % 2 == 1 ? (columns/2 * spacing) : (columns/2 * (float)0.9 * spacing)),
                    y * spacing + trans.transform.position.y + (rows/2 * (float)0.10 * spacing),
                    // + (columns/2 * spacing),
                    trans.transform.position.z + 4
                );

                cube[index] = Instantiate(cubePrefab, position, Quaternion.identity, transform);
                
                cube[index].transform.localScale = cube[index].transform.localScale * (float)0.3;
                cube[index].name = $"Cube_{x}_{y}";
            }
        }
    }

    public void Initial()
    {
        curTime = 0.0f;
        string folderPath = $"Materials/{randomFolder}";

        // 폴더 내 머티리얼 불러오기
        AnswerMaterial = Resources.Load<Texture>($"{folderPath}/Answer");
        DefaultMaterial = Resources.Load<Texture>($"{folderPath}/Default");

        int randomNumber = Random.Range(0, rows * columns);  // 0 이상 10 미만 (즉, 0~9 중 하나)
        
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                int index = y * columns + x;
                if(index == randomNumber)
                {
                    MeshRenderer Quad = cube[index].GetComponent<MeshRenderer>();
                    Quad.material.mainTexture = AnswerMaterial;
                    Debug.Log(randomNumber);
                }
                else
                {
                    MeshRenderer Quad = cube[index].GetComponent<MeshRenderer>();
                    Quad.material.mainTexture = DefaultMaterial;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        float ratio = Mathf.Clamp01(1f - curTime / maxTime);
        fillImage.sizeDelta = new Vector2(initialWidth * ratio, fillImage.sizeDelta.y);
        if (curTime >= maxTime)
        {
            fail();
        }
    }
    public void fail()
    {
        if (GameManager.instance.cubeflag)
            return;
        GameManager.instance.cubeflag = true;
        Debug.Log("fail");
        if (GameManager.instance.num == 1)
        {
            GameManager.instance.TotalTime[1, (GameManager.instance.gameCount-1) % 6] = curTime;
        }
        else
        {
            GameManager.instance.TotalTime[0, (GameManager.instance.gameCount-1) % 6] = curTime;
        }
        // ui.setResult("fail");
        gameObject.SetActive(false);
        timeUi.gameResult(false,curTime);
    }

    public void success()
    {
        if (GameManager.instance.cubeflag)
            return;
        GameManager.instance.cubeflag = true;
        Debug.Log("success");
        if (GameManager.instance.num == 1)
        {
            if (((GameManager.instance.gameCount-1) % 6) < 3)
                GameManager.instance.easysuccess1++;
            else
                GameManager.instance.hardsuccess1++;
            GameManager.instance.TotalTime[1, (GameManager.instance.gameCount-1) % 6] = curTime;
            GameManager.instance.firstSuccess++;
        }
        else
        {
            if (((GameManager.instance.gameCount -1)% 6)  < 3)
                GameManager.instance.easysuccess2++;
            else
                GameManager.instance.hardsuccess2++;
            GameManager.instance.TotalTime[0, (GameManager.instance.gameCount-1) % 6] = curTime;
            GameManager.instance.secondSuccess += 1;
        }
        // ui.setResult("success");
        gameObject.SetActive(false);
        timeUi.gameResult(true,curTime);
    }

    private void OnEnable()
    {
        curTime = 0;
        GameManager.instance.cubeflag = false;
    }

    public void enableQuad()
    {
        foreach (var quad in cube)
        {
            var interactable = quad.GetComponent<XRBaseInteractable>();
            if (interactable != null)
            {
                interactable.enabled = true;
            }
        }
    }
    public void disableQuad()
    {
        foreach (var quad in cube)
        {
            var interactable = quad.GetComponent<XRBaseInteractable>();
            if (interactable != null)
            {
                interactable.enabled = true;
            }
        }
    }
}
