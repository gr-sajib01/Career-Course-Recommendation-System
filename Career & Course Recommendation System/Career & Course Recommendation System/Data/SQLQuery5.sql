SELECT 
    u.UserId,
    u.FullName,
    u.Email,
    s.StudentId,
    s.StudentIdNumber,
    s.Department,
    s.CGPA,
    s.MathematicsScore,
    s.ProgrammingScore,
    s.StatisticsScore
FROM Users u
LEFT JOIN Students s
    ON u.UserId = s.UserId;