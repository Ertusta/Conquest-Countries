using UnityEngine;
using TMPro;
using System.IO;

public class taskmaster : MonoBehaviour
{
    public GameObject taskPanel;
    public TextMeshProUGUI taskText;
    public TextMeshProUGUI optionAText;
    public TextMeshProUGUI optionBText;
    public TextMeshProUGUI optionCText;

    public Player player;  // Player objesine inspector'dan atayacaksýn

    private TaskList taskList;
    private Task currentTask;

    public GameObject resultPanel;




    void Start()
    {
        taskPanel.SetActive(false);
        LoadTasksFromJson();
        ShowRandomTask();
    }

    void LoadTasksFromJson()
    {
        string filePath = Path.Combine(Application.dataPath, "scripts/task.json");
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            string wrappedJson = "{\"tasks\":" + jsonString + "}";
            taskList = JsonUtility.FromJson<TaskList>(wrappedJson);
            Debug.Log("Görevler yüklendi. Toplam görev sayýsý: " + taskList.tasks.Length);
        }
        else
        {
            Debug.LogError("task.json bulunamadý: " + filePath);
        }
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
    public string result; // <== EKLENDÝ
}


