using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class StoryNav : MonoBehaviour
{
    public Flowchart flowchart;
    public string nextBlock;
    public string prevBlock;
    public Button forwardButton;
    public Button backButton;

    void Awake()
    {
        forwardButton.onClick.AddListener(goForward);
        backButton.onClick.AddListener(goBack);
    }

    void Update()
    {
        nextBlock = flowchart.GetStringVariable("NextBlock");
        prevBlock = flowchart.GetStringVariable("PrevBlock");

        // Check if both RightChoice and LeftChoice are not empty
        bool hasRightChoice = !string.IsNullOrEmpty(flowchart.GetStringVariable("RightChoice"));
        bool hasLeftChoice = !string.IsNullOrEmpty(flowchart.GetStringVariable("LeftChoice"));

        // If both RightChoice and LeftChoice are present, disable the forward button
        if (hasRightChoice && hasLeftChoice)
        {
            forwardButton.gameObject.SetActive(true);
        }
        else
        {
            forwardButton.gameObject.SetActive(false);
        }
    }

    public void goForward()
    {
        //if forward button is pressed, call the next block
        flowchart.ExecuteBlock(nextBlock);
    }

    public void goBack()
    {
        //if back button is pressed, call the last block
        flowchart.ExecuteBlock(prevBlock);
    }
}
