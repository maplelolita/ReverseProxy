var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration((context, configurationBuilder) =>
{
    configurationBuilder.AddApollo(context.Configuration.GetSection("apollo"));
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