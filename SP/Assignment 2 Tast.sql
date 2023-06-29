--Assignment 2 query

-- 1) write select query to find name of employee whose salary is highest.

SELECT CONCAT(E.FirstName, ' ', E.LastName) AS fullName, E.Salary AS maxSalary
FROM Employee E
WHERE E.Salary = (SELECT MAX(e.Salary) FROM Employee e); 

-- 2) write select query to find name of employee whose salary is second highest.

-- a)
SELECT CONCAT(E.FirstName, ' ', E.LastName) AS fullName, E.Salary AS secondMaxSalary
FROM Employee E
WHERE E.Salary = (SELECT MAX(e.Salary) AS maxSalary FROM Employee e
					WHERE e.Salary < (SELECT MAX(e2.Salary) FROM Employee e2) 
					); 
			
-- b) used by fetch and offset
SELECT CONCAT(E.FirstName, ' ', E.LastName) AS fullName, E.Salary AS secondMaxSalary
FROM Employee E
ORDER BY E.Salary DESC
OFFSET 2 ROWS FETCH NEXT 1 ROWS ONLY;

-- C) used by rank
SELECT CONCAT(rankedE.FirstName, ' ', rankedE.LastName) AS fullName, rankedE.Salary
FROM (
    SELECT FirstName, LastName, Salary, DENSE_RANK() OVER (ORDER BY Salary DESC) AS salaryRank
    FROM Employee
) AS rankedE
WHERE rankedE.salaryRank = 2;

--DENSE_RANK = number not skipped so not gap between data group if same.
--RANK = number skipp so may be gap beteen data group if same. 
--ROW_NUMBER = it not get all data if same get only one data each of that generate number not group.

-- 3) write select query to find name of employee whose salary is lowest.

-- a) 
-- all min records get
SELECT E.FirstName, E.Salary 
FROM Employee E
WHERE E.Salary = (
					SELECT MIN(e.Salary) AS minSalary FROM Employee e
				 )

-- b) 
-- it get only one record
SELECT TOP 1 E.FirstName, E.Salary
FROM Employee E
ORDER BY E.Salary ASC;

-- 4) write select query to find name of employee whose is from "USA" country
-- a)
SELECT E.FirstName, C.CountryName
FROM Employee E
JOIN Country C ON C.countryID = E.CountryID
WHERE C.CountryName = 'USA' ;

-- b)
SELECT E.FirstName
FROM Employee E
WHERE E.countryID = (SELECT C.CountryID FROM Country C WHERE C.CountryName = 'USA');

-- 5) write select query to find number of employee of all department. It should return two columns:
	--1. EmployeeCount
	--2. Department Name

SELECT COUNT(E.EmployeeID) AS totalEmployee, D.DepartmentName
FROM Employee E
JOIN Department D ON E.DepartmentID = D.DepartmentID
GROUP BY D.DepartmentName;


-- 6) write select query to find name of department who has highest paid salary employee

SELECT TOP 1 D.DepartmentName, E.Salary AS max_salary
FROM Department D
JOIN Employee E ON E.DepartmentID = D.DepartmentID
WHERE E.Salary = (SELECT MAX(e.Salary) FROM Employee e);

-- 7) write select query to find name of employee who is younger most (smallest age employee)
-- a)
SELECT E.FirstName 
FROM Employee E
WHERE E.DateOfBirth = (SELECT MAX(e.DateOfBirth) FROM Employee e);

-- b)
SELECT TOP 1 E.FirstName, E.LastName, DATEDIFF(YEAR, DateOfBirth, GETDATE()) AS Age
FROM Employee E
ORDER BY Age ASC

-- also we have use the datepart but more accurate is date diff for finding age

-- 8) write select query to find name of country for which we don't have any employee.
-- a)
SELECT C.CountryName
FROM Country C
LEFT JOIN Employee E ON E.countryID = C.CountryID
WHERE E.EmployeeID IS NULL;

-- b)
SELECT C.CountryName
FROM Country C 
WHERE C.CountryID NOT IN ( SELECT DISTINCT E.CountryID FROM Employee E); 

-- 9) if DA is 20% of salary then find DA for all employee. It should return three columns:
	-- 1. EmployeeName
	-- 2. Salary
	-- 3. DA

SELECT E.FirstName, E.Salary, E.Salary * 0.2 AS DA
FROM Employee E;

-- 10) List All users with Age (Only Year) Like 25,26,20

-- a)
SELECT pr.FirstNAme, pr.Age
	FROM(
			SELECT E.FirstName, 
			DATEPART(YEAR, GETDATE()) - DATEPART(YEAR, E.DateOfBirth) AS Age
			FROM Employee E
		) AS pr
WHERE Age IN (26)

-- b)
SELECT pr.FirstName, pr.Age
FROM (
		SELECT FirstName, DATEDIFF(YEAR, DateOFBirth, GETDATE()) AS Age
		FROM Employee
	 ) AS pr
WHERE Age in (26);

