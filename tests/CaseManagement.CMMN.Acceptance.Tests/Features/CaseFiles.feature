Feature: CaseFiles
	Check result returned by /case-files
	
Scenario: Publish case file and check caseplan is inserted
	When execute HTTP POST JSON request 'http://localhost/case-files'
	| Key         | Value       |
	| name        | name        |
	| description | description |
	And extract JSON from body
	And extract 'id' from JSON body into 'caseFileId'
	And poll 'http://localhost/case-files/$caseFileId$', until 'name'='name'
	And extract JSON from body into 'caseFile'
	And execute HTTP GET request 'http://localhost/case-files/$caseFileId$/publish'
	And poll 'http://localhost/case-files/$caseFileId$', until 'status'='Published'
	And poll HTTP POST JSON request 'http://localhost/case-plans/search', until 'totalLength'='1'
	| Key        | Value        |
	| caseFileId | $caseFileId$ |
	And extract JSON from body into 'casePlans'
	
	Then extract JSON 'caseFile', JSON 'name'='name'
	Then extract JSON 'caseFile', JSON 'description'='description'
	Then extract JSON 'casePlans', JSON 'totalLength'='1'