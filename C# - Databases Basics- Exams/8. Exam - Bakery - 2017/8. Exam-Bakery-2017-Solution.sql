-- 1. Data Definition

CREATE TABLE Countries
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(50) NOT NULL UNIQUE
)

CREATE TABLE Distributors
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(25) NOT NULL UNIQUE,
	AddressText NVARCHAR(30),
	Summary NVARCHAR(200),
	CountryId INT NOT NULL,
	CONSTRAINT FK_Distributors_Countries FOREIGN KEY(CountryId)
	REFERENCES Countries(Id)
)

CREATE TABLE Customers
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(25),
	LastName NVARCHAR(25),
	Gender CHAR(1) CHECK (Gender = 'M' OR Gender = 'F'),
	Age INT,
	PhoneNumber CHAR(10),
	CountryId INT,
	CONSTRAINT FK_Customers_Countries FOREIGN KEY(CountryId)
	REFERENCES Countries(Id)
)

CREATE TABLE Products
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(25) UNIQUE,
	Description NVARCHAR(250),
	Recipe NVARCHAR(MAX),
	Price MONEY CHECK (Price > 0)
)

CREATE TABLE Feedbacks
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Description NVARCHAR(255),
	Rate DECIMAL(10,2),
	ProductId INT,
	CustomerId INT,
	CONSTRAINT FK_Feedbacks_Products FOREIGN KEY(ProductId)
	REFERENCES Products(Id),
	CONSTRAINT FK_Feedbacks_Customers FOREIGN KEY(CustomerId)
	REFERENCES Customers(Id)
)

CREATE TABLE Ingredients
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(30),
	Description NVARCHAR(200),
	OriginCountryId INT,
	DistributorId INT,
	CONSTRAINT FK_Ingredients_Countries FOREIGN KEY(OriginCountryId)
	REFERENCES Countries(Id),
	CONSTRAINT FK_Ingredients_Distributors FOREIGN KEY(DistributorId)
	REFERENCES Distributors(Id)
)

CREATE TABLE ProductsIngredients
(
	ProductId INT NOT NULL,
	IngredientId INT NOT NULL,
	CONSTRAINT PK_ProductsIngredients PRIMARY KEY(ProductId, IngredientId),
	CONSTRAINT FK_ProductsIngredients_Products FOREIGN KEY(ProductId)
	REFERENCES Products(Id),
	CONSTRAINT FK_ProductsIngredients_Ingredients FOREIGN KEY(IngredientId)
	REFERENCES Ingredients(Id)
)

-- 2. Insert

INSERT INTO Distributors(Name, CountryId, AddressText, Summary)
     VALUES ('Deloitte & Touche', 2, '6 Arch St #9757', 'Customizable neutral traveling')
	 , ('Congress Title', 13, '58 Hancock St', 'Customer loyalty')
	 , ('Kitchen People', 1, '3 E 31st St #77', 'Triple-buffered stable delivery')
	 , ('General Color Co Inc', 21,	'6185 Bohn St #72', 'Focus group')
	 , ('Beck Corporation', 23, '21 E 64th Ave', 'Quality-focused 4th generation hardware')

INSERT INTO Customers (FirstName, LastName, Age, Gender, PhoneNumber, CountryId)
     VALUES ('Francoise', 'Rautenstrauch', 15, 'M', '0195698399', 5)
			, ('Kendra', 'Loud', 22, 'F', '0063631526', 11)
			, ('Lourdes', 'Bauswell', 50, 'M', '0139037043', 8)
			, ('Hannah', 'Edmison',	18, 'F', '0043343686', 1)
			, ('Tom', 'Loeza', 31, 'M', '0144876096', 23)
			, ('Queenie', 'Kramarczyk',	30, 'F', '0064215793', 29)
			, ('Hiu', 'Portaro', 25, 'M', '0068277755', 16)
			, ('Josefa', 'Opitz', 43, 'F', '0197887645', 17) 

-- 3. Update

UPDATE Ingredients
   SET DistributorId = 35
 WHERE Name IN ('Bay Leaf', 'Paprika', 'Poppy')

UPDATE Ingredients
   SET OriginCountryId = 14
 WHERE OriginCountryId = 8

-- 4. Delete

DELETE FROM Feedbacks
 WHERE CustomerId = 14
    OR ProductId = 5

-- 5. Product By Price

SELECT p.Name
	   , p.Price
	   , p.Description
  FROM Products AS p
 ORDER BY p.Price DESC, p.Name

-- 6. Ingredients

SELECT i.Name
	   , i.Description
	   , i.OriginCountryId
  FROM Ingredients AS i
 WHERE i.OriginCountryId IN (1,10,20)
 ORDER BY i.Id 

-- 7. Ingredients From Bulgaria And Greece

SELECT TOP (15)
	   i.Name AS [Name]
	   , i.Description AS [Description]
	   , c.Name AS [CountryName]
  FROM Ingredients AS i
  JOIN Countries AS c ON i.OriginCountryId = c.Id
 WHERE c.Name IN ('Bulgaria', 'Greece')
 ORDER BY i.Name, c.Name

-- 8. Best Rated Products

 SELECT TOP (10)
		p.Name AS [Name]
	    , p.Description AS [Description]
		, AVG(f.Rate) AS [AverageRate]
		, COUNT(f.ProductId) AS [FeedbacksAmount]
   FROM	Products AS p
   JOIN Feedbacks AS f ON p.Id = f.ProductId
  GROUP BY p.Name, p.Description	
  ORDER BY AVG(f.Rate) DESC, COUNT(f.ProductId) DESC

-- 9. Negative Feedback

SELECT f.ProductId
  	   , f.Rate
	   , f.Description
	   , f.CustomerId
	   , c.Age
	   , c.Gender
  FROM Feedbacks AS f
  LEFT JOIN Customers AS c ON f.CustomerId = c.Id
    -- the JUDGE system also accepts the solution with simple JOIN
 WHERE f.Rate < 5.0
 ORDER BY f.ProductId DESC, f.Rate

-- 10. Customers Without Feedback

SELECT CONCAT(c.FirstName, ' ', c.LastName) AS [CustomerName]
       , c.PhoneNumber AS [PhoneNumber]
	   , c.Gender AS [Gender]
  FROM Customers AS c
  LEFT JOIN Feedbacks AS f ON c.Id = f.CustomerId
 WHERE f.CustomerId IS NULL

-- 11. Honorable Mentions

SELECT f.ProductId AS [ProductId]
       , CONCAT(c.FirstName, ' ', c.LastName) AS [CustomerName]
	   , ISNULL(f.Description, '') AS [FeedbackDescription]
	-- the JUDGE system also accepts the solution without the ISNULL check
  FROM Customers AS c
  JOIN Feedbacks AS f ON c.Id = f.CustomerId 
 WHERE f.CustomerId IN (SELECT f.CustomerId FROM Customers AS c
						  JOIN Feedbacks AS f ON c.Id = f.CustomerId
						 GROUP BY f.CustomerId
						HAVING COUNT(f.CustomerId) >= 3)
 ORDER BY f.ProductId, [CustomerName], f.Id

-- 12. Customers By Criteria

SELECT c.FirstName
	   , c.Age
	   , c.PhoneNumber
  FROM Customers AS c
  JOIN Countries AS co ON c.CountryId = co.Id
 WHERE (c.Age >= 21 AND c.FirstName LIKE '%an%')
    OR (co.Name != 'Greece' AND c.PhoneNumber LIKE '%38')
 ORDER BY c.FirstName, c.Age DESC

-- 13. Middle Range Distributors

SELECT d.Name AS [DistributorName]
	   , i.Name AS [IngredientName]
	   , p.Name AS [ProductName]
	   , AVG(f.Rate) AS [AverageRate]
  FROM Distributors AS d
  JOIN Ingredients AS i ON d.Id = i.DistributorId
  JOIN ProductsIngredients AS ip ON i.Id = ip.IngredientId
  JOIN Products AS p ON ip.ProductId = p.Id 
  JOIN Feedbacks AS f ON p.Id = f.ProductId
 GROUP BY d.Name, i.Name, p.Name
HAVING AVG(f.Rate) >= 5.0 AND AVG(f.Rate) <= 8.0
 ORDER BY d.Name, i.Name, p.Name

-- 14. The Most Positive Country

SELECT TOP (1) WITH TIES
    -- using WITH TIES is required by the JUDGE system
	   co.Name AS [CountryName]
	   , AVG(f.Rate) AS [FeedbackRate]
  FROM Customers AS c
  JOIN Countries AS co ON c.CountryId = co.Id
  JOIN Feedbacks AS f ON c.Id = f.CustomerId
 GROUP BY co.Name
 ORDER BY AVG(f.Rate) DESC

-- 15. Country Representative

SELECT c.Name AS [CountryName]
	   , d.Name AS [DisributorName]
  FROM Countries AS c
  JOIN Distributors AS d ON c.Id = d.CountryId
 WHERE d.Name IN (
				  SELECT res.DistributorName
				    FROM (
						  SELECT d2.Name AS [DistributorName]
							     , co.Name AS [CountryName]
							     , DENSE_RANK() OVER (PARTITION BY co.Name ORDER BY COUNT(i.Id) DESC) AS [curRank]
					        FROM Ingredients AS i
					       RIGHT JOIN Distributors AS d2 ON i.DistributorId = d2.Id
						   RIGHT JOIN Countries AS co ON d2.CountryId = co.Id
					       GROUP BY d2.Name, co.Name
						  ) AS [res]
				   WHERE res.curRank = 1
				 )
 GROUP BY c.Name, d.Name
 ORDER BY c.Name, d.Name

-- 15. Country Representative

SELECT res.CountryName AS [CountryName]
	   , res.DistributorName AS [DisributorName]
  FROM (
		SELECT d.Name AS [DistributorName]
		       , c.Name AS [CountryName]
		       , DENSE_RANK() OVER (PARTITION BY c.Name ORDER BY COUNT(i.Id) DESC) AS [curRank]
          FROM Countries AS c
          LEFT JOIN Distributors AS d ON c.Id = d.CountryId
	      LEFT JOIN Ingredients AS i ON d.Id = i.DistributorId
         GROUP BY d.Name, c.Name
		) AS res
  WHERE res.curRank = 1
	AND res.DistributorName IS NOT NULL

-- 16. Customers With Countries

CREATE VIEW v_UserWithCountries 
AS
SELECT CONCAT(c.FirstName, ' ', c.LastName) AS [CustomerName]
	   , c.Age AS [Age]
	   , c.Gender AS [Gender]
	   , co.Name AS [CountryName]
  FROM Customers AS c
  JOIN Countries AS co ON c.CountryId = co.Id

-- 17. Feedback ByProduct Name

CREATE FUNCTION udf_GetRating(@Name NVARCHAR(25))
RETURNS VARCHAR(10)
BEGIN
	DECLARE @Rating NVARCHAR(10)
	DECLARE @ProductRate DECIMAL(10,2)

	SET @ProductRate = (
						 SELECT AVG(f.Rate) 
						   FROM Products AS p
						   JOIN Feedbacks AS f ON p.Id = f.ProductId
						  WHERE p.Name = @Name
					   )
	IF @ProductRate < 5
	BEGIN
	   SET @Rating = 'Bad'
	END
	ELSE IF @ProductRate BETWEEN 5 AND 8
	BEGIN
	   SET @Rating = 'Average'
	END
	ElSE IF @ProductRate > 8
	BEGIN
	   SET @Rating = 'Good'
	END
	ELSE 
	BEGIN
	   SET @Rating = 'No rating'
	END

	RETURN @Rating
END 

-- 18. Send Feedback

CREATE PROCEDURE usp_SendFeedback(@CustomerId INT, @ProductId INT, @Rate DECIMAL(10,2), @Description NVARCHAR(255))
AS
BEGIN
	BEGIN TRANSACTION

	INSERT INTO Feedbacks (CustomerId, ProductId, Rate, Description)
	     VALUES (@CustomerId, @ProductId, @Rate, @Description)

	DECLARE @NumerOfFeedbacks INT
	    SET @NumerOfFeedbacks = (
								  SELECT COUNT(f.Id)
									FROM Feedbacks AS f
								   WHERE f.CustomerId = @CustomerId
								   GROUP BY f.CustomerId	 
								)
	IF @NumerOfFeedbacks > 3
	BEGIN
		ROLLBACK
		RAISERROR('You are limited to only 3 feedbacks per product!', 16, 1)
		RETURN
	END
	COMMIT
END

-- 19. Delete Products

CREATE TRIGGER tr_DeleteProductRelations
ON Products
INSTEAD OF DELETE
AS
BEGIN

	DELETE FROM ProductsIngredients
	 WHERE ProductId IN (SELECT d.Id FROM DELETED AS d)

	DELETE FROM Feedbacks
	 WHERE ProductId IN (SELECT d.Id FROM DELETED AS d)

	DELETE FROM Products
	 WHERE Id IN (SELECT d.Id FROM DELETED AS d) 

END

-- 20. Products By One Distributor

SELECT (
		SELECT p.Name 
		  FROM Products AS p 
		 WHERE p.Id = ip.ProductId
	   ) AS ProductName

	   , (
		  SELECT AVG(f.Rate) 
		    FROM Feedbacks AS f 
		   WHERE f.ProductId = ip.ProductId 
		   GROUP BY f.ProductId
		 ) AS ProductAverageRate

       , (
	      SELECT TOP (1) 
		         d.Name 
		    FROM Distributors AS d
			JOIN Ingredients AS i ON i.DistributorId = d.Id
			JOIN ProductsIngredients AS ip2 ON i.Id = ip2.IngredientId 
			WHERE ip2.ProductId = ip.ProductId
	     ) AS DistributorName
		 
		, (
		   SELECT c.Name 
		     FROM Countries AS c 
			WHERE c.Id = (
						   SELECT TOP (1) 
						          d.CountryId 
						     FROM Distributors AS d
						     JOIN Ingredients AS i ON i.DistributorId = d.Id
						     JOIN ProductsIngredients AS ip2 ON i.Id = ip2.IngredientId 
						    WHERE ip2.ProductId = ip.ProductId
						 )
		  ) AS DistributorCountry 
  FROM Products AS p
  JOIN ProductsIngredients AS ip ON p.Id = ip.ProductId
  JOIN Ingredients AS i ON ip.IngredientId = i.Id
 GROUP BY ip.ProductId
HAVING COUNT(DISTINCT i.DistributorId) = 1
 ORDER BY ip.ProductId