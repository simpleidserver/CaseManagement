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
	| Key  | Value                 |
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

Scenario: Check presentationElement can be updated
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                     |
	| name | updatePresentationElement |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP PUT JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/presentationelts'
	| Key                 | Value                                               |
	| presentationElement | { names: [ { language: "fr", value: "bonjour" } ] } |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON 'name'='updatePresentationElement'
	Then JSON 'presentationElementResult.names[0].language'='fr'
	Then JSON 'presentationElementResult.names[0].value'='bonjour'

Scenario: Check add start deadline
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value            |
	| name | addStartDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start'
	| Key      | Value                                     |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S" } |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='addStartDeadline'
	Then JSON 'deadLines.startDeadLines[0].name'='name'
	Then JSON 'deadLines.startDeadLines[0].until'='P0Y0M0DT0H0M2S'

Scenario: Check add completion deadline
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                 |
	| name | addCompletionDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/completion'
	| Key      | Value                                     |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S" } |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='addCompletionDeadline'
	Then JSON 'deadLines.completionDeadLines[0].name'='name'
	Then JSON 'deadLines.completionDeadLines[0].until'='P0Y0M0DT0H0M2S'

Scenario: Check start deadline can be removed
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value               |
	| name | removeStartDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start'
	| Key      | Value                                     |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start/$deadLineId$'
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='removeStartDeadline'
	Then JSON nb 'deadLines.startDeadLines[*]'='0'

Scenario: Check completion deadline can be removed
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                    |
	| name | removeCompletionDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/completion'
	| Key      | Value                                     |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/completion/$deadLineId$'
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='removeCompletionDeadline'
	Then JSON nb 'deadLines.completionDeadLines[*]'='0'

Scenario: Check start deadline can be updated
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value               |
	| name | updateStartDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start'
	| Key      | Value                                     |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP PUT JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start/$deadLineId$'
	| Key          | Value            |
	| deadLineInfo | { name: "name2" } |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='updateStartDeadline'
	Then JSON 'deadLines.startDeadLines[0].name'='name'
	Then JSON 'deadLines.startDeadLines[0].until'='P0Y0M0DT0H0M2S'

Scenario: Check completion deadline can be updated
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                    |
	| name | updateCompletionDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/completion'
	| Key      | Value                                     |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP PUT JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/completion/$deadLineId$'
	| Key          | Value            |
	| deadLineInfo | { name: "name2" } |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='updateCompletionDeadline'
	Then JSON 'deadLines.completionDeadLines[0].name'='name'
	Then JSON 'deadLines.completionDeadLines[0].until'='P0Y0M0DT0H0M2S'

Scenario: Check escalation start deadline can be added
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                           |
	| name | addEscalationStartDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start'
	| Key      | Value                                     |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/start/$deadLineId$/escalations'
	| Key       | Value |
	| condition | true  |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='addEscalationStartDeadline'
	Then JSON 'deadLines.startDeadLines[0].name'='name'
	Then JSON 'deadLines.startDeadLines[0].until'='P0Y0M0DT0H0M2S'
	Then JSON 'deadLines.startDeadLines[0].escalations[0].condition'='true'

Scenario: Check escalation completion deadline can be added
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                           |
	| name | addEscalationCompletionDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/completion'
	| Key      | Value                                     |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/completion/$deadLineId$/escalations'
	| Key       | Value |
	| condition | true  |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='addEscalationCompletionDeadline'
	Then JSON 'deadLines.completionDeadLines[0].name'='name'
	Then JSON 'deadLines.completionDeadLines[0].until'='P0Y0M0DT0H0M2S'
	Then JSON 'deadLines.completionDeadLines[0].escalations[0].condition'='true'