using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public string DialogueID;
    public string speakerName;
    public string sentence;
    public string nextKey; 
    public string eventKey;
}
[System.Serializable]
public class DialogueSequence
{
    public List<DialogueData> Dialogue;
}
