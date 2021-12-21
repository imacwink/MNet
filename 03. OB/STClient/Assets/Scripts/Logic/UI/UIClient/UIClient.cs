using UnityEngine;
using UnityEngine.UI;
using StateMachine;
using Client;
using Manager;

public class UIClient : MonoBehaviour
{
    [Header("Server Settings")]
    public string mServerIP = "127.0.0.1";
    public int mPort = 6337;
    public string mServerName = "MNetSimple";

    [Header("UI Settings")]
    public Button mBack;

    private STClient mClient;

    void Start()
    {
        mBack.onClick.AddListener(OnBackEvent);

        STClientNetworkManager.GetInstance().Init(mPort, mServerIP, mServerName);
        STEntityManager.GetInstance().Init();
    }

    private void OnBackEvent()
    {
        STClientNetworkManager.GetInstance().SendDisconnect();
        STStateMachine.ChangeState(STStateConfig.s_stateName);
    }
}
