using System;
using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;

namespace FTN.Services.NetworkModelService.DataModel.Assets
{
    public class Seal : IdentifiedObject
    {
        private SealConditionKind condition;
        private SealKind kind;
        private string sealNumber = string.Empty;

        public Seal(long globalId) : base(globalId) { }

        public SealConditionKind Condition { get => condition; set => condition = value; }
        public SealKind Kind { get => kind; set => kind = value; }
        public string SealNumber { get => sealNumber; set => sealNumber = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                Seal x = (Seal)obj;
                return condition == x.condition &&
                       kind == x.kind &&
                       sealNumber == x.sealNumber;
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.SEAL_CONDITION:
                case ModelCode.SEAL_KIND:
                case ModelCode.SEAL_SEALNUMBER:
                    return true;
                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.SEAL_CONDITION: prop.SetValue((short)condition); break;
                case ModelCode.SEAL_KIND: prop.SetValue((short)kind); break;
                case ModelCode.SEAL_SEALNUMBER: prop.SetValue(sealNumber); break;
                default: base.GetProperty(prop); break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SEAL_CONDITION: condition = (SealConditionKind)property.AsEnum(); break;
                case ModelCode.SEAL_KIND: kind = (SealKind)property.AsEnum(); break;
                case ModelCode.SEAL_SEALNUMBER: sealNumber = property.AsString(); break;
                default: base.SetProperty(property); break;
            }
        }
    }
}