-- 10. Countries Holding 'A'

SELECT c.CountryName, c.IsoCode 
  FROM Countries AS c
 WHERE c.CountryName LIKE '%a%a%a%'
 ORDER BY c.IsoCode

-- 11. Mix of Peak and River 

SELECT p.PeakName, 
	   r.RiverName, 
	   LOWER(CONCAT(SUBSTRING(p.PeakName, 1, LEN(p.PeakName) -1), r.RiverName)) AS Mix
  FROM Peaks AS p, Rivers AS r
 WHERE RIGHT(p.PeakName, 1) = LEFT(r.RiverName, 1)
 ORDER BY Mix

-- 12. Games From 2011 and 2012 Year

 SELECT TOP 50 g.Name, 
CONVERT(VARCHAR(10), g.Start, 120) AS 'Start'
   FROM Games AS g
  WHERE YEAR(g.Start) IN (2011, 2012)
  ORDER BY g.Start, g.Name

-- 13. User Email Providers

SELECT Username,
 RIGHT(Email, LEN(Email) - CHARINDEX('@', email)) AS [Email Provider]
  FROM Users
 ORDER BY [Email Provider], Username

-- 14. Get Users with IPAddress Like Pattern

SELECT u.Username, u.IpAddress 
  FROM Users as u
 WHERE u.IpAddress LIKE '___.[1]%.%.___'
 ORDER BY u.Username

-- 15. Show All Games with Duration

SELECT g.Name, 
	CASE 
		WHEN DATEPART(HOUR, g.Start) >= 0 AND DATEPART(HOUR, g.Start) < 12 THEN 'Morning'
		WHEN DATEPART(HOUR, g.Start) >= 12 AND DATEPART(HOUR, g.Start) < 18 THEN 'Afternoon'
		ELSE 'Evening'		 
	END AS 'Part of the Day',
	CASE 
		WHEN g.Duration <= 3 THEN 'Extra Short'
		WHEN g.Duration >= 4 AND g.Duration <= 6 THEN 'Short'
		WHEN g.Duration > 6 THEN 'Long'
		WHEN g.Duration IS NULL THEN 'Extra Long'
	END AS 'Duration'	 
 FROM Games AS g
ORDER BY g.Name, 'Duration', 'Part of the Day'

-- 16. Orders Table

SELECT o.ProductName 
		,o.OrderDate
		,DATEADD(DAY, 3, o.OrderDate) AS 'Pay Due'
		,DATEADD(MONTH, 1, o.OrderDate) AS 'Deliver Due'
  FROM Orders o;