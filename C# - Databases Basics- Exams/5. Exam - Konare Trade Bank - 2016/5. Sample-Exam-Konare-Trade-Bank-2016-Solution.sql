-- 1. DDL Create Tables

CREATE TABLE DepositTypes
(
	DepositTypeID INT,
	Name VARCHAR(20),
	CONSTRAINT PK_DepositTypes PRIMARY KEY(DepositTypeID)
)

CREATE TABLE Deposits
(
	DepositID INT IDENTITY,
	Amount DECIMAL(10,2),
	StartDate DATE,
	EndDate DATE,
	DepositTypeID INT,
	CustomerID INT,
	CONSTRAINT PK_Deposits PRIMARY KEY(DepositID),
	CONSTRAINT FK_Deposits_DepositTypes FOREIGN KEY(DepositTypeID) 
	REFERENCES DepositTypes(DepositTypeID),
	CONSTRAINT FK_Deposits_Customers FOREIGN KEY(CustomerID) 
	REFERENCES Customers(CustomerID)
)

CREATE TABLE EmployeesDeposits
(
	EmployeeID INT,
	DepositID INT,
	CONSTRAINT PK_EmployeesDeposits PRIMARY KEY(EmployeeID, DepositID),
	CONSTRAINT FK_EmployeesDeposits_Employees FOREIGN KEY(EmployeeID) 
	REFERENCES Employees(EmployeeID),
	CONSTRAINT FK_EmployeesDeposits_Deposits FOREIGN KEY(DepositID) 
	REFERENCES Deposits(DepositID)
)

CREATE TABLE CreditHistory
(
	CreditHistoryID INT,
	Mark CHAR(1),
	StartDate DATE,
	EndDate DATE,
	CustomerID INT,
	CONSTRAINT PK_CreditHistory PRIMARY KEY(CreditHistoryID),
	CONSTRAINT FK_CreditHistory_Customers FOREIGN KEY(CustomerID) 
	REFERENCES Customers(CustomerID)
)

CREATE TABLE Payments
(
	PaymentID INT,
	Date DATE,
	Amount DECIMAL(10,2),
	LoanID INT,
	CONSTRAINT PK_Payments PRIMARY KEY(PaymentID),
	CONSTRAINT FK_Payments_Loans FOREIGN KEY(LoanID) 
	REFERENCES Loans(LoanID)
)

CREATE TABLE Users
(
	UserID INT,
	UserName VARCHAR(20),
	Password VARCHAR(20),
	CustomerID INT UNIQUE,
	CONSTRAINT PK_Users PRIMARY KEY(UserID),
	CONSTRAINT FK_Users_Customers FOREIGN KEY(CustomerID) 
	REFERENCES Customers(CustomerID)
)

ALTER TABLE Employees
ADD ManagerID INT

ALTER TABLE Employees
ADD CONSTRAINT FK_Employees_Employees FOREIGN KEY(ManagerID)
REFERENCES Employees(EmployeeID)

-- 2. DML Insert data into the following tables 

INSERT INTO DepositTypes(DepositTypeID, Name)
	 VALUES (1,	'Time Deposit'),
			(2,	'Call Deposit'),
			(3,	'Free Deposit')

INSERT INTO Deposits(Amount, StartDate, EndDate, DepositTypeID, CustomerID)
SELECT CASE 
		    WHEN c.DateOfBirth > '01-01-1980' THEN 1000
			ELSE 1500
	   END
	   +
	   CASE 
			WHEN c.Gender = 'm' THEN 100
			WHEN c.Gender = 'f' THEN 200
	   END AS Amount,
	   GETDATE() AS StartDate,
	   NULL AS EndDate,
	   CASE	
		    WHEN c.CustomerID > 15 THEN 3
			ELSE
			 	 CASE
					 WHEN c.CustomerID % 2 = 1 THEN 1
					 WHEN c.CustomerID % 2 = 0 THEN 2
				 END
	   END AS DepositTypeID,
	   c.CustomerID
  FROM Customers AS c
 WHERE c.CustomerID < 20

INSERT INTO EmployeesDeposits
     VALUES (15, 4), (20, 15), (8, 7),
			(4, 8), (3, 13), (3, 8), (4, 10), 
			(10, 1), (13, 4), (14, 9) 

-- 3. DML Update Employees

UPDATE Employees
   SET ManagerID = CASE
					   WHEN EmployeeID >= 2 AND EmployeeID <= 10 THEN 1
					   WHEN EmployeeID >= 12 AND EmployeeID <= 20 THEN 11
					   WHEN EmployeeID >= 22 AND EmployeeID <= 30 THEN 21
					   WHEN EmployeeID IN (11, 21) THEN 1
				   END

-- 4. Delete Records

DELETE FROM EmployeesDeposits
 WHERE DepositID = 9 OR EmployeeID = 3

-- 5. Employees’ Salary

SELECT e.EmployeeID
	   , e.HireDate
       , e.Salary
       , e.BranchID
  FROM Employees AS e
 WHERE e.Salary > 2000 
   AND e.HireDate > '2009-06-15'

-- 6. Customer Age

SELECT c.FirstName
	   , c.DateOfBirth
	   , DATEDIFF(YEAR, c.DateOfBirth, '2016-10-01') AS [Age]
  FROM Customers AS c
 WHERE DATEDIFF(YEAR, c.DateOfBirth, '2016-10-01') BETWEEN 40 AND 50

-- 7. Customer City

SELECT c.CustomerID
  	   , c.FirstName
	   , c.LastName
	   , c.Gender
	   , ci.CityName
  FROM Customers AS c
  JOIN Cities AS ci ON c.CityID =  ci.CityID
 WHERE (c.LastName LIKE 'Bu%' OR c.FirstName LIKE '%a')
   AND LEN(ci.CityName) >= 8
 ORDER BY c.CustomerID

-- 8. Employee Accounts

SELECT TOP (5) 
	   e.EmployeeID
	   , e.FirstName
	   , a.AccountNumber
  FROM Employees AS e
  JOIN EmployeesAccounts AS ea ON e.EmployeeID = ea.EmployeeID
  JOIN Accounts AS a ON ea.AccountID = a.AccountID
 WHERE a.StartDate > '2012-01-01'
 ORDER BY e.FirstName DESC

-- 9. Employee Cities

SELECT c.CityName
	   , b.Name
	   , COUNT(e.BranchID) AS [EmployeesCount]
  FROM Cities AS c
  JOIN Branches AS b ON c.CityID = b.CityID
  JOIN Employees AS e ON b.BranchID = e.BranchID
 WHERE c.CityID != 4 AND c.CityID != 5
 GROUP BY c.CityName, b.Name
HAVING COUNT(e.BranchID) >= 3

-- 10. Loan Statistics

SELECT SUM(l.Amount) AS [TotalLoanAmount]
       , MAX(l.Interest) AS [MaxInterest]
	   , MIN(e.Salary) [MinEmployeeSalary]
  FROM Loans AS l
  JOIN EmployeesLoans AS el ON l.LoanID = el.LoanID
  JOIN Employees AS e ON el.EmployeeID = e.EmployeeID 

-- 11. Unite People

SELECT TOP (3) 
	   e.FirstName
	   , c.CityName
  FROM Employees AS e
  JOIN Branches AS b ON e.BranchID = b.BranchID
  JOIN Cities AS c ON b.CityID = c.CityID
 UNION ALL
SELECT TOP (3) 
       c.FirstName
	   , ci.CityName
  FROM Customers AS c
  JOIN Cities AS ci ON c.CityID = ci.CityID

-- 12. Customers without Accounts

SELECT c.CustomerID
       , c.Height
  FROM Customers AS c
  LEFT JOIN Accounts AS a ON c.CustomerID = a.CustomerID 
 WHERE (c.Height BETWEEN 1.74 AND 2.04) 
   AND a.AccountID IS NULL

-- 13. Customers With Accounts Larger Than Average

SELECT TOP (5) 
	   c.CustomerID
       , l.Amount
  FROM Customers AS c
  JOIN Loans AS l
    ON c.CustomerID = l.CustomerID
 WHERE l.Amount > (SELECT AVG(l2.Amount)
					 FROM Loans AS l2
 					 JOIN Customers AS c2 ON l2.CustomerID = c2.CustomerID 
					WHERE c2.Gender = 'm')
 ORDER BY c.LastName

-- 14. Oldest Account

SELECT TOP (1) 
       c.CustomerID
	   , c.FirstName
	   , a.StartDate
  FROM Customers AS c
  JOIN Accounts AS a ON c.CustomerID = a.CustomerID
 ORDER BY a.StartDate

-- 14. Oldest Account

SELECT c.CustomerID
	   , c.FirstName
	   , a.StartDate
  FROM Customers AS c
  JOIN Accounts AS a ON c.CustomerID = a.CustomerID
 WHERE a.StartDate IN (
					   SELECT MIN(ac.StartDate) 
					     FROM Accounts AS ac
					  )

-- 15. String Joiner Function

CREATE FUNCTION udf_ConcatString (@FirstString VARCHAR(MAX), @SecondString VARCHAR(MAX))
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @ReturnString VARCHAR(MAX)
	
	SET @ReturnString  = CONCAT(REVERSE(@FirstString), REVERSE(@SecondString))
	
	RETURN @ReturnString 
END

-- 16. Unexpired Loans Procedure

CREATE PROCEDURE usp_CustomersWithUnexpiredLoans (@CustomerID INT)
AS
BEGIN	
    SELECT c.CustomerID, c.FirstName, l.LoanID 
	  FROM Customers AS c
	  JOIN Loans AS l ON c.CustomerID = l.CustomerID
	 WHERE c.CustomerID = @CustomerID
	   AND l.ExpirationDate IS NULL 
END

-- 17. Take Loan Procedure

CREATE PROCEDURE usp_TakeLoan (@CustomerID INT, @LoanAmount DECIMAL(10,2), @Interest DECIMAL(10,2), @StartDate DATE)
AS
BEGIN	
	BEGIN TRANSACTION

	INSERT INTO Loans(CustomerID, Amount, Interest, StartDate)
		 VALUES (@CustomerID, @LoanAmount, @Interest, @StartDate)

	IF @LoanAmount NOT BETWEEN 0.01 AND 100000
	BEGIN
		ROLLBACK
		RAISERROR('Invalid Loan Amount.', 16, 1)
		RETURN
	END
	ELSE
	BEGIN
		COMMIT
	END
END

-- 18. Trigger Hire Employee

CREATE TRIGGER tr_NewEmployeeLoan
ON Employees
AFTER INSERT
AS
BEGIN
    UPDATE EmployeesLoans
       SET EmployeeID = i.EmployeeID
      FROM EmployeesLoans AS e, INSERTED AS i
      WHERE e.EmployeeID + 1 = i.EmployeeID
END

-- 19. Delete Trigger

CREATE TRIGGER tr_DeletedAccounts
ON Accounts
INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM EmployeesAccounts
		  WHERE AccountID = (SELECT d.AccountID FROM DELETED AS d)
	
		 DELETE Accounts
		  WHERE AccountID = (SELECT d.AccountID FROM DELETED AS d)

	INSERT INTO AccountLogs
		 SELECT * FROM DELETED
		 
END

-- 19. Delete Trigger

CREATE TRIGGER tr_DeletedAccounts
ON Accounts
INSTEAD OF DELETE
AS
BEGIN

	ALTER TABLE EmployeesAccounts
	DROP CONSTRAINT FK_EmployeesAccounts_Account

	ALTER TABLE EmployeesAccounts
	ADD CONSTRAINT FK_EmployeesAccounts_Account FOREIGN KEY(AccountID)
	REFERENCES Accounts(AccountID) ON DELETE CASCADE

	DELETE FROM Accounts
	WHERE AccountID = (SELECT AccountID FROM DELETED)

	INSERT INTO AccountLogs
	SELECT * FROM DELETED

END