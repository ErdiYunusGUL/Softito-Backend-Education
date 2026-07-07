using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SneakerDropPlatform.Data
{
    public class DbInitializer
    {
        private readonly string _connectionString;

        public DbInitializer(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public void Initialize()
        {
            var builder = new SqlConnectionStringBuilder(_connectionString);
            string databaseName = builder.InitialCatalog;
            builder.InitialCatalog = "master";
            
            using (var masterConnection = new SqlConnection(builder.ConnectionString))
            {
                masterConnection.Open();
                string createDbSql = $@"
                    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{databaseName}')
                    BEGIN
                        CREATE DATABASE [{databaseName}];
                    END
                ";
                using (var cmd = new SqlCommand(createDbSql, masterConnection))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Create Tables
                string createTablesSql = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Brands' and xtype='U')
                    CREATE TABLE Brands (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(100) NOT NULL,
                        LogoUrl NVARCHAR(500)
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Sneakers' and xtype='U')
                    CREATE TABLE Sneakers (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        BrandId INT FOREIGN KEY REFERENCES Brands(Id),
                        ModelName NVARCHAR(200) NOT NULL,
                        Description NVARCHAR(MAX),
                        Price DECIMAL(18,2) NOT NULL,
                        ImageUrl NVARCHAR(500)
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Drops' and xtype='U')
                    CREATE TABLE Drops (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        SneakerId INT FOREIGN KEY REFERENCES Sneakers(Id),
                        DropDate DATETIME2 NOT NULL,
                        IsActive BIT NOT NULL DEFAULT 1
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Inventory' and xtype='U')
                    CREATE TABLE Inventory (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        DropId INT FOREIGN KEY REFERENCES Drops(Id),
                        Size NVARCHAR(50) NOT NULL,
                        Quantity INT NOT NULL
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Waitlists' and xtype='U')
                    CREATE TABLE Waitlists (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        DropId INT FOREIGN KEY REFERENCES Drops(Id),
                        Email NVARCHAR(255) NOT NULL,
                        JoinedAt DATETIME2 NOT NULL DEFAULT GETDATE()
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Orders' and xtype='U')
                    CREATE TABLE Orders (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        CustomerName NVARCHAR(255) NOT NULL,
                        Email NVARCHAR(255) NOT NULL,
                        TotalAmount DECIMAL(18,2) NOT NULL,
                        OrderDate DATETIME2 NOT NULL DEFAULT GETDATE()
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='OrderItems' and xtype='U')
                    CREATE TABLE OrderItems (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        OrderId INT FOREIGN KEY REFERENCES Orders(Id),
                        DropId INT FOREIGN KEY REFERENCES Drops(Id),
                        Quantity INT NOT NULL,
                        Price DECIMAL(18,2) NOT NULL
                    );

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Admins' and xtype='U')
                    CREATE TABLE Admins (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Username NVARCHAR(100) NOT NULL,
                        Password NVARCHAR(100) NOT NULL
                    );
                ";

                using (var command = new SqlCommand(createTablesSql, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Seed Admin
                string checkAdminSql = "SELECT COUNT(*) FROM Admins";
                using (var checkCmd = new SqlCommand(checkAdminSql, connection))
                {
                    if ((int)checkCmd.ExecuteScalar() == 0)
                    {
                        using (var seedAdminCmd = new SqlCommand("INSERT INTO Admins (Username, Password) VALUES ('admin', 'admin123')", connection))
                        {
                            seedAdminCmd.ExecuteNonQuery();
                        }
                    }
                }

                // Seed Initial Data (if completely empty)
                string checkDataSql = "SELECT COUNT(*) FROM Brands";
                using (var checkCmd = new SqlCommand(checkDataSql, connection))
                {
                    if ((int)checkCmd.ExecuteScalar() == 0)
                    {
                        string seedSql = @"
                            INSERT INTO Brands (Name, LogoUrl) VALUES 
                            ('Dr. Martens', 'https://upload.wikimedia.org/wikipedia/commons/4/41/Dr._Martens_logo.svg'),
                            ('Vans', 'https://upload.wikimedia.org/wikipedia/commons/9/90/Vans-logo.svg'),
                            ('Nike', 'https://upload.wikimedia.org/wikipedia/commons/a/a6/Logo_NIKE.svg'),
                            ('New Balance', 'https://upload.wikimedia.org/wikipedia/commons/e/ea/New_Balance_logo.svg'),
                            ('Adidas', 'https://upload.wikimedia.org/wikipedia/commons/2/20/Adidas_Logo.svg'),
                            ('Jordan', 'https://upload.wikimedia.org/wikipedia/en/thumb/3/37/Jumpman_logo.svg/1200px-Jumpman_logo.svg.png');

                            DECLARE @DrMartensId INT = (SELECT Id FROM Brands WHERE Name = 'Dr. Martens');
                            DECLARE @VansId INT = (SELECT Id FROM Brands WHERE Name = 'Vans');
                            DECLARE @NikeId INT = (SELECT Id FROM Brands WHERE Name = 'Nike');
                            DECLARE @NewBalanceId INT = (SELECT Id FROM Brands WHERE Name = 'New Balance');
                            DECLARE @AdidasId INT = (SELECT Id FROM Brands WHERE Name = 'Adidas');
                            DECLARE @JordanId INT = (SELECT Id FROM Brands WHERE Name = 'Jordan');

                            INSERT INTO Sneakers (BrandId, ModelName, Description, Price, ImageUrl) VALUES 
                            (@DrMartensId, '1460 Smooth Leather', 'The original Dr. Martens 8-eye boot.', 170.00, 'https://media.gq.com/photos/5a3be20e6f2c3d523bf1bf01/master/w_2000,h_1333,c_limit/boots-01.jpg'),
                            (@VansId, 'Skate Old Skool', 'Classic skate shoe upgraded for performance.', 75.00, 'https://images.vans.com/is/image/Vans/VN0A5FCBBLK-HERO?$583x583$'),
                            (@NikeId, 'Air Force 1 ''07', 'The radiance lives on in the Nike Air Force 1 ''07.', 115.00, 'https://static.nike.com/a/images/t_PDP_1280_v1/f_auto,q_auto:eco/b7d9211c-26e7-431a-ac24-b0540fb3c00f/air-force-1-07-mens-shoes-jBrhbr.png'),
                            (@NewBalanceId, '574 Core', 'The most New Balance shoe ever.', 89.99, 'https://nb.scene7.com/is/image/NB/ml574evg_nb_02_i?$dw_detail_main_lg$&bgc=f1f1f1&layer=1&bgcolor=f1f1f1&blendMode=mult&scale=10&wid=1600&hei=1600'),
                            (@AdidasId, 'Yeezy Boost 350 V2', 'Iconic silhouette designed by Kanye West.', 230.00, 'https://images.stockx.com/360/adidas-Yeezy-Boost-350-V2-Beluga-Reflective/Images/adidas-Yeezy-Boost-350-V2-Beluga-Reflective/Lv2/img01.jpg?fm=avif&auto=compress&w=576&dpr=2&updated_at=1639078759&h=384&q=75'),
                            (@NikeId, 'Dunk Low Panda', 'The most hyped Dunk in recent years.', 110.00, 'https://images.stockx.com/360/Nike-Dunk-Low-Retro-White-Black-2021/Images/Nike-Dunk-Low-Retro-White-Black-2021/Lv2/img01.jpg?fm=avif&auto=compress&w=576&dpr=2&updated_at=1633023253&h=384&q=75'),
                            (@JordanId, 'Air Jordan 1 Retro High OG', 'Chicago Reimagined.', 180.00, 'https://images.stockx.com/360/Air-Jordan-1-Retro-High-OG-Chicago-Reimagined-Lost-and-Found/Images/Air-Jordan-1-Retro-High-OG-Chicago-Reimagined-Lost-and-Found/Lv2/img01.jpg?fm=avif&auto=compress&w=576&dpr=2&updated_at=1667232231&h=384&q=75'),
                            (@AdidasId, 'Samba OG', 'Born on the pitch, built for the streets.', 100.00, 'https://images.stockx.com/360/adidas-Samba-OG-Cloud-White-Core-Black/Images/adidas-Samba-OG-Cloud-White-Core-Black/Lv2/img01.jpg?fm=avif&auto=compress&w=576&dpr=2&updated_at=1672304953&h=384&q=75');

                            DECLARE @Sneaker1 INT = (SELECT Id FROM Sneakers WHERE ModelName = '1460 Smooth Leather');
                            DECLARE @Sneaker2 INT = (SELECT Id FROM Sneakers WHERE ModelName = 'Skate Old Skool');
                            DECLARE @Sneaker3 INT = (SELECT Id FROM Sneakers WHERE ModelName = 'Air Force 1 ''07');
                            DECLARE @Sneaker4 INT = (SELECT Id FROM Sneakers WHERE ModelName = '574 Core');
                            DECLARE @Sneaker5 INT = (SELECT Id FROM Sneakers WHERE ModelName = 'Yeezy Boost 350 V2');
                            DECLARE @Sneaker6 INT = (SELECT Id FROM Sneakers WHERE ModelName = 'Dunk Low Panda');
                            DECLARE @Sneaker7 INT = (SELECT Id FROM Sneakers WHERE ModelName = 'Air Jordan 1 Retro High OG');
                            DECLARE @Sneaker8 INT = (SELECT Id FROM Sneakers WHERE ModelName = 'Samba OG');

                            INSERT INTO Drops (SneakerId, DropDate, IsActive) VALUES 
                            (@Sneaker1, GETDATE(), 1),
                            (@Sneaker2, GETDATE(), 1),
                            (@Sneaker3, DATEADD(day, 2, GETDATE()), 1),
                            (@Sneaker4, DATEADD(day, -1, GETDATE()), 1),
                            (@Sneaker5, GETDATE(), 1),
                            (@Sneaker6, GETDATE(), 1),
                            (@Sneaker7, GETDATE(), 1),
                            (@Sneaker8, GETDATE(), 1);

                            DECLARE @Drop1 INT = (SELECT Id FROM Drops WHERE SneakerId = @Sneaker1);
                            DECLARE @Drop2 INT = (SELECT Id FROM Drops WHERE SneakerId = @Sneaker2);
                            DECLARE @Drop3 INT = (SELECT Id FROM Drops WHERE SneakerId = @Sneaker3);
                            DECLARE @Drop4 INT = (SELECT Id FROM Drops WHERE SneakerId = @Sneaker4);
                            DECLARE @Drop5 INT = (SELECT Id FROM Drops WHERE SneakerId = @Sneaker5);
                            DECLARE @Drop6 INT = (SELECT Id FROM Drops WHERE SneakerId = @Sneaker6);
                            DECLARE @Drop7 INT = (SELECT Id FROM Drops WHERE SneakerId = @Sneaker7);
                            DECLARE @Drop8 INT = (SELECT Id FROM Drops WHERE SneakerId = @Sneaker8);

                            INSERT INTO Inventory (DropId, Size, Quantity) VALUES 
                            (@Drop1, '8', 5), (@Drop1, '9', 10), (@Drop1, '10', 0),
                            (@Drop2, '9', 20), (@Drop2, '9.5', 15), (@Drop2, '10', 5),
                            (@Drop3, '10', 50), (@Drop3, '11', 40),
                            (@Drop4, '8.5', 2), (@Drop4, '9', 1),
                            (@Drop5, '10', 3), (@Drop5, '11', 2),
                            (@Drop6, '9', 15), (@Drop6, '10', 12),
                            (@Drop7, '10', 1), (@Drop7, '11', 0),
                            (@Drop8, '8', 25), (@Drop8, '9', 30);
                        ";

                        using (var seedCmd = new SqlCommand(seedSql, connection))
                        {
                            seedCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // DB already exists. We will just check if we have the new products (Adidas).
                        // If not, we run a partial seed.
                        string checkExtraSql = "SELECT COUNT(*) FROM Brands WHERE Name = 'Adidas'";
                        using (var checkExtraCmd = new SqlCommand(checkExtraSql, connection))
                        {
                            if ((int)checkExtraCmd.ExecuteScalar() == 0)
                            {
                                string extraSeedSql = @"
                                    INSERT INTO Brands (Name, LogoUrl) VALUES 
                                    ('Adidas', 'https://upload.wikimedia.org/wikipedia/commons/2/20/Adidas_Logo.svg'),
                                    ('Jordan', 'https://upload.wikimedia.org/wikipedia/en/thumb/3/37/Jumpman_logo.svg/1200px-Jumpman_logo.svg.png');

                                    DECLARE @AdidasId INT = (SELECT Id FROM Brands WHERE Name = 'Adidas');
                                    DECLARE @JordanId INT = (SELECT Id FROM Brands WHERE Name = 'Jordan');
                                    DECLARE @NikeId INT = (SELECT Id FROM Brands WHERE Name = 'Nike');

                                    INSERT INTO Sneakers (BrandId, ModelName, Description, Price, ImageUrl) VALUES 
                                    (@AdidasId, 'Yeezy Boost 350 V2', 'Iconic silhouette designed by Kanye West.', 230.00, 'https://images.stockx.com/360/adidas-Yeezy-Boost-350-V2-Beluga-Reflective/Images/adidas-Yeezy-Boost-350-V2-Beluga-Reflective/Lv2/img01.jpg?fm=avif&auto=compress&w=576&dpr=2&updated_at=1639078759&h=384&q=75'),
                                    (@NikeId, 'Dunk Low Panda', 'The most hyped Dunk in recent years.', 110.00, 'https://images.stockx.com/360/Nike-Dunk-Low-Retro-White-Black-2021/Images/Nike-Dunk-Low-Retro-White-Black-2021/Lv2/img01.jpg?fm=avif&auto=compress&w=576&dpr=2&updated_at=1633023253&h=384&q=75'),
                                    (@JordanId, 'Air Jordan 1 Retro High OG', 'Chicago Reimagined.', 180.00, 'https://images.stockx.com/360/Air-Jordan-1-Retro-High-OG-Chicago-Reimagined-Lost-and-Found/Images/Air-Jordan-1-Retro-High-OG-Chicago-Reimagined-Lost-and-Found/Lv2/img01.jpg?fm=avif&auto=compress&w=576&dpr=2&updated_at=1667232231&h=384&q=75'),
                                    (@AdidasId, 'Samba OG', 'Born on the pitch, built for the streets.', 100.00, 'https://images.stockx.com/360/adidas-Samba-OG-Cloud-White-Core-Black/Images/adidas-Samba-OG-Cloud-White-Core-Black/Lv2/img01.jpg?fm=avif&auto=compress&w=576&dpr=2&updated_at=1672304953&h=384&q=75');

                                    DECLARE @Sneaker5 INT = (SELECT Id FROM Sneakers WHERE ModelName = 'Yeezy Boost 350 V2');
                                    DECLARE @Sneaker6 INT = (SELECT Id FROM Sneakers WHERE ModelName = 'Dunk Low Panda');
                                    DECLARE @Sneaker7 INT = (SELECT Id FROM Sneakers WHERE ModelName = 'Air Jordan 1 Retro High OG');
                                    DECLARE @Sneaker8 INT = (SELECT Id FROM Sneakers WHERE ModelName = 'Samba OG');

                                    INSERT INTO Drops (SneakerId, DropDate, IsActive) VALUES 
                                    (@Sneaker5, GETDATE(), 1),
                                    (@Sneaker6, GETDATE(), 1),
                                    (@Sneaker7, GETDATE(), 1),
                                    (@Sneaker8, GETDATE(), 1);

                                    DECLARE @Drop5 INT = (SELECT Id FROM Drops WHERE SneakerId = @Sneaker5);
                                    DECLARE @Drop6 INT = (SELECT Id FROM Drops WHERE SneakerId = @Sneaker6);
                                    DECLARE @Drop7 INT = (SELECT Id FROM Drops WHERE SneakerId = @Sneaker7);
                                    DECLARE @Drop8 INT = (SELECT Id FROM Drops WHERE SneakerId = @Sneaker8);

                                    INSERT INTO Inventory (DropId, Size, Quantity) VALUES 
                                    (@Drop5, '10', 3), (@Drop5, '11', 2),
                                    (@Drop6, '9', 15), (@Drop6, '10', 12),
                                    (@Drop7, '10', 1), (@Drop7, '11', 0),
                                    (@Drop8, '8', 25), (@Drop8, '9', 30);
                                ";
                                using (var extraSeedCmd = new SqlCommand(extraSeedSql, connection))
                                {
                                    extraSeedCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
