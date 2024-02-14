using System;
using UnityEngine.SceneManagement;

[Serializable]
public class SceneData
{
	public int sceneIndex;
	public string sceneName;

	//
	public bool bossDead;
	public bool chestOpened;
	public bool savePointOpened;

	public SceneData()
	{
		var currScene = SceneManager.GetActiveScene();
		sceneIndex = currScene.buildIndex;
		sceneName = currScene.name;
		
		bossDead = false;
		chestOpened = false;
		savePointOpened = false;
	}

}
