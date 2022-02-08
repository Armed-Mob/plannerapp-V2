using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PlannerApp.Shared.Models;
using PlannerApp.Shared.Responses;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PlannerApp.Components
{
    public partial class RegisterForm
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public ILocalStorageService Storage { get; set; }



        private RegisterRequest _model = new RegisterRequest();

        private bool _isBusy = false;

        private string _errorMessage = string.Empty;



        private async Task RegisterUserAsync()
        {
            _isBusy = true;
            _errorMessage = string.Empty;


            var response = await HttpClient.PostAsJsonAsync("/api/v2/auth/register", _model);

            if (response.IsSuccessStatusCode)
            {
                
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<RegisterResult>>();
                // Store it in local storage

                string hash = result.Value.Password.GetHashCode().ToString();

                await Storage.SetItemAsStringAsync("username", result.Value.UserName);
                await Storage.SetItemAsStringAsync("firstname", result.Value.FirstName);
                await Storage.SetItemAsStringAsync("lastname", result.Value.LastName);
                await Storage.SetItemAsStringAsync("email", result.Value.Email);
                await Storage.SetItemAsStringAsync("password", result.Value.Password); 
                await Storage.SetItemAsStringAsync("passwordhash", hash);
                await Storage.SetItemAsync<DateTime>("createdat", result.Value.CreatedAt);

                await AuthenticationStateProvider.GetAuthenticationStateAsync();

                Navigation.NavigateTo("/");
            }
            else
            {
                var errorResult = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

                _errorMessage = errorResult.Message;
            }

            _isBusy = false;
        }
    }
}
