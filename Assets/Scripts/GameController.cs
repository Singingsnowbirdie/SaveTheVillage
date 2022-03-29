using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] int warriorConsumption; //сколько "съедает" воин за сутки
    [SerializeField] int goldProfit; //сколько золота приносит один крестьянский двор за сутки
    [SerializeField] int goldToWin; //сколько золота нужно накопить для победы
    [SerializeField] int peasantsToWin; //сколько крестьян нужно накопить для победы

    [Header("Интерфейс")]
    [SerializeField] UIController ui;
    [SerializeField] RecruitButton peasantRecruting;
    [SerializeField] RecruitButton warriorRecruting;

    int enemiesAmount; //количество врагов в следующей волне
    int peasantsAmount = 1; //сколько крестьян
    int warriorsAmount = 0; //сколко воинов
    int goldAmount = 0; //сколько денег
    int wavesExperienced = 0; //сколько пережито волн

    //события
    public Action<int> OnGoldChanged; //поменялось кол-во золота
    public Action<int> OnPeasantsAmountChanged; //поменялось кол-во крестьян
    public Action<int> OnWarriorsAmountChanged; //поменялось кол-во воинов
    public Action<int, int, int> OnWarriorsDeserted; //часть солдат дезертировала
    public Action<int, int> OnWavesAmountChanged; //поменялось кол-во волн
    public Action<bool> OnGameEnd; //игра завершена

    public int GoldAmount { get => goldAmount; }

    private void OnEnable()
    {
        ui.OnFight += Fight;
        ui.OnGoldTimerEnd += GoldUpdate;
        peasantRecruting.OnGoldSpent += SpendGold;
        warriorRecruting.OnGoldSpent += SpendGold;
        warriorRecruting.OnUnitHired += AddWarrior;
        peasantRecruting.OnUnitHired += AddPeasant;
    }

    /// <summary>
    /// Тратим деньги
    /// </summary>
    /// <param name="amount"></param>
    private void SpendGold(int amount)
    {
        goldAmount -= amount;
        Debug.Log($"Потрачено {amount} монет");
        OnGoldChanged?.Invoke(goldAmount);
    }

    /// <summary>
    /// Добавляем крестьянина
    /// </summary>
    private void AddPeasant()
    {
        peasantsAmount++;
        Debug.Log("Крестьянин нанят");
        СheckVictoryConditions();
        OnPeasantsAmountChanged?.Invoke(peasantsAmount);
        OnGoldChanged?.Invoke(goldAmount);
    }

    /// <summary>
    /// Добавляем воина
    /// </summary>
    private void AddWarrior()
    {
        warriorsAmount++;
        Debug.Log("Воин нанят");
        OnWarriorsAmountChanged?.Invoke(warriorsAmount);
        OnGoldChanged?.Invoke(goldAmount);
    }


    /// <summary>
    /// Считаем суточный расход/доход
    /// </summary>
    private void GoldUpdate()
    {
        goldAmount += peasantsAmount * goldProfit; //получаем доход с крестьян

        int armySpending = warriorsAmount * warriorConsumption; //расходы на армию

        if (goldAmount > armySpending) //если денег в казне хватает на содержание армии
        {
            goldAmount -= armySpending; //выплачиваем содерджание солдатам
            OnGoldChanged?.Invoke(goldAmount); //оповещаем интерфейс
        }
        else //если денег в казне НЕ хватает на содержание всех солдат
        {
            int paidWarriors = goldAmount / armySpending; //солдаты, на которых хватило денег
            goldAmount -= paidWarriors * warriorConsumption; //выплачиваем жалование тем, кому его хватило
            int deserters = warriorsAmount - paidWarriors; //дезертиры
            warriorsAmount -= deserters; //вычитаем дезертиров из числа солдат
            OnWarriorsDeserted?.Invoke(paidWarriors, deserters, goldAmount); //оповещаем интерфейс
        }

        СheckVictoryConditions(); //проверяем условия победы
    }

    /// <summary>
    /// Отражаем нападение
    /// </summary>
    private void Fight()
    {
        if (enemiesAmount > warriorsAmount) //если пришло врагов больше, чем у нас вояк
        {
            OnGameEnd(false);
        }
        else
        {
            warriorsAmount -= enemiesAmount; //осталось воинов
            OnWarriorsAmountChanged?.Invoke(warriorsAmount);
            wavesExperienced++; //пережили волн
            if (wavesExperienced >=3) //если пережили уже больше трех волн
            {
                enemiesAmount++; //врагов в след. волне
            }
            OnWavesAmountChanged?.Invoke(wavesExperienced, enemiesAmount);
            Debug.Log("Пережили еще одну волну");
        }
    }

    /// <summary>
    /// Проверяем условия победы
    /// </summary>
    private void СheckVictoryConditions()
    {
        if (goldAmount >= goldToWin && peasantsAmount >= peasantsToWin)
        {
            OnGameEnd(true);
        }
    }

}
