using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LineController[] laserControllers;
    private bool isWinLogged = false;
    [SerializeField] private int rightToMove = 4;
    private int leftRightToMove;
    [HideInInspector] public bool MirrorInteraction;
    public static GameManager Instance;

    [SerializeField] private Text rightToMoveText;
    private float levelStartTime;
    private bool isFirstAttempt = true;
    private int mirrorsUsed = 0;
    private HashSet<string> usedLightSources = new HashSet<string>();
    private int consecutivePerfectLevels = 0; 
    
    [Header("-----UI Panels-----")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private CanvasGroup losePanelGroup;
    [SerializeField] private CanvasGroup backgroundPanelGroup;
    
    [SerializeField] private GameObject[] countdownSprites; 
    [SerializeField] private float countdownInterval = 0.5f;
    AudioManager audioManager;
    
    private IEnumerator HandleLoseSequence()
    {
        
        foreach (var go in countdownSprites)
            go.SetActive(false);

        yield return new WaitForSecondsRealtime(0.5f); 

        foreach (var spriteGO in countdownSprites)
        {
            spriteGO.SetActive(true);
            yield return new WaitForSecondsRealtime(countdownInterval);
            spriteGO.SetActive(false);
        }

        ShowLosePanel(); 
        audioManager.PlaySFX(audioManager.gameOver);
        AudioManager.instance.musicSource.Pause();
    }
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        Instance = this;
    }
    private void Start()
    {
        
        leftRightToMove = rightToMove;
        UpdateRightToMoveUI();
        levelStartTime = Time.time;
        isFirstAttempt = PlayerPrefs.GetInt("Level_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + "_Attempted", 0) == 0;
        mirrorsUsed = 0;
        usedLightSources.Clear();
        consecutivePerfectLevels = PlayerPrefs.GetInt("ConsecutivePerfectLevels", 0); 
        
        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        if (backgroundPanelGroup != null)
        {
            backgroundPanelGroup.alpha = 0;
            backgroundPanelGroup.interactable = false;
            backgroundPanelGroup.blocksRaycasts = false;
        }

        if (losePanelGroup != null)
        {
            losePanelGroup.alpha = 0;
            losePanelGroup.interactable = false;
            losePanelGroup.blocksRaycasts = false;
            losePanelGroup.transform.localScale = Vector3.zero;
        }
    }
    
    void Update()
    {
        if (AllLasersSuccessful() && !isWinLogged)
        {
            Debug.Log("OLEY! Tüm lazerler tamamlandı!");
            isWinLogged = true;

            
            CheckAchievements();

            if (LevelTransitionManager.Instance != null)
            {
                LevelTransitionManager.Instance.PlayLightCoreAndLoadNext();
            }
            else
            {
                Debug.LogError("LevelTransitionManager sahnede yok!");
            }
        }
    }

    private void CheckAchievements()
    {
        
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
        {
            AchievementManager.Instance.UnlockAchievement("first_spark");
        }

        
        if (mirrorsUsed >= 3)
        {
            AchievementManager.Instance.UnlockAchievement("master_reflection");
        }

        
        if (isFirstAttempt) 
        {
            consecutivePerfectLevels++;
            PlayerPrefs.SetInt("ConsecutivePerfectLevels", consecutivePerfectLevels);

            if (consecutivePerfectLevels >= 5)
            {
                AchievementManager.Instance.UnlockAchievement("flawless_victory");
            }
        }
        else
        {
            
            consecutivePerfectLevels = 0;
            PlayerPrefs.SetInt("ConsecutivePerfectLevels", 0);
        }

        
        if (isFirstAttempt)
        {
            AchievementManager.Instance.UnlockAchievement("mind_spark");
        }

        
        if (usedLightSources.Count >= 3)
        {
            AchievementManager.Instance.UnlockAchievement("color_commander");
        }

        
        if (Time.time - levelStartTime <= 20f)
        {
            AchievementManager.Instance.UnlockAchievement("quick_thinker");
        }

        
        PlayerPrefs.SetInt("Level_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + "_Attempted", 1);
        PlayerPrefs.Save();
    }

    public void RegisterMirrorUse()
    {
        mirrorsUsed++;
    }

    public void RegisterLightSource(string lightSourceId)
    {
        usedLightSources.Add(lightSourceId);
    }

    private bool AllLasersSuccessful()
    {
        foreach (var controller in laserControllers)
        {
            var beam = controller.currentLaserBeam;
            if (beam == null || !beam.isCheckpointHit || !beam.isTargetHit)
                return false;
        }
        return true;
    }
    public bool CheckRightToMove()
    {
        return leftRightToMove > 0;
    }
    public void DecreaseRightToMove()
    {
        leftRightToMove--;
        
        UpdateRightToMoveUI();

        if (leftRightToMove <= 0)
        {
            StartCoroutine(HandleLoseSequence());
        }
    }
    
    private void ShowLosePanel()
    {
        if (losePanel != null)
            losePanel.SetActive(true);
        
        
        if (backgroundPanelGroup != null && backgroundPanelGroup.transform.parent != null)
        {
            backgroundPanelGroup.transform.parent.SetAsLastSibling();
        }

        if (losePanelGroup != null && losePanelGroup.transform.parent != null)
        {
            losePanelGroup.transform.parent.SetAsLastSibling();
        }

        if (backgroundPanelGroup != null)
        {
            LeanTween.alphaCanvas(backgroundPanelGroup, 1f, 0.5f).setIgnoreTimeScale(true);
            backgroundPanelGroup.interactable = true;
            backgroundPanelGroup.blocksRaycasts = true;
        }

        if (losePanelGroup != null)
        {
            LeanTween.alphaCanvas(losePanelGroup, 1f, 0.5f).setIgnoreTimeScale(true);
            LeanTween.scale(losePanelGroup.gameObject, Vector3.one, 0.5f)
                .setEaseOutBack()
                .setIgnoreTimeScale(true);
            losePanelGroup.interactable = true;
            losePanelGroup.blocksRaycasts = true;
        }

        Time.timeScale = 0f;
    }
    public void ResetOtherLasers(LineController lineController)
    {
        for (int i = 0; i < laserControllers.Length; i++)
        {
            if (laserControllers[i] != lineController)
            {
                laserControllers[i].DrawLaserAgain();
            }
        }
    }

    public void LoseFirstAttempt()
    {
        isFirstAttempt = false;
        consecutivePerfectLevels = 0;
        PlayerPrefs.SetInt("ConsecutivePerfectLevels", 0);
    }
    private void UpdateRightToMoveUI()
    {
        if (rightToMoveText != null)
        {
            rightToMoveText.text = "Moves Left: " + leftRightToMove;
        }
    }
}