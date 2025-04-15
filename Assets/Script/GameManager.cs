using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public int firstSuccess = 0;
    public int secondSuccess = 0;

    public float[,] TotalTime = new float [2,6];
    
    public int hardsuccess1 = 0;
    public int hardsuccess2 = 0;
    public int easysuccess1 = 0;
    public int easysuccess2 = 0;
    public static GameManager instance
    {
        get;
        set;
    }
    
    // public CubeCreater cubeCreater;

    public bool cubeflag;
    
    // random order of Picture
    private int[] randomFolder = {1,2,3,4,5,6};
    public CubeCreater cubeCreater;
   
    // Gamecount
    public int gameCount
    {
        get;
        private set;
    }
    public Canvas startUI;
    public EndUI endUI;
    public Canvas levelUI;
    public Canvas gazeUI;
    public Canvas controllerUI;
    
    // interaction genre
    private bool toggle;

    public bool game_level
    {
        get;
        private set;
    }

    public int num
    {
        get;
        private set;
    }
    public XRGazeInteractor gazeInteractor;
    public XRRayInteractor leftInteractor;
    public XRRayInteractor rightInteractor;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    void Start()
    {
        XRSettings.eyeTextureResolutionScale = 1.5f;
        game_level = false;
        toggle = false;
        gameCount = 0;
        num = Random.Range(0, 2);
        
        if (num == 1)
        {
            gazeInteractor.gameObject.SetActive(false);
            gazeInteractor.enabled = false;
        }
        else
        {
            gazeInteractor.gameObject.SetActive(true);
            leftInteractor.enabled = false;
            rightInteractor.enabled = false;
        }
        
        startUI.gameObject.SetActive(false);
        levelUI.gameObject.SetActive(false);
        controllerUI.gameObject.SetActive(false);
        gazeUI.gameObject.SetActive(false);
        // 랜덤 셔플 인덱스
        for (int i = 0; i < randomFolder.Length; i++)
        {
            int randIndex = Random.Range(i, randomFolder.Length);
            (randomFolder[i], randomFolder[randIndex]) = (randomFolder[randIndex], randomFolder[i]);
            Debug.Log(randomFolder[i]);
        }
        
        cubeCreater.setting();
        cubeCreater.gameObject.SetActive(false);
        // Start 대신 수동 실행
        StartGame();
    }
    
    public void resetGame()
    {
        cubeCreater.randomFolder = randomFolder[gameCount % 6];
        cubeCreater.Initial();
        cubeCreater.enableQuad();
        cubeCreater.gameObject.SetActive(true);
        gameCount++;
    }

    public void StartGame()
    {
        if (gameCount == 0)
        {
            if (num == 1)
            {
                controllerUI.gameObject.SetActive(true);
            }
            else
            {
                gazeUI.gameObject.SetActive(true);
            }
        }
        else if ((gameCount % 3) == 0 && (gameCount % 6) != 0)
        {
            game_level = true;
            cubeCreater.delete();
            cubeCreater.setting();
            levelUI.gameObject.SetActive(true);
        }
        else if (!toggle && gameCount == 6)
        {
            game_level = false;
            
            cubeCreater.delete();
            cubeCreater.setting();
            toggle = true;
            if (num == 1)
            {
                gazeInteractor.gameObject.SetActive(true);
                gazeInteractor.enabled = true;
                leftInteractor.enabled = false;
                rightInteractor.enabled = false;
                gazeUI.gameObject.SetActive(true);
                num = 0;
            }
            else
            {
                gazeInteractor.gameObject.SetActive(false);
                gazeInteractor.enabled = false;
                leftInteractor.enabled = true;
                rightInteractor.enabled = true;
                controllerUI.gameObject.SetActive(true);
                num = 1;
            }
        }
        else if (gameCount >= 12)
        {
            // game End
            Debug.Log("end");
            endUI.gameResult();
        }
        else
        {
            Debug.Log("start");
            startUI.gameObject.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
