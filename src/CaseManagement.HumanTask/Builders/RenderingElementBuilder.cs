using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Builders
{
    public class RenderingElementBuilder
    {
        public RenderingElementBuilder(RenderingElement renderingElement)
        {
            RenderingElement = renderingElement;
        }

        protected RenderingElement RenderingElement { get; private set; }

        public RenderingElementBuilder AddLabel(string language, string value)
        {
            RenderingElement.Labels.Add(new Translation
            {
                Language = language,
                Value = value
            });
            return this;
        }
    }

    public class SelectOptionRenderingElementBuilder : RenderingElementBuilder
    {
        public SelectOptionRenderingElementBuilder(RenderingElement renderingElement) : base(renderingElement) { }

        public SelectOptionRenderingElementBuilder AddOptionValue(string value, ICollection<Translation> dispayNames)
        {
            RenderingElement.Values.Add(new OptionValue
            {
                DisplayNames = dispayNames,
                Value = value
            });
            return this;
        }
    }
}
