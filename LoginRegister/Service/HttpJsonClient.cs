using InfoManager.Interface;
using InfoManager.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using InfoManager.Helpers;
using Microsoft.Extensions.DependencyInjection;


namespace InfoManager.Services
{
    internal class HttpJsonService<T> : IHttpJsonProvider<T> where T : class
    {

        LoginDTO loginDTO = App.Current.Services.GetService<LoginDTO>();

        public async Task<IEnumerable<T?>> GetAsync(string path)
        {
            try
            {
                var loginDTO = App.Current.Services.GetService<LoginDTO>(); 
                using HttpClient httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {loginDTO.Token}");
                HttpResponseMessage request = await httpClient.GetAsync($"{Constants.BASE_URL}{path}");

                if (request.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await Authenticate(path, httpClient, request);
                    request = await httpClient.GetAsync($"{Constants.BASE_URL}{path}");
                }

                string dataRequest = await request.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<T>>(dataRequest, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }

        public async Task Authenticate(string path, HttpClient httpClient, HttpResponseMessage request)
        {
            var loginDTO = App.Current.Services.GetService<LoginDTO>();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            HttpContent httpContent = new StringContent(JsonSerializer.Serialize(loginDTO, options), Encoding.UTF8, "application/json");

            HttpResponseMessage requestToken = await httpClient.PostAsync($"{Constants.BASE_URL}{Constants.LOGIN_PATH}", httpContent);
            string dataTokenRequest = await requestToken.Content.ReadAsStringAsync();

            UserDTO tokenUser = JsonSerializer.Deserialize<UserDTO>(dataTokenRequest, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (tokenUser != null)
            {
                loginDTO.Token = tokenUser.Token; 
                httpClient.DefaultRequestHeaders.Remove("Authorization");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {loginDTO.Token}");
            }
        }

        public async Task<bool> PostAsync(string path, T data)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {loginDTO.Token}");

                    string jsonContent = JsonSerializer.Serialize(data);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync($"{Constants.BASE_URL}{path}", content);

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await Authenticate(path, httpClient, response);
                        response = await httpClient.PostAsync($"{Constants.BASE_URL}{path}", content);
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        if (responseBody.Trim().ToLower() == "false")
                            return false;

                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Error en la respuesta: {response.StatusCode}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la solicitud POST: {ex.Message}");
                return false;
            }
        }


        public async Task<T?> LoginPostAsync(string path, LoginDTO data)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                    string jsonContent = JsonSerializer.Serialize(data, options);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync($"{Constants.BASE_URL}{path}", content);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error de conexión: {ex.Message}");
            }
        }

        public async Task<T?> RegisterPostAsync(string path, UserRegistroDTO data)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                    string jsonContent = JsonSerializer.Serialize(data, options);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync($"{Constants.BASE_URL}{path}", content);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en registro: {ex.Message}");
            }
            return default;
        }



        public async Task<T?> PutAsync(string path, T data)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {loginDTO.Token}");

                    string jsonContent = JsonSerializer.Serialize(data,
                     new JsonSerializerOptions
                     {
                         DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                         WriteIndented = true  
                     });

                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage request = await httpClient.PutAsync($"{Constants.BASE_URL}{path}", content);

                    if (request.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await Authenticate(path, httpClient, request);
                        request = await httpClient.PutAsync($"{Constants.BASE_URL}{path}", content);

                        if (request.IsSuccessStatusCode)
                        {
                            string responseBody = await request.Content.ReadAsStringAsync();
                            return JsonSerializer.Deserialize<T?>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        }
                        else
                        {
                            Console.WriteLine("Error en la respuesta: " + request.StatusCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la solicitud PATCH: {ex.Message}");
            }
            return default;
        }

        public async Task<bool> DeleteAsync(string path)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {loginDTO.Token}");

                HttpResponseMessage response = await httpClient.DeleteAsync($"{Constants.BASE_URL}{path}");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await Authenticate(path, httpClient, response);
                    response = await httpClient.DeleteAsync($"{Constants.BASE_URL}{path}");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la solicitud DELETE: {ex.Message}");
                return false;
            }
        }

    }
}




