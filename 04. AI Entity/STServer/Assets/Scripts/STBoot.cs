using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using StateMachine;
using Manager;

public class STBoot : STMonoSingleton<STBoot>
{
	void Awake()
	{
	}

	void Start()
	{
		// 设置屏幕不锁屏;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		// 当前节点不能删除;
		Object.DontDestroyOnLoad(gameObject);

		// 状态机初始化;
		STStateMachine.Init();
	}

	private void Update()
	{
		STStateMachine.Update();
	}

	private void FixedUpdate()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnApplicationQuit()
	{
		STClientNetworkManager.GetInstance().SendDisconnect();
	}
}
