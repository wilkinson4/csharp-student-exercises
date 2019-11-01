--INSERT INTO Exercise(Name, Language) VALUES ('Asynchronous Functions', 'JavaScript');
--INSERT INTO Exercise(Name, Language) VALUES ('Atoms', 'Elixir');
--INSERT INTO Exercise(Name, Language) VALUES ('Tuples', 'Elixir');

--CREATE TABLE Cohort (
--    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--    Name VARCHAR(55) NOT NULL,
--);

--CREATE TABLE Instructor (
--    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--    FirstName VARCHAR(55) NOT NULL,
--    LastName VARCHAR(55) NOT NULL,
--	SlackHandle VARCHAR(55) NOT NULL,
--    CohortId INTEGER NOT NULL,
--    CONSTRAINT FK_Instructor_Cohort FOREIGN KEY(CohortId) REFERENCES Cohort(Id),
--);

--INSERT INTO Cohort (Name) VALUES('C34');
--INSERT INTO Cohort (Name) VALUES('C36');
--INSERT INTO Cohort (Name) VALUES('E9');

--INSERT INTO Instructor (FirstName, LastName, SlackHandle, CohortId) Values('Andy', 'Collins', '@andy', 1);
--INSERT INTO Instructor (FirstName, LastName, SlackHandle, CohortId) Values('Steve', 'Brownlee', '@steve', 2);
--INSERT INTO Instructor (FirstName, LastName, SlackHandle, CohortId) Values('Jenna', 'Solis', '@Jenna', 3);

--CREATE TABLE Student (
--    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--    FirstName VARCHAR(55) NOT NULL,
--    LastName VARCHAR(55) NOT NULL,
--	SlackHandle VARCHAR(55) NOT NULL,
--    CohortId INTEGER NOT NULL,
--    CONSTRAINT FK_Student_Cohort FOREIGN KEY(CohortId) REFERENCES Cohort(Id),
--);

--INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId) Values('Bobby', 'Brady', '@bobby', 1);
--INSERT INTO Student(FirstName, LastName, SlackHandle, CohortId) Values('Noah', 'Bartfield', '@noah', 2);
--INSERT INTO Student(FirstName, LastName, SlackHandle, CohortId) Values('Kelly', 'Coles', '@kelly', 3);
--INSERT INTO Student(FirstName, LastName, SlackHandle, CohortId) Values('Sarah', 'Flemming', '@sarah', 1);
--INSERT INTO Student(FirstName, LastName, SlackHandle, CohortId) Values('Matthew', 'Ross', '@matthew', 2);

--CREATE TABLE Exercise (
--    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--    Name VARCHAR(55) NOT NULL,
--    Language VARCHAR(55) NOT NULL
--);


--INSERT INTO Exercise (Name, Language) VALUES ('Functions', 'JavaScript');
--INSERT INTO Exercise (Name, Language) VALUES ('Objects', 'JavaScript');
--INSERT INTO Exercise (Name, Language) VALUES ('Classes', 'C#');
--INSERT INTO Exercise (Name, Language) VALUES ('Interfaces', 'C#');

--SELECT s.Id, s.FirstName, s.LastName, s.SlackHandle, s.CohortId, se.ExerciseId, c.Name AS CohortName, e.Name AS ExerciseName, e.Language FROM Student s 
--INNER JOIN Cohort c 
--ON s.CohortId = c.Id
--LEFT JOIN StudentExercise se ON se.StudentId = s.Id
--LEFT JOIN Exercise e ON se.ExerciseId = e.Id;

--CREATE TABLE StudentExercise (
--    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--    ExerciseId INTEGER NOT NULL,
--    StudentId INTEGER NOT NULL,
--    CONSTRAINT FK_StudentExercise_Exercise FOREIGN KEY(ExerciseId) REFERENCES Exercise(Id),
--    CONSTRAINT FK_StudentExercise_Student FOREIGN KEY(StudentId) REFERENCES Student(Id),
--);

--INSERT INTO StudentExercise (ExerciseId, StudentId) VALUES ('4','2');
--INSERT INTO StudentExercise (ExerciseId, StudentId) VALUES ('5','1');
--INSERT INTO StudentExercise (ExerciseId, StudentId) VALUES ('6','3');


--SELECT s.Id AS TheStudentId, s.FirstName AS StudentFirst, s.LastName AS StudentLast, s.SlackHandle AS StudentSlack, 
--c.Id AS CohortId, c.Name AS CohortName, 
--i.Id AS InstructorId, i.FirstName AS InstructorFirst, i.LastName AS InstructorLast, i.SlackHandle AS InstructorSlack,
--se.ExerciseId, se.StudentId,
--e.Name AS ExerciseName, e.Language
--FROM Cohort c 
--INNER JOIN Student s 
--ON s.CohortId = c.Id
--INNER Join Instructor i
--ON i.CohortId = c.Id
--LEFT JOIN StudentExercise se ON se.StudentId = s.Id
--LEFT JOIN Exercise e ON se.ExerciseId = e.Id;

--SELECT s.Id, s.FirstName, s.LastName, s.SlackHandle, s.CohortId, c.Name AS CohortName 
--                                        FROM Student s 
--                                        INNER JOIN Cohort c 
--                                        ON s.CohortId = c.Id

--SELECT s.Id AS TheStudentId, s.FirstName AS StudentFirst, s.LastName AS StudentLast, s.SlackHandle AS StudentSlack, 
--                                        c.Id AS CohortId, c.Name AS CohortName, 
--                                        i.Id AS InstructorId, i.FirstName AS InstructorFirst, i.LastName AS InstructorLast, i.SlackHandle AS InstructorSlack,
--                                        se.ExerciseId, se.StudentId,
--                                        e.Name AS ExerciseName, e.Language
--                                        FROM Cohort c 
--                                        INNER JOIN Student s 
--                                        ON s.CohortId = c.Id
--                                        INNER Join Instructor i
--                                        ON i.CohortId = c.Id
--                                        LEFT JOIN StudentExercise se ON se.StudentId = s.Id
--                                        LEFT JOIN Exercise e ON se.ExerciseId = e.Id;

SELECT e.Id AS TheExerciseId, e.Name, e.Language, s.Id AS StudentId, s.FirstName, s.LastName, s.SlackHandle 
FROM Exercise e
INNER JOIN StudentExercise se
ON e.Id = se.ExerciseId
INNER JOIN Student s
ON s.Id = se.StudentId