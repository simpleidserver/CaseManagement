Feature: ErrorHumanTaskInstances
	Check errors returned by /humantaskinstances
	
Scenario: Check error is returned when try to create task with bad TaskName
	When execute HTTP POST JSON request 'http://localhost/humantaskinstances'
	| Key           | Value       |
	| humanTaskName | invalidname |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'