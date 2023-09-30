using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
	public GameObject objectiveObject;
	public TextMeshProUGUI objectiveQty;
	public int numOfObjectives;
	public List<Transform> posOfObjectives = new List<Transform>();

	private List<Transform> objectsPosOnMap = new List<Transform>();
	private int progressOfObjective = 0;

	//String
	private string _preGameClearText = "Find the way...";
	
    //
	public static ObjectiveManager instance;
	private void Awake() {
		instance = this;
	}
    void Start()
    {
		foreach (var item in GameObject.FindGameObjectsWithTag("Objective")){
			objectsPosOnMap.Add(item.transform);
		}

    }

    void Update()
    {
		
    }

	public void SetupObjective()
	{
		objectiveObject.SetActive(true);
		objectiveQty.gameObject.SetActive(true);

		numOfObjectives = Random.Range(5, 9); //Random number of objectives

		ShufflePositions(objectsPosOnMap); //Random Position
		SetObjectivePos(posOfObjectives[0]);

		SetObjectiveText($"{progressOfObjective}/{numOfObjectives}");
	}

	public void ReSetObjective()
	{
		objectiveObject.SetActive(false);
		objectiveQty.gameObject.SetActive(false);
		progressOfObjective = 0;
		numOfObjectives = 0;
	}

	public void ShufflePositions(List<Transform> list)
	{
		for (var i = 0; i < list.Count-1; ++i)
		{
			var r = Random.Range(i, list.Count);
			var tmp = list[i];
			list[i] = list[r];
			list[r] = tmp;
		}

		for (int i = 0; i < numOfObjectives; i++) 
		{
			posOfObjectives.Add(list[i]);
		}
	}

	public void SetObjectivePos(Transform transform){
		objectiveObject.transform.position = transform.position;
		objectiveObject.transform.rotation = transform.rotation;
	}

	public void GetObjective(){		
		progressOfObjective++;
		SetObjectiveText($"{progressOfObjective}/{numOfObjectives}");

		if (IsPreGameClear()){
			objectiveObject.SetActive(false);
			SetObjectiveText(_preGameClearText);
			GameManager.instance.PreGameClear();
		}
		else SetObjectivePos(posOfObjectives[progressOfObjective]);	
	}

	public bool IsPreGameClear(){
		return progressOfObjective >= numOfObjectives;
	}

	public void SetObjectiveText(string text){
		objectiveQty.text = text;
	}

}
