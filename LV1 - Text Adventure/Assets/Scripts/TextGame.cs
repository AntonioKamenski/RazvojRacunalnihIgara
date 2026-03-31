using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TextGame : MonoBehaviour
{
    [SerializeField] TMP_Text textTMP;
    [SerializeField] State startingState;

    State state;

    void Start()
    {
        state = startingState;
        textTMP.text = state.GetStateStory();
    }

    void Update()
    {
        ManageStates();
    }

    private void ManageStates() 
    {
        var nextStates = state.getNextStates();

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            state = nextStates[0];
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            state = nextStates[1];
        }

        textTMP.text = state.GetStateStory();
    }
}