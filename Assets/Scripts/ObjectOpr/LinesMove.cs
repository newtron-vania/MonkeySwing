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
        Debug.Log("Item Setting!");
        int count = 0;
        foreach(Banana banana in gameObject.GetComponentsInChildren<Banana>(true))
        {
            banana.gameObject.SetActive(true);
            count++;
        }
    }

}
