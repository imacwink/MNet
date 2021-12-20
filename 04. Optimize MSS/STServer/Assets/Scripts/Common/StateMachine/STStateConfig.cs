using UnityEngine;
using UnityEngine.SceneManagement;

namespace StateMachine
{
    public class STStateConfig : IState
    {
        public const string s_stateName = "STStateLogo";
		public const string s_sceneName = "STConfigScene";
       
		private bool m_sceneLoaded = false;

        //----------------------------------------------
        /// 初始化状态
        //----------------------------------------------
        public void InitializeState()
        {
			m_sceneLoaded = false;
        }

        //----------------------------------------------
        /// 反初始化状态
        //----------------------------------------------
        public void UninitializeState()
        {

        }

        //----------------------------------------------
        /// 状态Update
        //----------------------------------------------
        public void UpdateState()
        {
        }

        //----------------------------------------------
        /// 状态进入状态栈
        //----------------------------------------------
        public void OnStateEnter()
        {
			m_sceneLoaded = false;

			SceneManager.sceneLoaded += HandleSceneLoaded;

			SceneManager.LoadSceneAsync(STStateConfig.s_sceneName);
        }

		//----------------------------------------------
		/// 场景加载完成
		//----------------------------------------------
		private void HandleSceneLoaded(Scene scence, LoadSceneMode mod)
		{
			m_sceneLoaded = true;
		}

        //----------------------------------------------
        /// 状态退出状态栈
        //----------------------------------------------
        public void OnStateLeave()
        {
			m_sceneLoaded = false;

			SceneManager.sceneLoaded -= HandleSceneLoaded;

			Resources.UnloadUnusedAssets();
        }

        //----------------------------------------------
        /// 状态由栈顶变成非栈顶
        //----------------------------------------------
        public void OnStateOverride()
        {

        }

        //----------------------------------------------
        /// 状态由非栈顶变成栈顶
        //----------------------------------------------
        public void OnStateResume()
        {

        }
    }
}