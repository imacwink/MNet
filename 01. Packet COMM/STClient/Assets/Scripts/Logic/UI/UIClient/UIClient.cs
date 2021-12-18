using UnityEngine;
using UnityEngine.UI;
using StateMachine;
using Manager;

public class UIClient : MonoBehaviour
{
    [Header("Server Settings")]
    public string mServerIP = "127.0.0.1";
    public int mPort = 6337;
    public string mServerName = "MNetSimple";

    [Header("UI Settings")]
    public Button mBack;

    void Start()
    {
        mBack.onClick.AddListener(OnBackEvent);

        STEntityManager.GetInstance().Init(mPort, mServerIP, mServerName);
    }

    private void OnBackEvent()
    {
        STStateMachine.ChangeState(STStateConfig.s_stateName);
    }
}
