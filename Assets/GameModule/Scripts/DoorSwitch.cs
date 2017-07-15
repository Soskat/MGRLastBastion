using LastBastion.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that represents door switcher behaviour.
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class DoorSwitch : MonoBehaviour, IInteractiveObject {

    #region Private fields
    [SerializeField] private Door door;
    private Animator animator;
    private int pushButtonTrigger;
    private bool isBusy = false;
    #endregion


    #region MonoBehaviour methods
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        pushButtonTrigger = Animator.StringToHash("PushButton");
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion


    #region Public methods
    /// <summary>
    /// Implementation of <see cref="IInteractiveObject"/> interface method.
    /// </summary>
    public void Interact()
    {
        if (!isBusy)
        {
            animator.SetTrigger(pushButtonTrigger);
            Debug.Log("Pushed the button!");
        }
    }

    /// <summary>
    /// Sets isBusy flag to true.
    /// </summary>
    public void SetBusyOn()
    {
        isBusy = true;
    }

    /// <summary>
    /// Sets isBusy flag to false.
    /// </summary>
    public void SetBusyOff()
    {
        isBusy = false;
    }

    /// <summary>
    /// Switches watched door state.
    /// </summary>
    public void SwitchDoorState()
    {
        door.IsLocked = door.IsLocked ? false : true;
    }
    #endregion
}
