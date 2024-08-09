using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell_Click : MonoBehaviour
{
    public Text h;
    public Text w;
    public Text num;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectButton()
    {
        num.GetComponent<Text>().text = GetComponentInChildren<Text>().text;

    }
}
