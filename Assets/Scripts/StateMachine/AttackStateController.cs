using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class AttackStateController : MonoBehaviour
{
    #region Variables

    public delegate void OnEnterAttackState();
    public delegate void OnExitAttackState();

    public OnEnterAttackState enterAttackStateHandler;
    public OnExitAttackState exitAttackStateHandler;

    #endregion Variables

    public bool IsInAttackState
    {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        enterAttackStateHandler = new OnEnterAttackState(EnterAttackState);
        exitAttackStateHandler = new OnExitAttackState(ExitAttackState);
    }

    public void OnEnterOfAttackstate()
    {
        IsInAttackState = true;
        EnterAttackState();
    }

    public void OnEndOfAttackState()
    {
        IsInAttackState = false;
        ExitAttackState();
    }

    private void EnterAttackState()
    {

    }

    private void ExitAttackState()
    {

    }

    public void OnCheckAttackCollider(int attackIndex)
    {
        GetComponent<IAttackable>()?.OnExcuteAttack(attackIndex);
    }


}
