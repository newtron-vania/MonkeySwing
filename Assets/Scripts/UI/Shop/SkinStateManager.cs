using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinStateManager : MonoBehaviour, StateManager<SkinState>
{
    public SkinState CurrentState, AnotherState;

    private SkinState temp;

    public void ChangeState(SkinState _SkinState)
    {
        bool isNotNull = CurrentState != null && AnotherState != null;
        if (isNotNull && CurrentState.SkinStateName != _SkinState.SkinStateName)
        {
            _SkinState.On();
            CurrentState.Off();

            // �� State �ٲٱ�
            AnotherState = CurrentState;
            CurrentState = _SkinState;

        }
        else
        {
            //�ƹ��͵� ����
        }
    }
}
