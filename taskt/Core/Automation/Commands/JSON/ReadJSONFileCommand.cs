﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("File")]
    [Attributes.ClassAttributes.Description("This command reads JSON data into a variable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to read data from JSON files.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ReadJSONFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the path to the file")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the text file.")]
        [SampleUsage("**C:\\temp\\myfile.txt** or **{{{vTextFilePath}}}** or **http://example.com/api/** or **{{{vURL}}}**")]
        [Remarks("If file does not contain extensin, supplement txt automatically.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Path")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please define where the JSON should be stored")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.JSON, true)]
        [PropertyValidationRule("Store", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_userVariableName { get; set; }

        public ReadJSONFileCommand()
        {
            this.CommandName = "ReadJSONFileCommand";
            this.SelectionName = "Read JSON File";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string filePath;
            if (!FilePathControls.isURL(v_FilePath))
            {
                 filePath = FilePathControls.formatFilePath_NoFileCounter(v_FilePath, engine, "json", true);
            }
            else
            {
                filePath = v_FilePath.ConvertToUserVariable(engine);
            }

            ScriptCommand readFile = new ReadTextFileCommand
            {
                v_FilePath = filePath,
                v_userVariableName = this.v_userVariableName
            };
            readFile.RunCommand(sender);
        }
    }
}