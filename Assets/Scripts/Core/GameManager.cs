using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Dialogue System References")]
    [SerializeField] private DialogueView dialogueViewInstance;
    private DialoguePresenter _dialoguePresenter;

    void Start()
    {
        if (dialogueViewInstance == null)
        {
            enabled = false;
            return;
        }
        _dialoguePresenter = new DialoguePresenter(dialogueViewInstance);
        Debug.Log("GameManager initialized. Press Space to start dialogue.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_dialoguePresenter != null && DataManager.Instance.dialogueSequence != null)
            {
                Debug.Log("Space pressed, starting dialogue...");
                _dialoguePresenter.StartDialogue(DataManager.Instance.dialogueSequence);
            }
            else
            {
                Debug.LogWarning("Cannot start dialogue, DialogueSequence is not assigned.");
            }
        }
    }

    void OnDestroy()
    {
        _dialoguePresenter?.Cleanup();
    }
}