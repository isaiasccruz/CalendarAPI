IF OBJECT_ID ('dbo.tInterviewerAvailability') IS NOT NULL
	DROP TABLE dbo.tInterviewerAvailability
GO

CREATE TABLE dbo.tInterviewerAvailability
	(
		Id						Int Identity(1,1) NOT NULL,
		InterviewerName         VARCHAR (100) NOT NULL,
		DateStart				DATETIME NOT NULL,
		DateEnd					DATETIME NOT NULL,
	)
GO

CREATE UNIQUE CLUSTERED INDEX IX1_tInterviewerAvailability
	ON dbo.tInterviewerAvailability (Id)
GO

ALTER TABLE tInterviewerAvailability
ADD CONSTRAINT PK_tInterviewerAvailability PRIMARY KEY (Id)
GO

ALTER TABLE tInterviewerAvailability ADD CONSTRAINT [IX_UniqueInterviewerNameAndDates] UNIQUE NONCLUSTERED 
(
    InterviewerName ASC,
	DateStart ASC,
	DateEnd ASC
)
