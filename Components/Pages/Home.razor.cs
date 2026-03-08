using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using FruitWebApp.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Web;

namespace FruitWebApp.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    public required IHttpClientFactory HttpClientFactory { get; set; }

    [Inject]
    private NavigationManager? NavigationManager { get; set; }

    [Inject]
    private ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;

    private IEnumerable<FruitModel>? _fruitList;

    // --- Login ---
    private string Username { get; set; } = "";
    private string Password { get; set; } = "";
    private string ErrorMessage { get; set; } = "";
    private bool IsLoggedIn { get; set; } = false;

    // OnInitializedAsync pour initialisation sans JS interop
    protected override Task OnInitializedAsync()
    {
        // On ne fait rien avec ProtectedLocalStorage ici pour éviter l'erreur de prerender
        return Task.CompletedTask;
    }

    // OnAfterRenderAsync pour JS interop (localStorage)
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Lire le flag de login depuis le localStorage
            var result = await ProtectedLocalStorage.GetAsync<bool>("isLoggedIn");
            IsLoggedIn = result.Success && result.Value;

            if (IsLoggedIn)
            {
                await LoadFruits();
            }

            // Forcer le rendu pour mettre à jour l'UI après la lecture du localStorage
            StateHasChanged();
        }
    }

    private async Task LoadFruits()
    {
        var httpClient = HttpClientFactory.CreateClient("FruitAPI");
        using HttpResponseMessage response = await httpClient.GetAsync("/fruits");

        if (response.IsSuccessStatusCode)
        {
            using var contentStream = await response.Content.ReadAsStreamAsync();
            _fruitList = await JsonSerializer.DeserializeAsync<IEnumerable<FruitModel>>(contentStream);
        }
        else
        {
            Console.WriteLine($"Failed to load fruit list. Status code: {response.StatusCode}");
        }
    }

    private void DeleteButton(int id) => NavigationManager!.NavigateTo($"/delete/{id}");
    private void EditButton(int id) => NavigationManager!.NavigateTo($"/edit/{id}");

    // --- Gestion du login ---
    private async Task LoginUser()
    {
        if (Username == "admin" && Password == "admin")
        {
            IsLoggedIn = true;
            ErrorMessage = "";

            // Stocker le login dans localStorage
            await ProtectedLocalStorage.SetAsync("isLoggedIn", true);

            await LoadFruits(); // Charger la liste après login
        }
        else
        {
            ErrorMessage = "Nom d'utilisateur ou mot de passe incorrect";
        }
    }

    private async Task Logout()
    {
        IsLoggedIn = false;
        Username = "";
        Password = "";
        _fruitList = null;

        // Supprimer le flag dans localStorage
        await ProtectedLocalStorage.DeleteAsync("isLoggedIn");
    }
}