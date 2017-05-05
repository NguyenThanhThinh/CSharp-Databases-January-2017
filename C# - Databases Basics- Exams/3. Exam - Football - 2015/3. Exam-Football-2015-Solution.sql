-- 1. All Teams

SELECT TeamName AS [TeamName]
  FROM Teams
 ORDER BY TeamName

-- 2. Biggest Countries by Population

SELECT TOP (50)
       CountryName AS [CountryName]
	   , Population AS [Population]	
  FROM Countries
 ORDER BY Population DESC, CountryName

-- 3. Countries and Currency (Eurozone)

SELECT CountryName
       , CountryCode
	   , CASE 
			WHEN CurrencyCode = 'EUR ' THEN 'Inside'
			ELSE 'Outside'
		 END AS 'Eurozone'
  FROM Countries
 ORDER BY CountryName

-- 4. Teams Holding Numbers

SELECT TeamName AS [Team Name]
	  , CountryCode AS [Country Code]
  FROM Teams
 WHERE TeamName LIKE '%[0-9]%'
 ORDER BY CountryCode

-- 5. International Matches

SELECT c.CountryName AS [Home Team]
	   , c2.CountryName AS [Away Team]
	   , im.MatchDate AS [Match Date]
  FROM InternationalMatches AS im
  JOIN Countries AS c ON im.HomeCountryCode = c.CountryCode
  JOIN Countries AS c2 ON im.AwayCountryCode = c2.CountryCode
 ORDER BY im.MatchDate DESC

-- 6. Teams with their League and League Country

SELECT t.TeamName AS [Team Name]
	   , l.LeagueName AS [League]
	   , ISNULL(c.CountryName, 'International') AS [League Country]
  FROM Teams AS t
  JOIN Leagues_Teams AS lt ON t.Id = lt.TeamId
  JOIN Leagues AS l ON lt.LeagueId = l.Id
  LEFT JOIN Countries AS c ON l.CountryCode = c.CountryCode
 GROUP BY t.TeamName, l.LeagueName, c.CountryName
 ORDER BY t.TeamName

-- 7. Teams with more than One Match

SELECT t.TeamName AS [Team]
       , COUNT(t.Id) AS [Matches Count]	
  FROM Teams AS t
  JOIN TeamMatches AS tm ON t.Id = tm.HomeTeamId OR t.Id = tm.AwayTeamId 
 GROUP BY t.TeamName
HAVING COUNT(t.Id) > 1
 ORDER BY t.TeamName

-- 8. Number of Teams and Matches in Leagues

SELECT l.LeagueName AS [League Name]
       , COUNT(DISTINCT lt.TeamId) AS [Teams]
	   , COUNT(DISTINCT tm.Id) AS [Matches]
	   , ISNULL(AVG(tm.HomeGoals + tm.AwayGoals), 0) [Average Goals]
  FROM Leagues AS l
  LEFT JOIN Leagues_Teams AS lt ON l.Id = lt.LeagueId
  LEFT JOIN TeamMatches AS tm ON l.Id = tm.LeagueId
 GROUP BY l.LeagueName
 ORDER BY COUNT(DISTINCT lt.TeamId) DESC, COUNT(DISTINCT tm.Id) DESC

-- 9. Total Goals per Team in all Matches

SELECT t.TeamName AS [TeamName]
       , ISNULL(SUM(tm.HomeGoals),0) + ISNULL(SUM(tm2.AwayGoals), 0) AS [Total Goals]
  FROM Teams AS t
  LEFT JOIN TeamMatches AS tm ON t.Id = tm.HomeTeamId 
  LEFT JOIN TeamMatches AS tm2 ON t.Id = tm2.AwayTeamId
 GROUP BY t.TeamName
 ORDER BY [Total Goals] DESC, [TeamName]

-- 10. Pairs of Matches on the Same Day

SELECT tm.MatchDate AS [First Date]
	   , tm2.MatchDate AS [Second Date]
  FROM TeamMatches AS tm
  JOIN TeamMatches AS tm2 ON tm.Id != tm2.Id
 WHERE DAY(tm.MatchDate) = DAY(tm2.MatchDate)
   AND tm2.MatchDate > tm.MatchDate
 ORDER BY tm.MatchDate DESC, tm2.MatchDate DESC

-- 10. Pairs of Matches on the Same Day

SELECT tm.MatchDate AS [First Date]
	   , tm2.MatchDate AS [Second Date]
  FROM TeamMatches tm, TeamMatches tm2
 WHERE tm2.MatchDate > tm.MatchDate 
   AND DATEDIFF(DAY, tm.MatchDate, tm2.MatchDate) < 1
ORDER BY tm.MatchDate DESC, tm2.MatchDate DESC

-- 11. Mix of Team Names

SELECT LOWER(CONCAT(LEFT(t.TeamName, LEN(t.TeamName) - 1), REVERSE(t2.TeamName))) AS [Mix]
  FROM Teams AS t
 CROSS JOIN Teams AS t2
 WHERE RIGHT(t.TeamName, 1) = LEFT(REVERSE(t2.TeamName), 1)
 ORDER BY [Mix]

-- 11. Mix of Team Names

SELECT LOWER(SUBSTRING(t1.teamname, 1, LEN(t1.TeamName) - 1) +  REVERSE(t2.TeamName)) AS [Mix]
  FROM Teams t1, Teams t2
 WHERE RIGHT(t1.TeamName, 1) = RIGHT(t2.TeamName, 1)
 ORDER BY [Mix]

-- 12. Countries with International and Team Matches

SELECT c.CountryName AS [Country Name]
	   , COUNT(DISTINCT im.Id) AS [International Matches]
	   , COUNT(DISTINCT tm.Id) AS [Team Matches]
  FROM Countries AS c
  LEFT JOIN InternationalMatches AS im ON im.HomeCountryCode = c.CountryCode OR im.AwayCountryCode = c.CountryCode
  LEFT JOIN Leagues AS l ON c.CountryCode = l.CountryCode
  LEFT JOIN TeamMatches AS tm ON l.Id = tm.LeagueId
 GROUP BY c.CountryName
HAVING COUNT(DISTINCT im.Id) > 0 OR COUNT(DISTINCT tm.Id) > 0
 ORDER BY COUNT(im.Id) DESC, COUNT(tm.Id) DESC, c.CountryName

-- 13. Non-international Matches

CREATE TABLE FriendlyMatches
(
	Id INT PRIMARY KEY IDENTITY, 
	HomeTeamId INT, 
	AwayTeamId INT, 
	MatchDate DATETIME,
	CONSTRAINT FK_FriendlyMatches_Teams_HomeTeam FOREIGN KEY(HomeTeamId)
	REFERENCES Teams(Id),
	CONSTRAINT FK_FriendlyMatches_Teams_AwayTeam FOREIGN KEY(AwayTeamId )
	REFERENCES Teams(Id)
)

INSERT INTO Teams(TeamName) VALUES
 ('US All Stars'),
 ('Formula 1 Drivers'),
 ('Actors'),
 ('FIFA Legends'),
 ('UEFA Legends'),
 ('Svetlio & The Legends')
GO

INSERT INTO FriendlyMatches(HomeTeamId, AwayTeamId, MatchDate) 
     VALUES
  
((SELECT Id FROM Teams WHERE TeamName='US All Stars'), 
 (SELECT Id FROM Teams WHERE TeamName='Liverpool'),
 '30-Jun-2015 17:00'),
 
((SELECT Id FROM Teams WHERE TeamName='Formula 1 Drivers'), 
 (SELECT Id FROM Teams WHERE TeamName='Porto'),
 '12-May-2015 10:00'),
 
((SELECT Id FROM Teams WHERE TeamName='Actors'), 
 (SELECT Id FROM Teams WHERE TeamName='Manchester United'),
 '30-Jan-2015 17:00'),

((SELECT Id FROM Teams WHERE TeamName='FIFA Legends'), 
 (SELECT Id FROM Teams WHERE TeamName='UEFA Legends'),
 '23-Dec-2015 18:00'),

((SELECT Id FROM Teams WHERE TeamName='Svetlio & The Legends'), 
 (SELECT Id FROM Teams WHERE TeamName='Ludogorets'),
 '22-Jun-2015 21:00')
GO

SELECT t.TeamName AS [Home Team]
	   , t2.TeamName AS [Away Team]
	   , fm.MatchDate AS [Match Date]
  FROM FriendlyMatches AS fm
  JOIN Teams AS t ON fm.HomeTeamId = t.Id
  JOIN Teams AS t2 ON fm.AwayTeamId = t2.Id
 UNION
SELECT t.TeamName AS [Home Team]
       , t2.TeamName AS [Away Team]
	   , tm.MatchDate AS [Match Date]
  FROM TeamMatches AS tm
  JOIN Teams AS t ON tm.HomeTeamId = t.Id
  JOIN Teams AS t2 ON tm.AwayTeamId = t2.Id
 ORDER BY [Match Date] DESC

-- 14. Seasonal Matches

ALTER TABLE Leagues
ADD IsSeasonal BIT NOT NULL
CONSTRAINT DF_IsSeasonal DEFAULT 0

INSERT INTO TeamMatches(HomeTeamId,AwayTeamId,HomeGoals,AwayGoals,MatchDate,LeagueId)
	 VALUES (
			  (SELECT Id FROM Teams WHERE TeamName = 'Empoli')
			  , (SELECT Id FROM Teams WHERE TeamName = 'Parma')
			  , 2, 2, '2015-04-19 16:00', (SELECT Id FROM Leagues WHERE LeagueName = 'Italian Serie A')
			) 	

INSERT INTO TeamMatches(HomeTeamId,AwayTeamId,HomeGoals,AwayGoals,MatchDate,LeagueId)
	 VALUES (
			  (SELECT Id FROM Teams WHERE TeamName = 'Internazionale')
			  , (SELECT Id FROM Teams WHERE TeamName = 'AC Milan')
			  , 0, 0, '2015-04-19 21:45', (SELECT Id FROM Leagues WHERE LeagueName = 'Italian Serie A')
			) 

 UPDATE Leagues 
    SET IsSeasonal = 1
  WHERE Id IN (SELECT LeagueId FROM TeamMatches)
--WHERE Id IN (
--				SELECT l.Id
--				  FROM Leagues l
--				  JOIN TeamMatches tm ON tm.LeagueId = l.Id
--				 GROUP BY l.Id
--				HAVING COUNT(tm.Id) > 0
--			  )

 SELECT t.TeamName AS [Home Team]
        , tm.HomeGoals AS [Home Goals]
		, t2.TeamName AS [Away Team]
		, tm.AwayGoals AS [Away Goals]
		, l.LeagueName AS [League Name]
   FROM Leagues AS l
   JOIN TeamMatches AS tm ON l.Id = tm.LeagueId
   JOIN Teams AS t ON tm.HomeTeamId = t.Id
   JOIN Teams AS t2 ON tm.AwayTeamId = t2.Id
  WHERE l.IsSeasonal = 1
    AND tm.MatchDate > '2015-04-10'
     -- tm.MatchDate > '10-Apr-2015'
  ORDER BY l.LeagueName, tm.HomeGoals DESC, tm.AwayGoals DESC

-- 15. Stored Function: Bulgarian Teams with Matches JSON

IF OBJECT_ID('fn_TeamsJSON') IS NOT NULL
  DROP FUNCTION fn_TeamsJSON
GO

CREATE FUNCTION fn_TeamsJSON()
RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @json VARCHAR(MAX) = '{"teams":['
 
    DECLARE teamsCursor CURSOR FOR
    SELECT Id, TeamName FROM Teams
    WHERE CountryCode = 'BG'
    ORDER BY TeamName
 
    OPEN teamsCursor
    DECLARE @teamId INT
    DECLARE @teamName VARCHAR(MAX)
    FETCH NEXT FROM teamsCursor INTO @teamId, @teamName
 
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @json = @json + '{"name":"' + @teamName + '","matches":['
 
        DECLARE teamMatchesCursor CURSOR FOR
         SELECT t.TeamName, tm.HomeGoals, t2.TeamName, tm.AwayGoals, tm.MatchDate
           FROM TeamMatches AS tm
           JOIN Teams AS t ON tm.HomeTeamId = t.Id
           JOIN Teams AS t2 ON tm.AwayTeamId = t2.Id
          WHERE tm.HomeTeamId = @teamId OR tm.AwayTeamId = @teamId
          ORDER BY tm.MatchDate DESC
 
        OPEN teamMatchesCursor
        DECLARE @homeTeamName VARCHAR(MAX)  
        DECLARE @homeTeamGoals INT
        DECLARE @awayTeamName VARCHAR(MAX)
        DECLARE @awayTeamGoals INT
        DECLARE @matchDate DATETIME
        FETCH NEXT FROM teamMatchesCursor INTO @homeTeamName, @homeTeamGoals, @awayTeamName, @awayTeamGoals, @matchDate
       
        WHILE @@FETCH_STATUS = 0
        BEGIN
            SET @json = @json + '{"' + @homeTeamName + '":' + CONVERT(VARCHAR(MAX), @homeTeamGoals) + ',"' + @awayTeamName + '":' +                              
                        CONVERT(VARCHAR(MAX), @awayTeamGoals) + ',"date"' + ':' + CONVERT(VARCHAR(10), @matchDate, 103) + '}'
       
            FETCH NEXT FROM teamMatchesCursor INTO @homeTeamName, @homeTeamGoals, @awayTeamName, @awayTeamGoals, @matchDate
            IF @@FETCH_STATUS = 0
                SET @json = @json + ','
        END
        CLOSE teamMatchesCursor
        DEALLOCATE teamMatchesCursor
 
        SET @json = @json + ']}'
 
        FETCH NEXT FROM teamsCursor INTO @teamId, @teamName
       
        IF @@FETCH_STATUS = 0
            SET @json = @json + ','
    END
    CLOSE teamsCursor
    DEALLOCATE teamsCursor
 
    SET @json = @json + ']}'
    RETURN @json
END