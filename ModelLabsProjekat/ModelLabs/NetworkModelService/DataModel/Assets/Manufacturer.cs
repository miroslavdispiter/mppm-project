using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class Manufacturer : OrganisationRole
    {
        private List<long> productAssetModels = new List<long>();

        public Manufacturer(long globalId) : base(globalId) { }

        public List<long> ProductAssetModels
        {
            get => productAssetModels;
            set => productAssetModels = value;
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Manufacturer x = (Manufacturer)obj;
                return CompareHelper.CompareLists(x.productAssetModels, productAssetModels, true);
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool IsReferenced => productAssetModels.Count > 0 || base.IsReferenced;

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (productAssetModels.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
                references[ModelCode.MANUFACTURER_PRODUCTASSETMODEL] = new List<long>(productAssetModels);

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            if (referenceId == ModelCode.PRODUCTASSETMODEL_MANUFACTURER)
                productAssetModels.Add(globalId);
            else
                base.AddReference(referenceId, globalId);
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            if (referenceId == ModelCode.PRODUCTASSETMODEL_MANUFACTURER)
            {
                if (productAssetModels.Contains(globalId))
                    productAssetModels.Remove(globalId);
            }
            else
                base.RemoveReference(referenceId, globalId);
        }
    }
}
