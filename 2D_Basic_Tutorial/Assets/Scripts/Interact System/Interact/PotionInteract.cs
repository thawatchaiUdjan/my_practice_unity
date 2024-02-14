public class PotionInteract : Interactable
{
	public override void Interact(Interactor interactor)
	{
		base.Interact(interactor);
		interactor.GetComponent<PotionManager>().AddPotion();
		SoundManager.instance.OnPotionPickup();
		Destroy(gameObject);
	}
}
