Feature: CaseFiles
	Check result returned by /case-files
	
Scenario: Add case file and check it is created
	When execute HTTP POST JSON request 'http://localhost/case-files'
	| Key         | Value       |
	| name        | name        |
	| description | description |
	And extract JSON from body
	And extract 'id' from JSON body into 'casefileid'
	And execute HTTP GET request 'http://localhost/case-files/$casefileid$'	
	And extract JSON from body into 'casefile'
	And execute HTTP GET request 'http://localhost/case-files/$casefileid$/publish'
	And execute HTTP GET request 'http://localhost/case-plans/search?case_file=$casefileid$'
	And extract JSON from body into 'casedefinition'
	
	Then extract JSON 'casefile', JSON 'name'='name'
	Then extract JSON 'casefile', JSON 'description'='description'
	Then extract JSON 'casedefinition', JSON 'total_length'='1'