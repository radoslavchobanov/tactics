using Packages.Rider.Editor.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosestChampion : MonoBehaviour
{
    float distance;

    GameObject[] champions;
    GameObject closestChampion;
    float distanceToTheClosestChampion;

    EnemyChampController gameObjectEnemy;

    private void Start()
    {
        closestChampion = null;
        gameObjectEnemy = gameObject.GetComponent<EnemyChampController>();
    }

    private void Update()
    {
        champions = GameObject.FindGameObjectsWithTag("Champion");
        distanceToTheClosestChampion = Mathf.Infinity;

        foreach (GameObject champion in champions)
        {
            distance = Vector3.Distance(gameObject.transform.position, champion.transform.position);

            if (distance < distanceToTheClosestChampion)
            {
                closestChampion = champion;
                distanceToTheClosestChampion = distance;
            }
        }

        if (distanceToTheClosestChampion <= 9 && gameObjectEnemy.target == null)
            gameObjectEnemy.target = closestChampion; // promenq targeta na Playera na nai blizkoto enemy - closestEnemy
        // na dolniq else if da napravq taka che kogato geroq e pochnal da atakuva da ne moje da si smeni targeta -> attack state v controllera
        else if (gameObjectEnemy.target != null && gameObjectEnemy.championState != ChampionState.Attacking && distanceToTheClosestChampion <= gameObjectEnemy.distanceToTarget)
            // ako ima target, no se poqvi po blizak i geroq ne e pochnal da atakuva
            gameObjectEnemy.target = closestChampion;
    }
}