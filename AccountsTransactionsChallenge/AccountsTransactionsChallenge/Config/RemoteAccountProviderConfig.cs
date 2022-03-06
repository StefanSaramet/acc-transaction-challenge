namespace AccountsTransactionsChallenge.Config;

public class RemoteAccountProviderConfig
{
    public bool UseRemoteAccountProvider { get; set; }
    public string RemoteAccountProviderBaseUrl { get; set; } = String.Empty;
    public string RemoteAccountProviderSignCertificate { get; set; } = String.Empty;
    public string RemoteAccountProviderTlsCertificate { get; set; } = String.Empty;
    public string RemoteAccountProviderAuthUrl { get; set; } = String.Empty;
    public string RemoteAccountProviderAccountUrl { get; set; } = String.Empty;
    public string RemoteAccountProviderKeyId { get; set; } = String.Empty;
}