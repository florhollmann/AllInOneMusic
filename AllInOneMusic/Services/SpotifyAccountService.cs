using AllInOneMusic.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AllInOneMusic.Services
{
    public class SpotifyAccountService : ISpotifyAccountService
    {
        private readonly HttpClient _httpclient;
        public SpotifyAccountService(HttpClient httpClient)
        {
            _httpclient = httpClient;
        }
        public async Task<string> GetToken(string clientId, string clientSecret)
        {
            // 1 - request is represented by instance of HttpRequestMessage and the first argument is the method that we are using (POST) and the 2dn the URI /uri relative paht - base url declared when registring the service
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");


            //To provide authorization in the header we set a new instance of the authentication header value class 
            //1RST Argument is the type of authentication , the 2nd argument is a string that combines the client id and the client secret encoded in base 64 (pedido x spotify)
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));


            //the request content takes an instance of the Form Url Encoded content Class and it takes as an argument a dictionary of strings
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials" }
            });

            //With this the request object is done 
            //We invoke the request using the async method
            var response = await _httpclient.SendAsync(request);

            //we call the ensure success status on the response object to make sure that our request succeeds, other wise we can raise an exception

            response.EnsureSuccessStatusCode();
            // se agrega un using because el Stream is disposable
            using var responseStream = await response.Content.ReadAsStreamAsync();

            //vamos a deserializar con las propiedades de la clase que contiene el Response (authResult) la propia response
            var authResult = await JsonSerializer.DeserializeAsync<AuthResult>(responseStream);

            //Retornamos el access token
            return authResult.access_token;


        }
    }
}
