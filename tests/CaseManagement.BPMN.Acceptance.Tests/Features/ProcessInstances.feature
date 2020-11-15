Feature: ProcessInstances
	Check result returned by /processinstances
	
Scenario: Start BPMN process and complete human task instance
	When execute HTTP GET request 'http://localhost/processinstances/d33dadd922651238f7c9ec864068ee1ea5a64fc4f233ff001977ebdd82c2af44/start'
	And poll HTTP POST JSON request 'http://localhost/humantaskinstances/.search', until 'totalLength'='1'
	| Key | Value |
	And extract JSON from body
	And extract 'content[0].id' from JSON body into 'humanTaskInstanceId'
	And execute HTTP GET request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/start'
	And execute HTTP POST JSON request 'http://localhost/humantaskinstances/$humanTaskInstanceId$/complete'
	| Key                 | Value |
	| operationParameters | {}    |
	And poll 'http://localhost/processinstances/d33dadd922651238f7c9ec864068ee1ea5a64fc4f233ff001977ebdd82c2af44', until 'elementInstances[0].state'='Complete'
	And poll 'http://localhost/processinstances/d33dadd922651238f7c9ec864068ee1ea5a64fc4f233ff001977ebdd82c2af44', until 'elementInstances[1].state'='Complete'