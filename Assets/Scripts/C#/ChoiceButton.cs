using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChoiceButton : MonoBehaviour
{
    public Button button;
    public Text label;

    void Awake()
    {
        button = GetComponent<Button>();
        label = GetComponentInChildren<Text>();
    }
}
