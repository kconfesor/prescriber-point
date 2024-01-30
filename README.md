# Prescriber Point

## User Story - PP-101

### Summary
Prescriber Point requires a journal for Doctors to write down random 
notes during patients diagnosis. Doctors should be able to write a note containing
the patient's name and a note. The dates of creation/modification of the record should
be added automatically.

### Acceptance Criteria

* User should be able to write the note and patient name.
* User should be able to retrieve a list of notes
* User should not be able to see journal from another user
* User can modify and/or delete any record

### Technical Requirements
* The application should be a web api
* The application should be built using dotnet stack
* The application should be built using a database
* The application must have a docker file

### Running the application

* Clone the repository
* Navigate to the root of the repository
* Run the following command to start the application
```docker-compose up --build```
* Navigate to the following url to access the application
```http://localhost:8080```

### Using the api endpoints
* Create a user on /security/singUp
* Login on /security/signin with the credentials used to sign up
* Use the token returned to access the other endpoints using swagger Authenticate