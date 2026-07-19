import React from 'react';
import { PassportStamp, User } from '../types';
import { MapPin, Plane, Award } from 'lucide-react';

interface Props {
  stamps: PassportStamp[];
  user: User;
}

const PassportBook: React.FC<Props> = ({ stamps, user }) => {
  
  // Helper to generate random rotation for natural stamp look
  const getRandomRotation = (seed: number) => {
    // Simple pseudo-random based on string length or id
    return (seed % 30) - 15; // Between -15 and 15 degrees
  };

  const getRandomColor = (seed: number) => {
      const colors = [
          'border-red-800 text-red-900', 
          'border-blue-800 text-blue-900', 
          'border-emerald-800 text-emerald-900',
          'border-purple-800 text-purple-900'
      ];
      return colors[seed % colors.length];
  };

  return (
    <div className="w-full max-w-3xl mx-auto perspective-1000">
       <div className="bg-[#e2e8f0] rounded-r-2xl rounded-l-md shadow-2xl relative overflow-hidden min-h-[500px] border-l-[12px] border-slate-800 flex flex-col md:flex-row">
          
          {/* Background Texture Overlay */}
          <div className="absolute inset-0 opacity-10 pointer-events-none bg-[url('https://www.transparenttextures.com/patterns/paper-fibers.png')]"></div>
          <div className="absolute inset-0 bg-gradient-to-br from-white/40 to-slate-300/40 pointer-events-none"></div>

          {/* Left Page (User Info) */}
          <div className="md:w-1/3 p-6 border-b md:border-b-0 md:border-r border-slate-300/50 flex flex-col relative z-10">
             <div className="flex items-center gap-2 mb-6 opacity-70">
                <div className="w-8 h-8 rounded-full bg-slate-800 flex items-center justify-center text-white">
                   <Plane size={16} />
                </div>
                <div className="font-mono text-xs font-bold tracking-widest text-slate-700 uppercase">Sinefil Pasaportu</div>
             </div>

             <div className="mb-6 text-center">
                <div className="w-24 h-24 mx-auto bg-slate-200 rounded-lg p-1 border border-slate-300 shadow-inner mb-3 grayscale contrast-125">
                   <img src={user.avatarUrl} className="w-full h-full object-cover rounded" alt="passport" />
                </div>
                <h3 className="font-serif font-bold text-slate-900 text-lg uppercase tracking-wide">{user.fullName}</h3>
                <div className="font-mono text-xs text-slate-500">ID: {user.id.toUpperCase()}</div>
             </div>

             <div className="space-y-3 mt-auto">
                <div className="bg-slate-100/50 p-2 rounded border border-slate-300">
                   <div className="text-[10px] text-slate-500 uppercase">Sinema Seviyesi</div>
                   <div className="font-mono font-bold text-slate-800">LVL {user.stats.level}</div>
                </div>
                <div className="bg-slate-100/50 p-2 rounded border border-slate-300">
                   <div className="text-[10px] text-slate-500 uppercase">Toplam Ziyaret</div>
                   <div className="font-mono font-bold text-slate-800">{stamps.length}</div>
                </div>
             </div>
          </div>

          {/* Right Page (Stamps Grid) */}
          <div className="md:w-2/3 p-8 relative z-10 bg-[radial-gradient(#cbd5e1_1px,transparent_1px)] [background-size:16px_16px]">
             <h4 className="font-mono text-xs text-slate-400 uppercase tracking-widest mb-6 text-right border-b border-slate-300 pb-2">Vize & Giriş Damgaları</h4>
             
             {stamps.length === 0 ? (
                <div className="h-full flex flex-col items-center justify-center opacity-40">
                   <div className="border-4 border-dashed border-slate-400 rounded-full w-32 h-32 flex items-center justify-center mb-2">
                      <Award size={48} className="text-slate-400" />
                   </div>
                   <p className="font-serif text-slate-500 italic">Henüz seyahat yok...</p>
                </div>
             ) : (
                <div className="grid grid-cols-2 sm:grid-cols-3 gap-6 content-start">
                   {stamps.map((stamp, i) => {
                      const rotation = getRandomRotation(i + stamp.theaterId.length);
                      const colorClass = getRandomColor(i);
                      
                      return (
                         <div 
                           key={`${stamp.theaterId}-${i}`}
                           className={`aspect-square rounded-full border-[3px] p-2 flex flex-col items-center justify-center text-center select-none hover:scale-110 transition-transform duration-300 cursor-help group relative ${colorClass}`}
                           style={{ 
                              transform: `rotate(${rotation}deg)`,
                              maskImage: 'url(https://www.transparenttextures.com/patterns/broken-noise.png)', // Texture effect for stamp look
                              WebkitMaskImage: 'url(https://www.transparenttextures.com/patterns/broken-noise.png)'
                           }}
                           title={`${stamp.theaterName} - ${new Date(stamp.visitedAt).toLocaleDateString()}`}
                         >
                            <div className="text-[9px] uppercase font-bold opacity-70 leading-tight mb-1">{stamp.theaterName.split(' ')[0]}</div>
                            <MapPin size={16} className="opacity-50 my-1" />
                            <div className="font-mono text-[8px] font-bold opacity-80 border-t border-current pt-0.5 mt-0.5 w-full">
                               {new Date(stamp.visitedAt).toLocaleDateString('tr-TR', { day: '2-digit', month: '2-digit', year: '2-digit' })}
                            </div>

                            {/* Tooltip on Hover */}
                            <div className="absolute -top-10 left-1/2 -translate-x-1/2 bg-black text-white text-[10px] py-1 px-2 rounded opacity-0 group-hover:opacity-100 pointer-events-none transition whitespace-nowrap z-20">
                               {stamp.theaterName}
                            </div>
                         </div>
                      )
                   })}
                </div>
             )}
          </div>
       </div>
    </div>
  );
};

export default PassportBook;