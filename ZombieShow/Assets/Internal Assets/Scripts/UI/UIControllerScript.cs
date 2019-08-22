using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerScript : MonoBehaviour
{
    [Header("Interfaces")]
    public GameObject startInterface;
    public GameObject shopInterface;
    public GameObject aftermatchInterface;
    public GameObject optionsInterface;
    public GameObject upgradeInterface;
    public GameObject gameInterface;
    [Header("Upper Menu Links")]
    public GameObject upperMenuShopLink;
    public GameObject upperMenuOptionsLink;
    public GameObject upperMenuUnknownLink;


    public void Upgrade()
    {
        CloseAll();
        CloseUpperLinksButNotPreferences();
        upgradeInterface.SetActive(true);
    }

    public void Shop()
    {
        CloseAll();
        shopInterface.SetActive(true);
        CloseUpperLinksButNotPreferences();
    }


    public void Options()
    {
        CloseAll();
        ShowUpperLinksButNotPreferences();
        optionsInterface.SetActive(true);
    }

    public void ToMainMenu()
    {
        CloseAll();
        ShowUpperLinksButNotPreferences();
        startInterface.SetActive(true);
    }

    public void Game()
    {
        CloseAll();
        CloseUpperLinksButNotPreferences();
        gameInterface.SetActive(true);
    }


    public void Aftermatch()
    {
        CloseAll();
        ShowUpperLinksButNotPreferences();
        aftermatchInterface.SetActive(true);
    }

    public void StartAD()
    {
        Debug.Log("Commercial was Initialized");
        Upgrade();
    }

    void CloseAll()
    {
        startInterface.SetActive(false);
      shopInterface.SetActive(false);
        aftermatchInterface.SetActive(false);
        optionsInterface.SetActive(false);
        upgradeInterface.SetActive(false);
        gameInterface.SetActive(false);
    }

    void CloseUpperLinksButNotPreferences()
    {
        upperMenuShopLink.SetActive(false);
        upperMenuUnknownLink.SetActive(false);
    }


    void ShowUpperLinksButNotPreferences()
    {
        upperMenuShopLink.SetActive(true);
        upperMenuUnknownLink.SetActive(true);
    }


}
