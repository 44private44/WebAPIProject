
CREATE DATABASE  Assignment1;

USE Assignment1;
--Employee
CREATE TABLE Employee (
    EmployeeID INT NOT NULL IDENTITY(1,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50),
    Gender CHAR(1) NOT NULL CHECK (Gender IN ('M', 'F')),
    DateOfBirth DATETIME,
    DepartmentID INT DEFAULT 1,
    Salary DECIMAL(18, 2) NOT NULL,
    [Address] VARCHAR(200),
    CityID INT,
    PinCode CHAR(6),	
    IsActive BIT,
    
);

ALTER TABLE Employee 
ADD CONSTRAINT PK_Emp PRIMARY KEY (EmployeeID);

ALTER TABLE Employee
ADD CONSTRAINT FK_Emp_City FOREIGN KEY (CityID) REFERENCES City(CityID);

ALTER TABLE Employee
ADD CONSTRAINT FK_Emp_Country FOREIGN KEY (CountryID) REFERENCES Country(countryID);

INSERT INTO Employee(FirstName, LastName, Gender, DateOfBirth, DepartmentID, Salary, [Address], CityID, PinCode, IsActive) 
VALUES ('Soham', 'Modi', 'M', '2001-09-30', 1, 80000, 'Govinda', 1, '382845', 1),
		('Kenvi', 'Patel', 'F', '1997-06-07', 2, 180000, 'Gokul', 2, '382845', 1),
		('Rushi', 'Singh', 'M', '2004-08-11', 3, 280000, 'madhavpark', 4, '382845', 0);


--Department

CREATE TABLE Department(
	DepartmentID INT NOT NULL IDENTITY(1, 1),
	DepartmentName VARCHAR(50)
);

ALTER TABLE Department 
ADD CONSTRAINT PK_depid PRIMARY KEY (DepartmentID);

INSERT INTO Department(DepartmentName)
VALUES ('IT'), ('Networking'), ('HR'), ('Testing'), ('Developers');

--City

CREATE TABLE City(
	CityID INT NOT NULL IDENTITY(1,1),
	CityName varchar(50) NOT NULL,
	StatusID INT 
);

ALTER TABLE City
ADD CONSTRAINT PK_Cityid PRIMARY KEY (cityID);

ALTER TABLE City
ADD CONSTRAINT FK_City_State FOREIGN KEY (StatusID) REFERENCES State(StateID);

INSERT INTO City(CityName, StatusID)
VALUES('Gandhinagar', 1), ('ahmedabad', 1) , ('vellor' , 3), ('Mansa' , 2);

--State

CREATE TABLE [State](
	StateID INT NOT NULL IDENTITY(1,1),
	StateName VARCHAR(50) NOT NULL,
	CountryID INT 
);

ALTER TABLE [State]
ADD CONSTRAINT PK_StateID PRIMARY KEY (StateID);

ALTER TABLE [State]
ADD CONSTRAINT FK_State_Country FOREIGN KEY (CountryID) REFERENCES Country(CountryID);

INSERT INTO [State](StateName, CountryID)
VALUES ('Gujarat', 1), ('velly', 2),('chennai', 1), ('vosicton', 3)

--Country

CREATE TABLE Country(
	CountryID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	CountryName VARCHAR(50) NOT NULL
);


INSERT INTO Country(CountryName)
VALUES ('India'), ('Canada'), ('USA'), ('UK'), ('Australlia')

SELECT * FROM City;
SELECT * FROM Country;
SELECT * FROM [STATE];
SELECT * FROM Employee;
SELECT * FROM Department;

DROP TABLE City;
DROP TABLE Country;
DROP TABLE [STATE];
DROP TABLE Employee;

--Assignment 1 query

-- 1) List All User

SELECT * FROM Employee;

-- 2) List All Country

SELECT * FROM Country;

-- 3) List All State

SELECT * FROM [STATE];

-- 4) List All State Of 5 different countries

SELECT S.StateID, S.StateName, C.CountryName
FROM State S
JOIN Country C ON S.CountryID = C.CountryID
WHERE S.CountryID IN (
    SELECT DISTINCT TOP 5 CountryID
    FROM [State]
)
ORDER BY C.CountryID;

-- 5) List All Department
SELECT * FROM Department;

-- 6) List All Cities
SELECT * FROM City;

-- 7) List User Who have no address specified

SELECT *
FROM Employee
WHERE [Address] IS NULL OR [Address] = '';

-- 8) List User with of 5 different departments

SELECT e.*, d.DepartmentName
FROM Employee e 
JOIN Department d ON d.DepartmentID = e.DepartmentID
WHERE e.DepartmentID IN(
	SELECT DISTINCT TOP 5 DepartmentID
	FROM Employee 
)	
ORDER BY e.DepartmentID;

-- 9) List All Male Users

SELECT * FROM Employee
WHERE Gender = 'M';

-- 10) LList All Female Users

SELECT * FROM Employee
WHERE Gender = 'F';

-- 11) List Users Whoes Last name is null

SELECT * FROM Employee
WHERE (LastNAme IS NULL OR LastName = '');

-- 12)	List Users Whoes Last name is not null

SELECT * FROM Employee
WHERE LastName IS NOT NULL;

-- 13)	List All Users with state,country,department,address Of 5 different department

SELECT E.EmployeeID, E.FirstName, E.LastName, D.DepartmentName, E.Address, C.CityName
FROM Employee E
JOIN Department D ON E.DepartmentID = D.DepartmentID
JOIN City C ON C.CityID = E.CityID
WHERE E.DepartmentID IN (
    SELECT DISTINCT TOP 5 DepartmentID
    FROM Employee
)
ORDER BY E.DepartmentID;


-- 16) List Users with all fields who is born after year 2000

SELECT *
FROM Employee
WHERE YEAR(DateOfBirth) > 2000;


-- 17) List Users with all fields who is born before year 2000

SELECT *
FROM Employee
WHERE YEAR(DateOfBirth) < 2000;

-- 18) List users whose birthday is 01-Jan-1990

SELECT *
FROM Employee
WHERE CAST(DateOfBirth AS DATE) = '1997-06-07';

-- 19) List users whose birthday is between 01-Jan-1990 and 01-Jan-1995

SELECT *
FROM Employee
WHERE CAST(DateOfBirth AS DATE) BETWEEN '1999-06-07' AND '2004-01-01';

-- 20)    List All Users Order by Date of Birth

SELECT * FROM Employee
ORDER BY DateOfBirth DESC;

-- 21) List All Active and inactive users.

SELECT * FROM Employee
WHERE IsActive = 1 OR IsActive = 0;

-- 22) List all users whose First Name Start with  A

SELECT * FROM Employee
WHERE FirstName LIKE 'K%';

-- 23) List All Users whose email address contains domain @server1.com
--SELECT *
--FROM Employee
--WHERE EmailAddress LIKE '%@server1.com';

-- 24) List all users to whose department is not assigned

SELECT *
FROM Employee E
LEFT JOIN Department D ON E.DepartmentID = D.DepartmentID
WHERE D.DepartmentID IS NULL;

-- 25) List all female users from state gujarat

SELECT E.*
FROM Employee E
JOIN City C ON E.CityID = C.CityID
WHERE E.Gender = 'F' AND C.CityName = 'Ahmedabad';
--JOIN State S ON C.StateID = S.StateID
--WHERE E.Gender = 'F' AND S.StateName = 'Gujarat';

-- 26) List all male users from City Mumbai

SELECT E.*
FROM Employee E
JOIN City C ON E.CityID = C.CityID
WHERE E.Gender = 'M' AND C.CityName = 'Mansa';

-- 27) List User's all detail who belongs to account department

SELECT E.*
FROM Employee E
JOIN Department D ON E.DepartmentID = D.DepartmentID
WHERE D.DepartmentName = 'IT';

-- 28) List User's all detail whoes zip code is 380015
SELECT *
FROM Employee
WHERE PinCode = '382845';

