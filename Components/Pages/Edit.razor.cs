using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using FruitWebApp.Models;
using System.Net.Http.Json;

namespace FruitWebApp.Components.Pages;

public partial class Edit : ComponentBase
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
            var res = await ProtectedLocalStorage.GetAsync<UserSession>("userSession");
            if (!res.Success) NavigationManager.NavigateTo("/");

            var client = HttpClientFactory.CreateClient("FruitAPI");
            _fruit = await client.GetFromJsonAsync<FruitModel>($"/fruits/{Id}");
            StateHasChanged();
        }
    }

    private async Task Submit()
    {
        var client = HttpClientFactory.CreateClient("FruitAPI");
        // On s'assure de renvoyer le fruit avec son userId
        await client.PutAsJsonAsync($"/fruits/{Id}", _fruit);
        NavigationManager.NavigateTo("/");
    }
}