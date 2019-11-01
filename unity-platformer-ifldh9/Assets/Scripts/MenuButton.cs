using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    private TextMeshProUGUI text;
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

   public void SetColor()
    {
       text.color=new Color32(29,30,80,255);
    }

   public void SetBackColor()
    {
        text.color = new Color32(255,255,255,255);
    }



}
