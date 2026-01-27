using System;
using System.Collections.Generic;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class Asset : IdentifiedObject
    {
        private bool critical;
        private string initialCondition;
        private string lotNumber;
        private string serialNumber;
        private string type;
        private string utcNumber;

        private long assetInfo;                       // 0..1
        private List<long> organisationRoles = new();  // 0..n

        public Asset(long globalId) : base(globalId) { }

        public bool Critical { get => critical; set => critical = value; }
        public string InitialCondition { get => initialCondition; set => initialCondition = value; }
        public string LotNumber { get => lotNumber; set => lotNumber = value; }
        public string SerialNumber { get => serialNumber; set => serialNumber = value; }
        public string Type { get => type; set => type = value; }
        public string UtcNumber { get => utcNumber; set => utcNumber = value; }

        public long AssetInfo { get => assetInfo; set => assetInfo = value; }
        public List<long> OrganisationRoles { get => organisationRoles; set => organisationRoles = value; }

        public override bool Equals(object obj)
        {
            return obj is Asset x &&
                base.Equals(x) &&
                x.critical == critical &&
                x.initialCondition == initialCondition &&
                x.lotNumber == lotNumber &&
                x.serialNumber == serialNumber &&
                x.type == type &&
                x.utcNumber == utcNumber &&
                x.assetInfo == assetInfo &&
                CompareHelper.CompareLists(x.organisationRoles, organisationRoles);
        }

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.ASSET_CRITICAL:
                case ModelCode.ASSET_INITIALCONDITION:
                case ModelCode.ASSET_LOTNUMBER:
                case ModelCode.ASSET_SERIALNUMBER:
                case ModelCode.ASSET_TYPE:
                case ModelCode.ASSET_UTCNUMBER:
                case ModelCode.ASSET_ASSETINFO:
                case ModelCode.ASSET_ORGANISATIONROLES:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.ASSET_CRITICAL: prop.SetValue(critical); break;
                case ModelCode.ASSET_INITIALCONDITION: prop.SetValue(initialCondition); break;
                case ModelCode.ASSET_LOTNUMBER: prop.SetValue(lotNumber); break;
                case ModelCode.ASSET_SERIALNUMBER: prop.SetValue(serialNumber); break;
                case ModelCode.ASSET_TYPE: prop.SetValue(type); break;
                case ModelCode.ASSET_UTCNUMBER: prop.SetValue(utcNumber); break;
                case ModelCode.ASSET_ASSETINFO: prop.SetValue(assetInfo); break;
                case ModelCode.ASSET_ORGANISATIONROLES: prop.SetValue(organisationRoles); break;
                default: base.GetProperty(prop); break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ASSET_CRITICAL: critical = property.AsBool(); break;
                case ModelCode.ASSET_INITIALCONDITION: initialCondition = property.AsString(); break;
                case ModelCode.ASSET_LOTNUMBER: lotNumber = property.AsString(); break;
                case ModelCode.ASSET_SERIALNUMBER: serialNumber = property.AsString(); break;
                case ModelCode.ASSET_TYPE: type = property.AsString(); break;
                case ModelCode.ASSET_UTCNUMBER: utcNumber = property.AsString(); break;
                case ModelCode.ASSET_ASSETINFO: assetInfo = property.AsReference(); break;
                default: base.SetProperty(property); break;
            }
        }

        public override bool IsReferenced => organisationRoles.Count > 0 || base.IsReferenced;

        public override void GetReferences(Dictionary<ModelCode, List<long>> refs, TypeOfReference t)
        {
            if (assetInfo != 0 && (t == TypeOfReference.Reference || t == TypeOfReference.Both))
                refs[ModelCode.ASSET_ASSETINFO] = new() { assetInfo };

            if (organisationRoles.Count > 0 && (t == TypeOfReference.Target || t == TypeOfReference.Both))
                refs[ModelCode.ASSET_ORGANISATIONROLES] = new List<long>(organisationRoles);

            base.GetReferences(refs, t);
        }

        public override void AddReference(ModelCode refId, long gid)
        {
            if (refId == ModelCode.ASSETORGANISATIONROLE_ASSETS)
                organisationRoles.Add(gid);
            else
                base.AddReference(refId, gid);
        }

        public override void RemoveReference(ModelCode refId, long gid)
        {
            if (refId == ModelCode.ASSETORGANISATIONROLE_ASSETS)
                organisationRoles.Remove(gid);
            else
                base.RemoveReference(refId, gid);
        }
    }
}