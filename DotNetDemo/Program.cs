using DotNetDemo.Configs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;
var environmentName = environment.EnvironmentName.ToLower();

// setup a bootstrap logger to capture output until the real logger is configured
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

// make our own configuration provider so that we can choose the precedence of sources
var config = new ConfigurationBuilder()
    .SetBasePath(environment.ContentRootPath)
    .AddJsonFile("appSettings.json", true, true)
    .AddSecretsManager(configurator: cfg =>
    {
        cfg.SecretFilter = item => item.Name == ($"{environmentName}/quad-worldpay/config");
        // TODO: tweak the key name to match that in the appSettings.development.json file
        cfg.KeyGenerator = (secret, name) => "Settings";
        cfg.PollingInterval = TimeSpan.FromMinutes(15);
    })
    .AddEnvironmentVariables()
    .AddJsonFile($"appSettings.{environmentName}.json", true, true)
    .Build();

// Add services to the container.
builder.Services.Configure<DemoConfig>(config.GetSection("Demo"));
// TODO: load app config as a section from the secrets
// TODO: verify the precedence of the sources

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// enable logging as defined in the configuration
builder.Host.UseSerilog(
    (context, serilogConfig) => serilogConfig.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
