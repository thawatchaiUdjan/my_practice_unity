using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
	public GameObject healthSystem;
	public float health;
	public float maxHealth = 100f;
	public float decreaseHealth;
	public float decreaseTime = 1.0f;

	[Header("Decrease Health of Distance Ghost")]
	public float decHealthSlow = 1.5f;
	public float decHealthMedium = 10f;
	public float decHealthFast = 50f;

	[Header("Increase Health")]
	public float increaseHealth = 5f;
	public float increaseTime = 1.0f;

	//AnimID
	private string _animIdle = "health-idle";
	private string _animFast = "health-fast";
	private string _animFast2 = "health-fast-2";

	private Animation _animation;
	//
	public static HealthManager instance;

	private void Awake() {
		instance = this;
	}

    void Start()
    {
        _animation = healthSystem.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_animation.isPlaying)
		{
			//Lower 40%
			if (health/maxHealth * 100 <= 40f)
			{
				_animation.Play(_animFast2);
			}
			//Lower 70%
			else if (health/maxHealth * 100 <= 70f)
			{
				_animation.Play(_animFast);
			}
			//Higher 70%
			else if (health/maxHealth * 100 > 70f)
			{
				_animation.Play(_animIdle);
			}
		}
    }

	public void SetupHealth(){
		health = maxHealth;
		decreaseHealth = decHealthSlow;
	}

	public IEnumerator DecreaseHealth(){ 
		while (true)
		{	
			yield return new WaitForSeconds(decreaseTime); 
			if (health > 0f)
			{	
				health -= decreaseHealth;
			}
			else
			{
				health = 0f;
				GameManager.instance.GameOver();
				break;
			}
		}
	}

	public IEnumerator IncreaseHealth(){
		while (true)
		{
			yield return new WaitForSeconds(increaseTime);
			if (health < maxHealth)
			{	
				health += increaseHealth;
			}
			else
			{
				health = maxHealth;
			}
		}
	}

}
