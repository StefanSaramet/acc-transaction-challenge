using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using AccountsTransactionsChallenge.Config;
using AccountsTransactionsChallenge.Data;
using AccountsTransactionsChallenge.Data.Accounts;
using AccountsTransactionsChallenge.Data.Transactions;
using AccountsTransactionsChallenge.Services;
using AccountsTransactionsChallenge.Services.Database;
using AccountsTransactionsChallenge.Services.Remote;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<DbOptions>(builder.Configuration.GetSection("LiteDbOptions"));
builder.Services.Configure<RemoteAccountProviderConfig>(builder.Configuration.GetSection("RemoteAccount"));

builder.Services.AddSingleton<AppDbContext>();

builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

if (!builder.Configuration.GetValue<bool>("UseRemoteAccountProvider"))
{
    builder.Services.AddScoped<IAccountService, StorageAccountService>();
}
else
{
    builder.Services.AddHttpClient<IAccountService, StorageAccountService>();
    builder.Services.AddHttpClient<ITokenService, TokenService>(client =>
    {
        client.BaseAddress = new(builder.Configuration.GetValue<string>("RemoteAccount:RemoteAccountProviderBaseUrl"));
    }).ConfigurePrimaryHttpMessageHandler(_ =>
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(
            new X509Certificate2(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                $"Certs\\{builder.Configuration.GetValue<string>("RemoteAccount:RemoteAccountProviderTlsCertificate")}")));
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11;

        return handler;
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
