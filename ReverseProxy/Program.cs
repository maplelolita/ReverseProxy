var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 1024;
});

builder.Configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);

var useHttps = builder.Configuration.GetValue<bool>("UseHttps");
var useApollo = builder.Configuration.GetValue<bool>("UseApollo");

if (useApollo)
{
    builder.Configuration.AddJsonFile("appolo.json", true, true);
    builder.Configuration.AddApollo(builder.Configuration.GetSection("apollo"));
}
else //use local config file
{
    builder.Configuration.AddJsonFile("config.json", true, true);
}

if(useHttps)
{
    builder.Configuration.AddJsonFile("https.json", true, true);
}

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    builder.Configuration.GetSection("ForwardedHeadersOptions").Bind(options);
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseForwardedHeaders();

if (useHttps)
{
    app.UseHsts();

    app.UseHttpsRedirection();
}

app.MapReverseProxy();

app.Run();