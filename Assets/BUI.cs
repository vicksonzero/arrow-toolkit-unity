using UnityEngine;
using UnityEngine.UI;

public class BUI : MonoBehaviour
{
    public Text text;
    private int coins = 0;
    private int maxCombo = 0;
    private BShop shop;


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
        get => coins;
        set
        {
            coins = value;
            UpdateText();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        shop = FindObjectOfType<BShop>();
    }

    // Update is called once per frame
    void UpdateText()
    {
        text.text = "Score: " + coins +
            "\nMax Combo: " + maxCombo +
            "\n" + shop.GetShop();
    }
}
