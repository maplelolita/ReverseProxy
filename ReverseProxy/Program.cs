var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 1024;
});

builder.WebHost.ConfigureAppConfiguration(configurationBuilder =>
{
    configurationBuilder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
    configurationBuilder.AddJsonFile("config.json", true, true);
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    builder.Configuration.GetSection("ForwardedHeadersOptions").Bind(options);
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapReverseProxy();

app.Run();