using System.Collections.Generic;

namespace StateMachine
{
    public interface IState
    {
        //----------------------------------------------
        /// 初始化状态
        //----------------------------------------------
        void InitializeState();

        //----------------------------------------------
        /// 反初始化状态
        //----------------------------------------------
        void UninitializeState();

        //----------------------------------------------
        /// 状态Update
        //----------------------------------------------
        void UpdateState();

        //----------------------------------------------
        /// 状态进入状态栈
        //----------------------------------------------
        void OnStateEnter();

        //----------------------------------------------
        /// 状态退出状态栈
        //----------------------------------------------
        void OnStateLeave();

        //----------------------------------------------
        /// 状态由栈顶变成非栈顶
        //----------------------------------------------
        void OnStateOverride();

        //----------------------------------------------
        /// 状态由非栈顶变成栈顶
        //----------------------------------------------
        void OnStateResume();
    }

    public class STStateMachine
    {
        //状态Map
        private static Dictionary<string, IState> s_registedStates = new Dictionary<string, IState>();

        //状态Stack
        private static Stack<IState> s_stateStack = new Stack<IState>();

        //next状态name
        private static string s_nextStateName;

		//currunt状态name
		private static string s_curruntStateName;

        //----------------------------------------------
        /// 初始化
        //----------------------------------------------
        public static void Init()
        {

            RegisterState(STStateConfig.s_stateName, new STStateConfig());
            RegisterState(STStateServer.s_stateName, new STStateServer());
			RegisterState(STStateClient.s_stateName, new STStateClient());

            // 初始状态为 STStateConfig;
            Push(STStateConfig.s_stateName);
        }

        //----------------------------------------------
        /// 清理
        //----------------------------------------------
        public static void Clean()
        {
            UnRegisterState(STStateConfig.s_stateName);
            UnRegisterState(STStateServer.s_stateName);
			UnRegisterState(STStateClient.s_stateName);
        }

        //----------------------------------------------
        /// 状态切换
        /// @name
        /// @changeImmediately : 为true时直接切换（可能会导致某些状态机的enter，exit工作不正常，慎用！！！）
        //----------------------------------------------
        public static void  ChangeState(string nextStateName, bool changeImmediately = false)
        {
            if (nextStateName == null)
            {
                return;
            }

            s_nextStateName = nextStateName;

            if (changeImmediately)
            {
                ExecuteChangeState();
            }
        }

        //----------------------------------------------
        /// Update 栈顶状态
        //----------------------------------------------
        public static void Update()
        {
            //执行切换
            if (s_nextStateName != null)
            {
                ExecuteChangeState();
            }

            //Update
            IState topState = TopState();

            if (topState != null)
            {
                topState.UpdateState();
            }
        }

        //----------------------------------------------
        /// 注册状态
        //----------------------------------------------
        private static void RegisterState(string name, IState state)
        {
            if (name == null || state == null)
            {
                return;
            }

            if (s_registedStates.ContainsKey(name))
            {
                return;
            }

            state.InitializeState();
            s_registedStates.Add(name, state);
        }

        //----------------------------------------------
        /// 注销状态
        //----------------------------------------------
        private static IState UnRegisterState(string name)
        {
            if (name == null)
            {
                return null;
            }

            IState state;
            if (!s_registedStates.TryGetValue(name, out state))
            {
                return null;
            }

            state.UninitializeState();
            s_registedStates.Remove(name);

            return state;
        }

        //----------------------------------------------
        /// 压入状态
        //----------------------------------------------
        private static void Push(IState state)
        {
            if (state == null)
            {
                return;
            }

            if (s_stateStack.Count > 0)
            {
                s_stateStack.Peek().OnStateOverride();
            }

            s_stateStack.Push(state);

            state.OnStateEnter();
        }

        //----------------------------------------------
        /// 压入状态
        //----------------------------------------------
        private static void Push(string name)
        {
            if (name == null)
            {
                return;
            }

            IState state;
            if (!s_registedStates.TryGetValue(name, out state))
            {
                return;
            }

            Push(state);
        }

        //----------------------------------------------
        /// 弹出状态
        //----------------------------------------------
        private static IState PopState()
        {
            if (s_stateStack.Count == 0)
            {
                return null;
            }

            IState state = s_stateStack.Pop();
            state.OnStateLeave();

            if (s_stateStack.Count > 0)
            {
                s_stateStack.Peek().OnStateResume();
            }

            return state;
        }

        //----------------------------------------------
        /// 获取栈顶状态
        //----------------------------------------------
        private static IState TopState()
        {
            if (s_stateStack.Count <= 0)
            {
                return null;
            }

            return s_stateStack.Peek();
        }

        //----------------------------------------------
        /// 清空堆栈
        //----------------------------------------------
        public static void Clear()
        {
            while (s_stateStack.Count > 0)
            {
                s_stateStack.Pop().OnStateLeave();
            }
        }

        //----------------------------------------------
        /// 执行状态切换
        //----------------------------------------------
        private static void ExecuteChangeState()
        {
            IState state = null;            
            s_registedStates.TryGetValue(s_nextStateName, out state);
			s_curruntStateName = s_nextStateName;
            s_nextStateName = null;

            if (state == null)
            {
                return;
            }

            //当前正处于该状态，不需切换
            if (state == s_stateStack.Peek())
            {
                return;
            }

            PopState();
            Push(state);
        }

		//----------------------------------------------
		/// 获取当前状态
		//----------------------------------------------
		public static string GetCurrentState()
		{
			return s_curruntStateName;
		}
    }
}