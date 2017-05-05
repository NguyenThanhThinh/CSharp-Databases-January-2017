-- 01. Employee Address

SELECT TOP (5) e.EmployeeID, e.JobTitle, a.AddressID, a.AddressText 
  FROM Employees AS e
  JOIN Addresses AS a ON e.AddressID = a.AddressID
 ORDER BY a.AddressID

-- 02. Addresses with Towns

SELECT TOP (50) e.FirstName, e.LastName, t.Name AS Town, a.AddressText
  FROM Employees AS e
  JOIN Addresses AS a ON e.AddressID = a.AddressID
  JOIN Towns AS t ON a.TownID = t.TownID
 ORDER BY e.FirstName, e.LastName

-- 03. Sales Employees

SELECT e.EmployeeID, e.FirstName, e.LastName, d.Name AS DepartmentName
  FROM Employees AS e
  JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
 WHERE d.Name = 'Sales'
 ORDER BY e.EmployeeID

-- 04. Employee Departments

SELECT TOP (5) e.EmployeeID, e.FirstName, e.Salary, d.Name AS DepartmentName
  FROM Employees AS e
  JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
 WHERE e.Salary > 15000
 ORDER BY d.DepartmentID

-- 05. Employees Without Projects

SELECT TOP (3) e.EmployeeID, e.FirstName
  FROM Employees AS e
  LEFT JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
 WHERE ep.EmployeeID IS NULL
 ORDER BY e.EmployeeID

-- 06. Employees Hired After

SELECT e.FirstName, e.LastName, e.HireDate, d.Name AS DepartmentName
  FROM Employees AS e
  JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
 WHERE e.HireDate > '1991-01-01' 
   AND (d.Name = 'Sales' OR d.Name = 'Finance')

-- 07. Employees With Project

SELECT TOP (5) e.EmployeeID, e.FirstName, p.Name AS ProjectName
  FROM Employees AS e
  JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
  JOIN Projects AS p ON ep.ProjectID = p.ProjectID
 WHERE p.StartDate > '2002-08-13' 
   AND p.EndDate IS NULL
 ORDER BY e.EmployeeID

-- 08. Employee 24

SELECT e.EmployeeID, e.FirstName, p.Name AS ProjectName
  FROM Employees AS e 
  JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
  LEFT JOIN Projects AS p ON ep.ProjectID = p.ProjectID AND p.StartDate < '2005-01-01'
 WHERE e.EmployeeID = 24 

-- 09. Employee Manager 

SELECT e.EmployeeID, e.FirstName, e.ManagerID, m.FirstName AS ManagerName
  FROM Employees AS e
  JOIN Employees AS m ON e.ManagerID= m.EmployeeID
 WHERE e.ManagerID = 3 OR e.ManagerID = 7
 ORDER BY e.EmployeeID

-- 10. Employees Summary

SELECT TOP (50) 
	   e.EmployeeID
	   , CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName
	   , CONCAT(m.FirstName, ' ', m.LastName) AS ManagerName 
	   , d.Name AS DepartmentName	   
  FROM Employees AS e
  JOIN Employees AS m ON e.ManagerID = m.EmployeeID
  JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
 ORDER BY e.EmployeeID

-- 11. Min Average Salary

SELECT MIN (averageSalariesPerDepartment.averageSalaries) AS [MinAverageSalary]
  FROM (
         SELECT e.DepartmentID
                , AVG(e.Salary) AS [averageSalaries]
           FROM employees AS e
          GROUP BY e.DepartmentID
       )     AS [averageSalariesPerDepartment

-- 12. Highest Peaks in Bulgaria

SELECT mc.CountryCode, m.MountainRange, p.PeakName, p.Elevation 
  FROM Mountains AS m
  JOIN Peaks AS p ON m.Id = p.MountainId
  JOIN MountainsCountries AS mc ON p.MountainId = mc.MountainId
 WHERE p.Elevation > 2835 AND mc.CountryCode = 'BG'
 ORDER BY p.Elevation DESC

-- 13. Count Mountain Ranges

SELECT mc.CountryCode, COUNT(m.MountainRange) AS [MountainRanges]
  FROM Mountains AS m
  JOIN MountainsCountries AS mc ON m.Id = mc.MountainId
 WHERE mc.CountryCode IN ('BG', 'RU', 'US')
 GROUP BY mc.CountryCode

-- 14. Countries With or Without Rivers

SELECT TOP (5) c.CountryName, r.RiverName
  FROM Countries AS c
  LEFT JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode
  LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
 WHERE c.ContinentCode = 'AF'
 ORDER BY c.CountryName 

-- 15. Continents and Currencies

SELECT usages.ContinentCode
	   , usages.CurrencyCode
	   , usages.Usage 
  FROM
	  (
		 SELECT c.ContinentCode, c.CurrencyCode, COUNT(*) AS Usage 
		   FROM Countries c
		  GROUP BY c.ContinentCode, c.CurrencyCode
		 HAVING COUNT(*) > 1
	  )		 AS usages
  JOIN	
	  (
		 SELECT usages.ContinentCode
		        , MAX(usages.Usage) AS MaxUsage  
		   FROM
			   (
				  SELECT c.ContinentCode, c.CurrencyCode, COUNT(*) AS Usage 
				    FROM Countries c
				   GROUP BY c.ContinentCode, c.CurrencyCode
			   )	  AS usages
		  GROUP BY usages.ContinentCode
	  )		 AS maxUsages 
	ON usages.ContinentCode = maxUsages.ContinentCode
   AND usages.Usage = maxUsages.MaxUsage

-- 15. Continents and Currencies

SELECT
      usages.ContinentCode
      , usages.CurrencyCode
      , usages.CurrUsage AS CurrencyUsage
  FROM
      (
         SELECT c.ContinentCode
                , cr.CurrencyCode
                , COUNT(cr.CurrencyCode) AS CurrUsage
                , DENSE_RANK() OVER (PARTITION BY c.ContinentCode ORDER BY COUNT(cr.CurrencyCode) DESC) AS ranks
           FROM  Currencies cr
           JOIN Countries c ON cr.CurrencyCode = c.CurrencyCode
          GROUP BY c.ContinentCode, cr.CurrencyCode
         HAVING COUNT(cr.CurrencyCode) > 1
      )      AS usages
 WHERE usages.ranks = 1
 ORDER BY ContinentCode

-- 16. Countries Without any Mountains

  SELECT COUNT(c.CountryCode) AS [CountryCode]
   FROM Countries AS c
   LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
  WHERE mc.MountainId IS NULL

-- 17. Highest Peak and Longest River by Country

SELECT TOP (5) 
	   c.CountryName
	   , MAX(p.Elevation) AS [HighestPeakElevation]
	   , MAX(r.Length) AS [LongestRiverLength]
  FROM Countries AS c
  LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
  LEFT JOIN Peaks AS p ON p.MountainId = mc.MountainId
  LEFT JOIN CountriesRivers AS cr ON c.CountryCode = cr.CountryCode
  LEFT JOIN Rivers AS r ON r.Id = cr.RiverId 
 GROUP BY c.CountryName
 ORDER BY [HighestPeakElevation] DESC, [LongestRiverLength] DESC, c.CountryName


-- 18. Highest Peak Name and Elevation by Country

WITH res 
AS
(
	SELECT
		  c.CountryName
		  , p.PeakName
		  , p.Elevation
		  , m.MountainRange
		  , ROW_NUMBER() OVER (PARTITION BY c.CountryName ORDER BY p.Elevation DESC) AS currentRank
		  -- RANK() or DENSE_RANK()
	  FROM Countries AS c
	  LEFT JOIN MountainsCountries AS mc ON c.CountryCode = mc.CountryCode
	  LEFT JOIN Mountains AS m ON mc.MountainId = m.Id
	  LEFT JOIN Peaks p ON m.Id = p.MountainId
)

SELECT TOP (5)
       res.CountryName AS [Country],
       ISNULL(res.PeakName, '(no highest peak)') AS [Highest Peak Name],
       ISNULL(res.Elevation, 0) AS [Highest Peak Elevation],
       CASE 
		  WHEN res.PeakName IS NOT NULL THEN res.MountainRange 
		  ELSE '(no mountain)' 
	   END AS [Mountain]
  FROM res
 WHERE currentRank = 1


-- 18. Highest Peak Name and Elevation by Country

SELECT TOP (5) * 
  FROM 
	  (
	    SELECT c.CountryName
			   , p.PeakName 
			   , p.Elevation 
  			   , m.MountainRange
		  FROM Countries AS c
		  JOIN MountainsCountries AS mc
			ON c.CountryCode = mc.CountryCode
		  JOIN Mountains AS m
			ON m.Id = mc.MountainId
		  JOIN Peaks AS p
			ON p.MountainId = m.Id
		  JOIN (
				SELECT c.CountryName, MAX(p.Elevation) AS [MaxElevation] 
				  FROM Countries AS c 
  				  JOIN MountainsCountries AS mc
					ON c.CountryCode = mc.CountryCode
				  JOIN Mountains AS m
					ON m.Id = mc.MountainId
				  JOIN Peaks AS p
					ON p.MountainId = m.Id
			     GROUP BY c.CountryName
				)   AS [max]
			 ON max.MaxElevation = p.Elevation
            AND max.CountryName = c.CountryName
	     UNION ALL
	    SELECT c.CountryName
			   , '(no highest peak)' AS [PeakName]
			   , 0 AS [Elevation]
			   , '(no mountain)' AS [MountainRange] 
		  FROM Countries AS c
		  LEFT JOIN MountainsCountries AS mc
			ON c.CountryCode = mc.CountryCode
		 WHERE mc.MountainId IS NULL
	  )     AS result
 ORDER BY result.CountryName, result.PeakName

-- 19. Peaks in Rila (Problem 09. Table Relations)
 
SELECT m.MountainRange, p.PeakName, p.Elevation
  FROM Mountains AS m
  JOIN Peaks AS p ON m.Id = p.MountainId
 WHERE m.MountainRange = 'Rila'
 ORDER BY p.Elevation DESC