﻿//using Microsoft.AspNetCore.Http;
//using System.Net.Http.Json;


//namespace Infrastructure.Clients;

//public class UserProfileClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
//{
//    private readonly HttpClient _httpClient = httpClient;
//    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

//    public async Task<UserProfileDto?> GetUserProfileAsync(Guid userId)
//    {
//        var accessToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

//        if (!string.IsNullOrEmpty(accessToken))
//        {
//            _httpClient.DefaultRequestHeaders.Authorization =
//                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.Replace("Bearer ", ""));
//        }

//        var response = await _httpClient.GetAsync($"api/userprofiles/{userId}");
//        if (response.IsSuccessStatusCode)
//        {
//            return await response.Content.ReadFromJsonAsync<UserProfileDto>();
//        }

//        return null;
//    }
//}
