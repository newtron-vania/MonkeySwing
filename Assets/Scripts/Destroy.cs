using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "LineBottom"){
            Destroy(other.transform.parent.parent.gameObject, 1f);
            //Debug.Log("line destroy");
        }
        else if (other.gameObject){
            Destroy(other.gameObject, 0.5f);
        }
    }
}
