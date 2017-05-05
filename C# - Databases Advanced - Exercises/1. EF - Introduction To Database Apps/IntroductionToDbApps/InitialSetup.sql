CREATE TABLE Towns 
(
	Id INT PRIMARY KEY IDENTITY, 
	Name VARCHAR(50), 
	Country VARCHAR(50)
)
CREATE TABLE Minions 
(
	Id INT PRIMARY KEY IDENTITY, 
	Name VARCHAR(50), 
	Age INT, 
	TownId INT,
	CONSTRAINT FK_Minions_Towns FOREIGN KEY (TownId) REFERENCES Towns(Id)
)
 
CREATE TABLE Villains 
(
	Id INT PRIMARY KEY IDENTITY, 
	Name VARCHAR(50), 
	EvilnessFactor VARCHAR(20)
)
CREATE TABLE MinionsVillains
(
	MinionId INT,
	VillainId INT,
	CONSTRAINT FK_MinionsVillains_Minions FOREIGN KEY (MinionId) REFERENCES Minions(Id),
	CONSTRAINT FK_MinionsVillains_Villains FOREIGN KEY (VillainId) REFERENCES Villains(Id)
)

INSERT INTO Towns (Name, Country) 
     VALUES ('Sofia','Bulgaria'), ('Burgas','Bulgaria'), ('Varna', 'Bulgaria'), ('London','UK'),
			('Liverpool','UK'),('Ocean City','USA'),('Paris','France')

INSERT INTO Minions (Name, Age, TownId) 
     VALUES ('bob',10,1),('kevin',12,2),('steward',9,3),('rob',22,3), ('michael',5,2),('pep',3,2)

INSERT INTO Villains (Name, EvilnessFactor) 
	 VALUES ('Gru','super evil'),('Victor','evil'), ('Simon Cat','good'),('Pusheen','super good'),('Mammal','evil')
	 
INSERT INTO MinionsVillains (MinionId, VillainId)
	 VALUES (1,2), (3,1),(1,3),(3,3),(4,1),(2,2),(1,1),(3,4),(1, 4), (1,5), (5, 1), (4,1), (3, 1)