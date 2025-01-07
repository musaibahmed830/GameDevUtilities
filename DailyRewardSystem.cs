using UnityEngine;
using UnityEngine.UI;
using System;
using Mkey;

public class DailyRewardSystem : MonoBehaviour
{
    [Header("Boosters")]
    public Booster hammer;
    public Booster cannon;
    public Booster brush;
    public Booster dice;
    public FieldBooster rndrocket;
    public FieldBooster bomb;
    public FieldBooster colorbomb;
    public Image rewardImage;
    public Image rewardImage2, rewadShine2; 
    public Image rewardImage3, rewadShine3;
    public GameObject rewardPanel;
    public Button claimButton; 
    public Button missRewardButton;

    public Sprite[] rewardSprites;
    private int currentDay; 
    private DateTime lastClaimDate;
    private const int totalDays = 7; 
    public Text quantityText, quantityText1, quantityText2;
    public GameObject timeline;
    private void Start()
    {
       
        currentDay = PlayerPrefs.GetInt("CurrentDay", 1);
        lastClaimDate = DateTime.Parse(PlayerPrefs.GetString("LastClaimDate", DateTime.MinValue.ToString()));
        UpdateRewardUI();
        ShowRewardPopup();
    }

    private void ShowRewardPopup()
    {
        rewardImage.sprite = rewardSprites[currentDay - 1];
        if (currentDay == 7)
        {
            rewardImage.sprite = rewardSprites[4];
            rewardImage2.sprite = rewardSprites[6];
            rewardImage3.sprite = rewardSprites[7];
        }
        if (PlayerPrefs.GetInt("1stday") == 0)
        {
            rewardPanel.SetActive(true);
            PlayerPrefs.SetInt("1stday", 1);
        }
        bool canClaim = IsClaimable();
        claimButton.interactable = canClaim;
        missRewardButton.interactable = canClaim;
    }

    public void ClaimReward()
    {
        if (IsClaimable())
        {
            UnlockReward(currentDay);
            SetButtonsInteractable(false);
            AdvanceToNextDay();
            rewardPanel.gameObject.SetActive(false);
            if (PlayerPrefs.GetInt("timeline") == 0)
            {
                timeline.SetActive(true);
                PlayerPrefs.SetInt("timeline", 1);
            }
            Invoke("OffPanel", 3f);
        }
    }

    public void CrossPanel()
    {
        if (IsClaimable())
        {
            rewardPanel.SetActive(false);
            PlayerPrefs.SetInt("Claim", 1);
           
            claimButton.interactable = false;
            if (PlayerPrefs.GetInt("timeline") == 0)
            {
                timeline.SetActive(true);
                PlayerPrefs.SetInt("timeline", 1);
            }
            missRewardButton.interactable = true;
        }
        else
        {
            rewardPanel.SetActive(false);
            if (PlayerPrefs.GetInt("timeline") == 0)
            {
                timeline.SetActive(true);
                PlayerPrefs.SetInt("timeline", 1);
            }
        }
    }

    public void RewardClaim()
    {
        if (OmmySDK.Agent != null)
        {
            OmmySDK.Agent.ShowRewardedAd(ClaimReward, () => Debug.Log("No video available!"));
        }
    }

    public void MissReward()
    {
        if (IsClaimable())
        {
            SetButtonsInteractable(false);
            AdvanceToNextDay();
        }
    }

    private void UnlockReward(int day)
    {
        Debug.Log("Reward claimed for day: " + day);
        switch (day)
        {
            case 1:
                PlayerPrefs.SetInt("Wrench", PlayerPrefs.GetInt("Wrench") + 16);
                CoinManager.instance.WrenchAnim();
                break;
            case 2:
                bomb.AddCount(4);
                break;
            case 3:
                dice.AddCount(4);
                break;
            case 4:
                CoinsHolder.Add(100);
                break;
            case 5:
                brush.AddCount(9);
                break;
            case 6:
                CoinsHolder.Add(400);
                break;
            case 7:
                CoinsHolder.Add(900);
                hammer.AddCount(9);
                PlayerPrefs.SetInt("Wrench", PlayerPrefs.GetInt("Wrench") + 9);
                break;
        }
        lastClaimDate = DateTime.Now;
        PlayerPrefs.SetString("LastClaimDate", lastClaimDate.ToString());
        if (PlayerPrefs.GetInt("Claim") == 1)
        {
            claimButton.interactable = false;
        }
    }

    private void AdvanceToNextDay()
    {
        currentDay = (currentDay % totalDays) + 1;
        PlayerPrefs.SetInt("CurrentDay", currentDay);
        rewardPanel.SetActive(false);
    }

    private void UpdateRewardUI()
    {
        rewardImage.sprite = rewardSprites[currentDay - 1];

        if (currentDay == 1)
        {
            rewardImage.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            quantityText.text = " 4X".ToString();
        }
        else if (currentDay == 2)
        {
            quantityText.text = " 2X".ToString();
        }
        else if (currentDay == 3)
        {
            quantityText.text = " 2X".ToString();
        }
        else if (currentDay == 4)
        {
            quantityText.text = " 10X".ToString();
        }
        else if (currentDay == 5)
        {
            quantityText.text = " 3X".ToString();
        }
        else if (currentDay == 6)
        {
            quantityText.text = "20X".ToString();
        }
        else if (currentDay == 7)
        {
            rewardImage2.gameObject.SetActive(true);
            rewardImage3.gameObject.SetActive(true);
            rewadShine2.gameObject.SetActive(true);
            rewadShine3.gameObject.SetActive(true);
            quantityText.text = "30X".ToString();
            quantityText1.text = " 3X".ToString();
            quantityText2.text = " 3X".ToString();
        }
    }

    private bool IsClaimable()
    {
        return (DateTime.Now - lastClaimDate).TotalHours >= 24;
    }

    private void SetButtonsInteractable(bool isInteractable)
    {
        claimButton.interactable = isInteractable;
        missRewardButton.interactable = isInteractable;
    }
}
