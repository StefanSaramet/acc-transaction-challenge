using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AccountsTransactionsChallenge.Config;
using Microsoft.Extensions.Options;

namespace AccountsTransactionsChallenge.Services.Remote;

public class TokenService : ITokenService
{
    private readonly Dictionary<string, string> _grantTypesDictionary = new()
    {
        { "grant_type", "client_credentials" }
    };

    private readonly HttpClient _httpClient;
    private readonly IOptions<RemoteAccountProviderConfig> _remoteAccountProviderConfig;

    public TokenService(
        HttpClient httpClient, 
        IOptions<RemoteAccountProviderConfig> remoteAccountProviderConfig)
    {
        _httpClient = httpClient;
        _remoteAccountProviderConfig = remoteAccountProviderConfig;
    }

    public async Task<string> GetToken()
    {
        using var content = new FormUrlEncodedContent(_grantTypesDictionary);
        using var cert = new X509Certificate2(Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
            $"Certs\\{_remoteAccountProviderConfig.Value.RemoteAccountProviderSignCertificate}"));

        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(_remoteAccountProviderConfig.Value.RemoteAccountProviderAuthUrl, UriKind.Relative),
            Method = HttpMethod.Post,
            Content = content
        };

        var currentDate = DateTime.Now.ToUniversalTime().ToString("r");
        string digestValue = await content.DigestValue();

        var signingString =
            @$"(request-target): post {_remoteAccountProviderConfig.Value.RemoteAccountProviderAuthUrl}
date: {currentDate}
digest: {digestValue}";
        string signature = cert.SignData(signingString);

        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Digest", digestValue);
        request.Headers.Add("Date", currentDate);
        request.Headers.Add("TPP-Signature-Certificate", "-----BEGIN CERTIFICATE----- MIIENjCCAx6gAwIBAgIEXkKZvjANBgkqhkiG9w0BAQsFADByMR8wHQYDVQQDDBZB cHBDZXJ0aWZpY2F0ZU1lYW5zQVBJMQwwCgYDVQQLDANJTkcxDDAKBgNVBAoMA0lO RzESMBAGA1UEBwwJQW1zdGVyZGFtMRIwEAYDVQQIDAlBbXN0ZXJkYW0xCzAJBgNV BAYTAk5MMB4XDTIwMDIxMDEyMTAzOFoXDTIzMDIxMTEyMTAzOFowPjEdMBsGA1UE CwwUc2FuZGJveF9laWRhc19xc2VhbGMxHTAbBgNVBGEMFFBTRE5MLVNCWC0xMjM0 NTEyMzQ1MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkJltvbEo4/SF cvtGiRCar7Ah/aP0pY0bsAaCFwdgPikzFj+ij3TYgZLykz40EHODtG5Fz0iZD3fj BRRM/gsFPlUPSntgUEPiBG2VUMKbR6P/KQOzmNKF7zcOly0JVOyWcTTAi0VAl3ME O/nlSfrKVSROzdT4Aw/h2RVy5qlw66jmCTcp5H5kMiz6BGpG+K0dxqBTJP1WTYJh cEj6g0r0SYMnjKxBnztuhX5XylqoVdUy1a1ouMXU8IjWPDjEaM1TcPXczJFhakkA neoAyN6ztrII2xQ5mqmEQXV4BY/iQLT2grLYOvF2hlMg0kdtK3LXoPlbaAUmXCoO 8VCfyWZvqwIDAQABo4IBBjCCAQIwNwYDVR0fBDAwLjAsoCqgKIYmaHR0cHM6Ly93 d3cuaW5nLm5sL3Rlc3QvZWlkYXMvdGVzdC5jcmwwHwYDVR0jBBgwFoAUcEi7XgDA 9Cb4xHTReNLETt+0clkwHQYDVR0OBBYEFLQI1Hig4yPUm6xIygThkbr60X8wMIGG BggrBgEFBQcBAwR6MHgwCgYGBACORgEBDAAwEwYGBACORgEGMAkGBwQAjkYBBgIw VQYGBACBmCcCMEswOTARBgcEAIGYJwEDDAZQU1BfQUkwEQYHBACBmCcBAQwGUFNQ X0FTMBEGBwQAgZgnAQIMBlBTUF9QSQwGWC1XSU5HDAZOTC1YV0cwDQYJKoZIhvcN AQELBQADggEBAEW0Rq1KsLZooH27QfYQYy2MRpttoubtWFIyUV0Fc+RdIjtRyuS6 Zx9j8kbEyEhXDi1CEVVeEfwDtwcw5Y3w6Prm9HULLh4yzgIKMcAsDB0ooNrmDwds YcU/Oju23ym+6rWRcPkZE1on6QSkq8avBfrcxSBKrEbmodnJqUWeUv+oAKKG3W47 U5hpcLSYKXVfBK1J2fnk1jxdE3mWeezoaTkGMQpBBARN0zMQGOTNPHKSsTYbLRCC Gxcbf5oy8nHTfJpW4WO6rK8qcFTDOWzsW0sRxYviZFAJd8rRUCnxkZKQHIxeJXNQ rrNrJrekLH3FbAm/LkyWk4Mw1w0TnQLAq+s= -----END CERTIFICATE-----");
        request.Headers.Add("Authorization",
            $"Signature keyId=\"{_remoteAccountProviderConfig.Value.RemoteAccountProviderKeyId}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\"");

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync();

        return responseContent;
    }
}

public static class TokenServiceExtensions
{
    public static async Task<string> DigestValue(this FormUrlEncodedContent content)
    {
        string payload = await content.ReadAsStringAsync();
        return "SHA-256=" + payload.ComputeSha256HashAsBase64String();
    }

    public static string ComputeSha256HashAsBase64String(this string stringToHash)
    {
        using var hash = SHA256.Create();

        byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
        return Convert.ToBase64String(result);
    }

    public static string SignData(this X509Certificate2 cert, string stringToSign)
    {
        byte[] dataToSign = Encoding.UTF8.GetBytes(stringToSign);
        byte[]? signedData = cert.GetRSAPrivateKey()
            ?.SignData(dataToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        string base64Signature = Convert.ToBase64String(signedData!);
        return base64Signature;
    }
}