using UnitTestCours;

namespace Tests;

[TestFixture]
public class ItemTest
{
	private Item item;

	[SetUp]
	public void Setup()
	{
		item = new Item("Item1", "Contenu");
	}

	[Test]
	public void IsValid_ShouldReturnFalse_WhenNameIsEmpty()
	{
		item.Name = "";
		Assert.IsFalse(item.IsValid());
	}

	[Test]
	public void IsValid_ShouldReturnFalse_WhenContentIsNull()
	{
		item.Content = null;
		Assert.IsFalse(item.IsValid());
	}

	[Test]
	public void IsValid_ShouldReturnFalse_WhenContentIsTooLong()
	{
		item.Content = new string('a', 1001);
		Assert.IsFalse(item.IsValid());
	}

	[Test]
	public void IsValid_ShouldReturnTrue_WhenItemIsValid()
	{
		Assert.IsTrue(item.IsValid());
	}
}