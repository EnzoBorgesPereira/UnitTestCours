using Moq;
using UnitTestCours;
using UnitTestCours.Services;

namespace Tests;

[TestFixture]
public class ToDoListTest
{
    private User user;
    private Mock<IEmailSenderService> emailServiceMock;
    private Mock<IToDoListRepository> repositoryMock;
    private ToDoList todoList;

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

        emailServiceMock = new Mock<IEmailSenderService>();
        repositoryMock = new Mock<IToDoListRepository>();

        todoList = new ToDoList(user, emailServiceMock.Object, repositoryMock.Object);
    }

    [Test]
    public void Add_ShouldReturnFalse_WhenSaveThrowsException()
    {
        repositoryMock.Setup(r => r.Save(It.IsAny<Item>())).Throws(new Exception("Erreur de sauvegarde"));

        var item = new Item("Item1", "Contenu");

        var result = todoList.Add(item);

        Assert.IsFalse(result);
        Assert.IsEmpty(todoList.Items);
    }
    
    [Test]
    public void Add_ShouldReturnTrue_WhenSaveSucceeds()
    {
        repositoryMock.Setup(r => r.Save(It.IsAny<Item>()));

        var item = new Item("Item1", "Contenu");

        var result = todoList.Add(item);

        Assert.IsTrue(result);
        Assert.AreEqual(1, todoList.Items.Count);
        Assert.AreEqual(item, todoList.Items[0]);
    }
    
    [Test]
    public void Add_ShouldReturnFalse_WhenUserIsInvalid()
    {
        user.Email = "email_invalide";
        var item = new Item("Item1", "Contenu");
        var result = todoList.Add(item);

        Assert.IsFalse(result);
    }

    [Test]
    public void Add_ShouldReturnFalse_WhenItemIsInvalid()
    {
        var item = new Item("", "Contenu");
        var result = todoList.Add(item);

        Assert.IsFalse(result);
    }

    [Test]
    public void Add_ShouldReturnFalse_WhenItemNameIsNotUnique()
    {
        var item1 = new Item("Item1", "Contenu1");
        var item2 = new Item("Item1", "Contenu2");

        todoList.Add(item1);
        var result = todoList.Add(item2);

        Assert.IsFalse(result);
    }

    [Test]
    public void Add_ShouldReturnFalse_WhenLessThan30MinutesSinceLastItem()
    {
        var item1 = new Item("Item1", "Contenu1");
        todoList.Add(item1);

        var item2 = new Item("Item2", "Contenu2");
        item2.CreationDate = item1.CreationDate.AddMinutes(10);

        var result = todoList.Add(item2);

        Assert.IsFalse(result);
    }

    [Test]
    public void Add_ShouldSendEmail_When8thItemIsAdded()
    {
        // Ajout de 7 items avec un intervalle de 30 minutes
        for (int i = 1; i <= 7; i++)
        {
            var item = new Item($"Item{i}", "Contenu");
            item.CreationDate = DateTime.Now.AddMinutes(-30 * (7 - i + 1));
            todoList.Add(item);
        }

        // Ajout du 8ème item
        var item8 = new Item("Item8", "Contenu");
        var result = todoList.Add(item8);

        Assert.IsTrue(result);

        emailServiceMock.Verify(es =>
                es.SendEmail(
                    user.Email,
                    "Votre ToDoList est presque remplie",
                    "Votre ToDoList contient 8 éléments."
                ),
            Times.Once
        );
    }

    [Test]
    public void Add_ShouldNotSendEmail_WhenLessThan8Items()
    {
        // Ajout de 7 items
        for (int i = 1; i <= 7; i++)
        {
            var item = new Item($"Item{i}", "Contenu");
            item.CreationDate = DateTime.Now.AddMinutes(-30 * (7 - i + 1));
            todoList.Add(item);
        }

        emailServiceMock.Verify(es =>
                es.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never
        );
    }
}