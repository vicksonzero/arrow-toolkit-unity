using UnityEngine;
using UnityEngine.UI;

public class BUI : MonoBehaviour
{
    public Text text;
    private int coins = 0;
    private int maxCombo = 0;

    public int MaxCombo
    {
        get => maxCombo;
        set
        {
            maxCombo = value;
            UpdateText();
        }
    }
    public int Coins
    {
        get => coins; set
        {
            coins = value;
            UpdateText();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void UpdateText()
    {
        text.text = "Coins: " + coins +
            "\nMax Combo: " + maxCombo;
    }
}
