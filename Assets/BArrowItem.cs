using UnityEngine;
using UnityEngine.UI;

public class BArrowItem : MonoBehaviour
{
    public int coin = 0;

    public Text text;

    public void ApplyCoin(int amount)
    {
        coin = amount;
        text.text = "" + coin;
    }
}
