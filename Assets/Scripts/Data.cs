using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Храним данные о выбранном игроком персонаже

public class Data : MonoBehaviour
{
    public Sprite character;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
