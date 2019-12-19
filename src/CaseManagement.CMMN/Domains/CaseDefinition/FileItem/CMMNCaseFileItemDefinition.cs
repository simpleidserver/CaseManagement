namespace CaseManagement.CMMN.Domains
{
    public class CMMNCaseFileItemDefinition
    {
        /*
        public CMMNCaseFileItemDefinition(string id, string name) : base(id, name)
        {
            // TransitionHistories = new List<CMMNCaseFileItemTransitionHistory>();
            // MetadataLst = new List<CMMNCaseFileItemMetadata>();
        }
        */

        // public override string ElementType => ELEMENT_TYPE;
        // public const string ELEMENT_TYPE = "casefileitem";
        public CMMNMultiplicities Multiplicity { get; set; }
        public string Definition { get; set; }
        public CMMNCaseFileItemStates? State { get; set; }

        /*
        public override void HandleLaunch()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleEvent(string state)
        {
            throw new System.NotImplementedException();
        }
        */

        // public ICollection<CMMNCaseFileItemMetadata> MetadataLst { get; set; }
        // public ICollection<CMMNCaseFileItemTransitionHistory> TransitionHistories { get; set; }

        /*
        public void Create()
        {
            State = CMMNCaseFileItemStates.Available;
            TransitionHistories.Add(new CMMNCaseFileItemTransitionHistory(DateTime.UtcNow, CMMNCaseFileItemTransitions.Create));
        }

        public void Replace()
        {
            State = CMMNCaseFileItemStates.Available;
            TransitionHistories.Add(new CMMNCaseFileItemTransitionHistory(DateTime.UtcNow, CMMNCaseFileItemTransitions.Replace));
        }
        
        public void RemoveChild()
        {
            State = CMMNCaseFileItemStates.Available;
            TransitionHistories.Add(new CMMNCaseFileItemTransitionHistory(DateTime.UtcNow, CMMNCaseFileItemTransitions.RemoveChild));
        }

        public void RemoveReference()
        {
            State = CMMNCaseFileItemStates.Available;
            TransitionHistories.Add(new CMMNCaseFileItemTransitionHistory(DateTime.UtcNow, CMMNCaseFileItemTransitions.RemoveReference));
        }

        public void Update()
        {
            State = CMMNCaseFileItemStates.Available;
            TransitionHistories.Add(new CMMNCaseFileItemTransitionHistory(DateTime.UtcNow, CMMNCaseFileItemTransitions.Update));
        }

        public void AddChild()
        {
            State = CMMNCaseFileItemStates.Available;
            TransitionHistories.Add(new CMMNCaseFileItemTransitionHistory(DateTime.UtcNow, CMMNCaseFileItemTransitions.AddChild));
        }

        public void AddReference()
        {
            State = CMMNCaseFileItemStates.Available;
            TransitionHistories.Add(new CMMNCaseFileItemTransitionHistory(DateTime.UtcNow, CMMNCaseFileItemTransitions.AddReference));
        }

        public void Delete()
        {
            State = CMMNCaseFileItemStates.Discarded;
            TransitionHistories.Add(new CMMNCaseFileItemTransitionHistory(DateTime.UtcNow, CMMNCaseFileItemTransitions.Delete));
        }

        public override void HandleLaunch()
        {
            Create();
        }

        public override void HandleEvent(string state)
        {
            var stateEnum = (CMMNCaseFileItemTransitions)Enum.Parse(typeof(CMMNCaseFileItemTransitions), state);
            switch(stateEnum)
            {
                case CMMNCaseFileItemTransitions.Create:
                    Create();
                    break;
                case CMMNCaseFileItemTransitions.Replace:
                    Replace();
                    break;
                case CMMNCaseFileItemTransitions.RemoveChild:
                    RemoveChild();
                    break;
                case CMMNCaseFileItemTransitions.RemoveReference:
                    RemoveReference();
                    break;
                case CMMNCaseFileItemTransitions.Update:
                    Update();
                    break;
                case CMMNCaseFileItemTransitions.AddChild:
                    AddChild();
                    break;
                case CMMNCaseFileItemTransitions.AddReference:
                    AddReference();
                    break;
                case CMMNCaseFileItemTransitions.Delete:
                    Delete();
                    break;
            }
        }

        public override object Clone()
        {
            return new CMMNCaseFileItemDefinition(Id, Name)
            {
                Status = Status,
                Multiplicity = Multiplicity,
                Definition = Definition
            };
        }
        */
    }
}
