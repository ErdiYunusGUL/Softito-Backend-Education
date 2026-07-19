using CineCompare.Core.DTOs;
using CineCompare.Core.Entities;
using CineCompare.Core.Interfaces;
using CineCompare.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace CineCompare.Service.Services
{
    public class MovieService : IMovieService
    {
        private readonly IGenericRepository<Movie> _movieRepository;
        private readonly IDapperRepository _dapperRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        // Dependency Injection ile gerekli araçları içeri alıyoruz
        public MovieService(IGenericRepository<Movie> movieRepository,
                            IDapperRepository dapperRepository,
                            IUnitOfWork unitOfWork,
                            IMemoryCache cache)
        {
            _movieRepository = movieRepository;
            _dapperRepository = dapperRepository;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        // --- 1. YAZMA İŞLEMİ (EF Core & Unit Of Work) ---
        public async Task AddMovieAsync(Movie movie)
        {
            await _movieRepository.AddAsync(movie);
            await _unitOfWork.CommitAsync(); // Hata olmazsa veritabanına kalıcı olarak yaz
        }




        // ... (Constructor ve EF Core kısımları aynı kalıyor)

        public async Task<IEnumerable<ShowtimePricingDto>> GetShowtimePricesAsync(int movieId, string city, DateTime selectedDate)
        {
            // Cache anahtarını oluşturuyoruz
            string cacheKey = $"ShowtimePrices_{movieId}_{city}_{selectedDate:yyyyMMdd}";

            // Eğer cache'de varsa oradan dön
            if (_cache.TryGetValue(cacheKey, out IEnumerable<ShowtimePricingDto> cachedPrices))
            {
                return cachedPrices;
            }

            // Kullanıcı filmi, şehri ve tarihi (Örn: Bugün) seçtiğinde bu sorgu çalışır.
            // CASE WHEN ile TicketCategory kolonundaki "Student" ve "Adult" verilerini ayrı kolonlara bölüyoruz.
            string sqlQuery = @"
                SELECT 
                    t.Name AS TheaterName, 
                    h.HardwareType AS HardwareType, 
                    s.StartTime AS StartTime, 
                    MIN(CASE WHEN tp.TicketCategory = 'Student' THEN tp.Price END) AS StudentPrice,
                    MIN(CASE WHEN tp.TicketCategory = 'Adult' THEN tp.Price END) AS AdultPrice
                FROM Showtimes s
                INNER JOIN Halls h ON s.HallId = h.Id
                INNER JOIN Theaters t ON h.TheaterId = t.Id
                LEFT JOIN TicketPrices tp ON s.Id = tp.ShowtimeId
                WHERE s.MovieId = @MovieId 
                  AND t.City = @City 
                  AND CAST(s.StartTime AS DATE) = CAST(@SelectedDate AS DATE)
                GROUP BY t.Name, h.HardwareType, s.StartTime
                ORDER BY t.Name, s.StartTime";

            var parameters = new
            {
                MovieId = movieId,
                City = city,
                SelectedDate = selectedDate
            };

            var prices = await _dapperRepository.QueryAsync<ShowtimePricingDto>(sqlQuery, parameters);

            // Veriyi cache'e 10 dakikalığına ekle
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            _cache.Set(cacheKey, prices, cacheOptions);

            return prices;
        }
    }
}