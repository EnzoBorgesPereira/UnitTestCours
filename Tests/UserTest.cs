using UnitTestCours;

namespace Tests;

[TestFixture]
public class UserTest
{
	private User user;

	[SetUp]
	public void Setup()
	{
		user = new User
		{
			Email = "jean.dupont@example.com",
			FirstName = "Jean",
			LastName = "Dupont",
			Password = "MotDePasse123",
			DateOfBirth = DateTime.Today.AddYears(-20)
		};
	}

	[Test]
	public void IsValid_ShouldReturnFalse_WhenEmailIsInvalid()
	{
		user.Email = "email_invalide";
		Assert.IsFalse(user.IsValid());
	}

	[Test]
	public void IsValid_ShouldReturnFalse_WhenFirstNameIsMissing()
	{
		user.FirstName = "";
		Assert.IsFalse(user.IsValid());
	}

	[Test]
	public void IsValid_ShouldReturnFalse_WhenLastNameIsMissing()
	{
		user.LastName = "";
		Assert.IsFalse(user.IsValid());
	}

	[Test]
	public void IsValid_ShouldReturnFalse_WhenPasswordIsInvalid()
	{
		user.Password = "mdp";
		Assert.IsFalse(user.IsValid());
	}

	[Test]
	public void IsValid_ShouldReturnFalse_WhenUserIsUnder13()
	{
		user.DateOfBirth = DateTime.Today.AddYears(-12);
		Assert.IsFalse(user.IsValid());
	}

	[Test]
	public void IsValid_ShouldReturnTrue_WhenUserIsValid()
	{
		Assert.IsTrue(user.IsValid());
	}
}