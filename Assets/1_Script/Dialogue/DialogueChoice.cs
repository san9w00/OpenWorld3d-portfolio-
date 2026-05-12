using System;
using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string buttonText;

    [NonSerialized]
    public Action action;
}
