// A. VERİTABANI ŞEMASI MANTIĞI (TypeScript Interfaces olarak)

// 0. Cast Member
export interface CastMember {
  id: string;
  name: string;
  role: string;
  imageUrl: string;
}

// 1. Movies (Filmler)
export interface Movie {
  id: string;
  title: string;
  posterUrl: string;
  backdropUrl: string;
  genre: string[];
  durationMinutes: number;
  rating: number;
  releaseDate: string;
  description: string;
  // NEW: Cast & Crew
  director: string;
  cast: CastMember[];
  
  // MODÜL: Soundtrack Integration
  soundtrack?: {
    title: string;
    artist: string;
    spotifyUrl: string;
  };
  // MODÜL 14: After-Credits Info
  creditScenes?: {
    hasMid: boolean;      // Jenerik ortasında sahne var mı?
    hasPost: boolean;     // Jenerik sonunda sahne var mı?
    description: string;  // Sahne ne hakkında? (Spoiler)
    confidenceScore: number; // % doğruluk oranı (Kullanıcı oyları)
  };
}

// 2. Theaters (Sinemalar)
export interface Theater {
  id: string;
  name: string;
  address: string;
  latitude: number;
  longitude: number;
  facilities: string[];
}

// 3. Showtimes (Seanslar) - Updated for Module 1 & 3
export interface Showtime {
  id: string;
  movieId: string;
  theaterId: string;
  time: string; // "14:30"
  date: string; // "2023-10-27"
  
  // MODÜL 3: Deep Linking
  ticketUrl: string; 
  
  // MODÜL 1: Attributes (JSONB mapping)
  attributes: string[]; 
}

// 4. Pricing (Fiyatlandırma)
export interface Pricing {
  movieId: string;
  theaterId: string;
  amount: number;
  currency: string;
}

// MODÜL 4: Social Plan Types
export interface SocialPlan {
  id: string; // UUID
  movieTitle: string;
  theaterName: string;
  showtime: string;
  votes: { user: string; status: 'join' | 'decline' | 'maybe' }[];
}

// MODÜL 8: Gamification
export interface Badge {
  id: string;
  slug: string;
  name: string;
  icon: string; // Emoji or URL
  description: string;
  xpValue: number;
}

// MODÜL 11: Stats (Cinema Wrapped)
export interface GenreStat {
  genre: string;
  count: number;
  percentage: number;
}

// MODÜL 15: Passport Stamp
export interface PassportStamp {
  theaterId: string;
  theaterName: string;
  visitedAt: string; // ISO Date
}

export interface UserStats {
  xp: number;
  level: number;
  badges: string[]; // Array of Badge IDs
  totalMinutesWatched: number; // Module 11
  favoriteGenres: GenreStat[]; // Module 11
}

// MODÜL 5: User Profile
export interface User {
  id: string;
  fullName: string;
  avatarUrl: string;
  email: string;
  preferences: {
    favoriteGenres: string[];
    defaultLocation?: { lat: number; lng: number };
  };
  stats: UserStats; 
  // MODÜL 15: Cinephile Passport
  passportStamps: PassportStamp[];
}

// MODÜL 6: Friendships
export interface Friendship {
  requesterId: string;
  addresseeId: string;
  status: 'pending' | 'accepted' | 'blocked';
}

// MODÜL 7: Activity Feed
export interface Activity {
  id: string;
  userId: string;
  type: 'CHECK_IN' | 'TICKET_BOUGHT' | 'PLAN_CREATED' | 'REVIEW';
  payload: {
    movieId?: string; // Added for spoiler check logic
    movieTitle?: string;
    theaterName?: string;
    showtime?: string;
    rating?: number;
    comment?: string;
  };
  createdAt: string; // ISO Date
}

// MODÜL 12: Review System
export interface Review {
  id: string;
  movieId: string;
  userId: string;
  userName: string;
  userAvatar: string;
  rating: number;
  content: string;
  isSpoiler: boolean;
  createdAt: string;
}

// UI State Types
export type SortOption = 'nearest' | 'cheapest' | 'default';

export interface LocationState {
  lat: number | null;
  lng: number | null;
  error: string | null;
}

export interface TheaterWithDetails extends Theater {
  distance?: number;
  price: number;
  showtimes: Showtime[];
}