using UnityEngine;

public interface IDialogueHandler
{
    void TalkTo(Transform other);
    void BeingTalkedTo(Transform other);
    void ProcessDialogue();
    bool IsInDialogue();
}