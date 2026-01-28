using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class AssetModel : IdentifiedObject
    {
        private List<long> assetInfos = new List<long>();

        public AssetModel(long globalId) : base(globalId) { }

        public List<long> AssetInfos { get => assetInfos; set => assetInfos = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                AssetModel x = (AssetModel)obj;
                return CompareHelper.CompareLists(x.assetInfos, assetInfos, true);
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool IsReferenced => assetInfos.Count > 0 || base.IsReferenced;

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (assetInfos.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
                references[ModelCode.ASSETMODEL_ASSETINFO] = new List<long>(assetInfos);

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            if (referenceId == ModelCode.ASSETINFO_ASSETMODEL)
                assetInfos.Add(globalId);
            else
                base.AddReference(referenceId, globalId);
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            if (referenceId == ModelCode.ASSETINFO_ASSETMODEL)
            {
                if (assetInfos.Contains(globalId))
                    assetInfos.Remove(globalId);
            }
            else
                base.RemoveReference(referenceId, globalId);
        }
    }
}
