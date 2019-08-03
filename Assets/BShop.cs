using UnityEngine;

public class BShop : MonoBehaviour
{
    public KeyCode arrowShopKey = KeyCode.Alpha1;
    public ShopItem[] arrowShop = new ShopItem[]{
        new ShopItem("Arrow speed Lv1", 10),
        new ShopItem("Arrow speed Lv2", 50),
        new ShopItem("Arrow speed Lv3", 100),
    };


    BUI ui;


    public void Start()
    {
        ui = FindObjectOfType<BUI>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(arrowShopKey))
        {
            UpgradeArrow();
        }
    }

    public void UpgradeArrow()
    {
        var bow = FindObjectOfType<BBow>();
        if (bow && bow.level < 3)
        {
            bow.UpgradeArrow();
        }
    }

    public string GetShop()
    {
        var str = "";

        var bow = FindObjectOfType<BBow>();
        if (bow && bow.level < 3)
        {
            if (ui.Coins >= arrowShop[bow.level].cost * 0.7)
            {
                str += "<1> $" + arrowShop[bow.level].cost + "  " + arrowShop[bow.level].name;
            }
        }

        return str;

    }









    public struct ShopItem
    {
        public ShopItem(string _name, int _cost)
        {
            name = _name;
            cost = _cost;
        }
        public string name;
        public int cost;
    }

}