using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//контроллер меню

public class MenuController : MonoBehaviour
{
    /// <summary>
    /// Кнопка "Играть"
    /// </summary>
    public void StartBttnPress()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Кнопка "Выйти"
    /// </summary>
    public void ExitBttnPress()
    {
        Application.Quit();
    }
}
