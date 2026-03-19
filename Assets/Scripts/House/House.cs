using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class House : MonoBehaviour
{
    [SerializeField] public Team team;
    public GameObject weapon;
    public GameObject HP;

    public int teamName;

    void Start()
    {
        teamName = team.teamName;
    }

    public void SpawnWeapon()
    {
        StartCoroutine(SpawnWeaponOnPoint());
    }

    public IEnumerator SpawnWeaponOnPoint()
    {
        yield return new WaitForSeconds(10f);

        if (weapon.activeSelf) yield break;

        weapon.SetActive(true);
        weapon.GetComponent<TriggerWeaponPoint>()._taken = false;
    }

    public void SpawnHP()
    {
        StartCoroutine(SpawnHPOnPoint());
    }

    public IEnumerator SpawnHPOnPoint()
    {
        yield return new WaitForSeconds(10f);

        if (HP.activeSelf) yield break;

        HP.SetActive(true);
        HP.GetComponent<TriggerHPPoint>()._taken = false;
    }

    
}
