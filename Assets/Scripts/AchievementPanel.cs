using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class AchievementPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image backgroundImage;

    public event Action OnPanelDestroyed;

    private void Awake()
    {
        // Canvas'ı bul ve ayarla
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100; // En üstte görünsün
        }

        // Bileşenleri otomatik bul
        if (titleText == null)
            titleText = transform.Find("Canvas/TitleText")?.GetComponent<TextMeshProUGUI>();

        if (descriptionText == null)
            descriptionText = transform.Find("Canvas/DescriptionText")?.GetComponent<TextMeshProUGUI>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (backgroundImage == null)
            backgroundImage = transform.Find("Canvas/Background")?.GetComponent<Image>();

        // Bileşenleri kontrol et
        if (titleText == null)
            Debug.LogError("Title Text bileşeni bulunamadı!");
        if (descriptionText == null)
            Debug.LogError("Description Text bileşeni bulunamadı!");
        if (canvasGroup == null)
            Debug.LogError("CanvasGroup bileşeni bulunamadı!");
        if (backgroundImage == null)
            Debug.LogError("Background Image bileşeni bulunamadı!");
    }

    private void Start()
    {
        Debug.Log("AchievementPanel başlatılıyor...");

        // RectTransform ayarlarını düzelt
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(600f, 200f);
        }

        // Canvas'ı düzelt
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            canvasRect.anchorMin = Vector2.zero;
            canvasRect.anchorMax = Vector2.one;
            canvasRect.sizeDelta = Vector2.zero;
            canvasRect.anchoredPosition = Vector2.zero;
            canvasRect.localScale = Vector3.one;
        }

        // Text bileşenlerinin konumlarını ayarla
        if (titleText != null)
        {
            RectTransform titleRect = titleText.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.5f);
            titleRect.anchorMax = new Vector2(0.5f, 0.5f);
            titleRect.pivot = new Vector2(0.5f, 0.5f);
            titleRect.anchoredPosition = new Vector2(0, 33);
            titleRect.sizeDelta = new Vector2(500, 50);
            titleRect.localScale = Vector3.one;
        }

        if (descriptionText != null)
        {
            RectTransform descRect = descriptionText.GetComponent<RectTransform>();
            descRect.anchorMin = new Vector2(0.5f, 0.5f);
            descRect.anchorMax = new Vector2(0.5f, 0.5f);
            descRect.pivot = new Vector2(0.5f, 0.5f);
            descRect.anchoredPosition = new Vector2(0, -20);
            descRect.sizeDelta = new Vector2(500, 50);
            descRect.localScale = Vector3.one;
        }

        // Background'u düzelt
        if (backgroundImage != null)
        {
            RectTransform bgRect = backgroundImage.GetComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0.5f, 0.5f);
            bgRect.anchorMax = new Vector2(0.5f, 0.5f);
            bgRect.pivot = new Vector2(0.5f, 0.5f);
            bgRect.anchoredPosition = Vector2.zero;
            bgRect.sizeDelta = new Vector2(600, 200);
            bgRect.localScale = Vector3.one;
        }

        // Animasyonlu giriş efekti
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
        sequence.Join(canvasGroup.DOFade(1f, 0.5f));
        sequence.AppendInterval(3f); // 3 saniye bekle
        sequence.Append(canvasGroup.DOFade(0f, 0.5f));
        sequence.Join(transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack));
        sequence.OnComplete(() =>
        {
            OnPanelDestroyed?.Invoke(); // Panel kapandığında event'i tetikle
            Destroy(gameObject);
        });

        Debug.Log("Panel gösteriliyor...");
    }

    public void SetAchievement(AchievementManager.Achievement achievement)
    {
        Debug.Log($"Başarım ayarlanıyor: {achievement.title}");

        if (titleText != null)
        {
            titleText.text = achievement.title;
            titleText.fontSize = 36; // Başlık boyutunu büyüt
            titleText.color = Color.white; // Yazı rengini beyaz yap
            titleText.alignment = TextAlignmentOptions.Center; // Ortala
            Debug.Log($"Başlık ayarlandı: {achievement.title}");
        }

        if (descriptionText != null)
        {
            descriptionText.text = achievement.description;
            descriptionText.fontSize = 24; // Açıklama boyutunu büyüt
            descriptionText.color = Color.white; // Yazı rengini beyaz yap
            descriptionText.alignment = TextAlignmentOptions.Center; // Ortala
            Debug.Log($"Açıklama ayarlandı: {achievement.description}");
        }
    }
}