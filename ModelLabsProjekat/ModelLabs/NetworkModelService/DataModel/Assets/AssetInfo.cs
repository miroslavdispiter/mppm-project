using FTN.Common;
using System;
using System.Collections.Generic;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class AssetInfo : IdentifiedObject
    {
        private long assetModel;   // 0..1

        public AssetInfo(long globalId) : base(globalId) { }

        public long AssetModel
        {
            get => assetModel;
            set => assetModel = value;
        }

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.ASSETINFO_ASSETMODEL:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            if (prop.Id == ModelCode.ASSETINFO_ASSETMODEL)
                prop.SetValue(assetModel);
            else
                base.GetProperty(prop);
        }

        public override void SetProperty(Property property)
        {
            if (property.Id == ModelCode.ASSETINFO_ASSETMODEL)
                assetModel = property.AsReference();
            else
                base.SetProperty(property);
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> refs, TypeOfReference t)
        {
            if (assetModel != 0 && (t == TypeOfReference.Reference || t == TypeOfReference.Both))
                refs[ModelCode.ASSETINFO_ASSETMODEL] = new List<long> { assetModel };

            base.GetReferences(refs, t);
        }
    }
}