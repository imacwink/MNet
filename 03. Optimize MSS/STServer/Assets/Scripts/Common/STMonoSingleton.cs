using UnityEngine;

namespace Common
{
	public class STMonoSingleton<T> : MonoBehaviour where T : Component
	{
		private static T _instance;
		private static GameObject _attachGameObject = null;
	
		public static T GetInstance()
		{
			if (STMonoSingleton<T>._instance == null)
			{
				STMonoSingleton<T>._instance = Object.FindObjectOfType<T>();
				if (STMonoSingleton<T>._instance == null)
				{
					GameObject obj2 = new GameObject(typeof(T).Name);
					Object.DontDestroyOnLoad(obj2);

					STMonoSingleton<T>._instance = obj2.AddComponent<T>();
					GameObject obj1 = GameObject.Find("STMonoSingletonRoot");
					if (obj1 != null)
					{
						obj2.transform.parent = obj1.transform;
					}
					_attachGameObject = obj2;
				}
			}
		
			return STMonoSingleton<T>._instance;
		}
	
		public static void DestroyInstance()
		{
			if (STMonoSingleton<T>._instance != null)
			{
				Object.Destroy(STMonoSingleton<T>._instance.gameObject);
			}
			STMonoSingleton<T>._instance = null;
		
			if (STMonoSingleton<T>._attachGameObject != null)
			{
				Object.Destroy(STMonoSingleton<T>._attachGameObject);
			}
			STMonoSingleton<T>._attachGameObject = null;
		}
	}
}