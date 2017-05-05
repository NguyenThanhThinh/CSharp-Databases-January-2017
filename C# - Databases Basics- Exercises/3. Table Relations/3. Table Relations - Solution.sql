-- 1. One-To-One

CREATE TABLE Passports
(
	PassportID int NOT NULL PRIMARY KEY,
	PassportNumber varchar(20)
)

CREATE TABLE Persons
(
	PersonID int NOT NULL PRIMARY KEY,
	FirstName varchar(20),
	Salary decimal(18,2),
	PassportID int NOT NULL
)

INSERT INTO Passports (PassportID, PassportNumber)
			VALUES (101,'N34FG21B'), 
				   (102,'K65LO4R7'), 
				   (103,'ZE657QP2')

INSERT INTO Persons (PersonID, FirstName, Salary, PassportID)
			VALUES (1,'Roberto',43300.00,102), 
				   (2,'Tom',56100.00,103), 
				   (3,'Yana',60200.00,101)

ALTER TABLE Persons
ADD CONSTRAINT FK_Persons_Passports FOREIGN KEY(PassportID) 
REFERENCES Passports(PassportID)



-- 2. One-To-Many

CREATE TABLE Manufacturers
(
	ManufacturerID int PRIMARY KEY,
	Name varchar(50) NOT NULL,
	EstablishedOn date NOT NULL
)
 
INSERT INTO Manufacturers(ManufacturerID, Name, EstablishedOn)
				VALUES (1, 'BMW', '1916-03-07'), 
					   (2, 'Tesla', '2003-01-01'), 
					   (3, 'Lada', '1966-05-01')
 
CREATE TABLE Models
(
	ModelID int PRIMARY KEY,
	Name varchar(50) NOT NULL,
	ManufacturerID int NOT NULL,
	CONSTRAINT FK_Models_Manufacturers FOREIGN KEY(ManufacturerID) 
	REFERENCES Manufacturers(ManufacturerID)
)
 
INSERT INTO Models(ModelID, Name, ManufacturerID) 
					VALUES (101, 'X1', 1), (102, 'i6', 1), 
						   (103, 'Model S', 2), (104, 'Model X', 2), 
						   (105, 'Model 3', 2), (106, 'Nova', 3)


-- 3. Many-To-Many

CREATE TABLE Students
(
	StudentID int PRIMARY KEY,
	Name varchar(50) NOT NULL
)

INSERT INTO Students 
		VALUES (1, 'Mila'), 
		   	   (2, 'Toni'), 
			   (3, 'Ron')

CREATE TABLE Exams
(
	ExamID int PRIMARY KEY,
	Name varchar(50) NOT NULL
);

INSERT INTO Exams
		VALUES (101, 'Spring MVC'), 
			   (102, 'Neo4j'), 
			   (103, 'Oracle 11g')

CREATE TABLE StudentsExams
(
	StudentID int NOT NULL,
	ExamID int NOT NULL,
	CONSTRAINT PK_StudentsExams_StudentID_ExamID PRIMARY KEY(StudentID, ExamID),
	CONSTRAINT FK_StudentsExams_StudentID FOREIGN KEY(StudentID) 
	REFERENCES Students(StudentID),
	CONSTRAINT FK_StudentsExams_ExamID FOREIGN KEY(ExamID) 
	REFERENCES Exams(ExamID)
)

INSERT INTO StudentsExams
     VALUES (1, 101), (1, 102),
            (2, 101), (3, 103),
            (2, 102), (2, 103)

-- 04. Self-Referencing

CREATE TABLE Teachers
(
	TeacherID int,
	Name varchar(50),
	ManagerID int,
	CONSTRAINT PK_Teachers PRIMARY KEY(TeacherID),
	CONSTRAINT FK_ManagerID_TeacherID FOREIGN KEY(ManagerID) REFERENCES Teachers(TeacherID)
)

INSERT INTO Teachers (TeacherID, Name, ManagerID)
	 VALUES (101, 'John', NULL), (102, 'Maya', 106),
		    (103, 'Silvia', 106), (104, 'Ted', 105), 
			(105, 'Mark', 101), (106, 'Greta', 101)

-- 05. Online Store Database (I)

CREATE DATABASE OnlineStore
GO

Use OnlineStore
GO

CREATE TABLE Cities
(
	CityID int PRIMARY KEY,
	Name varchar(50) NOT NULL
)

CREATE TABLE Customers
(
	CustomerID int PRIMARY KEY,
	Name varchar(50) NOT NULL,
	Birthday date NOT NULL,
	CityID int FOREIGN KEY 
	REFERENCES Cities(CityID)
)

CREATE TABLE Orders
(
	OrderID int PRIMARY KEY,
	CustomerID int FOREIGN KEY 
	REFERENCES Customers(CustomerID)
)

CREATE TABLE ItemTypes
(
	ItemTypeID int PRIMARY KEY,
	Name varchar(50) NOT NULL
)

CREATE TABLE Items
(
	ItemID int PRIMARY KEY,
	Name varchar(50) NOT NULL,
	ItemTypeID int FOREIGN KEY 
	REFERENCES ItemTypes(ItemTypeID)
)

CREATE TABLE OrderItems
(
	OrderID int,
	ItemID int,
	CONSTRAINT PK_OrdersItems PRIMARY KEY(OrderID, ItemID),
	CONSTRAINT FK_OrdersItems_OrderID FOREIGN KEY(OrderID ) 
	REFERENCES Orders(OrderID),
	CONSTRAINT FK_OrdersItems_ItemID FOREIGN KEY(ItemID) 
	REFERENCES Items(ItemID)
)

-- 05. Online Store Database (II)

CREATE DATABASE OnlineStore
GO

Use OnlineStore
GO

CREATE TABLE Cities
(
	CityID int,
	Name varchar(50) NOT NULL
	CONSTRAINT PK_Cities PRIMARY KEY(CityID)
)

CREATE TABLE Customers
(
	CustomerID int,
	Name varchar(50) NOT NULL,
	Birthday date NOT NULL,
	CityID int,
	CONSTRAINT PK_Customers PRIMARY KEY(CustomerID),
	CONSTRAINT FK_Customers_Cities FOREIGN KEY(CityID) 
	REFERENCES Cities(CityID)
)

CREATE TABLE Orders
(
	OrderID int,
	CustomerID int,
	CONSTRAINT PK_Orders PRIMARY KEY(OrderID),
	CONSTRAINT FK_Orders_Customers FOREIGN KEY(CustomerID) 
	REFERENCES Customers(CustomerID)

)

CREATE TABLE ItemTypes
(
	ItemTypeID int,
	Name varchar(50) NOT NULL,
	CONSTRAINT PK_ItemTypes PRIMARY KEY(ItemTypeID)
)

CREATE TABLE Items
(
	ItemID int,
	Name varchar(50) NOT NULL,
	ItemTypeID int,
	CONSTRAINT PK_Items PRIMARY KEY(ItemID),
	CONSTRAINT FK_Items_ItemTypes FOREIGN KEY(ItemTypeID)
	REFERENCES ItemTypes(ItemTypeID)
)

CREATE TABLE OrderItems
(
	OrderID int,
	ItemID int,
	CONSTRAINT PK_OrdersItems PRIMARY KEY(OrderID, ItemID),
	CONSTRAINT FK_OrdersItems_OrderID FOREIGN KEY(OrderID ) 
	REFERENCES Orders(OrderID),
	CONSTRAINT FK_OrdersItems_ItemID FOREIGN KEY(ItemID) 
	REFERENCES Items(ItemID)
)

-- 06. University Database

CREATE DATABASE University
GO

USE University
GO

CREATE TABLE Majors
(
    MajorID int,
    Name nvarchar(50) NOT NULL,
	CONSTRAINT PK_Majors PRIMARY KEY(MajorID)
)

CREATE TABLE Subjects
(
    SubjectID int,
    SubjectName nvarchar(50) NOT NULL,
	CONSTRAINT PK_Subjects PRIMARY KEY(SubjectID)
)

CREATE TABLE Students
(
	StudentID int,
	StudentNumber int NOT NULL,
	StudentName nvarchar(50) NOT NULL,
	MajorID int,
	CONSTRAINT PK_Students PRIMARY KEY(StudentID),
	CONSTRAINT FK_Students_Majors FOREIGN KEY(MajorID) 
	REFERENCES Majors(MajorID)
)

CREATE TABLE Agenda
(
	StudentID int,
	SubjectID int,
	CONSTRAINT PK_Agenda PRIMARY KEY(StudentID, SubjectID),
	CONSTRAINT FK_Agenda_Students FOREIGN KEY(StudentID)
	REFERENCES Students(StudentID),
	CONSTRAINT FK_Agenda_Subjects FOREIGN KEY(SubjectID)
	REFERENCES Subjects(SubjectID)
)

CREATE TABLE Payments
(
    PaymentID int,
    PaymentDate date NOT NULL,
	PaymentAmount decimal (18,2) NOT NULL,
	StudentID int NOT NULL,
	CONSTRAINT PK_Payments PRIMARY KEY(PaymentID),
	CONSTRAINT FK_Payments_Students FOREIGN KEY(StudentID) 
	REFERENCES Students(StudentID)
)