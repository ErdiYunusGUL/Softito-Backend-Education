import React, { useState, useEffect } from 'react';
import { Movie } from '../types';
import { Plus, Trash2, ShieldAlert, Film } from 'lucide-react';

interface AdminPanelProps {
  onBack: () => void;
}

const AdminPanel: React.FC<AdminPanelProps> = ({ onBack }) => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState(true);
  
  // Yeni film state'i
  const [newMovie, setNewMovie] = useState({
    title: '',
    genre: '',
    durationMinutes: 120,
    posterUrl: ''
  });

  const fetchMovies = async () => {
    try {
      const res = await fetch("https://localhost:7066/api/movie");
      const data = await res.json();
      const mappedMovies = data.map((m: any) => ({
        id: m.id.toString(),
        title: m.title,
        description: m.description || "",
        posterUrl: m.posterUrl,
        genre: m.genre ? m.genre.split(',') : ["Bilinmiyor"],
        durationMinutes: m.durationInMinutes,
        rating: 8.0,
      }));
      setMovies(mappedMovies);
    } catch (err) {
      console.error("Filmler getirilemedi:", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchMovies();
  }, []);

  const handleAddMovie = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newMovie.title) return alert("Film adı zorunludur!");

    try {
      const res = await fetch("https://localhost:7066/api/movie", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(newMovie)
      });
      if (res.ok) {
        alert("Film başarıyla eklendi!");
        setNewMovie({ title: '', genre: '', durationMinutes: 120, posterUrl: '' });
        fetchMovies(); // Listeyi yenile
      } else {
        alert("Film eklenirken bir hata oluştu.");
      }
    } catch (err) {
      console.error(err);
      alert("Sunucuya bağlanılamadı.");
    }
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm("Bu filmi silmek istediğinize emin misiniz?")) return;

    try {
      const res = await fetch(`https://localhost:7066/api/movie/${id}`, {
        method: "DELETE"
      });
      if (res.ok) {
        setMovies(movies.filter(m => m.id !== id));
      } else {
        alert("Silme işlemi başarısız.");
      }
    } catch (err) {
      console.error(err);
      alert("Sunucuya bağlanılamadı.");
    }
  };

  return (
    <div className="min-h-screen bg-[#0f172a] pt-24 pb-20 px-6">
      <div className="max-w-6xl mx-auto">
        <div className="flex items-center justify-between mb-8">
          <div className="flex items-center gap-3">
            <ShieldAlert className="text-rose-500" size={32} />
            <h1 className="text-3xl font-black text-white">Yönetim Paneli</h1>
          </div>
          <button onClick={onBack} className="bg-slate-800 hover:bg-slate-700 text-white px-4 py-2 rounded-lg transition">
            Ana Sayfaya Dön
          </button>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Film Ekleme Formu */}
          <div className="bg-slate-800/50 p-6 rounded-2xl border border-slate-700">
            <h2 className="text-xl font-bold text-white mb-6 flex items-center gap-2">
              <Plus className="text-emerald-400" /> Yeni Film Ekle
            </h2>
            <form onSubmit={handleAddMovie} className="space-y-4">
              <div>
                <label className="block text-sm text-slate-400 mb-1">Film Adı *</label>
                <input 
                  type="text" 
                  value={newMovie.title}
                  onChange={e => setNewMovie({...newMovie, title: e.target.value})}
                  className="w-full bg-[#0f172a] border border-slate-700 rounded-lg p-3 text-white focus:border-indigo-500 outline-none"
                  placeholder="Örn: Inception"
                />
              </div>
              <div>
                <label className="block text-sm text-slate-400 mb-1">Türler (Virgülle ayırın)</label>
                <input 
                  type="text" 
                  value={newMovie.genre}
                  onChange={e => setNewMovie({...newMovie, genre: e.target.value})}
                  className="w-full bg-[#0f172a] border border-slate-700 rounded-lg p-3 text-white focus:border-indigo-500 outline-none"
                  placeholder="Örn: Bilim Kurgu, Aksiyon"
                />
              </div>
              <div>
                <label className="block text-sm text-slate-400 mb-1">Süre (Dakika)</label>
                <input 
                  type="number" 
                  value={newMovie.durationMinutes}
                  onChange={e => setNewMovie({...newMovie, durationMinutes: parseInt(e.target.value) || 0})}
                  className="w-full bg-[#0f172a] border border-slate-700 rounded-lg p-3 text-white focus:border-indigo-500 outline-none"
                />
              </div>
              <div>
                <label className="block text-sm text-slate-400 mb-1">Afiş URL (İsteğe bağlı)</label>
                <input 
                  type="text" 
                  value={newMovie.posterUrl}
                  onChange={e => setNewMovie({...newMovie, posterUrl: e.target.value})}
                  className="w-full bg-[#0f172a] border border-slate-700 rounded-lg p-3 text-white focus:border-indigo-500 outline-none"
                  placeholder="https://..."
                />
              </div>
              <button type="submit" className="w-full bg-indigo-600 hover:bg-indigo-500 text-white font-bold py-3 rounded-xl transition mt-4">
                Filmi Kaydet
              </button>
            </form>
          </div>

          {/* Film Listesi */}
          <div className="lg:col-span-2 bg-slate-800/50 p-6 rounded-2xl border border-slate-700">
            <h2 className="text-xl font-bold text-white mb-6 flex items-center gap-2">
              <Film className="text-indigo-400" /> Mevcut Filmler ({movies.length})
            </h2>
            
            {loading ? (
              <div className="text-slate-400 text-center py-10">Yükleniyor...</div>
            ) : (
              <div className="space-y-3 max-h-[600px] overflow-y-auto pr-2 custom-scrollbar">
                {movies.map(movie => (
                  <div key={movie.id} className="flex items-center justify-between bg-[#0f172a] p-4 rounded-xl border border-slate-700/50 hover:border-slate-600 transition">
                    <div className="flex items-center gap-4">
                      <img src={movie.posterUrl} alt={movie.title} className="w-12 h-16 object-cover rounded-md" />
                      <div>
                        <h3 className="text-white font-bold">{movie.title}</h3>
                        <p className="text-slate-400 text-xs">{movie.genre.join(', ')} • {movie.durationMinutes} dk</p>
                      </div>
                    </div>
                    <button 
                      onClick={() => handleDelete(movie.id)}
                      className="text-slate-500 hover:text-rose-500 p-2 bg-slate-800 hover:bg-rose-500/10 rounded-lg transition"
                      title="Filmi Sil"
                    >
                      <Trash2 size={20} />
                    </button>
                  </div>
                ))}
                {movies.length === 0 && (
                  <div className="text-slate-500 text-center py-10">Sistemde hiç film yok.</div>
                )}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminPanel;
