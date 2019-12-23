using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FB_Configure : MonoBehaviour
{
    public Text idText;

    void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(SetInit, OnHideUnity);
        }
        else
            FB.ActivateApp();
 
    }

    private void SetInit()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("Facebook is active");
        }
        else
            Debug.LogError("Couldnt initialize Facebook");
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;
    }


    public void FacebookLogin()
    {
        var perms = new List<string>() { "public_profile"};
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }

    private void AuthCallback(IResult result)
    {

        if (result.Error != null)
            Debug.Log(result.Error);
        else
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("Facebook is LogIn");
            }
            else
            {
                Debug.Log("User cancelled login");
            }
        }
}

    public void FacebookLogout()
    {
        FB.LogOut();
        idText.text = "Your Facebook is Logged Out";
    }



    public void FacebookShare()
    {
        FB.ShareLink(
            contentURL:new System.Uri("http://vk.com"),
           contentTitle: "Хочешь узнать секрет?",
            contentDescription:"Кажется, главный разработчик этого приложения любит лупить свою девушку по заднице. Я слышал ее крики в Дискорде",
           callback: OnShare);

    }

    void OnShare(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);

        }

        else if (!string.IsNullOrEmpty(result.PostId))
            Debug.Log(result.PostId);
        else
            Debug.Log("Share Suckseed (succeed,lol)");

    }

    void DealWithMenu(bool isLoggedIn)
    {
        if (isLoggedIn) {
            idText.text = "Your Facebook Logged In";
        } }
}

