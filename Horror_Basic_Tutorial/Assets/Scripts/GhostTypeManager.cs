using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTypeManager : MonoBehaviour
{	
	public Anim animType = new Anim();
	public enum Anim { Idle, Walk, Humming, Crying, Sitting, ThrillerIdle, ZombieIdle};
}
