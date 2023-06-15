using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    AudioSource audioSource;
    public GameObject greenAudio, redAudio, blackAudio;
    public AudioClip attack, hit, death, battleVictory, gameVictory, potion, revive, declineClick, confirmClick, chestOpen, shrine, run, mouseHover;
    public AudioClip menuPause, menuUnpause, diceRoll, encounter, dodge, atkbuff, debuff, fireatk, iceatk, thunderatk, toxicatk, absorb;
    public AudioClip firespell, waterspell, rockspell, lightspell, strongatk;
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
            audioSource.PlayOneShot(attack, 1);
        }
        else if (soundName.Contains("hit"))
        {
            audioSource.PlayOneShot(hit, 1);
        }
        else if (soundName.Contains("death"))
        {
            audioSource.PlayOneShot(death, 1);
        }
        else if (soundName.Contains("battleVictory"))
        {
            audioSource.PlayOneShot(battleVictory, 1);
        }
        else if (soundName.Contains("gameVictory"))
        {
            audioSource.PlayOneShot(gameVictory, 1);
        }
        else if (soundName.Contains("potion"))
        {
            audioSource.PlayOneShot(potion, 1);
        }
        else if (soundName.Contains("declineClick"))
        {
            audioSource.PlayOneShot(declineClick, 1);
        }
        else if (soundName.Contains("chestOpen"))
        {
            audioSource.PlayOneShot(chestOpen, 1);
        }
        else if (soundName.Contains("shrine"))
        {
            audioSource.PlayOneShot(shrine, 1);
        }
        else if (soundName.Contains("run"))
        {
            audioSource.PlayOneShot(run, 1);
        }
        else if (soundName.Contains("menuPause"))
        {
            audioSource.PlayOneShot(menuPause, 1);
        }
        else if (soundName.Contains("menuUnpause"))
        {
            audioSource.PlayOneShot(menuUnpause, 1);
        }
        else if (soundName.Contains("diceRoll"))
        {
            audioSource.PlayOneShot(diceRoll, 1);
        }
        else if (soundName.Contains("encounter"))
        {
            audioSource.PlayOneShot(encounter, 1);
        }
        else if (soundName.Contains("dodge"))
        {
            audioSource.PlayOneShot(dodge, 1);
        }
        else if (soundName.Contains("atkbuff"))
        {
            audioSource.PlayOneShot(atkbuff, 1);
        }
        else if (soundName.Contains("debuff"))
        {
            audioSource.PlayOneShot(debuff, 1);
        }
        else if (soundName.Contains("absorb"))
        {
            audioSource.PlayOneShot(absorb, 1);
        }
        else if (soundName.Contains("fireatk"))
        {
            audioSource.PlayOneShot(fireatk, 1);
        }
        else if (soundName.Contains("iceatk"))
        {
            audioSource.PlayOneShot(iceatk, 1);
        }
        else if (soundName.Contains("thunderatk"))
        {
            audioSource.PlayOneShot(thunderatk, 1);
        }
        else if (soundName.Contains("toxicatk"))
        {
            audioSource.PlayOneShot(toxicatk, 1);
        }
        else if (soundName.Contains("revive"))
        {
            audioSource.PlayOneShot(revive, 1);
        }
        else if (soundName.Contains("firespell"))
        {
            audioSource.PlayOneShot(firespell, 1);
        }
        else if (soundName.Contains("waterspell"))
        {
            audioSource.PlayOneShot(waterspell, 1);
        }
        else if (soundName.Contains("rockspell"))
        {
            audioSource.PlayOneShot(rockspell, 1);
        }
        else if (soundName.Contains("lightspell"))
        {
            audioSource.PlayOneShot(lightspell, 1);
        }
        else if (soundName.Contains("strongatk"))
        {
            audioSource.PlayOneShot(strongatk, 1);
        }
    }
}
