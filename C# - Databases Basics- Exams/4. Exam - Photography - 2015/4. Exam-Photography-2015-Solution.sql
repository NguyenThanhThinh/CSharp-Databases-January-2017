-- 1. Album’s Name and Description

SELECT Name AS [Name]
	   , ISNULL(Description, 'No description') AS [Description]
  FROM Albums
 ORDER BY [Name]

-- 2. Photographs and Albums

SELECT p.Title AS [Title]
	   , a.Name AS [Name]
  FROM Photographs AS p
  JOIN AlbumsPhotographs AS ap ON p.Id = ap.PhotographId
  JOIN Albums AS a ON ap.AlbumId = a.Id 
 ORDER BY [Name], [Title] DESC

-- 3. Photographs with Category and Author

SELECT p.Title AS [Title]
	   , p.Link AS [Link]
	   , p.Description AS [Description]
	   , c.Name AS [CategoryName]
	   , u.FullName AS [Author]
  FROM Photographs AS p
  JOIN Categories AS c ON p.CategoryId = c.Id
  JOIN Users AS u ON p.UserId = u.Id
 WHERE p.Description IS NOT NULL
 ORDER BY [Title]

-- 4. Users Born in January

  SELECT u.Username AS [Username]
	     , u.FullName AS [FullName]
	     , u.BirthDate AS [BirthDate]
	     , ISNULL(p.Title, 'No photos') AS [Photo]
    FROM Photographs AS p
   RIGHT JOIN Users AS u ON p.UserId = u.Id
   WHERE DATEPART(MONTH, u.BirthDate) = 1
 --WHERE MONTH(u.BirthDate) = 1
   ORDER BY [FullName]

-- 5. Photographs with Equipment

SELECT p.Title AS [Title]
       , c.Model AS [CameraModel]
       , l.Model AS [LensModel]
  FROM Photographs AS p
  JOIN Equipments AS e ON p.EquipmentId = e.Id
  JOIN Cameras AS c ON e.CameraId = c.Id
  JOIN Lenses AS l ON e.LensId = l.Id
 ORDER BY [Title]

-- 6. Most Expensive Photos

SELECT p.Title AS [Title]
       , cat.Name AS [Category Name]
	   , c.Model AS [Model]
	   , m.Name AS [Manufacturer Name]
	   , c.Megapixels AS [Megapixels]
       , c.Price AS [Price]
  FROM Photographs AS p
  JOIN Categories AS cat ON p.CategoryId = cat.Id
  JOIN Equipments AS e ON p.EquipmentId = e.Id
  JOIN Cameras AS c ON e.CameraId = c.Id
  JOIN Manufacturers m ON c.ManufacturerId = m.Id
 WHERE c.Price = (
				  SELECT MAX(cam.Price)
				    FROM Cameras AS cam
				    JOIN Equipments AS equip ON cam.Id = equip.CameraId
				    JOIN Photographs AS photo ON equip.Id = photo.EquipmentId
				   WHERE photo.CategoryId = p.CategoryId
				 )
 ORDER BY [Price] DESC, [Title]

-- 7. Price Larger Than Average

SELECT m.Name AS [Name]
       , c.Model AS [Model]
       , c.Price AS [Price]
  FROM Cameras AS c
  JOIN Manufacturers AS m ON c.ManufacturerId = m.Id
 WHERE c.Price > (SELECT AVG(Price) FROM Cameras WHERE Price IS NOT NULL)
 GROUP BY m.Name, c.Model, c.Price
 ORDER BY [Price] DESC, [Model]

-- 8. Total Price of Lenses

SELECT m.Name AS [Name]
		, SUM(l.Price) AS [Total Price]
  FROM Lenses AS l
  JOIN Manufacturers AS m ON l.ManufacturerId = m.Id
 WHERE l.Price IS NOT NULL
 GROUP BY m.Name
 ORDER BY [Name]

-- 8. Total Price of Lenses

SELECT DISTINCT
		m.Name
		, (SELECT SUM(l2.Price)
			 FROM Lenses AS l2
			 JOIN Manufacturers AS m2 ON l2.ManufacturerId = m2.Id
			WHERE m2.Id = m.Id AND l2.Price IS NOT NULL
		   ) AS [Total Price]
   FROM Manufacturers AS m
   JOIN Lenses AS l ON m.Id = l.ManufacturerId
  ORDER BY m.Name ASC

-- 9. Users with Old Cameras

SELECT u.FullName AS [FullName]
	   , m.Name AS [Manufacturer]
	   , c.Model AS [Camera Model]
	   , c.Megapixels AS [Megapixels]
  FROM Users AS u
  JOIN Equipments AS e ON u.EquipmentId = e.Id
  JOIN Cameras AS c ON e.CameraId = c.Id
  JOIN Manufacturers AS m ON c.ManufacturerId = m.Id
 WHERE c.Year < 2015
 ORDER BY [FullName]

-- 10. Lenses per Type

SELECT l.Type AS [Type]
	   , COUNT(l.Id) AS [Count]
  FROM Lenses AS l
 GROUP BY Type
 ORDER BY [Count] DESC, [Type]

-- 11. Sort Users

SELECT u.Username AS [Username]
 	    , u.FullName AS [FullName]
   FROM Users AS u
  ORDER BY (LEN(u.Username) + LEN(u.FullName)), u.BirthDate DESC
--ORDER BY LEN(CONCAT(u.FullName, u.UserName)), u.BirthDate DESC

-- 12. Manufacturers without Products

SELECT m.Name AS [Name]
  FROM Manufacturers AS m
  LEFT JOIN Cameras AS c ON m.Id = c.ManufacturerId
  LEFT JOIN Lenses AS l ON m.Id = l.ManufacturerId
 WHERE c.ManufacturerId IS NULL 
   AND l.ManufacturerId IS NULL
 ORDER BY [Name]

-- 12. Manufacturers without Products

SELECT m.Name AS [Name]
  FROM Manufacturers AS m
 WHERE m.Id NOT IN (SELECT l.ManufacturerId FROM Lenses AS l) 
   AND m.Id NOT IN (SELECT c.ManufacturerId FROM Cameras AS c)
 ORDER BY [Name]

-- 13. Cameras rise!

UPDATE Cameras 
  SET Price += (
 				 (
				  SELECT AVG(Price) 
				    FROM Cameras AS c 
				   WHERE ManufacturerId = c.ManufacturerId
				 ) 
				 * 
				 (
				  (SELECT LEN(Name)/100.00 FROM Manufacturers AS m WHERE ManufacturerId = m.Id)
				 )
			   )

SELECT sub.Model
	   , sub.Price
	   , sub.ManufacturerId 
  FROM (
		 SELECT Model
				,Price
				,ManufacturerId
				, Rank() OVER (PARTITION BY ManufacturerId ORDER BY Price DESC ) AS ranks
		   FROM Cameras
       ) AS sub 
 WHERE ranks <= 3 AND Price IS NOT NULL

-- 13. Cameras rise!

DECLARE Manufacturers CURSOR FOR
SELECT m.Id FROM Manufacturers AS m

OPEN Manufacturers 
DECLARE @ManufacturerId INT 

FETCH NEXT FROM Manufacturers INTO @ManufacturerId

WHILE @@FETCH_STATUS = 0
BEGIN
	UPDATE Cameras
	   SET Price += (
                     (
					   SELECT AVG(c.Price) 
					     FROM Cameras AS c
					    WHERE c.ManufacturerId = @ManufacturerId 
					      AND c.Price IS NOT NULL
					 ) 
					 * (LEN((SELECT m.Name FROM Manufacturers AS m WHERE m.Id = @ManufacturerId))) / 100.00
                    )
	 WHERE ManufacturerId = @ManufacturerId 

	 FETCH NEXT FROM Manufacturers INTO @ManufacturerId
END

CLOSE Manufacturers
DEALLOCATE Manufacturers

SELECT sub.Model AS [Model]
	   , sub.Price AS [Price]
	   , sub.ManufacturerId AS [ManufacturerId]
  FROM (
		 SELECT c.Model
				, c.Price 
				, c.ManufacturerId
				, RANK() OVER (PARTITION BY c.ManufacturerId ORDER BY c.Price DESC) AS ranks
		   FROM Cameras AS c
		) AS sub
 WHERE sub.ranks BETWEEN 1 AND 3
   AND sub.Price IS NOT NULL

-- 14. Most cameras for given cash

DECLARE @Table TABLE
(	
	Id INT, 
	ManufacturerId INT, 
	Model NVARCHAR(MAX), 
	Year INT, 
	Price MONEY, 
	Megapixels INT
)

DECLARE @Money MONEY = 54187
DECLARE CamerasCursor CURSOR FOR 
SELECT Id, ManufacturerId, Model, Year, Price, Megapixels 
  FROM Cameras 
 WHERE Price IS NOT NULL 
 ORDER BY Price ASC, Id DESC
OPEN CamerasCursor

DECLARE @Id INT
DECLARE @ManufacturerId INT
DECLARE @Model NVARCHAR(MAX)
DECLARE @Year INT
DECLARE @Price MONEY
DECLARE @Megapixels INT

FETCH NEXT FROM CamerasCursor INTO @Id, @ManufacturerId, @Model, @Year, @Price, @Megapixels

WHILE (@Money >= 0 AND @@FETCH_STATUS = 0)
BEGIN
	IF (@Price > @Money)
		BREAK

	SET @Money = @Money - @Price

	INSERT INTO @Table VALUES (@Id, @ManufacturerId, @Model, @Year, @Price, @Megapixels)
	FETCH NEXT FROM CamerasCursor INTO @Id, @ManufacturerId, @Model, @Year, @Price, @Megapixels
END

CLOSE CamerasCursor
DEALLOCATE CamerasCursor

SELECT * 
  FROM @Table 
 ORDER BY Year DESC, ManufacturerId DESC, Id ASC

-- 15. Stored procedure for creating equipment

CREATE PROCEDURE dbo.usp_CreateEquipment @modelName VARCHAR(max)
AS 
BEGIN
    DECLARE CamerasCur CURSOR FOR 
	SELECT Id, ManufacturerId, Model, Year, Price, Megapixels 
	  FROM Cameras 
	 WHERE Model = @modelName
    OPEN CamerasCur

	DECLARE @Id int
	DECLARE @ManufacturerId int
	DECLARE @Model nvarchar(max)
	DECLARE @Year int
	DECLARE @Price money
	DECLARE @Megapixels int

	DECLARE @MaxManufacturerId INT = 0;
	DECLARE ManCursor CURSOR FOR 
	 SELECT MAX(Id) 
	   FROM Manufacturers
	OPEN ManCursor
	FETCH NEXT FROM ManCursor INTO @MaxManufacturerId

	CLOSE ManCursor
	DEALLOCATE ManCursor

	FETCH NEXT FROM CamerasCur INTO @Id, @ManufacturerId, @Model, @Year, @Price, @Megapixels

	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		DECLARE @CurrentManId INT = @ManufacturerId
		WHILE (@CurrentManId <= @MaxManufacturerId)
		BEGIN
			DECLARE LensesCurCount CURSOR FOR 
			 SELECT COUNT(*) 
			   FROM Lenses 
			  WHERE ManufacturerId = @CurrentManId
			OPEN LensesCurCount

			DECLARE @CNT int = 0
			FETCH NEXT FROM LensesCurCount INTO @CNT
			IF (@CNT <= 0)
			BEGIN
				SET @CurrentManId = @CurrentManId + 1
				CLOSE LensesCurCount
				DEALLOCATE LensesCurCount
				CONTINUE
			END
			CLOSE LensesCurCount
			DEALLOCATE LensesCurCount

			DECLARE LensesCur CURSOR FOR 
			 SELECT Id 
			   FROM Lenses 
			  WHERE ManufacturerId = @CurrentManId
			OPEN LensesCur

			DECLARE @LenseId int = 0
			FETCH NEXT FROM LensesCur INTO @LenseId

			DECLARE @ToExit BIT = 0;
			WHILE (@@FETCH_STATUS = 0)
			BEGIN
				DECLARE @EquipExists int = 0;
				DECLARE EquipExistsCur CURSOR FOR 
				 SELECT COUNT(*) 
				   FROM Equipments 
				  WHERE CameraId = @Id AND LensId = @LenseId
				OPEN EquipExistsCur

				FETCH NEXT FROM EquipExistsCur INTO @EquipExists
				IF (@EquipExists > 0)
				BEGIN
					FETCH NEXT FROM LensesCur INTO @LenseId
					CLOSE EquipExistsCur
					DEALLOCATE EquipExistsCur
					CONTINUE
				END
				CLOSE EquipExistsCur
				DEALLOCATE EquipExistsCur

				INSERT INTO Equipments (CameraId, LensId) VALUES (@Id, @LenseId)

				SET @ToExit = 1
				FETCH NEXT FROM LensesCur INTO @LenseId
			END

			CLOSE LensesCur
			DEALLOCATE LensesCur

			IF (@ToExit > 0)
				BREAK

			SET @CurrentManId = @CurrentManId + 1
		END

		FETCH NEXT FROM CamerasCur INTO @Id, @ManufacturerId, @Model, @Year, @Price, @Megapixels
	END

	CLOSE CamerasCur
	DEALLOCATE CamerasCur
END

EXEC usp_CreateEquipment 'XG-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XG-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XG-1'
EXEC usp_CreateEquipment 'XG-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XH-1'
EXEC usp_CreateEquipment 'XQ3'
EXEC usp_CreateEquipment 'XQ3'
EXEC usp_CreateEquipment 'XQ3'
EXEC usp_CreateEquipment 'XQ3'
EXEC usp_CreateEquipment 'XQ3'
EXEC usp_CreateEquipment 'XQ3'
EXEC usp_CreateEquipment 'XQ3'
EXEC usp_CreateEquipment 'XQ2'
EXEC usp_CreateEquipment 'XQ3'
EXEC usp_CreateEquipment 'XQ2'
EXEC usp_CreateEquipment 'XQ3'
EXEC usp_CreateEquipment 'XQ3'
EXEC usp_CreateEquipment 'XQ3'
EXEC usp_CreateEquipment 'XQ3'
EXEC usp_CreateEquipment 'X30'
EXEC usp_CreateEquipment 'X30'
EXEC usp_CreateEquipment 'X30'
EXEC usp_CreateEquipment 'X30'
EXEC usp_CreateEquipment 'X30'
EXEC usp_CreateEquipment 'X30'
EXEC usp_CreateEquipment 'X30'
EXEC usp_CreateEquipment 'X Vario'
EXEC usp_CreateEquipment 'X Vario'
EXEC usp_CreateEquipment 'NX30'
EXEC usp_CreateEquipment 'Alpha 7'

SELECT * 
  FROM Equipments 
 ORDER BY Id ASC