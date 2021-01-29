Feature: HumanTaskDef
	Check /humantaskdefs
	
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

Scenario: Check parameter can be added
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value             |
	| name | addInputParameter |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters'
	| Key       | Value                                                  |
	| parameter | { name: 'parameter', type: 'STRING', usage : 'INPUT' } |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='addInputParameter'
	Then JSON 'operationParameters[0].name'='parameter'
	Then JSON 'operationParameters[0].type'='STRING'
	Then JSON 'operationParameters[0].usage'='INPUT'

Scenario: Check parameter can be removed
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                 |
	| name | removeOutputParameter |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters'
	| Key       | Value                                                  |
	| parameter | { name: 'parameter', type: 'STRING', usage: 'OUTPUT' } |
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/parameter'
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='removeOutputParameter'
	Then JSON nb 'operation.outputParameters[*]'='0'

Scenario: Check add deadline
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value            |
	| name | addStartDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines'
	| Key      | Value                                                       |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S", "usage": "START" } |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='addStartDeadline'
	Then JSON 'deadLines[0].name'='name'
	Then JSON 'deadLines[0].usage'='START'
	Then JSON 'deadLines[0].until'='P0Y0M0DT0H0M2S'

Scenario: Check deadline can be removed
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                    |
	| name | removeCompletionDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines'
	| Key      | Value                                                            |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S", "usage": "COMPLETION" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/$deadLineId$'
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='removeCompletionDeadline'
	Then JSON nb 'deadLines.completionDeadLines[*]'='0'

Scenario: Check deadline can be updated
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value               |
	| name | updateStartDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines'
	| Key      | Value                                                       |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S", "usage": "START" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP PUT JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/$deadLineId$'
	| Key          | Value                               |
	| deadLineInfo | { name: "name2", "usage": "START" } |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='updateStartDeadline'
	Then JSON 'deadLines[0].name'='name'
	Then JSON 'deadLines[0].until'='P0Y0M0DT0H0M2S'

Scenario: Check escalation deadline can be added
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                           |
	| name | addEscalationCompletionDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines'
	| Key      | Value                                                            |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S", "usage": "COMPLETION" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/$deadLineId$/escalations'
	| Key       | Value |
	| condition | true  |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='addEscalationCompletionDeadline'
	Then JSON 'deadLines[0].name'='name'
	Then JSON 'deadLines[0].until'='P0Y0M0DT0H0M2S'
	Then JSON 'deadLines[0].escalations[0].condition'='true'
	Then JSON 'deadLines[0].usage'='COMPLETION'

Scenario: Check escalation can be removed from deadline
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value                         |
	| name | removeEscalationStartDeadline |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines'
	| Key      | Value                                                       |
	| deadLine | { name: "name", until: "P0Y0M0DT0H0M2S", "usage": "START" } |
	And extract JSON from body
	And extract 'id' from JSON body into 'deadLineId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/$deadLineId$/escalations'
	| Key       | Value |
	| condition | true  |
	And extract JSON from body
	And extract 'id' from JSON body into 'escId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/deadlines/$deadLineId$/escalations/$escId$'
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='removeEscalationStartDeadline'
	Then JSON nb 'deadLines.startDeadLines[0].escalations[*]'='0'

Scenario: Check rendering can be updated
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value           |
	| name | updateRendering |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'	
	And execute HTTP PUT JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/rendering'
	| Key       | Value                   |
	| rendering | { "type": "container" } |
	And execute HTTP GET request 'http://localhost/humantasksdefs/$humanTaskDefId$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='updateRendering'
	Then JSON 'rendering.type'='container'