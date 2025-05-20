using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePresenter
{

    private IDialogueView _view;
    private DialogueData _currentDialogue;
    private DialogueSequence _currentSequence;
    public float _charsPerSecond = 20f;

    public DialoguePresenter(IDialogueView view)
    {
        _view = view;
        _view.OnViewClicked += HandleViewClicked; // View가 다음으로 가도 된다고 할 때 호출됨
        _view.OnTypingAnimationComplete += HandleTypingComplete;
    }
    public void StartDialogue(DialogueSequence sequence)
    {
        if (sequence == null || sequence.Dialogue == null || sequence.Dialogue.Count == 0)
        {
            Debug.LogWarning("DialoguePresenter: Sequence is null or empty.");
            EndDialogue();
            return;
        }

        _currentSequence = sequence;
        _currentDialogue = sequence.Dialogue[0]; // 첫 대사로 시작
        _view.ShowView(true);
        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (_currentDialogue == null)
        {
            EndDialogue();
            return;
        }
        _view.SetSpeakerName(_currentDialogue.speakerName);
        _view.AnimateMessage(_currentDialogue.sentence, _charsPerSecond);
    }

    private void HandleViewClicked()
    {
        // 현재 대사의 nextKey를 이용해 다음 대사로 이동
        if (_currentDialogue != null && !string.IsNullOrEmpty(_currentDialogue.nextKey))
        {
            _currentDialogue = _currentSequence.Dialogue.Find(d => d.DialogueID == _currentDialogue.nextKey);
            ShowCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }
    private void HandleTypingComplete()
    {
        // 이 이벤트는 View에서 현재 줄의 텍스트 출력이 (애니메이션이든, 사용자의 즉시 완료 요청이든)
        // 완전히 끝났을 때 호출됩니다.
        // 여기서 "다음으로 진행 가능"을 나타내는 시각적 표시(예: 깜빡이는 화살표 아이콘)를
        // View에 지시하는 등의 추가 로직을 넣을 수 있습니다.
        Debug.Log("DialoguePresenter: Typing complete for current line.");
        // 예시: _view.ShowNextIndicator(true); // (IDialogueView와 DialogueView에 이 메서드 추가 필요)
    }

    public void EndDialogue()
    {
        _view.ShowView(false);
        _view.ClearMessage();
        _currentSequence = null;
        Debug.Log("Dialogue Ended.");
        // TODO: 대화 종료 후 콜백 또는 이벤트 발생
    }

    public void SetTypingSpeed(float charsPerSecond)
    {
        _charsPerSecond = charsPerSecond;
    }

    public void Cleanup()
    {
        if (_view != null)
        {
            _view.OnViewClicked -= HandleViewClicked;
            _view.OnTypingAnimationComplete -= HandleTypingComplete;
        }
    }
}