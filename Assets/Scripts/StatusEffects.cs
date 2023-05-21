using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static System.Net.Mime.MediaTypeNames;


[System.Serializable]
public class StatusEffect
{
    public string effectName;

    public GameObject effectPrefab;

    public int animation;

    public GameObject auraPrefab;

    public float auraDelay;

    public Vector3 standingHitPosition;
    public Vector3 standingAuraPosition;
    public Vector3 stunnedHitPosition;
    public Vector3 stunnedAuraPosition;
    public Vector3 woundedHitPosition;
    public Vector3 woundedAuraPosition;
}


public class StatusEffects : MonoBehaviour
{

    [SerializeField]
    private int currentAnimation = 0;

    public List<StatusEffect> buffs;
    public List<StatusEffect> debuffs;
    public List<StatusEffect> ailments;


    public GameObject lightGO;

    public GameObject aurasRoot;

    private int buffIndex;
    private int appliedBuffIndex = -1;
    private int debuffIndex;
    private int appliedDebuffIndex = -1;
    private int ailmentIndex;
    private int appliedAilmentIndex = -1;

    private IEnumerator spawnAuraCO;


    void Start()
    {
        buffIndex = 0;
        debuffIndex = 0;
        ailmentIndex = 0;
    }

    void Update()
    {
    }

    //Buff, Debuff and Ailment Buttons

    public void ApplyBuff(Vector3 userPosition)
    {
        Transform spawnedBuff = SpawnBuff().transform;
        userPosition += new Vector3(0, -0.9f, 0);
        //Buff Hit Position
        Vector3 newPosition = spawnedBuff.position;
        switch (currentAnimation)
        {
            //Idle and Buff animations
            case 0:
            case 1:
                newPosition = userPosition;
                break;

            //Stunned animation
            case 2:
                newPosition = buffs[buffIndex].stunnedHitPosition;
                break;

            //Wounded animation
            case 3:
                newPosition = buffs[buffIndex].woundedHitPosition;
                break;

        }

        spawnedBuff.transform.position = newPosition;

        appliedBuffIndex = buffIndex;


        //Buff Aura Position
        switch (currentAnimation)
        {
            //Idle and Buff animations
            case 0:
            case 1:
                newPosition = userPosition;
                break;

            //Stunned animation
            case 2:
                newPosition = buffs[buffIndex].stunnedAuraPosition;
                break;

            //Wounded animation
            case 3:
                newPosition = buffs[buffIndex].woundedAuraPosition;
                break;

        }

        CleanAuras();
        if (buffs[buffIndex].auraPrefab != null)
        {
            StartCoroutine(SpawnAura(buffs[buffIndex].auraPrefab, newPosition, buffs[buffIndex].auraDelay));
        }

        //RefreshStatusUI(0);

    }
    public void chooseBuff(int i)
    {
        buffIndex = i;
    }
    public void chooseDebuff(int i)
    {
        debuffIndex = i;
    }
    public void chooseAilment(int i)
    {
        ailmentIndex = i;
    }

    public void ApplyDebuff()
    {
        Transform spawnedDebuff = SpawnDebuff().transform;

        //Debuff Hit Position
        Vector3 newPosition = spawnedDebuff.position;
        switch (currentAnimation)
        {
            //Idle and Debuff animations
            case 0:
            case 1:
                newPosition = debuffs[debuffIndex].standingHitPosition;
                break;

            //Stunned animation
            case 2:
                newPosition = debuffs[debuffIndex].stunnedHitPosition;
                break;

            //Wounded animation
            case 3:
                newPosition = debuffs[debuffIndex].woundedHitPosition;
                break;

        }

        spawnedDebuff.transform.position = newPosition;

        appliedDebuffIndex = debuffIndex;

        //Debuff Aura Position
        switch (currentAnimation)
        {
            //Idle and Buff animations
            case 0:
            case 1:
                newPosition = debuffs[debuffIndex].standingAuraPosition;
                break;

            //Stunned animation
            case 2:
                newPosition = debuffs[debuffIndex].stunnedAuraPosition;
                break;

            //Wounded animation
            case 3:
                newPosition = debuffs[debuffIndex].woundedAuraPosition;
                break;

        }

        CleanAuras();
        if (debuffs[debuffIndex].auraPrefab != null)
        {
            StartCoroutine(SpawnAura(debuffs[debuffIndex].auraPrefab, newPosition, debuffs[debuffIndex].auraDelay));
        }

    }

    public void ApplyAilment()
    {
        Transform spawnedAilment = SpawnAilment().transform;

        //Ailment Hit Position
        Vector3 newPosition = spawnedAilment.position;
        switch (currentAnimation)
        {
            //Idle and Ailment animations
            case 0:
            case 1:
                newPosition = ailments[ailmentIndex].standingHitPosition;
                break;

            //Stunned animation
            case 2:
                newPosition = ailments[ailmentIndex].stunnedHitPosition;
                break;

            //Wounded animation
            case 3:
                newPosition = ailments[ailmentIndex].woundedHitPosition;
                break;

        }

        spawnedAilment.transform.position = newPosition;

        appliedAilmentIndex = ailmentIndex;

        //Ailment Aura Position
        switch (currentAnimation)
        {
            //Idle and Buff animations
            case 0:
            case 1:
                newPosition = ailments[ailmentIndex].standingAuraPosition;
                break;

            //Stunned animation
            case 2:
                newPosition = ailments[ailmentIndex].stunnedAuraPosition;
                break;

            //Wounded animation
            case 3:
                newPosition = ailments[ailmentIndex].woundedAuraPosition;
                break;

        }

        CleanAuras();
        if (ailments[ailmentIndex].auraPrefab != null)
        {
            StartCoroutine(SpawnAura(ailments[ailmentIndex].auraPrefab, newPosition, ailments[ailmentIndex].auraDelay));
        }
    }

    //Spawn Buff, Debuff and Ailment

    private GameObject SpawnBuff()
    {
        GameObject spawnedBuff = Instantiate(buffs[buffIndex].effectPrefab);
        spawnedBuff.SetActive(true);
        return spawnedBuff;
    }

    private GameObject SpawnDebuff()
    {
        GameObject spawnedDebuff = Instantiate(debuffs[debuffIndex].effectPrefab);
        spawnedDebuff.SetActive(true);
        return spawnedDebuff;
    }

    private GameObject SpawnAilment()
    {
        GameObject spawnedAilment = Instantiate(ailments[ailmentIndex].effectPrefab);
        spawnedAilment.SetActive(true);
        return spawnedAilment;
    }

    private IEnumerator SpawnAura(GameObject auraToSpawn, Vector3 newPosition, float delay)
    {
        yield return new WaitForSeconds(delay);

        CleanAuras();

        GameObject newAuraToSpawn = Instantiate(auraToSpawn, aurasRoot.transform);

        newAuraToSpawn.transform.position = newPosition;

        newAuraToSpawn.SetActive(true);

    }

    private void CleanAuras()
    {
        int totalAuras = aurasRoot.transform.childCount;
        for (int i = 0; i < totalAuras; i++)
        {
            Destroy(aurasRoot.transform.GetChild(0).gameObject);
        }
    }
}