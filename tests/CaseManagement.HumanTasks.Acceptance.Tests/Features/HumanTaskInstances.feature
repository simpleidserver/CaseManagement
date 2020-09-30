Feature: HumanTaskInstances
	Check /humantaskinstances
	
Scenario: Create human task instance
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances'
	| Key                 | Value                                                  |
	| humanTaskName       | addClient                                              |
	| operationParameters | { "isGoldenClient": "true", "firstName": "FirstName" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/details'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON exists 'id'
	Then JSON 'name'='addClient'
	Then JSON 'status'='RESERVED'

Scenario: Create deferred human task instance (activated after 5 seconds)
	When authenticate
	| Key                                                                  | Value         |
	| http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier | taskInitiator |
	And add '5' seconds into 'activationDeferralTime'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances'
	| Key                    | Value                                                  |
	| humanTaskName          | addClient                                              |
	| operationParameters    | { "isGoldenClient": "true", "firstName": "FirstName" } |
	| activationDeferralTime | $activationDeferralTime$                               |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskInstanceId'	
	And poll 'http://localhost/humantaskinstances/$humanTaskInstanceId$/details', until 'status'='RESERVED'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON exists 'id'
	Then JSON 'name'='addClient'
	Then JSON 'status'='RESERVED'