using System;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class AssetFunction : IdentifiedObject
    {
        private string configID;
        private string firmwareID;
        private string hardwareID;
        private string password;
        private string programID;

        public AssetFunction(long globalId) : base(globalId) { }

        public string ConfigID { get => configID; set => configID = value; }
        public string FirmwareID { get => firmwareID; set => firmwareID = value; }
        public string HardwareID { get => hardwareID; set => hardwareID = value; }
        public string Password { get => password; set => password = value; }
        public string ProgramID { get => programID; set => programID = value; }

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.ASSETFUNCTION_CONFIGID:
                case ModelCode.ASSETFUNCTION_FIRMWAREID:
                case ModelCode.ASSETFUNCTION_HARDWAREID:
                case ModelCode.ASSETFUNCTION_PASSWORD:
                case ModelCode.ASSETFUNCTION_PROGRAMID:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.ASSETFUNCTION_CONFIGID: prop.SetValue(configID); break;
                case ModelCode.ASSETFUNCTION_FIRMWAREID: prop.SetValue(firmwareID); break;
                case ModelCode.ASSETFUNCTION_HARDWAREID: prop.SetValue(hardwareID); break;
                case ModelCode.ASSETFUNCTION_PASSWORD: prop.SetValue(password); break;
                case ModelCode.ASSETFUNCTION_PROGRAMID: prop.SetValue(programID); break;
                default: base.GetProperty(prop); break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ASSETFUNCTION_CONFIGID: configID = property.AsString(); break;
                case ModelCode.ASSETFUNCTION_FIRMWAREID: firmwareID = property.AsString(); break;
                case ModelCode.ASSETFUNCTION_HARDWAREID: hardwareID = property.AsString(); break;
                case ModelCode.ASSETFUNCTION_PASSWORD: password = property.AsString(); break;
                case ModelCode.ASSETFUNCTION_PROGRAMID: programID = property.AsString(); break;
                default: base.SetProperty(property); break;
            }
        }
    }
}