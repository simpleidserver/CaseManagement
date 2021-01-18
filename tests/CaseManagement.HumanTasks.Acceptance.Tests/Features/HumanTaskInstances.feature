Feature: HumanTaskInstances
	Check /humantaskinstances
	
Scenario: Create human task instance
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/me'
	| Key                 | Value                                                  |
	| humanTaskName       | addClient                                              |
	| operationParameters | { "isGoldenClient": "true", "firstName": "FirstName" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/details'
	And extract JSON from body into 'detailsHumanTaskInstance'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/history'
	| Key | Value |
	And extract JSON from body into 'historyHumanTaskInstance'

	Then HTTP status code equals to '200'
	Then extract JSON 'detailsHumanTaskInstance', JSON exists 'id'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'name'='addClient'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'status'='RESERVED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].userPrincipal'='taskInitiator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].eventType'='CREATED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].endOwner'='administrator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].taskStatus'='RESERVED'

Scenario: Create deferred human task instance (activated after 5 seconds)
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And add '5' seconds into 'activationDeferralTime'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/me'
	| Key                    | Value                                                  |
	| humanTaskName          | addClient                                              |
	| operationParameters    | { "isGoldenClient": "true", "firstName": "FirstName" } |
	| activationDeferralTime | $activationDeferralTime$                               |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'	
	And poll 'http://localhost/humantaskinstances/$humanTaskInstanceId$/details', until 'status'='RESERVED'
	And extract JSON from body into 'detailsHumanTaskInstance'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/history'
	| Key | Value |
	And extract JSON from body into 'historyHumanTaskInstance'

	Then HTTP status code equals to '200'
	Then extract JSON 'detailsHumanTaskInstance', JSON exists 'id'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'name'='addClient'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'status'='RESERVED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].userPrincipal'='taskInitiator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].eventType'='CREATED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].taskStatus'='CREATED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].userPrincipal'='ProcessActivationTimer'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].eventType'='ACTIVATE'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].endOwner'='administrator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].taskStatus'='RESERVED'

Scenario: Nominate a human task
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/me'
	| Key           | Value            |
	| humanTaskName | noPotentialOwner |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'
	And authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | businessAdmin |	
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/nominate'
	| Key             | Value              |
	| userIdentifiers | ["potentialOwner"] |
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/details'
	And extract JSON from body into 'detailsHumanTaskInstance'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/history'
	| Key | Value |
	And extract JSON from body into 'historyHumanTaskInstance'
	
	Then HTTP status code equals to '200'
	Then extract JSON 'detailsHumanTaskInstance', JSON exists 'id'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'name'='noPotentialOwner'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'status'='RESERVED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].userPrincipal'='taskInitiator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].eventType'='CREATED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].taskStatus'='CREATED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].userPrincipal'='businessAdmin'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].eventType'='ACTIVATE'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].endOwner'='potentialOwner'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].taskStatus'='RESERVED'

Scenario: Get humantask instance description
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/me'
	| Key                 | Value                                                  |
	| humanTaskName       | addClient                                              |
	| operationParameters | { "isGoldenClient": "true", "firstName": "FirstName" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/description'
	| Key             | Value |
	| Accept-Language | en-US |
	
	Then HTTP status code equals to '200'
	Then html = '<b>firstname is FirstName, isGoldenClient true</b>'

Scenario: Claim a human task instance
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/me'
	| Key           | Value                   |
	| humanTaskName | multiplePotentialOwners |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'
	And authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | administrator |
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/claim'
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/details'
	And extract JSON from body into 'detailsHumanTaskInstance'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/history'
	| Key | Value |
	And extract JSON from body into 'historyHumanTaskInstance'

	Then HTTP status code equals to '200'
	Then extract JSON 'detailsHumanTaskInstance', JSON exists 'id'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'name'='multiplePotentialOwners'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'status'='RESERVED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].userPrincipal'='taskInitiator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].eventType'='CREATED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].taskStatus'='READY'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].userPrincipal'='administrator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].eventType'='CLAIM'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].endOwner'='administrator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].taskStatus'='RESERVED'

Scenario: Start a human task instance
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/me'
	| Key           | Value                   |
	| humanTaskName | multiplePotentialOwners |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'
	And authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | administrator |	
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/start'
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/details'
	And extract JSON from body into 'detailsHumanTaskInstance'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/history'
	| Key | Value |
	And extract JSON from body into 'historyHumanTaskInstance'

	Then HTTP status code equals to '200'
	Then extract JSON 'detailsHumanTaskInstance', JSON exists 'id'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'name'='multiplePotentialOwners'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'status'='INPROGRESS'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].userPrincipal'='taskInitiator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].eventType'='CREATED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].taskStatus'='READY'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].userPrincipal'='administrator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].eventType'='START'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].endOwner'='administrator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].taskStatus'='INPROGRESS'

Scenario: Complete a human task instance
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
    And execute HTTP POST JSON request 'http://localhost/humantaskinstances/me'
	| Key                 | Value                                                  |
	| Key                 | Value                                                  |
	| humanTaskName       | addClient                                              |
	| operationParameters | { "firstName": "firstname", "isGoldenClient": "true" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'
	And authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | administrator |	
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/start'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/complete'
	| Key                 | Value           |
	| operationParameters | { "wage": "2" } |
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/details'
	And extract JSON from body into 'detailsHumanTaskInstance'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/history'
	| Key | Value |
	And extract JSON from body into 'historyHumanTaskInstance'
	
	Then HTTP status code equals to '200'
	Then extract JSON 'detailsHumanTaskInstance', JSON exists 'id'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'name'='addClient'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'status'='COMPLETED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].userPrincipal'='taskInitiator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].eventType'='CREATED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[0].taskStatus'='RESERVED'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].userPrincipal'='administrator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].eventType'='START'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].endOwner'='administrator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[1].taskStatus'='INPROGRESS'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[2].userPrincipal'='administrator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[2].eventType'='COMPLETE'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[2].endOwner'='administrator'
	Then extract JSON 'historyHumanTaskInstance', JSON 'content[2].taskStatus'='COMPLETED'

Scenario: Check a notification is created by StartDeadLine
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/me'
	| Key                 | Value                        |
	| humanTaskName       | startDeadLine                |
	| operationParameters | { "firstName": "FirstName" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'
	And authenticate
	| Key                                                                  | Value |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | guest |
	And poll HTTP POST JSON request 'http://localhost/notificationinstances/.search', until 'totalLength'='1'
	| Key | Value |
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'totalLength'='1'
	Then JSON 'content[0].name'='notification'
	Then JSON 'content[0].status'='READY'

Scenario: Check a notification is created by CompletionDeadLine
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/me'
	| Key                 | Value                        |
	| humanTaskName       | completionDeadLine           |
	| operationParameters | { "firstName": "FirstName" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'
	And authenticate
	| Key                                                                  | Value |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | guest |
	And poll HTTP POST JSON request 'http://localhost/notificationinstances/.search', until '$.content[?(@.name == 'notificationCompletion')].name'='notificationCompletion'
	| Key | Value |
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON '$.content[?(@.name == 'notificationCompletion')].name'='notificationCompletion'
	Then JSON '$.content[?(@.name == 'notificationCompletion')].status'='READY'

Scenario: Execute a composite task
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/me'
	| Key                 | Value                                                  |
	| humanTaskName       | compositeTask                                          |
	| operationParameters | { "isGoldenClient": "true", "firstName": "FirstName" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'
	And authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | administrator |	
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/start'	
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/subtasks'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'content[0].name'='addClient'

Scenario: Check the result of an automatic completion behavior
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/me'
	| Key                 | Value                                                  |
	| humanTaskName       | compositeTaskWithAutomaticCompletionBehavior           |
	| operationParameters | { "isGoldenClient": "true", "firstName": "FirstName" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'
	And authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | administrator |	
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/start'	
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/subtasks'	
	And extract JSON from body	
	And extract '$.content[?(@.name == 'addClient')].id' from JSON body into 'subHumanTaskInstanceId'
	And execute HTTP GET request 'http://localhost/humantaskinstances/$subHumanTaskInstanceId$/start'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$subHumanTaskInstanceId$/complete'
	| Key                 | Value           |
	| operationParameters | { "wage": "3" } |
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/details'
	And extract JSON from body into 'detailsHumanTaskInstance'
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/subtasks'
	And extract JSON from body into 'subTasks'
	
	Then HTTP status code equals to '200'
	Then extract JSON 'detailsHumanTaskInstance', JSON exists 'id'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'name'='compositeTaskWithAutomaticCompletionBehavior'
	Then extract JSON 'detailsHumanTaskInstance', JSON 'status'='COMPLETED'	
	Then extract JSON 'subTasks', JSON '$.content[?(@.name == 'addClient')].status'='COMPLETED'
	Then extract JSON 'subTasks', JSON '$.content[?(@.name == 'emptyTask')].status'='OBSOLETE'

Scenario: Search human task instances
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value           |
	| name | searchHumanTask |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/assignments'
    | Key              | Value                                                                       |
    | peopleAssignment | { "type": "USERIDENTIFIERS", "value": "user1", "usage" : "POTENTIALOWNER" } |
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/assignments'
	| Key              | Value                                                                       |
	| peopleAssignment | { "type" : "USERIDENTIFIERS", "value": "user1", "usage" : "TASKINITIATOR" } |
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/presentationelts'
	| Key                 | Value                                                       |
	| presentationElement | { "language" : "fr", "value": "bonjour", "usage" : "NAME" } |
	And authenticate
	| Key                                                                  | Value |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | user1 |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/me'
	| Key           | Value           |
	| humanTaskName | searchHumanTask |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/.search'
	| Key             | Value |
	| Accept-Language | fr    |
	And extract JSON from body
		
	Then HTTP status code equals to '200'
	Then JSON 'content[0].name'='searchHumanTask'
	Then JSON 'content[0].actualOwner'='user1'
	Then JSON 'content[0].presentationName'='bonjour'