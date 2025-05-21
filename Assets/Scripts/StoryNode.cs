using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single node in the branching story system. Each node displays a  scenario, 
/// a set of response options, and can be linked to other nodes for progression.
/// </summary>
[CreateAssetMenu(fileName = "NewStoryNode", menuName = "Scenarios/Story Node")]
public class StoryNode : ScriptableObject
{
    [Header("Node Content")]
    [Tooltip("An identifier that the backend will use to recognize the node.")]
    public string nodeId;
    [Tooltip("The main text or question shown to the player for this story node.")]
    [TextArea] public string questionText;

    [Tooltip("Optional image to visually support the node's content.")]
    public Sprite nodeImage;

    [Tooltip("If true, the nodeImage will be shown in the UI. True by default.")]
    public bool showImage = true;

    [System.Serializable]
    public class StoryOption
    {
        [Tooltip("The label that will be displayed on the choice button.")]
        public string optionText = "Option";

        [Tooltip("The next StoryNode to transition to when this option is selected.")]
        public StoryNode nextNode;

        [Tooltip("If false, this option will not be shown in the UI.")]
        public bool isVisible = true;
    }

    [Tooltip("List of response options the player can choose from. Can be any length.")]
    [SerializeField]
    private List<StoryOption> options = new();

    /// <summary>
    /// Read-only access to the list of story options.
    /// Use this instead of modifying the list directly to preserve integrity.
    /// </summary>
    public IReadOnlyList<StoryOption> Options => options;

    [Header("Node Behavior")]
    [Tooltip("If true, shows a button that returns the player to the main menu.")]
    public bool showReturnToMenu;
}