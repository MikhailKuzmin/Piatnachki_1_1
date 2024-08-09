using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAudio : MonoBehaviour
{
    public void Aud_Click()
    {
        GetComponent<AudioSource>().Play(0); 
    }
}
