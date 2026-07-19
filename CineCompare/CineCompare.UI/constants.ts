import { Movie, Theater, Pricing, Showtime, User, Activity, Friendship, Badge, Review } from './types';

// Mock Data simulating a backend database

export const MOCK_MOVIES: Movie[] = [
  {
    id: 'm1',
    title: 'Dune: Part Two',
    posterUrl: 'https://image.tmdb.org/t/p/w500/1pdfLvkbY9ohJlCjQH2CZjjYVvJ.jpg',
    backdropUrl: 'https://image.tmdb.org/t/p/original/xOMo8BRK7PfcJv9JCnx7s5hj0PX.jpg',
    genre: ['Bilim Kurgu', 'Macera'],
    durationMinutes: 166,
    rating: 8.9,
    releaseDate: '2024-03-01',
    description: 'Paul Atreides, ailesini yok eden komploculara karşı bir intikam savaşında Chani ve Fremenlerle birleşiyor. Hüzünlü, destansı ve görsel bir şölen.',
    director: 'Denis Villeneuve',
    cast: [
      { id: 'c1', name: 'Timothée Chalamet', role: 'Paul Atreides', imageUrl: 'https://image.tmdb.org/t/p/w200/BE2sdjpgEHRr9wbc1ZChYX99IA.jpg' },
      { id: 'c2', name: 'Zendaya', role: 'Chani', imageUrl: 'https://image.tmdb.org/t/p/w200/cbCibOAkn54qjfltL3J4onjYtDA.jpg' },
      { id: 'c3', name: 'Rebecca Ferguson', role: 'Lady Jessica', imageUrl: 'https://image.tmdb.org/t/p/w200/lJloTOheuQSurIYgwBDWXkJ9g53.jpg' },
      { id: 'c4', name: 'Javier Bardem', role: 'Stilgar', imageUrl: 'https://image.tmdb.org/t/p/w200/gJdD7Fm3l5J94B733056e48.jpg' },
    ],
    soundtrack: {
      title: 'Dune: Part Two (Original Motion Picture Soundtrack)',
      artist: 'Hans Zimmer',
      spotifyUrl: '#'
    },
    // MODULE 14 Data
    creditScenes: {
      hasMid: false,
      hasPost: false,
      description: 'Dune: Part Two filminde jenerik sonrası sahne bulunmamaktadır. Film bittiğinde ışıklar açılabilir.',
      confidenceScore: 98
    }
  },
  {
    id: 'm2',
    title: 'Oppenheimer',
    posterUrl: 'https://image.tmdb.org/t/p/w500/8Gxv8gSFCU0XGDykEGv7zR1n2ua.jpg',
    backdropUrl: 'https://image.tmdb.org/t/p/original/fm6KqXpk3M2HVveHwCrBSSBaO0V.jpg',
    genre: ['Biyografi', 'Dram', 'Tarih'],
    durationMinutes: 180,
    rating: 8.4,
    releaseDate: '2023-07-21',
    description: 'Amerikalı bilim adamı J. Robert Oppenheimer ve atom bombasının geliştirilmesindeki rolünün hikayesi. Karmaşık, gerilimli ve tarihi.',
    director: 'Christopher Nolan',
    cast: [
      { id: 'c5', name: 'Cillian Murphy', role: 'J. Robert Oppenheimer', imageUrl: 'https://image.tmdb.org/t/p/w200/2l3j785aJj1w8z4a6v756.jpg' },
      { id: 'c6', name: 'Emily Blunt', role: 'Kitty Oppenheimer', imageUrl: 'https://image.tmdb.org/t/p/w200/nPJXaRMGD19.jpg' },
      { id: 'c7', name: 'Matt Damon', role: 'Leslie Groves', imageUrl: 'https://image.tmdb.org/t/p/w200/elSlNgV8xV998C.jpg' },
    ],
    soundtrack: {
      title: 'Oppenheimer (Original Motion Picture Soundtrack)',
      artist: 'Ludwig Göransson',
      spotifyUrl: '#'
    },
    creditScenes: {
      hasMid: false,
      hasPost: false,
      description: 'Christopher Nolan filmlerinde genellikle jenerik sonrası sahne olmaz.',
      confidenceScore: 99
    }
  },
  {
    id: 'm3',
    title: 'Kung Fu Panda 4',
    posterUrl: 'https://image.tmdb.org/t/p/w500/kDp1vUBnMpe8ak4rjgl3cLELqjU.jpg',
    backdropUrl: 'https://image.tmdb.org/t/p/original/1XDDXPXGiI8id7MrUxK36ke7gkX.jpg',
    genre: ['Animasyon', 'Aksiyon', 'Komedi'],
    durationMinutes: 94,
    rating: 7.6,
    releaseDate: '2024-03-08',
    description: 'Po, Barış Vadisi\'nin Ruhani Lideri olmaya hazırlanırken, yeni bir düşman olan Bukalemun ile karşı karşıya kalır. Eğlenceli ve aile dostu.',
    director: 'Mike Mitchell',
    cast: [
      { id: 'c8', name: 'Jack Black', role: 'Po (Voice)', imageUrl: 'https://image.tmdb.org/t/p/w200/rtCx0fiYx1T.jpg' },
      { id: 'c9', name: 'Awkwafina', role: 'Zhen (Voice)', imageUrl: 'https://image.tmdb.org/t/p/w200/l5AKkg0H1j.jpg' },
    ],
    soundtrack: {
      title: 'Baby One More Time (Cover)',
      artist: 'Tenacious D',
      spotifyUrl: '#'
    },
    creditScenes: {
      hasMid: true,
      hasPost: false,
      description: 'Jeneriğin hemen başında karakterlerin eğlenceli çizimleriyle süslenmiş kısa bir animasyon sekansı var.',
      confidenceScore: 95
    }
  }
];

export const MOCK_UPCOMING_MOVIES: Movie[] = [
  {
    id: 'u1',
    title: 'Joker: Folie à Deux',
    posterUrl: 'https://image.tmdb.org/t/p/w500/aciP8Km0waTLXEYf5ybXC5ErFyd.jpg',
    backdropUrl: 'https://image.tmdb.org/t/p/original/uQ17M939b4fM1Fv8g44mH1XF5Ld.jpg',
    genre: ['Dram', 'Suç', 'Müzikal'],
    durationMinutes: 138,
    rating: 0, // Not rated yet
    releaseDate: '2024-10-04',
    description: 'Joker, Arkham Akıl Hastanesi\'nde hapsedilirken hayatının aşkı Harley Quinn ile tanışır. Delilik artık iki kişiliktir.',
    director: 'Todd Phillips',
    cast: [
      { id: 'c10', name: 'Joaquin Phoenix', role: 'Arthur Fleck / Joker', imageUrl: 'https://image.tmdb.org/t/p/w200/nXMzvVF6v.jpg' },
      { id: 'c11', name: 'Lady Gaga', role: 'Harley Quinn', imageUrl: 'https://image.tmdb.org/t/p/w200/asB5x46Hh.jpg' },
    ]
  },
  {
    id: 'u2',
    title: 'Gladiator 2',
    posterUrl: 'https://image.tmdb.org/t/p/w500/p40eF8684xXU1yJvYQv1lM1c1d.jpg', // Placeholder logic
    backdropUrl: 'https://image.tmdb.org/t/p/original/zFbFwxF4XQf1B5z5r3r3x5x5x5.jpg', // Placeholder
    genre: ['Aksiyon', 'Macera', 'Dram'],
    durationMinutes: 150,
    rating: 0,
    releaseDate: '2024-11-22',
    description: 'Maximus\'un ölümünden yıllar sonra, Lucius arenaya girmek zorunda kalır. Roma İmparatorluğu\'nun kaderi bir kez daha belirlenecek.',
    director: 'Ridley Scott',
    cast: [
      { id: 'c12', name: 'Paul Mescal', role: 'Lucius', imageUrl: '' },
      { id: 'c13', name: 'Denzel Washington', role: 'Macrinus', imageUrl: '' },
    ]
  },
  {
    id: 'u3',
    title: 'Mickey 17',
    posterUrl: 'https://image.tmdb.org/t/p/w500/c35M4P4v4b4x4x4x4x4x4x4x4x4.jpg', // Placeholder
    backdropUrl: 'https://image.tmdb.org/t/p/original/b9UCfdzD83nFHKhaSf4n7m3L6i.jpg', // Placeholder
    genre: ['Bilim Kurgu', 'Dram'],
    durationMinutes: 139,
    rating: 0,
    releaseDate: '2025-01-31',
    description: 'Bong Joon-ho\'dan (Parasite) yeni bir bilim kurgu. İnsanlığın geleceği için tehlikeli görevlere gönderilen "harcanabilir" bir çalışanın hikayesi.',
    director: 'Bong Joon-ho',
    cast: [
      { id: 'c14', name: 'Robert Pattinson', role: 'Mickey Barnes', imageUrl: '' },
      { id: 'c15', name: 'Mark Ruffalo', role: 'Hieronymous Marshall', imageUrl: '' },
    ]
  }
];

export const MOCK_THEATERS: Theater[] = [
  {
    id: 't1',
    name: 'Cinemaximum Zorlu Center',
    address: 'Levazım, Koru Sokağı No:2, 34340 Beşiktaş/İstanbul',
    latitude: 41.0660,
    longitude: 29.0173,
    facilities: ['IMAX', 'Gold Class']
  },
  {
    id: 't2',
    name: 'Paribu Cineverse Kanyon',
    address: 'Büyükdere Cd. No:185, 34394 Şişli/İstanbul',
    latitude: 41.0784,
    longitude: 29.0128,
    facilities: ['Dolby Atmos']
  },
  {
    id: 't3',
    name: 'Cinema Pink Beyoğlu',
    address: 'İstiklal Cd. No:20, 34435 Beyoğlu/İstanbul',
    latitude: 41.0335,
    longitude: 28.9778,
    facilities: ['2D']
  },
  {
    id: 't4',
    name: 'Avşar Sinemaları Kadıköy',
    address: 'Caferağa, Bahariye Cd. No:30, 34710 Kadıköy/İstanbul',
    latitude: 40.9890,
    longitude: 29.0260,
    facilities: ['2D', '3D']
  }
];

export const MOCK_PRICING: Pricing[] = [
  { movieId: 'm1', theaterId: 't1', amount: 250, currency: 'TRY' }, 
  { movieId: 'm1', theaterId: 't2', amount: 220, currency: 'TRY' },
  { movieId: 'm1', theaterId: 't3', amount: 150, currency: 'TRY' },
  { movieId: 'm1', theaterId: 't4', amount: 180, currency: 'TRY' },
];

export const MOCK_SHOWTIMES: Showtime[] = [
  { 
    id: 's1', movieId: 'm1', theaterId: 't1', time: '13:00', date: '2024-05-20',
    attributes: ['IMAX', '3D', 'SUB'],
    ticketUrl: 'https://www.paribucineverse.com'
  },
  { 
    id: 's2', movieId: 'm1', theaterId: 't1', time: '16:30', date: '2024-05-20',
    attributes: ['IMAX', '2D', 'DUB'],
    ticketUrl: 'https://www.paribucineverse.com'
  },
  { 
    id: 's3', movieId: 'm1', theaterId: 't1', time: '20:00', date: '2024-05-20',
    attributes: ['Gold Class', '2D', 'SUB'],
    ticketUrl: 'https://www.paribucineverse.com'
  },
  { 
    id: 's4', movieId: 'm1', theaterId: 't2', time: '14:15', date: '2024-05-20',
    attributes: ['2D', 'SUB'],
    ticketUrl: 'https://www.paribucineverse.com'
  },
  { 
    id: 's5', movieId: 'm1', theaterId: 't2', time: '17:45', date: '2024-05-20',
    attributes: ['Dolby Atmos', '2D', 'SUB'],
    ticketUrl: 'https://www.paribucineverse.com'
  },
  { 
    id: 's6', movieId: 'm1', theaterId: 't3', time: '12:00', date: '2024-05-20',
    attributes: ['2D', 'DUB'],
    ticketUrl: 'https://www.cinemapink.com'
  },
  { 
    id: 's7', movieId: 'm1', theaterId: 't3', time: '15:00', date: '2024-05-20',
    attributes: ['2D', 'SUB'],
    ticketUrl: 'https://www.cinemapink.com'
  },
  { 
    id: 's9', movieId: 'm1', theaterId: 't4', time: '19:30', date: '2024-05-20',
    attributes: ['3D', 'SUB'],
    ticketUrl: 'https://www.avsar.com.tr'
  },
];

export const MOCK_BADGES: Badge[] = [
  { id: 'b1', slug: 'explorer', name: 'Kaşif', icon: '🧭', description: '5 farklı sinemada film izle.', xpValue: 100 },
  { id: 'b2', slug: 'critic', name: 'Eleştirmen', icon: '✍️', description: 'İlk yorumunu yaz.', xpValue: 50 },
  { id: 'b3', slug: 'scifi-geek', name: 'Bilim Kurgu Uzmanı', icon: '👽', description: '3 Bilim Kurgu filmi izle.', xpValue: 200 },
];

export const CURRENT_USER: User = {
  id: 'u1',
  fullName: 'Sen',
  email: 'ben@cinescope.app',
  avatarUrl: 'https://i.pravatar.cc/150?u=u1',
  preferences: {
    favoriteGenres: ['Bilim Kurgu', 'Gerilim'],
    defaultLocation: { lat: 41.0660, lng: 29.0173 }
  },
  stats: {
    xp: 340,
    level: 3,
    badges: ['b1', 'b3'],
    totalMinutesWatched: 840, // ~14 hours
    favoriteGenres: [
       { genre: 'Bilim Kurgu', count: 4, percentage: 50 },
       { genre: 'Dram', count: 2, percentage: 25 },
       { genre: 'Aksiyon', count: 2, percentage: 25 },
    ]
  },
  passportStamps: [
    { theaterId: 't1', theaterName: 'Cinemaximum Zorlu', visitedAt: '2023-12-15T20:00:00Z' },
    { theaterId: 't2', theaterName: 'Paribu Kanyon', visitedAt: '2024-01-20T18:30:00Z' }
  ]
};

export const MOCK_USERS: User[] = [
  CURRENT_USER,
  {
    id: 'u2',
    fullName: 'Ahmet Yılmaz',
    email: 'ahmet@test.com',
    avatarUrl: 'https://i.pravatar.cc/150?u=u2',
    preferences: { 
      favoriteGenres: ['Komedi', 'Bilim Kurgu'],
      defaultLocation: { lat: 40.9890, lng: 29.0260 }
    },
    stats: { xp: 120, level: 1, badges: [], totalMinutesWatched: 120, favoriteGenres: [] },
    passportStamps: []
  },
  {
    id: 'u3',
    fullName: 'Zeynep Kaya',
    email: 'zeynep@test.com',
    avatarUrl: 'https://i.pravatar.cc/150?u=u3',
    preferences: { 
      favoriteGenres: ['Dram', 'Sanat'],
      defaultLocation: { lat: 41.0335, lng: 28.9778 }
    },
    stats: { xp: 850, level: 8, badges: ['b2'], totalMinutesWatched: 2000, favoriteGenres: [] },
    passportStamps: []
  },
  {
    id: 'u4',
    fullName: 'Can Demir',
    email: 'can@test.com',
    avatarUrl: 'https://i.pravatar.cc/150?u=u4',
    preferences: { 
      favoriteGenres: ['Aksiyon'],
      defaultLocation: { lat: 41.0082, lng: 28.9784 }
    },
    stats: { xp: 50, level: 1, badges: [], totalMinutesWatched: 90, favoriteGenres: [] },
    passportStamps: []
  }
];

export const MOCK_FRIENDSHIPS: Friendship[] = [
  { requesterId: 'u1', addresseeId: 'u2', status: 'accepted' }, 
  { requesterId: 'u3', addresseeId: 'u1', status: 'accepted' }, 
  { requesterId: 'u4', addresseeId: 'u1', status: 'pending' },  
];

export const MOCK_ACTIVITIES: Activity[] = [
  {
    id: 'a1',
    userId: 'u2', 
    type: 'CHECK_IN',
    payload: {
      movieId: 'm1',
      movieTitle: 'Dune: Part Two',
      theaterName: 'Cinemaximum Zorlu',
      showtime: '21:00'
    },
    createdAt: new Date(Date.now() - 1000 * 60 * 30).toISOString()
  },
  {
    id: 'a2',
    userId: 'u3', 
    type: 'REVIEW',
    payload: {
      movieId: 'm2',
      movieTitle: 'Oppenheimer',
      rating: 5,
      comment: 'Mükemmel bir başyapıt! Kesinlikle IMAX izlenmeli.'
    },
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 2).toISOString() 
  },
  {
    id: 'a3',
    userId: 'u2', 
    type: 'PLAN_CREATED',
    payload: {
      movieId: 'm3',
      movieTitle: 'Kung Fu Panda 4',
      theaterName: 'Paribu Cineverse Kanyon',
      showtime: 'Cumartesi 14:00'
    },
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 5).toISOString() 
  }
];

// --- MODULE 12: MOCK REVIEWS ---
export const MOCK_REVIEWS: Review[] = [
  {
    id: 'r1',
    movieId: 'm1',
    userId: 'u3',
    userName: 'Zeynep Kaya',
    userAvatar: 'https://i.pravatar.cc/150?u=u3',
    rating: 5,
    content: 'Müzikler ve görsellik inanılmazdı. Özellikle son savaş sahnesi nefes kesiciydi!',
    isSpoiler: false,
    createdAt: '2024-05-19T10:00:00Z'
  },
  {
    id: 'r2',
    movieId: 'm1',
    userId: 'u2',
    userName: 'Ahmet Yılmaz',
    userAvatar: 'https://i.pravatar.cc/150?u=u2',
    rating: 4,
    content: 'Kitabı okuyanlar için bazı değişiklikler şaşırtıcı olabilir. Paul’un finaldeki kararı beni çok etkiledi.',
    isSpoiler: true,
    createdAt: '2024-05-18T14:30:00Z'
  },
  {
    id: 'r3',
    movieId: 'm1',
    userId: 'u4',
    userName: 'Can Demir',
    userAvatar: 'https://i.pravatar.cc/150?u=u4',
    rating: 5,
    content: 'Sinemada izlediğim en iyi deneyimlerden biriydi. IMAX şart.',
    isSpoiler: false,
    createdAt: '2024-05-21T18:15:00Z'
  }
];