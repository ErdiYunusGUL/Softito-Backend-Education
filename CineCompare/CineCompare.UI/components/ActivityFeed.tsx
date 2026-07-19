import React from 'react';
import { Activity, User } from '../types';
import { MapPin, Film, Star, Calendar } from 'lucide-react';

interface Props {
  activities: Activity[];
  users: User[];
}

const ActivityFeed: React.FC<Props> = ({ activities, users }) => {
  
  const getUser = (id: string) => users.find(u => u.id === id);

  const renderActivityIcon = (type: Activity['type']) => {
    switch(type) {
      case 'CHECK_IN': return <MapPin size={14} className="text-emerald-400" />;
      case 'TICKET_BOUGHT': return <Film size={14} className="text-blue-400" />;
      case 'REVIEW': return <Star size={14} className="text-amber-400" />;
      case 'PLAN_CREATED': return <Calendar size={14} className="text-purple-400" />;
      default: return <Film size={14} />;
    }
  };

  const renderActivityText = (act: Activity, user?: User) => {
    const userName = user?.fullName || 'Biri';
    
    switch(act.type) {
      case 'CHECK_IN':
        return (
          <span>
            <span className="font-semibold text-white">{userName}</span>, <span className="font-bold text-white">{act.payload.movieTitle}</span> filmine <span className="text-emerald-300">{act.payload.theaterName}</span> sinemasında check-in yaptı.
          </span>
        );
      case 'REVIEW':
        return (
           <div className="flex flex-col">
              <span>
                <span className="font-semibold text-white">{userName}</span>, <span className="font-bold text-white">{act.payload.movieTitle}</span> filmini oyladı.
              </span>
              <div className="mt-2 bg-slate-800/50 p-2 rounded text-sm italic text-slate-300 border-l-2 border-amber-500">
                "{act.payload.comment}"
                <div className="text-amber-400 text-xs mt-1">★ {act.payload.rating}/5</div>
              </div>
           </div>
        );
      case 'PLAN_CREATED':
        return (
          <span>
             <span className="font-semibold text-white">{userName}</span>, hafta sonu için <span className="font-bold text-white">{act.payload.movieTitle}</span> planı oluşturdu.
             <button className="ml-2 text-xs bg-purple-600 text-white px-2 py-0.5 rounded hover:bg-purple-500 transition">Katıl</button>
          </span>
        );
      default:
        return <span>Yeni bir aktivite.</span>;
    }
  };

  return (
    <div className="bg-slate-800 rounded-xl p-5 border border-slate-700 h-full">
      <h3 className="font-bold text-lg text-white mb-4 flex items-center gap-2">
        <div className="w-2 h-2 bg-green-500 rounded-full animate-pulse"></div>
        Arkadaş Aktiviteleri
      </h3>
      
      <div className="space-y-6 relative">
         {/* Vertical Line */}
         <div className="absolute left-3.5 top-2 bottom-2 w-0.5 bg-slate-700"></div>

         {activities.map(act => {
           const user = getUser(act.userId);
           return (
             <div key={act.id} className="relative pl-10 flex gap-3">
               {/* Avatar Bubble */}
               <div className="absolute left-0 top-0 w-8 h-8 rounded-full border-2 border-slate-800 overflow-hidden z-10 bg-slate-700">
                 <img src={user?.avatarUrl} alt={user?.fullName} className="w-full h-full object-cover" />
                 <div className="absolute bottom-0 right-0 bg-slate-900 rounded-full p-0.5 transform translate-x-1 translate-y-1">
                   {renderActivityIcon(act.type)}
                 </div>
               </div>

               {/* Content */}
               <div className="text-sm text-slate-400 w-full">
                 <div className="mb-0.5">
                    {renderActivityText(act, user)}
                 </div>
                 <div className="text-[10px] text-slate-500 uppercase tracking-wide font-medium">
                   {new Date(act.createdAt).toLocaleTimeString('tr-TR', {hour: '2-digit', minute:'2-digit'})}
                 </div>
               </div>
             </div>
           )
         })}
      </div>
    </div>
  );
};

export default ActivityFeed;