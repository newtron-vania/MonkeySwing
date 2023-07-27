using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StateManager<T> where T : State
{
    public void ChangeState(T SkinState);

}
