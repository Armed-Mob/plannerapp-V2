using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlannerApp
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {

        private readonly ILocalStorageService _storage;

        public JwtAuthenticationStateProvider(ILocalStorageService storage)
        {
            _storage = storage;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //return new AuthenticationState(new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("Id", "3354") }, "Bearer")));

            if (await _storage.ContainKeyAsync("access_token"))
            {
                //The user is logged in
                var tokenAsString = await _storage.GetItemAsStringAsync("access_token");
                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.ReadJwtToken(tokenAsString);
                var identity = new ClaimsIdentity(token.Claims, "Bearer");
                var user = new ClaimsPrincipal(identity);
                var authState = new AuthenticationState(user);

                NotifyAuthenticationStateChanged(Task.FromResult(authState));

                return authState;
            }

            return new AuthenticationState(new ClaimsPrincipal()); // Empty claims principal means no identity and the user is not logged in
        }
    }
}
