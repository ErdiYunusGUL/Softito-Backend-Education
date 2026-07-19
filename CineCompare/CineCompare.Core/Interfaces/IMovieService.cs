using CineCompare.Core.DTOs;
using CineCompare.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CineCompare.Core.Interfaces
{
    public interface IMovieService
    {
        // EF Core ile yapılacak işlem (Ekleme)
        Task AddMovieAsync(Movie movie);

        // Sınıfta (MovieService) değiştirdiğimiz yeni Dapper metodumuzun imzasını buraya koyuyoruz
        Task<IEnumerable<ShowtimePricingDto>> GetShowtimePricesAsync(int movieId, string city, DateTime selectedDate);
    }
}