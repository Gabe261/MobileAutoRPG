using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueUI : MonoBehaviour
{
    private VisualElement root;

    private Label label;
    private Button button1, button2, button3;

    private bool responseHit;
    
    [SerializeField] private string prompt, response1, response2, response3, promptMissed, promptHit;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        label = root.Q<Label>("Label");
        button1 = root.Q<Button>("Button1");
        button2 = root.Q<Button>("Button2");
        button3 = root.Q<Button>("Button3");

        button1.clicked += () => ButtonClick("That's Good!");
        button2.clicked += () => ButtonClick("Oh No!");
        button3.clicked += () => ButtonClick("*Tumble weed rolls past*");

        Hide();
    }

    public void Show()
    {
        Debug.Log("==================== Dialogue ONE");
        label.text = prompt;
        button1.text = response1;
        button2.text = response2;
        button3.text = response3;
        root.style.display = DisplayStyle.Flex;
    }

    public void Hide()
    {
        root.style.display = DisplayStyle.None;
    }
    
    private void ButtonClick(string response)
    {
        Debug.Log("==================== Dialogue Two");
        label.text = response;
        responseHit = true;
    }

    public void CloseConvo()
    {
        StartCoroutine(CloseConvoDelay());
    }

    private IEnumerator CloseConvoDelay()
    {
        Debug.Log("==================== Dialogue Three");
        button1.style.display = DisplayStyle.None;
        button2.style.display = DisplayStyle.None;
        button3.style.display = DisplayStyle.None;
        if (responseHit)
        {
            label.text = promptHit;
        }
        else
        {
            label.text = promptMissed;
        }
        yield return new WaitForSeconds(1f);
        Hide();
    }
}
