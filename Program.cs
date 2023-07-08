using MeuAppWebSocket.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

webSocketOptions.AllowedOrigins.Add("https://localhost:5001");

app.UseWebSockets(webSocketOptions);
app.UseMiddleware<WebSocketHandler>();
app.Run();
