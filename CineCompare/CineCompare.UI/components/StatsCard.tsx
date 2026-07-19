import React from 'react';
import { UserStats } from '../types';
import { PieChart, Clock, TrendingUp } from 'lucide-react';

interface Props {
  stats: UserStats;
}

const StatsCard: React.FC<Props> = ({ stats }) => {
  return (
    <div className="bg-slate-800 rounded-xl p-5 border border-slate-700 h-full flex flex-col">
      <h3 className="font-bold text-lg text-white mb-4 flex items-center gap-2">
        <TrendingUp className="text-pink-500" size={20} />
        Cinema Wrapped
      </h3>
      
      {/* Total Watch Time */}
      <div className="flex items-center gap-4 mb-6 bg-slate-900/50 p-4 rounded-xl border border-slate-700/50">
         <div className="p-3 bg-pink-500/10 rounded-full text-pink-400">
           <Clock size={24} />
         </div>
         <div>
           <div className="text-sm text-slate-400">Toplam İzleme Süresi</div>
           <div className="text-2xl font-black text-white">
             {Math.floor(stats.totalMinutesWatched / 60)} <span className="text-base font-normal text-slate-500">saat</span> {stats.totalMinutesWatched % 60} <span className="text-base font-normal text-slate-500">dk</span>
           </div>
         </div>
      </div>

      {/* Genre Distribution (Simple Bar Chart Simulation) */}
      <div className="flex-1">
         <h4 className="text-sm font-semibold text-slate-300 mb-3 flex items-center gap-2">
            <PieChart size={14} /> Tür Dağılımı
         </h4>
         <div className="space-y-3">
           {stats.favoriteGenres.map((g, idx) => (
             <div key={idx}>
               <div className="flex justify-between text-xs text-slate-400 mb-1">
                  <span>{g.genre}</span>
                  <span>{g.count} film ({g.percentage}%)</span>
               </div>
               <div className="w-full bg-slate-700 rounded-full h-2">
                  <div 
                    className={`h-2 rounded-full ${idx === 0 ? 'bg-indigo-500' : idx === 1 ? 'bg-purple-500' : 'bg-emerald-500'}`} 
                    style={{ width: `${g.percentage}%` }}
                  ></div>
               </div>
             </div>
           ))}
         </div>
      </div>
      
      <div className="mt-6 pt-4 border-t border-slate-700 text-center text-xs text-slate-500 italic">
         "Sen gerçek bir sinefilsin!"
      </div>
    </div>
  );
};

export default StatsCard;