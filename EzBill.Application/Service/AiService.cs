using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using EzBill.Application.IService;
using Microsoft.Extensions.Configuration;

namespace Ezbill.Infrastructure.Services
{
    public class AiService : IAiService
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly string _systemPrompt;
        private readonly HttpClient _httpClient;

        public AiService(IConfiguration config)
        {
            _apiKey = config["OpenAI:ApiKey"] ?? throw new ArgumentNullException("OpenAI API key not found");
            _model = config["OpenAI:Model"] ?? "gpt-4o-mini";
            _systemPrompt = config["OpenAI:SystemPrompt"] ?? "Bạn là trợ lý Ezbill AI.";
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.openai.com/v1/")
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> GetEzbillResponseAsync(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                return "Vui lòng nhập nội dung để tôi có thể hỗ trợ.";

            var payload = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = _systemPrompt },
                    new { role = "user", content = userMessage }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                return $"Lỗi khi gọi OpenAI API:\n```\n{errorText}\n```";
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var reply = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return reply?.Trim() ?? "_(Không có phản hồi từ AI)_";
        }
    }

}
