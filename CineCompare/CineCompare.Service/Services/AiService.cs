using CineCompare.Core.Entities;
using CineCompare.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CineCompare.Service.Services
{
    public class AiService : IAiService
    {
        private readonly IConfiguration _configuration;
        private readonly IGenericRepository<SystemLog> _logRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AiService(IConfiguration configuration, IGenericRepository<SystemLog> logRepository, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GetMovieRecommendationAsync(string userPrompt, string userId)
        {
            try
            {
                // 1. appsettings.json'dan API Key'i alıyoruz ve Google Gemini URL'sini hazırlıyoruz
                var apiKey = _configuration["Gemini:ApiKey"];
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

                // 2. Sine-Bot'un karakterini (Persona) belirliyoruz
                string systemPrompt = "Sen 'Sine-Bot' adında, sadece sinema ve film önerileri üzerine uzmanlaşmış bir asistansın. Kullanıcıya sadece filmlerle ilgili cevap ver. Eğer sinema dışı bir şey sorulursa kibarca reddet. Kısa ve net cevaplar ver.";
                string fullPrompt = systemPrompt + "\n\nKullanıcının Sorusu: " + userPrompt;

                // 3. Google'ın istediği formatta bir JSON verisi oluşturuyoruz
                var requestBody = new
                {
                    contents = new[]
                    {
                        new { parts = new[] { new { text = fullPrompt } } }
                    }
                };

                var jsonString = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                // 4. İsteği atıyoruz (İşte hocanın bayılacağı kısım burası!)
                using var client = new HttpClient();
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Google API Hatası: {response.StatusCode}");
                }

                // 5. Gelen karmaşık JSON cevabının içinden sadece bize lazım olan metni (Cevabı) cımbızla çekiyoruz
                var responseString = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(responseString);
                var aiAnswer = jsonDoc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                // 6. HOCANIN ŞARTI: Loglama (Veritabanına kayıt)
                var logEntry = new SystemLog
                {
                    LogType = "AiChat",
                    Message = $"Soru: {userPrompt} | Cevap: {aiAnswer}",
                    CreatedAt = DateTime.Now,
                    UserId = userId
                };

                await _logRepository.AddAsync(logEntry);
                await _unitOfWork.CommitAsync();

                return aiAnswer;
            }
            catch (Exception ex)
            {
                // Hata olursa yine logluyoruz
                var errorLog = new SystemLog
                {
                    LogType = "Error_AiChat",
                    Message = $"Yapay zeka hatası: {ex.Message}",
                    CreatedAt = DateTime.Now,
                    UserId = userId
                };
                await _logRepository.AddAsync(errorLog);
                await _unitOfWork.CommitAsync();

                return "Sine-Bot şu an sinemada mısır yiyor, lütfen daha sonra tekrar deneyin.";
            }
        }
    }
}