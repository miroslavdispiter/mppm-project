using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class AssetOrganisationRole : OrganisationRole
    {
        private List<long> assets = new List<long>();

        public AssetOrganisationRole(long globalId) : base(globalId) { }

        public List<long> Assets { get => assets; set => assets = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                AssetOrganisationRole x = (AssetOrganisationRole)obj;
                return CompareHelper.CompareLists(x.assets, assets, true);
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool IsReferenced => assets.Count > 0 || base.IsReferenced;

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (assets.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
                references[ModelCode.ASSETORGANISATIONROLE_ASSET] = new List<long>(assets);

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            if (referenceId == ModelCode.ASSET_ORGANISATIONROLES)
                assets.Add(globalId);
            else
                base.AddReference(referenceId, globalId);
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            if (referenceId == ModelCode.ASSET_ORGANISATIONROLES)
            {
                if (assets.Contains(globalId))
                    assets.Remove(globalId);
            }
            else
                base.RemoveReference(referenceId, globalId);
        }
    }
}
