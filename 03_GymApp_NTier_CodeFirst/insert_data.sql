USE GymProjectDb;
GO

-- Insert 5 new Plans
SET IDENTITY_INSERT Plans ON;
INSERT INTO Plans (Id, Name, Price, DurationInMonths) VALUES (4, 'Student Plan', 150.00, 1);
INSERT INTO Plans (Id, Name, Price, DurationInMonths) VALUES (5, 'Couples Plan', 400.00, 1);
INSERT INTO Plans (Id, Name, Price, DurationInMonths) VALUES (6, 'Corporate Annual', 1500.00, 12);
INSERT INTO Plans (Id, Name, Price, DurationInMonths) VALUES (7, 'Weekend Warrior', 100.00, 1);
INSERT INTO Plans (Id, Name, Price, DurationInMonths) VALUES (8, 'Ultimate Titan', 3000.00, 12);
SET IDENTITY_INSERT Plans OFF;
GO

-- Insert 5 new Trainers
SET IDENTITY_INSERT Trainers ON;
INSERT INTO Trainers (Id, FirstName, LastName, Specialization) VALUES (3, 'Mike', 'Tyson', 'Boxing & Agility');
INSERT INTO Trainers (Id, FirstName, LastName, Specialization) VALUES (4, 'Sarah', 'Connor', 'Strength & Conditioning');
INSERT INTO Trainers (Id, FirstName, LastName, Specialization) VALUES (5, 'Arnold', 'Schwarzenegger', 'Powerlifting');
INSERT INTO Trainers (Id, FirstName, LastName, Specialization) VALUES (6, 'Bruce', 'Lee', 'Martial Arts & Flexibility');
INSERT INTO Trainers (Id, FirstName, LastName, Specialization) VALUES (7, 'Dwayne', 'Johnson', 'Hypertrophy');
SET IDENTITY_INSERT Trainers OFF;
GO

-- Insert 10 new Members
SET IDENTITY_INSERT Members ON;
INSERT INTO Members (Id, FirstName, LastName, Email, PhoneNumber, JoinDate, PlanId) VALUES (3, 'Clark', 'Kent', 'clark@dailyplanet.com', '555-1001', GETDATE(), 8);
INSERT INTO Members (Id, FirstName, LastName, Email, PhoneNumber, JoinDate, PlanId) VALUES (4, 'Bruce', 'Wayne', 'bruce@wayne.com', '555-1002', GETDATE(), 3);
INSERT INTO Members (Id, FirstName, LastName, Email, PhoneNumber, JoinDate, PlanId) VALUES (5, 'Diana', 'Prince', 'diana@themyscira.com', '555-1003', GETDATE(), 2);
INSERT INTO Members (Id, FirstName, LastName, Email, PhoneNumber, JoinDate, PlanId) VALUES (6, 'Barry', 'Allen', 'barry@ccpd.com', '555-1004', GETDATE(), 1);
INSERT INTO Members (Id, FirstName, LastName, Email, PhoneNumber, JoinDate, PlanId) VALUES (7, 'Arthur', 'Curry', 'arthur@atlantis.com', '555-1005', GETDATE(), 6);
INSERT INTO Members (Id, FirstName, LastName, Email, PhoneNumber, JoinDate, PlanId) VALUES (8, 'Victor', 'Stone', 'victor@star.com', '555-1006', GETDATE(), 4);
INSERT INTO Members (Id, FirstName, LastName, Email, PhoneNumber, JoinDate, PlanId) VALUES (9, 'Hal', 'Jordan', 'hal@oa.com', '555-1007', GETDATE(), 5);
INSERT INTO Members (Id, FirstName, LastName, Email, PhoneNumber, JoinDate, PlanId) VALUES (10, 'Oliver', 'Queen', 'oliver@queen.com', '555-1008', GETDATE(), 2);
INSERT INTO Members (Id, FirstName, LastName, Email, PhoneNumber, JoinDate, PlanId) VALUES (11, 'Kara', 'Danvers', 'kara@catco.com', '555-1009', GETDATE(), 1);
INSERT INTO Members (Id, FirstName, LastName, Email, PhoneNumber, JoinDate, PlanId) VALUES (12, 'John', 'Constantine', 'john@magic.com', '555-1010', GETDATE(), 7);
SET IDENTITY_INSERT Members OFF;
GO
