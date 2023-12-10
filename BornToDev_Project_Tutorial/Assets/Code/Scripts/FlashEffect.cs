using System.Collections;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
	[SerializeField] private Material _flashMaterial;
	[SerializeField] private Color _flashColor = Color.white;
	public float flashDuration = 0.1f;

	//
	private Renderer _renderer;
	private Material _originalMate;
	private Coroutine _flashing;

	private void Awake()
	{
		if (TryGetComponent(out _renderer))
		{
			_originalMate = _renderer.material;
		}
	}

	public void Show()
	{
		if (_flashing != null) Reset();
		_flashing = StartCoroutine(Flash());
	}

	private IEnumerator Flash()
	{
		if (_renderer != null)
		{
			_originalMate = _renderer.material;
			_renderer.material = _flashMaterial;
			_renderer.material.color = _flashColor;
			yield return new WaitForSeconds(flashDuration);
			Reset();
		}
		else
		{
			Debug.LogWarning("Don't have Renderer component to Flash Effect.");
		}

		_flashing = null;
	}

	public void Reset()
	{
		if (_renderer != null)
		{
			StopAllCoroutines();
			_renderer.material = _originalMate;
		}
		else
		{
			Debug.LogWarning("Don't have Renderer component to Flash Effect.");
		}
	}
}
