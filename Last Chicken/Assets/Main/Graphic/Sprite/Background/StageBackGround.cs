﻿using UnityEngine;

public class StageBackGround : MonoBehaviour
{
    public static StageBackGround instance;

    [System.NonSerialized] public SpriteRenderer spriteRenderer;
    [System.NonSerialized] public Animator animator;
    public GameObject filter;

    public void Awake()
    {
        instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if(filter)
            filter.SetActive(!CaveManager.inCave);
    }

    public void Fadein()
    {
        animator.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
    }
}