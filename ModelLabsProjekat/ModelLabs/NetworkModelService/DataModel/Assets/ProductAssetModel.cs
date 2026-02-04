using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class ProductAssetModel : AssetModel
    {
        private CorporateStandardKind corporateStandardKind;
        private string modelNumber = string.Empty;
        private string modelVersion = string.Empty;
        private AssetModelUsageKind usageKind;
        private long manufacturer = 0;

        public ProductAssetModel(long globalId) : base(globalId) { }

        public CorporateStandardKind CorporateStandardKind { get => corporateStandardKind; set => corporateStandardKind = value; }
        public string ModelNumber { get => modelNumber; set => modelNumber = value; }
        public string ModelVersion { get => modelVersion; set => modelVersion = value; }
        public AssetModelUsageKind UsageKind { get => usageKind; set => usageKind = value; }
        public long Manufacturer { get => manufacturer; set => manufacturer = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                ProductAssetModel x = (ProductAssetModel)obj;
                return corporateStandardKind == x.corporateStandardKind &&
                       modelNumber == x.modelNumber &&
                       modelVersion == x.modelVersion &&
                       usageKind == x.usageKind &&
                       manufacturer == x.manufacturer;
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.PRODUCTASSETMODEL_CORPORATESTANDARDKIND:
                case ModelCode.PRODUCTASSETMODEL_MODELNUMBER:
                case ModelCode.PRODUCTASSETMODEL_MODELVERSION:
                case ModelCode.PRODUCTASSETMODEL_USAGEKIND:
                case ModelCode.PRODUCTASSETMODEL_MANUFACTURER:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.PRODUCTASSETMODEL_CORPORATESTANDARDKIND: prop.SetValue((short)corporateStandardKind); break;
                case ModelCode.PRODUCTASSETMODEL_MODELNUMBER: prop.SetValue(modelNumber); break;
                case ModelCode.PRODUCTASSETMODEL_MODELVERSION: prop.SetValue(modelVersion); break;
                case ModelCode.PRODUCTASSETMODEL_USAGEKIND: prop.SetValue((short)usageKind); break;
                case ModelCode.PRODUCTASSETMODEL_MANUFACTURER: prop.SetValue(manufacturer); break;
                default: base.GetProperty(prop); break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.PRODUCTASSETMODEL_CORPORATESTANDARDKIND: corporateStandardKind = (CorporateStandardKind)property.AsEnum(); break;
                case ModelCode.PRODUCTASSETMODEL_MODELNUMBER: modelNumber = property.AsString(); break;
                case ModelCode.PRODUCTASSETMODEL_MODELVERSION: modelVersion = property.AsString(); break;
                case ModelCode.PRODUCTASSETMODEL_USAGEKIND: usageKind = (AssetModelUsageKind)property.AsEnum(); break;
                case ModelCode.PRODUCTASSETMODEL_MANUFACTURER: manufacturer = property.AsReference(); break;
                default: base.SetProperty(property); break;
            }
        }

        public override bool IsReferenced => base.IsReferenced;

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (manufacturer != 0 &&
                (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.PRODUCTASSETMODEL_MANUFACTURER] = new List<long> { manufacturer };
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.MANUFACTURER_PRODUCTASSETMODEL:
                    manufacturer = globalId;
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
                case ModelCode.MANUFACTURER_PRODUCTASSETMODEL:
                    if (manufacturer == globalId)
                        manufacturer = 0;
                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }
    }
}
