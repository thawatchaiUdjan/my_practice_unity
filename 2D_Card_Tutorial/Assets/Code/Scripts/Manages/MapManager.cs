using UnityEngine;

public class MapManager : MonoBehaviour
{
	[SerializeField] private GameObject _lightNormal;
	[SerializeField] private GameObject _lightBoss;
	[SerializeField] private GameObject _normalEffect;
	[SerializeField] private GameObject _bossEffect;

	//
	private GameObject _currLight;
	private GameObject _currEffect;

	//
	public static MapManager instance;
	private void Awake()
	{
		instance = this;
		_currLight = _lightNormal;
		_currEffect = _normalEffect;
	}

	public void NormalMap()
	{
		SetLight(_lightNormal);
		SetEffect(_normalEffect);
	}

	public void BossMap()
	{
		SetLight(_lightBoss);
		SetEffect(_bossEffect);
	}

	public void OnTurnLight(bool isOn)
	{
		_currLight.GetComponent<LightManager>().TurnOn(isOn);
	}

	private void SetLight(GameObject light)
	{
		if (_currLight != light) _currLight.SetActive(false);
		_currLight = light;
		_currLight.SetActive(true);		
	}

	private void SetEffect(GameObject effect)
	{
		if (_currEffect != effect) _currEffect.SetActive(false);
		_currEffect = effect;
		_currEffect.SetActive(true);		
	}
}
