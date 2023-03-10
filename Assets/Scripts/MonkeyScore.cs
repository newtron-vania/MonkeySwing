using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyScore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Banana"){
            Destroy(other.gameObject);
            BananaCount.bananacount++;
        }
        else if(other.gameObject.tag == "Enemy"){
            //Destroy(other.gameObject);
            HeartCount.heartcount--;
            //Debug.Log(HeartCount.heartcount);
        }
        else if(other.gameObject.tag == "LineMid" || other.gameObject.tag == "LineTop"){
            //Destroy(other.gameObject);
            Distance.distance += 5;
        }
   }
}
