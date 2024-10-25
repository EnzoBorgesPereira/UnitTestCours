namespace UnitTestCours;

public class User
{
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Password { get; set; }
	public DateTime DateOfBirth { get; set; }

	public bool IsValid()
	{
		return IsEmailValid() && HasFirstAndLastName() && IsPasswordValid() && IsAtLeast13YearsOld();
	}

	private bool IsEmailValid()
	{
		try
		{
			var addr = new System.Net.Mail.MailAddress(Email);
			return addr.Address == Email;
		}
		catch
		{
			return false;
		}
	}

	private bool HasFirstAndLastName()
	{
		return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName);
	}

	private bool IsPasswordValid()
	{
		if (Password == null || Password.Length < 8 || Password.Length > 40)
			return false;

		var hasLower = Password.Any(char.IsLower);
		var hasUpper = Password.Any(char.IsUpper);
		var hasDigit = Password.Any(char.IsDigit);

		return hasLower && hasUpper && hasDigit;
	}

	private bool IsAtLeast13YearsOld()
	{
		var age = DateTime.Today.Year - DateOfBirth.Year;
		if (DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
		return age >= 13;
	}
}