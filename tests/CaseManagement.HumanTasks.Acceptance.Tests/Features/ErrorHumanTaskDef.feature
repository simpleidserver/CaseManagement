Feature: ErrorHumanTaskDef
	Check errors returned by /humantasksdefs
	
Scenario: Check error is returned when trying to create task with missing name
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key           | Value       |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'name' is missing'

Scenario: Check error is returned when trying to add humantaskdef with same name
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value |
	| name | name  |
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value |
	| name | name  |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='The human task def 'name' already exists'

Scenario: Check error is returned when trying to get an unknown humantaskdef
	When execute HTTP GET request 'http://localhost/humantasksdefs/def'
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to update information and no name parameter is passed
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/id/info'
	| Key | Value |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'name' is missing'

Scenario: Check error is returned when trying to update information of an unknown humantaskdef
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/id/info'
	| Key  | Value |
	| name | name  |
	And extract JSON from body

	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'id''

Scenario: Check error is returned when trying to update information with an existing name
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value |
	| name | n1    |
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value |
	| name | n2    |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP PUT JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/info'
	| Key  | Value |
	| name | n1    |
	And extract JSON from body

	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='The human task def 'n1' already exists'

Scenario: Check error is returned when trying to update people assignment
	When execute HTTP PUT JSON request 'http://localhost/humantasksdefs/def/assignment'
	| Key  | Value |
	And extract JSON from body
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to add input parameter to unknown humantaskdef
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/def/parameters/input'
	| Key  | Value |
	And extract JSON from body
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to add input parameter and parameter is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value           |
	| name | inputParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/input'
	| Key  | Value |
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'parameter' is missing'

Scenario: Check error is returned when trying to add already existing input parameter
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value           |
	| name | inputParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/input'
	| Key       | Value                                 |
	| parameter | { name: 'parameter', type: 'STRING' } |
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/input'
	| Key       | Value                                 |
	| parameter | { name: 'parameter', type: 'STRING' } |
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Input parameter 'parameter' already exists'
	
Scenario: Check error is returned when trying to add output parameter to unknown humantaskdef
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs/def/parameters/output'
	| Key  | Value |
	And extract JSON from body
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to add output parameter and parameter is missing
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value           |
	| name | outputParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/output'
	| Key  | Value |
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Parameter 'parameter' is missing'

Scenario: Check error is returned when trying to add already existing output parameter
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value           |
	| name | outputParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/output'
	| Key       | Value                                 |
	| parameter | { name: 'parameter', type: 'STRING' } |
	And execute HTTP POST JSON request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/output'
	| Key       | Value                                 |
	| parameter | { name: 'parameter', type: 'STRING' } |
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Output parameter 'parameter' already exists'

Scenario: Check error is returned when trying to delete input parameter from unknown humantaskdef
	When execute HTTP DELETE request 'http://localhost/humantasksdefs/def/parameters/input/name'
	And extract JSON from body
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to delete unknown input parameter
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value            |
	| name | deleteParameter1 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/input/name'
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Input parameter 'parameter' doesn't exist'


Scenario: Check error is returned when trying to delete ouput parameter from unknown humantaskdef
	When execute HTTP DELETE request 'http://localhost/humantasksdefs/def/parameters/output/name'
	And extract JSON from body
	Then HTTP status code equals to '404'
	Then JSON 'status'='404'
	Then JSON 'errors.bad_request[0]'='Unknown human task definition 'def''

Scenario: Check error is returned when trying to delete unknown output parameter
	When execute HTTP POST JSON request 'http://localhost/humantasksdefs'
	| Key  | Value            |
	| name | deleteParameter2 |
	And extract JSON from body
	And extract 'id' from JSON body into 'humanTaskDefId'
	And execute HTTP DELETE request 'http://localhost/humantasksdefs/$humanTaskDefId$/parameters/output/name'
	And extract JSON from body
	Then HTTP status code equals to '400'
	Then JSON 'status'='400'
	Then JSON 'errors.bad_request[0]'='Output parameter 'parameter' doesn't exist'