using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.Android;

public class EndUI : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManager;
    public Display flag;
    [SerializeField] public TextMeshProUGUI countdownText;

    void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
{
    Permission.RequestUserPermission(Permission.ExternalStorageWrite);
}
#endif
    }

    void Start()
    {
        string testPath = System.IO.Path.Combine(Application.persistentDataPath, "test.txt");
        System.IO.File.WriteAllText(testPath, "Hello from Unity!");
        gameManager = GameManager.instance;
    }


    public void gameResult()
    {
        float totalAverage1 = 0f;
        float totalAverage2 = 0f;
        float hardAverage1 = 0f;
        float hardAverage2 = 0f;
        float easyAverage1 = 0f;
        float easyAverage2 = 0f;
        for (int i = 0; i < 6; i++)
        {
            if (i >= 3)
            {
                hardAverage1 += GameManager.instance.TotalTime[1, i];
                hardAverage2 += GameManager.instance.TotalTime[0, i];
            }
            else
            {
                easyAverage1 += GameManager.instance.TotalTime[1, i];
                easyAverage2 += GameManager.instance.TotalTime[0, i];
            }
            totalAverage1 += GameManager.instance.TotalTime[1, i];
            totalAverage2 += GameManager.instance.TotalTime[0, i];
        }

        totalAverage1 /= 6;
        totalAverage2 /= 6;
        
        countdownText.text += "<b><color=red>Total</color></b>\n<color=blue>컨트롤러</color> 기반 정답: " + GameManager.instance.firstSuccess + "/6"+"\n";
        countdownText.text += "<color=blue>컨트롤러</color> 기반 평균 게임시간: " + totalAverage1+"\n";
        countdownText.text += "<color=green>머리</color> 기반 정답: " + GameManager.instance.secondSuccess + "/6"+"\n";
        countdownText.text += "<color=green>머리</color> 기반 평균 게임시간: " + totalAverage2+"\n\n<b><color=red>쉬움</color></b>(적은 그림)\n";
        
        countdownText.text += "<color=blue>컨트롤러</color> 기반 정답: " + GameManager.instance.easysuccess1 + "/3"+"\n";
        countdownText.text += "<color=blue>컨트롤러</color> 기반 평균 게임시간: " + easyAverage1/3 +"\n";
        countdownText.text += "<color=green>머리</color> 기반 정답: " + GameManager.instance.easysuccess2 + "/3"+"\n";
        countdownText.text += "<color=green>머리</color> 기반 평균 게임시간: " + easyAverage2/3 +"\n\n<b><color=red>어려움</color></b>(많은 그림)\n";
        
        countdownText.text += "<color=blue>컨트롤러</color> 기반 정답: " + GameManager.instance.hardsuccess1 + "/3"+"\n";
        countdownText.text += "<color=blue>컨트롤러</color> 기반 평균 게임시간: " + hardAverage1/3 +"\n";
        countdownText.text += "<color=green>머리</color> 기반 정답: " + GameManager.instance.hardsuccess2 + "/3"+"\n";
        countdownText.text += "<color=green>머리</color> 기반 평균 게임시간: " + hardAverage2/3 +"\n\n\n";
        
        
        countdownText.color = Color.white;
        Debug.Log("gogo");
        gameObject.SetActive(true);
        StartCoroutine(CaptureVRScreen($"Screenshot_{System.DateTime.Now:yyyyMMdd_HHmmss}.png",Camera.main));
        // flag.gameObject.SetActive(true);
    }
    public IEnumerator CaptureVRScreen(string fileName, Camera captureCam)
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;

        RenderTexture rt = new RenderTexture(width, height, 24);
        captureCam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);

        captureCam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        captureCam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        string path = System.IO.Path.Combine(Application.persistentDataPath, fileName);
        System.IO.File.WriteAllBytes(path, bytes);

        Debug.Log("Screenshot saved to: " + path);

#if UNITY_ANDROID && !UNITY_EDITOR
    using (AndroidJavaClass mediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection"))
    using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
             .GetStatic<AndroidJavaObject>("currentActivity"))
    {
        mediaScannerConnection.CallStatic("scanFile", activity, new string[] { path }, null, null);
    }
#endif
    }
}
