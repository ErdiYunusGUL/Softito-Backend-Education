import React, { useState, useEffect } from 'react';
import { User, Activity } from '../types';
import { MOCK_BADGES, MOCK_REVIEWS, MOCK_MOVIES, MOCK_FRIENDSHIPS } from '../constants';
import StatsCard from './StatsCard';
import PassportBook from './PassportBook'; // Module 15
import { Settings, Edit3, Award, Layout, Clock, Camera, Save, History, Music, PlayCircle, ExternalLink, UserPlus, UserCheck, Users, Trash2, Bell, Shield, Lock, AlertTriangle, X, Check, Book, Trophy } from 'lucide-react';
import { useAuth } from '../contexts/AuthContext';

interface Props {
  user: User;
  myActivities: Activity[];
}

type TabType = 'overview' | 'passport' | 'history' | 'music' | 'badges' | 'reviews' | 'settings';

const AVAILABLE_GENRES = [
  'Bilim Kurgu', 'Aksiyon', 'Dram', 'Komedi', 'Macera', 
  'Korku', 'Gerilim', 'Animasyon', 'Romantik', 'Fantastik', 
  'Belgesel', 'Suç', 'Gizem', 'Aile'
];

const UserProfile: React.FC<Props> = ({ user: initialUser, myActivities }) => {
  const { user: currentUser } = useAuth();
  
  // Local state to handle optimistic updates during editing
  const [displayUser, setDisplayUser] = useState<User>(initialUser);
  
  const [activeTab, setActiveTab] = useState<TabType>('overview');
  const [isEditing, setIsEditing] = useState(false);
  
  // Form States
  const [editName, setEditName] = useState(initialUser.fullName);
  const [editGenres, setEditGenres] = useState<string[]>(initialUser.preferences.favoriteGenres);
  
  // Settings State (Interactive)
  const [userSettings, setUserSettings] = useState({
    notifications: true,
    spoilerProtection: true,
    privacy: false
  });

  // Local Review State for Deletion Simulation
  const [deletedReviewIds, setDeletedReviewIds] = useState<Set<string>>(new Set());
  
  // Friend Request State
  const [friendStatus, setFriendStatus] = useState<'none' | 'pending' | 'accepted'>('none');

  // Determine if this is the logged-in user's profile
  const isOwnProfile = currentUser?.id === initialUser.id;

  // Sync props to state if initialUser changes (e.g. navigation)
  useEffect(() => {
    setDisplayUser(initialUser);
    setEditName(initialUser.fullName);
    setEditGenres(initialUser.preferences.favoriteGenres);
  }, [initialUser]);

  // Check initial friendship status
  useEffect(() => {
    if (currentUser && !isOwnProfile) {
      const friendship = MOCK_FRIENDSHIPS.find(f => 
        (f.requesterId === currentUser.id && f.addresseeId === initialUser.id) || 
        (f.requesterId === initialUser.id && f.addresseeId === currentUser.id)
      );
      if (friendship) {
        setFriendStatus(friendship.status as 'pending' | 'accepted');
      }
    }
  }, [currentUser, initialUser.id, isOwnProfile]);

  const handleAddFriend = () => {
    setFriendStatus('pending');
    alert(`${displayUser.fullName} kişisine arkadaşlık isteği gönderildi!`);
  };

  const toggleSetting = (key: keyof typeof userSettings) => {
    setUserSettings(prev => ({ ...prev, [key]: !prev[key] }));
  };

  const handleDeleteReview = (reviewId: string) => {
    if (window.confirm('Bu yorumu silmek istediğinize emin misiniz?')) {
      setDeletedReviewIds(prev => new Set(prev).add(reviewId));
    }
  };
  
  const handleSaveProfile = () => {
    // In a real app, this would make an API call.
    // Here we update the local display state to simulate persistence.
    setDisplayUser(prev => ({
      ...prev,
      fullName: editName,
      preferences: {
        ...prev.preferences,
        favoriteGenres: editGenres
      }
    }));
    setIsEditing(false);
  };

  const handleCancelEdit = () => {
    setEditName(displayUser.fullName);
    setEditGenres(displayUser.preferences.favoriteGenres);
    setIsEditing(false);
  };

  const toggleGenreSelection = (genre: string) => {
    setEditGenres(prev => 
      prev.includes(genre) 
        ? prev.filter(g => g !== genre)
        : [...prev, genre]
    );
  };

  // Filter reviews for this user
  const myReviews = MOCK_REVIEWS.filter(r => r.userId === initialUser.id && !deletedReviewIds.has(r.id));

  const calculateNextLevelProgress = () => {
    // Mock logic: Each level requires 100 * level XP
    const nextLevelXp = displayUser.stats.level * 100 + 200; 
    const progress = (displayUser.stats.xp / nextLevelXp) * 100;
    return Math.min(progress, 100);
  };

  const renderTabs = () => (
    <div className="flex gap-1 bg-slate-900/50 p-1 rounded-xl mb-6 overflow-x-auto no-scrollbar">
      {[
        { id: 'overview', label: 'Genel Bakış', icon: Layout },
        { id: 'passport', label: 'Pasaport', icon: Book },
        { id: 'history', label: 'İzleme Geçmişi', icon: History },
        { id: 'music', label: 'Soundtrack', icon: Music },
        { id: 'badges', label: 'Koleksiyon', icon: Award },
        { id: 'reviews', label: 'İncelemeler', icon: Edit3 },
        // Only show settings tab if it is own profile
        ...(isOwnProfile ? [{ id: 'settings', label: 'Ayarlar', icon: Settings }] : []),
      ].map((tab) => (
        <button
          key={tab.id}
          onClick={() => setActiveTab(tab.id as TabType)}
          className={`flex items-center gap-2 px-4 py-2.5 rounded-lg text-sm font-medium transition-all whitespace-nowrap ${
            activeTab === tab.id
              ? 'bg-indigo-600 text-white shadow-lg'
              : 'text-slate-400 hover:text-white hover:bg-slate-800'
          }`}
        >
          <tab.icon size={16} />
          {tab.label}
        </button>
      ))}
    </div>
  );

  const renderOverview = () => {
    const nextLevelXp = displayUser.stats.level * 100 + 200;

    return (
    <div className="grid grid-cols-1 gap-6 animate-in fade-in slide-in-from-bottom-4 duration-500">
      
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          {/* NEW: Gamification Status Card */}
          <div className="bg-gradient-to-br from-slate-800 to-slate-900 rounded-xl p-5 border border-amber-500/20 relative overflow-hidden group shadow-lg">
             {/* Decorative background glow */}
             <div className="absolute -top-10 -right-10 w-40 h-40 bg-amber-500/10 rounded-full blur-3xl"></div>

             <div className="flex justify-between items-start mb-6 relative z-10">
                <div>
                   <h3 className="font-bold text-white flex items-center gap-2">
                      <Trophy className="text-amber-400" size={20} />
                      Seviye Durumu
                   </h3>
                   <p className="text-xs text-slate-400 mt-1">Sonraki seviyeye yolculuk</p>
                </div>
                <div className="bg-amber-500/10 text-amber-400 px-3 py-1 rounded-full text-sm font-black border border-amber-500/20 shadow-[0_0_15px_rgba(245,158,11,0.2)]">
                   LVL {displayUser.stats.level}
                </div>
             </div>

             {/* Progress Bar */}
             <div className="mb-6 relative z-10">
                <div className="flex justify-between text-xs mb-2 font-medium">
                   <span className="text-slate-300">{displayUser.stats.xp} XP</span>
                   <span className="text-slate-500">{nextLevelXp} XP</span>
                </div>
                <div className="w-full bg-slate-950 rounded-full h-3 border border-slate-700/50">
                   <div 
                     className="bg-gradient-to-r from-amber-600 to-yellow-400 h-3 rounded-full shadow-[0_0_10px_rgba(245,158,11,0.5)] transition-all duration-1000 relative" 
                     style={{ width: `${calculateNextLevelProgress()}%` }}
                   >
                      <div className="absolute top-0 right-0 bottom-0 w-1 bg-white/50 animate-pulse"></div>
                   </div>
                </div>
                <div className="text-right mt-2">
                   <span className="text-[10px] text-amber-500 font-bold bg-amber-950/30 px-2 py-0.5 rounded border border-amber-500/10">
                     {Math.round(nextLevelXp - displayUser.stats.xp)} XP kaldı
                   </span>
                </div>
             </div>

             {/* Recent Badges Mini Preview */}
             <div className="relative z-10 border-t border-slate-700/50 pt-4">
                <div className="flex justify-between items-center mb-3">
                   <span className="text-xs text-slate-400 uppercase tracking-wide font-bold">Koleksiyon</span>
                   <button onClick={() => setActiveTab('badges')} className="text-[10px] text-indigo-400 hover:text-indigo-300 transition">Tümünü Gör</button>
                </div>
                <div className="flex gap-3">
                   {MOCK_BADGES.filter(b => displayUser.stats.badges.includes(b.id)).slice(0, 4).map(badge => (
                      <div key={badge.id} className="w-10 h-10 bg-slate-800 rounded-lg flex items-center justify-center text-lg border border-slate-700 shadow-md transform hover:scale-110 transition cursor-help" title={badge.name}>
                         {badge.icon}
                      </div>
                   ))}
                   {displayUser.stats.badges.length === 0 && <span className="text-xs text-slate-600 italic">Henüz rozet yok.</span>}
                </div>
             </div>
          </div>

          {/* Module 11 Stats Card */}
          <div className="h-full">
             <StatsCard stats={displayUser.stats} />
          </div>
      </div>

      {/* Recent Activity Mini List */}
      <div className="bg-slate-800 rounded-xl p-5 border border-slate-700">
        <h3 className="font-bold text-white mb-4 flex items-center gap-2">
          <Clock size={18} className="text-emerald-400" /> Son Hareketlerin
        </h3>
        <div className="space-y-4">
          {myActivities.length === 0 ? (
            <div className="text-slate-500 text-sm">Henüz bir aktivite yok.</div>
          ) : (
            myActivities.slice(0, 3).map(act => (
              <div key={act.id} className="flex items-center gap-3 pb-3 border-b border-slate-700/50 last:border-0 last:pb-0">
                <div className="w-2 h-2 rounded-full bg-emerald-500"></div>
                <div className="text-sm">
                   <div className="text-slate-200">
                     {act.type === 'CHECK_IN' && 'Sinemada Check-in'}
                     {act.type === 'TICKET_BOUGHT' && 'Bilet Alımı'}
                     {act.type === 'REVIEW' && 'Film İncelemesi'}
                     {act.type === 'PLAN_CREATED' && 'Plan Oluşturuldu'}
                   </div>
                   <div className="text-xs text-slate-500">
                     {new Date(act.createdAt).toLocaleDateString()}
                   </div>
                </div>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  )};

  // Module 15 Render
  const renderPassport = () => (
     <div className="animate-in slide-in-from-bottom-4 duration-700">
        <PassportBook stamps={displayUser.passportStamps || []} user={displayUser} />
     </div>
  );

  const renderHistory = () => {
    // Filter activities related to watching movies and sort by date descending
    const historyActivities = myActivities
      .filter(a => a.type === 'CHECK_IN' || a.type === 'TICKET_BOUGHT')
      .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
    
    return (
      <div className="space-y-4 animate-in slide-in-from-right-4 duration-500">
        <h3 className="font-bold text-white mb-4 flex items-center gap-2">
           <History size={20} className="text-purple-400" /> Sinema Günlüğün
        </h3>
        
        {historyActivities.length === 0 ? (
          <div className="bg-slate-800 rounded-xl p-8 text-center border border-slate-700 border-dashed">
             <History className="mx-auto text-slate-600 mb-3" size={32} />
             <p className="text-slate-400">Henüz izlenen film kaydı yok.</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
             {historyActivities.map(act => {
                // Try to find the actual movie object to get poster
                const movie = MOCK_MOVIES.find(m => m.id === act.payload.movieId);
                
                return (
                  <div key={act.id} className="bg-slate-800 rounded-xl overflow-hidden border border-slate-700 flex group hover:border-indigo-500/50 transition">
                     <div className="w-24 h-36 bg-slate-900 flex-shrink-0 relative">
                        {movie ? (
                          <img src={movie.posterUrl} className="w-full h-full object-cover" alt="poster"/>
                        ) : (
                          <div className="w-full h-full flex items-center justify-center text-slate-600"><FilmIcon /></div>
                        )}
                        <div className="absolute inset-0 bg-black/20 group-hover:bg-transparent transition"></div>
                     </div>
                     <div className="p-4 flex flex-col justify-center">
                        <div className="text-[10px] text-indigo-400 font-bold uppercase tracking-wider mb-1">
                           {act.type === 'CHECK_IN' ? 'Check-in' : 'Bilet Alımı'}
                        </div>
                        <h4 className="font-bold text-white text-lg leading-tight mb-1">{act.payload.movieTitle || 'Bilinmeyen Film'}</h4>
                        <div className="text-xs text-slate-400 flex items-center gap-1 mb-2">
                           <Clock size={10} />
                           {new Date(act.createdAt).toLocaleDateString('tr-TR', { day: 'numeric', month: 'long', year: 'numeric' })}
                        </div>
                        <div className="text-xs text-slate-500">
                           {act.payload.theaterName}
                        </div>
                     </div>
                  </div>
                );
             })}
          </div>
        )}
      </div>
    );
  };

  const renderMusic = () => {
    // Find unique movies user has interacted with
    const seenMovieIds = new Set(myActivities.filter(a => a.type === 'CHECK_IN' || a.type === 'TICKET_BOUGHT').map(a => a.payload.movieId));
    
    // Filter Mock Movies that are seen AND have soundtracks
    const userSoundtracks = MOCK_MOVIES.filter(m => seenMovieIds.has(m.id) && m.soundtrack);

    return (
       <div className="space-y-4 animate-in slide-in-from-right-4 duration-500">
          <div className="flex items-center justify-between mb-4">
            <h3 className="font-bold text-white flex items-center gap-2">
              <Music size={20} className="text-pink-400" /> Soundtrack Koleksiyonu
            </h3>
            <span className="text-xs text-slate-400">{userSoundtracks.length} Albüm</span>
          </div>

          {userSoundtracks.length === 0 ? (
            <div className="bg-slate-800 rounded-xl p-8 text-center border border-slate-700 border-dashed">
               <Music className="mx-auto text-slate-600 mb-3" size={32} />
               <p className="text-slate-400">İzlediğin filmlerin müzikleri burada listelenir.</p>
            </div>
          ) : (
             <div className="space-y-3">
               {userSoundtracks.map(movie => (
                 <div key={movie.id} className="bg-gradient-to-r from-slate-800 to-slate-800/50 p-4 rounded-xl border border-slate-700 flex items-center justify-between group hover:border-pink-500/30 transition">
                    <div className="flex items-center gap-4">
                       <div className="w-12 h-12 rounded bg-slate-900 relative overflow-hidden shadow-lg group-hover:scale-105 transition-transform">
                          <img src={movie.posterUrl} className="w-full h-full object-cover opacity-80" alt="cover" />
                          <div className="absolute inset-0 flex items-center justify-center">
                             <PlayCircle className="text-white drop-shadow-md opacity-80" size={20} />
                          </div>
                       </div>
                       <div>
                          <div className="text-white font-bold text-sm">{movie.soundtrack?.title}</div>
                          <div className="text-slate-400 text-xs">{movie.soundtrack?.artist}</div>
                          <div className="text-indigo-400 text-[10px] mt-0.5">{movie.title} OST</div>
                       </div>
                    </div>
                    <button className="bg-slate-700 hover:bg-green-600 text-white p-2 rounded-full transition-colors group-hover:shadow-lg hover:shadow-green-900/50">
                       <ExternalLink size={16} />
                    </button>
                 </div>
               ))}
             </div>
          )}
       </div>
    );
  }

  const renderBadges = () => (
    <div className="bg-slate-800 rounded-xl p-6 border border-slate-700 animate-in zoom-in-95 duration-300">
       <div className="flex justify-between items-end mb-6">
         <div>
            <h3 className="text-lg font-bold text-white">Rozet Koleksiyonu</h3>
            <p className="text-slate-400 text-sm">Toplam {MOCK_BADGES.length} rozetten {displayUser.stats.badges.length} tanesine sahipsin.</p>
         </div>
         <div className="text-amber-400 font-mono text-xl">{displayUser.stats.xp} XP</div>
       </div>

       <div className="grid grid-cols-2 sm:grid-cols-3 gap-4">
          {MOCK_BADGES.map(badge => {
            const isUnlocked = displayUser.stats.badges.includes(badge.id);
            return (
              <div 
                key={badge.id} 
                className={`relative group p-4 rounded-xl border-2 flex flex-col items-center text-center transition-all duration-300 ${
                  isUnlocked 
                    ? 'bg-gradient-to-b from-slate-700 to-slate-800 border-amber-500/30 hover:border-amber-500 hover:shadow-lg hover:shadow-amber-900/20' 
                    : 'bg-slate-900 border-slate-800 opacity-50 grayscale'
                }`}
              >
                 <div className="text-4xl mb-3 drop-shadow-md transform group-hover:scale-110 transition duration-300">{badge.icon}</div>
                 <div className="font-bold text-slate-200 text-sm mb-1">{badge.name}</div>
                 <div className="text-[10px] text-slate-400 leading-tight">{badge.description}</div>
                 
                 {isUnlocked ? (
                   <div className="mt-3 text-[10px] font-bold bg-amber-500/20 text-amber-300 px-2 py-0.5 rounded-full">
                     KAZANILDI
                   </div>
                 ) : (
                   <div className="absolute inset-0 bg-black/40 flex items-center justify-center rounded-xl opacity-0 group-hover:opacity-100 transition backdrop-blur-[1px]">
                      <span className="text-xs font-bold text-white bg-slate-900 px-2 py-1 rounded border border-slate-700">Locked</span>
                   </div>
                 )}
              </div>
            )
          })}
       </div>
    </div>
  );

  const renderReviews = () => (
    <div className="space-y-4 animate-in slide-in-from-right-4 duration-500">
      {myReviews.length === 0 ? (
        <div className="bg-slate-800 rounded-xl p-8 text-center border border-slate-700 border-dashed">
          <Edit3 className="mx-auto text-slate-600 mb-3" size={32} />
          <p className="text-slate-400">Henüz hiç film incelemesi yapmadın.</p>
        </div>
      ) : (
        myReviews.map(review => (
          <div key={review.id} className="bg-slate-800 rounded-xl p-4 border border-slate-700 hover:border-indigo-500/30 transition">
             <div className="flex justify-between items-start mb-2">
                <div>
                  <span className="font-bold text-white text-sm block">
                    {/* Mock: Find movie name by ID */}
                    Film #{review.movieId}
                  </span>
                  <span className="text-amber-400 text-sm font-mono">★ {review.rating}</span>
                </div>
                {isOwnProfile && (
                  <button 
                    onClick={() => handleDeleteReview(review.id)} 
                    className="text-slate-500 hover:text-red-400 transition p-1 rounded hover:bg-slate-700"
                    title="Yorumu Sil"
                  >
                    <Trash2 size={14} />
                  </button>
                )}
             </div>
             <p className="text-slate-300 text-sm italic">"{review.content}"</p>
             <div className="mt-3 flex justify-between items-center text-xs text-slate-500">
                <span>{new Date(review.createdAt).toLocaleDateString()}</span>
                {review.isSpoiler && <span className="text-red-400 bg-red-900/20 px-1.5 py-0.5 rounded">Spoiler</span>}
             </div>
          </div>
        ))
      )}
    </div>
  );

  const renderSettings = () => (
    <div className="bg-slate-800 rounded-xl p-6 border border-slate-700 animate-in fade-in duration-300">
       <h3 className="font-bold text-white mb-6 flex items-center gap-2">
         <Settings size={20} className="text-slate-400" /> Hesap Ayarları
       </h3>
       
       <div className="space-y-6">
          {/* Notification Setting */}
          <div className="flex items-center justify-between group">
             <div className="flex items-center gap-3">
                <div className="bg-indigo-500/10 p-2 rounded-lg text-indigo-400 group-hover:bg-indigo-500/20 transition">
                   <Bell size={20} />
                </div>
                <div>
                  <div className="text-sm font-medium text-slate-200">Bildirimler</div>
                  <div className="text-xs text-slate-500">Bilet fiyatları düşünce haber ver</div>
                </div>
             </div>
             <button 
               onClick={() => toggleSetting('notifications')}
               className={`w-11 h-6 rounded-full relative transition-colors duration-200 ease-in-out focus:outline-none focus:ring-2 focus:ring-indigo-500/50 ${userSettings.notifications ? 'bg-indigo-600' : 'bg-slate-700'}`}
             >
                <div className={`absolute top-1 w-4 h-4 bg-white rounded-full transition-transform duration-200 ease-in-out ${userSettings.notifications ? 'left-6' : 'left-1'}`}></div>
             </button>
          </div>

          {/* Spoiler Setting */}
          <div className="flex items-center justify-between group">
             <div className="flex items-center gap-3">
                <div className="bg-amber-500/10 p-2 rounded-lg text-amber-400 group-hover:bg-amber-500/20 transition">
                   <Shield size={20} />
                </div>
                <div>
                  <div className="text-sm font-medium text-slate-200">Spoiler Koruması</div>
                  <div className="text-xs text-slate-500">İzlemediğim filmlerin yorumlarını gizle</div>
                </div>
             </div>
             <button 
               onClick={() => toggleSetting('spoilerProtection')}
               className={`w-11 h-6 rounded-full relative transition-colors duration-200 ease-in-out focus:outline-none focus:ring-2 focus:ring-indigo-500/50 ${userSettings.spoilerProtection ? 'bg-indigo-600' : 'bg-slate-700'}`}
             >
                <div className={`absolute top-1 w-4 h-4 bg-white rounded-full transition-transform duration-200 ease-in-out ${userSettings.spoilerProtection ? 'left-6' : 'left-1'}`}></div>
             </button>
          </div>

          {/* Privacy Setting */}
          <div className="flex items-center justify-between group">
             <div className="flex items-center gap-3">
                <div className="bg-emerald-500/10 p-2 rounded-lg text-emerald-400 group-hover:bg-emerald-500/20 transition">
                   <Lock size={20} />
                </div>
                <div>
                  <div className="text-sm font-medium text-slate-200">Profil Gizliliği</div>
                  <div className="text-xs text-slate-500">Aktivitelerimi sadece arkadaşlarım görsün</div>
                </div>
             </div>
             <button 
               onClick={() => toggleSetting('privacy')}
               className={`w-11 h-6 rounded-full relative transition-colors duration-200 ease-in-out focus:outline-none focus:ring-2 focus:ring-indigo-500/50 ${userSettings.privacy ? 'bg-emerald-600' : 'bg-slate-700'}`}
             >
                <div className={`absolute top-1 w-4 h-4 bg-white rounded-full transition-transform duration-200 ease-in-out ${userSettings.privacy ? 'left-6' : 'left-1'}`}></div>
             </button>
          </div>

          <div className="pt-4 border-t border-slate-700 mt-4">
             <button className="flex items-center gap-2 text-red-400 text-sm hover:text-red-300 transition px-2 py-1 rounded hover:bg-red-500/10">
                <AlertTriangle size={16} /> Hesabı Sil
             </button>
          </div>
       </div>
    </div>
  );

  return (
    <div className="w-full max-w-4xl mx-auto">
      {/* 1. Header Section */}
      <div className="relative mb-20">
         {/* Cover Image */}
         <div className="h-48 rounded-2xl bg-gradient-to-r from-indigo-900 via-purple-900 to-slate-900 overflow-hidden relative">
            <div className="absolute inset-0 bg-[url('https://www.transparenttextures.com/patterns/cubes.png')] opacity-30"></div>
            <button className="absolute top-4 right-4 bg-black/30 hover:bg-black/50 p-2 rounded-full text-white transition backdrop-blur-sm">
               <Camera size={18} />
            </button>
         </div>

         {/* Profile Info Overlay */}
         <div className="absolute -bottom-16 left-6 right-6 flex items-end justify-between">
            <div className="flex items-end gap-4">
               <div className="relative group">
                  <div className="w-32 h-32 rounded-full border-4 border-[#0f172a] bg-slate-800 overflow-hidden relative">
                     <img src={displayUser.avatarUrl} alt={displayUser.fullName} className="w-full h-full object-cover" />
                  </div>
                  {isOwnProfile && (
                    <button className="absolute bottom-1 right-1 bg-indigo-600 p-1.5 rounded-full text-white border-2 border-[#0f172a] opacity-0 group-hover:opacity-100 transition-opacity">
                       <Edit3 size={14} />
                    </button>
                  )}
               </div>
               
               <div className="mb-3">
                  {isOwnProfile && isEditing ? (
                    <div className="flex items-center gap-2">
                       <input 
                         type="text" 
                         value={editName} 
                         onChange={(e) => setEditName(e.target.value)}
                         className="bg-slate-800 border border-slate-600 text-white rounded px-2 py-1 text-lg font-bold outline-none focus:border-indigo-500 w-full sm:w-auto"
                         placeholder="Adınız Soyadınız"
                       />
                       <button onClick={handleSaveProfile} className="bg-emerald-600 p-2 rounded text-white hover:bg-emerald-500" title="Kaydet"><Check size={16}/></button>
                       <button onClick={handleCancelEdit} className="bg-slate-600 p-2 rounded text-white hover:bg-slate-500" title="İptal"><X size={16}/></button>
                    </div>
                  ) : (
                    <div className="flex items-center gap-3">
                       <h1 className="text-3xl font-black text-white drop-shadow-lg flex items-center gap-2">
                          {displayUser.fullName}
                          {isOwnProfile && <button onClick={() => setIsEditing(true)} className="text-slate-400 hover:text-white transition"><Edit3 size={16} /></button>}
                       </h1>
                       
                       {/* Add Friend Button - Only show if not own profile */}
                       {!isOwnProfile && (
                         <button 
                           onClick={handleAddFriend}
                           disabled={friendStatus !== 'none'}
                           className={`flex items-center gap-2 px-4 py-1.5 rounded-full text-xs font-bold shadow-lg transition-all ${
                             friendStatus === 'accepted' 
                               ? 'bg-emerald-600 text-white cursor-default' 
                               : friendStatus === 'pending'
                                 ? 'bg-slate-700 text-slate-300 cursor-default'
                                 : 'bg-indigo-600 hover:bg-indigo-500 text-white'
                           }`}
                         >
                           {friendStatus === 'accepted' ? (
                             <><Users size={14} /> Arkadaşsınız</>
                           ) : friendStatus === 'pending' ? (
                             <><UserCheck size={14} /> İstek Gönderildi</>
                           ) : (
                             <><UserPlus size={14} /> Arkadaş Ekle</>
                           )}
                         </button>
                       )}
                    </div>
                  )}
                  <p className="text-indigo-200 font-medium">Seviye {displayUser.stats.level} Sinefil</p>
               </div>
            </div>

            {/* Level Progress */}
            <div className="hidden sm:block w-48 mb-4 bg-slate-800/80 backdrop-blur-md p-3 rounded-xl border border-slate-700 shadow-xl">
               <div className="flex justify-between text-xs text-slate-300 mb-1">
                  <span>Sonraki Seviye</span>
                  <span>{Math.floor(calculateNextLevelProgress())}%</span>
               </div>
               <div className="w-full bg-slate-700 rounded-full h-1.5">
                  <div className="bg-gradient-to-r from-indigo-500 to-purple-500 h-1.5 rounded-full" style={{ width: `${calculateNextLevelProgress()}%` }}></div>
               </div>
            </div>
         </div>
      </div>

      {/* 2. Main Content Area */}
      <div className="grid grid-cols-1 lg:grid-cols-4 gap-8">
         {/* Left: Navigation Tabs (Vertical on Desktop maybe? sticking to horizontal for now) */}
         <div className="lg:col-span-4">
            {renderTabs()}
         </div>
         
         {/* Content Slot */}
         <div className="lg:col-span-3">
            {activeTab === 'overview' && renderOverview()}
            {activeTab === 'passport' && renderPassport()}
            {activeTab === 'history' && renderHistory()}
            {activeTab === 'music' && renderMusic()}
            {activeTab === 'badges' && renderBadges()}
            {activeTab === 'reviews' && renderReviews()}
            {activeTab === 'settings' && isOwnProfile && renderSettings()}
         </div>

         {/* Right Sidebar: Quick Friends / Info */}
         <div className="lg:col-span-1 space-y-6">
             <div className="bg-slate-800 rounded-xl p-5 border border-slate-700">
                <h4 className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-4">İstatistikler</h4>
                <div className="grid grid-cols-2 gap-4">
                   <div className="text-center p-2 bg-slate-900/50 rounded-lg">
                      <div className="text-xl font-bold text-white">{myActivities.filter(a => a.type === 'TICKET_BOUGHT').length}</div>
                      <div className="text-[10px] text-slate-500">Bilet</div>
                   </div>
                   <div className="text-center p-2 bg-slate-900/50 rounded-lg">
                      <div className="text-xl font-bold text-white">{displayUser.stats.badges.length}</div>
                      <div className="text-[10px] text-slate-500">Rozet</div>
                   </div>
                   <div className="text-center p-2 bg-slate-900/50 rounded-lg">
                      <div className="text-xl font-bold text-white">{myReviews.length}</div>
                      <div className="text-[10px] text-slate-500">İnceleme</div>
                   </div>
                   <div className="text-center p-2 bg-slate-900/50 rounded-lg">
                      <div className="text-xl font-bold text-white">2</div>
                      <div className="text-[10px] text-slate-500">Arkadaş</div>
                   </div>
                </div>
             </div>
             
             {/* Favorite Genres Tags */}
             <div className="bg-slate-800 rounded-xl p-5 border border-slate-700">
                <h4 className="text-xs font-bold text-slate-400 uppercase tracking-wider mb-3 flex justify-between items-center">
                  Favori Türler
                  {isOwnProfile && isEditing && <span className="text-[10px] text-indigo-400 normal-case">(Seçmek için tıkla)</span>}
                </h4>
                
                {isOwnProfile && isEditing ? (
                  <div className="flex flex-wrap gap-2">
                    {AVAILABLE_GENRES.map(genre => {
                      const isSelected = editGenres.includes(genre);
                      return (
                        <button 
                          key={genre}
                          onClick={() => toggleGenreSelection(genre)}
                          className={`text-xs px-2 py-1 rounded border transition-colors ${
                            isSelected 
                              ? 'bg-indigo-600 text-white border-indigo-500' 
                              : 'bg-slate-700 text-slate-400 border-slate-600 hover:bg-indigo-600/50 hover:text-white'
                          }`}
                        >
                           {genre}
                        </button>
                      )
                    })}
                  </div>
                ) : (
                  <div className="flex flex-wrap gap-2">
                     {displayUser.preferences.favoriteGenres.map(g => (
                        <span key={g} className="text-xs bg-indigo-900/40 text-indigo-300 px-2 py-1 rounded border border-indigo-500/20">
                           {g}
                        </span>
                     ))}
                     {displayUser.preferences.favoriteGenres.length === 0 && (
                       <span className="text-xs text-slate-500 italic">Henüz tür seçilmedi.</span>
                     )}
                  </div>
                )}
             </div>
         </div>
      </div>
    </div>
  );
};

// Helper for image placeholder
const FilmIcon = () => (
   <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><rect width="18" height="18" x="3" y="3" rx="2" /><path d="M7 3v18" /><path d="M3 7.5h4" /><path d="M3 12h18" /><path d="M3 16.5h4" /><path d="M17 3v18" /><path d="M17 7.5h4" /><path d="M17 16.5h4" /></svg>
);

export default UserProfile;