using DotNetDemo.Configs;
using DotNetDemo.Services;
using Serilog;
using Sharpbrake.Client;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;
var environmentName = environment.EnvironmentName.ToLower();
var secretsPath = $"{environmentName}/ttp/config";
var appName = AppDomain.CurrentDomain.FriendlyName;

// setup a bootstrap logger to capture output until the real logger is configured
// TODO: need to see where startup issues can be logged in a way that DevOps can see
//       them when an instance fails during startup.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

// add configuration sources in the order of precedence with higher precedence last
builder.Configuration
    .AddSecretsManager(configurator: options =>
    {
        options.KeyGenerator = (entry, key) => key.Replace(secretsPath, appName);
        options.PollingInterval = TimeSpan.FromMinutes(15);
        options.SecretFilter = entry => entry.Name.StartsWith(secretsPath);
    })
    .AddJsonFile($"appSettings.{environmentName}.json", true, true);

builder.Services
    .Configure<DemoConfig>(builder.Configuration.GetSection(appName));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services
    .AddTransient<IWeatherService, WeatherService>();

// enable logging as defined in the configuration
builder.Host.UseSerilog((context, serilogConfig) =>
    serilogConfig
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Airbrake(context.Configuration.GetSection(appName).Get<AirbrakeConfig>())
        // TODO: add Splunk, set log level filtering, see how to send json params to Airbrake, enrich Airbrake with trace id
);

var app = builder.Build();

// configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // handy UI for testing endpoints in development
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // redirect all errors to a safe handler when not in development
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
