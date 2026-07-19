import React, { useState } from 'react';
import { Review, User } from '../types';
import { Eye, EyeOff, Star, AlertTriangle, CheckCircle2 } from 'lucide-react';
import { MOCK_ACTIVITIES } from '../constants';

interface Props {
  reviews: Review[];
  movieId: string;
  currentUser: User | null;
}

const ReviewList: React.FC<Props> = ({ reviews, movieId, currentUser }) => {
  const filteredReviews = reviews.filter(r => r.movieId === movieId);
  
  // UX Rule: If user has 'CHECK_IN' activity for this movie, auto-reveal spoilers
  const hasWatchedMovie = currentUser ? MOCK_ACTIVITIES.some(a => 
    a.userId === currentUser.id && 
    a.payload.movieId === movieId && 
    (a.type === 'CHECK_IN' || a.type === 'TICKET_BOUGHT')
  ) : false;

  const [revealedSpoilers, setRevealedSpoilers] = useState<Set<string>>(new Set());

  const toggleSpoiler = (reviewId: string) => {
    const newSet = new Set(revealedSpoilers);
    if (newSet.has(reviewId)) {
      newSet.delete(reviewId);
    } else {
      newSet.add(reviewId);
    }
    setRevealedSpoilers(newSet);
  };

  if (filteredReviews.length === 0) {
    return (
      <div className="text-slate-500 text-center py-8 text-sm italic bg-slate-800/30 rounded-xl border border-slate-700/50">
        Henüz yorum yapılmamış. İlk yorumu sen yap!
      </div>
    );
  }

  return (
    <div className="space-y-4">
      {filteredReviews.map(review => {
        // Spoiler Display Logic
        const isActuallyRevealed = revealedSpoilers.has(review.id) || hasWatchedMovie;
        const isContentHidden = review.isSpoiler && !isActuallyRevealed;

        return (
          <div 
            key={review.id} 
            className={`rounded-xl p-4 border transition-all duration-300 ${
              review.isSpoiler 
                ? 'bg-amber-950/10 border-amber-900/30 hover:border-amber-700/50' 
                : 'bg-slate-800 border-slate-700 hover:border-slate-600'
            }`}
          >
             {/* Header */}
             <div className="flex justify-between items-start mb-3">
                <div className="flex items-center gap-3">
                   <div className="relative">
                     <img src={review.userAvatar} alt={review.userName} className="w-9 h-9 rounded-full border border-slate-600" />
                     {review.isSpoiler && (
                        <div className="absolute -bottom-1 -right-1 bg-amber-500 text-black rounded-full p-0.5" title="Spoiler İçerir">
                           <AlertTriangle size={10} />
                        </div>
                     )}
                   </div>
                   <div>
                      <div className="text-sm font-bold text-slate-200">{review.userName}</div>
                      <div className="flex text-amber-400 text-[10px] mt-0.5">
                         {[...Array(5)].map((_, i) => (
                           <Star key={i} size={10} fill={i < review.rating ? "currentColor" : "none"} />
                         ))}
                      </div>
                   </div>
                </div>
                
                <div className="flex flex-col items-end gap-1">
                   <div className="text-[10px] text-slate-500">
                      {new Date(review.createdAt).toLocaleDateString()}
                   </div>
                   {/* Manual Hide Toggle (Top Right) - Only visible if revealed and is a spoiler */}
                   {review.isSpoiler && !isContentHidden && (
                      <button 
                        onClick={() => toggleSpoiler(review.id)}
                        className="text-slate-500 hover:text-amber-400 transition"
                        title="Spoiler'ı Gizle"
                      >
                         <EyeOff size={14} />
                      </button>
                   )}
                </div>
             </div>

             {/* Content Area */}
             <div className="relative mt-1">
                <div className={`text-sm leading-relaxed transition-all duration-500 ${isContentHidden ? 'blur-sm opacity-50 select-none' : 'text-slate-300'}`}>
                   {review.content}
                </div>

                {/* Spoiler Overlay (The Curtain) */}
                {isContentHidden && (
                  <div className="absolute inset-0 flex flex-col items-center justify-center z-10 rounded-lg backdrop-blur-[2px] bg-slate-900/40 border border-white/5">
                     <div className="bg-slate-900/90 p-4 rounded-xl border border-amber-500/30 shadow-2xl flex flex-col items-center text-center max-w-[80%]">
                        <div className="text-amber-500 mb-2">
                           <AlertTriangle size={24} />
                        </div>
                        <h4 className="text-white font-bold text-sm mb-1">Spoiler Uyarısı</h4>
                        <p className="text-[10px] text-slate-400 mb-3">Bu yorum film hakkında kritik detaylar içeriyor olabilir.</p>
                        <button 
                          onClick={() => toggleSpoiler(review.id)}
                          className="bg-amber-600 hover:bg-amber-500 text-white text-xs font-bold px-4 py-2 rounded-lg transition flex items-center gap-2 shadow-lg shadow-amber-900/20"
                        >
                           <Eye size={14} /> Yorumu Göster
                        </button>
                     </div>
                  </div>
                )}
                
                {/* Auto-Reveal Indicator */}
                {review.isSpoiler && hasWatchedMovie && !revealedSpoilers.has(review.id) && ( // Don't show if user manually toggled it
                   <div className="mt-3 inline-flex items-center gap-1.5 px-2 py-1 bg-emerald-500/10 border border-emerald-500/20 rounded text-[10px] text-emerald-400 font-medium">
                      <CheckCircle2 size={12} /> Filmi izlediğin için spoiler otomatik açıldı.
                   </div>
                )}
             </div>
          </div>
        )
      })}
    </div>
  );
};

export default ReviewList;