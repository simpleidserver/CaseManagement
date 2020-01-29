namespace CaseManagement.CMMN
{
    public class CMMNServerOptions
    {
        public CMMNServerOptions()
        {
            MaxConcurrentTask = 20;
            PerformanceIntervalInSeconds = 4;
            BlockThreadMS = 20;
            MaxNbPerformanceRecords = 10;
            SnapshotFrequency = 100;
            DefaultCMMNSchema = "<?xml version='1.0' encoding='UTF-8'?>\n" +
            "<cmmn:definitions xmlns:dc='http://www.omg.org/spec/CMMN/20151109/DC' xmlns:cmmndi='http://www.omg.org/spec/CMMN/20151109/CMMNDI' xmlns:cmmn='http://www.omg.org/spec/CMMN/20151109/MODEL' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' id='Definitions_0m2br72' targetNamespace='http://bpmn.io/schema/cmmn' exporter='cmmn-js (https://demo.bpmn.io/cmmn)' exporterVersion='0.19.2'>\n" +
            "  <cmmn:case id='Case_0t7z7vp'>\n" +
            "    <cmmn:casePlanModel id='CasePlanModel_08bspn4' name='A CasePlanModel'>\n" +
            "      <cmmn:planItem id='PlanItem_1dphpiz' definitionRef='Task_1r5vfut' />\n" +
            "      <cmmn:task id='Task_1r5vfut' />\n" +
            "    </cmmn:casePlanModel>\n" +
            "  </cmmn:case>\n" +
            "  <cmmndi:CMMNDI>\n" +
            "    <cmmndi:CMMNDiagram id='CMMNDiagram_1'>\n" +
            "      <cmmndi:Size width='500' height='500' />\n" +
            "      <cmmndi:CMMNShape id='DI_CasePlanModel_08bspn4' cmmnElementRef='CasePlanModel_08bspn4'>\n" +
            "        <dc:Bounds x='156' y='99' width='534' height='389' />\n" +
            "        <cmmndi:CMMNLabel />\n" +
            "      </cmmndi:CMMNShape>\n" +
            "      <cmmndi:CMMNShape id='PlanItem_1dphpiz_di' cmmnElementRef='PlanItem_1dphpiz'>\n" +
            "        <dc:Bounds x='192' y='132' width='100' height='80' />\n" +
            "        <cmmndi:CMMNLabel />\n" +
            "      </cmmndi:CMMNShape>\n" +
            "    </cmmndi:CMMNDiagram>\n" +
            "  </cmmndi:CMMNDI>\n" +
            "</cmmn:definitions>";
        }

        /// <summary>
        /// Set the interval time in seconds to compute the performance.
        /// </summary>
        public int PerformanceIntervalInSeconds { get; set; }
        /// <summary>
        /// Set the time in milliseconds used to block a thread.
        /// </summary>
        public int BlockThreadMS { get; set; }
        /// <summary>
        /// Set the maximum number of records (performance).
        /// </summary>
        public int MaxNbPerformanceRecords { get; set; }
        /// <summary>
        /// Set the maximum of concurrent tasks.
        /// </summary>
        public int MaxConcurrentTask { get; set; }
        /// <summary>
        /// Set the snapshot frequency.
        /// </summary>
        public int SnapshotFrequency { get; set; }
        /// <summary>
        /// Default CMMN schema used when adding a default case file.
        /// </summary>
        public string DefaultCMMNSchema { get; set; }
    }
}
