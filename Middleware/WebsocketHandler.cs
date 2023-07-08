using Microsoft.AspNetCore.Http;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MeuAppWebSocket.Middleware
{
    public class WebSocketHandler
    {
        private readonly RequestDelegate _next;

        public WebSocketHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                await HandleWebSocketConnection(webSocket);
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private async Task HandleWebSocketConnection(WebSocket webSocket)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var buffer = new byte[1024];
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var responseBytes = Encoding.UTF8.GetBytes("Mensagem recebida com sucesso!");
                    await webSocket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing WebSocket connection", CancellationToken.None);
                }
            }
        }
    }
}
