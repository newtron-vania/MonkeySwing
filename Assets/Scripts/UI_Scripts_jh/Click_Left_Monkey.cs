using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_Left_Monkey : MonoBehaviour
{
    public Animator monkey;

    public void OnMouseDown()
    {
        Debug.Log("¿ÞÂÊ Å¬¸¯");
        //monkey.SetTrigger("Right");
        StartCoroutine(LeftMove_AfterDelay());
    }

    private IEnumerator LeftMove_AfterDelay()
    {
        monkey.SetTrigger("Left");
        yield return new WaitForSeconds(0.5f);
        monkey.ResetTrigger("Left");
    }
}
