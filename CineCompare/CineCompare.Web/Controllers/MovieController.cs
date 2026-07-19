using CineCompare.Core.Entities;
using CineCompare.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CineCompare.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IGenericRepository<Movie> _movieRepository;
        private readonly IMovieService _movieService;
        private readonly IUnitOfWork _unitOfWork;

        public MovieController(IGenericRepository<Movie> movieRepository, IMovieService movieService, IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _movieService = movieService;
            _unitOfWork = unitOfWork;
        }

        // GET: api/movie
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieRepository.GetAllAsync();
            // Film ID'sine göre ters sıralama yapalım ki son eklenen en üstte çıksın
            return Ok(movies.OrderByDescending(m => m.Id));
        }

        // GET: api/movie/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null) return NotFound();

            var showtimes = await _movieService.GetShowtimePricesAsync(id, "İstanbul", DateTime.Today);
            var groupedShowtimes = showtimes.GroupBy(s => s.TheaterName).Select(g => new {
                TheaterName = g.Key,
                Showtimes = g.ToList()
            }).ToList();

            var result = new
            {
                Movie = movie,
                Showtimes = groupedShowtimes
            };

            return Ok(result);
        }

        public class MovieDto
        {
            public string Title { get; set; }
            public string Genre { get; set; }
            public int DurationMinutes { get; set; }
            public string PosterUrl { get; set; }
        }

        // POST: api/movie
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MovieDto movieDto)
        {
            if (movieDto == null || string.IsNullOrWhiteSpace(movieDto.Title))
                return BadRequest("Geçersiz film bilgisi.");

            var newMovie = new Movie
            {
                Title = movieDto.Title,
                Genre = movieDto.Genre ?? "Bilinmiyor",
                DurationInMinutes = movieDto.DurationMinutes > 0 ? movieDto.DurationMinutes : 120,
                PosterUrl = movieDto.PosterUrl ?? "https://images.unsplash.com/photo-1485846234645-a62644f84728?auto=format&fit=crop&q=80&w=1200",
                ReleaseDate = DateTime.UtcNow,
                DirectorId = 1 // Şimdilik varsayılan yönetmen
            };

            await _movieRepository.AddAsync(newMovie);
            await _unitOfWork.CommitAsync();

            return Ok(newMovie);
        }

        // DELETE: api/movie/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null) return NotFound();

            _movieRepository.Remove(movie);
            await _unitOfWork.CommitAsync();

            return Ok(new { message = "Film başarıyla silindi." });
        }
    }
}