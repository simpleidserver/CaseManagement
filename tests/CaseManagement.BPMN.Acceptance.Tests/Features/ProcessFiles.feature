Feature: ProcessFiles
	Check result returned by /processfiles

Scenario: Search process files
	When execute HTTP POST JSON request 'http://localhost/processfiles/search'
	| Key | Value |
	And extract JSON from body
	Then HTTP status code equals to '200'
	Then JSON 'content[0].name'='CreateUserAccount.bpmn'
	Then JSON 'content[0].description'='CreateUserAccount.bpmn'