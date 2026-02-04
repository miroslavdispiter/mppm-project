using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class AssetModel : IdentifiedObject
    {
        private long assetInfo = 0;

        public AssetModel(long globalId) : base(globalId) { }

        public long AssetInfo
        {
            get => assetInfo;
            set => assetInfo = value;
        }

        #region Equality
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                AssetModel x = (AssetModel)obj;
                return assetInfo == x.assetInfo;
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();
        #endregion

        #region Access
        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.ASSETMODEL_ASSETINFO:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.ASSETMODEL_ASSETINFO:
                    prop.SetValue(assetInfo);
                    break;
                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ASSETMODEL_ASSETINFO:
                    assetInfo = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion

        #region References
        public override bool IsReferenced => (assetInfo != 0) || base.IsReferenced;

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (assetInfo != 0 &&
                (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.ASSETMODEL_ASSETINFO] = new List<long> { assetInfo };
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.ASSETINFO_ASSETMODEL:
                    assetInfo = globalId;
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
                case ModelCode.ASSETINFO_ASSETMODEL:
                    if (assetInfo == globalId)
                        assetInfo = 0;
                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }
        #endregion
    }
}