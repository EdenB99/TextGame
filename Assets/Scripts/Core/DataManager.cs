using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public TextAsset data;
    // 대화 데이터 캐시
    public DialogueSequence dialogueSequence;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadDialogue();
    }

    /// <summary>
    /// Resources 폴더에서 지정한 json 파일을 읽어 DialogueSequence로 반환
    /// </summary>
    /// <param name="resourceName">Resources/ 이하의 파일명(확장자 제외)</param>
    public void LoadDialogue()
    {
        TextAsset dialoguedata = Resources.Load<TextAsset>("Dialogue"); 
        dialogueSequence = JsonUtility.FromJson<DialogueSequence>(dialoguedata.text);
        foreach (var VARIABLE in dialogueSequence.Dialogue)
        {
            Debug.Log($"ID: {VARIABLE.DialogueID}, Speaker: {VARIABLE.speakerName}, Sentence: {VARIABLE.sentence}, NextKey: {VARIABLE.nextKey}");
        }
    }

    // 필요하다면 다른 데이터 타입도 유사하게 추가 가능
}