using UnityEngine;
using UnityEngine.UI;
using Models.Storage;

public class SaveLoadUI : MonoBehaviour
{
    [Header("Кнопки")]
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;

    [Header("Статус")]
    [SerializeField] private Text statusText;
    [SerializeField] private GameObject saveExistsIcon;
    [SerializeField] private GameObject noSaveIcon;

    void Start()
    {
        if (saveButton != null)
            saveButton.onClick.AddListener(SaveGame);

        if (loadButton != null)
            loadButton.onClick.AddListener(LoadGame);

        if (deleteButton != null)
            deleteButton.onClick.AddListener(DeleteSave);

        UpdateStatusUI();
    }

    void Update()
    {
        if (Time.frameCount % 30 == 0) 
        {
            UpdateStatusUI();
        }
    }


    public void SaveGame()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame();
            UpdateStatusUI();
            ShowMessage("Игра сохранена!", Color.green);
        }
        else
        {
            Debug.LogError("SaveManager не найден!");
            ShowMessage("Ошибка: SaveManager не найден", Color.red);
        }
    }

    public void LoadGame()
    {
        if (SaveManager.Instance != null)
        {
            if (SaveManager.Instance.SaveExists())
            {
                SaveManager.Instance.LoadGame();
                ShowMessage("Игра загружена!", Color.green);
            }
            else
            {
                ShowMessage("Сохранение не найдено", Color.yellow);
            }
        }
        else
        {
            Debug.LogError("SaveManager не найден!");
            ShowMessage("Ошибка загрузки", Color.red);
        }
    }

    public void DeleteSave()
    {
        if (SaveManager.Instance != null)
        {
            if (SaveManager.Instance.SaveExists())
            {
                SaveManager.Instance.DeleteSave();
                UpdateStatusUI();
                ShowMessage("Сохранение удалено", Color.yellow);
            }
            else
            {
                ShowMessage("Нет сохранения для удаления", Color.yellow);
            }
        }
    }

    private void UpdateStatusUI()
    {
        if (SaveManager.Instance == null) return;

        bool exists = SaveManager.Instance.SaveExists();

        if (statusText != null)
        {
            statusText.text = exists ? "? Сохранение есть" : "? Нет сохранения";
            statusText.color = exists ? Color.green : Color.gray;
        }

        if (saveExistsIcon != null)
            saveExistsIcon.SetActive(exists);

        if (noSaveIcon != null)
            noSaveIcon.SetActive(!exists);
    }

    private void ShowMessage(string message, Color color)
    {
        Debug.Log(message);

        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = color;


            Invoke("UpdateStatusUI", 2f);
        }
    }
}