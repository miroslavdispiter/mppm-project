using System;
using System.Collections.Generic;
using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class Asset : IdentifiedObject
    {
        private bool critical;
        private string initialCondition = string.Empty;
        private string lotNumber = string.Empty;
        private string serialNumber = string.Empty;
        private string type = string.Empty;
        private string utcNumber = string.Empty;
        private long assetInfo = 0;
        private List<long> organisationRoles = new List<long>();

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
            if (base.Equals(obj))
            {
                Asset x = (Asset)obj;
                return (x.critical == this.critical &&
                        x.initialCondition == this.initialCondition &&
                        x.lotNumber == this.lotNumber &&
                        x.serialNumber == this.serialNumber &&
                        x.type == this.type &&
                        x.utcNumber == this.utcNumber &&
                        x.assetInfo == this.assetInfo &&
                        CompareHelper.CompareLists(x.organisationRoles, this.organisationRoles, true));
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        #region Access
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
        #endregion

        #region References
        public override bool IsReferenced => organisationRoles.Count > 0 || base.IsReferenced;

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (assetInfo != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
                references[ModelCode.ASSET_ASSETINFO] = new List<long> { assetInfo };

            if (organisationRoles.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
                references[ModelCode.ASSET_ORGANISATIONROLES] = organisationRoles;

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.ASSETORGANISATIONROLE_ASSET:
                    organisationRoles.Add(globalId);
                    break;
                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.ASSETORGANISATIONROLE_ASSET:
                    if (organisationRoles.Contains(globalId))
                        organisationRoles.Remove(globalId);
                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }
        #endregion
    }
}