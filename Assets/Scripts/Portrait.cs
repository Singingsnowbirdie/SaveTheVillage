using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//устанавливаем выбранный игроком портер

public class Portrait : MonoBehaviour
{
    Data data;
    [SerializeField] Image portrait;

    void Start()
    {
        data = FindObjectOfType<Data>();
        portrait.sprite = data.character;
    }
}
