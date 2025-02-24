using UnityEngine;
using UnityEngine.UI;

public class Script_BarMenu : MonoBehaviour
{
    [SerializeField] GameObject bar;
    [SerializeField] GameObject contentPrefab;
    [SerializeField] int amount = 20;

    public bool inMenu = false;

    private void Start()
    {
        Populate();
    }

    private void Populate()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newContent = Instantiate(contentPrefab, transform);
            newContent.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().color = Random.ColorHSV();
        }
    }

    public void Close()
    {
        this.gameObject.GetComponentInParent<ScrollRect>().gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Script_OtherControls>().ToggleCursor();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Script_OtherControls>().ToggleInput(true);
        bar.GetComponent<Script_Bar>().ReactivatePrompt();
        inMenu = false;
    }
}
