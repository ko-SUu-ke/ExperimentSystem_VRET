using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeText : MonoBehaviour
{
    public TextMeshProUGUI testText;
    // Start is called before the first frame update
    void Start()
    {
        testText.text = "Hello";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
