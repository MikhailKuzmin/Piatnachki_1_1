using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Click : MonoBehaviour
{
    public Text num;
    // Start is called before the first frame update
    public void SelectButton()
    {
        num.GetComponent<Text>().text = GetComponentInChildren<Text>().text;
    }
}
