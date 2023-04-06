using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_Right_Monkey : MonoBehaviour
{
    public Animator monkey;

    public void OnMouseDown()
    {
        Debug.Log("오른쪽 클릭");
        //monkey.SetTrigger("Right");
        StartCoroutine(RightMove_AfterDelay());
    }

    private IEnumerator RightMove_AfterDelay()
    {
        monkey.ResetTrigger("Right");
        yield return new WaitForSeconds(0.5f);
        monkey.SetTrigger("Right");
    }
    
}
