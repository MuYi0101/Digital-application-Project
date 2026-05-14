using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class PlayIntroVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "MainMenu";
    public Image fadeImage;

    public float fadeDuration = 1.5f;
    public float delayAfterFade = 0.3f;

    private bool isSkipping = false; // ✅ 防止重複觸發

    void Start()
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void Update()
    {
        // ✅ 按 E 跳過影片
        if (Input.GetKeyDown(KeyCode.E) && !isSkipping)
        {
            isSkipping = true;

            videoPlayer.Stop(); // 停止影片
            OnVideoFinished(videoPlayer); // 直接走結束流程
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (fadeImage != null)
        {
            fadeImage.DOFade(1f, fadeDuration).OnComplete(() =>
            {
                StartCoroutine(DelayedLoadScene());
            });
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSeconds(delayAfterFade);
        SceneManager.LoadScene(nextSceneName);
    }
}