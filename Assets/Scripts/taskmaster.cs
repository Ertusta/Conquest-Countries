using UnityEngine;
using TMPro;
using System.Collections;

public class taskmaster : MonoBehaviour
{
    public GameObject taskPanel;
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI optionAText;
    public TextMeshProUGUI optionBText;
    public TextMeshProUGUI optionCText;

    public GameObject kritikPanel;
    public TextMeshProUGUI kritikText;
    public TextMeshProUGUI kritikAText;
    public TextMeshProUGUI kritikBText;

    public Player player;  
    public GameObject resultPanel;

    private TaskList taskList;
    private Task currentTask;

    public GameObject cameraObject;

    void Start()
    {
        taskPanel.SetActive(false);
        LoadTasksFromJson();
        ShowRandomTask();
    }

    void LoadTasksFromJson()
    {
        // Her iki kayna��n JSON'unu birle�tir
        string combinedJson = @"{ ""tasks"": " + TasksData.Json.TrimEnd(']', '\n', '\r') + "," + TasksData2.Json.TrimStart('[', '\n', '\r') + @" }";

        taskList = JsonUtility.FromJson<TaskList>(combinedJson);
        Debug.Log("T�m g�revler y�klendi. Toplam g�rev say�s�: " + taskList.tasks.Length);
    }


    public void ShowRandomTask()
    {
        if (taskList == null || taskList.tasks.Length == 0) return;

        int chosenIndex;
        float roll = Random.value;

        if (roll < 0.75f)
        {
            chosenIndex = Random.Range(0, 30);
            cameraObject.GetComponent<Cameras>().FocusOnGoodLeader();
            StartCoroutine(ShowTaskWithDelay(taskList.tasks[chosenIndex].id, 2f));
        }
        else
        {
            chosenIndex = Random.Range(30, taskList.tasks.Length);
            cameraObject.GetComponent<Cameras>().FocusOnRandomBadLeader();
            StartCoroutine(ShowKritikWithDelay(taskList.tasks[chosenIndex].id, 2f));
        }
    }

    private IEnumerator ShowTaskWithDelay(int id, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowTaskById(id);
    }

    private IEnumerator ShowKritikWithDelay(int id, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowKritikById(id);
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

    public void ShowKritikById(int id)
    {
        currentTask = System.Array.Find(taskList.tasks, t => t.id == id);
        if (currentTask == null) return;


        kritikPanel.SetActive(true);
        kritikText.text = currentTask.task;
        kritikAText.text = currentTask.options[0].text;
        kritikBText.text = currentTask.options[1].text;
    }

    public void OnClickOptionA() => SelectOption(0);
    public void OnClickOptionB() => SelectOption(1);
    public void OnClickOptionC() => SelectOption(2);

    void SelectOption(int optionIndex)
    {
        if (currentTask == null) return;
        var option = currentTask.options[optionIndex];
        kritikPanel.SetActive(false);
        string resultMessage = player.ApplyEffect(option);
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
  },
    {

  
    ""id"": 11,
    ""task"": ""Uzaydan sinyal al�nd�. Ne yapacaks�n?"",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Sinyali analiz et"", ""happiness"": 0, ""people"": 0, ""money"": -20, ""science"": 30, ""soldier"": 0, ""result"": ""Bilim insanlar� heyecanland�, prestij kazand�n."" },
      { ""id"": ""B"", ""text"": ""Yok say"", ""happiness"": -10, ""people"": 0, ""money"": 0, ""science"": -20, ""soldier"": 0, ""result"": ""Bilim �evreleri hayal k�r�kl���na u�rad�."" },
      { ""id"": ""C"", ""text"": ""Tehlikeli olabilir, askeri alarm ver"", ""happiness"": -20, ""people"": -10, ""money"": -30, ""science"": 0, ""soldier"": 20, ""result"": ""Gereksiz panik ya�and� ama ordu haz�rland�."" }
    ]
  },
  {
    ""id"": 12,
    ""task"": ""�lkede dev bir festival talebi var."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""B�t�eden b�y�k pay ay�r"", ""happiness"": 40, ""people"": 20, ""money"": -40, ""science"": 0, ""soldier"": 0, ""result"": ""Halk �ok e�lendi, turizm artt�."" },
      { ""id"": ""B"", ""text"": ""K���k �apl� izin ver"", ""happiness"": 10, ""people"": 5, ""money"": -10, ""science"": 0, ""soldier"": 0, ""result"": ""K�s�tl� ama tatmin edici oldu."" },
      { ""id"": ""C"", ""text"": ""Reddet"", ""happiness"": -30, ""people"": -5, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Gen�ler ve sanat��lar tepkili."" }
    ]
  },
  {
    ""id"": 13,
    ""task"": ""Bir hacker grubu devlete ait gizli bilgileri �ald�."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Sald�r�ya kar��l�k ver"", ""happiness"": -10, ""people"": 0, ""money"": -20, ""science"": 0, ""soldier"": 10, ""result"": ""Sald�r� p�sk�rt�ld� ama veri kayb� oldu."" },
      { ""id"": ""B"", ""text"": ""Gizlice anla�ma yap"", ""happiness"": 0, ""people"": 0, ""money"": -10, ""science"": -10, ""soldier"": 0, ""result"": ""Veriler geri al�nd� ama etik tart���ld�."" },
      { ""id"": ""C"", ""text"": ""Kamuoyunu bilgilendir"", ""happiness"": 10, ""people"": 5, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""�effafl�k takdir edildi."" }
    ]
  },
  {
    ""id"": 14,
    ""task"": ""Devlet dairelerinde robotlar kullan�lmak isteniyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Hemen uygula"", ""happiness"": -10, ""people"": -10, ""money"": -30, ""science"": 20, ""soldier"": 0, ""result"": ""Verim artt� ama memurlar i�siz kald�."" },
      { ""id"": ""B"", ""text"": ""Pilot b�lge se�"", ""happiness"": 0, ""people"": -5, ""money"": -10, ""science"": 10, ""soldier"": 0, ""result"": ""Yava� ama dengeli ilerleme sa�land�."" },
      { ""id"": ""C"", ""text"": ""Reddet"", ""happiness"": 10, ""people"": 0, ""money"": 0, ""science"": -20, ""soldier"": 0, ""result"": ""�nsan eme�i korundu, bilim geriledi."" }
    ]
  },
  {
    ""id"": 15,
    ""task"": ""Tarihi eserler yurt d���na ka��r�l�yor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""S�k� g�venlik �nlemleri al"", ""happiness"": 0, ""people"": 0, ""money"": -20, ""science"": 0, ""soldier"": 10, ""result"": ""Ka�ak��lar yakaland�, k�lt�r korundu."" },
      { ""id"": ""B"", ""text"": ""Uluslararas� yard�m iste"", ""happiness"": 10, ""people"": 0, ""money"": -10, ""science"": 0, ""soldier"": 0, ""result"": ""K�resel i�birli�i sa�land�."" },
      { ""id"": ""C"", ""text"": ""Umursama"", ""happiness"": -30, ""people"": -10, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Tarihi eserler kaybedildi, tepkiler b�y�d�."" }
    ]
  },
  {
    ""id"": 16,
    ""task"": ""Y�ksek lisansl� gen�ler i�siz."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Ara�t�rma merkezleri kur"", ""happiness"": 20, ""people"": 10, ""money"": -40, ""science"": 30, ""soldier"": 0, ""result"": ""Bilimsel at�l�m ba�lad�."" },
      { ""id"": ""B"", ""text"": ""�zel sekt�r� te�vik et"", ""happiness"": 10, ""people"": 5, ""money"": -20, ""science"": 10, ""soldier"": 0, ""result"": ""�stihdam k�smen ��z�ld�."" },
      { ""id"": ""C"", ""text"": ""Yurt d���na te�vik et"", ""happiness"": -10, ""people"": -20, ""money"": 10, ""science"": -10, ""soldier"": 0, ""result"": ""Beyin g��� ya�and�."" }
    ]
  },
  {
    ""id"": 17,
    ""task"": ""Kom�u �lke yard�m istedi."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Maddi yard�m g�nder"", ""happiness"": 10, ""people"": 0, ""money"": -30, ""science"": 0, ""soldier"": 0, ""result"": ""Uluslararas� itibar kazand�n."" },
      { ""id"": ""B"", ""text"": ""Asker g�nder"", ""happiness"": -10, ""people"": -5, ""money"": -20, ""science"": 0, ""soldier"": 10, ""result"": ""�stikrar sa�land� ama tepki ald�n."" },
      { ""id"": ""C"", ""text"": ""Umursama"", ""happiness"": -20, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""�nsanl�k krizi ya�and�, ele�tirildin."" }
    ]

  },
  {
    ""id"": 18,
    ""task"": ""�lke genelinde komplo teorileri yay�l�yor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""E�itim kampanyas� ba�lat"", ""happiness"": 0, ""people"": 0, ""money"": -20, ""science"": 10, ""soldier"": 0, ""result"": ""Toplum bilgiye y�neldi."" },
      { ""id"": ""B"", ""text"": ""Sosyal medyada filtre uygula"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 0, ""soldier"": 0, ""result"": ""Yay�lma yava�lad� ama sans�r ele�tirildi."" },
      { ""id"": ""C"", ""text"": ""Yay�lmas�na izin ver"", ""happiness"": -30, ""people"": -10, ""money"": 0, ""science"": -10, ""soldier"": 0, ""result"": ""G�ven kayb� ve kaos olu�tu."" }
    ]
  },
  {
    ""id"": 19,
    ""task"": ""�nl� bir bilim insan� devlete dan��manl�k teklif etti."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Kabul et ve ek b�t�e ver"", ""happiness"": 10, ""people"": 0, ""money"": -20, ""science"": 20, ""soldier"": 0, ""result"": ""Bilim politikalar� geli�ti."" },
      { ""id"": ""B"", ""text"": ""Sadece sembolik rol ver"", ""happiness"": 5, ""people"": 0, ""money"": -5, ""science"": 5, ""soldier"": 0, ""result"": ""Toplum moral kazand� ama etki s�n�rl� kald�."" },
      { ""id"": ""C"", ""text"": ""Reddet"", ""happiness"": -10, ""people"": 0, ""money"": 0, ""science"": -10, ""soldier"": 0, ""result"": ""Bilim �evresi hayal k�r�kl���na u�rad�."" }
    ]
  },
  {
    ""id"": 20,
    ""task"": ""Yabanc� yat�r�mc�lar �lkeye gelmek istiyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Tam te�vik ver"", ""happiness"": 10, ""people"": 10, ""money"": 30, ""science"": 0, ""soldier"": 0, ""result"": ""Ekonomi b�y�d�, halk sevindi."" },
      { ""id"": ""B"", ""text"": ""S�k� denetimle izin ver"", ""happiness"": 0, ""people"": 5, ""money"": 10, ""science"": 0, ""soldier"": 0, ""result"": ""Yat�r�mlar dengeli �ekilde artt�."" },
      { ""id"": ""C"", ""text"": ""Reddet"", ""happiness"": -20, ""people"": -5, ""money"": -10, ""science"": 0, ""soldier"": 0, ""result"": ""F�rsat ka�t�, tepkiler y�kseldi."" }
    ]
  },
  {
    ""id"": 21,
    ""task"": ""Uzayl�larla temas kuruldu. D�nya liderleri senin karar�n� bekliyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Hemen diplomatik g�r��me ba�lat"", ""happiness"": 20, ""people"": 0, ""money"": -10, ""science"": 50, ""soldier"": 0, ""result"": ""Tarihi bir anla�ma imzaland�. �nsanl�k yeni bir �a�a girdi."" },
      { ""id"": ""B"", ""text"": ""Gizli tut ve askeri haz�rl�k yap"", ""happiness"": -30, ""people"": 0, ""money"": -20, ""science"": 0, ""soldier"": 40, ""result"": ""S�zd�r�lan belgeler halk� pani�e s�r�kledi."" },
      { ""id"": ""C"", ""text"": ""Halka a��k duyuru yap"", ""happiness"": -40, ""people"": 0, ""money"": 0, ""science"": 20, ""soldier"": 0, ""result"": ""D�nya �ap�nda kaos ba�lad�. Dini gruplar ayakland�."" }
    ]
  },
  {
    ""id"": 22,
    ""task"": ""�lkede vampir salg�n� ��kt�. Halk panik halinde."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Gece soka�a ��kma yasa�� ilan et"", ""happiness"": -20, ""people"": 0, ""money"": -10, ""science"": 0, ""soldier"": 10, ""result"": ""Salg�n kontrol alt�na al�nd� ama ekonomi durdu."" },
      { ""id"": ""B"", ""text"": ""Bilim insanlar�na a�� geli�tirme g�revi ver"", ""happiness"": 10, ""people"": 0, ""money"": -30, ""science"": 40, ""soldier"": 0, ""result"": ""����r a�an ke�if yap�ld�. Vampirler tedavi edilebildi."" },
      { ""id"": ""C"", ""text"": ""Vampirlerle bar�� anla�mas� yap"", ""happiness"": -60, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Halk seni vatan haini ilan etti. B�y�k protesto ba�lad�."" }
    ]
  },
  {
    ""id"": 23,
    ""task"": ""Zaman makinesi icat edildi. Nas�l kullan�laca�� tart���l�yor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Tarihsel hatalar� d�zelt"", ""happiness"": 30, ""people"": 0, ""money"": 0, ""science"": 20, ""soldier"": 0, ""result"": ""Zaman paradoksu yarat�ld�. Garip olaylar ya�anmaya ba�lad�."" },
      { ""id"": ""B"", ""text"": ""Gelece�i ke�fet"", ""happiness"": 10, ""people"": 0, ""money"": 0, ""science"": 30, ""soldier"": 0, ""result"": ""Gelece�in bilgisiyle haz�rl�klar yap�ld�. B�y�k avantaj sa�land�."" },
      { ""id"": ""C"", ""text"": ""Zaman makinesini yasakla"", ""happiness"": -10, ""people"": 0, ""money"": 0, ""science"": -40, ""soldier"": 0, ""result"": ""Bilim insanlar� gizlice �al��maya devam etti."" }
    ]
  },
  {
    ""id"": 24,
    ""task"": ""�lkedeki b�t�n kediler aniden konu�maya ba�lad�."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Kedilerle diplomatik ili�ki kur"", ""happiness"": 40, ""people"": 0, ""money"": -5, ""science"": 10, ""soldier"": 0, ""result"": ""Kediler devlet i�lerine dan��man oldu. Verimlili�imiz artt�."" },
      { ""id"": ""B"", ""text"": ""Bilimsel ara�t�rma ba�lat"", ""happiness"": 0, ""people"": 0, ""money"": -20, ""science"": 50, ""soldier"": 0, ""result"": ""Hayvanlarla ileti�im teknolojisi geli�tirildi."" },
      { ""id"": ""C"", ""text"": ""Kedi n�fusunu kontrol et"", ""happiness"": -50, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 5, ""result"": ""Kedi haklar� savunucular� seni protesto etti."" }
    ]
  },
  {
    ""id"": 25,
    ""task"": ""Ulusal kahvalt� krizi: �ay m� kahve mi resmi i�ecek olacak?"",
    ""options"": [
      { ""id"": ""A"", ""text"": ""�ay resmi i�ecek olsun"", ""happiness"": 50, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""�ay �reticileri seni destekledi. Kahve severler k�st�."" },
      { ""id"": ""B"", ""text"": ""Kahve resmi i�ecek olsun"", ""happiness"": -30, ""people"": 0, ""money"": 10, ""science"": 0, ""soldier"": 0, ""result"": ""Gen�ler memnun ama ya�l�lar tepkili. �ay evi sahipleri iflas etti."" },
      { ""id"": ""C"", ""text"": ""�kisini de e�it kabul et"", ""happiness"": 10, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Herkes biraz memnun ama kimse tam mutlu de�il."" }
    ]
  },
  {
    ""id"": 26,
    ""task"": ""�lkede ejderha g�r�ld�. Halk korkuyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Ejderhay� evcille�tirmeye �al��"", ""happiness"": -10, ""people"": 0, ""money"": -15, ""science"": 30, ""soldier"": 0, ""result"": ""Ejderha dostun oldu. U�arak seyahat edebiliyorsun art�k."" },
      { ""id"": ""B"", ""text"": ""Ejderha avc�lar� �a��r"", ""happiness"": 20, ""people"": 0, ""money"": -30, ""science"": 0, ""soldier"": 20, ""result"": ""Ejderha �ld�r�ld� ama �evre tahribat� b�y�k."" },
      { ""id"": ""C"", ""text"": ""Ejderhayla bar�� anla�mas� yap"", ""happiness"": -20, ""people"": 0, ""money"": 0, ""science"": 10, ""soldier"": 0, ""result"": ""Ejderha �lkeni koruyor ama halk hala korkuyor."" }
    ]
  },
  {
    ""id"": 27,
    ""task"": ""T�m �lkedeki k�pr�ler bir gecede kayboldu."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Acil k�pr� in�aat� ba�lat"", ""happiness"": 10, ""people"": 0, ""money"": -60, ""science"": 0, ""soldier"": 0, ""result"": ""Ula��m sorunu ��z�ld� ama b�t�e bitti."" },
      { ""id"": ""B"", ""text"": ""U�an ara�lar geli�tir"", ""happiness"": 30, ""people"": 0, ""money"": -40, ""science"": 60, ""soldier"": 0, ""result"": ""Teknolojik devrim ya�and�. D�nya seni �rnek al�yor."" },
      { ""id"": ""C"", ""text"": ""K�pr� olmadan ya�amaya al��"", ""happiness"": -40, ""people"": -20, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""�ehirler birbirinden koptu. Ekonomi ��kt�."" }
    ]
  },
  {
    ""id"": 28,
    ""task"": ""�lkede s�per g��l� insanlar ortaya ��kt�."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Onlar� devlet hizmetine al"", ""happiness"": 20, ""people"": 0, ""money"": -20, ""science"": 20, ""soldier"": 30, ""result"": ""S�per kahramanlar �lkeni koruyor. Su� oran� s�f�r."" },
      { ""id"": ""B"", ""text"": ""S�per g��leri yasakla"", ""happiness"": -30, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""S�per g��l�ler gizlice �rg�tlendi. Yeralt� hareketi ba�lad�."" },
      { ""id"": ""C"", ""text"": ""S�per g��l�leri kaydet ve denetle"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 10, ""soldier"": 0, ""result"": ""Kontroll� s�per g�� sistemi kuruldu."" }
    ]
  },
  {
    ""id"": 29,
    ""task"": ""�lkede yemek krizi: Herkes sadece pizza yemek istiyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Pizza fabrikalar� kur"", ""happiness"": 40, ""people"": 0, ""money"": -20, ""science"": 0, ""soldier"": 0, ""result"": ""Halk mutlu ama beslenme sorunu olu�tu."" },
      { ""id"": ""B"", ""text"": ""Zorunlu diyet program� ba�lat"", ""happiness"": -40, ""people"": 0, ""money"": -15, ""science"": 15, ""soldier"": 0, ""result"": ""Halk sa�l�kl� beslenmeye zorland�. Direni� ba�lad�."" },
      { ""id"": ""C"", ""text"": ""Pizza d���nda her yeme�i yasakla"", ""happiness"": 30, ""people"": 0, ""money"": 0, ""science"": -20, ""soldier"": 0, ""result"": ""Herkes mutlu ama �ift�iler i�siz kald�."" }
    ]
  },
  {
    ""id"": 30,
    ""task"": ""�lkendeki t�m arabalar kendili�inden dans etmeye ba�lad�."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Dans yar��mas� d�zenle"", ""happiness"": 60, ""people"": 0, ""money"": 20, ""science"": 0, ""soldier"": 0, ""result"": ""Turizm patlamas� ya�and�. Herkes �lkeni g�rmeye geliyor."" },
      { ""id"": ""B"", ""text"": ""Arabalar� durdurmaya �al��"", ""happiness"": -20, ""people"": 0, ""money"": -30, ""science"": 20, ""soldier"": 0, ""result"": ""Teknik ��z�m bulundu ama e�lence bitti."" },
      { ""id"": ""C"", ""text"": ""Dans eden arabalar� m�ze yap"", ""happiness"": 20, ""people"": 0, ""money"": 10, ""science"": 10, ""soldier"": 0, ""result"": ""D�nyan�n en ilgin� m�zesi oldu. K�lt�r seviyesi artt�."" }
    ]
  }
]
";
}

public static class TasksData2
{
    public static string Json = @"
[
  {
    ""id"": 31,
    ""task"": ""Kom�umuz n�kleer silah geli�tiriyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Casuslarla teknolojiyi �al"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 10, ""soldier"": -10, ""result"": ""Bilim kazan�m� sa�land� ama sava� ��kt�."" },
      { ""id"": ""B"", ""text"": ""Birlikte geli�tir"", ""happiness"": -5, ""people"": 0, ""money"": -20, ""science"": 10, ""soldier"": 10, ""result"": ""�ttifak sa�land� ama halk endi�eli."" }
    ]
  },
  {
    ""id"": 32,
    ""task"": ""Kom�umuz s�n�r�m�za duvar �r�yor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Protesto et"", ""happiness"": 10, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": -5, ""result"": ""Halk kararl� duru�u takdir etti."" },
      { ""id"": ""B"", ""text"": ""Daha y�ksek duvar �r"", ""happiness"": -10, ""people"": 0, ""money"": -20, ""science"": 0, ""soldier"": 10, ""result"": ""Gerginlik artt�, kom�uluk bitti."" }
    ]
  },
  {
    ""id"": 33,
    ""task"": ""Kom�umuz uzaya uydu g�nderdi."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Biz de hemen g�nderelim"", ""happiness"": 5, ""people"": 0, ""money"": -30, ""science"": 15, ""soldier"": 0, ""result"": ""Bilimsel rekabet ba�lad�."" },
      { ""id"": ""B"", ""text"": ""Uyduyu hackle"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 10, ""soldier"": -5, ""result"": ""Bilgiler al�nd�, diplomasi bozuldu."" }
    ]
  },
  {
    ""id"": 34,
    ""task"": ""Kom�umuz m�lteci kabul etmiyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""T�m m�ltecileri kabul et"", ""happiness"": 15, ""people"": 20, ""money"": -30, ""science"": 0, ""soldier"": 0, ""result"": ""�nsanl�k �rne�i oldun ama ekonomi zorland�."" },
      { ""id"": ""B"", ""text"": ""S�n�r� kapat"", ""happiness"": -20, ""people"": -10, ""money"": 10, ""science"": 0, ""soldier"": 10, ""result"": ""G�venlik sa�land� ama ele�tiriler artt�."" }
    ]
  },
  {
    ""id"": 35,
    ""task"": ""Kom�umuz i� sava� ya��yor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Bar�� g�c� g�nder"", ""happiness"": 10, ""people"": 0, ""money"": -20, ""science"": 0, ""soldier"": -10, ""result"": ""�yilik yap�ld� ama kay�plar ya�and�."" },
      { ""id"": ""B"", ""text"": ""Durumu avantaja �evir"", ""happiness"": -10, ""people"": 0, ""money"": 20, ""science"": 0, ""soldier"": 5, ""result"": ""K�r sa�land� ama ahlaki tart��ma ba�lad�."" }
    ]
  },
  {
    ""id"": 36,
    ""task"": ""Kom�umuz yerli otomobil �retti."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Biz de AR-GE'yi h�zland�ral�m"", ""happiness"": 5, ""people"": 0, ""money"": -20, ""science"": 15, ""soldier"": 0, ""result"": ""Yerli �retim ata�� ba�lad�."" },
      { ""id"": ""B"", ""text"": ""Kom�udan lisans al"", ""happiness"": 0, ""people"": 0, ""money"": -10, ""science"": 5, ""soldier"": 0, ""result"": ""H�zl� �retim ama d��a ba��ml�l�k olu�tu."" }
    ]
  },
  {
    ""id"": 37,
    ""task"": ""Kom�umuz uluslararas� fuar d�zenliyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Kat�l ve tan�t�m yap"", ""happiness"": 10, ""people"": 5, ""money"": -10, ""science"": 5, ""soldier"": 0, ""result"": ""Tan�t�m sayesinde turist �ekildi."" },
      { ""id"": ""B"", ""text"": ""Kendi fuar�n� organize et"", ""happiness"": 15, ""people"": 10, ""money"": -30, ""science"": 10, ""soldier"": 0, ""result"": ""B�y�k ba�ar� sa�land�."" }
    ]
  },
  {
    ""id"": 38,
    ""task"": ""Kom�umuz ekonomik ambargo uygulad�."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""�� �retimi art�r"", ""happiness"": 10, ""people"": 0, ""money"": -20, ""science"": 10, ""soldier"": 0, ""result"": ""Ba��ms�zl�k artt�."" },
      { ""id"": ""B"", ""text"": ""Ambargoya misilleme yap"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 0, ""soldier"": 10, ""result"": ""Gerginlik t�rmand�."" }
    ]
  },
  {
    ""id"": 39,
    ""task"": ""Kom�umuz s�n�rda do�algaz buldu."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Ortakl�k teklif et"", ""happiness"": 5, ""people"": 0, ""money"": 10, ""science"": 5, ""soldier"": 0, ""result"": ""Enerji payla��m� sa�land�."" },
      { ""id"": ""B"", ""text"": ""S�n�r� yeniden �iz"", ""happiness"": -20, ""people"": -5, ""money"": 20, ""science"": 0, ""soldier"": 10, ""result"": ""B�y�k kriz ��kt�."" }
    ]
  },
  {
    ""id"": 40,
    ""task"": ""Kom�umuz ba��ms�zl�k referandumu d�zenliyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Tarafs�z kal"", ""happiness"": 0, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Denge korundu."" },
      { ""id"": ""B"", ""text"": ""Taraftan birini destekle"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 0, ""soldier"": 10, ""result"": ""Kom�u ile ili�kiler bozuldu."" }
    ]
  }
]
";
}

