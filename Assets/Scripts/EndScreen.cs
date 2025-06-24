using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class EndScreen : MonoBehaviour
{
    private VisualElement root;

    public UnityEvent OnResetButton;

    [SerializeField] private PlayableDirector director;
    
    private Label title;
    private Button submitButton;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        title = root.Q<Label>("Title");
        submitButton = root.Q<Button>("Submit");

        submitButton.clicked += () => OnResetClicked();
        Hide();
    }

    public void WinState()
    {
        title.text = "You Won!";
        submitButton.text = "Play Again";
        Show();
    }

    public void LoseState()
    {
        title.text = "You Failed";
        submitButton.text = "Try Again";
        Show();
    }
    
    public void Show()
    {
        root.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        root.style.display = DisplayStyle.None;
    }

    private void OnResetClicked()
    {
        OnResetButton.Invoke();
        director.time = 0;
        Hide();
    }
}
