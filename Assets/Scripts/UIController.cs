using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//главный UI

public class UIController : MonoBehaviour
{
    [Header("Верхняя панель")]
    [SerializeField] TextMeshProUGUI wavesExperiencedCounter; //счетчик пережитых волн
    [SerializeField] TextMeshProUGUI nextWaveEnemiesAmount; //врагов в следующей волне
    [SerializeField] TextMeshProUGUI nextWaveTimer; //таймер до следующей волны

    [Header("Нижняя панель")]
    [SerializeField] TextMeshProUGUI peasantsAmount; //количество крестьян
    [SerializeField] TextMeshProUGUI warriorsAmount; //количество воинов
    [SerializeField] TextMeshProUGUI goldAmount; //количество золота

    [Header("Панели")]
    [SerializeField] GameObject messagePanel; //родительская панель
    [SerializeField] GameObject introPanel; //вводное сообщение
    [SerializeField] GameObject winPanel; //сообщение о победе
    [SerializeField] GameObject losePanel; //сообщение о поражении
    [SerializeField] GameObject pausePanel; //сообщение о паузе
    [SerializeField] GameObject desertersPanel; //сообщение о дезертирах

    [Header("Сообщения")]
    [SerializeField] TextMeshProUGUI desertersAmount; //количество дезертиров
    [SerializeField] TextMeshProUGUI raidsAmount; //количество пережитых набегов
    [SerializeField] TextMeshProUGUI endgamePeasantsAmount; //количество крестьян на конец игры
    [SerializeField] TextMeshProUGUI endgameGoldAmount; //количество денег на конец игры

    [Header("Интервалы")]
    [SerializeField] int nextWaveInterval; //время до прихода волны врагов (в секундах)
    [SerializeField] int nextDayInterval; //время до наступления следующих суток (в секундах)

    [Header("Прочее")]
    [SerializeField] GameController game;
    [SerializeField] Image goldTimerImg;

    float timeUntilNextWave; //осталось до следующей волны

    bool isGame; //идет ли игра

    public Action OnGameStarted; //игра началась
    public Action OnGoldTimerEnd; //сработал таймер обновления кол-ва денег
    public Action OnFight; //рейд

    private void OnEnable()
    {
        //подписываемся на все необходимые события
        game.OnGoldChanged += UpdateGoldCounter;
        game.OnWarriorsDeserted += Deserters;
        game.OnWarriorsAmountChanged += UpdateWarriorsCounter;
        game.OnPeasantsAmountChanged += UpdatePeasantsCounter;
        game.OnWavesAmountChanged += UpdateWavesCounter;
        game.OnGameEnd += EndGame;
        ShowIntro();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale != 0)
            {
                PauseGame(); //включаем паузу
            }
            else
            {
                EndPause(); //выключаем паузу
            }
        }
    }

    /// <summary>
    /// Показывает игроку вводное сообщение
    /// </summary>
    void ShowIntro()
    {
        messagePanel.SetActive(true);
        introPanel.SetActive(true);
    }

    /// <summary>
    /// Запускает игру
    /// </summary>
    public void StartGame()
    {
        introPanel.SetActive(false);
        messagePanel.SetActive(false);

        isGame = true;

        StartCoroutine(EnemyWaveTimer());
        StartCoroutine(GoldTimer());

        Debug.Log("Игра началась");
    }

    /// <summary>
    /// Часть солдат дезертировала
    /// </summary>
    /// <param name="warriors">сколько солдат осталось</param>
    /// <param name="deserters">сколько солдат дезертировало</param>
    /// <param name="gold">сколько денег в казне</param>
    private void Deserters(int warriors, int deserters, int gold)
    {
        warriorsAmount.text = warriors.ToString();
        goldAmount.text = gold.ToString();

        Time.timeScale = 0;
        messagePanel.SetActive(true);
        desertersPanel.SetActive(true);
        desertersAmount.text = $"-{deserters}";
    }

    /// <summary>
    /// Таймер до прихода врагов
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyWaveTimer()
    {
        timeUntilNextWave = nextWaveInterval;

        while (timeUntilNextWave > 0)
        {
            yield return null;
            timeUntilNextWave -= Time.deltaTime;
            nextWaveTimer.text = $"{timeUntilNextWave:f0}с.";
        }
        OnFight?.Invoke();
        StartCoroutine(EnemyWaveTimer());
    }

    /// <summary>
    /// Таймер до обновления кол-ва денег
    /// </summary>
    /// <returns></returns>
    IEnumerator GoldTimer()
    {
        while (isGame)
        {
            float timeLeft = nextDayInterval;
            goldTimerImg.fillAmount = 0;

            while (timeLeft > 0)
            {
                yield return null;
                timeLeft -= Time.deltaTime;
                goldTimerImg.fillAmount = (1 - timeLeft / nextDayInterval);
            }
            OnGoldTimerEnd?.Invoke();
        }
    }

    /// <summary>
    /// Показываем кол-во денег
    /// </summary>
    /// <param name="u">кол-во денег</param>
    void UpdateGoldCounter(int amount)
    {
        goldAmount.text = amount.ToString();
    }

    /// <summary>
    /// Показываем количество крестьян
    /// </summary>
    /// <param name="amount"></param>
    private void UpdatePeasantsCounter(int amount)
    {
        peasantsAmount.text = amount.ToString();
    }

    /// <summary>
    /// Показываем кол-во воинов
    /// </summary>
    /// <param name="amount"></param>
    private void UpdateWarriorsCounter(int amount)
    {
        warriorsAmount.text = amount.ToString();
    }

    /// <summary>
    /// Показываем, сколько волн пережито
    /// </summary>
    /// <param name="obj">кол-во волн</param>
    private void UpdateWavesCounter(int waves, int enemies)
    {
        wavesExperiencedCounter.text = waves.ToString();
        nextWaveEnemiesAmount.text = enemies.ToString();
    }

    /// <summary>
    /// Завершение игры
    /// </summary>
    /// <param name="obj"></param>
    private void EndGame(bool isWin)
    {
        isGame = false;

        messagePanel.SetActive(true);
        if (isWin)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
            raidsAmount.text = "Пережито набегов: " + wavesExperiencedCounter.text;
            endgameGoldAmount.text = "Денег в казне: " + goldAmount.text;
            endgamePeasantsAmount.text = "Крестьянских дворов: " + peasantsAmount.text;
        }
        Time.timeScale = 0;
    }

    /// <summary>
    /// Кнопка "Выход"
    /// </summary>
    public void ExitBttnPress()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Пауза
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
        messagePanel.SetActive(true);
        pausePanel.SetActive(true);
    }

    /// <summary>
    /// Завершить паузу
    /// </summary>
    public void EndPause()
    {
        pausePanel.SetActive(false);
        desertersPanel.SetActive(false);
        messagePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
