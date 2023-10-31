using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public static bool gameIsPaused = false;

    public enum UI
    {
        SettingMenu,
        Inventory,
        Quest,
        PopupWindow
    };

    public UIPrefabs uiPrefabs;
    public List<GameObject> uiPrefabsInGame;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameIsPaused)
            {
                Debug.Log("HI");
                GetUIPrefab(UI.SettingMenu);
                Pause();
            }
        }
    }

    public void Resume()
    {
        //! 다시 시작
        Cursor.visible = false;     //마우스 커서를 보이지 않게
        Cursor.lockState = CursorLockMode.Locked; //마우스 커서 위치 고정
        Time.timeScale = 1f;
        gameIsPaused = false;

    }

    public void Pause()
    {
        //! 멈춤
        Cursor.visible = true;     //마우스 커서를 보이지 않게
        Cursor.lockState = CursorLockMode.None; //마우스 커서 위치 고정
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public GameObject GetUIPrefab(UI ui)
    {
        GameObject prefab = null;

        switch (ui)
        {
            case UI.SettingMenu:
                if (uiPrefabs.settingUI == null)
                    prefab = Resources.Load<GameObject>("SystemPrefabs/" + "Popup_Settings");
                else
                    prefab = uiPrefabs.settingUI;
                break;
            case UI.Inventory:
                break;
            case UI.Quest:
                break;
            case UI.PopupWindow:
                if (uiPrefabs.popupWindow == null)
                    prefab = Resources.Load<GameObject>("SystemPrefabs/" + "Popup_Window");
                else
                    prefab = uiPrefabs.popupWindow;
                break;
            default:
                break;
        }

        if (prefab != null)
        {
            if (!uiPrefabsInGame.Contains(prefab))
            {
                prefab = Instantiate(prefab, GameManager.Instance.m_canvas.transform);
                prefab.SetActive(true);
                uiPrefabsInGame.Add(prefab);

                switch (ui)
                {
                    case UI.SettingMenu:
                        uiPrefabs.settingUI = prefab;
                        break;
                    case UI.Inventory:
                        break;
                    case UI.Quest:
                        break;
                    case UI.PopupWindow:
                        uiPrefabs.popupWindow = prefab;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                int index = uiPrefabsInGame.IndexOf(prefab);
                uiPrefabsInGame[index].SetActive(true);
            }
        }

        return prefab;
    }
}





