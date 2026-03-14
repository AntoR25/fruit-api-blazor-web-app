using Xunit;
using FruitWebApp.Components.Pages;
using Moq;
using System.Net.Http;

public class HomeTests
{
    [Fact]
    public void Home_Component_Creation_Test()
    {
        // Création d'un HttpClientFactory factice
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

        // Création du composant en injectant la factory
        var home = new Home
        {
            HttpClientFactory = mockFactory.Object
        };

        // Vérifie que l'objet existe
        Assert.NotNull(home);
    }
}