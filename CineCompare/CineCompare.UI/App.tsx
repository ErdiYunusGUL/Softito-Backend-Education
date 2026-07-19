import React, { useState, useMemo, useEffect } from 'react';
import { Movie, TheaterWithDetails, LocationState, SortOption, SocialPlan, User } from './types';
import { MOCK_MOVIES, MOCK_UPCOMING_MOVIES, MOCK_THEATERS, MOCK_PRICING, MOCK_SHOWTIMES, MOCK_ACTIVITIES, MOCK_USERS, MOCK_BADGES, MOCK_REVIEWS } from './constants';
import { calculateDistance } from './utils/geo';
import { MapPin, Filter, ArrowLeft, Search, Navigation, Users, Film, LogIn, Sparkles, Trophy, Handshake, Bell, MessageSquare, Projector, Music, Play, ExternalLink, Info, Ticket, Calendar, Clock, Star, Hourglass, Clapperboard } from 'lucide-react';
import TheaterCard from './components/TheaterCard';
import AISummary from './components/AISummary';
import SocialModal from './components/SocialModal';
import ActivityFeed from './components/ActivityFeed';
import StatsCard from './components/StatsCard'; // Module 11
import ReviewList from './components/ReviewList'; // Module 12
import UserProfile from './components/UserProfile'; // New Component
import CreditSceneInfo from './components/CreditSceneInfo'; // Module 14
import AdminPanel from './components/AdminPanel'; // Admin Paneli
import { AuthProvider, useAuth } from './contexts/AuthContext';
import { gameEngine, calculateXPGain } from './utils/gamification';
import { findMatch } from './utils/matchmaker';
import { semanticSearchMovies } from './services/geminiService';

// Main content component wrapped to use Auth Hook
const MainApp = () => {
  const { user, login } = useAuth();

  // Navigation State
  const [view, setView] = useState<'list' | 'detail' | 'social' | 'admin'>('list');
  const [selectedMovie, setSelectedMovie] = useState<Movie | null>(null);

  // Backend Data State
  const [movies, setMovies] = useState<Movie[]>(MOCK_MOVIES);

  useEffect(() => {
    fetch("https://localhost:7066/api/movie")
      .then(res => res.json())
      .then(data => {
        const apiMovies: Movie[] = data.map((m: any) => ({
          id: m.id.toString(),
          title: m.title,
          description: m.title + " için detaylar yükleniyor...",
          posterUrl: m.posterUrl || "https://images.unsplash.com/photo-1485846234645-a62644f84728?auto=format&fit=crop&q=80&w=1200",
          backdropUrl: m.posterUrl || "https://images.unsplash.com/photo-1485846234645-a62644f84728?auto=format&fit=crop&q=80&w=1200",
          genre: m.genre ? m.genre.split(',') : ["Sinema"],
          durationMinutes: m.durationInMinutes || 120,
          rating: 8.5,
          releaseDate: m.releaseDate || new Date().toISOString()
        }));
        if (apiMovies.length > 0) {
          setMovies(apiMovies);
        }
      })
      .catch(err => console.error("API Error:", err));
  }, []);

  // Filter & Search State
  const [sortBy, setSortBy] = useState<SortOption>('default');
  const [selectedFilters, setSelectedFilters] = useState<string[]>([]);
  const [isFilterOpen, setIsFilterOpen] = useState(false);
  const [searchQuery, setSearchQuery] = useState('');
  const [isAISearching, setIsAISearching] = useState(false);
  const [filteredMovieIds, setFilteredMovieIds] = useState<string[] | null>(null); 

  // Social Plan State
  const [activePlan, setActivePlan] = useState<SocialPlan | null>(null);
  
  // Gamification & Notification State
  const [xpNotification, setXpNotification] = useState<{amount: number, message: string} | null>(null);
  const [alertNotification, setAlertNotification] = useState<string | null>(null); // Module 13

  // Matchmaking State
  const [matchResult, setMatchResult] = useState<{score: number, theater: string, genres: string[], friend: string} | null>(null);

  // Geolocation State
  const [userLocation, setUserLocation] = useState<LocationState>({
    lat: null,
    lng: null,
    error: null,
  });

  // --- MODULE 8: Gamification Listener ---
  useEffect(() => {
    const unsubscribe = gameEngine.subscribe((event) => {
      const xp = calculateXPGain(event.type);
      if (xp > 0) {
        setXpNotification({ amount: xp, message: event.type === 'TICKET_PURCHASED' ? 'Bilet Alındı!' : 'Aktivite!' });
        setTimeout(() => setXpNotification(null), 3000);
      }
    });
    return unsubscribe;
  }, []);

  useEffect(() => {
    if ('geolocation' in navigator) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          setUserLocation({
            lat: position.coords.latitude,
            lng: position.coords.longitude,
            error: null,
          });
          setSortBy('nearest');
        },
        (err) => {
          console.warn("Geolocation error:", err.message);
          setUserLocation({ lat: null, lng: null, error: "Konum alınamadı" });
        }
      );
    }
  }, []);

  const handleMovieSelect = (movie: Movie) => {
    setSelectedMovie(movie);
    setView('detail');
    setSelectedFilters([]);
    if (userLocation.lat) setSortBy('nearest');
    // Scroll to top
    window.scrollTo(0, 0);
  };

  const goBack = () => {
    setView('list');
    setSelectedMovie(null);
  };

  // --- MODULE 10: Semantic Search Handler ---
  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!searchQuery.trim()) {
      setFilteredMovieIds(null);
      return;
    }

    setIsAISearching(true);
    const ids = await semanticSearchMovies(searchQuery, [...movies, ...MOCK_UPCOMING_MOVIES]);
    setFilteredMovieIds(ids);
    setIsAISearching(false);
  };

  // --- MODULE 13: Notification Logic (Simulated) ---
  const handleSubscribeAlert = () => {
    if (!user) {
      alert("Bildirim almak için giriş yapmalısınız.");
      return;
    }
    // Simulate API call to Add to Queue
    setAlertNotification("Haber vereceğiz! 🔔");
    setTimeout(() => setAlertNotification(null), 3000);

    // Simulate Worker picking up a job later (Demo purpose)
    setTimeout(() => {
      if (Math.random() > 0.5) { // 50% chance to simulate a delayed notification
        setAlertNotification(`Fırsat: ${selectedMovie?.title} için biletler %20 indirimde!`);
        setTimeout(() => setAlertNotification(null), 5000);
      }
    }, 10000);
  };

  const handleMatchmake = (friendId: string) => {
    if (!user) return;
    const friend = MOCK_USERS.find(u => u.id === friendId);
    if (!friend) return;

    const result = findMatch(user, friend, MOCK_THEATERS);
    const theater = MOCK_THEATERS.find(t => t.id === result.recommendedTheaterId);
    
    setMatchResult({
      score: result.score,
      theater: theater ? theater.name : 'Bilinmiyor',
      genres: result.commonGenres,
      friend: friend.fullName
    });
  };

  const handleCreatePlan = (theaterName: string, showtime: string) => {
    if (!user) {
      alert("Plan yapmak için giriş yapmalısınız.");
      return;
    }
    gameEngine.emit('TICKET_PURCHASED', user.id);

    const newPlan: SocialPlan = {
      id: crypto.randomUUID(),
      movieTitle: selectedMovie?.title || '',
      theaterName,
      showtime,
      votes: [
        { user: user.fullName, status: 'join' }
      ]
    };
    setActivePlan(newPlan);
  };

  // --- Data Logic ---

  const displayedMovies = useMemo(() => {
    if (filteredMovieIds === null) return movies;
    return movies.filter(m => filteredMovieIds.includes(m.id));
  }, [filteredMovieIds, movies]);
  
  const displayedUpcomingMovies = useMemo(() => {
    if (filteredMovieIds === null) return MOCK_UPCOMING_MOVIES;
    return MOCK_UPCOMING_MOVIES.filter(m => filteredMovieIds.includes(m.id));
  }, [filteredMovieIds]);

  const processedTheaters = useMemo(() => {
    if (!selectedMovie) return [];

    let theaters: TheaterWithDetails[] = MOCK_THEATERS.map(t => {
      const priceRecord = MOCK_PRICING.find(p => p.movieId === selectedMovie.id && p.theaterId === t.id);
      
      const shows = MOCK_SHOWTIMES.filter(s => {
         const isMovieMatch = s.movieId === selectedMovie.id && s.theaterId === t.id;
         if (!isMovieMatch) return false;

         if (selectedFilters.length > 0) {
           const hasAllFilters = selectedFilters.every(f => s.attributes.includes(f));
           if (!hasAllFilters) return false;
         }
         return true;
      });
      
      let dist = undefined;
      if (userLocation.lat && userLocation.lng) {
        dist = calculateDistance(userLocation.lat, userLocation.lng, t.latitude, t.longitude);
      }

      return {
        ...t,
        distance: dist,
        price: priceRecord ? priceRecord.amount : 0,
        showtimes: shows.sort((a,b) => a.time.localeCompare(b.time))
      };
    }).filter(t => t.showtimes.length > 0);

    if (sortBy === 'cheapest') {
      theaters.sort((a, b) => a.price - b.price);
    } else if (sortBy === 'nearest') {
      theaters.sort((a, b) => {
        if (a.distance !== undefined && b.distance !== undefined) {
          return a.distance - b.distance;
        }
        return 0;
      });
    }

    return theaters;
  }, [selectedMovie, userLocation, sortBy, selectedFilters]);

  // --- Shared Header Component ---
  const Header = ({ isTransparent = false }) => (
    <header className={`flex justify-between items-center py-4 px-6 absolute top-0 left-0 right-0 z-50 transition-all duration-300 ${isTransparent ? 'bg-transparent' : 'bg-[#0f172a]/90 backdrop-blur-xl border-b border-white/5'}`}>
      <div className="flex items-center gap-2 cursor-pointer" onClick={() => setView('list')}>
        <div className="bg-indigo-600 p-1.5 rounded-lg shadow-lg shadow-indigo-500/30">
          <Film className="text-white" size={20} />
        </div>
        <h1 className="text-2xl font-black tracking-tight text-white drop-shadow-md">
          CineScope
        </h1>
      </div>
      
      <div className="flex items-center gap-4">
        <div className="hidden md:flex bg-black/20 p-1 rounded-full border border-white/10 backdrop-blur-md">
           <button 
             onClick={() => setView('list')}
             className={`px-4 py-1.5 rounded-full text-sm font-medium transition ${view === 'list' ? 'bg-white text-black shadow-lg' : 'text-slate-300 hover:text-white'}`}
           >
             Vizyon
           </button>
           <button 
             onClick={() => setView('social')}
             className={`px-4 py-1.5 rounded-full text-sm font-medium transition ${view === 'social' ? 'bg-white text-black shadow-lg' : 'text-slate-300 hover:text-white'}`}
           >
             Sosyal
           </button>
        </div>

        {/* Mobile View Toggle */}
        <div className="md:hidden flex bg-black/40 p-1 rounded-full border border-white/10">
           <button onClick={() => setView('list')} className={`p-2 rounded-full ${view === 'list' ? 'bg-white text-black' : 'text-slate-300'}`}><Film size={16}/></button>
           <button onClick={() => setView('social')} className={`p-2 rounded-full ${view === 'social' ? 'bg-white text-black' : 'text-slate-300'}`}><Users size={16}/></button>
        </div>

         {user ? (
          <div className="flex items-center gap-2 pl-3 border-l border-white/10">
            <button 
              onClick={() => setView('admin')}
              className="hidden sm:flex bg-rose-600 hover:bg-rose-500 text-white text-xs font-bold px-3 py-1.5 rounded-lg mr-2 shadow-lg shadow-rose-900/30 transition"
              title="Yönetim Paneli"
            >
               Admin
            </button>
            <div className="text-right hidden sm:block">
              <div className="text-xs font-bold text-white">{user.fullName}</div>
              <div className="text-[10px] text-indigo-300">Lvl {user.stats.level} Sinefil</div>
            </div>
            <img src={user.avatarUrl} alt={user.fullName} className="w-9 h-9 rounded-full border-2 border-indigo-500 shadow-lg cursor-pointer hover:scale-105 transition" onClick={() => setView('social')} />
          </div>
        ) : (
          <button onClick={login} className="flex items-center gap-2 text-sm text-white font-bold bg-indigo-600 hover:bg-indigo-500 px-4 py-2 rounded-full shadow-lg shadow-indigo-900/50 transition transform hover:scale-105">
             <LogIn size={16} /> Giriş
          </button>
        )}
      </div>
    </header>
  );

  const toggleFilter = (filter: string) => {
    setSelectedFilters(prev => 
      prev.includes(filter) ? prev.filter(f => f !== filter) : [...prev, filter]
    );
  };

  // --- Views ---

  const renderSocialView = () => (
    <div className="min-h-screen bg-[#0f172a] pb-20 pt-20">
       <Header />
       <div className="container mx-auto px-4">
          <div className="flex flex-col gap-10">
              {/* User Dashboard */}
              {user ? (
                 <UserProfile 
                    user={user} 
                    myActivities={MOCK_ACTIVITIES.filter(a => a.userId === user.id)} 
                 />
              ) : (
                <div className="text-center py-20 bg-slate-800 rounded-2xl border border-slate-700 border-dashed mt-10">
                   <h2 className="text-2xl font-bold text-white mb-2">Profilini Gör</h2>
                   <p className="text-slate-400 mb-6">İstatistiklerini, rozetlerini ve aktivitelerini görmek için giriş yap.</p>
                   <button onClick={login} className="bg-indigo-600 text-white px-6 py-3 rounded-xl font-bold hover:bg-indigo-500 transition">Giriş Yap</button>
                </div>
              )}
          </div>
       </div>
    </div>
  );

  const renderMovieList = () => {
    // Feature the first movie or a random one for the Hero Section
    const featuredMovie = movies.length > 0 ? movies[0] : MOCK_MOVIES[0]; 

    return (
      <div className="bg-[#0f172a] min-h-screen pb-20">
        <Header isTransparent={true} />
        
        {/* HERO SECTION */}
        <div className="relative h-[85vh] w-full overflow-hidden">
          {/* Background Image */}
          <div className="absolute inset-0">
             <img src={featuredMovie.backdropUrl} className="w-full h-full object-cover" alt="Hero" />
             {/* Gradient Overlay - Bottom & Left */}
             <div className="absolute inset-0 bg-gradient-to-t from-[#0f172a] via-[#0f172a]/60 to-transparent"></div>
             <div className="absolute inset-0 bg-gradient-to-r from-[#0f172a] via-[#0f172a]/40 to-transparent"></div>
          </div>

          {/* Hero Content */}
          <div className="absolute bottom-0 left-0 w-full p-6 md:p-12 lg:p-20 z-10 flex flex-col justify-end h-full">
             <div className="max-w-2xl animate-in slide-in-from-bottom-10 duration-700 fade-in">
                {/* Spotlight Tag */}
                <div className="inline-flex items-center gap-2 bg-indigo-600/90 text-white text-[10px] font-bold px-3 py-1 rounded-full uppercase tracking-wider mb-4 shadow-lg shadow-indigo-900/50">
                   <Sparkles size={12} /> Günün Tavsiyesi
                </div>
                
                <h2 className="text-4xl md:text-6xl font-black text-white leading-tight mb-4 drop-shadow-2xl">
                   {featuredMovie.title}
                </h2>
                
                <div className="flex items-center gap-4 text-sm text-slate-300 mb-6 font-medium">
                   <span className="text-emerald-400 font-bold flex items-center gap-1"><Star size={14} fill="currentColor" /> {featuredMovie.rating}</span>
                   <span>•</span>
                   <span>{featuredMovie.durationMinutes} dk</span>
                   <span>•</span>
                   <span>{featuredMovie.genre.join(', ')}</span>
                </div>

                <p className="text-slate-200 text-lg mb-8 line-clamp-3 leading-relaxed drop-shadow-md max-w-lg">
                   {featuredMovie.description}
                </p>

                <div className="flex items-center gap-4">
                   <button 
                     onClick={() => handleMovieSelect(featuredMovie)}
                     className="bg-white text-black px-8 py-3.5 rounded-xl font-bold flex items-center gap-2 hover:bg-slate-200 transition transform hover:scale-105 shadow-xl"
                   >
                      <Ticket size={20} />
                      Bilet Al
                   </button>
                   <button 
                     onClick={() => handleMovieSelect(featuredMovie)}
                     className="bg-white/10 backdrop-blur-md text-white border border-white/20 px-8 py-3.5 rounded-xl font-bold flex items-center gap-2 hover:bg-white/20 transition"
                   >
                      <Info size={20} />
                      Detaylar
                   </button>
                </div>
             </div>
          </div>
        </div>

        {/* SEARCH & FILTERS SECTION (Floating) */}
        <div className="container mx-auto px-4 -mt-8 relative z-20">
           <form onSubmit={handleSearch} className="max-w-4xl mx-auto">
              <div className="relative group">
                 <div className="absolute -inset-1 bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500 rounded-2xl opacity-70 blur opacity-40 group-hover:opacity-100 transition duration-1000 group-hover:duration-200"></div>
                 <div className="relative flex items-center bg-[#1e293b]/90 backdrop-blur-xl rounded-2xl border border-white/10 p-2 shadow-2xl">
                    <Search className="text-slate-400 ml-4" size={24} />
                    <input 
                      type="text" 
                      placeholder="Film, oyuncu veya 'hüzünlü bilim kurgu' gibi bir mod ara..." 
                      value={searchQuery}
                      onChange={(e) => setSearchQuery(e.target.value)}
                      className="w-full bg-transparent border-none text-white text-lg px-4 py-3 focus:outline-none placeholder-slate-500"
                    />
                    {isAISearching ? (
                      <div className="pr-4">
                        <div className="animate-spin h-6 w-6 border-2 border-indigo-500 border-t-transparent rounded-full"></div>
                      </div>
                    ) : (
                      <button type="submit" className="bg-indigo-600 hover:bg-indigo-500 text-white p-3 rounded-xl transition shadow-lg">
                         <Sparkles size={20} />
                      </button>
                    )}
                 </div>
              </div>
           </form>
        </div>

        {/* MOVIE GRID SECTION */}
        <div className="container mx-auto px-4 py-16">
          <div className="flex items-end justify-between mb-8 border-b border-slate-800 pb-4">
             <div>
                <h3 className="text-2xl font-bold text-white flex items-center gap-2">
                   <Projector className="text-indigo-500" /> Vizyondaki Filmler
                </h3>
                <p className="text-slate-400 text-sm mt-1">Şehrindeki en popüler yapımlar</p>
             </div>
             
             {filteredMovieIds && (
               <button 
                 onClick={() => { setFilteredMovieIds(null); setSearchQuery(''); }}
                 className="text-sm text-red-400 hover:text-red-300 underline"
               >
                 Aramayı Temizle
               </button>
             )}
          </div>

          <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-6 mb-20">
            {displayedMovies.length === 0 ? (
              <div className="col-span-full text-center py-20 text-slate-500 bg-slate-800/30 rounded-2xl border border-slate-800 border-dashed">
                 <Film size={48} className="mx-auto mb-4 opacity-20" />
                 <p className="text-lg">Aradığınız kriterlere uygun film bulunamadı.</p>
                 <p className="text-sm opacity-60">Farklı anahtar kelimelerle tekrar deneyin.</p>
              </div>
            ) : (
              displayedMovies.map((movie, index) => {
                const uniqueTheatersCount = new Set(MOCK_SHOWTIMES.filter(s => s.movieId === movie.id).map(s => s.theaterId)).size;
                return (
                  <div 
                    key={movie.id} 
                    onClick={() => handleMovieSelect(movie)}
                    className="group cursor-pointer relative bg-slate-800 rounded-xl overflow-hidden shadow-lg hover:shadow-2xl hover:shadow-indigo-900/20 transition-all duration-300 transform hover:-translate-y-2"
                  >
                    {/* Poster */}
                    <div className="aspect-[2/3] relative overflow-hidden">
                      <img src={movie.posterUrl} alt={movie.title} className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-110" />
                      
                      {/* Hover Overlay */}
                      <div className="absolute inset-0 bg-black/60 opacity-0 group-hover:opacity-100 transition-opacity duration-300 flex flex-col items-center justify-center gap-3 backdrop-blur-[2px]">
                         <button className="bg-indigo-600 text-white px-4 py-2 rounded-full font-bold text-sm transform translate-y-4 group-hover:translate-y-0 transition duration-300">
                            Bilet Al
                         </button>
                         <div className="flex items-center gap-1 text-xs text-white transform translate-y-4 group-hover:translate-y-0 transition duration-300 delay-75">
                            <MapPin size={12} className="text-emerald-400" /> {uniqueTheatersCount} Salon
                         </div>
                      </div>

                      {/* Rating Badge */}
                      <div className="absolute top-2 right-2 bg-black/60 backdrop-blur-md text-amber-400 text-xs font-bold px-2 py-1 rounded border border-white/10 flex items-center gap-1">
                         <Trophy size={10} /> {movie.rating}
                      </div>
                    </div>

                    {/* Info */}
                    <div className="p-4 bg-[#1e293b]">
                      <h3 className="text-white font-bold text-base line-clamp-1 mb-1 group-hover:text-indigo-400 transition">{movie.title}</h3>
                      <div className="text-slate-400 text-xs line-clamp-1">{movie.genre.join(', ')}</div>
                    </div>
                  </div>
                )
              })
            )}
          </div>

          {/* UPCOMING MOVIES SECTION */}
          {displayedUpcomingMovies.length > 0 && (
             <div className="pt-10 border-t border-slate-800">
                <div className="flex items-end justify-between mb-8">
                   <div>
                      <h3 className="text-2xl font-bold text-white flex items-center gap-2">
                         <Hourglass className="text-purple-500" /> Yakında Gelecekler
                      </h3>
                      <p className="text-slate-400 text-sm mt-1">Sabırsızlıkla beklenen yapımlar</p>
                   </div>
                </div>

                <div className="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-5 gap-6">
                   {displayedUpcomingMovies.map(movie => (
                      <div 
                        key={movie.id} 
                        onClick={() => handleMovieSelect(movie)}
                        className="group cursor-pointer relative bg-slate-800 rounded-xl overflow-hidden shadow-lg transition-all duration-300 transform hover:-translate-y-2 opacity-90 hover:opacity-100"
                      >
                         <div className="aspect-[2/3] relative overflow-hidden grayscale-[50%] group-hover:grayscale-0 transition duration-500">
                            <img src={movie.posterUrl} alt={movie.title} className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-105" />
                            
                            <div className="absolute top-2 left-2 bg-purple-600 text-white text-[10px] font-bold px-2 py-1 rounded uppercase tracking-wider shadow-lg">
                               YAKINDA
                            </div>

                            <div className="absolute inset-0 bg-black/40 group-hover:bg-black/20 transition-colors flex items-center justify-center">
                               <div className="bg-black/60 backdrop-blur-sm px-3 py-1.5 rounded-lg border border-white/10 text-white text-xs font-bold opacity-0 group-hover:opacity-100 transform translate-y-4 group-hover:translate-y-0 transition duration-300">
                                  İncele & Ekle
                               </div>
                            </div>
                         </div>
                         <div className="p-4 bg-[#1e293b]">
                            <h3 className="text-white font-bold text-base line-clamp-1 mb-1">{movie.title}</h3>
                            <div className="text-purple-400 text-xs font-medium flex items-center gap-1">
                               <Calendar size={12} /> {new Date(movie.releaseDate).toLocaleDateString()}
                            </div>
                         </div>
                      </div>
                   ))}
                </div>
             </div>
          )}
        </div>
      </div>
    );
  };

  const renderMovieDetail = () => {
    if (!selectedMovie) return null;

    // CHECK IF THIS IS AN UPCOMING MOVIE
    const isUpcoming = MOCK_UPCOMING_MOVIES.some(m => m.id === selectedMovie.id);

    // Calculate Rating Breakdown
    const movieReviews = MOCK_REVIEWS.filter(r => r.movieId === selectedMovie.id);
    const totalReviews = movieReviews.length;
    const ratingBreakdown = [5, 4, 3, 2, 1].map(stars => {
       const count = movieReviews.filter(r => r.rating === stars).length;
       const percentage = totalReviews > 0 ? (count / totalReviews) * 100 : 0;
       return { stars, count, percentage };
    });

    const AVAILABLE_FILTERS = [
      { id: 'IMAX', label: 'IMAX' },
      { id: '3D', label: '3D' },
      { id: 'SUB', label: 'Altyazılı' },
      { id: 'DUB', label: 'Dublaj' },
      { id: 'Gold Class', label: 'Gold Class' },
      { id: 'Dolby Atmos', label: 'Dolby Atmos' },
    ];

    // Mock Tracklist for Soundtrack UI
    const mockTracks = [
      { id: 1, title: `${selectedMovie.title} Main Theme`, duration: '3:42' },
      { id: 2, title: 'The Journey Begins', duration: '2:15' },
      { id: 3, title: 'Love & Loss', duration: '4:05' },
      { id: 4, title: 'Final Battle', duration: '5:20' },
    ];

    return (
      <div className="min-h-screen bg-[#0f172a] pb-20">
        {activePlan && (
          <SocialModal plan={activePlan} onClose={() => setActivePlan(null)} />
        )}

        {/* --- CINEMATIC HERO SECTION --- */}
        <div className="relative w-full h-[70vh]">
          {/* Background */}
          <div className="absolute inset-0">
             <img src={selectedMovie.backdropUrl} className="w-full h-full object-cover" alt="backdrop" />
             {/* Complex Gradient Map for readability */}
             <div className="absolute inset-0 bg-gradient-to-b from-slate-900/10 via-slate-900/60 to-[#0f172a]"></div>
             <div className="absolute inset-0 bg-gradient-to-r from-[#0f172a] via-[#0f172a]/70 to-transparent"></div>
          </div>
          
          {/* Top Nav (Floating) */}
          <div className="absolute top-0 left-0 w-full p-6 z-20 flex justify-between items-start">
            <button 
              onClick={goBack} 
              className="bg-white/10 backdrop-blur-md p-3 rounded-full text-white hover:bg-white/20 transition hover:scale-105 border border-white/10 group"
            >
              <ArrowLeft size={24} className="group-hover:-translate-x-1 transition-transform" />
            </button>
            <button 
               onClick={handleSubscribeAlert} 
               className="bg-white/10 backdrop-blur-md p-3 rounded-full text-white hover:bg-indigo-600 transition hover:scale-105 border border-white/10"
               title={isUpcoming ? "Vizyona girince haber ver" : "Fiyat düşünce haber ver"}
            >
               <Bell size={24} />
            </button>
          </div>

          {/* Hero Content */}
          <div className="absolute bottom-0 left-0 w-full p-6 md:p-16 lg:p-24 z-10 flex flex-col justify-end h-full">
            <div className="max-w-4xl animate-in slide-in-from-bottom-8 duration-700 fade-in">
              {/* Genre Pills */}
              <div className="flex flex-wrap gap-2 mb-4">
                 {isUpcoming && <span className="px-3 py-1 bg-purple-600 rounded-full text-xs font-bold text-white uppercase tracking-wider shadow-lg animate-pulse">Yakında</span>}
                 {selectedMovie.genre.map(g => (
                    <span key={g} className="px-3 py-1 bg-white/10 backdrop-blur-md border border-white/20 rounded-full text-xs font-bold text-white uppercase tracking-wider">
                       {g}
                    </span>
                 ))}
              </div>

              <h1 className="text-4xl md:text-6xl lg:text-7xl font-black text-white mb-4 leading-[1.1] drop-shadow-xl">
                 {selectedMovie.title}
              </h1>

              {/* Metadata Bar */}
              <div className="flex items-center gap-6 text-slate-300 text-sm md:text-base font-medium mb-6">
                 {!isUpcoming && (
                   <div className="flex items-center gap-1.5 text-amber-400">
                      <Star size={18} fill="currentColor" />
                      <span className="text-white font-bold">{selectedMovie.rating}</span>/10
                   </div>
                 )}
                 <div className="flex items-center gap-1.5">
                    <Clock size={18} />
                    <span>{Math.floor(selectedMovie.durationMinutes / 60)}s {selectedMovie.durationMinutes % 60}dk</span>
                 </div>
                 <div className="flex items-center gap-1.5">
                    <Calendar size={18} />
                    <span>{new Date(selectedMovie.releaseDate).getFullYear()}</span>
                 </div>
              </div>

              <p className="text-slate-200 text-lg md:text-xl line-clamp-3 leading-relaxed mb-8 max-w-2xl text-shadow">
                 {selectedMovie.description}
              </p>

              {/* Action Buttons */}
              <div className="flex flex-wrap gap-4">
                 {isUpcoming ? (
                   <button 
                     onClick={handleSubscribeAlert}
                     className="bg-purple-600 text-white px-8 py-4 rounded-xl font-bold text-lg flex items-center gap-2 hover:bg-purple-500 transition transform hover:scale-105 shadow-xl shadow-purple-900/30"
                   >
                      <Bell size={24} />
                      Gelince Haber Ver
                   </button>
                 ) : (
                   <button 
                     onClick={() => document.getElementById('showtimes-section')?.scrollIntoView({ behavior: 'smooth' })}
                     className="bg-white text-black px-8 py-4 rounded-xl font-bold text-lg flex items-center gap-2 hover:bg-slate-200 transition transform hover:scale-105 shadow-xl shadow-white/10"
                   >
                      <Ticket size={24} />
                      Bilet Al
                   </button>
                 )}
                 {selectedMovie.soundtrack && (
                   <button 
                     onClick={() => document.getElementById('soundtrack-section')?.scrollIntoView({ behavior: 'smooth' })}
                     className="bg-white/10 backdrop-blur-md text-white border border-white/20 px-6 py-4 rounded-xl font-bold text-lg flex items-center gap-2 hover:bg-white/20 transition"
                   >
                      <Play size={24} />
                      Fragman / Müzik
                   </button>
                 )}
              </div>
            </div>
          </div>
        </div>

        {/* --- MAIN CONTENT GRID --- */}
        <div className="container mx-auto px-4 lg:px-12 -mt-10 relative z-10">
           
           {/* Top Stats & AI Summary Row */}
           <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 mb-10">
              <div className="lg:col-span-2">
                 <AISummary movieTitle={selectedMovie.title} genre={selectedMovie.genre} />
              </div>
              <div className="lg:col-span-1">
                 {selectedMovie.creditScenes && (
                    <div className="h-full">
                       <CreditSceneInfo creditInfo={selectedMovie.creditScenes} />
                    </div>
                 )}
              </div>
           </div>

           {/* Cast & Crew Section (NEW) */}
           {selectedMovie.cast && selectedMovie.cast.length > 0 && (
              <div className="mb-12">
                 <h3 className="text-xl font-bold text-white mb-4 flex items-center gap-2">
                    <Users className="text-indigo-400" /> Oyuncular ve Ekip
                 </h3>
                 <div className="flex gap-4 overflow-x-auto no-scrollbar pb-4">
                    {/* Director Card */}
                    {selectedMovie.director && (
                       <div className="flex-shrink-0 w-40 bg-slate-800 rounded-xl p-3 border border-indigo-500/30 flex flex-col items-center justify-center text-center shadow-lg">
                          <div className="w-16 h-16 rounded-full bg-slate-700 flex items-center justify-center mb-2 border-2 border-indigo-500">
                             <Clapperboard size={24} className="text-indigo-400" />
                          </div>
                          <div className="font-bold text-white text-sm line-clamp-1">{selectedMovie.director}</div>
                          <div className="text-[10px] text-indigo-400 uppercase tracking-wider font-bold">Yönetmen</div>
                       </div>
                    )}

                    {/* Cast List */}
                    {selectedMovie.cast.map(actor => (
                       <div key={actor.id} className="flex-shrink-0 w-36 bg-slate-800/50 rounded-xl p-3 border border-slate-700 flex flex-col items-center text-center hover:bg-slate-800 transition">
                          <div className="w-16 h-16 rounded-full bg-slate-900 mb-2 overflow-hidden shadow-md">
                             {actor.imageUrl ? (
                                <img src={actor.imageUrl} alt={actor.name} className="w-full h-full object-cover" />
                             ) : (
                                <div className="w-full h-full flex items-center justify-center text-slate-600 font-bold text-xs">{actor.name.charAt(0)}</div>
                             )}
                          </div>
                          <div className="font-bold text-white text-xs line-clamp-1">{actor.name}</div>
                          <div className="text-[10px] text-slate-400 line-clamp-1">{actor.role}</div>
                       </div>
                    ))}
                 </div>
              </div>
           )}

           <div className="grid grid-cols-1 lg:grid-cols-12 gap-10">
             
             {/* Left Column: Showtimes & Soundtrack (8/12) */}
             <div className="lg:col-span-8" id="showtimes-section">
                
                {isUpcoming ? (
                   <div className="bg-slate-800/50 rounded-2xl border border-slate-700 p-10 text-center">
                      <Calendar size={64} className="mx-auto text-purple-500 mb-6 opacity-80" />
                      <h3 className="text-2xl font-bold text-white mb-2">Vizyon Tarihi Bekleniyor</h3>
                      <p className="text-slate-400 max-w-md mx-auto mb-8">
                         Bu film <span className="text-white font-bold">{new Date(selectedMovie.releaseDate).toLocaleDateString('tr-TR', { day: 'numeric', month: 'long', year: 'numeric' })}</span> tarihinde vizyona girecek. 
                         Şimdilik bilet satışı bulunmamaktadır.
                      </p>
                      <div className="inline-flex flex-col items-center">
                         <div className="text-sm text-slate-500 mb-2 uppercase tracking-wide">Geri Sayım</div>
                         <div className="flex gap-4 font-mono text-3xl text-white">
                            <div className="bg-slate-900 p-3 rounded-lg border border-slate-700">00<span className="text-xs block text-slate-500 mt-1">GÜN</span></div>
                            <div className="bg-slate-900 p-3 rounded-lg border border-slate-700">00<span className="text-xs block text-slate-500 mt-1">SAAT</span></div>
                         </div>
                      </div>
                   </div>
                ) : (
                  <>
                    {/* Filters & Header */}
                    <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-6 sticky top-0 bg-[#0f172a]/95 backdrop-blur-xl p-4 -mx-4 md:mx-0 z-30 border-b border-slate-800/50 rounded-b-xl">
                        <h2 className="text-2xl font-bold text-white flex items-center gap-2">
                          <Projector className="text-indigo-500" />
                          Seanslar
                          <span className="bg-slate-800 text-slate-400 text-sm px-2 py-0.5 rounded-full">{processedTheaters.length} Sinema</span>
                        </h2>
                        
                        <div className="flex items-center gap-2 overflow-x-auto no-scrollbar">
                          <button 
                            onClick={() => setSortBy('nearest')}
                            className={`px-4 py-2 rounded-lg text-sm font-bold flex items-center gap-2 transition-all border ${
                              sortBy === 'nearest' ? 'bg-indigo-600 border-indigo-500 text-white' : 'bg-slate-800 border-slate-700 text-slate-400 hover:text-white'
                            }`}
                          >
                            <Navigation size={14} /> En Yakın
                          </button>
                          <button 
                            onClick={() => setSortBy('cheapest')}
                            className={`px-4 py-2 rounded-lg text-sm font-bold flex items-center gap-2 transition-all border ${
                              sortBy === 'cheapest' ? 'bg-indigo-600 border-indigo-500 text-white' : 'bg-slate-800 border-slate-700 text-slate-400 hover:text-white'
                            }`}
                          >
                            <Filter size={14} /> En Ucuz
                          </button>
                          <button
                            onClick={() => setIsFilterOpen(!isFilterOpen)}
                            className={`px-4 py-2 rounded-lg text-sm font-bold flex items-center gap-2 transition-all border ${
                              selectedFilters.length > 0 ? 'bg-purple-600 border-purple-500 text-white' : 'bg-slate-800 border-slate-700 text-slate-400 hover:text-white'
                            }`}
                          >
                            Filtrele
                            {selectedFilters.length > 0 && <span className="bg-white/20 px-1.5 rounded-full text-[10px]">{selectedFilters.length}</span>}
                          </button>
                        </div>
                    </div>

                    {isFilterOpen && (
                      <div className="mb-6 bg-slate-800/50 p-4 rounded-xl border border-slate-700 animate-in slide-in-from-top-2">
                         <h4 className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-3">Format Seçimi</h4>
                         <div className="flex flex-wrap gap-2">
                            {AVAILABLE_FILTERS.map(f => {
                              const isActive = selectedFilters.includes(f.id);
                              return (
                                <button
                                  key={f.id}
                                  onClick={() => toggleFilter(f.id)}
                                  className={`px-4 py-2 rounded-lg text-sm font-medium transition-all ${
                                    isActive 
                                      ? 'bg-purple-600 text-white shadow-lg' 
                                      : 'bg-slate-700 text-slate-300 hover:bg-slate-600'
                                  }`}
                                >
                                  {f.label}
                                </button>
                              )
                            })}
                            {selectedFilters.length > 0 && (
                              <button onClick={() => setSelectedFilters([])} className="text-sm text-red-400 hover:text-red-300 ml-auto font-medium">
                                Temizle
                              </button>
                            )}
                         </div>
                      </div>
                    )}

                    <div className="space-y-4">
                      {processedTheaters.length === 0 ? (
                        <div className="text-center py-20 bg-slate-800/30 rounded-2xl border border-slate-800 border-dashed">
                          <Filter className="mx-auto text-slate-600 mb-4" size={48} />
                          <p className="text-slate-400 text-lg">Bu kriterlere uygun seans bulunamadı.</p>
                          <button onClick={() => setSelectedFilters([])} className="mt-4 text-indigo-400 hover:text-indigo-300 font-bold">
                            Filtreleri Sıfırla
                          </button>
                        </div>
                      ) : (
                        processedTheaters.map(theater => (
                          <TheaterCard 
                            key={theater.id} 
                            theater={theater} 
                            onPlanClick={handleCreatePlan}
                          />
                        ))
                      )}
                    </div>
                  </>
                )}

                {/* Soundtrack Section */}
                {selectedMovie.soundtrack && (
                   <div id="soundtrack-section" className="mt-16 relative overflow-hidden rounded-2xl bg-slate-900 border border-slate-800 p-8 group">
                      <div className="absolute top-0 right-0 w-64 h-64 bg-indigo-600/20 rounded-full blur-[100px] -translate-y-1/2 translate-x-1/2"></div>
                      
                      <div className="relative z-10 flex flex-col md:flex-row gap-8 items-center">
                         <div className="w-40 h-40 flex-shrink-0 relative shadow-2xl rounded-lg overflow-hidden group-hover:scale-105 transition-transform duration-500">
                            <img src={selectedMovie.posterUrl} className="w-full h-full object-cover" alt="album art" />
                            <div className="absolute inset-0 bg-black/30 flex items-center justify-center">
                               <div className="w-12 h-12 bg-white/20 backdrop-blur-md rounded-full flex items-center justify-center border border-white/30">
                                  <Play size={20} className="text-white fill-white ml-1" />
                               </div>
                            </div>
                         </div>
                         
                         <div className="flex-1 text-center md:text-left">
                            <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-pink-500/10 text-pink-400 text-xs font-bold uppercase mb-2">
                               <Music size={12} /> Soundtrack
                            </div>
                            <h3 className="text-2xl font-bold text-white mb-1">{selectedMovie.soundtrack.title}</h3>
                            <p className="text-slate-400 text-lg mb-6">{selectedMovie.soundtrack.artist}</p>
                            
                            <div className="bg-slate-800/50 rounded-xl p-4 border border-slate-700/50">
                               {mockTracks.map((track, i) => (
                                  <div key={i} className="flex items-center justify-between p-3 rounded-lg hover:bg-white/5 cursor-pointer group/track transition">
                                     <div className="flex items-center gap-4">
                                        <span className="text-slate-600 font-mono text-xs w-4">{i+1}</span>
                                        <span className="text-slate-300 font-medium group-hover/track:text-white transition">{track.title}</span>
                                     </div>
                                     <span className="text-slate-600 text-xs">{track.duration}</span>
                                  </div>
                               ))}
                            </div>
                            
                            <div className="mt-6 flex justify-center md:justify-start">
                               <button className="flex items-center gap-2 text-emerald-400 font-bold hover:text-emerald-300 transition bg-emerald-900/20 px-4 py-2 rounded-lg border border-emerald-500/20">
                                  <ExternalLink size={16} /> Spotify'da Dinle
                               </button>
                            </div>
                         </div>
                      </div>
                   </div>
                )}
             </div>

             {/* Right Column: Reviews (4/12) */}
             <div className="lg:col-span-4">
                <div className="bg-slate-900/50 rounded-2xl border border-slate-800 p-6 sticky top-24">
                   <div className="flex items-center justify-between mb-6">
                      <h3 className="font-bold text-white flex items-center gap-2 text-lg">
                         <MessageSquare className="text-amber-500" /> Yorumlar
                      </h3>
                      <span className="text-xs font-mono text-slate-500 bg-slate-800 px-2 py-1 rounded">
                         {MOCK_REVIEWS.filter(r => r.movieId === selectedMovie.id).length} Yorum
                      </span>
                   </div>
                   
                   {/* Rating Breakdown Chart */}
                   {totalReviews > 0 && (
                      <div className="mb-6 bg-slate-800 p-4 rounded-xl border border-slate-700/50">
                         <div className="flex items-center gap-2 mb-3">
                            <span className="text-3xl font-black text-white">{selectedMovie.rating}</span>
                            <div className="flex flex-col">
                               <div className="flex text-amber-400 text-xs">
                                  {[...Array(5)].map((_, i) => (
                                    <Star key={i} size={10} fill={i < Math.round(selectedMovie.rating / 2) ? "currentColor" : "none"} />
                                  ))}
                               </div>
                               <span className="text-[10px] text-slate-500">{totalReviews} oy üzerinden</span>
                            </div>
                         </div>
                         <div className="space-y-1.5">
                            {ratingBreakdown.map((item) => (
                               <div key={item.stars} className="flex items-center gap-2 text-xs">
                                  <div className="flex items-center gap-1 w-8 text-slate-400 font-mono">
                                     {item.stars} <Star size={8} />
                                  </div>
                                  <div className="flex-1 h-1.5 bg-slate-700 rounded-full overflow-hidden">
                                     <div 
                                       className="h-full bg-amber-500 rounded-full" 
                                       style={{ width: `${item.percentage}%` }}
                                     ></div>
                                  </div>
                                  <div className="w-8 text-right text-slate-500">{Math.round(item.percentage)}%</div>
                               </div>
                            ))}
                         </div>
                      </div>
                   )}

                   <div className="max-h-[600px] overflow-y-auto pr-2 custom-scrollbar">
                      <ReviewList reviews={MOCK_REVIEWS} movieId={selectedMovie.id} currentUser={user} />
                   </div>
                   
                   <div className="mt-6 pt-6 border-t border-slate-800">
                      <button className="w-full bg-slate-800 hover:bg-indigo-600 hover:text-white border border-slate-700 hover:border-indigo-500 text-slate-300 py-3 rounded-xl font-bold transition-all shadow-lg">
                         Yorum Yap
                      </button>
                   </div>
                </div>
             </div>

           </div>
        </div>
      </div>
    );
  };
  
  // Toasts
  const renderToast = () => {
    return (
      <div className="fixed top-24 right-4 z-50 flex flex-col gap-2">
         {xpNotification && (
           <div className="bg-amber-500 text-black px-4 py-2 rounded-lg shadow-2xl font-bold flex items-center gap-2 animate-in slide-in-from-right duration-300">
             <div className="bg-white/30 p-1 rounded-full"><Trophy size={16} /></div>
             <div>
               <div className="text-xs uppercase opacity-80">{xpNotification.message}</div>
               <div>+{xpNotification.amount} XP</div>
             </div>
           </div>
         )}
         {/* Module 13 Notification */}
         {alertNotification && (
           <div className="bg-indigo-600 text-white px-4 py-3 rounded-lg shadow-2xl flex items-center gap-3 animate-in slide-in-from-right duration-300 max-w-sm border border-indigo-500">
             <div className="bg-white/20 p-1.5 rounded-full"><Bell size={16} /></div>
             <div className="text-sm font-medium">{alertNotification}</div>
           </div>
         )}
      </div>
    )
  };

  return (
    <div className="min-h-screen bg-[#0f172a] text-slate-200 font-sans selection:bg-indigo-500/30">
      {view === 'admin' && <AdminPanel onBack={() => setView('list')} />}
      {view === 'list' && renderMovieList()}
      {view === 'detail' && renderMovieDetail()}
      {view === 'social' && renderSocialView()}
      
      {/* Gamification Notification (Bottom Left) */}
      {renderToast()}
    </div>
  );
}

export default function App() {
  return (
    <AuthProvider>
      <MainApp />
    </AuthProvider>
  );
}