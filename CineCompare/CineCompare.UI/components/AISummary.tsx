import React, { useState } from 'react';
import { getMovieAnalysis } from '../services/geminiService';
import { Sparkles, Bot, ChevronRight } from 'lucide-react';

interface Props {
  movieTitle: string;
  genre: string[];
}

const AISummary: React.FC<Props> = ({ movieTitle, genre }) => {
  const [analysis, setAnalysis] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [isOpen, setIsOpen] = useState(false);

  const handleGenerate = async () => {
    setIsOpen(true);
    setLoading(true);
    const result = await getMovieAnalysis(movieTitle, genre);
    setAnalysis(result);
    setLoading(false);
  };

  if (!isOpen) {
    return (
      <button 
        onClick={handleGenerate}
        className="w-full bg-gradient-to-r from-indigo-900/50 to-purple-900/50 hover:from-indigo-800 hover:to-purple-800 border border-indigo-500/30 rounded-xl p-4 flex items-center justify-between group transition-all duration-300"
      >
        <div className="flex items-center gap-3">
          <div className="bg-indigo-500/20 p-2 rounded-lg text-indigo-400">
            <Sparkles size={20} />
          </div>
          <div className="text-left">
            <h3 className="font-bold text-indigo-100 text-sm">Yapay Zeka Analizi</h3>
            <p className="text-xs text-indigo-300">Bu film hakkında AI görüşünü almak için tıkla</p>
          </div>
        </div>
        <ChevronRight className="text-indigo-400 group-hover:translate-x-1 transition-transform" size={20} />
      </button>
    );
  }

  return (
    <div className="bg-gradient-to-br from-indigo-950 to-slate-900 rounded-xl p-6 border border-indigo-500/30 shadow-2xl relative overflow-hidden animate-in fade-in zoom-in-95 duration-300">
      <div className="absolute top-0 right-0 p-3 opacity-10">
        <Bot size={64} className="text-indigo-400" />
      </div>
      
      <div className="flex items-center gap-2 mb-4">
        <Sparkles className="text-indigo-400" size={20} />
        <h3 className="font-bold text-indigo-100 text-lg">CineScope AI Rehberi</h3>
      </div>
      
      {loading ? (
        <div className="space-y-3 animate-pulse">
           <div className="h-4 bg-indigo-900/50 rounded w-3/4"></div>
           <div className="h-4 bg-indigo-900/50 rounded w-full"></div>
           <div className="h-4 bg-indigo-900/50 rounded w-5/6"></div>
           <div className="flex justify-center mt-4">
              <span className="text-xs text-indigo-300 animate-bounce">Yapay zeka filmi izliyor...</span>
           </div>
        </div>
      ) : (
        <div className="prose prose-invert prose-sm max-w-none text-slate-300 whitespace-pre-line leading-relaxed font-light">
          {analysis}
        </div>
      )}
    </div>
  );
};

export default AISummary;