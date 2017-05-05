-- 1. All Mountain Peaks

SELECT PeakName 
  FROM Peaks
 ORDER BY PeakName

-- 2. Biggest Countries by Population

SELECT TOP (30) 
	   CountryName
	   , Population 
  FROM Countries
 WHERE ContinentCode = 'EU'
 ORDER BY Population DESC, CountryName

-- 3. Countries and Currency (Euro / Not Euro)

SELECT CountryName
       , CountryCode
	   , CASE 
			WHEN ContinentCode = 'EU' THEN 'Euro'
			ELSE 'Not Euro'
		 END AS 'Currency'
  FROM Countries
 ORDER BY CountryName

-- 4. Countries Holding 'A' 3 or More Times

SELECT CountryName AS [Country Name]
       , IsoCode AS [ISO Code]
  FROM Countries
 WHERE CountryName LIKE '%a%a%a%'
 ORDER BY IsoCode

-- 5. Peaks and Mountains

SELECT p.PeakName
	   , m.MountainRange AS [Mountain]
	   , p.Elevation
  FROM Peaks AS p
  JOIN Mountains AS m ON p.MountainId = m.Id
 ORDER BY p.Elevation DESC, m.MountainRange

-- 6. Peaks with Their Mountain, Country and Continent

SELECT p.PeakName
	   , m.MountainRange AS [Mountain]
	   , c.CountryName
	   , cont.ContinentName
  FROM Peaks AS p
  JOIN Mountains AS m ON p.MountainId = m.Id
  JOIN MountainsCountries AS mc ON m.Id = mc.MountainId
  JOIN Countries AS c ON mc.CountryCode = c.CountryCode
  JOIN Continents AS cont ON c.ContinentCode = cont.ContinentCode
 ORDER BY p.PeakName, c.CountryName

-- 7. Rivers Passing through 3 or More Countries

SELECT r.RiverName AS [River]
       , COUNT(cr.CountryCode) AS [Countries Count]
  FROM Rivers AS r
  JOIN CountriesRivers AS cr ON r.Id = cr.RiverId
  JOIN Countries AS c ON cr.CountryCode = c.CountryCode
 GROUP BY r.RiverName
HAVING COUNT(cr.RiverId) >= 3
 ORDER BY r.RiverName

-- 8. Highest, Lowest and Average Peak Elevation

SELECT MAX(Elevation) AS [MaxElevation]
	   , MIN(Elevation) AS [MinElevation]
	   , AVG(Elevation) AS [AverageElevation]
  FROM Peaks
  
-- 9. Rivers by Country

SELECT c.CountryName
	   , cont.ContinentName
	   , COUNT(cr.RiverId) AS [RiversCount] 
       , SUM(ISNULL(r.Length, 0)) AS [TotalLength]
  FROM Countries AS c
  LEFT JOIN Continents AS cont ON c.ContinentCode = cont.ContinentCode
  LEFT JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode
  LEFT JOIN Rivers AS r ON cr.RiverId = r.Id
 GROUP BY c.CountryName, cont.ContinentName
 ORDER BY COUNT(cr.RiverId) DESC, SUM(r.Length) DESC, c.CountryName

-- 10. Count of Countries by Currency

SELECT cur.CurrencyCode
       , cur.Description AS [Currency]
	   , COUNT(c.CurrencyCode) AS [NumberOfCountries]
  FROM Countries AS c
  RIGHT JOIN Currencies AS cur ON c.CurrencyCode = cur.CurrencyCode
 GROUP BY cur.CurrencyCode, cur.Description
 ORDER BY COUNT(c.CurrencyCode) DESC, Currency

-- 11. Population and Area by Continent

SELECT cont.ContinentName AS [ContinentName]
       , SUM(CONVERT(BIGINT, c.AreaInSqKm)) AS [CountriesArea]
	   , SUM(CAST(c.Population AS BIGINT)) AS [CountriesPopulation]
  FROM Continents AS cont
  JOIN Countries AS c ON cont.ContinentCode = c.ContinentCode
 GROUP BY cont.ContinentName
 ORDER BY SUM(CAST(c.Population AS BIGINT)) DESC

-- 11. Population and Area by Continent
SELECT cont.ContinentName AS [ContinentName]
       , CountriesArea.currentArea AS [CountriesArea] 
	   , CountriesPopulation.CurrentPopulation AS [CountriesPopulation]
  FROM 
  Continents AS cont
  JOIN
  (
	SELECT ContinentCode, SUM(CAST(AreaInSqKm AS BIGINT)) AS currentArea 
	  FROM Countries
	 GROUP BY ContinentCode
  )     AS [CountriesArea]
	ON cont.ContinentCode = CountriesArea.ContinentCode
  JOIN  
  (
	SELECT ContinentCode, SUM(CAST(Population AS BIGINT)) AS CurrentPopulation 
	  FROM Countries
	 GROUP BY ContinentCode
  )     AS [CountriesPopulation]
    ON CountriesArea.ContinentCode = CountriesPopulation.ContinentCode
 GROUP BY cont.ContinentName, CountriesArea.currentArea, CountriesPopulation.CurrentPopulation
 ORDER BY CountriesPopulation.CurrentPopulation DESC


-- 12. Highest Peak and Longest River by Country

SELECT c.CountryName AS [CountryName]
	   , Max(p.Elevation) AS [HighestPeakElevation]
	   , MAX(r.Length) AS [LongestRiverLength]
  FROM Countries AS c
  LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
  LEFT JOIN Peaks AS p ON mc.MountainId = p.MountainId
  LEFT JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode 
  LEFT JOIN Rivers AS r ON cr.RiverId = r.Id
 GROUP BY c.CountryName
 ORDER BY Max(p.Elevation) DESC, MAX(r.Length) DESC, c.CountryName

-- 13. Mix of Peak and River Names

SELECT p.PeakName
       , r.RiverName
	   , LOWER(CONCAT(LEFT(p.PeakName, LEN(p.PeakName) - 1), r.RiverName)) AS [Mix] 
  FROM Peaks AS p, Rivers AS r
 WHERE RIGHT(p.PeakName, 1) = LEFT(r.RiverName, 1)
 ORDER BY [Mix]

-- 14. Highest Peak Name and Elevation by Country

WITH res 
AS
(
	SELECT
		  c.CountryName,
		  p.PeakName,
		  p.Elevation,
		  m.MountainRange,
		  ROW_NUMBER() OVER (PARTITION BY c.CountryName ORDER BY p.Elevation DESC) AS currentRank
		  -- RANK() or DENSE_RANK()
	  FROM Countries AS c
	  LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
	  LEFT JOIN Mountains AS m ON mc.MountainId = m.Id
	  LEFT JOIN Peaks p ON m.Id = p.MountainId
)

SELECT
      res.CountryName AS [Country],
      ISNULL(res.PeakName, '(no highest peak)') AS [Highest Peak Name],
      ISNULL(res.Elevation, 0) AS [Highest Peak Elevation],
      CASE 
		  WHEN res.PeakName IS NOT NULL THEN res.MountainRange 
		  ELSE '(no mountain)' 
	  END AS [Mountain]
  FROM res
 WHERE currentRank = 1

-- 15. Monasteries by Country

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

ALTER TABLE Countries
ADD IsDeleted BIT NOT NULL
CONSTRAINT DF_IsDeletedValue DEFAULT 0

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


-- 16. Monasteries by Continents and Countries

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
							  WHERE CountryName = 'Burma'))

SELECT cont.ContinentName
       , c.CountryName
	   , COUNT(m.Id) AS [MonasteriesCount]
  FROM Continents AS cont 
  LEFT JOIN Countries AS c ON cont.ContinentCode = c.ContinentCode 
  LEFT JOIN Monasteries AS m ON c.CountryCode = m.CountryCode
 WHERE c.IsDeleted != 1
 -- WHERE c.IsDeleted = 0
 GROUP BY cont.ContinentName, c.CountryName
 ORDER BY COUNT(m.Id) DESC, c.CountryName ASC

-- 17. Stored Function: Mountain Peaks JSON

IF OBJECT_ID('fn_MountainsPeaksJSON') IS NOT NULL
  DROP FUNCTION fn_MountainsPeaksJSON
GO

CREATE FUNCTION fn_MountainsPeaksJSON()
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @json NVARCHAR(MAX) = '{"mountains":['

	DECLARE mountainsCursor CURSOR FOR
	SELECT Id, MountainRange FROM Mountains

	OPEN mountainsCursor
	DECLARE @mountainId INT
	DECLARE @mountainName NVARCHAR(MAX)
	FETCH NEXT FROM mountainsCursor INTO @mountainId, @mountainName
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @json = @json + '{"name":"' + @mountainName + '","peaks":['

		DECLARE peaksCursor CURSOR FOR
		SELECT PeakName, Elevation FROM Peaks
		WHERE MountainId = @mountainId

		OPEN peaksCursor
		DECLARE @peakName NVARCHAR(MAX)
		DECLARE @elevation INT
		FETCH NEXT FROM peaksCursor INTO @peakName, @elevation

		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @json = @json + '{"name":"' + @peakName + '",' +
				'"elevation":' + CONVERT(NVARCHAR(MAX), @elevation) + '}'
			FETCH NEXT FROM peaksCursor INTO @peakName, @elevation
			IF @@FETCH_STATUS = 0
				SET @json = @json + ','
		END
		CLOSE peaksCursor
		DEALLOCATE peaksCursor
		
		SET @json = @json + ']}'

		FETCH NEXT FROM montainsCursor INTO @mountainId, @mountainName
		IF @@FETCH_STATUS = 0
			SET @json = @json + ','
	END
	CLOSE montainsCursor
	DEALLOCATE montainsCursor

	SET @json = @json + ']}'
	RETURN @json
END

-- 17. Stored Function: Mountain Peaks JSON

IF OBJECT_ID('fn_MountainsPeaksJSON2') IS NOT NULL
  DROP FUNCTION fn_MountainsPeaksJSON2
GO

CREATE FUNCTION fn_MountainsPeaksJSON2() 
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @json NVARCHAR(MAX)
	DECLARE @res TABLE
	(
		MountainId INT, 
		MountainRange NVARCHAR(50), 
		PeakId INT, 
		PeakName NVARCHAR(50), 
		Elevation INT
	) 
	
	INSERT INTO @res
		 SELECT
				m.Id,
				m.MountainRange,
				p.Id,
				p.PeakName,
				p.Elevation
		   FROM Mountains AS m
		   LEFT JOIN Peaks AS p ON m.Id = p.MountainId	

	 SET @json = '{"mountains":[' + 
		 STUFF((
				SELECT 
					   ',{"name":"' + currRow.Name
					   + '","peaks":' + ISNULL(currRow.Peaks, '[]') + '}'
				  FROM 		
				  (
				    SELECT
						  currRes.MountainId,  
						  currRes.MountainRange AS Name,
						  '[{' + 
						  STUFF(ISNULL(
						  (SELECT 
							    ',{"name":"' + 
							    x.PeakName + '","elevation":' + 
							    CAST(x.Elevation AS NVARCHAR(MAX)) + '}'
						    FROM @res x
						   WHERE x.MountainRange = currRes.MountainRange
						   GROUP BY x.PeakId, x.PeakName, x.Elevation
						     FOR XML PATH (''), TYPE).value('.','NVARCHAR(max)'), ''), 1, 2, '') + 
						         ']' AS [Peaks]
					  FROM @res currRes
				     GROUP BY currRes.MountainId, currRes.MountainRange
				  ) AS currRow
					   FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(max)'), 1, 1, '')
		 + ']}'

	RETURN @json
END

-- 18. Design a Database Schema in MySQL and Write a Query – WRONG RESULT ON SQL SERVER

INSERT INTO TrainingCenters
VALUES (1, 'Sofia Learning', NULL, 'http://sofialearning.org'), 
(2, 'Varna Innovations & Learning', 'Innovative training center, located in Varna. Provides trainings in software development and foreign languages', 'http://vil.edu'), 
(3, 'Plovdiv Trainings & Inspiration', NULL, NULL),
(4, 'Sofia West Adult Trainings', 'The best training center in Lyulin', 'https://sofiawest.bg'), 
(5, 'Software Trainings Ltd.', NULL, 'http://softtrain.eu'),
(6, 'Polyglot Language School', 'English, French, Spanish and Russian language courses', NULL), 
(7, 'Modern Dances Academy', 'Learn how to dance!', 'http://danceacademy.bg');

INSERT INTO Courses
VALUES (101, 'Java Basics', 'Learn more at https://softuni.bg/courses/java-basics/'), (102, 'English for beginners', '3-month English course'), 
(103, 'Salsa: First Steps', NULL), (104, 'Avancée Français', 'French language: Level III'), (105, 'HTML & CSS', NULL), 
(106, 'Databases', 'Introductionary course in databases, SQL, MySQL, SQL Server and MongoDB'), (107, 'C# Programming', 'Intro C# corse for beginners'), 
(108, 'Tango dances', NULL), (109, 'Spanish, Level II', 'Aprender Español');

INSERT INTO Timetable(CourseId, TrainingCenterId, StartDate) 
VALUES (101, 1, '2015-01-31'), (101, 5, '2015-02-28'), (102, 6, '2015-01-21'), (102, 4, '2015-01-07'), 
(102, 2, '2015-02-14'), (102, 1, '2015-03-05'), (102, 3, '2015-03-01'), (103, 7, '2015-02-25'), (103, 3, '2015-02-19'), 
(104, 5, '2015-01-07'), (104, 1, '2015-03-30'), (104, 3, '2015-04-01'), (105, 5, '2015-01-25'), (105, 4, '2015-03-23'), (105, 3, '2015-04-17'), 
(105, 2, '2015-03-19'), (106, 5, '2015-02-26'), (107, 2, '2015-02-20'), (107, 1, '2015-01-20'), (107, 3, '2015-03-01'), (109, 6, '2015-01-13');

UPDATE Timetable 
   SET StartDate = DATEADD(DAY, -7, StartDate)
 WHERE CourseId IN (
			        SELECT c.CourseId 
					  FROM Timetable AS t
					  JOIN Courses AS c ON t.CourseId = c.CourseId
					 WHERE c.Name LIKE '[a-j][a-j][a-j][a-j][a-j]%'
						OR c.Name LIKE '[a-j][a-j][a-j][a-j]%'
						OR c.Name LIKE '[a-j][a-j][a-j]%'
						OR c.Name LIKE '[a-j][a-j]%'
						OR c.Name LIKE '[a-j]%'
					)
SELECT tc.Name AS [training center]
       , t.StartDate AS [start date]
	   , c.Name AS [course name]
	   , c.Description AS [more info]
  FROM Timetable AS t
  JOIN TrainingCenters AS tc ON t.TrainingCenterId = tc.TrainingCenterId
  JOIN Courses AS c ON t.CourseId = c.CourseId 
 ORDER BY t.StartDate, t.Id