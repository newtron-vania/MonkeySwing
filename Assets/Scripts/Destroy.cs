using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public MakeLines lineGenerator;
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "LineBottom"){
            GameObject go = lineGenerator.lineQueue.Dequeue();
            Object.Destroy(go, 1f);
            //Debug.Log("line destroy");
        }
    }
}
