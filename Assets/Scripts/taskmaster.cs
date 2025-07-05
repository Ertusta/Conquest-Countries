using UnityEngine;
using TMPro;

public class taskmaster : MonoBehaviour
{
    public GameObject taskPanel;
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI optionAText;
    public TextMeshProUGUI optionBText;
    public TextMeshProUGUI optionCText;

    public Player player;  // Player objesine inspector'dan atayacaksýn
    public GameObject resultPanel;

    private TaskList taskList;
    private Task currentTask;

    void Start()
    {
        taskPanel.SetActive(false);
        LoadTasksFromJson();
        ShowRandomTask();
    }

    void LoadTasksFromJson()
    {
        string jsonString = @"
        {
            ""tasks"": " + TasksData.Json + @"
        }";

        taskList = JsonUtility.FromJson<TaskList>(jsonString);
        Debug.Log("Görevler yüklendi.Toplam görev sayýsý: " + taskList.tasks.Length);
    }

    public void ShowRandomTask()
    {
        if (taskList == null || taskList.tasks.Length == 0) return;
        int randomIndex = Random.Range(0, taskList.tasks.Length);
        ShowTaskById(taskList.tasks[randomIndex].id);
    }

    public void ShowTaskById(int id)
    {
        currentTask = System.Array.Find(taskList.tasks, t => t.id == id);
        if (currentTask == null) return;

        taskPanel.SetActive(true);
        taskText.text = currentTask.task;
        optionAText.text = currentTask.options[0].text;
        optionBText.text = currentTask.options[1].text;
        optionCText.text = currentTask.options[2].text;
    }

    public void OnClickOptionA() => SelectOption(0);
    public void OnClickOptionB() => SelectOption(1);
    public void OnClickOptionC() => SelectOption(2);

    void SelectOption(int optionIndex)
    {
        if (currentTask == null) return;
        var option = currentTask.options[optionIndex];

        string resultMessage = player.ApplyEffect(option);
        // Eðer sonuç panelinde mesaj gösteriyorsan buraya ekleyebilirsin
    }

    public void closeResult()
    {
        Debug.Log("Sonuç paneli kapatýlýyor.");
        resultPanel.SetActive(false);
        taskPanel.SetActive(false);
        Invoke("ShowRandomTask", 10f);
    }
}

[System.Serializable]
public class TaskList
{
    public Task[] tasks;
}

[System.Serializable]
public class Task
{
    public int id;
    public string task;
    public Option[] options;
}

[System.Serializable]
public class Option
{
    public string id;
    public string text;
    public int happiness;
    public int people;
    public int money;
    public int science;
    public int soldier;
    public string result;
}

// Gömülü JSON burada saklanýr
public static class TasksData
{
    public static string Json = @"
[
  {
    ""id"": 1,
    ""task"": ""Enflasyon yüksek. Ne yapacaksýn?"",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Vergileri artýr"", ""happiness"": -30, ""people"": 0, ""money"": 20, ""science"": 0, ""soldier"": 0, ""result"": ""Halk bunu beðenmedi. Tepkiler arttý."" },
      { ""id"": ""B"", ""text"": ""Para bas"", ""happiness"": -10, ""people"": 0, ""money"": 30, ""science"": 0, ""soldier"": 0, ""result"": ""Kýsa vadede rahatlama oldu ama güven azaldý."" },
      { ""id"": ""C"", ""text"": ""Doðal kaynaklarý sat"", ""happiness"": 0, ""people"": 0, ""money"": 10, ""science"": -20, ""soldier"": 0, ""result"": ""Ek gelir saðlandý ama bilim çevreleri tepki gösterdi."" }
    ]
  },
  {
    ""id"": 2,
    ""task"": ""Ýsyan baþladý. Ne yapacaksýn?"",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Asker gönder"", ""happiness"": -40, ""people"": -10, ""money"": -20, ""science"": 0, ""soldier"": 10, ""result"": ""Ýsyan bastýrýldý ama halk öfkelendi."" },
      { ""id"": ""B"", ""text"": ""Müzakere et"", ""happiness"": 20, ""people"": 0, ""money"": -10, ""science"": 0, ""soldier"": 0, ""result"": ""Barýþ saðlandý, halk güven duydu."" },
      { ""id"": ""C"", ""text"": ""Umursama"", ""happiness"": -60, ""people"": -30, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Ýsyan büyüdü. Halk güvenini kaybetti."" }
    ]
  },
  {
    ""id"": 3,
    ""task"": ""Nüfus hýzla artýyor. Ne yapacaksýn?"",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Yeni þehirler kur"", ""happiness"": 10, ""people"": 30, ""money"": -40, ""science"": 0, ""soldier"": 0, ""result"": ""Nüfus yerleþtirildi ama bütçe zorlandý."" },
      { ""id"": ""B"", ""text"": ""Göçü sýnýrla"", ""happiness"": -20, ""people"": -30, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Tepkiler geldi. Nüfus politikasý tartýþýldý."" },
      { ""id"": ""C"", ""text"": ""Sosyal yardým artýr"", ""happiness"": 30, ""people"": 20, ""money"": -20, ""science"": 0, ""soldier"": 0, ""result"": ""Halk memnun ama ekonomi zorlandý."" }
    ]
  },
  {
    ""id"": 4,
    ""task"": ""Bilim insanlarý yeni teknoloji öneriyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Hemen yatýrým yap"", ""happiness"": 0, ""people"": 0, ""money"": -50, ""science"": 40, ""soldier"": 0, ""result"": ""Bilim geliþti ama kasada açýk oluþtu."" },
      { ""id"": ""B"", ""text"": ""Gözlemle ve bekle"", ""happiness"": 0, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Risk alýnmadý, durum sabit kaldý."" },
      { ""id"": ""C"", ""text"": ""Reddet"", ""happiness"": -10, ""people"": 0, ""money"": 0, ""science"": -20, ""soldier"": 0, ""result"": ""Bilim insanlarý tepki gösterdi."" }
    ]
  },
  {
    ""id"": 5,
    ""task"": ""Ýklim krizi uyarýsý alýndý."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Çevre dostu yatýrýmlar yap"", ""happiness"": 10, ""people"": 0, ""money"": -30, ""science"": 20, ""soldier"": 0, ""result"": ""Halk çevreci adýmlarý takdir etti."" },
      { ""id"": ""B"", ""text"": ""Umursama"", ""happiness"": -30, ""people"": 0, ""money"": 0, ""science"": -10, ""soldier"": 0, ""result"": ""Doða tahrip oldu, halk kýzgýn."" },
      { ""id"": ""C"", ""text"": ""Karbon vergisi koy"", ""happiness"": -20, ""people"": 0, ""money"": 15, ""science"": 10, ""soldier"": 0, ""result"": ""Ek gelir saðlandý ama sanayi þikayetçi."" }
    ]
  },
  {
    ""id"": 6,
    ""task"": ""Yapay zeka iþ gücünü tehdit ediyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Yapay zekayý yasakla"", ""happiness"": 10, ""people"": 0, ""money"": -10, ""science"": -30, ""soldier"": 0, ""result"": ""Ýþçiler memnun, teknoloji sektörü öfkeli."" },
      { ""id"": ""B"", ""text"": ""Ýnsanlara yeni iþler öðret"", ""happiness"": 30, ""people"": 0, ""money"": -20, ""science"": 10, ""soldier"": 0, ""result"": ""Toplum dönüþüme hazýr hale geldi."" },
      { ""id"": ""C"", ""text"": ""Umursama"", ""happiness"": -20, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Ýþsizlik arttý, halk huzursuz."" }
    ]
  },
  {
    ""id"": 7,
    ""task"": ""Ordu bütçesi tartýþýlýyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Bütçeyi artýr"", ""happiness"": -10, ""people"": 0, ""money"": -30, ""science"": 0, ""soldier"": 30, ""result"": ""Ordu güçlendi ama halk tepkili."" },
      { ""id"": ""B"", ""text"": ""Mevcut haliyle býrak"", ""happiness"": 0, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Durum deðiþmedi. Tartýþmalar sürdü."" },
      { ""id"": ""C"", ""text"": ""Ordu bütçesini kes"", ""happiness"": 10, ""people"": 0, ""money"": 20, ""science"": 0, ""soldier"": -20, ""result"": ""Halk mutlu ama askerler hoþnutsuz."" }
    ]
  },
  {
    ""id"": 8,
    ""task"": ""Gençler iþsiz."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Yeni fabrikalar aç"", ""happiness"": 20, ""people"": 10, ""money"": -40, ""science"": 0, ""soldier"": 0, ""result"": ""Ýstihdam arttý, ekonomi canlandý."" },
      { ""id"": ""B"", ""text"": ""Giriþimcilik hibesi ver"", ""happiness"": 10, ""people"": 0, ""money"": -20, ""science"": 10, ""soldier"": 0, ""result"": ""Giriþimcilik desteklendi. Yenilikçi çözümler doðdu."" },
      { ""id"": ""C"", ""text"": ""Umursama"", ""happiness"": -30, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Gençler protesto etti. Toplumsal huzur bozuldu."" }
    ]
  },
  {
    ""id"": 9,
    ""task"": ""Halk sosyal medya kýsýtlamasý istiyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Sansür uygula"", ""happiness"": -40, ""people"": 0, ""money"": 0, ""science"": -10, ""soldier"": 0, ""result"": ""Özgürlükler kýsýtlandý. Tepkiler büyüdü."" },
      { ""id"": ""B"", ""text"": ""Kýsýtlamayý reddet"", ""happiness"": 10, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Özgürlük korundu. Gençler destek verdi."" },
      { ""id"": ""C"", ""text"": ""Geliþmiþ denetim sistemi kur"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 5, ""soldier"": 0, ""result"": ""Denge saðlandý ama maliyetli oldu."" }
    ]
  },
  {
    ""id"": 10,
    ""task"": ""Ülkede enerji krizi baþladý."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Nükleer enerjiye geç"", ""happiness"": -10, ""people"": 0, ""money"": -50, ""science"": 30, ""soldier"": 0, ""result"": ""Enerji saðlandý ama halk korktu."" },
      { ""id"": ""B"", ""text"": ""Elektrik kýsýtlamasý yap"", ""happiness"": -50, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Halk karanlýkta kaldý. Tepkiler büyüdü."" },
      { ""id"": ""C"", ""text"": ""Yenilenebilir enerjiye yatýrým yap"", ""happiness"": 10, ""people"": 0, ""money"": -30, ""science"": 15, ""soldier"": 0, ""result"": ""Gelecek için umut verici bir adým."" }
    ]
  }
]
";
}

