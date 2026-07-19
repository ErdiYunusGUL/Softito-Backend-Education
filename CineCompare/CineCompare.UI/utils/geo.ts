// C. ALGORİTMA MANTIĞI (Backend Logic Simulation)

/**
 * Calculates the great-circle distance between two points on the Earth's surface.
 * Uses the Haversine formula.
 * @param lat1 User Latitude
 * @param lon1 User Longitude
 * @param lat2 Theater Latitude
 * @param lon2 Theater Longitude
 * @returns Distance in Kilometers (km)
 */
export const calculateDistance = (lat1: number, lon1: number, lat2: number, lon2: number): number => {
  const R = 6371; // Earth's radius in km
  const dLat = toRad(lat2 - lat1);
  const dLon = toRad(lon2 - lon1);
  const a =
    Math.sin(dLat / 2) * Math.sin(dLat / 2) +
    Math.cos(toRad(lat1)) * Math.cos(toRad(lat2)) *
    Math.sin(dLon / 2) * Math.sin(dLon / 2);
  const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
  const d = R * c;
  return parseFloat(d.toFixed(1)); // Return rounded to 1 decimal place
};

const toRad = (value: number): number => {
  return (value * Math.PI) / 180;
};