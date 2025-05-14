using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System;


public class DialogueView : MonoBehaviour, IDialogueView, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("UI Elements")]
    [SerializeField] private GameObject dialoguePanelObject;
    [SerializeField] private TextMeshProUGUI speakerNameText;  // 화자 이름을 표시할 TextMeshPro UI
    [SerializeField] private TextMeshProUGUI messageText;      // 대화 내용을 표시할 TextMeshPro UI

    [Header("Typing Effect Settings")]
    [SerializeField] private float defaultCharsPerSecond = 20f; // 기본 타이핑 속도 (초당 글자 수)
    [SerializeField][Range(1f, 10f)] private float accelerationFactor = 3.0f; // 클릭 유지 시 타이핑 가속 배율

    [Header("Double Tap Settings")] // 더블 탭 관련 설정 추가
    [SerializeField] private float doubleClickThreshold = 0.3f; // 더블 클릭으로 간주할 최대 시간 간격 (초 단위)

    public event Action OnViewClicked;
    public event Action OnTypingAnimationComplete;

    private Coroutine _typingCoroutine;
    private bool _isTyping = false;     // 현재 타이핑 중인지 여부
    private string _fullMessageToType;  // 현재 타이핑 중인 전체 메시지 내용
    private float _currentCharsPerSecondSetting; // 현재 적용 중인 타이핑 속도
    private bool _isPointerDownOnView = false; // 사용자가 패널을 누르고 있는지 여부 (가속용)
    private float _lastClickTime = -1f; // 마지막 클릭 시간을 기록 (음수는 아직 클릭 없음을 의미)

    void Awake()
    {
        if (dialoguePanelObject == null || speakerNameText == null || messageText == null)
        {
            Debug.LogError("DialogueView: UI Elements are not assigned in the Inspector!");
            enabled = false;
            return;
        }
        Image panelImage = dialoguePanelObject.GetComponent<Image>();
        if (panelImage == null)
        {
            panelImage = dialoguePanelObject.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0);
        }
        if (!panelImage.raycastTarget)
        {
            panelImage.raycastTarget = true;
        }
        _currentCharsPerSecondSetting = defaultCharsPerSecond;
        ShowView(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDownOnView = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDownOnView = false;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        float currentTime = Time.time; // 현재 시간

        if (_isTyping) // 현재 텍스트가 타이핑 효과로 출력 중인 경우
        {
            if ((currentTime - _lastClickTime) <= doubleClickThreshold)
            {
                Debug.Log("Double tap detected while typing. Completing text.");
                StopTypingAnimation(true);
                _lastClickTime = -1f; // 더블 탭 처리 후 초기화 (연속 더블 탭 방지 또는 의도에 따라 조절)
            }
            else
                Debug.Log("Single tap detected while typing. Waiting for potential double tap.");
        }
        else // 현재 텍스트가 모두 출력된 상태인 경우
        {
            Debug.Log("Tap detected when text is complete. Proceeding to next line.");
            OnViewClicked?.Invoke();
        }
        _lastClickTime = currentTime;
    }

    public void SetSpeakerName(string name)
    {
        speakerNameText.text = name ?? "";
    }

    public void ShowMessage(string message)
    {
        StopTypingAnimation(false);
        messageText.text = message;
        _isTyping = false;
        OnTypingAnimationComplete?.Invoke();
    }

    public void AnimateMessage(string message, float charsPerSecond)
    {
        StopTypingAnimation(false);
        _fullMessageToType = message;
        _currentCharsPerSecondSetting = (charsPerSecond <= 0) ? defaultCharsPerSecond : charsPerSecond;
        _typingCoroutine = StartCoroutine(TypeTextRoutine(message));
    }

    private IEnumerator TypeTextRoutine(string message)
    {
        _isTyping = true;
        messageText.text = "";
        int charIndex = 0;

        while (charIndex < message.Length)
        {
            if (message[charIndex] == '<')
            {
                string tag = "<";
                charIndex++;
                while (charIndex < message.Length && message[charIndex] != '>')
                {
                    tag += message[charIndex];
                    charIndex++;
                }
                if (charIndex < message.Length && message[charIndex] == '>')
                {
                    tag += '>';
                    charIndex++;
                }
                messageText.text += tag;
            }
            else
            {
                messageText.text += message[charIndex];
                charIndex++;
            }

            float currentDelay = 1f / _currentCharsPerSecondSetting;
            if (_isPointerDownOnView)
            {
                currentDelay /= accelerationFactor;
            }
            if (currentDelay > 0) yield return new WaitForSeconds(currentDelay);
            else yield return null;
        }
        StopTypingAnimation(true);
    }

    private void StopTypingAnimation(bool triggerCompleteEvent)
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
        }
        if (_isTyping)
        {
            messageText.text = _fullMessageToType;
            _isTyping = false;
            if (triggerCompleteEvent)
            {
                OnTypingAnimationComplete?.Invoke();
            }
        }
    }
    public void ClearMessage()
    {
        StopTypingAnimation(false);
        speakerNameText.text = "";
        messageText.text = "";
    }

    public void ShowView(bool show)
    {
        dialoguePanelObject.SetActive(show);
        if (!show) _isPointerDownOnView = false;
    }

    public void SetFontSize(float size) { /* ... */ }
    public void SetLineSpacing(float spacing) { /* ... */ }
    public void SetParagraphSpacing(float spacing) { /* ... */ }
}