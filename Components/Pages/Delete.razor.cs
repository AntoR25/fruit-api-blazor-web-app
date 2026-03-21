using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage; // Pour ProtectedLocalStorage
using FruitWebApp.Models;
using System.Net.Http.Json; // Pour GetFromJsonAsync

namespace FruitWebApp.Components.Pages;

public partial class Delete : ComponentBase
{
    [Parameter] public int Id { get; set; }

    [Inject] public IHttpClientFactory HttpClientFactory { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;

    private FruitModel? _fruit;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Vérification de la session
            var result = await ProtectedLocalStorage.GetAsync<UserSession>("userSession");
            
            if (!result.Success || result.Value == null)
            {
                NavigationManager.NavigateTo("/");
                return;
            }

            // Récupération du fruit à supprimer
            try 
            {
                var httpClient = HttpClientFactory.CreateClient("FruitAPI");
                _fruit = await httpClient.GetFromJsonAsync<FruitModel>($"/fruits/{Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }

            StateHasChanged();
        }
    }

    private async Task Submit()
    {
        var httpClient = HttpClientFactory.CreateClient("FruitAPI");
        var response = await httpClient.DeleteAsync($"/fruits/{Id}");

        if (response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/");
        }
    }
}