using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SerializeDicTest : MonoBehaviour
{
    [Serializable]
    public class SerializeDicString : SerializeDictionary<string, string> { }

    public SerializeDicString dicString = new SerializeDicString();
}
