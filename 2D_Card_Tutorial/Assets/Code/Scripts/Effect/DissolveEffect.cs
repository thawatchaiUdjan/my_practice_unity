using System.Collections;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    public float speedDissolve = 0.5f;
	public float dissolveDoneAmount = 1f;

	//
	private Material _material;
	private float _dissolveAmount = 1;
	private float _dissolveTarget;
	private bool _isDissolve = false;
	private float _dissolveDeltaTime;

	//Shader String
	private string _strDissolveAmt = "_DissolveAmount";

	private void Awake()
	{
		_material = GetComponent<SpriteRenderer>().material;
	}

	private void Update()
	{
		CheckDissolve();
	}

	private void CheckDissolve()
	{
		if (_isDissolve)
		{
			_dissolveAmount = Mathf.MoveTowards(_dissolveAmount, _dissolveTarget, speedDissolve * Time.deltaTime);
			_material.SetFloat(_strDissolveAmt, _dissolveAmount);

			_dissolveDeltaTime = _dissolveTarget <= _dissolveAmount ? 1f - _dissolveAmount : _dissolveAmount;

			if (_dissolveDeltaTime >= dissolveDoneAmount)
			{
				_isDissolve = false;
				_dissolveDeltaTime = 0f;
				_dissolveAmount = _dissolveTarget;
				_material.SetFloat(_strDissolveAmt, _dissolveAmount);
			}
		}
	}

	public IEnumerator Dissolve()
	{
		_material.SetFloat(_strDissolveAmt, 1f);
		_dissolveAmount = 1f;
		_dissolveTarget = 0f;
		_isDissolve = true;
		yield return new WaitUntil(() => !_isDissolve);
	}

	public IEnumerator Resolve()
	{
		_material.SetFloat(_strDissolveAmt, 0f);
		_dissolveAmount = 0f;
		_dissolveTarget = 1f;
		_isDissolve = true;
		yield return new WaitUntil(() => !_isDissolve);
	}
}
