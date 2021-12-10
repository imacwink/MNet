using UnityEngine;
using UnityEngine.SceneManagement;

namespace StateMachine
{
	class STStateServer : IState
	{
		public const string s_stateName = "STStateServer";
		public const string s_sceneName = "STServerScene";

		/// <summary>
		/// 初始化状态
		/// </summary>
		public void InitializeState()
		{
		}

		/// <summary>
		/// 反初始化状态
		/// </summary>
		public void UninitializeState()
		{
		}

        // 状态Update
        public void UpdateState()
        {
        }

		/// <summary>
		/// 状态进入状态栈
		/// </summary>
		public void OnStateEnter()
		{
			SceneManager.sceneLoaded += HandleSceneLoaded;

			SceneManager.LoadSceneAsync(STStateServer.s_sceneName);
		}

		/// <summary>
		/// 场景加载完毕
		/// </summary>
		private void HandleSceneLoaded(Scene scence, LoadSceneMode mod)
		{
		}

		/// <summary>
		/// 状态退出状态栈
		/// </summary>
		public void OnStateLeave()
		{
			SceneManager.sceneLoaded -= HandleSceneLoaded;
		}

		/// <summary>
		/// 状态由栈顶变成非栈顶
		/// </summary>
		public void OnStateOverride()
		{
		}
		
		/// <summary>
		/// 状态由非栈顶变成栈顶
		/// </summary>
		public void OnStateResume()
		{
		}
	}
}
