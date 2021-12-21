using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Manager;
using Common;

public class UIOBSelect : MonoBehaviour
{
    private Dropdown mDropdown;
    private int mSelectedIndex;

    void Start()
    {
        mDropdown = GetComponent<Dropdown>();
        AddDropdownListener(OnDropdownValueChange);

        List<Dropdown.OptionData> listOptions = new List<Dropdown.OptionData>();
        listOptions.Add(new Dropdown.OptionData("上帝视角"));
        mDropdown.AddOptions(listOptions);

        SetDropDownItemValue(0);
    }

    void OnEnable()
    {
        STGlobalEventNotify.GetInstance().SubscribeEvent((int)STGlobalEventDef.EVENT_CMD_UPDATE_OBS_ENTITY, UpdateOBPlayerEvent);
    }

    void OnDisable()
    {
        STGlobalEventNotify.GetInstance().UnSubscribeEvent((int)STGlobalEventDef.EVENT_CMD_UPDATE_OBS_ENTITY, UpdateOBPlayerEvent);
    }

    public void UpdateOBPlayerEvent(int iEventID, System.Object args)
    {
        Dictionary<string, GameObject> all = STEntityManager.GetInstance().AllEntities();

        mDropdown.ClearOptions();

        List<Dropdown.OptionData> listOptions = new List<Dropdown.OptionData>();
        listOptions.Add(new Dropdown.OptionData("上帝视角"));

        foreach (var temp in all.Keys)
        {
            listOptions.Add(new Dropdown.OptionData(temp));
        }

        mDropdown.AddOptions(listOptions);

        SetDropDownItemValue(mSelectedIndex);
    }

    void AddDropdownListener(UnityAction<int> OnValueChangeListener)
    {
        mDropdown.onValueChanged.AddListener((value) =>
        {
            OnValueChangeListener(value);
        });
    }


    void OnDropdownValueChange(int iIndex)
    {
        Debug.Log("OnDropdownValueChange Index : " + iIndex);

        GameObject worldCamera = GameObject.Find("STServerSceneRoot/GameRoot/WorldCamera");
        GameObject entityRoot = GameObject.Find("STServerSceneRoot/GameRoot/EntityRoot");

        worldCamera.SetActive(0 == iIndex ? true : false);
        SetDropDownItemValue(iIndex);

        for (int i = 1;  i < mDropdown.options.Count; i++)
        {
            if (iIndex == i)
            {
                GameObject obj = entityRoot.transform.Find(mDropdown.options[i].text + "/Main Camera").gameObject;
                obj.SetActive(true);
            }
            else
            {
                GameObject obj = entityRoot.transform.Find(mDropdown.options[i].text + "/Main Camera").gameObject;
                obj.SetActive(false);
            }
        }
    }

    void SetDropDownItemValue(int iItemIndex)
    {
        if (mDropdown.options == null)
        {
            Debug.Log(GetType() + "/SetDropDownItemValue()/下拉列表为空，请检查");
            return;
        }

        if (iItemIndex >= mDropdown.options.Count)
        {
            iItemIndex = mDropdown.options.Count - 1;
        }

        if (iItemIndex < 0)
        {
            iItemIndex = 0;
        }

        mDropdown.value = iItemIndex;
        mSelectedIndex = iItemIndex;
    }

    void Update()
    {
    }
}
