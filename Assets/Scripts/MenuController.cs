using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public GameObject mainMenu, story, instructions, credits;

    public void Action(MenuButton.Button button)
    {
        mainMenu.SetActive(false); story.SetActive(false); instructions.SetActive(false); credits.SetActive(false);
        switch (button)
        {
            case MenuButton.Button.start:
                story.SetActive(true);
                break;
            case MenuButton.Button.instructions:
                instructions.SetActive(true);
                break;
            case MenuButton.Button.credits:
                credits.SetActive(true);
                break;
            case MenuButton.Button.ggj:
                mainMenu.SetActive(true);
                Application.OpenURL("http://globalgamejam.org/");
                break;
            case MenuButton.Button.back:
                mainMenu.SetActive(true);
                break;
        }
    }
}
