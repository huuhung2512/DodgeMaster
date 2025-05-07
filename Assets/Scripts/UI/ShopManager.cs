using System.Collections;
using System.Collections.Generic;
using TMPro; // Thư viện cho TextMeshPro
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : SingletonBehavior<ShopManager>
{
    public int savedCharacter;
    public int currentCharacter;
    public GameObject[] characterModels;
    public GameObject equipText;
    public GameObject costText;
    public Character[] chars;
    public Button buyButton;
    public TextMeshProUGUI nameCharText;
    public TextMeshProUGUI totalcoinText;
    void Start()
    {
        foreach (Character charac in chars)
        {
            if (charac.price == 0)
            {
                charac.isUnlocked = true;
            }
            else
            {
                charac.isUnlocked = PlayerPrefs.GetInt(charac.nameChar, 0) == 0 ? false : true;
            }
        }
        totalcoinText.text = PlayerPrefs.GetInt("Coins", 0).ToString();
        savedCharacter = PlayerPrefs.GetInt("SelectedCar", 0);
        currentCharacter = savedCharacter;

        foreach (GameObject car in characterModels)
        {
            car.SetActive(false);
        }
        characterModels[savedCharacter].SetActive(true);
        UpdateEquip();
    }
    private void Update()
    {
        UpdateUI();
    }

    public void ChangeNext()
    {
        characterModels[currentCharacter].SetActive(false);
        currentCharacter++;
        if (currentCharacter == characterModels.Length)
        {
            currentCharacter = 0;
        }
        characterModels[currentCharacter].SetActive(true);
        UpdateEquip();
    }

    public void ChangePre()
    {
        characterModels[currentCharacter].SetActive(false);
        currentCharacter--;
        if (currentCharacter == -1)
        {
            currentCharacter = characterModels.Length - 1;
        }
        characterModels[currentCharacter].SetActive(true);
        UpdateEquip();
    }

    public void SaveChange()
    {
        Character c = chars[currentCharacter];
        if (c.isUnlocked)
        {
            savedCharacter = currentCharacter;
            PlayerPrefs.SetInt("SelectedCar", savedCharacter);
            PlayerPrefs.Save();
            UpdateEquip();
        }
        else
        {
            Debug.Log("Nhan vat chua duoc mo khoa");
        }
    }
    void UpdateEquip()
    {
        if (currentCharacter == savedCharacter)
        {
            equipText.SetActive(true);
        }
        else
        {
            equipText.SetActive(false);
        }
    }

    public void ExitShop()
    {
        if (currentCharacter != savedCharacter)
        {
            characterModels[currentCharacter].SetActive(false);
            characterModels[savedCharacter].SetActive(true);
            currentCharacter = savedCharacter;
        }
    }

    public void UnlockChar()
    {
        Character c = chars[currentCharacter];
        PlayerPrefs.SetInt(c.nameChar, 1);
        PlayerPrefs.SetInt("SelectedCar", currentCharacter);
        c.isUnlocked = true;
        totalcoinText.text = (PlayerPrefs.GetInt("Coins", 0) - c.price).ToString();
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) - c.price);
    }
    public void UpdateUI()
    {
        Character c = chars[currentCharacter];
        nameCharText.text = c.nameChar.ToString();
        if (c.isUnlocked)
        {
            buyButton.gameObject.SetActive(false);
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = c.price.ToString();
            if (c.price <= PlayerPrefs.GetInt("Coins", 0))
            {
                buyButton.interactable = true;
            }
            else
            {
                buyButton.interactable = false;
            }
        }
    }
}
