using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject SeviyelerMenu;
    public GameObject AyarlarMenu;
    public GameObject YapimcilarMenu;
    public GameObject BasarimlarMenu;
    public GameObject CikisMenu;

    [SerializeField] RectTransform SeviyelerAnaPanelRect, AyarlarAnaPanelRect, YapimcilarAnaPanelRect, BasarimlarAnaPanelRect, CikisAnaPanelRect;
    [SerializeField] private float topPosY, middlePosY;
    [SerializeField] private float tweenDuration;
    [SerializeField] CanvasGroup seviyelerBackgroundCanvas, ayarlarBackgroundCanvas, yapimcilarBackgroundCanvas, basarimlarBackgroundCanvas, cikisBackgroundCanvas;
    [SerializeField] private GameObject[] levelButtons;
    
   

    

    private void Start()
    {
        InitiliazeLevelUI();
    }
    public void InitiliazeLevelUI(bool hardReset = false)
    {
        int levelProgress = PlayerPrefs.GetInt("level", 0);
        levelProgress--;
        levelProgress = Mathf.Max(levelProgress, 0);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i <= levelProgress)
            {
                if (levelButtons[i].transform.childCount > 1)
                    levelButtons[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                levelButtons[i].GetComponent<Button>().enabled = false;
                levelButtons[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    public void SeviyelerButton()
    {

        ShowPanel(SeviyelerMenu, SeviyelerAnaPanelRect, seviyelerBackgroundCanvas);
    }

    public void AyarlarButton()
    {

        ShowPanel(AyarlarMenu, AyarlarAnaPanelRect, ayarlarBackgroundCanvas);
    }

    public void YapimcilarButton()
    {

        ShowPanel(YapimcilarMenu, YapimcilarAnaPanelRect, yapimcilarBackgroundCanvas);
    }

    public void BasarimlarButton()
    {
        ShowPanel(BasarimlarMenu, BasarimlarAnaPanelRect, basarimlarBackgroundCanvas);
        AchievementManager.Instance.PopulateAchievementsMenu();
    }

    public void CikisSoruButton()
    {

        ShowPanel(CikisMenu, CikisAnaPanelRect, cikisBackgroundCanvas);
    }

    public async void SeviyeGeriButton()
    {

        await HidePanel(SeviyelerAnaPanelRect, seviyelerBackgroundCanvas);
        SeviyelerMenu.SetActive(false);
      
    }

    public async void AyarlarGeriButton()
    {

        await HidePanel(AyarlarAnaPanelRect, ayarlarBackgroundCanvas);
        AyarlarMenu.SetActive(false);
        
    }

    public async void YapimcilarGeriButton()
    {
        await HidePanel(YapimcilarAnaPanelRect, yapimcilarBackgroundCanvas);
        YapimcilarMenu.SetActive(false);
      
    }

    public async void BasarimlarGeriButton()
    {

        await HidePanel(BasarimlarAnaPanelRect, basarimlarBackgroundCanvas);
        BasarimlarMenu.SetActive(false);
       
    }

    public async void CikisHayirButton()
    {

        await HidePanel(CikisAnaPanelRect, cikisBackgroundCanvas);
        CikisMenu.SetActive(false);
        
    }

    public void CikisEvetButton()
    {

        Application.Quit();
    }

    public void LevelBasla(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
        Time.timeScale = 1;
    }

    private void ShowPanel(GameObject menu, RectTransform panelRect, CanvasGroup canvasGroup)
    {
        menu.SetActive(true);
        canvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        panelRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    private async Task HidePanel(RectTransform panelRect, CanvasGroup canvasGroup)
    {

        canvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        await panelRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("level");
        PlayerPrefs.DeleteKey("ConsecutivePerfectLevels");
        for (int i = 1; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
        {
            PlayerPrefs.DeleteKey("Level_" + i + "_Attempted");
        }
        if (AchievementManager.Instance != null)
        {
            AchievementManager.Instance.ResetAllAchievements();
        }
        SeviyeGeriButton();
        InitiliazeLevelUI(true);
    }
}

