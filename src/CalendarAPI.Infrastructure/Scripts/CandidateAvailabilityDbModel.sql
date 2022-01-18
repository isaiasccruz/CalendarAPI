IF OBJECT_ID ('dbo.tCandidateAvailability') IS NOT NULL
	DROP TABLE dbo.tCandidateAvailability
GO

CREATE TABLE dbo.tCandidateAvailability
	(
		Id						Int Identity(1,1) NOT NULL,
		CandidateName           VARCHAR (100) NOT NULL,
		DateStart				DATETIME NOT NULL,
		DateEnd					DATETIME NOT NULL,
	)
GO

CREATE UNIQUE CLUSTERED INDEX IX1_tCandidateAvailability
	ON dbo.tCandidateAvailability (Id)
GO

ALTER TABLE tCandidateAvailability
ADD CONSTRAINT PK_tCandidateAvailability PRIMARY KEY (Id)
GO

ALTER TABLE tCandidateAvailability ADD CONSTRAINT [IX_UniqueCandidateNameAndDates] UNIQUE NONCLUSTERED 
(
    CandidateName ASC,
	DateStart ASC,
	DateEnd ASC
)