using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Monitor : MonoBehaviour
{
    public TextMeshProUGUI testText;
    private int second = 0; //éûä‘Åiïb)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        second = (int)Math.Round(Time.time);
        var span = new TimeSpan(0, 0, second);
        string mmss = span.ToString(@"mm\:ss");

        testText.SetText("TIME : " + mmss) ;
    }
}
