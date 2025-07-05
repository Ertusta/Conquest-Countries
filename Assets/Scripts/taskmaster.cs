using UnityEngine;
using TMPro;

public class taskmaster : MonoBehaviour
{
    public GameObject taskPanel;
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI optionAText;
    public TextMeshProUGUI optionBText;
    public TextMeshProUGUI optionCText;

    public Player player;  // Player objesine inspector'dan atayacaks�n
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
        Debug.Log("G�revler y�klendi.Toplam g�rev say�s�: " + taskList.tasks.Length);
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
        // E�er sonu� panelinde mesaj g�steriyorsan buraya ekleyebilirsin
    }

    public void closeResult()
    {
        Debug.Log("Sonu� paneli kapat�l�yor.");
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

// G�m�l� JSON burada saklan�r
public static class TasksData
{
    public static string Json = @"
[
  {
    ""id"": 1,
    ""task"": ""Enflasyon y�ksek. Ne yapacaks�n?"",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Vergileri art�r"", ""happiness"": -30, ""people"": 0, ""money"": 20, ""science"": 0, ""soldier"": 0, ""result"": ""Halk bunu be�enmedi. Tepkiler artt�."" },
      { ""id"": ""B"", ""text"": ""Para bas"", ""happiness"": -10, ""people"": 0, ""money"": 30, ""science"": 0, ""soldier"": 0, ""result"": ""K�sa vadede rahatlama oldu ama g�ven azald�."" },
      { ""id"": ""C"", ""text"": ""Do�al kaynaklar� sat"", ""happiness"": 0, ""people"": 0, ""money"": 10, ""science"": -20, ""soldier"": 0, ""result"": ""Ek gelir sa�land� ama bilim �evreleri tepki g�sterdi."" }
    ]
  },
  {
    ""id"": 2,
    ""task"": ""�syan ba�lad�. Ne yapacaks�n?"",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Asker g�nder"", ""happiness"": -40, ""people"": -10, ""money"": -20, ""science"": 0, ""soldier"": 10, ""result"": ""�syan bast�r�ld� ama halk �fkelendi."" },
      { ""id"": ""B"", ""text"": ""M�zakere et"", ""happiness"": 20, ""people"": 0, ""money"": -10, ""science"": 0, ""soldier"": 0, ""result"": ""Bar�� sa�land�, halk g�ven duydu."" },
      { ""id"": ""C"", ""text"": ""Umursama"", ""happiness"": -60, ""people"": -30, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""�syan b�y�d�. Halk g�venini kaybetti."" }
    ]
  },
  {
    ""id"": 3,
    ""task"": ""N�fus h�zla art�yor. Ne yapacaks�n?"",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Yeni �ehirler kur"", ""happiness"": 10, ""people"": 30, ""money"": -40, ""science"": 0, ""soldier"": 0, ""result"": ""N�fus yerle�tirildi ama b�t�e zorland�."" },
      { ""id"": ""B"", ""text"": ""G��� s�n�rla"", ""happiness"": -20, ""people"": -30, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Tepkiler geldi. N�fus politikas� tart���ld�."" },
      { ""id"": ""C"", ""text"": ""Sosyal yard�m art�r"", ""happiness"": 30, ""people"": 20, ""money"": -20, ""science"": 0, ""soldier"": 0, ""result"": ""Halk memnun ama ekonomi zorland�."" }
    ]
  },
  {
    ""id"": 4,
    ""task"": ""Bilim insanlar� yeni teknoloji �neriyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Hemen yat�r�m yap"", ""happiness"": 0, ""people"": 0, ""money"": -50, ""science"": 40, ""soldier"": 0, ""result"": ""Bilim geli�ti ama kasada a��k olu�tu."" },
      { ""id"": ""B"", ""text"": ""G�zlemle ve bekle"", ""happiness"": 0, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Risk al�nmad�, durum sabit kald�."" },
      { ""id"": ""C"", ""text"": ""Reddet"", ""happiness"": -10, ""people"": 0, ""money"": 0, ""science"": -20, ""soldier"": 0, ""result"": ""Bilim insanlar� tepki g�sterdi."" }
    ]
  },
  {
    ""id"": 5,
    ""task"": ""�klim krizi uyar�s� al�nd�."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""�evre dostu yat�r�mlar yap"", ""happiness"": 10, ""people"": 0, ""money"": -30, ""science"": 20, ""soldier"": 0, ""result"": ""Halk �evreci ad�mlar� takdir etti."" },
      { ""id"": ""B"", ""text"": ""Umursama"", ""happiness"": -30, ""people"": 0, ""money"": 0, ""science"": -10, ""soldier"": 0, ""result"": ""Do�a tahrip oldu, halk k�zg�n."" },
      { ""id"": ""C"", ""text"": ""Karbon vergisi koy"", ""happiness"": -20, ""people"": 0, ""money"": 15, ""science"": 10, ""soldier"": 0, ""result"": ""Ek gelir sa�land� ama sanayi �ikayet�i."" }
    ]
  },
  {
    ""id"": 6,
    ""task"": ""Yapay zeka i� g�c�n� tehdit ediyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Yapay zekay� yasakla"", ""happiness"": 10, ""people"": 0, ""money"": -10, ""science"": -30, ""soldier"": 0, ""result"": ""���iler memnun, teknoloji sekt�r� �fkeli."" },
      { ""id"": ""B"", ""text"": ""�nsanlara yeni i�ler ��ret"", ""happiness"": 30, ""people"": 0, ""money"": -20, ""science"": 10, ""soldier"": 0, ""result"": ""Toplum d�n���me haz�r hale geldi."" },
      { ""id"": ""C"", ""text"": ""Umursama"", ""happiness"": -20, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""��sizlik artt�, halk huzursuz."" }
    ]
  },
  {
    ""id"": 7,
    ""task"": ""Ordu b�t�esi tart���l�yor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""B�t�eyi art�r"", ""happiness"": -10, ""people"": 0, ""money"": -30, ""science"": 0, ""soldier"": 30, ""result"": ""Ordu g��lendi ama halk tepkili."" },
      { ""id"": ""B"", ""text"": ""Mevcut haliyle b�rak"", ""happiness"": 0, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Durum de�i�medi. Tart��malar s�rd�."" },
      { ""id"": ""C"", ""text"": ""Ordu b�t�esini kes"", ""happiness"": 10, ""people"": 0, ""money"": 20, ""science"": 0, ""soldier"": -20, ""result"": ""Halk mutlu ama askerler ho�nutsuz."" }
    ]
  },
  {
    ""id"": 8,
    ""task"": ""Gen�ler i�siz."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Yeni fabrikalar a�"", ""happiness"": 20, ""people"": 10, ""money"": -40, ""science"": 0, ""soldier"": 0, ""result"": ""�stihdam artt�, ekonomi canland�."" },
      { ""id"": ""B"", ""text"": ""Giri�imcilik hibesi ver"", ""happiness"": 10, ""people"": 0, ""money"": -20, ""science"": 10, ""soldier"": 0, ""result"": ""Giri�imcilik desteklendi. Yenilik�i ��z�mler do�du."" },
      { ""id"": ""C"", ""text"": ""Umursama"", ""happiness"": -30, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Gen�ler protesto etti. Toplumsal huzur bozuldu."" }
    ]
  },
  {
    ""id"": 9,
    ""task"": ""Halk sosyal medya k�s�tlamas� istiyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Sans�r uygula"", ""happiness"": -40, ""people"": 0, ""money"": 0, ""science"": -10, ""soldier"": 0, ""result"": ""�zg�rl�kler k�s�tland�. Tepkiler b�y�d�."" },
      { ""id"": ""B"", ""text"": ""K�s�tlamay� reddet"", ""happiness"": 10, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""�zg�rl�k korundu. Gen�ler destek verdi."" },
      { ""id"": ""C"", ""text"": ""Geli�mi� denetim sistemi kur"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 5, ""soldier"": 0, ""result"": ""Denge sa�land� ama maliyetli oldu."" }
    ]
  },
  {
    ""id"": 10,
    ""task"": ""�lkede enerji krizi ba�lad�."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""N�kleer enerjiye ge�"", ""happiness"": -10, ""people"": 0, ""money"": -50, ""science"": 30, ""soldier"": 0, ""result"": ""Enerji sa�land� ama halk korktu."" },
      { ""id"": ""B"", ""text"": ""Elektrik k�s�tlamas� yap"", ""happiness"": -50, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Halk karanl�kta kald�. Tepkiler b�y�d�."" },
      { ""id"": ""C"", ""text"": ""Yenilenebilir enerjiye yat�r�m yap"", ""happiness"": 10, ""people"": 0, ""money"": -30, ""science"": 15, ""soldier"": 0, ""result"": ""Gelecek i�in umut verici bir ad�m."" }
    ]
  }
]
";
}

