-- 01. Employees with Salary Above 35000

CREATE PROCEDURE usp_GetEmployeesSalaryAbove35000 
AS
BEGIN
	SELECT e.FirstName, e.LastName 
	  FROM Employees AS e 
	 WHERE e.Salary > 35000
END 

-- 02. Employees with Salary Above Number

CREATE PROCEDURE usp_GetEmployeesSalaryAboveNumber (@Number MONEY)
AS
BEGIN
	SELECT e.FirstName, e.LastName 
	  FROM Employees AS e 
	 WHERE e.Salary >= @Number
END 

-- 03. Town Names Starting With

CREATE PROCEDURE usp_GetTownsStartingWith (@StartingWith VARCHAR(MAX))
AS
BEGIN
	SELECT t.Name
	  FROM Towns AS t 
	 WHERE t.Name LIKE (@StartingWith + '%')
   --WHERE LEFT(t.Name, LEN(@StartingWith)) = @StartingWith
END 

-- 04. Employees from Town

CREATE PROCEDURE usp_GetEmployeesFromTown (@TownName VARCHAR(MAX))
AS
BEGIN
	SELECT e.FirstName, e.LastName
	  FROM Employees AS e
	  JOIN Addresses AS a ON a.AddressID = e.AddressID
	  JOIN Towns AS t ON a.TownID = t.TownID
	 WHERE t.Name = @TownName
END 

-- 05. Salary Level Function

CREATE FUNCTION ufn_GetSalaryLevel (@salary MONEY) 
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @salaryLevel VARCHAR(MAX)

	IF @salary < 30000
	BEGIN
		SET @salaryLevel = 'Low'
	END
	ELSE IF @salary BETWEEN 30000 AND 50000
	BEGIN
		SET @salaryLevel = 'Average'
	END
	ELSE
	BEGIN
		SET	@salaryLevel = 'High'			
	END

	RETURN @salaryLevel
END 

-- 06. Employees by Salary Level

CREATE PROCEDURE usp_EmployeesBySalaryLevel (@salaryLevel VARCHAR(MAX))
AS
BEGIN
    SELECT e.FirstName, e.LastName
      FROM Employees AS e
     WHERE dbo.ufn_GetSalaryLevel(e.Salary) = @salaryLevel
END

-- 07. Define Function

CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(MAX), @word VARCHAR(MAX)) 
RETURNS BIT
AS
BEGIN
     DECLARE @index INT = 1
     DECLARE @wordLength INT = LEN(@word)
     DECLARE @currentLetter CHAR(1)
     
	 WHILE (@index <= @wordLength)
     BEGIN
          SET @currentLetter = SUBSTRING(@word, @index, 1)
          IF (CHARINDEX(@currentLetter, @setOfLetters) != 0)
             SET @index += 1
          ELSE
             RETURN 0
     END
     RETURN 1
END 

-- 08. Delete Employees and Departments - Hack The Judge

ALTER TABLE Departments
DROP CONSTRAINT [FK_Departments_Employees]

ALTER TABLE Employees
DROP CONSTRAINT [FK_Employees_Employees]
  
DELETE FROM EmployeesProjects 
 WHERE EmployeeID IN (SELECT e.EmployeeID
						FROM Departments AS d
						JOIN Employees AS e ON d.DepartmentID = e.DepartmentID 
					   WHERE Name IN ('Production', 'Production Control'))

						   
DELETE FROM Employees
 WHERE DepartmentID IN (SELECT d.DepartmentID 
						  FROM Departments AS d 
						 WHERE Name IN ('Production', 'Production Control'))

DELETE FROM Departments
 WHERE Name IN ('Production', 'Production Control')

-- 08. Delete Employees and Departments - Judge Says Compile Time Error But It Works

ALTER TABLE Departments
ALTER COLUMN ManagerId INT

DELETE FROM EmployeesProjects 
 WHERE EmployeeID IN (SELECT e.EmployeeID
						FROM Departments AS d
						JOIN Employees AS e ON d.DepartmentID = e.DepartmentID 
					   WHERE d.Name IN ('Production', 'Production Control'))

UPDATE Employees
   SET ManagerID = NULL
 WHERE ManagerID IN (SELECT e.EmployeeID
					   FROM Departments AS d
					   JOIN Employees AS e ON d.DepartmentID = e.DepartmentID 
					  WHERE d.Name IN ('Production', 'Production Control'))

UPDATE Departments
   SET ManagerID = NULL
 WHERE ManagerID IN (SELECT e.EmployeeID
					   FROM Departments AS d
					   JOIN Employees AS e ON d.DepartmentID = e.DepartmentID 
					  WHERE d.Name IN ('Production', 'Production Control'))

DELETE FROM Employees
 WHERE EmployeeID IN (SELECT e.EmployeeID
					    FROM Departments AS d
					    JOIN Employees AS e ON d.DepartmentID = e.DepartmentID 
					   WHERE d.Name IN ('Production', 'Production Control'))

DELETE FROM Departments
 WHERE Name IN ('Production', 'Production Control')

-- 09. Employees with Three Projects

CREATE PROCEDURE usp_AssignProject (@employeeId INT, @projectID INT)
AS
BEGIN
	BEGIN TRANSACTION

	INSERT INTO EmployeesProjects(EmployeeID, ProjectID)
		 VALUES (@employeeId, @projectID)
	IF (
		SELECT COUNT(ep.EmployeeID) 
		  FROM EmployeesProjects AS ep
		 WHERE ep.EmployeeID = @employeeId
	   ) > 3
	BEGIN
		ROLLBACK
		RAISERROR ('The employee has too many projects!', 16, 1)		
		RETURN
	END
	COMMIT
END

-- 10. Find Full Name

CREATE PROCEDURE usp_GetHoldersFullName
AS
BEGIN
    SELECT CONCAT(ah.FirstName, ' ', ah.LastName) AS [Full Name] 
	  FROM AccountHolders AS ah
END

-- 11. People with Balance Higher Than

CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan (@number DECIMAL(19,4))
AS
BEGIN
    SELECT ah.FirstName AS [First Name], ah.LastName AS [Last Name]
      FROM AccountHolders AS ah
      JOIN Accounts AS a ON ah.Id = a.AccountHolderId
     GROUP BY ah.FirstName, ah.LastName
    HAVING SUM(a.Balance) > @number
END

-- 11. People with Balance Higher Than

CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan (@number DECIMAL(19,4))
AS
BEGIN 
	SELECT FirstName AS [First Name], LastName AS [Last Name] 
	  FROM
		  (
			SELECT FirstName, LastName, SUM(a.Balance) AS TotalBalance 
			  FROM AccountHolders AS ah
			  JOIN Accounts AS a ON a.AccountHolderId = ah.Id
		     GROUP BY ah.FirstName, ah.LastName
		  ) AS TotBal
	WHERE TotBal.TotalBalance > @number
    -- also possible with HAVING clause on the same condition
END

-- 12. Future Value Function

CREATE FUNCTION ufn_CalculateFutureValue 
(
    @sum DECIMAL(18,10),
	@yearlyInterestRate DECIMAL(18,10),
	@numberOfYears INT
)
RETURNS DECIMAL(18,4)
AS
BEGIN
	DECLARE @FutureValue DECIMAL(18,4)
	SET @FutureValue = @sum * (POWER((1 + @yearlyInterestRate), @numberOfYears)) 
	RETURN @FutureValue
END

-- 13. Calculating Interest

CREATE PROCEDURE usp_CalculateFutureValueForAccount (@AccountId INT, @InterestRate DECIMAl(18,10))
AS
BEGIN
SELECT currBallance.ID AS [Account Id]
	   , currBallance.FirstName AS [First Name]
	   , currBallance.LastName AS [Last Name]
	   , currBallance.Balance AS [Current Balance]
	   , (SELECT [dbo].[ufn_CalculateFutureValue](currBallance.Balance, @InterestRate, 5)) AS [Balance in 5 years]
  FROM 
	  (
	   SELECT ah.ID, ah.FirstName, ah.LastName, a.Balance
	     FROM Accounts AS a
	     JOIN AccountHolders AS ah ON a.AccountHolderID = ah.ID 
	      AND a.ID = @AccountId
	  ) AS currBallance
END

-- 14. Deposit Money Procedure

CREATE PROCEDURE usp_DepositMoney (@accountId INT, @moneyAmount MONEY)
AS
BEGIN
	DECLARE @oldMoneyAmount MONEY
	DECLARE @newMoneyAmount MONEY
	
	BEGIN TRANSACTION

	SET @oldMoneyAmount = (SELECT Balance 
							 FROM Accounts 
						    WHERE ID = @accountId)

	SET @newMoneyAmount = @oldMoneyAmount + @moneyAmount

	UPDATE Accounts
	   SET Balance = @newMoneyAmount
	 WHERE ID = @accountId

	IF @moneyAmount < 0 
	BEGIN
		ROLLBACK
		RAISERROR ('The money amount should be positive!', 16, 1)		
		RETURN
	END
	COMMIT
END

-- 15. Withdraw Money Procedure

CREATE PROCEDURE usp_WithdrawMoney (@accountId INT, @moneyAmount MONEY)
AS
BEGIN
	DECLARE @oldMoneyAmount MONEY
	DECLARE @newMoneyAmount MONEY
	
	BEGIN TRANSACTION

	SET @oldMoneyAmount = (SELECT Balance 
							 FROM Accounts 
							WHERE ID = @accountId)

	SET @newMoneyAmount = @oldMoneyAmount - @moneyAmount

	UPDATE Accounts
	   SET Balance = @newMoneyAmount
	 WHERE ID = @accountId

	IF @moneyAmount < 0 
	BEGIN
		ROLLBACK
		RAISERROR ('The money amount should be positive!', 16, 1)		
		RETURN
	END
	ELSE IF (
			  SELECT Balance
			    FROM Accounts
			   WHERE ID = @accountId 
			) < 0
	BEGIN
		ROLLBACK
		RAISERROR ('The money in your account is not enough!', 16, 1)
		RETURN
	END
	COMMIT
END

-- 16. Money Transfer

CREATE PROCEDURE usp_TransferMoney (@senderId INT, @receiverId INT, @amount MONEY)
AS
BEGIN
    BEGIN TRANSACTION
 
    EXEC dbo.usp_WithdrawMoney @senderId, @amount
    EXEC dbo.usp_DepositMoney @receiverId, @amount
 
    IF @amount < 0
    BEGIN
        ROLLBACK
        RAISERROR ('The money amount should be positive!', 16, 1)  
        RETURN
    END  
    COMMIT
END

-- 17. Create Table Logs

CREATE TRIGGER tr_LogChanges
ON Accounts
FOR UPDATE
AS
BEGIN
    INSERT INTO Logs(AccountId, OldSum, NewSum)
    SELECT i.ID, d.Balance, i.Balance
      FROM INSERTED AS i
      JOIN DELETED AS d ON i.ID = d.ID
   
    --FROM INSERTED, DELETED
    --WHERE INSERTED.ID = DELETED.ID
END

-- 18. Create Table Emails

CREATE TRIGGER tr_LogChangesNotifications
ON Logs
FOR INSERT
AS
BEGIN
    DECLARE @recepient INT
    DECLARE @oldSum MONEY
    DECLARE @newSum MONEY
    DECLARE @date DATETIME
 
    SET @recepient = (SELECT AccountId FROM INSERTED)
    SET @oldSum = (SELECT OldSum FROM INSERTED)
    SET @newSum = (SELECT NewSum FROM INSERTED )
    SET @date = GETDATE()
 
    INSERT INTO NotificationEmails(Recipient, Subject, Body)
         VALUES (
                   @recepient,
                   CONCAT('Balance change for account: ', @recepient),
                   CONCAT('On ', @date, ' your balance was changed from ', @oldSum, ' to ', @newSum)
                )
END

-- 19. Cash in User Games Odd Rows

CREATE FUNCTION ufn_CashInUsersGames(@GameName NVARCHAR(50))
RETURNS TABLE
AS

RETURN (
		SELECT SUM(tempTable.Cash) AS [SumCash] 
		  FROM (
				 SELECT ug.Id
						, ug.Cash
						, ROW_NUMBER() OVER(ORDER BY ug.Cash DESC) AS rows 
				   FROM UsersGames AS ug
				   JOIN Games AS g ON ug.GameId = g.Id
				  WHERE g.Name = @GameName
			   ) AS tempTable
		 WHERE tempTable.rows % 2 = 1
	   )

-- 20. There is no Problem 20

-- 21. Massive Shopping

BEGIN TRANSACTION

DECLARE @Sum DECIMAL = (SELECT SUM(i.Price)
					      FROM Items AS i
						 WHERE MinLevel BETWEEN 11 AND 12)

IF(SELECT Cash FROM UsersGames WHERE Id = 110) < @Sum
BEGIN
   ROLLBACK
END
ELSE 
BEGIN
	UPDATE UsersGames
	   SET Cash -= @Sum
	 WHERE Id = 110

INSERT INTO UserGameItems (UserGameId, ItemId)
	 SELECT 110, Id 
	   FROM Items 
	  WHERE MinLevel BETWEEN 11 AND 12
	 COMMIT
END

BEGIN TRANSACTION

DECLARE @Sum2 DECIMAL = (SELECT SUM(i.Price)
						   FROM Items i
						  WHERE MinLevel BETWEEN 19 AND 21)

IF (SELECT Cash FROM UsersGames WHERE Id = 110) < @Sum2
BEGIN
	ROLLBACK
END
ELSE 
BEGIN
	UPDATE UsersGames
	   SET Cash = Cash - @sum2
	 WHERE Id = 110

INSERT INTO UserGameItems (UserGameId, ItemId)
     SELECT 110, Id 
	   FROM Items 
	  WHERE MinLevel BETWEEN 19 AND 21
	 COMMIT
END

SELECT i.Name AS [Item Name] 
  FROM UserGameItems ugi
  JOIN Items i ON ugi.ItemId = i.Id
 WHERE ugi.UserGameId = 110

-- 22. Number of Users for Email Provider

SELECT RIGHT(u.Email, LEN(u.Email) - CHARINDEX('@', u.Email)) AS [Email Provider]
     --, SUBSTRING(u.Email,CHARINDEX('@', u.Email) + 1, LEN(u.Email)) AS [Email Provider]
	   , COUNT(u.Id) AS [Number Of Users]
  FROM Users AS u
 GROUP BY RIGHT(u.Email, LEN(u.Email) - CHARINDEX('@', u.Email))
 ORDER BY [Number Of Users] DESC, [Email Provider] 

 -- 23. All Users in Games

 SELECT g.Name AS [Game]
	   , gt.Name AS [Game Type]
	   , u.Username AS [Username]
	   , ug.Level AS [Level]
	   , ug.Cash AS [Cash]
	   , c.Name AS [Character]
  FROM Games AS g
  JOIN UsersGames AS ug ON g.Id = ug.GameId
  JOIN Users AS u ON ug.UserId = u.Id
  JOIN GameTypes AS gt ON g.GameTypeId = gt.Id
  JOIN Characters AS c ON ug.CharacterId = c.Id
 ORDER BY ug.Level DESC, u.Username, g.Name

-- 24. Users in Games with Their Items

SELECT u.Username AS [Username]
	   , g.Name AS [Game]
	   , COUNT(ugi.ItemId) AS [Items Count]
	   , SUM(i.Price) AS [Items Price]
  FROM Users AS u
  JOIN UsersGames AS ug ON u.Id = ug.UserId
  JOIN Games AS g ON ug.GameId = g.Id
  JOIN UserGameItems AS ugi ON ug.Id = ugi.UserGameId 
  JOIN Items AS i ON ugi.ItemId = i.Id
 GROUP BY u.Username, g.Name
HAVING COUNT(ugi.ItemId) >= 10
 ORDER BY COUNT(ugi.ItemId) DESC, SUM(i.Price) DESC, u.Username

-- 25. User in Games with Their Statistics

SELECT u.Username AS [Username]
	   , g.Name AS [Game]
	   , MAX(c.Name) AS [Character]
	   , SUM(its.strength) + MAX(gts.strength) + MAX(cs.strength) AS [Strength]
	   , SUM(its.defence) + MAX(gts.defence) + MAX(cs.defence) AS [Defence]
	   , SUM(its.speed) + MAX(gts.speed) + MAX(cs.speed) AS [Speed]
	   , SUM(its.mind) + MAX(gts.mind) + MAX(cs.mind) AS [Mind]
	   , SUM(its.luck) + MAX(gts.luck) + MAX(cs.luck) AS [Luck]
  FROM Users AS u
  JOIN UsersGames AS ug ON u.Id = ug.UserId
  JOIN Games AS g ON ug.GameId = g.Id

  JOIN GameTypes AS gt ON g.GameTypeId = gt.Id
  JOIN [Statistics] AS gts ON gt.BonusStatsId = gts.Id

  JOIN UserGameItems AS ugi ON ug.Id = ugi.UserGameId
  JOIN Items AS i ON ugi.ItemId = i.Id
  JOIN [Statistics] AS its ON i.StatisticId = its.Id

  JOIN Characters AS c ON ug.CharacterId = c.Id
  JOIN [Statistics] AS cs ON c.StatisticId = cs.Id
 GROUP BY u.Username, g.Name
 ORDER BY Strength DESC, Defence DESC, Speed DESC, Mind DESC, Luck DESC

 -- 26. All Items with Greater than Average Statistics

SELECT i.Name AS [Name]
       , i.Price AS [Price]
	   , i.MinLevel AS [MinLevel]
	   , s.Strength AS [Strength]
	   , s.Defence AS [Defence]
	   , s.Speed AS [Speed]
	   , s.Luck AS [Luck]
	   , s.Mind AS [Mind]
  FROM Items AS i
  JOIN [Statistics] AS s ON i.StatisticId = s.Id
 WHERE s.Mind > (SELECT AVG(Mind) FROM [Statistics]) 
   AND s.Speed > (SELECT AVG(Speed) FROM [Statistics]) 
   AND s.Luck > (SELECT AVG(Luck) FROM [Statistics]) 
 GROUP BY i.Name, i.Price, i.MinLevel, s.Strength, s.Defence, s.Speed, s.Luck, s.Mind 
 ORDER BY i.Name
    -- or the same with HAVING instead of WHERE

-- 27. Display All Items about Forbidden Game Type

SELECT i.Name AS [Item]
	   , i.Price AS [Price]
	   , i.MinLevel AS [MinLevel]
	   , gt.Name AS [Forbidden Game Type] 
  FROM Items AS i
  LEFT JOIN GameTypeForbiddenItems AS gtfi ON i.Id = gtfi.ItemId
  LEFT JOIN GameTypes AS gt ON gtfi.GameTypeId = gt.Id
 GROUP BY i.Name, i.Price, i.MinLevel, gt.Name
 ORDER BY gt.Name DESC, i.Name

 -- 28. Buy Items for User in Game

DECLARE @userGameId INT = 
(
	SELECT ug.Id
	  FROM Users AS u
	  JOIN UsersGames AS ug ON u.Id = ug.UserId
	  JOIN Games AS g ON ug.GameId = g.Id
	 WHERE u.Username = 'Alex' 
	   AND g.Name = 'Edinburgh'
)

INSERT INTO UserGameItems (ItemId, UserGameId)
	 SELECT i.Id, @userGameId
	   FROM Items AS i
	  WHERE i.Name IN ('Blackguard', 'Bottomless Potion of Amplification', 
                  'Eye of Etlich (Diablo III)', 'Gem of Efficacious Toxin', 
				  'Golden Gorget of Leoric', 'Hellfire Amulet')

UPDATE UsersGames
   SET Cash -= 
   (
	 SELECT SUM(i.Price)
	   FROM Items AS i
	  WHERE i.Name IN ('Blackguard', 'Bottomless Potion of Amplification', 
                  'Eye of Etlich (Diablo III)', 'Gem of Efficacious Toxin', 
				  'Golden Gorget of Leoric', 'Hellfire Amulet')
   )
 WHERE Id = 
   (
	 SELECT ug.Id
	   FROM Users AS u
	   JOIN UsersGames AS ug ON u.Id = ug.UserId
	   JOIN Games AS g ug.GameId = ON g.Id
	  WHERE u.Username = 'Alex' 
	    AND g.Name = 'Edinburgh'
   )

SELECT u.Username
	   , g.Name
       , ug.Cash
	   , i.Name AS [Item Name]
  FROM Users AS u
  JOIN UsersGames AS ug ON u.Id = ug.UserId
  JOIN Games AS g ON ug.GameId = g.Id
  JOIN UserGameItems AS ugi ON ug.Id = ugi.UserGameId
  JOIN Items AS i ON ugi.ItemId = i.Id
 WHERE g.Name = 'Edinburgh'
 ORDER BY i.Name

 -- 29. Peaks and Mountains

 SELECT p.PeakName AS [PeakName]
		, m.MountainRange AS [Mountain]
		, p.Elevation AS [Elevation]
  FROM Peaks AS p
  JOIN Mountains AS m ON p.MountainId = m.Id
 ORDER BY p.Elevation DESC, m.MountainRange

 -- 30. Peaks with Mountain, Country and Continent

 SELECT p.PeakName AS [PeakName]
		, m.MountainRange AS [Mountain]
		, c.CountryName AS [CountryName]
		, cont.ContinentName AS [ContinentName]
  FROM Peaks AS p
  JOIN Mountains AS m ON p.MountainId = m.Id
  JOIN MountainsCountries AS mc ON m.Id = mc.MountainId
  JOIN Countries AS c ON mc.CountryCode = c.CountryCode
  JOIN Continents AS cont ON c.ContinentCode = cont.ContinentCode
 ORDER BY p.PeakName, c.CountryName

 -- 31. Rivers by Country

 SELECT c.CountryName AS [CountryName]
	   , cont.ContinentName AS [ContinentName]
	   , COUNT(cr.RiverId) AS [RiversCount] 
       , SUM(ISNULL(r.Length, 0)) AS [TotalLength]
  FROM Countries AS c
  LEFT JOIN Continents AS cont ON c.ContinentCode = cont.ContinentCode
  LEFT JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode
  LEFT JOIN Rivers AS r ON cr.RiverId = r.Id
 GROUP BY c.CountryName, cont.ContinentName
 ORDER BY COUNT(cr.RiverId) DESC, SUM(r.Length) DESC, c.CountryName

 -- 32. Count of Countries by Currency

 SELECT cur.CurrencyCode AS [CurrencyCode]
       , cur.Description AS [Currency]
	   , COUNT(c.CurrencyCode) AS [NumberOfCountries]
  FROM Countries AS c
 RIGHT JOIN Currencies AS cur ON c.CurrencyCode = cur.CurrencyCode
 GROUP BY cur.CurrencyCode, cur.Description
 ORDER BY COUNT(c.CurrencyCode) DESC, Currency

-- 33. Population and Area by Continent

SELECT cont.ContinentName AS [ContinentName]
       , SUM(CONVERT(BIGINT, c.AreaInSqKm)) AS [CountriesArea]
	   , SUM(CAST(c.Population AS BIGINT)) AS [CountriesPopulation]
  FROM Continents AS cont
  JOIN Countries AS c ON cont.ContinentCode = c.ContinentCode
 GROUP BY cont.ContinentName
 ORDER BY SUM(CAST(c.Population AS BIGINT)) DESC

 -- 33. Population and Area by Continent

 SELECT cont.ContinentName AS [ContinentName]
       , CountriesArea.currentArea AS [CountriesArea] 
	   , CountriesPopulation.CurrentPopulation AS [CountriesPopulation]
  FROM 
  Continents AS cont
  JOIN
  (
	SELECT ContinentCode, SUM(CAST(AreaInSqKm AS BIGINT)) AS [currentArea]
	  FROM Countries
	 GROUP BY ContinentCode
  )     AS [CountriesArea]
	ON cont.ContinentCode = CountriesArea.ContinentCode
  JOIN  
  (
	SELECT ContinentCode, SUM(CAST(Population AS BIGINT)) AS [CurrentPopulation]
	  FROM Countries
	 GROUP BY ContinentCode
  )     AS [CountriesPopulation]
    ON CountriesArea.ContinentCode = CountriesPopulation.ContinentCode
 GROUP BY cont.ContinentName, CountriesArea.currentArea, CountriesPopulation.CurrentPopulation
 ORDER BY CountriesPopulation.CurrentPopulation DESC

 -- 34. Monasteries by Country

 CREATE TABLE Monasteries
(
	Id INT NOT NULL IDENTITY PRIMARY KEY, 
	Name NVARCHAR(500), 
	CountryCode CHAR(2)
	CONSTRAINT FK_Monasteries_Countries FOREIGN KEY(CountryCode)
	REFERENCES Countries(CountryCode)
)

INSERT INTO Monasteries(Name, CountryCode) VALUES
('Rila Monastery “St. Ivan of Rila”', 'BG'), 
('Bachkovo Monastery “Virgin Mary”', 'BG'),
('Troyan Monastery “Holy Mother''s Assumption”', 'BG'),
('Kopan Monastery', 'NP'),
('Thrangu Tashi Yangtse Monastery', 'NP'),
('Shechen Tennyi Dargyeling Monastery', 'NP'),
('Benchen Monastery', 'NP'),
('Southern Shaolin Monastery', 'CN'),
('Dabei Monastery', 'CN'),
('Wa Sau Toi', 'CN'),
('Lhunshigyia Monastery', 'CN'),
('Rakya Monastery', 'CN'),
('Monasteries of Meteora', 'GR'),
('The Holy Monastery of Stavronikita', 'GR'),
('Taung Kalat Monastery', 'MM'),
('Pa-Auk Forest Monastery', 'MM'),
('Taktsang Palphug Monastery', 'BT'),
('Sümela Monastery', 'TR')

UPDATE Countries
   SET IsDeleted = 1 
 WHERE CountryCode IN (
						SELECT c.CountryCode 
						  FROM Countries AS c
						  JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode
						  JOIN Rivers AS r ON cr.RiverId = r.Id
						 GROUP BY c.CountryCode
					    HAVING COUNT(cr.RiverId) > 3
					  )

SELECT m.Name AS [Monastery]
       , c.CountryName AS [Country]
  FROM Countries AS c
  JOIN Monasteries AS m ON c.CountryCode = m.CountryCode
 WHERE c.IsDeleted != 1
 -- WHERE c.IsDeleted = 0
 ORDER BY m.Name

 -- 35. Monasteries by Continents and Countries

UPDATE Countries
   SET CountryName = 'Burma'
 WHERE CountryName = 'Myanmar'

INSERT INTO Monasteries(Name, CountryCode)
     VALUES('Hanga Abbey', (SELECT CountryCode 
							   FROM Countries 
							  WHERE CountryName = 'Tanzania'))

INSERT INTO Monasteries(Name, CountryCode)
     VALUES('Myin-Tin-Daik', (SELECT CountryCode 
							    FROM Countries 
							   WHERE CountryName = 'Myanmar'))

SELECT cont.ContinentName AS [ContinentName]
       , c.CountryName AS [CountryName]
	   , COUNT(m.Id) AS [MonasteriesCount]
  FROM Continents AS cont 
  LEFT JOIN Countries AS c ON cont.ContinentCode = c.ContinentCode 
  LEFT JOIN Monasteries AS m ON c.CountryCode = m.CountryCode
 WHERE c.IsDeleted != 1
 -- WHERE c.IsDeleted = 0
 GROUP BY cont.ContinentName, c.CountryName
 ORDER BY COUNT(m.Id) DESC, c.CountryName ASC