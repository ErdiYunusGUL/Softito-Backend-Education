import React, { useState } from 'react';
import { Info, ThumbsUp, Eye, EyeOff, AlertCircle, Film } from 'lucide-react';

interface Props {
  creditInfo: {
    hasMid: boolean;
    hasPost: boolean;
    description: string;
    confidenceScore: number;
  };
}

const CreditSceneInfo: React.FC<Props> = ({ creditInfo }) => {
  const [isRevealed, setIsRevealed] = useState(false);

  const hasAnyScene = creditInfo.hasMid || creditInfo.hasPost;

  return (
    <div className="bg-slate-900 rounded-xl border border-slate-700 overflow-hidden shadow-lg h-full flex flex-col">
      {/* Header Bar */}
      <div className="bg-slate-800 px-4 py-3 border-b border-slate-700 flex justify-between items-center shrink-0">
        <div className="flex items-center gap-2">
          <Film className="text-purple-400" size={18} />
          <h3 className="text-white font-bold text-sm">Jenerik Sonrası Bilgisi</h3>
        </div>
        
        {/* Community Confidence Badge */}
        <div className="flex items-center gap-1.5 bg-slate-900 px-2 py-1 rounded text-xs text-slate-400" title="Topluluk Doğrulama Oranı">
           <ThumbsUp size={12} className="text-emerald-500" />
           <span className="font-mono text-emerald-400">%{creditInfo.confidenceScore}</span>
           <span className="hidden sm:inline">Güvenilir</span>
        </div>
      </div>

      {/* Visual Status Indicators */}
      <div className="flex divide-x divide-slate-700 shrink-0">
         <div className="flex-1 p-3 flex flex-col items-center justify-center gap-1 text-center">
            <span className="text-[10px] text-slate-500 uppercase font-bold tracking-wider">Jenerik Ortası</span>
            {creditInfo.hasMid ? (
               <div className="flex items-center gap-1.5 text-emerald-400 font-bold text-sm animate-pulse">
                  <div className="w-2 h-2 rounded-full bg-emerald-500"></div> VAR
               </div>
            ) : (
               <div className="text-slate-500 font-medium text-sm">YOK</div>
            )}
         </div>
         <div className="flex-1 p-3 flex flex-col items-center justify-center gap-1 text-center">
            <span className="text-[10px] text-slate-500 uppercase font-bold tracking-wider">Jenerik Sonu</span>
            {creditInfo.hasPost ? (
               <div className="flex items-center gap-1.5 text-emerald-400 font-bold text-sm animate-pulse">
                  <div className="w-2 h-2 rounded-full bg-emerald-500"></div> VAR
               </div>
            ) : (
               <div className="text-slate-500 font-medium text-sm">YOK</div>
            )}
         </div>
      </div>

      {/* Description / Spoiler Section */}
      <div className="px-4 py-3 bg-slate-800/50 relative flex-1 flex flex-col justify-center">
         {!isRevealed ? (
            <button 
              onClick={() => setIsRevealed(true)}
              className="w-full flex flex-col items-center justify-center gap-2 py-2 text-slate-400 hover:text-white transition group"
            >
               <Eye size={24} className="group-hover:scale-110 transition-transform" />
               <span className="text-xs font-bold uppercase tracking-wider">Detayı Görmek İçin Tıkla (Spoiler)</span>
            </button>
         ) : (
            <div className="animate-in fade-in zoom-in duration-300">
               <div className="flex items-start gap-3">
                  <AlertCircle size={16} className="text-amber-500 mt-0.5 flex-shrink-0" />
                  <p className="text-sm text-slate-300 leading-relaxed">
                     {creditInfo.description}
                  </p>
               </div>
               <button 
                 onClick={() => setIsRevealed(false)}
                 className="mt-3 text-xs text-slate-500 hover:text-slate-300 flex items-center gap-1 w-full justify-end"
               >
                  <EyeOff size={12} /> Gizle
               </button>
            </div>
         )}
         
         {!isRevealed && hasAnyScene && (
             <div className="absolute inset-0 bg-slate-900/10 backdrop-blur-sm pointer-events-none"></div>
         )}
      </div>

      {/* User Action (Vote) - Simulated */}
      <div className="bg-slate-900 px-4 py-2 text-center text-[10px] text-slate-500 border-t border-slate-700 shrink-0">
         Bilgi yanlış mı? <button className="text-indigo-400 hover:underline">Bildir</button>
      </div>
    </div>
  );
};

export default CreditSceneInfo;