# Calendar API

## Description
An interview calendar API that manages availability time slots between candidates and interviers in order to provide possible slot times where a interview may occour.

## Considerations
- An interview slot is a 1-hour period of time that spreads from the beginning of any hour until the beginning of the next hour. For example, a time span between 9am and 10am is a valid interview slot, whereas between 9:30am and 10:30am is not.
  
- Each of the interviewers sets their availability slots. For example, the interviewer Mary is available next week each day from 9am through 4pm without breaks and the interviewer Diana is available from 12pm to 6pm on Monday and Wednesday next week, and from 9am to 12pm on Tuesday and Thursday.

- Each of the candidates sets their requested slots for the interview. For example, the candidate John is available for the interview from 9am to 10am any weekday next week and from 10am to 12pm on Wednesday.

- Anyone may then query the API to get a collection of periods of time when it’s possible to arrange an interview for a particular candidate and one or more interviewers. In this example, if the API queries for the candidate John and interviewers Mary and Diana, the response should be a collection of 1-hour slots: from 9am to 10am on Tuesday, from 9am to 10am on Thursday.


# Resolution

So I created a RESTful API using OpenAPI's swagger that manages a calendar for interviewers and candidates and is ready to run in CI/CD environments.
There is an azure-pipelines.yml file for simulating a possible integration of pipelines with AzureDevops.
In the /Config folder, there are yml files with configuration suggestions for the k8s HelmChart for each possible environment.

The data persistence is developed using Dapper's micro ORM extensions, and it was built using an instance of MSSQL\SQLEXPRESS

For the tests, xUnit and Moq was used with the intention of showcasing the testing opportunities and possible usage for Sonar/Sonar Cloud's coverage. 
Not all test paths were implemented. Only examples of success for demonstration purposes.

The application was deployed and tested in a Docker environment on Ubuntu 18.04.

The project was developed in NetCore 3.1 using swagger, dapper on an onion/DDD pattern.