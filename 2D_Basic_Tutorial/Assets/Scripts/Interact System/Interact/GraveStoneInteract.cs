public class GraveStoneInteract : Interactable
{
	public override void Interact(Interactor interactor)
	{
		base.Interact(interactor);
		GameManager.instance.GameRevive();
		Destroy(gameObject);
	}
}
