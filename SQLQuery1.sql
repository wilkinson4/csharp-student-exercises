﻿--INSERT INTO Exercise(Name, Language) VALUES ('Asynchronous Functions', 'JavaScript');
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
--INSERT INTO Student(FirstName, LastName, SlackHandle, CohortId) Values('Michael', 'Styles', '@michael', 3);

--CREATE TABLE Exercise (
--    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--    Name VARCHAR(55) NOT NULL,
--    Language VARCHAR(55) NOT NULL
--);


--INSERT INTO Exercise (Name, Language) VALUES ('Functions', 'JavaScript');
--INSERT INTO Exercise (Name, Language) VALUES ('Objects', 'JavaScript');
--INSERT INTO Exercise (Name, Language) VALUES ('Classes', 'C#');
--INSERT INTO Exercise (Name, Language) VALUES ('Interfaces', 'C#');

SELECT s.Id, s.FirstName, s.LastName, s.SlackHandle, s.CohortId, se.ExerciseId, c.Name AS CohortName, e.Name AS ExerciseName, e.Language FROM Student s 
INNER JOIN Cohort c 
ON s.CohortId = c.Id
LEFT JOIN StudentExercise se ON se.StudentId = s.Id
LEFT JOIN Exercise e ON se.ExerciseId = e.Id;

--CREATE TABLE StudentExercise (
--    Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
--    ExerciseId INTEGER NOT NULL,
--    StudentId INTEGER NOT NULL,
--    CONSTRAINT FK_StudentExercise_Exercise FOREIGN KEY(ExerciseId) REFERENCES Exercise(Id),
--    CONSTRAINT FK_StudentExercise_Student FOREIGN KEY(StudentId) REFERENCES Student(Id),
--);

--INSERT INTO StudentExercise (ExerciseId, StudentId) VALUES ('1','2');
--INSERT INTO StudentExercise (ExerciseId, StudentId) VALUES ('2','1');
--INSERT INTO StudentExercise (ExerciseId, StudentId) VALUES ('3','3');

