using RenownedGames.AITree;
using UnityEngine;

[NodeContent("Instantiate Dialogue", "Tasks/Base/Army/Instantiate Dialogue", IconPath = "Images/Icons/Node/SendMessageIcon.png")]
public class InstantiateDialogueTask : TaskNode
{
    [Header("Variables")]
    [SerializeField]
    private TransformKey target;

    // Stored required components.
    private Transform transform;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        transform = GetOwner().transform;
    }

    protected override void OnEntry()
    {
        base.OnEntry();
    }

    protected override State OnUpdate()
    {
        DialogueHandler dialogueHandler = target.GetValue().GetComponent<DialogueHandler>();
        if (!dialogueHandler)
        {
            return State.Failure;
        }

        // force start dialogue and pass themself
        dialogueHandler.InstantiateDialogue(transform);

        return State.Success;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }
}
