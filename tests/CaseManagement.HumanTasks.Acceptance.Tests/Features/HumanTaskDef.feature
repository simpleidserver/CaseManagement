Feature: HumanTaskDef
	Check errors returned by /humantasksdefs
	
Scenario: Check humantask can be created
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value   |
	| name | newName |
	And extract JSON from body

	Then HTTP status code equals to '201'
	Then JSON 'name'='newName'