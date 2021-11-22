using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Daniel Fors
//Secondary Author: Molly Röle

[CreateAssetMenu()]
public abstract class State : ScriptableObject
{
    protected StateMachine stateMachine;
    protected object owner;


    public void Initialize(StateMachine stateMachine, object owner)
    {
        this.stateMachine = stateMachine;
        this.owner = owner;
        Initialize();
    }
    public virtual void RunUpdate()
    {

    }

    public virtual void Enter() { }
    public virtual void Exit() { }

    protected virtual void Initialize() { }

}