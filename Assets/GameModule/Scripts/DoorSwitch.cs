using LastBastion.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that represents door switcher behaviour.
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class DoorSwitch : MonoBehaviour {

    #region Private fields
    [SerializeField] private Door door;
    private Animator animator;
    #endregion


    #region MonoBehaviour methods
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
}
