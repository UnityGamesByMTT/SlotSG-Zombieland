using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource bg_adudio;
    [SerializeField] internal AudioSource audioPlayer_wl;
    [SerializeField] internal AudioSource audioPlayer_button;
    [SerializeField] internal AudioSource audioSpin_button;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip[] Bonusclips;
    [SerializeField] private AudioSource bg_audioBonus;
    [SerializeField] private AudioSource audioPlayer_Bonus;
    [SerializeField] private SlotBehaviour slotBehaviour;


    private void Start()
    {
        if (bg_adudio) bg_adudio.Play();
        audioPlayer_button.clip = clips[clips.Length-1];
        audioSpin_button.clip = clips[clips.Length-2];
    }

    internal void CheckFocusFunction(bool focus, bool IsSpinning)
    {
        if (!focus)
        {
            bg_adudio.Pause();
            audioPlayer_wl.Pause();
            audioPlayer_button.Pause();
        }
        else
        {
            if (!bg_adudio.mute) bg_adudio.UnPause();
            if (IsSpinning)
            {
                if (!audioPlayer_wl.mute) audioPlayer_wl.UnPause();
            }
            else
            {
                StopWLAaudio();
            }
            if (!audioPlayer_button.mute) audioPlayer_button.UnPause();

        }
    }

    void RecieveReactNativeAudioChanges(bool focus){
      Debug.Log("React-Native Audio Changes Called");
      
      if(focus){
        if (!bg_adudio.mute) bg_adudio.UnPause();
        if (slotBehaviour.IsSpinning)
        {
            if (!audioPlayer_wl.mute) audioPlayer_wl.UnPause();
        }
        else
        {
            StopWLAaudio();
        }
        if (!audioPlayer_button.mute) audioPlayer_button.UnPause();
      }else{
        bg_adudio.Pause();
        audioPlayer_wl.Pause();
        audioPlayer_button.Pause();
      }
    }

    internal void SwitchBGSound(bool isbonus)
    {
        if(isbonus)
        {
            if (bg_audioBonus) bg_audioBonus.enabled = true;
            if (bg_adudio) bg_adudio.enabled = false;
        }
        else
        {
            if (bg_audioBonus) bg_audioBonus.enabled = false;
            if (bg_adudio) bg_adudio.enabled = true;
        }
    }

    internal void PlayWLAudio(string type)
    {
        audioPlayer_wl.loop = false;
        int index = 0;
        switch (type)
        {
            case "spin":
                index = 0;
                audioPlayer_wl.loop = true;
                break;
            case "win":
                index = 1;
                break;
            case "lose":
                index = 2;
                break;
            case "spinStop":
                index = 3;
                break;
        }
        StopWLAaudio();
        audioPlayer_wl.clip = clips[index];
        audioPlayer_wl.Play();

    }

    internal void PlayBonusAudio(string type)
    {
        audioPlayer_wl.loop = false;
        int index = 0;
        switch (type)
        {
            case "card":
                index = 0;
                break;
            case "lose":
                index = 1;
                break;
            case "win":
                index = 2;
                break;
        }
        StopBonusAaudio();
        audioPlayer_Bonus.clip = Bonusclips[index];
        audioPlayer_Bonus.Play();

    }

    internal void PlayButtonAudio()
    {
        audioPlayer_button.Play();
    }

    internal void PlaySpinButtonAudio()
    {
        audioSpin_button.Play();
    }

    internal void StopWLAaudio()
    {
        audioPlayer_wl.Stop();
        audioPlayer_wl.loop = false;
    }

    internal void StopBonusAaudio()
    {
        audioPlayer_Bonus.Stop();
        audioPlayer_Bonus.loop = false;
    }

    internal void StopBgAudio()
    {
        bg_adudio.Stop();
    }

    internal void ToggleMute(bool toggle, string type="all")
    {
        switch (type)
        {
            case "bg":
                bg_adudio.mute = toggle;
                break;
            case "button":
                audioPlayer_button.mute=toggle;
                audioSpin_button.mute=toggle;
                break;
            case "wl":
                audioPlayer_wl.mute=toggle;
                break;
            case "all":
                audioPlayer_wl.mute = toggle;
                bg_adudio.mute = toggle;
                audioPlayer_button.mute = toggle;
                audioSpin_button.mute = toggle;
                break;
        }
    }

    internal void ChangeVolume(string type, float vol)
    {
        switch (type)
        {
            case "bg":

                bg_adudio.mute = (vol == 0);
                bg_adudio.volume = vol;
                bg_audioBonus.mute = (vol == 0);
                bg_audioBonus.volume = vol;
                break;
            case "button":
                audioPlayer_button.mute = (vol == 0);
                audioPlayer_button.volume = vol;
                break;
            case "wl":
                audioPlayer_wl.mute = (vol == 0);
                audioPlayer_wl.volume = vol;
                audioPlayer_Bonus.mute = (vol == 0);
                audioPlayer_Bonus.volume = vol;
                audioSpin_button.volume = vol;
                break;
            case "all":

                audioPlayer_wl.mute = (vol == 0);
                bg_adudio.mute = (vol == 0);
                audioPlayer_button.mute = (vol == 0);
                audioPlayer_wl.volume = vol;
                bg_adudio.volume = vol;
                audioPlayer_button.volume = vol;
                break;
        }

    }

}
