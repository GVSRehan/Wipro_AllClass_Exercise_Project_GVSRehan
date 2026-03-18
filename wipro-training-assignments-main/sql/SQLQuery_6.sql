USE CrimeDB;
GO

-- Display Tables
SELECT * FROM Crime;
SELECT * FROM Victim;
SELECT * FROM Suspect;

-- 1. Selecting all open incidents.
SELECT * 
FROM Crime
WHERE Status = 'Open';
-- 2. Finding total number of incidents.

SELECT COUNT(*) AS TotalIncidents
FROM Crime;


-- 3. Listing all unique incident types.
SELECT DISTINCT IncidentType
FROM Crime;
-- 4. Retrieve incidents between 2023-09-01 and 2023-09-10.
SELECT *
FROM Crime
WHERE IncidentDate BETWEEN '2023-09-01' AND '2023-09-10';



-- 5. List persons involved in descending order of age.
SELECT Name, Age
FROM (
    SELECT Name, Age FROM Victim
    UNION
    SELECT Name, Age FROM Suspect
) AS Persons
ORDER BY Age DESC;


-- 6. Find average age of persons involved.
SELECT AVG(Age) AS AverageAge
FROM (
    SELECT Age FROM Victim
    UNION ALL
    SELECT Age FROM Suspect
) AS Persons;


-- 7. List incident types and their counts (Open cases only).
SELECT IncidentType, COUNT(*) AS TotalOpenCases
FROM Crime
WHERE Status = 'Open'
GROUP BY IncidentType;


-- 8. Find persons with names containing 'Doe'.
SELECT Name FROM Victim
WHERE Name LIKE '%Doe%'
UNION
SELECT Name FROM Suspect
WHERE Name LIKE '%Doe%';

