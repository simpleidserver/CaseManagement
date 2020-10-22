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

Scenario: Check humantaskdef people assignment can be updated
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                  |
	| name | updatePeopleAssignment |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP PUT JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/assignment'
    | Key              | Value                                                                          |
    | peopleAssignment | { potentialOwner : { type: "USERIDENTIFIERS", userIdentifiers: [ "user1" ]  } } |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='updatePeopleAssignment'
	Then JSON 'peopleAssignment.potentialOwner.type'='USERIDENTIFIERS'
	Then JSON 'peopleAssignment.potentialOwner.userIdentifiers[0]'='user1'

Scenario: Check input parameter can be added
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value             |
	| name | addInputParameter |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/input'
	| Key       | Value                                 |
	| parameter | { name: 'parameter', type: 'STRING' } |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='addInputParameter'
	Then JSON 'operation.inputParameters[0].name'='parameter'
	Then JSON 'operation.inputParameters[0].type'='STRING'

Scenario: Check output parameter can be added
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value              |
	| name | addOutputParameter |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/output'
	| Key       | Value                                 |
	| parameter | { name: 'parameter', type: 'STRING' } |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='addOutputParameter'
	Then JSON 'operation.outputParameters[0].name'='parameter'
	Then JSON 'operation.outputParameters[0].type'='STRING'

Scenario: Check input parameter can be removed
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                |
	| name | removeInputParameter |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/input'
	| Key       | Value                                 |
	| parameter | { name: 'parameter', type: 'STRING' } |	
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/input/parameter'
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='removeInputParameter'
	Then JSON nb 'operation.inputParameters[*]'='0'


Scenario: Check output parameter can be removed
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                |
	| name | removeOutputParameter |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/output'
	| Key       | Value                                 |
	| parameter | { name: 'parameter', type: 'STRING' } |	
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/output/parameter'
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='removeOutputParameter'
	Then JSON nb 'operation.outputParameters[*]'='0'