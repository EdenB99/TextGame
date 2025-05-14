using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialogueLine
{
    public string speakerName;
    [TextArea(3, 10)]
    public string sentence;

    // 필요에 따라 추가 정보들을 여기에 정의할 수 있습니다. 예를 들어:
    // public AudioClip voiceClip;          // 해당 대사의 음성 파일
    // public Sprite characterPortrait;    // 해당 대사를 말하는 캐릭터의 초상화 이미지
    // public float customTypingSpeed;    // 이 대사만 특별한 타이핑 속도를 가질 경우 (0 이하면 기본값 사용)
    // public bool waitForUserInput = true; // 이 대사 후 사용자의 입력을 기다릴지 여부 (자동 진행 대사 등을 위해)
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    public DialogueLine[] lines;
}