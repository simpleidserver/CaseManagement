Feature: ProcessFiles
	Check result returned by /processfiles

Scenario: Search process files
	When execute HTTP POST JSON request 'http://localhost/processfiles/search'
	| Key | Value |
	And extract JSON from body
	Then HTTP status code equals to '200'
	Then JSON 'content[0].name'='CreateUserAccount.bpmn'
	Then JSON 'content[0].description'='CreateUserAccount.bpmn'

Scenario: Add process file
	When execute HTTP POST JSON request 'http://localhost/processfiles'
	| Key         | Value       |
	| name        | Name.bpmn   |
	| description | description |
	And extract JSON from body
	And extract 'id' from JSON body into 'newProcessFile'	
	And execute HTTP GET request 'http://localhost/processfiles/$newProcessFile$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='Name.bpmn'
	Then JSON 'description'='description'

Scenario: Update process file
	When execute HTTP POST JSON request 'http://localhost/processfiles'
	| Key         | Value       |
	| name        | Name.bpmn   |
	| description | description |
	And extract JSON from body
	And extract 'id' from JSON body into 'newProcessFile'
	And execute HTTP PUT JSON request 'http://localhost/processfiles/$newProcessFile$'
	| Key         | Value          |
	| name        | NewName.bpmn   |
	| description | NewDescription |
	And execute HTTP GET request 'http://localhost/processfiles/$newProcessFile$'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'name'='NewName.bpmn'
	Then JSON 'description'='NewDescription'

Scenario: Publish process file
	When execute HTTP POST JSON request 'http://localhost/processfiles'
	| Key         | Value                   |
	| name        | PublishProcessFile.bpmn |
	| description | description             |
	And extract JSON from body
	And extract 'id' from JSON body into 'newProcessFile'
	And execute HTTP GET request 'http://localhost/processfiles/$newProcessFile$/publish'
	And execute HTTP POST JSON request 'http://localhost/processfiles/search'
	| Key | Value |
	And extract JSON from body
	
	Then HTTP status code equals to '200'
	Then JSON '$.content[?(@.name == 'PublishProcessFile.bpmn' && @.status == 'Edited')].status'='Edited'
