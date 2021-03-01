namespace Pry_TitulacionMovil.Services
{
    //using Helpers;
    using Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class ApiService
    {
        #region Connections
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Falló la Conexión"
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                "google.com");

            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No tiene Conección a Internet"
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Conexión Exitosa"
            };
        }
        #endregion

        #region Methods
        public async Task<User> GetLogin(
            string urlBase,
            string api,
            string controller,
            string loginname,
            string password)
        {
            try
            {
                var client = new HttpClient();
                //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format(
                    "{0}/{1}/{2}/{3}",
                    api,
                    controller,
                    loginname,
                    password);
                var response = await client.GetAsync(url);
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<User>(resultJSON);
                return result;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error de Login",
                    ex.ToString(),
                    "Aceptar");
                return null;
            }
        }

        //public async Task<Response> GetListByOperario<T>(
        //    string urlBase,
        //    string api,
        //    string controller,
        //    int operario,
        //    int emprcodigo)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        client.BaseAddress = new Uri(urlBase);
        //        var url = string.Format("{0}/{1}/{2}/{3}",
        //            api,
        //            controller,
        //            operario,
        //            emprcodigo);
        //        var response = await client.GetAsync(url);
        //        var result = await response.Content.ReadAsStringAsync();

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return new Response
        //            {
        //                IsSuccess = false,
        //                Message = result,
        //            };
        //        }

        //        var list = JsonConvert.DeserializeObject<List<T>>(result);
        //        return new Response
        //        {
        //            IsSuccess = true,
        //            Message = Languages.Accept,
        //            Result = list,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response
        //        {
        //            IsSuccess = false,
        //            Message = ex.Message,
        //        };
        //    }
        //}

        //public async Task<Response> GetEnterpriseParameter<T>(
        //    string urlBase,
        //    string api,
        //    string controller,
        //    string parameter,
        //    int emprcodigo)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        client.BaseAddress = new Uri(urlBase);
        //        var url = string.Format("{0}/{1}/{2}/{3}",
        //            api,
        //            controller,
        //            parameter,
        //            emprcodigo);

        //        var response = await client.GetAsync(url);
        //        var result = await response.Content.ReadAsStringAsync();

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return new Response
        //            {
        //                IsSuccess = false,
        //                Message = result,
        //            };
        //        }

        //        var list = JsonConvert.DeserializeObject<List<T>>(result);
        //        return new Response
        //        {
        //            IsSuccess = true,
        //            Message = Languages.Accept,
        //            Result = list,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response
        //        {
        //            IsSuccess = false,
        //            Message = ex.Message,
        //        };
        //    }
        //}

        //public async Task<Response> GetWorkList<T>(
        //    string urlBase,
        //    string api,
        //    string controller,
        //    int emprcodigo)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        client.BaseAddress = new Uri(urlBase);
        //        var url = string.Format("{0}/{1}/{2}",
        //            api,
        //            controller,
        //            emprcodigo);

        //        var response = await client.GetAsync(url);
        //        var result = await response.Content.ReadAsStringAsync();

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return new Response
        //            {
        //                IsSuccess = false,
        //                Message = result,
        //            };
        //        }

        //        var list = JsonConvert.DeserializeObject<List<T>>(result);
        //        return new Response
        //        {
        //            IsSuccess = true,
        //            Message = Languages.Accept,
        //            Result = list,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response
        //        {
        //            IsSuccess = false,
        //            Message = ex.Message,
        //        };
        //    }
        //}

        //public async Task<Response> Put<T>(
        //    string urlBase,
        //    string servicePrefix,
        //    string controller,
        //    T model)
        //{
        //    try
        //    {
        //        var request = JsonConvert.SerializeObject(model);
        //        var content = new StringContent(
        //            request,
        //            Encoding.UTF8, "application/json");
        //        var client = new HttpClient();
        //        client.BaseAddress = new Uri(urlBase);
        //        var url = string.Format(
        //            "{0}{1}/{2}",
        //            servicePrefix,
        //            controller,
        //            model.GetHashCode());
        //        var response = await client.PutAsync(url, content);
        //        var result = await response.Content.ReadAsStringAsync();

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            var error = JsonConvert.DeserializeObject<Response>(result);
        //            error.IsSuccess = false;
        //            return error;
        //        }

        //        var newRecord = JsonConvert.DeserializeObject<T>(result);

        //        return new Response
        //        {
        //            IsSuccess = true,
        //            Result = newRecord,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response
        //        {
        //            IsSuccess = false,
        //            Message = ex.Message,
        //        };
        //    }
        //}

        //public async Task<Response> ChangePassword(
        //    string urlBase,
        //    string servicePrefix,
        //    string controller,
        //    ChangePasswordRequest changePasswordRequest)
        //{
        //    try
        //    {
        //        var request = JsonConvert.SerializeObject(changePasswordRequest);
        //        var content = new StringContent(request, Encoding.UTF8, "application/json");
        //        var client = new HttpClient();
        //        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
        //        client.BaseAddress = new Uri(urlBase);
        //        var url = string.Format("{0}{1}", servicePrefix, controller);
        //        var response = await client.PostAsync(url, content);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return new Response
        //            {
        //                IsSuccess = false,
        //            };
        //        }

        //        return new Response
        //        {
        //            IsSuccess = true,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response
        //        {
        //            IsSuccess = false,
        //            Message = ex.Message,
        //        };
        //    }
        //}

        //public async Task<User> GetUser(
        //    string urlBase,
        //    string servicePrefix,
        //    string controller,
        //    string login)
        //{
        //    try
        //    {
        //        var model = new UserRequest
        //        {
        //            Login = login
        //        };

        //        var request = JsonConvert.SerializeObject(model);
        //        var content = new StringContent(
        //            request,
        //            Encoding.UTF8,
        //            "application/json");
        //        var client = new HttpClient();
        //        client.BaseAddress = new Uri(urlBase);
        //        var url = string.Format("{0}{1}", servicePrefix, controller);
        //        var response = await client.PostAsync(url, content);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return null;
        //        }

        //        var result = await response.Content.ReadAsStringAsync();
        //        return JsonConvert.DeserializeObject<User>(result);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public async Task<Response> Post<T>(
        //    string urlBase,
        //    string servicePrefix,
        //    string controller,
        //    T model)
        //{
        //    try
        //    {
        //        var request = JsonConvert.SerializeObject(model);
        //        var content = new StringContent(
        //            request,
        //            Encoding.UTF8,
        //            "application/json");
        //        var client = new HttpClient();
        //        client.BaseAddress = new Uri(urlBase);
        //        var url = string.Format("{0}{1}", servicePrefix, controller);
        //        var response = await client.PostAsync(url, content);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return new Response
        //            {
        //                IsSuccess = false,
        //                Message = response.StatusCode.ToString()
        //            };
        //        }

        //        var result = await response.Content.ReadAsStringAsync();
        //        //var newRecord = JsonConvert.DeserializeObject<T>(result);

        //        return new Response
        //        {
        //            IsSuccess = true,
        //            Message = Languages.MessageResponse,
        //            Result = result,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response
        //        {
        //            IsSuccess = false,
        //            Message = ex.Message,
        //        };
        //    }
        //}
        #endregion
    }
}
