
-- 9Retrieve names involved in Open and Closed cases.

SELECT V.Name, C.Status
FROM Victim V
JOIN Crime C ON V.CrimeID = C.CrimeID
WHERE C.Status IN ('Open','Closed');

-- 10. Incident types where persons aged 30 or 35 involved.

SELECT DISTINCT C.IncidentType
FROM Crime C
JOIN Victim V ON C.CrimeID = V.CrimeID
WHERE V.Age IN (30,35)

UNION

SELECT DISTINCT C.IncidentType
FROM Crime C
JOIN Suspect S ON C.CrimeID = S.CrimeID
WHERE S.Age IN (30,35);


--  Persons involved in incidents of type 'Robbery'.
SELECT V.Name
FROM Victim V
JOIN Crime C ON V.CrimeID = C.CrimeID
WHERE C.IncidentType = 'Robbery';

-- 12. Incident types with more than one open case.
SELECT IncidentType, COUNT(*) AS Total
FROM Crime
WHERE Status = 'Open'
GROUP BY IncidentType
HAVING COUNT(*) > 1;

-- 13. Incidents where suspect name appears as victim elsewhere.

SELECT C.*
FROM Crime C
JOIN Suspect S ON C.CrimeID = S.CrimeID
WHERE S.Name IN (SELECT Name FROM Victim);

-- 14. Retrieve all incidents with victim and suspect details.
SELECT C.CrimeID,
       C.IncidentType,
       V.Name AS VictimName,
       S.Name AS SuspectName
FROM Crime C
LEFT JOIN Victim V ON C.CrimeID = V.CrimeID
LEFT JOIN Suspect S ON C.CrimeID = S.CrimeID;
