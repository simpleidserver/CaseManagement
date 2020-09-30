﻿Feature: ErrorHumanTaskInstances
	Check errors returned by /humantaskinstances
	
Scenario: Check error is returned when trying to create task with bad TaskName
	When execute HTTP POST JSON request 'http://localhost/humantaskinstances'
	| Key           | Value       |
	| humanTaskName | invalidname |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'invalidname''

Scenario: Check error is returned when trying to create task and authenticated user is not a task initiato
    When execute HTTP POST JSON request 'http://localhost/humantaskinstances'
	| Key           | Value     |
	| humanTaskName | addClient |
	And extract JSON from body
	
	Then HTTP status code equals to '401'
	Then JSON 'status'='401'
	Then JSON 'errors.bad_request[0]'='User is not authorized'

Scenario: Check error is returned when trying to create task and parameters are invalid
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances'
	| Key                 | Value                       |
	| humanTaskName       | addClient                   |
	| operationParameters | { "isGoldenClient": "bad" } |
	And extract JSON from body
	
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'firstName' is missing'
	Then JSON 'errors.bad_request[1]'='Parameter 'isGoldenClient' is not a valid 'BOOL''

Scenario: Check error is returned when trying to get invalid humantask instance details
	When execute HTTP GET request 'http://localhost/humantaskinstances/invalid/details'
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task instance 'invalid''

Scenario: Check error is returned when trying to get invalid humantask instance history
	When execute HTTP POST JSON request 'http://localhost/humantaskinstances/invalid/history'
	| Key | Value |
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task instance 'invalid''

Scenario: Check error is returned when trying to nominate and GroupNames, UserIdentifiers parameters are specified
	When execute HTTP POST JSON request 'http://localhost/humantaskinstances/invalid/nominate'
	| Key             | Value     |
	| groupNames      | ["group"] |
	| userIdentifiers | ["user"]  |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='GroupNames and UserIdentifiers parameters cannot be specified at the same time'

Scenario: Check error is returned when trying to nominate and GroupNames, UserIdentifiers parameters are not specified
	When execute HTTP POST JSON request 'http://localhost/humantaskinstances/invalid/nominate'
	| Key             | Value     |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameters 'GroupNames,UserIdentifiers' are missing'
	
Scenario: Check error is returned when trying to nominate an unknown human task instance
	When execute HTTP POST JSON request 'http://localhost/humantaskinstances/invalid/nominate'
	| Key        | Value     |
	| groupNames | ["group"] |
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task instance 'invalid''
	
Scenario: Check error is returned when trying to nominate and user is not authorized
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances'
	| Key           | Value            |
	| humanTaskName | noPotentialOwner |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'
	When execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/nominate'
	| Key        | Value     |
	| groupNames | ["group"] |
	And extract JSON from body
	
	Then HTTP status code equals to '401'
	Then JSON 'status'='401'
	Then JSON 'errors.bad_request[0]'='User is not authorized'

Scenario: Check error is returned when trying to get description of an unknown human task instance
	When execute HTTP GET request 'http://localhost/humantaskinstances/invalid/description'
	And extract JSON from body
	
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task instance 'invalid''