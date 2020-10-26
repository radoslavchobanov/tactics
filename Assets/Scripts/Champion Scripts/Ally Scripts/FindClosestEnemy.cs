using Packages.Rider.Editor.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosestEnemy : MonoBehaviour
{
    float distance;

    GameObject[] enemies;
    GameObject closestEnemy;
    float distanceToTheClosestEnemy;

    AllyChampController gameObjectChamp;

    private void Start()
    {
        closestEnemy = null;
        gameObjectChamp = gameObject.GetComponent<AllyChampController>();
    }

    private void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        distanceToTheClosestEnemy = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);

            if (distance < distanceToTheClosestEnemy)
            {
                closestEnemy = enemy;
                distanceToTheClosestEnemy = distance;
            }
        }

        if (distanceToTheClosestEnemy <= 9 && gameObjectChamp.target == null)
            gameObjectChamp.target = closestEnemy; // promenq targeta na Playera na nai blizkoto enemy - closestEnemy
        else if (gameObjectChamp.target != null && gameObjectChamp.championState != ChampionState.Attacking && distanceToTheClosestEnemy <= gameObjectChamp.distanceToTarget)
            // ako ima target, no se poqvi po blizak i geroq ne e pochnal da atakuva
            gameObjectChamp.target = closestEnemy;
    }
}