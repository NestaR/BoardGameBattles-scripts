using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    AudioSource audioSource;
    public GameObject greenAudio, redAudio, blackAudio;
    public AudioClip attack, hit, death, battleVictory, gameVictory, potion, declineClick, confirmClick, chestOpen, shrine, run, mouseHover;
    public AudioClip menuPause, menuUnpause, diceRoll, encounter;
    bool playing;
    // Start is called before the first frame update
    void Start()
    {
        if (blackAudio == null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }
        else if (PlayerPrefs.GetString("CurrentTile").Contains("Green"))
        {
            greenAudio.SetActive(true);
            audioSource = greenAudio.GetComponent<AudioSource>();
        }
        else if (PlayerPrefs.GetString("CurrentTile").Contains("Red"))
        {
            redAudio.SetActive(true);
            audioSource = redAudio.GetComponent<AudioSource>();
        }
        else if (PlayerPrefs.GetString("CurrentTile").Contains("Black"))
        {
            blackAudio.SetActive(true);
            audioSource = blackAudio.GetComponent<AudioSource>();
        }
    }

    public void playHover()
    {
        audioSource.PlayOneShot(mouseHover);
    }
    public void playMouseClick()
    {
        audioSource.PlayOneShot(confirmClick);
    }
    public void playClip(string soundName)
    {
        if(soundName.Contains("attack"))
        {
            audioSource.PlayOneShot(attack);
        }
        else if (soundName.Contains("hit"))
        {
            audioSource.PlayOneShot(hit);
        }
        else if (soundName.Contains("death"))
        {
            audioSource.PlayOneShot(death);
        }
        else if (soundName.Contains("battleVictory"))
        {
            audioSource.PlayOneShot(battleVictory);
        }
        else if (soundName.Contains("gameVictory"))
        {
            audioSource.PlayOneShot(gameVictory);
        }
        else if (soundName.Contains("potion"))
        {
            audioSource.PlayOneShot(potion);
        }
        else if (soundName.Contains("declineClick"))
        {
            audioSource.PlayOneShot(declineClick);
        }
        else if (soundName.Contains("chestOpen"))
        {
            audioSource.PlayOneShot(chestOpen);
        }
        else if (soundName.Contains("shrine"))
        {
            audioSource.PlayOneShot(shrine);
        }
        else if (soundName.Contains("run"))
        {
            audioSource.PlayOneShot(run);
        }
        else if (soundName.Contains("menuPause"))
        {
            audioSource.PlayOneShot(menuPause);
        }
        else if (soundName.Contains("menuUnpause"))
        {
            audioSource.PlayOneShot(menuUnpause);
        }
        else if (soundName.Contains("diceRoll"))
        {
            audioSource.PlayOneShot(diceRoll);
        }
        else if (soundName.Contains("encounter"))
        {
            audioSource.PlayOneShot(encounter);
        }
    }
}
