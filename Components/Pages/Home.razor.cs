using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using FruitWebApp.Models;
using System.Net.Http.Json;

namespace FruitWebApp.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject] public IHttpClientFactory HttpClientFactory { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;

    private List<FruitModel>? _fruitList;
    private UserSession? CurrentUser;
    private string Username = "";
    private string Password = "";
    private string ErrorMessage = "";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var res = await ProtectedLocalStorage.GetAsync<UserSession>("userSession");
            if (res.Success && res.Value != null)
            {
                CurrentUser = res.Value;
                await LoadFruits();
                StateHasChanged();
            }
        }
    }

    private async Task LoadFruits()
    {
        var client = HttpClientFactory.CreateClient("FruitAPI");
        // On demande à l'API uniquement les fruits de CET utilisateur
        _fruitList = await client.GetFromJsonAsync<List<FruitModel>>($"/fruits?userId={CurrentUser!.Id}");
    }

    private async Task LoginUser()
    {
        if (Username == "admin" && Password == "admin") CurrentUser = new UserSession { Id = 1, Username = "Admin" };
        else if (Username == "toto" && Password == "toto") CurrentUser = new UserSession { Id = 2, Username = "Toto" };

        if (CurrentUser != null)
        {
            await ProtectedLocalStorage.SetAsync("userSession", CurrentUser);
            await LoadFruits();
        } else { ErrorMessage = "Erreur d'identifiants"; }
    }

    private async Task Logout() {
        CurrentUser = null;
        await ProtectedLocalStorage.DeleteAsync("userSession");
    }

    private void EditButton(int id) => NavigationManager.NavigateTo($"/edit/{id}");
    private void DeleteButton(int id) => NavigationManager.NavigateTo($"/delete/{id}");
}