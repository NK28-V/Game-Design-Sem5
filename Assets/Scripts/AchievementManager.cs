using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }
    AudioManager audioManager;

    [Serializable]
    public class Achievement
    {
        public string id;
        public string title;
        public string description;
        public bool isUnlocked;

    }

    public List<Achievement> achievements = new List<Achievement>();
    public GameObject achievementPanelPrefab;
    public Transform achievementPanelParent;
    public GameObject achievementListItemPrefab;
    public Transform achievementsListParent;

    private Queue<Achievement> achievementQueue = new Queue<Achievement>();
    private bool isShowingAchievement = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAchievements();
            LoadAchievements();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        Debug.Log("Sahne yüklendi, parent'lar aranıyor...");

        // Sadece ana menü sahnesindeyken parent'ı bul
        var parentObj = GameObject.Find("AchievementsListParent");
        if (parentObj != null)
        {
            achievementsListParent = parentObj.transform;
            Debug.Log("AchievementsListParent bulundu");
        }
        else
        {
            Debug.LogWarning("AchievementsListParent bulunamadı!");
        }

        // Başarım bildirimi için parent'ı bul
        var notificationParent = GameObject.Find("AchievementNotificationParent");
        if (notificationParent != null)
        {
            achievementPanelParent = notificationParent.transform;
            Debug.Log("AchievementNotificationParent bulundu");
        }
        else
        {
            Debug.LogWarning("AchievementNotificationParent bulunamadı, oluşturuluyor...");

            // Canvas'ı bul veya oluştur
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogWarning("Canvas bulunamadı, yeni bir Canvas oluşturuluyor...");
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
            }

            // Notification parent'ı oluştur
            GameObject notificationParentObj = new GameObject("AchievementNotificationParent");
            notificationParentObj.transform.SetParent(canvas.transform, false);

            // RectTransform ayarlarını yap
            RectTransform rectTransform = notificationParentObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 1f);
            rectTransform.anchorMax = new Vector2(0.5f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, -100f);

            achievementPanelParent = notificationParentObj.transform;
            Debug.Log("AchievementNotificationParent oluşturuldu");
        }
    }

    private void InitializeAchievements()
    {
        achievements = new List<Achievement>
        {
            new Achievement { id = "first_spark", title = "First Spark of Light", description = "Complete first level.", isUnlocked = false },
            new Achievement { id = "master_reflection", title = "Master of Reflection", description = "Direct the light with 3 mirrors.", isUnlocked = false },
            new Achievement { id = "flawless_victory", title = "Flawless Victory", description = "Finish 5 Levels in a row without mistakes.", isUnlocked = false },
            new Achievement { id = "mind_spark", title = "Mind Spark", description = "Complete a level on the first try.", isUnlocked = false },
            new Achievement { id = "color_commander", title = "Color Commander", description = "Be able to use 3 light sources.", isUnlocked = false },
            new Achievement { id = "quick_thinker", title = "Quick Thinker", description = "Complete a level in 20 seconds.", isUnlocked = false }
        };
    }

    public void UnlockAchievement(string achievementId)
    {
       
        Achievement achievement = achievements.Find(a => a.id == achievementId);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.isUnlocked = true;
            
            achievementQueue.Enqueue(achievement);
            SaveAchievements();

            // Eğer şu anda başarım gösterilmiyorsa, kuyruktaki ilk başarımı göster
            if (!isShowingAchievement)
            {
                ShowNextAchievement();
                
            }
        }
    }

    private void ShowNextAchievement()
    {
        if (achievementQueue.Count > 0)
        {
            isShowingAchievement = true;
            Achievement achievement = achievementQueue.Dequeue();
            audioManager.PlaySFX(audioManager.Notification);
            ShowAchievementUnlocked(achievement);
        }
        else
        {
            isShowingAchievement = false;
        }
    }

    private void ShowAchievementUnlocked(Achievement achievement)
    {
        Debug.Log($"Başarım kazanıldı: {achievement.title}");
        Debug.Log($"AchievementPanelPrefab null mu?: {achievementPanelPrefab == null}");
        Debug.Log($"AchievementPanelParent null mu?: {achievementPanelParent == null}");

        if (achievementPanelPrefab == null)
        {
            Debug.LogError("Achievement Panel Prefab atanmamış!");
            return;
        }

        if (achievementPanelParent == null)
        {
            Debug.LogError("Achievement Notification Parent bulunamadı!");
            return;
        }

        try
        {
            GameObject panel = Instantiate(achievementPanelPrefab, achievementPanelParent);
            Debug.Log($"Panel oluşturuldu: {panel != null}");

            AchievementPanel achievementPanel = panel.GetComponent<AchievementPanel>();
            Debug.Log($"AchievementPanel bileşeni bulundu mu?: {achievementPanel != null}");

            if (achievementPanel != null)
            {
                achievementPanel.SetAchievement(achievement);
                achievementPanel.OnPanelDestroyed += () => ShowNextAchievement(); // Panel kapandığında sıradaki başarımı göster
                Debug.Log("Başarım bildirimi gösteriliyor");
            }
            else
            {
                Debug.LogError("AchievementPanel bileşeni bulunamadı!");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Panel oluşturulurken hata: {e.Message}");
        }
    }

    private void SaveAchievements()
    {
        foreach (Achievement achievement in achievements)
        {
            PlayerPrefs.SetInt("Achievement_" + achievement.id, achievement.isUnlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void LoadAchievements()
    {
        foreach (Achievement achievement in achievements)
        {
            achievement.isUnlocked = PlayerPrefs.GetInt("Achievement_" + achievement.id, 0) == 1;
        }
    }

    public void PopulateAchievementsMenu()
    {
        // Eğer parent yoksa, tekrar bulmaya çalış
        if (achievementsListParent == null)
        {
            var parentObj = GameObject.Find("AchievementsListParent");
            if (parentObj != null)
                achievementsListParent = parentObj.transform;
            else
            {
                Debug.LogError("AchievementsListParent bulunamadı!");
                return;
            }
        }

        foreach (Transform child in achievementsListParent)
            Destroy(child.gameObject);

        int i = 0;
        foreach (var achievement in achievements)
        {
            GameObject item = Instantiate(achievementListItemPrefab, achievementsListParent);
            var listItem = item.GetComponent<AchievementListItem>();
            listItem.SetAchievement(achievement, i);
            i++;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Level_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + "_Attempted", 1);
        PlayerPrefs.Save();
    }

    public void ResetAllAchievements()
    {
        foreach (Achievement achievement in achievements)
        {
            achievement.isUnlocked = false;
            PlayerPrefs.DeleteKey("Achievement_" + achievement.id);
        }
        PlayerPrefs.Save();
    }
}