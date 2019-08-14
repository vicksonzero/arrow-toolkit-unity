using UnityEngine;
using UnityEngine.UI;

public class BArrowItem : MonoBehaviour
{
    public int coin = 0;
    public int combo = 0;
    public int level = 0;
    public int bounceLevel = 0;

    public Text text;

    public void ApplyCoin(int amount)
    {
        coin = amount;
        text.text = "" + coin;
    }

    public void ApplyStat(int combo, int level, int bounceLevel)
    {
        this.combo = combo;
        this.level = level;
        this.bounceLevel = bounceLevel;
    }
}
