Feature: CaseFormInstances
	Check result returned by /case-form-instances
	
Scenario: Launch caseWithHumanTaskAndRole case instance and check form instance exists
	When execute HTTP POST JSON request 'http://localhost/case-instances'
	| Key                | Value					|
	| case_definition_id | caseWithHumanTaskAndRole |
	| case_id            | testCase					|

	And extract JSON from body
	And extract 'id' from JSON body
	And execute HTTP GET request 'http://localhost/case-instances/$id$/launch'
	And wait '1' seconds
	And execute HTTP POST JSON request 'http://localhost/case-instances/$id$/confirm/PI_ProcessTask_1'
	| Key  | Value |	
	| name | name  |
	And wait '5' seconds
	And execute HTTP GET request 'http://localhost/case-form-instances/.me/search'
	And extract JSON from body

	Then HTTP status code equals to '200'
	Then JSON 'content[0].status'='complete'
	Then JSON 'content[0].form_id'='createMeetingForm'
	Then JSON 'content[0].title#en'='Create meeting'
	Then JSON 'content[0].content[0].form_element_id'='name'
	Then JSON 'content[0].content[0].is_required'='true'
	Then JSON 'content[0].content[0].value'='name'
	Then JSON 'content[0].content[0].type'='txt'
	Then JSON 'content[0].content[0].name#en'='Name'
	Then JSON 'content[0].content[0].description#en'='Name of the meeting'
	Then JSON 'content[0].content[0].description#fr'='Intitulé de la réunion'