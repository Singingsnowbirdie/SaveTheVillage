using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//класс выбора персонажа (в меню)

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] List<Sprite> maleCharacters; //мужские портреты
    [SerializeField] List<Sprite> femaleCharacters; //женские портреты

    [SerializeField] Image portrait;
    [SerializeField] Data data;

    bool isMale;
    int characterID;

    void Start()
    {
        isMale = true;
        characterID = 0;
        ShowPortrait();
    }

    /// <summary>
    /// кнопки переключения пола
    /// </summary>
    /// <param name="isMale"></param>
    public void GenderBttn(bool isMale)
    {
        this.isMale = isMale;
        ShowPortrait();
    }

    /// <summary>
    /// //кнопки "следующий" и "предыдущий" персонаж
    /// </summary>
    /// <param name="isNext"></param>
    public void NextCharacterBttn(bool isNext)
    {
        if (isNext)
        {
            if (characterID < 3)
            {
                characterID++;
            }
            else
            {
                characterID = 0;
            }
        }
        else
        {
            if (characterID > 0)
            {
                characterID--;
            }
            else
            {
                characterID = 3;
            }
        }
        ShowPortrait();
    }

    /// <summary>
    /// показать новый портрет
    /// </summary>
    void ShowPortrait()
    {
        List<Sprite> list;

        if (isMale)
        {
            list = maleCharacters;
        }
        else
        {
            list = femaleCharacters;
        }

        portrait.sprite = list[characterID];
        SaveCharacter();
    }

    /// <summary>
    /// Запоминаем выбор игрока
    /// </summary>
    void SaveCharacter()
    {
        data.character = portrait.sprite;
    }



}
