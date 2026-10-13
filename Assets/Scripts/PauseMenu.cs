using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu, bilgiMenu;
    [SerializeField] RectTransform pausePanelRect, pauseButtonRect, bilgiPanelRect, bilgiButtonRect;
    [SerializeField] private float topPosY, middlePosY, bilgiTopPosY, bilgiMiddlePosY;
    [SerializeField] private float tweenDuration;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] CanvasGroup bilgiCanvasGroup;
    
    AudioManager audioManager;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void PauseMenuAcma()
    {
        pauseMenu.SetActive(true);
        AudioManager.instance.musicSource.Pause();
        Time.timeScale = 0;
        PausePanelIntro();
        
    }

    public void Home()
    {
        GameManager.Instance.LoseFirstAttempt();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        AudioManager.instance.musicSource.UnPause();
    }

    public async void Devam()
    {
        PausePanelOutro();
        AudioManager.instance.musicSource.UnPause();
        await Task.Delay((int)(tweenDuration * 1000));
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        
        

    }

    public void YenidenOyna()
    {
        GameManager.Instance.LoseFirstAttempt();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        AudioManager.instance.musicSource.UnPause();
        
    }

    public void BilgiPanelAcma()
    {

        bilgiMenu.SetActive(true);
        AudioManager.instance.musicSource.Pause();
        Time.timeScale = 0;
        BilgiPanelIntro();
    }

    public async void BilgiPanelKapatma()
    {

        BilgiPanelOutro();
        await Task.Delay((int)(tweenDuration * 1000));
        bilgiMenu.SetActive(false);
        AudioManager.instance.musicSource.UnPause();
        Time.timeScale = 1;
    }

    void PausePanelIntro()
    {
        canvasGroup.DOFade(1, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.OutQuad);

        pausePanelRect.DOAnchorPosY(middlePosY, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.OutBack);

        pauseButtonRect.DOAnchorPosX(210, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.OutQuad);

        bilgiButtonRect.DOAnchorPosX(-210, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.OutQuad);

        bilgiPanelRect.DOAnchorPosY(bilgiMiddlePosY, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.OutBack);
    }

    void PausePanelOutro()
    {
        canvasGroup.DOFade(0, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.InQuad);

        pausePanelRect.DOAnchorPosY(topPosY, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.InBack);

        pauseButtonRect.DOAnchorPosX(-50, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.InQuad);

        bilgiButtonRect.DOAnchorPosX(50, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.InQuad);

        bilgiPanelRect.DOAnchorPosY(bilgiTopPosY, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.InBack);
    }

    void BilgiPanelIntro()
    {
        bilgiCanvasGroup.DOFade(1, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.OutQuad);

        bilgiPanelRect.DOAnchorPosY(bilgiMiddlePosY, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.OutBack);
        pauseButtonRect.DOAnchorPosX(210, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.OutQuad);

        bilgiButtonRect.DOAnchorPosX(-210, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.OutQuad);




    }

    void BilgiPanelOutro()
    {
        bilgiCanvasGroup.DOFade(0, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.InQuad);

        bilgiPanelRect.DOAnchorPosY(bilgiTopPosY, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.InBack);
        pauseButtonRect.DOAnchorPosX(-50, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.InQuad);

        bilgiButtonRect.DOAnchorPosX(50, tweenDuration)
            .SetUpdate(true)
            .SetEase(Ease.InQuad);



    }
}




