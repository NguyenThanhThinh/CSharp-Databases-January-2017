-- 01. Records’ Count

SELECT COUNT(Id) 
  FROM WizzardDeposits

-- 02. Longest Magic Wand

SELECT MAX(MagicWandSize) AS [LongestMagicWand]
  FROM WizzardDeposits

-- 03. Longest Magic Wand per Deposit Groups

SELECT DepositGroup, MAX(MagicWandSize) AS [LongestMagicWand]
  FROM WizzardDeposits
 GROUP BY DepositGroup

-- 04. Smallest Deposit Group per Magic Wand Size

SELECT TOP (1) WITH TIES w.DepositGroup
  FROM WizzardDeposits AS w
 GROUP BY w.DepositGroup 
 ORDER BY AVG(w.MagicWandSize)

-- 04. Smallest Deposit Group per Magic Wand Size

SELECT DepositGroup 
  FROM WizzardDeposits
GROUP BY DepositGroup
HAVING AVG(MagicWandSize) = (
							 SELECT MIN(AllAverageMagicWandSize.AverageMagicWandSize)
							   FROM (
									 SELECT w.DepositGroup, AVG(w.MagicWandSize) AS AverageMagicWandSize
									   FROM WizzardDeposits AS w
									  GROUP BY w.DepositGroup
					   			    )    AS AllAverageMagicWandSize
							 )

-- 04. Smallest Deposit Group per Magic Wand Size

SELECT DepositGroup
  FROM WizzardDeposits
	   , (
		   SELECT AVG(MagicWandSize) AS AverageMagicWandSize
             FROM WizzardDeposits
            GROUP BY DepositGroup
		 )     AS AllAverageMagicWandSize 
 GROUP BY DepositGroup
HAVING AVG(MagicWandSize) = MIN(AllAverageMagicWandSize.AverageMagicWandSize)

-- 05. Deposits Sum

SELECT DepositGroup, SUM(DepositAmount) AS DepositAmountSum
  FROM WizzardDeposits
 GROUP BY DepositGroup

-- 06. Deposits Sum for Ollivander Family

SELECT DepositGroup, SUM(DepositAmount) AS DepositAmountSum
  FROM WizzardDeposits
 WHERE MagicWandCreator = 'Ollivander family'
 GROUP BY DepositGroup

-- 07. Deposits Filter

SELECT DepositGroup, SUM(DepositAmount) AS DepositAmountSum
  FROM WizzardDeposits
 WHERE MagicWandCreator = 'Ollivander family'
 GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
 ORDER BY SUM(DepositAmount) DESC

-- 08. Deposit Charge

SELECT DepositGroup, MagicWandCreator, MIN(DepositCharge) AS MinDepositCharge 
  FROM WizzardDeposits
 GROUP BY DepositGroup, MagicWandCreator
 ORDER BY MagicWandCreator, DepositGroup

-- 09. Age Groups

SELECT 
	CASE 
		WHEN [Age] BETWEEN 0 AND 10 THEN '[0-10]'
		WHEN [Age] BETWEEN 11 AND 20 THEN '[11-20]'
		WHEN [Age] BETWEEN 21 AND 30 THEN '[21-30]'
		WHEN [Age] BETWEEN 31 AND 40 THEN '[31-40]'
		WHEN [Age] BETWEEN 41 AND 50 THEN '[41-50]'
		WHEN [Age] BETWEEN 51 AND 60 THEN '[51-60]'
		ELSE '[61+]'
	END AS 'AgeGroup', COUNT(*) AS 'WizardCount'
FROM [WizzardDeposits]
GROUP BY 
	(CASE 
		WHEN [Age] BETWEEN 0 AND 10 THEN '[0-10]'
		WHEN [Age] BETWEEN 11 AND 20 THEN '[11-20]'
		WHEN [Age] BETWEEN 21 AND 30 THEN '[21-30]'
		WHEN [Age] BETWEEN 31 AND 40 THEN '[31-40]'
		WHEN [Age] BETWEEN 41 AND 50 THEN '[41-50]'
		WHEN [Age] BETWEEN 51 AND 60 THEN '[51-60]'
		ELSE '[61+]'
	END)
 	
-- 09. Age Groups

SELECT AgeGroups.AgeGroup, COUNT(*) 
  FROM ( 
		SELECT
			CASE 
				WHEN [Age] BETWEEN 0 AND 10 THEN '[0-10]'
				WHEN [Age] BETWEEN 11 AND 20 THEN '[11-20]'
				WHEN [Age] BETWEEN 21 AND 30 THEN '[21-30]'
				WHEN [Age] BETWEEN 31 AND 40 THEN '[31-40]'
				WHEN [Age] BETWEEN 41 AND 50 THEN '[41-50]'
				WHEN [Age] BETWEEN 51 AND 60 THEN '[51-60]'
				ELSE '[61+]'
			 END AS [AgeGroup]
			FROM [WizzardDeposits]
		)     AS [AgeGroups]
GROUP BY AgeGroups.AgeGroup


-- 10. First Letter

SELECT LEFT(FirstName, 1) AS FirstLetter
  FROM WizzardDeposits
 WHERE DepositGroup = 'Troll chest'
 GROUP BY LEFT(FirstName, 1)
 ORDER BY FirstLetter

-- 11. Average Interest

SELECT w.DepositGroup, w.IsDepositExpired, AVG(w.DepositInterest) AS AverageDepositInterest
  FROM WizzardDeposits as w
 WHERE w.DepositStartDate >= '1985-01-01'
 GROUP BY w.DepositGroup, w.IsDepositExpired
 ORDER BY w.DepositGroup DESC, w.IsDepositExpired

-- 12. Rich Wizard, Poor Wizard

DECLARE @SumDifference DECIMAL(18,2)

    SET @SumDifference = (
						  SELECT SUM(w.DepositAmount - wi.DepositAmount)
						    FROM WizzardDeposits w
						    JOIN WizzardDeposits wi
						      ON wi.Id = w.Id + 1
						 )

SELECT @SumDifference AS SumDifference

-- 12. Rich Wizard, Poor Wizard

SELECT Sum(Result.Differences) AS SumDifference
  FROM (
		SELECT currentGroup.DepositAmount - (
											  SELECT DepositAmount
											    FROM WizzardDeposits
											   WHERE Id = currentGroup.Id + 1
											)     AS Differences
		  FROM WizzardDeposits AS currentGroup
	   )    AS Result

-- 12. Rich Wizard, Poor Wizard

SELECT SUM(Result.Differences)
  FROM ( 
		SELECT  
			   FirstName
			   , DepositAmount
			   , LEAD(FirstName) OVER (ORDER BY Id) AS NextWizzard --LAG
			   , LEAD(DepositAmount) OVER (ORDER BY Id) AS NextDepostiAmount --LAG
			   , DepositAmount - LEAD(DepositAmount) OVER (ORDER BY Id) AS Differences
		  FROM WizzardDeposits 
	   )    AS Result

-- 12. Rich Wizard, Poor Wizard

DECLARE @previousDeposit DECIMAL(8,2)
DECLARE @currentDeposit DECIMAL(8,2)
DECLARE @totalSum DECIMAL(8,2) = 0

DECLARE wizardCursor CURSOR
FOR SELECT DepositAmount FROM WizzardDeposits

OPEN wizardCursor
FETCH NEXT FROM wizardCursor INTO @currentDeposit

WHILE @@FETCH_STATUS = 0
BEGIN
IF @previousDeposit IS NOT NULL
BEGIN
	SET @totalSum += @previousDeposit - @currentDeposit
END

SET @previousDeposit = @currentDeposit
FETCH NEXT FROM wizardCursor INTO @currentDeposit
END

CLOSE wizardCursor
DEALLOCATE wizardCursor
 
SELECT @totalSum AS SumDifference

-- 13. Departments Total Salaries

SELECT e.DepartmentId, SUM(e.Salary) AS SumSalary 
  FROM Employees AS e
 GROUP BY DepartmentId

-- 14. Employees Minimum Salaries

SELECT e.DepartmentId, MIN(e.Salary) AS MinSalary
  FROM Employees AS e
 WHERE e.DepartmentId in (2,5,7) AND e.HireDate >= '2000-01-01'
 GROUP BY e.DepartmentId

-- 15. Employees Average Salaries

SELECT * INTO RichEmployees
  FROM Employees AS e
 WHERE e.Salary > 30000 

DELETE FROM RichEmployees 
 WHERE ManagerId = 42

UPDATE RichEmployees
   SET Salary += 5000
 WHERE DepartmentId = 1

SELECT DepartmentId, AVG(Salary) AS AverageSalary
  FROM RichEmployees
 GROUP BY DepartmentId

-- 16. Employees Maximum Salaries

SELECT e.DepartmentId, MAX(e.Salary) AS MaxSalary 
  FROM Employees AS e
 GROUP BY DepartmentId
HAVING MAX(e.Salary) NOT BETWEEN 30000 AND 70000

-- 17. Employees Count Salaries

SELECT COUNT(e.EmployeeId) AS Count 
  FROM Employees AS e
 WHERE e.ManagerId IS NULL

-- 18. 3rd Highest Salary

SELECT sal.DepartmentId, sal.Salary
  FROM (
		SELECT e.DepartmentId
			   , e.Salary
			   , DENSE_RANK() OVER (PARTITION BY e.DepartmentId
		 ORDER BY e.Salary DESC) AS SalaryRank
	      FROM Employees AS e
	   )    AS sal
 WHERE sal.SalaryRank = 3
 GROUP BY sal.DepartmentId, sal.Salary

-- 19. Salary Challenge

SELECT TOP (10) e.FirstName, e.LastName, e.DepartmentID 
  FROM Employees AS e
 WHERE e.Salary > (
					SELECT AVG(emp.Salary)
					  FROM Employees AS emp
					 WHERE e.DepartmentID = emp.DepartmentID
					 GROUP BY emp.DepartmentID
				  )

-- 19. Salary Challenge

SELECT TOP (10) e.FirstName, e.LastName, e.DepartmentId
  FROM Employees AS e
       , (
		  SELECT DepartmentId AS avgSalaryOfDepartmentId
				 , AVG(Salary) AS avgSalaryOfDepartment
            FROM Employees
           GROUP BY DepartmentId
		 )    AS avgSalaryByDepartments
 WHERE e.DepartmentId = avgSalaryByDepartments.avgSalaryOfDepartmentId
   AND e.Salary > avgSalaryOfDepartment
 ORDER BY e.DepartmentId