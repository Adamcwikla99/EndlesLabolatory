using System.Collections;
using Events.Scene;
using Structures.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesManager
{
	/// <summary>
	///  Class that implements scene changing logic
	/// </summary>
	public class SceneController : MonoBehaviour
	{
		#region Private Propertis			
		[field: SerializeField]
		private Animator FadeAnimation { get; set; }

		[field: SerializeField]
		private SwitchScene SwitchCurrentScene { get; set; }
		
		#endregion
		#region Private Fields

		private SceneType currentScene = SceneType.None;
		private static readonly int StartAnimation = Animator.StringToHash("Start");

		#endregion
		#region Unity Callbacks

		private void Start()
		{
			Init();
		}

		private void OnEnable()
		{
			EnableEvents();
		}

		private void OnDisable()
		{
			DisableEvents();
		} 

		#endregion
		#region Private Methods

		/// <summary>
		/// method responsible for initialization of variable storing currently active scene
		/// </summary>
		private void Init()
		{
			if (this.currentScene == SceneType.None)
			{
				this.currentScene = SceneType.Menu;
			}
		}
		
		/// <summary>
		/// methode that enables active scene manipulation events
		/// </summary>
		private void EnableEvents()
		{
			this.SwitchCurrentScene.SwitchCurrentScene += LoadScene;
			this.SwitchCurrentScene.RestartGameMap += RestartMap;
		}

		/// <summary>
		/// methode that disables active scene manipulation events
		/// </summary>
		private void DisableEvents()
		{
			this.SwitchCurrentScene.SwitchCurrentScene -= LoadScene;
			this.SwitchCurrentScene.RestartGameMap -= RestartMap;
		}
		
		/// <summary>
		/// methode that changes currently active scene 
		/// </summary>
		private void ChangeCurrentSceneType(SceneType scene) => this.currentScene = scene;

		/// <summary>
		/// methode that loads selected scene
		/// </summary>
		/// <param name="scene"></param>
		private void LoadScene(SceneType scene) => StartCoroutine(LoadNextScene(scene));

		/// <summary>
		/// methode that restarts game scene
		/// </summary>
		private void RestartMap()
		{
			SceneManager.UnloadSceneAsync(1);
			SceneManager.LoadScene(1, LoadSceneMode.Additive);
		}
		
		/// <summary>
		/// methode that loads next scene
		/// </summary>
		private IEnumerator LoadNextScene(SceneType scene)
		{
			this.FadeAnimation.SetTrigger(StartAnimation);
			yield return new WaitForSeconds(1f);
			
			if (this.currentScene == SceneType.Menu)
			{
				ChangeCurrentSceneType(scene);
				SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
			}
			else if (scene == SceneType.Menu)
			{
				Cursor.visible = true;
				ChangeCurrentSceneType(SceneType.Menu);
				SceneManager.LoadSceneAsync((int)SceneType.Menu, LoadSceneMode.Single);
			}
			else
			{
				Cursor.visible = false;
				SceneManager.UnloadSceneAsync((int)currentScene);
				SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
				ChangeCurrentSceneType(scene);
			}
			
		}

		#endregion
	}
}
