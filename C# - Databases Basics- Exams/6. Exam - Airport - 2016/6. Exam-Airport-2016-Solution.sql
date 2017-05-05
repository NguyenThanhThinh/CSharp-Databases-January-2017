-- 1. Data Definition

CREATE TABLE Flights
(
	FlightID INT NOT NULL PRIMARY KEY,
	DepartureTime DATETIME NOT NULL,
	ArrivalTime DATETIME NOT NULL,
	Status VARCHAR(9) NOT NULL,
	OriginAirportID INT NOT NULL,
	DestinationAirportID INT NOT NULL,
	AirlineID INT NOT NULL,
	CONSTRAINT FK_Flights_Airports_Origin FOREIGN KEY(OriginAirportID)
	REFERENCES Airports(AirportID),
	CONSTRAINT FK_Flights_Airports_Destination FOREIGN KEY(DestinationAirportID)
	REFERENCES Airports(AirportID),
	CONSTRAINT FK_Flights_Airlines FOREIGN KEY(AirlineID)
	REFERENCES Airlines(AirlineID),
	CONSTRAINT check_Status CHECK (Status IN ('Departing', 'Delayed', 'Arrived', 'Cancelled'))
)

CREATE TABLE Tickets
(
	TicketID INT NOT NULL PRIMARY KEY,
	Price DECIMAL(8,2) NOT NULL,
	Class VARCHAR(6) NOT NULL,
	Seat VARCHAR(5) NOT NULL,
	CustomerID INT NOT NULL,
	FlightID INT NOT NULL,
	CONSTRAINT FK_Tickets_Customers FOREIGN KEY(CustomerID)
	REFERENCES Customers(CustomerID),
	CONSTRAINT FK_Tickets_Flights FOREIGN KEY(FlightID)
	REFERENCES Flights(FlightID),
	CONSTRAINT check_Class CHECK (Class IN ('First', 'Second', 'Third'))
)

-- 2. Data Insertion

INSERT INTO Flights(FlightID, DepartureTime, ArrivalTime, Status, OriginAirportID, DestinationAirportID, AirlineID)
     VALUES (1,	'2016-10-13 06:00 AM', '2016-10-13 10:00 AM', 'Delayed', 1,4, 1),
			(2,	'2016-10-12 12:00 PM', '2016-10-12 12:01 PM', 'Departing', 1, 3, 2),
			(3,	'2016-10-14 03:00 PM', '2016-10-20 04:00 AM', 'Delayed', 4,	2, 4),
			(4,	'2016-10-12 01:24 PM', '2016-10-12 4:31 PM', 'Departing', 3, 1, 3),
			(5,	'2016-10-12 08:11 AM', '2016-10-12 11:22 PM', 'Departing', 4, 1, 1),
			(6,	'1995-06-21 12:30 PM', '1995-06-22 08:30 PM', 'Arrived', 2,	3, 5),
			(7,	'2016-10-12 11:34 PM', '2016-10-13 03:00 AM', 'Departing', 2, 4, 2),
			(8,	'2016-11-11 01:00 PM', '2016-11-12 10:00 PM', 'Delayed', 4, 3, 1),
			(9,	'2015-10-01 12:00 PM', '2015-12-01 01:00 AM', 'Arrived', 1,	2, 1),
			(10,'2016-10-12 07:30 PM', '2016-10-13 12:30 PM', 'Departing', 2, 1, 7)

INSERT INTO Tickets(TicketID, Price, Class, Seat, CustomerID, FlightID)
     VALUES (1, 3000.00, 'First', '233-A', 3, 8),
			(2, 1799.90, 'Second', '123-D', 1, 1),
			(3, 1200.50, 'Second', '12-Z', 2, 5),
			(4, 410.68, 'Third', '45-Q', 2, 8),
			(5, 560.00, 'Third', '201-R', 4, 6),
			(6, 2100.00, 'Second', '13-T', 1, 9),
			(7, 5500.00, 'First', '98-O', 2, 7)

-- 3. Update Flights

UPDATE Flights
   SET AirlineID = 1
 WHERE Status = 'Arrived'


-- 4. Update Tickets

UPDATE Tickets
   SET Price *= 1.5
 WHERE FlightID IN (
					SELECT f.FlightID 
					  FROM Flights AS f
					  JOIN Airlines AS a ON f.AirlineID = a.AirlineID
					 WHERE a.Rating = (SELECT MAX(Rating) FROM Airlines)
				   )

-- 4. Update Tickets 2

UPDATE Tickets
   SET Price *= 1.5
 WHERE FlightID IN (
					 SELECT f.FlightID 
					   FROM Flights AS f
					  WHERE f.AirlineID = (SELECT TOP (1) a.AirlineID FROM Airlines AS a ORDER BY a.Rating DESC)
				   ) 

-- 5. Table Creation

CREATE TABLE CustomerReviews
(
	ReviewID INT NOT NULL PRIMARY KEY,
	ReviewContent VARCHAR(255) NOT NULL,
	ReviewGrade INT NOT NULL CHECK(ReviewGrade BETWEEN 0 AND 10),
	AirlineID INT NOT NULL,
	CustomerID INT NOT NULL,
	CONSTRAINT FK_CustomerReviews_Airlines FOREIGN KEY(AirlineID)
	REFERENCES Airlines(AirlineID),
	CONSTRAINT FK_CustomerReviews_Customers FOREIGN KEY(CustomerID)
	REFERENCES Customers(CustomerID)
)

CREATE TABLE CustomerBankAccounts
(
	AccountID INT NOT NULL PRIMARY KEY,
	AccountNumber VARCHAR(10) NOT NULL UNIQUE,
	Balance DECIMAL(10,2) NOT NULL,
	CustomerID INT NOT NULL,
	CONSTRAINT FK_CustomerBankAccounts_Customers FOREIGN KEY(CustomerID)
	REFERENCES Customers(CustomerID)
)   

-- 6. Fillin New Tables

INSERT INTO CustomerReviews(ReviewID, ReviewContent, ReviewGrade, AirlineID, CustomerID)
     VALUES (1, 'Me is very happy. Me likey this airline. Me good.', 10, 1, 1),
			(2, 'Ja, Ja, Ja... Ja, Gut, Gut, Ja Gut! Sehr Gut!', 10, 1, 4),
			(3, 'Meh...', 5, 4, 3),
			(4, 'Well Ive seen better, but Ive certainly seen a lot worse...', 7, 3, 5)

INSERT INTO CustomerBankAccounts(AccountID, AccountNumber, Balance, CustomerID)
     VALUES (1, '123456790', 2569.23, 1),
			(2, '18ABC23672', 14004568.23, 2),
			(3, 'F0RG0100N3', 19345.20,	5)

-- 7. Extract All Tickets

SELECT TicketID
	   , Price
       , Class
       , Seat
  FROM Tickets
 ORDER BY TicketID

-- 8. Extract All Customers

SELECT CustomerID
       , CONCAT(FirstName, ' ', LastName) AS [FullName]
       , Gender
  FROM Customers
 ORDER BY [FullName], CustomerID

-- 9. Extract Delayed Flights

SELECT FlightID
       , DepartureTime
       , ArrivalTime
  FROM Flights
 WHERE Status = 'Delayed'
 ORDER BY FlightID

-- 10. Top 5 Airlines

SELECT TOP (5) 
       a.AirlineID
       , a.AirlineName
       , a.Nationality
       , a.Rating
  FROM Airlines AS a, Flights AS f
 WHERE f.AirlineID = a.AirlineID
 GROUP BY a.AirlineID, a.AirlineName, a.Nationality, a.Rating 
 -- or JOIN Airlines and Flights on the same WHERE condition
 -- or using DISTINCT instead of GROUP BY
 ORDER BY Rating DESC, AirlineID

-- 11. All Tickets Below 5000

SELECT t.TicketID
       , a.AirportName AS [Destination]
       , CONCAT(c.FirstName, ' ', c.LastName) AS [CustomerName]
  FROM Tickets AS t
  JOIN Customers AS c ON t.CustomerID = c.CustomerID
  JOIN Flights AS f ON t.FlightID = f.FlightID
  JOIN Airports As a ON f.DestinationAirportID = a.AirportID 
 WHERE t.Price < 5000
   AND t.Class = 'First'
 ORDER BY t.TicketID

-- 12. Customers From Home

SELECT c.CustomerID
       , CONCAT(c.FirstName, ' ', c.LastName) AS [FullName]
       , t.TownName AS [HomeTown]
  FROM Customers AS c
  JOIN Tickets AS tick ON c.CustomerID = tick.CustomerID
  JOIN Flights AS f ON tick.FlightID = f.FlightID
  JOIN Airports AS a ON f.OriginAirportID = a.AirportID
  JOIN Towns AS t ON a.TownID = t.TownID
 WHERE c.HomeTownID = t.TownID
   AND f.Status = 'Departing'
 ORDER BY c.CustomerID

-- 13. Customers who will fly

SELECT DISTINCT 
	   c.CustomerID
	   , CONCAT(c.FirstName, ' ', c.LastName) AS [FullName]
	   , DATEDIFF(YEAR, c.DateOfBirth, '2016-01-01') AS [Age] 
  FROM Customers AS c
  JOIN Tickets AS t ON c.CustomerID = t.CustomerID
  JOIN Flights AS f ON t.FlightID = f.FlightID
 WHERE f.Status = 'Departing'
 ORDER BY [Age], c.CustomerID

-- 13. Customers who will fly

SELECT DISTINCT 
	   c.CustomerID
       , CONCAT(c.FirstName, ' ', c.LastName) AS [FullName]
       , DATEDIFF(YEAR, c.DateOfBirth, '2016-01-01') AS [Age] 
  FROM Customers AS c, Tickets AS t, Flights AS f
 WHERE c.CustomerID = t.CustomerID 
   AND t.FlightID = f.FlightID
   AND f.Status = 'Departing'
 ORDER BY [Age], c.CustomerID

-- 14. Top 3 Customers Delayed

SELECT TOP (3) 
	   c.CustomerID
       , CONCAT(c.FirstName, ' ', c.LastName) AS [FullName]
       , t.Price AS [TicketPrice]
       , a.AirportName AS [Destination]
  FROM Customers AS c
  JOIN Tickets AS t ON c.CustomerID = t.CustomerID
  JOIN Flights AS f ON t.FlightID = f.FlightID
  JOIN Airports AS a ON f.DestinationAirportID = a.AirportID
 WHERE f.Status = 'Delayed'
 ORDER BY [TicketPrice] DESC, c.CustomerID

-- 15. Last 5 Departing Flights

SELECT result.FlightID
       , result.DepartureTime
       , result.ArrivalTime
       , result.Origin
       , result.Destination
  FROM (
		SELECT TOP (5) 
			   f.FlightID
               , f. DepartureTime
               , f.ArrivalTime
               , a.AirportName AS [Origin]
               , a2.AirportName AS [Destination]
		  FROM Flights AS f
		  JOIN Airports AS a ON f.OriginAirportID = a.AirportID
		  JOIN Airports AS a2 ON f.DestinationAirportID = a2.AirportID
		 WHERE f.Status = 'Departing'
		 ORDER BY f.DepartureTime DESC, f.FlightID 
		) AS result
 ORDER BY result.DepartureTime

-- 16. Customers Below 21

SELECT DISTINCT 
       c.CustomerID
	   , CONCAT(c.FirstName, ' ', c.LastName) AS [FullName]
       , DATEDIFF(YEAR, c.DateOfBirth, '2016-01-01') AS [Age] 
  FROM Customers AS c
  JOIN Tickets AS t ON c.CustomerID = t.CustomerID
  JOIN Flights AS f ON t.FlightID = f.FlightID
 WHERE f.Status = 'Arrived'
   AND DATEDIFF(YEAR, c.DateOfBirth, '2016-01-01') < 21
 ORDER BY [Age] DESC, c.CustomerID

-- 17. Airports and Passengers

SELECT a.AirportID
	   , a.AirportName
       , COUNT(c.CustomerID) AS [Passengers]
  FROM Airports AS a
  JOIN Flights AS f ON a.AirportID = f.OriginAirportID
  JOIN Tickets AS t ON f.FlightID = t.FlightID
  JOIN Customers AS c ON t.CustomerID = c.CustomerID
 WHERE f.Status = 'Departing'
 GROUP BY a.AirportID, a.AirportName
 ORDER BY a.AirportID

 -- 18. Submit Review

CREATE PROCEDURE usp_SubmitReview(@CustomerID INT, @ReviewContent VARCHAR(255), @ReviewGrade INT, @AirlineName VARCHAR(30))
AS
BEGIN
    BEGIN TRANSACTION
   
    DECLARE @Index INT = ISNULL((SELECT MAX(ReviewID) FROM CustomerReviews), 0) + 1
   
    DECLARE @AirlineId INT = (
                               SELECT AirlineID
                                 FROM Airlines
                                WHERE AirlineName = @AirlineName
                             )
    IF @AirlineId IS NULL
    BEGIN
        ROLLBACK
        RAISERROR('Airline does not exist.', 16, 1)
        RETURN
    END
   
    INSERT INTO CustomerReviews(ReviewID, ReviewContent, ReviewGrade, AirlineID, CustomerID)
         VALUES (
                    @Index
                  , @ReviewContent
                  , @ReviewGrade
                  , @AirlineId
                  , @CustomerID
                )
    COMMIT
END

-- 19. Ticket Purchase

CREATE PROCEDURE usp_PurchaseTicket(@CustomerID INT, @FlightID INT, @TicketPrice DECIMAL(8,2), @Class VARCHAR(6), @Seat VARCHAR(5))
AS
BEGIN
	BEGIN TRANSACTION
	DECLARE @CustomerBalance DECIMAL(10,2) 
		SET @CustomerBalance = (
								 SELECT Balance
								   FROM CustomerBankAccounts
							      WHERE CustomerID = @CustomerID
							   )

	IF @CustomerBalance < @TicketPrice OR @CustomerBalance IS NULL 
	BEGIN
		ROLLBACK
		RAISERROR('Insufficient bank account balance for ticket purchase.', 16, 1)
		RETURN
	END
	
	DECLARE @NextTicketID INT = ISNULL((SELECT COUNT(TicketID)FROM Tickets), 0) + 1

	INSERT INTO Tickets (TicketID, Price, Class, Seat, CustomerID, FlightID)
		 VALUES (
					@NextTicketID
				  , @TicketPrice
				  , @Class
				  , @Seat
				  , @CustomerID
				  , @FlightID
				)

	UPDATE CustomerBankAccounts
	   SET Balance -= @TicketPrice
	 WHERE CustomerID = @CustomerID

	COMMIT
END

-- 20. Update Trigger

CREATE TRIGGER tr_ArrivedFlights
ON Flights  
AFTER UPDATE  
AS
BEGIN
  
INSERT INTO ArrivedFlights
SELECT i.FlightID
       , i.ArrivalTime
       , a.AirportName AS [Origin]
       , a2.AirportName AS [Destination]
       , (SELECT COUNT(*) FROM Tickets AS t WHERE t.FlightID in (SELECT FlightID FROM INSERTED)) AS [Passengers]
  FROM INSERTED AS i
  JOIN Airports AS a ON a.AirportID = i.OriginAirportID
  JOIN Airports AS a2 ON i.DestinationAirportID = a2.AirportID
  JOIN DELETED AS d ON i.FlightID = d.FlightID
 WHERE i.Status = 'Arrived'
   AND d.Status != 'Arrived'
    -- adding to ArrivedFlights only the flights with changes in the status
END

-- 20. Update Trigger 2

CREATE TRIGGER tr_ArrivedFlights
ON Flights
AFTER UPDATE
AS
BEGIN

DECLARE @NewFlightStatus VARCHAR(9) = (SELECT DISTINCT i.Status FROM INSERTED AS i)

IF (@NewFlightStatus = 'Arrived')
BEGIN
	INSERT INTO ArrivedFlights(FlightID, ArrivalTime, Origin, Destination, Passengers)
	SELECT * FROM
	(
	  SELECT 
			i.FlightID
			, i.ArrivalTime
			, (
				SELECT AirportName 
				  FROM Airports AS a 
				  JOIN Flights AS f ON a.AirportID = f.OriginAirportID
				 WHERE f.FlightID = i.FlightID
			  ) AS [Origin]
			, (
				SELECT AirportName 
				  FROM Airports AS a 
				  JOIN Flights AS f ON AirportID = f.DestinationAirportID  
				 WHERE f.FlightID = i.FlightID
			  ) AS [Destination] 
			, (
				SELECT COUNT(t.TicketID) 
				  FROM Tickets AS t 
				  JOIN Flights AS f ON t.FlightID = f.FlightID  
				 WHERE f.FlightID = i.FlightID
			  ) AS [Passengers] 
		FROM INSERTED AS i
	) AS result
END
END

-- 20. Update Trigger 3

CREATE TRIGGER tr_ArrivedFlights
ON Flights  
AFTER UPDATE  
AS
BEGIN
 
IF(
	(SELECT COUNT(*)
	   FROM INSERTED AS i
	   JOIN DELETED AS d on i.FlightID = d.FlightID
	  WHERE i.Status = 'Arrived' and d.Status <> 'Arrived') = 0 
  )

RETURN
 
INSERT INTO ArrivedFlights
SELECT FlightID
	   , ArrivalTime
	   , a.AirportName
	   , a2.AirportName
	   , (SELECT COUNT(*) FROM Flights AS f
						  JOIN Tickets AS t ON t.FlightID = f.FlightID
						 WHERE t.FlightID in (SELECT FlightID FROM INSERTED))
  FROM INSERTED AS i
  JOIN Airports AS a ON a.AirportID = i.OriginAirportID
  JOIN Airports AS a2 ON i.DestinationAirportID = a2.AirportID
 
END