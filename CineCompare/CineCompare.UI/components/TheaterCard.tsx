import React from 'react';
import { TheaterWithDetails, Showtime } from '../types';
import { MapPin, Ticket, Navigation, Share2, ExternalLink } from 'lucide-react';

interface Props {
  theater: TheaterWithDetails;
  onPlanClick: (theaterName: string, showtime: string) => void;
}

const TheaterCard: React.FC<Props> = ({ theater, onPlanClick }) => {
  
  // Helper to render attribute badges (IMAX, SUB, etc.)
  const renderAttributes = (attrs: string[]) => {
    return (
      <div className="flex flex-wrap gap-1 mt-1.5 justify-center">
        {attrs.map(attr => {
           let colorClass = "text-slate-400 bg-slate-900 border-slate-700";
           if (attr === 'IMAX') colorClass = "text-blue-300 bg-blue-950 border-blue-800/50 shadow-[0_0_10px_rgba(30,58,138,0.3)]";
           if (attr === '3D') colorClass = "text-purple-300 bg-purple-950 border-purple-800/50";
           if (attr === 'SUB') colorClass = "text-amber-300 bg-amber-950 border-amber-800/50"; // Altyazı
           if (attr === 'DUB') colorClass = "text-pink-300 bg-pink-950 border-pink-800/50"; // Dublaj
           if (attr === 'Gold Class') colorClass = "text-yellow-200 bg-yellow-950 border-yellow-700/50";
           
           return (
             <span key={attr} className={`text-[9px] font-bold px-1.5 py-0.5 rounded border ${colorClass}`}>
               {attr}
             </span>
           )
        })}
      </div>
    );
  };

  return (
    <div className="bg-[#1e293b]/50 backdrop-blur-sm rounded-xl p-5 shadow-lg border border-slate-700 hover:border-indigo-500/50 transition-all duration-300 group">
      
      {/* Header */}
      <div className="flex flex-col sm:flex-row justify-between items-start mb-4 gap-3">
        <div className="flex-1">
          <h3 className="font-bold text-lg text-white leading-tight group-hover:text-indigo-300 transition-colors">{theater.name}</h3>
          <p className="text-slate-400 text-xs mt-1.5 flex items-center gap-1.5">
            <MapPin size={12} className="text-slate-500" />
            {theater.address}
          </p>
          
          {/* Facilities Tags */}
          <div className="flex gap-2 mt-3 overflow-x-auto no-scrollbar">
            {theater.facilities.map((f, i) => (
              <span key={i} className="text-[10px] uppercase font-bold tracking-wider bg-slate-800 text-slate-400 px-2 py-0.5 rounded border border-slate-700/50">
                {f}
              </span>
            ))}
          </div>
        </div>

        <div className="flex sm:flex-col items-end gap-2 shrink-0">
          {theater.distance !== undefined && (
            <div className="bg-emerald-500/10 text-emerald-400 border border-emerald-500/20 px-2.5 py-1 rounded-lg text-xs font-bold flex items-center gap-1.5">
              <Navigation size={12} />
              {theater.distance} km
            </div>
          )}
          <div className="bg-amber-500/10 text-amber-400 border border-amber-500/20 px-2.5 py-1 rounded-lg text-xs font-bold flex items-center gap-1.5">
            <Ticket size={12} />
            {theater.price} TL
          </div>
        </div>
      </div>

      {/* Divider */}
      <div className="h-px bg-slate-700/50 w-full mb-4"></div>

      {/* Showtimes Grid */}
      <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-3">
        {theater.showtimes.map((st) => (
          <div key={st.id} className="relative group/time">
            {/* Main Time Button (Deep Link) */}
            <a 
              href={st.ticketUrl}
              target="_blank"
              rel="noopener noreferrer nofollow"
              className="flex flex-col items-center justify-center py-2.5 px-1 bg-slate-800 rounded-lg hover:bg-indigo-600 transition-all border border-slate-700 hover:border-indigo-400 w-full shadow-sm hover:shadow-indigo-500/20 hover:-translate-y-0.5"
            >
              <div className="flex items-center gap-1.5">
                <span className="text-base font-black text-white group-hover/time:text-white tracking-tight">{st.time}</span>
                <ExternalLink size={10} className="text-slate-600 group-hover/time:text-indigo-200" />
              </div>
              {/* Render Attributes */}
              {renderAttributes(st.attributes)}
            </a>

            {/* Plan Button (appears on hover) */}
            <button 
              onClick={(e) => {
                e.preventDefault();
                onPlanClick(theater.name, st.time);
              }}
              className="absolute -top-2 -right-2 bg-indigo-500 text-white p-1.5 rounded-full shadow-lg opacity-0 group-hover/time:opacity-100 transition-all hover:bg-white hover:text-indigo-600 scale-75 hover:scale-100 z-10 border-2 border-[#1e293b]"
              title="Arkadaşlarınla Planla"
            >
              <Share2 size={12} strokeWidth={3} />
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};

export default TheaterCard;