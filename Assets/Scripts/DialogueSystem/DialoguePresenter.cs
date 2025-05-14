using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePresenter
{
    private IDialogueView _view;
    private DialogueSequence _currentSequence;
    private int _currentLineIndex = -1;
    private float _charsPerSecond = 20f;

    public DialoguePresenter(IDialogueView view)
    {
        _view = view;
        _view.OnViewClicked += HandleViewClicked; // View가 다음으로 가도 된다고 할 때 호출됨
        _view.OnTypingAnimationComplete += HandleTypingComplete;
    }

    public void StartDialogue(DialogueSequence sequence)
    {
        if (sequence == null || sequence.lines.Length == 0)
        {
            Debug.LogWarning("DialoguePresenter: Sequence is null or empty.");
            EndDialogue();
            return;
        }

        _currentSequence = sequence;
        _currentLineIndex = -1;
        _view.ShowView(true);
        // TODO: View에 폰트 크기, 자간 등 초기 설정 적용
        ShowNextLine();
    }

    private void ShowNextLine()
    {
        _currentLineIndex++;
        if (_currentSequence != null && _currentLineIndex < _currentSequence.lines.Length)
        {
            DialogueLine line = _currentSequence.lines[_currentLineIndex];
            _view.SetSpeakerName(line.speakerName);
            _view.AnimateMessage(line.sentence, _charsPerSecond);
        }
        else
            EndDialogue();

    }

    // ▼▼▼▼▼ 여기가 핵심 수정 부분입니다 ▼▼▼▼▼
    private void HandleViewClicked()
    {
        // DialogueView의 OnPointerClick 핸들러에서 이미 다음 대사로 넘어갈 준비가 되었을 때만
        // OnViewClicked 이벤트가 발생하도록 수정되었습니다.
        // 따라서 이 메서드는 View가 "다음 대사로 진행해도 돼!"라고 알려줄 때 호출되므로,
        // 여기서는 단순히 ShowNextLine()만 호출하면 됩니다.
        ShowNextLine();
    }
    // ▲▲▲▲▲ 여기가 핵심 수정 부분입니다 ▲▲▲▲▲

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
        _currentLineIndex = -1;
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