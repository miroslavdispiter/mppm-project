using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class AssetInfo : IdentifiedObject
    {
        private long assetModel = 0;

        public AssetInfo(long globalId) : base(globalId) { }

        public long AssetModel
        {
            get => assetModel;
            set => assetModel = value;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                AssetInfo x = (AssetInfo)obj;
                return assetModel == x.assetModel;
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.ASSETINFO_ASSETMODEL: return true;
                default: return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.ASSETINFO_ASSETMODEL: prop.SetValue(assetModel); break;
                default: base.GetProperty(prop); break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ASSETINFO_ASSETMODEL: assetModel = property.AsReference(); break;
                default: base.SetProperty(property); break;
            }
        }
    }
}
