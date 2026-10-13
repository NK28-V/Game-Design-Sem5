using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementListItem : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image iconImage;
    public Sprite lockedSprite;
    public Sprite unlockedSprite1;
    public Sprite unlockedSprite2;
    public Sprite unlockedSprite3;
    public Sprite unlockedSprite4;
    public Sprite unlockedSprite5;
    public Sprite unlockedSprite6;
    public Image backgroundImage;

    public void SetAchievement(AchievementManager.Achievement achievement, int index)
    {
        titleText.text = achievement.title;
        descriptionText.text = achievement.description;
        if (achievement.isUnlocked)
        {
            switch (index)
            {
                case 0: iconImage.sprite = unlockedSprite1; break;
                case 1: iconImage.sprite = unlockedSprite2; break;
                case 2: iconImage.sprite = unlockedSprite3; break;
                case 3: iconImage.sprite = unlockedSprite4; break;
                case 4: iconImage.sprite = unlockedSprite5; break;
                case 5: iconImage.sprite = unlockedSprite6; break;
                default: iconImage.sprite = unlockedSprite1; break;
            }
        }
        else
        {
            iconImage.sprite = lockedSprite;
        }
        iconImage.color = achievement.isUnlocked ? Color.white : Color.gray;
        backgroundImage.rectTransform.sizeDelta = new Vector2(400, 150);
        backgroundImage.color = Color.white;
    }
}