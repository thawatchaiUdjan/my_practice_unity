using UnityEngine;

public class BeamParticle : MonoBehaviour
{
	private void Start()
	{
		var flip = transform.parent.rotation.y;
		var beam = GetComponent<ParticleSystemRenderer>();
		beam.flip = new Vector3(0, flip, 0);
	}
}
