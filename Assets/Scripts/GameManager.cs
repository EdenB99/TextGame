using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Dialogue System References")]
    [Tooltip("Scene에 있는 DialogueView 컴포넌트를 가진 GameObject를 할당해주세요.")]
    [SerializeField] private DialogueView dialogueViewInstance;
    [Tooltip("시작할 Dialogue Sequence 에셋을 할당해주세요.")]
    [SerializeField] private DialogueSequence testDialogueSequence;

    private DialoguePresenter _dialoguePresenter;

    void Start()
    {
        if (dialogueViewInstance == null)
        {
            Debug.LogError("GameManager: DialogueView instance is not assigned in the Inspector!");
            enabled = false; // GameManager 비활성화.
            return;
        }
        if (testDialogueSequence == null)
        {
            Debug.LogError("GameManager: Test Dialogue Sequence is not assigned in the Inspector!");
            // 대화 없이 게임을 진행할 수도 있으므로, enabled = false;는 주석 처리.
            // 필요에 따라 에러 처리 방식을 결정하세요.
        }

        // Presenter 생성 및 View 연결
        _dialoguePresenter = new DialoguePresenter(dialogueViewInstance);

        // (선택 사항) 특정 설정값들을 Presenter를 통해 View에 전달할 수 있습니다.
        // 예: _dialoguePresenter.SetTypingSpeed(25f);
        // 예: _dialoguePresenter.SetDefaultFontSize(30f); 
        // (이런 메서드들은 Presenter와 View 인터페이스에 추가해야 함)


        // 테스트를 위해 스페이스바 누르면 대화 시작 (또는 게임 시작 시 바로 실행)
        Debug.Log("GameManager initialized. Press Space to start dialogue, or implement auto-start.");
        // 만약 게임 시작 시 자동으로 대화를 시작하고 싶다면 아래 주석 해제
        // if (_dialoguePresenter != null && testDialogueSequence != null)
        // {
        //     _dialoguePresenter.StartDialogue(testDialogueSequence);
        // }
    }

    void Update()
    {
        // 테스트용: 스페이스바 입력 시 대화 시작 (이미 시작된 경우 중복 실행 방지 로직은 없음)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_dialoguePresenter != null && testDialogueSequence != null)
            {
                Debug.Log("Space pressed, starting dialogue...");
                _dialoguePresenter.StartDialogue(testDialogueSequence);
            }
            else if (testDialogueSequence == null)
            {
                Debug.LogWarning("Cannot start dialogue, Test Dialogue Sequence is not assigned.");
            }
        }
    }

    void OnDestroy()
    {
        // Presenter가 View의 이벤트를 구독하고 있으므로, GameManager 파괴 시 정리해주는 것이 좋습니다.
        _dialoguePresenter?.Cleanup();
    }
}