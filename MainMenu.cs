using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Easy()
    {
        Application.LoadLevel("Easy");
    }

    public void Medium()
    {
        Application.LoadLevel("Medium");
    }

    public void Hard()
    {
        Application.LoadLevel("Hard");
    }

    public void Extremely()
    {
        Application.LoadLevel("Extremely");
    }

    public void About()
    {
        Application.LoadLevel("About");
    }

    public void Instruction()
    {
        Application.LoadLevel("Instruction");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
