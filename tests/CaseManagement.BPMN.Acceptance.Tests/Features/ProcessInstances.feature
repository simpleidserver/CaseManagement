Feature: ProcessInstances
	Check result returned by /processinstances

Scenario: Launch MessageEvent bpmn process
	When execute HTTP POST JSON request 'http://localhost/processinstances'
	| Key           | Value                                                            |
	| processFileId | 52e27e4659af9be63154a6094e8392ce222107a063a78b8328d967bd4b9982cb |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'processInstanceId'
	And execute HTTP GET request 'http://localhost/processinstances/$processInstanceId$/start'
	And execute HTTP POST JSON request 'http://localhost/processinstances/$processInstanceId$/messages'
	| Key            | Value                            |
	| name           | newMessage                       |
	| messageContent | { "email": "email@hotmail.com" } |
	And execute HTTP GET request 'http://localhost/processinstances/$processInstanceId$'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'executionPaths[0].executionPointers[?(@.flowNodeId == 'Event_1x42h83')].flowNodeInstance.state'='Complete'

Scenario: Launch CreateUserAccount bpmn process
	When execute HTTP POST JSON request 'http://localhost/processinstances'
	| Key           | Value                                                            |
	| processFileId | 17ac18f07c031f808c55b8e9ff543161b90492947ac7449f682b67bd23e92053 |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'processInstanceId'
	And execute HTTP GET request 'http://localhost/processinstances/$processInstanceId$/start'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/.search'
	| Key | Value |
	And extract JSON from body
	And extract '$.content[?(@.name == 'emptyTask')].id' from JSON body into 'humanTaskInstanceId'
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/start'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/complete'
	| Key                 | Value |
	| operationParameters | {}    |
	And execute HTTP GET request 'http://localhost/processinstances/$processInstanceId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'executionPaths[0].executionPointers[0].flowNodeInstance.state'='Complete'
	Then JSON 'executionPaths[0].executionPointers[1].flowNodeInstance.state'='Complete'

Scenario: Launch GetWeatherInformation bpmn process
	When execute HTTP POST JSON request 'http://localhost/processinstances'
	| Key           | Value                                                            |
	| processFileId | 5ff28e2e6e1175bf69ec33fc5253620bfad4b1340a1ecfb20ea771e3bc76bb0e |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'processInstanceId'
	When execute HTTP GET request 'http://localhost/processinstances/$processInstanceId$/start'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/.search'
	| Key | Value |	
	And extract JSON from body
	And extract '$.content[?(@.name == 'dressAppropriateForm')].id' from JSON body into 'humanTaskInstanceId'
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/start'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/complete'
	| Key                 | Value |
	| operationParameters | {}    |
	And execute HTTP GET request 'http://localhost/processinstances/$processInstanceId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'executionPaths[0].executionPointers[0].flowNodeInstance.state'='Complete'
	Then JSON 'executionPaths[0].executionPointers[1].flowNodeInstance.state'='Complete'

Scenario: Launch GetAppropriateDress bpmn process
	When execute HTTP POST JSON request 'http://localhost/processinstances'
	| Key           | Value                                                            |
	| processFileId | db7c8302dfca4222832aaa98320d228ae2eed2d63b16ed25a5e761a2f781b719 |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'processInstanceId'
	And execute HTTP GET request 'http://localhost/processinstances/$processInstanceId$/start'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/.search'
	| Key | Value |
	And extract JSON from body
	And extract '$.content[?(@.name == 'temperatureForm')].id' from JSON body into 'humanTaskInstanceId'
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/start'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/complete'
	| Key                 | Value               |
	| operationParameters | { "degree" : "30" } |
	And execute HTTP GET request 'http://localhost/processinstances/$processInstanceId$'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'executionPaths[0].executionPointers[?(@.flowNodeId == 'Activity_12xhvyl')].flowNodeInstance.state'='Complete'