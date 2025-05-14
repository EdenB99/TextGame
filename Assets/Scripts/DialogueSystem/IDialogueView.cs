using System;
public interface IDialogueView
{
    // --- Presenter가 View에게 명령하는 메서드 ---
    void SetSpeakerName(string name); // 화자 이름을 설정합니다.
    void ShowMessage(string message); // 메시지를 타이핑 효과 없이 즉시 표시합니다.
    void AnimateMessage(string message, float charsPerSecond); // 메시지를 지정된 속도로 타이핑 효과와 함께 표시합니다.
    void ClearMessage(); // 화자 이름과 메시지 내용을 모두 지웁니다.
    void ShowView(bool show); // 전체 대화창 UI를 보이거나 숨깁니다.
    void SetFontSize(float size); // 메시지 텍스트의 폰트 크기를 설정합니다.
    void SetLineSpacing(float spacing); // 메시지 텍스트의 줄 간격(TMPro의 lineSpacing)을 설정합니다.
    void SetParagraphSpacing(float spacing); // 메시지 텍스트의 단락 간격(TMPro의 paragraphSpacing)을 설정합니다.

    // --- View가 Presenter에게 알리는 이벤트 ---
    event Action OnViewClicked; // 사용자가 뷰를 클릭/탭하여 다음으로 진행하길 원할 때 발생합니다. (텍스트 출력이 완료된 상태에서의 클릭)
    event Action OnTypingAnimationComplete; // 텍스트 타이핑 애니메이션이 완료되었을 때 발생합니다. (사용자가 클릭하여 즉시 완료한 경우 포함)
}