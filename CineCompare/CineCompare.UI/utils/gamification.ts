// MODÜL 8 & 15: Gamification Logic & Event-Driven Architecture

import { User } from '../types';

type EventType = 'TICKET_PURCHASED' | 'CHECK_IN' | 'REVIEW_ADDED';

interface GameEvent {
  type: EventType;
  userId: string;
  payload?: any;
}

type Listener = (event: GameEvent) => void;

class GamificationEngine {
  private listeners: Listener[] = [];

  // Register a listener
  subscribe(listener: Listener) {
    this.listeners.push(listener);
    return () => {
      this.listeners = this.listeners.filter(l => l !== listener);
    };
  }

  // Dispatch an event
  emit(type: EventType, userId: string, payload?: any) {
    const event: GameEvent = { type, userId, payload };
    this.listeners.forEach(l => l(event));
    
    // Simulate Backend "Observer" Logic for Module 15 (Passport)
    if (type === 'CHECK_IN' && payload && payload.theaterId && payload.theaterName) {
       this.handleCheckIn(userId, payload.theaterId, payload.theaterName);
    }
  }

  // --- MODULE 15: Backend Logic Simulation ---
  // In a real backend (e.g., Laravel), this would be an Event Listener like `CheckPassportMilestones`.
  private handleCheckIn(userId: string, theaterId: string, theaterName: string) {
    console.log(`[Backend Logic] Processing Check-in for user ${userId} at ${theaterName}`);
    
    // 1. Fetch User (Simulated)
    // const user = await User.find(userId);
    
    // 2. Check if theater is already stamped
    // const hasStamp = user.passportStamps.some(s => s.theaterId === theaterId);
    
    // 3. If not, add stamp
    // if (!hasStamp) {
    //    user.passportStamps.push({ 
    //       theaterId, 
    //       theaterName, 
    //       visitedAt: new Date().toISOString() 
    //    });
    //    await user.save();
    //    NotificationService.send(userId, "Pasaportuna yeni bir damga eklendi!");
    // }
    
    // 4. Check for 'Explorer' Badge (5 Unique Theaters)
    // if (user.passportStamps.length >= 5 && !user.badges.includes('explorer')) {
    //    user.badges.push('explorer');
    //    await user.save();
    //    NotificationService.send(userId, "Tebrikler! 'Kaşif' rozetini kazandın!");
    // }
  }
}

export const gameEngine = new GamificationEngine();

// Simulate XP Gain Logic
export const calculateXPGain = (event: EventType): number => {
  switch (event) {
    case 'TICKET_PURCHASED': return 100;
    case 'CHECK_IN': return 50;
    case 'REVIEW_ADDED': return 75;
    default: return 0;
  }
};