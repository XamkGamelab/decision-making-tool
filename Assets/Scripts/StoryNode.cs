using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryNode", menuName = "ChooseYourAdventure/Story Node")]
public class StoryNode : ScriptableObject
{
    [TextArea]
    public string questionText;

    public string Button1Text = "1";
    public string Button2Text = "2";
    public string Button3Text = "3";
    public string Button4Text = "4";

    public StoryNode Node1;
    public StoryNode Node2;
    public StoryNode Node3;
    public StoryNode Node4;

    public bool showReturnToMenu;

    public bool showButton1 = true;
    public bool showButton2 = true;
    public bool showButton3 = true;
    public bool showButton4 = true;
}