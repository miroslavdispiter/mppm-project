using System;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class Seal : IdentifiedObject
    {
        private string sealNumber;
        private SealConditionKind condition;
        private SealKind kind;

        public Seal(long globalId) : base(globalId) { }

        public string SealNumber
        {
            get => sealNumber;
            set => sealNumber = value;
        }

        public SealConditionKind Condition
        {
            get => condition;
            set => condition = value;
        }

        public SealKind Kind
        {
            get => kind;
            set => kind = value;
        }

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.SEAL_SEALNUMBER:
                case ModelCode.SEAL_CONDITION:
                case ModelCode.SEAL_KIND:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.SEAL_SEALNUMBER:
                    prop.SetValue(sealNumber);
                    break;
                case ModelCode.SEAL_CONDITION:
                    prop.SetValue((short)condition);
                    break;
                case ModelCode.SEAL_KIND:
                    prop.SetValue((short)kind);
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
                case ModelCode.SEAL_SEALNUMBER:
                    sealNumber = property.AsString();
                    break;
                case ModelCode.SEAL_CONDITION:
                    condition = (SealConditionKind)property.AsEnum();
                    break;
                case ModelCode.SEAL_KIND:
                    kind = (SealKind)property.AsEnum();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
    }
}