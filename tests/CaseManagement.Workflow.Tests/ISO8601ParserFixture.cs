using CaseManagement.Workflow.ISO8601;
using Xunit;

namespace CaseManagement.Workflow.Tests
{
    public class ISO8601ParserFixture
    {
        [Fact]
        public void When_Parse_ISO8601_Expression()
        {
            var firstTimeInterval = ISO8601Parser.ParseTimeInterval("P1Y2DT3M");
            var secondTimeInterval = ISO8601Parser.ParseTimeInterval("2007-03-01T13:00:00Z/2008-05-11T15:30:00Z");
            var thirdTimeInterval = ISO8601Parser.ParseTimeInterval("2007-03-01T13:00:00Z/P1Y2M10DT2H30M");
            var fourthTimeInterval = ISO8601Parser.ParseTimeInterval("P1Y2M10DT2H30M/2008-05-11T15:30:00Z");
            var fifthTimeInterval = ISO8601Parser.ParseTimeInterval("P0Y0M0DT0H0M10S");
            var firstRepeatingInterval = ISO8601Parser.ParseRepeatingTimeInterval("R2/P1Y2M10DT2H30M/2007-03-01T13:00:00Z");
            
            Assert.NotNull(firstTimeInterval);
            Assert.NotNull(secondTimeInterval);
            Assert.NotNull(thirdTimeInterval);
            Assert.NotNull(fourthTimeInterval);
            Assert.NotNull(fifthTimeInterval);
            Assert.NotNull(firstRepeatingInterval);
            Assert.True(ISO8601Parser.IsDuration("P1Y2DT3M"));
            Assert.True(ISO8601Parser.IsTimeIntervalDuration("2007-03-01T13:00:00Z/2008-05-11T15:30:00Z"));
            Assert.True(ISO8601Parser.IsTimeIntervalStartAndDuration("2007-03-01T13:00:00Z/P1Y2M10DT2H30M"));
            Assert.True(ISO8601Parser.IsTimeIntervalDurationAndEnd("P1Y2M10DT2H30M/2008-05-11T15:30:00Z"));
            Assert.True(ISO8601Parser.IsRepeatingInterval("R2/P1Y2M10DT2H30M/2007-03-01T13:00:00Z"));
        }
    }
}
