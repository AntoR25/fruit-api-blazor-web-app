using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using FruitWebApp.Models;
using System.Net.Http.Json;

namespace FruitWebApp.Components.Pages;

public partial class Add : ComponentBase
{
    [Inject] public IHttpClientFactory HttpClientFactory { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public ProtectedLocalStorage ProtectedLocalStorage { get; set; } = default!;

    private FruitModel _fruit = new();

    private async Task Submit()
    {
        var res = await ProtectedLocalStorage.GetAsync<UserSession>("userSession");
        if (res.Success && res.Value != null)
        {
            // ON LIE LE FRUIT À L'UTILISATEUR ICI
            _fruit.userId = res.Value.Id;

            var client = HttpClientFactory.CreateClient("FruitAPI");
            await client.PostAsJsonAsync("/fruits", _fruit);
            NavigationManager.NavigateTo("/");
        }
    }
}