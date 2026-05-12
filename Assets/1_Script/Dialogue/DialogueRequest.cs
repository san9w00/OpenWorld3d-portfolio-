using System.Collections.Generic;
using System;

public class DialogueRequest
{
    public string text;

    public List<DialogueChoice> choices;

    public Action onContinue;
}
