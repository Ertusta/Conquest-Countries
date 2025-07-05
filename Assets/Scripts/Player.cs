using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public int happiness = 100;
    public int people = 1000;
    public int money = 0;
    public int science = 0;
    public int soldier = 0;

    public int areaCount = 3;
    public int cost = 100;

    public TextMeshProUGUI happinessText;
    public TextMeshProUGUI peopleText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI scienceText;
    public TextMeshProUGUI solderText;

    public List<GameObject> goodAreas;
    public List<GameObject> badAreas;
    public List<GameObject> nearAreas;
    public List<GameObject> redAreas;
    public List<GameObject> greenAreas;

    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public GameObject taskPanel;

    public TextMeshProUGUI happinessText2;
    public TextMeshProUGUI peopleText2;
    public TextMeshProUGUI moneyText2;
    public TextMeshProUGUI scienceText2;
    public TextMeshProUGUI solderText2;

    public GameObject warSymbol;
    public List<GameObject> warSymbolss;

    public GameObject battlePanel;
    public TextMeshProUGUI ourSoldierText;
    public TextMeshProUGUI enemySoldierText;
    public TextMeshProUGUI winChanceText;


    private GameObject selectedArea;

    public GameObject warResultPanel;
    public TextMeshProUGUI warResultText;

    public GameObject greenLeader;
    public GameObject redLeader;




    void Start()
    {
        UpdateUI();
        InvokeRepeating("ProduceResources", 1f, 1f); 
        FindNearbyBadAreas();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (nearAreas.Contains(clickedObject))
                {
                    selectedArea = clickedObject; // Seçilen objeyi sakla

                    int enemy = cost;
                    int ours = soldier;
                    int chance = CalculateChance(ours, enemy);

                    // UI güncelle
                    ourSoldierText.text = "Bizim Askerimiz: " + ours;
                    enemySoldierText.text = "Düþmanýn Askeri: " + enemy;
                    winChanceText.text = "Kazanma ihtimalimiz: %" + chance;

                    // Paneli göster
                    battlePanel.SetActive(true);
                }
                else
                {
                    Debug.Log("Bu obje nearAreas listesinde deðil.");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            soldier *= 10;
            UpdateUI();
        }
    }


    public void FindNearbyBadAreas()
    {
        nearAreas.Clear(); // Önceki verileri temizle

        foreach (GameObject bad in badAreas)
        {
            Vector3 badPos = bad.transform.position;

            foreach (GameObject good in goodAreas)
            {
                Vector3 goodPos = good.transform.position;

                float distance = Vector3.Distance(badPos, goodPos);

                if (distance <= 1f) // 1 birimden yakýnsa
                {
                    if (!nearAreas.Contains(bad)) // Listeye tekrar ekleme
                    {
                        nearAreas.Add(bad);



                        Vector3 spawnPos = new Vector3(badPos.x, badPos.y, -9f);

                        bool alreadyExists = false;
                        foreach (GameObject symbol in warSymbolss)
                        {
                            if (Vector3.Distance(symbol.transform.position, spawnPos) < 0.1f)
                            {
                                alreadyExists = true;
                                break;
                            }
                        }

                        if (!alreadyExists)
                        {
                            GameObject symbol = Instantiate(warSymbol, spawnPos, Quaternion.Euler(90f, 90f, -90f));
                            warSymbolss.Add(symbol);
                        }




                    }
                    break;
                }
            }
        }
    }

    void ProduceResources()
    {
        
        int newPeople = areaCount*happiness*10;
        people += newPeople;

  
        int newMoney = areaCount*90;
        money += newMoney;

    
        int newScience = areaCount * 8;
        science += newScience;

     
        int newSolder = areaCount ;
        soldier += newSolder;

        UpdateUI();
    }

    void UpdateUI()
    {
        happinessText.text =happiness.ToString();
        peopleText.text = people.ToString();
        moneyText.text =money.ToString();
        scienceText.text =science.ToString();
        solderText.text = soldier.ToString();
    }


    public string ApplyEffect(Option option)
    {
        
        resultPanel.SetActive(true);
        resultText.text = option.result;

        taskPanel.SetActive(false);

        happinessText2.text = ((happiness * option.happiness) / 100).ToString();
        peopleText2.text = ((people * option.people) / 100).ToString();
        moneyText2.text = ((money * option.money) / 100).ToString();
        scienceText2.text = ((science * option.science) / 100).ToString();
        solderText2.text = ((soldier * option.soldier)/100).ToString();

        happiness = Mathf.Clamp(happiness + (happiness * option.happiness) / 100, 0, 100);
        people = Mathf.Clamp(people + (people * option.people) / 100, 0, int.MaxValue);
        money = Mathf.Clamp(money + (money * option.money) / 100, 0, int.MaxValue);
        science = Mathf.Clamp(science + (science * option.science) / 100, 0, int.MaxValue);
        soldier = Mathf.Clamp(soldier + (soldier * option.soldier) / 100, 0, int.MaxValue);

        
        

        return option.result;
    }
    // Add this utility method to the Player class
    private void RemoveWarSymbolForObject(GameObject obj)
    {
        Vector3 objPos = obj.transform.position;
        for (int i = warSymbolss.Count - 1; i >= 0; i--)
        {
            GameObject symbol = warSymbolss[i];
            Vector3 symbolPos = symbol.transform.position;
            if (Mathf.Approximately(symbolPos.x, objPos.x) && Mathf.Approximately(symbolPos.y, objPos.y))
            {
                Destroy(symbol);
                warSymbolss.RemoveAt(i);
                break;
            }
        }
    }

    int CalculateChance(int ours, int enemy)
    {
        if (ours >= 2 * enemy)
            return 100;
        else if (ours == enemy)
            return 50;
        else if (ours <= enemy / 2)
            return 0;
        else
            return Mathf.RoundToInt(100f * ours / (2f * enemy)); // Oranlama
    }

    public void CancelBattle()
    {
        battlePanel.SetActive(false);
        selectedArea = null;
    }

    public void AttemptBattle()
    {
        if (selectedArea == null) return;

        int enemy = cost;
        int chance = CalculateChance(soldier, enemy);
        int roll = Random.Range(0, 100);

        string resultMessage;

        if (roll < chance)
        {
            // Baþarýlý savaþ
            Renderer renderer = selectedArea.GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                renderer.material.color = Color.blue;
            }

            if (greenAreas.Contains(selectedArea))
                greenAreas.Remove(selectedArea);

            if (redAreas.Contains(selectedArea))
                redAreas.Remove(selectedArea);

            if (greenAreas.Count == 0 && greenLeader != null)
            {
                Destroy(greenLeader);
                greenLeader = null;
            }

            if (redAreas.Count == 0 && redLeader != null)
            {
                Destroy(redLeader);
                redLeader = null;
            }



            soldier -= cost;
            areaCount++;
            cost = ((areaCount+1) * cost) / areaCount;
            goodAreas.Add(selectedArea);
            badAreas.Remove(selectedArea);
            RemoveWarSymbolForObject(selectedArea);
            FindNearbyBadAreas();
            UpdateUI();

            resultMessage = "Zafer! Bölge ele geçirildi.";
        }
        else
        {
            soldier -= cost / 10;
            resultMessage = "Maalesef baþarýsýz olduk. Birliklerimizden bazýlarý kaybedildi.";
        }

        selectedArea = null;
        battlePanel.SetActive(false);

        // Sonuç panelini aç
        warResultPanel.SetActive(true);
        warResultText.text = resultMessage;
    }

    public void CloseWarResultPanel()
    {
        warResultPanel.SetActive(false);
    }




}
