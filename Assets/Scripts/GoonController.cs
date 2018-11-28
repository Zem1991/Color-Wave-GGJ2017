using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoonController : MonoBehaviour {
    private Animator animator;

    private static int animatorParameterChoice = Animator.StringToHash("choice");

    void Start () {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetInteger(animatorParameterChoice, Mathf.CeilToInt(Random.value * 3) - 1);
    }
}
