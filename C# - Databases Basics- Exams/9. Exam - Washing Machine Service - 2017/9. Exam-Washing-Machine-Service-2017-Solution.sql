-- 1. Data Definition

CREATE TABLE Clients
(
	ClientId INT NOT NULL PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Phone CHAR(12) NOT NULL
  --Phone NVARCHAR(12) NOT NULL CHECK (LEN(Phone) = 12)
)

CREATE TABLE Mechanics
(
	MechanicId INT NOT NULL PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Address NVARCHAR(255) NOT NULL
)

CREATE TABLE Models
(
	ModelId INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(50) NOT NULL UNIQUE
)

CREATE TABLE Vendors
(
	VendorId INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(50) NOT NULL UNIQUE
)

CREATE TABLE Parts
(
	PartId INT NOT NULL PRIMARY KEY IDENTITY,
	SerialNumber NVARCHAR(50) NOT NULL UNIQUE,
	Description NVARCHAR(255),
	Price MONEY CHECK (Price > 0 AND Price < 99999.99),
	VendorId INT NOT NULL,
	CONSTRAINT FK_Parts_Vendors FOREIGN KEY(VendorId)
	REFERENCES Vendors(VendorId),
	StockQty INT NOT NULL CHECK (StockQty > -1) DEFAULT 0
)

CREATE TABLE Jobs
(
	JobId INT NOT NULL PRIMARY KEY IDENTITY,
	ModelId INT NOT NULL,
	CONSTRAINT FK_Jobs_Models FOREIGN KEY(ModelId)
	REFERENCES Models(ModelId),IssueDate DATE NOT NULL,
	Status NVARCHAR(11) CHECK (Status IN ('Pending', 'In Progress', 'Finished')) DEFAULT 'Pending',
	ClientId INT NOT NULL,
	CONSTRAINT FK_Jobs_Clients FOREIGN KEY(ClientId)
	REFERENCES Clients(ClientId),
	FinishDate DATE,
	MechanicId INT,
	CONSTRAINT FK_Jobs_Mechanics FOREIGN KEY(MechanicId)
	REFERENCES Mechanics(MechanicId)
)

CREATE TABLE Orders
(
	OrderId INT NOT NULL PRIMARY KEY IDENTITY,
	JobId INT NOT NULL,
	CONSTRAINT FK_Orders_Jobs FOREIGN KEY(JobId)
	REFERENCES Jobs(JobId),	
	IssueDate DATE,
	Delivered BIT NOT NULL DEFAULT 0
)

CREATE TABLE OrderParts
(
	OrderId INT NOT NULL,
	PartId INT NOT NULL,
	CONSTRAINT PK_OrderParts PRIMARY KEY(OrderId, PartId),
	CONSTRAINT FK_OrderParts_Orders FOREIGN KEY(OrderId)
	REFERENCES Orders(OrderId),
	CONSTRAINT FK_OrderParts_Parts FOREIGN KEY(PartId)
	REFERENCES Parts(PartId),
	Quantity INT NOT NULL CHECK (Quantity > 0) DEFAULT 1
)

CREATE TABLE PartsNeeded
(
	JobId INT NOT NULL,
	PartId INT NOT NULL,
	CONSTRAINT PK_PartsNeeded PRIMARY KEY(JobId, PartId),
	CONSTRAINT FK_PartsNeeded_Jobs FOREIGN KEY(JobId)
	REFERENCES Jobs(JobId),
	CONSTRAINT FK_PartsNeeded_Parts FOREIGN KEY(PartId)
	REFERENCES Parts(PartId),
	Quantity INT NOT NULL CHECK (Quantity > 0) DEFAULT 1
)

-- 2. Data Insertion

INSERT INTO Clients (FirstName, LastName, Phone) 
      VALUES ('Teri', 'Ennaco', '570-889-5187'),
	         ('Merlyn', 'Lawler', '201-588-7810'),
	         ('Georgene', 'Montezuma', '925-615-5185'),
	         ('Jettie', 'Mconnell', '908-802-3564'),
	         ('Lemuel', 'Latzke', '631-748-6479'),
	         ('Melodie', 'Knipp', '805-690-1682'),
	         ('Candida', 'Corbley', '908-275-8357')

INSERT INTO Parts (SerialNumber, Description, Price, VendorId) 
     VALUES ('WP8182119', 'Door Boot Seal', 117.86, 2),
	        ('W10780048', 'Suspension Rod', 42.81, 1),
	        ('W10841140', 'Silicone Adhesive', 6.77, 4),
	        ('WPY055980', 'High Temperature Adhesive', 13.94, 3)

-- 3. Update 

UPDATE Jobs
   SET Status = 'In Progress', MechanicId = 3
 WHERE Status = 'Pending'

-- 4. Delete

 DELETE OrderParts
 WHERE OrderId = 19

DELETE Orders
 WHERE OrderId = 19
 
-- 5. Clients By Name

 SELECT FirstName, LastName, Phone
   FROM Clients
  ORDER BY LastName, ClientId

-- 6. Job Status

SELECT Status, IssueDate
  FROM Jobs
 WHERE Status != 'Finished'
 ORDER BY IssueDate, JobId

-- 7. Mechanic Assignments

 SELECT CONCAT(FirstName, ' ', LastName) AS [Mechanic]
		, j.Status
		, j.IssueDate
  FROM Mechanics AS m
  JOIN Jobs AS j ON m.MechanicId = j.MechanicId
 ORDER BY m.MechanicId, j.IssueDate, j.JobId

-- 8. Current Clients

SELECT CONCAT(FirstName, ' ', LastName) AS [Client]
	   , DATEDIFF(DAY, j.IssueDate, '2017-04-24') AS [Days going]
	   , j.Status 
  FROM Clients AS c
  JOIN Jobs AS j ON c.ClientId = j.ClientId
 WHERE j.Status != 'Finished'
 ORDER BY [Days going] DESC, c.ClientId

-- 9. Mechanic Performance

SELECT CONCAT(FirstName, ' ', LastName) AS [Mechanic]
	   , AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate)) AS [Average days] 
  FROM Mechanics AS m
  JOIN Jobs AS j ON m.MechanicId = j.MechanicId
 GROUP BY CONCAT(FirstName, ' ', LastName), m.MechanicId
 ORDER BY m.MechanicId

-- 10. Hard Earners

SELECT CONCAT(FirstName, ' ', LastName) AS [Mechanic]
	   , COUNT(j.MechanicId) AS [Jobs]
  FROM Mechanics AS m
  JOIN Jobs AS j ON m.MechanicId = j.MechanicId
 WHERE j.Status != 'Finished'
 GROUP BY CONCAT(FirstName, ' ', LastName), m.MechanicId
HAVING COUNT(j.MechanicId) > 1
 ORDER BY [Jobs] DESC, m.MechanicId

-- 11. Available Mechanics

-- Only passes the zero test

SELECT [Group1].Name AS [Mechanic]
  FROM
	  	(
		  SELECT m.MechanicId
			 	 , CONCAT(m.FirstName, ' ', m.LastName) AS [Name]
				 , COUNT(j.JobId) AS [NumberOfJobs]
			FROM Mechanics AS m
			LEFT JOIN Jobs AS j ON m.MechanicId = j.MechanicId
		   WHERE j.Status = 'Finished'
		   GROUP BY m.MechanicId, CONCAT(m.FirstName, ' ', m.LastName)
		) AS [Group1]
		JOIN 
		(
		  SELECT m.MechanicId
				 , CONCAT(m.FirstName, ' ', m.LastName) AS [Name]
				 , COUNT(j.JobId) AS [NumberOfJobs]
		    FROM Mechanics AS m
		    LEFT JOIN Jobs AS j ON m.MechanicId = j.MechanicId
		   GROUP BY m.MechanicId, CONCAT(m.FirstName, ' ', m.LastName)
		) AS [Group2]
		ON [Group1].NumberOfJobs = [Group2].NumberOfJobs
 ORDER BY [Group1].MechanicId

-- 12. Parts Cost

SELECT ISNULL(SUM(p.Price  * op.Quantity), 0.00) AS [Parts Total]
  FROM Orders AS o
  JOIN OrderParts AS op ON o.OrderId = op.OrderId
  JOIN Parts AS p ON op.PartId = p.PartId
 WHERE DATEDIFF(DAY, o.IssueDate, '2017-04-24') <= 21

-- 13. Past Expenses

SELECT j.JobId
	   , ISNULL(SUM(p.Price  * op.Quantity), 0.00) AS [Total]
  FROM Orders AS o
  JOIN OrderParts AS op ON o.OrderId = op.OrderId
  JOIN Parts AS p ON op.PartId = p.PartId
  RIGHT JOIN Jobs AS j ON o.JobId = j.JobId
 WHERE j.Status = 'Finished'
 GROUP BY j.JobId
 ORDER BY [Total] DESC, j.JobId

-- 14. Model Repair Time

SELECT m.ModelId
	   , m.Name
	   , CONCAT(AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate)), ' days') [Average Service Time]
  FROM Models AS m
  JOIN Jobs AS j ON m.ModelId = j.ModelId
 GROUP BY m.ModelId, m.Name
 ORDER BY [Average Service Time]

-- 15. Faultiest Model

-- Only finds the max Times Serviced, but I don't know how to extract Parts Total

SELECT m.Name AS [Model]
	   , COUNT(j.ModelId) AS [Times Serviced]
	--   , SUM(p.Price * op.Quantity) AS [Parts Total]
  FROM Models AS m
  JOIN Jobs AS j ON m.ModelId = j.ModelId
 GROUP BY m.Name

-- 16. Missing Parts

-- Only finds the needed parts in a bigger list of parts

SELECT p.PartId, p.Description, pn.Quantity, p.StockQty, op.Quantity
  FROM Jobs AS j 
  JOIN PartsNeeded AS pn ON j.JobId = pn.JobId
  JOIN Parts AS p ON pn.PartId = p.PartId
  JOIN OrderParts AS op ON p.PartId = op.PartId
  JOIN Orders AS o ON op.OrderId = o.OrderId
WHERE j.Status != 'Finished'

-- 17. Cost Of Order 

CREATE FUNCTION [dbo].[udf_GetCost](@JobId INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
	DECLARE @Result DECIMAL(10,2);
	SET @Result = ISNULL((
						  SELECT SUM(p.Price * op.Quantity)
						    FROM Jobs AS j
						    JOIN Orders AS o ON j.JobId = o.JobId
						    JOIN OrderParts AS op ON o.OrderId = op.OrderId
						    JOIN Parts AS p ON op.PartId = p.PartId
						   WHERE o.JobId = @JobId
						   GROUP BY o.JobId
				        ), 0)
	RETURN CAST(@Result AS DECIMAL(10,2));
END

-- 18. Place Order



-- 19. Detect Delivery

