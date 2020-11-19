Feature: ProcessInstances
	Check result returned by /processinstances

Scenario: Launch CreateUserAccount bpmn process
	When execute HTTP POST JSON request 'http://localhost/processinstances'
	| Key           | Value                                                            |
	| processFileId | 17ac18f07c031f808c55b8e9ff543161b90492947ac7449f682b67bd23e92053 |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'processInstanceId'
	When execute HTTP GET request 'http://localhost/processinstances/$processInstanceId$/start'
	And poll HTTP POST JSON request 'http://localhost/humantaskinstances/.search', until 'totalLength'='1'
	| Key | Value |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'humanTaskInstanceId'
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/start'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/complete'
	| Key                 | Value |
	| operationParameters | {}    |
	And poll 'http://localhost/processinstances/$processInstanceId$', until 'elementInstances[0].state'='Complete'
	And poll 'http://localhost/processinstances/$processInstanceId$', until 'elementInstances[1].state'='Complete'