namespace UnitTestCours;

public class Item(string name, string content)
{
	public string Name { get; set; } = name;
	public string Content { get; set; } = content;
	public DateTime CreationDate { get; set; } = DateTime.Now;

	public bool IsValid()
	{
		return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Content) && Content.Length <= 1000;
	}
}