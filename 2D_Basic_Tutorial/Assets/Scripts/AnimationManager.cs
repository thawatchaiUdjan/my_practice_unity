using UnityEngine;

public class AnimationManager : MonoBehaviour
{
	private PlayerController _player;
	private Rigidbody2D _rigidBody;
	private PlayerData _playerData;
	private SaveManager _save;
	private SoundManager _sound;
	private GameManager _game;
	private LayerMask _attackMask;

	void Start()
	{
		_playerData = PlayerData.instance;
		_save = SaveManager.instance;
		_sound = SoundManager.instance;
		_game = GameManager.instance;
		_player = GetComponent<PlayerController>();
		_rigidBody = GetComponent<Rigidbody2D>();

		_attackMask = LayerMask.GetMask("Enemy");
	}

	private void OnFootstep()
	{
		AudioSource.PlayClipAtPoint(_sound.footsteps, transform.position, _sound.audioVolume * 0.6f);
	}

	private void OnAttackHit(int attackCombo)
	{
		AudioClip clip = null;
		var hitsPoint = Physics2D.OverlapCircleAll(_player.attackPoint.position, _player.attackRange, _attackMask);
		var damage = 0;

		switch (attackCombo)
		{
			case 1:
				clip = _sound.swordSwipe[0];
				damage = _player.attackDamageCombo_1;
				break;
			case 2:
				clip = _sound.swordSwipe[1];
				damage = _player.attackDamageCombo_2;
				break;
			case 3:
				clip = _sound.swordSwipe[2];
				damage = _player.attackDamageCombo_3;
				break;
		}

		foreach (var hit in hitsPoint)
		{
			var randDmg = Random.Range(damage - _player.damageRange, damage + _player.damageRange);
			if (hit.TryGetComponent(out Enemy enemy)){
				enemy.TakeDamage(randDmg);
			}
			Debug.Log($"Hit {hit.name} : [{randDmg} Damage]");
		}

		AudioSource.PlayClipAtPoint(clip, transform.position, _sound.audioVolume * 0.7f);
	}

	private void OnAttackCombo(int attackCombo)
	{
		float movePos = 0f;

		switch (attackCombo)
		{
			case 1: movePos = _player.distanceCombo_1; break;
			case 3: movePos = _player.distanceCombo_3; break;
		}

		_rigidBody.MovePosition(transform.position + transform.right * movePos);
		// _rigidBody.AddForce(transform.position + transform.right * movePos);
		// transform.Translate(Vector2.right * movePos);
	}

}
