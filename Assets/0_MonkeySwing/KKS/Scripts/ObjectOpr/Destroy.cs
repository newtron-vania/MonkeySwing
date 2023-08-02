using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public MakeLines lineGenerator;
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "LineBottom"){
            GameObject go = lineGenerator.lineQueue.Dequeue();
            Managers.Resource.Destroy(go, 1f);
            //Debug.Log("line destroy");
        }
        else if (other.CompareTag("Item"))
        {
            Debug.Log("Item Destroy!");
            Managers.Resource.Destroy(other.gameObject);
        }
    }
}
