using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData
{
	//General Status
	public float health;
	public int soul, potions, maxPotion;
	public int scene;
	public Vector2 pos;

	//Dead Status
	public int deadScene;
	public int deadSoulBefore;
	public Vector2 deadPosition;

	//Scene Data
	public List<SceneData> scenes;

	//Default Value
	private int defaultHealth = 100;
	private int defaultPotions = 2;
	private int defaultMaxPotion = 2;
	private int defaultSoul = 0;
	private int defaultScene = 1;

	private static PlayerData Instance;
	public static PlayerData instance
	{
		get
		{
			Instance ??= new PlayerData(newGame: true);
			return Instance;
		}
	}

	public PlayerData(bool newGame = false)
	{
		health = newGame ? defaultHealth : 0f;
		potions = newGame ? defaultPotions : 0;
		maxPotion = newGame ? defaultMaxPotion : 0;
		soul = defaultSoul;
		scene = newGame ? defaultScene : -1;
		pos = Vector2.zero;
		scenes = new List<SceneData>();
		
		ResetDead();
	}

	public void SetPlayerData(PlayerData data)
	{
		health = data.health;
		potions = data.potions;
		maxPotion = data.maxPotion;
		soul = data.soul;
		scene = data.scene;
		pos = data.pos;

		deadScene = data.deadScene;
		deadSoulBefore = data.deadSoulBefore;
		deadPosition = data.deadPosition;

		scenes = data.scenes;
	}

	public void ResetDead()
	{
		deadScene = 0;
		deadSoulBefore = 0;
		deadPosition = Vector2.zero;
	}

	public SceneData FindSceneData()
	{
		var sceneIndex = SceneManager.GetActiveScene().buildIndex;
		var scene = scenes.Find(scene => scene.sceneIndex.Equals(sceneIndex));
		return scene;
	}

	public SceneData FindSavePointNear()
	{
		var currScene = FindSceneData();
		if (currScene.savePointOpened){
			return currScene;
		}
		var nearScenes = scenes.Where(scene => scene.savePointOpened);
		var scene = nearScenes.Count() == 0 ? scenes[0] : nearScenes.Last(); 
		return scene;
	}
}