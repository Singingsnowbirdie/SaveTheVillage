using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RecruitButton : MonoBehaviour
{
    [SerializeField] GameController game;
    [SerializeField] Button bttn;
    [SerializeField] Image timerImg; //изображение
    [SerializeField] int hireDelay; //сколько времени занимает найм (в секундах)
    [SerializeField] int hireCost; //сколько стоит найм

    public Action<int> OnGoldSpent; //потрачены деньги
    public Action OnUnitHired; //событие: юнит нанят

    bool isTimer; //идет ли отсчет времени

    private void OnEnable()
    {
        bttn.interactable = false;
        game.OnGoldChanged += CheckButtonState;
    }

    /// <summary>
    /// Включаем и выключаем кнопку
    /// </summary>
    /// <param name="amount"></param>
    private void CheckButtonState(int amount)
    {
        if (amount >= hireCost)
        {
            if (!isTimer)
            {
                bttn.interactable = true;
                timerImg.fillAmount = 0;
            }
        }
        else
        {
            bttn.interactable = false;
        }
    }

    /// <summary>
    /// Нажата кнопка
    /// </summary>
    public void Press()
    {
        OnGoldSpent?.Invoke(hireCost);
        StartCoroutine(Timer());
    }

    /// <summary>
    /// Таймер до появления у нас нового юнита и включения кнопки
    /// </summary>
    IEnumerator Timer()
    {
        isTimer = true;
        bttn.interactable = false;
        float timeLeft = hireDelay;
        while (timeLeft > 0)
        {
            yield return null;
            timeLeft -= Time.deltaTime;
            timerImg.fillAmount = (1 - timeLeft / hireDelay);
        }
        timerImg.fillAmount = 0;
        isTimer = false;
        OnUnitHired?.Invoke();
    }
}
