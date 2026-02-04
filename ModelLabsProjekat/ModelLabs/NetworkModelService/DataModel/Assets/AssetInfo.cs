using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class AssetInfo : IdentifiedObject
    {
        private long assetModel = 0;
        private List<long> assets = new List<long>();

        public AssetInfo(long globalId) : base(globalId) { }

        public long AssetModel
        {
            get => assetModel;
            set => assetModel = value;
        }

        public List<long> Assets
        {
            get => assets;
            set => assets = value;
        }

        #region Equality
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                AssetInfo x = (AssetInfo)obj;
                return assetModel == x.assetModel &&
                       CompareHelper.CompareLists(x.assets, assets, true);
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
                case ModelCode.ASSETINFO_ASSETMODEL:
                case ModelCode.ASSETINFO_ASSET:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.ASSETINFO_ASSETMODEL:
                    prop.SetValue(assetModel);
                    break;
                case ModelCode.ASSETINFO_ASSET:
                    prop.SetValue(assets);
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
                case ModelCode.ASSETINFO_ASSETMODEL:
                    assetModel = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
        #endregion

        #region References
        public override bool IsReferenced =>
            (assets != null && assets.Count > 0) || base.IsReferenced;

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (assetModel != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.ASSETINFO_ASSETMODEL] = new List<long> { assetModel };
            }

            if (assets != null && assets.Count > 0 &&
                (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.ASSETINFO_ASSET] = new List<long>(assets);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.ASSET_ASSETINFO:
                    assets.Add(globalId);
                    break;

                case ModelCode.ASSETMODEL_ASSETINFO:
                    assetModel = globalId;
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
                case ModelCode.ASSET_ASSETINFO:
                    if (assets.Contains(globalId))
                        assets.Remove(globalId);
                    break;

                case ModelCode.ASSETMODEL_ASSETINFO:
                    if (assetModel == globalId)
                        assetModel = 0;
                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }
        #endregion
    }
}