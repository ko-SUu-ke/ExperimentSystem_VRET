using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SUDS : MonoBehaviour
{
    public TextMeshProUGUI NumKeypad;
    private GameObject Monitor;
    private PresenSystem presenSystem;
    internal List<int> numList;
    internal int SUDSScore;
    internal List<int> SUDSs;


    // Start is called before the first frame update
    void Start()
    {
        numList = new List<int>();
        SUDSs = new List<int>();
        NumKeypad.SetText("0");
        Monitor = GameObject.Find("Monitor");
        presenSystem = Monitor.GetComponent<PresenSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(numList.Count > 0)
        {
            SUDSScore = int.Parse(numList.ConvertAll<string>(x => x.ToString()).Aggregate((x, y) => x + y));
            NumKeypad.SetText(SUDSScore.ToString());
        }
        else
        {
            NumKeypad.SetText("0");
        }
    }

    internal void AddNum(int num)
    {
        if(SUDSScore < 10)
        {
            numList.Add(num);
        }else if(SUDSScore == 10)
        {
            if(int.Parse(num.ToString()) == 0)
            {
                numList.Add(num);
            }
        }
    }

    internal void RemoveNum()
    {
        numList.RemoveAt(numList.Count - 1);
    }

    internal void Determination()
    {
        presenSystem.SUDSScores.Add(SUDSScore);
        numList = new List<int>();
        SUDSScore = 0;
        presenSystem.EndSUDS();
    }
}
