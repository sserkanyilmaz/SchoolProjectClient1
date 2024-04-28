﻿using System;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SchoolProjectClient.Client.Model.Common;
using SchoolProjectClient.Client.Model.User;

namespace SchoolProjectClient.Client.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpClientService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<TResponse> DeleteAsync<TResponse>(RequestParameter requestParameter, string id)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(Url(requestParameter));
            urlBuilder.Append(!String.IsNullOrEmpty(id) ? "" + id : "");
            HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync(urlBuilder.ToString());
            return await httpResponseMessage.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse> GetAsync<TResponse>(RequestParameter requestParameter, string id = null)
        {
            JsonSerializerOptions options = new();
            options.PropertyNameCaseInsensitive = true;
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(Url(requestParameter));
            urlBuilder.Append(!String.IsNullOrEmpty(id) ? id : "");
            return await _httpClient.GetFromJsonAsync<TResponse>(urlBuilder.ToString(), options);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(RequestParameter requestParameter, TRequest body)
        {
            string url = Url(requestParameter);
            HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync<TRequest>(url, body);
            return await httpResponseMessage.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(RequestParameter requestParameter, TRequest body)
        {
            JsonSerializerOptions jsonSerializerOptions = new();
            jsonSerializerOptions.PropertyNameCaseInsensitive = true;
            string url = Url(requestParameter);
            HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync<TRequest>(url, body, jsonSerializerOptions);
            return await httpResponseMessage.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<BaseResponse<AuthenticationResponse>> LoginUserAsync(LoginRequest request)
        {
            var url = _configuration["BaseUrl"] + "/api/Auth/Login"; 

            var response = await _httpClient.PostAsJsonAsync(url, request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BaseResponse<AuthenticationResponse>>();
            }
            else
            {
                return new BaseResponse<AuthenticationResponse>() { IsSucceeded = false };
            }
        }



        private string Url(RequestParameter requestParameter)
        {
            StringBuilder urlBuilder = new StringBuilder();
            //urlBuilder.Append(!String.IsNullOrEmpty(requestParameter.BaseUrl) ? requestParameter.BaseUrl : _configuration["BaseUrl"]);
            urlBuilder.Append("https://localhost:7154/api/");
            urlBuilder.Append(requestParameter.Controller + "/");
            urlBuilder.Append(!String.IsNullOrEmpty(requestParameter.Action) ? requestParameter.Action : "");
            urlBuilder.Append((!String.IsNullOrEmpty(requestParameter.QueryString) ? "?" + requestParameter.QueryString : "/"));
            return !String.IsNullOrEmpty(requestParameter.FullEndPoint) ? requestParameter.FullEndPoint : urlBuilder.ToString();

        }


    }
}

