using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("Звуки")]
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource clickSound;
    [SerializeField] AudioSource coinSound;
    [SerializeField] AudioSource unitReadySound;
    [SerializeField] AudioSource battleSound;

    [Header("Объекты")]
    [SerializeField] GameController game;
    [SerializeField] UIController UI;

    bool isMute;

    private void OnEnable()
    {
        if (UI != null)
        {
            UI.OnGoldTimerEnd += PlayCoinSound;
            UI.OnFight += PlayFightSound;
        }
        if (game != null)
        {
            game.OnWarriorsAmountChanged += PlayUnitSound;
            game.OnPeasantsAmountChanged += PlayUnitSound;
        }
    }

    /// <summary>
    /// Включить/выключить звуки
    /// </summary>
    public void Mute()
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Pause();
            isMute = true;
        }
        else
        {
            backgroundMusic.Play();
            isMute = false;
        }
    }

    /// <summary>
    /// Проиграть звук клика
    /// </summary>
    /// <param name="sound"></param>
    public void PlayClickSound()
    {
        if (!isMute)
        {
            clickSound.Play();
        }
    }

    /// <summary>
    /// Проиграть звук монеток
    /// </summary>
    void PlayCoinSound()
    {
        if (!isMute)
        {
            coinSound.Play();
        }
    }

    /// <summary>
    /// Проиграть звук "юнит готов"
    /// </summary>
    void PlayUnitSound(int i)
    {
        if (!isMute)
        {
            unitReadySound.Play();
        }
    }

    /// <summary>
    /// Проиграть звук битвы
    /// </summary>
    private void PlayFightSound()
    {
        if (!isMute)
        {
            battleSound.Play();
        }
    }

}
