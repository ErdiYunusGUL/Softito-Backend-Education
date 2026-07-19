import { GoogleGenAI } from "@google/genai";
import { Movie } from "../types";

const API_KEY = process.env.API_KEY || ''; 

// Helper for existing summary feature
export const getMovieAnalysis = async (movieTitle: string, genre: string[]): Promise<string> => {
  try {
    const prompt = `Şu film için Türkçe, kısa, vurucu ve eğlenceli bir özet ve "Neden İzlenmeli?" analizi yap: Film Adı: ${movieTitle}, Tür: ${genre.join(', ')}`;
    
    const response = await fetch("https://localhost:7066/api/ai/chat", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ prompt: prompt })
    });

    if (!response.ok) {
      return "Backend API'sine ulaşılamadı.";
    }

    const data = await response.json();
    return data.answer || "Özet oluşturulamadı.";
  } catch (error) {
    console.error("Backend AI Error:", error);
    return "Şu anda yapay zeka bağlantısı kurulamıyor.";
  }
};

// MODÜL 10: Semantic Search Implementation
export const semanticSearchMovies = async (query: string, availableMovies: Movie[]): Promise<string[]> => {
  try {
    const moviesContext = availableMovies.map(m => 
      `ID: ${m.id}, Title: ${m.title}, Genre: ${m.genre.join(', ')}, Desc: ${m.description}`
    ).join('\n');

    const prompt = `
      Aşağıdaki film listesinden, kullanıcının doğal dildeki arama sorgusuna en uygun olan filmleri bul.
      Mevcut Filmler: ${moviesContext}
      Kullanıcı Sorgusu: "${query}"
      Sadece eşleşen filmlerin ID'lerini JSON array olarak döndür.
    `;

    const response = await fetch("https://localhost:7066/api/ai/chat", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ prompt: prompt })
    });

    const data = await response.json();
    const text = data.answer;
    
    // Güvenli JSON ayıklaması (Backend bazen düz metin ile birlikte kod bloğu döndürebilir)
    const jsonMatch = text.match(/\[.*\]/s);
    if (jsonMatch) {
        const resultIds = JSON.parse(jsonMatch[0]);
        return Array.isArray(resultIds) ? resultIds : [];
    }
    return [];
  } catch (error) {
    console.error("Semantic Search Error:", error);
    return [];
  }
};