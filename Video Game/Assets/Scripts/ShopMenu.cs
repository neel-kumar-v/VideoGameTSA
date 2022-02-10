using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    // This stuff most likely won't need to be changed so doon't even worry about it
    public static bool inShop = false;
    public GameObject[] uiElementsToRemove;
    public GameObject[] uiElementsToAdd;
    [Space(10)]
    public GameObject shopPrompt;

    public static bool canPause = true;

    public void ShopOn() {
        inShop = true;
        canPause = !inShop;
        AddShopUI();
    }

    public void ShopOff() {
        inShop = false;
        canPause = !inShop;
        AddGameUI();
        RemoveShopUI();
    }

    public void Prompt() {
        shopPrompt.SetActive(true);
        RemoveGameUI();
    }

    public void RemoveGameUI() {
        foreach (GameObject uiElementToRemove in uiElementsToRemove)
        {
            uiElementToRemove.SetActive(false);
        }
    }

    public void AddGameUI() {
        foreach (GameObject uiElementToRemove in uiElementsToRemove)
        {
            uiElementToRemove.SetActive(true);
        }
    }

    public void RemoveShopUI() {
        foreach (GameObject uiElementToAdd in uiElementsToAdd)
        {
            uiElementToAdd.SetActive(false);
        }
    }
    public void AddShopUI() {
        foreach (GameObject uiElementToAdd in uiElementsToAdd)
        {
            uiElementToAdd.SetActive(true);
        }
    }

}
