using System.Collections;
using UnityEngine;

public class ChestInteract : Interactable
{
	[SerializeField] private GameObject[] _itemsDrop;
	public int soulDrop = 200;

	[Header("Item Drop System")]
	public float forceY = 100f;
	public float forceX = 80f;
	public float forceDelay = 0.2f;

	//
	private PlayerData _playerData;
	private Vector2 _dropPosition;

	private void Start()
	{
		_playerData = PlayerData.instance;
		_dropPosition = GetComponent<Collider2D>().bounds.center;
	}

	//AnimID
	private string _animOpen = "Open";
	public override void Interact(Interactor interactor)
	{
		base.Interact(interactor);
		isInteractable = false;
		interactor.HideText();
		SoundManager.instance.OnOpenChest();
		GetComponent<Animator>().SetTrigger(_animOpen);
		interactor.GetComponent<SoulManager>().DropSoul(_dropPosition, soulDrop);
		_playerData.FindSceneData().chestOpened = true;
		StartCoroutine(OpenChest());
	}

	private IEnumerator OpenChest()
	{
		if (_itemsDrop != null)
		yield return StartCoroutine(DropItem());
		yield return StartCoroutine(GetComponent<DissolveEffect>().Dissolve());
		yield return new WaitForSeconds(5f);
		Destroy(gameObject);
	}

	private IEnumerator DropItem()
	{
		var newForceX = 0f;

		foreach (var item in _itemsDrop)
		{
			var itemObj = Instantiate(item, _dropPosition, item.transform.rotation);
			newForceX = RandomForceX(newForceX);
			var forcePos = new Vector2(newForceX, forceY);
			itemObj.GetComponent<Rigidbody2D>().AddForce(forcePos);
			yield return new WaitForSeconds(forceDelay);
		}
	}

	private float RandomForceX(float newForceX)
	{
		var min = -forceX;
		var max = forceX;

		if (newForceX > 0) max = 0f;
		else if (newForceX < 0) min = 0f;

		newForceX = Random.Range(min, max);
		return newForceX;
	}

}
