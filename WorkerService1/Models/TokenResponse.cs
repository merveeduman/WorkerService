using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WorkerService1.Models
{
    public class TokenResponse
    {
        [JsonProperty("token")] public string Token { get; set; }
        [JsonProperty("access_token")] public string AccessTokenSnake { get; set; }
        [JsonProperty("accessToken")] public string AccessTokenCamel { get; set; }
        [JsonProperty("jwt")] public string Jwt { get; set; }
        [JsonProperty("bearerToken")] public string BearerToken { get; set; }
        public TokenData data { get; set; } // eğer data.token ise
    }
    public class TokenData { public string token { get; set; } }



}
