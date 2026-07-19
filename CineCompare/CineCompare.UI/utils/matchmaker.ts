// MODÜL 9: Matchmaking Algorithm
import { User, Theater } from '../types';
import { calculateDistance } from './geo';

interface MatchResult {
  score: number; // 0-100
  commonGenres: string[];
  recommendedTheaterId: string | null;
  midpoint: { lat: number, lng: number };
}

export const findMatch = (userA: User, userB: User, theaters: Theater[]): MatchResult => {
  
  // 1. Genre Matching (Intersection)
  const genresA = new Set(userA.preferences.favoriteGenres);
  const commonGenres = userB.preferences.favoriteGenres.filter(g => genresA.has(g));

  // 2. Geographic Midpoint Calculation
  const locA = userA.preferences.defaultLocation;
  const locB = userB.preferences.defaultLocation;
  
  // Default to Istanbul center if locations missing
  const latA = locA?.lat || 41.0082;
  const lngA = locA?.lng || 28.9784;
  const latB = locB?.lat || 41.0082;
  const lngB = locB?.lng || 28.9784;

  const midLat = (latA + latB) / 2;
  const midLng = (lngA + lngB) / 2;

  // 3. Find Best Theater (Sort by distance to midpoint)
  // Clone array to sort
  const sortedTheaters = [...theaters].sort((a, b) => {
    const distA = calculateDistance(midLat, midLng, a.latitude, a.longitude);
    const distB = calculateDistance(midLat, midLng, b.latitude, b.longitude);
    return distA - distB;
  });

  const bestTheater = sortedTheaters[0];

  // Calculate generic score
  let score = 50; // Base
  if (commonGenres.length > 0) score += 30;
  if (bestTheater) score += 20;

  return {
    score,
    commonGenres,
    recommendedTheaterId: bestTheater ? bestTheater.id : null,
    midpoint: { lat: midLat, lng: midLng }
  };
};