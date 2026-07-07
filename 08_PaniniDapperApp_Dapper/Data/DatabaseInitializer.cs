using Microsoft.Data.SqlClient;
using Dapper;

namespace PaniniDapperApp.Data
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        public DatabaseInitializer(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public void Initialize()
        {
            var builder = new SqlConnectionStringBuilder(_connectionString);
            var databaseName = builder.InitialCatalog;
            builder.InitialCatalog = "master";
            
            using (var masterConnection = new SqlConnection(builder.ConnectionString))
            {
                masterConnection.Open();
                var dropDbQuery = $@"
                    IF EXISTS (SELECT * FROM sys.databases WHERE name = '{databaseName}')
                    BEGIN
                        ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        DROP DATABASE [{databaseName}];
                    END
                    CREATE DATABASE [{databaseName}];
                ";
                masterConnection.Execute(dropDbQuery);
            }

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            // Create Tables
            connection.Execute(@"
                CREATE TABLE Teams (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    CountryName NVARCHAR(100) NOT NULL
                )");

            connection.Execute(@"
                CREATE TABLE Stickers (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Number INT NOT NULL,
                    PlayerName NVARCHAR(100) NOT NULL,
                    TeamId INT NOT NULL,
                    FOREIGN KEY (TeamId) REFERENCES Teams(Id)
                )");

            connection.Execute(@"
                CREATE TABLE Collectors (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Username NVARCHAR(50) NOT NULL UNIQUE,
                    Password NVARCHAR(255) NOT NULL
                )");

            connection.Execute(@"
                CREATE TABLE AlbumItems (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    CollectorId INT NOT NULL,
                    StickerId INT NOT NULL,
                    Quantity INT NOT NULL DEFAULT 1,
                    FOREIGN KEY (CollectorId) REFERENCES Collectors(Id),
                    FOREIGN KEY (StickerId) REFERENCES Stickers(Id)
                )");

            // TradeListings: I have this sticker and I want to trade it
            connection.Execute(@"
                CREATE TABLE TradeListings (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    CollectorId INT NOT NULL,
                    StickerId INT NOT NULL,
                    Status NVARCHAR(20) NOT NULL DEFAULT 'Open',
                    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                    FOREIGN KEY (CollectorId) REFERENCES Collectors(Id),
                    FOREIGN KEY (StickerId) REFERENCES Stickers(Id)
                )");

            // TradeOffers: I offer you THIS sticker for your Listing
            connection.Execute(@"
                CREATE TABLE TradeOffers (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    ListingId INT NOT NULL,
                    OfferedByCollectorId INT NOT NULL,
                    OfferedStickerId INT NOT NULL,
                    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',
                    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                    FOREIGN KEY (ListingId) REFERENCES TradeListings(Id),
                    FOREIGN KEY (OfferedByCollectorId) REFERENCES Collectors(Id),
                    FOREIGN KEY (OfferedStickerId) REFERENCES Stickers(Id)
                )");

            // --- MASSIVE SEED DATA ---
            
            // 1. Teams
            connection.Execute(@"
                INSERT INTO Teams (CountryName) VALUES 
                ('Argentina'), ('Brazil'), ('France'), ('England'), 
                ('Spain'), ('Germany'), ('Portugal'), ('Netherlands'),
                ('Italy'), ('Belgium'), ('Croatia'), ('Uruguay')");

            // 2. Stickers (Players)
            connection.Execute(@"
                INSERT INTO Stickers (Number, PlayerName, TeamId) VALUES 
                (10, 'Lionel Messi', 1), (11, 'Angel Di Maria', 1), (9, 'Julian Alvarez', 1), (23, 'Emi Martinez', 1),
                (10, 'Neymar Jr', 2), (9, 'Richarlison', 2), (20, 'Vinicius Jr', 2), (5, 'Casemiro', 2),
                (10, 'Kylian Mbappe', 3), (7, 'Antoine Griezmann', 3), (14, 'A. Tchouameni', 3), (22, 'Theo Hernandez', 3),
                (9, 'Harry Kane', 4), (10, 'Jude Bellingham', 4), (7, 'Bukayo Saka', 4), (11, 'Phil Foden', 4),
                (8, 'Pedri', 5), (9, 'Gavi', 5), (11, 'Ferran Torres', 5), (19, 'Lamine Yamal', 5),
                (10, 'Jamal Musiala', 6), (9, 'Niclas Füllkrug', 6), (21, 'Ilkay Gündogan', 6), (8, 'Toni Kroos', 6),
                (7, 'Cristiano Ronaldo', 7), (8, 'Bruno Fernandes', 7), (10, 'Bernardo Silva', 7), (14, 'Rafael Leao', 7),
                (4, 'Virgil van Dijk', 8), (11, 'Cody Gakpo', 8), (21, 'Frenkie de Jong', 8), (10, 'Xavi Simons', 8)
                ");

            // 3. Collectors
            connection.Execute(@"
                INSERT INTO Collectors (Username, Password) VALUES 
                ('ali', '12345'), ('ayse', '12345'), ('mehmet', '12345'),
                ('johndoe', '12345'), ('alex_collects', '12345'), ('maria_99', '12345'),
                ('pro_trader', '12345'), ('football_fan', '12345'), ('ronaldo_goat', '12345')");

            // 4. Album Items (Give users lots of stickers and duplicates)
            // Ali (Id=1)
            connection.Execute(@"
                INSERT INTO AlbumItems (CollectorId, StickerId, Quantity) VALUES 
                (1, 1, 4), (1, 2, 1), (1, 7, 2), (1, 10, 1), (1, 15, 3), (1, 25, 1)");
            
            // Ayşe (Id=2)
            connection.Execute(@"
                INSERT INTO AlbumItems (CollectorId, StickerId, Quantity) VALUES 
                (2, 5, 2), (2, 6, 1), (2, 9, 3), (2, 12, 1), (2, 20, 2), (2, 30, 1)");
                
            // Mehmet (Id=3)
            connection.Execute(@"
                INSERT INTO AlbumItems (CollectorId, StickerId, Quantity) VALUES 
                (3, 8, 2), (3, 4, 1), (3, 11, 4), (3, 21, 1), (3, 31, 2)");
                
            // Others
            connection.Execute(@"
                INSERT INTO AlbumItems (CollectorId, StickerId, Quantity) VALUES 
                (4, 25, 3), (4, 26, 1), (4, 14, 2), (4, 3, 1),
                (5, 10, 5), (5, 9, 1), (5, 22, 2), (5, 8, 1),
                (6, 15, 2), (6, 16, 1), (6, 1, 2), (6, 2, 1),
                (7, 30, 4), (7, 31, 1), (7, 12, 3), (7, 5, 1),
                (8, 20, 3), (8, 19, 1), (8, 7, 2), (8, 6, 1)");

            // 5. Open Market Listings
            connection.Execute(@"
                INSERT INTO TradeListings (CollectorId, StickerId, Status, CreatedDate) VALUES 
                (1, 1, 'Open', DATEADD(hour, -2, GETDATE())),    -- Ali listing Messi (1)
                (1, 15, 'Open', DATEADD(hour, -5, GETDATE())),   -- Ali listing Saka (15)
                (2, 5, 'Open', DATEADD(hour, -1, GETDATE())),    -- Ayse listing Neymar (5)
                (2, 9, 'Open', DATEADD(hour, -24, GETDATE())),   -- Ayse listing Mbappe (9)
                (3, 11, 'Open', DATEADD(hour, -48, GETDATE())),  -- Mehmet listing Tchouameni (11)
                (4, 25, 'Open', DATEADD(hour, -12, GETDATE())),  -- johndoe listing Ronaldo (25)
                (5, 10, 'Open', DATEADD(hour, -3, GETDATE())),   -- alex listing Griezmann (10)
                (7, 30, 'Open', DATEADD(minute, -30, GETDATE())),-- pro_trader listing Gakpo (30)
                (8, 20, 'Open', DATEADD(minute, -15, GETDATE())) -- football_fan listing Yamal (20)
                ");

            // 6. Pending Offers
            connection.Execute(@"
                INSERT INTO TradeOffers (ListingId, OfferedByCollectorId, OfferedStickerId, Status, CreatedDate) VALUES 
                (1, 4, 25, 'Pending', DATEADD(hour, -1, GETDATE())), -- johndoe offers Ronaldo for Ali's Messi
                (1, 6, 15, 'Pending', DATEADD(minute, -45, GETDATE())), -- maria offers Saka for Ali's Messi
                (3, 1, 7, 'Pending', DATEADD(minute, -10, GETDATE())), -- Ali offers Vinicius for Ayse's Neymar
                (4, 5, 22, 'Pending', DATEADD(hour, -10, GETDATE())), -- alex offers Theo for Ayse's Mbappe
                (6, 2, 20, 'Pending', DATEADD(hour, -5, GETDATE()))  -- Ayse offers Vinicius for johndoe's Ronaldo
                ");
                
            // 7. Dummy Completed Trades (For Dashboard Stats)
            // Create some listings and immediately accept offers for them.
            // But we can just fake it by inserting directly into TradeListings and TradeOffers with 'Completed'/'Accepted' status.
            connection.Execute(@"
                -- Completed Listing 1
                INSERT INTO TradeListings (CollectorId, StickerId, Status, CreatedDate) VALUES (4, 3, 'Completed', DATEADD(day, -2, GETDATE()));
                INSERT INTO TradeOffers (ListingId, OfferedByCollectorId, OfferedStickerId, Status, CreatedDate) VALUES (IDENT_CURRENT('TradeListings'), 1, 1, 'Accepted', DATEADD(day, -2, GETDATE()));
                
                -- Completed Listing 2
                INSERT INTO TradeListings (CollectorId, StickerId, Status, CreatedDate) VALUES (5, 9, 'Completed', DATEADD(day, -3, GETDATE()));
                INSERT INTO TradeOffers (ListingId, OfferedByCollectorId, OfferedStickerId, Status, CreatedDate) VALUES (IDENT_CURRENT('TradeListings'), 2, 5, 'Accepted', DATEADD(day, -3, GETDATE()));
                
                -- Completed Listing 3
                INSERT INTO TradeListings (CollectorId, StickerId, Status, CreatedDate) VALUES (6, 16, 'Completed', DATEADD(day, -4, GETDATE()));
                INSERT INTO TradeOffers (ListingId, OfferedByCollectorId, OfferedStickerId, Status, CreatedDate) VALUES (IDENT_CURRENT('TradeListings'), 7, 30, 'Accepted', DATEADD(day, -4, GETDATE()));

                -- Completed Listing 4
                INSERT INTO TradeListings (CollectorId, StickerId, Status, CreatedDate) VALUES (8, 6, 'Completed', DATEADD(day, -5, GETDATE()));
                INSERT INTO TradeOffers (ListingId, OfferedByCollectorId, OfferedStickerId, Status, CreatedDate) VALUES (IDENT_CURRENT('TradeListings'), 1, 7, 'Accepted', DATEADD(day, -5, GETDATE()));
                
                -- Completed Listing 5
                INSERT INTO TradeListings (CollectorId, StickerId, Status, CreatedDate) VALUES (1, 25, 'Completed', DATEADD(day, -1, GETDATE()));
                INSERT INTO TradeOffers (ListingId, OfferedByCollectorId, OfferedStickerId, Status, CreatedDate) VALUES (IDENT_CURRENT('TradeListings'), 3, 11, 'Accepted', DATEADD(day, -1, GETDATE()));
                ");
        }
    }
}
