using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
