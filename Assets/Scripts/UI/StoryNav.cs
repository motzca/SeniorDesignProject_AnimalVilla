using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class StoryNav : MonoBehaviour
{
    public Flowchart flowchart;
    public Button forwardButton;
    public Button backButton;

    private string nextBlock;
    private string prevBlock;

    void Awake()
    {
        forwardButton.onClick.AddListener(GoForward);
        backButton.onClick.AddListener(GoBack);
    }

void Start()
{
    ClearChoiceVariables();
    UpdateButtonState();
}

    void Update()
    {
        nextBlock = flowchart.GetStringVariable("NextBlock");
        prevBlock = flowchart.GetStringVariable("PrevBlock");

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        bool hasRightChoice = !string.IsNullOrEmpty(flowchart.GetStringVariable("RightChoice"));
        bool hasLeftChoice = !string.IsNullOrEmpty(flowchart.GetStringVariable("LeftChoice"));

        forwardButton.interactable = !(hasRightChoice && hasLeftChoice);
    }

    public void GoForward()
    {
        if (!string.IsNullOrEmpty(nextBlock) && flowchart.HasBlock(nextBlock))
        {
            flowchart.ExecuteBlock(nextBlock);
        }
        else
        {
            Debug.LogWarning($"Next block '{nextBlock}' does not exist or is not set.");
        }
    }

    public void GoBack()
    {
        if (!string.IsNullOrEmpty(prevBlock) && flowchart.HasBlock(prevBlock))
        {
            flowchart.ExecuteBlock(prevBlock);
        }
        else
        {
            Debug.LogWarning($"Previous block '{prevBlock}' does not exist or is not set.");
        }
    }

    public void ClearChoiceVariables()
    {
        flowchart.SetStringVariable("LeftActionQuote", "");
        flowchart.SetStringVariable("LeftChoice", "");
        flowchart.SetStringVariable("RightActionQuote", "");
        flowchart.SetStringVariable("RightChoice", "");
    }
}
