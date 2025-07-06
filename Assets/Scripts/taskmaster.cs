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
        // Her iki kaynaðýn JSON'unu birleþtir
        string combinedJson = @"{ ""tasks"": " + TasksData.Json.TrimEnd(']', '\n', '\r') + "," + TasksData2.Json.TrimStart('[', '\n', '\r') + @" }";

        taskList = JsonUtility.FromJson<TaskList>(combinedJson);
        Debug.Log("Tüm görevler yüklendi. Toplam görev sayýsý: " + taskList.tasks.Length);
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
  },
    {

  
    ""id"": 11,
    ""task"": ""Uzaydan sinyal alýndý. Ne yapacaksýn?"",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Sinyali analiz et"", ""happiness"": 0, ""people"": 0, ""money"": -20, ""science"": 30, ""soldier"": 0, ""result"": ""Bilim insanlarý heyecanlandý, prestij kazandýn."" },
      { ""id"": ""B"", ""text"": ""Yok say"", ""happiness"": -10, ""people"": 0, ""money"": 0, ""science"": -20, ""soldier"": 0, ""result"": ""Bilim çevreleri hayal kýrýklýðýna uðradý."" },
      { ""id"": ""C"", ""text"": ""Tehlikeli olabilir, askeri alarm ver"", ""happiness"": -20, ""people"": -10, ""money"": -30, ""science"": 0, ""soldier"": 20, ""result"": ""Gereksiz panik yaþandý ama ordu hazýrlandý."" }
    ]
  },
  {
    ""id"": 12,
    ""task"": ""Ülkede dev bir festival talebi var."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Bütçeden büyük pay ayýr"", ""happiness"": 40, ""people"": 20, ""money"": -40, ""science"": 0, ""soldier"": 0, ""result"": ""Halk çok eðlendi, turizm arttý."" },
      { ""id"": ""B"", ""text"": ""Küçük çaplý izin ver"", ""happiness"": 10, ""people"": 5, ""money"": -10, ""science"": 0, ""soldier"": 0, ""result"": ""Kýsýtlý ama tatmin edici oldu."" },
      { ""id"": ""C"", ""text"": ""Reddet"", ""happiness"": -30, ""people"": -5, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Gençler ve sanatçýlar tepkili."" }
    ]
  },
  {
    ""id"": 13,
    ""task"": ""Bir hacker grubu devlete ait gizli bilgileri çaldý."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Saldýrýya karþýlýk ver"", ""happiness"": -10, ""people"": 0, ""money"": -20, ""science"": 0, ""soldier"": 10, ""result"": ""Saldýrý püskürtüldü ama veri kaybý oldu."" },
      { ""id"": ""B"", ""text"": ""Gizlice anlaþma yap"", ""happiness"": 0, ""people"": 0, ""money"": -10, ""science"": -10, ""soldier"": 0, ""result"": ""Veriler geri alýndý ama etik tartýþýldý."" },
      { ""id"": ""C"", ""text"": ""Kamuoyunu bilgilendir"", ""happiness"": 10, ""people"": 5, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Þeffaflýk takdir edildi."" }
    ]
  },
  {
    ""id"": 14,
    ""task"": ""Devlet dairelerinde robotlar kullanýlmak isteniyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Hemen uygula"", ""happiness"": -10, ""people"": -10, ""money"": -30, ""science"": 20, ""soldier"": 0, ""result"": ""Verim arttý ama memurlar iþsiz kaldý."" },
      { ""id"": ""B"", ""text"": ""Pilot bölge seç"", ""happiness"": 0, ""people"": -5, ""money"": -10, ""science"": 10, ""soldier"": 0, ""result"": ""Yavaþ ama dengeli ilerleme saðlandý."" },
      { ""id"": ""C"", ""text"": ""Reddet"", ""happiness"": 10, ""people"": 0, ""money"": 0, ""science"": -20, ""soldier"": 0, ""result"": ""Ýnsan emeði korundu, bilim geriledi."" }
    ]
  },
  {
    ""id"": 15,
    ""task"": ""Tarihi eserler yurt dýþýna kaçýrýlýyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Sýký güvenlik önlemleri al"", ""happiness"": 0, ""people"": 0, ""money"": -20, ""science"": 0, ""soldier"": 10, ""result"": ""Kaçakçýlar yakalandý, kültür korundu."" },
      { ""id"": ""B"", ""text"": ""Uluslararasý yardým iste"", ""happiness"": 10, ""people"": 0, ""money"": -10, ""science"": 0, ""soldier"": 0, ""result"": ""Küresel iþbirliði saðlandý."" },
      { ""id"": ""C"", ""text"": ""Umursama"", ""happiness"": -30, ""people"": -10, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Tarihi eserler kaybedildi, tepkiler büyüdü."" }
    ]
  },
  {
    ""id"": 16,
    ""task"": ""Yüksek lisanslý gençler iþsiz."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Araþtýrma merkezleri kur"", ""happiness"": 20, ""people"": 10, ""money"": -40, ""science"": 30, ""soldier"": 0, ""result"": ""Bilimsel atýlým baþladý."" },
      { ""id"": ""B"", ""text"": ""Özel sektörü teþvik et"", ""happiness"": 10, ""people"": 5, ""money"": -20, ""science"": 10, ""soldier"": 0, ""result"": ""Ýstihdam kýsmen çözüldü."" },
      { ""id"": ""C"", ""text"": ""Yurt dýþýna teþvik et"", ""happiness"": -10, ""people"": -20, ""money"": 10, ""science"": -10, ""soldier"": 0, ""result"": ""Beyin göçü yaþandý."" }
    ]
  },
  {
    ""id"": 17,
    ""task"": ""Komþu ülke yardým istedi."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Maddi yardým gönder"", ""happiness"": 10, ""people"": 0, ""money"": -30, ""science"": 0, ""soldier"": 0, ""result"": ""Uluslararasý itibar kazandýn."" },
      { ""id"": ""B"", ""text"": ""Asker gönder"", ""happiness"": -10, ""people"": -5, ""money"": -20, ""science"": 0, ""soldier"": 10, ""result"": ""Ýstikrar saðlandý ama tepki aldýn."" },
      { ""id"": ""C"", ""text"": ""Umursama"", ""happiness"": -20, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Ýnsanlýk krizi yaþandý, eleþtirildin."" }
    ]

  },
  {
    ""id"": 18,
    ""task"": ""Ülke genelinde komplo teorileri yayýlýyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Eðitim kampanyasý baþlat"", ""happiness"": 0, ""people"": 0, ""money"": -20, ""science"": 10, ""soldier"": 0, ""result"": ""Toplum bilgiye yöneldi."" },
      { ""id"": ""B"", ""text"": ""Sosyal medyada filtre uygula"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 0, ""soldier"": 0, ""result"": ""Yayýlma yavaþladý ama sansür eleþtirildi."" },
      { ""id"": ""C"", ""text"": ""Yayýlmasýna izin ver"", ""happiness"": -30, ""people"": -10, ""money"": 0, ""science"": -10, ""soldier"": 0, ""result"": ""Güven kaybý ve kaos oluþtu."" }
    ]
  },
  {
    ""id"": 19,
    ""task"": ""Ünlü bir bilim insaný devlete danýþmanlýk teklif etti."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Kabul et ve ek bütçe ver"", ""happiness"": 10, ""people"": 0, ""money"": -20, ""science"": 20, ""soldier"": 0, ""result"": ""Bilim politikalarý geliþti."" },
      { ""id"": ""B"", ""text"": ""Sadece sembolik rol ver"", ""happiness"": 5, ""people"": 0, ""money"": -5, ""science"": 5, ""soldier"": 0, ""result"": ""Toplum moral kazandý ama etki sýnýrlý kaldý."" },
      { ""id"": ""C"", ""text"": ""Reddet"", ""happiness"": -10, ""people"": 0, ""money"": 0, ""science"": -10, ""soldier"": 0, ""result"": ""Bilim çevresi hayal kýrýklýðýna uðradý."" }
    ]
  },
  {
    ""id"": 20,
    ""task"": ""Yabancý yatýrýmcýlar ülkeye gelmek istiyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Tam teþvik ver"", ""happiness"": 10, ""people"": 10, ""money"": 30, ""science"": 0, ""soldier"": 0, ""result"": ""Ekonomi büyüdü, halk sevindi."" },
      { ""id"": ""B"", ""text"": ""Sýký denetimle izin ver"", ""happiness"": 0, ""people"": 5, ""money"": 10, ""science"": 0, ""soldier"": 0, ""result"": ""Yatýrýmlar dengeli þekilde arttý."" },
      { ""id"": ""C"", ""text"": ""Reddet"", ""happiness"": -20, ""people"": -5, ""money"": -10, ""science"": 0, ""soldier"": 0, ""result"": ""Fýrsat kaçtý, tepkiler yükseldi."" }
    ]
  },
  {
    ""id"": 21,
    ""task"": ""Uzaylýlarla temas kuruldu. Dünya liderleri senin kararýný bekliyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Hemen diplomatik görüþme baþlat"", ""happiness"": 20, ""people"": 0, ""money"": -10, ""science"": 50, ""soldier"": 0, ""result"": ""Tarihi bir anlaþma imzalandý. Ýnsanlýk yeni bir çaða girdi."" },
      { ""id"": ""B"", ""text"": ""Gizli tut ve askeri hazýrlýk yap"", ""happiness"": -30, ""people"": 0, ""money"": -20, ""science"": 0, ""soldier"": 40, ""result"": ""Sýzdýrýlan belgeler halký paniðe sürükledi."" },
      { ""id"": ""C"", ""text"": ""Halka açýk duyuru yap"", ""happiness"": -40, ""people"": 0, ""money"": 0, ""science"": 20, ""soldier"": 0, ""result"": ""Dünya çapýnda kaos baþladý. Dini gruplar ayaklandý."" }
    ]
  },
  {
    ""id"": 22,
    ""task"": ""Ülkede vampir salgýný çýktý. Halk panik halinde."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Gece sokaða çýkma yasaðý ilan et"", ""happiness"": -20, ""people"": 0, ""money"": -10, ""science"": 0, ""soldier"": 10, ""result"": ""Salgýn kontrol altýna alýndý ama ekonomi durdu."" },
      { ""id"": ""B"", ""text"": ""Bilim insanlarýna aþý geliþtirme görevi ver"", ""happiness"": 10, ""people"": 0, ""money"": -30, ""science"": 40, ""soldier"": 0, ""result"": ""Çýðýr açan keþif yapýldý. Vampirler tedavi edilebildi."" },
      { ""id"": ""C"", ""text"": ""Vampirlerle barýþ anlaþmasý yap"", ""happiness"": -60, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Halk seni vatan haini ilan etti. Büyük protesto baþladý."" }
    ]
  },
  {
    ""id"": 23,
    ""task"": ""Zaman makinesi icat edildi. Nasýl kullanýlacaðý tartýþýlýyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Tarihsel hatalarý düzelt"", ""happiness"": 30, ""people"": 0, ""money"": 0, ""science"": 20, ""soldier"": 0, ""result"": ""Zaman paradoksu yaratýldý. Garip olaylar yaþanmaya baþladý."" },
      { ""id"": ""B"", ""text"": ""Geleceði keþfet"", ""happiness"": 10, ""people"": 0, ""money"": 0, ""science"": 30, ""soldier"": 0, ""result"": ""Geleceðin bilgisiyle hazýrlýklar yapýldý. Büyük avantaj saðlandý."" },
      { ""id"": ""C"", ""text"": ""Zaman makinesini yasakla"", ""happiness"": -10, ""people"": 0, ""money"": 0, ""science"": -40, ""soldier"": 0, ""result"": ""Bilim insanlarý gizlice çalýþmaya devam etti."" }
    ]
  },
  {
    ""id"": 24,
    ""task"": ""Ülkedeki bütün kediler aniden konuþmaya baþladý."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Kedilerle diplomatik iliþki kur"", ""happiness"": 40, ""people"": 0, ""money"": -5, ""science"": 10, ""soldier"": 0, ""result"": ""Kediler devlet iþlerine danýþman oldu. Verimliliðimiz arttý."" },
      { ""id"": ""B"", ""text"": ""Bilimsel araþtýrma baþlat"", ""happiness"": 0, ""people"": 0, ""money"": -20, ""science"": 50, ""soldier"": 0, ""result"": ""Hayvanlarla iletiþim teknolojisi geliþtirildi."" },
      { ""id"": ""C"", ""text"": ""Kedi nüfusunu kontrol et"", ""happiness"": -50, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 5, ""result"": ""Kedi haklarý savunucularý seni protesto etti."" }
    ]
  },
  {
    ""id"": 25,
    ""task"": ""Ulusal kahvaltý krizi: Çay mý kahve mi resmi içecek olacak?"",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Çay resmi içecek olsun"", ""happiness"": 50, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Çay üreticileri seni destekledi. Kahve severler küstü."" },
      { ""id"": ""B"", ""text"": ""Kahve resmi içecek olsun"", ""happiness"": -30, ""people"": 0, ""money"": 10, ""science"": 0, ""soldier"": 0, ""result"": ""Gençler memnun ama yaþlýlar tepkili. Çay evi sahipleri iflas etti."" },
      { ""id"": ""C"", ""text"": ""Ýkisini de eþit kabul et"", ""happiness"": 10, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Herkes biraz memnun ama kimse tam mutlu deðil."" }
    ]
  },
  {
    ""id"": 26,
    ""task"": ""Ülkede ejderha görüldü. Halk korkuyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Ejderhayý evcilleþtirmeye çalýþ"", ""happiness"": -10, ""people"": 0, ""money"": -15, ""science"": 30, ""soldier"": 0, ""result"": ""Ejderha dostun oldu. Uçarak seyahat edebiliyorsun artýk."" },
      { ""id"": ""B"", ""text"": ""Ejderha avcýlarý çaðýr"", ""happiness"": 20, ""people"": 0, ""money"": -30, ""science"": 0, ""soldier"": 20, ""result"": ""Ejderha öldürüldü ama çevre tahribatý büyük."" },
      { ""id"": ""C"", ""text"": ""Ejderhayla barýþ anlaþmasý yap"", ""happiness"": -20, ""people"": 0, ""money"": 0, ""science"": 10, ""soldier"": 0, ""result"": ""Ejderha ülkeni koruyor ama halk hala korkuyor."" }
    ]
  },
  {
    ""id"": 27,
    ""task"": ""Tüm ülkedeki köprüler bir gecede kayboldu."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Acil köprü inþaatý baþlat"", ""happiness"": 10, ""people"": 0, ""money"": -60, ""science"": 0, ""soldier"": 0, ""result"": ""Ulaþým sorunu çözüldü ama bütçe bitti."" },
      { ""id"": ""B"", ""text"": ""Uçan araçlar geliþtir"", ""happiness"": 30, ""people"": 0, ""money"": -40, ""science"": 60, ""soldier"": 0, ""result"": ""Teknolojik devrim yaþandý. Dünya seni örnek alýyor."" },
      { ""id"": ""C"", ""text"": ""Köprü olmadan yaþamaya alýþ"", ""happiness"": -40, ""people"": -20, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Þehirler birbirinden koptu. Ekonomi çöktü."" }
    ]
  },
  {
    ""id"": 28,
    ""task"": ""Ülkede süper güçlü insanlar ortaya çýktý."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Onlarý devlet hizmetine al"", ""happiness"": 20, ""people"": 0, ""money"": -20, ""science"": 20, ""soldier"": 30, ""result"": ""Süper kahramanlar ülkeni koruyor. Suç oraný sýfýr."" },
      { ""id"": ""B"", ""text"": ""Süper güçleri yasakla"", ""happiness"": -30, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Süper güçlüler gizlice örgütlendi. Yeraltý hareketi baþladý."" },
      { ""id"": ""C"", ""text"": ""Süper güçlüleri kaydet ve denetle"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 10, ""soldier"": 0, ""result"": ""Kontrollü süper güç sistemi kuruldu."" }
    ]
  },
  {
    ""id"": 29,
    ""task"": ""Ülkede yemek krizi: Herkes sadece pizza yemek istiyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Pizza fabrikalarý kur"", ""happiness"": 40, ""people"": 0, ""money"": -20, ""science"": 0, ""soldier"": 0, ""result"": ""Halk mutlu ama beslenme sorunu oluþtu."" },
      { ""id"": ""B"", ""text"": ""Zorunlu diyet programý baþlat"", ""happiness"": -40, ""people"": 0, ""money"": -15, ""science"": 15, ""soldier"": 0, ""result"": ""Halk saðlýklý beslenmeye zorlandý. Direniþ baþladý."" },
      { ""id"": ""C"", ""text"": ""Pizza dýþýnda her yemeði yasakla"", ""happiness"": 30, ""people"": 0, ""money"": 0, ""science"": -20, ""soldier"": 0, ""result"": ""Herkes mutlu ama çiftçiler iþsiz kaldý."" }
    ]
  },
  {
    ""id"": 30,
    ""task"": ""Ülkendeki tüm arabalar kendiliðinden dans etmeye baþladý."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Dans yarýþmasý düzenle"", ""happiness"": 60, ""people"": 0, ""money"": 20, ""science"": 0, ""soldier"": 0, ""result"": ""Turizm patlamasý yaþandý. Herkes ülkeni görmeye geliyor."" },
      { ""id"": ""B"", ""text"": ""Arabalarý durdurmaya çalýþ"", ""happiness"": -20, ""people"": 0, ""money"": -30, ""science"": 20, ""soldier"": 0, ""result"": ""Teknik çözüm bulundu ama eðlence bitti."" },
      { ""id"": ""C"", ""text"": ""Dans eden arabalarý müze yap"", ""happiness"": 20, ""people"": 0, ""money"": 10, ""science"": 10, ""soldier"": 0, ""result"": ""Dünyanýn en ilginç müzesi oldu. Kültür seviyesi arttý."" }
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
    ""task"": ""Komþumuz nükleer silah geliþtiriyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Casuslarla teknolojiyi çal"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 10, ""soldier"": -10, ""result"": ""Bilim kazanýmý saðlandý ama savaþ çýktý."" },
      { ""id"": ""B"", ""text"": ""Birlikte geliþtir"", ""happiness"": -5, ""people"": 0, ""money"": -20, ""science"": 10, ""soldier"": 10, ""result"": ""Ýttifak saðlandý ama halk endiþeli."" }
    ]
  },
  {
    ""id"": 32,
    ""task"": ""Komþumuz sýnýrýmýza duvar örüyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Protesto et"", ""happiness"": 10, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": -5, ""result"": ""Halk kararlý duruþu takdir etti."" },
      { ""id"": ""B"", ""text"": ""Daha yüksek duvar ör"", ""happiness"": -10, ""people"": 0, ""money"": -20, ""science"": 0, ""soldier"": 10, ""result"": ""Gerginlik arttý, komþuluk bitti."" }
    ]
  },
  {
    ""id"": 33,
    ""task"": ""Komþumuz uzaya uydu gönderdi."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Biz de hemen gönderelim"", ""happiness"": 5, ""people"": 0, ""money"": -30, ""science"": 15, ""soldier"": 0, ""result"": ""Bilimsel rekabet baþladý."" },
      { ""id"": ""B"", ""text"": ""Uyduyu hackle"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 10, ""soldier"": -5, ""result"": ""Bilgiler alýndý, diplomasi bozuldu."" }
    ]
  },
  {
    ""id"": 34,
    ""task"": ""Komþumuz mülteci kabul etmiyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Tüm mültecileri kabul et"", ""happiness"": 15, ""people"": 20, ""money"": -30, ""science"": 0, ""soldier"": 0, ""result"": ""Ýnsanlýk örneði oldun ama ekonomi zorlandý."" },
      { ""id"": ""B"", ""text"": ""Sýnýrý kapat"", ""happiness"": -20, ""people"": -10, ""money"": 10, ""science"": 0, ""soldier"": 10, ""result"": ""Güvenlik saðlandý ama eleþtiriler arttý."" }
    ]
  },
  {
    ""id"": 35,
    ""task"": ""Komþumuz iç savaþ yaþýyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Barýþ gücü gönder"", ""happiness"": 10, ""people"": 0, ""money"": -20, ""science"": 0, ""soldier"": -10, ""result"": ""Ýyilik yapýldý ama kayýplar yaþandý."" },
      { ""id"": ""B"", ""text"": ""Durumu avantaja çevir"", ""happiness"": -10, ""people"": 0, ""money"": 20, ""science"": 0, ""soldier"": 5, ""result"": ""Kâr saðlandý ama ahlaki tartýþma baþladý."" }
    ]
  },
  {
    ""id"": 36,
    ""task"": ""Komþumuz yerli otomobil üretti."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Biz de AR-GE'yi hýzlandýralým"", ""happiness"": 5, ""people"": 0, ""money"": -20, ""science"": 15, ""soldier"": 0, ""result"": ""Yerli üretim ataðý baþladý."" },
      { ""id"": ""B"", ""text"": ""Komþudan lisans al"", ""happiness"": 0, ""people"": 0, ""money"": -10, ""science"": 5, ""soldier"": 0, ""result"": ""Hýzlý üretim ama dýþa baðýmlýlýk oluþtu."" }
    ]
  },
  {
    ""id"": 37,
    ""task"": ""Komþumuz uluslararasý fuar düzenliyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Katýl ve tanýtým yap"", ""happiness"": 10, ""people"": 5, ""money"": -10, ""science"": 5, ""soldier"": 0, ""result"": ""Tanýtým sayesinde turist çekildi."" },
      { ""id"": ""B"", ""text"": ""Kendi fuarýný organize et"", ""happiness"": 15, ""people"": 10, ""money"": -30, ""science"": 10, ""soldier"": 0, ""result"": ""Büyük baþarý saðlandý."" }
    ]
  },
  {
    ""id"": 38,
    ""task"": ""Komþumuz ekonomik ambargo uyguladý."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Ýç üretimi artýr"", ""happiness"": 10, ""people"": 0, ""money"": -20, ""science"": 10, ""soldier"": 0, ""result"": ""Baðýmsýzlýk arttý."" },
      { ""id"": ""B"", ""text"": ""Ambargoya misilleme yap"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 0, ""soldier"": 10, ""result"": ""Gerginlik týrmandý."" }
    ]
  },
  {
    ""id"": 39,
    ""task"": ""Komþumuz sýnýrda doðalgaz buldu."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Ortaklýk teklif et"", ""happiness"": 5, ""people"": 0, ""money"": 10, ""science"": 5, ""soldier"": 0, ""result"": ""Enerji paylaþýmý saðlandý."" },
      { ""id"": ""B"", ""text"": ""Sýnýrý yeniden çiz"", ""happiness"": -20, ""people"": -5, ""money"": 20, ""science"": 0, ""soldier"": 10, ""result"": ""Büyük kriz çýktý."" }
    ]
  },
  {
    ""id"": 40,
    ""task"": ""Komþumuz baðýmsýzlýk referandumu düzenliyor."",
    ""options"": [
      { ""id"": ""A"", ""text"": ""Tarafsýz kal"", ""happiness"": 0, ""people"": 0, ""money"": 0, ""science"": 0, ""soldier"": 0, ""result"": ""Denge korundu."" },
      { ""id"": ""B"", ""text"": ""Taraftan birini destekle"", ""happiness"": -10, ""people"": 0, ""money"": -10, ""science"": 0, ""soldier"": 10, ""result"": ""Komþu ile iliþkiler bozuldu."" }
    ]
  }
]
";
}

