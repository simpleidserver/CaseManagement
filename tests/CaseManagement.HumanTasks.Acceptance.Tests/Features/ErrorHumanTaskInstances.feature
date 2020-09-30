Feature: ErrorHumanTaskInstances
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