namespace FruitWebApp.Models;

public class FruitModel
{
    public int id { get; set; }
    public string name { get; set; } = "";
    public bool instock { get; set; }
    public int userId { get; set; } // <--- CLEF DE LA SÉPARATION
}

public class UserSession
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
}