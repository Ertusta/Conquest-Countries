using UnityEngine;
using TMPro;

public class Baskent : MonoBehaviour
{
    public Player player;

    public int sanayiLvl = 1;
    public int argeLvl = 1;
    public int askerLvl = 1;

    public int sanayiCost = 100;
    public int argeCost = 50;
    public int askerCost = 200;

    // UI: Text alanlarý
    public TextMeshProUGUI sanayiLvlText;
    public TextMeshProUGUI argeLvlText;
    public TextMeshProUGUI askerLvlText;

    public TextMeshProUGUI sanayiCostText;
    public TextMeshProUGUI argeCostText;
    public TextMeshProUGUI askerCostText;

    public GameObject baskentPanel; 
    public TextMeshProUGUI buttonText;

    void Start()
    {
        UI_Guncelle();
    }

    public void SanayiYukselt()
    {
        if (player.money >= sanayiCost)
        {
            player.money -= sanayiCost;
            sanayiLvl += 1;
            player.multipler += 1;
            sanayiCost *= 2;
            UI_Guncelle();
        }
    }

    public void ArgeYukselt()
    {
        if (player.science >= argeCost)
        {
            player.science -= argeCost;
            argeLvl += 1;
            player.multipler += 1;
            argeCost *= 2;
            UI_Guncelle();
        }
    }

    public void AskerYukselt()
    {
        if (player.people >= askerCost)
        {
            player.people -= askerCost;
            askerLvl += 1;
            player.multipler += 1;
            askerCost *= 2;
            UI_Guncelle();
        }
    }

    void UI_Guncelle()
    {
        sanayiLvlText.text = "Level " + sanayiLvl;
        argeLvlText.text = "Level " + argeLvl;
        askerLvlText.text = "Level " + askerLvl;

        sanayiCostText.text = sanayiCost.ToString();
        argeCostText.text = argeCost.ToString();
        askerCostText.text = askerCost.ToString();
    }

    public void PanelToggle()
    {
        bool panelAcikMi = baskentPanel.activeSelf;

        baskentPanel.SetActive(!panelAcikMi);

        if (panelAcikMi)
        {
            buttonText.text = "Baþkent";
        }
        else
        {
            buttonText.text = "Kapat";
        }
    }
}
