using System.Collections;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration = 0.125f;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;

    void Start()
    {
     	spriteRenderer = GetComponent<SpriteRenderer>();       
		originalMaterial = spriteRenderer.material;
    }

    public void Flash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
		flashRoutine = StartCoroutine(FlashRoutine());        
    }

    private IEnumerator FlashRoutine()
    {  
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = originalMaterial;
        flashRoutine = null;
    }

}