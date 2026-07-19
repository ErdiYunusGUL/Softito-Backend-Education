import React, { createContext, useContext, useState, ReactNode } from 'react';
import { User } from '../types';
import { CURRENT_USER } from '../constants';

interface AuthContextType {
  user: User | null;
  login: () => void; // Mock function
  logout: () => void;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  // In a real app, check localStorage or valid cookie on mount
  const [user, setUser] = useState<User | null>(CURRENT_USER);

  const login = () => {
    setUser(CURRENT_USER);
  };

  const logout = () => {
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, isAuthenticated: !!user }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};