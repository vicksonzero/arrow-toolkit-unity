using UnityEngine;
using UnityEngine.UI;

public class BUI : MonoBehaviour
{
    public Text text;
    public Text hintText;
    private int coins = 0;
    private int maxCombo = 0;
    private BShop shop;

    public string[] tutContent;
    public int[] tutScoreReq;


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
            "\n";

        var tutString = "";
        for (int i = 0; i < tutScoreReq.Length; i++)
        {
            if (Coins >= tutScoreReq[i])
            {
                tutString = tutContent[i];
            }
        }
        hintText.text = tutString;
    }
}
