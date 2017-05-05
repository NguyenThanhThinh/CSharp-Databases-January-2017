-- 1. All Ad Titles

SELECT Title 
  FROM Ads
 ORDER BY Title

-- 2. Ads in Date Range

SELECT Title
	   , Date 
  FROM Ads
 WHERE Date BETWEEN '2014-12-26' AND '2015-01-02'
 ORDER BY Date

-- 3. Ads with "Yes/No" Images

SELECT Title
	   , Date 
	   , CASE
			  WHEN ImageDataURL IS NULL THEN 'no'
			  ELSE 'yes'
		 END AS 'Has Image' 
  FROM Ads
 ORDER BY Id

-- 4. Ads without Town, Category or Image

SELECT * 
  FROM Ads
 WHERE TownId IS NULL
    OR CategoryId IS NULL
    OR ImageDataURL IS NULL
 ORDER BY Id

-- 5. Ads with Their Town

SELECT Title
	   , t.Name AS [Town] 
  FROM Ads AS a
  LEFT JOIN Towns AS t ON a.TownId = t.Id
 ORDER BY a.Id

-- 6. Ads with Their Category, Town and Status

SELECT Title
	   , c.Name AS [CategoryName]
	   , t.Name AS [TownName]
	   , ads.Status 
  FROM Ads AS a
  LEFT JOIN Towns AS t ON t.Id = a.TownId
  LEFT JOIN Categories AS c ON c.Id = a.CategoryId
  LEFT JOIN AdStatuses AS ads ON ads.Id = a.StatusId 
 ORDER BY a.Id

-- 7. Filtered Ads with Category, Town and Status

SELECT Title
	   , c.Name AS [CategoryName]
	   , t.Name AS [TownName]
	   , ads.Status 
  FROM Ads AS a
  JOIN Towns AS t ON t.Id = a.TownId
  JOIN Categories AS c ON c.Id = a.CategoryId
  JOIN AdStatuses AS ads ON ads.Id = a.StatusId 
 WHERE t.Name IN ('Sofia', 'Blagoevgrad', 'Stara Zagora')
   AND ads.Status = 'Published'
 ORDER BY a.Title

-- 8. Earliest and Latest Ads

SELECT MIN(Date) AS MinDate
	   , MAX(Date) AS MaxDate 
  FROM Ads

-- 9. Latest 10 Ads

SELECT TOP (10) 
	   a.Title
	   , a.Date
	   , ads.Status 
  FROM Ads AS a
  JOIN AdStatuses AS ads ON a.StatusId = ads.Id
 ORDER BY a.Date DESC

-- 10. Not Published Ads from the First Month

SELECT a.Id
	   , a.Title
	   , a.Date
	   , ads.Status
  FROM Ads AS a
  JOIN AdStatuses AS ads ON a.StatusId = ads.Id
 WHERE ads.Status != 'Published'
   AND YEAR(a.Date) = YEAR((SELECT MIN(Date) FROM Ads))
   AND MONTH(a.Date) = MONTH((SELECT MIN(Date) FROM Ads))
 ORDER BY a.Id

-- 11. Ads by Status

SELECT ads.Status
	   , COUNT(a.Id) AS [Count]
  FROM Ads AS a
  JOIN AdStatuses AS ads ON a.StatusId = ads.Id
 GROUP BY ads.Status
 ORDER BY ads.Status

-- 12. Ads by Town and Status
SELECT t.Name AS [Town Name]
	   , ads.Status AS [Status]
	   , COUNT(a.Id) AS [Count]
  FROM Ads AS a
  JOIN AdStatuses AS ads ON a.StatusId = ads.Id
  JOIN Towns AS t ON a.TownId = t.Id
 GROUP BY t.Name, ads.Status
 ORDER BY t.Name, ads.Status

-- 13. Ads by Users

SELECT users.UserName AS [UserName]
	   , COUNT(a.Id) AS [AdsCount]
	   , CASE
			 WHEN admins.UserName IS NULL THEN 'no'
			 ELSE 'yes'
		 END AS [IsAdministrator]
  FROM AspNetUsers AS users
  LEFT JOIN Ads AS a ON users.Id = a.OwnerId
  LEFT JOIN (
			  SELECT u.UserName
			    FROM AspNetUsers AS u
			    JOIN AspNetUserRoles ur ON u.Id = ur.UserId
 			    JOIN AspNetRoles AS r ON ur.RoleId = r.Id
			   WHERE r.Name = 'Administrator'
			 )    AS admins
	ON users.UserName = admins.UserName
 GROUP BY users.UserName, admins.UserName
 ORDER BY users.UserName

--13. Ads by Users

SELECT users.UserName AS [UserName]
	   , COUNT(a.Id) AS [AdsCount]
	   , CASE
			 WHEN users.UserName 
				  IN (
						SELECT UserName 
						  FROM AspNetUsers u
						  JOIN AspNetUserRoles ur ON u.Id = ur.UserId
						  JOIN AspNetRoles AS r ON r.Id = ur.RoleId
						 WHERE r.Name = 'Administrator'
					  )   THEN 'yes'
			 ELSE 'no'
		 END AS [IsAdministrator]
  FROM AspNetUsers AS users
  LEFT JOIN Ads AS a ON users.Id = a.OwnerId
 GROUP BY users.UserName
 ORDER BY users.UserName

-- 14. Ads by Town

SELECT COUNT(a.Id) AS [AdsCount]
	   , ISNULL(t.Name, '(no town)')  AS [Town]
  FROM Ads AS a
  LEFT JOIN Towns AS t ON a.TownId = t.Id
 GROUP BY t.Name
HAVING COUNT(a.Id) IN (2, 3)
 ORDER BY t.Name

-- 15. Pairs of Dates within 12 Hours

SELECT a.Date AS [FirstDate]
	   , a2.Date AS [SecondDate]
  FROM Ads AS a, Ads AS a2
 WHERE a.Date < a2.Date
   AND a2.Date <= DATEADD(HOUR, 12, a.Date)
 --AND DATEDIFF(HOUR, a.Date, a2.Date) < 12
 ORDER BY FirstDate, SecondDate

-- 15. Pairs of Dates within 12 Hours

SELECT a.Date AS [FirstDate]
	   , a2.Date AS [SecondDate]
  FROM Ads AS a 
  JOIN Ads AS a2 ON a.Id != a2.Id
 WHERE a.Date < a2.Date
   AND a2.Date <= DATEADD(HOUR, 12, a.Date)
 --AND DATEDIFF(HOUR, a.Date, a2.Date) < 12
 ORDER BY FirstDate, SecondDate

-- 16. Ads by Country
CREATE TABLE Countries
(
	Id INT IDENTITY,
	Name NVARCHAR(50)
	CONSTRAINT PK_Countries PRIMARY KEY (Id)
)

ALTER TABLE Towns
ADD CountryId INT

ALTER TABLE Towns
ADD CONSTRAINT FK_Towns_Countries FOREIGN KEY (CountryId)
REFERENCES Countries(Id)

INSERT INTO Countries(Name) 
     VALUES ('Bulgaria'), ('Germany'), ('France')
UPDATE Towns 
   SET CountryId = (SELECT Id FROM Countries WHERE Name='Bulgaria')
INSERT INTO Towns VALUES
('Munich', (SELECT Id FROM Countries WHERE Name='Germany')),
('Frankfurt', (SELECT Id FROM Countries WHERE Name='Germany')),
('Berlin', (SELECT Id FROM Countries WHERE Name='Germany')),
('Hamburg', (SELECT Id FROM Countries WHERE Name='Germany')),
('Paris', (SELECT Id FROM Countries WHERE Name='France')),
('Lyon', (SELECT Id FROM Countries WHERE Name='France')),
('Nantes', (SELECT Id FROM Countries WHERE Name='France'))

UPDATE Ads
   SET TownId = (SELECT Id FROM Towns WHERE Name = 'Paris')
 WHERE DATENAME(WEEKDAY, Date) = 'Friday' 
 
UPDATE Ads
   SET TownId = (SELECT Id FROM Towns WHERE Name = 'Hamburg')
 WHERE DATENAME(WEEKDAY, Date) = 'Thursday' 

DELETE FROM Ads
 WHERE OwnerId IN (
					SELECT u.Id 
					FROM AspNetUsers AS u
					JOIN AspNetUserRoles AS ur ON u.Id = ur.UserId
					JOIN AspNetRoles AS r ON ur.RoleId = r.Id
					WHERE r.Name = 'Partner'
	 			  )	

 -- DELETE FROM Ads
	--	FROM Ads a
	--  JOIN AspNetUsers u ON a.OwnerId = u.Id
	--	JOIN AspNetUserRoles ur ON u.Id = ur.UserId
	--	JOIN AspNetRoles r ON r.Id = ur.RoleId
 -- WHERE r.Name = 'Partner'

INSERT INTO Ads(Title, Text, Date, OwnerId, StatusId)
	 VALUES (
			  'Free Book'
		      , 'Free C# Book'
			  , GETDATE()
			  , (SELECT Id FROM AspNetUsers WHERE Name = 'nakov')
			  , (SELECT Id FROM AdStatuses WHERE Status = 'Waiting Approval')
	 	    )

SELECT t.Name AS [Town]
       , c.Name AS [Country]
	   , COUNT(a.Id) AS [AdsCount]
  FROM Ads AS a
  FULL JOIN Towns AS t ON a.TownId = t.Id
  FULL JOIN Countries AS c ON t.CountryId = c.Id
 GROUP BY t.Name, c.Name
 ORDER BY t.Name, c.Name

-- 17. Create a View and a Stored Function

CREATE VIEW v_AllAds
AS
SELECT a.Id, a.Title, u.Name AS [Author], a.Date, t.Name AS [Town], c.Name AS [Category], ads.Status
  FROM Ads AS a
  LEFT JOIN Categories AS c ON a.CategoryId = c.Id
  LEFT JOIN AspNetUsers AS u ON a.OwnerId = u.Id
  LEFT JOIN Towns AS t  ON a.TownId = t.Id
  LEFT JOIN AdStatuses AS ads ON a.StatusId = ads.Id

IF (OBJECT_ID('fn_ListUsersAds') IS NOT NULL)
DROP FUNCTION fn_ListUsersAds
GO

CREATE FUNCTION fn_ListUsersAds()
RETURNS @tbl_UsersAds TABLE
(
	UserName NVARCHAR(MAX),
	AdDates NVARCHAR(MAX)
)
AS
BEGIN
	DECLARE UsersCursor CURSOR FOR
		SELECT UserName 
		  FROM AspNetUsers
		 ORDER BY UserName DESC
	OPEN UsersCursor
	
	DECLARE @username NVARCHAR(MAX)
	FETCH NEXT FROM UsersCursor INTO @username

	WHILE @@FETCH_STATUS = 0
	BEGIN
		 DECLARE @dates NVARCHAR(MAX) = NULL
		  SELECT @dates = 
				 CASE
	 				WHEN @dates IS NULL THEN CONVERT(NVARCHAR(MAX), Date, 112)
					ELSE @dates + '; ' + CONVERT(NVARCHAR(MAX), Date, 112)
				 END
		    FROM v_AllAds AS a
		   WHERE a.Author = @username
		   ORDER BY Date

		 INSERT INTO @tbl_UsersAds
		 VALUES(@username, @dates)
		
		 FETCH NEXT FROM UsersCursor INTO @username
	END

	CLOSE UsersCursor
	DEALLOCATE UsersCursor
	RETURN
END
GO

SELECT * FROM fn_ListUsersAds()

-- 17. Create a View and a Stored Function

CREATE VIEW v_AllAds
AS
SELECT a.Id, a.Title, u.Name AS [Author], a.Date, t.Name AS [Town], c.Name AS [Category], ads.Status
  FROM Ads AS a
  LEFT JOIN Categories AS c ON a.CategoryId = c.Id
  LEFT JOIN AspNetUsers AS u ON a.OwnerId = u.Id
  LEFT JOIN Towns AS t ON a.TownId = t.Id
  LEFT JOIN AdStatuses AS ads ON a.StatusId = ads.Id

IF (OBJECT_ID('fn_ListUsersAds') IS NOT NULL)
DROP FUNCTION fn_ListUsersAds
GO

CREATE FUNCTION fn_ListUsersAds()
RETURNS @tbl_UsersAds TABLE
(
	UserName NVARCHAR(MAX),
	AdDates NVARCHAR(MAX)
)
AS
BEGIN
	DECLARE UsersCursor CURSOR FOR
		SELECT UserName 
		  FROM AspNetUsers
		 ORDER BY UserName DESC
	OPEN UsersCursor
	
	DECLARE @username NVARCHAR(MAX)
	FETCH NEXT FROM UsersCursor INTO @username

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @dates NVARCHAR(MAX) = NULL
		SET @dates = 
			   (
			     SELECT STUFF
							  (
								 (
								   SELECT '; ' + FORMAT (Date, 'yyyyMMdd')
									 FROM v_AllAds AS a 
								    WHERE a.Author = @username	
								      FOR XML PATH('')
								 ),
							     1, 2, ''
							  )
			   )
			 
		INSERT INTO @tbl_UsersAds
		VALUES(@username, @dates)
		
		FETCH NEXT FROM UsersCursor INTO @username
	END

	CLOSE UsersCursor
	DEALLOCATE UsersCursor
	RETURN
END
GO

SELECT * FROM fn_ListUsersAds()

-- 18. Design a Database Schema in MSSQL and Write a Query

CREATE TABLE Products
(
	Id INT NOT NULL IDENTITY PRIMARY KEY,
	Name NVARCHAR(50),
	Price DECIMAL(10,2)
)

CREATE TABLE Customers
(
	Id INT NOT NULL IDENTITY PRIMARY KEY,
	Name NVARCHAR(500)
)

CREATE TABLE Orders
(
	Id INT NOT NULL IDENTITY PRIMARY KEY,
	CustomerId INT,
	Name NVARCHAR(500),
	Date DATETIME
	CONSTRAINT FK_Orders_Customers FOREIGN KEY(CustomerId) 
	REFERENCES Customers(Id)
)

CREATE TABLE OrderItems
(
	Id INT NOT NULL IDENTITY PRIMARY KEY,
	OrderId INT,
	ProductId INT,
	Quantity DECIMAL(10,2),
	CONSTRAINT FK_OrderItems_OrderId FOREIGN KEY(OrderId) 
	REFERENCES Orders(Id),
	CONSTRAINT FK_OrderItems_ProductId FOREIGN KEY(ProductId) 
	REFERENCES Products(Id) 
)

INSERT INTO Products(Name, Price) VALUES ('beer',1.20), ('cheese',9.50), ('rakiya',12.40), ('salami',6.33), ('tomatos',2.50), ('cucumbers',1.35), ('water',0.85), ('apples',0.75);
INSERT INTO Customers(Name) VALUES ('Peter'), ('Maria'), ('Nakov'), ('Vlado');
INSERT INTO Orders(CustomerId, Date) VALUES (1,'2015-02-13 13:47:04'), (2,'2015-02-14 22:03:44'), (3,'2015-02-18 09:22:01'), (4,'2015-02-11 20:17:18');

SET IDENTITY_INSERT OrderItems ON
INSERT INTO OrderItems(Id, OrderId, ProductId, Quantity) VALUES (12,4,6,2.00), (13,3,2,4.00), (14,3,5,1.50), (15,2,1,6.00), (16,2,3,1.20), (17,1,2,1.00), (18,1,3,1.00), (19,1,4,2.00), (20,1,5,1.00), (21,3,1,4.00), (22,1,1,3.00);

SELECT p.Name AS [ProductName]
       , COUNT(oi.ProductId) AS [NumOrders]
	   , ISNULL(SUM(oi.Quantity), 0) AS [Quantity]
	   , p.Price AS [Price]
	   , ISNULL(SUM(oi.Quantity) * p.Price, 0) AS [TotalPrice] 
  FROM Products AS p
  LEFT JOIN OrderItems AS oi ON p.Id = oi.ProductId
  LEFT JOIN Orders AS o ON oi.OrderId = o.Id
 GROUP BY p.Name, p.Price
 ORDER BY p.Name