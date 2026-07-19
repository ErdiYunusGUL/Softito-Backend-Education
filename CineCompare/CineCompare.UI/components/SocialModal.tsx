import React from 'react';
import { X, Copy, Check, User, Link as LinkIcon } from 'lucide-react';
import { SocialPlan } from '../types';

interface Props {
  plan: SocialPlan;
  onClose: () => void;
}

const SocialModal: React.FC<Props> = ({ plan, onClose }) => {
  const [copied, setCopied] = React.useState(false);

  const copyLink = () => {
    // Simulate copying a deep link like: cinescope.app/plan/{uuid}
    navigator.clipboard.writeText(`https://cinescope.app/plan/${plan.id}`);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm">
      <div className="bg-slate-800 rounded-2xl w-full max-w-md border border-slate-700 shadow-2xl overflow-hidden animate-in fade-in zoom-in duration-200">
        
        {/* Header */}
        <div className="bg-gradient-to-r from-indigo-600 to-purple-600 p-6 relative">
          <button onClick={onClose} className="absolute top-4 right-4 text-white/70 hover:text-white transition">
            <X size={20} />
          </button>
          <h2 className="text-xl font-bold text-white mb-1">Sinema Planı Oluşturuldu! 🍿</h2>
          <p className="text-indigo-100 text-sm">Arkadaşlarını davet et ve oyları topla.</p>
        </div>

        {/* Content */}
        <div className="p-6">
          <div className="mb-6 bg-slate-700/50 rounded-lg p-4 border border-slate-600">
            <h3 className="font-bold text-white text-lg">{plan.movieTitle}</h3>
            <p className="text-slate-400 text-sm mb-2">{plan.theaterName}</p>
            <div className="inline-block bg-indigo-900/50 text-indigo-300 text-xs font-mono px-2 py-1 rounded">
              Saat: {plan.showtime}
            </div>
          </div>

          {/* Share Link */}
          <div className="mb-6">
            <label className="block text-xs text-slate-400 mb-2 uppercase tracking-wide font-bold">Davet Linki</label>
            <div className="flex gap-2">
              <div className="flex-1 bg-slate-900 rounded-lg px-3 py-2 text-sm text-slate-300 truncate flex items-center gap-2 border border-slate-700">
                <LinkIcon size={14} className="text-slate-500" />
                <span>cinescope.app/plan/{plan.id}</span>
              </div>
              <button 
                onClick={copyLink}
                className={`px-4 py-2 rounded-lg font-medium text-sm transition-all flex items-center gap-2 ${
                  copied ? 'bg-emerald-600 text-white' : 'bg-slate-700 text-white hover:bg-slate-600'
                }`}
              >
                {copied ? <Check size={16} /> : <Copy size={16} />}
                {copied ? 'Kopyalandı' : 'Kopyala'}
              </button>
            </div>
          </div>

          {/* Dummy Voters */}
          <div>
             <label className="block text-xs text-slate-400 mb-2 uppercase tracking-wide font-bold">Katılımcılar</label>
             <div className="space-y-2">
                {plan.votes.map((v, i) => (
                  <div key={i} className="flex items-center justify-between p-2 rounded bg-slate-700/30">
                    <div className="flex items-center gap-2">
                      <div className="w-6 h-6 rounded-full bg-indigo-500/20 flex items-center justify-center text-indigo-300">
                        <User size={12} />
                      </div>
                      <span className="text-sm text-slate-200">{v.user}</span>
                    </div>
                    <span className={`text-xs px-2 py-0.5 rounded-full font-medium ${
                      v.status === 'join' ? 'bg-emerald-500/10 text-emerald-400' : 
                      v.status === 'decline' ? 'bg-red-500/10 text-red-400' : 'bg-amber-500/10 text-amber-400'
                    }`}>
                      {v.status === 'join' ? 'Geliyor' : v.status === 'decline' ? 'Gelemiyor' : 'Belki'}
                    </span>
                  </div>
                ))}
             </div>
          </div>
        </div>
        
        <div className="p-4 bg-slate-900 border-t border-slate-800 flex justify-end">
           <button onClick={onClose} className="text-slate-400 hover:text-white text-sm">Kapat</button>
        </div>
      </div>
    </div>
  );
};

export default SocialModal;