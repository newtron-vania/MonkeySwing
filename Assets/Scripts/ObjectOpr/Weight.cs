using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Weight : MonoBehaviour
{
    [SerializeField] Slider WeightSlider;
    [SerializeField] protected MonkeyController monkey;
    //TextMeshProUGUI weightText;

    private void Start()
    {
        StartCoroutine("FindMonkey");
        //weightText = this.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        WeightSlider.value = monkey.Weight;
        // weightText.text = $"Weight : {monkey.weight}";
    }

    IEnumerator FindMonkey()
    {
        while (GameObject.FindWithTag("Monkey") == null)
        {
            yield return new WaitForFixedUpdate();
        }
        monkey = GameObject.FindWithTag("Monkey").GetComponent<MonkeyController>();
        //Debug.Log($"{this.transform.name} find {monkey.transform.name}");
    }
}
