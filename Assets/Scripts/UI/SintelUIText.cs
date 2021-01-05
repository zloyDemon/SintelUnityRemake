using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SintelUIText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPro;

    private string text;

    public string Text { get { return text; } set { text = value; textPro.text = text; } }
}
