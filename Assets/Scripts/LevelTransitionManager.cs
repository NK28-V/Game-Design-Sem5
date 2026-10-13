using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionManager : MonoBehaviour
{
    private const int LEVEL_START_INDEX = 1;
    private const int MAX_PLAYABLE_LEVEL = 5;
    public static LevelTransitionManager Instance;

    public GameObject lightCorePrefab;
    public float animationDuration = 2.5f;

    private bool isTransitioning = false;  
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayLightCoreAndLoadNext()
    {
        if (isTransitioning) return; 

        isTransitioning = true;
        audioManager.PlaySFX(audioManager.gameWin);
        StartCoroutine(PlayAnimationAndLoadNextLevel());
    }

    private IEnumerator PlayAnimationAndLoadNextLevel()
    {
        GameObject effect = Instantiate(lightCorePrefab, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(animationDuration);

        Destroy(effect);
        int levelProgress = PlayerPrefs.GetInt("level", LEVEL_START_INDEX);
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        
        if (currentIndex >= MAX_PLAYABLE_LEVEL)
        {
            SceneManager.LoadScene(0);
        }
        else if (currentIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            if (levelProgress == currentIndex && levelProgress < MAX_PLAYABLE_LEVEL)
            {
                levelProgress++;
                PlayerPrefs.SetInt("level", levelProgress);
            }

            SceneManager.LoadScene(currentIndex + 1);
        }
        else
        {
            Debug.Log("Tüm seviyeler tamamlandı ama sahne tanımlı değil!");
            SceneManager.LoadScene(0);
        }

        isTransitioning = false;
    }

}
