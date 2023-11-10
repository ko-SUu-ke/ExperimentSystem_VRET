using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputNumber : MonoBehaviour
{
    public int num;
    private GameObject obj;
    private SUDS suds;
   
    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("NumKeypad");
        suds = obj.GetComponent<SUDS>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendNumber()
    {
        suds.AddNum(num);
    }

    public void BackSpace()
    {
        suds.RemoveNum();
    }

    public void Determination()
    {
        suds.Determination();
    }
}
