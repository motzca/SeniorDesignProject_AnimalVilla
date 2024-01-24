/*
    Programmer: Kelli Porter
    Email: portekn@mail.uc.edu
    Date: 1/24/2023
    Info: Program brings functionallity to the menus
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    //--Variables--//
    public Button start;
    public Button options;
    public Button sign_in;
    public Button apply_return;
    public GameObject StartMenu;
    public GameObject OptionsMenu;


public void Start()
{
    StartMenu.SetActive(true);
    OptionsMenu.SetActive(false);

    start.onClick.AddListener(StartGame);
    options.onClick.AddListener(ShowOptionsMenu);
    sign_in.onClick.AddListener(OpenSigninPage);
    apply_return.onClick.AddListener(ShowStartMenu);
}

    //-----Button Clicks-----//

    public void StartGame()
    {
        //Open Another Scene
    }

    public void ShowOptionsMenu()
    {
        StartMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void OpenSigninPage()
    {
        //Prompt user to sign in with google account
    }

    public void ShowStartMenu()
    {
        StartMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

}
