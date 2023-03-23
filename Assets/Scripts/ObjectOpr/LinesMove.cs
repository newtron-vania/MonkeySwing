using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesMove : MonoBehaviour
{
    public float speed = 2;
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
        // Debug.Log("movemove");
    }

    private void OnEnable()
    {
        foreach(Banana banana in gameObject.GetComponentsInChildren<Banana>())
        {
            banana.gameObject.SetActive(true);
        }
    }


}
