using UnityEngine;
using UnityEngine.SceneManagement;

namespace StateMachine
{
	class STStateClient : IState
	{
		public const string s_stateName = "STStateClient";
		public const string s_sceneName = "STClientScene";

		//----------------------------------
		/// 初始化状态
		//----------------------------------
		public void InitializeState()
		{
		}
		
		//----------------------------------
		/// 反初始化状态
		//----------------------------------
		public void UninitializeState()
		{
		}
		
		//----------------------------------
		/// 状态Update
		//----------------------------------
		public void UpdateState()
		{
		}
		
		//----------------------------------
		/// 状态进入状态栈
		//----------------------------------
		public void OnStateEnter()
		{
            Resources.UnloadUnusedAssets();

			SceneManager.sceneLoaded += HandleSceneLoaded;

			SceneManager.LoadSceneAsync(STStateClient.s_sceneName);
		}

		/// <summary>
		/// 场景加载完毕
		/// </summary>
		private void HandleSceneLoaded(Scene scence, LoadSceneMode mod)
		{
		}
		
		//----------------------------------
		/// 状态退出状态栈
		//----------------------------------
		public void OnStateLeave()
		{
		}
		
		//----------------------------------
		/// 状态由栈顶变成非栈顶
		//----------------------------------
		public void OnStateOverride()
		{
			
		}
		
		//----------------------------------
		/// 状态由非栈顶变成栈顶
		//----------------------------------
		public void OnStateResume()
		{
			
		}
	}
}
