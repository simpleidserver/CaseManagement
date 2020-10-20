Feature: HumanTaskDef
	Check errors returned by /humantasksdefs
	
Scenario: Check humantask can be created
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value   |
	| name | newName |
	And extract JSON from body

	Then HTTP status code equals to '201'
	Then JSON 'name'='newName'

Scenario: Check humantaskdef info can be updated
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value    |
	| name | oldName |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP PUT JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/info'
    | Key  | Value    |
    | name | newName2 |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='newName2'