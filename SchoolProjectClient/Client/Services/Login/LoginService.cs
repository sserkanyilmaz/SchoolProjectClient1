﻿using System;
using SchoolProjectClient.Client.Model.Common;
using SchoolProjectClient.Client.Model.Login;

namespace SchoolProjectClient.Client.Services.Login
{
    public class LoginService : ILoginService
    {
        public AppUserId AppUserId { get; set; }

        public AppUserId User => new AppUserId();

        public readonly IHttpClientService _httpClientService;

        public LoginService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;

        }
        public async Task<BaseResponse<AuthenticationResponse>> LoginUserAsync(LoginRequest loginRequest)
            => await _httpClientService.LoginAsync<LoginRequest, BaseResponse<AuthenticationResponse>>(new RequestParameter() { Controller = "Auth", Action = "Login" }, loginRequest);


        public Task Logout()
        {
            throw new NotImplementedException();
        }
        //=> await _httpClientService.LoginUserAsync<BaseResponse<AuthenticationResponse>>(new RequestParameter() { Controller = "Auth", Action = "Login" }, loginRequest);
    }
}