using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using PaniniDapperApp.Models;

namespace PaniniDapperApp.Repositories
{
    public class StickerRepository
    {
        private readonly string _connectionString;

        public StickerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        // --- AUTHENTICATION ---
        public async Task<Collector?> AuthenticateAsync(string username, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<Collector>(
                "SELECT * FROM Collectors WHERE Username = @Username AND Password = @Password",
                new { Username = username, Password = password });
        }

        // --- ALBUM ---
        public async Task<IEnumerable<AlbumItem>> GetMyAlbumAsync(int collectorId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                SELECT a.Id, a.CollectorId, a.StickerId, a.Quantity,
                       s.Id, s.Number, s.PlayerName, s.TeamId,
                       t.Id, t.CountryName
                FROM AlbumItems a
                INNER JOIN Stickers s ON a.StickerId = s.Id
                INNER JOIN Teams t ON s.TeamId = t.Id
                WHERE a.CollectorId = @CollectorId
                ORDER BY t.Id, s.Number";
            
            return await connection.QueryAsync<AlbumItem, Sticker, Team, AlbumItem>(
                query, (a, s, t) => { s.Team = t; a.Sticker = s; return a; },
                new { CollectorId = collectorId }, splitOn: "Id,Id"
            );
        }

        public async Task<IEnumerable<Sticker>> GetMissingStickersAsync(int collectorId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                SELECT s.Id, s.Number, s.PlayerName, s.TeamId,
                       t.Id, t.CountryName
                FROM Stickers s
                INNER JOIN Teams t ON s.TeamId = t.Id
                WHERE s.Id NOT IN (SELECT StickerId FROM AlbumItems WHERE CollectorId = @CollectorId AND Quantity > 0)
                ORDER BY t.Id, s.Number";
            
            return await connection.QueryAsync<Sticker, Team, Sticker>(
                query, (s, t) => { s.Team = t; return s; },
                new { CollectorId = collectorId }, splitOn: "Id"
            );
        }

        // --- MARKET (LISTINGS) ---
        public async Task<IEnumerable<TradeListing>> GetMarketListingsAsync(int excludeCollectorId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                SELECT tl.Id, tl.CollectorId, tl.StickerId, tl.Status, tl.CreatedDate,
                       c.Id, c.Username,
                       s.Id, s.Number, s.PlayerName, s.TeamId, t.Id, t.CountryName
                FROM TradeListings tl
                INNER JOIN Collectors c ON tl.CollectorId = c.Id
                INNER JOIN Stickers s ON tl.StickerId = s.Id
                INNER JOIN Teams t ON s.TeamId = t.Id
                WHERE tl.Status = 'Open' AND tl.CollectorId != @ExcludeId
                ORDER BY tl.CreatedDate DESC";
            
            return await connection.QueryAsync<TradeListing, Collector, Sticker, Team, TradeListing>(
                query, (tl, c, s, t) => { tl.Collector = c; s.Team = t; tl.Sticker = s; return tl; },
                new { ExcludeId = excludeCollectorId }, splitOn: "Id,Id,Id"
            );
        }

        public async Task<IEnumerable<TradeListing>> GetMyListingsAsync(int collectorId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                SELECT tl.Id, tl.CollectorId, tl.StickerId, tl.Status, tl.CreatedDate,
                       s.Id, s.Number, s.PlayerName, s.TeamId, t.Id, t.CountryName
                FROM TradeListings tl
                INNER JOIN Stickers s ON tl.StickerId = s.Id
                INNER JOIN Teams t ON s.TeamId = t.Id
                WHERE tl.CollectorId = @CId
                ORDER BY tl.CreatedDate DESC";
            
            return await connection.QueryAsync<TradeListing, Sticker, Team, TradeListing>(
                query, (tl, s, t) => { s.Team = t; tl.Sticker = s; return tl; },
                new { CId = collectorId }, splitOn: "Id,Id"
            );
        }

        public async Task CreateListingAsync(int collectorId, int stickerId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "INSERT INTO TradeListings (CollectorId, StickerId) VALUES (@CId, @SId)",
                new { CId = collectorId, SId = stickerId });
        }
        
        public async Task RemoveListingAsync(int listingId, int collectorId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE TradeListings SET Status = 'Cancelled' WHERE Id = @LId AND CollectorId = @CId",
                new { LId = listingId, CId = collectorId });
        }

        // --- OFFERS ---
        public async Task MakeOfferAsync(int listingId, int offeredByCollectorId, int offeredStickerId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "INSERT INTO TradeOffers (ListingId, OfferedByCollectorId, OfferedStickerId) VALUES (@LId, @CId, @SId)",
                new { LId = listingId, CId = offeredByCollectorId, SId = offeredStickerId });
        }

        public async Task<IEnumerable<TradeOffer>> GetOffersForMyListingsAsync(int collectorId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
                SELECT o.Id, o.ListingId, o.OfferedByCollectorId, o.OfferedStickerId, o.Status, o.CreatedDate,
                       l.Id, l.CollectorId, l.StickerId, l.Status, l.CreatedDate,
                       ls.Id, ls.Number, ls.PlayerName,
                       c.Id, c.Username,
                       os.Id, os.Number, os.PlayerName, ot.Id, ot.CountryName
                FROM TradeOffers o
                INNER JOIN TradeListings l ON o.ListingId = l.Id
                INNER JOIN Stickers ls ON l.StickerId = ls.Id
                INNER JOIN Collectors c ON o.OfferedByCollectorId = c.Id
                INNER JOIN Stickers os ON o.OfferedStickerId = os.Id
                INNER JOIN Teams ot ON os.TeamId = ot.Id
                WHERE l.CollectorId = @CId AND o.Status = 'Pending'
                ORDER BY o.CreatedDate DESC";
                
            return await connection.QueryAsync<TradeOffer, TradeListing, Sticker, Collector, Sticker, Team, TradeOffer>(
                query, (o, l, ls, c, os, ot) => {
                    l.Sticker = ls; o.Listing = l; o.OfferedBy = c; os.Team = ot; o.OfferedSticker = os; return o;
                },
                new { CId = collectorId }, splitOn: "Id,Id,Id,Id,Id"
            );
        }

        public async Task RejectOfferAsync(int offerId, int listingOwnerId)
        {
            using var connection = new SqlConnection(_connectionString);
            // Verify ownership implicitly
            var query = @"
                UPDATE TradeOffers SET Status = 'Rejected' 
                FROM TradeOffers o INNER JOIN TradeListings l ON o.ListingId = l.Id
                WHERE o.Id = @OId AND l.CollectorId = @CId";
            await connection.ExecuteAsync(query, new { OId = offerId, CId = listingOwnerId });
        }

        public async Task AcceptOfferAsync(int offerId, int listingOwnerId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                var offer = await connection.QuerySingleOrDefaultAsync<TradeOffer>(
                    @"SELECT o.*, l.CollectorId as ListingOwnerId, l.StickerId as ListingStickerId 
                      FROM TradeOffers o INNER JOIN TradeListings l ON o.ListingId = l.Id 
                      WHERE o.Id = @OId", 
                    new { OId = offerId }, transaction);
                
                if (offer == null || offer.Status != "Pending") throw new Exception("Invalid offer");

                // Execute Swap
                int userA = listingOwnerId;
                int userB = offer.OfferedByCollectorId;
                int stickerA = connection.QuerySingle<int>("SELECT StickerId FROM TradeListings WHERE Id=@Id", new {Id=offer.ListingId}, transaction);
                int stickerB = offer.OfferedStickerId;

                async Task ModifyQuantity(int uId, int sId, int diff)
                {
                    var item = await connection.QuerySingleOrDefaultAsync<AlbumItem>(
                        "SELECT * FROM AlbumItems WHERE CollectorId = @CId AND StickerId = @SId",
                        new { CId = uId, SId = sId }, transaction);

                    if (item == null) {
                        if (diff > 0) await connection.ExecuteAsync("INSERT INTO AlbumItems (CollectorId, StickerId, Quantity) VALUES (@CId, @SId, @Qty)", new { CId = uId, SId = sId, Qty = diff }, transaction);
                    } else {
                        int newQty = item.Quantity + diff;
                        if (newQty < 0) throw new Exception("Not enough stickers");
                        await connection.ExecuteAsync("UPDATE AlbumItems SET Quantity = @Qty WHERE Id = @Id", new { Qty = newQty, Id = item.Id }, transaction);
                    }
                }

                // Swap
                await ModifyQuantity(userA, stickerA, -1);
                await ModifyQuantity(userA, stickerB, 1);
                await ModifyQuantity(userB, stickerB, -1);
                await ModifyQuantity(userB, stickerA, 1);

                // Mark Offer Accepted
                await connection.ExecuteAsync("UPDATE TradeOffers SET Status = 'Accepted' WHERE Id = @OId", new { OId = offerId }, transaction);
                // Mark Listing Completed
                await connection.ExecuteAsync("UPDATE TradeListings SET Status = 'Completed' WHERE Id = @LId", new { LId = offer.ListingId }, transaction);
                // Reject all other offers for this listing
                await connection.ExecuteAsync("UPDATE TradeOffers SET Status = 'Rejected' WHERE ListingId = @LId AND Id != @OId", new { LId = offer.ListingId, OId = offerId }, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        // --- REPORTS ---
        public async Task<ReportStats> GetDashboardStatsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var stats = new ReportStats();
            stats.TotalCollectors = await connection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM Collectors");
            stats.TotalStickersInCirculation = await connection.ExecuteScalarAsync<int>("SELECT ISNULL(SUM(Quantity),0) FROM AlbumItems");
            stats.CompletedTrades = await connection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM TradeListings WHERE Status='Completed'");
            return stats;
        }

        public async Task<IEnumerable<TopCollector>> GetTopCollectorsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<TopCollector>(@"
                SELECT TOP 5 c.Username, COUNT(a.StickerId) as UniqueStickersCount
                FROM Collectors c
                LEFT JOIN AlbumItems a ON c.Id = a.CollectorId AND a.Quantity > 0
                GROUP BY c.Username
                ORDER BY UniqueStickersCount DESC");
        }

        public async Task<IEnumerable<MostTradedSticker>> GetMostTradedStickersAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<MostTradedSticker>(@"
                SELECT TOP 5 s.PlayerName, t.CountryName, COUNT(o.Id) as TradeCount
                FROM Stickers s
                INNER JOIN Teams t ON s.TeamId = t.Id
                LEFT JOIN TradeOffers o ON s.Id = o.OfferedStickerId AND o.Status = 'Accepted'
                GROUP BY s.PlayerName, t.CountryName
                ORDER BY TradeCount DESC");
        }
    }
}
