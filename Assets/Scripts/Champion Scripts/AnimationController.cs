using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private Dictionary<ChampionState, string> championStateAnimName;

    private ChampionController controller;

    private float timer = 0;


    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        championStateAnimName = new Dictionary<ChampionState, string>()
        {
            {ChampionState.Idle, "arthur_idle_01"},
            {ChampionState.Moving, "arthur_walk_01"},
            {ChampionState.Attacking, "arthur_attack_01"},
        };

        if (gameObject != null)
            controller = gameObject.GetComponent<ChampionController>();
    }
    
    private void Update() 
    {
        timer = Time.deltaTime;

        // if (timer == 60)
        {
            anim.Play(championStateAnimName[controller.championState]);
            timer = 0;
        }
    }
}