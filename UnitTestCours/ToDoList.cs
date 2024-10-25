using UnitTestCours.Services;

namespace UnitTestCours;

public class ToDoList(User owner, IEmailSenderService emailSenderService, IToDoListRepository repository)
{
	private const int MaxItems = 10;
	private const int NotifyItemCount = 8;
	private const int MinTimeBetweenItemsMinutes = 30;

	public User Owner { get; set; } = owner;
	public List<Item> Items { get; private set; } = new();
	
	public bool Add(Item item)
	{
		if (!Owner.IsValid())
			return false;

		if (Items.Count >= MaxItems)
			return false;

		if (!item.IsValid())
			return false;

		if (Items.Any(i => i.Name == item.Name))
			return false;

		if (Items.Count > 0)
		{
			var lastItem = Items.Last();
			if ((item.CreationDate - lastItem.CreationDate).TotalMinutes < MinTimeBetweenItemsMinutes)
				return false;
		}

		try
		{
			repository.Save(item);
		}
		catch (Exception)
		{
			// Gestion de l'exception
			return false;
		}

		Items.Add(item);

		if (Items.Count == NotifyItemCount)
		{
			emailSenderService.SendEmail(
				Owner.Email,
				"Votre ToDoList est presque remplie",
				"Votre ToDoList contient 8 éléments."
			);
		}

		return true;
	}
}