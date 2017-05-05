-- 1. Data Definition

CREATE TABLE [Locations]
(
	Id INT PRIMARY KEY,
	Latitude FLOAT,
	Longitude FLOAT
)

CREATE TABLE [Credentials]
(
	Id INT PRIMARY KEY,
	Email VARCHAR(30),
	Password VARCHAR(20)
)

CREATE TABLE [Users]
(
	Id INT PRIMARY KEY IDENTITY,
	Nickname VARCHAR(25), 
	Gender CHAR(1),
	Age INT,
	LocationId INT,
	CredentialId INT UNIQUE,
	CONSTRAINT FK_Users_Locations FOREIGN KEY(LocationId)
	REFERENCES [Locations](Id),
	CONSTRAINT FK_Users_Credentials FOREIGN KEY(CredentialId)
	REFERENCES [Credentials](Id)
)

CREATE TABLE [Chats]
(
	Id INT PRIMARY KEY,
	Title VARCHAR(32),
	StartDate DATE,
	IsActive BIT
)

CREATE TABLE [Messages]
(
	Id INT PRIMARY KEY,
	Content VARCHAR(200),
	SentOn DATE,
	ChatId INT,
	UserId INT,
	CONSTRAINT FK_Messages_Chats FOREIGN KEY(ChatId)
	REFERENCES [Chats](Id),
	CONSTRAINT FK_Messages_Users FOREIGN KEY(UserId)
	REFERENCES [Users](Id)
)

CREATE TABLE [UsersChats]
(
	UserId INT,
	ChatId INT,
	CONSTRAINT PK_UsersChats PRIMARY KEY(ChatId, UserId),
	CONSTRAINT FK_UsersChats_Users FOREIGN KEY(UserId)
	REFERENCES [Users](Id),
	CONSTRAINT FK_UsersChats_Chats FOREIGN KEY(ChatId)
	REFERENCES [Chats](Id)
)

-- 2. Insert

INSERT INTO [Messages](Content, SentOn, ChatId, UserId)
     SELECT  CONCAT(u.Age, '-', u.Gender, '-', l.Latitude, '-', l.Longitude)
			 , CONVERT(DATE, GETDATE()) 
			 , CASE u.Gender 
					WHEN 'F' THEN CEILING(SQRT(u.Age * 2))
					WHEN 'M' THEN CEILING(POWER(u.Age / 18, 3))
			   END
			 , u.Id
   	   FROM Users AS u
	   JOIN Locations AS l ON u.LocationId = l.Id
	   WHERE u.Id BETWEEN 10 AND 20

-- 3. Update

UPDATE [Chats]
   SET StartDate = EarlistMessage.SentOn
  FROM (
		 SELECT m.ChatId, MIN(m.SentOn) AS SentOn
		   FROM [Chats] AS c
		   JOIN [Messages] AS m ON c.Id = m.ChatId
		  WHERE c.StartDate > m.SentOn
		  GROUP BY c.Id, m.ChatId
		) AS EarlistMessage
 WHERE Chats.StartDate > EarlistMessage.SentOn 
   AND Chats.Id = EarlistMessage.ChatId

-- 3. Update

UPDATE [Chats]
   SET StartDate = ( 
			        SELECT MIN(m.SentOn) 
					  FROM [Chats] AS c 
					  JOIN [Messages] AS m ON c.Id = m.ChatId 
					 WHERE c.Id = Chats.Id 
				   )
 WHERE Chats.Id IN (
					SELECT c.Id 
					  FROM Chats AS c 
					  JOIN [Messages] AS m ON c.Id = m.ChatId 
					 GROUP BY c.Id, c.StartDate 
					HAVING c.StartDate > MIN(m.SentOn)
				   )

-- 3. Update - Zero Test Fails

UPDATE c
   SET c.StartDate = m.SentOn
  FROM [Chats] AS c
  JOIN [Messages] AS m ON c.Id = m.ChatId
 WHERE c.StartDate > m.SentOn

-- 4. Delete

DELETE FROM Locations
 WHERE Id IN (SELECT l.Id 
				FROM Locations AS l 
				LEFT JOIN Users AS u ON l.Id = u.LocationId
			   WHERE u.LocationId IS NULL)

-- 4. Delete

DELETE FROM Locations
 WHERE Id NOT IN (SELECT DISTINCT LocationId FROM Users WHERE LocationId IS NOT NULL)

-- 5. Age Range

SELECT Nickname
	   , Gender
	   , Age
  FROM Users
 WHERE Age BETWEEN 22 AND 37

-- 6. Messages

SELECT Content
	   , SentOn
  FROM [Messages]
 WHERE SentOn > '2014-05-12'
   AND Content LIKE '%just%'
 ORDER BY Id DESC

-- 7. Chats

SELECT Title
	   , IsActive
  FROM Chats
 WHERE (IsActive = 0 AND LEN(Title) < 5)
    OR Title LIKE '__tl%' 
 ORDER BY Title DESC

-- 8. Chat Messages

SELECT c.Id
       , c.Title
	   , m.Id
  FROM [Chats] AS c
  JOIN [Messages] AS m ON c.Id = m.ChatId
 WHERE m.SentOn < '2012-03-26'
   AND c.Title LIKE '%x' 
-- AND RIGHT(c.Title, 1) = 'x'  
 ORDER BY c.Id, m.Id

-- 9. Message Count

SELECT TOP (5) 
	   m.ChatId
	   , COUNT(m.Id) AS TotalMessages 
  FROM [Messages] AS m
 WHERE m.Id < 90
 GROUP BY m.ChatId
 ORDER BY COUNT(m.Id) DESC, m.ChatId

-- 10. Credentials

SELECT u.Nickname
       , c.Email
       , c.Password 
  FROM [Users] AS u
  JOIN [Credentials] AS c ON u.CredentialId = c.Id 
 WHERE c.Email LIKE '%co.uk'
 ORDER BY c.Email

-- 11. Locations

SELECT u.Id
       , u.Nickname
       , u.Age 
  FROM [Users] AS u
  LEFT JOIN [Locations] AS l ON u.LocationId = l.Id 
 WHERE l.Id IS NULL

-- 12. Left Users

SELECT m.Id
	   , m.ChatId
	   , m.UserId 
  FROM [Messages] AS m
 WHERE m.ChatId = 17 
   AND m.UserId NOT IN (
						 SELECT UserId 
						   FROM UsersChats AS uc
						  WHERE uc.ChatId = 17
                       ) 
	OR m.UserId IS NULL
 ORDER BY m.Id DESC

-- 13. Users in Bulgaria

SELECT u.Nickname
       , c.Title
       , l.Latitude
       , l.Longitude
  FROM Users AS u 
  JOIN UsersChats AS uc ON u.Id = uc.UserId
  JOIN Chats AS c ON uc.ChatId = c.Id
  JOIN Locations AS l ON u.LocationId = l.Id
 WHERE CAST(l.Latitude AS NUMERIC(38,18)) BETWEEN 41.14 AND 44.13
   AND CAST(l.Longitude AS NUMERIC(38,18)) BETWEEN 22.21 AND 28.36
 ORDER BY c.Title

-- 14. Last Chat

SELECT c.Title
	   , m.Content 
  FROM [Messages] AS m 
 RIGHT JOIN [Chats] AS c ON m.ChatId = c.Id
 WHERE c.StartDate = (SELECT MAX(StartDate) FROM Chats)

-- 14. Last Chat

SELECT TOP (1) WITH TIES 
       c.Title
       , m.Content
  FROM [Messages] AS m 
 RIGHT JOIN [Chats] AS c ON m.ChatId = c.Id
 ORDER BY c.StartDate DESC

-- 15. Radians

CREATE FUNCTION udf_GetRadians(@Degrees FLOAT)
RETURNS FLOAT
AS 
BEGIN
	DECLARE @Radians FLOAT
		SET @Radians = (@Degrees * PI()) / 180
 	 RETURN @Radians
END

-- 16. Change Password

CREATE PROCEDURE udp_ChangePassword(@Email VARCHAR(30), @NewPassword VARCHAR(20))
AS 
BEGIN
	BEGIN TRANSACTION

   UPDATE [Credentials]
      SET Password = @NewPassword
    WHERE Email = @Email

	IF @Email NOT IN (SELECT Email FROM [Credentials])
	BEGIN
		ROLLBACK
		RAISERROR('The email does''t exist!', 16, 1)
		RETURN
	END
    COMMIT
END

-- 17. Send Messages

CREATE PROCEDURE udp_SendMessage(@UserId INT, @ChatId INT, @Content VARCHAR(MAX))
AS
BEGIN
	BEGIN TRANSACTION

	SET IDENTITY_INSERT [Messages] ON
	-- or the same solution without specifying the NextId to insert

	DECLARE @NextId INT = ISNULL((SELECT MAX(Id) FROM [Messages]), 0) + 1
			 	 
    INSERT INTO [Messages](Id, Content, SentOn, ChatId, UserId)
	     VALUES (
					@NextId
				  , @Content
				  , CONVERT(DATE, GETDATE()) 
				  , @ChatId
				  , @UserId
				)
	
	IF @UserId NOT IN (SELECT UserId FROM [UsersChats] WHERE ChatId = @ChatId)
    BEGIN
		ROLLBACK
	    RAISERROR('There is no chat with that user!', 16, 1)
		RETURN
	END
	COMMIT
END

-- 18. Log Messages

CREATE TRIGGER tr_DeletedMessages
ON [Messages]
AFTER DELETE
AS
BEGIN

INSERT INTO MessageLogs(Id, Content, SentOn, ChatId, UserId)
	 SELECT d.Id, d.Content, d.SentOn, d.ChatId, d.UserId FROM DELETED AS d

END

-- 19. Delete Users

CREATE TRIGGER tr_DeleteUserRelations
ON Users
INSTEAD OF DELETE
AS
BEGIN
    DECLARE @deletedUserId int = (SELECT d.Id FROM DELETED AS d)
    DECLARE @deletedUserCredentialId INT = (SELECT d.CredentialId FROM DELETED AS d)
  
    DELETE FROM [Credentials]
     WHERE Id = @deletedUserCredentialId

    DELETE FROM [UsersChats]
     WHERE UserId = @deletedUserId
 
    DELETE FROM [Messages]
     WHERE UserId = @deletedUserId
 
    DELETE FROM [Users]
     WHERE Id = @deletedUserId
END